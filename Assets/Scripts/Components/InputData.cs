using Unity.Entities;
using Unity.Mathematics;
using Unity.NetCode;

public struct InputData : IInputComponentData
{
    public int2 DirectionInput;
    public bool ShotInput;
}
