using UnityEngine;

public class SumoEnemyController : PooledObject
{
    [SerializeField] private float movementSpeed;

    private Rigidbody rigidBody;
    private Transform player;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();

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
