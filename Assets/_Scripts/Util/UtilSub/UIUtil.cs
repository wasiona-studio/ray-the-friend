using UnityEngine;
using UnityEngine.EventSystems;

namespace MyUtilities
{
    public class UIUtil
    {
        /// <summary>
        /// Checks if the pointer (mouse) is over the UI
        /// </summary>
        /// <param name="touch">is it checking for touches or mouse inputs</param>
        /// <returns>Returns true the pointer is over UI, otherwise returns false.</returns>
        public bool IsMouseOverUI(bool touch)
        {
            if (!touch)
            {
                var es = EventSystem.current;
                return es != null && EventSystem.current.IsPointerOverGameObject();
            }

            foreach (Touch t in Input.touches)
            {
                var id = t.fingerId;
                return EventSystem.current.IsPointerOverGameObject(id);
            }

            return false;
        }

    }
}