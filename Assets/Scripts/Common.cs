using TMPro;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public static class Common
{
    public static void SetAlpha(this Image image, float alpha)
    {
        Color color = image.color;
        image.color = new Color(color.r, color.g, color.b, alpha);
    }

    public static void SetAlpha(this TextMeshProUGUI text, float alpha)
    {
        Color color = text.color;
        text.color = new Color(color.r, color.g, color.b, alpha);
    }
}