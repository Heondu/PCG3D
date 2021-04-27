using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshCreator : MonoBehaviour
{
    [SerializeField]
    private GameObject cube;

    private int[] meshFront = new int[] { 0, 2, 1, 0, 3, 2 };
    private int[] meshTop = new int[] { 2, 3, 4, 2, 4, 5 };
    private int[] meshRight = new int[] { 1, 2, 5, 1, 5, 6 };
    private int[] meshLeft = new int[] { 0, 7, 4, 0, 4, 3 };
    private int[] meshBack = new int[] { 5, 4, 7, 5, 7, 6 };
    //private int[] meshBottom = new int[] { 0, 6, 7, 0, 1, 6 };

    private MeshCollider meshCollider;
    private MeshFilter meshFilter;

    private void Awake()
    {
        meshCollider = GetComponent<MeshCollider>();
        meshFilter = GetComponent<MeshFilter>();
    }

    public void CreateMeshOnChild(Chunk chunk)
    {
        int width = chunk.blocks.GetLength(0);
        int height = chunk.blocks.GetLength(1);
        int length = chunk.blocks.GetLength(2);

        int index = 0;
        for (int x = 1; x < width - 1; x++)
        {
            for (int z = 1; z < length - 1; z++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (chunk.blocks[x, y, z] != Block.air)
                    {
                        Vector3Int newPos = new Vector3Int(x, y, z);
                        GameObject cubeClone;
                        Mesh mesh;
                        if (transform.childCount > index)
                        {
                            cubeClone = transform.GetChild(index).gameObject;
                            cubeClone.SetActive(true);
                            cubeClone.transform.position = newPos - Vector3.one * ((float)(width - 2) / 2);
                            mesh = cubeClone.GetComponent<MeshFilter>().mesh;
                        }
                        else
                        {
                            cubeClone = Instantiate(cube, newPos - Vector3.one * ((float)(width - 2) / 2), Quaternion.identity, transform);
                            mesh = new Mesh();
                        }
                        cubeClone.GetComponent<MeshFilter>().mesh = BuildMesh(chunk, newPos, mesh);
                        index++;
                    }
                }
            }
        }

        Combine();
        meshFilter.mesh.RecalculateNormals();
    }

    public Mesh BuildMesh(Chunk chunk, Vector3Int pos, Mesh mesh)
    {
        List<int[]> triangleList = new List<int[]>();
        if (chunk.GetBlock(pos.x, pos.y, pos.z + 1) == Block.air) triangleList.Add(meshBack);
        if (chunk.GetBlock(pos.x, pos.y, pos.z - 1) == Block.air) triangleList.Add(meshFront);
        if (chunk.GetBlock(pos.x + 1, pos.y, pos.z) == Block.air) triangleList.Add(meshRight);
        if (chunk.GetBlock(pos.x - 1, pos.y, pos.z) == Block.air) triangleList.Add(meshLeft);
        if (chunk.GetBlock(pos.x, pos.y + 1, pos.z) == Block.air) triangleList.Add(meshTop);

        int[] triangles = new int[triangleList.Count * 6];
        for (int i = 0; i < triangleList.Count; i++)
        {
            for (int j = 0; j < triangleList[i].Length; j++)
            {
                triangles[i * triangleList[i].Length + j] = triangleList[i][j];
            }
        }

        mesh.Clear();
        mesh.vertices = new Vector3[]
        {
            new Vector3(0, 0, 0),
            new Vector3(1, 0, 0),
            new Vector3(1, 1, 0),
            new Vector3(0, 1, 0),
            new Vector3(0, 1, 1),
            new Vector3(1, 1, 1),
            new Vector3(1, 0, 1),
            new Vector3(0, 0, 1)
        };
        mesh.triangles = triangles;

        mesh.RecalculateNormals();

        return mesh;
    }

    public void Combine()
    {
        meshFilter.mesh.Clear();

        List<MeshFilter> meshFilterList = new List<MeshFilter>();
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).gameObject.activeSelf == true)
                meshFilterList.Add(transform.GetChild(i).GetComponent<MeshFilter>());
        }
        MeshFilter[] meshFilters = meshFilterList.ToArray();
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
