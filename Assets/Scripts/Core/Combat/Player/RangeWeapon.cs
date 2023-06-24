using System.Collections;
using Controllers.Player;
using UnityEngine;

public class RangeWeapon : MonoBehaviour
{
    [Header("Shooting")]
    public float fireRate;

    [Header("Reloading")]
    public int currentAmmo;
    public int reserveAmmo;
    public int magSize;
    public float reloadTime;

    [HideInInspector]
    public bool reloading;

    [Header("Bullet")]
    public GameObject bullet;
    public int minDamage;
    public int maxDamage;
    public float bulletSpeed;

    [Header("Bullet styling")]
    [ColorUsageAttribute(true, true)]
    public Color albedo;
    [ColorUsageAttribute(true, true)]
    public Color glow;
    public float glowPower;
    public Gradient trailGradient;

    [Header("Effect styling")]
    [SerializeField]
    private ParticleSystem destructionEffect;
    [ColorUsageAttribute(true, true)]
    public Color emission;

    public Color nonEmissive;

    [SerializeField]
    private Transform bulletOutput;

    private float timeSinceLastShot;

    private Character player;

    private void Start()
    {
        player = PlayerManager.Instance.player.GetComponent<Character>();
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

        currentAmmo = reserveAmmo - magSize > 0 ? magSize : reserveAmmo;
        reserveAmmo -= currentAmmo;

        reloading = false;
    }

    private bool CanShoot() => !reloading && timeSinceLastShot > 1f / (fireRate / 60f) && currentAmmo > 0;

    public void Shoot()
    {
        if (!CanShoot())
            return;

        currentAmmo--;
        timeSinceLastShot = 0;
        OnGunShot();

        if (currentAmmo <= 0 && reserveAmmo > 0)
        {
            StartReload();
        }
    }

    private void Update()
    {
        timeSinceLastShot += Time.deltaTime;
    }

    private void OnGunShot()
    {
        Vector3 bulletOutputWorldPos = bulletOutput.transform.position;
        Vector3 bulletDirection = (player.lookAtPosition - bulletOutputWorldPos);

        bulletDirection.y = 0f;

        GameObject newBullet = Instantiate(bullet, bulletOutputWorldPos, Quaternion.identity);

        newBullet.transform.LookAt(player.lookAtPosition);
        newBullet.transform.rotation = new Quaternion(0, newBullet.transform.rotation.y, 0, newBullet.transform.rotation.w);

        BulletBehaviour newBulletBehaviour = newBullet.GetComponentInChildren<BulletBehaviour>();

        newBulletBehaviour.SetDamage(minDamage, maxDamage);
        newBulletBehaviour.SetSpeed(bulletSpeed);
        newBulletBehaviour.SetDirection(bulletDirection);
        newBulletBehaviour.SetDestructionEffect(destructionEffect);
        newBulletBehaviour.SetBulletStyle(albedo, glow, glowPower, trailGradient);
        newBulletBehaviour.SetBulletDestructionStyle(emission, nonEmissive);
    }
}
