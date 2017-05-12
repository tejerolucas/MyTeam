using UnityEngine;
using System.Collections;
using MaterialUI;
using UnityEngine.UI;

public class Colorring : MonoBehaviour {
	public VectorImage imagen;
	public Image data;
	public float[] numeros;
	public Color[] colores;
	public float num;

	void Start () {
		num = data.fillAmount;
	}
	
	void Update () {
		if(num!=data.fillAmount){
			num = data.fillAmount;
			for (int i = 0; i < numeros.Length; i++) {
				if(num>numeros[i]){
					float dif = (numeros [i + 1] - numeros [i]);
					imagen.color = Color.Lerp (colores [i], colores [i+1],(num-numeros[i])/dif);
				}
			}

		}
	}
}
