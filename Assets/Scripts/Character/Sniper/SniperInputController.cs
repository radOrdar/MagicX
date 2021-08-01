using UnityEngine.InputSystem;

public class SniperInputController : BaseCharacterInputController {
    public override void OnShoot(InputAction.CallbackContext ctx) {
        if (!hasAuthority) { return; }

        if (ctx.canceled) {
            characterShooting.ShootInputVal = InputType.Canceled;
        }
    }
}