var pokerCam = null;

let betObject = null;
var CasinoPedsssID = [];

mp.game.streaming.requestAnimDict("anim_casino_b@amb@casino@games@threecardpoker@dealer");
mp.game.streaming.requestAnimDict("anim_casino_b@amb@casino@games@threecardpoker@player");

var canDoPBets = false;
var canAns = false;
var cardsAtHand = false;

let pTable = -1;
let pSeat = -1;

var CasinoPedsss = [
    {Hash: 0xBC92BED5, Pos: new mp.Vector3(1145.7991, 261.79266, -51.840824), Angle: -135.13168},
    {Hash: 0xBC92BED5, Pos: new mp.Vector3(1143.9452, 263.70554, -51.840828), Angle: 45.093562},
];

let selfDeck = [];

var TablePos = [
    new mp.Vector3(1146.329, 261.2543, -52.8409),
    new mp.Vector3(1143.338, 264.2453, -52.8409),
];

let animSeat = {
    3: 1,
    2: 2,
    1: 3,
    0: 4,
}

var TableRot = [
    45.0,
    -135.0,
];
var ChairPos = [
    [
        new mp.Vector3(1146.849, 261.9344, -51.8167),
        new mp.Vector3(1146.865, 261.2238, -51.8003),
        new mp.Vector3(1146.325, 260.7546, -51.812),
        new mp.Vector3(1145.63, 260.7765, -51.7979),
    ],
    [
        new mp.Vector3(1142.798, 263.5501, -51.7869),
        new mp.Vector3(1142.82, 264.2595, -51.8004),
        new mp.Vector3(1143.339, 264.7519, -51.8289),
        new mp.Vector3(1144.052, 264.7396, -51.7913),
    ]
];

/*
var CardOffset = [
    new mp.Vector3(0.59535, 0.200875, 1),
    new mp.Vector3(0.51655, 0.2268, 1),
    new mp.Vector3(0.689125, 0.171575, 1),
];*/

var CardOffset = [
    [
        new mp.Vector3(-0.69795-0.03, 0.211525+0.07 , 0.953),
        new mp.Vector3(-0.69795, 0.211525, 0.953),
        new mp.Vector3(-0.69795+0.03, 0.211525-0.07, 0.953),
    ],
    [
        new mp.Vector3(-0.30935-0.07, -0.205675+0.03, 0.953),
        new mp.Vector3(-0.30935, -0.205675, 0.953),
        new mp.Vector3(-0.30935+0.07, -0.205675-0.03, 0.953),
    ],
    [
       
        new mp.Vector3(0.2869-0.07, -0.211925-0.03, 0.953),
        new mp.Vector3( 0.2869, -0.211925, 0.953),
        new mp.Vector3(0.2869+0.07, -0.211925+0.03, 0.953),
    ],
    [
        new mp.Vector3( 0.689125-0.03, 0.171575-0.07, 0.953),
        new mp.Vector3( 0.689125, 0.171575, 0.953),
        new mp.Vector3(0.689125+0.03, 0.171575+ 0.07, 0.953),
    ],
 
];


  // -0.03 -0.07 // 0.03 0.07


  // -0.07 -0.03 // 0.07 0.03

  // 0.07  -0.03 // -0.07 0.03

  // 0.03  -0.07 // -0.03  0.07 


    // -0.139 0.350 // self card 1 - // pp 3

     // -0.28 0.32 // self card 2 - //pp 5

    // 0.17 0.34 // self card 3 - // pp 6
var CardRot = [
   
    -71.64,
    -21.6,
    21.24,
    66.96,
];

var SelfCardRot = [
    21.24,
    0.0,
    -21.6,
];

let cardObjects = [
    [
        [null, null , null],
        [null, null , null],
        [null, null , null],
        [null, null , null],
        [null, null , null],
    ],
    [
        [null, null , null],
        [null, null , null],
        [null, null , null],
        [null, null , null],
        [null, null , null],
    ],
];

var SelfCardsPos = [
    new mp.Vector3(0.2869-0.139, -0.211925+0.350, 0.953),
    new mp.Vector3(0.2869-0.28, -0.211925+0.32, 0.953),
    new mp.Vector3(-0.30935 + 0.17, -0.205675 + 0.34, 0.953),
];

