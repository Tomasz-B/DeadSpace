using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class BulletAuthoring : MonoBehaviour
{
    public int PlayerId;
    public int3 Direction;
    public float Speed;
    public int Power;
    public float LifeTime;


    private class Baker : Baker<BulletAuthoring>
    {
        public override void Bake(BulletAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);

            AddComponent(entity,
                new BulletData
                {
                    PlayerId = authoring.PlayerId,
                    Direction = authoring.Direction,
                    Speed = authoring.Speed,
                    Power = authoring.Power,
                    LifeTime = authoring.LifeTime,
                });
        }
    }
}
