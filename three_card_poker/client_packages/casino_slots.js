
var reels = [];
var data = {};
var d = null;

var a = Math.floor;

let started = false;

const SLOTS_MACHINE = [
    {
        model: "vw_prop_casino_slot_04a",
        position: new mp.Vector3(1100.483, 230.4082, -50.8409),
        rot: 45.0,
    },
    {
        model: "vw_prop_casino_slot_05a",
        position: new mp.Vector3(1100.939, 231.0017, -50.8409),
        rot: 60.0,
    },
    {
        model: "vw_prop_casino_slot_06a",
        position: new mp.Vector3(1101.221, 231.6943, -50.8409),
        rot: 75.0,
    },
    {
        model: "vw_prop_casino_slot_07a",
        position: new mp.Vector3(1101.323, 232.4321, -50.8409),
        rot: 90.0,
    },
    {
        model: "vw_prop_casino_slot_08a",
        position: new mp.Vector3(1101.229, 233.1719, -50.8409),
        rot: 105.0,
    },
    {
        model: "vw_prop_casino_slot_01a",
        position: new mp.Vector3(1108.938, 239.4797, -50.8409),
        rot: -45.0,
    },
    {
        model: "vw_prop_casino_slot_02a",
        position: new mp.Vector3(1109.536, 239.0278, -50.8409),
        rot: -30.0,
    },
    {
        model: "vw_prop_casino_slot_03a",
        position: new mp.Vector3(1110.229, 238.7428, -50.8409),
        rot: -15.0,
    },
    {
        model: "vw_prop_casino_slot_04a",
        position: new mp.Vector3(1110.974, 238.642, -50.8409),
        rot: 0.0,
    },
    {
        model: "vw_prop_casino_slot_05a",
        position: new mp.Vector3(1111.716, 238.7384, -50.8409),
        rot: 15.0,
    },
    {
        model: "vw_prop_casino_slot_06a",
        position: new mp.Vector3(1112.407, 239.0216, -50.8409 ),
        rot: 30.0,
    },
    {
        model: "vw_prop_casino_slot_07a",
        position: new mp.Vector3( 1112.999, 239.4742, -50.8409 ),
        rot: 45.0,
    },
    {
        model: "vw_prop_casino_slot_01a",
        position: new mp.Vector3( 1120.853, 233.1621, -50.8409 ),
        rot: -105.0,
    },
    {
        model: "vw_prop_casino_slot_02a",
        position: new mp.Vector3( 1120.753, 232.4272, -50.8409 ),
        rot: -90.0,
    },
    {
        model: "vw_prop_casino_slot_03a",
        position: new mp.Vector3( 1120.853, 231.6886, -50.8409 ),
        rot: -75.0,
    },
    {
        model: "vw_prop_casino_slot_04a",
        position: new mp.Vector3( 1121.135, 230.9999, -50.8409 ),
        rot: -60.0,
    },
    {
        model: "vw_prop_casino_slot_05a",
        position: new mp.Vector3( 1121.592, 230.4106, -50.8409 ),
        rot: -45.0,
    },
    {
        model: "vw_prop_casino_slot_04a",
        position: new mp.Vector3( 1104.572, 229.4451, -50.8409 ),
        rot: -36.0,
    },
    {
        model: "vw_prop_casino_slot_05a",
        position: new mp.Vector3( 1104.302, 230.3183, -50.8409 ),
        rot: -108.0,
    },
    {
        model: "vw_prop_casino_slot_01a",
        position: new mp.Vector3(  1105.049, 230.845, -50.8409 ),
        rot: 180.0,
    },
    {
        model: "vw_prop_casino_slot_02a",
        position: new mp.Vector3(  1105.781, 230.2973, -50.8409 ),
        rot: 108.0,
    },
    {
        model: "vw_prop_casino_slot_03a",
        position: new mp.Vector3( 1105.486, 229.4322, -50.8409),
        rot: 36.0,
    },
    {
        model: "vw_prop_casino_slot_07a",
        position: new mp.Vector3( 1108.005, 233.9177, -50.8409),
        rot: -36.0,
    },
    {
        model: "vw_prop_casino_slot_08a",
        position: new mp.Vector3(  1107.735, 234.7909, -50.8409),
        rot: -108.0,
    },
    {
        model: "vw_prop_casino_slot_04a",
        position: new mp.Vector3(1108.482, 235.3176, -50.8409),
        rot: -80.0,
    },
    {
        model: "vw_prop_casino_slot_05a",
        position: new mp.Vector3(1109.214, 234.7699, -50.8409),
        rot: 108.0,
    },
    {
        model: "vw_prop_casino_slot_06a",
        position: new mp.Vector3( 1108.919, 233.9048, -50.8409),
        rot: 36.0,
    },
    {
        model: "vw_prop_casino_slot_04a",
        position: new mp.Vector3( 1113.64, 233.6755, -50.8409),
        rot: -36.0,
    },
    {
        model: "vw_prop_casino_slot_05a",
        position: new mp.Vector3(1113.37, 234.5486, -50.8409),
        rot: -108.0,
    },
    {
        model: "vw_prop_casino_slot_01a",
        position: new mp.Vector3(1114.117, 235.0753, -50.8409),
        rot: 180.0,
    },
    {
        model: "vw_prop_casino_slot_02a",
        position: new mp.Vector3(1114.848, 234.5277, -50.8409),
        rot: 108.0,
    },
    {
        model: "vw_prop_casino_slot_03a",
        position: new mp.Vector3(1114.554, 233.6625, -50.8409),
        rot: 36.0,
    },
    {
        model: "vw_prop_casino_slot_07a",
        position: new mp.Vector3(1116.662, 228.8896, -50.8409),
        rot: -36.0,
    },
    {
        model: "vw_prop_casino_slot_08a",
        position: new mp.Vector3(1116.392, 229.7628, -50.8409),
        rot: -108.0,
    },
    {
        model: "vw_prop_casino_slot_04a",
        position: new mp.Vector3(1117.139, 230.2895, -50.8409),
        rot: 180.0,
    },
    {
        model: "vw_prop_casino_slot_05a",
        position: new mp.Vector3(1117.871, 229.7419, -50.8409),
        rot: 108.0,
    },
    {
        model: "vw_prop_casino_slot_06a",
        position: new mp.Vector3(1117.576, 228.8767, -50.8409),
        rot: 36.0,
    },
    {
        model: "vw_prop_casino_slot_08a",
        position: new mp.Vector3( 1129.64, 250.451, -52.0409),
        rot: 180.0,
    },
    {
        model: "vw_prop_casino_slot_07a",
        position: new mp.Vector3( 1130.376, 250.3577, -52.0409),
        rot: 165.0,
    },
    {
        model: "vw_prop_casino_slot_06a",
        position: new mp.Vector3(1131.062, 250.0776, -52.0409),
        rot: 150.0,
    },
    {
        model: "vw_prop_casino_slot_05a",
        position: new mp.Vector3(1131.655, 249.6264, -52.0409),
        rot: 135.0,
    },
    {
        model: "vw_prop_casino_slot_04a",
        position: new mp.Vector3(1132.109, 249.0355, -52.0409),
        rot: 120.0,
    },
    {
        model: "vw_prop_casino_slot_03a",
        position: new mp.Vector3(1132.396, 248.3382, -52.0409),
        rot: 105.0,
    },
    {
        model: "vw_prop_casino_slot_02a",
        position: new mp.Vector3(1132.492, 247.5984, -52.0409),
        rot: 90.0,
    },
    {
        model: "vw_prop_casino_slot_03a",
        position: new mp.Vector3(1133.952, 256.1037, -52.0409),
        rot: -45,
    },
    {
        model: "vw_prop_casino_slot_04a",
        position: new mp.Vector3(1133.827, 256.9098, -52.0409),
        rot: -117,
    },
    {
        model: "vw_prop_casino_slot_05a",
        position: new mp.Vector3(1134.556, 257.2778, -52.0409),
        rot: 171,
    },
    {
        model: "vw_prop_casino_slot_01a",
        position: new mp.Vector3(1135.132, 256.699, -52.0409),
        rot: 99,
    },
    {
        model: "vw_prop_casino_slot_02a",
        position: new mp.Vector3(1134.759, 255.9734, -52.0409),
        rot: 27,
    },
    {
        model: "vw_prop_casino_slot_06a",
        position: new mp.Vector3(1138.195, 251.8611, -52.0409),
        rot: -45,
    },
    {
        model: "vw_prop_casino_slot_07a",
        position: new mp.Vector3(1138.07, 252.6677, -52.0409),
        rot: -117,
    },
    {
        model: "vw_prop_casino_slot_08a",
        position: new mp.Vector3(1138.799, 253.0363, -52.0409),
        rot: 171,
    },
    {
        model: "vw_prop_casino_slot_04a",
        position: new mp.Vector3(1139.372, 252.4563, -52.0409),
        rot: 99,
    },
    {
        model: "vw_prop_casino_slot_05a",
        position: new mp.Vector3(1139, 251.7306, -52.0409),
        rot: 27,
    },
]

