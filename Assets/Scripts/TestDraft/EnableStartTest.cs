using Mirror;
using UnityEngine;

public class EnableStartTest : NetworkBehaviour {
    // private void OnEnable() {
    //     Debug.Log("Enable");
    // }
    //
    // private void Start() {
    //     Debug.Log("Start");
    // }
    public override void OnStartClient() {
        if (netIdentity.isClient) {
            Debug.Log("isClientActive");
        }

        if (netIdentity.isServer) {
            Debug.Log("isServerActive");
        }
        Debug.Log("startClient");
    }

    // [Client]
    private void Print() {
        if (isServer) {
            Debug.Log("PRINT SERVER " + GetComponent<NetworkIdentity>().netId);
        }

        if (isClient) {
            Debug.Log("PRINT " + GetComponent<NetworkIdentity>().netId);
        }
    }

    private void Update() {
        // if (!isLocalPlayer) {
        //     return;
        // }

        Print();
    }
}