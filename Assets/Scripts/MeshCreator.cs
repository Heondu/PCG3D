using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshCreator : MonoBehaviour
{
    private int[] meshFront = new int[] { 0, 2, 1, 0, 3, 2 };
    private int[] meshTop = new int[] { 2, 3, 4, 2, 4, 5 };
    private int[] meshRight = new int[] { 1, 2, 5, 1, 5, 6 };
    private int[] meshLeft = new int[] { 0, 7, 4, 0, 4, 3 };
    private int[] meshBack = new int[] { 5, 4, 7, 5, 7, 6 };
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

    public void BuildMesh(Chunk chunk, Vector3Int pos, GameObject block)
    {
        if (chunk.GetBlock(pos.x, pos.y, pos.z + 1) == Block.air)
        { 
            block.transform.Find("Back").gameObject.SetActive(true);
            block.transform.gameObject.SetActive(true);
        }
        if (chunk.GetBlock(pos.x, pos.y, pos.z - 1) == Block.air)
        { 
            block.transform.Find("Front").gameObject.SetActive(true);
            block.transform.gameObject.SetActive(true);
        }
        if (chunk.GetBlock(pos.x + 1, pos.y, pos.z) == Block.air)
        { 
            block.transform.Find("Right").gameObject.SetActive(true);
            block.transform.gameObject.SetActive(true);
        }
        if (chunk.GetBlock(pos.x - 1, pos.y, pos.z) == Block.air)
        { 
            block.transform.Find("Left").gameObject.SetActive(true);
            block.transform.gameObject.SetActive(true);
        }
        if (chunk.GetBlock(pos.x, pos.y + 1, pos.z) == Block.air)
        {
            block.transform.Find("Top").gameObject.SetActive(true);
            block.transform.gameObject.SetActive(true);
        }
    }

    public void Combine()
    {
        MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>(false);

        CombineInstance[] combine = new CombineInstance[meshFilters.Length - 1];

        for (int i = 1; i < meshFilters.Length; i++)
        {
            combine[i - 1].mesh = meshFilters[i].sharedMesh;
            combine[i - 1].transform = meshFilters[i].transform.localToWorldMatrix;
            meshFilters[i].gameObject.SetActive(false);
            meshFilters[i].transform.parent.gameObject.SetActive(false);
        }
        if (meshFilters.Length > 1)
        {
            //meshRenderer.material = meshFilters[1].GetComponent<MeshRenderer>().material;
            meshFilter.mesh.CombineMeshes(combine);
            meshFilter.mesh.RecalculateNormals();
            meshFilter.gameObject.SetActive(true);
            meshCollider.sharedMesh = meshFilter.sharedMesh;
        }
    }


    //public void Combine(BlockData blockData)
    //{
    //    for (int i = 0; i < blockData.meshFilters.Length; i++)
    //    {
    //        blockData.meshFilters[i].gameObject.SetActive(false);
    //    }
    //
    //    List<MeshFilter> meshFilterListTop = new List<MeshFilter>();
    //    List<MeshFilter> meshFilterListSide = new List<MeshFilter>();
    //    List<MeshFilter> meshFilterListBottom = new List<MeshFilter>();
    //    for (int i = 0; i < blockData.transform.childCount; i++)
    //    {
    //        if (blockData.transform.GetChild(i).gameObject.activeSelf == false) continue;
    //
    //        for (int j = 0; j < blockData.transform.GetChild(i).childCount; j++)
    //        {
    //            if (blockData.transform.GetChild(i).GetChild(j).gameObject.activeSelf == false) continue;
    //
    //            if (blockData.transform.GetChild(i).GetChild(j).name == "Top")
    //            {
    //                meshFilterListTop.Add(blockData.transform.GetChild(i).GetChild(j).GetComponent<MeshFilter>());
    //            }
    //            else if (blockData.transform.GetChild(i).GetChild(j).name == "Bottom")
    //            {
    //                meshFilterListBottom.Add(blockData.transform.GetChild(i).GetChild(j).GetComponent<MeshFilter>());
    //            }
    //            else
    //            {
    //                meshFilterListSide.Add(blockData.transform.GetChild(i).GetChild(j).GetComponent<MeshFilter>());
    //            }
    //        }
    //    }
    //
    //    CombineMesh(blockData.meshFilters[0], blockData.meshRenderers[0], blockData.meshColliders[0], meshFilterListTop.ToArray());
    //    CombineMesh(blockData.meshFilters[1], blockData.meshRenderers[1], blockData.meshColliders[1], meshFilterListSide.ToArray());
    //    CombineMesh(blockData.meshFilters[2], blockData.meshRenderers[2], blockData.meshColliders[2], meshFilterListBottom.ToArray());
    //}
    //private void CombineMesh(MeshFilter meshFilter, MeshRenderer meshRenderer, MeshCollider meshCollider, MeshFilter[] meshFilters)
    //{
    //    CombineInstance[] combine = new CombineInstance[meshFilters.Length];
    //
    //    for (int i = 0; i < meshFilters.Length; i++)
    //    {
    //        combine[i].mesh = meshFilters[i].sharedMesh;
    //        combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
    //        meshFilters[i].gameObject.SetActive(false);
    //        meshFilters[i].transform.parent.gameObject.SetActive(false);
    //    }
    //    if (meshFilters.Length != 0)
    //    {
    //        meshRenderer.material = meshFilters[0].GetComponent<MeshRenderer>().material;
    //        meshFilter.mesh.CombineMeshes(combine);
    //        meshFilter.mesh.RecalculateNormals();
    //        meshFilter.gameObject.SetActive(true);
    //        meshCollider.sharedMesh = meshFilter.sharedMesh;
    //    }
    //}
}
