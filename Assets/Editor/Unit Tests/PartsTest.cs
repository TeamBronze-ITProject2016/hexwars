using UnityEngine;
using TeamBronze.HexWars;
using NUnit;
using System.Collections;
using NUnit.Framework;
using System.Collections.Generic;

public class PartsTest : MonoBehaviour {

    /** Test adding of part */
    [Test]
    public void AddPartTest()
    {
        // Test randomly attached part
        PartData partData = new PartData();
        AxialCoordinate coord = new AxialCoordinate { x = -1, y = 0 };
        Part addedPart = new Part { shape = new GameObject(), type = -1};
        partData.addPart(coord, addedPart);

        // If false, means coord is occupied
        Assert.False(partData.checkPart(coord));
    }

    /** Test removal of part */
    [Test]
    public void RemovePartTest()
    {
        // Test randomly attached part
        PartData partData = new PartData();
        AxialCoordinate coord = new AxialCoordinate { x = -1, y = 0 };
        Part addedPart = new Part { shape = new GameObject(), type = -1 };
        partData.addPart(coord, addedPart);

        partData.removePart(coord);
        // If null, part is not present
        Assert.Null(partData.getPart(coord));
    }

    /* Test if free edges return is correct */
    [Test]
    public void EmptyPlayerTest()
    {
        // Add player
        PartData partData = new PartData();
        Part player = new Part { shape = new GameObject(), type = 1 };
        AxialCoordinate coord = new AxialCoordinate { x = 0, y = 0 };
        partData.addPart(coord, player);

        // Get all empty neighbours (ie, places to attach objects)
        List<AxialCoordinate> neighbours = partData.getEmptyNeighbors(coord);
        // If 6 free edges, pass
        Assert.True(neighbours.Count == 6);
    }

    /* Test if free edges return is correct */
    [Test]
    public void EmptyHexagonTest()
    {
        // Add player
        PartData partData = new PartData();
        Part hexagon = new Part { shape = new GameObject(), type = 0 };
        AxialCoordinate coord = new AxialCoordinate { x = 0, y = 0 };
        partData.addPart(coord, hexagon);

        // Get all empty neighbours (ie, places to attach objects)
        List<AxialCoordinate> neighbours = partData.getEmptyNeighbors(coord);
        // If 6 free edges, pass
        Assert.True(neighbours.Count == 6);
    }

    /* Test if triangle edges return is correct */
    [Test]
    public void EmptyTriangleTest()
    {
        // Add player
        PartData partData = new PartData();
        Part triangle = new Part { shape = new GameObject(), type = -1 };
        AxialCoordinate coord = new AxialCoordinate { x = 0, y = 0 };
        partData.addPart(coord, triangle);

        // Get all empty neighbours (ie, places to attach objects - there should be
        // 0 for a triangle)
        List<AxialCoordinate> neighbours = partData.getEmptyNeighbors(coord);
        // If 0 free edges, pass
        Assert.True(neighbours.Count == 0);
    }

    /* Test if free edges return is correct */
    [Test]
    public void FullPlayerTest()
    {
        // Add a heaxgon to player, then test player
        PartData partData = new PartData();
        Part player = new Part { shape = new GameObject(), type = 1 };
        Part hexagon = new Part { shape = new GameObject(), type = 0 };
        AxialCoordinate playerCoord = new AxialCoordinate { x = 0, y = 0 };
        AxialCoordinate hexCoord = new AxialCoordinate { x = -1, y = 0 };
        partData.addPart(playerCoord, player);
        partData.addPart(hexCoord, hexagon);

        // Get all attached neighbours
        List<AxialCoordinate> neighbours = partData.getFullNeighbors(playerCoord);
        // If 1 attached neighbour, pass
        Assert.True(neighbours.Count == 1);
    }

    /* Test if triangle edges return is correct */
    [Test]
    public void FullTriangleTest()
    {
        // Add a traingle to a hexagon, then test traingle
        PartData partData = new PartData();
        Part player = new Part { shape = new GameObject(), type = 1 };
        Part triangle = new Part { shape = new GameObject(), type = 0 };
        AxialCoordinate playerCoord = new AxialCoordinate { x = 0, y = 0 };
        AxialCoordinate triangleCoord = new AxialCoordinate { x = -1, y = 0 };
        partData.addPart(playerCoord, player);
        partData.addPart(triangleCoord, triangle);

        // Get all full neighbours (ie, places to attach objects - there should be
        // 1 for a triangle)
        List<AxialCoordinate> neighbours = partData.getFullNeighbors(triangleCoord);
        // If 1 object attached, pass
        Assert.True(neighbours.Count == 1);
    }

    /* Tests if the number of parts destroyed is correct */
    [Test]
    public void PathExistsTest()
    {
        // Add a row of three hexagons including player
        PartData partData = new PartData();
        Part player = new Part { shape = new GameObject(), type = 1 };
        Part hexagon = new Part { shape = new GameObject(), type = 0 };
        AxialCoordinate playerCoord = new AxialCoordinate { x = 0, y = 0 };
        AxialCoordinate hexCoord = new AxialCoordinate { x = -1, y = 0 };
        AxialCoordinate hex2Coord = new AxialCoordinate { x = -2, y = 1 };
        partData.addPart(playerCoord, player);
        partData.addPart(hexCoord, hexagon);
        partData.addPart(hex2Coord, hexagon);

        // Find all destroyed parts if middle hexagon hit
        List<AxialCoordinate> neighbours = partData.findDestroyedPartLocations(hexCoord);
        // If 2 object destroyed, pass
        Assert.True(neighbours.Count == 2);
    }
}
