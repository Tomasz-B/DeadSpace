using Unity.NetCode;
using UnityEngine;

public struct UpgradeRpc : IRpcCommand
{
    public int UpgradeShootingSpeed;
    public int UpgradeBulletSpeed;
    public int UpgradeBulletPower;
    public int UpgradeBulletLife;
}
