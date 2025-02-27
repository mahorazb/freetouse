mp.game.streaming.requestIpl("ba_int_placement_ba_interior_1_dlc_int_02_ba_milo_");

mp.game.interior.enableInteriorProp(271617, "Int01_ba_clubname_09");
mp.game.interior.enableInteriorProp(271617, "Int01_ba_Style03");
mp.game.interior.enableInteriorProp(271617, "Int01_ba_style03_podium");
mp.game.interior.enableInteriorProp(271617, "Int01_ba_equipment_setup");
mp.game.interior.enableInteriorProp(271617, "Int01_ba_equipment_upgrade");
mp.game.interior.enableInteriorProp(271617, "Int01_ba_dj04");


var lamps = [];

var lampsPosotions = [
    new mp.Vector3(-1591.597, -3013.749, -77.3800),
    new mp.Vector3(-1591.597, -3012.749, -77.3800),
    new mp.Vector3(-1591.597, -3011.749, -77.3800),
    new mp.Vector3(-1591.597, -3010.702, -77.3800),
    new mp.Vector3(-1594.379, -3008.334, -77.3800),
    new mp.Vector3(-1596.179, -3008.334, -77.3800),
    new mp.Vector3(-1597.731, -3008.334, -77.3800),

    new mp.Vector3(-1602.168, -3010.862, -76.9113),
    new mp.Vector3(-1602.168, -3010.862, -75.9113),
    new mp.Vector3(-1602.168, -3010.862, -74.9113),
    new mp.Vector3(-1602.168, -3014.519, -76.9113),
    new mp.Vector3(-1602.168, -3014.519, -75.9113),
    new mp.Vector3(-1602.168, -3014.519, -74.9113),

    new mp.Vector3(-1606.630, -3010.431, -75.7108),
    new mp.Vector3(-1606.629, -3014.947, -75.7108),
    new mp.Vector3(-1606.630, -3010.431, -74.3108),
    new mp.Vector3(-1606.629, -3014.947, -74.3108),
    new mp.Vector3(-1602.368, -3018.949, -77.3800),
];

let lampRotations =
[
    new mp.Vector3(0, 0, 180),
    new mp.Vector3(0, 0, 180),
    new mp.Vector3(0, 0, 180),
    new mp.Vector3(0, 0, 180),
    new mp.Vector3(0, 0, -90),
    new mp.Vector3(0, 0, -90),
    new mp.Vector3(0, 0, -90),
    new mp.Vector3(0, 0, 0),
    new mp.Vector3(0, 0, 0),
    new mp.Vector3(0, 0, 0),
    new mp.Vector3(0, 0, 0),
    new mp.Vector3(0, 0, 0),
    new mp.Vector3(0, 0, 0),
    new mp.Vector3(0, 0, -30),
    new mp.Vector3(0, 0, 30),
    new mp.Vector3(0, 0, -30),
    new mp.Vector3(0, 0, 30),
    new mp.Vector3(0, 0, 40),
];

mp.events.add('nightclub:create_lamps', () => {
    let i = 0;
  lampsPosotions.forEach(
    (pos) => {
        lamps.push(
            mp.objects.new(mp.game.joaat('ba_prop_battle_lights_fx_rigb'), pos,
                {
                    rotation: lampRotations[i],
                    alpha: 255,
                    dimension: 0
                })
        );

        mp.labels.new(" sdasdasd", pos - new mp.Vector3(0.3, 0.3, 0.3),
            {
                los: false,
                font: 1,
                drawDistance: 150,
            });
        
        i++;
    }
  );

    setTimeout(() => {
        lamps.forEach((l) => {
           
                mp.game.invoke('0x5F048334B4A4E774', l.handle, true, 255, 0, 0);
           
        });

        //mp.game.invoke('0x5F048334B4A4E774', lamps[7].handle, true, 0, 255, 0);
    }, 1000);


});

