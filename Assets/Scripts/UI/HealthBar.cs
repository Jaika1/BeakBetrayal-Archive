using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider Slider;
    [HideInInspector]
    public Entity TargetEntity;

    public void UpdateHealthBar()
    {
        Slider.maxValue = TargetEntity.EntityModifiers.MaxHealth;
        Slider.value = TargetEntity.EntityModifiers.Health;
    }
}
