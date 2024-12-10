using Kdevaulo.GridPositionEditor;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(MyNumberGridData),
        menuName = nameof(MyNumberGridData))]
public class MyNumberGridData : ScriptableObject
{
    public int CellsCount => CellValues.Count;

    [Range(0,30)]
    public int PointsXCount;
    [Range(0, 30)]
    public int PointsYCount;

    public int MinValue;
    public int MaxValue;

    [SerializeField] public List<int> CellValues;

    private void OnValidate()
    {
        PointsXCount = Mathf.Clamp(PointsXCount, 0, 30);
        PointsYCount = Mathf.Clamp(PointsYCount, 0, 30);
    }
}
