using System;
using UnityEngine;

public class PipeItem : MonoBehaviour
{
    [SerializeField] private PipeType currentPipeType;
    [SerializeField] float originAngle, currentAngle;
    [SerializeField] bool isRightAngle;
    [SerializeField] int[] angles = new int[4] { 0, 90, 180, 270 };
    [SerializeField] int angleIndex;
    

    public Action checkWinEvent;
    public bool IsRightAngle => isRightAngle;
    private void Start()
    {
        Init();
        int ran = UnityEngine.Random.Range(1, angles.Length);
        angleIndex = ran;
        RotatePipe(ran);
    }
    private void OnMouseDown()
    {
        if (Input.GetMouseButtonDown(0) && currentPipeType != PipeType.None)
        {
            angleIndex++;
            angleIndex = Mathf.Abs(angleIndex) % angles.Length;
            RotatePipe(angleIndex);
            checkWinEvent?.Invoke();
        }
    }

    public void Init()
    {
        originAngle = transform.rotation.eulerAngles.z;
        currentAngle = originAngle;
        isRightAngle = true;
    }
    private void RotatePipe(int angleIndex)
    {
        this.transform.eulerAngles = new Vector3(0, 0, angles[angleIndex]);
        currentAngle = transform.rotation.eulerAngles.z;
        CheckRightAngle();
    }
    private void CheckRightAngle()
    {
        float deltaAngle = currentAngle - originAngle;

        if (deltaAngle == 0)
        {
            isRightAngle = true;
        }
        else
        {
            isRightAngle = false;
        }

        if (currentPipeType == PipeType.Straight_Vertical || currentPipeType == PipeType.Straight_Horizontal)
        {
            if (deltaAngle == 180)
            {
                isRightAngle = true;
            }
        }
    }
}
public enum PipeType
{
    None = 0,
    Straight_Vertical = 1,
    Straight_Horizontal = 2,
    Elbow_DownRight = 3,
    Elbow_DownLeft = 4,
    Elbow_UpRight = 5,
    Elbow_UpLeft = 6,
}
