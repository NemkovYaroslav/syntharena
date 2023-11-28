using Support;
using Unity.Netcode;
using UnityEngine;

namespace Network
{
    [DefaultExecutionOrder(0)]
    public class ServerPlayerMove : NetworkBehaviour
    {
        [SerializeField] private AnalyticsComponent analytics;
            
        private ServerHealthReplicator _healthTracker;
        
        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            
            if (!IsServer)
            {
                //enabled = false;
                return;
            }
            
            OnServerSpawnPlayer();
        }

        private void Start()
        {
            _healthTracker = GetComponent<ServerHealthReplicator>();
        }

        public void OnServerRespawnPlayer()
        {
            _healthTracker.Health = 100;

            GetComponent<CharacterController>().enabled = false;
            GetComponent<CharacterController>().transform.position = ServerPlayerSpawnPoints.Instance.ConsumeNextSpawnPoint().transform.position;
            GetComponent<CharacterController>().enabled = true;
        }
        
        void OnServerSpawnPlayer()
        {
            var spawnPoint = ServerPlayerSpawnPoints.Instance.ConsumeNextSpawnPoint();
            var spawnPosition = spawnPoint ? spawnPoint.transform.position : Vector3.zero;
            transform.position = spawnPosition;
        }
    }
}
