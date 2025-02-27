mp.game.streaming.requestModel(mp.game.joaat("poker_table"));

setTimeout(() => {
    mp.objects.new(mp.game.joaat("poker_table"), new mp.Vector3(1133.6095, 261.95905, -51.5409),
    {
        rotation: new mp.Vector3(0,0,110),
        alpha: 255,
        dimension: 0,
    });
}, 5000);


var pokerCamera = null;
let pockerTable = -1;

global.tPocker = false;

let tablePos = [
    new mp.Vector3(1133.7095, 261.65905, -51.5409),
];

let tableRot = [
    110
];

let tableMainCardsOffset = [
    [0.305, 0.18, 1.65],
    [0.305, 0.09, 1.65],
    [0.305, 0.005, 1.65],
    [0.305, -0.083, 1.65],
    [0.305, -0.17, 1.65]
];

let tablePlayersCardsOffset = [
    [
        [0.18, 0.45, 1.65],
        [0.18, 0.37, 1.65],
    ],
    [
        [0.45, 0.45, 1.65],
        [0.45, 0.37, 1.65],
    ],
    [
        [0.5, 0.04, 1.65],
        [0.5, -0.04, 1.65],
    ],
    [
        [0.45, -0.45, 1.65],
        [0.45, -0.37, 1.65],
    ],
    [
        [0.18, -0.45, 1.65],
        [0.18, -0.37, 1.65],
    ]
];


var playersCards = 
[
    [null, null],
    [null, null],
    [null, null],
    [null, null],
    [null, null],
];

var tableCards = [null, null, null, null, null];


mp.events.add('addPlayerCard', function(pNum){
    try {
    let p11 = mp.game.object.getObjectOffsetFromCoords(tablePos[pockerTable].x, tablePos[pockerTable].y, tablePos[pockerTable].z, tableRot[pockerTable], tablePlayersCardsOffset[pNum][0][0], tablePlayersCardsOffset[pNum][0][1], tablePlayersCardsOffset[pNum][0][2]);
    let p12 = mp.game.object.getObjectOffsetFromCoords(tablePos[pockerTable].x, tablePos[pockerTable].y, tablePos[pockerTable].z, tableRot[pockerTable], tablePlayersCardsOffset[pNum][1][0], tablePlayersCardsOffset[pNum][1][1], tablePlayersCardsOffset[pNum][1][2]);

    playersCards[pNum][0] = mp.objects.new(mp.game.joaat("vw_prop_cas_card_dia_10"), new mp.Vector3(p11.x, p11.y, p11.z),
    {
        rotation: new mp.Vector3(180,0,tableRot[pockerTable] + 90),
        alpha: 255,
        dimension: 0,
    });

    playersCards[pNum][1] = mp.objects.new(mp.game.joaat("vw_prop_cas_card_dia_10"), new mp.Vector3(p12.x, p12.y, p12.z),
    {
        rotation: new mp.Vector3(180,0,tableRot[pockerTable] + 90),
        alpha: 255,
        dimension: 0,
    });
    }
    catch{}
});


mp.events.add('openPlayerCard', function(pNum, card1, card2){
    try{
    playersCards[pNum][0].destroy();
    playersCards[pNum][1].destroy();

    let p11 = mp.game.object.getObjectOffsetFromCoords(tablePos[pockerTable].x, tablePos[pockerTable].y, tablePos[pockerTable].z, tableRot[pockerTable], tablePlayersCardsOffset[pNum][0][0], tablePlayersCardsOffset[pNum][0][1], tablePlayersCardsOffset[pNum][0][2]);
    let p12 = mp.game.object.getObjectOffsetFromCoords(tablePos[pockerTable].x, tablePos[pockerTable].y, tablePos[pockerTable].z, tableRot[pockerTable], tablePlayersCardsOffset[pNum][1][0], tablePlayersCardsOffset[pNum][1][1], tablePlayersCardsOffset[pNum][1][2]);

    playersCards[pNum][0] = mp.objects.new(mp.game.joaat(`vw_prop_cas_card_${card1}`), new mp.Vector3(p11.x, p11.y, p11.z),
    {
        rotation: new mp.Vector3(0,0,tableRot[pockerTable] + 90),
        alpha: 255,
        dimension: 0,
    });

    playersCards[pNum][1] = mp.objects.new(mp.game.joaat(`vw_prop_cas_card_${card2}`), new mp.Vector3(p12.x, p12.y, p12.z),
    {
        rotation: new mp.Vector3(0,0,tableRot[pockerTable] + 90),
        alpha: 255,
        dimension: 0,
    });
    }
    catch{}
});

