using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshCreator : MonoBehaviour
{
    //private int[] meshFront = new int[] { 0, 2, 1, 0, 3, 2 };
    //private int[] meshTop = new int[] { 2, 3, 4, 2, 4, 5 };
    //private int[] meshRight = new int[] { 1, 2, 5, 1, 5, 6 };
    //private int[] meshLeft = new int[] { 0, 7, 4, 0, 4, 3 };
    //private int[] meshBack = new int[] { 5, 4, 7, 5, 7, 6 };
    //private int[] meshBottom = new int[] { 0, 6, 7, 0, 1, 6 };

    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;
    private MeshCollider meshCollider;

    private void Awake()
    {
        meshFilter = GetComponent<MeshFilter>();
        meshRenderer = GetComponent<MeshRenderer>();
        meshCollider = GetComponent<MeshCollider>();
    }

    public bool BuildMesh(Chunk chunk, Vector3Int pos, BlockData blockData, out List<MeshFilter> meshFilterList, out List<MeshRenderer> meshRendererList)
    {
        meshFilterList = new List<MeshFilter>();
        meshRendererList = new List<MeshRenderer>();
        bool flag = false;

        int x = pos.x;
        int y = pos.y;
        int z = pos.z;

        bool[] isAirBlock = { false, false, false, false, false, false };

        if (chunk.GetBlock(x, y, z + 1) == Block.air) isAirBlock[0] = true;
        if (chunk.GetBlock(x, y, z - 1) == Block.air) isAirBlock[1] = true;
        if (chunk.GetBlock(x + 1, y, z) == Block.air) isAirBlock[2] = true;
        if (chunk.GetBlock(x - 1, y, z) == Block.air) isAirBlock[3] = true;
        if (chunk.GetBlock(x, y + 1, z) == Block.air) isAirBlock[4] = true;
        if (chunk.GetBlock(x, y - 1, z) == Block.air) isAirBlock[5] = true;

        for (int i = 0; i < 6; i++)
        {
            if (isAirBlock[i])
            {
                meshFilterList.Add(blockData.meshFilters[i]);
                meshRendererList.Add(blockData.meshRenderers[i]);
                flag = true;
            }
        }

        return flag;
    }

    public void Combine(MeshFilter[] meshFilters, MeshRenderer[] meshRenderers)
    {
        List<Material> materials = new List<Material>();
        for (int i = 0; i < meshRenderers.Length; i++)
        {
            if (!materials.Contains(meshRenderers[i].sharedMaterial))
            {
                materials.Add(meshRenderers[i].sharedMaterial);
            }
        }
        
        List<Mesh> meshes = new List<Mesh>();
        for (int i = 0; i < materials.Count; i++)
        {
            List<CombineInstance> combine = new List<CombineInstance>();
            for (int j = 0; j < meshFilters.Length; j++)
            {
                if (materials[i] == meshRenderers[j].sharedMaterial)
                {
                    CombineInstance ci = new CombineInstance();
        
                    ci.mesh = meshFilters[j].sharedMesh;
                    ci.transform = meshFilters[j].transform.localToWorldMatrix;
                    ci.subMeshIndex = 0;
                    combine.Add(ci);
                }
            }
            Mesh mesh = new Mesh();
            mesh.CombineMeshes(combine.ToArray(), true);
            meshes.Add(mesh);
        }
        
        List<CombineInstance> combineFinal = new List<CombineInstance>();
        for (int i = 0; i < meshes.Count; i++)
        {
            CombineInstance ci = new CombineInstance();
            ci.mesh = meshes[i];
            ci.transform = Matrix4x4.identity;
            ci.subMeshIndex = 0;
            combineFinal.Add(ci);
        }
        
        meshFilter.mesh.CombineMeshes(combineFinal.ToArray(), false);
        meshRenderer.materials = materials.ToArray();
        meshCollider.sharedMesh = meshFilter.sharedMesh;
        gameObject.SetActive(true);
    }
}
