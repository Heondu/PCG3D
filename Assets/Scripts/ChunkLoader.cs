using System.Collections.Generic;
using UnityEngine;

public class ChunkLoader : MonoBehaviour
{
    [SerializeField]
    private GameObject cube;
    [SerializeField]
    private GameObject blockHolder;

    public Chunk chunk;

    private float chunkLoadDistance;

    private GameObject player;
    private MeshCreator meshCreator;
    private TerrainGeneratorByPerlinNoise terrainGenerator;

    private GameObject[] blocks;
    private BlockData[] blockDatas;

    private void Awake()
    {
        player = FindObjectOfType<PlayerController>().gameObject;
        meshCreator = GetComponent<MeshCreator>();
        terrainGenerator = FindObjectOfType<TerrainGeneratorByPerlinNoise>();
    }

    private void Start()
    {
        chunkLoadDistance = terrainGenerator.chunkLoadDistance;
    }

    private void Update()
    {
        LoadChunk();
    }

    public void UpdateChunk()
    {
        int width = chunk.blocks.GetLength(0);
        int height = chunk.blocks.GetLength(1);
        int length = chunk.blocks.GetLength(2);

        List<MeshFilter> meshFilters = new List<MeshFilter>();
        List<MeshRenderer> meshRenderers = new List<MeshRenderer>();

        int index = 0;
        for (int x = 1; x < width - 1; x++)
        {
            for (int z = 1; z < length - 1; z++)
            {
                for (int y = 0; y < height; y++)
                {
                    Vector3Int newPos = new Vector3Int(x, y, z);
                    blockDatas[index].transform.position = newPos - new Vector3((width - 2) / 2, 0, (length - 2) / 2);

                    if (chunk.blocks[x, y, z] != Block.air)
                    {
                        if (meshCreator.BuildMesh(chunk, newPos, blockDatas[index], out List<MeshFilter> meshFilterList, out List<MeshRenderer> meshRendererList))
                        {
                            meshFilters.AddRange(meshFilterList);
                            meshRenderers.AddRange(meshRendererList);
                        }
                    }
                    index++;
                }
            }
        }

        meshCreator.Combine(meshFilters.ToArray(), meshRenderers.ToArray());
    }

    private void LoadChunk()
    {
        float xDistance = Mathf.Abs(player.transform.position.x - transform.position.x);
        float zDistance = Mathf.Abs(player.transform.position.z - transform.position.z);

        if (xDistance >= chunkLoadDistance)
        {
            Vector3 dir = Vector3.zero;
            if (player.transform.position.x > transform.position.x) dir = Vector3.right;
            else if (player.transform.position.x < transform.position.x) dir = Vector3.left;

            transform.position += dir * chunkLoadDistance * 2;
            terrainGenerator.CreateWorld(this, transform.position);
        }
        if (zDistance >= chunkLoadDistance)
        {
            Vector3 dir = Vector3.zero;
            if (player.transform.position.z > transform.position.z) dir = Vector3.forward;
            else if (player.transform.position.z < transform.position.z) dir = Vector3.back;

            transform.position += dir * chunkLoadDistance * 2;
            terrainGenerator.CreateWorld(this, transform.position);
        }
    }

    public void SetChunk(Chunk chunk)
    {
        this.chunk = chunk;

        int width = chunk.blocks.GetLength(0);
        int height = chunk.blocks.GetLength(1);
        int length = chunk.blocks.GetLength(2);

        List<GameObject> blockList = new List<GameObject>();
        List<BlockData> blockDataList = new List<BlockData>();
        for (int x = 1; x < width - 1; x++)
        {
            for (int z = 1; z < length - 1; z++)
            {
                for (int y = 0; y < height; y++)
                {
                    GameObject clone = Instantiate(cube, new Vector3Int(x, y, z) - Vector3.one * ((float)(width - 2) / 2), Quaternion.identity, transform);
                    blockList.Add(clone);
                    blockDataList.Add(clone.GetComponent<BlockData>());
                    clone.SetActive(false);
                }
            }
        }
        blocks = blockList.ToArray();
        blockDatas = blockDataList.ToArray();
    }
}
