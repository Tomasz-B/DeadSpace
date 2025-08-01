using Unity.Entities;
using UnityEngine;

public class ColliderAuthoring : MonoBehaviour
{
    public ColliderLayer Layer;
    public float Radius;

    private class Baker : Baker<ColliderAuthoring>
    {
        public override void Bake(ColliderAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);

            AddComponent(entity, new ColliderData()
            {
                Layer = authoring.Layer,
                Radius = authoring.Radius,
            });
        }
    }
}
