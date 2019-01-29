using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Takes the available penguin NPCs and allocates them to
 * available spawn points around the game world.
 * Note, this does not handle spawning of the player penguin,
 * that still sits in the PlayerStateManager.
 */
public class PenguinSpawnController : MonoBehaviour
{
    public PenguinPreBaker prebaked;
    public PenguinStateManager stateManager;
    public PenguinRescueController rescueController;

    public List<GameObject> spawnPoints;
    public GameObject playerSpawnPoint;

    public GameObject aiUnit; // The AI penguin prefab
    public GameObject playerUnit; // The player unit prefab


    // Start is called before the first frame update
    public void SpawnPenguins(List<GameObject> penguins)
    {
        // Get sceneless prebake of penguins
        prebaked = GameObject.FindGameObjectWithTag("_PenguinPrebake").GetComponent<PenguinPreBaker>();

        // Find the spawn points in the world
        playerSpawnPoint = GameObject.FindGameObjectWithTag("_PlayerSpawnPoint");
        spawnPoints = new List<GameObject>(GameObject.FindGameObjectsWithTag("_SpawnPoint"));

        // After we've got the penguins out, we don't need to see the spawn points
        // any longer. So let's turn their mesh renderers off now. We'll still need the
        // spawn points active, for later states in the game, so let's not get rid of it
        // entirely.
        foreach (GameObject spawnPoints in spawnPoints)
            spawnPoints.GetComponent<MeshRenderer>().enabled = false;

        playerSpawnPoint.GetComponent<MeshRenderer>().enabled = false;

        // Run shuffler from PenguinStateManager
        spawnPoints = stateManager.Shuffle(spawnPoints);

        // Run through the list of penguins instantiate each prefab 
        // at its assigned spawn location.
        int i = penguins.Count;
        while (i > 0)
        {
            i--;
            GameObject penguin = Instantiate(aiUnit, spawnPoints[i].transform.position, spawnPoints[i].transform.rotation);
            GameObject pengArt = Instantiate(penguins[i], penguin.transform.position, penguin.transform.rotation);
            GameObject pengPoint = penguin.transform.Find("Peng Point").gameObject;

            pengArt.transform.parent = pengPoint.transform;
            pengArt.transform.position = pengPoint.transform.position;
            pengArt.transform.localScale = new Vector3(1, 1, 1);

            rescueController.AddToPenguinsLost(penguin);

            // Tell AI unit to start waiting for the player to show up
            aiUnit.GetComponent<PenguinNavController>().isWaitingForPlayer = true;
        }
    }

    // Instantiate the elected player penguin and make it a child of the 
    // actual player object for movement and camera control
    public void SpawnPlayerPenguin()
    {
        GameObject player = Instantiate(playerUnit, playerSpawnPoint.transform.position, playerSpawnPoint.transform.rotation);
        GameObject playerArt = Instantiate(prebaked.playerPenguin, new Vector3(), new Quaternion());
        GameObject pengPoint = player.transform.Find("Peng Point").gameObject;

        playerArt.transform.parent = pengPoint.transform;
        playerArt.transform.position = pengPoint.transform.position;
        playerArt.transform.localScale = new Vector3(1, 1, 1);

        GameObject.FindGameObjectWithTag("_CameraRig").GetComponent<CameraController>().target = player.transform;
    }
}
