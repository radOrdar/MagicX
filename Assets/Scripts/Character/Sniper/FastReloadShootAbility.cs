using Mirror;

public class FastReloadShootAbility : BaseCooldownAbility {
    private SniperShooting sniperShooting;

    private void Start() {
        sniperShooting = GetComponent<SniperShooting>();
    }

    protected override void TriggerAction() {
        base.TriggerAction();
        sniperShooting.FastReload();
    }

    [Command]
    protected override void CmdActivate() {
        if (sniperShooting.CurrentBullets > 0) { return; }

        UseAllBaseRoutines();

        sniperShooting.StopReloadMagazineRoutine();
    }
}