let on = false;
var inter = null;
mp.keys.bind(0x44, true, function() {
    if(!on){
        let check = false;
        inter = setInterval(() => {
 
            lamps[7].setRotation(check ? 90 : -90, 0, 0, 2, true);
            mp.game.invoke('0x5F048334B4A4E774', lamps[7].handle, true, check ? 238 : 0,  check ? 130 : 255, 238);
            lamps[8].setRotation(check ? -90 : 90, 0, 0, 2, true);
            mp.game.invoke('0x5F048334B4A4E774', lamps[8].handle, true, check ? 238 : 0,  check ? 130 : 255, 238);
            lamps[9].setRotation(check ? 90 : -90, 0, 0, 2, true);
            mp.game.invoke('0x5F048334B4A4E774', lamps[9].handle, true, check ? 0 : 255,  check ? 255 : 0, 238);
            lamps[10].setRotation(check ? -90 : 90, 0, 0, 2, true);
            mp.game.invoke('0x5F048334B4A4E774', lamps[10].handle, true, check ? 0 : 255,  check ? 255 : 0, 238);
            lamps[11].setRotation(check ? 90 : -90, 0, 0, 2, true);
            mp.game.invoke('0x5F048334B4A4E774', lamps[11].handle, true, check ? 238 : 0,  check ? 130 : 255, 238);
            lamps[12].setRotation(check ? -90 : 90, 0, 0, 2, true);
            mp.game.invoke('0x5F048334B4A4E774', lamps[12].handle, true, check ? 238 : 0,  check ? 130 : 255, 238);
            check = !check;

            setTimeout(() => {
                mp.game.invoke('0x5F048334B4A4E774', lamps[7].handle, true, 0, 0, 0);
                mp.game.invoke('0x5F048334B4A4E774', lamps[8].handle, true, 0, 0, 0);
                mp.game.invoke('0x5F048334B4A4E774', lamps[9].handle, true, 0, 0, 0);
                mp.game.invoke('0x5F048334B4A4E774', lamps[10].handle, true, 0, 0, 0);
                mp.game.invoke('0x5F048334B4A4E774', lamps[11].handle, true, 0, 0, 0);
                mp.game.invoke('0x5F048334B4A4E774', lamps[12].handle, true, 0, 0, 0);
            }, 60000 / (174 * 8) / 2);
        },60000 / (174 * 8));

        on = true;
    }
    else {
        mp.game.invoke('0x5F048334B4A4E774', lamps[7].handle, true, 0, 0, 0);
        mp.game.invoke('0x5F048334B4A4E774', lamps[8].handle, true, 0, 0, 0);
        mp.game.invoke('0x5F048334B4A4E774', lamps[9].handle, true, 0, 0, 0);
        mp.game.invoke('0x5F048334B4A4E774', lamps[10].handle, true, 0, 0, 0);
        mp.game.invoke('0x5F048334B4A4E774', lamps[11].handle, true, 0, 0, 0);
        mp.game.invoke('0x5F048334B4A4E774', lamps[12].handle, true, 0, 0, 0);
        clearInterval(inter);
        on = false;
    }
});

