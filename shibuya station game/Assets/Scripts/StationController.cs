using System.Collections;
using UnityEngine;
using System.Collections.Generic;

public class StationController : MonoBehaviour
{
    [System.Serializable]
    public class StationMapping
    {
        public GameObject station;
        public GameObject associatedObject;
    }

    public GameObject CharacterPrefab;
    public float minSpawnInterval = 10f;
    public float maxSpawnInterval = 20f;
    public List<StationMapping> stationMappings = new List<StationMapping>();
    private bool isCharacterSpawning = false;
    public float initialSpeed = 5f;
    public Vector2 initialDirection = new Vector2(0f, 0f); // Direction initiale vers la droite

    void Start()
    {
        isCharacterSpawning = true;
        StartCoroutine(SpawnCharacter());
    }

    IEnumerator SpawnCharacter()
    {
        while (isCharacterSpawning)
        {
            yield return new WaitForSeconds(Random.Range(minSpawnInterval, maxSpawnInterval));
            GameObject destinationStation = GetRandomDestinationStation();
            Vector3 spawnposition = transform.position;
            spawnposition.y -= 20; 
            GameObject character = Instantiate(CharacterPrefab, spawnposition, Quaternion.identity);
            GameObject prefabObject = GetAssociatedObject(destinationStation);
        
            Vector3 spawnPosition = transform.position;  
            Quaternion spawnRotation = Quaternion.identity;
            GameObject associatedObject = Instantiate(prefabObject, spawnPosition, spawnRotation);
            associatedObject.transform.localScale = prefabObject.transform.localScale;

            string direction = "none";
            string dirName = destinationStation.name;
            string srcName = gameObject.name;

            if(srcName == "Station 1")
            {
                if (dirName== "Station 2")
                {
                    direction = "tsf";
                }
                else if(dirName == "Exit")
                {
                    direction = "out";
                }
            }
            else if (srcName == "Station 2")
            {
                if (dirName == "Station 1")
                {
                    direction = "tsf";
                }
                else if (dirName == "Exit")
                {
                    direction = "out";
                }
            }
            else if (srcName == "Exit")
            {
                if (dirName == "Station 1" || dirName == "Station 2")
                {
                    direction = "in";
                }
            }


            character.GetComponent<MyCharacterController>().SetDestination(destinationStation, associatedObject,direction);
            // Obtention du composant Rigidbody2D du personnage
            Rigidbody2D rb2d = character.GetComponent<Rigidbody2D>();

            // Vérification si le composant Rigidbody2D existe
            if (rb2d != null)
            {
                // Normalise la direction pour obtenir un vecteur unitaire
                Vector2 normalizedDirection = initialDirection.normalized;

                // Applique la vitesse initiale dans la direction souhaitée
                rb2d.velocity = normalizedDirection * initialSpeed;
            }
            else
            {
                // Affiche un message d'erreur si le composant Rigidbody2D est manquant
                Debug.LogError("Le composant Rigidbody2D est manquant sur le personnage instancié.");
            }

        }
    }

    GameObject GetRandomDestinationStation()
    {
        GameObject destination = stationMappings[Random.Range(0, stationMappings.Count)].station;
        while (destination == gameObject)
        {
            destination = stationMappings[Random.Range(0, stationMappings.Count)].station;
        }
        return destination;
    }

    GameObject GetAssociatedObject(GameObject station)
    {
        foreach (var mapping in stationMappings)
        {
            if (mapping.station == station)
            {
                return mapping.associatedObject;
            }
        }
        return null;
    }
}