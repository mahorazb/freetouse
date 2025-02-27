const tables = [
    new mp.Vector3(200.33763, -938.28668, 29.68678),
];

const tableHash = mp.game.joaat("prop_pooltable_02");
const ballHashes = [
    mp.game.joaat("prop_poolball_cue"),
];

const poolCue = mp.game.joaat("prop_pool_cue");

let poolTables = [];
let poolTablesPos = [];
let tableNow = -1;

let cue = null;

let poolTable = null;
let tablePos = new mp.Vector3();

let whiteBallHandle = null;
let whiteBallPos = null;

let ballVelocities = [];
let ballHandles = [];
let ballCoords = [];

let camHandle = null;

let lastPos = null;

let leftBorder = 0;
let rightBorder = 0;
let topBorder = 0;
let bottomBorder = 0;

let allBalls = 0;

let started = false;
let hit = false;
let shotOffset = [
    new mp.Vector3(0.75,1.48,0),
    new mp.Vector3(-0.76,1.48,0),
    new mp.Vector3(0.75,0.12,0),
    new mp.Vector3(-0.76,0.12,0),
    new mp.Vector3(0.75,-1.2,0),
    new mp.Vector3(-0.76,-1.2,0)
];

let ballOffset = [
    new mp.Vector3(0, 0.551758, 0.0),
    new mp.Vector3(0.0, 0.0, 0.0),
    new mp.Vector3(0.04,0.08,0),
    new mp.Vector3(-0.04,0.08,0),
    new mp.Vector3(-0.04,0.08,0).multiply(2),
    new mp.Vector3(0,0.08,0).multiply(2),
    new mp.Vector3(0.04,0.08,0).multiply(2),
    new mp.Vector3(0.04,0.08,0).multiply(3),
    new mp.Vector3(0.04,0.24,0),
    new mp.Vector3(-0.04,0.24,0),
    new mp.Vector3(-0.04,0.08,0).multiply(3),
    new mp.Vector3(0.04,0.08,0).multiply(4),
    new mp.Vector3(0.08,0.32,0),
    new mp.Vector3(0,0.32,0),
    new mp.Vector3(-0.08,0.32,0),
    new mp.Vector3(-0.04,0.08,0).multiply(4)
];

let shotDist = [
    0.03,
    0.03,
    0.05,
    0.05,
    0.03,
    0.03,
];

let balls = [1,2,3,4,5,6,7,8,9,10,11,12,13,14,15];

const cameraRotator = require("./pool/vie");


setTimeout(() => {
    for(let i = 0; i < tables.length; i++){
        createTable(tables[i].x, tables[i].y, tables[i].z);
    }
    
}, 5000);

async function createTable(x, y, z){

    poolTablesPos.push(new mp.Vector3(x, y, z));

    poolTables.push(mp.objects.new(tableHash, new mp.Vector3(x, y, z),
        {
            rotation: new mp.Vector3(0, 0, 0),
            alpha: 255,
            dimension: 0
        })
    );


}

 function createBall(hash, pos){
    var ball = mp.objects.new(hash, pos.add(new mp.Vector3(-0.07, 0, 0.93)),
    {
        rotation: new mp.Vector3(0, 0, 0),
        alpha: 255,
        dimension: 0
    });

    return ball;
}

function getBallHash(index){
    return mp.game.joaat(`prop_poolball_${index}`);
}

async function spawnBalls(){
    let ret = createBall(ballHashes[0], tablePos.subtract(new mp.Vector3(0, 0.551758, 0.0)));

    whiteBallHandle = ret;

    whiteBallPos = tablePos.subtract(new mp.Vector3(0.0, 0.551758, 0.0));

    for(let i = 0; i < 16; i++){
        ballVelocities.push(new mp.Vector3(0, 0, 0));
        ballHandles[i] = null;
    }

    ballHandles[0] = ret;

    for(let i = 1; i < 16; i++)
    {
        let triStart = tablePos.add(ballOffset[i]);
        ballHandles[i] = createBall(getBallHash(balls[i - 1]), triStart);
    }
}

let collisionDecrease = 0.03;

mp.events.add('pushedBall', function(camX, camY, power){
    pushBall(power, camX, camY);
});

