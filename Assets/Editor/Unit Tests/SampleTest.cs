using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using TeamBronze.HexWars;

using System.Collections;

public class SampleTest : MonoBehaviour{

	[Test]
	public void EditorTest()
	{
		//Arrange
		var gameObject = new GameObject();

		//Act
		//Try to rename the GameObject
		var newGameObjectName = "My game object";
		gameObject.name = newGameObjectName;

		//Assert
		//The object has a new name
		Assert.AreEqual(newGameObjectName, gameObject.name);
	}


    
}
