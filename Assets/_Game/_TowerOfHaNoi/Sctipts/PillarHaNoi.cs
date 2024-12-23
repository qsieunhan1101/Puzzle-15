using System;
using UnityEngine;

public class PillarHaNoi : MonoBehaviour
{
    [SerializeField] private Transform bg;
    public int index = 0;

    private Action<int> onClickEvent = null;
    public void Init(int index, Action<int> onClickEvent, int ringNumber)
    {
        this.index = index;
        this.onClickEvent = onClickEvent;
        bg.localScale = new Vector3(1f, ringNumber + 1f, 1f);
        bg.localPosition = new Vector3(0, bg.localScale.y/2, 0);
    }
    public void OnClickPillar()
    {
        onClickEvent?.Invoke(index);
    }
}