mp.keys.bind(0x45, true, function() {
    if(!on){
        let check = false;
        inter = setInterval(() => {
 
            lamps[7].setRotation(check ? 90 : -90, 0, 0, 2, true);
            mp.game.invoke('0x5F048334B4A4E774', lamps[7].handle, true, check ? 238 : 0,  check ? 130 : 255, 238);
            lamps[8].setRotation(check ? -90 : 90, 0, 0, 2, true);
            mp.game.invoke('0x5F048334B4A4E774', lamps[8].handle, true, check ? 238 : 0,  check ? 130 : 255, 238);
            lamps[9].setRotation(check ? 90 : -90, 0, 0, 2, true);
            mp.game.invoke('0x5F048334B4A4E774', lamps[9].handle, true, check ? 0 : 255,  check ? 255 : 0, 238);
            lamps[10].setRotation(check ? -90 : 90, 0, 0, 2, true);
            mp.game.invoke('0x5F048334B4A4E774', lamps[10].handle, true, check ? 0 : 255,  check ? 255 : 0, 238);
            lamps[11].setRotation(check ? 90 : -90, 0, 0, 2, true);
            mp.game.invoke('0x5F048334B4A4E774', lamps[11].handle, true, check ? 238 : 0,  check ? 130 : 255, 238);
            lamps[12].setRotation(check ? -90 : 90, 0, 0, 2, true);
            mp.game.invoke('0x5F048334B4A4E774', lamps[12].handle, true, check ? 238 : 0,  check ? 130 : 255, 238);
            check = !check;

            setTimeout(() => {
                mp.game.invoke('0x5F048334B4A4E774', lamps[7].handle, true, 0, 0, 0);
                mp.game.invoke('0x5F048334B4A4E774', lamps[8].handle, true, 0, 0, 0);
                mp.game.invoke('0x5F048334B4A4E774', lamps[9].handle, true, 0, 0, 0);
                mp.game.invoke('0x5F048334B4A4E774', lamps[10].handle, true, 0, 0, 0);
                mp.game.invoke('0x5F048334B4A4E774', lamps[11].handle, true, 0, 0, 0);
                mp.game.invoke('0x5F048334B4A4E774', lamps[12].handle, true, 0, 0, 0);
            }, 60000 / (174 * 4) / 2);
        },60000 / (174 * 4));

        on = true;
    }
    else {
        mp.game.invoke('0x5F048334B4A4E774', lamps[7].handle, true, 0, 0, 0);
        mp.game.invoke('0x5F048334B4A4E774', lamps[8].handle, true, 0, 0, 0);
        mp.game.invoke('0x5F048334B4A4E774', lamps[9].handle, true, 0, 0, 0);
        mp.game.invoke('0x5F048334B4A4E774', lamps[10].handle, true, 0, 0, 0);
        mp.game.invoke('0x5F048334B4A4E774', lamps[11].handle, true, 0, 0, 0);
        mp.game.invoke('0x5F048334B4A4E774', lamps[12].handle, true, 0, 0, 0);
        clearInterval(inter);
        on = false;
    }
});


mp.keys.bind(0x46, true, function() {
    if(!on){
        let check = false;
        inter = setInterval(() => {
         

            lamps[7].setRotation(check ? 90 : -90, 0, 0, 2, true);
            mp.game.invoke('0x5F048334B4A4E774', lamps[7].handle, true, check ? 0 : 255,  check ? 255 : 0, 0);
            lamps[8].setRotation(check ? -90 : 90, 0, 0, 2, true);
            mp.game.invoke('0x5F048334B4A4E774', lamps[8].handle, true, check ? 255 : 0,  check ? 0 : 255, 0);
            lamps[9].setRotation(check ? 90 : -90, 0, 0, 2, true);
            mp.game.invoke('0x5F048334B4A4E774', lamps[9].handle, true, check ? 0 : 255,  check ? 255 : 0, 0);
            lamps[10].setRotation(check ? -90 : 90, 0, 0, 2, true);
            mp.game.invoke('0x5F048334B4A4E774', lamps[10].handle, true, check ? 0 : 255,  check ? 255 : 0, 0);
            lamps[11].setRotation(check ? 90 : -90, 0, 0, 2, true);
            mp.game.invoke('0x5F048334B4A4E774', lamps[11].handle, true, check ? 255 : 0,  check ? 0 : 255, 0);
            lamps[12].setRotation(check ? -90 : 90, 0, 0, 2, true);
            mp.game.invoke('0x5F048334B4A4E774', lamps[12].handle, true, check ? 0 : 255,  check ? 255 : 0, 0);
            check = !check;

            setTimeout(() => {
                mp.game.invoke('0x5F048334B4A4E774', lamps[7].handle, true, 0, 0, 0);
                mp.game.invoke('0x5F048334B4A4E774', lamps[8].handle, true, 0, 0, 0);
                mp.game.invoke('0x5F048334B4A4E774', lamps[9].handle, true, 0, 0, 0);
                mp.game.invoke('0x5F048334B4A4E774', lamps[10].handle, true, 0, 0, 0);
                mp.game.invoke('0x5F048334B4A4E774', lamps[11].handle, true, 0, 0, 0);
                mp.game.invoke('0x5F048334B4A4E774', lamps[12].handle, true, 0, 0, 0);
            }, 60000 / (174 * 2) / 2);
        },60000 / (174 * 2));

        on = true;
    }
    else {
        mp.game.invoke('0x5F048334B4A4E774', lamps[7].handle, true, 0, 0, 0);
        mp.game.invoke('0x5F048334B4A4E774', lamps[8].handle, true, 0, 0, 0);
        mp.game.invoke('0x5F048334B4A4E774', lamps[9].handle, true, 0, 0, 0);
        mp.game.invoke('0x5F048334B4A4E774', lamps[10].handle, true, 0, 0, 0);
        mp.game.invoke('0x5F048334B4A4E774', lamps[11].handle, true, 0, 0, 0);
        mp.game.invoke('0x5F048334B4A4E774', lamps[12].handle, true, 0, 0, 0);
        clearInterval(inter);
        on = false;
    }
});

