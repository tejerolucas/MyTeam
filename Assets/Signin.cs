
using UnityEngine;
using System.Collections;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;

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
				
		auth.CreateUserWithEmailAndPasswordAsync(email, password)
						.ContinueWith((task) => HandleCreateResult(task));			
	}

		void HandleCreateResult(Task<Firebase.Auth.FirebaseUser> authTask) {
				if (LogTaskCompletion(authTask, "User Creation")) {
						if (auth.CurrentUser != null) {

								//UpdateUserProfile();
						}
				}
		}
		public void UpdateUserProfile(string newDisplayName = null) {
				if (user == null) {
						return;
				}
				displayName = newDisplayName ?? displayName;
				user.UpdateUserProfileAsync(new Firebase.Auth.UserProfile {
						DisplayName = displayName,
						PhotoUrl = user.PhotoUrl,
				}).ContinueWith(HandleUpdateUserProfile);
		}

		void HandleUpdateUserProfile(Task authTask) {
				if (LogTaskCompletion(authTask, "User profile")) {
						DisplayUserInfo(user, 1);
				}
		}

		void DisplayUserInfo(Firebase.Auth.IUserInfo userInfo, int indentLevel) {
				string indent = new String(' ', indentLevel * 2);
				var userProperties = new Dictionary<string, string> {
						{"Display Name", userInfo.DisplayName},
						{"Email", userInfo.Email},
						{"Photo URL", userInfo.PhotoUrl != null ? userInfo.PhotoUrl.ToString() : null},
						{"Provider ID", userInfo.ProviderId},
						{"User ID", userInfo.UserId}
				};
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
