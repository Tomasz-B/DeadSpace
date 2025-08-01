using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

[WorldSystemFilter(WorldSystemFilterFlags.ServerSimulation)]
partial struct BulletSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<BulletData>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        EntityCommandBuffer ecb = new EntityCommandBuffer(Allocator.Temp);

        foreach
            ((

            RefRW<BulletData> bullet,
            RefRW<LocalTransform> transform,
            Entity entity)
            in
            SystemAPI.Query<RefRW<BulletData>, RefRW<LocalTransform>>().WithEntityAccess().WithAll<Simulate>()
            )
        {
            transform.ValueRW.Position += (float3)bullet.ValueRO.Direction * bullet.ValueRO.Speed * SystemAPI.Time.DeltaTime;
            bullet.ValueRW.LifeTime -= SystemAPI.Time.DeltaTime;

            if (bullet.ValueRO.LifeTime < 0)
            {
                ecb.DestroyEntity(entity);
            }
        }

        ecb.Playback(state.EntityManager);
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
        
    }
}
