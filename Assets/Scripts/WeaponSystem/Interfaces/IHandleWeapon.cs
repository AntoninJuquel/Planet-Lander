using System;

namespace WeaponSystem
{
    public interface IHandleWeapon
    {
        bool PullTrigger { get; }
        public event EventHandler<int> OnSwitchWeapon;
    }
}