using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BoidSettings", menuName = "ScriptableObjects/BoidSettings", order = 1)]
public class BoidSettings : ScriptableObject
{
    public int boidNumber = 200;
    public float viewRadius = 2.5f;
    public float avoidanceRadius = 2;
    public float weightforward = 10;
    public float weightCohesion = 1.2f;
    public float weightAvoidance = 5;
    public float weightAlignment = 1.5f;
    public float forwardSpeed = 20;
    public float turnSpeed = 10;
    public float avoidWallRadius = 7;
    public float avoidWallSteerForce = 10;
    public bool useComputeShader = true;
}
