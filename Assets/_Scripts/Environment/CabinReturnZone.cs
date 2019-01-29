using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CabinReturnZone : MonoBehaviour
{
    public PenguinRescueController rescueController;
    public Transform cabinReturnPoint;

    private List<GameObject> rescuedPenguins;

    // Start is called before the first frame update
    void Start()
    {
        // Since this object is instantiated the way it is, and needs to phone home,
        // let's scrub the scene for the one RescueController that should exist
        rescueController = GameObject.FindGameObjectWithTag("_RescueController").GetComponent<PenguinRescueController>();
        GetComponent<Renderer>().enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        // The player has entered the return zone
        if(other.tag == "_PlayerUnit")
        {
            // Time to see if they've brought any penguins home! If there are any
            // then i will be greater than 0 and the While will enumerate over any
            // penguins who are returning. If there are none, it'll enumerate nothing
            rescuedPenguins = new List<GameObject>(rescueController.penguinsFollowing);
            int i = rescuedPenguins.Count;
            Debug.Log(i);

            while (i > 0)
            {
                i--;
                rescueController.PenguinHasBeenFound(rescuedPenguins[i]);
                rescuedPenguins[i].GetComponent<PenguinNavController>().followDistance = 0f;
                rescuedPenguins[i].GetComponent<PenguinNavController>().GoHome(cabinReturnPoint.position);
            }
        }
    }
}
