using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeCameraAnimation : MonoBehaviour
{
    public float speed = 5.0f;
    public float length;
    public float start;
    public float end;

    // Update is called once per frame
    void Update()
    {
        //transform.position += new Vector3(transform.position.x + Mathf.PingPong(Time.time * speed, length), transform.position.y, transform.position.z);
        // transform.position = new Vector3(transform.position.x, transform.position.y, Mathf.PingPong(Time.time * speed, 26) - 13);
        transform.position = new Vector3(Mathf.PingPong(Time.time * speed, start) - end,
            transform.position.y, transform.position.z);
        
    }
}
