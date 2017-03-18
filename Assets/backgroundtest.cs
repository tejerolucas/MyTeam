using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class backgroundtest : MonoBehaviour {
	public Sprite[] sprites;
	public Image imagen;
	// Use this for initialization
	void Awake ()
	{
		if (Screen.width > 600) {
			imagen.sprite = sprites [0];
		} else {
			imagen.sprite=sprites[1];
		}
	}
}
