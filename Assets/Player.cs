/* Player.cs
* Authors: Nihal Mirpuri, William Pan, Jamie Grooby, Michael De Pasquale
* Description: The base class for all players. 
*/

using UnityEngine;
using System.Collections;

public class Player : Entity
{
    public Rigidbody2D rb;
    public float acceleration = 9.81f;

    /*Initialise*/
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    /*Called once per frame*/
    void FixedUpdate()
    {
        if(Input.GetMouseButtonDown(0)) /* CHANGE TO USE INPUT CONTROLLER CLASS */
        {
            Debug.Log("Player/FixedUpdate(): Mouse Button Down");
            Vector2 coordinate = (Vector2)Input.mousePosition; /* CHANGE TO USE INPUT CONTROLLER CLASS */
            RotateToPoint(coordinate);
            MoveForward(coordinate);
        }
    }

    void MoveForward(Vector2 coord)
    {
        /* Apply a forward force to the hexagon.
         * Deceleration can probably be handled via rigidbody linear drag parameter */
        Vector3 p = Camera.main.ScreenToWorldPoint(new Vector3(coord.x, coord.y, 1));

        rb.AddForce(p * acceleration);
    }

    void RotateToPoint(Vector2 x)
    {
        /* Rotate hexagon at a constant rate towards a certain point */
    }
}