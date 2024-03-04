using System;
using DG.Tweening;
using DG.Tweening.Plugins.Core.PathCore;
using UnityEngine;

namespace MyUtilities
{
    public class MoveUtil
    {
        /// <summary>
        /// Moves the object to position specified for a given duration
        /// </summary>
        /// <param name="useLocal">Move object in local positions</param>
        /// <param name="toMove">Object to move</param>
        /// <param name="destination">Vector3 of the final position to move to</param>
        /// <param name="speed">duration of the move</param>
        /// <param name="unscaledTime">Use unscaled time</param>
        /// <param name="speedBased">Is movement speed based or duration based</param>
        /// <param name="ease">ease of the movement</param>
        /// <returns></returns>
        public void MoveToPosition(bool useLocal, Transform toMove, Vector3 destination,
            float speed,bool unscaledTime = false,bool speedBased = false, Ease ease = Ease.Linear)
        {
            
            if (!useLocal)
                toMove.DOMove(destination, speed).SetUpdate(unscaledTime).SetEase(ease).SetSpeedBased(speedBased);
            else
                toMove.DOLocalMove(destination, speed).SetUpdate(unscaledTime).SetEase(ease).SetSpeedBased(speedBased);
        }

        /// <summary>
        /// Moves the object to position specified for a given duration
        /// </summary>
        /// <param name="useLocal">Move object in local position</param>
        /// <param name="toMove">Object to move</param>
        /// <param name="destination">Vector3 of the final position to move to</param>
        /// <param name="speed">duration of the move</param>
        /// <param name="unscaledTime">Use unscaled time</param>
        /// <param name="speedBased">Is movement speed based or duration based</param>
        /// <param name="ease">ease of the movement</param>
        /// <param name="callback">callback method that happens after the move has finished</param>
        /// <returns></returns>
        public void MoveToPosition(  bool useLocal, Transform toMove, Vector3 destination,
            float speed,  Action callback,bool unscaledTime = false,bool speedBased = false,Ease ease = Ease.Linear)
        {
            if (!useLocal)
                toMove.DOMove(destination, speed).SetEase(ease).SetUpdate(unscaledTime).OnComplete(() => callback()).SetSpeedBased(speedBased);
            else 
                toMove.DOLocalMove(destination, speed).SetEase(ease).SetUpdate(unscaledTime).OnComplete(() => callback()).SetSpeedBased(speedBased);
        }

        /// <summary>
        /// Moves the object to position specified for a given duration
        /// </summary>
        /// <param name="tween">Out parameter to control the Tween</param>
        /// <param name="useLocal">Move object in local positions</param>
        /// <param name="toMove">Object to move</param>
        /// <param name="destination">Vector3 of the final position to move to</param>
        /// <param name="speed">duration of the move</param>
        /// <param name="unscaledTime">use unscaled time</param>
        /// <param name="speedBased">Is movement speed based or duration based</param>
        /// <param name="ease">ease of the movement</param>
        /// <returns></returns>
        public void MoveToPosition(out Tween tween, bool useLocal, Transform toMove, Vector3 destination,
            float speed,bool unscaledTime = false,bool speedBased = false, Ease ease = Ease.Linear)
        {
            tween = !useLocal ? toMove.DOMove(destination, speed).SetUpdate(unscaledTime).SetEase(ease).SetSpeedBased(speedBased) 
                : toMove.DOLocalMove(destination, speed).SetUpdate(unscaledTime).SetEase(ease).SetSpeedBased(speedBased);
        }

        /// <summary>
        /// Moves the object to position specified for a given duration
        /// </summary>
        /// <param name="tween">Out parameter to control the Tween</param>
        /// <param name="useLocal">Move object in local position</param>
        /// <param name="toMove">Object to move</param>
        /// <param name="destination">Vector3 of the final position to move to</param>
        /// <param name="speed">duration of the move</param>
        /// <param name="unscaledTime">use unscaled time</param>
        /// <param name="speedBased">Is movement speed based or duration based</param>
        /// <param name="ease">ease of the movement</param>
        /// <param name="callback">callback method that happens after the move has finished</param>
        /// <returns></returns>
        public void MoveToPosition( out Tween tween, bool useLocal, Transform toMove, Vector3 destination,
            float speed,  Action callback,bool unscaledTime = false,bool speedBased = false,Ease ease = Ease.Linear)
        {
            tween = !useLocal ? toMove.DOMove(destination, speed).SetUpdate(unscaledTime).SetEase(ease).OnComplete(() => callback()).SetSpeedBased(speedBased)
                : toMove.DOLocalMove(destination, speed).SetEase(ease).OnComplete(() => callback()).SetSpeedBased(speedBased);
        }

