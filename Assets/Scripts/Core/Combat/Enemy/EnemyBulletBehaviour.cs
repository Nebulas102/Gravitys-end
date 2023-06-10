using Controllers.Player;
using UI.Tokens;
using UnityEngine;

public class EnemyBulletBehaviour : MonoBehaviour
{
    [SerializeField]
    private float bulletSpeed = 5f;

    private int startDamage;
    private int endDamage;
    private Vector3 direction;
    
    private GameObject player;

    private void Update()
    {
        transform.root.Translate(direction * bulletSpeed * Time.deltaTime, Space.World);

        player = PlayerManager.Instance.player;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            player.GetComponent<PlayerStatsController>().GetPlayerObject().entity.TakeDamage(startDamage, endDamage, 0);

            Destroy(gameObject);
        }

        if (other.gameObject.CompareTag("Wall") || other.gameObject.CompareTag("Door")) Destroy(gameObject);
    }

    public void SetDamage(int _startDamage, int _endDamage)
    {
        startDamage = _startDamage;
        endDamage = _endDamage;
    }

    public void SetDirection(Vector3 direction)
    {
        this.direction = direction.normalized;
    }
}
