using System;
using Player;
using Unity.Netcode;
using UnityEngine;

namespace Projectile
{
    public class MoveProjectile : NetworkBehaviour
    {
        [HideInInspector] public ShootProjectile projectileOwner;
        [SerializeField] private GameObject hitImpactEffect;
        [SerializeField] private float shootForce;
        private Rigidbody _rigidbody;
        
        public override void OnNetworkSpawn()
        {
            if (!IsOwner)
            {
                return;
            }
        }
        
        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _rigidbody.AddForce(_rigidbody.transform.forward * shootForce * 75.0f);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!NetworkManager.Singleton.IsServer || !NetworkObject.IsSpawned)
            {
                return;
            }
            
            DestroyProjectileServerRpc();
            
            /*
            var playerNetworkObject = other.gameObject.GetComponent<NetworkObject>();
            
            if (playerNetworkObject != null)
            {
                ClientTakeDamageServerRpc(playerNetworkObject.OwnerClientId);
            }
            InstantiateHitImpactEffectServerRpc(other.transform.position);
            */
        }

        /*
        [ServerRpc]
        private void ClientTakeDamageServerRpc(ulong clientId)
        {
            var client = NetworkManager.Singleton.ConnectedClients[clientId]
                .PlayerObject.GetComponent<PlayerControllerAlternative>();
            if (client != null && client.networkPlayerHealth.Value > 0.0f)
            {
                client.networkPlayerHealth.Value -= 10.0f;
                
                Debug.unityLogger.Log(LogType.Log, "Current Health: " + client.networkPlayerHealth.Value);
            }
        }
        */

        /*
        [ServerRpc]
        private void InstantiateHitImpactEffectServerRpc(Vector3 hitPoint)
        {
            GameObject go = Instantiate(hitImpactEffect, transform.position, Quaternion.identity);
            go.GetComponent<NetworkObject>().Spawn();
        }
        */
        
        [ServerRpc(RequireOwnership = false)]
        private void DestroyProjectileServerRpc()
        {
            GetComponent<NetworkObject>().Despawn(true);
            Destroy(this);
        }
    }
}
