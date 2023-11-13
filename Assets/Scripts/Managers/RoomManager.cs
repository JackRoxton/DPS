using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : Singleton<RoomManager>
{
    public GameObject magePrefab;
    GameObject mage;

    public GameObject player;

    public List<GameObject> WallPaterns = new List<GameObject>();
    public List<Transform> WallSpots = new List<Transform>();
    public List<Transform> MageSpots = new List<Transform>();

    List<GameObject> CurrentWalls = new List<GameObject>();

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
        int wallWhat = Random.Range(0, WallPaterns.Count);
        int wallWhere = Random.Range(0, WallSpots.Count);
        int mageWhere = Random.Range(0, MageSpots.Count);
        lastMageSpot = mageWhere;
        mage = Instantiate(magePrefab, MageSpots[mageWhere]);
        mage.GetComponent<Mage>().Player = player;
        //warning
        yield return new WaitForSeconds(2);
        CurrentWalls.Add(Instantiate(WallPaterns[wallWhat], WallSpots[wallWhere]));
    }

    public IEnumerator RoomChange()
    {
        foreach (GameObject go in CurrentWalls)
        {
            //effets sur les murs
            Destroy(go);
        }
        CurrentWalls.Clear();

        int wallWhat = Random.Range(0, WallPaterns.Count);
        int wallWhere = Random.Range(0, WallSpots.Count);
        int mageWhere;
        do
        {
            mageWhere = Random.Range(0, MageSpots.Count);
        } while (mageWhere == lastMageSpot);
        lastMageSpot = mageWhere;
        mage.GetComponent<Mage>().Teleport(MageSpots[mageWhere]);
        //warning
        yield return new WaitForSeconds(2);
        Instantiate(WallPaterns[wallWhat], WallSpots[wallWhere]);
    }
}
