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
    public int minHeight = 5;
    public int scaleHeight = 10;
    public int aroundChunkNum = 1;
    public int chunkLoadDistance = 0;

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
        noiseWidth = width + 2;
        noiseLength = length + 2;
    }

    private void Start()
    {
        for (int x = -aroundChunkNum; x <= aroundChunkNum; x++)
        {
            for (int z = -aroundChunkNum; z <= aroundChunkNum; z++)
            {
                Vector3 pos = new Vector3(width * x, 0, length * z);

                GameObject chunkObjClone = Instantiate(chunkObj, pos, Quaternion.identity, chunkHolder);
                ChunkLoader chunkLoader = chunkObjClone.GetComponent<ChunkLoader>();
                Chunk chunk = new Chunk();
                chunk.blocks = new Block[noiseWidth, height, noiseLength];
                chunkLoader.SetChunk(chunk);

                CreateWorld(chunkLoader, pos);
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

    public void CreateWorld(ChunkLoader chunkLoader, Vector3 pos)
    {
        CreateChunk(chunkLoader, Vector3Int.RoundToInt(pos));
    }

    private void UpdateWorld()
    {
        for (int i = 0; i < chunkHolder.childCount; i++)
        {
            CreateChunk(chunkHolder.GetChild(i).GetComponent<ChunkLoader>(), Vector3Int.RoundToInt(chunkHolder.GetChild(i).position));
        }
    }

    private void CreateChunk(ChunkLoader chunkLoader, Vector3Int pos)
    {
        chunkLoader.chunk.width = noiseWidth;
        chunkLoader.chunk.height = height;
        chunkLoader.chunk.length = noiseLength;

        for (int x = 0; x < noiseWidth; x++)
        {
            for (int z = 0; z < noiseLength; z++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (GetHeight(perlinNoise.GenerateMap(x + pos.x, y, z + pos.z), y) > 0f)
                    //if (perlinNoise.GenerateMap(x + pos.x, y, z + pos.z) > 0f)
                    {
                        chunkLoader.chunk.blocks[x, y, z] = Block.dirt;
                    }
                    else
                    {
                        chunkLoader.chunk.blocks[x, y, z] = Block.air;
                    }
                }
            }
        }
        chunkLoader.UpdateChunk();
    }

    public float GetHeight(float perlinValue, int y)
    {
        //float heightValue = 1f / (maxHeight - minHeight);
        //float yCoord = Mathf.Floor(perlinValue / heightValue) + minHeight - y;
        //return Mathf.Clamp((int)yCoord, 0, maxHeight - 1);

        return perlinValue * scaleHeight + minHeight - y;
    }
}
