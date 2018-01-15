using UnityEngine;
using System.Collections;

/// <summary>
/// 2015-04-01
/// ULTIMATE RAGDOLL GENERATOR V4.6
/// © THE ARC GAMES STUDIO 2015
/// 
/// helper class to trigger the ragdoll function in a gameobject with a ragdollifier attached
/// </summary>
public class clsragdollhelper : MonoBehaviour {
	private bool varcanragdoll = false;
	private clsragdollify varlocalragdollifier;
	
	void Start () {
		Animation varanimation = GetComponent<Animation>();
		varanimation.wrapMode = WrapMode.Loop;
		varlocalragdollifier = GetComponent<clsragdollify>();
		if (varlocalragdollifier != null) {
			if (varlocalragdollifier.vargamragdoll != null)
				varcanragdoll = true;
		}
	}
	
	/// <summary>
	/// shortcut to the ragdollify component
	/// </summary>
	public Transform metgoragdoll(Vector3 varpspeed = new Vector3()) {
		Transform varreturn = null;
		if (varcanragdoll) {
			varreturn = varlocalragdollifier.metgoragdoll(varpspeed);
			Destroy(gameObject);
		}
		return varreturn;
	}
}
