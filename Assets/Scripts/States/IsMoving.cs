using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsMoving : MonoBehaviour
{

    public Animator EntityAnimator;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        EntityAnimator.SetBool("isMoving", false);
        /*if ()
        {
            EntityAnimator.SetBool("isMoving", true);
        }*/
    }
}
