using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;
using UnityEngine.XR.ARFoundation;

public class InputManager : MonoBehaviour
{
    [SerializeField]
    private Camera arCam;
    [SerializeField]
    private ARRaycastManager arRaycastManager;
    List<ARRaycastHit> hits = new List<ARRaycastHit>();
    [SerializeField]
    private GameObject pointer;
    private GameObject model;

    private Touch touch;
    private GameObject instantiatedObject = null; // Track the instantiated object
    private bool isObjectInstantiated = false; // Track if an object is instantiated
    private GameObject activeObject = null; // Track the active object for movement
    private float speedModifier = 0.0005f; // Control the speed of movement
    private Vector3 translationVector; // Translation vector for movement
    private float previousRotationAngle = 0f;
    private float currentRotationAngle = 0f;
    private float rotateSpeed = 5f;
    private float previousAverageDistance = 0f;

    void Update()
    {
        if (Input.touchCount > 0)
        {
            touch = Input.GetTouch(0);

            // Check if an object is already instantiated
            if (instantiatedObject == null && !isObjectInstantiated)
            {
                if (touch.phase == TouchPhase.Began && !IsPointerOverUI(touch))
                {
                    // Call the function to instantiate the object
                    InstantiateObject();
                }
            }
            // Initialize rotation angle for 2-finger object rotation
            previousRotationAngle = currentRotationAngle;

            // Touch Movement
            if (Input.GetTouch(0).phase == TouchPhase.Moved && activeObject != null && !IsPointerOverUI(Input.GetTouch(0)))
            {
                // Convert X-Y touch movement to object translation in world space
                translationVector = new Vector3(Camera.main.transform.forward.x, 0f, Camera.main.transform.forward.z);
                activeObject.transform.Translate(translationVector * Input.GetTouch(0).deltaPosition.y * speedModifier, Space.World);

                translationVector = new Vector3(Camera.main.transform.right.x, 0f, Camera.main.transform.right.z);
                activeObject.transform.Translate(translationVector * Input.GetTouch(0).deltaPosition.x * speedModifier, Space.World);
            }

            // Rotation Movement
            if (Input.touchCount == 2 && !IsPointerOverUI(Input.GetTouch(0)) && !IsPointerOverUI(Input.GetTouch(1)))
            {
                currentRotationAngle = Mathf.Atan((Input.GetTouch(0).position.y - Input.GetTouch(1).position.y) / (Input.GetTouch(0).position.x - Input.GetTouch(1).position.x));
                if ((currentRotationAngle - previousRotationAngle) > 0)
                {
                    activeObject.transform.Rotate(0, -rotateSpeed, 0);
                }
                if ((currentRotationAngle - previousRotationAngle) < 0)
                {
                    activeObject.transform.Rotate(0, rotateSpeed, 0);
                }
            }

            // Update previous rotation angle
            previousRotationAngle = currentRotationAngle;

            // Scaling 
            // Check if user is touching with 3 fingers and is not touching any UI
            if (Input.touchCount == 3 && !IsPointerOverUI(Input.GetTouch(0)) && !IsPointerOverUI(Input.GetTouch(1)) && !IsPointerOverUI(Input.GetTouch(2)))
            {
                // Calculate the average position of the three touches
                Vector2 touch1Position = Input.GetTouch(0).position;
                Vector2 touch2Position = Input.GetTouch(1).position;
                Vector2 touch3Position = Input.GetTouch(2).position;
                Vector2 centroid = (touch1Position + touch2Position + touch3Position) / 3f;

                // Calculate the average distance of the three touches from the centroid
                float touch1Distance = Vector2.Distance(touch1Position, centroid);
                float touch2Distance = Vector2.Distance(touch2Position, centroid);
                float touch3Distance = Vector2.Distance(touch3Position, centroid);
                float averageDistance = (touch1Distance + touch2Distance + touch3Distance) / 3f;

                // Calculate the scale factor based on the average distance change
                float scaleFactor = averageDistance / previousAverageDistance;

                // Scale the active object based on the scale factor
                activeObject.transform.localScale *= scaleFactor;

                // Update the previous average distance
                previousAverageDistance = averageDistance;
            }
        }

    }

    bool IsPointerOverUI(Touch touch)
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = new Vector2(touch.position.x, touch.position.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);
        return results.Count > 0;
    }

    IEnumerator LerpObjectScale(Transform objectTransform, Vector3 startScale, Vector3 endScale, float time)
    {
        float elapsedTime = 0f;
        while (elapsedTime < time)
        {
            objectTransform.localScale = Vector3.Lerp(startScale, endScale, elapsedTime / time);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        objectTransform.localScale = endScale;
    }

    public void InstantiateObject()
    {
        instantiatedObject = Instantiate(DataHandler.instance.model, PlaceIndicator.pose.position, PlaceIndicator.pose.rotation);
        StartCoroutine(LerpObjectScale(instantiatedObject.transform, Vector3.zero, Vector3.one, 1.5f));
        isObjectInstantiated = true;
        activeObject = instantiatedObject; // Set the instantiated object as the active object for movement
    }

    public void DestroyInstantiatedObject()
    {
        if (instantiatedObject != null)
        {
            Destroy(instantiatedObject);
            instantiatedObject = null;
            isObjectInstantiated = false;
            activeObject = null; // Clear the active object reference
        }
    }
}
