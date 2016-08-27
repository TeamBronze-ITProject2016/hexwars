using UnityEngine;
using System.Collections;

public class Part : ScriptableObject
{
    public GameObject thisPart;

    public static int horizontalDiagOffset = 29;
    public static int verticalDiagOffset = 47;

    public static int horizontalParallelOffset = 56;
    public static int verticalParallelOffset = 0;

    // Store pixel locations of where part is to be attached
    public Vector3[] hexEdgeOffsets = {
        new Vector3(horizontalDiagOffset, verticalDiagOffset),
        new Vector3(horizontalParallelOffset, verticalParallelOffset),
        new Vector3(horizontalDiagOffset, -verticalDiagOffset),
        new Vector3(-horizontalDiagOffset, -verticalDiagOffset),
        new Vector3(-horizontalParallelOffset, verticalParallelOffset),
        new Vector3(-horizontalDiagOffset, verticalDiagOffset)
    };

    public Part(GameObject parent, int position)
    {
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
