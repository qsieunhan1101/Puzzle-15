using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwoDotsManager : MonoBehaviour
{
    [SerializeField] private int dotNummber;
    [SerializeField] private Transform circle_Left;
    [SerializeField] private Transform circle_Right;
    [SerializeField] private ButtonRotate btnLeft;
    [SerializeField] private ButtonRotate btnRight;
    [SerializeField] private float circleRadius;
    [SerializeField] private float alpha;
    [SerializeField] private DotItem dotPrefabs;

    [SerializeField] private List<DotItem> listSharedDotItem;

    [SerializeField] private Queue<DotItem> leftCircleQueue;
    [SerializeField] private Queue<DotItem> rightCircleQueue;

    private bool isRotate = false;
    private void Start()
    {
        Init();
    }
    private void Init()
    {
        alpha = 360 / dotNummber;
        isRotate = false;
        listSharedDotItem = new List<DotItem>();
        leftCircleQueue = new Queue<DotItem>();
        rightCircleQueue = new Queue<DotItem>();
        SpawnDots();
        btnLeft.rotateEvent = RotateEvent;
        btnRight.rotateEvent = RotateEvent;
        btnLeft.transform.position = circle_Left.position;
        btnRight.transform.position = circle_Right.position;
    }
    private void CheckWin()
    {
        int countShared = 0;
        int leftDotRightCount = 0;
        for (int i = 0; i < listSharedDotItem.Count; i++)
        {
            if (listSharedDotItem[i].CurrentColor != DotColor.White)
            {
                return;
            }
            countShared++;
        }
        int leftQueueCount = leftCircleQueue.Count;
        foreach (DotItem item in leftCircleQueue)
        {
            if (item.CurrentColor != DotColor.Red)
            {
                return;
            }
            leftDotRightCount++;
        }
        if (countShared == 2 && leftDotRightCount == leftQueueCount)
        {
            Debug.Log("Win Game");
        }
    }
    private void RotateEvent(ButtonRotateType rotateType)
    {
        StartCoroutine(RotateCircle(rotateType));
    }
    private IEnumerator RotateCircle(ButtonRotateType rotateType)
    {
        float newAngle = 0;
        float elapsedTime = 0;
        float duration = 0.2f;
        if (isRotate == false)
        {
            isRotate = true;
            if (rotateType == ButtonRotateType.Left)
            {
                SetDotSharedParent(rotateType);
                float currentAngle = circle_Left.transform.eulerAngles.z;
                newAngle = currentAngle - alpha;
                while (elapsedTime < duration)
                {
                    circle_Left.transform.eulerAngles = new Vector3(0, 0, Mathf.Lerp(currentAngle, newAngle, elapsedTime / duration));
                    elapsedTime += Time.deltaTime;
                    yield return null;
                }
                circle_Left.transform.eulerAngles = new Vector3(0, 0, newAngle);
            }
            if (rotateType == ButtonRotateType.Right)
            {
                SetDotSharedParent(rotateType);
                float currentAngle = circle_Right.transform.eulerAngles.z;
                newAngle = currentAngle + alpha;
                while (elapsedTime < duration)
                {
                    circle_Right.transform.eulerAngles = new Vector3(0, 0, Mathf.Lerp(currentAngle, newAngle, elapsedTime / duration));
                    elapsedTime += Time.deltaTime;
                    yield return null;
                }
                circle_Right.transform.eulerAngles = new Vector3(0, 0, newAngle);
            }
            isRotate = false;
        }
        CheckWin();
    }
    private void SetDotSharedParent(ButtonRotateType rotateType)
    {
        if (rotateType == ButtonRotateType.Left)
        {
            listSharedDotItem[0].transform.SetParent(circle_Left.transform);
            listSharedDotItem[listSharedDotItem.Count - 1].transform.SetParent(circle_Left.transform);
            DotItem lastSharedDot = listSharedDotItem[listSharedDotItem.Count - 1];
            listSharedDotItem[listSharedDotItem.Count - 1] = listSharedDotItem[0];
            listSharedDotItem[0] = leftCircleQueue.Dequeue();
            leftCircleQueue.Enqueue(lastSharedDot);
        }
        if (rotateType == ButtonRotateType.Right)
        {
            listSharedDotItem[0].transform.SetParent(circle_Right.transform);
            listSharedDotItem[listSharedDotItem.Count - 1].transform.SetParent(circle_Right.transform);
            DotItem lastSharedDot = listSharedDotItem[listSharedDotItem.Count - 1];
            listSharedDotItem[listSharedDotItem.Count - 1] = listSharedDotItem[0];
            listSharedDotItem[0] = rightCircleQueue.Dequeue();
            rightCircleQueue.Enqueue(lastSharedDot);
        }
    }
    private void SpawnDots()
    {
        circle_Left.position = this.transform.position;
        float alphaToRadians = alpha * Mathf.Deg2Rad;
        for (int i = 0; i < dotNummber; i++)
        {
            DotItem dot = Instantiate(dotPrefabs, circle_Left);
            dot.SetColor(DotColor.Red);
            if (i == 0 || i == dotNummber - 1)
            {
                dot.SetColor(DotColor.White);
                listSharedDotItem.Add(dot);
            }
            else if (i != 0 || i != dotNummber - 1)
            {
                leftCircleQueue.Enqueue(dot);
            }
            dot.transform.position = GetPointOnCircle(circle_Left.position, circleRadius, new Vector2(circle_Left.position.x + circleRadius, circle_Left.position.y), alphaToRadians * i + alphaToRadians / 2);
        }

        float xM = (listSharedDotItem[0].transform.position.x + listSharedDotItem[listSharedDotItem.Count - 1].transform.position.x) / 2;
        float yM = (listSharedDotItem[0].transform.position.y + listSharedDotItem[listSharedDotItem.Count - 1].transform.position.y) / 2;


        float xCenter2 = 2 * xM - circle_Left.position.x;
        float yCenter2 = 2 * yM - circle_Left.position.y;

        circle_Right.position = new Vector2(xCenter2, yCenter2);


        for (int i = 0; i < dotNummber - 2; i++)
        {
            DotItem dot = Instantiate(dotPrefabs, circle_Right);
            dot.SetColor(DotColor.Green);
            dot.transform.position = GetPointOnCircle(circle_Right.position, circleRadius, listSharedDotItem[0].transform.position, (alphaToRadians) * (i + 1) * -1);
            rightCircleQueue.Enqueue(dot);
        }

    }

    private Vector2 GetPointOnCircle(Vector2 center, float radius, Vector2 pointP, float alpha)
    {
        float theta = Mathf.Atan2(pointP.y - center.y, pointP.x - center.x);

        float newTheta = theta + alpha;
        float x2 = center.x + radius * Mathf.Cos(newTheta);
        float y2 = center.y + radius * Mathf.Sin(newTheta);

        return new Vector2(x2, y2);
    }

}
