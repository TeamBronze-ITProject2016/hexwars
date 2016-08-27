/* Player.cs
* Authors: Nihal Mirpuri, William Pan, Jamie Grooby, Michael De Pasquale
* Description: The base class for all players. 
*/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Player : Entity
{
    public float acceleration;
    public float rotationSpeed;

    private Rigidbody2D rb;
    private TeamBronze.HexWars.InputManager inputManager; /* Need to fix namespaces */

    /*Initialise*/
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        inputManager = GameObject.Find("InputManager").GetComponent<TeamBronze.HexWars.InputManager>();
    }

    /*Called once per frame*/
    void FixedUpdate()
    {
        if (inputManager.IsActive())
        {
            Vector2 coordinate = inputManager.GetPos();
            RotateToPoint(coordinate);
            MoveForward();
        }
    }

    /* Apply a forward force to the hexagon. */
    void MoveForward()
    {
        float angleInRad = rb.rotation * Mathf.Deg2Rad;
        Vector2 direction = new Vector2(-(float)Mathf.Cos(angleInRad), -(float)Mathf.Sin(angleInRad));
        rb.AddForce(direction * acceleration * rb.mass);
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