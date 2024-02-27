using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;

public class Player : MonoBehaviour
{
    Vector2 movement;
    Animator controller;
    Rigidbody2D body;
    public WeaponParent weaponparent;
    public Weapon weapon;
    public GameObject shield;
    PlayerInput input;

    public bool endFlag = false;
    bool pause = false;
    //bool mouseLock = false;

    public enum states
    {
        Base,
        Dodge,
        Parry,
        Dialogue
    }
    public states currentState = states.Base;

    [NonSerialized] public float attackSpeed = 1f;
    [NonSerialized] public float attackSpeedUpgrade = 1f;
    [NonSerialized] public float dodgePow = 1f;
    [NonSerialized] public float parryPow = 1f;
    [NonSerialized] public float speedUpgrade = 1f;
    [NonSerialized] public float speed = 0.1f;
    [NonSerialized] public float dashPower = 30f;
    float kbForce = 2f;

    bool stun = false;
    float stunTimer = 0.25f;

    [NonSerialized] public bool dashOnMovement = false;
    [NonSerialized] public bool mouseControls = false;

   /*public bool mouseLock = false;
    Vector2 mouseLastPos = Vector2.zero;*/

    bool parryIFrame = true;
    bool parryIFrameFlag = false;
    float parryIFrameCD = 1f;
    float parryIFrameTimer = 0.1f;
    float timer;

    bool hitIFrame = false;
    float hitIFrameTime = 0.15f;

    GameObject parriedMinion;

    bool shielded = false;

    bool faceR = false;

    private void Awake()
    {
        
    }

