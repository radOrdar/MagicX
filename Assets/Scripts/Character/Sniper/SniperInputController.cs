using UnityEngine.InputSystem;

public class SniperInputController : BaseCharacterInputController {
    private FastReloadShootAbility fastReloadShootAbility;

    public override void OnStartAuthority() {
        base.OnStartAuthority();
        fastReloadShootAbility = GetComponent<FastReloadShootAbility>();
    }

    public override void OnShoot(InputAction.CallbackContext ctx) {
        if (!hasAuthority) { return; }

        if (ctx.canceled) {
            characterShooting.ShootInputVal = InputType.Canceled;
        }
    }

    public void OnFastReload(InputAction.CallbackContext ctx) {
        if (ctx.canceled) {
            fastReloadShootAbility.InputVal = InputType.Canceled;
        }
    }
}