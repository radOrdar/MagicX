using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public abstract class BaseCharacterShooting : NetworkBehaviour {
    [SerializeField] private float projSpeed = 20;
    [SerializeField] private float coolDownTime = 3;
    [SerializeField] private int bulletsPool = 2;
    [SerializeField] protected Transform spawnProjTrans;
    [SerializeField] private GameObject pfBullet;
    [SerializeField] private Transform handAndGunToRotate;
    [SerializeField] private Image magazineReloadIndicator;
    [SerializeField] private GameObject bulletIndicatorPanel;
    [SerializeField] private GameObject bulletIndicator;
    [SerializeField] private ParticleSystem shootEffect;

    public InputType ShootInputVal { protected get; set; } = InputType.None;

    [SyncVar(hook = nameof(HandleCurrentBulletChange))]
    private int currentBullets;
    private List<GameObject> bulletIndicators;
    private Camera mainCamera;


    public override void OnStartClient() {
        bulletIndicators = new List<GameObject>(bulletsPool);
        for (int i = 0; i < bulletsPool; i++) {
            var bullIndic = Instantiate(bulletIndicator, bulletIndicatorPanel.transform);
            bulletIndicators.Add(bullIndic);
        }
    }

    public override void OnStartAuthority() {
        base.OnStartAuthority();
        mainCamera = Camera.main;
        // magazineReloadIndicator.gameObject.SetActive(false);
    }

    public override void OnStartServer() {
        currentBullets = bulletsPool;
    }

    private void Update() {
        if (!hasAuthority) { return; }

        RotateGunToCursor();
        Shoot();
    }

    private void RotateGunToCursor() {
        Vector2 cursorPosWorldPoint = mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        Quaternion targetRotation;
        if (transform.localScale.x >= 0) {
            spawnProjTrans.localRotation = Quaternion.Euler(0, 0, 0);
            // spawnProjTrans.localScale = new Vector3(1, 1, 1);
            targetRotation = Quaternion.FromToRotation(Vector2.right, cursorPosWorldPoint - (Vector2) handAndGunToRotate.position);
            if (cursorPosWorldPoint.x < transform.position.x) {
                targetRotation = Quaternion.Euler(180, 0, -targetRotation.eulerAngles.z);
            }
        } else {
            spawnProjTrans.localRotation = Quaternion.Euler(0, 180, 0);
            targetRotation = Quaternion.FromToRotation(Vector2.left, cursorPosWorldPoint - (Vector2) handAndGunToRotate.position);
            if (cursorPosWorldPoint.x > transform.position.x) {
                targetRotation = Quaternion.Euler(180, 0, -targetRotation.eulerAngles.z);
            }
        }

        handAndGunToRotate.transform.rotation = targetRotation;
        // else if (transform.localScale.x < 0) {
        //     if (cursorPos.x < transform.position.x) {
        //         targetRotation = Quaternion.Euler(0, 0,  90 - targetRotation.eulerAngles.z);
        //     } else {
        //         targetRotation = Quaternion.Euler(180, 0, 90 - targetRotation.eulerAngles.z);
        //     }
        // }

        // handAndGunToRotate.transform.LookAt(cursorPos, Vector3.right);
    }

    protected abstract void Shoot();

    private IEnumerator ReloadMagazineIndicatorRoutine() {
        magazineReloadIndicator.fillAmount = 0;
        magazineReloadIndicator.gameObject.SetActive(true);
        float startTime = Time.time;
        while (Time.time - startTime < coolDownTime) {
            magazineReloadIndicator.fillAmount = (Time.time - startTime) / coolDownTime;
            yield return null;
        }

        magazineReloadIndicator.gameObject.SetActive(false);
    }

    private void HandleCurrentBulletChange(int oldBulletsCount, int newBulletsCount) {
        if (bulletIndicators == null) { return; }

        foreach (var indicator in bulletIndicators) {
            indicator.SetActive(false);
        }

        for (int i = 0; i < newBulletsCount; i++) {
            bulletIndicators[i].SetActive(true);
        }

        if (newBulletsCount == 0) {
            StartCoroutine(ReloadMagazineIndicatorRoutine());
        }
    }

    [ClientRpc]
    private void RpcPlayShootEffect() {
        shootEffect.Play();
    }

    #region Server

    [Command]
    public void CmdShoot(Vector3 shootPos, Vector3 shootDir) {
        if (currentBullets <= 0) { return; }

        currentBullets--;
        GameObject proj = Instantiate(pfBullet, shootPos, Quaternion.Euler(new Vector3(0, 0, Mathf.Atan2(shootDir.y, shootDir.x) * Mathf.Rad2Deg)));
        proj.GetComponent<Bullet>().Initialize(shootDir * projSpeed, gameObject);
        NetworkServer.Spawn(proj, connectionToClient);

        RpcPlayShootEffect();
        if (currentBullets <= 0) {
            StartCoroutine(ReloadMagazineRoutine());
        }
    }

    private IEnumerator ReloadMagazineRoutine() {
        yield return new WaitForSeconds(coolDownTime);
        currentBullets = bulletsPool;
    }

    #endregion

    // public enum ShootInput {
    //     Started,
    //     Canceled,
    //     None
    // }
}