using UnityEngine.InputSystem;

public class KiwiInputController : BaseCharacterInputController {
    
    public override void OnShoot(InputAction.CallbackContext ctx) {
        if (ctx.started) {
            characterShooting.ShootInputVal = BaseCharacterShooting.ShootInput.Started;
        } else if (ctx.canceled) {
            characterShooting.ShootInputVal = BaseCharacterShooting.ShootInput.Canceled;
        }
    }
}
