const functions = require('firebase-functions');

const admin =require("firebase-admin");
admin.initializeApp(functions.config().firebase);

//Nuevo Jugador
// exports.newPlayer = functions.database.ref("Jugadores/{newplayer}/nombre").onWrite(event =>{
// 	if (event.data.previous.exists()) {
//         return;
//       }
//       if (!event.data.exists()) {
//                 return;
//       }
// 	const request =event.data.val();
// 	var     payload={
// 		notification: {
//     		   title: "Nuevo Jugador Creado",
//     		body: event.data.val()+" creo su usuario"
//   		}
// 	};
// 	var registrationToken="fMBwu_nJF7o:APA91bFmYW6fTPzvDFBP0_h-o1BBN0inIsKdQFtZ7cB0KVn5PvPFTlTaJjtnS_fWtOFdzV-cRog4tzHw-bf6MM4YU2auZ9gLW_rcu0_QgtQ6iBZugkBVev9kMfUyqmLk_BazeuBR0n9Z";
// 	admin.messaging().sendToDevice(registrationToken,payload)
// });

// exports.newPlayerSlack = functions.database.ref("Jugadores/{newplayer}").onWrite(event =>{
//    if (event.data.previous.exists()|| !event.data.exists()) {
//         return Promise.resolve();
//       }
// // Grab the current value of what was written to the Realtime Database.
// const original = event.data.val();
// const userRef = event.data.adminRef.root.child('Jugadores').child(event.params.newplayer);
// original.isAdmin=true;
// console.log(original.foto);
// console.log(userRef.child('foto').val())
//  return userRef.update(original).then(() => {
//       return Promise.resolve();
//     });
// });

// //HTTP Request Agrega 10 jugadores
// exports.AddPlayersEvent = functions.https.onRequest((req, res) => {
//   for (var i = 10 - 1; i >= 0; i--) {
//     admin.database().ref('/Evento/Jugadores').push({i: "original"});
//   }
//   res.send("Agregue 10 Jugadores al Evento!");
// });

exports.Test = functions.https.onRequest((req, res) => {
  const id = req.query.id;
  const ref= admin.database().ref('Jugadores/'+id);

ref.once("value").then(function(snapshot) {
    var data = snapshot.val();
    res.send(data.nombre);
  });
   
   });

exports.SendMessage = functions.https.onRequest((req, res) => {
  const id = req.query.id;
  const mensaje=req.query.mensaje;
  const ref= admin.database().ref('Jugadores/'+id);

ref.once("value").then(function(snapshot) {
    var data = snapshot.val();
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
  });
   
   });





