using UnityEngine;

public class BlockDataManager : MonoBehaviour
{
    public static BlockDataManager instance;

    [SerializeField]
    private BlockData[] blockDatas;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    public BlockData GetBlockData(Block block)
    {
        return blockDatas[(int)block];
    }
}
