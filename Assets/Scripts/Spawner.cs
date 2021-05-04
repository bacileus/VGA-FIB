using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    bool[,] map;
    int height, width;
    public GameObject[] enemies;
    public float slimeChance, sealChance;
    public int maxEnemies;

    void Start()
    {
        map = GetComponent<CellCreateLevel>().map;
        height = GetComponent<CellCreateLevel>().height;
        width = GetComponent<CellCreateLevel>().width;
        Spawn();
    }

    void Spawn()
    {
        int i = 0;
        while (i < maxEnemies)
        {
            int h = Random.Range(0, height);
            int w = Random.Range(0, width);
            if(map[h, w])
            {
                i++;
                float chance = Random.value;
                if (chance < slimeChance)
                    Instantiate(enemies[0], new Vector2(w + 0.5f, h + 0.5f), Quaternion.identity);
                else if (chance >= slimeChance && chance < slimeChance + sealChance)
                    Instantiate(enemies[1], new Vector2(w + 0.5f, h + 0.5f), Quaternion.identity);
                else Instantiate(enemies[2], new Vector2(w + 0.5f, h + 0.5f), Quaternion.identity);
            }
        }
    }
}
