using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{
    private Button button;
    public GameObject model;
    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(GetObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void GetObject()
    {
        DataHandler.instance.model = model;
    }
}
