using System;
using System.Collections;
using Network;
using Sample;
using Support;
using Unity.Netcode;
using UnityEngine;

namespace Player
{
    public class ShootProjectile : NetworkBehaviour
    {
        [SerializeField] private InputManager inputManager;
    
        [SerializeField] private GameObject projectile;
        [SerializeField] private Transform shootTransform;
    
        [SerializeField] private AnalyticsComponent analytics;
    
        private readonly NetworkVariable<int> _playerIndexMaterial = new NetworkVariable<int>(0);

        private bool _canShoot = true;
        private const float ProjectileShootDelay = 0.5f;

        private void Start()
        {
            _playerIndexMaterial.OnValueChanged += OnMaterialChange;

            inputManager.InputMaster.Player.Fire.started += _ => ClientShootProjectile();
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
                analytics.OnPlayerShot(Convert.ToInt32(OwnerClientId.ToString()));
                Debug.Log("Player: " + Convert.ToInt32(OwnerClientId.ToString()) + " shot");
            
                _canShoot = false;
                ChangeMaterialServerRpc();
                InstantiateProjectileServerRpc();
                StartCoroutine(ShootDelay(ProjectileShootDelay));
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
}