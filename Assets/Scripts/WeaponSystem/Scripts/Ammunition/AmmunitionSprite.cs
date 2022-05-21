using UnityEngine;

namespace WeaponSystem
{
    public class AmmunitionSprite : MonoBehaviour
    {
        private SpriteRenderer _sr;

        private void Awake()
        {
            _sr = GetComponent<SpriteRenderer>();
        }

        public void Initialize(Ammunition ammunition)
        {
            _sr.color = ammunition.color;
            _sr.sprite = ammunition.sprite;
        }
    }
}