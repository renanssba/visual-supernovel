using UnityEngine;
using System.Collections;

public class DestroyIfDuplicate : MonoBehaviour {
	
	void Awake() {
		DestroyDuplicate();
	}

	void DestroyDuplicate(){
		if(GameObject.FindGameObjectsWithTag("Persistence").Length > 1)
			Destroy(this.gameObject);
	}
}
