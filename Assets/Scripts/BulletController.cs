using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float speed = 10.0f;
    public float maxTime = 10.0f;

    private float timer = 0.0f;

    private void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;

        if (maxTime < timer)
        {
            Destroy(gameObject);
        }

        timer += Time.deltaTime;

    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.tag == "Player")
        {
            other.GetComponent<PlayerController>().Explode();
        }

        else if (other.tag == "Guard")
        {
            other.GetComponent<GuardController>().Explode();
        }

        //Debug.Log($"I hit {other.gameObject.name}");

        Destroy(gameObject);
    }

}
