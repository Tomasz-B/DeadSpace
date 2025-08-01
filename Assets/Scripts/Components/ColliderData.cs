using Unity.Entities;

public struct ColliderData : IComponentData
{
    public ColliderLayer Layer;
    public float Radius;             // assuming for simplicity that everything is an orb
}

public enum ColliderLayer
{
    Player      = 367623,
    Enemy       = 857333,
    Bullet      = 935852,
    Upgrade     = 532672,
}
