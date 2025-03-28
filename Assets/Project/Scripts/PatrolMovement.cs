using System.Collections;
using System.Collections.Generic;   
using UnityEditor.Rendering;
using UnityEngine;

public class PatrolMovement : MonoBehaviour
{
    public Transform Pos1, Pos2;
    public float speed;

    Transform origin, objective;

    Vector3 velocity = Vector3.zero;

    // Start is called before the first frame update
    private void Start()
    {
        origin = Pos1;
        objective = Pos2;
        SetVelocity();
    }

    private void FixedUpdate()
    {
        Vector3 selfToObj = objective.position - this.transform.position;
        Vector3 movementVec = objective.position - origin.position;
        if (Vector2.Dot(selfToObj, movementVec) < 0)
        {
            Transform temp = objective;
            objective = origin;
            origin = temp;
            SetVelocity();
        }
    }

    private void Update()
    {
        this.transform.position += velocity * Time.deltaTime;
    }

    void SetVelocity()
    {
        velocity = (objective.position - origin.position).normalized * speed;
    }
}