using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraDrag : MonoBehaviour
{
    public Camera cam;

    [Header("Camera Movement")]

    [Tooltip("Allow the Camera to be dragged.")]
    public bool dragEnabled = true;
    [Range(-5, 5)]
    [Tooltip("Speed the camera moves when dragged.")]
    public float dragSpeed = -0.06f;

    [SerializeField]
    public bool xd, inverce;

    [SerializeField]
    public int xMin, xMax;

    void Start()
    {
        if (cam == null)
        {
            cam = Camera.main;
        }
    }

    void Update()
    {
        if (dragEnabled)
        {
            panControl();
        }

    }

    public void panControl()
    {

        if (Input.GetMouseButton(0))
        {

            float x = Input.GetAxis("Mouse X") * dragSpeed;

            x *= Camera.main.orthographicSize;

            if (inverce) x = -x;

            transform.position = new Vector3(transform.position.x, transform.position.y, Mathf.Clamp(transform.position.z + x, xMin, xMax));


        }


    }
}
