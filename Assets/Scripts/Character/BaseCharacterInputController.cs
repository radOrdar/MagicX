using Mirror;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class BaseCharacterInputController : NetworkBehaviour {
   
    [SerializeField] protected BaseCharacterMovement characterMovement;
    [SerializeField] protected BaseCharacterShooting characterShooting;
    [SerializeField] protected UnityEngine.InputSystem.PlayerInput characterInputAsset;

    public override void OnStartAuthority() {
        characterInputAsset.enabled = true;
    }

    public void OnMovement(InputAction.CallbackContext ctx) {
        if (!isLocalPlayer) { return; }
        characterMovement.MoveInput = ctx.ReadValue<float>();
    }

    public void OnJump(InputAction.CallbackContext ctx) {
        if (!isLocalPlayer) { return; }
        if (ctx.canceled) {
            characterMovement.JumpInput = true;
        }
    }

    public abstract void OnShoot(InputAction.CallbackContext ctx);
}
