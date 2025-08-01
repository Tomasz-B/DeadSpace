using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.NetCode;
using UnityEngine;
using UnityEngine.InputSystem;

[WorldSystemFilter(WorldSystemFilterFlags.ClientSimulation)]
partial struct ClientConnectionSystem : ISystem
{

    public void OnCreate(ref SystemState state)
    {
        
    }

    public void OnUpdate(ref SystemState state)
    {
        if (Keyboard.current[Key.C].wasPressedThisFrame)
        {
            EntityCommandBuffer ecb = new EntityCommandBuffer(Allocator.Temp);

            foreach
                (
                (RefRO<NetworkId> networkId, Entity entity)
                in
                SystemAPI.Query<RefRO<NetworkId>>().WithNone<NetworkStreamInGame>().WithEntityAccess()
                )
            {

                ecb.AddComponent<NetworkStreamInGame>(entity);
                Debug.Log("Connecting client to ingame ");

                Entity rpcEntity = ecb.CreateEntity();
                ecb.AddComponent(rpcEntity, new IngameRpc());
                ecb.AddComponent(rpcEntity, new SendRpcCommandRequest());
            }

            ecb.Playback(state.EntityManager);
        }

    }

    public void OnDestroy(ref SystemState state)
    {
        
    }
}
