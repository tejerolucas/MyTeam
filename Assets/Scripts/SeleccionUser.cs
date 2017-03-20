using UnityEngine;
using System.Collections;
using Firebase.Database;
using UnityEngine.UI;
using MaterialUI;

public class SeleccionUser : GenericPopUp {

	public void CreateDB_Player ()
	{
		if (UserAuth.instance.user !=null) {
			UserAuth.instance.UpdateUserProfile (_name.text);
			Debug.Log ("Update player");
			DatabaseReference reference = FirebaseDatabase.DefaultInstance.GetReference ("Jugadores");
			reference=reference.Child (UserAuth.instance.user.UserId);
			reference.Child ("nombre").SetValueAsync(_name.text);
			reference.Child ("puesto").SetValueAsync(_position.text);
			reference.Child ("amonestaciones").SetValueAsync(0);
			reference.Child ("foto").SetValueAsync(_pictureurl);
			reference.Child ("filename").SetValueAsync(_filename);
			reference.Child ("userid").SetValueAsync(UserAuth.instance.user.UserId);
			reference.Child ("token").SetValueAsync(UserAuth.instance._usertoken);
			reference.Child ("email").SetValueAsync(UserAuth.instance.user.Email);
			reff.Child ("Usado").SetValueAsync ("true");
		}
		UserAuth.instance._username = _name.text;
		UserAuth.instance._userposition = _position.text;
		UserAuth.instance._userfilename = _filename;
		this.gameObject.SetActive (false);
	}

}
