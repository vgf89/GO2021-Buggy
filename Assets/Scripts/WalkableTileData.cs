using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu]
public class WalkableTileData : ScriptableObject
{

    //Works with custom scriptable tiles
    [Tooltip("Insert walkable tiles here")]
    public TileBase[] tiles;

    public bool explored;

}
