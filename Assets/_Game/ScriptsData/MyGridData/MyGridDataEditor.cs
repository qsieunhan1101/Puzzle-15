using Kdevaulo.GridPositionEditor;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;

[CustomEditor(typeof(MyGridData))]
public class MyGridDataEditor : Editor
{
    private SerializedProperty _arrayWidth;
    private SerializedProperty _arrayHeight;
    private MyGridData myGridData;

    private const float FixedMargin = 35f;

    private readonly Color NormalColor = new Color(0.4f, 0.4f, 0.4f);
    private readonly Color RedColor = new Color(1f, 0f, 0f);
    private readonly Color GreenColor = new Color(0f, 1f, 0f);
    private readonly Color BlueColor = new Color(0f, 0f, 1f);

    private Vector2 _scrollPosition;

    public override void OnInspectorGUI()
    {
        _arrayWidth = serializedObject.FindProperty("PointsXCount");
        _arrayHeight = serializedObject.FindProperty("PointsYCount");

        EditorGUILayout.PropertyField(_arrayWidth);
        EditorGUILayout.PropertyField(_arrayHeight);

        myGridData = (MyGridData) target;
        Assert.IsNotNull(myGridData);
        
        if (myGridData.CellStates == null || myGridData.CellStates.Count == 0)
        {
            myGridData.CellStates = new List<CellStateColor>();
            for (int i = 0; i < _arrayWidth.intValue * _arrayHeight.intValue; i++)
            {
                myGridData.CellStates.Add(CellStateColor.Normal);
            }
        }


        EnsureCellStatesAreInitialized();

        if (GUILayout.Button("Print Selected Cells"))
        {
            PrintCells();
        }

        if (GUILayout.Button("Clear Cells"))
        {
            ClearCells();
        }

        float totalWidth = EditorGUIUtility.currentViewWidth - FixedMargin;
        float buttonSize = totalWidth / _arrayWidth.intValue - EditorGUIUtility.standardVerticalSpacing * 1.5f;

        _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition, GUIStyle.none, GUIStyle.none);

        for (int y = _arrayHeight.intValue - 1; y >= 0; y--)
        {
            EditorGUILayout.BeginHorizontal();

            for (int x = 0; x < _arrayWidth.intValue; x++)
            {
                int index = y * _arrayWidth.intValue + x;
                CellStateColor currentState = myGridData.CellStates[index];
                Color cellColor = GetCellColor(currentState);

                if (GUILayout.Button("", EditorStyles.label, GUILayout.Width(buttonSize), GUILayout.Height(buttonSize)))
                {
                    myGridData.CellStates[index] = GetNextCellState(currentState);
                    EditorUtility.SetDirty(target);
                }

                EditorGUI.DrawRect(GUILayoutUtility.GetLastRect(), cellColor);
            }

            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.EndScrollView();

        myGridData.PointsXCount = _arrayWidth.intValue;
        myGridData.PointsYCount = _arrayHeight.intValue;
    }

    private void EnsureCellStatesAreInitialized()
    {
        int requiredCount = _arrayWidth.intValue * _arrayHeight.intValue;

        if (myGridData.CellStates == null || myGridData.CellStates.Count != requiredCount)
        {
            myGridData.CellStates = new List<CellStateColor>(requiredCount);

            for (int i = 0; i < requiredCount; i++)
            {
                myGridData.CellStates.Add(CellStateColor.Normal);
                EditorUtility.SetDirty(target);
            }
        }
    }

    private Color GetCellColor(CellStateColor state)
    {
        switch (state)
        {
            case CellStateColor.Red:
                return RedColor;
            case CellStateColor.Green:
                return GreenColor;
            case CellStateColor.Blue:
                return BlueColor;
            default:
                return NormalColor;
        }
    }

    private CellStateColor GetNextCellState(CellStateColor currentState)
    {
        switch (currentState)
        {
            case CellStateColor.Normal:
                return CellStateColor.Red;
            case CellStateColor.Red:
                return CellStateColor.Green;
            case CellStateColor.Green:
                return CellStateColor.Blue;
            case CellStateColor.Blue:
                return CellStateColor.Normal;
            default:
                return CellStateColor.Normal;
        }
    }


    private void PrintCells()
    {
        Debug.Log(nameof(PrintCells));

        for (int i = 0; i < myGridData.CellStates.Count; i++)
        {
            CellStateColor state = myGridData.CellStates[i];
            int x = i % _arrayWidth.intValue;
            int y = i / _arrayWidth.intValue;
            Debug.Log($"Cell ({x}, {y}) has state: {state}");
        }

        Debug.Log($"Total cells: {myGridData.CellStates.Count}");
    }

    private void ClearCells()
    {
        Debug.Log(nameof(ClearCells));

        for (int i = 0; i < myGridData.CellStates.Count; i++)
        {
            myGridData.CellStates[i] = CellStateColor.Normal;
        }

        EditorUtility.SetDirty(target);
    }
}
