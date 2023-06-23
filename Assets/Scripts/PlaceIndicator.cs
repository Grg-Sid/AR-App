using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
public class PlaceIndicator : MonoBehaviour
{
    [SerializeField]
    private ARRaycastManager raycastManager;
    [SerializeField]
    private GameObject indicator;
    public static Pose pose;
    //private 
    void Start()
    {
        raycastManager = FindObjectOfType<ARRaycastManager>();
        indicator = this.transform.GetChild(0).gameObject;
        indicator.SetActive(false);
    }

    void Update()
    {
        List<ARRaycastHit> hits = new List<ARRaycastHit>();
        //var ray = ;
        raycastManager.Raycast(new Vector2(Screen.width / 2, Screen.height / 2), hits, TrackableType.Planes);
        if(hits.Count > 0)
        {
            pose = hits[0].pose;

            transform.position = pose.position;
            transform.rotation = pose.rotation;
            //transform.eulerAngles = new Vector3(90, 0, 0);

            if (!indicator.activeInHierarchy)
            {
                indicator.SetActive(true);
            }
        }
    }
}

