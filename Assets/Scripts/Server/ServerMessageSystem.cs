using Unity.Collections;
using Unity.Entities;
using Unity.NetCode;
using UnityEngine;

[WorldSystemFilter(WorldSystemFilterFlags.ServerSimulation)]
public partial struct ServerMessageSystem : ISystem
{
    public void OnCreate(ref SystemState state) { }

    public void OnDestroy(ref SystemState state) { }

    public void OnUpdate(ref SystemState state)
    {
        EntityCommandBuffer ecb = new EntityCommandBuffer(Allocator.Temp);

        // SIMPLE RPC CHECK
        foreach
            (
            (RefRO<ValueRpc> valRpc, RefRO<ReceiveRpcCommandRequest> recRpcComReq, Entity entity)
            in
            SystemAPI.Query<RefRO<ValueRpc>, RefRO<ReceiveRpcCommandRequest>>().WithEntityAccess()
            )
        {

            Debug.Log("Recived value: " + valRpc.ValueRO.Value + " from " + recRpcComReq.ValueRO.SourceConnection);

            ecb.DestroyEntity(entity);
        }

        // UPGRADE RPC
        foreach
            (
            (RefRO<UpgradeRpc> upgradeRpc, RefRO<ReceiveRpcCommandRequest> recRpcComReq, Entity entity)
            in
            SystemAPI.Query<RefRO<UpgradeRpc>, RefRO<ReceiveRpcCommandRequest>>().WithEntityAccess()
            )
        {

            Debug.Log("Trying to upgrade"
                + (upgradeRpc.ValueRO.UpgradeShootingSpeed > 1 ? "" : " shooting speed")
                + (upgradeRpc.ValueRO.UpgradeBulletSpeed > 1 ? "" : " bullet speed")
                + (upgradeRpc.ValueRO.UpgradeBulletPower > 1 ? "" : " bullet power")
                + (upgradeRpc.ValueRO.UpgradeBulletLife > 1 ? "" : " bullet lifetime")
                + " for " + recRpcComReq.ValueRO.SourceConnection);


            foreach 
                (
                (RefRW<PlayerData> player, RefRO<GhostOwner> ghost)
                in
                SystemAPI.Query<RefRW<PlayerData>, RefRO<GhostOwner>>().WithAll<Simulate>()
                )
            {
                if (ghost.ValueRO.NetworkId == SystemAPI.GetComponent<NetworkId>(recRpcComReq.ValueRO.SourceConnection).Value)
                {
                    Debug.Log("Player upgrading!");

                    player.ValueRW.ShootingSpeedBonus += upgradeRpc.ValueRO.UpgradeShootingSpeed;
                    player.ValueRW.BulletSpeedBonus += upgradeRpc.ValueRO.UpgradeBulletSpeed;
                    player.ValueRW.BulletPowerBonus += upgradeRpc.ValueRO.UpgradeBulletPower;
                    player.ValueRW.BulletLifeBonus += upgradeRpc.ValueRO.UpgradeBulletLife;
                }
            }

            ecb.DestroyEntity(entity);
        }


        ecb.Playback(state.EntityManager);
    }

}
