using UnityEngine;

namespace Player
{
    public class InputManager : MonoBehaviour
    {
        public InputMaster InputMaster;

        private void Awake()
        {
            InputMaster = new InputMaster();
        }

        private void OnEnable()
        {
            InputMaster.Enable();
        }

        private void OnDisable()
        {
            InputMaster.Disable();
        }
    }
}
