using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControls : MonoBehaviour
{
    private Transform cameraTransform;
    private Transform pivotTransform;

    private Vector3 localRotation;
    private float cameraDistance;

    public float mouseSensitivity = 4f;
    public float scrollSensitivity = 2f;
    public float orbitDampening = 10f;
    public float scrollDampening = 6f;

    public bool cameraMoving = false;
    public bool autoRotate;

    // Start is called before the first frame update
    void Start()
    {
        cameraTransform = transform;
        pivotTransform = transform.parent;
        cameraDistance = Vector3.Distance(cameraTransform.position, pivotTransform.position);
        autoRotate = true;
    }

    // Update is called once per frame
    void LateUpdate()
    { 
        if(autoRotate)
        {
            // TODO: doesn't work
            transform.LookAt(pivotTransform);
            transform.Translate(Vector3.right * Time.deltaTime);
        }
        else
        {
            // Right Click and Drag to control camera
            if (Input.GetMouseButtonDown(1))
                cameraMoving = true;
            if (Input.GetMouseButtonUp(1))
                cameraMoving = false;

            if (cameraMoving && Input.GetMouseButton(1))
            {
                // Rotation of the camera based on Mouse coordinates
                if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
                {
                    localRotation.x += Input.GetAxis("Mouse X") * mouseSensitivity;
                    localRotation.y -= Input.GetAxis("Mouse Y") * mouseSensitivity;

                    // Clamp the Y rotation to the horizon and not flipping over the top
                    localRotation.y = Mathf.Clamp(localRotation.y, 0f, 90f);
                }
            }

            // Zooming input from Mouse Scroll Wheel
            if (Input.GetAxis("Mouse ScrollWheel") != 0)
            {
                float scrollAmount = Input.GetAxis("Mouse ScrollWheel") * scrollSensitivity;

                // makes camera zoom faster the further away it is from the target
                scrollAmount *= cameraDistance * 0.3f;

                cameraDistance -= scrollAmount;

                cameraDistance = Mathf.Clamp(cameraDistance, 2f, 80f);
            }
        }

        // Actual camera rig transformations
        Quaternion desiredRotation = Quaternion.Euler(localRotation.y, localRotation.x, 0);
        pivotTransform.rotation = Quaternion.Lerp(pivotTransform.rotation, desiredRotation, Time.deltaTime * orbitDampening);

        if (cameraTransform.localPosition.z != -cameraDistance)
        {
            cameraTransform.localPosition = new Vector3(0f, 0f, Mathf.Lerp(cameraTransform.localPosition.z, -cameraDistance, Time.deltaTime * scrollDampening));
        }
    }

    // called by Play method
    public void ActivateControls()
    {
        autoRotate = false;
    }
}
