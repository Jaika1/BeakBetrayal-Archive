using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIChasePlayer : StateMachineBehaviour
{
    Enemy parentEntity;
    EntityMovement parentMovement;
    Entity otherPlayer;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        parentEntity = animator.gameObject.GetComponent<Enemy>();
        parentMovement = animator.gameObject.GetComponent<EntityMovement>();
        otherPlayer = parentMovement.NearbyPlayers[0];
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!otherPlayer) parentMovement.NearbyPlayers.Remove(otherPlayer);

        if (parentMovement.NearbyPlayers.Contains(otherPlayer))
        {
            // Super-inefficient early pathfinding fun!
            NavMeshPath path = new NavMeshPath();
            if (NavMesh.CalculatePath(parentEntity.transform.position, otherPlayer.transform.position, - 1, path))
                parentMovement.PathPoints = path.corners;

            Vector3 dirVect = otherPlayer.gameObject.transform.position - animator.gameObject.transform.position;

            Ray r = new Ray(animator.gameObject.transform.position, dirVect);
            RaycastHit rh;
            Debug.DrawRay(r.origin, r.direction * 2.0f, Color.red);
            if (Physics.Raycast(r, out rh))
            {
                if (rh.collider.gameObject.layer == LayerMask.NameToLayer("Player"))
                {
                    parentEntity.Attack(parentEntity.gameObject, new Vector2(dirVect.x, dirVect.z).normalized);
                }
            }
        }
        else
        {
            if (parentMovement.PathPoints.Length == 0)
            {
                animator.SetBool("PlayerSeen", false);
            }
        }
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
