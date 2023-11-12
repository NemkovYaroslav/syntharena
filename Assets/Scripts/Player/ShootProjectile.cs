using System;
using System.Collections;
using System.Collections.Generic;
using Projectile;
using Support;
using Unity.Netcode;
using UnityEngine;

public class ShootProjectile : NetworkBehaviour
{
    [SerializeField] private GameObject projectile;
    [SerializeField] private Transform shootTransform;

    private int _index = 0;
    
    private NetworkVariable<int> _playerIndexMaterial = new NetworkVariable<int>(0);

    void Update()
    {
        //Debug.Log(" CURRENT ID: " + OwnerClientId + " VALUE " + _playerIndexMaterial.Value);
        
        GetComponent<MeshRenderer>().sharedMaterial = GameObject.Find("StaticManager").GetComponent<StaticVariables>()
            .PlayerMaterial[_playerIndexMaterial.Value];
        
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            ChangeMaterialServerRpc();
            
            GetComponent<MeshRenderer>().sharedMaterial = GameObject.Find("StaticManager").GetComponent<StaticVariables>()
                .PlayerMaterial[_playerIndexMaterial.Value];

            //InstantiateProjectileServerRpc();
        }
    }

    [ServerRpc]
    private void ChangeMaterialServerRpc()
    {
        _playerIndexMaterial.Value = _playerIndexMaterial.Value == 0 ? 1 : 0;
    }
    
    IEnumerator ShootDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
    }
    
    [ServerRpc]
    private void InstantiateProjectileServerRpc(ServerRpcParams serverRpcParams = default)
    {
        GameObject go = Instantiate(projectile, shootTransform.position, shootTransform.rotation);
        go.GetComponent<NetworkObject>().SpawnWithOwnership(serverRpcParams.Receive.SenderClientId);
    }
}