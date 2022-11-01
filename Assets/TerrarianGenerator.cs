using UnityEngine;

public static class TerrarianGenerator
{
    public static BlockType[,,] GenerateTerrarian(float xOffset, float zOffset)
    {
        var result = new BlockType[ChunkRedenderer.ChunkWidth, ChunkRedenderer.ChunkHeight, ChunkRedenderer.ChunkWidth];
        
        for (int x = 0; x < ChunkRedenderer.ChunkWidth; x++)
        {
            for (int z = 0; z < ChunkRedenderer.ChunkWidth; z++)
            { 
                float height = Mathf.PerlinNoise((x/4f + xOffset) * .2f, (z/4f + zOffset) * .2f) * 25 + 10;

                for (int y = 0; y < height; y++)
                {
                    result[x, y, z] = BlockType.Grass;
                }
            }
        }
        return result;
    }
}
