using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GuardController : MonoBehaviour
{
    public Transform eyes;
    public List<Transform> guardPath;
    public float arriveThreshold = 0.1f;
    public float seeDistance = 50.0f;
    public float timeToLossPlayer = 5.0f;
    [HideInInspector]
    public bool isLookingAtPlayer;
    public float shootDistanceThreshold = 3.0f;
    public float shootPullOutTime = 0.5f;
    public float reloadTime = 2.0f;
    public Vector3 gunScaleTarget;
    public GameObject gun;
    public GameObject bullet;
    public int magSize = 8;
    //[HideInInspector]
    public int magCurrentSize = 0;
    public AudioClip reloadSound;
    public AudioClip shootSound;
    [HideInInspector]
    public bool reloading = false;
    public Vector2 wanderRange = new Vector2(-5.0f, 5.0f);

    private RaycastHit hit;
    private Animator animator;
    private GameObject player;
    private AudioSource audioSrc;
    private NavMeshAgent agent;
    private LineRenderer lRender;

    public void Reload()
    {
        StartCoroutine(reloadGun());
    }

    public void ShootPlayer()
    {
        if (!reloading && magCurrentSize > 0)
        {
            GameObject tmp = Instantiate(bullet, gun.transform.position, Quaternion.identity);

            tmp.transform.LookAt(gun.transform.forward + gun.transform.position);

            audioSrc.PlayOneShot(shootSound);
            magCurrentSize--;
        }
    }

    private void Start()
    {
        if (guardPath.Count < 1)
        {
            Debug.LogWarning($"No Patrol set for guard: {gameObject.name}");
        }

        player = GameObject.FindGameObjectWithTag("Player");

        animator = GetComponent<Animator>();
        audioSrc = GetComponent<AudioSource>();
        agent = GetComponent<NavMeshAgent>();
        lRender = GetComponent<LineRenderer>();
    }

    private void Update()
    {

        Debug.DrawRay(eyes.position, (player.transform.position - eyes.position).normalized, Color.red);

        RaycastHit hit;

        // Does the ray intersect any objects excluding the player layer
        Vector3 dir = ((player.transform.position + (Vector3.up * 0.5f)) - eyes.position).normalized;

        //is the player in front of the guard?
        float angleDot = Vector3.Dot(dir, transform.forward);
        isLookingAtPlayer = angleDot > -0.1f;

        if (Physics.Raycast(eyes.position, dir, out hit, seeDistance))
        {
            //If we have hit the player with a ray and we are looking at the player
            if (hit.collider.gameObject.tag == "Player" && isLookingAtPlayer)
            {
                //Look at player more
                if (angleDot < 1.0f)
                {
                    Quaternion targetRotation = Quaternion.LookRotation(player.transform.position - transform.position);

                    // Smoothly rotate towards the target point.
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, agent.angularSpeed * Time.deltaTime);
                }

                Debug.DrawRay(eyes.position, dir * seeDistance, Color.yellow);
                animator.SetBool("CanSeePlayer", true);

                //Aim gun at player
                gun.transform.LookAt(player.transform.position + (Vector3.up * 0.5f));
                gun.transform.rotation = Quaternion.Euler(new Vector3(gun.transform.rotation.eulerAngles.x, gun.transform.rotation.eulerAngles.y, 180.0f));
                lRender.SetPosition(1, player.transform.position + (Vector3.up * 0.5f));

            }
            else
            {
                Debug.DrawRay(eyes.position, dir * hit.distance, Color.red);
                animator.SetBool("CanSeePlayer", false);
                lRender.SetPosition(1, gun.transform.forward * hit.distance);
            }
        }
        else
        {
            Debug.DrawRay(eyes.position, dir * seeDistance, Color.red);
            lRender.SetPosition(1, gun.transform.forward * seeDistance);
            animator.SetBool("CanSeePlayer", false);
        }

        //If the gun is out draw laser
        if (gun.transform.localScale.x > 0.0f)
        {
            lRender.SetPosition(0, gun.transform.position);
            lRender.enabled = true;
        }
        else
        {
            lRender.enabled = false;
        }

    }

    IEnumerator reloadGun()
    {
        reloading = true;
        audioSrc.PlayOneShot(reloadSound);
        yield return new WaitForSeconds(reloadTime);
        magCurrentSize = magSize;
        reloading = false;
    }

}
