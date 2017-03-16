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
	public string userid;

	void Start () {
		button.SetActive(false);
	}
	
	public void updatepuntaje () {
		if(estrellas.ready&&corazones.ready){
			button.SetActive(true);
		}
	}

	public void SetData (string nombre, string posicion, Sprite sprite, string id){
		_name.text=nombre;
		_position.text=posicion;
		_image.sprite=sprite;
		userid = id;
	}

	public void SendValoration(){
		DatabaseReference reference = FirebaseDatabase.DefaultInstance.GetReference ("Jugadores");
		reference=reference.Child (UserAuth.instance.user.UserId);
		reference=reference.Child ("valoraciones");
		reference=reference.Child (userid);
		reference.Child ("nombre").SetValueAsync(_name.text);
		reference.Child ("puesto").SetValueAsync(_position.text);
		reference.Child ("estrellas").SetValueAsync(estrellas.cant);
		reference.Child ("corazones").SetValueAsync(corazones.cant);
		this.gameObject.SetActive (false);
	}
	
	public void Reset(){
				button.SetActive(false);
				corazones.Reset();
				estrellas.Reset();
	}
}
