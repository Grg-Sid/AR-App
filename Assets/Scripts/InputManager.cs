using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
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

    void Update()
    {
        touch = Input.GetTouch(0);

        if (Input.touchCount < 0)
        {
            return;
        }

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
        if (Input.GetTouch(0).phase == TouchPhase.Moved && activeObject != null)
        {
            // Convert X-Y touch movement to object translation in world space
            translationVector = new Vector3(Camera.main.transform.forward.x, 0f, Camera.main.transform.forward.z);
            activeObject.transform.Translate(translationVector * Input.GetTouch(0).deltaPosition.y * speedModifier, Space.World);

            translationVector = new Vector3(Camera.main.transform.right.x, 0f, Camera.main.transform.right.z);
            activeObject.transform.Translate(translationVector * Input.GetTouch(0).deltaPosition.x * speedModifier, Space.World);
        }

        // Rotation Movement
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
