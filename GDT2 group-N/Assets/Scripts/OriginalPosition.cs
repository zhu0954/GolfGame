using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OriginalPosition : MonoBehaviour
{
    Vector3 originalPos;
    void Start()
    {
        originalPos = gameObject.transform.position;
    }

    public void ObjectReset()
    {
        transform.position = originalPos;
    }
}
