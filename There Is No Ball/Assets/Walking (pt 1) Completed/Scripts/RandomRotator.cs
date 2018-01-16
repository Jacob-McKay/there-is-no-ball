using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomRotator : MonoBehaviour {

    public Vector3 rotationSpeedAboutEachAxis = Vector3.one;
    public Vector3 revolutionSpeedAboutEachAxis = Vector3.one;

	// Use this for initialization
	void Start () {
        Debug.Log("Start called");
	}
	
	// Update is called once per frame
	void Update () {
        // Say whaaaaaat https://docs.unity3d.com/ScriptReference/Transform.Rotate.html

        Debug.Log("Update Called");

        // Rotate the object around its local X axis at 1 degree per second
        transform.Rotate(rotationSpeedAboutEachAxis * Time.deltaTime );

        // ...also rotate around the World's Y axis
        transform.Rotate(revolutionSpeedAboutEachAxis * Time.deltaTime, Space.World);
    }
}
