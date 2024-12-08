using System.Collections.Generic;

using UnityEngine;


[CreateAssetMenu(fileName = nameof(MyGridData),
        menuName = "MyGridDataEditor")]
public class MyGridData : ScriptableObject
{
    public int CellsCount => CellStates.Count;

    public int PointsXCount;
    public int PointsYCount;

    [SerializeField] public List<CellStateColor> CellStates;
}

public enum CellStateColor
{
    Normal,
    Red,
    Green,
    Blue
}

