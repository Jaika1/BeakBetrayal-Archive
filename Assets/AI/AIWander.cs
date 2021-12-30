using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIWander : StateMachineBehaviour
{
    Enemy parentEntity;
    EntityMovement parentMovement;

    //override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    parentEntity = animator.gameObject.GetComponent<Enemy>();
    //    parentMovement = animator.gameObject.GetComponent<EntityMovement>();
    //}

    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    if (parentMovement.PathPoints.Length == 0)
    //    {
    //        Vector3 nextPoint = parentEntity.PointsReference[Random.Range(0, parentEntity.PointsReference.Length)];
    //        NavMeshPath path = new NavMeshPath();
    //        if (NavMesh.CalculatePath(parentEntity.transform.position, nextPoint, NavMesh.AllAreas, path))
    //            parentMovement.PathPoints = path.corners;
    //    }
    //    animator.SetBool("PlayerSeen", parentMovement.NearbyPlayers.Count > 0);
    //}

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
