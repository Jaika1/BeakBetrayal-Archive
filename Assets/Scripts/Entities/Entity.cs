using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using System.Linq;
using System;

/// <summary>
/// Contains important information and other variables for players and other NPCs
/// </summary>
public class Entity : MonoBehaviour
{
    public ItemModifiers EntityModifiers = new ItemModifiers()
    {
        Acceleration = 320.0f,
        SpeedCap = 580.0f,
        Health = 100,
        MaxHealth = 100,
        Damage = 20,
        AttackCooldown = 0.3f,
        ProjectileSpeed = 22.0f,
        TrackingRadius = 0.0f,
        InstantDeath = false
    };
    public WispAbilties WispModifiers = new WispAbilties();
    [HideInInspector]
    public EntityInformation Information;
    [HideInInspector]
    public HealthBar HealthBar;
    [HideInInspector]
    public List<BetterCamera> PlayerCameras = new List<BetterCamera>();
    public GameObject PlayerWisp;

    public Animator[] Animations;
    public AudioClip DeathJingle;
    public GameObject BulletPrefab;
    public GameObject EventTextRef;
    protected GameObject killer;

    public float WispOrbitRate = 0.1f;
    public float WispDistance = 2.0f;

    private float cooldownTimer = 0.0f;
    private List<WispController> attachedWisps = new List<WispController>();
    private List<GameObject> attachedWispObjects = new List<GameObject>();
    private GameManagerScript managerScriptRef;

    private void Awake()
    {
        managerScriptRef = FindObjectOfType<GameManagerScript>();
    }

    private void Update()
    {
        if (cooldownTimer > 0.0f) 
            cooldownTimer -= Time.deltaTime;

        if (WispModifiers.SpeedAbilityDuration > 0.0f)
            WispModifiers.SpeedAbilityDuration -= Time.deltaTime;

        if (WispModifiers.FireAbilityDuration > 0.0f)
            WispModifiers.FireAbilityDuration -= Time.deltaTime;

        if (WispModifiers.VulnerabilityAbilityDuration > 0.0f)
            WispModifiers.VulnerabilityAbilityDuration -= Time.deltaTime;

        if (WispModifiers.FoVAbilityDuration > 0.0f)
            WispModifiers.FoVAbilityDuration -= Time.deltaTime;

        for (int i = 0; i < attachedWispObjects.Count; ++i)
        {
            float r = (Time.time * (Mathf.PI * 2.0f) * WispOrbitRate) + (Mathf.PI * 2.0f / attachedWispObjects.Count * i);
            attachedWispObjects[i].transform.position = new Vector3(transform.position.x + (Mathf.Cos(r) * WispDistance), transform.position.y, transform.position.z - (Mathf.Sin(r) * WispDistance));
            float lookAngle = (r * (180.0f / Mathf.PI)) + 180.0f;
            attachedWispObjects[i].transform.rotation = Quaternion.Euler(0.0f, lookAngle, 0.0f);
        }

        if (!WispModifiers.FoVAbilityInEffect && WispModifiers.FoVAbilityDuration > 0.0f)
        {
            WispModifiers.FoVAbilityInEffect = true;
            PlayerCameras.ForEach(x => x.SetFOV(WispAbilties.FoVValue));
        }
        else if (WispModifiers.FoVAbilityInEffect && WispModifiers.FoVAbilityDuration <= 0f)
        {
            WispModifiers.FoVAbilityInEffect = false;
            PlayerCameras.ForEach(x => x.ResetFOV());
        }
    }

    public void TakeDamage(BulletScript bullet)
    {
        if (bullet.Sender.GetComponent<Entity>().EntityModifiers.Health > 0)
        {
            int damage = bullet.Damage;
            if (WispModifiers.VulnerabilityAbilityDuration > 0.0f) damage = Mathf.RoundToInt(damage * WispAbilties.VulnerabilityModifier);
            if (EntityModifiers.InstantDeath) damage = EntityModifiers.Health;
            AdjustHealth(-damage);
            if (EntityModifiers.Health <= 0) OnDeath(bullet.Sender);
        }
    }

