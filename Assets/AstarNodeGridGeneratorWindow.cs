using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class AstarNodeGridGeneratorWindow : EditorWindow
{
    private GridGenerator gridGenerator;
    private AStarNode slot;
    private int height;
    private int width;
    private Transform initialPosition;
    private float distanceBetweenNodes;
    private float radious;

    [MenuItem("Window/GridGenerator")]
    public static void OpenWindow()
    {
        GetWindow<AstarNodeGridGeneratorWindow>();

        AstarNodeGridGeneratorWindow myWindow = (AstarNodeGridGeneratorWindow)GetWindow(typeof(AstarNodeGridGeneratorWindow));
        myWindow.wantsMouseMove = true;
        myWindow.Show();
    }

    private void OnGUI()
    {
        InitialGenerateGrid();
    }

    void InitialGenerateGrid()
    {
        EditorGUILayout.LabelField("Setting Generador de Grid");

        height = EditorGUILayout.IntField("Height", height);

        width = EditorGUILayout.IntField("Width", width);

        distanceBetweenNodes = EditorGUILayout.FloatField("Distance Node", distanceBetweenNodes);

        initialPosition = (Transform)EditorGUILayout.ObjectField("Initial Position", initialPosition, typeof(Transform), true);

        EditorGUILayout.LabelField("Setting Node");

        radious = EditorGUILayout.FloatField("Radious", radious);

        if (GUILayout.Button("Generate Grid"))
        {
            Generate();
        }

        if (GUILayout.Button("Stop Generation"))
        {
            gridGenerator.StopGeneration();
        }
    }

    void Generate()
    {
        if (FindObjectOfType<GridGenerator>() == null)
        {
            gridGenerator = new GameObject("Grid Generator").AddComponent<GridGenerator>();
        }

        gridGenerator.heightSlot = height;
        gridGenerator.widthSlot = width;
        gridGenerator.distanceBetweenNodes = distanceBetweenNodes;
        gridGenerator.space = initialPosition;

        gridGenerator.CreateMaze();
    }
}
