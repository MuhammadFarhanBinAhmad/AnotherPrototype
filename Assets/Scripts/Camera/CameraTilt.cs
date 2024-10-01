using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTilt : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float minRotation;
    [SerializeField] private float maxRotation;

    private float zRotation;


    void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            zRotation += speed * Time.deltaTime;
            //transform.Rotate(rotation * speed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            zRotation -= speed * Time.deltaTime;
            //transform.Rotate(-rotation * speed * Time.deltaTime);
        }
        else
        {
            if (zRotation <= 0)
            {
                zRotation += speed * Time.deltaTime;
            }
            if (zRotation >= 0)
            {
                zRotation -= speed * Time.deltaTime;
            }
        }

        zRotation = Mathf.Clamp(zRotation, minRotation, maxRotation);

        transform.localRotation = Quaternion.Euler(0, 0, zRotation);
    }
}
