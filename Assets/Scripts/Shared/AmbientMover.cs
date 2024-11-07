using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AmbientMover : MonoBehaviour
{
    public GameObject[] waypoints;
    public Vector3 targetPosition;
    public float moveSpeed;

    // Start is called before the first frame update
    void Start()
    {
        AssignNewTarget();
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position != targetPosition)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        }
        else
        {
            AssignNewTarget();
        }
    }

    public void AssignNewTarget()
    {
        GameObject waypoint = waypoints[Random.Range(0, waypoints.Length)];
        targetPosition = waypoint.transform.position;
    }
}
