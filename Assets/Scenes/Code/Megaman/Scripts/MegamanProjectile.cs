using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MegamanProjectile : MonoBehaviour
{
    public event EventHandler OnShootDestroy;

    private Vector3 direction;
    private void Awake()
    {
        Destroy(gameObject, 1f);    
    }

    public void Setup(int direction)
    {
        this.direction = new Vector3(direction, 0, 0);
    }

    void Update()
    {
        transform.position += direction * 200f * Time.deltaTime;
    }

    private void OnDestroy()
    {
        OnShootDestroy?.Invoke(this, EventArgs.Empty);
    }
}
