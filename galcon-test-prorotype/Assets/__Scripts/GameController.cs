using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    static public GameController S;

    [HideInInspector] public List<PlayerPlanet> attackers;

    [SerializeField] private GameObject planetPref;
    [HideInInspector] private List<Planet> planets;

    [Header("GAME SETTINGS")]

    [SerializeField] private int numPlanetsMin;
    [SerializeField] private int numPlanetsMax;
    [SerializeField] private int planetScaleMin;
    [SerializeField] private int planetScaleMax;
    [SerializeField] private int scaleOffsetDistance;

    public const float maxPosX = 53.5f;
    public const float maxPosZ = 27.5f;
 
    void Start()
    {
        S = this;

        planets = new List<Planet>();
        attackers = new List<PlayerPlanet>();

        int num = Random.Range(numPlanetsMin, numPlanetsMax);

        for (int i = 0; i < num; i++)
        {
            CreatePlanet();
        }

        ChoosePlayerPlanet();

        foreach (Planet planet in planets)
        {
            planet.gameObject.AddComponent(typeof(NeutralPlanet));
        }
    }

    public void CreatePlanet()
    {
        GameObject planetGO = Instantiate(planetPref);
        Planet planet = planetGO.GetComponent<Planet>();
        planets.Add(planet);

        int scale = Random.Range(planetScaleMin, planetScaleMax);
        planet.transform.localScale = new Vector3(scale, scale, scale);
        planet.radius = scale;

        Vector3 offset = Random.insideUnitSphere * scaleOffsetDistance;
        offset.y = 0;
    }

    private void ChoosePlayerPlanet()
    {
        int rand = Random.Range(0, planets.Count);
        Planet player = planets[rand];
        PlayerPlanet playerStarterPlanet = player.gameObject.AddComponent(typeof(PlayerPlanet)) as PlayerPlanet;
        playerStarterPlanet.gameObject.layer = 11;
        playerStarterPlanet.shipsNumber = 50;
        planets.Remove(player);
    }
}
