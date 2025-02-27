let golfhole = 0;
let golfstrokes = 0;
let totalgolfstrokes = 0;

let golfplaying = false;

let ballstate = 1;
let balllocation = 0;

let golfstate = 1;
let golfclub = 1;

let inLoop = false;
let inTask = false;
let power = 0.1;

let mygolfball = null;

let golfInterval = null;

let doingdrop = false;

mp.events.add('beginGolf', function(){
    startGolf();
});

mp.events.add('beginGolfHud', function(){
    startGolfHud();
});

mp.events.add('render', function(){
    if(golfplaying){
        mp.game.graphics.drawRect(0.5, 0.93, 0.2, 0.04, 0, 0, 0, 140);

        if(golfhole != 0){
            let pos = mygolfball.getCoords(false);

            let distance = mp.game.gameplay.getDistanceBetweenCoords(pos.x, pos.y, pos.z, holes[golfhole].x2, holes[golfhole].y2, holes[golfhole].z2, true);
            drawGolfTxt(0.9193, 1.391, 1.0,1.0,0.4, "~s~" + golfstrokes + "~r~ - ~s~" + totalgolfstrokes + "~r~ - ~s~" + clubname + "~r~ - ~s~" + distance + " m", 255, 255, 255, 255)
        }
    }
});

function startGolf(){
    mp.game.invoke("0xB4EDDC19532BFB85");

    inTask = false;

    golfplaying = true;

    startGolfHud();

    golfInterval = setInterval(() => {

        if(!golfplaying){
            clearInterval(golfInterval);
            return;
        }

        if(ballstate == 1){
            golfhole = golfhole + 1;

            if(golfhole == 10){
                endgame();
            }
            else {
                startHole();
            }
        }
        else {
            if(golfstate == 2 && !inTask && !doingdrop){
                idleShot();
            }
            else if(golfstate == 1 && !inTask && !doingdrop){
                MoveToBall();
            }
        }
    }, 1000);
}

function rotateShot(moveType){
    let curHeading = mygolfball.getHeading();

    if(curHeading > 360.0){
        curHeading = 0.0;
    }

    if(moveType){
        mygolfball.setHeading(curHeading - 0.7);
    }
    else {
        mygolfball.setHeading(curHeading + 0.7);
    }
}

function createBall(x, y, z){
    if(mygolfball != null){
        mygolfball.destroy();
    }

    mygolfball = mp.objects.new(mp.game.joaat(`prop_golf_ball`), new mp.Vector3(x, y, z));

    setTimeout(() => {
        mp.game.invoke('0x0A50A1EEDAD01E65', mygolfball.handle, true);
        addBallBlip();
        mygolfball.setCollision(true, true);

        mp.game.invoke('0x4A4722448F18EEF5', mygolfball.handle, true);

        mygolfball.freezePosition(true);

        mygolfball.setHeading(mp.players.local.heading);

    }, 100);  
}

function endgame(){
    if(mygolfball != null){
        mygolfball.destroy();
    }

    golfhole = 0;
    golfstrokes = 0;
    golfplaying = false;
    ballstate = 1;
    balllocation = 0;
    golfstate = 1;
    golfclub = 1;
    inLoop = false;
    inTask = false;
}

function MoveToBall(){
    
}

function DisplayHelpText(){

}

function startGolfHud(){

}


let holes = [
    {par: 5, x: -1371.3370361328, y: 173.09497070313, z: 57.013027191162, x2: -1114.2274169922, y2: 220.8424987793, z2: 63.8947830200},
    {par: 4, x: -1107.1888427734, y: 156.581298828, z: 62.03958129882, x2: -1322.0944824219, y2: 158.8779296875, z2: 56.80027008056},
    {par: 3, x: -1312.1020507813, y: 125.8329391479, z: 56.4341888427, x2: -1237.347412109, y2: 112.9838562011, z2: 56.20140075683},
    {par: 4, x: -1216.913208007, y: 106.9870910644, z: 57.03926086425, x2: -1096.6276855469, y2: 7.780227184295, z2: 49.73574447631},
    {par: 4, x: -1097.859619140, y: 66.41466522216, z: 52.92545700073, x2: -957.4982910156, y2: -90.37551879882, z2: 39.2753639221},
    {par: 3, x: -987.7417602539, y: -105.0764007568, z: 39.585887908936, x2: -1103.506958007, y2: -115.2364349365, z2: 40.55868911743},
    {par: 4, x: -1117.0194091797, y: -103.8586044311, z: 40.8405838012, x2: -1290.536499023, y2: 2.7952194213867, z2: 49.34057998657},
    {par: 5, x: -1272.251831054, y: 38.04283142089, z: 48.72544860839, x2: -1034.80187988, y2: -83.16706085205, z2: 43.0353431701},
    {par: 4, x: -1138.319580078, y: -0.1342505216598, z: 47.98218917846, x2: -1294.685913085, y2: 83.5762557983, z2: 53.92817306518},
];