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
            transform.position = hits[0].pose.position;
            transform.rotation = hits[0].pose.rotation;

            if (!indicator.activeInHierarchy)
            {
                indicator.SetActive(true);
            }
        }
    }
}