const PokerTablesSeatsHeading = [
    [330.5,65.5, 20.5, 110.5],
    [-330.5,-65.5, -20.5, -110.5],
    
];

var CardsOff = [
    [0, 0],
    [0.02, 0.02],
    [-0.02, 0.02]
];

var CardsName = [
    "vw_prop_cas_card_dia_ace",
    "vw_prop_cas_card_dia_queen",
    "vw_prop_cas_card_dia_10"
];

setTimeout(function () {

    let n = 0;

    CasinoPedsss.forEach(ped => {
        CasinoPedsssID[n] = mp.peds.new(ped.Hash, ped.Pos, ped.Angle, 0);
        CasinoPedsssID[n].setComponentVariation(0, 2, 1, 0);
        CasinoPedsssID[n].setComponentVariation(1, 1, 0, 0);
        CasinoPedsssID[n].setComponentVariation(2, 2, 0, 0);
        CasinoPedsssID[n].setComponentVariation(3, 0, n + 2, 0);
        CasinoPedsssID[n].setComponentVariation(4, 0, 0, 0);
        CasinoPedsssID[n].setComponentVariation(6, 1, 0, 0);
        CasinoPedsssID[n].setComponentVariation(7, 2, 0, 0);
        CasinoPedsssID[n].setComponentVariation(8, 1, 0, 0);
        CasinoPedsssID[n].setComponentVariation(10, 1, 0, 0);
        CasinoPedsssID[n].setComponentVariation(11, 1, 0, 0);
        CasinoPedsssID[n].setConfigFlag(185, true);
        CasinoPedsssID[n].setConfigFlag(108, true);
        CasinoPedsssID[n].setConfigFlag(208, true);
        CasinoPedsssID[n].taskPlayAnim("anim_casino_b@amb@casino@games@shared@dealer@", "idle", 1000.0, -2.0, -1, 2, 1148846080, false, false, false);
        n = n + 1;
        //CasinoPedssID[0].playFacialAnim("idle_facial", "anim_casino_b@amb@casino@games@shared@dealer@");
        //mp.game.invoke("0xEA47FE3719165B94", CasinoPedssID[0].handle, "anim_casino_b@amb@casino@games@shared@dealer@", "idle", 1000.0, -2.0, -1, 2, 1148846080, false, false, false)
    });

    n = 0;

    TablePos.forEach(table => {
        selfDeck[n] = mp.objects.new(mp.game.joaat(`vw_prop_casino_cards_01`), mp.game.ped.getAnimInitialOffsetPosition("anim_casino_b@amb@casino@games@threecardpoker@dealer", "deck_pick_up_deck", table.x, table.y, table.z, 0, 0, TableRot[n], 0.01, 2),
        {
            rotation: mp.game.ped.getAnimInitialOffsetRotation("anim_casino_b@amb@casino@games@threecardpoker@dealer", "deck_pick_up_deck", table.x, table.y, table.z, 0, 0, TableRot[n], 0.01, 2),
            alpha: 255,
            dimension: 0,
        });
        n = n + 1;
    });

    /*
    let selfDeck = mp.objects.new(mp.game.joaat(`vw_prop_casino_cards_01`), mp.game.ped.getAnimInitialOffsetPosition("anim_casino_b@amb@casino@games@threecardpoker@dealer", "deck_pick_up_deck", tablePos.x, tablePos.y, tablePos.z, 0, 0, 45, 0.01, 2),
    {
        rotation: mp.game.ped.getAnimInitialOffsetRotation("anim_casino_b@amb@casino@games@threecardpoker@dealer", "deck_pick_up_deck", tablePos.x, tablePos.y, tablePos.z, 0, 0, 45, 0.01, 2),
        alpha: 255,
        dimension: 0,
    });*/
    
}, 10000);
/*
mp.events.add('poker_dealCard', function(num){
    CasinoPedsssID[0].taskPlayAnim("anim_casino_b@amb@casino@games@threecardpoker@dealer", "female_deck_deal_p0" + num, 3.0, 1.0, -1, 2, 0, false, false, false);


});*/


