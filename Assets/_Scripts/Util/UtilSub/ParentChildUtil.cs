using System;
using System.Collections.Generic;
using UnityEngine;

namespace MyUtilities
{
    public class ParentChildUtil
    {
        /// <summary>
        /// Activates the parent of the given object
        /// </summary>
        /// <param name="child">child object to activate parent</param>
        /// <returns></returns>
        public static void ActivateParent(Transform child)
        {
            if (child.transform.parent != null)
                child.transform.parent.gameObject.SetActive(true);
            else
                Debug.LogError("Given object does not have a parent! Please check your references.");
        }

        /// <summary>
        /// Deactivates the parent of the given object
        /// </summary>
        /// <param name="child">child object to activate parent</param>
        /// <returns></returns>
        public static void DeactivateParent(Transform child)
        {
            if (child.transform.parent != null)
                child.transform.parent.gameObject.SetActive(false);
            else
                Debug.LogError("Given object does not have a parent! Please check your references.");
        }

        /// <summary>
        /// Checks if all the children of a gameObject are disabled
        /// </summary>
        /// <param name="parent">Parent of the children</param>
        /// <returns>Returns true if all the children are disabled, else returns false</returns>
        public static bool AllChildrenDisabled(Transform parent)
        {
            foreach (Transform t in parent)
            {
                if (t.gameObject.activeSelf) return false;
            }

            return true;
        }

        /// <summary>
        /// Checks if all the children of a gameObject are enabled
        /// </summary>
        /// <param name="parent">Parent of the children</param>
        /// <returns>Returns true if all the children are enabled, else returns false</returns>
        public static bool AllChildrenEnabled(Transform parent)
        {
            foreach (Transform t in parent)
            {
                if (t.gameObject.activeSelf) return true;
            }

            return false;
        }
        
         /// <summary>
        /// Destroys all children in a given parent;
        /// </summary>
        /// <param name="parent">Parent to remove children from</param>
        public static void RemoveChildren(Transform parent)
        {
            foreach (Transform t in parent)
            {
                UnityEngine.Object.Destroy(t.gameObject);
            }
        }

        /// <summary>
        /// Adds components gotten from children of a given parent to the given list
        /// </summary>
        /// <param name="parent">parent of the children you are getting the components from</param>
        /// <param name="componentList">list to add the children to</param>
        /// <typeparam name="T">Any component</typeparam>
        public static void GetAllChildrenComponents<T>(Transform parent, List<T> componentList) where T : Component
        {
            foreach (Transform child in parent)
            {
                if (child.GetComponent(typeof(T)) != null)
                    componentList.Add(child.GetComponent<T>());
                GetAllChildrenComponents(child, componentList);
            }
        }

        /// <summary>
        /// Adds components gotten from children of a given parent to the given list
        /// </summary>
        /// <param name="parent">parent of the children you are getting the components from</param>
        /// <param name="componentList">list to add the children to</param>
        /// <param name="callback"> On complete callback method</param>
        /// <typeparam name="T">Any component</typeparam>
        public static void GetAllChildrenComponents<T>(Transform parent, List<T> componentList, Action callback)
            where T : Component
        {
            foreach (Transform child in parent)
            {
                if (child.GetComponent(typeof(T)) != null)
                    componentList.Add(child.GetComponent<T>());
                GetAllChildrenComponents(child, componentList);
            }

            callback();
        }
       
    }
}