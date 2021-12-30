using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Entity))]
/// <summary>
/// Allows player movement via input from a pre-defined controller port
/// </summary>
public class PlayerMovement : MonoBehaviour
{
    /// <summary>
    /// The rigidbody that is attached to the GameObject this component is also attached to
    /// </summary>
    private Rigidbody rbody;

    /// <summary>
    /// The entity class inherited by the attached GameObject
    /// </summary>
    private Entity entity;

    public Animator[] Animations;

    Transform waistTransform;
    Transform legTransform;

    // Start is called before the first frame update
    void Start()
    {
        entity = GetComponent<Entity>();

        rbody = GetComponent<Rigidbody>();
        rbody.freezeRotation = true;

        waistTransform = transform.Find("ToucanTopHalfIdle");
        legTransform = transform.Find("ToucanBottomHalfIdle");
        //inputActions = new InputActions();
        //inputActions.Enable();
    }

    
    void FixedUpdate()
    {
        Gamepad PlayerGamepad = entity.Information.PlayerGamepad;
        if (!PlayerGamepad.added) return;
        Vector2 moveMagnitude = PlayerGamepad.leftStick.ReadValue();//inputActions.Player.Move.ReadValue<Vector2>();
        if (moveMagnitude.magnitude > 0.0f)
        {
            moveMagnitude = moveMagnitude.magnitude > 1.0f ? moveMagnitude.normalized : moveMagnitude; // Limits the total magnitude of the vector to 1 (abs(x) + abs(y) <= 1.0f)
            moveMagnitude *= entity.EntityModifiers.Acceleration * Time.deltaTime; // Muliply the movement vector by the speed and delta time
            Vector2 newPlaneMovement = new Vector2(rbody.velocity.x, rbody.velocity.z) + moveMagnitude; // Add the new velocity vector
            newPlaneMovement = newPlaneMovement.magnitude > entity.EntityModifiers.SpeedCap * Time.deltaTime ? newPlaneMovement.normalized * entity.EntityModifiers.SpeedCap * Time.deltaTime : newPlaneMovement; // Cap the magnitude of the new velocity vector to the entities speed cap.
            if (entity.WispModifiers.SpeedAbilityDuration > 0.0f) newPlaneMovement *= WispAbilties.SpeedModifier;
            rbody.velocity = new Vector3(newPlaneMovement.x, rbody.velocity.y, newPlaneMovement.y); // Apply the new velocity to the rigidbody
            float legRotation = Mathf.Atan2(newPlaneMovement.x, newPlaneMovement.y) - (Mathf.PI / 2.0f);
            legTransform.rotation = Quaternion.Euler(0.0f, legRotation * (180.0f / Mathf.PI) - 90.0f, 0.0f);

        }
        Array.ForEach(Animations, a => a.SetFloat("runSpeed", moveMagnitude.magnitude)); // Set 

        Vector2 rightStickValue = PlayerGamepad.rightStick.ReadValue();
        if (rightStickValue.magnitude >= 0.1f)
        {
            rightStickValue.Normalize();
            entity.Attack(gameObject, rightStickValue);

            //Turn Animation
            float waistRotation = Mathf.Atan2(-rightStickValue.y, rightStickValue.x) - (Mathf.PI / 2.0f);
            waistTransform.rotation = Quaternion.Euler(0.0f, waistRotation * (180.0f/Mathf.PI), 0.0f);
        }


    }

}