    public void AdjustHealth(int diff)
    {
        EntityModifiers.Health += diff;
        if (EntityModifiers.Health > EntityModifiers.MaxHealth)
            EntityModifiers.Health = EntityModifiers.MaxHealth;
        HealthBar?.UpdateHealthBar();
    }

    public void Attack(GameObject sender, Vector2 attackDirection)
    {
        if (EntityModifiers.Health > 0 && cooldownTimer <= 0.0f && WispModifiers.FireAbilityDuration <= 0.0f)
        {
            CreateBullet(sender, attackDirection);
            cooldownTimer = EntityModifiers.AttackCooldown;
        }
    }

    public void CreateBullet(GameObject sender, Vector2 attackDirection)
    {
        GameObject newBullet = Instantiate(BulletPrefab);

        // Set the bullets location to the players location
        newBullet.transform.position = transform.position;

        BulletScript bScript = newBullet.GetComponent<BulletScript>();

        // Make the bullets attackDirection = to the entites attakDirection multiplied with projectile speed
        bScript.AttackDirection = attackDirection * EntityModifiers.ProjectileSpeed;

        // Ensure the bullet remembers who sent it
        bScript.Sender = sender;

        bScript.Damage = EntityModifiers.Damage;
        bScript.TracerRadius = EntityModifiers.TrackingRadius;
    }

    public virtual void OnDeath(GameObject sender)
    {
        PlayerCameras.ForEach(x => x.ResetFOV());
        sender.GetComponent<Entity>().Information.Score += Scoring.PlayerKill;
        if (sender.GetComponent<Player>() != null)
        {
            GameObject eventText = Instantiate(EventTextRef);
            eventText.GetComponent<EventText>().SetEventTextAndCamera(sender.GetComponent<Player>().PlayerCameras[0].AttachedCamera, $"+{Scoring.PlayerKill}");
            eventText.transform.position = transform.position;
        }
        managerScriptRef.CheckGameFinished();

        AudioSource.PlayClipAtPoint(DeathJingle, Vector3.zero);
        killer = sender;
        gameObject.GetComponent<CapsuleCollider>().enabled = false;
        gameObject.GetComponent<Rigidbody>().isKinematic = true;
        Array.ForEach(Animations, a => a.SetTrigger("onDeath"));

        BulletDeletion(sender);

        killer.GetComponent<Entity>().AddWisp(this);
    }

    public virtual void DeathChange()
    {
        Destroy(this.gameObject);
    }

    public void AddWisp(Entity deadEntity)
    {
        if (deadEntity is Player)
        {
            Player deadPlayer = deadEntity as Player;
            PlayerCameras.Add(deadPlayer.PlayerCameras[0]);
            deadPlayer.PlayerCameras[0].ObjectToFollow = gameObject;
            WispController wispCon = deadPlayer.PlayerCameras[0].gameObject.GetComponentInChildren<WispController>();
            wispCon.TargetEntity = this;
            attachedWisps.Add(wispCon);
            GameObject wispObject = Instantiate(PlayerWisp);
            wispObject.transform.parent = transform;
            attachedWispObjects.Add(wispObject);
        }
        for (int i = 0; i < deadEntity.attachedWisps.Count; ++i)
        {
            WispController con = deadEntity.attachedWisps[i];
            PlayerCameras.Add(con.gameObject.GetComponentInParent<BetterCamera>());
            con.gameObject.GetComponentInParent<BetterCamera>().ObjectToFollow = gameObject;
            con.TargetEntity = this;
            attachedWisps.Add(con);
            GameObject wispObject = Instantiate(PlayerWisp);
            wispObject.transform.parent = transform;
            attachedWispObjects.Add(wispObject);
        }
    }


    public void BulletDeletion(GameObject sender)
    {
        BulletScript[] components = GameObject.FindObjectsOfType<BulletScript>();

        for (int i = 0; i < components.Length; ++i)
        {
            BulletScript bScript = components[0];
            if (bScript.Sender == gameObject)
                Destroy(bScript.gameObject);
            components = components.Skip(1).ToArray();
        }
    }
}
