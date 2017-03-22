using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Splash : MonoBehaviour {
	void Start () {
		Invoke ("LoadApp", 4);
	}
	
	public void LoadApp () {
		SceneManager.LoadScene (1);
	}
}
