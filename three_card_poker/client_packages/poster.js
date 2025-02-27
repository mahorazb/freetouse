
let animDict = "anim@amb@nightclub@poster@";

mp.game.streaming.requestAnimDict(animDict);

mp.events.add('startPoster', function(){

    try {
    var ped = mp.game.player.getPed();


    var animPos = mp.game.ped.getAnimInitialOffsetPosition(animDict, "poster_placement", -1063.016, -2543.65, 20.62, 0.0, 0.0, 60.0, 0, 2);
    var targetHeading = 52.8159;

    mp.players.local.taskGoStraightToCoord(animPos.x, animPos.y, animPos.z, 0.025, 5000, targetHeading, 0.05);

    setTimeout(() => {


        var scene = mp.game.invoke("0x7CD6BC4C2BBDD526", -1063.016, -2543.65, 20.62, 0.0, 0.0, 60.0, 2, false, false, 1065353216, 0, 1.3);

        mp.game.invoke("0x742A637471BCECD9", ped, scene, animDict, "poster_placement", 1.5, -4.0, 1, 16, 1148846080, 0);

        var obj = mp.game.object.createObjectNoOffset(mp.game.joaat(`ba_prop_battle_poster_skin_01`), -1063.016, -2543.65, 20.62, true, true, false);
        mp.game.invoke("0xF2404D68CBC855FA", obj, scene, animDict, "poster_poster_placement", 4.0, -8.0, 1);
        mp.game.invoke("0x9A1B3FCDB36C8697", scene);

    }, 1000);

}
catch(ex){
    mp.gui.chat.push(`${ex}`);
}

});