using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class StaticVariables : MonoBehaviour
{
    private static List<Material> _playerMaterials = new List<Material>();
    
    public List<Material> PlayerMaterial => _playerMaterials;
    
    void Start()
    {
        _playerMaterials.Add(Resources.Load<Material>("doomguy_calmface"));
        _playerMaterials.Add(Resources.Load<Material>("doomguy_screames"));
    }
}
