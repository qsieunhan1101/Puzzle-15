using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingItem : MonoBehaviour
{
    [SerializeField] private SpriteRenderer bg;
    public int index = 0;
    public int lastIndexStack = -1;
    public void Init(int index, Color bgColor , float scale)
    {
        this.index = index;
        bg.color = bgColor;
        transform.localScale = new Vector3(transform.localScale.x + scale, transform.localScale.y, transform.localScale.z);
    }
}
