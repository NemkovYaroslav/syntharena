using Unity.Netcode;
using UnityEngine;

namespace Network
{
    [RequireComponent(typeof(NetworkObject))]
    public class ServerHealthReplicator : NetworkBehaviour
    {
        public NetworkVariable<int> ReplicatedHealth { get; } = new NetworkVariable<int>(100, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            
            ReplicatedHealth.Value = 100;
            
            if (!IsServer)
            {
                enabled = false;
                return;
            }
        }
    
        public int Health
        {
            get => ReplicatedHealth.Value;
            set => ReplicatedHealth.Value = value;
        }
    }
}
