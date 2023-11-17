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

    public GameObject player;

    public Tilemap AddWallTilemap;
    public List<Tilemap> WallPaterns;
    //public Vector3Int[] tilePos;
    int wallWhere;

    //public List<GameObject> WallPaterns = new List<GameObject>();
    public List<Transform> WallSpots = new List<Transform>();
    public List<Transform> MageSpots = new List<Transform>();
    public List<Transform> MinionSpots = new List<Transform>();

    int lastMageSpot = 0; //so the mage can't tp where he already was

    void Start()
    {
        StartCoroutine(FirstRoomChange());
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
        wallWhere = Random.Range(0, WallSpots.Count);
        int mageWhere = Random.Range(0, MageSpots.Count);
        int minionWhere = Random.Range(0, MinionSpots.Count);
        lastMageSpot = mageWhere;
        mage = Instantiate(magePrefab, MageSpots[mageWhere].position, Quaternion.identity);
        mage.GetComponent<Mage>().Player = player;

        //warnings

        yield return new WaitForSeconds(2);

        minion = Instantiate(minionPrefab, MinionSpots[minionWhere]);
        minion.GetComponent<Minion>().player = this.player;

        AddPaternToTilemap(WallPaterns[wallWhat]);
    }

    public IEnumerator RoomChange()
    {
        ClearTiles();

        int wallWhat = Random.Range(0, WallPaterns.Count);
        wallWhere = Random.Range(0, WallSpots.Count);
        int minionWhere = Random.Range(0, MinionSpots.Count);
        int mageWhere;
        do
        {
            mageWhere = Random.Range(0, MageSpots.Count);
        } while (mageWhere == lastMageSpot);
        lastMageSpot = mageWhere;
        mage.GetComponent<Mage>().Teleport(MageSpots[mageWhere]);

        //warning

        yield return new WaitForSeconds(2);

        if (minion == null)
        {
            minion = Instantiate(minionPrefab, MinionSpots[minionWhere].position, Quaternion.identity);
            minion.GetComponent<Minion>().player = this.player;
        }

        AddPaternToTilemap(WallPaterns[wallWhat]);
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
                AddWallTilemap.SetTile(Vector3Int.FloorToInt(WallSpots[wallWhere].position) + pos, tile);
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
