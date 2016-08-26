/* Player.cs
* Authors: Nihal Mirpuri, William Pan, Jamie Grooby, Michael De Pasquale
* Description: The base class for all players. 
*/

using UnityEngine;
using System.Collections;

public class Player : Entity
{
    private Rigidbody2D rb;
    public float acceleration;
    public float rotationSpeed;

    /*Initialise*/
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    /*Called once per frame*/
    void FixedUpdate()
    {
        Vector2 coordinate = (Vector2)Input.mousePosition; /* CHANGE TO USE INPUT CONTROLLER CLASS */

        // Convert the coordinate into a Vector3 from rb.position to the coordinate

        if (Input.GetMouseButton(0)) /* CHANGE TO USE INPUT CONTROLLER CLASS */
        {
            Debug.Log("Player/FixedUpdate(): Mouse Button Down");
            RotateToPoint(coordinate);
            MoveForward();
        }
    }

    /* Apply a forward force to the hexagon. */
    void MoveForward()
    {
        float angleInRad = rb.rotation * Mathf.Deg2Rad;
        Vector2 direction = new Vector2(-(float)Mathf.Cos(angleInRad), -(float)Mathf.Sin(angleInRad));
        rb.AddForce(direction * acceleration);
    }

    /* Rotate hexagon at a constant rate towards a certain point */
    void RotateToPoint(Vector2 coord)
    {
        /* Find the vector pointing from our position to the target */
        Vector3 p = new Vector3(rb.position.x, rb.position.y, 1) - Camera.main.ScreenToWorldPoint(new Vector3(coord.x, coord.y, 1));

        float targetAngle = Mathf.Atan2(p.y, p.x) * Mathf.Rad2Deg;
        rb.MoveRotation(Mathf.MoveTowardsAngle(rb.rotation, targetAngle, rotationSpeed * Time.deltaTime));
    }
}