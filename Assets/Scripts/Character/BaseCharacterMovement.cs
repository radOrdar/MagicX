using System.Collections;
using Mirror;
using UnityEngine;

public abstract class BaseCharacterMovement : NetworkBehaviour {
    // [Header("Shift Params")]
    // [SerializeField] private bool enableShiftCooldown;
    // [SerializeField] private int shiftForce;
    // [SerializeField] private int shiftCoolDown = 2;

    [Header("Movement params")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpSpeed;

    private float moveSpeedMultiplier = 1;
    private float jumpSpeedMultiplier = 1;

    private Rigidbody2D rb;
    private Animator animator;

    private void Start() {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    public bool JumpInput { private get; set; }
    private int jumpCounter;
    private bool isGrounded;

    public float MoveInput { private get; set; }
    private bool isMoveDisabled;

    // private bool shiftIsReady = true;
    // private bool shiftInput;
    // private int shiftDirectionSign; //1 or -1 (right or left)

    //TODO replace with flag mb?
    private Coroutine updateGroundRoutine;

    [ClientCallback]
    private void OnCollisionEnter2D(Collision2D other) {
        if (!hasAuthority) { return; }

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
        if (!hasAuthority) { return; }

        if (other.collider.CompareTag("Floor")) {
            updateGroundRoutine = StartCoroutine(nameof(UpdateGroundedFlagRoutine));
        }
    }

    [ClientCallback]
    private void FixedUpdate() {
        if (!hasAuthority) { return; }

        Move();
        Jump();
        Flip();
        animator.SetFloat("HorizontalSpeed", Mathf.Abs(rb.velocity.x));
        animator.SetFloat("VerticalSpeed", rb.velocity.y);
    }

    private IEnumerator UpdateGroundedFlagRoutine() {
        yield return new WaitForSeconds(.1f);
        isGrounded = false;
    }

    private void Move() {
        if (isMoveDisabled) { return; }

        rb.velocity = new Vector2(MoveInput * moveSpeed * moveSpeedMultiplier * Time.fixedDeltaTime, rb.velocity.y);
    }

    private void Jump() {
        if (!JumpInput) { return; }

        JumpInput = false;

        Vector2 velocity = rb.velocity;
        if (isGrounded) {
            rb.velocity = new Vector2(velocity.x, jumpSpeed * jumpSpeedMultiplier);
            jumpCounter = 1;
            animator.SetTrigger("Jump");
        } else if (jumpCounter == 1) {
            rb.velocity = new Vector2(velocity.x, jumpSpeed * jumpSpeedMultiplier);
            animator.SetTrigger("Jump");
            jumpCounter = 2;
        }
    }

    //TODO mb flip only on authoritive instance? will it sync?
    private void Flip() {
        float velocityX = rb.velocity.x;
        if (velocityX < 0) {
            transform.localScale = new Vector3(-1, 1, 1);
        }

        if (velocityX > 0) {
            transform.localScale = new Vector3(1, 1, 1);
        }
    }

    public void SlowMovement(float duration, float multiplier) {
        StartCoroutine(SlowMovementRoutine(duration, multiplier));
    }

    private IEnumerator SlowMovementRoutine(float duration, float multiplier) {
        moveSpeedMultiplier = multiplier;
        yield return new WaitForSeconds(duration);
        moveSpeedMultiplier = 1;
    }

    public void SlowJump(float duration, float multiplier) {
        StartCoroutine(SlowJumpRoutine(duration, multiplier));
    }

    private IEnumerator SlowJumpRoutine(float duration, float multiplier) {
        jumpSpeedMultiplier = multiplier;
        yield return new WaitForSeconds(duration);
        jumpSpeedMultiplier = 1;
    }

    // private IEnumerator ShiftCooldownCoroutine() {
    //     yield return new WaitForSeconds(shiftCoolDown);
    //     shiftIsReady = true;
    // }
    // public void UpdateShiftInput(bool newShiftInput, int newShiftSign) {
    //     shiftInput = newShiftInput;
    //     shiftDirectionSign = newShiftSign;
    // }
    //
    // private IEnumerator EnableMovementCoroutine() {
    //     yield return new WaitForSeconds(.1f);
    //     isMoveDisabled = false;
    // }


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
}