using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MageLaser : MonoBehaviour
{
    float lifeTime = 0.75f;
    float expandSpeed = 0.025f;
    float shrinkSpeed = 0.0025f;
    float maxSize = 10;
    float minShrink = 0.2f;
    float laserSize = 0.2f;
    public GameObject Zone;
    public bool hitFlag = true;

    void Start()
    {
        StartCoroutine(ExpandThenLaser());
    }

    public IEnumerator ExpandThenLaser()
    {
        Zone.GetComponent<BoxCollider2D>().enabled = false;

        do
        {
            Zone.transform.localScale = new Vector2(Zone.transform.localScale.x + expandSpeed, Zone.transform.localScale.y);
            yield return null;
        } while (Zone.transform.localScale.x < maxSize);

        do
        {
            Zone.transform.localScale = new Vector2(Zone.transform.localScale.x, Zone.transform.localScale.y - shrinkSpeed);
            yield return null;
        } while (Zone.transform.localScale.y > minShrink);

        Zone.transform.localScale = new Vector2(Zone.transform.localScale.x, laserSize);
        hitFlag = true;
        Zone.GetComponent<BoxCollider2D>().enabled = true;
        Zone.GetComponent<SpriteRenderer>().color = Color.blue;

        yield return new WaitForSeconds(lifeTime);
        Die();

    }

    public void Die()
    {
        Destroy(this.gameObject);
    }

}
