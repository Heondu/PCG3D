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
    private int[] meshBottom = new int[] { 0, 6, 7, 0, 1, 6 };

    //private void Start()
    //{
    //    BuildMesh();
    //}

    public void BuildMesh(int[,] heightMap, Vector3Int pos)
    {
        int width = heightMap.GetLength(0);
        int height = heightMap.GetLength(1);

        List<int[]> triangleList = new List<int[]>();
        triangleList.Add(meshTop);
        if (pos.z + 1 < height && heightMap[pos.x, pos.z + 1] != pos.y) triangleList.Add(meshBack);
        if (pos.z - 1 > -1 && heightMap[pos.x, pos.z - 1] != pos.y) triangleList.Add(meshFront);
        if (pos.x + 1 < width && heightMap[pos.x + 1, pos.z] != pos.y) triangleList.Add(meshRight);
        if (pos.x - 1 > -1 && heightMap[pos.x - 1, pos.z] != pos.y) triangleList.Add(meshLeft);

        int[] triangles = new int[triangleList.Count * 6];
        for (int i = 0; i < triangleList.Count; i++)
        {
            for (int j = 0; j < triangleList[i].Length; j++)
            {
                triangles[i * triangleList[i].Length + j] = triangleList[i][j];
            }
        }

        Mesh mesh = new Mesh();
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
        GetComponent<MeshFilter>().mesh = mesh;
    }
}
