using System;
using System.Collections;
using Mirror;
using Telepathy;
using UnityEngine;

public class PlayerMovement : NetworkBehaviour {
    // [Header("Shift Params")]
    // [SerializeField] private bool enableShiftCooldown;
    // [SerializeField] private int shiftForce;
    // [SerializeField] private int shiftCoolDown = 2;

    [Header("Movement params")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpSpeed;

    [Header("Components")]
    [SerializeField] private Rigidbody2D myRigidbody;
    [SerializeField] private Collider2D myCollider;
    [SerializeField] private Animator animator;

    private bool jumpInput;
    private int jumpCounter;
    private bool isGrounded;

    private float moveInput;
    private bool isMoveDisabled;

    private bool shiftIsReady = true;
    private bool shiftInput;
    private int shiftDirectionSign; //1 or -1 (right or left)

    private Coroutine updateGroundRoutine;

    [ClientCallback]
    private void OnCollisionEnter2D(Collision2D other) {
        if (other.collider.CompareTag("Floor")) {
            if (updateGroundRoutine != null) {
                StopCoroutine(updateGroundRoutine);
            }

            jumpCounter = 0;
            isGrounded = true;
            animator.SetTrigger("Land");
        }
    }

    [ClientCallback]
    private void OnCollisionExit2D(Collision2D other) {
        if (other.collider.CompareTag("Floor")) {
            updateGroundRoutine = StartCoroutine(nameof(UpdateGroundedFlagRoutine));
        }
    }

    private IEnumerator UpdateGroundedFlagRoutine() {
        yield return new WaitForSeconds(.1f);
        isGrounded = false;
    }

    public void UpdateMoveInput(float newMoveInput) {
        moveInput = newMoveInput;
    }

    public void UpdateJumpInput() {
        jumpInput = true;
    }

    // public void UpdateShiftInput(bool newShiftInput, int newShiftSign) {
    //     shiftInput = newShiftInput;
    //     shiftDirectionSign = newShiftSign;
    // }

    [ClientCallback]
    private void FixedUpdate() {
        Move();
        Jump();
        // Shift();
        Flip();
        animator.SetFloat("HorizontalSpeed", Mathf.Abs(myRigidbody.velocity.x));
        animator.SetFloat("VerticalSpeed", myRigidbody.velocity.y);
    }

    private void Move() {
        if (isMoveDisabled) { return; }

        myRigidbody.velocity = new Vector2(moveInput * moveSpeed * Time.fixedDeltaTime, myRigidbody.velocity.y);
    }

    private void Jump() {
        if (!jumpInput) { return; }

        jumpInput = false;

        Vector2 velocity = myRigidbody.velocity;
        if (isGrounded) {
            myRigidbody.velocity = new Vector2(velocity.x, jumpSpeed);
            jumpCounter = 1;
            animator.SetTrigger("Jump");
        } else if (jumpCounter == 1) {
            myRigidbody.velocity = new Vector2(velocity.x, jumpSpeed);
            animator.SetTrigger("Jump");
            jumpCounter = 2;
        }
    }

    // private void Shift() {
    //     if (!shiftInput || !shiftIsReady) { return; }
    //
    //     isMoveDisabled = true;
    //     shiftInput = false;
    //     myRigidbody.AddForce(new Vector2(shiftForce * shiftDirectionSign, 0), ForceMode2D.Impulse);
    //     StartCoroutine(EnableMovementCoroutine());
    //     if (enableShiftCooldown) {
    //         shiftIsReady = false;
    //         StartCoroutine(ShiftCooldownCoroutine());
    //     }
    // }

    private void Flip() {
        float velocityX = myRigidbody.velocity.x;
        if (velocityX < 0) {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }

        if (velocityX > 0) {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

    // private IEnumerator ShiftCooldownCoroutine() {
    //     yield return new WaitForSeconds(shiftCoolDown);
    //     shiftIsReady = true;
    // }
    //
    // private IEnumerator EnableMovementCoroutine() {
    //     yield return new WaitForSeconds(.1f);
    //     isMoveDisabled = false;
    // }
}