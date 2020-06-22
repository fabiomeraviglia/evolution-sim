using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellMovement : MonoBehaviour
{
    public float UPPER_BOUNDARY = 20;
    public float RIGHT_BOUNDARY =20;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        Vector3 pos = transform.position;
        if (pos.y < 0)
        {
            pos.y = UPPER_BOUNDARY + pos.y;
        }
        if (pos.y > UPPER_BOUNDARY)
        {
            pos.y = pos.y- UPPER_BOUNDARY;
        }
        if (pos.x < 0)
        {
            pos.x = RIGHT_BOUNDARY + pos.x;
        }
        if (pos.x > RIGHT_BOUNDARY)
        {
            pos.x = pos.x - RIGHT_BOUNDARY;
        }
        transform.position = pos;
    }
}
