public class BananaShooting : BaseCharacterShooting {
    protected override void Shoot() {
        if (ShootInputVal == InputType.Canceled) {
            CmdShoot(spawnProjTrans.position, spawnProjTrans.right);
            ShootInputVal = InputType.None;
        }
    }
}