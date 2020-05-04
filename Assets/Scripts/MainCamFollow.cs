using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamFollow : MonoBehaviour
{

    public Transform playerTransform;
    public float timeToRot = 1.0f;
    public List<Transform> angles = new List<Transform>();

    private Vector3 positionFromPlayer = new Vector3(15.0f, 28.0f, -17.0f);
    private GameObject cam;
    private float rotTimer = 0.0f;
    private Transform target;
    private Transform oldTarget;
    private int currentAngleElement = 0;
    

    // Start is called before the first frame update
    void Start()
    {
        if (!playerTransform)
        {
            playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        }
        //Get distance from camera
        positionFromPlayer.x = transform.position.x - playerTransform.position.x;
        positionFromPlayer.y = transform.position.y - playerTransform.position.y;
        positionFromPlayer.z = transform.position.z - playerTransform.position.z;

        cam = transform.GetChild(0).gameObject;

        Debug.Log(positionFromPlayer);

        target = angles[currentAngleElement];
        oldTarget = target;

    }

    public void RotateCam()
    {

        currentAngleElement++;
        if (currentAngleElement >= angles.Count)
        {
            currentAngleElement = 0;
        }

        oldTarget = target;
        target = angles[currentAngleElement];

        rotTimer = 0.0f;
        
    }

    // Update is called once per frame
    void Update()
    {
        //Follow player
        transform.position = playerTransform.position + positionFromPlayer;

        //Rotate Camera Towards target
        cam.transform.position = Vector3.Lerp(oldTarget.position, target.position, rotTimer/timeToRot);
        cam.transform.rotation = Quaternion.Lerp(oldTarget.rotation, target.rotation, rotTimer / timeToRot);

        rotTimer += Time.unscaledDeltaTime;

    }
}
