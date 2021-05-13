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

    public bool[] IsAirBlock(int x, int y, int z)
    {
        bool[] isAirBlock = { false, false, false, false, false, false };

        //if (x < 0) return Block.air;
        //if (x >= width) return Block.air;
        //if (y < 0) return Block.air;
        //if (y >= height) return Block.air;
        //if (z < 0) return Block.air;
        //if (z >= length) return Block.air;

        if (z + 1 < length  && blocks[x, y, z + 1] == Block.air) isAirBlock[0] = true; //back
        if (z - 1 >= 0      && blocks[x, y, z - 1] == Block.air) isAirBlock[1] = true; //front
        if (x + 1 < width   && blocks[x + 1, y, z] == Block.air) isAirBlock[2] = true;//right
        if (x - 1 >= 0      && blocks[x - 1, y, z] == Block.air) isAirBlock[3] = true;//left
        if (y + 1 < height  && blocks[x, y + 1, z] == Block.air) isAirBlock[4] = true;//top
        if (y - 1 >= 0      && blocks[x, y - 1, z] == Block.air) isAirBlock[5] = true;//bottom

        return isAirBlock;
    }
}
