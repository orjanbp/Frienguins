using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * This CameraController is intended for the CameraRig, not the Camera directly.
 * The actual camera controlled is attached as a child of the CameraRig.
 *  Mainly this is because we're messing with directional angles on an orthographic
 * camera setup.
 */
public class CameraController : MonoBehaviour
{
    public Transform target;                                      // Camera's focal point
    
    public float cameraDistance = 5f;                             // Camera's distance to the target
    public float cameraVerticalOffset = 0.3f;                     // The vertical offet applied to the target's focal point
    public float cameraZoomSpeed = 2f;                            // Increases the speed at which the camera interopolates its distance

    public float cameraSmoothing = 10f;                           // Increases the smoothing delay at which the camrea follows the player
    public Vector3 cameraVelocity = Vector3.zero;                 // Increases velocity of the camera following the target

    private Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        // We need to access the camera child to change its offset
        cam = GetComponentInChildren<Camera>();

        transform.position = getCameraElevation(target.position);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        // Realign camera distance
        if (!Mathf.Approximately(cam.orthographicSize, cameraDistance))
            cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, cameraDistance, (cameraZoomSpeed * Time.deltaTime));

        // Realign camera position with a slight delay
        transform.position = Vector3.SmoothDamp(transform.position, getCameraElevation(target.position), ref cameraVelocity, (cameraSmoothing * Time.deltaTime));
    }

    // If we're trying to just put the camera to a target.transform, we'll
    // end up with a point at the target's base. So we'll bake a small camera
    // elevation into this function to counteract that.
    Vector3 getCameraElevation(Vector3 targetCameraPosition)
    {
        targetCameraPosition.y = target.position.y + cameraVerticalOffset;
        return targetCameraPosition;
    }
}
