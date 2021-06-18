using Assets.UnityFoundation.HealthSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SimpleHealthSystemDemo : MonoBehaviour
{
    [SerializeField] private HealthSystem[] healthSystems;

    [SerializeField] private Button damage10Btn;
    [SerializeField] private Button damage50Btn;
    [SerializeField] private Button healBtn;

    void Start()
    {
        damage10Btn.onClick.AddListener(() => {
            foreach(var healthSystem in healthSystems)
                healthSystem.Damage(healthSystem.BaseHealth * .1f);
        });
        damage50Btn.onClick.AddListener(() => {
            foreach(var healthSystem in healthSystems)
                healthSystem.Damage(healthSystem.BaseHealth * .5f);
        });
        healBtn.onClick.AddListener(() => {
            foreach(var healthSystem in healthSystems)
                healthSystem.HealFull();
        });
    }
}
