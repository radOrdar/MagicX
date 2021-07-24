using System.Collections;
using UnityEngine;

public class KiwiShooting : BaseCharacterShooting {
    [Tooltip("In seconds")] [SerializeField]
    private float shootingBurstSpeed;
    [SerializeField] private float disperseFactor;
    [SerializeField] private float delayBetweenClick;
    [SerializeField] private float maxDisperseAngle = 15;

    private int performedBullets;
    private bool shootingAllowed = true;

    protected override void Shoot() {
        if (ShootInputVal == ShootInput.Started) {
            ShootInputVal = ShootInput.None;
            if (!shootingAllowed) { return; }

            StartCoroutine(nameof(ShootRoutine));
        } else if (ShootInputVal == ShootInput.Canceled) {
            StopCoroutine(nameof(ShootRoutine));
        }
    }

    private IEnumerator ShootRoutine() {
        StartCoroutine(nameof(DelayBetweenClick));
        performedBullets = 0;
        CmdShoot(spawnProjTrans.position, spawnProjTrans.right);
        while (true) {
            performedBullets++;
            yield return new WaitForSeconds(shootingBurstSpeed);
            float angle = Mathf.Atan2(spawnProjTrans.right.y, spawnProjTrans.right.x) * Mathf.Rad2Deg;
            float dispersion = Random.Range(-disperseFactor * performedBullets, disperseFactor * performedBullets);
            dispersion = Mathf.Clamp(dispersion, -maxDisperseAngle, maxDisperseAngle);
            angle += dispersion;
            CmdShoot(spawnProjTrans.position, new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad)));
        }
    }

    private IEnumerator DelayBetweenClick() {
        shootingAllowed = false;
        yield return new WaitForSeconds(delayBetweenClick);
        shootingAllowed = true;
    }
}