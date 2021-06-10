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
    [SerializeField]
    private GameObject tree;
    [Range(0, 100)]
    [SerializeField]
    private int treeProb = 10;

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

    public void CreateWorld(ChunkLoader chunkLoader, Vector3 pos)
    {
        CreateChunk(chunkLoader, Vector3Int.RoundToInt(pos));
    }

    private void CreateChunk(ChunkLoader chunkLoader, Vector3Int pos)
    {
        chunkLoader.chunk.width = noiseWidth;
        chunkLoader.chunk.height = height;
        chunkLoader.chunk.length = noiseLength;

        List<Vector2Int> ground = new List<Vector2Int>();
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < noiseWidth; x++)
            {
                for (int z = 0; z < noiseLength; z++)
                {
                    float perlinValue = GetHeight(perlinNoise.GenerateMap(x + pos.x, y, z + pos.z), y);
                    if (perlinValue > 0)
                    {
                        chunkLoader.chunk.blocks[x, y, z] = Block.stone;

                        if (y == 0)
                        {
                            ground.Add(new Vector2Int(x, z));
                        }
                    }
                    else
                    {
                        if (y < 8)
                        {
                            chunkLoader.chunk.blocks[x, y, z] = Block.water;
                        }
                        else
                        {
                            chunkLoader.chunk.blocks[x, y, z] = Block.air;
                        }
                    }
                }
            }
        }

        for (int i = 0; i < ground.Count; i++)
        {
            int y = height;
            while (y != 0 && chunkLoader.chunk.blocks[ground[i].x, y - 1, ground[i].y] != Block.stone) y--;
            if (y < height)chunkLoader.chunk.blocks[ground[i].x, y, ground[i].y] = Block.grass;
            for (int j = 1; j < 4; j++)
            {
                if (y - j >= 0) chunkLoader.chunk.blocks[ground[i].x, y - j, ground[i].y] = Block.dirt;
            }

            int rand = Random.Range(0, 101);
            if (rand > treeProb) continue;
            if (ground[i].x >= 2 && ground[i].x < noiseWidth - 3 && ground[i].y >= 2 && ground[i].y < noiseLength - 3)
            {
                if (y + 1 < height && y >= 0 && chunkLoader.chunk.blocks[ground[i].x, y + 1, ground[i].y] == Block.air)
                    BuildTree(chunkLoader.chunk, ground[i].x, y + 1, ground[i].y);
            }
        }

        chunkLoader.UpdateChunk();
    }

    public float GetHeight(float perlinValue, int y)
    {
        return perlinValue * scaleHeight + minHeight - y;
    }

    private void BuildTree(Chunk chunk, int _x, int _y, int _z)
    {
        int width = Structure.instance.tree.GetLength(0);
        int height = Structure.instance.tree.GetLength(1);
        int length = Structure.instance.tree.GetLength(2);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                for (int z = 0; z < length; z++)
                {
                    chunk.SetBlock(Structure.instance.tree[x, y, z], -height / 2 + y + _x, x + _y, -length / 2 + z + _z);
                }
            }
        }
    }
}