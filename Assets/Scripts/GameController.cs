using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameController : MonoBehaviour
{
    public Tilemap worldMap;
    public Tile waterTile;
    public List<Tile> earthTiles;
    // Start is called before the first frame update
    void Start()
    {
        worldMap.SetTile(new Vector3Int(0, 0, 0), waterTile);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
