using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class messagingtest : MonoBehaviour {

	public void Start() {
  		Firebase.Messaging.FirebaseMessaging.TokenReceived += OnTokenReceived;
  		Firebase.Messaging.FirebaseMessaging.MessageReceived += OnMessageReceived;
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
}
