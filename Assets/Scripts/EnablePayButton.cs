using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnablePayButton : MonoBehaviour
{
    private Button button;

    public MillionairBaby IAPController;

    private void Start()
    {
        button = GetComponent<Button>();
    }

    private void FixedUpdate()
    {
        if (IAPController.IsInitialized())
        {
            button.interactable = true;
        }
        else
        {
            button.interactable = false;
        }
    }

}
