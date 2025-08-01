using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.NetCode;
using UnityEngine.InputSystem;

[UpdateInGroup(typeof(GhostInputSystemGroup))]
partial struct InputSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        
    }

    
    public void OnUpdate(ref SystemState state)
    {
        foreach
            (
            RefRW<InputData> inputData
            in
            SystemAPI.Query<RefRW<InputData>>().WithAll<GhostOwnerIsLocal>()
            )
        {
            int2 wsadInput = int2.zero;

            if (Keyboard.current[Key.W].isPressed)
            {
                wsadInput.y -= 1;
            }
            if (Keyboard.current[Key.S].isPressed)
            {
                wsadInput.y += 1;
            }
            if (Keyboard.current[Key.A].isPressed)
            {
                wsadInput.x += 1;
            }
            if (Keyboard.current[Key.D].isPressed)
            {
                wsadInput.x -= 1;
            }

            bool spaceInput = Keyboard.current[Key.Space].isPressed;

            inputData.ValueRW.DirectionInput = wsadInput;
            inputData.ValueRW.ShotInput = spaceInput;
        }
        
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
        
    }
}
