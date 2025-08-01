using Unity.Entities;

public struct EnemySystemData : ICleanupComponentData
{
    public float Timer;
    public int EnemySpawnedCount;
}
