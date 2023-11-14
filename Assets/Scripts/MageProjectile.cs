using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            Destroy(this.gameObject);
        }
    }
}
