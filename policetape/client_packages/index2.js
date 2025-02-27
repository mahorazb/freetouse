//
mp.game.gameplay.disableAutomaticRespawn(true);
mp.game.gameplay.ignoreNextRestart(true);
mp.game.gameplay.setFadeInAfterDeathArrest(false);
mp.game.gameplay.setFadeOutAfterDeath(false);
mp.game.gameplay.setFadeInAfterLoad(false);
mp.game.vehicle.defaultEngineBehaviour = false;

mp.players.local.setConfigFlag(241, true); 
mp.players.local.setConfigFlag(429, true); 
mp.players.local.setConfigFlag(35, false); 

require('/guns.js');

mp.game.audio.startAudioScene("CHARACTER_CHANGE_IN_SKY_SCENE");


for(let i = 0; i <= 16; i++){
    if(i != 6) mp.game.graphics.setLightsState(i, true);
}

mp.game.invoke('0x1268615ACE24D504', true);
mp.game.invoke('0xE2B187C0939B3D32', false);

mp.game.invoke('0x1A9205C1B9EE827F', mp.players.local.handle, true, true);


mp.events.add("playerEnterVehicle", (vehicle, seat)=>{
    vehicle.setLights(1);

    mp.game.invoke('0x3B988190C0AA6C0B', vehicle.handle, false);
    mp.game.invoke('0x92B35082E0B42F66', vehicle.handle, true);
    mp.game.invoke('0x6D645D59FB5F5AD3', vehicle.handle);
    mp.game.invoke('0xBC2042F090AF6AD3', vehicle.handle, false);
  //  mp.game.invoke('0xAA6A6098851C396F', true);
  

    vehicle.setMaxSpeed(120 / 3.6); 
});

mp.keys.bind(0x42, true, ()=> {
    if(mp.players.local.vehicle != null && mp.players.local.vehicle.getPedInSeat(-1) == mp.players.local.handle){
        mp.events.callRemote("engineStatus");
    }
});

mp.events.add('entityStreamIn', (entity) => {
    if(entity.type == 'vehicle'){
      

        var veh = entity;
        veh.setDirtLevel(15.0);

        if(veh.getVariable('V_DOORS')){
            var doors = JSON.parse(veh.getVariable('V_DOORS'));

            for(let i = 0; i < 4; i++){
                if(doors[i] == 0)
                    veh.setDoorBroken(i, false);
            }
        }

        if(veh.getVariable('V_WHEELS')){
            var wheels = JSON.parse(veh.getVariable('V_WHEELS'));

            for(let i = 0; i < 6; i++){
                if(i > 1 && i <= 3) continue;

                if(wheels[i] == 0){
                    veh.setTyreBurst(i, false, 1000);
                    mp.game.invoke('0x74C68EF97645E79D', veh.handle, i, 0);
                }
                else {
                    veh.setTyreFixed(i);
                }
            }  
        }

        if(veh.hasVariable('V_ADMIN'))
            veh.setInvincible(true);
        else
            veh.setInvincible(false);

    }
});



mp.events.add('render', () => {
    mp.game.ui.displayCash(false);
    mp.game.ui.displayAreaName(false);
    mp.game.ui.setMinimapVisible(false);
    mp.game.controls.disableControlAction(24, 38, true);
   
    mp.vehicles.forEachInStreamRange(
		(veh) => {
            var pPosition = mp.players.local.position;
            var vPosition = veh.position;
            var distance = mp.game.gameplay.getDistanceBetweenCoords(pPosition.x, pPosition.y, pPosition.z, vPosition.x, vPosition.y, vPosition.z, true);
            var engHp = veh.getEngineHealth();
            var bdHp = veh.getBodyHealth();
           
            var model = mp.game.vehicle.getDisplayNameFromVehicleModel(veh.model);

            var str = `${model} ${veh.id}\n E: ${engHp.toFixed(2)} B: ${bdHp.toFixed(2)}`;

            if(veh.hasVariable('V_DOORS'))
            {
                var doors = veh.getVariable('V_DOORS');
                str = str.concat(`\nD: ${doors}`);
            }

            if(veh.hasVariable('V_WHEELS'))
            {
                var wheels = veh.getVariable('V_WHEELS');
                str = str.concat(`\nW: ${wheels}`);
            }

            if(distance > 5){
                str = str.concat(`\nDIST: ${distance.toFixed(2)}m`)
            }

            var speed = veh.getSpeed();

            if(speed > 1){
                speed *= 3.6;
                str = str.concat(`\nSPD: ${speed.toFixed(2)}km/h`);
            }

            mp.game.graphics.drawLine(pPosition.x, pPosition.y, pPosition.z, vPosition.x, vPosition.y, vPosition.z, 255, 255, 255, 180);
           
            mp.game.graphics.drawText(str, [veh.position.x, veh.position.y, veh.position.z], {
                    font: 4,
                    color: [255, 255, 255, 185],
                    scale: [0.3, 0.3],
                    outline: true
            });
            
		}
	);
});