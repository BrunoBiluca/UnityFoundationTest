using Assets.UnityFoundation.Code.TimeUtils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DissolveTrigger : MonoBehaviour
{
    [SerializeField] private bool loop;

    private SkinnedMeshRenderer rend;
    private float dissolveSpeed = 1f;
    private bool dissolveReverse = false;

    private Timer dissolveLoopTimer;

    private void Awake()
    {
        rend = GetComponentInChildren<SkinnedMeshRenderer>();

        if(loop)
            dissolveLoopTimer = new Timer(3f, () => Render()).Loop().Start();
    }

    void Update()
    {
        if(Keyboard.current.dKey.wasPressedThisFrame)
        {
            Render();
        }
    }

    private void Render()
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
