using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.NetCode;
using Unity.Transforms;

[UpdateInGroup(typeof(PredictedSimulationSystemGroup))]
partial struct MovementSystem : ISystem
{
    private const float MOVEMENT_SPEED = 5f;

    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        foreach
            (
            (RefRO<InputData> inputData, RefRW<LocalTransform> transform, RefRW<PlayerData> playerdData)
            in
            SystemAPI.Query<RefRO<InputData>, RefRW<LocalTransform>, RefRW<PlayerData>>().WithAll<Simulate>()
            )
        {
            float3 move = new float3(
                inputData.ValueRO.DirectionInput.x * SystemAPI.Time.DeltaTime * MOVEMENT_SPEED,
                0,
                inputData.ValueRO.DirectionInput.y * SystemAPI.Time.DeltaTime * MOVEMENT_SPEED
                );

            transform.ValueRW.Position += move;

            if (inputData.ValueRO.ShotInput)
            {
                playerdData.ValueRW.PlayerId = UnityEngine.Random.Range(5, 555);
            }
            
        }

    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
        
    }
}
