using UnityEngine;

public class DashAbility : BaseCooldownAbility {
    [SerializeField] private float shiftForce;

    private Rigidbody2D rb;
    private BaseCharacterMovement characterMovement;

    public override void OnStartAuthority() {
        rb = GetComponent<Rigidbody2D>();
        characterMovement = GetComponent<BaseCharacterMovement>();
    }

    protected override void TriggerAction() {
        base.TriggerAction();
        Shift();
    }

    private void Shift() {
        characterMovement.RpcDisableMovement(.1f);
        rb.AddForce(transform.right * (shiftForce * transform.localScale.x), ForceMode2D.Impulse);
    }
}