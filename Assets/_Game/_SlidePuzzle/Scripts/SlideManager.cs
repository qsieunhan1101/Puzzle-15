using System.Collections;
using UnityEngine;

public class SlideManager : MonoBehaviour
{
    [SerializeField] private int n;
    [SerializeField] private int m;
    [SerializeField] private SlideItem slideItemPrefabs;
    [SerializeField] private Sprite[] listSprites;

    [SerializeField] private SlideItem prototypeSlideItem;
    private int iMax = 0;
    private int jMax = 0;
    private SlideItem[,] slidesMatrix;
    private bool isPlay = false;

    private void Start()
    {
        isPlay = false;
        Init();
        Shuffle();
        isPlay = true;
    }
    private void Init()
    {
        isPlay = false;
        slidesMatrix = new SlideItem[n, m];
        iMax = n;
        jMax = m;
        int count = 0;
        for (int i = 0; i < iMax; i++)
        {
            for (int j = 0; j < jMax; j++)
            {
                SlideItem slideItem = Instantiate(slideItemPrefabs, transform);
                slideItem.gameObject.name = $"{count + 1}";
                slidesMatrix[i, j] = slideItem;
                slideItem.Init(count + 1, i, j, listSprites[count], ClickToSwap, n, m);
                if (count + 1 == n * m)
                {
                    slideItem.SetEmpty();
                    slideItem.SetSprite(listSprites[listSprites.Length - 1]);
                }
                count++;
            }
        }
    }

    private void ClickToSwap(int i, int j)
    {
        Swap(slidesMatrix[i, j], GetEmptySlideItem(i, j));
    }
    private void Swap(SlideItem a, SlideItem b)
    {
        SlideItem temp = prototypeSlideItem;
        //doi gia tri hang va cot (i va j)
        temp.SwapRowAndColumn(a);
        a.SwapRowAndColumn(b);
        b.SwapRowAndColumn(temp);
        //doi cho trong matran
        temp = slidesMatrix[a.I, a.J];
        slidesMatrix[a.I, a.J] = slidesMatrix[b.I, b.J];
        slidesMatrix[b.I, b.J] = temp;
        //doi position hien thi tren scene
        a.CheckRightPos();
        b.CheckRightPos();
        if (isPlay == true)
        {
            temp = a;
            StartCoroutine(SwapMove(a, b));
            StartCoroutine(SwapMove(b, temp));
            if (CheckWin() == true)
            {
                Debug.Log("Win game");
            }
        }
        else
        {
            Vector2 tempPos = a.transform.localPosition;
            a.transform.localPosition = b.transform.localPosition;
            b.transform.localPosition = tempPos;

        }
    }

    private IEnumerator SwapMove(SlideItem a, SlideItem b)
    {
        float elapsedTime = 0;
        float duration = 0.1f;
        Vector2 star = a.transform.localPosition;
        Vector2 end = b.transform.localPosition;
        while (elapsedTime < duration)
        {
            a.transform.localPosition = Vector2.Lerp(star, end, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        a.transform.localPosition = end;
    }

    private SlideItem GetEmptySlideItem(int i, int j)
    {
        if (i - 1 >= 0 && slidesMatrix[i - 1, j].IsEmptyItem)
        {
            return slidesMatrix[i - 1, j];
        }
        if (i + 1 < iMax && slidesMatrix[i + 1, j].IsEmptyItem)
        {
            return slidesMatrix[i + 1, j];
        }
        if (j - 1 >= 0 && slidesMatrix[i, j - 1].IsEmptyItem)
        {
            return slidesMatrix[i, j - 1];
        }
        if (j + 1 < jMax && slidesMatrix[i, j + 1].IsEmptyItem)
        {
            return slidesMatrix[i, j + 1];
        }
        return slidesMatrix[i, j];
    }

    private void Shuffle()
    {
        for (int i = 0; i < 300; i++)
        {
            int ranI = Random.Range(0, iMax);
            int ranJ = Random.Range(0, jMax);
            Swap(slidesMatrix[ranI, ranJ], GetEmptySlideItem(ranI, ranJ));
        }
    }
    private bool CheckWin()
    {
        int countRight = 0;
        for (int i = 0; i < iMax; i++)
        {
            for (int j = 0; j < jMax; j++)
            {

                if (slidesMatrix[i, j].IsRightPos == true)
                {
                    countRight++;
                }
            }
        }
        if (countRight == n * m)
        {
            return true;
        }
        return false;
    }
}