        /// <summary>
        /// Moves the object to position specified for a given duration
        /// </summary>
        /// <param name="useLocal">Use local space for rotation</param>
        /// <param name="toRotate">Object to rotate</param>
        /// <param name="rotation">Vector3 of the final rotation to rotate to</param>
        /// <param name="speed">duration of the rotation</param>
        /// <param name="mode">Rotation mode</param>
        /// <param name="ease">ease of the rotation</param>
        /// <param name="tween">Out parameter to control the Tween</param>
        /// <returns></returns>
        public void RotateLike(out Tween tween, bool useLocal, Transform toRotate, Vector3 rotation, float speed,
            RotateMode mode, Ease ease)
        {
            tween = !useLocal ? toRotate.DORotate(rotation, speed, mode).SetEase(ease) : toRotate.DOLocalRotate(rotation, speed, mode).SetEase(ease);
        }

        /// <summary>
        /// Moves the object to position specified for a given duration
        /// </summary>
        /// <param name="tween">Out parameter to control the Tween</param>
        /// <param name="useLocal">Use local space for rotation</param>
        /// <param name="toRotate">Object to rotate</param>
        /// <param name="rotation">Vector3 of the final rotation to rotate to</param>
        /// <param name="speed">duration of the rotation</param>
        /// <param name="mode">Rotation mode</param>
        /// <param name="ease">ease of the rotation</param>
        /// <param name="callback">callback method that happens after the rotation has finished</param>
        /// <param name="unscaledTime">use unscaled time</param>
        /// <returns></returns>
        public void RotateLike(out Tween tween, bool useLocal, Transform toRotate, Vector3 rotation, float speed,
            RotateMode mode, Ease ease,bool unscaledTime, Action callback)
        {
            tween = !useLocal ? toRotate.DORotate(rotation, speed, mode).SetUpdate(unscaledTime).SetEase(ease).OnComplete(() => callback()) : toRotate.DOLocalRotate(rotation, speed, mode).SetEase(ease).OnComplete(() => callback());
        }

        /// <summary>
        /// Moves the object to position specified for a given duration
        /// </summary>
        /// <param name="toMoveAndRotate">Object to move and rotate</param>
        /// <param name="finalTransform">Transform of the object with final position and rotation set</param>
        /// <param name="speed">duration of the whole movement</param>
        /// <param name="mode">Rotation mode</param>
        /// <param name="ease">ease of the whole movement</param>
        /// <param name="unscaledTime">is movement time scale dependent</param>
        /// <param name="speedBased">Is movement speed based or duration based</param>
        /// <returns></returns>
        public void MoveAndRotate(Transform toMoveAndRotate, Transform finalTransform, float speed,
            RotateMode mode, Ease ease,bool unscaledTime = false,bool speedBased = false)
        {
            MoveToPosition(out _, false, toMoveAndRotate, finalTransform.position, speed, null,unscaledTime,speedBased,ease);
            RotateLike(out _, false, toMoveAndRotate, finalTransform.eulerAngles, speed, mode,ease,unscaledTime,null);
        }

        /// <summary>
        /// Move object along the path
        /// </summary>
        /// <param name="toMove">Object to move</param>
        /// <param name="wayPoints">Array of transforms that form the path</param>
        /// <param name="duration">Duration in seconds it takes to finish the path</param>
        /// <param name="pathType">Type of path</param>
        /// <param name="pathMode">Mode of path</param>
        /// <param name="callback">Callback for when the path is finished</param>
        /// <param name="onFail">Did path moving fail</param>
        /// <param name="unscaledTime">use unscaled time</param>
        /// <param name="ease">Movement ease</param>
        public void MoveOnPath(Transform toMove, Transform[] wayPoints, float duration,PathType pathType, PathMode pathMode, Action callback,Action onFail,bool unscaledTime = false,
            Ease ease = Ease.Linear)
        {
            if (wayPoints.Length <= 0)
            {
                Util.Log("No Path Found",Color.red,20);
                onFail?.Invoke();
                return;
            }
            var path = new Vector3[wayPoints.Length];
            for (var index = 0; index < wayPoints.Length; index++)
            {
                path[index] = wayPoints[index].position;
            }

            var p = new Path(pathType,path,10);
            toMove.DOPath(p, duration, pathMode).SetLookAt(0.01f).SetEase(ease).SetUpdate(unscaledTime).OnComplete(()=>callback());
        }
    }
}