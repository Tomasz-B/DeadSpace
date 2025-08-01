using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.NetCode;
using Unity.Transforms;
using UnityEngine;
using Random = UnityEngine.Random;


[WorldSystemFilter(WorldSystemFilterFlags.ServerSimulation)]
partial struct ServerConnectionSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        
    }

    public void OnDestroy(ref SystemState state)
    {

    }

    public void OnUpdate(ref SystemState state)
    {
        EntityCommandBuffer ecb = new EntityCommandBuffer(Allocator.Temp);

        foreach
            (
            (RefRO<ReceiveRpcCommandRequest> recRpcComReq, Entity entity)
            in
            SystemAPI.Query<RefRO<ReceiveRpcCommandRequest>>().WithAll<IngameRpc>().WithEntityAccess()
            )
        {

            ecb.AddComponent<NetworkStreamInGame>(recRpcComReq.ValueRO.SourceConnection);

            Debug.Log("Client connected to ingame " + recRpcComReq.ValueRO.SourceConnection);

            SpawnPlayer(
                ref state,
                ref ecb,
                SystemAPI.GetSingleton<PlayerEntityReference>(),
                recRpcComReq.ValueRO.SourceConnection
                );


            ecb.DestroyEntity(entity);
        }

        ecb.Playback(state.EntityManager);
    }

    private void SpawnPlayer(ref SystemState state, ref EntityCommandBuffer ecb, PlayerEntityReference reference, Entity clientEntity)
    {
        int networkId = SystemAPI.GetComponent<NetworkId>(clientEntity).Value;

        Entity player = ecb.Instantiate(reference.PlayerPrefabEntity);
        ecb.SetComponent(player, LocalTransform.FromPosition(new float3(Random.Range(-5, 5), 1f, 0)));
        ecb.AddComponent(player, new GhostOwner { NetworkId = networkId });
        ecb.SetComponent(player, new PlayerData { PlayerId = 25 });
        
        ecb.AppendToBuffer(clientEntity, new LinkedEntityGroup() { Value = player});

        Debug.Log("Spawned player prefab for player " + networkId);
    }


}
