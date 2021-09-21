using Assets.UnityFoundation.Code.ObjectPooling;
using UnityEngine;

public class SumoEnemyController : PooledObject
{
    [SerializeField] private float movementSpeed;

    private Rigidbody rigidBody;
    private Transform player;

    private Vector3 originalScale;

    private void Awake()
    {
        originalScale = transform.localScale;

        rigidBody = GetComponent<Rigidbody>();
    }

    void Start()
    {
        var playerGO = GameObject.Find("player");
        if(playerGO != null)
            player = playerGO.transform;
    }

    void Update()
    {
        FollowPlayer();

        if(transform.position.y < -10)
        {
            SumoGameManager.Instance.EnemyFell();
            Deactivate();
        }
    }

    public SumoEnemyController Setup(Vector3 position, float mass)
    {
        transform.position = position;
        transform.localScale = originalScale * mass;
        rigidBody.mass = mass;
        return this;
    }

    private void FollowPlayer()
    {
        if(player == null) {
            Deactivate();
            return;
        }

        try
        {
            var lookDirection = (player.position - transform.position).normalized;
            rigidBody.AddForce(lookDirection * movementSpeed);
        }
        catch(MissingReferenceException)
        {
            Deactivate();
        }
    }
}