mp.events.add('show_slots_text', () => {
    let i = 0;
    SLOTS_MACHINE.forEach((t) => {
        mp.labels.new(t.toString(), t.position,
        {
            los: false,
            font: 1,
            drawDistance: 100,
           
        }); 

        mp.checkpoints.new(1, t.position, 1,
            {
                visible: true,
                dimension: 0,
                color: [255, 255, 255, 50]
            });
        i = i + 1; 
        mp.gui.chat.push(`${i}`)});
});

mp.events.add('casino_start_slot', (i) => {

    var a = SLOTS_MACHINE[i];

    data = SLOTS_MACHINE[i];

    var seatPos = mp.game.object.getObjectOffsetFromCoords(a.position.x, a.position.y, a.position.z, a.rot, 0, -.8, .7);
    var reelsCenterPos = mp.game.object.getObjectOffsetFromCoords(a.position.x, a.position.y, a.position.z, a.rot, 0, .035, 1.1);
    mp.players.local.setVisible(!1, !0);
    mp.players.local.freezePosition(true);

    CreateCamera(new mp.Vector3(seatPos.x, seatPos.y, seatPos.z + .5), new mp.Vector3(0, 0, 0), 50, reelsCenterPos, 800);
});

mp.events.add('casino_exit_slot', () => {

    
    data = {};

    ResetCamera();

    mp.players.local.setVisible(!0, !0)
    mp.players.local.freezePosition(false);

    d = null;

});


