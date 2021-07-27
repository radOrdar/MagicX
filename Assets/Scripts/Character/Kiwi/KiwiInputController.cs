using UnityEngine.InputSystem;

public class KiwiInputController : BaseCharacterInputController {
    private ReturnDamageAbility returnDamageAbility;
    private FuryShootingAbility furyShootingAbility;

    public override void OnStartAuthority() {
        base.OnStartAuthority();
        returnDamageAbility = GetComponent<ReturnDamageAbility>();
        furyShootingAbility = GetComponent<FuryShootingAbility>();
    }

    public override void OnShoot(InputAction.CallbackContext ctx) {
        if (ctx.started) {
            characterShooting.ShootInputVal = InputType.Started;
        } else if (ctx.canceled) {
            characterShooting.ShootInputVal = InputType.Canceled;
        }
    }

    public void OnReturnDmgAbility(InputAction.CallbackContext ctx) {
        if (ctx.canceled) {
            returnDamageAbility.InputVal = InputType.Canceled;
        }
    }

    public void OnFuryAbility(InputAction.CallbackContext ctx) {
        if (ctx.canceled) {
            furyShootingAbility.InputVal = InputType.Canceled;
        }
    }
}