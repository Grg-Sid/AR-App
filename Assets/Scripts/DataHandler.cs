using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataHandler : MonoBehaviour
{
    public GameObject model;

    public static DataHandler instance;

    public void Awake()
    {
        instance = this;
    }
}
