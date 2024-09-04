using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using VDFramework;

namespace UtilityPackage.Utility.UI
{
    /// <summary>
    /// Select a specific selectable when this object is enabled
    /// </summary>
    public class SelectOnEnable : BetterMonoBehaviour
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
