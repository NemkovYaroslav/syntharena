using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.OnScreen;

namespace Player
{
    public class LookInput : OnScreenControl, IPointerMoveHandler, IPointerUpHandler, IPointerDownHandler
    {
        [InputControl(layout = "Vector2")]
        [SerializeField]
        private string inputControlPath;

        private bool _isPointerDown = false;
        
        protected override string controlPathInternal
        {
            get => inputControlPath;
            set => inputControlPath = value;
        }
        
        public void OnPointerMove(PointerEventData eventData)
        {
            if (!_isPointerDown) return;
            
            SendValueToControl(eventData.delta);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _isPointerDown = false;
            
            SendValueToControl(Vector2.zero);
        }
        
        public void OnPointerDown(PointerEventData eventData)
        {
            _isPointerDown = true;
        }
    }
}
