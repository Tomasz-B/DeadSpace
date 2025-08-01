using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.NetCode;
using Unity.Transforms;
using Random = UnityEngine.Random;

[WorldSystemFilter(WorldSystemFilterFlags.ServerSimulation)]
partial struct UpgradeSystem : ISystem
{
    private float timer;

    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        timer = 5f;
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        timer -= SystemAPI.Time.DeltaTime;

        if (timer <= 0)
        {
            timer = Random.Range(5f, 9f);
            int randomUpgrade = Random.Range(1, 4);

            EntityCommandBuffer ecb = new EntityCommandBuffer(Allocator.Temp);

            Entity upgrade = ecb.Instantiate(SystemAPI.GetSingleton<UpgradeEntityReference>().UpgradePrefabEntity);
            ecb.SetComponent(upgrade, LocalTransform.FromPosition(new float3(Random.Range(-5, 5), 1f, Random.Range(-5, -8))));
            
            if (randomUpgrade == 1) { ecb.SetComponent(upgrade, new UpgradeData() { UpgradeShootingSpeed = 1, }); }
            if (randomUpgrade == 2) { ecb.SetComponent(upgrade, new UpgradeData() { UpgradeBulletSpeed = 1, }); }
            if (randomUpgrade == 3) { ecb.SetComponent(upgrade, new UpgradeData() { UpgradeBulletPower = 1, }); }
            if (randomUpgrade == 4) { ecb.SetComponent(upgrade, new UpgradeData() { UpgradeBulletLife = 1, }); }

            ecb.Playback(state.EntityManager);
        }
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {

    }
}
