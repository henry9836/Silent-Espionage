using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{

    public GameObject destinationObj;
    public float thresholdDistanceForVisual = 0.5f;

    private NavMeshAgent agent;
    private LineRenderer lineR;
    private MeshRenderer destMeshRender;

    public void Explode()
    {
        Debug.Log("Player Dead");
    }

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        lineR = GetComponent<LineRenderer>();

        if (!destinationObj)
        {
            Debug.LogWarning("DestinationNode was not set attempting to find in game world...");
            destinationObj = GameObject.Find("DestinationNode");
        }

        destMeshRender = destinationObj.GetComponent<MeshRenderer>();

        lineR.enabled = false;
        destMeshRender.enabled = false;
    }

    private void Update()
    {

        //Movement
        if (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                agent.SetDestination(hit.point);
                destinationObj.transform.position = hit.point + (Vector3.up * 0.5f);
            }
        }

        //If we are far enough away from desintation visual on
        if ((thresholdDistanceForVisual <= agent.remainingDistance) || agent.remainingDistance == Mathf.Infinity)
        {

            //Make dest visualble
            destMeshRender.enabled = true;

            lineR.positionCount = agent.path.corners.Length;

            for (int i = 0; i < agent.path.corners.Length; i++)
            {
                lineR.SetPosition(i, agent.path.corners[i] + (Vector3.up * 0.45f));
            }

            lineR.enabled = true;

        }
        else
        {
            //Make dest gone
            destMeshRender.enabled = false;

            lineR.enabled = false;

        }

    }

}
