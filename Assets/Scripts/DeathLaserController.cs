using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathLaserController : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        //Do you want to explode!?
        if (other.tag == "Player")
        {
            other.GetComponent<PlayerController>().Explode();
        }
        else if (other.tag == "Guard")
        {
            other.GetComponent<GuardController>().Explode();
        }
    }
}
