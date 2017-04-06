using UnityEngine;
using System.Collections;
using MaterialUI;
using UnityEngine.UI;

public class cancha : MonoBehaviour {
	public Text tipo;
	public VectorImage icono;

	public void SetFiledColor (Color col){
		TweenManager.TweenColor(color => icono.color = color, icono.color, col, 3f);
	}
}
