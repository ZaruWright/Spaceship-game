using UnityEngine;
using System.Collections;

public class Hero : MonoBehaviour {
	static public Hero  S; //Singleton

	// For Debugging
	public bool debug = false;

	// This fields control de movement of the ship
	public float speed = 30;
	public float rollMult = -45;
	public float pitchMult = 30;

	//Ship status information
	public float shieldLevel = 1;

	public bool ____________________________;

	public Bounds bounds;

	// Before the game starts
	void Awake(){
		S = this;
		bounds = Utils.CombineBoundsOfChildren (this.gameObject);
	}

	// Update is called once per frame
	void Update () {
		// Pull in information from the input class
		float xAxis = Input.GetAxis ("Horizontal");
		float yAxis = Input.GetAxis ("Vertical");

		// Change transform.position based on the axes
		Vector3 pos = transform.position;
		pos.x += xAxis * speed * Time.deltaTime;
		pos.y += yAxis * speed * Time.deltaTime;
		transform.position = pos;
		if (debug) {
			Debug.Log ("xAxis" + xAxis.ToString ());
			Debug.Log ("pos.x" + pos.x.ToString ());
			Debug.Log ("pos.y" + pos.y.ToString ());
		}
		bounds.center = transform.position;
		// Keep the ship constrained to the screen bounds
		Vector3 off = Utils.ScreenBoundsCheck(bounds, BoundsTest.onScreen);
		if ( off != Vector3.zero ) {
			pos -= off;
			transform.position = pos;
		}

		//Rotate the ship to make it feel more dynamic
		transform.rotation = Quaternion.Euler (yAxis*pitchMult, xAxis*rollMult, 0);

	}
}
