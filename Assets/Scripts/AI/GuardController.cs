using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public int magSize = 8;
    //[HideInInspector]
    public int magCurrentSize = 0;
    public AudioClip reloadSound;
    public AudioClip shootSound;
    [HideInInspector]
    public bool reloading = false;

    private RaycastHit hit;
    private Animator animator;
    private GameObject player;
    private AudioSource audioSrc;

    public void Reload()
    {
        StartCoroutine(reloadGun());
    }

    public void ShootPlayer()
    {
        if (!reloading && magCurrentSize > 0)
        {
            player.GetComponent<PlayerController>().Explode();
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
    }

    private void Update()
    {

        Debug.DrawRay(eyes.position, (player.transform.position - eyes.position).normalized, Color.red);

        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer

        Vector3 dir = ((player.transform.position + (Vector3.up * 0.5f)) - eyes.position).normalized;

        //is the player in front of the guard?
        float angleDot = Vector3.Dot(dir, transform.forward);
        isLookingAtPlayer = angleDot > 0.0f;

        if (Physics.Raycast(eyes.position, dir, out hit, seeDistance))
        {
            //If we have hit the player with a ray and we are looking at the player
            if (hit.collider.gameObject.tag == "Player" && isLookingAtPlayer)
            {
                Debug.DrawRay(eyes.position, dir * seeDistance, Color.yellow);
                animator.SetBool("CanSeePlayer", true);
            }
            else
            {
                Debug.DrawRay(eyes.position, dir * hit.distance, Color.red);
                animator.SetBool("CanSeePlayer", false);
            }
        }
        else
        {
            Debug.DrawRay(eyes.position, dir * seeDistance, Color.red);
            animator.SetBool("CanSeePlayer", false);
        }



    }

    IEnumerator reloadGun()
    {
        Debug.Log("Reloading...");
        reloading = true;
        audioSrc.PlayOneShot(reloadSound);
        yield return new WaitForSeconds(reloadTime);
        magCurrentSize = magSize;
        reloading = false;
        Debug.Log("Reloaded");
    }

}
