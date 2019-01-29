using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PenguinStateManager : MonoBehaviour
{
    public PenguinPreBaker prebaked;
    public PenguinSpawnController spawnController;
    public PenguinRescueController rescueController;

    public List<GameObject> penguins;
    public GameObject playerPenguin;

    public GameObject playerUnit;

    private static Random rng = new Random();

    // Start is called before the first frame update
    void Start()
    {
        // Get sceneless prebake of penguins
        prebaked = GameObject.FindGameObjectWithTag("_PenguinPrebake").GetComponent<PenguinPreBaker>();

        penguins = new List<GameObject>(prebaked.penguins);
        playerPenguin = prebaked.playerPenguin;

        // Send penguins to spawnController
        spawnController.SpawnPenguins(penguins);
        spawnController.SpawnPlayerPenguin();

        // Tell the RescueController who the initial player penguin is
        rescueController.SetPlayerPenguin(playerPenguin);
    }

    // Perform a Fisher-Yates shuffle of a given array. Essentiall programmatically 
    // drawing from a hat until there's nothing left to draw. 
    public List<GameObject> Shuffle(List<GameObject> penguins)
    {
        int i = penguins.Count;

        while (i > 1)
        {
            i--;
            int n = Random.Range(i, penguins.Count);
            GameObject temp = penguins[n];

            penguins[n] = penguins[i];
            penguins[i] = temp;
        }

        return penguins;
    }
}
