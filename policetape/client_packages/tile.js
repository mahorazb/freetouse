
//require('./nightclub.js');
//require('./vehpush.js');
//require('./hunt.js');


/*var bp = mp.game.ui.addBlipForRadius(-1500, 0, 0, 10000);
mp.game.invoke("0xDF735600A4696DAF", bp, 364);
mp.game.invoke("0x45FF974EEE1C8734", bp, 80);
mp.game.invoke("0x03D7FB09E75D6B7E", bp, 1);*/



mp.game.ui.setMinimapComponent(15, true, -1);


let createdMarker = false;
let start = false;
var marker;
let go = false;



mp.events.add('render', function(){

    let id = mp.game.graphics.requestScaleformMovie("IAA_HEIST_BOARD");

    mp.game.invoke("0xB9449845F73F5E9C", "LOCK_MENU_ITEM");
    mp.game.invoke("0xC3D0841A0CC546A6", 0);
    mp.game.invoke("0xC58424BA936EB458", true);
    mp.game.invoke("0xC6796A8FFA375E53");

    if(createdMarker){
        //let pos3d = mp.game.graphics.screen2dToWorld3d(new mp.Vector3(900, 700, 0));

        const objpos = pointingAt(6);

        if(!objpos){
            const camera = mp.cameras.new("gameplay");

            let position = camera.getCoord(); // grab the position of the gameplay camera as Vector3

            let direction = camera.getDirection(); // get the forwarding vector of the direction you aim with the gameplay camera as Vector3

            let farAway = new mp.Vector3((direction.x * 5) + (position.x), (direction.y * 5) + (position.y), (direction.z * 5) + (position.z)); // calculate a random point, drawn on a invisible line between camera position and direction (* distance)

            marker.position = new mp.Vector3(farAway.x, farAway.y, started ? startPos.z : farAway.z);
            marker.color = [255, 255, 255, 255];

            if(started){
                
                let dist = mp.game.gameplay.getDistanceBetweenCoords(startPos.x, startPos.y, startPos.z, farAway.x, farAway.y, farAway.z, true);

                mp.game.graphics.drawLine(startPos.x, startPos.y, startPos.z, farAway.x, farAway.y, farAway.z, 255, dist < 12 ? 255 : 0, dist < 12 ? 255 : 0, 255);


                mp.game.graphics.drawText(`${dist.toFixed(2)} m`, [0.5, 0.005], { 
                    font: 7, 
                    color: [255, 255, 255, 185], 
                    scale: [1.2, 1.2], 
                    outline: true
                  });
            }

           
        }
        else {

            if(started){
                let dist = mp.game.gameplay.getDistanceBetweenCoords(startPos.x, startPos.y, startPos.z, objpos.position.x, objpos.position.y, objpos.position.z, true);

                mp.game.graphics.drawLine(startPos.x, startPos.y, startPos.z, objpos.position.x, objpos.position.y, objpos.position.z, 255, dist < 12 ? 255 : 0, dist < 12 ? 255 : 0, 255);

                mp.game.graphics.drawText(`${dist.toFixed(2)} m`, [0.5, 0.005], { 
                    font: 7, 
                    color: [255, 255, 255, 185], 
                    scale: [1.2, 1.2], 
                    outline: true
                  });
            }
            marker.position = new mp.Vector3(objpos.position.x, objpos.position.y, started ? startPos.z : objpos.position.z);
            marker.color = [0, 255, 0, 255];
        }
       
    }
    else if(!start && go){
        start = true;
        marker = mp.markers.new(0, new mp.Vector3(0, 0, 0), 0.1,
            {
                direction: new mp.Vector3(0, 0, 0),
                color: [255, 255, 255, 255],
                visible: true,
                dimension: 0
            });

        setTimeout(() => {
            createdMarker = true;
        }, 500);
    }
});

function pointingAt(distance) {
    const camera = mp.cameras.new("gameplay"); // gets the current gameplay camera

    let position = camera.getCoord(); // grab the position of the gameplay camera as Vector3

    let direction = camera.getDirection(); // get the forwarding vector of the direction you aim with the gameplay camera as Vector3

    let farAway = new mp.Vector3((direction.x * distance) + (position.x), (direction.y * distance) + (position.y), (direction.z * distance) + (position.z)); // calculate a random point, drawn on a invisible line between camera position and direction (* distance)

    let result = mp.raycasting.testPointToPoint(position, farAway, null, 19); // now test point to point - intersects with map and objects [1 + 16]

    return result; // and return the result ( undefined, if no hit )
}

let started = false;
let mark = null;
let startPos = new mp.Vector3(0, 0, 0);

mp.keys.bind(0x45, true, function() {
    if(!started){
        const objpos = pointingAt(6);

        mp.events.callRemote('startTape', objpos.position.x, objpos.position.y, objpos.position.z);

        mark = mp.markers.new(0, new mp.Vector3(objpos.position.x, objpos.position.y, objpos.position.z), 0.2,
        {
            direction: new mp.Vector3(0, 0, 0),
            color: [0, 255, 0, 255],
            visible: true,
            dimension: 0
        });

        startPos = new mp.Vector3(objpos.position.x, objpos.position.y, objpos.position.z);
        started = true;
    }
    else {
        const objpos = pointingAt(6);

        if(!objpos){
            const camera = mp.cameras.new("gameplay"); // gets the current gameplay camera

            let position = camera.getCoord(); // grab the position of the gameplay camera as Vector3
        
            let direction = camera.getDirection(); // get the forwarding vector of the direction you aim with the gameplay camera as Vector3
        
            let farAway = new mp.Vector3((direction.x * 5) + (position.x), (direction.y * 5) + (position.y), (direction.z * 5) + (position.z)); 

            mp.events.callRemote('stopTape', farAway.x, farAway.y, farAway.z);
        }
        else {
            mp.events.callRemote('stopTape', objpos.position.x, objpos.position.y, objpos.position.z);
        }
        started = false;
        mark.destroy();
    }
});




mp.keys.bind(0x31, true, function() {
    let obj = mp.objects.new(mp.game.joaat(`prop_police_tape_roll`), new mp.Vector3(mp.players.local.position.x, mp.players.local.position.y, mp.players.local.position.z),
    {
        rotation: new mp.Vector3(0, 0, 0),
        alpha: 255,
        dimension: 0,
    });

    setTimeout(() => {
        obj.attachTo(mp.players.local.handle, mp.players.local.getBoneIndex(28422), 0.05, 0.0, 0.0, 0.0, 0.0, 0.0, false, false, false, true, 2, true);
        go = true;
    }, 500);


});

