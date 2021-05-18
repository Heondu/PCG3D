using UnityEngine;

public class Chunk
{
    public Block[,,] blocks;
    public int width;
    public int height;
    public int length;

    public Block GetBlock(int x, int y, int z)
    {
        if (x < 0) return Block.air;
        if (x >= width) return Block.air;
        if (y < 0) return Block.air;
        if (y >= height) return Block.air;
        if (z < 0) return Block.air;
        if (z >= length) return Block.air;

        return blocks[x, y, z];
    }

    public void SetBlock(Block block, int x, int y, int z)
    {
        if (x < 0) return;
        if (x >= width) return;
        if (y < 0) return;
        if (y >= height) return;
        if (z < 0) return;
        if (z >= length) return;
        if (blocks[x, y, z] != Block.air) return;

        blocks[x, y, z] = block;
    }

    public void SetBlock(int id, int x, int y, int z)
    {
        SetBlock((Block)id, x, y, z);
    }

    public Block[] FindSurroundingBlock(int x, int y, int z)
    {
        Block[] blocks = new Block[6];
        blocks[0] = GetBlock(x, y, z + 1);
        blocks[1] = GetBlock(x, y, z - 1);
        blocks[2] = GetBlock(x + 1, y, z);
        blocks[3] = GetBlock(x - 1, y, z);
        blocks[4] = GetBlock(x, y + 1, z);
        blocks[5] = GetBlock(x, y - 1, z);
        return blocks;
    }
}