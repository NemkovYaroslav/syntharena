using System.Collections.Generic;
using UnityEngine;

namespace Network
{
    public class StaticVariables : MonoBehaviour
    {
        private static readonly List<Material> PlayerMaterials = new List<Material>();
    
        public List<Material> PlayerMaterial => PlayerMaterials;

        private void Start()
        {
            PlayerMaterials.Add(Resources.Load<Material>("doomguy_calmface"));
            PlayerMaterials.Add(Resources.Load<Material>("doomguy_screames"));
        }
    }
}