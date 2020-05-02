using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Invoker : MonoBehaviour
{
    private void Start()
    {
        //Start Ad Service
        AdTime.Initialize();
    }

    private void Update()
    {
        if (!AdTime.lastAdSuccessful)
        {
            Debug.Log($"Ad returned: {AdTime.AdThyme(AdTime.ADID_LOSS)}");
        }
    }
}
