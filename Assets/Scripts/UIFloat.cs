using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFloat : MonoBehaviour
{

    public float speed = 1.0f;
    public bool test = false;

    private RectTransform rectTransform;
    private UIFadeThenDestroy fader;
    private Vector3 initalPos;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        fader = GetComponent<UIFadeThenDestroy>();
        initalPos = rectTransform.localPosition;
    }

    void Update()
    {
        rectTransform.localPosition -= Vector3.up * speed * Time.unscaledDeltaTime;

        if (test)
        {
            Reset();
            test = false;
        }

    }

    public void Reset()
    {
        fader.timer = 0.0f;
        rectTransform.localPosition = initalPos + new Vector3(Random.Range(-100.0f, 100.0f), Random.Range(-50.0f, 50.0f), 0.0f);
    }

}
