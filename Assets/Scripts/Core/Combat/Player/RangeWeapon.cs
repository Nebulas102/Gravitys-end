using System.Collections;
using Controllers.Player;
using UnityEngine;

public class RangeWeapon : MonoBehaviour
{
    [Header("Shooting")]
    public float fireRate;

    [Header("Reloading")]
    public int currentAmmo;
    public int magSize;
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

    private PlayerManager playerManager;

    private void Start()
    {
        playerManager = PlayerManager.Instance;
    }

    private void OnDisable() => reloading = false;

    public void StartReload()
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

    public void Shoot()
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

        Vector3 bulletDirection = playerManager.player.GetComponent<Character>().lookAtPosition - playerManager.player.transform.position;

        newBullet.GetComponent<BulletBehaviour>().SetDamage(startDamage, endDamage);
        newBullet.GetComponent<BulletBehaviour>().SetDirection(bulletDirection);
    }
}
