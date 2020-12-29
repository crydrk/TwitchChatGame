using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimControl : MonoBehaviour
{
    public static AimControl SharedInstance;

    public Transform Reticle;
    public FollowCharacter CameraRigContainer;
    public int GroundLayerIndex = 9;

    private void Awake()
    {
        SharedInstance = this;
    }

    private void Update()
    {
        // Position and rotate the reticle to match the terrain or certain objects
        RaycastHit hit;
        Ray ray = CameraRigContainer.MainCamera.ScreenPointToRay(Input.mousePosition);

        int layerMask = 1 << GroundLayerIndex;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
        {
            Reticle.position = hit.point;

            Reticle.up = hit.normal;
        }
        
        CameraRigContainer.MoveCamera(Input.GetAxis("Mouse ScrollWheel"));
    }

}
