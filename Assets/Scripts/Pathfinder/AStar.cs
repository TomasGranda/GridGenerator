using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStar : MonoBehaviour
{
    [Header("PathFinding")]

    [Space(10)]

    public AStarNode start;
    public AStarNode end;
    AStarNode oStar;
    AStarNode oEnd;
    public LayerMask nodeCustom;
    public List<AStarNode> pathNodes = new List<AStarNode>();
    List<AStarNode> openNodes = new List<AStarNode>();
    List<AStarNode> closedNodes = new List<AStarNode>();

    private void OnEnable()
    {
        GetPath(start, end);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        if (pathNodes.Count > 0)
        {
            for (int i = 0; i < pathNodes.Count - 1; i++)
            {
                Gizmos.DrawLine(pathNodes[i].transform.position, pathNodes[i + 1].transform.position);
            }
        }
    }

    public void GetPath(AStarNode start, AStarNode end)
    {
        foreach (AStarNode node in openNodes)
        {
            node.Reset();
        }

        foreach (AStarNode node in closedNodes)
        {
            node.Reset();
        }

        openNodes.Clear();
        closedNodes.Clear();
        this.pathNodes.Clear();

        openNodes.Add(start);

        start.g = 0;

        while (openNodes.Count > 0)
        {
            AStarNode current = SearchNextNode();

            closedNodes.Add(current);

            foreach (AStarNode node in current.neighbors)
            {
                if (closedNodes.Contains(node)) continue;

                if (!openNodes.Contains(node))
                {
                    openNodes.Add(node);

                    node.h = Mathf.Abs(end.transform.position.x - node.transform.position.x) + Mathf.Abs(end.transform.position.y - node.transform.position.y) + Mathf.Abs(end.transform.position.z - node.transform.position.z);
                }

                float distanceToNeighbor = Vector3.Distance(current.transform.position, node.transform.position);

                float newG = current.g + distanceToNeighbor;

                if (newG < node.g)
                {
                    node.g = newG;
                    node.f = node.g + node.h;
                    node.previous = current;
                }
            }

            if (openNodes.Contains(end))
            {
                break;
            }
        }

        int watchdog = 1000;

        AStarNode currentTheta = end;

        while (currentTheta)
        {
            if (watchdog-- == 0)
            {
                break;
            }

            if (currentTheta.previous != null && currentTheta.previous.previous != null)
            {
                var direction = currentTheta.previous.transform.position - currentTheta.transform.position;

                var hits = Physics.RaycastAll(currentTheta.transform.position, direction.normalized, 1, nodeCustom);

                List<RaycastHit> hitList = new List<RaycastHit>(hits);

                hitList.Sort((A, B) =>
                {
                    if (Vector3.Distance(A.point, currentTheta.transform.position) < Vector3.Distance(B.point, currentTheta.transform.position)) return 1;
                    else return -1;
                });

                bool canSee = false;

                foreach (var hit in hitList)
                {
                    var node = hit.collider.GetComponent<AStarNode>();

                    if (node != null)
                    {
                        if (node == currentTheta.previous.previous)
                        {
                            canSee = true;
                            break;
                        }
                        else
                        {
                            canSee = false;
                            break;
                        }
                    }
                }

                if (canSee)
                {
                    currentTheta.previous = currentTheta.previous.previous;
                }
                else
                {
                    currentTheta = currentTheta.previous;
                }
            }
            else
            {
                currentTheta = currentTheta.previous;
            }
        }

        watchdog = 100;

        this.pathNodes.Add(end);

        AStarNode pathNode = end.previous;

        while (pathNode)
        {
            this.pathNodes.Insert(0, pathNode);

            pathNode = pathNode.previous;
            if (watchdog-- <= 0)
            {
                Debug.Log("Te Pasaste");
            }
        }
    }
    public AStarNode SearchNextNode()
    {
        AStarNode node = openNodes[0];

        for (int i = 1; i < openNodes.Count; i++)
        {
            if (openNodes[i].f < node.f)
            {
                node = openNodes[i];
            }
        }

        openNodes.Remove(node);

        return node;
    }

}