using System.Collections.Generic;

using UnityEngine;


[CreateAssetMenu(fileName = nameof(MyColorGridData),
        menuName = nameof(MyColorGridData))]
public class MyColorGridData : ScriptableObject
{
    public int CellsCount => CellStates.Count;

    [Range(0,30)]
    public int PointsXCount;

    [Range(0, 30)]
    public int PointsYCount;

    [SerializeField] public List<CellStateColor> CellStates;

    private void OnValidate()
    {
        PointsXCount = Mathf.Clamp(PointsXCount,0, 30);
        PointsYCount = Mathf.Clamp(PointsYCount, 0, 30);
    }
}

public enum CellStateColor
{
    Normal,
    Red,
    Green,
    Blue
}

