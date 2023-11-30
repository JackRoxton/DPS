using JetBrains.Annotations;
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
    public List<Tilemap> inWallPaterns;
    public List<Tilemap> outWallPaterns;

    public GameObject walls;
    public GameObject outWalls;
    public GameObject mages;
    public GameObject minions;
    List<Transform> inWallSpots = new List<Transform>();
    List<Transform> outWallSpots = new List<Transform>();
    List<Transform> MageSpots = new List<Transform>();
    List<Transform> MinionSpots = new List<Transform>();

    int lastMageSpot = 0;

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
        int inWallWhat = Random.Range(0, inWallPaterns.Count);
        int inWallWhat2 = Random.Range(0, inWallPaterns.Count);
        int outWallWhat = Random.Range(0, outWallPaterns.Count);
        int outWallWhat2 = Random.Range(0, outWallPaterns.Count);

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
        int mageWhere = Random.Range(0, MageSpots.Count);
        lastMageSpot = mageWhere;
        mage = Instantiate(magePrefab, MageSpots[mageWhere].position,Quaternion.identity);
        mage.GetComponent<Mage>().Player = player;

        AddInWarnings(inWallPaterns[inWallWhat], inWallPaterns[inWallWhat2], inWallWhere, inWallWhere2);
        Instantiate(warning, (MinionSpots[minionWhere].position), Quaternion.identity);
        AddOutWarnings(outWallPaterns[outWallWhat], outWallPaterns[outWallWhat2], outWallWhere, outWallWhere2);

        yield return new WaitForSeconds(2);
        while (GameManager.Instance.currentState == GameManager.gameStates.Pause)
        {
            yield return new WaitForEndOfFrame();
        }

        SoundManager.Instance.Play("spawn");

        if (minion == null)
        {
            minion = Instantiate(minionPrefab, MinionSpots[minionWhere].position, Quaternion.identity);
            minion.GetComponent<Minion>().player = this.player;
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
        mage.GetComponent<Mage>().Teleport(MageSpots[mageWhere]);

        AddInWarnings(inWallPaterns[inWallWhat], inWallPaterns[inWallWhat2], inWallWhere, inWallWhere2);
        Instantiate(warning, (MinionSpots[minionWhere].position), Quaternion.identity);
        AddOutWarnings(outWallPaterns[outWallWhat], outWallPaterns[outWallWhat2], outWallWhere, outWallWhere2);

        yield return new WaitForSeconds(2);
        while (GameManager.Instance.currentState == GameManager.gameStates.Pause)
        {
            yield return new WaitForEndOfFrame();
        }

        SoundManager.Instance.Play("spawn");

        if (minion == null)
        {
            minion = Instantiate(minionPrefab, MinionSpots[minionWhere].position, Quaternion.identity);
            minion.GetComponent<Minion>().player = this.player;
        }

        AddInsidePaterns(inWallPaterns[inWallWhat], inWallPaterns[inWallWhat2], inWallWhere, inWallWhere2);
        AddOutsidePaterns(outWallPaterns[outWallWhat], outWallPaterns[outWallWhat2], outWallWhere, outWallWhere2);
    }

    void ClearTiles()
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
            }
        }

        BoundsInt bounds2 = tilemap2.cellBounds;
        foreach (Vector3Int pos in bounds2.allPositionsWithin)
        {
            Tile tile = tilemap2.GetTile<Tile>(pos);
            if (tile != null)
            {
                AddWallTilemap.SetTile(Vector3Int.FloorToInt(inWallSpots[where2].position + pos), tile);
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
            }
        }

        BoundsInt bounds2 = tilemap2.cellBounds;
        foreach (Vector3Int pos in bounds2.allPositionsWithin)
        {
            Tile tile = tilemap2.GetTile<Tile>(pos);
            if (tile != null)
            {
                AddWallTilemap.SetTile(Vector3Int.FloorToInt(outWallSpots[where2].position + pos), tile);
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
                Instantiate(warning, inWallSpots[where].position * AddWallTilemap.transform.localScale.x + pos + new Vector3(0.4f, 0.4f), Quaternion.identity);
                
            }
        }

        BoundsInt bounds2 = tilemap2.cellBounds;
        foreach (Vector3Int pos in bounds2.allPositionsWithin)
        {
            Tile tile = tilemap2.GetTile<Tile>(pos);
            if (tile != null)
            {
                Instantiate(warning, inWallSpots[where2].position * AddWallTilemap.transform.localScale.x + pos + new Vector3(0.4f, 0.4f), Quaternion.identity);
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
                Instantiate(warning, outWallSpots[where].position * AddWallTilemap.transform.localScale.x + pos + new Vector3(0.4f, 0.4f), Quaternion.identity);

            }
        }

        BoundsInt bounds2 = tilemap2.cellBounds;
        foreach (Vector3Int pos in bounds2.allPositionsWithin)
        {
            Tile tile = tilemap2.GetTile<Tile>(pos);
            if (tile != null)
            {
                Instantiate(warning, outWallSpots[where2].position * AddWallTilemap.transform.localScale.x + pos + new Vector3(0.4f, 0.4f), Quaternion.identity);
            }
        }

    }

    public void EndGame()
    {
        ClearTiles();
        Destroy(mage);
        Destroy(minion);
        PlayerEndFlag(true);
    }

    public void NextPhase()
    {
        PlayerEndFlag(false);
        StartCoroutine(FirstRoomChange());
    }

    public void Pause(bool pause)
    {
        player.GetComponent<Player>().Pause(pause);
        if(minion != null)
            minion.GetComponent<Minion>().Pause(pause);
        mage.GetComponent<Mage>().Pause(pause);
    }

    public void PlayerEndFlag(bool state)
    {
        player.GetComponent<Player>().endFlag = state;
    }

    public void PlayerSpeed(float speedMod)
    {
        player.GetComponent<Player>().speedUpgrade = speedMod;
    }

    public void PlayerDash(bool OnMovement)
    {
        player.GetComponent<Player>().dashOnMovement = OnMovement;//check si ça fonctionne dans les ingame settings
    }

}