    void Start()
    {
        controller = GetComponent<Animator>();
        body = GetComponent<Rigidbody2D>();
        if (faceR != this.GetComponentInChildren<WeaponParent>().faceR)
        {
            if (faceR)
            {
                //this.GetComponent<SpriteRenderer>().flipX = true;
                controller.SetBool("FaceR", false);
                faceR = false;
            }
            else
            {
                //this.GetComponent<SpriteRenderer>().flipX = false;
                controller.SetBool("FaceR", true);
                faceR = true;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (endFlag) return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameManager.Instance.Pause();
        }

        if(stun)
        {
            stunTimer -= Time.deltaTime;
            if(stunTimer <= 0) 
            {
                stun = false;
                stunTimer = 0.25f;
            }
            else
            {
                return;
            }
        }

        if(hitIFrame)
        {
            hitIFrameTime -= Time.deltaTime;
            if(hitIFrameTime <= 0)
            {
                hitIFrame = false;
                hitIFrameTime = 0.15f;
            }
        }

        if (pause) return;

        
        if (Input.GetAxis("Slash") != 0)
        {
            if(currentState == states.Base)
                Attack();
        }
        if (Input.GetAxis("Parry") != 0)
        {
            if(currentState == states.Base)
                Parry();
        }
        if (Input.GetAxis("Dodge") != 0)
        {
            if(currentState == states.Base)
                Dodge();
        }

        /*if (Input.GetKeyDown(KeyCode.E))
        {
            mouseLock = true;
            Mouse.current.WarpCursorPosition(Camera.main.WorldToScreenPoint(this.transform.position));
            mouseLastPos = Camera.main.WorldToScreenPoint(this.transform.position);
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if(dashOnMovement)
                dashOnMovement = false;
            else
                dashOnMovement = true;
            Debug.Log(dashOnMovement);
        }
        if(mouseLock)
        {
            Vector2 mousePos = Input.mousePosition - new Vector3(Camera.main.WorldToScreenPoint(this.transform.position).x, Camera.main.WorldToScreenPoint(this.transform.position).y, 0);
            //Vector2 dist = new Vector3(mousePos.x - this.transform.position.x, mousePos.y - this.transform.position.y, this.transform.position.z);
            Debug.Log(Vector2.Distance(Input.mousePosition, new Vector3(Camera.main.WorldToScreenPoint(this.transform.position).x, Camera.main.WorldToScreenPoint(this.transform.position).y, 0)));
            
            if (Vector2.Distance(Input.mousePosition, new Vector3(Camera.main.WorldToScreenPoint(this.transform.position).x, Camera.main.WorldToScreenPoint(this.transform.position).y, 0)) > 150) 
            {
                Mouse.current.WarpCursorPosition(mouseLastPos);
            }
            else
            {
                mouseLastPos = Input.mousePosition;
            }
        }*/

        if (parryIFrameFlag)
        {
            timer -= Time.deltaTime;
            if (currentState == states.Parry)
            {
                parryIFrameFlag = false;
                parryIFrame = true;
                if (!UIManager.Instance.dps.GetState("P"))
                { 
                    VFXManager.Instance.PlayEffectOn("Success", parriedMinion.gameObject);
                    UIManager.Instance.FloatingText(this.transform.position, "parry", false, null, Color.blue);
                }
                UIManager.Instance.dps.Light("P",true);
                parriedMinion.GetComponent<Minion>().Stunned();
                VFXManager.Instance.PlayEffectOn("Parry", this.gameObject);
                VFXManager.Instance.HitStop();
                AddScore();
                
            }
            if(timer <= 0)
            {
                parryIFrameFlag = false;
                parryIFrame = false;
                timer = parryIFrameCD;
                TakeDamage();
            }
        }
        if (!parryIFrame)
        {
            timer -= Time.deltaTime;
            if(timer <= 0)
            {
                parryIFrame = true;
            }
        }


        //movement = Vector2.zero;
    }

    private void FixedUpdate()
    {
        if (endFlag) return;
        if (pause) return;
        if (stun) return;

        movement.x = Input.GetAxis("Horizontal");
        movement.y = Input.GetAxis("Vertical");

        if(faceR != this.GetComponentInChildren<WeaponParent>().faceR)
        {
            if (faceR)
            {
                //this.GetComponent<SpriteRenderer>().flipX = true;
                controller.SetBool("FaceR", false);
                faceR = false;
            }
            else
            {
                //this.GetComponent<SpriteRenderer>().flipX = false;
                controller.SetBool("FaceR", true);
                faceR = true;
            }
        }


        if(movement.magnitude > 0.1f)
            controller.SetBool("IsMoving", true);
        else
            controller.SetBool("IsMoving", false);
        body.velocity *= 0.9f;

       /*if (Input.GetKeyDown(KeyCode.O))
        {
            mouseLock = true;
            Debug.Log("mouse locked");
        }
        if(mouseLock)
        {
            
            Vector2 mousePos = Input.mousePosition;
            mousePos = Camera.main.ScreenToWorldPoint(mousePos);
            Debug.Log(mousePos);
            
            mousePos.x = Mathf.Clamp(mousePos.x, this.transform.position.x - 2, this.transform.position.x + 2);
            mousePos.y = Mathf.Clamp(mousePos.y, this.transform.position.y - 2, this.transform.position.y + 2);

            Debug.Log(this.transform.position);

            Mouse.current.WarpCursorPosition(Camera.main.WorldToScreenPoint(mousePos));
            //Mouse.current.WarpCursorPosition(mousePos);
        }*/

        //this.transform.position += new Vector3(movement.x, movement.y, 0).normalized * (speed * speedUpgrade);
        this.transform.position += new Vector3(movement.x, movement.y, 0) * (speed * speedUpgrade);
    }

    public void Pause(bool pause)
    {
        this.pause = pause;
    }

    public void Attack()
    {
        if (currentState != states.Base) return;
        weapon.Attack();
    }

    public void Parry()
    {
        if (currentState != states.Base) return;
        controller.speed = parryPow;

        if(faceR)
            controller.Play("Parry", 0);
        else
            controller.Play("Parry 1", 0);

        SoundManager.Instance.Play("parry");
    }

    public void Dodge()
    {
        if (currentState != states.Base) return;
        body.velocity = Vector2.zero;
        controller.speed = dodgePow;

        if(faceR)
            controller.Play("Dodge", 0);
        else
            controller.Play("Dodge 1", 0);

        SoundManager.Instance.Play("dodge");
        VFXManager.Instance.PlayEffectOn("DodgeTrail",this.gameObject);

        Vector3 dir = Vector3.zero;

        if (dashOnMovement || !mouseControls)
        {
            dir = movement;
        }
        else
        {
            Vector2 mousePos = Input.mousePosition - new Vector3(Camera.main.WorldToScreenPoint(this.transform.position).x, Camera.main.WorldToScreenPoint(this.transform.position).y, 0);
            dir = new Vector3(mousePos.x - this.transform.position.x, mousePos.y - this.transform.position.y, this.transform.position.z);
        }

        dir.Normalize();
        body.velocity = new Vector2(dir.x * dashPower * dodgePow,dir.y * dashPower * dodgePow);
    }

    private void TakeDamage()
    {
        if (hitIFrame)
            return;

        if(shielded) 
        {
            shielded = false;
            shield.SetActive(false);
            hitIFrame = true;
            UIManager.Instance.FloatingText(this.transform.position, "blocked", false, null, Color.magenta);
            return;
        }

        UIManager.Instance.FloatingText(this.transform.position, "hit", false, null, Color.magenta);

        hitIFrame = true;
        VFXManager.Instance.HitStop();
        GameManager.Instance.TakeDamage();
        SoundManager.Instance.Play("phit");
        body.velocity /= 10f;
        stun = true;
        controller.Play("HitStun");
    }

    public void Shielded()
    {
        shielded = true;
        shield.SetActive(true);
    }

    private void AddScore()
    {
        GameManager.Instance.AddScore();
    }

    public void AtkKnockBack(Vector2 kb)
    {
        body.velocity += kb.normalized * kbForce;
    }

    public void ModAttackSpeed(float atk)
    {
        attackSpeedUpgrade = atk;
        //controller.speed = attackSpeed;
        weapon.controller.speed = attackSpeed * attackSpeedUpgrade;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision == null) return;

        if (collision.gameObject.tag == "Spell")
        {
            if (collision.gameObject.GetComponent<MageProjectile>() != null)
            {
                MageProjectile spell = collision.gameObject.GetComponent<MageProjectile>();
                if (currentState != states.Dodge)
                {
                    spell.Die();
                    TakeDamage();
                }
                else
                {
                    if (spell.hitFlag == true)
                    {
                        AddScore();
                        VFXManager.Instance.HitStop();
                        if (!UIManager.Instance.dps.GetState("D"))
                        {
                            VFXManager.Instance.PlayEffectAt("Success", transform);
                            UIManager.Instance.FloatingText(this.transform.position, "dodge", false, null, Color.blue);
                        }
                        UIManager.Instance.dps.Light("D", true);
                        if (this.gameObject.GetComponentInChildren<WeaponParent>().faceR)
                            VFXManager.Instance.PlayEffectAt("Dodge", this.transform, true);
                        else
                            VFXManager.Instance.PlayEffectAt("Dodge", this.transform);
                    }
                    spell.hitFlag = false;
                    Physics2D.IgnoreCollision(this.GetComponent<Collider2D>(), collision, true);
                }
            }
            else if(collision.gameObject.GetComponent<MageAttack>() != null)
            {
                MageAttack spell = collision.gameObject.GetComponent<MageAttack>();
                if (currentState != states.Dodge)
                {
                    spell.Die();
                    TakeDamage();
                }
                else
                {
                    if(spell.hitFlag == true)
                    {
                        this.GetComponent<Rigidbody2D>().velocity += new Vector2(this.transform.position.x - spell.transform.position.x, this.transform.position.y - spell.transform.position.y).normalized * 10f ;
                        AddScore();
                        VFXManager.Instance.HitStop();
                        UIManager.Instance.dps.Light("D", true);
                        UIManager.Instance.FloatingText(this.transform.position, "dodge", false, null, Color.blue);
                        if (this.gameObject.GetComponentInChildren<WeaponParent>().faceR)
                            VFXManager.Instance.PlayEffectAt("Dodge", this.transform, true);
                        else
                            VFXManager.Instance.PlayEffectAt("Dodge", this.transform);
                    }
                    spell.hitFlag = false;
                    Physics2D.IgnoreCollision(this.GetComponent<Collider2D>(), collision, true);
                }
            }
            else if(collision.gameObject.GetComponentInParent<MageLaser>() != null)
            {
                MageLaser spell = collision.gameObject.GetComponentInParent<MageLaser>();
                if(currentState != states.Dodge)
                {
                    //spell.Die();
                    spell.hitFlag = false;
                    TakeDamage();
                }
                else
                {
                    if(spell.hitFlag == true)
                    {
                        this.GetComponent<Rigidbody2D>().velocity += new Vector2(this.transform.position.x - spell.transform.position.x, this.transform.position.y - spell.transform.position.y).normalized * 10f;
                        AddScore();
                        VFXManager.Instance.HitStop();
                        UIManager.Instance.dps.Light("D", true);
                        UIManager.Instance.FloatingText(this.transform.position, "dodge", false, null, Color.blue);
                        if (this.gameObject.GetComponentInChildren<WeaponParent>().faceR)
                            VFXManager.Instance.PlayEffectAt("Dodge", this.transform, true);
                        else
                            VFXManager.Instance.PlayEffectAt("Dodge", this.transform);
                    }
                    spell.hitFlag = false;
                    Physics2D.IgnoreCollision(this.GetComponent<Collider2D>(), collision, true);
                }
            }
        }
        else if (collision.gameObject.GetComponent<Minion>() != null)
        {
            Minion minion = collision.gameObject.GetComponent<Minion>();
            if (collision.isTrigger == true) return;
            if (currentState != states.Parry && !weapon.hitboxActive && minion.hitboxActive && !minion.stun && minion.dmgFlag)
            {
                if (parryIFrame)
                {
                    if (!parryIFrameFlag)
                        timer = parryIFrameTimer + (parryPow / 20);
                    parryIFrameFlag = true;
                    parriedMinion = collision.gameObject;
                }
                else
                {
                    minion.dmgFlag = false;
                    TakeDamage();
                }
            }
            else if (currentState == states.Parry && minion.hitboxActive && !minion.stun)
            {
                Physics2D.IgnoreCollision(this.GetComponent<Collider2D>(), collision, true);
                if (!UIManager.Instance.dps.GetState("P"))
                {
                    VFXManager.Instance.PlayEffectOn("Success", minion.gameObject);
                    UIManager.Instance.FloatingText(this.transform.position, "parry", false, null, Color.blue);
                }
                UIManager.Instance.dps.Light("P", true);
                minion.Stunned();
                AddScore();
                VFXManager.Instance.HitStop();
                VFXManager.Instance.PlayEffectOn("Parry", this.gameObject);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision == null) return;

        if (collision.gameObject.GetComponent<Minion>() != null)
            Physics2D.IgnoreCollision(this.GetComponent<Collider2D>(), collision, false);
    }

}
