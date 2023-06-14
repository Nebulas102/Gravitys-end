using Controllers.Player;
using UI.Tokens;
using UnityEngine;

public class EnemyBulletBehaviour : MonoBehaviour
{
    private int _minDamage;
    private int _maxDamage;
    private float _speed;
    private Vector3 _direction;
    
    private GameObject player;

    private void Start()
    {
        player = PlayerManager.Instance.player;    
    }

    private void Update()
    {
        transform.root.Translate(_direction * _speed * Time.deltaTime, Space.World);
    }

    private void OnCollisionEnter(Collision other)
    {
        Debug.Log(other.gameObject.name);

        if (other.gameObject.CompareTag("Player"))
        {
            player.GetComponent<PlayerStatsController>().GetPlayerObject().entity.TakeDamage(_minDamage, _maxDamage, 0);

            Destroy(gameObject);
        }

        if (other.gameObject.CompareTag("Wall") || other.gameObject.CompareTag("Door") || other.gameObject.CompareTag("Enemy")) Destroy(gameObject);
    }

    public void SetDamage(int minDamage, int maxDamage)
    {
        _minDamage = minDamage;
        _maxDamage = maxDamage;
    }

    public void SetSpeed(float speed)
    {
        _speed = speed;
    }

    public void SetDirection(Vector3 direction)
    {
        _direction = direction.normalized;
    }
}
