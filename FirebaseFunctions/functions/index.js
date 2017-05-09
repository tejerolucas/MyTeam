const functions = require('firebase-functions');

const admin =require("firebase-admin");
admin.initializeApp(functions.config().firebase);

// //HTTP Request Agrega 10 jugadores
// exports.AddPlayersEvent = functions.https.onRequest((req, res) => {
//   for (var i = 10 - 1; i >= 0; i--) {
//     admin.database().ref('/Evento/Jugadores').push({i: "original"});
//   }
//   res.send("Agregue 10 Jugadores al Evento!");
// });

//Envia push notification a un usuario especifico (id,mensaje)
exports.SendMessage = functions.https.onRequest((req, res) => {
  const id = req.query.id;
  const mensaje=req.query.mensaje;
  const ref= admin.database().ref('Jugadores/'+id);

  ref.once("value").then(function(snapshot) {
    var data = snapshot.val();
    if(data==null){
      res.send("No se encuentra jugador");
    }else{
      var payload={
        notification: {
          title: "Mensaje",
          body: mensaje,
          sound: "default"
        }
      };
      var registrationToken=data.token;
      admin.messaging().sendToDevice(registrationToken,payload)
      res.send("Mensaje enviado a "+data.nombre);
    }
  });
});

//devuelve un nombre random
exports.GetRandomUser=functions.https.onRequest((req,res)=>{
	const ref= admin.database().ref('Usuarios/');
	ref.once("value").then(function(snapshot) {
		var num=Math.floor(Math.random() * (snapshot.numChildren() - 0) + 0);
    var numstring=num.toString();
		if(snapshot.hasChild(numstring)){
   			res.send(snapshot.child(numstring).child("nombre").val());
   		}else{
   			res.send("NO tiene hijo");
   		}
	});
});

//Crea Jugadores Random(cantidad)
exports.CreatePlayers=functions.https.onRequest((req,res)=>{
  const cantidad=req.query.cantidad;
  var lista="";
  
  for (var i = cantidad - 1; i >= 0; i--) {
    const ref= admin.database().ref('Usuarios/');

    ref.once("value").then(function(snapshot) {
      var num=Math.floor(Math.random() * (snapshot.numChildren() - 0) + 0);
      var numstring=num.toString();

      if(snapshot.hasChild(numstring)){
        console.log(snapshot.child(numstring).child("nombre").val());
        lista=lista+snapshot.child(numstring).child("nombre").val()+"\n";
        snapshot.child(numstring).child("Usado").ref.set("true");
      }else{
        res.send("Error");
      }
    });
  }
  
  console.log("penis");
  console.log(lista);
  res.send(lista);
});

//borra todos los jugadores y saca el estado "Usado" de los usuarios ()
exports.ClearUsedUsers=functions.https.onRequest((req,res)=>{
var num=0;
var lista="Done";
var query = admin.database().ref("Usuarios").orderByKey();
query.once("value")
  .then(function(snapshot) {
    snapshot.forEach(function(childSnapshot) {
      if(childSnapshot.child("Usado").val()!=null) {
        childSnapshot.child("Usado").ref.remove().then(function() {
            console.log("Remove succeeded.")
        }).catch(function(error) {
            console.log("Remove failed: " + error.message)
        });
      }
  });
});
  const refjugadores= admin.database().ref('Jugadores');
  refjugadores.remove().then(function() {
            console.log("Remove Players succeeded.")
        }).catch(function(error) {
            console.log("Remove Players failed: " + error.message)
        });

  res.send(lista);
});

//crea un usuario (email,password)
exports.CreateNewUser=functions.https.onRequest((req,res)=>{
var lista="Done";
const email=req.query.email;
const password=req.query.password;
admin.auth().createUser({
    email: email,
    emailVerified: false,
    password: password
  })
  res.send(lista);
});


//borra un usuario especifico (email)
exports.DeleteUser=functions.https.onRequest((req,res)=>{
const email=req.query.email;
admin.auth().getUserByEmail(email)
  .then(function(userRecord) {
  	admin.auth().deleteUser(userRecord.uid)
  .then(function() {
    console.log("Successfully deleted user");
    res.send("Successfully deleted user");
  })
  .catch(function(error) {
    console.log("Error deleting user:", error);
    res.send(error.toString());
  });
  })
  .catch(function(error) {
  	res.send("ERROR FETCHING");
    console.log("Error fetching user data:", error);
  }); 
});





