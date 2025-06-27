using System;
using UnityEngine;

public class CopyPosition : MonoBehaviour
{
    [SerializeField] private bool x = true, y = true, z = true;
    [SerializeField] private Transform target;

    private void Update()
    {
        if (target == null) return;

        Vector3 newPos = transform.position;

        if (x) newPos.x = target.position.x;
        if (y) newPos.y = target.position.y;
        if (z) newPos.z = target.position.z;

        transform.position = newPos;
    }
    
    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }
}
