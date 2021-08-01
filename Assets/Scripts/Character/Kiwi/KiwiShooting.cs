using System.Collections;
using UnityEngine;

[RequireComponent(typeof(FuryShootingAbility))]
public class KiwiShooting : BaseShootingProjectile {
    [Tooltip("In seconds")] [SerializeField]
    private float shootingBurstSpeed;
    [SerializeField] private float disperseFactor;
    [SerializeField] private float delayBetweenClick;
    [SerializeField] private float maxDisperseAngle = 15;

    private Health myHealth;
    private FuryShootingAbility furyShootingAbility;

    private int performedBullets;
    private bool shootingAllowed = true;

    private void Start() {
        myHealth = GetComponent<Health>();
        furyShootingAbility = GetComponent<FuryShootingAbility>();
    }

    protected override void Shoot() {
        if (ShootInputVal == InputType.Started) {
            ShootInputVal = InputType.None;
            if (!shootingAllowed) { return; }

            StartCoroutine(nameof(ShootRoutine));
        } else if (ShootInputVal == InputType.Canceled) {
            StopCoroutine(nameof(ShootRoutine));
        }
    }

    private IEnumerator ShootRoutine() {
        StartCoroutine(nameof(DelayBetweenClick));
        performedBullets = 0;
        // CmdShootOneBullet(spawnProjTrans.position, spawnProjTrans.right, );
        CheckFuryAndShoot(spawnProjTrans.right);
        while (true) {
            performedBullets++;
            yield return new WaitForSeconds(shootingBurstSpeed);
            float angle = Mathf.Atan2(spawnProjTrans.right.y, spawnProjTrans.right.x) * Mathf.Rad2Deg;
            float dispersion = Random.Range(-disperseFactor * performedBullets, disperseFactor * performedBullets);
            dispersion = Mathf.Clamp(dispersion, -maxDisperseAngle, maxDisperseAngle);
            angle += dispersion;
            // CmdShootOneBullet(spawnProjTrans.position, new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad)));
            CheckFuryAndShoot(new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad)));
        }
    }

    private void CheckFuryAndShoot(Vector3 direction) {
        if (!furyShootingAbility.IsActive) {
            CmdShootOneBullet(spawnProjTrans.position, direction, dmgBullet);
        } else {
            CmdShootOneBullet(spawnProjTrans.position, direction, dmgBullet * furyShootingAbility.MultiplierDmgToEnemy);
            myHealth.CmdDealDmgNotKillable(dmgBullet * furyShootingAbility.MultiplierDmgSelf);
        }
    }

    private IEnumerator DelayBetweenClick() {
        shootingAllowed = false;
        yield return new WaitForSeconds(delayBetweenClick);
        shootingAllowed = true;
    }
}