using System;
using Network;
using Support;
using Unity.Netcode;
using UnityEngine;

namespace Projectile
{
    public class MoveProjectile : NetworkBehaviour
    {
        [SerializeField] private float shootForce;
        private Rigidbody _rigidbody;
        
        [SerializeField] private AnalyticsComponent analytics;
        
        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _rigidbody.AddForce(_rigidbody.transform.forward * shootForce * 75.0f);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.GetComponent<NetworkObject>() != null)
            {
                if (GetComponent<NetworkObject>().OwnerClientId == other.gameObject.GetComponent<NetworkObject>().OwnerClientId || !other.gameObject.GetComponent<NetworkObject>().IsOwner)
                {
                    return;
                }
            }
            else
            {
                if (other.gameObject.GetComponentInParent<NetworkObject>() != null)
                {
                    if (GetComponent<NetworkObject>().OwnerClientId == other.gameObject.GetComponentInParent<NetworkObject>().OwnerClientId || !other.gameObject.GetComponentInParent<NetworkObject>().IsOwner)
                    {
                        return;
                    }
                }
                else
                {
                    DestroyProjectileServerRpc();
                    return;
                }
            }
            
            ServerHealthReplicator serverHealthReplicator = null;
            if (other.gameObject.GetComponent<ServerHealthReplicator>() != null)
            {
                serverHealthReplicator = other.gameObject.GetComponent<ServerHealthReplicator>();
            }
            else
            {
                if (other.gameObject.GetComponentInParent<ServerHealthReplicator>() != null)
                {
                    serverHealthReplicator = other.gameObject.GetComponentInParent<ServerHealthReplicator>();
                }
                else
                {
                    DestroyProjectileServerRpc();
                    return;
                }
            }

            serverHealthReplicator.Health -= 25;
            
            if (serverHealthReplicator.Health <= 0)
            {
                analytics.OnPlayerDead(Convert.ToInt32(serverHealthReplicator.OwnerClientId.ToString()));
                Debug.Log("Player " + Convert.ToInt32(serverHealthReplicator.OwnerClientId.ToString()) + " is dead");
                
                serverHealthReplicator.gameObject.GetComponent<ServerPlayerMove>().OnServerRespawnPlayer();
            }
            
            DestroyProjectileServerRpc();
        }
        
        [ServerRpc(RequireOwnership = false)]
        private void DestroyProjectileServerRpc()
        {
            GetComponent<NetworkObject>().Despawn(true);
            Destroy(this);
        }
    }
}
