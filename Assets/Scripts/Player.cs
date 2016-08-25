/* Player.cs
* Authors: Nihal Mirpuri, William Pan, Jamie Grooby, Michael De Pasquale
* Description: The base class for all players. 
*/

using UnityEngine;
using System.Collections;

public class Player : Entity
{
    public Rigidbody2D rb;
    public float Acceleration;

    /*Initialise*/
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    /*Called once per frame*/
    void FixedUpdate()
    {
        Vector2 coordinate = (Vector2)Input.mousePosition; /* CHANGE TO USE INPUT CONTROLLER CLASS */

        if (Input.GetMouseButtonDown(0)) /* CHANGE TO USE INPUT CONTROLLER CLASS */
        {
            Debug.Log("Player/FixedUpdate(): Mouse Button Down");
            MoveForward(coordinate);
            RotateToPoint(coordinate);
        }

        RotateToPoint(coordinate);
    }

    void MoveForward(Vector2 coord)
    {
        /* Apply a forward force to the hexagon.
         * Deceleration can probably be handled via rigidbody linear drag parameter */
        Vector3 p = Camera.main.ScreenToWorldPoint(new Vector3(coord.x, coord.y, 1));

        Debug.Log("Player/MoveForward(): Adding force of size " + p);

        rb.AddForce(p * Acceleration);
    }

    void RotateToPoint(Vector2 coord)
    {
        /* Rotate hexagon at a constant rate towards a certain point */

        //find the vector pointing from our position to the target
        Vector3 p = new Vector3(rb.position.x, rb.position.y, 1) - Camera.main.ScreenToWorldPoint(new Vector3(coord.x, coord.y, 1));

        Debug.Log("Player/RotateToPoint(): Rotating to vector " + p);

        //rotate us over time according to speed until we are in the required rotation
        if (p != Vector3.zero)
        {
            float angle = Mathf.Atan2(p.y, p.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }
}