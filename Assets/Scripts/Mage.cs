using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mage : MonoBehaviour
{
    public GameObject SpellPrefab;
    public GameObject CurrentSpell;
    public GameObject Player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CastSpell()
    {
        CurrentSpell = Instantiate(SpellPrefab, this.transform);
        CurrentSpell.GetComponent<MageProjectile>().body.velocity = new Vector2(this.transform.position.x - Player.transform.position.x, this.transform.position.y - Player.transform.position.y);
    }

    public void Teleport(Transform spot)
    {

    }

    public void TakeDamage()
    {
        GameManager.Instance.AddScore(10);
    }
}
