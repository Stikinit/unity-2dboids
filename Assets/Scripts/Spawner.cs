using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

    public enum GizmoType { Never, SelectedOnly, Always }

    public Boid prefab;
    public float spawnRadius = 10;
    [HideInInspector]
    public int spawnCount = 10;
    public Color colour;
    public GizmoType showSpawnRegion;

    public void Spawn() {
        for (int i = 0; i < spawnCount; i++) {
            Vector2 pos = transform.position + Random.insideUnitSphere * spawnRadius;
            Boid boid = Instantiate (prefab);
            boid.transform.position = pos;
            boid.transform.eulerAngles = new Vector3(0,0,Random.Range(0, 360));

            //boid.SetColour (colour);
        }
    }
    public void Spawn(int num) {
        spawnCount = num;
        for (int i = 0; i < spawnCount; i++) {
            Vector2 pos = transform.position + Random.insideUnitSphere * spawnRadius;
            Boid boid = Instantiate (prefab);
            boid.transform.position = pos;
            boid.transform.eulerAngles = new Vector3(0,0,Random.Range(0, 360));

            //boid.SetColour (colour);
        }
    }

    private void OnDrawGizmos () {
        if (showSpawnRegion == GizmoType.Always) {
            DrawGizmos ();
        }
    }

    void OnDrawGizmosSelected () {
        if (showSpawnRegion == GizmoType.SelectedOnly) {
            DrawGizmos ();
        }
    }

    void DrawGizmos () {

        Gizmos.color = new Color (colour.r, colour.g, colour.b, 0.3f);
        Gizmos.DrawSphere (transform.position, spawnRadius);
    }

}
