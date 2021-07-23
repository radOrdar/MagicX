public class BananaShooting : BaseCharacterShooting {

    protected override void Shoot() {
        if (ShootInputVal == ShootInput.Canceled) {
            CmdShoot(spawnProjTrans.position, spawnProjTrans.right);
            ShootInputVal = ShootInput.None;
        }
    }
}