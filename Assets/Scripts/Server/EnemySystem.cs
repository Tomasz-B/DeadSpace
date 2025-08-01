using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.NetCode;
using Unity.Transforms;
using Random = UnityEngine.Random;

[WorldSystemFilter(WorldSystemFilterFlags.ServerSimulation)]
partial struct EnemySystem : ISystem
{
    private EnemySystemData data;

    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        data = new EnemySystemData
        {
            Timer = 10f,
            EnemySpawnedCount = 0,
        };
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        data.Timer -= SystemAPI.Time.DeltaTime;

        if (data.Timer <= 0)
        {
            data.EnemySpawnedCount++;
            data.Timer = 10f + 2f * data.EnemySpawnedCount;

            EntityCommandBuffer ecb = new EntityCommandBuffer(Allocator.Temp);

            Entity enemy = ecb.Instantiate(SystemAPI.GetSingleton<EnemyEntityReference>().EnemyPrefabEntity);
            ecb.SetComponent(enemy, LocalTransform.FromPosition(new float3(Random.Range(-5, 5), 1f, Random.Range(-3, -9))));
            ecb.Playback(state.EntityManager);
        }
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
        
    }
}
