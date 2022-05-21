using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace WeaponSystem
{
    [RequireComponent(typeof(WeaponBarrel))]
    public class WeaponCore : MonoBehaviour
    {
        [SerializeField] private WeaponInventory weaponInventory;
        [SerializeField] private UnityEvent<Weapon> onShoot;
        private IHandleWeapon _handleWeapon;
        private WeaponBarrel _weaponBarrel;

        private Weapon _weapon;
        private bool HasAmmoLeft => _weapon.magazine > 0;
        private bool TriggerPulled => _handleWeapon.PullTrigger;

        private void Awake()
        {
            _handleWeapon = GetComponentInParent<IHandleWeapon>();
            _weaponBarrel = GetComponent<WeaponBarrel>();
        }

        private void OnEnable()
        {
            weaponInventory.onWeaponChanged.AddListener(OnWeaponChanged);
        }

        private void OnDisable()
        {
            weaponInventory.onWeaponChanged.RemoveListener(OnWeaponChanged);
        }

        private IEnumerator HandleCurrentWeapon()
        {
            yield return StateHandler();
            StartCoroutine(HeatRecovery());

            while (enabled)
            {
                if (TriggerPulled)
                {
                    yield return Charging();

                    yield return Shooting();

                    if (!HasAmmoLeft)
                    {
                        yield return Reloading();
                    }
                    else
                    {
                        yield return RateOfFire();

                        yield return TriggerReady();
                    }

                    _weapon.state = State.Ready;
                }

                yield return null;
            }
        }

        private IEnumerator StateHandler()
        {
            var decreaseAmount = Mathf.Max(0, _weapon.TimeElapsed - _weapon.heatRecoveryDelay) * _weapon.heatRecoveryRate;
            _weapon.heat = Mathf.Max(0, _weapon.heat - (int) decreaseAmount);
            switch (_weapon.state)
            {
                case State.Ready:
                case State.Charging:
                default:
                    break;
                case State.Shooting:
                case State.Cooling:
                    yield return new WaitForSeconds((60f / _weapon.fireRate) - _weapon.TimeElapsed);
                    break;
                case State.WaitingTriggerUp:
                    yield return new WaitWhile(() => TriggerPulled);
                    break;
                case State.Reloading:
                    yield return new WaitForSeconds(_weapon.reloadTime - _weapon.TimeElapsed);
                    break;
            }

            _weapon.state = State.Ready;
        }

        private IEnumerator WaitUntilHeatStabilize()
        {
            while (enabled)
            {
                var previousHeat = _weapon.heat;
                yield return new WaitForSeconds(_weapon.heatRecoveryDelay);
                if (previousHeat == _weapon.heat) yield break;
            }
        }

        private IEnumerator HeatRecovery()
        {
            yield return new WaitUntil(() => _weapon.heatCapacity != 0);
            while (enabled)
            {
                yield return WaitUntilHeatStabilize();

                while (enabled)
                {
                    var previousHeat = _weapon.heat;
                    yield return new WaitForSeconds(1f / _weapon.heatRecoveryRate);
                    if (_weapon.heat > previousHeat || _weapon.heat == 0 || _weapon.state == State.Cooling) break;
                    _weapon.heat--;
                }
            }
        }

        private IEnumerator Charging()
        {
            _weapon.state = State.Charging;
            yield return new WaitForSeconds(_weapon.chargeTime);
        }

        private IEnumerator Shoot()
        {
            onShoot?.Invoke(_weapon);
            _weaponBarrel.Shoot(_weapon);
            _weapon.magazine--;
            _weapon.heat += _weapon.heatPerShot;

            if (_weapon.heatCapacity > 0 && _weapon.heat >= _weapon.heatCapacity)
            {
                yield return RecoverHeat();
            }
        }

        private IEnumerator Shooting()
        {
            _weapon.state = State.Shooting;

            if (0 < _weapon.CurrentFireMode.roundBurst && HasAmmoLeft)
            {
                yield return Shoot();
            }

            for (var i = 1; i < _weapon.CurrentFireMode.roundBurst && HasAmmoLeft; i++)
            {
                yield return new WaitForSeconds(60f / _weapon.burstRate);
                yield return Shoot();
            }
        }

        private IEnumerator Reloading()
        {
            _weapon.state = State.Reloading;
            yield return new WaitForSeconds(_weapon.reloadTime);
            _weapon.magazine = _weapon.capacity;
        }

        private IEnumerator RateOfFire()
        {
            _weapon.state = State.Cooling;
            yield return new WaitForSeconds(60f / _weapon.fireRate);
        }

        private IEnumerator TriggerReady()
        {
            if (_weapon.CurrentFireMode.automatic || !TriggerPulled) yield break;
            _weapon.state = State.WaitingTriggerUp;
            yield return new WaitWhile(() => TriggerPulled);
        }

        private IEnumerator RecoverHeat()
        {
            _weapon.state = State.Cooling;
            yield return new WaitForSeconds(_weapon.heatCapacity / _weapon.heatRecoveryRate);
            _weapon.heat = 0;
        }

        private void OnWeaponChanged(Weapon previousWeapon, Weapon currenWeapon, Weapon nextWeapon)
        {
            StopAllCoroutines();
            _weapon = currenWeapon;
            StartCoroutine(HandleCurrentWeapon());
        }
    }
}