using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Test : MonoBehaviour
{
    [SerializeField] private int[,] twoDArray;
    [SerializeField] private List<int> lists;
    public int row;
    public int col;

    private void Start()
    {
        twoDArray = new int[row, col];
        for (int i = 0; i< row*col; i++)
        {
            int ii = i / col;
            int jj = i % col;

            twoDArray[ii, jj] = lists[i];

            Debug.Log($"({ii},{jj}) = {twoDArray[ii, jj]}");
        }
    }
}
