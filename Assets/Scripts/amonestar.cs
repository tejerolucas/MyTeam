using UnityEngine;
using System.Collections;
using Firebase.Database;

public class amonestar : GenericPopUp {

	public void Amonestar (int cant)
	{
		DatabaseReference reference = FirebaseDatabase.DefaultInstance.GetReference ("Jugadores/"+_userid);
		int pre = 0;
		if( int.TryParse( reference.Child ("amonestaciones").GetValueAsync().ToString(),out pre)){
			reference.Child ("amonestaciones").SetValueAsync (cant+pre);
		}

	}
}
