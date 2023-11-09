using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Sample
{
    public class RandomPointsPlayerSpawner : MonoBehaviour
    {
        [SerializeField]
        List<GameObject> m_SpawnPoints = new List<GameObject>();
        
        /*
        static RandomPointsPlayerSpawner s_Instance;
        
        public static RandomPointsPlayerSpawner Instance
        {
            get
            {
                if (s_Instance == null)
                {
                    s_Instance = FindObjectOfType<RandomPointsPlayerSpawner>();
                }

                return s_Instance;
            }
        }

        void OnDestroy()
        {
            s_Instance = null;
        }
        
        public GameObject GetNextSpawnPoint()
        {
            var index = Random.Range(0, m_SpawnPoints.Count);
            Debug.Log("Spawn Index: " + index);
            return m_SpawnPoints[index];
        }
        */
        
        void Start()
        {
            NetworkManager.Singleton.ConnectionApprovalCallback += OnConnectionApprovalCallback;
        }
 
        void OnConnectionApprovalCallback(NetworkManager.ConnectionApprovalRequest request, NetworkManager.ConnectionApprovalResponse response)
        {
            response.Approved = true;
            response.CreatePlayerObject = true;
            response.Position = GetPlayerSpawnPosition();
        }
 
        Vector3 GetPlayerSpawnPosition()
        {
            Vector3 spawnPoint = m_SpawnPoints[Random.Range(0, m_SpawnPoints.Count)].transform.position;
            return spawnPoint;
        }
    }
}
