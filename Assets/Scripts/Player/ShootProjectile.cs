using System;
using System.Collections;
using System.Collections.Generic;
using Projectile;
using Support;
using Unity.Netcode;
using UnityEngine;

public class ShootProjectile : NetworkBehaviour
{
    [SerializeField] private GameObject projectile;
    [SerializeField] private Transform shootTransform;

    //private Material calmFaceMaterial;
    //private Material fireFaceMaterial;

    //private MeshRenderer cubeMeshRenderer;

    private NetworkVariable<NetworkString> _calmFaceMaterialName = new NetworkVariable<NetworkString>("doomguy_calmface");
    private NetworkVariable<NetworkString> _fireFaceMaterialName = new NetworkVariable<NetworkString>("doomguy_screames");
    
    private void Start()
    {
        //calmFaceMaterial = Resources.Load<Material>("doomguy_calmface");
        //fireFaceMaterial = Resources.Load<Material>("doomguy_screames");

        //cubeMeshRenderer = GameObject.Find("RandomCube").GetComponent<MeshRenderer>();
    }

    void Update()
    {
        /*
        if (!IsOwner)
        {
            return;
        }
        */
        
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            ChangeMaterialServerRpc(OwnerClientId);
            InstantiateProjectileServerRpc();
        }
    }

    [ServerRpc]
    private void ChangeMaterialServerRpc(ulong clientId)
    {
        //Debug.Log(" ID: " + clientId + " MATERIAL " + NetworkManager.Singleton.ConnectedClients[clientId].PlayerObject.GetComponentInChildren<Kostil>().gameObject.GetComponent<MeshRenderer>().sharedMaterial);

        //var meshRenderer = NetworkManager.Singleton.ConnectedClients[clientId].PlayerObject.GetComponentInChildren<Kostil>().gameObject.GetComponent<MeshRenderer>();

        var meshRenderer = GameObject.Find("RandomCube").GetComponent<MeshRenderer>();

        var calmFaceMaterial = Resources.Load<Material>(_calmFaceMaterialName.Value);
        var fireFaceMaterial = Resources.Load<Material>(_fireFaceMaterialName.Value);
        
        /*
        if (cubeMeshRenderer.sharedMaterial != calmFaceMaterial)
        {
            cubeMeshRenderer.sharedMaterial = calmFaceMaterial;
        }
        else
        {
            cubeMeshRenderer.sharedMaterial = fireFaceMaterial;
        }
        */

        ///*
        Debug.Log("OLD MATERIAL: " + meshRenderer.sharedMaterial);
        if (meshRenderer.sharedMaterial == calmFaceMaterial)
        {
            Debug.Log(" NOW I AM FIRE ");
            meshRenderer.sharedMaterial = fireFaceMaterial;
        }
        else
        {
            Debug.Log(" NOW I AM CALM ");
            meshRenderer.sharedMaterial = calmFaceMaterial;
        }
        Debug.Log("NEW MATERIAL: " + meshRenderer.sharedMaterial);
        //*/
    }

    IEnumerator ShootDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
    }
    
    [ServerRpc]
    private void InstantiateProjectileServerRpc(ServerRpcParams serverRpcParams = default)
    {
        GameObject go = Instantiate(projectile, shootTransform.position, shootTransform.rotation);
        go.GetComponent<NetworkObject>().SpawnWithOwnership(serverRpcParams.Receive.SenderClientId);
    }
}