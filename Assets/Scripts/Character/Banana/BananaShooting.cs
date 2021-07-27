public class BananaShooting : BaseCharacterShooting {
    protected override void Shoot() {
        if (ShootInputVal == InputType.Canceled) {
            CmdShootOneBullet(spawnProjTrans.position, spawnProjTrans.right, dmgBullet);
            ShootInputVal = InputType.None;
        }
    }
}