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

    public List<GameObject> WallPaterns = new List<GameObject>();//voir mettre tiles
    public List<Transform> WallSpots = new List<Transform>();
    public List<Transform> MageSpots = new List<Transform>();
    public List<Transform> MinionSpots = new List<Transform>();

    List<GameObject> CurrentWalls = new List<GameObject>();//voir mettre tiles

    //public Tilemap tilemap;
    //public Tile[] tiles;

    public int lastMageSpot = 0; //so the mage can't tp where he already was

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
        int wallWhat = Random.Range(0, WallPaterns.Count);//voir chg
        int wallWhere = Random.Range(0, WallSpots.Count);
        int mageWhere = Random.Range(0, MageSpots.Count);
        int minionWhere = Random.Range(0, MinionSpots.Count);
        lastMageSpot = mageWhere;
        mage = Instantiate(magePrefab, MageSpots[mageWhere]).GetComponent<Mage>().Player = player;
        //warnings
        yield return new WaitForSeconds(2);
        minion = Instantiate(minionPrefab, MinionSpots[minionWhere]).GetComponent<Minion>().player = this.player;
        CurrentWalls.Add(Instantiate(WallPaterns[wallWhat], WallSpots[wallWhere]));

        //tilemap.SetTile(tilemap.WorldToCell(WallSpots[wallWhere].position), WallPaterns[wallWhat]);
    }

    public IEnumerator RoomChange()
    {
        foreach (GameObject go in CurrentWalls)
        {
            //effets sur les murs
            Destroy(go);
        }
        CurrentWalls.Clear();

        int wallWhat = Random.Range(0, WallPaterns.Count);//voir chg
        int wallWhere = Random.Range(0, WallSpots.Count);
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
        if(minion == null)
            minion = Instantiate(minionPrefab, MinionSpots[minionWhere]).GetComponent<Minion>().player = this.player;
        CurrentWalls.Add(Instantiate(WallPaterns[wallWhat], WallSpots[wallWhere]));
    }
}
