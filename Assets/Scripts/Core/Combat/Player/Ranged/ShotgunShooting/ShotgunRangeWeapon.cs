using UnityEngine;

public class ShotgunRangeWeapon : RangeWeapon
{
    public float angleBetweenBullets;
    public override void Shoot()
    {
        if (!CanShoot())
            return;

        currentAmmo -= countOfProjectilesShot;
        timeSinceLastShot = 0;

        OnGunShot();

        if (currentAmmo <= 0 && reserveAmmo > 0)
        {
            currentAmmo = 0;
            StartReload();
        }
    }

    protected override void OnGunShot()
    {
        Vector3 bulletOutputWorldPos = bulletOutput.transform.position;
        bulletDirection = (player.lookAtPosition - bulletOutputWorldPos);

        bulletDirection.y = 0f;

        float totalSpreadAngle = countOfProjectilesShot * angleBetweenBullets;
        float startOffset = -totalSpreadAngle / 2f;

        for (int i = 0; i < countOfProjectilesShot; i++)
        {
            // Calculate the offset for each bullet
            float angleOffset = startOffset + ((i + 1) * angleBetweenBullets);

            // Rotate the bullet direction by the offset and spread angle
            Quaternion spreadRotation = Quaternion.Euler(0f, angleOffset, 0f);
            Vector3 spreadDirection = spreadRotation * bulletDirection;

            newBullet = Instantiate(projectile, bulletOutputWorldPos, Quaternion.identity);

            // Set the bullet's rotation to face the spread direction
            bulletDirection = spreadDirection;

            // Set the bullet's rotation to face the spread direction
            newBullet.transform.rotation = Quaternion.LookRotation(spreadDirection, Vector3.forward);
            base.OnGunShot();
        }
    }
}
