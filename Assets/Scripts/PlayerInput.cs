using UnityEngine;
using Mirror;
using UnityEngine.InputSystem;

public class PlayerInput : NetworkBehaviour {
   
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private PlayerShooting playerShooting;
    
    public void OnMovement(InputAction.CallbackContext ctx) {
        if (!isLocalPlayer) { return; }
        playerMovement.UpdateMoveInput(ctx.ReadValue<float>());
    }

    public void OnJump(InputAction.CallbackContext ctx) {
        if (!isLocalPlayer) { return; }
        if (ctx.canceled) {
            playerMovement.UpdateJumpInput();
        }
    }

    public void OnShoot(InputAction.CallbackContext ctx) {
        if (!isLocalPlayer) { return; }

        if (ctx.performed) {
            playerShooting.CmdShoot();            
        }
    }
}
