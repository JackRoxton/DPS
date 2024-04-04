using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.VFX;

public class RoomManager : Singleton<RoomManager>
{
    [Header("Prefabs")]
    public GameObject magePrefab;
    public GameObject mage;
    public GameObject minionPrefab;
    public List<GameObject> minionList = new List<GameObject>();
    GameObject pickup;
    public GameObject warning;
    public GameObject player;

    //public VisualEffect vfx;

    [Header("Tilemaps")]
    public Tilemap AddWallTilemap;
    public Tilemap SpikesTilemap;
    public List<Tilemap> inWallPaterns;
    public List<Tilemap> outWallPaterns;
    public List<Tilemap> spikePaterns;

    [Header("Spots")]
    public GameObject walls;
    public GameObject outWalls;
    public GameObject mages;
    public GameObject minions;
    public GameObject pickups;
    public GameObject spikes;
    List<Transform> inWallSpots = new List<Transform>();
    List<Transform> outWallSpots = new List<Transform>();
    List<Transform> MageSpots = new List<Transform>();
    List<Transform> MinionSpots = new List<Transform>();
    List<Transform> PickupsSpots = new List<Transform>();
    List<Transform> SpikeSpots = new List<Transform>();

    int pickupCount = 0;
    int lastMageSpot = 0;
    public List<GameObject> pickupsTypes;
    public int phaseMult = 0;
    public bool tuto = false;
    int spikeCD = 2;
    
    //
    // je devrais vraiment rework le code en faisant une fonction "summon patern" ou qqch comme �a
    //

    void Start()
    {
        Transform[] tr = walls.GetComponentsInChildren<Transform>();
        foreach (Transform t in tr)
        {
            inWallSpots.Add(t);
        }
        inWallSpots.Remove(walls.GetComponent<Transform>());
        tr = outWalls.GetComponentsInChildren<Transform>();
        foreach (Transform t in tr)
        {
            outWallSpots.Add(t);
        }
        outWallSpots.Remove(outWalls.GetComponent<Transform>());
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
        tr = pickups.GetComponentsInChildren<Transform>();
        foreach (Transform t in tr)
        {
            PickupsSpots.Add(t);
        }
        PickupsSpots.Remove(pickups.GetComponent<Transform>());
        tr = spikes.GetComponentsInChildren<Transform>();
        foreach (Transform t in tr)
        {
            SpikeSpots.Add(t);
        }
        SpikeSpots.Remove(spikes.GetComponent<Transform>());

        if (PlayerPrefs.GetInt("Controller") == 1)
            player.GetComponentInChildren<WeaponParent>().controller = true;
        else
            player.GetComponentInChildren<WeaponParent>().controller = false;

        /*StartCoroutine(FirstRoomChange());
        PlayerSpeed(GameManager.Instance.playerSpeedUp);
        PlayerDash(GameManager.Instance.playerDashOnMovement);*/
    }

    /*void Update()
    {
        if(GameManager.Instance.endFlag)
        {
            ClearTiles();
            StopAllCoroutines();
        }

    }*/

    public void FirstRoom()
    {
        StartCoroutine(FirstRoomChange());
    }

    public void ChangeRoom()
    {
        StartCoroutine(RoomChange());
    }

