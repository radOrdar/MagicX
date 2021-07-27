using UnityEngine.InputSystem;

public class KiwiInputController : BaseCharacterInputController {
    private ReturnDamageAbility returnDamageAbility;

    public override void OnStartAuthority() {
        base.OnStartAuthority();
        returnDamageAbility = GetComponent<ReturnDamageAbility>();
    }

    public override void OnShoot(InputAction.CallbackContext ctx) {
        if (ctx.started) {
            characterShooting.ShootInputVal = InputType.Started;
        } else if (ctx.canceled) {
            characterShooting.ShootInputVal = InputType.Canceled;
        }
    }

    public void OnReturnDmg(InputAction.CallbackContext ctx) {
        if (ctx.canceled) {
            returnDamageAbility.ShootInputVal = InputType.Canceled;
        }
    }
}