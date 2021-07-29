using Mirror;
using UnityEngine;

public class FastShotSelfSlowAbility : BaseDurationAbility {
    [SerializeField] private GameObject pfBullet;
    [SerializeField] private float dmgBullet;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private float moveSlowMultiplier;
    [SerializeField] private float jumpSlowMultiplier;
    [SerializeField] private Transform spawnProjTrans;

    private BananaMovement bananaMovement;

    private void Start() {
        bananaMovement = GetComponent<BananaMovement>();
    }

    protected override void TriggerAction() {
        base.TriggerAction();

        bananaMovement.SlowMovement(durationTime, moveSlowMultiplier);
        bananaMovement.SlowJump(durationTime, jumpSlowMultiplier);

        CmdShoot(spawnProjTrans.position, spawnProjTrans.right, dmgBullet);

        // RpcPlayShootEffect();
    }

    [Command]
    private void CmdShoot(Vector3 shootPos, Vector3 shootDir, float dmg) {
        GameObject proj = Instantiate(pfBullet, shootPos, Quaternion.Euler(new Vector3(0, 0, Mathf.Atan2(shootDir.y, shootDir.x) * Mathf.Rad2Deg)));
        proj.GetComponent<Bullet>().Initialize(shootDir * bulletSpeed, gameObject, dmgBullet);
        NetworkServer.Spawn(proj, connectionToClient);
    }
}