mp.events.add('poker_camera', function(table){
    pokerCam = mp.cameras.new('default', new mp.Vector3(TablePos[table].x, TablePos[table].y, TablePos[table].z+1.5), new mp.Vector3(0,45,0), 50);
    pokerCam.setRot(0.0, TableRot[table], 0.0, 2);
    pokerCam.setCoord(TablePos[table].x, TablePos[table].y, TablePos[table].z+2);
    pokerCam.pointAtCoord(TablePos[table].x, TablePos[table].y, TablePos[table].z);
  
    pokerCam.setActive(true);
    mp.game.cam.renderScriptCams(true, false, 0, true, false);

    setTimeout(() => {
        pokerCam.setActive(false);
        pokerCam.destroy(true);
        mp.game.cam.renderScriptCams(false, false, 1500, true, false);
    }, 7000);
});

mp.events.add('seat_to_poker_table', function( table, seat, player){
    canDoPBets = true;

    pSeat = seat;
    pTable = table;
    try {
    let objj = mp.objects.new(mp.game.joaat("vw_prop_casino_3cardpoker_01b"), new mp.Vector3(TablePos[table].x, TablePos[table].y, TablePos[table].z),
            {
                rotation: new mp.Vector3(0,0,TableRot[table]),
                alpha: 0,
                dimension: 0,
            });


    setTimeout(() => {
        mp.gui.chat.push(objj);
        let ind = objj.getBoneIndexByName(`Chair_Base_0${seat + 1}`);
    
        let pos = objj.getWorldPositionOfBone(ind);
    
       // mp.players.local.position = new mp.Vector3(pos.x, pos.y, pos.z);
    
        // obj = mp.game.object.getClosestObjectOfType(tablePos.x, tablePos.y, tablePos.z, 1, mp.game.joaat("vw_prop_casino_3cardpoker_01"), false, false, false);
    //
        //let ind = obj.getBoneIndexByName("Chair_Base_01");
    
      
    // let rotation = mp.game.invoke("0xCE6294A232D03786", objj.handle, ind);

    // mp.gui.chat.push(`${rotation}`);

        objj.destroy();

         player.position = new mp.Vector3(pos.x, pos.y, pos.z);
         player.setRotation(0, 0,  PokerTablesSeatsHeading[table][seat], 1 ,false);

         player.taskPlayAnim("anim_casino_b@amb@casino@games@shared@player@", "idle_cardgames", 1000.0, -2.0, -1, 2, 1148846080, false, false, false);
       
        
    }, 260);
   // let obj = mp.game.object.getClosestObjectOfType(tablePos.x, tablePos.y, tablePos.z, 2, mp.game.joaat("vw_prop_casino_3cardpoker_01b"), false, true, true);


  
    }
    catch(ex){
        mp.gui.chat.push(`${ex}`);
    }
});

mp.events.add('poker_bet', function(player){
    if(cardsAtHand && player == mp.players.local){
        cardObjects[pTable][pSeat][0].detach(true, true);
        cardObjects[pTable][pSeat][1].detach(true, true);
        cardObjects[pTable][pSeat][2].detach(true, true);

        for(let i = 0; i < 3; i++){
            let ps = mp.game.object.getObjectOffsetFromCoords(TablePos[pTable].x, TablePos[pTable].y, TablePos[pTable].z, TableRot[pTable], CardOffset[pSeat][i].x, CardOffset[pSeat][i].y, CardOffset[pSeat][i].z);
            cardObjects[pTable][pSeat][i].setCoordsNoOffset(ps.x, ps.y, ps.z, false, false ,true);
            cardObjects[pTable][pSeat][i].setRotation(180, 0, TableRot[pTable] + CardRot[pSeat], 2, true);
            cardObjects[pTable][pSeat][i].setVisible(true, false);
        }

        mp.players.local.taskPlayAnim("anim_casino_b@amb@casino@games@threecardpoker@player", "cards_fold", 1000.0, -2.0, -1, 2, 1148846080, false, false, false);

        cardsAtHand = false;

        setTimeout(() => {
            player.taskPlayAnim("anim_casino_b@amb@casino@games@blackjack@player", "place_bet_small", 3.0, 1.0, -1, 2, 0, false, false, false);
        }, 100);
    }
    else {

        setTimeout(() => {
            let ps = mp.game.object.getObjectOffsetFromCoords(TablePos[pTable].x, TablePos[pTable].y, TablePos[pTable].z, TableRot[pTable], CardOffset[pSeat][1].x, CardOffset[pSeat][1].y, CardOffset[pSeat][1].z);

            betObject = mp.objects.new(mp.game.joaat(`vw_prop_chip_100dollar_x1`), new mp.Vector3(ps.x, ps.y, ps.z),
            {
                rotation: new mp.Vector3(0,0,0),
                alpha: 255,
                dimension: 0,
            });
    
        }, 500);
       
        player.taskPlayAnim("anim_casino_b@amb@casino@games@blackjack@player", "place_bet_small", 3.0, 1.0, -1, 2, 0, false, false, false);
    }

});

