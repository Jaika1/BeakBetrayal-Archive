using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AISmartAttack : StateMachineBehaviour
{
    Enemy parentEntity;
    EntityMovement parentMovement;
    Entity otherPlayer;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        parentEntity = animator.gameObject.GetComponent<Enemy>();
        parentMovement = animator.gameObject.GetComponent<EntityMovement>();
        if (parentMovement.NearbyPlayers.Count > 0)
        {
            otherPlayer = parentMovement.NearbyPlayers[0];
        }
        else
        {
            animator.SetBool("PlayerSeen", false);
        }
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Check if the other player was destroyed
        if (!otherPlayer) parentMovement.NearbyPlayers.Remove(otherPlayer);

        // Check if the player is even alive
        if (otherPlayer.EntityModifiers.Health <= 0) parentMovement.NearbyPlayers.Remove(otherPlayer);

        if (parentMovement.NearbyPlayers.Contains(otherPlayer))
        {
            // Check if player is in LoS
            
            NavMeshPath path = new NavMeshPath();

            #region Average-point pathfinding
            // Find the average point inbetween all nearby players
            //Vector3 allPosAverage = Vector3.zero;
            //foreach (Entity e in parentMovement.NearbyPlayers)
            //    allPosAverage += e.gameObject.transform.position;
            //allPosAverage /= parentMovement.NearbyPlayers.Count;

            // Check if path to inbetween point is clear
            //Vector3 dirVect = allPosAverage - parentEntity.gameObject.transform.position;
            //Ray r = new Ray(parentEntity.gameObject.transform.position, dirVect);
            //RaycastHit rh;
            #endregion

            //Find point 'x' units away from the player
            Vector3 dirVect = otherPlayer.gameObject.transform.position - parentEntity.gameObject.transform.position;
            float direction = Mathf.Atan2(-dirVect.z, dirVect.x) - (2.0f * Mathf.PI / 2.0f);
            Vector3 goToPos = otherPlayer.gameObject.transform.position - new Vector3(-Mathf.Cos(direction), 0.0f, Mathf.Sin(direction)) * 10.0f;
            Debug.DrawLine(otherPlayer.gameObject.transform.position, goToPos, Color.red);

            // Closest bullet avoidance
            Vector3 goToBPos = goToPos;
            BulletScript[] bullets = FindObjectsOfType<BulletScript>(); // <-- This is stupid.
            bullets = Array.FindAll(bullets, x => (x.transform.position - parentEntity.gameObject.transform.position).magnitude <= 8.0f);
            if (bullets.Length > 0)
            {
                Array.Sort(bullets, (x, y) =>
                {
                    return Math.Sign((y.transform.position - parentEntity.gameObject.transform.position).magnitude - (x.transform.position - parentEntity.gameObject.transform.position).magnitude);
                });
                BulletScript closestBullet = bullets[0];

                //Go away from it
                goToBPos -= (closestBullet.transform.position - parentEntity.transform.position).normalized * 6.0f;
            }

            Debug.DrawLine(otherPlayer.gameObject.transform.position, goToBPos, Color.magenta);

            // Check if path to the player is clear
            Vector3 toPlayerDir = otherPlayer.gameObject.transform.position - parentEntity.gameObject.transform.position;
            Ray r2 = new Ray(parentEntity.gameObject.transform.position, toPlayerDir);
            RaycastHit rh2;
            Physics.Raycast(r2, out rh2);

            Vector3 goToBDir = goToBPos - parentEntity.gameObject.transform.position;
            Ray rb = new Ray(parentEntity.gameObject.transform.position, goToBDir);
            RaycastHit rbh;

            Vector3 goToDir = goToPos - parentEntity.gameObject.transform.position;
            Ray r = new Ray(parentEntity.gameObject.transform.position, goToDir);
            RaycastHit rh;

            if (!Physics.Raycast(rb, out rbh, goToBDir.magnitude) && rh2.collider.GetComponent<Entity>() != null)
            {
                // Path is clear, go to that point directly
                parentMovement.PathPoints = new Vector3[] { goToBPos };
            }
            else if (!Physics.Raycast(r, out rh, goToDir.magnitude) && rh2.collider.GetComponent<Entity>() != null)
            {
                // Path is clear, go to that point directly
                #region Slower, more diverse option
                //NavMeshHit hit;
                //if (NavMesh.SamplePosition(allPosAverage, out hit, float.MaxValue, NavMesh.AllAreas))
                //{
                //    if (NavMesh.CalculatePath(parentEntity.transform.position, hit.position, NavMesh.AllAreas, path))
                //        parentMovement.PathPoints = path.corners;
                //}
                #endregion
                parentMovement.PathPoints = new Vector3[] { goToPos };
            }
            else
            {
                // Path is unclear, navigate towards player
                if (NavMesh.CalculatePath(parentEntity.transform.position, otherPlayer.transform.position, NavMesh.AllAreas, path))
                        parentMovement.PathPoints = path.corners;
            }

            // Keep shooting non-stop
            Vector3 shootDirVect = otherPlayer.gameObject.transform.position - parentEntity.gameObject.transform.position;
            parentEntity.Attack(parentEntity.gameObject, new Vector2(shootDirVect.x, shootDirVect.z).normalized);

            #region old
            // Super-inefficient early pathfinding fun!
            //NavMeshPath path = new NavMeshPath();
            //if (NavMesh.CalculatePath(parentEntity.transform.position, otherPlayer.transform.position, - 1, path))
            //    parentMovement.PathPoints = path.corners;

            //Vector3 dirVect = otherPlayer.gameObject.transform.position - animator.gameObject.transform.position;

            //Ray r = new Ray(animator.gameObject.transform.position, dirVect);
            //RaycastHit rh;
            //Debug.DrawRay(r.origin, r.direction * 2.0f, Color.red);
            //if (Physics.Raycast(r, out rh))
            //{
            //    if (rh.collider.gameObject.layer == LayerMask.NameToLayer("Player"))
            //    {
            //        parentEntity.Attack(parentEntity.gameObject, new Vector2(dirVect.x, dirVect.z).normalized);
            //    }
            //}
            #endregion
        }
        else
        {
            //if (parentMovement.PathPoints.Length == 0)
            //{
                animator.SetBool("PlayerSeen", false);
            //}
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