async function pushBall(powers, camX = 0, camY = 0)
{
    let camCoords = new mp.Vector3();

    if(hit)
    {   
        camCoords = camHandle.getCoord();

        mp.events.callRemote("pushPoolBall", tableNow, parseFloat(camCoords.x), parseFloat(camCoords.y), parseFloat(powers));
    }
    else {
        camCoords = new mp.Vector3(camX, camY, 0.0);
    }

    let camDifference = new mp.Vector3(camCoords.x, camCoords.y, 0).subtract(whiteBallPos);
    let powerLevel = powers;

    let previousTimer = mp.game.invoke("0x9CD27B0045628463");
    let frameCounter = 0;

    ballVelocities[0] = new mp.Vector3(camDifference.x, camDifference.y, 0).unit().multiply(powerLevel).negative();

    let isDone = false;

    for(let i =0; i < 16; i++){
        if(ballHandles[i] == null) continue;
        ballCoords.push(ballHandles[i].position);
    }

    while (!isDone) {
        
        let currentTimer = mp.game.invoke("0x9CD27B0045628463");
        let delta = currentTimer - previousTimer + frameCounter;

        frameCounter = 0;

        let deltaTimeInS = (20/1000);

        while (delta >= 20) {
        
            let counter = 0;

            handleCollisions(deltaTimeInS)
        
            for(let i =0; i < 16; i++){
                if(ballHandles[i] == null) continue;

                ballVelocities[i] = ballVelocities[i].multiply(1 - collisionDecrease);

                if(Math.abs(ballVelocities[i].x) < 0.006 && Math.abs(ballVelocities[i].y) < 0.006){
                    ballVelocities[i] = new mp.Vector3(0, 0, 0);
                    counter += 1;
                }

                let entityCoord = ballCoords[i];

                let ballVelocityData = entityCoord.add(ballVelocities[i].multiply(deltaTimeInS));

                ballCoords[i] = entityCoord.add(ballVelocities[i].multiply(deltaTimeInS))

                checkShot();

                checkWalls(i, ballVelocityData);

                ballCoords[i] = entityCoord.add(ballVelocities[i].multiply(deltaTimeInS))

                if(ballHandles[i] == null) continue;

                ballHandles[i].setCoords(ballCoords[i].x, ballCoords[i].y, ballCoords[i].z, true, true, true, false);
                ballHandles[i].setVelocity(ballVelocities[i].x, ballVelocities[i].y, ballVelocities[i].z);

            }

            previousTimer = currentTimer;
            if(counter >= allBalls){
                isDone = true;
            }

            delta = delta - 20;
        }

        frameCounter = frameCounter + delta;

        await mp.game.waitAsync(0);
    }
  

    if(hit) camDestroy();

}

function checkShot(){
    if(!hit) return;

    let tableShotCords = [];

    for(let i = 0; i < 6; i++){
        tableShotCords.push(tablePos.add(shotOffset[i]));
    }

    for(let i = 1; i < 16; i++)
    {
        if(ballHandles[i] == null) continue;
        for(let j = 0; j < 6; j++){
            if(mp.game.gameplay.getDistanceBetweenCoords(ballCoords[i].x, ballCoords[i].y, ballCoords[i].z, tableShotCords[j].x,tableShotCords[j].y, tableShotCords[j].z, false) < shotDist[j]){
                mp.events.callRemote("ballInAngle", tableNow, i - 1);
                ballHandles[i].destroy();
                ballHandles[i] = null;
                allBalls--;
            }
        }
    }
}

function checkWalls(index, newPos) {
    if((newPos.x < leftBorder) || (newPos.x > rightBorder)){
        ballVelocities[index] = new mp.Vector3(-ballVelocities[index].x, ballVelocities[index].y, ballVelocities[index].z).multiply(0.97);
    }
    if((newPos.y < topBorder) || (newPos.y > bottomBorder)){
        ballVelocities[index] = new mp.Vector3(ballVelocities[index].x, -ballVelocities[index].y, ballVelocities[index].z).multiply(0.97);
    }
}

function handleCollisions(delta){
    for(let i = 0; i < 16; i++){
        if(ballHandles[i] == null) continue;
        let firstball = ballHandles[i];

        for(let j = i + 1; j < 16; j++){
            if(ballHandles[j] == null) continue;
            let firstBallCoords = ballCoords[i];
            let secondBall = ballHandles[j];
            let secondBallCoords = ballCoords[j];

            let newOne = firstBallCoords.add(ballVelocities[i].multiply(delta));
            let newTwo = secondBallCoords.add(ballVelocities[j].multiply(delta));

            let n = newOne.subtract(newTwo);

            let distance = n.length();

            if(distance <= 0.085)
            {
                let power = (Math.abs(ballVelocities[i].x) + Math.abs(ballVelocities[i].y)) + (Math.abs(ballVelocities[j].x) + Math.abs(ballVelocities[j].y))
                power = power * 0.00682;

                let opposite = firstBallCoords.y - secondBallCoords.y;
                let adjacent = firstBallCoords.x - secondBallCoords.x;

                let rotation = Math.atan2(opposite, adjacent);

                let velocity2 = new mp.Vector3(90 * Math.cos(rotation + Math.PI) * power, 90 * Math.sin(rotation + Math.PI)*power, 0);

                ballVelocities[j] = ballVelocities[j].add(velocity2).multiply(1-collisionDecrease);

                let velocity1 = new mp.Vector3(90 * Math.cos(rotation) * power, 90 * Math.sin(rotation)*power, 0);

                ballVelocities[i] = ballVelocities[i].add(velocity1).multiply(1-collisionDecrease);
            }
            
        }
    }
}

