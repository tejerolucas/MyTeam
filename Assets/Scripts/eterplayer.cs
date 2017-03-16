using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Firebase.Database;
using System.IO;

public class eterplayer : MonoBehaviour {
		public Image _image;
		public Text _name;
		public Text _posicion;
		public string nombre;
		public string posicion;
		private string url;
		public GameObject puntuador;
		public RectTransform recttransform;
		private string filename;
		private string userid;
	public DataSnapshot data;

		public void SetData (DataSnapshot child)
	{
		_name.text = child.Child ("nombre").Value.ToString ();
		_posicion.text = child.Child ("puesto").Value.ToString ();
		url = child.Child ("foto").Value.ToString ();

		nombre = child.Child ("nombre").Value.ToString ().ToLower ();
		posicion = child.Child ("puesto").Value.ToString ().ToLower ();
		data = child;
		filename = url.Replace ("http://images.etermax.com/rrhh/staff/", "");
		filename = filename.Replace (".jpg", "");
		userid=child.Child ("userid").Value.ToString ();
		if (Resources.Load ("Fotos/"+filename) == null) {
			StartCoroutine(GetPicture(url));
		} else {
			_image.sprite = Resources.Load<Sprite> ("Fotos/" + filename);
		}
			
		}

		IEnumerator GetPicture (string url2)
	{
		// Start a download of the given URL
		WWW www = new WWW (url2);
				
		// Wait for download to complete
		yield return www;

		// assign texture
		if (www.error == null) {
			Texture2D tex2d = www.texture;
			Sprite sp = Sprite.Create (www.texture, new Rect (0, 0, tex2d.width, tex2d.height), new Vector2 (0.5f, 0.5f));
				byte[] bytes = tex2d.EncodeToJPG();
				File.WriteAllBytes(Application.dataPath + "/../Assets/Resources/Fotos/"+filename+".jpg", bytes);
				_image.sprite=sp;
				}
		}

		public void AbrirPuntuador(){
		puntajemanager puntaje=puntuador.GetComponent<puntajemanager>();
		puntaje.Reset();
		puntaje.SetData (_name.text, _posicion.text, _image.sprite,userid);
		puntuador.SetActive(true);
		}
}
