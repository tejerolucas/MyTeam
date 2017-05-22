using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Firebase.Database;
using System.IO;

public class eterplayer : MonoBehaviour
{
	public Image _image;
	public Text _name;
	public Text _posicion;
	public string nombre;
	public string posicion;
	private string url;
	public GameObject Popup;
	public RectTransform recttransform;
	private string filename;
	private string userid;
	public DataSnapshot data;
	public int corazones;
	public int estrellas;

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
		userid = child.Child ("userid").Value.ToString ();
		if (Resources.Load ("Fotos/" + filename) == null) {
			StartCoroutine (GetPicture (url));
		} else {
			_image.sprite = Resources.Load<Sprite> ("Fotos/" + filename);
		}
			
	}

	public void SetValoration (int cora,int estre)
	{
		corazones = cora;
		estrellas = estre;
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
			byte[] bytes = tex2d.EncodeToJPG ();
			File.WriteAllBytes (Application.dataPath + "/../Assets/Resources/Fotos/" + filename + ".jpg", bytes);
			_image.sprite = sp;
		}
	}

	public void AbrirPopUp ()
	{
		puntajemanager puntaje = Popup.GetComponent<puntajemanager> ();
		FirebaseDatabase.DefaultInstance.GetReference ("Jugadores").Child("Editor").Child("valoraciones").Child(userid).GetValueAsync().ContinueWith (task2 => {
			puntaje.SetData (_name.text, _posicion.text, filename, url, userid);
			if (task2.IsFaulted) {
				Debug.LogError ("ERROR");
			} else if (task2.IsCompleted) {
				DataSnapshot sn = task2.Result;
				int corazones=0;
				int estrellas=0;
				if(sn.Child("corazones").Value!=null && sn.Child("estrellas").Value !=null){
				int.TryParse(sn.Child("corazones").Value.ToString(),out corazones);
				int.TryParse(sn.Child("estrellas").Value.ToString(),out estrellas);
				Debug.LogFormat("C:{0}-E:{1}",corazones.ToString(),estrellas.ToString());
				puntaje.GetValoration (corazones,estrellas);

				}else{
					puntaje.GetValoration (0,0);
				}
				Popup.SetActive (true);
			}
		});

	}
}
