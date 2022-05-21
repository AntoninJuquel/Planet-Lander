using UnityEngine;

namespace WeaponSystem
{
    [CreateAssetMenu(menuName = "WeaponSystem/Weapon")]
    public class Weapon : ScriptableObject
    {
        public Sprite sprite;
        public int damage;
        public float time;
        public State state;
        public WeaponBarrelEmission[] barrels;
        public Ammunition ammunition;
        public MuzzleFlash muzzleFlash;

        public FireMode[] fireModes;
        public int fireModeIndex;
        public FireMode CurrentFireMode => fireModes[fireModeIndex];
        public void NextFireMode() => fireModeIndex = (fireModeIndex + 1) % fireModes.Length;

        public float fireRate;
        public float burstRate;
        public float reloadTime;
        public float chargeTime;

        public int heat;
        public int heatCapacity;
        public int heatPerShot;
        public float heatRecoveryRate;
        public float heatRecoveryDelay;

        public int magazine;
        public int capacity;

        public float lastTimeHeld;
        public float TimeElapsed => Time.time - lastTimeHeld;
    }

    public enum State
    {
        Ready,
        Charging,
        Shooting,
        Cooling,
        WaitingTriggerUp,
        Reloading
    }

    [System.Serializable]
    public class WeaponBarrelEmission
    {
        public Barrel barrel;
        public Vector3 position;
        public float angle;
        public int emissionCount = 1;
        public float radius = 1;
    }
}