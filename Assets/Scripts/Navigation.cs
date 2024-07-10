using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Navigation : MonoBehaviour, INavigation
{
    //Nav agent
    private NavMeshAgent _agent;

    //Nav Mask
    private int NavMask;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        NavMask = 1 << NavMesh.GetAreaFromName("Walkable");
    }
    /// <summary>
    /// Properly places the Agent on the navmesh IF valid placement
    /// </summary>
    /// <exception cref="System.NotImplementedException"></exception>
    public void InitAgent()
    {
        NavMeshHit hit;
        
        if (NavMesh.SamplePosition(transform.position, out hit, 1, NavMask))
        {
            transform.position = hit.position;
        }
        
    }
    public void MoveTowards(Vector3 newPos)
    {
        _agent.SetDestination(newPos);
    }

    public void Stop()
    {
        _agent.SetDestination(transform.position);
    }

    
}
