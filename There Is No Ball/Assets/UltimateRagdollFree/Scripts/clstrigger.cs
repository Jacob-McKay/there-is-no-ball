using UnityEngine;
using System.Collections;

/// <summary>
/// 2015-04-01
/// ULTIMATE RAGDOLL GENERATOR V4.6
/// © THE ARC GAMES STUDIO 2015
// 
/// helper class to trigger the physics driven activation of a ragdolled character
/// 1- add a collider to the scene object
/// 2- (if the triggering object has no rigidbody) add a rigidbody to the same scene object, and set it as kinematic
/// 3- add the clskinetify script to the scene object
/// 4- for this example we've set the 'Collider' parameter for easiest approach.
///    Alternatively, the layer collision matrix can be used, from the project options and physics menu.
/// NOTE: not used in the demo scene
/// </summary>
public class clstrigger : MonoBehaviour {
	void OnTriggerEnter(Collider varsource) {
		//trigger only with collider of the object called 'bumper' 
		if (varsource.name == "bumper") {
			//cache the kinetifier
			clskinetify varlocalkinetifier = gameObject.GetComponent<clskinetify>();
			if ( varlocalkinetifier !=null)
				//activate the driving routine
				varlocalkinetifier.metgodriven();
		}
	}
	
		
}
