
using UnityEngine;
using System.Collections;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using Firebase.Database;

public class Signin : MonoBehaviour {
		Firebase.Auth.FirebaseAuth auth;
		Firebase.Auth.FirebaseUser user;
		private string email;
		private string password;
		private string displayName="";
		Firebase.DependencyStatus dependencyStatus = Firebase.DependencyStatus.UnavailableOther;
	// Use this for initialization
	void Start () {
				dependencyStatus = Firebase.FirebaseApp.CheckDependencies();
				if (dependencyStatus != Firebase.DependencyStatus.Available) {
						Firebase.FirebaseApp.FixDependenciesAsync().ContinueWith(task => {
								dependencyStatus = Firebase.FirebaseApp.CheckDependencies();
								if (dependencyStatus == Firebase.DependencyStatus.Available) {
										InitializeFirebase();
								} else {
										Debug.LogError("Could not resolve all Firebase dependencies: " + dependencyStatus);
								}
						});
				} else {
					InitializeFirebase();
				}
	}

		void InitializeFirebase(){
				Debug.Log("Setting up Firebase Auth");
				auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
		}
	
	public void SignIn(){
		Debug.LogWarning ("SignIn");	
		auth.CreateUserWithEmailAndPasswordAsync(email, password)
						.ContinueWith((task) => HandleCreateResult(task));			
	}

		void HandleCreateResult(Task<Firebase.Auth.FirebaseUser> authTask) {
		Debug.LogWarning ("HandleCreateResult");
				if (LogTaskCompletion(authTask, "User Creation")) {
						if (auth.CurrentUser != null) {

								UpdateUserProfile();
						}
				}
		}
		public void UpdateUserProfile(string newDisplayName = null) {
				Debug.LogWarning ("UpdateUserProfile");
				if (user == null) {
						return;
				}
				displayName = newDisplayName ?? displayName;
				DatabaseReference reference = FirebaseDatabase.DefaultInstance.GetReference ("Jugadores");
				reference = reference.Push ();
				reference.Child ("test").SetValueAsync("DisplayUSerInfo");
				reference.Child ("UID").SetValueAsync(user.UserId);
				user.UpdateUserProfileAsync(new Firebase.Auth.UserProfile {
						DisplayName = displayName,
						PhotoUrl = user.PhotoUrl,
				});
				this.gameObject.SetActive (false);
				Debug.LogWarning ("UpdateUserProfile2");
		}
		

		bool LogTaskCompletion(Task task, string operation) {
				bool complete = false;
				if (task.IsCompleted) {
						complete = true;
				}
				return complete;
		}

		public void changeemail(string st){
				email = st;
		}

		public void changepassword(string st){
				password = st;
		}
}
