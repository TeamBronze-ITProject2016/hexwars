using UnityEngine;
using System.Collections;

/**
 * Logic taken from Itsa from Unity3D forums
 * http://answers.unity3d.com/questions/716086/spin-a-2d-object-with-mouse-drag.html
 **/

public class SpinLogic : MonoBehaviour {

	float deltaRotation;
	float previousRotation;
	float currentRotation;
	public float maxRotateSpeed = 50f;

	// Use this for initialization
	void Start () 
	{

	}

	// Update is called once per frame
	public void spinUpdate () 
		{
			if (Input.GetMouseButtonDown (0))
			{
				deltaRotation = 0f;
				previousRotation = angleBetweenPoints( gameObject.transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition));
			}
			else if(Input.GetMouseButton(0))
			{
				currentRotation = angleBetweenPoints( gameObject.transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition) );
				deltaRotation = Mathf.DeltaAngle( currentRotation, previousRotation );
				if (deltaRotation > maxRotateSpeed) {
					deltaRotation = maxRotateSpeed;
				}
				if (deltaRotation < -maxRotateSpeed) {
					deltaRotation = -maxRotateSpeed;
				}
				previousRotation = currentRotation;
				gameObject.transform.Rotate(Vector3.back, deltaRotation);
			}

		}

		float angleBetweenPoints( Vector2 position1, Vector2 position2 )
		{
			var fromLine = position2 - position1;
			var toLine = new Vector2( 1, 0 );

			var angle = Vector2.Angle( fromLine, toLine );
			var cross = Vector3.Cross( fromLine, toLine );

			// did we wrap around?
			if( cross.z > 0 )
				angle = 360f - angle;

			return angle;
		}
}