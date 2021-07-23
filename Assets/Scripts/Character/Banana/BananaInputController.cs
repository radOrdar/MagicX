using UnityEngine.InputSystem;

public class BananaInputController : BaseCharacterInputController {
    
    public override void OnShoot(InputAction.CallbackContext ctx) {
        if (!isLocalPlayer) { return; }

        if (ctx.canceled) {
            characterShooting.ShootInputVal = BaseCharacterShooting.ShootInput.Canceled;
        }
    }
}
