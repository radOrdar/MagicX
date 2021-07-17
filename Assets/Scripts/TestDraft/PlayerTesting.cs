using System;
using Mirror;
using UnityEngine;

public class PlayerTesting : NetworkBehaviour {

    [SerializeField] private GameObject squarePref;

    public override void OnStartServer() {
        base.OnStartServer();
        var instantiate = Instantiate(squarePref);
        NetworkServer.Spawn(instantiate);
        // NetworkServer.Spawn(instantiate);
    }
}
