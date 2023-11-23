using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    int wallWhere3;

    public GameObject walls;
    public GameObject mages;
    public GameObject minions;
    List<Transform> WallSpots = new List<Transform>();
    List<Transform> MageSpots = new List<Transform>();
    List<Transform> MinionSpots = new List<Transform>();

    int lastMageSpot = 0;

    void Start()
    {
        Transform[] tr = walls.GetComponentsInChildren<Transform>();
        foreach (Transform t in tr)
        {
            WallSpots.Add(t);
        }
        WallSpots.Remove(walls.GetComponent<Transform>());
        tr = mages.GetComponentsInChildren<Transform>();
        foreach(Transform t in tr)
        {
            MageSpots.Add(t);
        }
        MageSpots.Remove(mages.GetComponent<Transform>());
        tr = minions.GetComponentsInChildren<Transform>();
        foreach (Transform t in tr)
        {
            MinionSpots.Add(t);
        }
        MinionSpots.Remove(minions.GetComponent<Transform>());


        StartCoroutine(FirstRoomChange());
        PlayerSpeed(GameManager.Instance.playerSpeedUp);
        PlayerDash(GameManager.Instance.playerDashOnMovement);
    }

    void Update()
    {
        
    }

    public void ChangeRoom()
    {
        StartCoroutine(RoomChange());
    }

    public IEnumerator FirstRoomChange()
    {
        int wallWhat = Random.Range(0, WallPaterns.Count);
        int wallWhat2 = Random.Range(0, WallPaterns.Count);
        int wallWhat3 = Random.Range(0, WallPaterns.Count);
        wallWhere = Random.Range(0, WallSpots.Count);
        do
        {
            wallWhere2 = Random.Range(0, WallSpots.Count);
        } while(wallWhere2 == wallWhere);
        do
        {
            wallWhere3 = Random.Range(0, WallSpots.Count);
        } while (wallWhere3 == wallWhere || wallWhere3 == wallWhere2);
        int mageWhere = Random.Range(0, MageSpots.Count);
        int minionWhere = Random.Range(0, MinionSpots.Count);
        lastMageSpot = mageWhere;
        mage = Instantiate(magePrefab, MageSpots[mageWhere].position, Quaternion.identity);
        mage.GetComponent<Mage>().Player = player;

        AddWarnings(WallPaterns[wallWhat], WallPaterns[wallWhat2], WallPaterns[wallWhat3], minionWhere);

        yield return new WaitForSeconds(2);

        minion = Instantiate(minionPrefab, MinionSpots[minionWhere].position,Quaternion.identity);
        minion.GetComponent<Minion>().player = this.player;

        AddPaternToTilemap(WallPaterns[wallWhat], WallPaterns[wallWhat2], WallPaterns[wallWhat3]);
    }

    public IEnumerator RoomChange()
    {
        ClearTiles();

        int wallWhat = Random.Range(0, WallPaterns.Count);
        int wallWhat2 = Random.Range(0, WallPaterns.Count);
        int wallWhat3 = Random.Range(0, WallPaterns.Count);
        wallWhere = Random.Range(0, WallSpots.Count);
        do
        {
            wallWhere2 = Random.Range(0, WallSpots.Count);
        } while (wallWhere2 == wallWhere);
        do
        {
            wallWhere3 = Random.Range(0, WallSpots.Count);
        } while (wallWhere3 == wallWhere || wallWhere3 == wallWhere2);
        int minionWhere = Random.Range(0, MinionSpots.Count);
        int mageWhere;
        do
        {
            mageWhere = Random.Range(0, MageSpots.Count);
        } while (mageWhere == lastMageSpot);
        lastMageSpot = mageWhere;
        mage.GetComponent<Mage>().Teleport(MageSpots[mageWhere]);

        AddWarnings(WallPaterns[wallWhat], WallPaterns[wallWhat2],WallPaterns[wallWhat3], minionWhere);

        yield return new WaitForSeconds(2);

        SoundManager.Instance.Play("spawn");

        if (minion == null)
        {
            minion = Instantiate(minionPrefab, MinionSpots[minionWhere].position, Quaternion.identity);
            minion.GetComponent<Minion>().player = this.player;
        }

        AddPaternToTilemap(WallPaterns[wallWhat], WallPaterns[wallWhat2], WallPaterns[wallWhat3]);
    }

    void ClearTiles()
    {
        AddWallTilemap.ClearAllTiles();
    }

    public void AddPaternToTilemap(Tilemap tilemap, Tilemap tilemap2, Tilemap tilemap3)
    {
        //if player eject

        BoundsInt bounds = tilemap.cellBounds;
        foreach (Vector3Int pos in bounds.allPositionsWithin)
        {
            Tile tile = tilemap.GetTile<Tile>(pos);
            if (tile != null)
            {
                //AddWallTilemap.SetTile(pos, tile);
                AddWallTilemap.SetTile(Vector3Int.FloorToInt(WallSpots[wallWhere].position + pos), tile);
            }
        }

        BoundsInt bounds2 = tilemap2.cellBounds;
        foreach (Vector3Int pos in bounds2.allPositionsWithin)
        {
            Tile tile = tilemap2.GetTile<Tile>(pos);
            if (tile != null)
            {
                //AddWallTilemap.SetTile(pos, tile);
                AddWallTilemap.SetTile(Vector3Int.FloorToInt(WallSpots[wallWhere2].position + pos), tile);
            }
        }

        BoundsInt bounds3 = tilemap3.cellBounds;
        foreach (Vector3Int pos in bounds3.allPositionsWithin)
        {
            Tile tile = tilemap3.GetTile<Tile>(pos);
            if (tile != null)
            {
                //AddWallTilemap.SetTile(pos, tile);
                AddWallTilemap.SetTile(Vector3Int.FloorToInt(WallSpots[wallWhere3].position + pos), tile);
            }
        }
    }

    public void AddWarnings(Tilemap tilemap, Tilemap tilemap2,Tilemap tilemap3, int minionWhere)
    {
        Instantiate(warning, (MinionSpots[minionWhere].position), Quaternion.identity);

        BoundsInt bounds = tilemap.cellBounds;
        foreach (Vector3Int pos in bounds.allPositionsWithin)
        {
            Tile tile = tilemap.GetTile<Tile>(pos);
            if (tile != null)
            {
                Instantiate(warning, WallSpots[wallWhere].position * 0.8f + pos + new Vector3(0.4f, 0.4f), Quaternion.identity);
                
            }
        }

        BoundsInt bounds2 = tilemap2.cellBounds;
        foreach (Vector3Int pos in bounds2.allPositionsWithin)
        {
            Tile tile = tilemap2.GetTile<Tile>(pos);
            if (tile != null)
            {
                Instantiate(warning, WallSpots[wallWhere2].position * 0.8f + pos + new Vector3(0.4f, 0.4f), Quaternion.identity);
            }
        }

        BoundsInt bounds3 = tilemap3.cellBounds;
        foreach (Vector3Int pos in bounds3.allPositionsWithin)
        {
            Tile tile = tilemap3.GetTile<Tile>(pos);
            if (tile != null)
            {
                Instantiate(warning, WallSpots[wallWhere3].position * 0.8f + pos + new Vector3(0.4f, 0.4f), Quaternion.identity);
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
