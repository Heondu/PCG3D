using UnityEngine;

public enum Block
{
    air,
    dirt
}

public class BlockData : MonoBehaviour
{
    public Block block = Block.air;
    public MeshFilter[] meshFilters = new MeshFilter[6];
    public MeshRenderer[] meshRenderers = new MeshRenderer[6];
    //public MeshCollider[] meshColliders = new MeshCollider[3];
}