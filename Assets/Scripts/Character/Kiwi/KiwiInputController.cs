using UnityEngine;
using UnityEngine.InputSystem;

public class KiwiInputController : BaseCharacterInputController {
    private ReturnDamage returnDamage;

    public override void OnStartAuthority() {
        base.OnStartAuthority();
        returnDamage = GetComponent<ReturnDamage>();
    }

    public override void OnShoot(InputAction.CallbackContext ctx) {
        if (ctx.started) {
            characterShooting.ShootInputVal = InputType.Started;
        } else if (ctx.canceled) {
            characterShooting.ShootInputVal = InputType.Canceled;
        }
    }

    public void OnReturnDmg(InputAction.CallbackContext ctx) {
        Debug.Log("OnReturnCallback");
        if (ctx.canceled) {
            returnDamage.ShootInputVal = InputType.Canceled;
        }
    }
}