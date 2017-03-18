using UnityEngine;
using System.Collections;
using Firebase.Database;
using UnityEngine.UI;
using MaterialUI;

public class SeleccionUser : MonoBehaviour {
	public Text _name;
	public Text _position;
	public Image _picture;
	private string _nameString;
	private string _filename;
	private string _url;

	public void UpdateData (string nombre,string puesto,string filename,string url) {
		_nameString = nombre;
		_name.text = nombre;
		_position.text = puesto;
		_filename = filename;
		_url = url;
		if (Resources.Load ("Fotos/"+filename) != null) {
			_picture.sprite = Resources.Load<Sprite> ("Fotos/" + filename);
		}
	}

	public void CreateDB_Player ()
	{
		if (UserAuth.instance.user !=null) {
			UserAuth.instance.UpdateUserProfile (_nameString);
			Debug.Log ("Update player");
			DatabaseReference reference = FirebaseDatabase.DefaultInstance.GetReference ("Jugadores");
			reference=reference.Child (UserAuth.instance.user.UserId);
			reference.Child ("nombre").SetValueAsync(_nameString);
			reference.Child ("puesto").SetValueAsync(_position.text);
			reference.Child ("amonestaciones").SetValueAsync(0);
			reference.Child ("foto").SetValueAsync(_url);
			reference.Child ("filename").SetValueAsync(_filename);
			reference.Child ("userid").SetValueAsync(UserAuth.instance.user.UserId);
			reference.Child ("token").SetValueAsync(UserAuth.instance._usertoken);
		}
		UserAuth.instance._username = _nameString;
		UserAuth.instance._userposition = _position.text;
		UserAuth.instance._userfilename = _filename;
	}

}
