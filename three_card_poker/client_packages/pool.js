let balls = {};

let triStart;
let ballSize;

let ballVelocities = {};
let ballCoords = {};

let leftBorder = 0;
let rightBorder = 0;
let topBorder = 0;
let bottomBorder = 0;

let collisionDecrease = 0.03;
let camHandle = null;
let powerLevel = 0;
let tableHash = "prop_pooltable_02";
let ballHashes = "prop_poolball_1";

let playerCoords;
let playerPed = mp.game.player.getPed();

let tableLoc;

let start = false;
let angleY = 0.0;
let angleX = 0.0;
let angleZ = 0.0;

let pushed = false;

var poolHandle = null;
var whiteBallHandle = null;

function size_dict(d){c=0; for (i in d) ++c; return c}

mp.events.add('pSpawnTable', function () {
    SpawnTable();
});

mp.events.add('pSpawnBalls', function () {
    SpawnBalls();
    mp.objects.newWeak(handle);
});

mp.events.add('pAttachCam', function () {
    AttachCam();
});


mp.events.add('pPushBall', function () {
    PushBall();
});

function AttachCam(){
    try {

    let pos = whiteBallHandle.getCoords(false);
    camHandle = mp.cameras.new('default', new mp.Vector3(pos.x, pos.y, pos.z).add(new mp.Vector3(0, -1, 0.7)), new mp.Vector3(-35,0,0), 40);

    camHandle.setActive(true);
    mp.game.cam.renderScriptCams(true, false, 0, true, false);

    start = true;
    }
    catch(ex){
        mp.gui.chat.push(`${ex}`);
    }
}

function SpawnTable() {

    tableLoc = mp.players.local.position;

    let poolHandle = SpawnModel(mp.game.joaat(tableHash), null);

    let min, max = mp.game.gameplay.getModelDimensions(mp.game.joaat(tableHash));

    let tableSize = max - min;

    leftBorder = tableLoc.x - 0.82;
    rightBorder = tableLoc.x + 0.68;
    topBorder = tableLoc.y - 1.22;
    bottomBorder = tableLoc.y + 1.47;
}

function SpawnBall(hashKey, coords){
    return SpawnModel(mp.game.joaat(hashKey), coords);
}

function SpawnBalls(){
    try {
    let ballMin, ballMax = mp.game.gameplay.getModelDimensions(mp.game.joaat(ballHashes));

    ballSize = ballMax - ballMin;

    
    whiteBallHandle =  SpawnBall(ballHashes, tableLoc.subtract(new mp.Vector3(0.07446289, 0.551758, -0.9064026)));

    let ret = whiteBallHandle;

    for(let i = 0; i < 16; i++){
        ballVelocities[i] = new mp.Vector3(0, 0, 0);
    }

    balls[0] = ret;

    triStart = tableLoc.subtract(new mp.Vector3(0.07446289, -0.41833594, -0.9064026));

    ret = SpawnBall(ballHashes, triStart);
    balls[1] = ret;

    ret = SpawnBall(ballHashes, triStart.add(new mp.Vector3(0.04,0.08,0)));
    balls[2] = ret;

    ret = SpawnBall(ballHashes, triStart.add(new mp.Vector3(-0.04,0.08,0)));
    balls[3] = ret;

    ret = SpawnBall(ballHashes, triStart.add(new mp.Vector3(2*-0.04,2*0.08,0)));
    balls[4] = ret;

    ret = SpawnBall(ballHashes, triStart.add(new mp.Vector3(0,2*0.08,0)));
    balls[5] = ret;

    ret = SpawnBall(ballHashes, triStart.add(new mp.Vector3(2*0.04,2*0.08,0)));
    balls[6] = ret;

    ret = SpawnBall(ballHashes, triStart.add(new mp.Vector3(3*0.04,3*0.08,0)));
    balls[7] = ret;

    ret = SpawnBall(ballHashes, triStart.add( new mp.Vector3(0.04,0.24,0)));
    balls[8] = ret;

    ret = SpawnBall(ballHashes, triStart.add(new mp.Vector3(-0.04,0.24,0)));
    balls[9] = ret;

    ret = SpawnBall(ballHashes, triStart.add( new mp.Vector3(3*-0.04,3*0.08,0)));
    balls[10] = ret;

    ret = SpawnBall(ballHashes, triStart.add(new mp.Vector3(4*0.04,4*0.08,0)));
    balls[11] = ret;

    ret = SpawnBall(ballHashes, triStart.add( new mp.Vector3(0.08,0.32,0)));
    balls[12] = ret;

    ret = SpawnBall(ballHashes, triStart.add(  new mp.Vector3(0, 0.32,0)));
    balls[13] = ret;

    ret = SpawnBall(ballHashes, triStart.add(  new mp.Vector3(-0.08,0.32,0)));
    balls[14] = ret;

    ret = SpawnBall(ballHashes, triStart.add( new mp.Vector3(4*-0.04,4*0.08,0)));
    balls[15] = ret;

    for(let i = 0; i < size_dict(balls); i++){
        let firstBall = balls[i];

        for(let j = i + 1; j < size_dict(balls); j++){
            let secondBall = balls[j];

            firstBall.setNoCollision(secondBall, false);
        }
    }
}
catch(ex){
    mp.gui.chat.push(`${ex}`);
}
}

