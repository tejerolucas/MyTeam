using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using MaterialUI;
using Firebase.Database;

public class UserAuth : MonoBehaviour
{
	Firebase.Auth.FirebaseAuth auth;
	public Firebase.Auth.FirebaseUser user;
	Firebase.DependencyStatus dependencyStatus = Firebase.DependencyStatus.UnavailableOther;
	public static UserAuth instance;

	public string _username;
	public string _userposition;
	public string _userfilename;
	public string _usertoken;
	public string tipo;
	public GameObject SigninGo;
	public GameObject LoginGo;
	public ScreenView screenmanager;
	public GameObject appbar;
	private string topic="Etermax";


	void Start ()
	{
		#if !UNITY_EDITOR
		LoginGo.SetActive (true);
		Debug.Log("START USER AUTH");
		#else
		//screenmanager.Transition ("Home");
		#endif
		if (instance == null) {
			instance = this;
		}
		dependencyStatus = Firebase.FirebaseApp.CheckDependencies ();
		if (dependencyStatus != Firebase.DependencyStatus.Available) {
			Firebase.FirebaseApp.FixDependenciesAsync ().ContinueWith (task => {
				dependencyStatus = Firebase.FirebaseApp.CheckDependencies ();
				if (dependencyStatus == Firebase.DependencyStatus.Available) {
					InitializeFirebase ();
				} else {
					Debug.LogError (
						"Could not resolve all Firebase dependencies: " + dependencyStatus);
				}
			});
		} else {
			InitializeFirebase ();
		}
	}

	// Handle initialization of the necessary firebase modules:
	void InitializeFirebase ()
	{
		Debug.Log ("Setting up Firebase Auth");
		auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
		auth.StateChanged += AuthStateChanged;
		AuthStateChanged (this, null);
		Firebase.Messaging.FirebaseMessaging.TokenReceived += OnTokenReceived;
  		Firebase.Messaging.FirebaseMessaging.MessageReceived += OnMessageReceived;
		ReloadUser ();
	}

	public void OnTokenReceived(object sender, Firebase.Messaging.TokenReceivedEventArgs token) {
		UserAuth.instance._usertoken = token.Token;
	}

	public void OnMessageReceived (object sender, Firebase.Messaging.MessageReceivedEventArgs e)
	{
		foreach (KeyValuePair<string,string> st in e.Message.Data) {
			Debug.Log ("Dato: Key: " + st.Key + "-Value: " + st.Value);
		}
	  UnityEngine.Debug.Log("Received a new message from: " + e.Message.From);
	}

	public void SubscribeTopic (bool state){
		if (state) {
			Firebase.Messaging.FirebaseMessaging.Subscribe (topic);
			PlayerPrefs.SetInt ("Notifications", 1);
		} else {
			Firebase.Messaging.FirebaseMessaging.Unsubscribe (topic);
			PlayerPrefs.SetInt ("Notifications", 0);
		}
	}

	void AuthStateChanged (object sender, System.EventArgs eventArgs)
	{
		Debug.Log ("AuthStateChanged");
		if (auth.CurrentUser != user) {
			bool signedIn = user != auth.CurrentUser && auth.CurrentUser != null;
			if (!signedIn && user != null) {
				Debug.Log ("Signed out " + user.UserId);
			}
			user = auth.CurrentUser;
		}
	}



	bool LogTaskCompletion (Task task, string operation)
	{
		bool complete = false;
		if (task.IsCanceled) {
			Debug.Log (operation + " canceled.");
		} else if (task.IsFaulted) {
			Debug.Log (operation + " encounted an error.");
			DialogManager.ShowAlert(task.Exception.ToString(), "Error!", MaterialIconHelper.GetIcon("error_outline"));
			Debug.Log (task.Exception.ToString ());
		} else if (task.IsCompleted) {
			Debug.Log (operation + " completed");
			complete = true;
		}
		return complete;
	}

	public void AlertTest ()
	{
		DialogManager.ShowAlert ("ALERT TEasdygsdfjhasgdfjhasgdfjhkasgdfkjhgasdjfkhgasjkdhfgaskjhdfgasjkhdgfjakshdgfajshdgfjkashgdfjkahsgdfjkahsgdfjkhasgdfkjhasgdfjhkagsdfkjhagsdfjkhasgdfkjhasgdfkjasdhgfjkhST!", "Error!", MaterialIconHelper.GetIcon ("error_outline"));
	}

	public void CreateUserWithEmail (string email, string password, string displayName)
	{
		Debug.Log (String.Format ("Attempting to create user {0}...", email));
		string newDisplayName = displayName;
		auth.CreateUserWithEmailAndPasswordAsync (email, password)
						.ContinueWith ((task) => HandleCreateResult (task, newDisplayName: newDisplayName));
	}

