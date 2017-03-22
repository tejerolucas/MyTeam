using UnityEngine;
using MaterialUI;

public class loginManager : MonoBehaviour {
	public string email;
	public string password;
	
	public void Login ()
	{
		Debug.Log ("BOTON LOGIN");
		if (!email.Contains ("@etermax.com")) {
			DialogManager.ShowAlert("No es un Email valido de Etermax", "Error!", MaterialIconHelper.GetIcon("error_outline"));
		} else {
			UserAuth.instance.LogIn (email, password);
		}

	}

	public void changeemail(string st){
				email = st;
		}

		public void changepassword(string st){
				password = st;
		}

	void OnEnable(){
	Debug.Log("ENABLE SIGNIN");
	}

	void Update(){
		if(Input.GetKeyDown(KeyCode.D)){
			UserAuth.instance.AlertTest ();
		}
	}
}
