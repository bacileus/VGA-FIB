using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class WalkerCreateLevel : MonoBehaviour
{
    enum TileType {wall, floor};
    TileType[,] map;

    struct Walker
    {
        public Vector2 pos;
        public Vector2 dir;
    }
    List<Walker> walkers;

    public GameObject wallTile;
    public GameObject floorTile;

    public int height;
    public int width;

    public int maxWalkers;
    public float dirChance; // chance of occurring a direction change
    public float destroyChance;
    public float createChance;

    public int iterations;
    public float fillPercentage;

    void Awake()
    {
        Initialize();
        CreateFloors();
        SpawnScene();
    }

    void Initialize()
    {
        // initialize cell values
        map = new TileType[width, height];
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                map[i, j] = TileType.wall;
            }
        }

        // create first walker
        walkers = new List<Walker>();
        Walker w = new Walker();
        w.pos = RandomPos();
        w.dir = RandomDir();
        walkers.Add(w);
    }

    void CreateFloors()
    {
        for (int it = 0; it < iterations; it++)
        {
            // put floor on each walker
            for (int i = 0; i < walkers.Count; i++)
            {
                Vector2 pos = walkers[i].pos;
                map[(int)pos.x, (int)pos.y] = TileType.floor;
            }

            // destroy walker by chance
            for (int i = 0; i < walkers.Count; i++)
            {
                if(Random.value < destroyChance && walkers.Count > 1)
                {
                    walkers.RemoveAt(i);
                    break;
                }
            }

            // choose new direction
            for (int i = 0; i < walkers.Count; i++)
            {
                if(Random.value < dirChance)
                {
                    Walker temp = walkers[i];
                    temp.dir = RandomDir();
                    walkers[i] = temp;
                }
            }

            // spawn walker by chance
            int count = walkers.Count;
            for (int i = 0; i < count; i++)
            {
                if (Random.value < createChance && walkers.Count <= maxWalkers)
                {
                    Walker w = new Walker();
                    w.pos = walkers[i].pos;
                    w.dir = RandomDir();
                    walkers.Add(w);
                }
            }

            // move walkers
            for (int i = 0; i < walkers.Count; i++)
            {
                Walker w = walkers[i];
                w.pos.x = Mathf.Clamp(w.pos.x + w.dir.x, 1, width - 2);
                w.pos.y = Mathf.Clamp(w.pos.y + w.dir.y, 1, height - 2);
                walkers[i] = w;
            }

            if ((float)HowManyFloors() / (float)map.Length > fillPercentage) break;
        }
    }

    void SpawnScene()
    {
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                switch (map[i, j])
                {
                    case TileType.floor:
                        Instantiate(floorTile, new Vector2(i, j), Quaternion.identity);
                        break;
                    case TileType.wall:
                        Instantiate(wallTile, new Vector2(i, j), Quaternion.identity);
                        break;
                }
            }
        }
    }

    Vector2 RandomPos()
    {
        return new Vector2(Random.Range(0, width), Random.Range(0, height));
        // return new Vector2(width / 2, height / 2);
    }

    Vector2 RandomDir()
    {
        int v = Random.Range(0, 4);
        Vector2 r = new Vector2(0, 0);
        switch (v)
        {
            case 0:
                r = Vector2.up;
                break;
            case 1:
                r = Vector2.left;
                break;
            case 2:
                r = Vector2.down;
                break;
            case 3:
                r = Vector2.right;
                break;
        }
        return r;
    }

    int HowManyFloors()
    {
        int count = 0;
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                if (map[i, j] == TileType.floor) count++;
            }
        }
        return count;
    }
}
