using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MageProjectile : MonoBehaviour
{
    public Vector2 velocity;
    bool flag = false;

    public Rigidbody2D body;

    float lifeTimer = 5f;

    public bool hitFlag = true;

    // Start is called before the first frame update
    void Start()
    {
        body = this.gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.endFlag)
        {
            Die();
        }

        if (GameManager.Instance.currentState == GameManager.gameStates.Pause) 
        {
            velocity = this.body.velocity;
            this.body.velocity = Vector2.zero;
            flag = true;
            return;
        }

        if (flag)
        {
            flag = false;
            this.body.velocity = velocity;
        }

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
