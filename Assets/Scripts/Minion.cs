using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minion : MonoBehaviour
{
    protected float hp;
    protected float speed;

    public Player player;

    bool stun = false;
    float stunTimer = 2f;
    float timer = 0;

    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        speed = 0.5f;
        rb = this.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (stun)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
                stun = false;
        }
        else
            Follow();
    }

    public void TakeDamage()
    {
        GameManager.Instance.AddScore();
        Destroy(this.gameObject);
    }

    public void Stunned()
    {
        rb.velocity = new Vector2(this.transform.position.x - player.transform.position.x,this.transform.position.y - player.transform.position.y)*5;
        stun = true;
        timer = stunTimer;
;   }

    protected void Follow()
    {
        if (player != null)
        {
            Vector3 newPos = new Vector3(player.transform.position.x, player.transform.position.y, 0);
            transform.position = Vector3.MoveTowards(transform.position, newPos, speed / 100);
        }
    }
}
