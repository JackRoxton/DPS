using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MageProjectile : MonoBehaviour
{
    public Rigidbody2D body;

    float lifeTimer = 4f;

    // Start is called before the first frame update
    void Start()
    {
        body = this.gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        lifeTimer -= Time.deltaTime;
        if(lifeTimer <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        Destroy(this.gameObject );
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision == null) return;

        if(collision.gameObject.GetComponent<TilemapCollider2D>() != null)
        {
            Die();
        }
    }
}