mp.events.add('poker_decline', function(player){
    if(cardsAtHand && player == mp.players.local){
        cardObjects[pTable][pSeat][0].detach(true, true);
        cardObjects[pTable][pSeat][1].detach(true, true);
        cardObjects[pTable][pSeat][2].detach(true, true);

        for(let i = 0; i < 3; i++){
            let ps = mp.game.object.getObjectOffsetFromCoords(TablePos[pTable].x, TablePos[pTable].y, TablePos[pTable].z, TableRot[pTable], CardOffset[pSeat][i].x, CardOffset[pSeat][i].y, CardOffset[pSeat][i].z);
            cardObjects[pTable][pSeat][i].setCoordsNoOffset(ps.x, ps.y, ps.z, false, false ,true);
            cardObjects[pTable][pSeat][i].setRotation(180, 0, TableRot[pTable] + CardRot[pSeat], 2, true);
            cardObjects[pTable][pSeat][i].setVisible(true, false);
            }

        mp.players.local.taskPlayAnim("anim_casino_b@amb@casino@games@threecardpoker@player", "cards_fold", 1000.0, -2.0, -1, 2, 1148846080, false, false, false);

        cardsAtHand = false;

    }

    player.taskPlayAnim("anim_casino_b@amb@casino@games@blackjack@player", "decline_card_001", 3.0, 1.0, -1, 2, 0, false, false, false);
});

mp.events.add('poker_shuffleDeck', function(table){
    CasinoPedsssID[table].taskPlayAnim("anim_casino_b@amb@casino@games@threecardpoker@dealer", "female_deck_pick_up", 3.0, 1.0, -1, 2, 0, false, false, false);

    selfDeck[table].attachTo(CasinoPedsssID[table].handle, CasinoPedsssID[table].getBoneIndex(60309), 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, false, false, false, true, 2, true);

    setTimeout(() => {
        CasinoPedsssID[table].taskPlayAnim("anim_casino_b@amb@casino@games@threecardpoker@dealer", "female_deck_shuffle", 3.0, 1.0, -1, 2, 0, false, false, false);
    }, 1000);

});

mp.events.add('exit_poker_table', function(player){
    pSeat = -1;
    pTable = -1;
    canDoPBets = false;
    player.taskPlayAnim("anim_casino_b@amb@casino@games@shared@player@", "sit_exit_left", 3.0, 1.0, 2500, 2, 0, false, false, false);
});

