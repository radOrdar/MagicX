using Mirror;
using UnityEngine;

public class Slider : MonoBehaviour {
    [SerializeField] private float forcePerMass;
    [SerializeField] private LeftOrRight leftOrRight = LeftOrRight.Right;

    private Vector3 direction;

    // [SerializeField] private float coolDownTime;
    // private SpriteRenderer spriteRenderer;
    // private bool isActive = true;

    // private BoxCollider2D boxCollider2D;

    // private Color32 defaultColor;
    // private Color32 disableColor = Color.black;

    private void Awake() {
        // boxCollider2D = GetComponent<BoxCollider2D>();
        // spriteRenderer = GetComponent<SpriteRenderer>();
        // defaultColor = spriteRenderer.color;
        direction = transform.right * (int)leftOrRight + new Vector3(0, .1f);
    }

    [ServerCallback]
    private void OnCollisionStay2D(Collision2D other) {
        if (other.collider.TryGetComponent(out BaseCharacterMovement bs)) {
            bs.RpcMovePosition(other.transform.position + direction * .3f);
        } else if (other.collider.TryGetComponent(out Rigidbody2D rb)) {
            rb.MovePosition(other.transform.position + direction * .3f);
        }
    }

    [ServerCallback]
    private void OnCollisionExit2D(Collision2D other) {
        if (!other.collider.TryGetComponent(out Rigidbody2D rb)) { return; }

        if (other.gameObject.TryGetComponent(out BaseCharacterMovement baseCharacterMovement)) {
            baseCharacterMovement.RpcStopDisablingMovementRoutine();
            baseCharacterMovement.RpcDisableMovement(2f);
            baseCharacterMovement.RpcAddForce(forcePerMass * rb.mass * direction);
        } else {
            rb.AddForce(forcePerMass * rb.mass * direction, ForceMode2D.Impulse);
        }
    }

    enum LeftOrRight {
        Left = -1,
        Right = 1
    }

    // [ServerCallback]
    // private void OnCollisionEnter2D(Collision2D other) {
    //     if (!other.collider.CompareTag("Player") || !isActive) { return;}
    //     StartCoroutine(CoolDownRoutine());
    //     
    //     other.gameObject.GetComponent<Rigidbody2D>().AddForce((force * direction, ForceMode2D.Impulse);
    //     var baseCharacterMovement = other.gameObject.GetComponent<BaseCharacterMovement>();
    //     baseCharacterMovement.StopDisablingMovementRoutine();
    //     baseCharacterMovement.DisableMovement(2f);
    // }


    // [Server]
    // private IEnumerator CoolDownRoutine() {
    //     boxCollider2D.enabled = false;
    //     isActive = false;
    //     spriteRenderer.color = disableColor;
    //     yield return new WaitForSeconds(coolDownTime);
    //     isActive = true;
    //     boxCollider2D.enabled = true;
    //     spriteRenderer.color = defaultColor;
    // }
}