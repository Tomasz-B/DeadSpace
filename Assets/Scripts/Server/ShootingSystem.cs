using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.NetCode;
using Unity.Transforms;
using UnityEngine;

[WorldSystemFilter(WorldSystemFilterFlags.ServerSimulation)]
partial struct ShootingSystem : ISystem
{
    private int3 DEFAULT_DIRECTION { get { return new int3(0, 0, -1); } }

    private const float DEFAULT_SPEED = 10f;
    private const int DEFAULT_POWER = 10;
    private const float DEFAULT_LIFETIME = 1f;

    private const float SPEED_MULTIPLIER = 1f;
    private const int POWER_MULTIPLIER = 1;
    private const float LIFETIME_MULTIPLIER = 0.1f;


    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        BulletEntityReference bulletEntityReference = SystemAPI.GetSingleton<BulletEntityReference>();

        EntityCommandBuffer ecb = new EntityCommandBuffer(Allocator.Temp);

        foreach
            ((
            RefRO<InputData> inputData,
            RefRO<LocalTransform> transform,
            RefRW<PlayerData> playerData)
            in
            SystemAPI.Query<RefRO<InputData>, RefRO<LocalTransform>, RefRW<PlayerData>>().WithAll<Simulate>()
            )
        {
            if (playerData.ValueRW.ShotDelay > 0)
            {
                // REDUCE COOLDOWN
                playerData.ValueRW.ShotDelay -= SystemAPI.Time.DeltaTime;

            }
            else if ((playerData.ValueRW.ShotDelay <= 0) && (inputData.ValueRO.ShotInput))
            {
                // SHOOT
                playerData.ValueRW.ShotDelay = CalculateShotDelay(playerData.ValueRO.ShootingSpeedBonus);

                Entity bullet = ecb.Instantiate(bulletEntityReference.BulletPrefabEntity);
                ecb.SetComponent(bullet, LocalTransform.FromPositionRotationScale(transform.ValueRO.Position, Quaternion.identity, 0.2f));
                ecb.SetComponent(bullet, new BulletData(){
                    PlayerId = playerData.ValueRO.PlayerId,
                    Direction = DEFAULT_DIRECTION,
                    Speed = DEFAULT_SPEED + SPEED_MULTIPLIER * playerData.ValueRO.BulletSpeedBonus,
                    Power = DEFAULT_POWER + POWER_MULTIPLIER * playerData.ValueRO.BulletPowerBonus,
                    LifeTime = DEFAULT_LIFETIME + LIFETIME_MULTIPLIER * playerData.ValueRO.BulletLifeBonus,
                });
            }
        }

        ecb.Playback(state.EntityManager);

    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
        
    }

    private float CalculateShotDelay(int delayReductionBonus)
    {
        return 10f / (10f + (float)delayReductionBonus);
        // 1.0f     at lvl 0
        // 0.5f     at lvl 10
        // 0.33f    at lvl 20
    }
}
