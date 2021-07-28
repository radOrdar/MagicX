using UnityEngine.InputSystem;

public class BananaInputController : BaseCharacterInputController {
    private FastShotSelfSlowAbility fastShotSelfSlowAbility;

    public override void OnStartAuthority() {
        base.OnStartAuthority();
        fastShotSelfSlowAbility = GetComponent<FastShotSelfSlowAbility>();
    }

    public override void OnShoot(InputAction.CallbackContext ctx) {
        if (!hasAuthority) { return; }

        if (ctx.canceled) {
            characterShooting.ShootInputVal = InputType.Canceled;
        }
    }

    public void OnFastShotSelfSlow(InputAction.CallbackContext ctx) {
        if (ctx.canceled) {
            fastShotSelfSlowAbility.InputVal = InputType.Canceled;
        }
    }
}