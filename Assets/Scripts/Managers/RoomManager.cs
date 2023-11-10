using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : Singleton<RoomManager>
{
    public Mage mage;

    public List<GameObject> WallPaterns = new List<GameObject>();
    public List<Transform> WallSpots = new List<Transform>();
    public List<Transform> MageSpots = new List<Transform>();

    public int lastMageSpot = 0; //so the mage can't tp where he already was

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void ChangeRoom()
    {
        Debug.Log("change");
        int wallWhat = Random.Range(0, WallPaterns.Count);
        int wallWhere = Random.Range(0, WallSpots.Count);
        int mageWhere;
        do{
            mageWhere = Random.Range(0, MageSpots.Count);
        } while (mageWhere == lastMageSpot);
        lastMageSpot = mageWhere;
        mage.Teleport(MageSpots[mageWhere]);
        //warning
        Instantiate(WallPaterns[wallWhat], WallSpots[wallWhere]);
        
    }
}
