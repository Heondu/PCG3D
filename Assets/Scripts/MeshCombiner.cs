using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshCombiner : MonoBehaviour
{
    private MeshCollider meshCollider;
    private MeshFilter meshFilter;

    private void Awake()
    {
        meshCollider = GetComponent<MeshCollider>();
        meshFilter = GetComponent<MeshFilter>();
    }

    [ContextMenu("Combine")]
    public void Combine()
    {
        meshFilter.mesh.Clear();

        MeshFilter[] meshFilters = new MeshFilter[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            meshFilters[i] = transform.GetChild(i).GetComponent<MeshFilter>();
        }
        CombineInstance[] combine = new CombineInstance[meshFilters.Length];

        for (int i = 0; i < meshFilters.Length; i++)
        {
            combine[i].mesh = meshFilters[i].sharedMesh;
            combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
            meshFilters[i].gameObject.SetActive(false);
        }
        meshFilter.mesh.CombineMeshes(combine);
        meshFilter.gameObject.SetActive(true);
        meshCollider.sharedMesh = meshFilter.sharedMesh;
    }
}
