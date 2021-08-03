using UnityEngine.InputSystem;

public class SniperInputController : BaseCharacterInputController {
    private FastReloadShootAbility fastReloadShootAbility;
    private DashAbility dashAbility;

    public override void OnStartAuthority() {
        base.OnStartAuthority();
        fastReloadShootAbility = GetComponent<FastReloadShootAbility>();
        dashAbility = GetComponent<DashAbility>();
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

    public void OnDash(InputAction.CallbackContext ctx) {
        if (ctx.canceled) {
            dashAbility.InputVal = InputType.Canceled;
        }
    }
}