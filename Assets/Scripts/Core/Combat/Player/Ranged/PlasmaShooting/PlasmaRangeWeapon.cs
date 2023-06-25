public class PlasmaRangeWeapon : RangeWeapon
{
    public float destroyDelay;

    protected override void OnGunShot()
    {
        RegularShotBehavior();

        newBulletBehavior = newBullet.GetComponentInChildren<PlasmaBulletBehavior>();

        newBulletBehavior.SetDamage(minDamage, maxDamage);
        newBulletBehavior.SetSpeed(bulletSpeed);
        newBulletBehavior.SetDirection(bulletDirection);
        newBulletBehavior.SetBulletStyle(albedo, glow, glowPower, trailGradient);
        newBulletBehavior.SetBulletDestructionStyle(standardColor, emission, nonEmissive);
    }
}
