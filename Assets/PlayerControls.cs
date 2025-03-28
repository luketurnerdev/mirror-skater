using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerControls : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float crouchAmount = 0.5f;
    public float jumpForce = 10f;
    public bool isCrouching;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        MoveHorizontally();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Crouch();
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            Jump();
        }
    }

    // On held
    void Crouch()
    {
        if (isCrouching) return;

        // Simulate crouching by moving downward
        rb.MovePosition(transform.position + new Vector3(0, -crouchAmount, 0));
        isCrouching = true;
    }

    // On release
    void Jump()
    {
        if (!isCrouching) return;

        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        isCrouching = false;
    }

    void MoveHorizontally()
    {
        Vector3 horizontalMovement = new Vector3(moveSpeed * Time.deltaTime, 0, 0);
        rb.MovePosition(transform.position + horizontalMovement);
    }
}