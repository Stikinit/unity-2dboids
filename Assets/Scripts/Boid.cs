using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boid : MonoBehaviour
{

    public BoidSettings settings;
    public Rigidbody2D rb2D;
    [HideInInspector]
    public Vector2 velocity;


    //  INITIALIZE EACH BOID WITH A CERTAIN STARTING SPEED
    public void Initialize() 
    {
        velocity = transform.up * settings.forwardSpeed;
        rb2D.velocity = velocity;
    }

    //  UPDATE THE BOID'S MOVEMENT VECTORS
    public void UpdateBoid() 
    {
        CalculateVelocity();
        FaceFront();
    }

    void CalculateVelocity()
    {
        rb2D.velocity = velocity;
        HandleWallCollision();
    }

    //  ROTATE THE VOID TOWARDS ITS CURRENT VELOCITY
    void FaceFront() 
    {
        float step = Time.fixedDeltaTime * settings.turnSpeed;
        Vector3 newDir = Vector3.RotateTowards(transform.up, rb2D.velocity, step, 0);

        float zOffset = Vector2.SignedAngle(transform.up, newDir);

        transform.Rotate(Vector3.forward, zOffset);
    }

    //  AVOID WALLS BY STEERING THE BOID THE OPPOSITE WAY
    void HandleWallCollision()
    {
        Vector2 escapeVector = BoidUtils.CalculateEscapeVector(transform, settings.avoidWallRadius, settings.avoidWallSteerForce);

        rb2D.velocity += escapeVector; // STEER OFF THE WALLS
    }
}
