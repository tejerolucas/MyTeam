using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Firebase.Database;

public class puntajemanager : MonoBehaviour {
		public GameObject button;
		public Image _image;
		public Text _name;
		public Text _position;
		public togglelist estrellas;
		public togglelist corazones;
	public string url;

	void Start () {
		button.SetActive(false);
	}
	
	public void updatepuntaje () {
		if(estrellas.ready&&corazones.ready){
			button.SetActive(true);
		}
	}

	public void SetImage(Sprite sprite,string ur){
		_image.sprite=sprite;	
		url = ur;
	}
	public void SetName(string nombre){
		_name.text=nombre;
	}
	public void SetPosition(string posicion){
		_position.text=posicion;
	}


	
	public void Reset(){
				button.SetActive(false);
				corazones.Reset();
				estrellas.Reset();
	}
}