mp.events.add('casino_spin_slot', (e) => {
    started = true;

    SpitRes(e);
});

mp.events.add('start_slot', (val) => {
    if(!started)
        mp.events.callRemote('casino_start_slot', val);
});



function CreateReels()
{

    reels.forEach(e => e.destroy());

    reels = 
    [
        mp.objects.new(mp.game.joaat(data.model + `_reels`), mp.game.object.getObjectOffsetFromCoords(data.position.x, data.position.y, data.position.z, data.rot, -.115, .035, 1.1), {
        dimension: 0,
        rotation: new mp.Vector3(0, 0, data.rot)
        }), 
        mp.objects.new(mp.game.joaat(data.model + `_reels`), mp.game.object.getObjectOffsetFromCoords(data.position.x, data.position.y, data.position.z, data.rot, .01, .035, 1.1), {
        dimension: 0,
        rotation: new mp.Vector3(0, 0, data.rot)
        }), 
        mp.objects.new(mp.game.joaat(data.model + `_reels`), mp.game.object.getObjectOffsetFromCoords(data.position.x, data.position.y, data.position.z, data.rot, .125, .035, 1.1), {
        dimension: 0,
        rotation: new mp.Vector3(0, 0, data.rot)
        }), 
    ];
}

function CreateCamera(a, b, c, e, f = 0) {
    null != d && mp.cameras.exists(d) && d.destroy(), d = mp.cameras.new("default", a, b, c), d.pointAtCoord(e.x, e.y, e.z), d.setActive(!0), mp.game.cam.renderScriptCams(!0, 0 < f, f, !0, !1)
}

function ResetCamera (a = 0) {
    null != d && mp.cameras.exists(d) && d.destroy(), mp.game.cam.renderScriptCams(!1, 0 < a, a, !0, !1)
};

function SpitRes(e)
{
    CreateReels();

    const n = a(Math.random() * 3) + 0 + (a(Math.random() * 2) + 1);
    let l = 0;
    const c = [280, 320, 360],
        d = [!0, !0, !0],
        p = -1 === e ? [n, n + (.5 < Math.random() ? 1 : 0), n + 2] : [e, e, e];

        

        const i = setInterval(() => {
            l += 5;
            for (let e = 0; 3 > e; e++) d[e] && (c[e]--, 0 > c[e] ? (reels[e].setRotation(22.5 * p[e], 0, data.rot, 2, !0), d[e] = !1) : reels[e].setRotation(l, 0, data.rot, 2, !0));
            if (!d[2]) return -1 === e ? mp.events.callRemote('casino_stop_slot', -1) : WinSlot(), void clearInterval(i), started = false;
        }, 10);

        mp.events.call('client_play_sound', 'casino_slot_spin');

}

function WinSlot(){
    mp.events.call('client_play_sound', 'casino_slot_win');
    mp.events.callRemote('casino_stop_slot', 1)
}