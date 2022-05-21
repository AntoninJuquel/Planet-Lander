using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeaponSystem
{
    [CreateAssetMenu(menuName = "WeaponSystem/FireMode")]
    public class FireMode : ScriptableObject
    {
        public int roundBurst;
        public bool automatic;
    }
}