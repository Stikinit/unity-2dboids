using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidManager : MonoBehaviour
{

    public Spawner spawner;
    public BoidSettings settings;

    private Boid[] boids;

    void Start()
    {
        if (!settings.useComputeShader) {
            spawner.Spawn(settings.boidNumber);
            boids = FindObjectsOfType<Boid>();
            foreach (Boid b in boids)
            {
                b.Initialize(); // INITIALIZE EVERY BOID
            }
        }
    }

    void FixedUpdate()
    {
        foreach (Boid b in boids)
        {
            // Calculate each movement vector based on the 3 boid rules
            List<Boid> flockmates = FindFlockmates(b);
            Vector2 avoidanceVector = AvoidanceDirection(b, flockmates);
            Vector2 alignmentVector = AlignmentDirection(b, flockmates);
            Vector2 comVector = CenterOfMassDirection(b, flockmates);

            Vector2 velocity = (
                settings.weightforward * b.GetComponent<Rigidbody2D>().velocity.normalized
                + settings.weightCohesion * comVector               // Cohesion component
                + settings.weightAvoidance * avoidanceVector        // Avoidance component
                + settings.weightAlignment * alignmentVector        // Alignment component
                ).normalized * settings.forwardSpeed;

            b.velocity = velocity;
            b.UpdateBoid();
        }
    }

    private List<Boid> FindFlockmates(Boid currentBoid) {
        List<Boid> flockmates = new List<Boid>();
        foreach (var boid in boids)
        {
            if (currentBoid.transform.position == boid.transform.position) {continue;}

            if (Vector2.Distance(currentBoid.transform.position, boid.transform.position) <= settings.viewRadius) {
                flockmates.Add(boid);
            }
        }
        return flockmates;
    }

    // ====================== RULE DEFINITIONS ====================== //

    private Vector2 AvoidanceDirection(Boid currentBoid, List<Boid> flockmates) {
        Vector2 avoidanceVector = Vector2.zero;
        foreach (var boid in flockmates)
        {
            if (Vector2.Distance(currentBoid.transform.position, boid.transform.position) < settings.avoidanceRadius) {
                avoidanceVector -= ((Vector2)boid.transform.position - (Vector2)currentBoid.transform.position);
            }
        }
        return avoidanceVector.normalized;
    }

    private Vector2 AlignmentDirection(Boid currentBoid, List<Boid> flockmates) {
        Vector2 avgVelocity = Vector2.zero;
        foreach (var boid in flockmates)
        {
            avgVelocity += boid.GetComponent<Rigidbody2D>().velocity;
        }

        if (flockmates.Count != 0) {
            avgVelocity /= flockmates.Count;
        }
        else {
            avgVelocity = currentBoid.GetComponent<Rigidbody2D>().velocity;
        }

        return (avgVelocity - (Vector2)currentBoid.GetComponent<Rigidbody2D>().velocity).normalized;
    }

    private Vector2 CenterOfMassDirection(Boid currentBoid, List<Boid> flockmates) {
        Vector2 COM = Vector2.zero;
        foreach (var boid in flockmates)
        {
            COM += (Vector2)boid.transform.position;
        }

        if (flockmates.Count != 0) {
            COM /= flockmates.Count;
        }
        else {
            COM = currentBoid.transform.position;
        }

        return (COM - (Vector2)currentBoid.transform.position).normalized;
    }
}
