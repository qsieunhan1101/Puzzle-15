using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ButtonRotate : MonoBehaviour
{
    [SerializeField] private ButtonRotateType currentRotateType;


    public Action<ButtonRotateType> rotateEvent = null;
    private void OnMouseDown()
    {
        if (Input.GetMouseButtonDown(0))
        {
            rotateEvent?.Invoke(currentRotateType);
        }
    }
    
}
public enum ButtonRotateType
{
    Left = 0,
    Right = 1,
}