mp.events.add('showWinnerCards', function(num, pCards, tCards){
    try {
    var pl = JSON.parse(pCards);
    var tb = JSON.parse(tCards);


    pl.forEach(card => {
        let p = mp.game.object.getObjectOffsetFromCoords(tablePos[pockerTable].x, tablePos[pockerTable].y, tablePos[pockerTable].z, tableRot[pockerTable], tablePlayersCardsOffset[num][card][0], tablePlayersCardsOffset[num][card][1], tablePlayersCardsOffset[num][card][2] + 0.5);
        playersCards[num][card].slide(p.x, p.y, p.z, 0.25, 0.25, 0.25, false);
    });
  
    tb.forEach(card => {
        let p = mp.game.object.getObjectOffsetFromCoords(tablePos[pockerTable].x, tablePos[pockerTable].y, tablePos[pockerTable].z, tableRot[pockerTable], tableMainCardsOffset[card][0], tableMainCardsOffset[card][1], tableMainCardsOffset[card][2] + 0.5);
        tableCards[card].slide(p.x, p.y, p.z, 0.25, 0.25, 0.25, false);
    });
    }
    catch{}

    global.menu.execute(`client_playMusic('./sound/poker_win.mp3', 0.7);`);

});

mp.events.add('pockerBetSound', function(){

    global.menu.execute(`client_playMusic('./sound/poker_bet.mp3', 0.7);`);
});

mp.events.add('cleanPlayerCards', function(pNum){
    try {
        playersCards[pNum][0].destroy();
        playersCards[pNum][1].destroy();

        playersCards[pNum][0] = null;
        playersCards[pNum][1] = null;
    }
    catch{}

    
});

mp.events.add('addTableCards', function(cards){
    try {
    var arr = JSON.parse(cards);

    let cf0 = mp.game.object.getObjectOffsetFromCoords(tablePos[pockerTable].x, tablePos[pockerTable].y, tablePos[pockerTable].z, tableRot[pockerTable], tableMainCardsOffset[0][0], tableMainCardsOffset[0][1], tableMainCardsOffset[0][2]);
    let cf1 = mp.game.object.getObjectOffsetFromCoords(tablePos[pockerTable].x, tablePos[pockerTable].y, tablePos[pockerTable].z, tableRot[pockerTable], tableMainCardsOffset[1][0], tableMainCardsOffset[1][1], tableMainCardsOffset[1][2]);
    let cf2 = mp.game.object.getObjectOffsetFromCoords(tablePos[pockerTable].x, tablePos[pockerTable].y, tablePos[pockerTable].z, tableRot[pockerTable], tableMainCardsOffset[2][0], tableMainCardsOffset[2][1], tableMainCardsOffset[2][2]);

    tableCards[0] = mp.objects.new(mp.game.joaat(`vw_prop_cas_card_${arr[0]}`), new mp.Vector3(cf0.x, cf0.y, cf0.z),
    {
        rotation: new mp.Vector3(0,0,tableRot[pockerTable] + 90),
        alpha: 255,
        dimension: 0,
    });

    tableCards[1] = mp.objects.new(mp.game.joaat(`vw_prop_cas_card_${arr[1]}`), new mp.Vector3(cf1.x, cf1.y, cf1.z),
    {
        rotation: new mp.Vector3(0,0,tableRot[pockerTable] + 90),
        alpha: 255,
        dimension: 0,
    });

    tableCards[2] = mp.objects.new(mp.game.joaat(`vw_prop_cas_card_${arr[2]}`), new mp.Vector3(cf2.x, cf2.y, cf2.z),
    {
        rotation: new mp.Vector3(0,0,tableRot[pockerTable] + 90),
        alpha: 255,
        dimension: 0,
    });
    }
    catch{}

    global.menu.execute(`client_playMusic('./sound/poker_deal.mp3', 0.9);`);
});

mp.events.add('addTableCard', function(num, card){
   
    try {
    let cf = mp.game.object.getObjectOffsetFromCoords(tablePos[pockerTable].x, tablePos[pockerTable].y, tablePos[pockerTable].z, tableRot[pockerTable], tableMainCardsOffset[num][0], tableMainCardsOffset[num][1], tableMainCardsOffset[num][2]);

    tableCards[num] = mp.objects.new(mp.game.joaat(`vw_prop_cas_card_${card}`), new mp.Vector3(cf.x, cf.y, cf.z),
    {
        rotation: new mp.Vector3(0,0,tableRot[pockerTable] + 90),
        alpha: 255,
        dimension: 0,
    });
    }
    catch{}

    global.menu.execute(`client_playMusic('./sound/poker_deal.mp3', 0.9);`);
});

