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

    private Touch touch;
    void Start()
    {
        
    }
    void Update()
    {
        touch = Input.GetTouch(0);

        if (Input.touchCount < 0 || touch.phase != TouchPhase.Began)
        {
            return;
        }  
        if (IsPointerOverUI(touch))
        {
            return;
        }
        
        Ray ray = arCam.ScreenPointToRay(touch.position);
        if(arRaycastManager.Raycast(ray, hits))
        {
             Pose pose = hits[0].pose;
             Instantiate(DataHandler.instance.model, pose.position, pose.rotation);
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
}
