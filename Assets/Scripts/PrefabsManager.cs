using Unity.Entities;
using UnityEngine;

public class PrefabsManager : MonoBehaviour
{
    public static PrefabsManager Instance { get; private set; }


    [SerializeField]
    private GameObject playerPrefab;
    public GameObject PlayerPrefab { get { return playerPrefab; } }

    [SerializeField]
    private GameObject bulletPrefab;
    public GameObject BulletPrefab { get { return bulletPrefab; } }

    [SerializeField]
    private GameObject enemyPrefab;
    public GameObject EnemyPrefab { get { return enemyPrefab; } }

    [SerializeField]
    private GameObject upgradePrefab;
    public GameObject UpgradePrefab { get { return upgradePrefab; } }


    private void Awake()
    {
        Instance = this;
    }


    public class PlayerPrefabBaker : Baker<PrefabsManager>
    {
        public override void Bake(PrefabsManager authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new PlayerEntityReference
            {
                PlayerPrefabEntity = GetEntity(authoring.PlayerPrefab, TransformUsageFlags.Dynamic),
            });
            AddComponent(entity, new BulletEntityReference
            {
                BulletPrefabEntity = GetEntity(authoring.BulletPrefab, TransformUsageFlags.Dynamic),
            });
            AddComponent(entity, new EnemyEntityReference
            {
                EnemyPrefabEntity = GetEntity(authoring.EnemyPrefab, TransformUsageFlags.Dynamic),
            });
            AddComponent(entity, new UpgradeEntityReference
            {
                UpgradePrefabEntity = GetEntity(authoring.UpgradePrefab, TransformUsageFlags.Dynamic),
            });

        }
    }
}

public struct PlayerEntityReference : IComponentData
{
    public Entity PlayerPrefabEntity;
}

public struct BulletEntityReference : IComponentData
{
    public Entity BulletPrefabEntity;
}

public struct EnemyEntityReference : IComponentData
{
    public Entity EnemyPrefabEntity;
}

public struct UpgradeEntityReference : IComponentData
{
    public Entity UpgradePrefabEntity;
}
