using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileTest : MonoBehaviour
{
    public Tilemap AddWallTilemap;
    public List<Tilemap> Paterns;
    public Vector3Int[] tilePos;
    int rand;

    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            SpawnTiles();
        }

        if(Input.GetMouseButtonDown(1))
        {
            ClearTiles();
        }
    }

    void SpawnTiles()
    {
        rand = Random.Range(0,tilePos.Length);
        int rand2 = Random.Range(0, Paterns.Count);
        Tilemap current = Paterns[rand2];
        AddPaternToTilemap(current);
    }

    void ClearTiles()
    {
        AddWallTilemap.ClearAllTiles();
    }

    public void AddPaternToTilemap(Tilemap tilemap)
    {    
        BoundsInt bounds = tilemap.cellBounds;
        foreach (Vector3Int pos in bounds.allPositionsWithin)
        {
            Tile tile = tilemap.GetTile<Tile>(pos);
            if (tile != null)
            {
                //AddWallTilemap.SetTile(pos, tile);
                AddWallTilemap.SetTile(tilePos[rand]+pos, tile);
            }
        }
    }
}