mp.events.add('clearCards', function(){
    global.menu.execute(`client_playMusic('./sound/poker_deal.mp3', 0.9);`);
   try {
  for(let i = 0; i < tableCards.length; i++){
      if(tableCards[i] != null){
        tableCards[i].destroy();
        tableCards[i] = null;
      }
  }

    for(let i = 0; i < playersCards.length; i++){
        for(let j = 0; j < playersCards[i].length; j++){
            if(playersCards[i][j] != null){
                playersCards[i][j].destroy();
                playersCards[i][j] = null;
              }
        }
    }
}
catch{}
});


mp.events.add('poker_SetChips', function(chips){
    global.menu.execute(`poker.chips = ${chips}`);
});

mp.events.add('poker_SetTableBets', function(minBet, maxBet){
    global.menu.execute(`poker.tableBet = ${minBet}`);
    global.menu.execute(`poker.maxBet = ${maxBet}`);
});

mp.events.add('openPokerTable', function(table, chips){

    for(let i = 0; i < tableCards.length; i++){
        if(tableCards[i] != null){
          tableCards[i].destroy();
          tableCards[i] = null;
        }
    }
  
      for(let i = 0; i < playersCards.length; i++){
          for(let j = 0; j < playersCards[i].length; j++){
              if(playersCards[i][j] != null){
                  playersCards[i][j].destroy();
                  playersCards[i][j] = null;
                }
          }
      }
    mp.players.local.freezePosition(true);

    pockerTable = table;

    global.tPocker = true;

    let cp = mp.game.object.getObjectOffsetFromCoords(tablePos[pockerTable].x, tablePos[pockerTable].y, tablePos[pockerTable].z, tableRot[pockerTable], 0.3, 0, 0);

    let ps = mp.game.object.getObjectOffsetFromCoords(cp.x, cp.y, cp.z, tableRot[pockerTable], 0.01, 0, 0);

    pokerCamera = mp.cameras.new('default', new mp.Vector3(cp.x, cp.y, cp.z + 2.5), new mp.Vector3(0,0,0), 60);
    pokerCamera.pointAtCoord(ps.x, ps.y, ps.z);
    pokerCamera.setActive(true);
    mp.game.cam.renderScriptCams(true, false, 0, true, false);

    global.casinoOpen();
    mp.gui.cursor.visible = false;
    global.menu.execute(`poker.active = true;`);

    mp.game.ui.displayAreaName(false);
    mp.game.ui.displayRadar(false);
    mp.game.ui.displayHud(false);

});

mp.events.add('pocker_setPlayers', function(data, spot){
    global.menu.execute(`poker.setPlayers('${data}')`);
});

mp.events.add('pocker_showPanel', function(){
    mp.gui.cursor.visible = true;
    global.menu.execute(`poker.showPanel()`);
});

mp.events.add('pocker_TimerSet', function(toggle, time){
    global.menu.execute(`poker.timerOn = ${toggle}`);
    global.menu.execute(`poker.timer = '${time}'`);
});


mp.events.add('exitPokerUi', function(){
    mp.events.callRemote('interactionPressed', 1);
});

mp.events.add('exitPokerTable', function(){

    mp.game.ui.displayAreaName(true);
    mp.game.ui.displayRadar(true);
    mp.game.ui.displayHud(true);

    mp.players.local.freezePosition(false);

    global.tPocker = false;

    global.menu.execute(`poker.hidePanel()`);
    global.menu.execute(`poker.active = false;`);

    global.casinoClose();

    for(let i = 0; i < tableCards.length; i++){
        if(tableCards[i] != null){
          tableCards[i].destroy();
          tableCards[i] = null;
        }
    }
  
      for(let i = 0; i < playersCards.length; i++){
          for(let j = 0; j < playersCards[i].length; j++){
              if(playersCards[i][j] != null){
                  playersCards[i][j].destroy();
                  playersCards[i][j] = null;
                }
          }
      }

      pokerCamera.setActive(false);
      pokerCamera.destroy(true);
      mp.game.cam.renderScriptCams(false, false, 1500, true, false);

      pockerTable = -1;
});

mp.events.add('pocker_hidePanel', function(){
    global.menu.execute(`poker.hidePanel()`);
});

mp.events.add('pocker_setMySeat', function(seat){
    global.menu.execute(`poker.setMySeat(${seat})`);
});

mp.events.add('pocker_setButtons', function(b1, b2){

    var res = [b1.toString(), b2.toString()];

    global.menu.execute(`poker.setButtons('${JSON.stringify(res)}')`);
});




mp.events.add('pockerAction', function(action, count = 0){
    mp.events.callRemote("pockerAction", action, parseInt(count));
});