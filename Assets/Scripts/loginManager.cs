using UnityEngine;

public class loginManager : MonoBehaviour {
	public string email;
	public string password;
	
	public void Login(){
		UserAuth.instance.Signin (email, password);
	}

	public void changeemail(string st){
				email = st;
		}

		public void changepassword(string st){
				password = st;
		}
}
