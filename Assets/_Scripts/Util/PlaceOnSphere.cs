#if UNITY_EDITOR

using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;
[CustomEditor(typeof(PlaceOnSphere))]
public class PlaceOnSphereEditor : Editor
{
    private bool randomRotation;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        randomRotation = EditorGUILayout.Toggle("Random rotation", randomRotation);
    }

    private void OnSceneGUI()
    {
        PlaceOnSphere p = target as PlaceOnSphere;

        if (p.useTool && Event.current.type == EventType.KeyDown && Event.current.keyCode == p.input)
        {
            Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform != null)
                {
                    p.SpawnObject(hit,randomRotation);
                    //p.MoveAgent(hit);
                }
            }
        }
    }
}

public class PlaceOnSphere : MonoBehaviour
{
    public bool useTool;

    public List<GameObject> objectsToSpawn = new();
    [SerializeField] public KeyCode input = KeyCode.E;

    public NavMeshAgent agent;
    public void SpawnObject(RaycastHit hit, bool randomRotation)
    {
        var random = Random.Range(0, objectsToSpawn.Count);
        var obj = Instantiate(objectsToSpawn[random]);
        var y = obj.transform.position.y;
        var hitPoint = hit.point;
        hitPoint.y += y;
        obj.transform.position = hitPoint;
        obj.transform.up = hit.normal;

        if (randomRotation)
        {
            obj.transform.Rotate(0, Random.Range(0, 360), 0);
        }
    }

    public void MoveAgent(RaycastHit hit)
    {
        agent.SetDestination(hit.point);
    }
}
#endif