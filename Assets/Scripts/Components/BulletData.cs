using Unity.Entities;
using Unity.Mathematics;

public struct BulletData : IComponentData
{
    public int PlayerId;
    public int3 Direction;
    public float Speed;
    public int Power;
    public float LifeTime;
    
}
