using UnityEngine;

public class Chunk
{
    public Block[,,] blocks;

    public Block GetBlock(int x, int y, int z)
    {
        if (x < 0) return Block.air;
        if (x >= blocks.GetLength(0)) return Block.air;
        if (y < 0) return Block.air;
        if (y >= blocks.GetLength(1)) return Block.air;
        if (z < 0) return Block.air;
        if (z >= blocks.GetLength(2)) return Block.air;

        return blocks[x, y, z];
    }
}
