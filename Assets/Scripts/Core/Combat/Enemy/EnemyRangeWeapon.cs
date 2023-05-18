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

    private PlayerManager playerManager;

    private void Start() {
        PlayerShoot.shootInput += Shoot;
        PlayerShoot.reloadEvent += StartReload;

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

        reloading = false;
    }

    private bool CanShoot() => !reloading && timeSinceLastShot > 1f / (fireRate / 60f);

    private void Shoot() 
    {

        if (CanShoot()) 
        {
            timeSinceLastShot = 0;
            OnGunShot();
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

        Vector3 bulletDirection = playerManager.player.GetComponent<Character>().lookAtPosition - playerManager.player.transform.position;

        newBullet.GetComponent<EnemyBulletBehaviour>().SetDamage(startDamage, endDamage);
        newBullet.GetComponent<EnemyBulletBehaviour>().SetDirection(bulletDirection);
    }
}
