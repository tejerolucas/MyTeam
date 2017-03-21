using UnityEngine;
using System.Collections;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;

public class amonestar : GenericPopUp {

	public void Amonestar (int cant)
	{

		FirebaseDatabase.DefaultInstance
			.GetReference("Jugadores")
			.GetValueAsync().ContinueWith(task => {
				if (task.IsFaulted) {
					// Handle the error...
				}
				else if (task.IsCompleted) {
					DataSnapshot snapshot = task.Result;
					DatabaseReference reference = FirebaseDatabase.DefaultInstance.GetReference ("Jugadores/"+_userid);
					int pre=0;
					Debug.Log(snapshot.Child(_userid).Child("amonestaciones").Value.ToString());
					if (int.TryParse(snapshot.Child(_userid).Child("amonestaciones").Value.ToString(), out pre))
					{	
						pre+=2;
						reference.Child ("amonestaciones").SetValueAsync (pre);
					}
				}
			});
		


	}
}
