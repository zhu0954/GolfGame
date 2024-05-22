using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    [SerializeField] private GameObject ball;
    [SerializeField] private UIManager manager;
    public Movement movement;
    public bool scored = false;
    private float timer = 3f;
    private float ogTime;
    
    void Start()
    {
        ogTime = timer;
    }
    void Update()
    {
        if(timer <0)
            {
            manager.RetryBox();
            timer = ogTime;
            }
       if(scored == true)
       {
            movement.rb.isKinematic = true;
            timer -= Time.deltaTime;
       }
       else
       {
            movement.rb.isKinematic = false;
       }
    }

    void OnTriggerEnter(Collider other)
    {
        if(ball == other.gameObject)
        {
            scored = true;
            movement.rb.isKinematic = true;
        }
    }

}
