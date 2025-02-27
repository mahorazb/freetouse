let checking = false;
var squidInterval = null;
let timerShow = false;
let time = 136;

mp.peds.new(0x0B881AEE, new mp.Vector3(-341.7239, -3283.2432, 290.0102), -90, 10);
mp.peds.new(0x0B881AEE, new mp.Vector3(-341.9039, -3290.5742, 290.0102), -90, 10);

mp.events.add('playerFailed', function(player){
    mp.game.gameplay.shootSingleBulletBetweenCoords(-348.78363, -3260.4478, 300.01044, player.position.x, player.position.y, player.position.z, 0, false, mp.game.joaat('WEAPON_SNIPERRIFLE'), player.handle, true, false, 5000);
    mp.game.gameplay.shootSingleBulletBetweenCoords(-348.78363, -3260.4478, 300.01044, player.position.x, player.position.y, player.position.z, 0, false, mp.game.joaat('WEAPON_SNIPERRIFLE'), player.handle, true, false, 7000);
    mp.game.gameplay.shootSingleBulletBetweenCoords(-348.78363, -3260.4478, 300.01044, player.position.x, player.position.y, player.position.z, 0, false, mp.game.joaat('WEAPON_SNIPERRIFLE'), player.handle, true, false, 9000);
});

mp.events.add('startTimer', function(){
    timerShow = true;
    time = 136;
    squidInterval = setInterval(() => {
        time--;

        if(time == 0){
            clearInterval(squidInterval);
            timerShow = false;
        }
    }, 1000);

    mp.players.local.setHeading(91.610634);

});

mp.events.add('checkPlayerSpeed', function(){

    if(mp.players.local.getSpeed() > 0.1){
        mp.events.callRemote('failSquidGame');
        
    }
    else {
        checking = true;
    }
});

mp.events.add('continueSquidGame', function(){
    checking = false;
    mp.players.local.freezePosition(false);
    global.menu.execute(`client_playMusic('./UI/auto/sq.mp3', 0.4);`);
});

mp.events.add('render', () => {
    if(mp.players.local.getVariable("IN_SQUID")){

        mp.game.controls.disableControlAction(2, 37, true);
        mp.game.controls.disableControlAction(0, 45, true);
        mp.game.controls.disableControlAction(0, 24, true);
        mp.game.controls.disableControlAction(0, 263, true);
        mp.game.controls.disableControlAction(0, 140, true);
        mp.game.controls.disableControlAction(0, 142, true);

        if(checking){
            if(mp.players.local.getSpeed() > 0.2){
                mp.events.callRemote('failSquidGame');
                checking = false;
            }
        }
        if(timerShow){

            var minutes = Math.floor(time / 60);
            var seconds = time - (minutes * 60);

            if(seconds >= 10){
            mp.game.graphics.drawText(`0${minutes}:${seconds}`, [0.5, 0.01], { 
                font: 7, 
                color: [255, 255, 255, 255], 
                scale: [1.2, 1.2], 
                outline: true
              });
            }
            else {
                mp.game.graphics.drawText(`0${minutes}:0${seconds}`, [0.5, 0.01], { 
                    font: 7, 
                    color: [255, 255, 255, 255], 
                    scale: [1.2, 1.2], 
                    outline: true
                  });
            }
        }
    }
});