using System;
using UnityEngine;

public class DetectGrinding : MonoBehaviour
{
    
    private GrindController grindController;
    
    private void OnCollisionEnter(Collision other)
    {
        Debug.Log("Collided with " + other.gameObject.name);
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player is grinding");
            grindController.SetIsGrinding(true);
        }
    }
    
    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player is no longer grinding");
            grindController.SetIsGrinding(false);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        grindController = FindObjectOfType<GrindController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
