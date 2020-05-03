using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class upAndDown : MonoBehaviour
{
    public float maxScaleUp = 2.0f;
    public float timeToScale = 1.5f;

    private Vector3 originalScale;
    private Vector3 maxScale;
    private bool scaleUp = true;
    private float timer = 0.0f;

    private void Start()
    {
        originalScale = transform.localScale;
        maxScale = originalScale * maxScaleUp;
    }

    private void Update()
    {
        if (scaleUp)
        {
            transform.localScale = Vector3.Slerp(originalScale, maxScale, timer / timeToScale);
        }
        else
        {
            transform.localScale = Vector3.Slerp(maxScale, originalScale, timer / timeToScale);
        }

        timer += Time.unscaledDeltaTime;

        if (timer >= timeToScale)
        {
            scaleUp = !scaleUp;
            timer = 0.0f;
        }

        
    }


}
