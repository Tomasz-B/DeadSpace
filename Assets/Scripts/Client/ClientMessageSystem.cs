using Unity.Burst;
using Unity.Entities;
using Unity.NetCode;
using UnityEngine;
using UnityEngine.Scripting;
using UnityEngine.InputSystem;

[WorldSystemFilter(WorldSystemFilterFlags.ClientSimulation)]
public partial struct ClientMessageSystem : ISystem
{
    public void OnCreate(ref SystemState state) { }

    public void OnDestroy(ref SystemState state) { }

    public void OnUpdate(ref SystemState state)
    {
        if (Keyboard.current[Key.X].wasPressedThisFrame)
        {
            int randomVal = Random.Range(0, 99);
            Entity rpcEntity = state.EntityManager.CreateEntity();
            state.EntityManager.AddComponentData(rpcEntity, new ValueRpc { Value = randomVal });
            state.EntityManager.AddComponentData(rpcEntity, new SendRpcCommandRequest());
            Debug.Log("Sending value " + randomVal);
        }

        if (Keyboard.current[Key.U].wasPressedThisFrame)
        {
            Entity rpcEntity = state.EntityManager.CreateEntity();
            state.EntityManager.AddComponentData(rpcEntity, new UpgradeRpc { UpgradeShootingSpeed = 1 });
            state.EntityManager.AddComponentData(rpcEntity, new SendRpcCommandRequest());
            Debug.Log("Trying to ugprade shooting speed.");
        }

        if (Keyboard.current[Key.I].wasPressedThisFrame)
        {
            Entity rpcEntity = state.EntityManager.CreateEntity();
            state.EntityManager.AddComponentData(rpcEntity, new UpgradeRpc { UpgradeBulletSpeed = 1 });
            state.EntityManager.AddComponentData(rpcEntity, new SendRpcCommandRequest());
            Debug.Log("Trying to ugprade bullet speed.");
        }

        if (Keyboard.current[Key.O].wasPressedThisFrame)
        {
            Entity rpcEntity = state.EntityManager.CreateEntity();
            state.EntityManager.AddComponentData(rpcEntity, new UpgradeRpc { UpgradeBulletPower = 1 });
            state.EntityManager.AddComponentData(rpcEntity, new SendRpcCommandRequest());
            Debug.Log("Trying to ugprade bullet power.");
        }

        if (Keyboard.current[Key.P].wasPressedThisFrame)
        {
            Entity rpcEntity = state.EntityManager.CreateEntity();
            state.EntityManager.AddComponentData(rpcEntity, new UpgradeRpc { UpgradeBulletLife = 1 });
            state.EntityManager.AddComponentData(rpcEntity, new SendRpcCommandRequest());
            Debug.Log("Trying to ugprade bullet lifetime.");
        }
    }

}
