﻿global.board = mp.browsers.new('package://cef/board.html');
global.openOutType = -1;

mp.keys.bind(Keys.VK_I, false, function () {

    if (!loggedin || chatActive || editing || cuffed || localplayer.getVariable('InDeath') == true) return;

    if (global.boardOpen)
        closeBoard();
    else
        openBoard();
});

mp.keys.bind(Keys.VK_ESCAPE, false, function() {

    if (global.boardOpen) {
        mp.game.ui.setPauseMenuActive(false);
        closeBoard();
    }
});


// DONATE //
var reds = 0;
var donateOpened = false;
mp.keys.bind(0x78, false, function () { // F9
    if (!global.loggedin) return;

    if (global.menuCheck()) {
        if (donateOpened) {
            global.menuClose();
            menu.execute(`donate.close()`);
            donateOpened = false;
        }
	} else {
        global.menuOpen();
        donateOpened = true;
        menu.execute(`donate.show(${reds})`);
	}
});

mp.events.add("WheelsRun", () => {
    board.execute(`wheelrun();`);
});
mp.events.add("WheelsRun", () => {
    board.execute(`wheelrun();`);
});

mp.events.add('wheelAdd', (id, data) => {
    mp.events.callRemote("wheelAddsrv", id, data);
});
mp.events.add('wheel', (id, data) => {
    mp.events.callRemote("donate", id, data);
});
mp.events.add('donbuy', (id, data) => {
	global.menuClose();
	mp.events.call('fromBlur', 200)
	board.execute(`board.close()`);
    mp.events.callRemote("donate", id, data);
});
mp.events.add('redset', (reds_) => {
    reds = reds_;
    if (board != null)
        board.execute(`board.balance=${reds}`);
});


function openBoard() {
	if(board == null) return;
	if (global.menuCheck()) return;
    menuOpen();
	board.execute('board.active=true');
	 mp.events.callRemote('checkDonate');
	global.boardOpen = true;
}

function closeBoard() {
	if(board == null) return;
    menuClose();
    board.execute('context.hide()');
	board.execute('board.active=false');
    board.execute('board.outside=false');
    global.boardOpen = false;

    if (global.openOutType != -1) {
        mp.events.callRemote('closeInventory');
        global.openOutType = -1;
    }
}
// // //
// 0 - Open
// 1 - Close
// 2 - Statistics data
// 3 - Inventory data
// 4 - Outside data
// 5 - Outside on/off
// // //

// mp.events.add('board', (act, data, index) => {
//     if (board === null)
//         global.board = mp.browsers.new('package://cef/board.html');
//
// 	switch(act){
// 		case 0:
// 			openBoard();
// 			break;
//         case 1:
// 			closeBoard();
// 			break;
//         case 2:
// 			board.execute(`board.stats=${data}`);
// 			break;
// 		case 3:
// 			board.execute(`board.itemsSet(${data})`);
// 			break;
// 		case 4:
// 			board.execute(`board.outSet(${data})`);
// 			break;
// 		case 5:
//             board.execute(`board.outside=${data}`);
//             global.openOutType = 0;
// 			break;
//         case 6:
//             board.execute(`board.itemUpd(${index},${data})`);
//         	break;
//         case 11:
//             global.openOutType = -1;
//             closeBoard();
//         	break;
// 	}
// });



// mp.events.add('boardCB', (act, type, index) => {
// 	if(new Date().getTime() - global.lastCheck < 100) return;
// 	global.lastCheck = new Date().getTime();
// 	// bullshit, required refactor
// 	switch(act){
// 		case 1:
// 		mp.events.callRemote('Inventory', type, index, 'use');
// 		break;
// 		case 2:
// 		mp.events.callRemote('Inventory', type, index, 'transfer');
// 		break;
// 		case 3:
// 		mp.events.callRemote('Inventory', type, index, 'take');
// 		break;
// 		case 4:
// 		mp.events.callRemote('Inventory', type, index, 'drop');
// 		break;
// 	}
// });

mp.events.add("playerQuit", (player, exitType, reason) => {
    if (board !== null) {
        if (player.name === localplayer.name) {
            board.destroy();
            board = null;
        }
    }
});

mp.events.add('BOARD::LOAD_ASSETS_INFO', (houseData, businessData, vehiclesData) => {
    try {
	houseData = JSON.parse(houseData);
	businessData = JSON.parse(businessData);
	vehiclesData = JSON.parse(vehiclesData);
	let data = {
		"House": houseData,
		"Business": businessData,
		"Vehicles": vehiclesData
	}
	let json = JSON.stringify(data);
	board.execute(`board.properties=${json}`);
    }
    catch(ex){
        mp.gui.chat.push(`${ex}`);
    }
});
mp.events.add('BOARD::GET_ASSETS_INFO', () => {
	mp.events.callRemote("REMOTE::LOAD_PROPERTIES_INFO_TO_BOARD");
});
