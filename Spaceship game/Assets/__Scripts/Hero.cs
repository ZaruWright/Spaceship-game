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
	[SerializeField]
	private float _shieldLevel = 1;
	// Restart delay
	public float gameRestartDelay = 2f;
	public bool ____________________________;
	public Bounds bounds;
	// Declare a new delegate type WeaponFireDelegate
	public delegate void WeaponFireDelegate();
	// Create a WeaponFireDelegate field named fireDelegate.
	public WeaponFireDelegate fireDelegate;

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
			Debug.Log ("xAxis " + xAxis.ToString ());
			Debug.Log ("pos.x " + pos.x.ToString ());
			Debug.Log ("pos.y " + pos.y.ToString ());
		}
		// Change the bound center position
		bounds.center = transform.position;
		// Keep the ship constrained to the screen bounds
		Vector3 off = Utils.ScreenBoundsCheck(bounds, BoundsTest.onScreen);
		if ( off != Vector3.zero ) {
			pos -= off;
			transform.position = pos;
		}

		//Rotate the ship to make it feel more dynamic
		transform.rotation = Quaternion.Euler (yAxis*pitchMult, xAxis*rollMult, 0);

		// Use the fireDelegate to fire Weapons
		// First, make sure the Axis("Jump") button is pressed
		// Then ensure that fireDelegate isn't null to avoid an error
		if (debug) {
			Debug.Log("Jump " + Input.GetAxis("Jump").ToString());
			Debug.Log("fireDelegate" + fireDelegate.ToString());
		}
		if (Input.GetAxis("Jump") == 1 && fireDelegate != null) { 
			fireDelegate();
		}

	}

	// This variable holds a reference to the last triggering GameObject
	public GameObject lastTriggerGo = null;

	void OnTriggerEnter(Collider other) {
		// Find the tag of other.gameObject or its parent GameObjects
		GameObject go = Utils.FindTaggedParent(other.gameObject);
		// If there is a parent with a tag
		if (go != null) {
			// Make sure it's not the same triggering go as last time
			if (go == lastTriggerGo) {
				return;
			}
			lastTriggerGo = go;
			if (go.tag == "Enemy") {
				// If the shield was triggered by an enemy
				// Decrease the level of the shield by 1
				shieldLevel--;
				// Destroy the enemy
				Destroy(go);
			} else {
				print("Triggered: "+go.name);
			}
		} 
		else {
			// Otherwise announce the original other.gameObject
			print("Triggered: "+other.gameObject.name);
		}
	}

	public float shieldLevel {
		get {
			return( _shieldLevel );
		}
		set {
			_shieldLevel = Mathf.Min( value, 4 );
			// If the shield is going to be set to less than zero
			if (value < 0) {
				Destroy(this.gameObject);
				// Tell Main.S to restart the game after a delay
				Main.S.DelayedRestart( gameRestartDelay );
			}
		}
	}

}
