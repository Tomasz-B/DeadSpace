using Unity.Entities;
using Unity.Mathematics;
using Unity.NetCode;

[GhostComponent]
public struct PlayerData : IComponentData
{
    public int PlayerId;
    public float ShotDelay;

    public int ShootingSpeedBonus;

    public int BulletSpeedBonus;
    public int BulletPowerBonus;
    public int BulletLifeBonus;
}
