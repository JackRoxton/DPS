using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class Minion : MonoBehaviour
{
    protected float hp;
    protected float speed = 0.05f;

    public GameObject player;

    public bool hitboxActive = false;
    public bool stop = false;
    public bool stun = false;
    public bool dmgFlag = false;

    public bool invincible = false;
    
    float stunTimer = 2f;
    float stopTimer = 2f;
    float timer = 0;
    float stunPower = 10f;
    float dashPower = 20f;

    bool pause = false;

    Rigidbody2D rb;
    Animator controller;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<Animator>();
        speed = 0.05f;
        rb = this.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (GameManager.Instance.currentState == GameManager.gameStates.Pause) return;
        if (pause) return;

        if (stun)
        {
            timer -= Time.deltaTime;
            dmgFlag = false;
            if (timer <= 0)
            {
                stun = false;
                dmgFlag = false;
            }
        }
        else if (stop)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                stop = false;
                dmgFlag = false;
            }
        }
        else
            Follow();

        rb.velocity *= 0.9f;
    }

    public void Pause(bool pause)
    {
        this.pause = pause;
        if (pause)
            this.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    }

    public void TakeDamage()
    {
        if(invincible)
        {
            Debug.Log("lol");
            return;
        }
        GameManager.Instance.ScreenShake();
        GameManager.Instance.AddScore();
        Destroy(this.gameObject);
    }

    public void Stunned()
    {
        rb.velocity = new Vector2(this.transform.position.x - player.transform.position.x,this.transform.position.y - player.transform.position.y)*stunPower;
        stun = true;
        stop = false;
        timer = stunTimer;
;   }

    protected void Follow()
    {
        if (player != null)
        {
            Vector3 newPos = new Vector3(player.transform.position.x, player.transform.position.y, 0);
            transform.position = Vector3.MoveTowards(transform.position, newPos, speed);
        }
    }

    IEnumerator Attack()
    {
        dmgFlag = true;
        controller.Play("MinionAttack", 0);
        stop = true;
        timer = stopTimer;
        rb.velocity = Vector2.zero;
        yield return new WaitUntil(HitboxActive);
        rb.velocity = new Vector2(player.transform.position.x - this.transform.position.x,player.transform.position.y - this.transform.position.y).normalized * dashPower;
    }

    bool HitboxActive()
    {
        return hitboxActive;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision == null) return;

        if(collision.gameObject.GetComponent<Player>() != null && !dmgFlag && !stop && !stun)
        {
            StartCoroutine(Attack());
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision == null) return;

        if (collision.gameObject.GetComponent<Player>() != null && !dmgFlag && !stop && !stun)
        {
            StartCoroutine(Attack());
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision == null) return;

        if (collision.gameObject.GetComponent<Player>() != null)
        {
            rb.velocity = Vector2.zero;
            stop = true;
            timer = stopTimer;
        }
    }
}