mp.events.add('poker_dealCards', function(table, seat, cards){

    if(seat == -1){
        CasinoPedsssID[table].taskPlayAnim("anim_casino_b@amb@casino@games@threecardpoker@dealer", `female_deck_deal_self`, 3.0, 1.0, -1, 2, 0, false, false, false);

        setTimeout(() => {
            for(let i = 0; i < 3; i++){
                let ps = mp.game.object.getObjectOffsetFromCoords(TablePos[table].x, TablePos[table].y, TablePos[table].z, TableRot[table], SelfCardsPos[i].x, SelfCardsPos[i].y, SelfCardsPos[i].z);
    
                cardObjects[table][4][i] = mp.objects.new(mp.game.joaat(`vw_prop_cas_card_dia_ace`), ps,
                {
                    rotation: new mp.Vector3(180, 0, TableRot[table] + SelfCardRot[i]),
                    alpha: 255,
                    dimension: 0,
                });
            }
            
        }, 1500);
        setTimeout(() => {
            CasinoPedsssID[table].taskPlayAnim("anim_casino_b@amb@casino@games@threecardpoker@dealer", "female_deck_put_down", 3.0, 1.0, -1, 2, 0, false, false, false);
        }, 2000);

        setTimeout(() => {
            selfDeck[table].detach(false, true);
        }, 3000);
    }
    else{
   
        
        cards = JSON.parse(cards);
        for(let i = 0; i < 3; i++){
            let ps = mp.game.object.getObjectOffsetFromCoords(TablePos[table].x, TablePos[table].y, TablePos[table].z, TableRot[table], CardOffset[seat][i].x, CardOffset[seat][i].y, CardOffset[seat][i].z);

            cardObjects[table][seat][i] = mp.objects.new(mp.game.joaat(`vw_prop_cas_card_${cards[i]}`), new mp.Vector3(ps.x , ps.y, ps.z - 1),
            {
                    rotation: new mp.Vector3(0,0,0),
                    alpha: 255,
                    dimension: 0,
            });

            cardObjects[table][seat][i].setVisible(false, false);
        }

        CasinoPedsssID[table].taskPlayAnim("anim_casino_b@amb@casino@games@threecardpoker@dealer", `female_deck_deal_p0${animSeat[seat]}`, 3.0, 1.0, -1, 2, 0, false, false, false);
        setTimeout(() => {
            for(let i = 0; i < 3; i++){
                let ps = mp.game.object.getObjectOffsetFromCoords(TablePos[table].x, TablePos[table].y, TablePos[table].z, TableRot[table], CardOffset[seat][i].x, CardOffset[seat][i].y, CardOffset[seat][i].z);
                cardObjects[table][seat][i].setCoordsNoOffset(ps.x, ps.y, ps.z, false, false ,true);
                cardObjects[table][seat][i].setRotation(180, 0, TableRot[table] + CardRot[seat], 2, true);
                cardObjects[table][seat][i].setVisible(true, false);
                }
        }, 1000);


    }
});


mp.events.add('poker_collectCard', function(table, seat){

    if(betObject != null){
        betObject.destroy();
        betObject = null;
    }
    selfDeck[table].attachTo(CasinoPedsssID[table].handle, CasinoPedsssID[table].getBoneIndex(60309), 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, false, false, false, true, 2, true);


    CasinoPedsssID[table].taskPlayAnim("anim_casino_b@amb@casino@games@threecardpoker@dealer", "female_deck_pick_up", 3.0, 1.0, -1, 2, 0, false, false, false);


    if(seat != -1){
    setTimeout(() => {
        seat+=1;
        CasinoPedsssID[table].taskPlayAnim("anim_casino_b@amb@casino@games@threecardpoker@dealer", `female_cards_collect_p0${animSeat[seat]}`, 3.0, 1.0, -1, 2, 0, false, false, false);
        seat-=1;
    }, 1000);

    setTimeout(() => {
        for(let i = 0; i < 3; i++){
            if(cardObjects[table][seat][i] != null){
                cardObjects[table][seat][i].destroy();
                cardObjects[table][seat][i] = null;
            }
        }
    }, 2000);

    setTimeout(() => {
        CasinoPedsssID[table].taskPlayAnim("anim_casino_b@amb@casino@games@threecardpoker@dealer", "female_deck_put_down", 3.0, 1.0, -1, 2, 0, false, false, false);
    }, 2500);

    setTimeout(() => {
        selfDeck[table].detach(false, true);
    }, 3500);

    }
    else {
        CasinoPedsssID[table].taskPlayAnim("anim_casino_b@amb@casino@games@threecardpoker@dealer", "female_cards_collect_self", 3.0, 1.0, -1, 2, 0, false, false, false);

        setTimeout(() => {
        
            for(let i = 0; i < 3; i++){
                if(cardObjects[table][4][i] != null){
                    cardObjects[table][4][i].destroy();
                    cardObjects[table][4][i] = null;
                }
            }
        }, 1000);

        setTimeout(() => {
            CasinoPedsssID[table].taskPlayAnim("anim_casino_b@amb@casino@games@threecardpoker@dealer", "female_deck_put_down", 3.0, 1.0, -1, 2, 0, false, false, false);
        }, 2000);
    
    }
    //selfDeck.attachTo(CasinoPedsssID[0].handle, CasinoPedsssID[0].getBoneIndex(60309), 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, false, false, false, true, 2, true);

  
});

mp.events.add('pokerAnswer', function(toggle){
    canAns = toggle;
});


