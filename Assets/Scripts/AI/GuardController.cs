using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardController : MonoBehaviour
{
    public List<Transform> guardPath;
    public float arriveThreshold = 0.1f;
    private void Start()
    {
        if (guardPath.Count < 1)
        {
            Debug.LogWarning($"No Patrol set for guard: {gameObject.name}");
        }
    }
}
