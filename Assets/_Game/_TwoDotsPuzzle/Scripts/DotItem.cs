using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DotItem : MonoBehaviour
{
    [SerializeField] private SpriteRenderer dotSpriteRenderer;
    [SerializeField] private Color colorWhite;
    [SerializeField] private Color colorRed;
    [SerializeField] private Color colorGreen;
    [SerializeField] private DotColor currentColor;

    public DotColor CurrentColor => currentColor;
    public void SetColor(DotColor dotColor)
    {
        currentColor = dotColor;
        switch (dotColor)
        {
            case DotColor.White:
                dotSpriteRenderer.color = colorWhite;
                break;
            case DotColor.Red:
                dotSpriteRenderer.color = colorRed;
                break;
            case DotColor.Green:
                dotSpriteRenderer.color = colorGreen;
                break;
            default:
                break;
        }
    }
}
public enum DotColor
{
    White = 0, 
    Red = 1, 
    Green = 2,
}
