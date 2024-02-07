using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class GameController : MonoBehaviour
{
    public Tilemap backgroundPrefab; // Reference to your background sprite prefab

    public Tile waterTile; // Reference to the background water tile 
    public Tile groundTile; // Reference to the background ground tile 
    public List <Tile> tiles; // Reference to the background corners and borders tile 
    public List <GameObject> enemies; // Reference to the background corners and borders tile 

    public GameObject ship;
    public GameObject enemy;

    //public Transform player; // Reference to your player's transform
    public float backgroundDistanceThreshold = 10f; // Distance at which backgrounds will be spawned
    
    public float thres; // Distance at which backgrounds will be spawned
    public float scale; 
    public int num_enemies; // number of enemies, change with difficulty

    private GameObject shipInstance;

    

    private void Start()
    {
        //GameObject.gamecomponent()
        shipInstance = GameObject.Instantiate(ship, new Vector3(0, 0, -1), Quaternion.identity);
        enemies = new List<GameObject>();
        //SpawnBackground();
    }
    private void Update()
    {
    
    }

    private void FixedUpdate()
    {
        SpawnBackground();
        SpawnEnemies();
        UpdateEnemies();
    }
    
    private void UpdateEnemies(){
        Vector2 pos_vec = shipInstance.GetComponent<MovementController>().map_position;


        foreach (var enemy in enemies) {
            Rigidbody2D rb;
            
            enemy.GetComponent<BadGuy>().player_pos = pos_vec;
        }
    }
    
    private void SpawnEnemies()
    {
    	while (enemies.Count < num_enemies)
    	{
            int randx = Random.Range(-20, 20);
            int randy = Random.Range(-20, 20);
            
            GameObject newObj = Instantiate(enemy, new Vector3(randx, randy, -3), Quaternion.identity);
            newObj.transform.SetParent(transform);
            newObj.SetActive(true);
            enemies.Add(newObj);
            
    	}
    
    }

    private void SpawnBackground()
    {
        float height;

        Vector2 pos_vec = shipInstance.GetComponent<MovementController>().map_position;
        Vector2 pos_vec_int = new Vector2(Mathf.Floor(pos_vec.x), Mathf.Floor(pos_vec.y));
        Vector2 pos_vec_dec = new Vector2(pos_vec.x - pos_vec_int.x, pos_vec.y - pos_vec_int.y);

        backgroundPrefab.gameObject.transform.position = -pos_vec_dec;

        for (int x = -70; x < 70; x++)
        {
            for (int y = -70; y < 70; y++)
            {
                // Calculate Perlin noise values for surrounding tiles
                float top = Mathf.PerlinNoise(1000 + (x + pos_vec_int.x) * scale, 1000 + ((y + pos_vec_int.y) + 1) * scale);
                float left = Mathf.PerlinNoise(1000 + ((x + pos_vec_int.x) - 1) * scale, 1000 + (y + pos_vec_int.y) * scale);
                float right = Mathf.PerlinNoise(1000 + ((x + pos_vec_int.x) + 1) * scale, 1000 + (y + pos_vec_int.y) * scale);
                float bottom = Mathf.PerlinNoise(1000 + (x + pos_vec_int.x) * scale, 1000 + ((y + pos_vec_int.y) - 1) * scale);

                Tile currentTile;

                height = Mathf.PerlinNoise(1000 + (x + pos_vec_int.x) * scale , 1000 + (y + pos_vec_int.y) * scale);

                if (height > thres)
                {
                    currentTile = GetCornerOrBorderTile(top, left, right, bottom);
                }
                else
                {
                    currentTile = waterTile;
                }

                backgroundPrefab.SetTile(new Vector3Int(x, y, 0), currentTile);
            }
        }
    }
    private Tile GetCornerOrBorderTile(float top, float left, float right, float bottom)
{
    // Define conditions for each corner and border based on surrounding Perlin noise values

    // Top-left corner
    if (top < thres && left < thres)
    {
        return tiles[0];
    }

    // Top-right corner
    if (top < thres && right < thres)
    {
        return tiles[2];
    }

    // Bottom-left corner
    if (left < thres && bottom < thres)
    {
        return tiles[5];
    }

    // Bottom-right corner
    if (bottom < thres && right < thres)
    {
        return tiles[7];
    }

    // Top border
    if (top < thres)
    {
        return tiles[1];
    }

    // Bottom border
    if (bottom < thres)
    {
        return tiles[6];
    }

    // Left border
    if (left < thres)
    {
        return tiles[3];
    }

    // Right border
    if (right < thres)
    {
        return tiles[4];
    }

    // Default to groundTile if none of the above conditions are met
    return groundTile;
    }
}
