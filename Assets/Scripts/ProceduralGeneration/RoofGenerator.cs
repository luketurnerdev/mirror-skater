using UnityEngine;

public class RoofGenerator : MonoBehaviour
{
    /* Every time the floor is generated, generate an identical block on the roof
    
    - Keep track of the current state (floor or roof skating)
    - Function to toggle the visibility of the roof / floor
    
    */
    
    public enum SkateSurface
    {
        Floor,
        Roof
    }
    
    public SkateSurface currentSkateSurface = SkateSurface.Floor;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
