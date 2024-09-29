using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundBrick : MonoBehaviour,IBrickable
{
    [SerializeField] Transform pointCheck;
    [SerializeField] float distanceCheck;
    [SerializeField] LayerMask playerLayer;
    private void Start()
    {
        GameManager.instance.StartBrick(this);
    }
    private void Update()
    {
        CheckPlayer();
    }

    private void CheckPlayer()
    {
       Physics.Raycast(pointCheck.position, Vector3.up, out RaycastHit hit, distanceCheck, playerLayer);
        if (hit.transform == null ) return;
        hit.transform.TryGetComponent(out Player player);
        if (player == null|| transform.GetChild(0).gameObject.activeInHierarchy) return;
        distanceCheck = -1;
        transform.GetChild(0).gameObject.SetActive(true);
        GameManager.instance.RemoveBrick();
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(pointCheck.position, pointCheck.position + new Vector3(0, distanceCheck));
    }
    public void AddBrick()
    {
        distanceCheck += 0.3f;
    }

    public void RemoveBrick()
    {
        
    }

    public void ClearBrick()
    {
        
    }
}
