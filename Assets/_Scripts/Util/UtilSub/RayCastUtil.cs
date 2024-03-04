using System.Linq;
using UnityEngine;

namespace MyUtilities
{
    public class RayCastUtil
    {
        /// <summary>
        /// Shoots a RayCast from mouse position
        /// </summary>
        /// <param name="tag">Tag of an object you want to check for</param>
        /// <returns>Returns true if raycast from mouse hits the object with a given tag, otherwise returns false.</returns>
        public bool BoolMouseRayCasting(string tag)
        {
            var ray = Camera.main!.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var hit))
            {
                Debug.DrawLine(ray.origin, hit.point, Color.red);
                return hit.transform.CompareTag(tag);
            }

            return false;
        }

        /// <summary>
        /// Shoots a RayCast from mouse position
        /// </summary>
        /// <param name="layer">Layer of an object you want to check for</param>
        /// <returns>Returns true if raycast from mouse hits the object with a given layer, otherwise returns false.</returns>
        public bool BoolMouseRayCasting(int layer)
        {
            var ray = Camera.main!.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var hit))
            {
                Debug.DrawLine(ray.origin, hit.point, Color.red);
                return hit.transform.gameObject.layer == layer;
            }
            else return false;
        }

        /// <summary>
        /// Shoots a RayCast from mouse position
        /// </summary>
        /// <param name="distance">Raycast distance to check</param>
        /// <returns>If the ray hits something at the distance given, returns the RaycastHit, else returns default value of new RaycastHit</returns>
        public RaycastHit MouseDistanceRayCasting(float distance)
        {
            var ray = Camera.main!.ScreenPointToRay(Input.mousePosition);
            return Physics.Raycast(ray, out var hit, distance) ? hit : default;
        }

        /// <summary>
        /// Shoots a RayCast from mouse position
        /// </summary>
        /// <param name="tag">Tag of an object you want to check for</param>
        /// <returns>If ray hits an object with a given tag ,returns Transform of that object, otherwise returns null.</returns>
        public Transform MouseRayCasting(string tag)
        {
            var ray = Camera.main!.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var hit))
            {
                Debug.DrawLine(ray.origin, hit.point, Color.green);
                return hit.transform.CompareTag(tag) ? hit.transform : null;
            }
            else return null;
        }

        /// <summary>
        /// Shoots a RayCast from mouse position
        /// </summary>
        /// <param name="layer">Layer of an object you want to check for</param>
        /// <returns>If ray hits an object with a given layer ,returns Transform of that object, otherwise returns null.</returns>
        public Transform MouseRayCasting(int layer)
        {
            var ray = Camera.main!.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var hit))
            {
                return hit.transform.gameObject.layer == layer ? hit.transform : null;
            }
            else return null;
        }

        /// <summary>
        /// Shoots a RayCast from mouse position
        /// </summary>
        /// <param name="tags">Tags of the objects you want to check for</param>
        /// <param name="hitInfo">RaycastHit info you get from the raycast</param>
        /// <returns>If ray hits an object with a given tag ,returns Transform of that object and RaycastHit information, otherwise returns null.</returns>
        public Transform HitMouseRayCasting(string[] tags, out RaycastHit hitInfo)
        {
            var ray = Camera.main!.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var hit))
            {
                Debug.DrawLine(ray.origin, hit.point, Color.green);
                var t = hit.transform.tag;
                if (tags.Any(tag => tag == t))
                {
                    hitInfo = hit;
                    return hit.transform;
                }
                else
                {
                    hitInfo = default;
                    return null;
                }
            }
            else
            {
                hitInfo = default;
                return null;
            }
        }
        
        /// <summary>
        /// Shoots a RayCast from mouse position
        /// </summary>
        /// <param name="layer">Layer of an object you want to check for</param>
        /// <param name="hitInfo">RaycastHit info you get from the raycast</param>
        /// <returns>If ray hits an object with a given layer ,returns Transform of that object and RaycastHit information, otherwise returns null.</returns>
        public Transform HitMouseRayCasting(int layer, out RaycastHit hitInfo)
        {
            var ray = Camera.main!.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var hit))
            {
                if (hit.transform.gameObject.layer == layer)
                {
                    hitInfo = hit;
                    return hit.transform;
                }
                else
                {
                    hitInfo = default;
                    return null;
                }
            }
            else
            {
                hitInfo = default;
                return null;
            }
        }

        /// <summary>
        /// Raycasts from given transform in a given direction
        /// </summary>
        /// <param name="origin">Transform from which to cast</param>
        /// <param name="direction">Direction to cast in to</param>
        /// <param name="tags">Tags of object we are looking for</param>
        /// <param name="hitInfo">Hit info of the object hit by raycast</param>
        /// <returns>Transform of the hit object</returns>
        public Transform TransformRayCasting(Transform origin,Vector3 direction,string[] tags, out RaycastHit hitInfo)
        {
            var ray = new Ray(origin.position,direction);
            if (Physics.Raycast(ray, out var hit))
            {
                Debug.DrawLine(ray.origin, hit.point, Color.green);
                var t = hit.transform.tag;
                if (tags.Any(tag => tag == t))
                {
                    hitInfo = hit;
                    return hit.transform;
                }
                else
                {
                    hitInfo = default;
                    return null;
                }
            }
            else
            {
                hitInfo = default;
                return null;
            }
        }

        /// <summary>
        /// Raycasts from given transform in a given direction
        /// </summary>
        /// <param name="origin">Transform from which to cast</param>
        /// <param name="direction">Direction to cast in to</param>
        /// <param name="tags">Tags of object we are looking for</param>
        /// <param name="distance">distance of a ray</param>
        /// <returns>Transform of the hit object</returns>
        public Transform TransformRayCasting(Transform origin,Vector3 direction,string[] tags,float distance = Mathf.Infinity)
        {
            var ray = new Ray(origin.position,direction);
            if (Physics.Raycast(ray, out var hit,distance))
            {
                Debug.DrawLine(ray.origin, hit.point, Color.green);
                var t = hit.transform.tag;
                if (tags.Any(tag => tag == t))
                {
                    return hit.transform;
                }
                return null;
            }
            return null;
        }
    }
}