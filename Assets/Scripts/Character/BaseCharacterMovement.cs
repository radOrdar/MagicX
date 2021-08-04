using System.Collections;
using Mirror;
using UnityEngine;

public abstract class BaseCharacterMovement : NetworkBehaviour {
    [Header("Movement params")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpSpeed;
    private LayerMask groundLayerMask;

    private float moveSpeedMultiplier = 1;
    private float jumpSpeedMultiplier = 1;

    private bool wasGrounded;

    private Rigidbody2D myRb;
    private Collider2D myCollider;
    private Animator animator;

    private void Start() {
        myRb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        myCollider = GetComponent<Collider2D>();
        groundLayerMask = LayerMask.GetMask("Floor");
    }

    public bool JumpInput { private get; set; }
    private int jumpCounter;

    public float MoveInput { private get; set; }
    private bool isMoveDisabled;

    //TODO replace with flag mb?
    private Coroutine updateGroundRoutine;

    [ClientCallback]
    private void OnCollisionEnter2D(Collision2D other) {
        if (!hasAuthority) { return; }

        if (other.collider.CompareTag("Floor")) {
            jumpCounter = 0;
            animator.SetTrigger("Land");
        }
    }

    [ClientCallback]
    private void OnCollisionExit2D(Collision2D other) {
        if (!hasAuthority) { return; }

        if (other.collider.CompareTag("Floor")) {
            StartCoroutine(nameof(UpdateWasGroundedRoutine));
        }
    }

    private IEnumerator UpdateWasGroundedRoutine() {
        wasGrounded = true;
        yield return new WaitForSeconds(.13f);
        wasGrounded = false;
    }

    [ClientCallback]
    private void FixedUpdate() {
        if (!hasAuthority) { return; }

        Move();
        Jump();
        Flip();
        animator.SetFloat("HorizontalSpeed", Mathf.Abs(myRb.velocity.x));
        animator.SetFloat("VerticalSpeed", myRb.velocity.y);
    }

    private void Move() {
        if (isMoveDisabled) { return; }

        myRb.velocity = new Vector2(MoveInput * moveSpeed * moveSpeedMultiplier * Time.fixedDeltaTime, myRb.velocity.y);
    }

    private void Jump() {
        if (!JumpInput) { return; }

        JumpInput = false;

        Vector2 velocity = myRb.velocity;
        if (IsGrounded() || wasGrounded) {
            myRb.velocity = new Vector2(velocity.x, jumpSpeed * jumpSpeedMultiplier);
            jumpCounter = 1;
            animator.SetTrigger("Jump");
        } else if (jumpCounter == 1) {
            myRb.velocity = new Vector2(velocity.x, jumpSpeed * jumpSpeedMultiplier);
            animator.SetTrigger("Jump");
            jumpCounter = 2;
        }
    }

    private bool IsGrounded() {
        float extra = .05f;
        var bounds = myCollider.bounds;
        RaycastHit2D hit = Physics2D.BoxCast(bounds.center, new Vector2(bounds.size.x + extra, bounds.size.y + extra),
            0f, Vector2.down, 0, groundLayerMask);
        return hit.collider != null;
    }

    //TODO mb flip only on authoritive instance? will it sync?
    private void Flip() {
        float velocityX = myRb.velocity.x;
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
}