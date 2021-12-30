using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AISmartWander : StateMachineBehaviour
{
    Enemy parentEntity;
    EntityMovement parentMovement;
    Transform toucanBottom;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        parentEntity = animator.gameObject.GetComponent<Enemy>();
        parentMovement = animator.gameObject.GetComponent<EntityMovement>();
        toucanBottom = parentEntity.gameObject.transform.Find("ToucanTopHalfIdle");
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        float lookDir = (toucanBottom.rotation.eulerAngles.y - 90.0f) * (Mathf.PI / 180);
        Vector3 nextPos = parentEntity.transform.position + new Vector3(-Mathf.Cos(lookDir), 0.0f, Mathf.Sin(lookDir)) * 30.0f;
        Debug.DrawLine(parentEntity.transform.position, nextPos, Color.green);
        if (parentMovement.PathPoints.Length == 0)
        {
            NavMeshPath path = new NavMeshPath();

            lookDir = (toucanBottom.rotation.eulerAngles.y + Random.Range(-45.0f, 45.0f) - 90.0f) * (Mathf.PI/180);
            nextPos = parentEntity.transform.position + new Vector3(-Mathf.Cos(lookDir), 0.0f, Mathf.Sin(lookDir)) * 30.0f;
            Debug.DrawLine(parentEntity.transform.position, nextPos);
            NavMeshHit hit;
            if (NavMesh.SamplePosition(nextPos, out hit, 8.0f, NavMesh.AllAreas))
            {
                if (NavMesh.CalculatePath(parentEntity.transform.position, hit.position, NavMesh.AllAreas, path))
                    parentMovement.PathPoints = path.corners;
            }
            else
            {
                lookDir = Random.Range(0.0f, Mathf.PI * 2.0f);
                nextPos = parentEntity.transform.position + new Vector3(-Mathf.Cos(lookDir), 0.0f, Mathf.Sin(lookDir)) * 30.0f;
                Debug.DrawLine(parentEntity.transform.position, nextPos);
                NavMeshHit hit2;
                if (NavMesh.SamplePosition(nextPos, out hit2, float.MaxValue, NavMesh.AllAreas))
                {
                    if (NavMesh.CalculatePath(parentEntity.transform.position, hit2.position, NavMesh.AllAreas, path))
                        parentMovement.PathPoints = path.corners;
                }
            }
        }
        animator.SetBool("PlayerSeen", parentMovement.NearbyPlayers.Count > 0);
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        parentMovement.PathPoints = new Vector3[0];
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
