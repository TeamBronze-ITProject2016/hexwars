/* Player.cs
* Authors: Nihal Mirpuri, William Pan, Jamie Grooby, Michael De Pasquale
* Description: The base class for all players. 
*/

using UnityEngine;
using System.Collections;

public class Player : Entity
{
    private Rigidbody2D rb;

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
            Vector2 coordinate = (Vector2)Input.mousePosition; /* CHANGE TO USE INPUT CONTROLLER CLASS */
            MoveForward();
            RotateToPoint(coordinate);
        }
    }

    void MoveForward()
    {
        /* Apply a forward force to the hexagon.
         * Deceleration can probably be handled via rigidbody linear drag parameter */
    }

    void RotateToPoint(Vector2 x)
    {
        /* Rotate hexagon at a constant rate towards a certain point */
    }
}