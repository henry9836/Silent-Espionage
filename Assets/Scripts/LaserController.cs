using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class LaserController : MonoBehaviour
{

    public GameObject laser;
    private NavMeshObstacle obstcle;

    private void Start()
    {
        obstcle = laser.GetComponent<NavMeshObstacle>();
    }

    public void toggle()
    {
        laser.SetActive(!laser.activeInHierarchy);
        StartCoroutine(obstculeUpdate());
    }

    IEnumerator obstculeUpdate()
    {
        yield return new WaitForSecondsRealtime(0.9f);
        obstcle.enabled = laser.activeInHierarchy;
    }
}
