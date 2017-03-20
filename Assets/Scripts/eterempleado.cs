using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Firebase.Database;
using System.IO;

public class eterempleado : MonoBehaviour {
	public Image _image;
	public Text _name;
	public Text _posicion;
	public string nombre;
	public string posicion;
	private string url;
	public GameObject puntuador;
	public RectTransform recttransform;
	private string filename;
	private DatabaseReference reff;
	private string valor;

	public void SetData (DataSnapshot child,DatabaseReference reference,object value)
	{
		valor = value.ToString();
		_name.text = child.Child ("nombre").Value.ToString ();
		_posicion.text = child.Child ("puesto").Value.ToString ();
		url = child.Child ("foto").Value.ToString ();
		reff = reference;
		nombre = child.Child ("nombre").Value.ToString ().ToLower ();
		posicion = child.Child ("puesto").Value.ToString ().ToLower ();
		filename = url.Replace ("http://images.etermax.com/rrhh/staff/", "");
		filename = filename.Replace (".jpg", "");
		if (Resources.Load ("Fotos/"+filename) == null) {
			StartCoroutine(GetPicture(url));
		} else {
			_image.sprite = Resources.Load<Sprite> ("Fotos/" + filename);
		}

	}

	IEnumerator GetPicture (string url2)
	{
		WWW www = new WWW (url2);
		yield return www;
		if (www.error == null) {
			Texture2D tex2d = www.texture;
			Sprite sp = Sprite.Create (www.texture, new Rect (0, 0, tex2d.width, tex2d.height), new Vector2 (0.5f, 0.5f));
			byte[] bytes = tex2d.EncodeToJPG();
			File.WriteAllBytes(Application.dataPath + "/../Assets/Resources/Fotos/"+filename+".jpg", bytes);
			_image.sprite=sp;
		}
	}

	public void AbrirPopUp(){
		GenericPopUp seleccionador=puntuador.GetComponent<GenericPopUp>();
		seleccionador.SetData (_name.text, _posicion.text, filename,url);
		seleccionador.SetReference (reff);
		puntuador.SetActive(true);
	}
}
