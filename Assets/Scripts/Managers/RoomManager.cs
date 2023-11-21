using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

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

    int wallWhere;
    int wallWhere2;

    public List<Transform> WallSpots = new List<Transform>();
    public List<Transform> MageSpots = new List<Transform>();
    public List<Transform> MinionSpots = new List<Transform>();

    int lastMageSpot = 0;

    void Start()
    {
        StartCoroutine(FirstRoomChange());
        PlayerSpeed(GameManager.Instance.playerSpeedUp);
        PlayerDash(GameManager.Instance.playerDashOnMovement);
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

        SoundManager.Instance.Play("spawn");

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
                Instantiate(warning, (WallSpots[wallWhere].position) * 0.8f + pos + new Vector3((tilemap.cellSize.x)/2, (tilemap.cellSize.y) / 2), Quaternion.identity);
                
            }
        }

        Instantiate(warning, (MinionSpots[minionWhere].position), Quaternion.identity);

        BoundsInt bounds2 = tilemap2.cellBounds;
        foreach (Vector3Int pos in bounds2.allPositionsWithin)
        {
            Tile tile = tilemap2.GetTile<Tile>(pos);
            if (tile != null)
            {
                Instantiate(warning, (WallSpots[wallWhere2].position) * 0.8f + pos + new Vector3((tilemap.cellSize.x) / 2, (tilemap.cellSize.y) / 2), Quaternion.identity);
            }
        }

    }

    public void EndGame()
    {
        Destroy(mage);
        Destroy(minion);
        player.GetComponent<Player>().endFlag = true;
    }

    public void Pause(bool pause)
    {
        player.GetComponent<Player>().Pause(pause);
        if(minion != null)
            minion.GetComponent<Minion>().Pause(pause);
    }

    public void PlayerSpeed(float speedMod)
    {
        player.GetComponent<Player>().speedUpgrade = speedMod;
    }

    public void PlayerDash(bool OnMovement)
    {
        player.GetComponent<Player>().dashOnMovement = OnMovement;
    }
}
