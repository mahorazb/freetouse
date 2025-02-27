
var findVeh;
let pushVeh;

let go = false;
mp.game.streaming.requestAnimDict("missfinale_c2ig_11");

mp.events.add('render', ( ) => {
    findVeh = pointingAt(6);

    if(findVeh){
        
          
          pushVeh = findVeh.entity;
    }

    if(go){
        pushVeh.setForwardSpeed(1.0);

        if(mp.game.controls.isDisabledControlPressed(0, 34)){
            mp.players.local.taskVehicleTempAction(pushVeh.handle, 11, 500);
        }
        if(mp.game.controls.isDisabledControlPressed(0, 9)){
            mp.players.local.taskVehicleTempAction(pushVeh.handle, 10, 500);
        }
    }
});


function pointingAt(distance) {
    const camera = mp.cameras.new("gameplay"); // gets the current gameplay camera

    let position = camera.getCoord(); // grab the position of the gameplay camera as Vector3

    let direction = camera.getDirection(); // get the forwarding vector of the direction you aim with the gameplay camera as Vector3

    let farAway = new mp.Vector3((direction.x * distance) + (position.x), (direction.y * distance) + (position.y), (direction.z * distance) + (position.z)); // calculate a random point, drawn on a invisible line between camera position and direction (* distance)

    let result = mp.raycasting.testPointToPoint(position, farAway, null, 2); // now test point to point - intersects with map and objects [1 + 16]

    return result; // and return the result ( undefined, if no hit )
}
/*

mp.keys.bind(0x48, true, function() {
    try {
    if(go){
        go = false;

        mp.players.local.stopAnimTask('missfinale_c2ig_11', 'pushcar_offcliff_m', 2.0);

        mp.players.local.detach(true, true);

        return;
    }
    if(pushVeh != null){
       
        var vehModel = pushVeh.getModel();

        var minMax = mp.game.gameplay.getModelDimensions(vehModel);

        mp.players.local.attachTo(pushVeh.handle, mp.players.local.getBoneIndex(6286), 0.0, minMax.min.y + 1.22, minMax.min.z + 0.6, 0.0, 0.0, 0.0, false, false, true, false, 0, true);

        mp.players.local.taskPlayAnim('missfinale_c2ig_11', 'pushcar_offcliff_m', 2.0, -8.0, -1, 35, 0.0, false, false, false);

        go = true;

    }

}
catch(e){
    mp.gui.chat.push(`${e.toString()}`);
}

});*/

