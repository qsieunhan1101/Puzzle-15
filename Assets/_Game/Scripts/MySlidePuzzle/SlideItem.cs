using System;
using System.Collections;
using UnityEngine;

public class SlideItem : MonoBehaviour
{
    [SerializeField] private SpriteRenderer itemSprite = null;
    [SerializeField] private int index = 0;
    [SerializeField] private int i = 0;
    [SerializeField] private int j = 0;
    [SerializeField] private bool isEmptyItem = false;
    [SerializeField] private bool isRightPos;
    private float posX = 0;
    private float posY = 0;
    private Action<int, int> swapEvent = null;
    private int n;
    private int m;

    public int Index => index;
    public bool IsEmptyItem => isEmptyItem;
    public bool IsRightPos => isRightPos;
    public int I => i;
    public int J => j;

    private void OnMouseDown()
    {
        if (Input.GetMouseButtonDown(0))
        {
            swapEvent?.Invoke(i, j);
        }
    }

    public void Init(int index, int i, int j, Sprite sprite, Action<int, int> swapEvent, int n, int m)
    {
        this.index = index;
        this.i = i;
        this.j = j;
        itemSprite.sprite = sprite;
        this.swapEvent = swapEvent;
        this.n = n;
        this.m = m;
        SetUpPos(j, -i);
    }
    public void SetEmpty()
    {
        this.isEmptyItem = true;
    }

    public void SetSprite(Sprite sprite)
    {
        itemSprite.sprite = sprite;
    }
    public void SetUpPos(float x, float y)
    {
        posX = x;
        posY = y;
        this.gameObject.transform.localPosition = new Vector2(posX, posY);
    }

    public void SwapRowAndColumn(SlideItem target)
    {
        i = target.i;
        j = target.j;
    }
    public void CheckRightPos()
    {
        int checkNumber = i * m + j;   //i * hang + j
        if (checkNumber == index - 1)
        {
            isRightPos = true;
        }
        else
        {
            isRightPos = false;
        }

    }
}
