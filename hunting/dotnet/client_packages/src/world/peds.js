const localPlayer = mp.players.local;

class ServerPed {
    constructor(){
        const controllerScripts = new Map();
        const a = new Map();
        const autoControlStreamPeds = new Set();

        let r = null;

        class Ped {
            constructor({id: _id, entity: _entity}){
                this.id = _id;
                this.entity = _entity;
                
                this.entity.ServerPed = this;

       
                this.controllerScriptOnStreamIn = () => {};
                this.controllerScriptOnStreamOut = () => {};
                this.controllerScriptOnChangeController = () => {};
                this.controllerScriptOnTick = () => {};
                const scriptController = controllerScripts.get(_entity.getVariable("controllerScript"));
           
                if(scriptController){
                  
         
                    this.controllerScriptOnStreamIn = scriptController.onStreamIn;
                    this.controllerScriptOnStreamOut = scriptController.onStreamOut;
                    this.controllerScriptOnChangeController = scriptController.onChangeController;
                    this.controllerScriptOnTick = scriptController.onTick;
          
                }
                else {
                    this.controllerScriptOnStreamIn = () => {};
                    this.controllerScriptOnStreamOut = () => {};
                    this.controllerScriptOnChangeController = () => {};
                    this.controllerScriptOnTick = () => {};                   
                }

              
                this.scenario = this.entity.getVariable("scenario");

                this.controllerStreamAuto = this.entity.getVariable("autoControl");
                
                this.staticDead = this.entity.getVariable("staticDead");
          
                this.controllerStreamInDist = 120;
                this.controllerStreamOutDist = 150;
                this.entity.__controllerStreamAuto = this.controllerStreamAuto;
                this.requireControllerStatusLastTime = -1;
                this.removeControllerStatusLastTime = -1;

                this.deathNotifyEnable = !1;
                this.deathNotifySend = !1;
                a.set(this.id, this);
         
            }
            async onStreamIn(){
                if(this.entity.isDynamic){
               
                    this.controllerScriptOnStreamIn(this),
                    this.entity.setInvincible(!0),
                    this.entity.freezePosition(!0),

                    this.controllerStreamAuto 
                        ? check(this) 
                        : (this.entity.setBlockingOfNonTemporaryEvents(!0),
                        this.entity.setAsMission(!0, !0),
                        this.entity.setAlertness(0),
                        this.entity.setCombatAttributes(17, !0),
                        this.entity.setFleeAttributes(0, !1),
                        this.entity.setCanBeDraggedOut(!1),
                        this.entity.setCanBeDamaged(!1)
                    )

                    if(this.staticDead) {
                        this.entity.setHealth(0), this.entity.freezePosition(!0);
                    }

                    if(this.scenario != null) this.entity.taskStartScenarioInPlace(this.scenario, 0, !0);
                }
            }
            onStreamOut() {
                try {
                if(this.entity.isDynamic){
                  this.controllerScriptOnStreamOut(this),
                  mp.peds.exists(this.entity) && this.entity.controller === localPlayer && this.removeControllerStatus(),
                  deletefromStreamSet(this);
                }
                }
                catch(e){
                    mp.gui.chat.push(`${e.toString()}`);
                }
            }
            onChangeController(e) {
                e
                  ? e === localPlayer &&
                    ((this.deathNotifyEnable = !1),
                    (this.deathNotifySend = !1),
                    check(this),
                    this.isOnStream() &&
                      (this.entity.setInvincible(!1),
                      this.entity.freezePosition(!1),
                      this.controllerScriptOnChangeController(this, e)))
                  : this.isOnStream() &&
                    (this.entity.setInvincible(!0),
                    this.entity.freezePosition(!0),
                    this.controllerScriptOnChangeController(this, e));
            }
            onDeath() {
                let cords = this.entity.getCoords(!0);
                mp.events.callRemote("peds_death", this.id, cords.x, cords.y, cords.z);
            }
            isOnStream() {
                return mp.peds.exists(this.entity) && 0 !== this.entity.handle;
            }
            isControlledByLocalPlayer() {
                return this.entity.controller === localPlayer;
            }
            requireControllerStatus() {
                mp.events.callRemote("peds_requireController", this.id);
            }
            removeControllerStatus() {
                mp.events.callRemote("peds_removeController", this.id);
            }
            getControllerScriptData() {
                return this.entity.getVariable("controllerScriptData");
            }
           
            static registerControllerScript(name, scripts) {
                controllerScripts.set(name, scripts);
            }
        }
        const getTime = () => new Date().getTime();
        const check = (e) => {
            autoControlStreamPeds.add(e),
              null === r &&
                (r = setInterval(() => {
                  const { x: e, y: a, z: t } = localPlayer.position;
                  for (const o of autoControlStreamPeds) {
            
                    if (!o.isOnStream()) {
                      
                      deletefromStreamSet(o);
                      continue;
                    }
                    if (o.entity.controller === localPlayer) {
                      const { x: _, y: r, z: i } = o.entity.getCoords(!0),
                        n = mp.game.gameplay.getDistanceBetweenCoords(_, r, i, e, a, t, true);
                      if (n > o.controllerStreamOutDist) {
                        const e = getTime();
                        if (o.removeControllerStatusLastTime + 1e3 < e) {
                          o.controllerStreamAuto || deletefromStreamSet(o),
                            o.removeControllerStatus(),
                            (o.removeControllerStatusLastTime = e);
                          continue;
                        }
                      } else
                        o.deathNotifyEnable &&
                          !o.deathNotifySend &&
                          0 === o.entity.getHealth() &&
                          (o.onDeath(), (o.deathNotifySend = !0)),
                          o.controllerScriptOnTick(o);
                    } else if (!o.entity.controller) {
                      const { x: _, y: r, z: i } = o.entity.getCoords(!0),
                        n = mp.game.gameplay.getDistanceBetweenCoords(_, r, i, e, a, t, true);
                      if (n < o.controllerStreamInDist) {
                        const e = getTime();
                        if (o.requireControllerStatusLastTime + 2500 < e) {
                          o.requireControllerStatus(),
                            (o.requireControllerStatusLastTime = e);
                          continue;
                        }
                      }
                    }
                  }
                }, 50));
        },
        deletefromStreamSet = (e) => {
            autoControlStreamPeds.has(e) &&
              (autoControlStreamPeds.delete(e),
              0 === autoControlStreamPeds.size && null !== r && (clearInterval(r), (r = null)));
            
        };

        global.ServerPed = Ped,
        mp.events.add("entityStreamIn", (e) => {

            if(e.ServerPed) e.ServerPed.onStreamIn();

        }),
        mp.events.add("entityStreamOut", (e) => {
            if (e.ServerPed) return void e.ServerPed.onStreamOut();
            if ("player" === e.type)
              for (const a of mp.peds.streamed.filter(
                (a) => a.__controllerStreamAuto && a.controller === e
              ))
                a.localControllerOverride = localPlayer;
        }),
        mp.events.add("entityControllerChange", (e, a) => {
     
            e.ServerPed && e.ServerPed.onChangeController(a);
        }),
        mp.events.add("serverWorldDataReady", () => {
            setTimeout(() => {
              mp.peds.forEach((e) => {
                let a = e.getVariable("pedId");
                
                if(a){
                    new Ped({ id: a, entity: e }).onStreamIn();
                }
              }),
                mp.events.addDataHandler("pedId", (e, a) => {
                
                    new Ped({ id: a, entity: e }).onStreamIn();
                });
     
            }, 5000);
        }),
        mp.events.add("client_ped_destroy", (e) => {
            const t = a.get(e);
            if(t){
                //let num = t.id;
                t.entity.destroy();
                a.delete(t.id);
            }
        });
    }
}

