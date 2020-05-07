using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LaserController : MonoBehaviour
{

    public GameObject laser;

    public void toggle()
    {
        laser.SetActive(!laser.activeInHierarchy);
    }
}
