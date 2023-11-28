using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Network
{
    public class ServerPlayerSpawnPoints : MonoBehaviour
    {
        [FormerlySerializedAs("mSpawnPoints")] [FormerlySerializedAs("m_SpawnPoints")] [SerializeField]
        private List<GameObject> spawnPoints;

        private static ServerPlayerSpawnPoints _instance;

        public static ServerPlayerSpawnPoints Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<ServerPlayerSpawnPoints>();
                }

                return _instance;
            }
        }

        private void OnDestroy()
        {
            _instance = null;
        }

        public GameObject ConsumeNextSpawnPoint()
        {
            var toReturn = spawnPoints[Random.Range(0, spawnPoints.Count - 1)];
            return toReturn;
        }
    }
}
