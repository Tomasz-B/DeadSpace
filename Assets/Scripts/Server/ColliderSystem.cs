using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

[WorldSystemFilter(WorldSystemFilterFlags.ServerSimulation)]
partial struct ColliderSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        
    }

    // With massive collisions it's preferred to use some Octree-type structures or something.

    //[BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        EntityCommandBuffer ecb = new EntityCommandBuffer(Allocator.Temp);
        List<Entity> entitiesToDestroy = new List<Entity>();

        foreach
            ((
            RefRO<ColliderData> col1,
            RefRW<LocalTransform> trans1,
            Entity e1)
            in
            SystemAPI.Query<RefRO<ColliderData>, RefRW<LocalTransform>>().WithEntityAccess().WithAll<Simulate>()
            )
        {
            foreach
                ((
                RefRO<ColliderData> col2,
                RefRW<LocalTransform> trans2,
                Entity e2)
                in
                SystemAPI.Query<RefRO<ColliderData>, RefRW<LocalTransform>>().WithEntityAccess().WithAll<Simulate>()
                )
            {
                if (CanLayersCollide(col1.ValueRO, col2.ValueRO))
                {
                    if (math.distancesq(trans1.ValueRO.Position, trans2.ValueRO.Position) <= ((col1.ValueRO.Radius + col2.ValueRO.Radius) * (col1.ValueRO.Radius + col2.ValueRO.Radius)))
                    {
                        // Collision occured!
                        // Debug.Log("Collision occured!");

                        // Not checking otherwise, because another foreach-foreach will find it anyway
                        if (col1.ValueRO.Layer == ColliderLayer.Player && col2.ValueRO.Layer == ColliderLayer.Upgrade)
                        {
                            PlayerData player = state.EntityManager.GetComponentData<PlayerData>(e1);
                            UpgradeData upg = state.EntityManager.GetComponentData<UpgradeData>(e2);

                            player.ShootingSpeedBonus += upg.UpgradeShootingSpeed;
                            player.BulletSpeedBonus += upg.UpgradeBulletSpeed;
                            player.BulletPowerBonus += upg.UpgradeBulletPower;
                            player.BulletLifeBonus += upg.UpgradeBulletLife;

                            ecb.SetComponent(e1, player);
                            entitiesToDestroy.Add(e2);

                            //Debug.Log("Destroying upgrade " + e2);
                        }

                        else if (col1.ValueRO.Layer == ColliderLayer.Enemy && col2.ValueRO.Layer == ColliderLayer.Bullet)
                        {
                            EnemyData enemy = state.EntityManager.GetComponentData<EnemyData>(e1);
                            BulletData bullet = state.EntityManager.GetComponentData<BulletData>(e2);

                            enemy.Health -= bullet.Power;

                            if (enemy.Health > 0)
                            {
                                ecb.SetComponent(e1, enemy);
                            }
                            else
                            {
                                entitiesToDestroy.Add(e1);
                                //Debug.Log("Destroying enemy " + e1);
                            }

                            entitiesToDestroy.Add(e2);
                            //Debug.Log("Destroying bullet " + e2);
                        }

                    }
                }
            }
        }

        foreach (Entity e in entitiesToDestroy)
        {
            ecb.DestroyEntity(e);
        }

        ecb.Playback(state.EntityManager);
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
        
    }

    private bool CanLayersCollide(ColliderData col1, ColliderData col2)
    {
        return
            (col1.Layer != col2.Layer)
            &&
            (
            (col1.Layer == ColliderLayer.Bullet && col2.Layer == ColliderLayer.Enemy) ||
            (col1.Layer == ColliderLayer.Enemy && col2.Layer == ColliderLayer.Bullet) ||

            (col1.Layer == ColliderLayer.Player && col2.Layer == ColliderLayer.Upgrade) ||
            (col1.Layer == ColliderLayer.Upgrade && col2.Layer == ColliderLayer.Player)
            );
    }
}
