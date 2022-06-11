using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarNode : MonoBehaviour
{
    public float g;
    public float h;
    public float f;
    public float radious = .5f;
    public List<AStarNode> neighbors = new List<AStarNode>();
    public AStarNode previous;
    public int id;
    public float[] pos = new float[2];

    public bool isAvaiable = true;

    public void Reset()
    {
        g = Mathf.Infinity;
        previous = null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radious);
    }

    private void OnTriggerEnter(Collider other)
    {
        isAvaiable = false;
    }
}