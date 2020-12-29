using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCharacter : MonoBehaviour
{

    public float Speed = 5f;
    public float ZoomSpeed = 5f;

    public float ZoomClose = 0.5f;
    public float ZoomFar = 0.5f;

    public Transform Player;
    public Camera MainCamera;
    public Transform CameraZoomContainer;

    private Transform lookTarget;

    // Start is called before the first frame update
    void Start()
    {
        lookTarget = Player;
    }

    // Update is called once per frame
    void Update()
    {
        // Make sure the zoom container is always looking at the player
        // CameraZoomContainer.transform.LookAt(Player.transform);

        // Make the camera itself look at the lookTarget (be it player, boss entering, or other event)
        // MainCamera.transform.LookAt(lookTarget);

        // Lerp the position of the camera rig to follow player position
        Vector3 targetPos = Player.transform.position;
        transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * Speed);
    }

    public void MoveCamera(float val)
    {
        CameraZoomContainer.transform.Translate(Vector3.forward * ZoomSpeed * val);

        float dist = Vector3.Distance(CameraZoomContainer.position, Player.position);

        while (dist < ZoomClose)
        {
            CameraZoomContainer.transform.Translate(Vector3.back * ZoomSpeed * 0.1f);
            dist = Vector3.Distance(CameraZoomContainer.position, Player.position);
        }

        while (dist > ZoomFar)
        {
            CameraZoomContainer.transform.Translate(Vector3.forward * ZoomSpeed * 0.1f);
            dist = Vector3.Distance(CameraZoomContainer.position, Player.position);
        }
    }
}
