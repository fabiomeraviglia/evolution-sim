using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrier : MonoBehaviour
{
    // Start is called before the first frame update

    private static System.Random r = new System.Random();
    void Start()
    {
        InvokeRepeating("ChangeSize", 10f, 60f);
    }

    float currentSize = 1;

    public void ChangeSize()
    {

        if (currentSize == 1)
        {

            currentSize = 1.1f;
            transform.localScale = new Vector2(transform.localScale.x * 1.1f, transform.localScale.y);

        }
        else
        {
            if (r.NextDouble() > 0.95)
            {
                currentSize = 1f;
                transform.localScale = new Vector2(transform.localScale.x / 1.1f, transform.localScale.y);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject other = collision.gameObject;

        if (other.tag == "food")
        {
            Destroy(other);
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        GameObject other = collision.gameObject;

        if (other.tag == "food")
        {
            Destroy(other);
        }
    }
}