new ServerPed();
const ANGRY_ANIMAL_LIST = [
    mp.game.joaat("a_c_coyote"),
    mp.game.joaat("a_c_mtlion"),
    mp.game.joaat("a_c_panther"),
    mp.game.joaat("a_c_coyote") << 0,
    mp.game.joaat("a_c_mtlion") << 0,
    mp.game.joaat("a_c_panther") << 0,
];
const CAN_BE_ANGRY_LIST = [mp.game.joaat("a_c_boar"), mp.game.joaat("a_c_deer")];

global.ServerPed.registerControllerScript("ANIMAL_HUNTING", {
    onStreamIn(a) {},
    onStreamOut(a) {},
    onChangeController(a, b) {
      if (localPlayer === b) {
        a.entity.setMaxHealth(500),
        a.entity.setHealth(500),
        a.entity.setCanBeDamaged(!0),
          a.entity.setCanRagdoll(!1),
          a.entity.setCanRagdollFromPlayerImpact(!1),
          a.entity.setProofs(!1, !0, !0, !0, !1, !0, !0, !0),
          a.entity.setSuffersCriticalHits(!1);
        const b = a.entity.getCoords(!0),
          c = mp.game.gameplay.getGroundZFor3dCoord(b.x, b.y, b.z + 25, 0, !1);
        a.entity.setCoordsNoOffset(b.x, b.y, c + 1, !1, !1, !1);
        const d = a.entity.getModel();
        (-1 !== ANGRY_ANIMAL_LIST.indexOf(d) ||
          -1 !== ANGRY_ANIMAL_LIST.indexOf(d << 0) ||
          (0.5 < Math.random() && -1 !== CAN_BE_ANGRY_LIST.indexOf(d))) &&
          (a.entity.setCombatAbility(2),
          a.entity.setCombatRange(2),
          a.entity.setCombatMovement(3),
          a.entity.setCombatAttributes(46, !0),
          a.entity.setCombatAttributes(5, !0),
          a.entity.setFleeAttributes(0, !0),
          a.entity.setConfigFlag(2, !0),
          a.entity.setConfigFlag(188, !0),
          (a.__angryAnimal = !0)),
          a.entity.taskWanderStandard(10, 10),
          (a.deathNotifyEnable = !0);
      }
    },
    onTick(a) {},
});

