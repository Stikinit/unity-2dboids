using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static class BoidUtils
{
    static Vector2[] cardinals = new Vector2[]{Vector2.up, Vector2.down, Vector2.left, Vector2.right};
    static public Vector2 CalculateEscapeVector(Transform transform, float collisionRadius, float steerForce)
    {
        Vector2 escapeV = Vector2.zero;
        RaycastHit2D hit;
        float distanceWeight = 0f;

        foreach (var c in cardinals)
        {
            hit = Physics2D.Raycast(transform.position, c);
            if(hit && 
                Vector2.Distance(transform.position, hit.point) <= collisionRadius &&
                hit.collider.gameObject.tag == "Wall") {

                distanceWeight = (1 - Vector2.Distance(transform.position, hit.point)/collisionRadius) * steerForce;
                escapeV -= (hit.point - (Vector2)transform.position).normalized * distanceWeight;
            }
        }

        return escapeV;
    }
}
