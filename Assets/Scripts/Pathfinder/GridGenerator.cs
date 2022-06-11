using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    private List<AStarNode> maze = new List<AStarNode>();
    [HideInInspector]
    public int heightSlot;
    [HideInInspector]
    public int widthSlot;
    [HideInInspector]
    public float distanceBetweenNodes;
    [HideInInspector]
    public Transform space;

    public float slotRadious = 0.5f;

    //Cuando se presiona "Generate Maze"
    public void CreateMaze()
    {
        StartCoroutine(Maze());
    }

    public void StopGeneration()
    {
        StopCoroutine(CreateEmptyMaze(heightSlot, widthSlot));
    }

    IEnumerator Maze()
    {
        //Limpiamos la grilla previa
        foreach (var item in maze)
        {
            yield return new WaitForEndOfFrame();
            Destroy(item.gameObject);
        }
        maze.Clear();
        //Creamos una nueva grilla
        StartCoroutine(CreateEmptyMaze(heightSlot, widthSlot));
    }

    private AStarNode createNode()
    {
        var node = Instantiate(new GameObject());
        AStarNode aStarNode = node.AddComponent<AStarNode>();

        var rg = node.AddComponent<Rigidbody>();
        rg.useGravity = false;

        var col = node.AddComponent<SphereCollider>();
        col.isTrigger = true;
        col.radius = slotRadious;

        return aStarNode;
    }


    IEnumerator CreateEmptyMaze(int x, int y)
    {
        //Creacion de la nueva grilla
        float xBase = 0;
        float zBase = 0;
        float yBase = 0;

        if (space != null)
        {
            xBase = space.position.x;
            zBase = space.position.z;
            yBase = space.position.y;
        }

        int id = 0;

        for (int i = 0; i < x; i++)
        {
            for (int j = 0; j < y; j++)
            {
                AStarNode currentNode = createNode();

                if (space != null)
                    currentNode.transform.parent = space;

                currentNode.transform.position = new Vector3(xBase + j * distanceBetweenNodes, yBase, zBase + i * distanceBetweenNodes);
                currentNode.id = id;

                currentNode.pos[0] = j;
                currentNode.pos[1] = i;

                maze.Add(currentNode);

                Debug.Log("Generating Slots: " + ((int)currentNode.id * 100 / (heightSlot * widthSlot)) + "%");

                id++;
                yield return new WaitForEndOfFrame();
            }
        }


        Debug.Log("Generating Slots: 100%");

        RemoveUnAvaiableNodes();
        CreateNeighbourhood();
    }

    void RemoveUnAvaiableNodes()
    {
        var nodesToRemove = new List<AStarNode>();
        for (var i = 0; i < maze.Count; i++)
        {
            var slot = maze[i];
            if (!slot.isAvaiable)
            {
                Destroy(slot.gameObject);
                nodesToRemove.Add(slot);
            }

            Debug.Log("Removing Unavaiable Slots: " + ((int)slot.id * 100 / (heightSlot * widthSlot)) + "%");
        }

        Debug.Log("Removing Unavaiable Slots: 100%");
        maze.RemoveAll((s) => nodesToRemove.Contains(s));
    }

    void CreateNeighbourhood()
    {

        foreach (var sl in maze)
        {

            if (sl.pos[0] != 0)
            {
                var slot = maze.Find((slot) => slot.pos[0] == sl.pos[0] - 1 && slot.pos[1] == sl.pos[1]);
                if (slot != null)
                    sl.neighbors.Add(slot);
            }

            if (sl.pos[1] != 0)
            {
                var slot = maze.Find((slot) => slot.pos[0] == sl.pos[0] && slot.pos[1] == sl.pos[1] - 1);
                if (slot != null)
                    sl.neighbors.Add(slot);
            }

            if (sl.pos[1] != heightSlot - 1)
            {
                var slot = maze.Find((slot) => slot.pos[0] == sl.pos[0] && slot.pos[1] == sl.pos[1] + 1);
                if (slot != null)
                    sl.neighbors.Add(slot);
            }

            if (sl.pos[0] != widthSlot - 1)
            {
                var slot = maze.Find((slot) => slot.pos[0] == sl.pos[0] + 1 && slot.pos[1] == sl.pos[1]);
                if (slot != null)
                    sl.neighbors.Add(slot);
            }

            Debug.Log("Setting Neighbors: " + ((int)sl.id * 100 / (heightSlot * widthSlot)) + "%");
        }

        Debug.Log("Setting Neighbors: 100%");

    }
}
