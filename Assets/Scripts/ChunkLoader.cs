using UnityEngine;

public class ChunkLoader : MonoBehaviour
{
    public Chunk chunk;

    private float chunkLoadDistance;

    private GameObject player;
    private MeshCreator meshCreator;
    private TerrainGeneratorByPerlinNoise terrainGenerator;

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

    public void CreateMesh()
    {
        meshCreator.CreateMeshOnChild(chunk);
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
    }
}
