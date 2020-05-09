using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class GuardController : MonoBehaviour
{
    public AudioClip reloadSound;
    public AudioClip shootSound;
    public Vector2 wanderRange = new Vector2(-5.0f, 5.0f);
    public Light muzzleLight;
    public Vector3 gunScaleTarget;
    public GameObject gun;
    public GameObject bullet;
    public GameObject muzzleObj;
    public Transform eyes;
    public List<Transform> guardPath;
    public float arriveThreshold = 0.1f;
    public float seeDistance = 50.0f;
    public float timeToLossPlayer = 5.0f;
    public float shootDistanceThreshold = 3.0f;
    public float shootPullOutTime = 0.5f;
    public float reloadTime = 2.0f;
    public int magSize = 8;
    [HideInInspector]
    public int magCurrentSize = 0;
    [HideInInspector]
    public bool amDead = false;
    [HideInInspector]
    public bool reloading = false;
    [HideInInspector]
    public bool isLookingAtPlayer;
    public GameObject normalEgg;
    public List<GameObject> brokenEggPieces = new List<GameObject>();
    public AudioClip dieSound;
    public bool test = false;

    private RaycastHit hit;
    private Animator animator;
    private GameObject player;
    private AudioSource audioSrc;
    private NavMeshAgent agent;
    private LineRenderer lRender;
    private LevelManager levelManager;
    private UIFloat bonusUI;

    public void Reload()
    {
        StartCoroutine(reloadGun());
    }

    public void Explode()
    {

        if (!amDead)
        {
            agent.ResetPath();
            lRender.enabled = false;
            agent.enabled = false;
            GetComponent<CapsuleCollider>().enabled = false;

            normalEgg.SetActive(false);

            for (int i = 0; i < brokenEggPieces.Count - 1; i++)
            {
                brokenEggPieces[i].SetActive(true);
                brokenEggPieces[i].transform.parent = null;
                if (brokenEggPieces[i].GetComponent<Rigidbody>())
                {
                    brokenEggPieces[i].GetComponent<Rigidbody>().AddExplosionForce(5000.0f, transform.position, 50.0f);
                }
            }

            audioSrc.PlayOneShot(dieSound);

            //Reward player for kill
            if (levelManager.timer > 10.0f)
            {
                levelManager.timer -= 10.0f;
            }
            else
            {
                levelManager.timer = 0.0f;
            }
        }
        amDead = true;
        bonusUI.Reset();
        StartCoroutine(killMe());
    }

    public void ShootPlayer()
    {
        if (!reloading && magCurrentSize > 0)
        {
            //Effects
            StartCoroutine(muzzleVisual());

            GameObject tmp = Instantiate(bullet, gun.transform.position, Quaternion.identity);

            tmp.transform.LookAt(gun.transform.forward + gun.transform.position);

            audioSrc.PlayOneShot(shootSound);
            magCurrentSize--;

        }
    }

    private void Start()
    {

        muzzleLight.enabled = false;
        muzzleObj.SetActive(false);

        if (guardPath.Count < 1)
        {
            Debug.LogWarning($"No Patrol set for guard: {gameObject.name}");
        }

        player = GameObject.FindGameObjectWithTag("Player");

        animator = GetComponent<Animator>();
        audioSrc = GetComponent<AudioSource>();
        agent = GetComponent<NavMeshAgent>();
        lRender = GetComponent<LineRenderer>();
        levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
        bonusUI = GameObject.Find("Bonus").GetComponent<UIFloat>();
    }

    private void Update()
    {

        if (test)
        {
            Explode();
        }

        if (!amDead)
        {
            //Debug.DrawRay(eyes.position, (player.transform.position - eyes.position).normalized, Color.red);

            RaycastHit hit;

            // Does the ray intersect any objects excluding the player layer
            Vector3 dir = ((player.transform.position + (Vector3.up * 1.0f)) - eyes.position).normalized;

            //is the player in front of the guard?
            float angleDot = Vector3.Dot(dir, transform.forward);
            isLookingAtPlayer = angleDot >= 0.0f;

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

                    //Debug.DrawRay(eyes.position, dir * seeDistance, Color.yellow);
                    animator.SetBool("CanSeePlayer", true);

                    //Aim gun at player
                    gun.transform.LookAt(player.transform.position + (Vector3.up * 0.5f));
                    gun.transform.rotation = Quaternion.Euler(new Vector3(gun.transform.rotation.eulerAngles.x, gun.transform.rotation.eulerAngles.y, 180.0f));
                    lRender.SetPosition(1, player.transform.position + (Vector3.up * 0.5f));

                }
                else
                {
                    //Laser
                    if (Physics.Raycast(gun.transform.position, gun.transform.forward, out hit, shootDistanceThreshold))
                    {

                        lRender.SetPosition(1, hit.point);
                    }
                    else
                    {
                        lRender.SetPosition(1, gun.transform.position + (gun.transform.forward * seeDistance));
                    }


                    //Debug.DrawRay(eyes.position, dir * hit.distance, Color.red);
                    animator.SetBool("CanSeePlayer", false);
                }
            }
            else
            {
                //Debug.DrawRay(eyes.position, dir * seeDistance, Color.red);
                lRender.SetPosition(1, gun.transform.position + (gun.transform.forward * seeDistance));
                animator.SetBool("CanSeePlayer", false);
            }

            //Draw Vision
            //Debug.DrawRay(eyes.position, transform.forward * seeDistance, Color.cyan);
            //Debug.DrawRay(eyes.position, transform.forward * shootDistanceThreshold, Color.magenta);

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
    }

    IEnumerator reloadGun()
    {
        reloading = true;
        audioSrc.PlayOneShot(reloadSound);
        yield return new WaitForSeconds(reloadTime);
        magCurrentSize = magSize;
        reloading = false;
    }

    IEnumerator muzzleVisual()
    {
        muzzleLight.enabled = true;
        muzzleObj.transform.rotation = Quaternion.Euler(Random.Range(-360, 360), muzzleObj.transform.rotation.eulerAngles.y, muzzleObj.transform.rotation.eulerAngles.z);
        muzzleObj.SetActive(true);
        yield return new WaitForSeconds(0.05f);
        muzzleLight.enabled = false;
        muzzleObj.SetActive(false);
    }

    IEnumerator killMe()
    {
        yield return new WaitForSecondsRealtime(7.0f);
        //for (int i = 0; i < brokenEggPieces.Count; i++)
        //{
        //    Destroy(brokenEggPieces[i]);
        //}
        Destroy(gameObject);
    }

}
