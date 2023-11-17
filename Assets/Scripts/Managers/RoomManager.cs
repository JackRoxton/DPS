using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using static UnityEditor.PlayerSettings;

public class RoomManager : Singleton<RoomManager>
{
    public GameObject magePrefab;
    GameObject mage;
    public GameObject minionPrefab;
    GameObject minion;

    public GameObject warning;

    public GameObject player;

    public Tilemap AddWallTilemap;
    public List<Tilemap> WallPaterns;
    //public Vector3Int[] tilePos;
    int wallWhere;
    int wallWhere2;

    //public List<GameObject> WallPaterns = new List<GameObject>();
    public List<Transform> WallSpots = new List<Transform>();
    public List<Transform> MageSpots = new List<Transform>();
    public List<Transform> MinionSpots = new List<Transform>();

    int lastMageSpot = 0; //so the mage can't tp where he already was

    void Start()
    {
        StartCoroutine(FirstRoomChange());
        playerSpeed(GameManager.Instance.playerSpeedUp);
    }

    void Update()
    {
        
    }

    public void ChangeRoom()
    {
        Debug.Log("change");
        StartCoroutine(RoomChange());
    }

    public IEnumerator FirstRoomChange()
    {
        int wallWhat = Random.Range(0, WallPaterns.Count);
        int wallWhat2 = Random.Range(0, WallPaterns.Count);
        wallWhere = Random.Range(0, WallSpots.Count);
        do
        {
            wallWhere2 = Random.Range(0, WallSpots.Count);
        } while(wallWhere2 == wallWhere);
        int mageWhere = Random.Range(0, MageSpots.Count);
        int minionWhere = Random.Range(0, MinionSpots.Count);
        lastMageSpot = mageWhere;
        mage = Instantiate(magePrefab, MageSpots[mageWhere].position, Quaternion.identity);
        mage.GetComponent<Mage>().Player = player;

        AddWarnings(WallPaterns[wallWhat], WallPaterns[wallWhat2], minionWhere);

        yield return new WaitForSeconds(2);

        minion = Instantiate(minionPrefab, MinionSpots[minionWhere]);
        minion.GetComponent<Minion>().player = this.player;

        AddPaternToTilemap(WallPaterns[wallWhat], WallPaterns[wallWhat2]);
    }

    public IEnumerator RoomChange()
    {
        ClearTiles();

        int wallWhat = Random.Range(0, WallPaterns.Count);
        int wallWhat2 = Random.Range(0, WallPaterns.Count);
        wallWhere = Random.Range(0, WallSpots.Count);
        do
        {
            wallWhere2 = Random.Range(0, WallSpots.Count);
        } while (wallWhere2 == wallWhere);
        int minionWhere = Random.Range(0, MinionSpots.Count);
        int mageWhere;
        do
        {
            mageWhere = Random.Range(0, MageSpots.Count);
        } while (mageWhere == lastMageSpot);
        lastMageSpot = mageWhere;
        mage.GetComponent<Mage>().Teleport(MageSpots[mageWhere]);

        AddWarnings(WallPaterns[wallWhat], WallPaterns[wallWhat2], minionWhere);

        yield return new WaitForSeconds(2);

        if (minion == null)
        {
            minion = Instantiate(minionPrefab, MinionSpots[minionWhere].position, Quaternion.identity);
            minion.GetComponent<Minion>().player = this.player;
        }

        AddPaternToTilemap(WallPaterns[wallWhat], WallPaterns[wallWhat2]);
    }

    void ClearTiles()
    {
        AddWallTilemap.ClearAllTiles();
    }

    public void AddPaternToTilemap(Tilemap tilemap, Tilemap tilemap2)
    {
        BoundsInt bounds = tilemap.cellBounds;
        foreach (Vector3Int pos in bounds.allPositionsWithin)
        {
            Tile tile = tilemap.GetTile<Tile>(pos);
            if (tile != null)
            {
                //AddWallTilemap.SetTile(pos, tile);
                AddWallTilemap.SetTile(Vector3Int.FloorToInt(WallSpots[wallWhere].position) + pos, tile);
            }
        }

        BoundsInt bounds2 = tilemap2.cellBounds;
        foreach (Vector3Int pos in bounds2.allPositionsWithin)
        {
            Tile tile = tilemap2.GetTile<Tile>(pos);
            if (tile != null)
            {
                //AddWallTilemap.SetTile(pos, tile);
                AddWallTilemap.SetTile(Vector3Int.FloorToInt(WallSpots[wallWhere2].position) + pos, tile);
            }
        }
    }

    public void AddWarnings(Tilemap tilemap, Tilemap tilemap2, int minionWhere)
    {
        BoundsInt bounds = tilemap.cellBounds;
        foreach (Vector3Int pos in bounds.allPositionsWithin)
        {
            Tile tile = tilemap.GetTile<Tile>(pos);
            if (tile != null)
            {
                Instantiate(warning, (WallSpots[wallWhere].position)*0.8f + pos, Quaternion.identity);
            }
        }

        Instantiate(warning, (MinionSpots[minionWhere].position)*0.8f, Quaternion.identity);

        BoundsInt bounds2 = tilemap2.cellBounds;
        foreach (Vector3Int pos in bounds2.allPositionsWithin)
        {
            Tile tile = tilemap2.GetTile<Tile>(pos);
            if (tile != null)
            {
                Instantiate(warning, (WallSpots[wallWhere2].position)*0.8f + pos, Quaternion.identity);
            }
        }

    }

    public void EndGame()
    {
        Destroy(mage);
        Destroy(minion);
        player.GetComponent<Player>().endFlag = true;
    }

    public void playerSpeed(float speedMod)
    {
        player.GetComponent<Player>().speedUpgrade = speedMod;
    }
}
