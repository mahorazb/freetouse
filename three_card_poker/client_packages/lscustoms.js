var lsc = mp.browsers.new('package://cef/lscustoms/home.html');
var lscSpeed = 0;
var lscBrakes = 0;
var lscBoost = 0;
var lscСlutch = 0;
var lscPage = 'home';
var lscSettedMod = -1;
var lscSettedWheelType = 0;
var lscSelected = -1;
var lscRGB = { r: 0, g: 0, b: 0 };
var lscPrimary = { r: 0, g: 0, b: 0 };
var lscSecondary = { r: 0, g: 0, b: 0 };
var lscNeon = { r: 0, g: 0, b: 0 };
var opened = false;
var priceMod = 1;
var isBike = false;
var modelPriceMod = 1;
var carName = "";
let corectedMod = {
    4: 0, //Глушитель
    3: 1, //Пороги
    7: 2, //Капот
    0: 3, //Спойлер
    6: 4, //Решетка
    8: 5, //Фары
    10: 6, //Крыша
    48: 6, //Винилы
    1: 8, //Передний бампер
    2: 9, //Задний бампер
    11: 10, //Двигатель
    18: 11, //Турбо
    14: 12, //Клаксоны
    13: 13, //Коробка
    56: 14, //Тонировка
    15: 15, //Подвеска
    12: 16, //Тормоза
    23: 19, //Диски
    24: 19, //Диски
    16: 20, //Броня
    5: 21, //Салон
    57: 57, //Покраска
    58: 58, //Основной цвет
    59: 59, //Вторичный цвет
    60: 60, //Неон
}
setTimeout(function() { lsc.execute(`tuning.show(${false});`); }, 1500);
mp.events.add("turnChange_castom", (val) => {
    mp.players.local.vehicle.setHeading(val);
});
mp.events.add("heightChange_castom", (val) => {
    tunCam.setCoord(-333.7966, -137.409, 40.58963 + val)
});
mp.events.add('hideTun', () => {
    lsc.execute(`tuning.show(${false});`);
});
mp.events.add('tuning.setVehiclePrice', (price) => {
    lsc.execute(`tuning.vehiclePrice=${parseInt(price)};`);
});
mp.events.add('buy-tuning', (mod, index, priceMod) => {
    let color;
    let paintType;
    if (parseInt(mod) == 58) {
        try {
            color = hexToRgb(index);
            mp.events.callRemote('buyTuning', 20, 0, color.r, color.g, color.b, priceMod);
            return;
        } catch (e) { lsc.execute(`console.log('${' '+e}');`); }
    }
    if (parseInt(mod) == 59) {
        try {
            color = hexToRgb(index);
            mp.events.callRemote('buyTuning', 20, 1, color.r, color.g, color.b, priceMod);
            return;
        } catch (e) {
            lsc.execute(`console.log('${' '+e}');`);
        }
    }
    if (parseInt(mod) == 60) {
        try {
            color = hexToRgb(index);
            mp.events.callRemote('buyTuning', 60, 0, color.r, color.g, color.b, priceMod);
            return;
        } catch (e) {
            lsc.execute(`console.log('${' '+e}');`);
        }
    }
    mod = corectedMod[parseInt(mod)];
    if (parseInt(mod) == 19) {
        try {
            mp.events.callRemote('buyTuning', 19, index, mp.players.local.vehicle.getWheelType(), -1, -1, priceMod);
            lsc.execute(`console.log(' mp.events.callRemote(buyTuning, 19, ${index, mp.players.local.vehicle.getWheelType()});');`);
            return;
        } catch (e) {
            lsc.execute(`console.log('${' '+e}');`);
        }
    }
    mp.events.callRemote('buyTuning', parseInt(mod), parseInt(index), -1, -1, -1, priceMod);

});
mp.events.add('exitTun', () => {
    global.menuClose();
    tunCam.destroy();
    mp.game.cam.renderScriptCams(false, false, 500, true, false);
    mp.events.callRemote('exitTuning');
    opened = false;
});
mp.events.add("setColorTuning", (mod, rgb) => {
    let color = hexToRgb(rgb);
    if (color) {
        if (mod == 58) {
            mp.players.local.vehicle.setCustomPrimaryColour(parseInt(color.r), parseInt(color.g), parseInt(color.b));
        } else if (mod == 59) {
            mp.players.local.vehicle.setCustomSecondaryColour(parseInt(color.r), parseInt(color.g), parseInt(color.b));
        } else if (mod == 60) {
            mp.players.local.vehicle.setNeonLightEnabled(0, true);
            mp.players.local.vehicle.setNeonLightEnabled(1, true);
            mp.players.local.vehicle.setNeonLightEnabled(2, true);
            mp.players.local.vehicle.setNeonLightEnabled(3, true);
            mp.players.local.vehicle.setNeonLightsColour(parseInt(color.r), parseInt(color.g), parseInt(color.b));
            lsc.execute(`console.log('mp.players.local.vehicle.setNeonLightsColour(${parseInt(color.r)}, ${parseInt(color.g)}, ${parseInt(color.b)});');`);
            lsc.execute(`console.log('r${mp.players.local.vehicle.getNeonLightsColour(1, 1, 1).r}g${mp.players.local.vehicle.getNeonLightsColour(1, 1, 1).g}b${mp.players.local.vehicle.getNeonLightsColour(1, 1, 1).b};');`);
        }
    }
});

