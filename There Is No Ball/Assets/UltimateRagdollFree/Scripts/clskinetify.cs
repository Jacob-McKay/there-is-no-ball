using UnityEngine;
using System.Collections;

/// <summary>
/// 2015-04-01
/// ULTIMATE RAGDOLL GENERATOR V4.6
/// © THE ARC GAMES STUDIO 2015
/// 
/// This is a very simple helper class that demonstrates the use of the new clskinetify utility, that
/// turns a kinematic ragdoll into a driven ragdoll. This class is particularly useful to animate 'roadside' objects
/// or scenery objects that need to become physical when they receive a trigger (or otherwise a collision when implemented like so)
/// using this class can save time and resources in those cases, since it avoids the use of a specific ragdoll prefab and a separate
/// scripted gameobject
/// </summary>
public class clskinetify : MonoBehaviour {
	public Transform varsource;

	public void metgodriven() {
		//retrieve all the rigidbodies of the current gameobject
		Rigidbody[] varrigidbodies;
		varrigidbodies = GetComponentsInChildren<Rigidbody>();
		//cycle the rigidbodies and turn them into physics driven objects
		foreach (Rigidbody varcurrentrigidbody in varrigidbodies) {
			varcurrentrigidbody.isKinematic = false;
			//nudge to avoid auto sleep
			if (varsource != null && varsource.GetComponent<Rigidbody>() != null)			
				varsource.GetComponent<Rigidbody>().AddForce(Vector3.up,ForceMode.VelocityChange);
		}
	}
}
