using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NotificationManager : MonoBehaviour {
	public static NotificationManager instance;
	public string topic="Etermax";

	public void Init ()
	{
		if (instance == null) {
			instance = this;
		}
  		Firebase.Messaging.FirebaseMessaging.TokenReceived += OnTokenReceived;
  		Firebase.Messaging.FirebaseMessaging.MessageReceived += OnMessageReceived;
		SubscribeTopic (true);
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
}
