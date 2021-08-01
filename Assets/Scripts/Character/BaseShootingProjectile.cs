using Mirror;
using UnityEngine;

public abstract class BaseShootingProjectile : BaseCharacterShooting {
    [SerializeField] private GameObject pfDefaultBullet;
    [SerializeField] private float projSpeed;

    protected override void SpawnBulletDependOnType(Vector3 shootPos, Vector3 shootDir, float dmg) {
        GameObject proj = Instantiate(pfDefaultBullet, shootPos, Quaternion.Euler(new Vector3(0, 0, Mathf.Atan2(shootDir.y, shootDir.x) * Mathf.Rad2Deg)));
        proj.GetComponent<Bullet>().Initialize(shootDir * projSpeed, gameObject, dmg);
        NetworkServer.Spawn(proj, connectionToClient);
    }
}