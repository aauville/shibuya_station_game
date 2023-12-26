using System.Collections;
using UnityEngine;

public class StationController : MonoBehaviour
{
    public GameObject CharacterPrefab; 
    public float minSpawnInterval = 10f; // Intervalle minimal entre les apparitions de personnages
    public float maxSpawnInterval = 20f; // Intervalle maximal entre les apparitions de personnages
    private GameObject[] allStations; 
    private bool isCharacterSpawning = false;

    void Start()
    {
        allStations = GameObject.FindGameObjectsWithTag("Station");
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
            character.GetComponent<MyCharacterController>().SetDestination(destinationStation);
        }
    }

    GameObject GetRandomDestinationStation()
    {
        GameObject destination = allStations[Random.Range(0, allStations.Length)];
        while (destination == gameObject)
        {
            destination = allStations[Random.Range(0, allStations.Length)];
        }
        return destination;
    }



}
