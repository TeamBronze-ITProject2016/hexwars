/* Player.cs
* Authors: Nihal Mirpuri, William Pan, Jamie Grooby, Michael De Pasquale
* Description: The base class for all players. 
*/

using UnityEngine;
using System.Collections;

public class Player : Entity
{

    private float speed = 2.0f;
    private float score;

    Rigidbody2D rigidBody;

    /*Parts system*/
    private int points = 0;
    private ArrayList parts;

    /*Initialise*/
    void Start()
    {
        parts = new ArrayList();

        /*Add RigidBody2D component dynamically*/
        rigidBody = (Rigidbody2D)gameObject.AddComponent(typeof(Rigidbody2D));

        /*Load part prefab and get reference to attached Part script*/
        GameObject partPrefab = (GameObject)Resources.Load("part");
        Part partScript = partPrefab.GetComponent<Part>();

        Instantiate(partPrefab, this.transform);
        parts.Add(partScript);
    }

    /*Called once per frame*/
    void FixedUpdate()
    {
        /*Hacky movement code, just for testing*/
        float deltaTime = Time.deltaTime;
        float forward = 0.0f;
        float turn = 0.0f;

        /*Forward/backwards*/
        if (Input.GetKey(KeyCode.W))
            forward += 1.0f;

        if (Input.GetKey(KeyCode.S))
            forward -= 1.0f;

        /*Turning*/
        if (Input.GetKey(KeyCode.A))
            turn -= 1.0f;

        if (Input.GetKey(KeyCode.D))
            turn += 1.0f;

        /*Apply force*/
        Vector3 eulerRot = this.transform.rotation.eulerAngles;
        float x = Mathf.Cos(eulerRot.z / 180.0f * 3.14159f) * forward;
        float y = Mathf.Sin(eulerRot.z / 180.0f * 3.14159f) * forward;
        Vector2 force = new Vector2(x, y);
        rigidBody.AddForce(force);

        /*Apply torque*/
        rigidBody.AddTorque(turn);
    }
}