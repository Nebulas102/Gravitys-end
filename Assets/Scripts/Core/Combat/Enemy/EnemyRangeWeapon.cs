using System.Collections;
using System.Collections.Generic;
using Controllers.Player;
using Core.Enemy;
using ScriptableObjects;
using UnityEngine;

public class EnemyRangeWeapon : MonoBehaviour
{
    [Header("Shooting")]
    public int startDamage;
    public int endDamage;
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

    [SerializeField]
    private Transform bulletOutput;

    private float timeSinceLastShot;
    private bool isEquipped;
    private RaycastHit hit;

    private PlayerManager playerManager;

    private void Start() {
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

    private void FixedUpdate()
    {
        if (Physics.Raycast(bulletOutput.transform.position, transform.TransformDirection(Vector3.back), out hit, maxDistance))
        {
            if(hit.transform.CompareTag("Player") && gameObject.transform.root.GetComponent<EnemyRangeAttackController>().allowShooting)
            {
                Shoot();
            }

            Debug.DrawRay(bulletOutput.transform.position, transform.TransformDirection(Vector3.back) * hit.distance, Color.yellow);
        }
    }

    private void OnGunShot()
    {
        Vector3 bulletOutputWorldPos = bulletOutput.TransformPoint(Vector3.zero);
        Vector3 bulletDirection = (hit.transform.position - bulletOutputWorldPos).normalized;

        bulletDirection.y = 0f;

        GameObject newBullet = Instantiate(bullet, bulletOutputWorldPos, Quaternion.LookRotation(bulletDirection, Vector3.down));

        newBullet.GetComponent<EnemyBulletBehaviour>().SetDamage(startDamage, endDamage);
        newBullet.GetComponent<EnemyBulletBehaviour>().SetDirection(bulletDirection);
    }
}
