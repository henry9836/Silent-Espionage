using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIFadeThenDestroy : MonoBehaviour
{
    public float timeToFade = 2.0f;

    [HideInInspector]
    public float timer = 0.0f;
    Text text;
    Color startColor;
    Color endColor;

    private void Start()
    {
        text = GetComponent<Text>();
        startColor = text.color;
        endColor = new Color(startColor.r, startColor.g, startColor.b, 0.0f);
        text.color = endColor;
        timer = timeToFade;
    }

    void Update()
    {
        text.color = Color.Lerp(startColor, endColor, (timer/timeToFade));
        timer += Time.unscaledDeltaTime;
    }
}
