
using UnityEngine;
using System.Collections;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using Firebase.Database;

public class Signin : MonoBehaviour {
	public string email;
	public string password;

	public void SignIn(){
		Debug.LogWarning ("SignIn");	
		UserAuth.instance.CreateUserWithEmail (email, password, "nombre");	
	}

		public void changeemail(string st){
				email = st;
		}

		public void changepassword(string st){
				password = st;
		}
}
