using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class Player : Entity
{
    [HideInInspector]
    public List<IPickupItem> HeldItems = new List<IPickupItem>();
    [HideInInspector]
    public UI PlayerUI;

    public override void OnDeath(GameObject sender)
    {
        PlayerUI.EnableCanvas(1);
        PlayerCameras[0].GetComponentInChildren<WispController>().EntityNfo = Information;
        base.OnDeath(sender);
    }

    //void FixedUpdate()
    //{
        
    //    if (!WispModifiers.FoVAbilityInEffect && WispModifiers.FoVAbilityDuration > 0.0f)
    //    {
    //        WispModifiers.FoVAbilityInEffect = true;
    //        PlayerCamera.SetFOV(WispAbilties.FoVValue);
    //    }
    //    else
    //    if (WispModifiers.FoVAbilityInEffect && WispModifiers.FoVAbilityDuration <= 0f)
    //    {
    //        WispModifiers.FoVAbilityInEffect = false;
    //        PlayerCamera.ResetFOV();
    //    }
    //}

    public void AddItem(IPickupItem item)
    {
        HeldItems.Add(item);
        PlayerUI.AddItem(item);

        foreach (FieldInfo fi in ItemModifiers.ModifierFields)
        {
            if (fi.FieldType == typeof(bool))
            {
                bool itemValue = (bool)fi.GetValue(item.Modifiers);
                bool entityValue = (bool)fi.GetValue(EntityModifiers);
                fi.SetValue(EntityModifiers, itemValue | entityValue);
            }
            else
            {
                ItemModifierTypeAttribute attrib = fi.GetCustomAttribute<ItemModifierTypeAttribute>();
                ItemModifierType modifierType = attrib != null ? attrib.ModifierType : ItemModifierType.Additive;
                dynamic itemValue = fi.GetValue(item.Modifiers);
                dynamic entityValue = fi.GetValue(EntityModifiers);
                dynamic newValue = modifierType == ItemModifierType.Additive ? entityValue + itemValue : entityValue * itemValue;
                fi.SetValue(EntityModifiers, newValue);
            }
        }
        
        HealthBar.UpdateHealthBar();
        //AttackCooldown *= item.Modifiers.AttackCooldown;
        //AttackDamage += item.Modifiers.Damage;
        //Acceleration += item.Modifiers.Acceleration;
        //SpeedCap += item.Modifiers.SpeedCap;
        //ProjectileSpeed += item.Modifiers.ProjectileSpeed;
        //Health += item.Modifiers.Health;
    }
}
