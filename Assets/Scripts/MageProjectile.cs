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
        transform.right = body.velocity;

        if (GameManager.Instance.endFlag)
        {
            Die();
        }

        if (GameManager.Instance.currentState == GameManager.gameStates.Pause) 
        {
            if(!flag) { 
                velocity = this.body.velocity;
                this.body.velocity = Vector2.zero;
                flag = true;
            }
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
        hitFlag = false;
        Destroy(this.gameObject );
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision == null) return;
        if (collision.tag == "Spikes") return;

        if(collision.gameObject.GetComponent<TilemapCollider2D>() != null)
        {
            Die();
        }
    }
}
