using System;
using System.Collections;
using System.Collections.Generic;
using Projectile;
using Sample;
using Support;
using Unity.Netcode;
using UnityEngine;

public class ShootProjectile : NetworkBehaviour
{
    [SerializeField] private InputManager inputManager;
    
    [SerializeField] private GameObject projectile;
    [SerializeField] private Transform shootTransform;
    
    private NetworkVariable<int> _playerIndexMaterial = new NetworkVariable<int>(0);

    private bool _canShoot = true;
    private float _shootDelay = 0.5f;

    private void Start()
    {
        _playerIndexMaterial.OnValueChanged += OnMaterialChange;

        inputManager.inputMaster.Player.Fire.started += _ => ClientShootProjectile();
    }

    private void OnMaterialChange(int previousValue, int newValue)
    {
        GetComponentInChildren<Kostil>().gameObject.GetComponent<MeshRenderer>().sharedMaterial 
            = GameObject.Find("StaticManager").GetComponent<StaticVariables>().PlayerMaterial[newValue];
    }
    
    private void ClientShootProjectile()
    {
        if (!IsOwner)
        {
            return;
        }
            
        if (_canShoot)
        {
            _canShoot = false;
            ChangeMaterialServerRpc();
            InstantiateProjectileServerRpc();
            StartCoroutine(ShootDelay(_shootDelay));
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
        _canShoot = true;
        ChangeMaterialServerRpc();
    }
    
    [ServerRpc]
    private void InstantiateProjectileServerRpc(ServerRpcParams serverRpcParams = default)
    {
        GameObject go = Instantiate(projectile, shootTransform.position, shootTransform.rotation);
        go.GetComponent<NetworkObject>().SpawnWithOwnership(serverRpcParams.Receive.SenderClientId);
    }
}