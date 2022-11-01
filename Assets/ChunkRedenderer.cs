using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class ChunkRedenderer : MonoBehaviour
{
    public const int ChunkWidth = 25;
    public const int ChunkHeight = 128;
    public const float BlockScale = .6f;

    public ChunkData ChunkData;
    public GameWorld ParentWorld;

    private List<Vector3> verticiles = new List<Vector3>();
    private List<int> triagles = new List<int>();

    private void Start()
    {
        Mesh chunkMesh = new Mesh();

        for (int y = 0; y < ChunkHeight; y++)
        {
            for (int x = 0; x < ChunkWidth; x++)
            {
                for (int z = 0; z < ChunkWidth; z++)
                {
                    GenerateBlock(x,y,z);
                }
            }
        }
        
        chunkMesh.vertices = verticiles.ToArray();
        chunkMesh.triangles = triagles.ToArray();
        
        chunkMesh.Optimize();
        
        chunkMesh.RecalculateBounds();
        chunkMesh.RecalculateNormals();

        GetComponent<MeshFilter>().mesh = chunkMesh;
        GetComponent<MeshCollider>().sharedMesh = chunkMesh;
    }

    private void GenerateBlock(int x, int y, int z)
    {
        var blockPosition = new Vector3Int(x,y,z);
        if(GetBlockOnPosition(blockPosition) == 0) return;
        
        if(GetBlockOnPosition(blockPosition + Vector3Int.right)== 0) GenerateRightSite(blockPosition);
        if(GetBlockOnPosition(blockPosition + Vector3Int.left)== 0) GenerateLeftSite(blockPosition);
        if(GetBlockOnPosition(blockPosition + Vector3Int.forward)== 0) GenerateFrontSite(blockPosition);
        if(GetBlockOnPosition(blockPosition + Vector3Int.back)== 0) GenerateBackSite(blockPosition);
        if(GetBlockOnPosition(blockPosition + Vector3Int.up)== 0) GenerateTopSite(blockPosition);
        if(GetBlockOnPosition(blockPosition + Vector3Int.down)== 0) GenerateBottomSite(blockPosition);

    }

    private BlockType GetBlockOnPosition(Vector3Int blockPosition)
    {
        if (blockPosition.x >= 0 && blockPosition.x < ChunkWidth &&
            blockPosition.y >= 0 && blockPosition.y < ChunkHeight &&
            blockPosition.z >= 0 && blockPosition.z < ChunkWidth)
        {
            return ChunkData.Blocks[blockPosition.x, blockPosition.y, blockPosition.z];
        }
        else
        {
            if (blockPosition.y < 0 || blockPosition.y >= ChunkHeight) return BlockType.Air;
            
            Vector2Int adJacentChukPosition = ChunkData.ChukPosition;
            if (blockPosition.x < 0)
            {
                adJacentChukPosition.x--;
                blockPosition.x += ChunkWidth;
            } 
            else if (blockPosition.x >= ChunkWidth)
            {
                adJacentChukPosition.x++;
                blockPosition.x -= ChunkWidth;
            }
            
            if (blockPosition.z < 0)
            {
                adJacentChukPosition.y--;
                blockPosition.z += ChunkWidth;
            } 
            else if (blockPosition.z >= ChunkWidth)
            {
                adJacentChukPosition.y++;
                blockPosition.z -= ChunkWidth;
            }
            
            if(ParentWorld.ChunkDatas.TryGetValue(adJacentChukPosition, out ChunkData adJacentChuk))
            {
                return adJacentChuk.Blocks[blockPosition.x, blockPosition.y, blockPosition.z];
            }
            else
            {
                return BlockType.Air;
            }
        }
    }

    private void GenerateRightSite(Vector3Int blockPosition)
    {
        verticiles.Add((new Vector3(1, 0, 0) + blockPosition)*BlockScale);
        verticiles.Add((new Vector3(1, 1, 0) + blockPosition)*BlockScale);
        verticiles.Add((new Vector3(1, 0, 1) + blockPosition)*BlockScale);
        verticiles.Add((new Vector3(1, 1, 1) + blockPosition)*BlockScale);

        AddLastVerticles();
    }
    
    private void GenerateLeftSite(Vector3Int blockPosition)
    {
        verticiles.Add((new Vector3(0, 0, 0) + blockPosition)*BlockScale);
        verticiles.Add((new Vector3(0, 0, 1) + blockPosition)*BlockScale);
        verticiles.Add((new Vector3(0, 1, 0) + blockPosition)*BlockScale);
        verticiles.Add((new Vector3(0, 1, 1) + blockPosition)*BlockScale);
        

        AddLastVerticles();
    }
    
    private void GenerateFrontSite(Vector3Int blockPosition)
    {
        verticiles.Add((new Vector3(0, 0, 1) + blockPosition)*BlockScale);
        verticiles.Add((new Vector3(1, 0, 1) + blockPosition)*BlockScale);
        verticiles.Add((new Vector3(0, 1, 1) + blockPosition)*BlockScale);
        verticiles.Add((new Vector3(1, 1, 1) + blockPosition)*BlockScale);   

        AddLastVerticles();
    }
    
    private void GenerateBackSite(Vector3Int blockPosition)
    {
        verticiles.Add((new Vector3(0, 0, 0) + blockPosition)*BlockScale);
        verticiles.Add((new Vector3(0, 1, 0) + blockPosition)*BlockScale);
        verticiles.Add((new Vector3(1, 0, 0) + blockPosition)*BlockScale);
        verticiles.Add((new Vector3(1, 1, 0) + blockPosition)*BlockScale);
        

        AddLastVerticles();
    }
    
    private void GenerateTopSite(Vector3Int blockPosition)
    {
        verticiles.Add((new Vector3(0, 1, 0) + blockPosition)*BlockScale);
        verticiles.Add((new Vector3(0, 1, 1) + blockPosition)*BlockScale);
        verticiles.Add((new Vector3(1, 1, 0) + blockPosition)*BlockScale);
        verticiles.Add((new Vector3(1, 1, 1) + blockPosition)*BlockScale);
        

        AddLastVerticles();
    }
    
    private void GenerateBottomSite(Vector3Int blockPosition)
    {
        verticiles.Add((new Vector3(0, 0, 0) + blockPosition)*BlockScale);
        verticiles.Add((new Vector3(1, 0, 0) + blockPosition)*BlockScale);
        verticiles.Add((new Vector3(0, 0, 1) + blockPosition)*BlockScale);
        verticiles.Add((new Vector3(1, 0, 1) + blockPosition)*BlockScale);
        

        AddLastVerticles();
    }

    private void AddLastVerticles()
    {
        triagles.Add(verticiles.Count - 4);
        triagles.Add(verticiles.Count - 3);
        triagles.Add(verticiles.Count - 2);

        triagles.Add(verticiles.Count - 3);
        triagles.Add(verticiles.Count - 1);
        triagles.Add(verticiles.Count - 2);
    }
}

