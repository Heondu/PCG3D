using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGeneratorByPerlinNoise : MonoBehaviour
{
    public int width = 16;
    public int height = 16;
    public int length = 16;
    private int noiseWidth;
    private int noiseLength;
    public float scale = 20.0f;
    public int octaves = 2;
    public float persistance = 0.5f;
    public float lacunarity = 2;
    public int minHeight = 5;
    public int maxHeight = 10;
    public int aroundChunkNum = 1;
    public int chunkLoadDistance = 0;

    public float xOrg = 0;
    public float zOrg = 0;

    public string seed;
    public bool useRandomSeed;
    public bool AutoUpdateMap;

    [SerializeField]
    private PerlinNoise perlinNoise;
    [SerializeField]
    private GameObject chunkObj;
    [SerializeField]
    private Transform chunkHolder;
    [SerializeField]
    private GameObject cube;

    private void Awake()
    {
        chunkLoadDistance = width * aroundChunkNum + width / 2;

        StartCoroutine("CoUpdateWorld");
    }

    private void Start()
    {
        for (int x = -aroundChunkNum; x <= aroundChunkNum; x++)
        {
            for (int z = -aroundChunkNum; z <= aroundChunkNum; z++)
            {
                Vector3 pos = new Vector3(width * x, 0, length * z);
                GameObject chunkObjClone = Instantiate(chunkObj, pos, Quaternion.identity, chunkHolder);
                CreateWorld(chunkObjClone.GetComponent<ChunkLoader>(), pos);
            }
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            UpdateWorld();
        }
    }

    private IEnumerator CoUpdateWorld()
    {
        while (true)
        {
            if (AutoUpdateMap)
            {
                UpdateWorld();
            }
            yield return new WaitForSeconds(1f);
        }
    }

    private void SetSeed()
    {
        if (useRandomSeed) seed = Time.time.ToString();
        System.Random pseudoRandom = new System.Random(seed.GetHashCode());
        xOrg = pseudoRandom.Next(0, 99999);
        zOrg = pseudoRandom.Next(0, 99999);
    }

    public void CreateWorld(ChunkLoader chunkLoader, Vector3 pos)
    {
        SetSeed();
        
        float[,] noiseMap = GenerateMap(pos);
        CreateChunk(noiseMap, chunkLoader);
    }

    private void UpdateWorld()
    {
        SetSeed();

        for (int i = 0; i < chunkHolder.childCount; i++)
        {
            float[,] noiseMap = GenerateMap(chunkHolder.GetChild(i).position);
            CreateChunk(noiseMap, chunkHolder.GetChild(i).GetComponent<ChunkLoader>());
        }
    }

    private float[,] GenerateMap(Vector3 pos)
    {
        noiseWidth = width + 2;
        noiseLength = length + 2;
        return perlinNoise.GenerateMap(noiseWidth, noiseLength, scale, octaves, persistance, lacunarity, xOrg, zOrg, pos.x, pos.z);
    }

    private void CreateChunk(float[,] noiseMap, ChunkLoader chunkLoader)
    {
        Chunk chunk = new Chunk();
        chunk.blocks = new Block[noiseWidth, height, noiseLength];

        for (int x = 0; x < noiseWidth; x++)
        {
            for (int z = 0; z < noiseLength; z++)
            {
                int yCoord = GetHeight(noiseMap[x, z]);

                for (int y = 0; y < height; y++)
                {
                    if (y <= yCoord)
                    {
                        chunk.blocks[x, y, z] = Block.dirt;
                    }
                    else
                    {
                        chunk.blocks[x, y, z] = Block.air;
                    }
                }
            }
        }
        chunkLoader.SetChunk(chunk);
        chunkLoader.CreateMesh();
    }

    public int GetHeight(float perlinValue)
    {
        float heightValue = 1f / (maxHeight - minHeight);
        float yCoord = Mathf.Floor(perlinValue / heightValue) + minHeight;
        return Mathf.Clamp((int)yCoord, 0, maxHeight - 1);
    }
}
