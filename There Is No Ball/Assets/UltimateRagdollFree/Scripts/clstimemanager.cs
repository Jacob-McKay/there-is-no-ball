using UnityEngine;
using System.Collections;

/// <summary>
/// 2015-04-01
/// ULTIMATE RAGDOLL GENERATOR V4.6
/// © THE ARC GAMES STUDIO 2015
/// 
/// the class is used as initializer for the "Fixed timestep" parameter of the
/// EDIT=> PROJECT SETTINGS=> TIME menu, that determines the frequency of physic updates for the game
/// It is suggested that the value be kept consistent throughout development to avoid
/// unexpected physic behavior
/// NOTE: this script was made obsolete by the scene manager and the Arc Layer Manager utility
/// </summary>
public class clstimemanager : MonoBehaviour {

    void Awake() {
		//set the Fixed timestep to 100 calls
		Time.fixedDeltaTime = 0.01f;
		#if !UNITY_5
			//set the minimum collision detection
			Physics.defaultContactOffset = 0.01f;
		#else
			Physics.defaultContactOffset = 0.01f;
		#endif
		//set the collision matrix to comply with actor controller and missiles
		Physics.IgnoreLayerCollision(2,0);
	}
}
