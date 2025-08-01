using Unity.Entities;
using UnityEngine;

class PlayerAuthoring : MonoBehaviour
{

    private class Baker : Baker<PlayerAuthoring>
    {
        public override void Bake(PlayerAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);

            AddComponent(entity, new PlayerData());
        }
    }

}
