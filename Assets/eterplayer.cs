using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Firebase.Database;

public class eterplayer : MonoBehaviour {
		public Image _image;
		public Text _name;
		public Text _posicion;
		public string nombre;
		public string posicion;
		private string url;
		public GameObject puntuador;
		public RectTransform recttransform;
	public DataSnapshot data;

		public void SetData(DataSnapshot child){
				_name.text=child.Child("nombre").Value.ToString();
				_posicion.text=child.Child("puesto").Value.ToString();
				url=child.Child("foto").Value.ToString();

				nombre=child.Child("nombre").Value.ToString().ToLower();
				posicion=child.Child("puesto").Value.ToString().ToLower();
		data = child;
			//StartCoroutine(GetPicture(url));
		}

		IEnumerator GetPicture(string url2) {
				// Start a download of the given URL
				WWW www = new WWW(url2);
				
				// Wait for download to complete
				yield return www;

				// assign texture
				if(www.error==null){
				Texture2D tex2d= www.texture;
				Sprite sp=Sprite.Create(www.texture,new Rect(0,0,tex2d.width,tex2d.height),new Vector2(0.5f,0.5f));
				_image.sprite=sp;
				}
		}

		public void AbrirPuntuador(){
				
				puntajemanager puntaje=puntuador.GetComponent<puntajemanager>();
				puntaje.Reset();
				puntaje.SetImage(_image.sprite,url);
				puntaje.SetName(_name.text);
				puntaje.SetPosition(_posicion.text);
				puntuador.SetActive(true);
		}
}
