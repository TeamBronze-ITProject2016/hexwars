/* Player.cs
* Authors: Nihal Mirpuri, William Pan, Jamie Grooby, Michael De Pasquale
* Description: The base class for all players. 
*/

using UnityEngine;
using System.Collections;

public class Player : Entity
{

    Rigidbody2D rigidBody;
    

    /*Initialise*/
    void Start()
    {

        /*Add RigidBody2D component dynamically*/
        rigidBody = (Rigidbody2D)this.gameObject.AddComponent(typeof(Rigidbody2D));
        rigidBody.gravityScale = 0.0f;

        /*Load part prefab and get reference to attached Part script*/
        GameObject partPrefab = (GameObject)Resources.Load("part");
        Part partScript = partPrefab.GetComponent<Part>();

        GameObject obj = (GameObject)Instantiate(partPrefab, new Vector2(1, 1), Quaternion.identity);
        obj.transform.parent = this.gameObject.transform;
    }

    /*Called once per frame*/
    void FixedUpdate()
    {
        Vector2 coordinate = (Vector2)Input.mousePosition;
        moveToPoint(coordinate);
    }

    void moveToPoint(Vector2 x)
    {
        // Tapping a place on a screen should apply a force to the object
        // There should be friction
        // Rotation should be done at a constant rate
        // Only forward acceleration
        

        /** FORCE CODE **/




        /** TORQUE CODE **/

    }
}