using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/**
 * Takes a GameObject playerUnit and follows that unit through the area,
 * staying at a minimum followDistance away from the followTarget.
 * 
 * Right now, the follow distance must be set at controller start. It doesn't
 * consider changes in the distance during runtime -- unless we later decide we need to?
 */
public class PenguinNavController : MonoBehaviour
{
    public PenguinRescueController rescueController;    // We need to tell the RescueController what this penguin's status is

    public GameObject playerUnit;                       // Caching the playerUnit when available, so we can check its location relative to this 
    public PlayerMovementController playerController;

    public float playerSpottingDistance = 8f;          // The number of game units before this unit will spot the player
    public float followDistance = 5f;                  // The distance the NPC will try to keep from the player

    public float moveSpeed = 5f;
    public float moveSpeedSlow = 3f;
    public float wobbleAngle = 1f;

    public bool isWaitingForPlayer;
    public bool hasSpottedPlayer;
    public bool isFollowingPlayer;
    public bool isPlayerMovingSlow;

    public bool isGoingHome;
    private Vector3 homeCoords;

    private NavMeshAgent agent;
    private Vector3 destination;
    private Vector3 distance;

    private bool wobble;
    private Vector3 currentWobVel;
    private Vector3 lastRot;
    private Vector3 currentRot;

    // Start is called before the first frame update
    void Start()
    {
        playerUnit = GameObject.FindGameObjectWithTag("_PlayerUnit");
    
        // If we're following a target with a playerMovementController, get the 
        // movespeeds from them
        if (playerUnit && playerUnit.GetComponent<PlayerMovementController>())
            playerController = playerUnit.GetComponent<PlayerMovementController>();

        // Since this object is instantiated the way it is, and needs to phone home,
        // let's scrub the scene for the one RescueController that should exist
        rescueController = GameObject.FindGameObjectWithTag("_RescueController").GetComponent<PenguinRescueController>();

        agent = GetComponent<NavMeshAgent>();
        agent.stoppingDistance = followDistance;
        agent.updateRotation = false;
    }

    
    void FixedUpdate()
    {
        // This penguin is currently waiting for the player to show up, so let's
        // try and look around to see if we've seen the player anywhere
        if (playerUnit && isWaitingForPlayer && !isGoingHome)
            LookingForPlayer();
            
        // The penguin has spotted the player, and it's time to follow!
        if (hasSpottedPlayer && !isGoingHome)
            StartFollowingPlayer();

        // Putting an if inside an if, because checking for a boolean is a bit more
        // cost effective than trying to calculate a Vector3.Distance every frame.
        if (isFollowingPlayer || isGoingHome)
        {
            if (isFollowingPlayer)
            {
                // If the unit is far enough away to catch up, it follows the player
                if (Vector3.Distance(playerUnit.transform.position, transform.position) > followDistance)
                {
                    float distance = Vector3.Distance(playerUnit.transform.position, transform.position);
                    // Debug.Log(name + " following player at a distance of " + distance);

                    moveSpeed = playerController.moveSpeed;
                    moveSpeedSlow = playerController.moveSpeedSlow;
                    isPlayerMovingSlow = playerController.isMovingSlow;

                    // If the player is moving slow, agent moves slow. Else move regular speed
                    agent.speed = isPlayerMovingSlow ? playerController.moveSpeedSlow : playerController.moveSpeed;

                    destination = playerUnit.transform.position;
                    agent.destination = destination;

                    if (transform.position.x > agent.destination.x)
                    {
                        transform.localScale = new Vector3(1, 1, 1);
                    }
                    else
                    {
                        transform.localScale = new Vector3(-1, 1, 1);
                    }

                }

                // If the unit has moved too far away to see the player, it stops
                if (Vector3.Distance(playerUnit.transform.position, transform.position) > playerSpottingDistance)
                    StopFollowingPlayer();
            }

            if (isGoingHome)
            {
                agent.speed = moveSpeed;
                agent.destination = homeCoords;
                Debug.Log("Agent is going home to " + agent.destination);
            }
            
            // Debug.Log(agent.velocity);

            // Time to check, has the character moved at all?
            if (agent.velocity.x != 0 || agent.velocity.y != 0)
            {
                // Debug.Log("Should be wobbling!");
                float wobSpeed = isPlayerMovingSlow ? 1.5f : 3f;
                float seesaw = Mathf.Clamp01(1f - Mathf.Abs(1f - 2f * (Time.time * wobSpeed % 1)));       // Seesaw linear curve going from 0 to 1 to 0
                float rotAngle = Mathf.Lerp(wobbleAngle, -wobbleAngle, seesaw);                 // Lerp between angle and minus angle with seesaw

                // Really hacky solution that combines Vector3.SmoothDamp & Quaternion.LookRotation. Don't judge.
                currentRot = new Vector3(rotAngle, 5, 0);
                currentRot = Vector3.SmoothDamp(lastRot, currentRot, ref currentWobVel, 0.05f);
                transform.rotation = Quaternion.LookRotation(Vector3.forward, currentRot);
                lastRot = currentRot;
            }

            else
            {
                // Lerp back to zero position when movement is zero
                currentRot = Vector3.SmoothDamp(lastRot, new Vector3(0, 5, 0), ref currentWobVel, 0.05f);
                transform.rotation = Quaternion.LookRotation(Vector3.forward, currentRot);
                lastRot = currentRot;
            }
        }
    }

    // This considers the unit to be currently looking for the player. It'll check if
    // the player is close enough and, if they are, will transition the character over to 
    // following the player around.
    void LookingForPlayer()
    {
        if (!isGoingHome && (Vector3.Distance(transform.position, playerUnit.transform.position) < playerSpottingDistance))
        {         
            Debug.Log(name + " spotted player from " + transform.position + " at a distance of " + Vector3.Distance(transform.position, playerUnit.transform.position));

            hasSpottedPlayer = true;
            isWaitingForPlayer = false;
        }      
    }

    // With this, the unit starts following after the player character
    void StartFollowingPlayer()
    {
        isFollowingPlayer = true;
        hasSpottedPlayer = false;

        // Add this penguin to list of following penguins
        rescueController.AddToPenguinsFollowing(gameObject);
    }

    void StopFollowingPlayer()
    {
        agent.destination = transform.position;
        isFollowingPlayer = false;
        hasSpottedPlayer = false;
        isWaitingForPlayer = true;

        // Remove this penguin to list of following penguins
        rescueController.RemoveFromPenguinsFollowing(gameObject);

        Debug.Log("Oh no! " + name + " has stopped following the player!");
    }

    // It's time to go home, penguin. This flags the nav controller to 
    // go home, mainly as just a quick botch fix to get them to go into the cabin
    public void GoHome(Vector3 coords)
    {
        isFollowingPlayer = false;
        isGoingHome = true;
        homeCoords = coords;
    }

    // Vector3.Distance includes the Y-axis. This'll let us calculate
    // the distance between two objects in 3D space by only counting
    // horizontal and depth axes.
    private float VectorDistance2D(Vector3 v1, Vector3 v2)
    {
        float xDiff = v1.x - v2.x;
        float zDiff = v1.z - v2.z;
        return Mathf.Sqrt((xDiff * xDiff) + (zDiff * zDiff));
    }
}