function SpawnModel(hashName, pos = null) {
    var handle;

    let cord = mp.players.local.position;

    if(pos == null){
        handle = mp.objects.new(hashName, new mp.Vector3(cord.x, cord.y, cord.z));
    }
    else {
        handle = mp.objects.new(hashName, new mp.Vector3(pos.x, pos.y, pos.z));
    }

    mp.gui.chat.push(`${handle}`)

    return handle;
}

function PushBall() {
    try {
    pushed = true;
    var camCoords = camHandle.getCoord();
    var camDifference = new mp.Vector3(camCoords.x, camCoords.y, 0) - whiteBallHandle.getCoords(false);

    ballVelocities[0] = (-norm(new mp.Vector3(camDifference.x, camDifference.y, 0))) * (8*powerLevel);

    let isDone = false;
    let previousTimer = mp.game.invoke("0x9CD27B0045628463");
    let frameCounter = 0;

    for(let i = 0; i < size_dict(balls); i++){
        ballCoords[i] = balls[i].getCoords(false);
    }

   // while (!isDone){
        var currentTimer = mp.game.invoke("0x9CD27B0045628463");
        let delta = currentTimer - previousTimer + frameCounter;

        frameCounter = 0;

        let deltaTimeIns = (20 / 1000)

        //while(delta >= 20){
            let counter = 0;

            HandleCollisions(deltaTimeIns);

            for(let i = 0; i < size_dict(balls); i++){
                ballVelocities[i] = ballVelocities[i] * (1 - collisionDecrease);

                if(Math.abs(ballVelocities[i].x) < 0.006 && Math.abs(ballVelocities[i].y) < 0.006){
                    ballVelocities[i] = new mp.Vector3(0, 0, 0)

                    counter = counter + 1;
                }

                let entityCoord = ballCoords[i];
                let ballVelocitiesDelta = entityCoord + (ballVelocities[i] * deltaTimeIns);

                CheckWalls(i, ballVelocitiesDelta);


                ballCoords[i] = entityCoord + (ballVelocities[i] * deltaTimeIns);

                balls[i].setCoords(ballCoords[i].x, ballCoords[i].y, ballCoords[i].z, true, false, false, false);
                balls[i].setVelocity(ballVelocities[i].x, ballVelocities[i].y, ballVelocities[i].z);
            }

            previousTimer = currentTimer;

            //if(counter == size_dict(balls))
            isDone = true;

            delta = delta - 20;
       // }

        frameCounter = frameCounter + delta;
    //}

    pushed = false;
        }
    catch(ex){
        mp.gui.chat.push(`${ex}`);
    }
}

function CheckWalls(index, newPos){
    if((newPos.x < leftBorder) || (newPos.x > rightBorder)){
        ballVelocities[index] = new mp.Vector3(-ballVelocities[index].x, ballVelocities[index].y, ballVelocities[index].z) * 0.97;
    }
    if((newPos.y < topBorder) || (newPos.y > bottomBorder)){
        ballVelocities[index] = new mp.Vector3(ballVelocities[index].x, -ballVelocities[index].y, ballVelocities[index].z) * 0.97;
    }
}



function HandleCollisions(delta){
    for(let i = 0; i < size_dict(balls); i++){
        let firstBall = balls[i];

        for(let j = i + 1; i < size_dict(balls); j++){
            let firstBallCoords = ballCoords[i];
            let secondBall = balls[j];
            let secondBallCoords = ballCoords[j];
            let newOne = firstBallCoords + ballVelocities[i] * delta;
            let newTwo = secondBallCoords + ballVelocities[j] * delta;

            let n = newOne - newTwo;

            let distance = n;

            if(distance <= 0.085){
                let power = (Math.abs(ballVelocities[i].x) + abs(ballVelocities[i].y)) +  (Math.abs(ballVelocities[j].x) + abs(ballVelocities[j].y));

                power = power * 0.00682;

                let opposite = firstBallCoords.y - secondBallCoords.y;
                let adjacent = firstBallCoords.x - secondBallCoords.x;

                let rotation = Math.atan2(opposite, adjacent);
                let velocity2 = new mp.Vector3(90 * Math.cos(rotation + Math.PI) * power, 90 * Math.sin(rotation = Math.PI) * power, 0);

                ballVelocities[j] = (ballVelocities[j] + velocity2) * (1 - collisionDecrease);

                let velocity1 = new mp.Vector3(90 * Math.cos(rotation) * power, 90 * Math.sin(rotation) * power, 0);
                ballVelocities[i] = (ballVelocities[i] + velocity1) * (1 - collisionDecrease);
            }
        }
    }
}

