using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameController : MonoBehaviour
{
    public static GameController gameController;

    public Tilemap backgroundPrefab; // Reference to your background sprite prefab

    public Tile waterTile; // Reference to the background water tile 
    public Tile groundTile; // Reference to the background ground tile 
    public List <Tile> tiles; // Reference to the background corners and borders tile 

    public GameObject ship;

    //public Transform player; // Reference to your player's transform
    public float backgroundDistanceThreshold = 10f; // Distance at which backgrounds will be spawned
    
    public float thres; // Distance at which backgrounds will be spawned
    public float scale;

    public GameObject arrowSprite;
    public GameObject arrowShipView;
    public float windChangeInterval;
    float lastWindChangeTime;
    public Vector2 windDirection { get; private set; }
    public Vector2 windDirectionShipView {  get; private set; }

    GameObject shipInstance;

    private void Start()
    {
        gameController = this;
        //GameObject.gamecomponent()
        shipInstance = GameObject.Instantiate(ship, new Vector3(0, 0, 0), Quaternion.identity);
        //SpawnBackground();
        windDirection = Random.insideUnitCircle.normalized;
        lastWindChangeTime = Time.time;
        arrowSprite.transform.rotation = Quaternion.Euler(0, 0, Mathf.Rad2Deg * Mathf.Atan2(windDirection.y, windDirection.x));
        arrowShipView.transform.rotation = Quaternion.Euler(0, 0, Mathf.Rad2Deg * (Mathf.Atan2(windDirection.y, windDirection.x) - Mathf.Atan2(shipInstance.transform.right.y, shipInstance.transform.right.x)) + 90);
    }
    private void Update()
    {
        if(Time.time - lastWindChangeTime > windChangeInterval)
        {
            windDirection = Random.insideUnitCircle;
            arrowSprite.transform.rotation = Quaternion.Euler(0, 0, Mathf.Rad2Deg * Mathf.Atan2(windDirection.y, windDirection.x));
            lastWindChangeTime = Time.time;
        }
        float shipViewWindAngle = Mathf.Rad2Deg * (Mathf.Atan2(windDirection.y, windDirection.x) - Mathf.Atan2(shipInstance.transform.right.y, shipInstance.transform.right.x)) + 90;
        arrowShipView.transform.rotation = Quaternion.Euler(0, 0, shipViewWindAngle);
        windDirectionShipView = new Vector2(Mathf.Cos(shipViewWindAngle * Mathf.Deg2Rad), Mathf.Sin(shipViewWindAngle * Mathf.Deg2Rad));
    }

    private void FixedUpdate()
    {
        SpawnBackground();
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