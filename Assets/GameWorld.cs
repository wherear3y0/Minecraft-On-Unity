using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameWorld : MonoBehaviour
{
    public Dictionary<Vector2Int, ChunkData> ChunkDatas = new Dictionary<Vector2Int, ChunkData>();
    public ChunkRedenderer ChubkPrefab;
    
    void Start()
    {
        for (int x = 0; x < 10; x++)
        {
            for (int y = 0; y < 10; y++)
            {
                float xPos = x * ChunkRedenderer.ChunkWidth * ChunkRedenderer.BlockScale;
                float zPos = y * ChunkRedenderer.ChunkWidth * ChunkRedenderer.BlockScale;
                
                ChunkData chunkData = new ChunkData();
                chunkData.ChukPosition = new Vector2Int(x, y);
                chunkData.Blocks = TerrarianGenerator.GenerateTerrarian(xPos, zPos);
                ChunkDatas.Add(new Vector2Int(x,y), chunkData);

                var chunk = Instantiate(ChubkPrefab, new Vector3(xPos, 0, zPos), Quaternion.identity, transform);
                chunk.ChunkData = chunkData;

                chunk.ParentWorld = this;
            }
        }
    }
}
