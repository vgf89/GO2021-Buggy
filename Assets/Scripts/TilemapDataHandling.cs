using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapDataHandling : MonoBehaviour
{

    Tilemap tilemap;
    [SerializeField]
    List<TileBase> tileList;

    // Start is called before the first frame update
    void Start()
    {
        tilemap = GetComponent<Tilemap>();
        tileList = new List<TileBase>();
        
        
        for (int x = tilemap.origin.x; x < (tilemap.origin.x + tilemap.size.x); x++)
            for (int y = tilemap.origin.y; y < (tilemap.origin.y + tilemap.size.y); y++)
            {
                {
                    Vector3Int tileTempPos = new Vector3Int(tilemap.origin.x, tilemap.origin.y, 0);
                    TileBase tempTile = tilemap.GetTile(new Vector3Int(x,y,0));
                    if (tempTile != null)
                    {
                        Debug.Log("Adding Tile from position " + tileTempPos.ToString());
                        tileList.Add(tempTile);
                    }
                }
            }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
