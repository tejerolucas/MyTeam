using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

public class UserAuth : MonoBehaviour {
	public static UserAuth instance;

	Firebase.DependencyStatus dependencyStatus = Firebase.DependencyStatus.UnavailableOther;
	Firebase.Auth.FirebaseAuth auth;
	Firebase.Auth.FirebaseUser user;

	void Awake ()
	{
		if (instance == null) {
			instance = this;
		}

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

	void InitializeFirebase() {
				auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
				auth.StateChanged += AuthStateChanged;
				AuthStateChanged(this, null);
		}

	void AuthStateChanged(object sender, System.EventArgs eventArgs) {
				if (auth.CurrentUser != user) {
						bool signedIn = user != auth.CurrentUser && auth.CurrentUser != null;
						user = auth.CurrentUser;
						if (signedIn) {

						}
				}
		}
	
	public void CreateUser(string email,string password) {
				auth.CreateUserWithEmailAndPasswordAsync(email, password)
						.ContinueWith((task) => HandleCreateResult(task));
		}

	void HandleCreateResult(Task<Firebase.Auth.FirebaseUser> authTask,
				string newDisplayName = null) {
//				if (LogTaskCompletion(authTask, "User Creation")) {
//						if (auth.CurrentUser != null) {
//								UpdateUserProfile(newDisplayName: newDisplayName);
//						}
//				}
		}

	public void UpdateUserProfile(string newDisplayName = null) {
				if (user == null) {
						return;
				}
				user.UpdateUserProfileAsync(new Firebase.Auth.UserProfile {
						DisplayName = "displayName",
						PhotoUrl = user.PhotoUrl,
				});
		}


}
