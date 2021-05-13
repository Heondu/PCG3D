using UnityEngine;

public class PerlinNoise : MonoBehaviour
{
    public float scale = 20.0f;
    public int octaves = 2;
    public float persistance = 0.5f;
    public float lacunarity = 2;

    public float xOrg = 0;
    public float zOrg = 0;

    public string seed;
    public bool useRandomSeed;
    public bool AutoUpdateMap;

    private void Awake()
    {
        if (useRandomSeed) seed = Time.time.ToString();
        System.Random pseudoRandom = new System.Random(seed.GetHashCode());
        xOrg = pseudoRandom.Next(0, 99999);
        zOrg = pseudoRandom.Next(0, 99999);
    }

    public float GenerateMap(int x, int y, int z)
    {
        scale = Mathf.Max(0.0001f, scale);

        float amplitude = 1;
        float frequency = 1;
        float noiseHeight = 0;

        for (int i = 0; i < octaves; i++)
        {
            float xCoord = xOrg + x / scale * frequency;
            float yCoord = y / scale * frequency;
            float zCoord = zOrg + z / scale * frequency;

            float xy = Mathf.PerlinNoise(xCoord, yCoord) * 2 - 1;
            float yz = Mathf.PerlinNoise(yCoord, zCoord) * 2 - 1;
            float xz = Mathf.PerlinNoise(xCoord, zCoord) * 2 - 1;
            float yx = Mathf.PerlinNoise(yCoord, xCoord) * 2 - 1;
            float zy = Mathf.PerlinNoise(zCoord, yCoord) * 2 - 1;
            float zx = Mathf.PerlinNoise(zCoord, xCoord) * 2 - 1;
            float perlinValue = (xy + yz + xz + yx + zy + zx) / 6f;

            noiseHeight += perlinValue * amplitude;
            amplitude *= persistance;
            frequency *= lacunarity;
        }
        return noiseHeight;
    }

    //public float[, ,] GenerateMap(int width, int height, int length, float scale, float octaves, float persistance, float lacunarity, 
    //                            float xOrg, float zOrg, float xPos, float zPos)
    //{
    //    float[, ,] noiseMap = new float[width, height, length];
    //    scale = Mathf.Max(0.0001f, scale);
    //    //float maxNoiseHeight = float.MinValue;
    //    //float minNoiseHeight = float.MaxValue;
    //    for (int x = 0; x < width; x++)
    //    {
    //        for (int y = 0; y < height; y++)
    //        {
    //            for (int z = 0; z < length; z++)
    //            {
    //                float amplitude = 1;
    //                float frequency = 1;
    //                float noiseHeight = 0;
    //
    //                for (int i = 0; i < octaves; i++)
    //                {
    //                    float xCoord = xOrg + (x + xPos) / scale * frequency;
    //                    float yCoord = y / scale * frequency;
    //                    float zCoord = zOrg + (z + zPos) / scale * frequency;
    //
    //                    float xy = Mathf.PerlinNoise(xCoord, yCoord) * 2 - 1;
    //                    float yz = Mathf.PerlinNoise(yCoord, zCoord) * 2 - 1;
    //                    float xz = Mathf.PerlinNoise(xCoord, zCoord) * 2 - 1;
    //                    float yx = Mathf.PerlinNoise(yCoord, xCoord) * 2 - 1;
    //                    float zy = Mathf.PerlinNoise(zCoord, yCoord) * 2 - 1;
    //                    float zx = Mathf.PerlinNoise(zCoord, xCoord) * 2 - 1;
    //                    float perlinValue = (xy + yz + xz + yx + zy + zx) / 6f;
    //
    //                    noiseHeight += perlinValue * amplitude;
    //                    amplitude *= persistance;
    //                    frequency *= lacunarity;
    //                }
    //                //if (noiseHeight > maxNoiseHeight) maxNoiseHeight = noiseHeight;
    //                //else if (noiseHeight < minNoiseHeight) minNoiseHeight = noiseHeight;
    //                noiseMap[x, y, z] = noiseHeight;
    //            }
    //        }
    //    }
    //
    //    //for (int x = 0; x < width; x++)
    //    //{
    //    //    for (int y = 0; y < height; y++)
    //    //    {
    //    //        for (int z = 0; z < length; z++)
    //    //        {
    //    //            noiseMap[x, y, z] = Mathf.InverseLerp(-1, 1, noiseMap[x, y, z]) * 2 - 1;
    //    //        }
    //    //    }
    //    //}
    //    return noiseMap;
    //}
}