function hexToRgb(hex) {
    var result = /^#?([a-f\d]{2})([a-f\d]{2})([a-f\d]{2})$/i.exec(hex);
    return result ? {
        r: parseInt(result[1], 16),
        g: parseInt(result[2], 16),
        b: parseInt(result[3], 16)
    } : null;
}
mp.events.add("setMods", (index, index2, index3 = false) => {
            lsc.execute(`console.log('${index}, ${index2}, ${index3} = false');`);
    if (parseInt(index) == 56) return mp.players.local.vehicle.setWindowTint(parseInt(index2));
    if (parseInt(index) == 25) return mp.players.local.vehicle.setNumberPlateTextIndex(parseInt(index2))
    if (parseInt(index) == 14) return mp.players.local.vehicle.startHorn(2000, hornNames[parseInt(index2)], false);
    if (parseInt(index) == 24) {
        lsc.execute(`console.log('localplayer.vehicle.setMod(23, ${parseInt(index2)})');`);
        mp.players.local.vehicle.setMod(23, parseInt(index2));
        return
    }
    mp.players.local.vehicle.setMod(parseInt(index), parseInt(index2));
   
});
let priceWhell = {
    0: 3,
    1: 4,
    2: 6,
    3: 5,
    4: 4,
    5: 3,
    6: 3,
    7: 6,
    8: 7,
    9: 5,
    10: 6,
    11: 4,
    12: 6,
    13: 3,
    14: 4,
    15: 6,
    16: 5,
    17: 4,
    18: 3,
    19: 3,
    20: 6,
    21: 7,
    22: 5,
    23: 6,
    24: 4,
    25: 6,
    26: 3,
    27: 4,
    28: 6,
    29: 5,
    30: 4,
    31: 3,
    32: 3,
    33: 6,
    34: 7,
    35: 5,
    36: 6,
    37: 4,
    38: 6,
    39: 3,
    40: 4,
    41: 6,
    42: 5,
    43: 4,
    44: 3,
    45: 3,
    46: 6,
    47: 7,
    48: 5,
    49: 6,
    50: 4,
    51: 6,
    52: 3,
    53: 4,
    54: 6,
    55: 5,
    56: 4,
    57: 3,
    58: 3,
    59: 6,
    60: 7,
    61: 5,
    62: 6,
    63: 4,
    64: 6,
    65: 3,
    66: 4,
    67: 6,
    68: 5,
    69: 4,
    70: 3,
    71: 3,
    72: 6,
    73: 7,
    74: 5,
    75: 6,
    76: 4,
    77: 6,
    78: 3,
    79: 4,
    80: 6,
    81: 5,
    82: 4,
    83: 3,
    84: 3,
    85: 6,
    86: 7,
    87: 5,
    88: 6,
    89: 4,
    90: 6,
    91: 3,
    92: 4,
    93: 6,
    94: 5,
    95: 4,
    96: 3,
    97: 3,
    98: 6,
    99: 7,
    100: 5
}
mp.events.add("getWhellType", (index) => {
    lsc.execute(`console.log(${index})`);
    try{
    mp.players.local.vehicle.setWheelType(parseInt(index));
    let lc = {name:index,submenu:{}};
    lc.submenu = {};
    for (var j = 0; j < mp.players.local.vehicle.getNumMods(23); j++) {
        lc.submenu[j] = {};
        lc.submenu[j].name = mp.players.local.vehicle.getModTextLabel(23, parseInt(j));
        lc.submenu[j].price = {};
        lc.submenu[j].price[j] = priceWhell[j];
    }

    // { 
    //                 0: {price:{0:1}, name: '10%' }, 
    //                 1: {price:{1:2}, name: '90%' }, 
    //                 2: {price:{2:3}, name: '50%' }, 
    //                 3: {price:{3:5}, name: '30%' } 
    //             }
    lsc.execute(`console.log('${JSON.stringify(lc)}')`);
    lsc.execute(`tuning.setSelectTun(JSON.parse('${JSON.stringify(lc)}'),24)`);
    }catch(e){lsc.execute(`console.log('${' '+e}')`);}
});
let lc = {}
mp.events.add("playerEnterVehicle", (vehicle, seat) => {
    // for (var i = 0; i <= 100; i++) {
    //     let lc2 = [];
    //     for (var j = 0; j < vehicle.getNumMods(i); j++) {
    //         lc2.push(vehicle.getModTextLabel(i, j))
    //         // mp.debug.log(`${i} - ${vehicle.getModTextLabel(i, j)}`)
    //     }
    //     if(lc2.length)lc[i] = lc2;
    // }
    // lsc.execute(`tuning.setModData(JSON.parse('${JSON.stringify(lc)}'));`);
    // vehicle.setHandling("diffuseTint",0x00FF0000)
})
mp.events.add('browserDomReady', (browser) => {
    if (browser === lsc && !opened) {
        lsc.execute(`tuning.show(${false});`);
    }
});
// Switch global page //
mp.events.add('tpage', (id) => {
    afkSecondsCount = 0;
    if (!opened) return;

    if (id == "exit_menu") {
        lsc.execute(`tuning.show(${false});`);
        global.menuClose();
        tunCam.destroy();
        mp.game.cam.renderScriptCams(false, false, 500, true, false);
        mp.events.callRemote('exitTuning');
        opened = false;
    } else {
        lsc.execute(`window.location = 'package://cef/lscustoms/${id}.html'`);
        lsc.execute(`tuning.set(${lscSpeed},${lscBrakes},${lscBoost},${lscСlutch})`);

        if (id == "home") {
            setTimeout(function() { lsc.execute(`tuning.disable(${JSON.stringify(toDisable)});`); }, 150);
            localplayer.vehicle.setHeading(148.9986);

            var camFrom = tunCam;
            tunCam = mp.cameras.new('default', new mp.Vector3(-333.7966, -137.409, 40.58963), new mp.Vector3(0, 0, 0), 60);
            tunCam.pointAtCoord(-338.7966, -137.409, 37.88963);
            tunCam.setActiveWithInterp(camFrom.handle, 500, 1, 1);

            if (lscPage == "numbers_menu") {
                localplayer.vehicle.setNumberPlateTextIndex(lscSettedMod);
            } else if (lscPage == "paint_menu_three") {
                localPlayer.vehicle.setNeonLightEnabled(0, false);
                localPlayer.vehicle.setNeonLightEnabled(1, false);
                localPlayer.vehicle.setNeonLightEnabled(2, false);
                localPlayer.vehicle.setNeonLightEnabled(3, false);
                mp.events.call("hideColorp");
            } else if (lscPage == "glasses_menu") {
                localplayer.vehicle.setWindowTint(lscSettedMod);
            } else if (lscPage != "paint_menu") {
                if (lscPage == "turbo_menu")
                    localplayer.vehicle.toggleMod(18, false);
                else if (lscPage == "lights_menu") {
                    mp.game.invoke('0xE41033B25D003A07', localplayer.vehicle.handle, 0);
                    localplayer.vehicle.setLights(false);
                } else if (lscPage == "wheels_menu")
                    localplayer.vehicle.setWheelType(lscSettedWheelType);

                if (categoryModsIds[lscPage] == undefined) return;
                localplayer.vehicle.setMod(categoryModsIds[lscPage], lscSettedMod);
            }
        } else if (categoryIds[id] != undefined) {
            var camFrom = tunCam;
            tunCam = mp.cameras.new('default', categoryPositions[categoryIds[id]].CamPosition, new mp.Vector3(0, 0, 0), 60);
            tunCam.pointAtCoord(-338.7966, -137.409, 37.88963);
            tunCam.setActiveWithInterp(camFrom.handle, 500, 1, 1);

            localplayer.vehicle.setHeading(categoryPositions[categoryIds[id]].CarHeading);

            if (id == "numbers_menu") {
                lscSettedMod = localplayer.vehicle.getNumberPlateTextIndex();
            } else if (id == "glasses_menu") {
                lscSettedMod = localplayer.vehicle.getWindowTint();
            } else if (id == "paint_menu") {
                if (lscPage != "home") {
                    mp.events.call("hideColorp");
                    localplayer.vehicle.setCustomPrimaryColour(lscPrimary.r, lscPrimary.g, lscPrimary.b);
                    localplayer.vehicle.setCustomSecondaryColour(lscSecondary.r, lscSecondary.g, lscSecondary.b);
                    localplayer.vehicle.setNeonLightEnabled(0, true);
                    localplayer.vehicle.setNeonLightEnabled(1, true);
                    localplayer.vehicle.setNeonLightEnabled(2, true);
                    localplayer.vehicle.setNeonLightEnabled(3, true);
                    localplayer.vehicle.setNeonLightsColour(lscNeon.r, lscNeon.g, lscNeon.b);
                } else {
                    lscPrimary = localplayer.vehicle.getCustomPrimaryColour(1, 1, 1);
                    lscSecondary = localplayer.vehicle.getCustomSecondaryColour(1, 1, 1);
                    lscNeon = localplayer.vehicle.getNeonLightsColour(1, 1, 1);
                }
            } else {
                if (lscPage == "home") {
                    if (id == "lights_menu") {
                        localplayer.vehicle.setLights(true);
                        if (id >= 0) mp.game.invoke('0xE41033B25D003A07', localplayer.vehicle.handle, lscSettedMod);
                    } else if (id == "wheels_menu")
                        lscSettedWheelType = localplayer.vehicle.getWheelType();
                    lscSettedMod = localplayer.vehicle.getMod(categoryModsIds[id]);
                }
            }

            if (categoryIds[id] <= 9) {
                var elements = [];
                if (tuningConf[carName][categoryIds[id]] != undefined) {
                    global.tuningConf[carName][categoryIds[id]].forEach(element => {
                        var price = element.Item3 * priceMod;
                        elements.push([`${element.Item1}`, element.Item2, price.toFixed()]);
                    });
                    setTimeout(function() { lsc.execute(`tuning.add(${JSON.stringify(elements)})`); }, 150);
                } else mp.events.call('notify', 1, 4, "Этот раздел недоступен для данного авто.", 3000);
            } else if (categoryIds[id] <= 18) {
                var prices = [];
                for (var key in global.tuningStandart[categoryIds[id]]) {
                    var price = global.tuningStandart[categoryIds[id]][key] * modelPriceMod * priceMod;
                    prices.push([`${key}`, price.toFixed()]);
                }
                setTimeout(function() { lsc.execute(`tuning.price(${JSON.stringify(prices)})`); }, 150);
            }
        } else if (wheelsTypes[id] != undefined) {
            localplayer.vehicle.setWheelType(wheelsTypes[id]);
            var prices = [];
            for (var key in global.tuningWheels[wheelsTypes[id]]) {
                var price = global.tuningWheels[wheelsTypes[id]][key] * priceMod;
                prices.push([`${key}`, price.toFixed()]);
            }
            setTimeout(function() { lsc.execute(`tuning.price(${JSON.stringify(prices)})`); }, 150);
        } else if (id == "paint_menu_one" || id == "paint_menu_two") {
            var price = 5000 * priceMod;
            var prices = ["buy", price.toFixed()];
            setTimeout(function() { lsc.execute(`tuning.price(${JSON.stringify(prices)})`); }, 150);
            mp.events.call("showColorp");
        } else if (id == "paint_menu_three") {
            var price = 250000 * priceMod;
            var prices = ["buy", price.toFixed()];
            setTimeout(function() { lsc.execute(`tuning.price(${JSON.stringify(prices)})`); }, 150);
            if (!isBike) mp.events.call("showColorp");
        }
        if (toDisable.includes(id)) {
            mp.events.call('tpage', "home");
            mp.events.call('notify', 1, 4, "Этот раздел недоступен для Вашего транспорта.", 3000);
        }

        setTimeout(function() { mp.events.call('tupd'); }, 150);
        lscPage = id;
    }
})
// Forced update //
mp.events.add('tupd', () => {
    lscSpeed = (mp.game.vehicle.getVehicleModelMaxSpeed(localplayer.vehicle.model) / 1.2).toFixed();
    lscBrakes = localplayer.vehicle.getMaxBraking() * 100;
    lscBoost = localplayer.vehicle.getAcceleration() * 100;
    lscСlutch = localplayer.vehicle.getMaxTraction() * 10;
    lsc.execute(`tuning.set(${lscSpeed},${lscBrakes},${lscBoost},${lscСlutch})`);
})
// Click on element //
mp.events.add('tclk', (id) => {
    afkSecondsCount = 0;
    if (id == undefined) return;

    id = parseInt(id);
    var setted = false;
    switch (lscPage) {
        case "muffler_menu":
            if (vehicleComponents.Muffler == id) setted = true;
            break;
        case "sideskirt_menu":
            if (vehicleComponents.SideSkirt == id) setted = true;
            break;
        case "hood_menu":
            if (vehicleComponents.Hood == id) setted = true;
            break;
        case "spoiler_menu":
            if (vehicleComponents.Spoiler == id) setted = true;
            break;
        case "lattice_menu":
            if (vehicleComponents.Lattice == id) setted = true;
            break;
        case "wings_menu":
            if (vehicleComponents.Wings == id) setted = true;
            break;
        case "roof_menu":
            if (vehicleComponents.Roof == id) setted = true;
            break;
        case "flame_menu":
            if (vehicleComponents.Vinyls == id) setted = true;
            break;
        case "bamper_menu_front":
            if (vehicleComponents.FrontBumper == id) setted = true;
            break;
        case "bamper_menu_back":
            if (vehicleComponents.RearBumper == id) setted = true;
            break;
        case "engine_menu":
            if (vehicleComponents.Engine == id) setted = true;
            break;
        case "turbo_menu":
            if (vehicleComponents.Turbo == id) setted = true;
            break;
        case "horn_menu":
            if (vehicleComponents.Horn == id) setted = true;
            break;
        case "transmission_menu":
            if (vehicleComponents.Transmission == id) setted = true;
            break;
        case "glasses_menu":
            if (vehicleComponents.WindowTint == id) setted = true;
            break;
        case "suspention_menu":
            if (vehicleComponents.Suspension == id) setted = true;
            break;
        case "brakes_menu":
            if (vehicleComponents.Brakes == id) setted = true;
            break;
        case "lights_menu":
            if (vehicleComponents.Headlights == id) setted = true;
            break;
        case "numbers_menu":
            if (vehicleComponents.NumberPlate == id) setted = true;
            break;
        case "wheels_exclusive":
        case "wheels_lowrider":
        case "wheels_musclecar":
        case "wheels_4x4":
        case "wheels_sport":
        case "wheels_4x4_2":
        case "wheels_tuner":
            if (vehicleComponents.WheelsType == wheelsTypes[lscPage] && vehicleComponents.Wheels == id) setted = true;
            break;
    }

    if (setted)
        mp.events.call('notify', 1, 9, "У Вас уже установлена данная модификация", 3000);
    else {
        lsc.execute(`tuning.show(${false});`);
        opened = false;
        lscSelected = id;

        if (lscPage === "paint_menu_one" || lscPage === "paint_menu_two" || lscPage === "paint_menu_three")
            mp.events.call("hideColorp");

        var title = (lscPage === "paint_menu_one" || lscPage === "paint_menu_two" || lscPage === "paint_menu_three") ? "Вы действительно хотите покрасить машину в данный цвет?" : "Вы действительно хотите установить данную модификацию?";
        mp.events.call('openDialog', 'tuningbuy', title);
    }
})
// Hover on element //
mp.events.add('thov', (id) => {
    afkSecondsCount = 0;
    if (lscPage === "wheels_menu") return;

    if (lscPage == "numbers_menu") {
        localplayer.vehicle.setNumberPlateTextIndex(parseInt(id));
    } else if (lscPage == "glasses_menu") {
        localplayer.vehicle.setWindowTint(parseInt(id));
    } else if (lscPage == "horn_menu") {
        localplayer.vehicle.startHorn(1000, hornNames[id], false);
    } else if (lscPage == "lights_menu") {
        localplayer.vehicle.setLights(true);
        if (id >= 0) {
            localplayer.vehicle.setMod(22, 0);
            mp.game.invoke('0xE41033B25D003A07', localplayer.vehicle.handle, parseInt(id));
        } else localplayer.vehicle.setMod(22, -1);
    } else {
        if (categoryModsIds[lscPage] != undefined) {
            if (lscPage == "turbo_menu")
                localplayer.vehicle.toggleMod(18, true);
            localplayer.vehicle.setMod(categoryModsIds[lscPage], parseInt(id));
            mp.events.call('tupd');
        } else if (wheelsTypes[lscPage] != undefined) {
            localplayer.vehicle.setMod(23, parseInt(id));
        }
    }
})
// Buy element //
mp.events.add('tunbuy', (state) => {
    afkSecondsCount = 0;
    if (state) {
        if (wheelsTypes[lscPage] != undefined)
            mp.events.callRemote('buyTuning', 19, lscSelected, wheelsTypes[lscPage]);
        else if (lscPage === "paint_menu_one" || lscPage === "paint_menu_two" || lscPage === "paint_menu_three") {
            var paintType;
            if (lscPage === "paint_menu_one") paintType = 0;
            else if (lscPage === "paint_menu_two") paintType = 1;
            else if (lscPage === "paint_menu_three") paintType = 2;
            if (paintType == 2 && isBike) {
                mp.events.call('notify', 1, 4, "Этот раздел недоступен для мотоциклов.", 3000);
                lsc.execute(`tuning.show(${true});`);
                opened = true;
            } else mp.events.callRemote('buyTuning', 20, paintType, lscRGB.r, lscRGB.g, lscRGB.b);
        } else
            mp.events.callRemote('buyTuning', categoryIds[lscPage], lscSelected, -1);
    } else {
        lsc.execute(`tuning.show(${true});`);
        opened = true;
        if (lscPage == "numbers_menu") {
            localplayer.vehicle.setNumberPlateTextIndex(lscSettedMod);
        } else if (lscPage == "glasses_menu") {
            localplayer.vehicle.setWindowTint(lscSettedMod);
        } else if (lscPage == "paint_menu_one") {
            localplayer.vehicle.setCustomPrimaryColour(lscPrimary.r, lscPrimary.g, lscPrimary.b);
        } else if (lscPage == "paint_menu_two") {
            localplayer.vehicle.setCustomSecondaryColour(lscSecondary.r, lscSecondary.g, lscSecondary.b);
        } else if (lscPage == "paint_menu_three") {
            localplayer.vehicle.setNeonLightsColour(lscNeon.r, lscNeon.g, lscNeon.b);
        } else {
            if (lscPage == "turbo_menu")
                localplayer.vehicle.toggleMod(18, false);

            if (categoryModsIds[lscPage] == undefined) return;
            localplayer.vehicle.setMod(categoryModsIds[lscPage], lscSettedMod);
        }
    }
})
// mp.events.add('tunBuySuccess', (id) => {
//     afkSecondsCount = 0;
//     lsc.execute(`tuning.show(${true});`);
//     opened = true;
//     if (id != -2) {

