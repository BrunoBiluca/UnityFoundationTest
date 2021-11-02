using Assets.UnityFoundation.Systems.HealthSystem;
using System;
using UnityEngine;

public class MegamanProjectile : MonoBehaviour
{
    public event EventHandler OnShootDestroy;

    private Vector3 direction;
    private GameObject player;

    private void Awake()
    {
        Destroy(gameObject, 1f);    
    }

    public void Setup(int direction, GameObject player)
    {
        this.direction = new Vector3(direction, 0, 0);
        this.player = player;
    }

    void Update()
    {
        transform.position += direction * 200f * Time.deltaTime;
    }

    private void OnDestroy()
    {
        OnShootDestroy?.Invoke(this, EventArgs.Empty);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);

        if(!collision.gameObject.TryGetComponent(out IDamageable entity))
            return;

        entity.Damage(1f, player.GetComponent<HealthSystem>().Layer);
    }
}
