using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileTest : MonoBehaviour
{
    public Tilemap WallTilemap;
    public Tile[] tiles;
    public Vector2[] tilePos;

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
        WallTilemap.SetTile(WallTilemap.WorldToCell(tilePos[0]), tiles[0]);
    }

    void ClearTiles()
    {
        WallTilemap.DeleteCells(WallTilemap.WorldToCell(tilePos[0]),1,1,0);
    }
}
