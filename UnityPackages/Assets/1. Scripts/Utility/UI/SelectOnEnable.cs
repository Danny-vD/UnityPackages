using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Utility.UI
{
    public class SelectOnEnable : MonoBehaviour
    {
        [SerializeField]
        private Selectable selectable;

        private void OnEnable()
        {
            if (ReferenceEquals(selectable, null))
            {
                return;
            }
            
            selectable.Select();
        }
        
        private void Start()
        {
            if (ReferenceEquals(selectable, null))
            {
                Destroy(this);
            }
        }

        private void OnDisable()
        {
            if (ReferenceEquals(selectable, null))
            {
                return;
            }
        
            if (EventSystem.current == null || EventSystem.current.alreadySelecting)
                return;

            if (ReferenceEquals(EventSystem.current.currentSelectedGameObject, selectable.gameObject))
            {
                EventSystem.current.SetSelectedGameObject(null);   
            }
        }
    }
}
