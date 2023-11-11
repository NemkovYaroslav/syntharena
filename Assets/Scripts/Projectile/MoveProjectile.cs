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
        [SerializeField] private GameObject hitImpactEffect;
        [SerializeField] private float shootForce;
        private Rigidbody _rigidbody;
        
        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _rigidbody.AddForce(_rigidbody.transform.forward * shootForce * 75.0f);
        }

        private void OnTriggerEnter(Collider other)
        {
            Debug.Log("Trigger works correctly!");
            
            if (other.gameObject.GetComponent<NetworkObject>() != null)
            {
                if (!other.gameObject.GetComponent<NetworkObject>().IsOwner)
                {
                    return;
                }
            }
            else
            {
                Debug.Log("THERE IS NO NETWORK IN OBJECT");
                
                if (other.gameObject.GetComponentInParent<NetworkObject>() != null)
                {
                    if (!other.gameObject.GetComponentInParent<NetworkObject>().IsOwner)
                    {
                        return;
                    }
                }
                else
                {
                    Debug.Log("THERE IS NO NETWORK IN PARENT");
                    
                    DestroyProjectileServerRpc();
                    return;
                }
            }
            
            ServerHealthReplicator serverHealthReplicator = null;
            NetworkObject networkObject = null;
            if (other.gameObject.GetComponent<ServerHealthReplicator>() != null)
            {
                serverHealthReplicator = other.gameObject.GetComponent<ServerHealthReplicator>();
                networkObject = other.gameObject.GetComponent<NetworkObject>();
                Debug.Log("In body: " + serverHealthReplicator);
            }
            else
            {
                if (other.gameObject.GetComponentInParent<ServerHealthReplicator>() != null)
                {
                    serverHealthReplicator = other.gameObject.GetComponentInParent<ServerHealthReplicator>();
                    networkObject = other.gameObject.GetComponentInParent<NetworkObject>();
                    Debug.Log("In head: " + serverHealthReplicator);
                }
                else
                {
                    Debug.Log("NOTHING");
                    DestroyProjectileServerRpc();
                    return;
                }
            }

            serverHealthReplicator.Health -= 10;
            
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