mp.events.add('revealSelf', function(table, card){
    CasinoPedsssID[table].taskPlayAnim("anim_casino_b@amb@casino@games@threecardpoker@dealer", "reveal_self", 3.0, 1.0, -1, 2, 0, false, false, false);

    let cards = JSON.parse(card);
    setTimeout(() => {
        for(let i = 0; i < 3; i++){
          
            cardObjects[table][4][i].destroy();

            let ps = mp.game.object.getObjectOffsetFromCoords(TablePos[table].x, TablePos[table].y, TablePos[table].z, TableRot[table], SelfCardsPos[i].x, SelfCardsPos[i].y, SelfCardsPos[i].z);
    
            cardObjects[table][4][i] = mp.objects.new(mp.game.joaat(`vw_prop_cas_card_${cards[i]}`), ps,
            {
                rotation: new mp.Vector3(0, 0, TableRot[table] + SelfCardRot[i]),
                alpha: 255,
                dimension: 0,
            });
        }
        
    }, 2000);
});

mp.events.add('revealPlayer', function(table, seat){
    CasinoPedsssID[table].taskPlayAnim("anim_casino_b@amb@casino@games@threecardpoker@dealer", `female_reveal_blind_p0${animSeat[seat]}`, 3.0, 1.0, -1, 2, 0, false, false, false);

    setTimeout(() => {
        for(let i = 0; i < 3; i++){
          
            cardObjects[table][seat][i].setRotation(0.0, 0.0,  TableRot[table] + CardRot[seat],2, true);
        }
        
    }, 1200);
});



mp.events.add('pocker_addCardHand', function () {

    cardsAtHand = true;

    mp.players.local.taskPlayAnim("anim_casino_b@amb@casino@games@threecardpoker@player", "cards_pickup", 1000.0, -2.0, -1, 2, 1148846080, false, false, false);

    setTimeout(() => {
        cardObjects[pTable][pSeat][0].attachTo(mp.players.local.handle, mp.players.local.getBoneIndex(60309), 0.09, 0.08, -0.05, -31, -89, -54, false, false, false, true, 2, false);
        cardObjects[pTable][pSeat][1].attachTo(mp.players.local.handle, mp.players.local.getBoneIndex(60309), 0.109, 0.09, 0, -17, -95, -59, false, false, false, true, 2, false);
        cardObjects[pTable][pSeat][2].attachTo(mp.players.local.handle, mp.players.local.getBoneIndex(60309), 0.119, 0.09, 0.06, -8, -94, -57, false, false, false, true, 2, false);
    }, 500);
  
  
});

mp.keys.bind(Keys.VK_UP, false, function () { // стрелка вверх
    if (!loggedin || chatActive || editing || global.menuCheck() || cuffed || localplayer.getVariable('InDeath') == true || new Date().getTime() - lastCheck < 400) return;
    if(canAns && !cardsAtHand){
       mp.events.call('pocker_addCardHand');
        return;
    }
    if(canDoPBets){
        mp.events.callRemote('pokerBetUp');
    }
    
});
mp.keys.bind(Keys.VK_DOWN, false, function () { // стрелка вниз
    if (!loggedin || chatActive || editing || global.menuCheck() || cuffed || localplayer.getVariable('InDeath') == true || new Date().getTime() - lastCheck < 400) return;
    if(canDoPBets){
        mp.events.callRemote('pokerBetDown');
    }
});

mp.keys.bind(Keys.VK_RETURN, false, function () { // enter
    if (!loggedin || chatActive || editing || global.menuCheck() || cuffed || localplayer.getVariable('InDeath') == true || new Date().getTime() - lastCheck < 400) return;
    if(canAns){
        mp.events.callRemote('pokerStand');
        return;
    }
    if(canDoPBets){
        mp.events.callRemote('pokerSetBet');
    }
    
});

mp.keys.bind(Keys.VK_SPACE, false, function () { // space
    if (!loggedin || chatActive || editing || global.menuCheck() || cuffed || localplayer.getVariable('InDeath') == true || new Date().getTime() - lastCheck < 400) return;
    if(canAns){
       
            mp.events.callRemote('pokerHit');
      

    }
    
});



// first 0.09 0.08 -0.05
// -31 -89 - 54

// second 0.109 0.09 0
// -17 -95 -59

// th 0.119 0.09 0.06
// -8 -94 -57