//         lscSettedMod = id;
//         if (wheelsTypes[lscPage] != undefined)
//             lscSettedWheelType = localplayer.vehicle.getWheelType();
//         else if (lscPage == "paint_menu_one") {
//             mp.events.call("showColorp");
//             lscPrimary = localplayer.vehicle.getCustomPrimaryColour(1, 1, 1);
//         } else if (lscPage == "paint_menu_two") {
//             mp.events.call("showColorp");
//             lscSecondary = localplayer.vehicle.getCustomSecondaryColour(1, 1, 1);
//         } else if (lscPage == "paint_menu_three") {
//             mp.events.call("showColorp");
//             lscNeon = localplayer.vehicle.getNeonLightsColour(1, 1, 1);
//         }
//     }
// })
mp.events.add('tunColor', function(c) {
    if (!opened) return;
    afkSecondsCount = 0;
    if (lscPage == "paint_menu_one") {
        localplayer.vehicle.setCustomPrimaryColour(c.r, c.g, c.b);
        lscRGB = { r: c.r, g: c.g, b: c.b };
    } else if (lscPage == "paint_menu_two") {
        localplayer.vehicle.setCustomSecondaryColour(c.r, c.g, c.b);
        lscRGB = { r: c.r, g: c.g, b: c.b };
    } else if (lscPage == "paint_menu_three") {
        localplayer.vehicle.setNeonLightsColour(c.r, c.g, c.b);
        localplayer.vehicle.setNeonLightEnabled(0, true);
        localplayer.vehicle.setNeonLightEnabled(1, true);
        localplayer.vehicle.setNeonLightEnabled(2, true);
        localplayer.vehicle.setNeonLightEnabled(3, true);
        lscRGB = { r: c.r, g: c.g, b: c.b };
    }
});
var tunCam = null;
mp.events.add('setCamTun', (id) => {
    tunCam = mp.cameras.new('default', categoryPositions[categoryIds[id]].CamPosition, new mp.Vector3(0, 0, 0), 60);
    tunCam.pointAtCoord(-338.7966, -137.409, 37.88963);
    tunCam.setActiveWithInterp(camFrom.handle, 500, 1, 1);
})
var categoryPositions = [
    { 'CarHeading': 85.0, 'CamPosition': new mp.Vector3(-333.7966, -137.409, 38.88963) },
    { 'CarHeading': 148.9986, 'CamPosition': new mp.Vector3(-333.7966, -137.409, 39.28963) },
    { 'CarHeading': 265.0, 'CamPosition': new mp.Vector3(-333.7966, -137.409, 40.58963) },
    { 'CarHeading': 85.0, 'CamPosition': new mp.Vector3(-333.7966, -137.409, 39.28963) },
    { 'CarHeading': 265.0, 'CamPosition': new mp.Vector3(-333.7966, -137.409, 38.88963) },
    { 'CarHeading': 148.9986, 'CamPosition': new mp.Vector3(-333.7966, -137.409, 39.28963) },
    { 'CarHeading': 148.9986, 'CamPosition': new mp.Vector3(-333.7966, -137.409, 40.08963) },
    { 'CarHeading': 160.9986, 'CamPosition': new mp.Vector3(-333.7966, -137.409, 42.08963) },
    { 'CarHeading': 265.0, 'CamPosition': new mp.Vector3(-333.7966, -137.409, 38.88963) },
    { 'CarHeading': 85.0, 'CamPosition': new mp.Vector3(-333.7966, -137.409, 38.88963) },
    { 'CarHeading': 265.0, 'CamPosition': new mp.Vector3(-333.7966, -137.409, 40.58963) },
    { 'CarHeading': 265.0, 'CamPosition': new mp.Vector3(-333.7966, -137.409, 40.58963) },
    { 'CarHeading': 148.9986, 'CamPosition': new mp.Vector3(-333.7966, -137.409, 40.58963) },
    { 'CarHeading': 265.0, 'CamPosition': new mp.Vector3(-333.7966, -137.409, 40.58963) },
    { 'CarHeading': 148.9986, 'CamPosition': new mp.Vector3(-333.7966, -137.409, 39.28963) },
    { 'CarHeading': 148.9986, 'CamPosition': new mp.Vector3(-333.7966, -137.409, 39.28963) },
    { 'CarHeading': 265.0, 'CamPosition': new mp.Vector3(-333.7966, -137.409, 40.58963) },
    { 'CarHeading': 265.0, 'CamPosition': new mp.Vector3(-333.7966, -137.409, 38.88963) },
    { 'CarHeading': 85.0, 'CamPosition': new mp.Vector3(-333.7966, -137.409, 38.88963) },
    { 'CarHeading': 148.9986, 'CamPosition': new mp.Vector3(-333.7966, -137.409, 39.28963) },
    { 'CarHeading': 160.9986, 'CamPosition': new mp.Vector3(-333.7966, -137.409, 40.08963) },
];
var categoryIds = {
    0: 0,
    1: 1,
    3: 2,
    4: 3,
    10: 4,
    11: 5,
    12: 6,
    13: 7,
    14: 8,
    18: 9,
    25: 10,
    55: 11,
    56: 12,
    57: 13,
    "glasses_menu": 14,
    "suspention_menu": 15,
    "brakes_menu": 16,
    "lights_menu": 17,
    "numbers_menu": 18,
    "wheels_menu": 19,
    "paint_menu": 20,
};
var categoryModsIds = {
    "muffler_menu": 4,
    "sideskirt_menu": 3,
    "hood_menu": 7,
    "spoiler_menu": 0,
    "lattice_menu": 6,
    "wings_menu": 8,
    "roof_menu": 10,
    "flame_menu": 48,
    "bamper_menu_front": 1,
    "bamper_menu_back": 2,
    "engine_menu": 11,
    "turbo_menu": 18,
    "transmission_menu": 13,
    "suspention_menu": 15,
    "brakes_menu": 12,
    "lights_menu": 22,
    "horn_menu": 14,
    "wheels_menu": 23,
};
var categoryMods = [
    { Name: "muffler_menu", Index: 4 },
    { Name: "sideskirt_menu", Index: 3 },
    { Name: "hood_menu", Index: 7 },
    { Name: "spoiler_menu", Index: 0 },
    { Name: "lattice_menu", Index: 6 },
    { Name: "wings_menu", Index: 8 },
    { Name: "roof_menu", Index: 10 },
    { Name: "flame_menu", Index: 48 },
    { Name: "bamper_menu", Index: 1 },
];
var hornNames = {
    0: "HORN_STOCK",
    1: "HORN_TRUCK",
    2: "HORN_POLICE",
    3: "HORN_CLOWN",
    4: "HORN_MUSICAL1",
    5: "HORN_MUSICAL2",
    6: "HORN_MUSICAL3",
    7: "HORN_MUSICAL4",
    8: "HORN_MUSICAL5",
    9: "HORN_SADTROMBONE",
    10: "HORN_CALSSICAL1",
    11: "HORN_CALSSICAL2",
    12: "HORN_CALSSICAL3",
    13: "HORN_CALSSICAL4",
    14: "HORN_CALSSICAL5",
    15: "HORN_CALSSICAL6",
    16: "HORN_CALSSICAL7",
    17: "HORN_SCALEDO",
    18: "HORN_SCALERE",
    19: "HORN_SCALEMI",
    20: "HORN_SCALEFA",
    21: "HORN_SCALESOL",
    22: "HORN_SCALELA",
    23: "HORN_SCALETI",
    24: "HORN_SCALEDO_HIGH",
    25: "HORN_JAZZ1",
    26: "HORN_JAZZ2",
    27: "HORN_JAZZ3",
    28: "HORN_JAZZLOOP",
    29: "HORN_STARSPANGBAN1",
    30: "HORN_STARSPANGBAN2",
    31: "HORN_STARSPANGBAN3",
    32: "HORN_STARSPANGBAN4",
    33: "HORN_CLASSICALLOOP1",
    34: "HORN_CLASSICAL8",
    35: "HORN_CLASSICALLOOP2",

};
var wheelsTypes = {
    "wheels_exclusive": 7,
    "wheels_lowrider": 2,
    "wheels_musclecar": 1,
    "wheels_4x4": 3,
    "wheels_sport": 0,
    "wheels_4x4_2": 4,
    "wheels_tuner": 5,
};
var toDisable = ["armor_menu"];
var vehicleComponents = {};
mp.events.add('tuningUpd', function(components) {
    vehicleComponents = JSON.parse(components);
});
mp.events.add('setBuyTun', function(id, comp) {
    lsc.execute(`tuning.setBuyComp('${id}','${comp}');`);
});
mp.events.add('openTun', (priceModief, carModel, modelPriceModief, components) => {

    mp.gui.chat.push(`1`);
    afkSecondsCount = 0;
    opened = true;
    global.menuOpen();
    toDisable = ["armor_menu", "protection_menu"];
    categoryMods.forEach(element => {
        if (localplayer.vehicle.getNumMods(element.Index) <= 0) toDisable.push(element.Name);
    });
    mp.gui.chat.push(`2`);
    isBike = false;

   
    //lsc.execute(`tuning.disable(${JSON.stringify(toDisable)});`);
    lc = {};
    for (var i = 0; i <= 100; i++) {
        let lc2 = [];
        for (var j = 0; j < mp.players.local.vehicle.getNumMods(i); j++) {
            lc2.push("1")
            // mp.debug.log(`${i} - ${mp.players.local.vehicle.getModTextLabel(i, j)}`)
        }
        if (lc2.length) lc[i] = lc2;
    }

    lsc.execute(`tuning.setModData(JSON.parse('${JSON.stringify(lc)}'));`);
    lsc.execute(`tuning.setComp('${components}');`);
    lsc.execute(`console.log('${JSON.stringify(lc)}');`);
    mp.events.call('tupd');
    // lsc.execute(`tuning.show(${true});`);

    priceMod = priceModief / 100;
    modelPriceMod = modelPriceModief;
    carName = carModel;

     tunCam = mp.cameras.new('default', new mp.Vector3(-333.7966, -137.409, 40.58963), new mp.Vector3(0, 0, 0), 60);
    tunCam.pointAtCoord(-338.7966, -137.409, 37.88963);
     tunCam.setActive(true);
    mp.game.cam.renderScriptCams(true, false, 500, true, false);
 
    vehicleComponents = JSON.parse(components);
});

mp.events.add('tuningSeatsCheck', function() {
    var veh = localplayer.vehicle;
    for (var i = 0; i < 7; i++)
        if (veh.getPedInSeat(i) !== 0) {
            mp.events.call('notify', 4, 9, 'Попросите выйти всех пассажиров', 3000);
            return;
        }
    mp.events.callRemote('tuningSeatsCheck');
});