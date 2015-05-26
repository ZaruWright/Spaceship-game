using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Pop_Up : MonoBehaviour {

	public GameObject popUp;
	public GameObject popUpParent;
	private GameObject go;
	public static Pop_Up S;

	void start(){
		S = this;
	}
	
	public void Create(string text, float time){
		go = (GameObject) Instantiate (popUp, popUpParent.transform.position, popUpParent.transform.rotation);
		go.GetComponentInChildren<Text> ().text = text;
		go.transform.parent = popUpParent.transform;
		Invoke("Destroy", time);
	}
	
	public void Destroy(){
		Destroy (go);
	}
}
