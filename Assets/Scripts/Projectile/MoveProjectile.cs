using System;
using Player;
using Sample;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Projectile
{
    public class MoveProjectile : NetworkBehaviour
    {
        [SerializeField] private float shootForce;
        private Rigidbody _rigidbody;
        
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
