using System.Collections;
using Mirror;
using UnityEngine;

public class SniperShooting : BaseCharacterShooting {
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private float impulseOnCollisionMultiplier;

    protected override void Shoot() {
        if (ShootInputVal == InputType.Canceled) {
            ShootInputVal = InputType.None;
            if (currentBullets <= 0) { return; }

            CmdShootOneBullet(spawnProjTrans.position, spawnProjTrans.right, dmgBullet);
        }
    }

    protected override void SpawnBulletDependOnType(Vector3 shootPos, Vector3 shootDir, float dmg) {
        RaycastHit2D hit = Physics2D.Raycast(shootPos, shootDir, 1000f);
        if (hit.collider == null) { return; }

        if (hit.collider.TryGetComponent(out Health health)) {
            health.DealDamage(dmgBullet, gameObject);
        }

        if (hit.collider.CompareTag("Floor")) {
            if (hit.collider.TryGetComponent(out Rigidbody2D otherRb)) {
                // otherRb.AddForceAtPosition(transform.right * rb.mass * impulseOnCollisionMultiplier, closestPoint);
                otherRb.AddForce(spawnProjTrans.right * impulseOnCollisionMultiplier, ForceMode2D.Impulse);
            }
        }

        ShowLineOnShoot(shootPos, hit.point);
    }

    [ClientRpc]
    private void ShowLineOnShoot(Vector3 start, Vector3 end) {
        StartCoroutine(ShootTraceRoutine(start, end));
    }

    [ClientRpc]
    private void StopReloadMagazineIndicatorRoutine() {
        StopCoroutine(reloadMagazineIndicatorRoutine);
        magazineReloadIndicator.gameObject.SetActive(false);
    }

    private IEnumerator ShootTraceRoutine(Vector3 start, Vector3 end) {
        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, end);
        lineRenderer.enabled = true;
        yield return new WaitForSeconds(.02f);
        lineRenderer.enabled = false;
    }

    [Server]
    public void StopReloadMagazineRoutine() {
        StopCoroutine(reloadMagazineRoutine);
    }

    [Command]
    public void FastReload() {
        if (currentBullets > 0) { return; }

        currentBullets = bulletsPool;
        StopReloadMagazineIndicatorRoutine();
    }
}