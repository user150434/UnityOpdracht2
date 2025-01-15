using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 3f; // Snelheid van de speler

    void Update()
    {
        // Beweging met W, A, S, D toetsen
        Vector3 moveDirection = Vector3.zero;

        if (Input.GetKey(KeyCode.W))
        {
            moveDirection += transform.forward; // Naar voren bewegen
        }
        if (Input.GetKey(KeyCode.S))
        {
            moveDirection -= transform.forward; // Naar achteren bewegen
        }
        if (Input.GetKey(KeyCode.A))
        {
            moveDirection -= transform.right; // Naar links bewegen
        }
        if (Input.GetKey(KeyCode.D))
        {
            moveDirection += transform.right; // Naar rechts bewegen
        }

        // Beweeg speler als er input is
        if (moveDirection != Vector3.zero)
        {
            transform.position += moveDirection.normalized * moveSpeed * Time.deltaTime;
        }

        // Rotatie met de muis
        float mouseXmovement = Input.GetAxis("Mouse X");
        transform.Rotate(new Vector3(0, mouseXmovement, 0));
    }
}
