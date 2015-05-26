using UnityEngine;
using System.Collections;

public class Change_Scene : MonoBehaviour {

	public void ChangeToScene (int sceneToChangeTo){
		Application.LoadLevel (sceneToChangeTo);
	}

	public void ChangeToLevel0(){
		Application.LoadLevel (0);
	}

}
