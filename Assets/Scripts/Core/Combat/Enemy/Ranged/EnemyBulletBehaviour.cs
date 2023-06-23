using Controllers.Player;
using UI.Inventory;
using UnityEngine;

public class EnemyBulletBehaviour : MonoBehaviour
{
    private int _minDamage;
    private int _maxDamage;
    private float _speed;
    private ParticleSystem _destructionEffect;
    private Vector3 _direction;

    private GameObject player;

    private void Awake()
    {
        player = PlayerManager.Instance.player;
    }

    private void Start()
    {
        _destructionEffect = Instantiate(_destructionEffect);
    }

    private void Update()
    {
        transform.root.Translate(_direction * _speed * Time.deltaTime, Space.World);
        _destructionEffect.gameObject.transform.position = transform.root.position;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            var armor = player.GetComponent<EquipmentSystem>()._equippedArmor;
            player.GetComponent<PlayerStatsController>().TakeDamage(_minDamage, _maxDamage, armor != null ? armor.GetComponent<Item>().GetArmorModifier() : 0);

            Destroy(transform.root.gameObject);
        }

        if (other.gameObject.CompareTag("Wall") || other.gameObject.CompareTag("Door") || other.gameObject.CompareTag("Enemy"))
        {
            _destructionEffect.Play();
            Destroy(gameObject);
        }
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

    public void SetDestructionEffect(ParticleSystem destructionEffect)
    {
        _destructionEffect = destructionEffect;
    }
}
