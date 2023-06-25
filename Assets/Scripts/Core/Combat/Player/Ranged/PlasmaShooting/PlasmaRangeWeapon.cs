public class PlasmaRangeWeapon : RangeWeapon
{
    public float destroyDelay;

    protected override void OnGunShot()
    {
        RegularShotBehavior();
        base.OnGunShot();
    }
}
