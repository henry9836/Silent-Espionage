using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{

    public GameObject destinationObj;
    public List<GameObject> brokenEggPieces = new List<GameObject>();
    public GameObject normalEgg;
    public float thresholdDistanceForVisual = 0.5f;
    public bool amDead = false;
    public bool test = false;
    public AudioClip dieSound;

    private AudioSource audioSrc;
    private NavMeshAgent agent;
    private LineRenderer lineR;
    private MeshRenderer destMeshRender;
    private LevelManager levelMan;

    public void Explode()
    {
        Debug.Log("Player Dead");

        if (!amDead)
        {
            GetComponent<CapsuleCollider>().enabled = false;

            normalEgg.SetActive(false);

            for (int i = 0; i < brokenEggPieces.Count - 1; i++)
            {
                brokenEggPieces[i].SetActive(true);
                brokenEggPieces[i].transform.parent = null;
                if (brokenEggPieces[i].GetComponent<Rigidbody>())
                {
                    Debug.Log("Applying Force...");
                    brokenEggPieces[i].GetComponent<Rigidbody>().AddExplosionForce(5000.0f, transform.position, 50.0f);
                }
            }

            audioSrc.PlayOneShot(dieSound);

        }

        amDead = true;

    }


    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        lineR = GetComponent<LineRenderer>();
        audioSrc = GetComponent<AudioSource>();
        levelMan = GameObject.Find("LevelManager").GetComponent<LevelManager>();

        if (!destinationObj)
        {
            Debug.LogWarning("DestinationNode was not set attempting to find in game world...");
            destinationObj = GameObject.Find("DestinationNode");
        }

        destMeshRender = destinationObj.GetComponent<MeshRenderer>();

        lineR.enabled = false;
        destMeshRender.enabled = false;

        amDead = false;

    }

    private void Update()
    {

        if (test)
        {
            Explode();
        }

        if (!amDead && !levelMan.levelOver)
        {

            //Movement
            if (Input.GetMouseButton(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, Mathf.Infinity))
                {
                    agent.SetDestination(hit.point);
                    destinationObj.transform.position = hit.point + (Vector3.up * 0.1f);
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

}
