using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Vector3 FixedCameraPosition = new Vector3(60, 0, 0);
    public Quaternion FixedCameraRotation = new Quaternion(0, -0.707106829f, 0, 0.707106829f);

    // Start is called before the first frame update
    void Start()
    {
        // TODO: Trying to setup an option to switch the camera view to a few different angles

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            // I think this is the right mouse button, dunno yet
        }
    }
}
