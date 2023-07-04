using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This particular version of the BoidManager uses a compute shader to calculate
// the movement vectors of each boid, making use of the parallel computation of the GPU.

public class BoidManagerCompute : MonoBehaviour
{

    public Spawner spawner;
    public BoidSettings settings;
    public ComputeShader compute;
    public float threadGroupSize = 1024f;

    private Boid[] boids;


    void Start()
    {
        if (settings.useComputeShader) {
            spawner.Spawn(settings.boidNumber);
            boids = FindObjectsOfType<Boid>();
            foreach (Boid b in boids)
            {
                b.Initialize(); // INITIALIZE EVERY BOID
            }
        }
    }

    void Update()
    {
        int numBoids = boids.Length;
        var boidData = new BoidData[numBoids];

        // SETUP THE COMPUTE SHADER
        for (int i = 0; i < numBoids; i++) {
            boidData[i].position = boids[i].transform.position;
            boidData[i].velocity = boids[i].GetComponent<Rigidbody2D>().velocity;
            boidData[i].avoidanceVector = Vector2.zero;
            boidData[i].alignmentVector = Vector2.zero;
            boidData[i].comVector = Vector2.zero;
            boidData[i].numFlockmates = 0;
        }

        var boidBuffer = new ComputeBuffer(numBoids, BoidData.Size);
        boidBuffer.SetData(boidData);

        compute.SetBuffer(0, "boids", boidBuffer);
        compute.SetInt("numBoids", boids.Length);
        compute.SetFloat("viewRadius", settings.viewRadius);
        compute.SetFloat("avoidRadius", settings.avoidanceRadius);

        int threadGroups = Mathf.CeilToInt (numBoids / (float) threadGroupSize);
        compute.Dispatch (0, threadGroups, 1, 1); // START THE COMPUTE SHADER

        boidBuffer.GetData(boidData);

        // CALCULATE FINAL VELOCITY VECTOR BASED ON THE COMPUTE SHADER RESULTS
        for(int i = 0; i < numBoids; i++)
        {
            Debug.Log(boidData[i].velocity + " " + boidData[i].alignmentVector);
            Vector2 velocity = (
            settings.weightforward * boidData[i].velocity.normalized
            + settings.weightCohesion * boidData[i].comVector.normalized            // Cohesion component
            + settings.weightAvoidance * boidData[i].avoidanceVector.normalized     // Avoidance component
            + settings.weightAlignment * boidData[i].alignmentVector.normalized     // Alignment component
            ).normalized * settings.forwardSpeed;

            boids[i].velocity = velocity;
            boids[i].UpdateBoid();
        }

        boidBuffer.Release();
    }

    public struct BoidData {
        public Vector2 position;
        public Vector2 velocity;
        public Vector2 comVector;
        public Vector2 avoidanceVector;
        public Vector2 alignmentVector;
        public int numFlockmates;

        public static int Size {
            get {
                return sizeof (float) * 2 * 5 + sizeof (int);
            }
        }
    }
}
