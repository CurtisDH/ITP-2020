﻿using UnityEngine;
using UnityEngine.AI;
using CurtisDH.Scripts.Managers;
//attribute require component

public abstract class AIBase : MonoBehaviour
{
    [SerializeField]
    private NavMeshAgent _agent;
    [SerializeField]
    private float _health;
    [SerializeField]
    private float _speed;
    [SerializeField]
    bool _AICollision = true; // When set to true - AI Will not be able to walk through eachother.

    [SerializeField]
    private int _warFund;
    public int WarFund
    {
        get
        {
            return _warFund;
        }
    }


    // How much money is awarded for killing the enemy.         //Make this value protected??
    // Might add my own twist to the income. replacing this feature.
    // Might influence warfund based on how far the enemy has made it into the course;

    private void OnEnable()
    {
        InitaliseAI();
    }
    public void InitaliseAI()
    {
        if (GetComponent<NavMeshAgent>() != null) //Checks if there is a navmesh agent..
        {
            _agent = GetComponent<NavMeshAgent>();
        }
        else // ..If there is no NavmeshAgent then create one and assign agent to it.
        {
            gameObject.AddComponent<NavMeshAgent>();
            _agent = GetComponent<NavMeshAgent>();
        }
        if (_AICollision == true)
        {
            _agent.obstacleAvoidanceType = ObstacleAvoidanceType.HighQualityObstacleAvoidance;
        }
        else
        {
            _agent.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;
        }
        _agent.Warp(SpawnManager.Instance.StartPos); // had issues with unity saying "NO AGENT ON NAVMESH" using Warp fixes this.
        MoveTo(SpawnManager.Instance.EndPos);
        transform.parent = GameObject.Find("EnemyContainer").transform;
        _agent.speed = _speed;

    }

    public virtual void MoveTo(Vector3 position)
    {
        if (_agent != null)
        {
            _agent.SetDestination(position);
            Debug.Log("Destination Acquired " + position);
        }
        else
        {
            if (GetComponent<NavMeshAgent>() != null) //Checks if there is a navmesh agent..
            {
                _agent = GetComponent<NavMeshAgent>();
            }
            else // ..If there is no NavmeshAgent then create one and assign agent to it.
            {
                Debug.Log("No NavmeshAgent found Assigning one... ");
                gameObject.AddComponent<NavMeshAgent>();
                _agent = GetComponent<NavMeshAgent>();
                Debug.Log("NavmeshAgent Created and Assigned Successfully ");
                _agent.SetDestination(position);
                Debug.Log("Destination Acquired After Creating NavMeshAgent" + position);

            }
        }
    }

    public virtual void onDeath()
    {
        PoolManager.Instance.PooledObjects.Add(this.gameObject); // haven't setup animation transitions yet.
        this.gameObject.transform.parent = null;
        GameManager.Instance.AdjustWarfund(WarFund, true);
        //play death animation and then setactive false, then recyle the gameobj;
        this.gameObject.SetActive(false);
    }
}
