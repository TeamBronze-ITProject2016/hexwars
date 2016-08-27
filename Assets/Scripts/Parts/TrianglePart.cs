using UnityEngine;
using System.Collections;

public class TrianglePart : Part {
    public GameObject trianglePart;

    private GameObject parent;
    private int position;

    public TrianglePart(GameObject parent, int position) : base(parent, position)
    {
        Vector3 pos = parent.transform.position;
        // Get position of where to add part
        pos = Camera.main.WorldToScreenPoint(pos) + hexEdgeOffsets[position];
        // Instantiate part at position
        GameObject addedPart = (GameObject)Instantiate(trianglePart, Camera.main.ScreenToWorldPoint(pos), Quaternion.Euler(new Vector3(0, 0, 30 - 60 * (position + 1))));
        // Add part as child of parent hexagon
        addedPart.transform.parent = parent.gameObject.transform;
        // Add gameObject
        this.thisPart = addedPart;
    }
}
