using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimedWall : MonoBehaviour
{
    public float DelayTime = 30.0f;
    public float DropDist = 3.0f;
    public float DropTime = 5.0f;

    public float WallLowestPoint = -5.0f;
    public bool HitLowestPoint = false;

    [HideInInspector]
    public Vector3 StartingLocation;

    float dropStep;
    bool dropFinished = false;

    private void Start()
    {
        StartingLocation = transform.position;

        dropStep = DropDist / DropTime;
    }

    void Update()
    {

        if (transform.position.y > WallLowestPoint && !dropFinished)
        {
            transform.position -= new Vector3(0, dropStep * Time.deltaTime, 0);

            if (transform.position.y <= WallLowestPoint) dropFinished = true;
        }
    }

    public void ResetPosition()
    {
        transform.position = StartingLocation;
    }
}
