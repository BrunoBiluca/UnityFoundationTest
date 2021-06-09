using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsMenuDemoManager : MonoBehaviour
{
    private AudioEffectsController audioEffectsController;

    private void Start()
    {
        audioEffectsController = FindObjectOfType<AudioEffectsController>();
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            audioEffectsController.PlayClickSound();
        }
    }
}
