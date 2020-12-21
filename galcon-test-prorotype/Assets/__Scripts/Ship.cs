using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Ship : MonoBehaviour
{
    [HideInInspector] public Transform target;

    private NavMeshAgent myAgent;

    private void Start()
    {     
        myAgent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        myAgent.SetDestination(target.position);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.transform == target)
        {
            NeutralPlanet neutralPlanet;
            if (collision.gameObject.GetComponent<NeutralPlanet>())
            {             
                neutralPlanet = collision.gameObject.GetComponent<NeutralPlanet>();
                neutralPlanet.shipsNumber--;
                Destroy(gameObject);
            }          
            if (collision.gameObject.GetComponent<PlayerPlanet>())
            {
                PlayerPlanet playerPlanet = collision.gameObject.GetComponent<PlayerPlanet>();
                playerPlanet.shipsNumber++;
                Destroy(gameObject);
            }
        }
    } 
}
