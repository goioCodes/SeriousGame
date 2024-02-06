using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameController : MonoBehaviour
{
    public Tilemap backgroundPrefab; // Reference to your background sprite prefab

    public Tile waterTile; // Reference to your background tile prefab
    public Tile groundTile;

    //public Transform player; // Reference to your player's transform
    public float backgroundDistanceThreshold = 10f; // Distance at which backgrounds will be spawned
    
    public float thres; // Distance at which backgrounds will be spawned
    public float scale;
    private float nextBackgroundSpawnTime = 0f; 

    private void Start()
    {
       
        SpawnBackground();
    }
    private void Update()
    {
        SpawnBackground();
    }

    private void SpawnBackground()
    {
        float height;
        for (int x = -50; x < 50; x++)
        {
            for (int y = -50; y < 50; y++)
            {
                height = Mathf.PerlinNoise(1000 + x * scale, 1000 + y * scale);

                Tile currentTile;
                if (height < thres)
                {
                    currentTile = waterTile;
                }
                else
                {
                    currentTile = groundTile;
                }

                backgroundPrefab.SetTile(new Vector3Int(x, y, 0), currentTile);
            }
        }
    }
}