function ProcessCamControls(){
    var newPos = ProcessNewPosition();

    mp.game.streaming.setFocusArea(newPos.x, newPos.y, newPos.z, 0.0, 0.0, 0.0);

    camHandle.setCoord(newPos.x, newPos.y, newPos.z);

    var ballCoords = whiteBallHandle.getCoords(false);

    camHandle.pointAtCoord(ballCoords.x, ballCoords.y, ballCoords.z);
}


function ProcessNewPosition() {
    let mouseX = 0.0;
    let mouseY = 0.0;

    if(mp.game.controls.isInputDisabled(0)){
        mouseX = mp.game.controls.getDisabledControlNormal(1, 1) * 8.0;
        mouseY = mp.game.controls.getDisabledControlNormal(1, 2) * 8.0;
    }
    else {
        mouseX = mp.game.controls.getDisabledControlNormal(1, 1) * 1.5;
        mouseY = mp.game.controls.getDisabledControlNormal(1, 2) * 1.5;
    }

    angleZ = angleZ - mouseX;
    angleY = angleY - mouseY;

    if(angleY > 89.0)
        angleY = 89.0
    else if(angleY < -89.0)
        angleY = -89.0

    let pCoords = whiteBallHandle.getCoords(false);

    let behindCam = {
        x : pCoords.x + ((Math.cos(angleZ * (Math.PI / 180)) * Math.cos(angleY * (Math.PI / 180))) + (Math.cos(angleY * (Math.PI / 180)) * Math.cos(angleZ * (Math.PI / 180)))) / 2 * (1.5 + 0.5),
        y : pCoords.y + ((Math.sin(angleZ * (Math.PI / 180)) * Math.cos(angleY * (Math.PI / 180))) + (Math.cos(angleY * (Math.PI / 180)) * Math.sin(angleZ * (Math.PI / 180)))) / 2 * (1.5 + 0.5),
        z : pCoords.z + ((Math.sin(angleY * (Math.PI / 180)))) * (1.5 + 0.5)
    }

    let maxRadius = 1.5;

    let offset = {
        x : ((Math.cos(angleZ * (Math.PI / 180)) * Math.cos(angleY * (Math.PI / 180))) + (Math.cos(angleY * (Math.PI / 180)) * Math.cos(angleZ * (Math.PI / 180)))) / 2 * maxRadius,
        y : ((Math.sin(angleZ * (Math.PI / 180)) * Math.cos(angleY * (Math.PI / 180))) + (Math.cos(angleY * (Math.PI / 180)) * Math.sin(angleZ * (Math.PI / 180)))) / 2 * maxRadius,
        z : ((Math.sin(angleY * (Math.PI / 180)))) * maxRadius
    }

    let pos = {
        x : pCoords.x + offset.x,
        y : pCoords.y + offset.y,
        z : pCoords.z + offset.z
    }

    return pos;
}

var mainTimer = mp.game.invoke("0x9CD27B0045628463");

mp.events.add('render', function (){
    let temptimer = mp.game.invoke("0x9CD27B0045628463");
    let deltaTimer = temptimer - mainTimer;

    if(start){
        ProcessCamControls();
        mp.gui.chat.push('ok go');

        if(mp.game.controls.isControlPressed(0, 29)){
            powerLevel = powerLevel + (0.1 * (deltaTimer/1000));
        }
        else {
            if(mp.game.controls.isControlPressed(0, 26)){
                powerLevel = powerLevel - (0.1 * (deltaTimer/1000));
            }
        }

        if(mp.game.controls.isControlPressed(0, 23)){
            if(!pushed) PushBall();
        }
    }

});

function v_divide(vec) {
    let scalar = v_magnitude(vec);
    return new mp.Vector3(vec.x / scalar, vec.y / scalar, vec.z / scalar);
};

function v_magnitude (vec) {
    return Math.sqrt(vec.x * vec.x + vec.y * vec.y + vec.z + vec.z);
}

function norm (vec) {
    return v_divide(vec);
}