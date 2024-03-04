using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    
    public List<Transform> targets = new List<Transform>();
    public Transform player;
    
    [Space]
    [Header("General settings")]
    public Vector3 camOffset;
    public Vector3 lookOffset;
    public float camFollowSpeed, camLookSpeed;

    [Space] [Header("Non player settings")]
    public int followIndex;
    public int lookAtIndex;

    [Space]
    [Header("Following")] 
    public bool followDirectionOnly;
    public bool followPlayer;
    public bool followMultiple;
    
    [Space]
    [Header("Looking")]
    public bool lookAtPlayer;
    public bool lookNone;
    public bool smoothLook;

    private readonly List<Transform> _allObjects = new List<Transform>();


    private void Start()
    {
        _allObjects.Add(player);
        foreach (var t in targets)
        {
            _allObjects.Add(t);
        }
    }

    private void FixedUpdate()
    {
        #region Moving

        if (!followDirectionOnly)
        {
            Vector3 toFollow;

            if (followPlayer)
                toFollow = player.position;
            else if (followMultiple)
                toFollow = GetCenter();
            else
                toFollow = targets[followIndex].position;

            transform.position = Vector3.Lerp(transform.position, toFollow + camOffset,
                camFollowSpeed * Time.deltaTime);
        }
        else
        {
            Vector3 toFollow;

            if (followPlayer)
                toFollow = new Vector3(0, 0, player.position.z);
            else if (followMultiple)
                toFollow = new Vector3(0, 0, GetCenter().z);
            else
                toFollow = new Vector3(0, 0, targets[followIndex].position.z);

            transform.position = Vector3.Lerp(transform.position, toFollow + camOffset,
                camFollowSpeed * Time.fixedDeltaTime);
        }

        #endregion

        #region Rotating
        
        if (lookNone || !smoothLook) return;

        var targetRotation = lookAtPlayer
            ? Quaternion.LookRotation(player.position - transform.position+lookOffset)
            : Quaternion.LookRotation(targets[lookAtIndex].position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, camLookSpeed * Time.fixedDeltaTime);

        #endregion
    }

    private Vector3 GetCenter()
    {
        if (_allObjects.Count == 1)
        {
            return _allObjects[0].position;
        }

        var bounds = new Bounds(targets[0].position, Vector3.zero);
        foreach (Transform t in _allObjects)
        {
            bounds.Encapsulate(t.position);
        }

        return bounds.center;
    }

    private void Update()
    {
        if (lookNone || smoothLook) return;
        
        transform.LookAt(lookAtPlayer ? player.position : targets[lookAtIndex].position);

    }

    public void Shake()
    {
        transform.DOShakePosition(.1f, 5);
    }
    public void LookAtPlayerAndFollow()
    {
        followPlayer = true;
        lookAtPlayer = true;
    }
}