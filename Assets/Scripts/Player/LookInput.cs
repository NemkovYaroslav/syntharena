using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.OnScreen;

namespace Sample
{
    public class LookInput : OnScreenControl, IPointerMoveHandler, IPointerUpHandler, IPointerDownHandler
    {
        [InputControl(layout = "Vector2")]
        [SerializeField]
        private string m_ControlPath;

        private bool _isPointerDown = false;
        
        protected override string controlPathInternal
        {
            get => m_ControlPath;
            set => m_ControlPath = value;
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
