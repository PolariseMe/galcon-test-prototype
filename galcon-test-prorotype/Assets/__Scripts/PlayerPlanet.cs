using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPlanet : Planet
{
    private GameObject shipPref;
    private GameObject lightGO;

    private bool isActive;
    private int shipsPerSecond = 5;
    private void Start()
    {
        shipPref = Resources.Load("Ship") as GameObject;
        gameObject.layer = 10;

        displayedShips = transform.Find("ShipCounter").GetComponent<TextMesh>();

        Renderer rend = gameObject.GetComponent<Renderer>();
        rend.material.color = new Color(0, 0, 1, 1);

        lightGO = transform.Find("Light").gameObject;
        Light light = lightGO.GetComponent<Light>();
        float radius = gameObject.GetComponent<Planet>().radius;
        light.range = radius * 1.5f;
        lightGO.transform.position = new Vector3(transform.position.x, radius / 2 + radius, transform.position.z);

        CreateShips();
    }

    private void Update()
    {
        displayedShips.text = shipsNumber.ToString();
    }

    private void OnMouseDown()
    {
        isActive = !isActive;
        lightGO.SetActive(isActive);
       
        if (isActive)
            GameController.S.attackers.Add(this);
        else
            GameController.S.attackers.Remove(this);
    }
    public void SpawnShips(Transform target) 
    { 
        if (isActive)
        {
            int sendCount = shipsNumber / 2;
            for (int i = 0; i < sendCount; i++)
            {
                GameObject shipGO = Instantiate(shipPref, transform.position, Quaternion.identity);
                Ship ship = shipGO.GetComponent<Ship>();
                ship.target = target.transform;
                shipsNumber--;
            }
        }
    }
    public void CreateShips()
    {
        shipsNumber += shipsPerSecond;

        Invoke("CreateShips", 1);
    }
}


