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

    public bool BuildMesh(Chunk chunk, Vector3Int pos, BlockData blockData, out MeshFilter[] meshFilters, out MeshRenderer[] meshRenderers)
    {
        List<MeshFilter> meshFilterList = new List<MeshFilter>();
        List<MeshRenderer> meshRendererList = new List<MeshRenderer>();
        bool flag = false;

        if (chunk.GetBlock(pos.x, pos.y, pos.z + 1) == Block.air)
        { 
            meshFilterList.Add(blockData.meshFilters[0]); //back
            meshRendererList.Add(blockData.meshRenderers[0]); //back
            flag = true;
        }
        if (chunk.GetBlock(pos.x, pos.y, pos.z - 1) == Block.air)
        {
            meshFilterList.Add(blockData.meshFilters[1]); //front
            meshRendererList.Add(blockData.meshRenderers[1]); //front
            flag = true;
        }
        if (chunk.GetBlock(pos.x + 1, pos.y, pos.z) == Block.air)
        {
            meshFilterList.Add(blockData.meshFilters[2]); //right
            meshRendererList.Add(blockData.meshRenderers[2]); //right
            flag = true;
        }
        if (chunk.GetBlock(pos.x - 1, pos.y, pos.z) == Block.air)
        {
            meshFilterList.Add(blockData.meshFilters[3]); // left
            meshRendererList.Add(blockData.meshRenderers[3]); // left
            flag = true;
        }
        if (chunk.GetBlock(pos.x, pos.y + 1, pos.z) == Block.air)
        {
            meshFilterList.Add(blockData.meshFilters[4]); //top
            meshRendererList.Add(blockData.meshRenderers[4]); //top
            flag = true;
        }

        meshFilters = meshFilterList.ToArray();
        meshRenderers = meshRendererList.ToArray();
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
                    meshFilters[j].gameObject.SetActive(false);
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
