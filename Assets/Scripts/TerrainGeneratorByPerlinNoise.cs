using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGeneratorByPerlinNoise : MonoBehaviour
{
    public int width = 256;
    public int height = 256;
    public float scale = 1.0f;
    public int octaves = 3;
    public float persistance = 0.5f;
    public float lacunarity = 2;
    public int maxHeight = 5;
    public int chunkNum = 3;

    public float xOrg = 0;
    public float zOrg = 0;

    public string seed;
    public bool useRandomSeed;
    public bool AutoUpdateMap;
    public bool UpdateMap;

    [SerializeField]
    private PerlinNoise perlinNoise;
    [SerializeField]
    private GameObject cube;
    [SerializeField]
    private GameObject chunk;
    [SerializeField]
    private Transform chunkHolder;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (useRandomSeed) seed = Time.time.ToString();
            System.Random pseudoRandom = new System.Random(seed.GetHashCode());
            xOrg = pseudoRandom.Next(0, 99999);
            zOrg = pseudoRandom.Next(0, 99999);
            CreateWorld(UpdateMap);
        }
        if (AutoUpdateMap)
        {
            CreateWorld(true);
        }
    }

    private void CreateWorld(bool isUpdate)
    {
        for (int x = 0; x < chunkNum; x++)
        {
            for (int z = 0; z < chunkNum; z++)
            {
                Vector3 pos = new Vector3(width * x, 0, height * z);
                float[,] noiseMap = GenerateMap(pos);

                if (isUpdate == false)
                {
                    GameObject chunkObj = Instantiate(chunk, pos, Quaternion.identity, chunkHolder);
                    CreateChunk(noiseMap, chunkObj.transform);
                }
                else
                {
                    if (chunkHolder.childCount > x * chunkNum + z)
                    {
                        CreateChunk(noiseMap, chunkHolder.GetChild(x * chunkNum + z));
                    }
                }
            }
        }
    }

    private float[,] GenerateMap(Vector3 pos)
    {
        return perlinNoise.GenerateMap(width, height, scale, octaves, persistance, lacunarity, xOrg, zOrg, pos.x, pos.z);
    }

    public void CreateChunk(float[,] noiseMap, Transform chunkObj)
    {
        int width = noiseMap.GetLength(0);
        int height = noiseMap.GetLength(1);
        int[,] heightMap = SetHeightMap(noiseMap);    

        for (int i = 0; i < chunkHolder.childCount; i++)
        {
            for (int x = 0; x < width; x++)
            {
                for (int z = 0; z < height; z++)
                {
                    Vector3Int newPos = new Vector3Int(x, heightMap[x, z], z);
                    int index = x * height + z;
                    if (chunkObj.childCount > index)
                        chunkObj.GetChild(index).transform.position = newPos;
                    else Instantiate(cube, newPos, Quaternion.identity, chunkObj);

                    chunkObj.GetChild(index).GetComponent<MeshCreator>().BuildMesh(heightMap, newPos);
                }
            }
            chunkObj.GetComponent<MeshCombiner>().Combine();
        }
    }

    public int[,] SetHeightMap(float[,]  noiseMap)
    {
        int width = noiseMap.GetLength(0);
        int height = noiseMap.GetLength(1);
        int[,] heightMap = new int[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                float divide = 1f / maxHeight;
                float yCoord = Mathf.Floor(noiseMap[x, z] / divide);
                heightMap[x, z] = Mathf.Clamp((int)yCoord, 0, maxHeight - 1);
            }
        }

        return heightMap;
    }
}
