using UnityEngine;

namespace WeaponSystem
{
    [CreateAssetMenu(menuName = "WeaponSystem/Barrel")]
    public class Barrel : ScriptableObject
    {
        [Range(0f, 1f)] public float thickness;
        [Range(0f, 360f)] public float arc;
        public ArcMode arcMode;
        public AnimationCurve spreadCurve;
    }

    public enum ArcMode
    {
        Random,
        Spread,
        Curve
    }
}