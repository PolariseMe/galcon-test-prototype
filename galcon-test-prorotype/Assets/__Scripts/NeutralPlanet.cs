using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NeutralPlanet : Planet
{
    private int minStartShip = 10;
    private int maxStartShip = 50;

    private void Start()
    {
        int rand = Random.Range(minStartShip, maxStartShip);
        shipsNumber = rand;

        displayedShips = transform.Find("ShipCounter").GetComponent<TextMesh>();
    }
    private void Update()
    {
        displayedShips.text = shipsNumber.ToString();

        if (shipsNumber <= 0)
        {
            gameObject.AddComponent(typeof(PlayerPlanet));
            Destroy(this);          
        }
    }

    private void OnMouseDown()
    {
        List<PlayerPlanet> attackers = GameController.S.attackers;

        foreach (PlayerPlanet attacker in attackers)
        {
            attacker.SpawnShips(gameObject.transform);
        }

        NavMeshObstacle navMesh = GetComponent<NavMeshObstacle>();
        navMesh.enabled = false;
    }
}
