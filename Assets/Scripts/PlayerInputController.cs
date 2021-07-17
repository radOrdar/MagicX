using UnityEngine;
using Mirror;
using UnityEngine.InputSystem;

public class PlayerInputController : NetworkBehaviour {
   
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private PlayerShooting playerShooting;
    [SerializeField] private UnityEngine.InputSystem.PlayerInput playerInputAsset;

    public override void OnStartAuthority() {
        playerInputAsset.enabled = true;
    }

    public void OnMovement(InputAction.CallbackContext ctx) {
        // if (!isLocalPlayer) { return; }
        playerMovement.MoveInput = ctx.ReadValue<float>();
    }

    public void OnJump(InputAction.CallbackContext ctx) {
        // if (!isLocalPlayer) { return; }
        if (ctx.canceled) {
            playerMovement.JumpInput = true;
        }
    }

    public void OnShoot(InputAction.CallbackContext ctx) {
        // if (!isLocalPlayer) { return; }

        if (ctx.canceled) {
            playerShooting.ShootInput = true;
        }
    }
}
