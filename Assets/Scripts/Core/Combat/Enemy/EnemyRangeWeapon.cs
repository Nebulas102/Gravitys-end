using System.Collections;
using System.Collections.Generic;
using Controllers.Player;
using Core.Enemy;
using ScriptableObjects;
using UnityEngine;

public class EnemyRangeWeapon : MonoBehaviour
{
    [Header("Shooting")]
    public float maxDistance;

    [Header("Reloading")]
    public int currentAmmo;
    public int magSize;
    public float fireRate;
    public float reloadTime;
    [HideInInspector]
    public bool reloading;

    [Header("Bullet")]
    public GameObject bullet;
    public int minDamage;
    public int maxDamage;
    public float bulletSpeed;

    [SerializeField]
    private Transform bulletOutput;

    private float timeSinceLastShot;
    private RaycastHit hit;

    private PlayerManager playerManager;
    private Transform enemy;

    private void Start()
    {
        playerManager = PlayerManager.Instance;
    }

    private void OnDisable() => reloading = false;

    private void StartReload()
    {
        if (!reloading && this.gameObject.activeSelf)
            StartCoroutine(Reload());
    }

    private IEnumerator Reload()
    {
        reloading = true;

        yield return new WaitForSeconds(reloadTime);

        currentAmmo = magSize;

        reloading = false;
    }

    private bool CanShoot() => !reloading && timeSinceLastShot > 1f / (fireRate / 60f);

    private void Shoot()
    {
        if (currentAmmo > 0)
        {
            if (CanShoot())
            {
                currentAmmo--;
                timeSinceLastShot = 0;
                OnGunShot();
            }
        }
        else
        {
            if (!reloading)
            {
                StartReload();
            }
        }
    }

    private void Update()
    {
        timeSinceLastShot += Time.deltaTime;
    }

    public void PerformShot()
    {   
        Vector3 bulletOutputWorldPos = transform.root.TransformPoint(bulletOutput.position);

        Debug.Log(transform.root.name);

        if (Physics.Raycast(bulletOutputWorldPos, bulletOutput.TransformDirection(Vector3.back), out hit, maxDistance))
        {   
            Debug.DrawRay(bulletOutputWorldPos, bulletOutput.TransformDirection(Vector3.back), Color.red);
            if (hit.transform.CompareTag("Player"))
            {
                Shoot();
            }
        }
    }

    private void OnGunShot()
    {
        Vector3 bulletOutputWorldPos = bulletOutput.TransformPoint(Vector3.zero);
        Vector3 bulletDirection = (hit.transform.position - bulletOutputWorldPos);

        bulletDirection.y = 0f;

        GameObject newBullet = Instantiate(bullet, bulletOutputWorldPos, Quaternion.identity);

        newBullet.transform.LookAt(hit.transform.position);
        newBullet.transform.rotation = new Quaternion(0, newBullet.transform.rotation.y, 0, newBullet.transform.rotation.w);

        EnemyBulletBehaviour enemyBulletBehaviour = newBullet.GetComponentInChildren<EnemyBulletBehaviour>();

        enemyBulletBehaviour.SetDamage(minDamage, maxDamage);
        enemyBulletBehaviour.SetSpeed(bulletSpeed);
        enemyBulletBehaviour.SetDirection(bulletDirection);

        SoundEffectsManager.instance.PlaySoundEffect(SoundEffectsManager.SoundEffect.EnemyShoots);
    }

    public void SetEnemy(Transform _enemy)
    {
        enemy = _enemy;
    }
}
