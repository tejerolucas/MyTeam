using UnityEngine;
using MaterialUI;

public class Signin : MonoBehaviour {
	public string email;
	public string password;

	public void SignIn ()
	{
		Debug.LogWarning ("SignIn");	
		if (!email.Contains ("@etermax.com")) {
			DialogManager.ShowAlert("No es un Email valido de Etermax", "Error!", MaterialIconHelper.GetIcon("error_outline"));
		} else {
			UserAuth.instance.CreateUserWithEmail (email, password, "nombre");	
		}

	}

		public void changeemail(string st){
				email = st;
		}

		public void changepassword(string st){
				password = st;
		}

	void OnEnable(){
		Debug.Log ("ENABLE LOGIN");
	}

	void Update(){
		if(Input.GetKeyDown(KeyCode.A)){
			UserAuth.instance.AlertTest ();
		}
	}
}
