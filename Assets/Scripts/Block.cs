using UnityEngine;

public enum Block
{
    air,
    dirt
}

public class BlockData
{
    public Block block = Block.air;
    public Transform transform;
    public MeshFilter[] meshFilters = new MeshFilter[3];
    public MeshRenderer[] meshRenderers = new MeshRenderer[3];
    public MeshCollider[] meshColliders = new MeshCollider[3];
}