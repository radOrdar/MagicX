public class BananaShooting : BaseShootingProjectile {
    protected override void Shoot() {
        if (ShootInputVal == InputType.Canceled) {
            ShootInputVal = InputType.None;
            if (currentBullets <= 0) { return; }

            CmdShootOneBullet(spawnProjTrans.position, spawnProjTrans.right, dmgBullet);
        }
    }
}