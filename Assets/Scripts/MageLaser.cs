using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MageLaser : MonoBehaviour
{
    float lifeTime = 0.35f;
    float expandSpeed = 0.05f;
    float shrinkSpeed = 0.005f;
    float maxSize = 10;
    float minShrink = 0.1f;
    float laserSize = 0.4f;
    public GameObject Zone;
    public bool hitFlag = false;

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
        Zone.GetComponent<BoxCollider2D>().enabled = false;
        Zone.GetComponent<SpriteRenderer>().color = Color.blue;

        yield return new WaitForSeconds(lifeTime);
        Die();

    }

    public void Die()
    {
        Destroy(this.gameObject);
    }

}
