using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObjects : MonoBehaviour
{
    [SerializeField] private string tagToDestroy;
    public void DestroyObjectsWithTag()
    {
        GameObject[] objectsWithTag = GameObject.FindGameObjectsWithTag(tagToDestroy);

        foreach (GameObject obj in objectsWithTag)
        {
            Destroy(obj);
        }
    }
}
