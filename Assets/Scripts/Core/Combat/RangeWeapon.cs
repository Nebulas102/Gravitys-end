using System.Collections;
using System.Collections.Generic;
using Core.Enemy;
using ScriptableObjects;
using UnityEngine;

public class RangeWeapon : MonoBehaviour
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

    private void Start() {
        PlayerShoot.shootInput += Shoot;
        PlayerShoot.reloadEvent += StartReload;
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
    }

    private void Update() 
    {
        timeSinceLastShot += Time.deltaTime;

        // Debug.DrawRay(bulletOutput.position, bulletOutput.forward * maxDistance);
    }

    private void OnGunShot()
    {
        Vector3 bulletOutputWorldPos = bulletOutput.TransformPoint(Vector3.zero);

        GameObject newBullet = Instantiate(bullet, bulletOutputWorldPos, bullet.transform.rotation);

        newBullet.GetComponent<BulletBehaviour>().SetDamage(startDamage, endDamage);
    }
}
