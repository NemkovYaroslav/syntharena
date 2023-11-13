using System;
using System.Collections;
using System.Collections.Generic;
using Sample;
using Unity.Netcode;
using UnityEngine;

public class DeathZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<ServerHealthReplicator>() != null)
        {
            
        }
    }
}
