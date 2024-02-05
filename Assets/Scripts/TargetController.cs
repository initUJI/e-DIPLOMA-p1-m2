using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetController : MonoBehaviour
{
    public enum actionType { Rotate, Move };
    public actionType action = actionType.Rotate;
    public Quaternion desiredRotation;
    public Vector3 desiredPosition;
    public GameObject associatedObject;

    private bool rotating = false;
    private bool moving = false;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Arrow") && action == actionType.Rotate)
            rotating = true;
        if (collision.gameObject.CompareTag("Arrow") && action == actionType.Move)
            moving = true;
    }

    private void Update()
    {
        if (rotating) Rotate();
        if (moving) Move();
    }

    public void Rotate()
    {
        associatedObject.transform.rotation = Quaternion.RotateTowards(associatedObject.transform.rotation, desiredRotation, 200f * Time.deltaTime);
    }

    public void Move()
    {
        associatedObject.transform.position = Vector3.MoveTowards(associatedObject.transform.position, desiredPosition, 5f * Time.deltaTime);
    }
}
