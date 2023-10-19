using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Vector2 movement;
    Rigidbody2D body;

    [Range(0f, 10f)]
    public float speed = 1f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis("Horizontal") != 0) // voir meilleurs inputs pour déplacements diagonaux
        {
            movement.x = Input.GetAxis("Horizontal");
        }
        if (Input.GetAxis("Vertical") != 0)
        {
            movement.y = Input.GetAxis("Vertical");
        }

        this.transform.position += new Vector3(movement.x, movement.y, 0) * speed / 100;
    }
}
