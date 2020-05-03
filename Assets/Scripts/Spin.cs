using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spin : MonoBehaviour
{
    public float speedOfRot = 1.0f;

    public Vector3 rot;

    private float x;
    private float y;
    private float z;

    void Update()
    {
        x = transform.rotation.eulerAngles.x + (rot.x * (speedOfRot * Time.unscaledDeltaTime));
        y = transform.rotation.eulerAngles.y + (rot.y * (speedOfRot * Time.unscaledDeltaTime));
        z = transform.rotation.eulerAngles.z + (rot.z * (speedOfRot * Time.unscaledDeltaTime));

        transform.rotation = Quaternion.Euler(new Vector3(x, y, z));
    }
}
