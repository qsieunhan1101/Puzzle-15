using Kdevaulo.GridPositionEditor;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;

[CustomEditor(typeof(MyNumberGridData))]
public class MyNumberDridDataEditor : Editor
{

    private SerializedProperty _arrayWidth;
    private SerializedProperty _arrayHeight;
    private SerializedProperty _minValue;
    private SerializedProperty _maxValue;
    private SerializedProperty _listCellValues;
    private MyNumberGridData myNumberGrid;
    private List<int> _cellValues;

    private const float FixedMargin = 35f;

    private readonly Color NormalColor = new Color(0.4f, 0.4f, 0.4f);

    private Vector2 _scrollPosition;

    public override void OnInspectorGUI()
    {
        _arrayWidth = serializedObject.FindProperty("PointsXCount");
        _arrayHeight = serializedObject.FindProperty("PointsYCount");
        _minValue = serializedObject.FindProperty("MinValue");
        _maxValue = serializedObject.FindProperty("MaxValue");

        EditorGUILayout.PropertyField(_arrayWidth);
        EditorGUILayout.PropertyField(_arrayHeight);
        EditorGUILayout.PropertyField(_minValue);
        EditorGUILayout.PropertyField(_maxValue);


        _listCellValues = serializedObject.FindProperty("CellValues");
        EditorGUILayout.PropertyField(_listCellValues);

        myNumberGrid = (MyNumberGridData) target;
        Assert.IsNotNull(myNumberGrid);

        if (myNumberGrid.CellValues == null || myNumberGrid.CellValues.Count == 0)
        {   
            myNumberGrid.CellValues = new List<int>();
            for (int i = 0; i < _arrayWidth.intValue * _arrayHeight.intValue; i++)
            {
                myNumberGrid.CellValues.Add(0);
            }
        }

        EnsureCellValuesAreInitialized();

        _cellValues = myNumberGrid.CellValues;

        if (GUILayout.Button("Print Cells"))
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

        for (int y = 0; y < _arrayHeight.intValue; y++) // loop cux (int y = _arrayHeight.intValue - 1; y >= 0; y--), int y = 0; y < _arrayHeight.intValue; y++
        {
            EditorGUILayout.BeginHorizontal();

            for (int x = 0; x < _arrayWidth.intValue; x++) //int x = 0; x < _arrayWidth.intValue; x++
            {
                int index = y * _arrayWidth.intValue + x;
                int currentValue = _cellValues[index];

                Rect textFieldRect = GUILayoutUtility.GetRect(GUIContent.none, EditorStyles.label, GUILayout.Width(buttonSize), GUILayout.Height(buttonSize));
                string inputValue = EditorGUI.TextField(textFieldRect, currentValue.ToString());

                if (inputValue != currentValue.ToString() && int.TryParse(inputValue, out int newValue))
                {
                    _cellValues[index] = Mathf.Clamp(newValue, _minValue.intValue, _maxValue.intValue); 
                    EditorUtility.SetDirty(target); 
                    Repaint(); 
                }

            }

            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.EndScrollView();

        myNumberGrid.PointsXCount = _arrayWidth.intValue;
        myNumberGrid.PointsYCount = _arrayHeight.intValue;
        myNumberGrid.MinValue = _minValue.intValue;
        myNumberGrid.MaxValue = _maxValue.intValue;
    }

    private void EnsureCellValuesAreInitialized()
    {
        int requiredCount = _arrayWidth.intValue * _arrayHeight.intValue;

        if (myNumberGrid.CellValues == null || myNumberGrid.CellValues.Count != requiredCount)
        {
            myNumberGrid.CellValues = new List<int>(requiredCount);

            for (int i = 0; i < requiredCount; i++)
            {
                if (myNumberGrid.CellValues.Count <= i)
                {
                    myNumberGrid.CellValues.Add(0);
                }
                else
                {
                    myNumberGrid.CellValues[i] = 0; 
                }
            }

            EditorUtility.SetDirty(target);
        }
    }


    private void PrintCells()
    {
        Debug.Log(nameof(PrintCells));

        for (int i = 0; i < myNumberGrid.CellValues.Count; i++)
        {
            int value = myNumberGrid.CellValues[i];
            int x = i % _arrayWidth.intValue;
            int y = i / _arrayWidth.intValue;
            Debug.Log($"Cell ({x}, {y}) has value: {value}");
        }

        Debug.Log($"Total cells: {myNumberGrid.CellValues.Count}");
    }

    private void ClearCells()
    {
        Debug.Log(nameof(ClearCells));

        for (int i = 0; i < myNumberGrid.CellValues.Count; i++)
        {
            myNumberGrid.CellValues[i] = 0;
        }

        EditorUtility.SetDirty(target);
        Repaint();
    }
}
