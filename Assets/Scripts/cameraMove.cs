using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class cameraMove : MonoBehaviour
{
    private Vector3 dragOrigin;

    // Assuming this script is attached to the camera
    public Image leftArrow;
    public Image rightArrow;
    public float dragSpeed = 2.0f;
    private float leftLimit = -9f;
    private float rightLimit = 9f;

    private bool isZoomed = false; // Track zoom state
    public float zoomSize = 3f; // Zoomed-in orthographic size
    private float originalSize; // Original orthographic size
    private Vector3 originalPosition; // Original position

    void Start()
    {
        originalSize = Camera.main.orthographicSize;
        originalPosition = Camera.main.transform.position;
    }
    void Update()
    {
                HandleDragging();
        UpdateArrowVisibility();
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity);

            // Toggle zoom if a "Zoomable" tag object is clicked or if already zoomed in
            if (hit.collider != null && hit.collider.CompareTag("Zoomable") && !isZoomed)
            {
                ZoomIn(hit);
            }
            else if (isZoomed)
            {
                ZoomOut();
            }
        }
    }

    void ZoomIn(RaycastHit2D hit)
    {
        Vector3 newPosition = new Vector3(hit.collider.transform.position.x, hit.collider.transform.position.y, originalPosition.z);
        // Optionally, adjust zoomPoint to ensure it's within bounds
        Camera.main.orthographicSize = zoomSize;
        Camera.main.transform.position = newPosition;
        isZoomed = true; // Update zoom state
    }

    void ZoomOut()
    {
        Camera.main.orthographicSize = originalSize;
        Camera.main.transform.position = originalPosition;
        isZoomed = false; // Update zoom state
    }
    void HandleDragging()
    {
        if (Input.GetMouseButtonDown(0))
        {
            dragOrigin = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.transform.position.z));
        }

        if (Input.GetMouseButton(0))
        {
            Vector3 currentPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.transform.position.z));
            Vector3 difference = dragOrigin - currentPos;

            difference.y = 0; // Ensure there's no vertical movement

            Vector3 newPosition = Camera.main.transform.position + difference;
            newPosition.x = Mathf.Clamp(newPosition.x, leftLimit, rightLimit);

            // Directly update the camera's position without intermediate variable
            Camera.main.transform.position = newPosition;
        }
       
    }
    void UpdateArrowVisibility()
    {
        // Show or hide the left arrow based on camera position
        if (Camera.main.transform.position.x <= leftLimit)
        {
            leftArrow.gameObject.SetActive(false);
        }
        else
        {
            leftArrow.gameObject.SetActive(true);
        }

        // Show or hide the right arrow based on camera position
        if (Camera.main.transform.position.x >= rightLimit)
        {
            rightArrow.gameObject.SetActive(false);
        }
        else
        {
            rightArrow.gameObject.SetActive(true);
        }
    }
}