mp.events.add('render', function () {
    try {
        let player = mp.players.local;

        mp.peds.forEach((p) => {
        
        let cords = p.getCoords(!0)

            if(mp.game.gameplay.getDistanceBetweenCoords(cords.x, cords.y, cords.z, player.position.x, player.position.y, player.position.z, true) < 150 &&  0 != p.getHealth()){
              let id = p.getVariable("pedId");

              if(id != undefined){
              mp.game.graphics.drawText(`${id}`, [cords.x, cords.y, cords.z], {
                font: 4,
                color: [255, 255, 255, 255],
                scale: [0.3, 0.3],
                outline: true
              });
            }
              //mp.game.graphics.drawLine(cords.x, cords.y, cords.z, player.position.x, player.position.y, player.position.z, 255, 255, 255, 120);
            }
          
        });
    } catch (e) { }
});


mp.events.add("client_hunting_anim", async () => {
    if (!mp.game.streaming.hasAnimDictLoaded("amb@medic@standing@kneel@base") || !mp.game.streaming.hasAnimDictLoaded("anim@gangops@facility@servers@bodysearch@")) 
    {
        mp.game.streaming.requestAnimDict("amb@medic@standing@kneel@base"),
        mp.game.streaming.requestAnimDict(
          "anim@gangops@facility@servers@bodysearch@"
        );

        do await mp.game.waitAsync(10);
        
        while (
            !mp.game.streaming.hasAnimDictLoaded(
            "amb@medic@standing@kneel@base"
            ) ||
            !mp.game.streaming.hasAnimDictLoaded(
            "anim@gangops@facility@servers@bodysearch@"
            )
        );
    }

    localPlayer.taskPlayAnim("amb@medic@standing@kneel@base","base",8,-8,-1,512,0,!1,!1,!1),
    localPlayer.taskPlayAnim("anim@gangops@facility@servers@bodysearch@","player_search",8,-8,-1,512,0,!1,!1,!1),
    await mp.game.waitAsync(5e3),
    localPlayer.stopAnimTask("amb@medic@standing@kneel@base", "base", 3),
    localPlayer.stopAnimTask("anim@gangops@facility@servers@bodysearch@","player_search",3);
});
/*
mp.events.add('outgoingDamage', (sourceEntity, sourcePlayer, targetEntity, weapon, boneIndex, damage) => {
    mp.gui.chat.push('outdam');
});


mp.events.add('incomingDamage', (sourceEntity, sourcePlayer, targetEntity, weapon, boneIndex, damage) => {
    mp.gui.chat.push('indamage');
});*/