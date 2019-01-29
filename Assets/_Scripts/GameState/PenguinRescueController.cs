using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PenguinRescueController : MonoBehaviour
{
    public List<GameObject> penguinsLost;               // Penguins not yet found
    public List<GameObject> penguinsFollowing;          // Penguins currently following player
    public List<GameObject> penguinsFound;              // Penguins who have come home

    public GameObject playerPenguin;

    //  Takes the provided penguin and adds them to the penguinsLost list
    public void AddToPenguinsLost(GameObject penguin)
    {
        penguinsLost.Add(penguin);
    }

    // Add penguin from the list of penguins following the player
    public void AddToPenguinsFollowing(GameObject penguin)
    {
        if (!penguinsFollowing.Contains(penguin))
            penguinsFollowing.Add(penguin);
    }

    // Remove penguin from the list of penguins following the player
    public void RemoveFromPenguinsFollowing(GameObject penguin)
    {
        penguinsFollowing.Remove(penguin);
    }

    // The penguin has been found! Time to take them off the lost list,
    // and put them into the found list
    public void PenguinHasBeenFound(GameObject penguin)
    {
        if(!penguinsFound.Contains(penguin) && penguinsLost.Contains(penguin))
        {
            penguinsLost.Remove(penguin);
            penguinsFollowing.Remove(penguin);
            penguinsFound.Add(penguin);
        }

        if (penguinsFound.Count == 6)
        {
            // Win condition
            StartCoroutine(GoToEndCredits());
        }
    }

    IEnumerator GoToEndCredits()
    {
        yield return new WaitForSeconds(4);
        SceneManager.LoadScene("InsideCredits");
    }
    
    // Sets the specified penguin as the currently active player penguin
    public void SetPlayerPenguin(GameObject penguin)
    {
        playerPenguin = penguin;

        // The player penguin should already be found. If not, add them now.
        if (!penguinsFound.Contains(penguin))
            penguinsFound.Add(penguin);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
