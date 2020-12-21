using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour
{
    [HideInInspector] public TextMesh displayedShips;
    [HideInInspector] public int shipsNumber;
    [HideInInspector] public int radius;

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Planet")
        {
            Planet planetCol = collision.gameObject.GetComponent<Planet>();
           
            var distance = Vector3.Distance(transform.position, collision.gameObject.transform.position);
            if (distance <= planetCol.radius + radius)
            {
                float offsetX = Random.Range(-20, 20);
                float offsetZ = Random.Range(-20, 20);

                transform.Translate(offsetX, 0, offsetZ);

                if (transform.position.x >= GameController.maxPosX)
                    transform.position = new Vector3(GameController.maxPosX, transform.position.y, transform.position.z);
                if (transform.position.x <= -GameController.maxPosX)
                    transform.position = new Vector3(-GameController.maxPosX, transform.position.y, transform.position.z);
                if (transform.position.z >= GameController.maxPosZ)
                    transform.position = new Vector3(transform.position.x, transform.position.y, GameController.maxPosZ);
                if (transform.position.z <= -GameController.maxPosZ)
                    transform.position = new Vector3(transform.position.x, transform.position.y, -GameController.maxPosZ);
            }
        }
    }

}
