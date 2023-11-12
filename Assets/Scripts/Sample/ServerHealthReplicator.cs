using Unity.Netcode;
using UnityEngine;

namespace Sample
{
    [RequireComponent(typeof(NetworkObject))]
    public class ServerHealthReplicator : NetworkBehaviour
    {
        NetworkVariable<int> m_ReplicatedHealth = new NetworkVariable<int>(100, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    
        public NetworkVariable<int> ReplicatedHealth => m_ReplicatedHealth; 
    
        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            
            m_ReplicatedHealth.Value = 100;
            
            if (!IsServer)
            {
                enabled = false;
                return;
            }
        }
    
        public int Health
        {
            get => m_ReplicatedHealth.Value;
            set => m_ReplicatedHealth.Value = value;
        }
    }
}
