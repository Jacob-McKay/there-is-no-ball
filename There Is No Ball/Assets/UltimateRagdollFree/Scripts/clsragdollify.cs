using UnityEngine;
using System.Collections;

/// <summary>
/// 2015-04-01
/// ULTIMATE RAGDOLL GENERATOR V4.6
/// © THE ARC GAMES STUDIO 2015
/// 
/// This is a simple helper class that can be used to turn models into ragdolls at any time.
/// 1- Create a ragdoll for the target gameobject model, using URG!
/// 2- Save the ragdoll in a new prefab (might be called originalmodelname_ragdoll, for ease of use. In case of a 'ratman' gameobject, the ragdoll prefab would be thus called 'ratman_ragdoll').
/// 3- Attach this script to the original prefab (i.e. 'ratman')
/// 4- Drag the ragdoll prefab into this script's vargamragdoll slot in the inspector
/// 5- Whenever needed, issue a call the metgoragdoll function from the clsragdollify component
/// </summary>
public class clsragdollify : MonoBehaviour {
	/// <summary>
	/// inspector slot for the ragdoll to instantiate
	/// </summary>
	public Transform vargamragdoll;
	
	/// <summary>
	/// copy the transforms from one armature to the other
	/// </summary>
	/// <param name="varpsource">
	/// source object
	/// </param>
	/// <param name="varpdestination">
	/// destination object
	/// </param>
	private void metcopytransforms(Transform varpsource, Transform varpdestination, Vector3 varpvelocity = new Vector3()) {
		varpdestination.position = varpsource.position;
		varpdestination.rotation = varpsource.rotation;
		if (varpvelocity != Vector3.zero) {
			Rigidbody varbody = varpdestination.GetComponent<Rigidbody>();
			if (varbody != null) {
				varbody.velocity = varpvelocity;
			}
		}
		foreach (Transform varchild in varpdestination) {
			Transform varcurrentsource = varpsource.Find(varchild.name);
			if (varcurrentsource) {
				metcopytransforms(varcurrentsource, varchild, varpvelocity);
			}
		}
	}
	
	/// <summary>
	/// Instantiates the vargamragdoll object, which should correspond to a ragdoll, and copies the two poses to allow a smooth transition
	/// </summary>
	public Transform metgoragdoll(Vector3 varpvelocity = new Vector3()) {
		Transform varnewragdoll = Instantiate(vargamragdoll, transform.position, transform.rotation) as Transform;
		metcopytransforms(transform, varnewragdoll, varpvelocity);
		return varnewragdoll;
	}
	
}
