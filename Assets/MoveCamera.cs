using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    float targetZoom;
    // Start is called before the first frame update
    void Start()
    {
        targetZoom = Camera.main.orthographicSize;
    }
    float speed = 3f;
    void Update()
    {
        Map map = FindObjectOfType<Map>();
        

        if (Input.GetKey(KeyCode.RightArrow) && transform.position.x<map.width)
        {
            transform.Translate(new Vector3(speed*2 * Time.deltaTime, 0, 0));
        }
        if (Input.GetKey(KeyCode.LeftArrow) && transform.position.x > 0)
        {
            transform.Translate(new Vector3(-speed * 2 * Time.deltaTime, 0, 0));
        }
        if (Input.GetKey(KeyCode.DownArrow) && transform.position.y > 0 )
        {
            transform.Translate(new Vector3(0, -speed * 2 * Time.deltaTime, 0));
        }
        if (Input.GetKey(KeyCode.UpArrow) && transform.position.y < map.height)
        {
            transform.Translate(new Vector3(0, speed * 2 * Time.deltaTime, 0));
        }

        float edgeSize = 30f;


        if(Input.mousePosition.x> Screen.width -edgeSize && transform.position.x < map.width)
        {
            transform.Translate(new Vector3(speed * Time.deltaTime, 0, 0));
        }

        if (Input.mousePosition.x < edgeSize && transform.position.x > 0)
        {
            transform.Translate(new Vector3(-speed * Time.deltaTime, 0, 0));
        }

        if (Input.mousePosition.y > Screen.height - edgeSize && transform.position.y < map.height)
        {
            transform.Translate(new Vector3(0, speed * Time.deltaTime, 0));
        }

        if (Input.mousePosition.y <  edgeSize && transform.position.y > 0)
        {
            transform.Translate(new Vector3(0, -speed * Time.deltaTime, 0));
        }


        float scrollData = Input.GetAxis("Mouse ScrollWheel");
        targetZoom -= scrollData * 10f;
        targetZoom = Mathf.Clamp(targetZoom, 3f,50f);
        Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, targetZoom, Time.deltaTime * 30);
    }
}
