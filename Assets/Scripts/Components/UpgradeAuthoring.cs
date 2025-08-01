using Unity.Entities;
using UnityEngine;

public class UpgradeAuthoring : MonoBehaviour
{
    public int UpgradeShootingSpeed;
    public int UpgradeBulletSpeed;
    public int UpgradeBulletPower;
    public int UpgradeBulletLife;

    private class Baker : Baker<UpgradeAuthoring>
    {
        public override void Bake(UpgradeAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);

            AddComponent(entity, new UpgradeData()
            {
                UpgradeShootingSpeed = authoring.UpgradeShootingSpeed,
                UpgradeBulletSpeed = authoring.UpgradeBulletSpeed,
                UpgradeBulletPower = authoring.UpgradeBulletPower,
                UpgradeBulletLife = authoring.UpgradeBulletLife,
            });
        }
    }
}