    public IEnumerator FirstRoomChange()
    {
        //Random what paterns
        int inWallWhat = Random.Range(0, inWallPaterns.Count);
        int inWallWhat2 = Random.Range(0, inWallPaterns.Count);
        int outWallWhat = Random.Range(0, outWallPaterns.Count);
        int outWallWhat2 = Random.Range(0, outWallPaterns.Count);

        //random where to put the paterns
        int inWallWhere = Random.Range(0, inWallSpots.Count);
        int inWallWhere2;
        do
        {
            inWallWhere2 = Random.Range(0, inWallSpots.Count);
        } while (inWallWhere2 == inWallWhere);
        int outWallWhere = Random.Range(0, outWallSpots.Count);
        int outWallWhere2;
        do
        {
            outWallWhere2 = Random.Range(0, outWallSpots.Count);
        } while (outWallWhere2 == outWallWhere);

        //Random where to put the ennemies
        int minionWhere = Random.Range(0, MinionSpots.Count);
        int mageWhere = Random.Range(0, MageSpots.Count);
        lastMageSpot = mageWhere;
        VFXManager.Instance.PlayEffectAt("Teleport_End", MageSpots[mageWhere]);
        mage = Instantiate(magePrefab, MageSpots[mageWhere].position,Quaternion.identity);
        mage.GetComponent<Mage>().player = player;
        mage.GetComponent<Mage>().phaseMult = phaseMult;
        if(tuto)
            mage.GetComponent<Mage>().tuto = true;

        //Spawn Warnings
        AddInWarnings(inWallPaterns[inWallWhat], inWallPaterns[inWallWhat2], inWallWhere, inWallWhere2);
        if(!tuto)
            Instantiate(warning, (MinionSpots[minionWhere].position), Quaternion.identity);
        AddOutWarnings(outWallPaterns[outWallWhat], outWallPaterns[outWallWhat2], outWallWhere, outWallWhere2);

        while (GameManager.Instance.currentState == GameManager.gameStates.Pause)
        {
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForSeconds(2);
        while (GameManager.Instance.currentState == GameManager.gameStates.Pause)
        {
            yield return new WaitForEndOfFrame();
        }

        SoundManager.Instance.Play("spawn");

        //Spawn everything
        if (!tuto && minionList.Count < 3)
        {
            GameObject m = Instantiate(minionPrefab, MinionSpots[minionWhere].position, Quaternion.identity);
            m.GetComponent<Minion>().player = this.player;
            m.GetComponent<Minion>().phaseMult = phaseMult;
            minionList.Add(m);
        }

        AddInsidePaterns(inWallPaterns[inWallWhat], inWallPaterns[inWallWhat2], inWallWhere, inWallWhere2);
        AddOutsidePaterns(outWallPaterns[outWallWhat], outWallPaterns[outWallWhat2], outWallWhere, outWallWhere2);
    }

    public IEnumerator RoomChange()
    {
        ClearTiles();
        int inWallWhat = Random.Range(0, inWallPaterns.Count);
        int inWallWhat2 = Random.Range(0, inWallPaterns.Count);
        int outWallWhat = Random.Range(0, outWallPaterns.Count);
        int outWallWhat2 = Random.Range(0, outWallPaterns.Count);
        int pickupWhat = Random.Range(0, pickupsTypes.Count);

        int inWallWhere = Random.Range(0, inWallSpots.Count);
        int inWallWhere2;
        do
        {
            inWallWhere2 = Random.Range(0, inWallSpots.Count);
        } while (inWallWhere2 == inWallWhere);
        int outWallWhere = Random.Range(0, outWallSpots.Count);
        int outWallWhere2;
        do
        {
            outWallWhere2 = Random.Range(0, outWallSpots.Count);
        } while (outWallWhere2 == outWallWhere);

        int minionWhere = Random.Range(0, MinionSpots.Count);
        int mageWhere;
        do
        {
            mageWhere = Random.Range(0, MageSpots.Count);
        } while (mageWhere == lastMageSpot);
        lastMageSpot = mageWhere;

        int pickupWhere = Random.Range(0, PickupsSpots.Count);
        if(pickupCount <= 0)
        {
            Instantiate(warning, (PickupsSpots[pickupWhere].position), Quaternion.identity);
        }

        VFXManager.Instance.PlayEffectAt("Teleport_End", MageSpots[mageWhere]);
        mage.GetComponent<Mage>().Teleport(MageSpots[mageWhere]);

        AddInWarnings(inWallPaterns[inWallWhat], inWallPaterns[inWallWhat2], inWallWhere, inWallWhere2);
        if(!tuto)
            Instantiate(warning, (MinionSpots[minionWhere].position), Quaternion.identity);
        AddOutWarnings(outWallPaterns[outWallWhat], outWallPaterns[outWallWhat2], outWallWhere, outWallWhere2);

        while (GameManager.Instance.currentState == GameManager.gameStates.Pause)
        {
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForSeconds(2);
        while (GameManager.Instance.currentState == GameManager.gameStates.Pause)
        {
            yield return new WaitForEndOfFrame();
        }

        SoundManager.Instance.Play("spawn");

        if (!tuto && minionList.Count < 3)
        {
            GameObject m = Instantiate(minionPrefab, MinionSpots[minionWhere].position, Quaternion.identity);
            m.GetComponent<Minion>().player = this.player;
            m.GetComponent<Minion>().phaseMult = phaseMult;
            minionList.Add(m);
        }

        if(pickup == null && pickupCount <= 0)
        {
            pickup = Instantiate(pickupsTypes[pickupWhat], (PickupsSpots[pickupWhere].position), Quaternion.identity);
            pickupCount = 3;
        }
        if(pickup == null) 
        { 
            pickupCount--;
        }

        AddInsidePaterns(inWallPaterns[inWallWhat], inWallPaterns[inWallWhat2], inWallWhere, inWallWhere2);
        AddOutsidePaterns(outWallPaterns[outWallWhat], outWallPaterns[outWallWhat2], outWallWhere, outWallWhere2);
        spikeCD--;
    }

    public void MidWaveSpawns()
    {

        if(spikeCD <= 0) 
        {
            spikeCD = 2;
            StartCoroutine(_MidSpikeSpawn());
        }

        if (tuto) return;
        
        if (minionList.Count < 3)
            StartCoroutine(_MidMinionSpawn());
    }

    public IEnumerator _MidMinionSpawn()
    {
        //if (tuto) StopCoroutine(_MidMinionSpawn());

        int minionWhere = Random.Range(0, MinionSpots.Count);
        Instantiate(warning, (MinionSpots[minionWhere].position), Quaternion.identity);
        yield return new WaitForSeconds(2);

        GameObject m = Instantiate(minionPrefab, MinionSpots[minionWhere].position, Quaternion.identity);
        m.GetComponent<Minion>().player = this.player;
        m.GetComponent<Minion>().phaseMult = phaseMult;
        minionList.Add(m);
    }

    public IEnumerator _MidSpikeSpawn()
    {
        SpikesTilemap.ClearAllTiles();

        /*LineRenderer line = new LineRenderer();
        line.positionCount = 2;
        line.SetPosition(0, player.transform.position);
        line.SetPosition(1, mage.transform.position);*/
        int x = Random.Range(10, 91)/10;
        Vector3 P = x * Vector3.Normalize(mage.transform.position - player.transform.position) + player.transform.position;



        Tilemap spikeWhat = spikePaterns[Random.Range(0, spikePaterns.Count)]; 
        BoundsInt bounds = spikeWhat.cellBounds;
        foreach (Vector3Int pos in bounds.allPositionsWithin)
        {
            Tile tile = spikeWhat.GetTile<Tile>(pos);
            if (tile != null)
            {
                Instantiate(warning, (Vector3)Vector3Int.FloorToInt(P + pos) * SpikesTilemap.transform.localScale.x + new Vector3(0.4f, 0.4f), Quaternion.identity);
            }
        }

        int spikeWhere = Random.Range(0, SpikeSpots.Count);
        Tilemap spikeWhat2 = spikePaterns[Random.Range(0, spikePaterns.Count)];
        BoundsInt bounds2 = spikeWhat2.cellBounds;
        foreach (Vector3Int pos in bounds2.allPositionsWithin)
        {
            Tile tile = spikeWhat2.GetTile<Tile>(pos);
            if (tile != null)
            {
                Instantiate(warning, (Vector3)Vector3Int.FloorToInt(SpikeSpots[spikeWhere].position + pos) * SpikesTilemap.transform.localScale.x + new Vector3(0.4f, 0.4f), Quaternion.identity);
            }
        }

        yield return new WaitForSeconds(2);

        bounds = spikeWhat.cellBounds;
        foreach (Vector3Int pos in bounds.allPositionsWithin)
        {
            Tile tile = spikeWhat.GetTile<Tile>(pos);
            if (tile != null)
            {
                SpikesTilemap.SetTile(Vector3Int.FloorToInt(P + pos), tile);
                Transform trs = transform;
                trs.position = (Vector3)Vector3Int.FloorToInt(P + pos) * SpikesTilemap.transform.localScale.x + new Vector3(0.4f, 0.4f);
                VFXManager.Instance.PlayEffectAt("Teleport_End", trs);
            }
        }
        
        bounds2 = spikeWhat2.cellBounds;
        foreach (Vector3Int pos in bounds2.allPositionsWithin)
        {
            Tile tile = spikeWhat2.GetTile<Tile>(pos);
            if (tile != null)
            {
                SpikesTilemap.SetTile(Vector3Int.FloorToInt(SpikeSpots[spikeWhere].position + pos), tile);
                Transform trs = transform;
                trs.position = (Vector3)Vector3Int.FloorToInt(SpikeSpots[spikeWhere].position + pos) * SpikesTilemap.transform.localScale.x + new Vector3(0.4f, 0.4f);
                VFXManager.Instance.PlayEffectAt("Teleport_End", trs);
            }
        }
    }

    public void ClearTiles()
    {
        AddWallTilemap.ClearAllTiles();
    }

    public void AddInsidePaterns(Tilemap tilemap, Tilemap tilemap2, int where, int where2)
    {
        BoundsInt bounds = tilemap.cellBounds;
        foreach (Vector3Int pos in bounds.allPositionsWithin)
        {
            Tile tile = tilemap.GetTile<Tile>(pos);
            if (tile != null)
            {
                AddWallTilemap.SetTile(Vector3Int.FloorToInt(inWallSpots[where].position + pos), tile);
                Transform trs = transform;
                trs.position = (Vector3)Vector3Int.FloorToInt(inWallSpots[where].position + pos) * AddWallTilemap.transform.localScale.x + new Vector3(0.4f, 0.4f);
                VFXManager.Instance.PlayEffectAt("Teleport_End",trs);
            }
        }

        BoundsInt bounds2 = tilemap2.cellBounds;
        foreach (Vector3Int pos in bounds2.allPositionsWithin)
        {
            Tile tile = tilemap2.GetTile<Tile>(pos);
            if (tile != null)
            {
                AddWallTilemap.SetTile(Vector3Int.FloorToInt(inWallSpots[where2].position + pos), tile);
                Transform trs = transform;
                trs.position = (Vector3)Vector3Int.FloorToInt(inWallSpots[where2].position + pos) * AddWallTilemap.transform.localScale.x + new Vector3(0.4f, 0.4f);
                VFXManager.Instance.PlayEffectAt("Teleport_End", trs);
            }
        }

    }

    public void AddOutsidePaterns(Tilemap tilemap, Tilemap tilemap2, int where, int where2)
    {
        BoundsInt bounds = tilemap.cellBounds;
        foreach (Vector3Int pos in bounds.allPositionsWithin)
        {
            Tile tile = tilemap.GetTile<Tile>(pos);
            if (tile != null)
            {
                AddWallTilemap.SetTile(Vector3Int.FloorToInt(outWallSpots[where].position + pos), tile);
                Transform trs = transform;
                trs.position = (Vector3)Vector3Int.FloorToInt(outWallSpots[where].position + pos) * AddWallTilemap.transform.localScale.x + new Vector3(0.4f, 0.4f);
                VFXManager.Instance.PlayEffectAt("Teleport_End", trs);
            }
        }

        BoundsInt bounds2 = tilemap2.cellBounds;
        foreach (Vector3Int pos in bounds2.allPositionsWithin)
        {
            Tile tile = tilemap2.GetTile<Tile>(pos);
            if (tile != null)
            {
                AddWallTilemap.SetTile(Vector3Int.FloorToInt(outWallSpots[where2].position + pos), tile);
                Transform trs = transform;
                trs.position = (Vector3)Vector3Int.FloorToInt(outWallSpots[where2].position + pos) * AddWallTilemap.transform.localScale.x + new Vector3(0.4f, 0.4f);
                VFXManager.Instance.PlayEffectAt("Teleport_End", trs);
            }
        }
    }

    public void AddInWarnings(Tilemap tilemap, Tilemap tilemap2, int where, int where2)
    {

        BoundsInt bounds = tilemap.cellBounds;
        foreach (Vector3Int pos in bounds.allPositionsWithin)
        {
            Tile tile = tilemap.GetTile<Tile>(pos);
            if (tile != null)
            {
                //Instantiate(warning, inWallSpots[where].position * AddWallTilemap.transform.localScale.x + pos + new Vector3(0.4f, 0.4f), Quaternion.identity);

                Instantiate(warning, (Vector3)Vector3Int.FloorToInt(inWallSpots[where].position + pos) * AddWallTilemap.transform.localScale.x + new Vector3(0.4f, 0.4f), Quaternion.identity);

            }
        }

        BoundsInt bounds2 = tilemap2.cellBounds;
        foreach (Vector3Int pos in bounds2.allPositionsWithin)
        {
            Tile tile = tilemap2.GetTile<Tile>(pos);
            if (tile != null)
            {
                //Instantiate(warning, inWallSpots[where2].position * AddWallTilemap.transform.localScale.x + pos + new Vector3(0.4f, 0.4f), Quaternion.identity);

                Instantiate(warning, (Vector3)Vector3Int.FloorToInt(inWallSpots[where2].position + pos) * AddWallTilemap.transform.localScale.x + new Vector3(0.4f, 0.4f), Quaternion.identity);
            }
        }

    }
    public void AddOutWarnings(Tilemap tilemap, Tilemap tilemap2, int where, int where2)
    {

        BoundsInt bounds = tilemap.cellBounds;
        foreach (Vector3Int pos in bounds.allPositionsWithin)
        {
            Tile tile = tilemap.GetTile<Tile>(pos);
            if (tile != null)
            {
                //Instantiate(warning, outWallSpots[where].position * AddWallTilemap.transform.localScale.x + pos + new Vector3(0.4f, 0.4f), Quaternion.identity);

                Instantiate(warning, (Vector3)Vector3Int.FloorToInt(outWallSpots[where].position + pos) * AddWallTilemap.transform.localScale.x + new Vector3(0.4f, 0.4f), Quaternion.identity);

            }
        }

        BoundsInt bounds2 = tilemap2.cellBounds;
        foreach (Vector3Int pos in bounds2.allPositionsWithin)
        {
            Tile tile = tilemap2.GetTile<Tile>(pos);
            if (tile != null)
            {
                //Instantiate(warning, outWallSpots[where2].position * AddWallTilemap.transform.localScale.x + pos + new Vector3(0.4f, 0.4f), Quaternion.identity);

                Instantiate(warning, (Vector3)Vector3Int.FloorToInt(outWallSpots[where2].position + pos) * AddWallTilemap.transform.localScale.x + new Vector3(0.4f, 0.4f), Quaternion.identity);
            }
        }

    }

    public void EndGame()
    {
        ClearTiles();
        SpikesTilemap.ClearAllTiles();
        StopAllCoroutines();
        Destroy(mage);
        /*foreach(GameObject m in minionList)
        {
            minionList.Remove(m);
            Destroy(m);
        }*/
        PlayerEndFlag(true);
        player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    }

    public void RemoveMinion(GameObject m)
    {
        minionList.Remove(m);
    }

    public void NextPhase()
    {
        PlayerEndFlag(false);
        phaseMult += 1;
        StartCoroutine(FirstRoomChange());
        //Debug.Log(phaseMult);
    }

    public void Pause(bool pause)
    {
        player.GetComponent<Player>().Pause(pause);
        if (minionList.Count > 0)
        {
            foreach (GameObject m in minionList)
            {
                m.GetComponent<Minion>().Pause(pause);
            }
        }
        if(mage)
            mage.GetComponent<Mage>().Pause(pause);
    }

    public void Resetvar()
    {
        phaseMult = 0;
    }

    public void PlayerEndFlag(bool state)
    {
        player.GetComponent<Player>().endFlag = state;
    }

    /*public void PlayerSpeed(float speedMod)
    {
        player.GetComponent<Player>().speedUpgrade += speedMod;
    }

    public void PlayerAttackSpeed(float speedMod)
    {
        player.GetComponent<Player>().ModAttackSpeed(speedMod);
    }

    public void PlayerAttack()
    {
        player.GetComponentInChildren<Weapon>().gameObject.transform.localScale *= 1.05f;
    }

    public void PlayerParryPow(float pow)
    {
        player.GetComponent<Player>().parryPow = pow;
    }

    public void PlayerDodgePow(float pow)
    {
        player.GetComponent<Player>().dodgePow = pow;
    }

    public void PlayerDash(bool OnMovement)
    {
        player.GetComponent<Player>().dashOnMovement = OnMovement;
    }*/

    public void PickupSpeed()
    {
        player.GetComponent<Player>().ModSpeed();
    }
    public void PickupAttackSpeed()
    {
        player.GetComponent<Player>().ModAttackSpeed();
    }
    public void UnLock()
    {
        if (!pickup) return;
        pickup.GetComponent<Pickup>().Unlock();
    }

    public void Shockwave()
    {
        foreach (GameObject m in minionList)
        {
            if(m == null)
            {
                minionList.Remove(m);
                return;
            }
            m.GetComponent<Minion>().Stunned();
        }
        mage.GetComponent<Mage>().Stunned();
        player.GetComponent<Player>().Shielded();
        VFXManager.Instance.PlayEffectOn("Strength",player.GetComponentInChildren<WeaponParent>().gameObject);
    }

    public void SetPickupLightState()
    {
        if(!pickup) return;
        if (UIManager.Instance.dps.GetD())
            pickup.GetComponent<Pickup>().SetLightState("D");
        if (UIManager.Instance.dps.GetP())
            pickup.GetComponent<Pickup>().SetLightState("P");
        if (UIManager.Instance.dps.GetS())
            pickup.GetComponent<Pickup>().SetLightState("S");
    }

    /*public void PlayConfettis()
    {
        vfx.Play();
    }*/

}