	void HandleCreateResult (Task<Firebase.Auth.FirebaseUser> authTask,
	                         string newDisplayName = null)
	{

		if (LogTaskCompletion (authTask, "User Creation")) {
			if (auth.CurrentUser != null) {
				Debug.Log (String.Format ("User Info: {0}  {1}", auth.CurrentUser.Email,
				auth.CurrentUser.ProviderId));
				UpdateUserProfile (newDisplayName: newDisplayName);
			}
		}
		SubscribeTopic (true);
		appbar.SetActive (false);
		screenmanager.Transition("Empleados");
	}

	// Update the user's display name with the currently selected display name.
	public void UpdateUserProfile (string newDisplayName)
	{
		if (user == null) {
			Debug.Log ("Not signed in, unable to update user profile");
			return;
		}
		Debug.Log ("Updating user profile");
				
		user.UpdateUserProfileAsync (new Firebase.Auth.UserProfile {
			DisplayName = newDisplayName,
			PhotoUrl = user.PhotoUrl,
		}).ContinueWith (HandleUpdateUserProfile);
	}

	void HandleUpdateUserProfile (Task authTask)
	{
		SigninGo.SetActive (false);
		LoginGo.SetActive (false);
	}

	public void LogIn (string email, string password)
	{
		Debug.Log (String.Format ("Attempting to sign in as {0}...", email));
		auth.SignInWithEmailAndPasswordAsync (email, password)
						.ContinueWith (HandleLoginResult);
	}

	void HandleLoginResult (Task<Firebase.Auth.FirebaseUser> authTask)
	{
				
		if (LogTaskCompletion (authTask, "Sign-in")) {
			LoginGo.SetActive (false);
			SigninGo.SetActive (false);
			screenmanager.Transition ("Home");
		}
	}

	public void ReloadUser ()
	{
		if (user == null) {
			Debug.Log ("Not signed in, unable to reload user.");
			return;
		}
		Debug.Log ("Reload User Data");
		user.ReloadAsync ().ContinueWith (HandleReloadUser);
		LoginGo.SetActive (false);
		SigninGo.SetActive (false);
		screenmanager.Transition ("Home");
	}

	void HandleReloadUser (Task authTask)
	{
		if (LogTaskCompletion (authTask, "Reload User")) {
			DatabaseReference reference = FirebaseDatabase.DefaultInstance.GetReference ("Jugadores");
			reference = reference.Child (UserAuth.instance.user.UserId);
			UserAuth.instance._username = reference.Child ("nombre").GetValueAsync ().ToString ();
			UserAuth.instance._userposition = reference.Child ("puesto").GetValueAsync ().ToString ();
			UserAuth.instance._userfilename = reference.Child ("filename").GetValueAsync ().ToString ();
		}
	}


	public void GetUserToken ()
	{
		if (user == null) {
			Debug.Log ("Not signed in, unable to get token.");
			return;
		}
		Debug.Log ("Fetching user token");
		user.TokenAsync (false).ContinueWith (HandleGetUserToken);
	}

	void HandleGetUserToken (Task<string> authTask)
	{
		if (LogTaskCompletion (authTask, "User token fetch")) {
			Debug.Log ("Token = " + authTask.Result);
		}
	}

	public void SignOut ()
	{
		Debug.Log ("Signing out.");
		auth.SignOut ();
	}


	public void DeleteUser ()
	{
		if (auth.CurrentUser != null) {
			Debug.Log (String.Format ("Attempting to delete user {0}...", auth.CurrentUser.UserId));
						
			auth.CurrentUser.DeleteAsync ().ContinueWith (HandleDeleteResult);
		} else {
			Debug.Log ("Sign-in before deleting user.");
		}
	}

	void HandleDeleteResult (Task authTask)
	{
		LogTaskCompletion (authTask, "Delete user");
	}

	// Show the providers for the current email address.
	public void DisplayProviders (string email)
	{
		auth.FetchProvidersForEmailAsync (email).ContinueWith ((authTask) => {
			if (LogTaskCompletion (authTask, "Fetch Providers")) {
				Debug.Log (String.Format ("Email Providers for '{0}':", email));
				foreach (string provider in authTask.Result) {
					Debug.Log (provider);
				}
			}
		});
	}

	// Send a password reset email to the current email address.
	public void SendPasswordResetEmail (string email)
	{
		auth.SendPasswordResetEmailAsync (email).ContinueWith ((authTask) => {
			if (LogTaskCompletion (authTask, "Send Password Reset Email")) {
				Debug.Log ("Password reset email sent to " + email);
			}
		});
	}
}