using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CellCreateLevel : MonoBehaviour
{
    public float aliveChance;
    public int aliveLimit;
    public int deadLimit;
    public int iterations;

    public TileBase ground;
    public TileBase water;

    public Tilemap waterTilemap;
    public Tilemap groundTilemap;

    public int height;
    public int width;
    public int margin;
    [HideInInspector]
    public bool[,] map;

    public float elemChance;
    public TileBase[] elem;

    void Awake()
    {
        Initialize();
        Compute();
        Render();
    }

    // assign value to the cells
    void Initialize()
    {
        map = new bool[height, width];

        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                if (Random.value <= aliveChance && i > 0 && i < height - 1 && j > 0 && j < width - 1) map[i, j] = true;
                else map[i, j] = false;
            }
        }
    }

    void Compute()
    {
        for (int it = 0; it < iterations; it++) UpdateMap(map);
    }

    void UpdateMap(bool[,] oldMap)
    {
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                if (oldMap[i, j])
                {
                    // the cell is alive
                    if (AliveCells(oldMap, i, j) < deadLimit) map[i, j] = false;
                    else map[i, j] = true;
                }
                else
                {
                    // the cell is dead
                    if (AliveCells(oldMap, i, j) > aliveLimit) map[i, j] = true;
                    else map[i, j] = false;
                }
            }
        }
    }

    void Render()
    {
        for (int i = height - 1; i >= 0; i--)
        {
            for (int j = 0; j < width; j++)
            {
                if (map[i, j])
                {
                    if (Random.value < elemChance)
                    {
                        float c = Random.Range(0, 20);
                        if(c < 6) groundTilemap.SetTile(new Vector3Int(i, j, 0), elem[0]);
                        else if(c >= 6 && c < 12) groundTilemap.SetTile(new Vector3Int(i, j, 0), elem[1]);
                        else if(c >= 12 && c < 19) groundTilemap.SetTile(new Vector3Int(i, j, 0), elem[2]);
                        else /*if(c >= 0.9 && c < 1)*/ groundTilemap.SetTile(new Vector3Int(i, j, 0), elem[3]);
                    }
                    else groundTilemap.SetTile(new Vector3Int(i, j, 0), ground);
                }
                else waterTilemap.SetTile(new Vector3Int(i, j, 0), water);
            }
        }
        CreateMargin();
    }

    int AliveCells(bool[,] oldMap, int x, int y)
    {
        int count = 0;
        for (int i = y - 1; i <= y + 1; i++)
        {
            for (int j = x - 1; j <= x + 1; j++)
            {
                if (i >= 0 && i < height && j >= 0 && j < width)
                    if (oldMap[i, j] && (i != x || j != y)) count++;
            }
        }
        return count;
    }

    void CreateMargin()
    {
        for (int i = - margin; i < height + margin; i++)
        {
            for (int j = - margin; j < width + margin; j++)
            {
                if (!(i >= 0 && i < height && j >= 0 && j < width))
                {
                    waterTilemap.SetTile(new Vector3Int(j, i, 0), water);
                }
            }
        }
    }
}