function attachCam()
{
    powerLevel = 0;
    //mp.gui.cursor.visible = true;
    let temp = ballHandles[0].getCoords(false);

    whiteBallPos = new mp.Vector3(temp.x, temp.y, temp.z - 1);

    let pos = whiteBallPos.add(new mp.Vector3(0, -2, 1.1));
    if(camHandle == null)
        camHandle = mp.cameras.new("DEFAULT_SCRIPTED_CAMERA", new mp.Vector3(pos.x, pos.y, whiteBallPos.z + 1.2), new mp.Vector3(0,0,0), 40);
    camHandle.setRot(360,0,0, 2);
    camHandle.pointAtCoord(whiteBallPos.x, whiteBallPos.y, whiteBallPos.z );
    camHandle.setActive(true);

    cameraRotator.start(camHandle, new mp.Vector3(whiteBallPos.x, whiteBallPos.y, whiteBallPos.z +1 ), new mp.Vector3(whiteBallPos.x, whiteBallPos.y, whiteBallPos.z + 1), new mp.Vector3(-1, -1, 2), 60);
    cameraRotator.setZBound(-1.8, 0);

    cameraRotator.setZUpMultipler(5);
    cameraRotator.pause(false);
    
    mp.game.cam.renderScriptCams(true, false, 3000, true, false);

    hit = false;
}

function camDestroy(){
    if(camHandle != null){
        cameraRotator.stop();
        camHandle.destroy();
        camHandle = null;
        mp.game.cam.renderScriptCams(false, false, 500, true, false);
       // camHandle.destroy();

        hit = false;

        started = false;

        mp.players.local.setAlpha(255);
        mp.players.local.freezePosition(false);

        cue.destroy();
        cue = null;

        poolTables[tableNow].setCollision(true, true);


    }
}

function quitGame(){
    if(camHandle != null){
        camDestroy();
    }

    if(tableNow != -1){
        for(let i = 0; i < 16; i++){
            if(ballHandles[i] != null){
                ballHandles[i].destroy();
                ballHandles[i] = null;
            }
        }

        tableNow = -1;

        allBalls = 0;
    }
}

function resetGame(ballls){
    if(tableNow != -1){
        for(let i = 0; i < 16; i++){
            if(ballHandles[i] != null){
                ballHandles[i].destroy();
                ballHandles[i] = null;
            }
        }

        balls = ballls;

        spawnBalls();

        allBalls = 16;
    }
}

function addCue(){
    let camCoords = camHandle.getCoord();

  
    cue = mp.objects.new(poolCue, new mp.Vector3(camCoords.x, camCoords.y, tablePos.z + 0.95),
    {
        rotation: new mp.Vector3(0, 90, cameraRotator.heading),
        alpha: 255,
        dimension: 0
    });

    cue.setCollision(false, false);
    poolTables[tableNow].setCollision(false, false);
}


function changeCue(){
    if(startHit) return;
    let camCoords = camHandle.getCoord();

    cue.setCoords(camCoords.x, camCoords.y, tablePos.z + 1, true, true, true, false);

    cue.setRotation(0, 90, 45 + cameraRotator.heading, 2, true);
}

mp.events.add('startPoolGame', function(table, ballls) {
    tableNow = table;
    tablePos = poolTablesPos[table];

    leftBorder = tablePos.x-0.82

    rightBorder = tablePos.x+0.68

    topBorder =  tablePos.y-1.22

    bottomBorder = tablePos.y+1.47

    balls = ballls;

    spawnBalls();

    allBalls = 16;
});

mp.events.add('quitPoolGame', function(){
    quitGame();
});

mp.events.add('startHitting', function(){
    mp.players.local.setAlpha(0);
    mp.players.local.freezePosition(true);

    setTimeout(() => {
        attachCam();
        addCue();
        started = true;
    }, 1000);
});

mp.events.add('stopHitting', function(){
    mp.players.local.setAlpha(255);
    mp.players.local.freezePosition(false);

    camDestroy();
});

mp.events.add('shotBall', function(id){
    if(ballHandles[id] != null){
        ballHandles[id].destroy();
        ballHandles[id] = null;
        allBalls--;
    }
});



mp.events.add('resetPoolGame', function(){
    resetGame();
});



let startHit = false;
let powerLevel = 0;

mp.events.add("render", function (){
   
    if(started){
        
        if(mp.game.controls.isControlPressed(0, 24) && !hit){
            cameraRotator.pause(true);
            startHit = true;

            if(powerLevel < 8){
                powerLevel = powerLevel + (0.5*(100/1000));

                let cuePos = cue.getCoords(false);

                let offCue = mp.game.object.getObjectOffsetFromCoords(cuePos.x, cuePos.y, cuePos.z, 45 + cameraRotator.heading, - (powerLevel / 1000), 0.0, 0.0);

                cue.setCoords(offCue.x, offCue.y, offCue.z, true, true, true, false);
            }
            
 
         }
         else if(startHit){
            startHit = false;
            hit = true;
          
		    pushBall(powerLevel);
         }

        changeCue();
    }

});