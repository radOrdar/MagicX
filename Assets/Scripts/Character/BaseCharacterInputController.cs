using Mirror;
using UnityEngine.InputSystem;

public abstract class BaseCharacterInputController : NetworkBehaviour {
    private BaseCharacterMovement characterMovement;
    protected BaseCharacterShooting characterShooting;

    public override void OnStartAuthority() {
        characterMovement = GetComponent<BaseCharacterMovement>();
        characterShooting = GetComponent<BaseCharacterShooting>();
        GetComponent<UnityEngine.InputSystem.PlayerInput>().enabled = true;
    }

    public void OnMovement(InputAction.CallbackContext ctx) {
        if (!hasAuthority) { return; }

        characterMovement.MoveInput = ctx.ReadValue<float>();
    }

    public void OnJump(InputAction.CallbackContext ctx) {
        if (!hasAuthority) { return; }

        if (ctx.canceled) {
            characterMovement.JumpInput = true;
        }
    }

    public abstract void OnShoot(InputAction.CallbackContext ctx);
}