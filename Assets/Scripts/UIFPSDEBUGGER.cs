using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIFPSDEBUGGER : MonoBehaviour
{

    private Text text;

    private void Start()
    {
        text = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        text.text = "FPS: " + (Mathf.RoundToInt(1.0f / Time.unscaledDeltaTime)).ToString();
    }
}
