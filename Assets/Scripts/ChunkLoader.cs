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
    private Dictionary<string, BlockData> blockDatas = new Dictionary<string, BlockData>();

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

        int index = 0;
        for (int x = 1; x < width - 1; x++)
        {
            for (int z = 1; z < length - 1; z++)
            {
                for (int y = 0; y < height; y++)
                {
                    //Transform parent = transform.Find(chunk.blocks[x, y, z].ToString());
                    //if (parent == null)
                    //{
                    //    parent = Instantiate(blockHolder, transform.position, Quaternion.identity, transform).transform;
                    //    string name = chunk.blocks[x, y, z].ToString();
                    //    parent.name = name;
                    //    blockDatas.Add(name, new BlockData());
                    //    blockDatas[name].block = chunk.blocks[x, y, z];
                    //    blockDatas[name].transform = parent;
                    //
                    //    Transform meshTop = parent.Find("MeshTop");
                    //    Transform meshSide = parent.Find("MeshSide");
                    //    Transform meshBottom = parent.Find("MeshBottom");
                    //
                    //    blockDatas[name].meshFilters[0] = meshTop.GetComponent<MeshFilter>();
                    //    blockDatas[name].meshFilters[1] = meshSide.GetComponent<MeshFilter>();
                    //    blockDatas[name].meshFilters[2] = meshBottom.GetComponent<MeshFilter>();
                    //
                    //    blockDatas[name].meshRenderers[0] = meshTop.GetComponent<MeshRenderer>();
                    //    blockDatas[name].meshRenderers[1] = meshSide.GetComponent<MeshRenderer>();
                    //    blockDatas[name].meshRenderers[2] = meshBottom.GetComponent<MeshRenderer>();
                    //
                    //    blockDatas[name].meshColliders[0] = meshTop.GetComponent<MeshCollider>();
                    //    blockDatas[name].meshColliders[1] = meshSide.GetComponent<MeshCollider>();
                    //    blockDatas[name].meshColliders[2] = meshBottom.GetComponent<MeshCollider>();
                    //}
                    //blocks[index].transform.SetParent(parent);

                    if (chunk.blocks[x, y, z] != Block.air)
                    {
                        Vector3Int newPos = new Vector3Int(x, y, z);
                        blocks[index].transform.position = newPos - Vector3.one * ((float)(width - 2) / 2);
                        meshCreator.BuildMesh(chunk, newPos, blocks[index]);
                    }
                    index++;
                }
            }
        }
        meshCreator.Combine();

        //foreach (string key in blockDatas.Keys)
        //{
        //    if (blockDatas[key].block != Block.air)
        //    {
        //        meshCreator.Combine(blockDatas[key]);
        //    }
        //}
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
        for (int x = 1; x < width - 1; x++)
        {
            for (int z = 1; z < length - 1; z++)
            {
                for (int y = 0; y < height; y++)
                {
                    GameObject clone = Instantiate(cube, new Vector3Int(x, y, z) - Vector3.one * ((float)(width - 2) / 2), Quaternion.identity, transform);
                    blockList.Add(clone);
                    clone.SetActive(false);
                }
            }
        }
        blocks = blockList.ToArray();
    }
}
