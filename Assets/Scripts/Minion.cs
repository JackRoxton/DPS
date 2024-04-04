using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.VFX;

public class Minion : MonoBehaviour
{
    protected float hp;
    protected float speed = 0.035f;

    public GameObject player;

    public bool hitboxActive = false;
    public bool stop = false;
    public bool stun = false;
    public bool dmgFlag = false;

    public bool invincible = false;
    
    float stunTimer = 2f;
    float stopTimer = 2f;
    float timer = 0;
    float stunPower = 30f;
    float dashPower = 30f;
    public GameObject slash;
    GameObject go;

    public float phaseMult = 1;

    bool pause = false;

    bool faceR = false;

    Rigidbody2D rb;
    Animator controller;

    // Start is called before the first frame update
    void Start()
    {
        if (GameManager.Instance.currentState == GameManager.gameStates.Pause) { Pause(true); }

        controller = GetComponent<Animator>();
        speed = 0.025f + (phaseMult / 150);
        rb = this.GetComponent<Rigidbody2D>();

        if (player == null) return;

        if (transform.position.x < player.transform.position.x)
        {
            faceR = false;
            this.GetComponent<SpriteRenderer>().flipX = true;
        }
        else if (transform.position.x > player.transform.position.x)
        {
            faceR = true;
            this.GetComponent<SpriteRenderer>().flipX = false;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (GameManager.Instance.endFlag == true) Die();

        rb.velocity *= 0.9f;
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

        if (transform.position.x < player.transform.position.x && faceR)
        {
            faceR = false;
            this.GetComponent<SpriteRenderer>().flipX = true;
        }
        else if (transform.position.x > player.transform.position.x && !faceR)
        {
            faceR = true;
            this.GetComponent<SpriteRenderer>().flipX = false;
        }

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
            return;
        }
        GameManager.Instance.ScreenShake();
        GameManager.Instance.AddScore();
        Die();
    }

    public void Stunned()
    {
        VFXManager.Instance.PlayEffectOn("Stun", this.gameObject);
        UIManager.Instance.FloatingText(this.transform.position, "stun", true, this.gameObject,new Color32(0xf8,0xe0,0x28,255),12); //#f8e028

        rb.velocity = Vector2.zero;
        rb.velocity = new Vector2(this.transform.position.x - player.transform.position.x,this.transform.position.y - player.transform.position.y).normalized*stunPower;
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

    void Die()
    {
        RoomManager.Instance.RemoveMinion(this.gameObject);
        SoundManager.Instance.Play("hit");
        Destroy(this.gameObject);
    }

    IEnumerator Attack()
    {
        dmgFlag = true;
        controller.Play("MinionAttack", 0);
        stop = true;
        timer = stopTimer;
        rb.velocity = Vector2.zero;
        VFXManager.Instance.PlayEffectOn("Circle", this.gameObject);
        if (stun) StopCoroutine(Attack());
        yield return new WaitUntil(HitboxActive);
        while (GameManager.Instance.currentState == GameManager.gameStates.Pause)
        {
            yield return new WaitForEndOfFrame();
        }
        if (!stun)
        {
            go = Instantiate(slash, transform.position, Quaternion.identity);
            go.transform.right = new Vector3(player.transform.position.x - this.transform.position.x, player.transform.position.y - this.transform.position.y,0);
            go.GetComponentInChildren<VisualEffect>().Play();
            rb.velocity = new Vector2(player.transform.position.x - this.transform.position.x,player.transform.position.y - this.transform.position.y).normalized * dashPower;
        }
        yield return new WaitForSeconds(1.5f);
        if(go)
            Destroy(go);
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

    /*private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision == null) return;

        if (collision.gameObject.GetComponent<Player>() != null)
        {
            rb.velocity = Vector2.zero;
            stop = true;
            timer = stopTimer;
        }
    }*/
}
