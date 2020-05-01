using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{

    public GameObject destinationObj;

    private NavMeshAgent agent;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        if (!destinationObj)
        {
            Debug.LogWarning("");
            destinationObj = GameObject.Find("DestinationNode");
        }
            
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
                destinationObj.transform.position = hit.point;
            }
        }

    }

}
