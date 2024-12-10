using System.Collections.Generic;
using UnityEngine;

public class PipeManager : MonoBehaviour
{
    [SerializeField] PipeItem[] pipeItemPrefabs;
    [SerializeField] Transform pipeParent;
    [SerializeField] MyNumberGridData numberGridData;
    [SerializeField] List<PipeItem> listPipeRight;
    private void Start()
    {
        Init();
    }
    private void Init()
    {
        SpawnPipe();
    }

    private void SpawnPipe()
    {
        for (int y = 0; y < numberGridData.PointsYCount; y++)
        {
            for (int x = 0; x < numberGridData.PointsXCount; x++)
            {
                int index = y * numberGridData.PointsXCount + x;
                int currentValue = numberGridData.CellValues[index];
                PipeItem pipeItem = Instantiate(pipeItemPrefabs[currentValue], pipeParent);
                pipeItem.transform.localPosition = new Vector2(x, -y);
                pipeItem.checkWinEvent = CheckWin;
                if (currentValue != 0)
                {
                    listPipeRight.Add(pipeItem);
                }
                
            }
        }
        listPipeRight[0].GetComponent<SpriteRenderer>().enabled = true;
        listPipeRight[listPipeRight.Count-1].GetComponent<SpriteRenderer>().enabled = true;
    }

    private void CheckWin()
    {
        int countRight = 0;
        for (int i = 0; i < listPipeRight.Count; i++)
        {
            if (listPipeRight[i].IsRightAngle == true)
            {
                countRight++;
            }
            else
            {
                return;
            }
        }
        if (countRight == listPipeRight.Count)
        {
            Debug.Log("Win Game Pipe");
        }
    }

}
