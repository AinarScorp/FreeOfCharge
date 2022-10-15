using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float upDownSpeed = 3f;
    public float leftRightSpeed = 4f;

    void Update()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * moveSpeed, Space.World);

        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(Vector3.left * Time.deltaTime * upDownSpeed);
        }
        
        if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(Vector3.left * Time.deltaTime * upDownSpeed * -1);
        }
        
        if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(Vector3.forward * Time.deltaTime * leftRightSpeed);
        }
        
        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(Vector3.forward * Time.deltaTime * leftRightSpeed * -1);
        }
    }
}
