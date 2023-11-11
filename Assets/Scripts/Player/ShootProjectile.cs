using System;
using System.Collections;
using System.Collections.Generic;
using Projectile;
using Unity.Netcode;
using UnityEngine;

public class ShootProjectile : NetworkBehaviour
{
    [SerializeField] private GameObject projectile;
    [SerializeField] private Transform shootTransform;
    
    void Update()
    {
        if (!IsOwner)
        {
            return;
        }
        
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            InstantiateProjectileServerRpc();
        }
    }
    
    [ServerRpc]
    private void InstantiateProjectileServerRpc(ServerRpcParams serverRpcParams = default)
    {
        GameObject go = Instantiate(projectile, shootTransform.position, shootTransform.rotation);
        
        //Physics.IgnoreCollision(GetComponent<Collider>(), go.GetComponent<Collider>());
        //Physics.IgnoreCollision(GetComponentInChildren<BoxCollider>(), go.GetComponent<Collider>());
        
        //go.GetComponent<NetworkObject>().Spawn(true);
        
        go.GetComponent<NetworkObject>().SpawnWithOwnership(serverRpcParams.Receive.SenderClientId);
    }
}