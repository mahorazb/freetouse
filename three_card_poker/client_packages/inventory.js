global.inventory = mp.browsers.new('package://cef/inventory.html');
global.openOutType = -1;
global.inventoryOpen = false;

mp.keys.bind(Keys.VK_TAB, false, function () {

    if (!loggedin || chatActive || editing || cuffed || localplayer.getVariable('InDeath') == true) return;

    if (global.inventoryOpen)
        mp.events.call('board', 'hide');
    else
        mp.events.call('board', 'open');
});

mp.keys.bind(Keys.VK_NUMPAD3, false, function () {
    mp.gui.execute(`HUD.togglePrev()`);
});

mp.keys.bind(Keys.VK_NUMPAD4, false, function () {
    mp.gui.execute(`HUD.toggleNext()`);
});

function openBoard() {
    if(global.inventory == null) return;
    if (global.openOutType !== 5) {
        if (global.menuCheck()) return;
    }
    global.menuOpen();
    global.inventory.execute('inventory.active=true');
    global.inventory.execute('inventory.show();');
    global.inventoryOpen = true;
}

function closeBoard() {
    if(inventory == null) return;
    global.menuClose();
    inventory.execute('context.hide()');
    inventory.execute('inventory.active=false');
    inventory.execute('inventory.outside=false');
    inventory.execute('inventory.trade=false');
    inventory.execute('inventory.tradeReady=false');
    inventory.execute('inventory.checked=false');
    inventory.execute('inventory.tradeMessage=""');
    inventory.execute('inventory.tradeCash=""');
    inventory.execute('inventory.clear()');
    global.inventoryOpen = false;

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

mp.events.add('board', (act, data, info) => {
    if (global.inventory === null)
        global.inventory = mp.browsers.new('package://cef/inventory.html');
    // mp.gui.chat.push(`act: ${act} | data: ${data}`);

    switch(act) {
        case 'open':
            if (global.circleOpen) {
                mp.events.call('circleCallback', -1);
            }
            openBoard();
            break;
        case "close_Board":
            if (global.circleOpen) {
                mp.events.call('circleCallback', -1);
            }
            closeBoard();
            break;
        case 2:
            board.execute(`board.stats=${data}`);
            break;
        case 'close':
            inventory.execute(`inventory.hideA();`);
            break;
        case 'hide':
            inventory.execute(`inventory.hideA();`);
            break;
        case 'stats':
            inventory.execute(`inventory.stats=${data}`);
            break;
        case 'itemSet':
            inventory.execute(`inventory.itemsSet(${data}, ${info})`);
            break;
        case 'weaponsSet':
            inventory.execute(`inventory.weaponsSet(${data})`);
            break;
        case 'weaponsItemUpdate':
            inventory.execute(`inventory.weaponsItemUpdate(${info}, ${data})`);
            break;
        case 'outSet':
            inventory.execute(`inventory.outSet(${data})`);
            break;
        case 'tradeSet':
            inventory.execute(`inventory.tradeSet(${data})`);
            break;
        case 'tradeReady':
            inventory.execute(`inventory.tradeReady=${data}`);
            break;
        case 'outside':
            if (info == 5) {
                inventory.execute(`inventory.trade=${data}`);
                global.openOutType = 5;
            } else if (info == 6) {
                inventory.execute(`inventory.outside=${data}`);
                global.openOutType = 6;
            }
            else {
                inventory.execute(`inventory.outside=${data}`);
                global.openOutType = 0;
            }
            break;
        case 'itemUpdate':
            inventory.execute(`inventory.itemUpd(${info}, ${data})`);
            break;
        case 'updateItemFromPlayer':
            inventory.execute(`inventory.updateItemFromPlayer(${info}, ${data})`);
            break;
        case 'updateMoneyFrom':
            inventory.execute(`inventory.moneyFrom=${data}`);
            break;
        case 'updateItemToPlayer':
            inventory.execute(`inventory.updateItemToPlayer(${info}, ${data})`);
            break;
        case 'clothesItemUpdate':
            inventory.execute(`inventory.clothesItemUpdate(${info}, ${data})`);
            break;
        case 'outItemUpdate':
            inventory.execute(`inventory.outItemUpdate(${info}, ${data})`);
            break;
        case 'setTotalWeight':
            inventory.execute(`inventory.totalWeight=${data}`);
            break;
        case 'setTotalWeightOut':
            inventory.execute(`inventory.totalWeightOut=${data}`);
            break;
        case 'closeBoard':
            global.openOutType = -1;
            inventory.execute(`inventory.hideA();`);
            break;
    }
});

mp.events.add('boardCB', (act, type, index, amount) => {
    if(new Date().getTime() - global.lastCheck < 100) return;
    global.lastCheck = new Date().getTime();
    switch(act) {
        case 1:
            mp.events.callRemote('Inventory', type, index, 'use');
            break;
        case 6:
            mp.events.callRemote('Inventory', type, index, 'use_clothes', amount);
            break;
        case 2:
            mp.events.callRemote('Inventory', type, index, 'transfer');
            break;
        case 3:
            mp.events.callRemote('Inventory', type, index, 'take');
            break;
        case 4:
            mp.events.callRemote('Inventory', type, index, 'drop');
            break;
        case 5:
            //mp.events.call('notify', 1, 2, `type ${type} | index ${index} | amount ${amount}`, 5000);
            mp.events.callRemote('Inventory', type, index, 'split', amount);
            break;
    }
});

mp.events.add('inventory_swap', (data) => {
    data = JSON.parse(data);
    let [type, fromIndex, toIndex] = data;
    mp.events.callRemote('Inventory', type, fromIndex, 'swap', toIndex);
});

mp.events.add('inventory_stack', (data) => {
    data = JSON.parse(data);
    let [type, fromIndex, toIndex] = data;
    mp.events.callRemote('Inventory', type, fromIndex, 'stack', toIndex);
});

mp.events.add('inventory_transfer', (data) => {
    data = JSON.parse(data);
    let [type, fromIndex, toIndex] = data;
    mp.events.callRemote('Inventory', type, fromIndex, 'transfer', toIndex);
});

mp.events.add('inventory_take', (data) => {
    data = JSON.parse(data);
    let [type, fromIndex, toIndex] = data;
    mp.events.callRemote('Inventory', type, fromIndex, 'take', toIndex);
});

mp.events.add('inventory_stuff', (data) => {
    data = JSON.parse(data);
    let [fromIndex, toIndex] = data;
    mp.events.callRemote('removeClothes', fromIndex, toIndex);
});

mp.events.add('debug_notify_client', (data) => {
    data = JSON.parse(data);
    mp.game.graphics.notify(`${data}`);
});

mp.events.add('inventory_use_clothes', (data) => {
    data = JSON.parse(data);
    let [fromIndex, toIndex] = data;
    mp.events.callRemote('Inventory', 0, fromIndex, 'use_clothes', toIndex);
});

mp.events.add('inventory_use_weapon', (data) => {
    data = JSON.parse(data);
    let [fromIndex, toIndex] = data;
    mp.events.callRemote('Inventory', 0, fromIndex, 'use_weapon', toIndex);
});

mp.events.add('inventory_remove_weapon', (data) => {
    data = JSON.parse(data);
    let [fromIndex, toIndex] = data;
    mp.events.callRemote('removeWeapon', fromIndex, toIndex);
});

mp.events.add('inventory_change_weapon_slot', (data) => {
    data = JSON.parse(data);
    let [fromIndex, toIndex] = data;
    mp.events.callRemote('changeWeaponSlot', fromIndex, toIndex);
});

mp.events.add('trade_check', (checked, tradeCash) => {
    mp.events.callRemote('tradeCheck', checked, tradeCash);
});

mp.events.add('trade_cancel', () => {
    mp.events.callRemote('tradeCancel');
});

mp.events.add('trade_accept', () => {
    mp.events.callRemote('tradeAccept');
});

// // //
mp.events.add("playerQuit", (player, exitType, reason) => {
    if (inventory !== null) {
        if (player.name === localplayer.name) {
            inventory.destroy();
            inventory = null;
        }
    }
});
