using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Firebase.Database;
using System.IO;
using MaterialUI;

public class GenericPopUp : MaterialDialog {
	public Text _name;
	public Text _position;
	public Image _picture;
	public string _filename;
	public string _pictureurl;
	public string _userid;
	public string _ownuserid;
	public DatabaseReference reff;

	public void SetData (string nombre,string puesto,string filename,string url,string userid="") {
		_name.text = nombre;
		_position.text = puesto;
		_filename = filename;
		_pictureurl = url;
		#if UNITY_EDITOR
		_ownuserid="Editor";
		#else
		_ownuserid=UserAuth.instance.user.UserId;
		#endif
		_userid = userid;
		if (Resources.Load ("Fotos/"+filename) != null) {
			_picture.sprite = Resources.Load<Sprite> ("Fotos/" + filename);
		}
	}


	public void SetReference (DatabaseReference re)
	{
		reff = re;
	}
}