mp.keys.bind(0x47, true, function() {
    if(!on){
        let check = false;
        inter = setInterval(() => {
         

            lamps[7].setRotation(check ? 90 : -90, 0, 0, 2, true);
            mp.game.invoke('0x5F048334B4A4E774', lamps[7].handle, true, check ? 0 : 255,  check ? 255 : 0, 0);
            lamps[8].setRotation(check ? -90 : 90, 0, 0, 2, true);
            mp.game.invoke('0x5F048334B4A4E774', lamps[8].handle, true, check ? 255 : 0,  check ? 0 : 255, 0);
            lamps[9].setRotation(check ? 90 : -90, 0, 0, 2, true);
            mp.game.invoke('0x5F048334B4A4E774', lamps[9].handle, true, check ? 0 : 255,  check ? 255 : 0, 0);
            lamps[10].setRotation(check ? -90 : 90, 0, 0, 2, true);
            mp.game.invoke('0x5F048334B4A4E774', lamps[10].handle, true, check ? 0 : 255,  check ? 255 : 0, 0);
            lamps[11].setRotation(check ? 90 : -90, 0, 0, 2, true);
            mp.game.invoke('0x5F048334B4A4E774', lamps[11].handle, true, check ? 255 : 0,  check ? 0 : 255, 0);
            lamps[12].setRotation(check ? -90 : 90, 0, 0, 2, true);
            mp.game.invoke('0x5F048334B4A4E774', lamps[12].handle, true, check ? 0 : 255,  check ? 255 : 0, 0);
            check = !check;

            /*setTimeout(() => {
                mp.game.invoke('0x5F048334B4A4E774', lamps[7].handle, true, 0, 0, 0);
                mp.game.invoke('0x5F048334B4A4E774', lamps[8].handle, true, 0, 0, 0);
                mp.game.invoke('0x5F048334B4A4E774', lamps[9].handle, true, 0, 0, 0);
                mp.game.invoke('0x5F048334B4A4E774', lamps[10].handle, true, 0, 0, 0);
                mp.game.invoke('0x5F048334B4A4E774', lamps[11].handle, true, 0, 0, 0);
                mp.game.invoke('0x5F048334B4A4E774', lamps[12].handle, true, 0, 0, 0);
            }, 60000 / 128 / 2);*/
        },60000 / 174);

        on = true;
    }
    else {
        mp.game.invoke('0x5F048334B4A4E774', lamps[7].handle, true, 0, 0, 0);
        mp.game.invoke('0x5F048334B4A4E774', lamps[8].handle, true, 0, 0, 0);
        mp.game.invoke('0x5F048334B4A4E774', lamps[9].handle, true, 0, 0, 0);
        mp.game.invoke('0x5F048334B4A4E774', lamps[10].handle, true, 0, 0, 0);
        mp.game.invoke('0x5F048334B4A4E774', lamps[11].handle, true, 0, 0, 0);
        mp.game.invoke('0x5F048334B4A4E774', lamps[12].handle, true, 0, 0, 0);
        clearInterval(inter);
        on = false;
    }
});