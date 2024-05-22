using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class CameraTracking : MonoBehaviour
{
    public Transform target;

    [SerializeField] private float moveSpeed;
    private Rigidbody rigidBody;
    
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        Vector3 offset = Vector3.Lerp(transform.position, target.position, moveSpeed);
        rigidBody.MovePosition(offset);
    }
}
