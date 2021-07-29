using UnityEngine.InputSystem;

public class BananaInputController : BaseCharacterInputController {
    private FastShotSelfSlowAbility fastShotSelfSlowAbility;
    private InvisibilityAbility invisibilityAbility;

    public override void OnStartAuthority() {
        base.OnStartAuthority();
        fastShotSelfSlowAbility = GetComponent<FastShotSelfSlowAbility>();
        invisibilityAbility = GetComponent<InvisibilityAbility>();
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

    public void OnInvisibility(InputAction.CallbackContext ctx) {
        if (ctx.canceled) {
            invisibilityAbility.InputVal = InputType.Canceled;
        }
    }
}