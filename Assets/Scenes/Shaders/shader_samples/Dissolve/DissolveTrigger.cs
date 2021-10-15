using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DissolveTrigger : MonoBehaviour
{
    private SkinnedMeshRenderer rend;
    private float dissolveSpeed = 1f;
    private bool dissolveReverse = false;

    private void Awake()
    {
        rend = GetComponentInChildren<SkinnedMeshRenderer>();
    }

    void Update()
    {
        if(Keyboard.current.dKey.wasPressedThisFrame)
        {
            foreach(var mat in rend.materials)
            {
                mat.SetInt("_DissolveReverse", dissolveReverse ? 1 : 0);
                mat.SetFloat("_DissolveSpeed", dissolveSpeed);
                mat.SetFloat("_DissolveTime", Time.time);
            }
            dissolveReverse = !dissolveReverse;
        }
    }
}
