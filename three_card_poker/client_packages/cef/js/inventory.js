var itemsData = {
    "-1": "Маска",
    "-3": "Перчатки",
    "-4": "Штаны",
    "-5": "Рюкзак",
    "-6": "Обувь",
    "-7": "Аксессуар",
    "-8": "Нижняя одежда",
    "-9": "Бронежилет",
    "-10": "Украшения",
    "-11": "Верхняя одежда",
    "-12": "Головной убор",
    "-13": "Очки",
    "-14": "Аксессуар",
    0: "Test Item",
    1: "Аптечка",
    2: "Канистра",
    3: "Чипсы",
    4: "Пиво",
    5: "Пицца",
    6: "Бургер",
    7: "Хот-Дог",
    8: "Сэндвич",
    9: "eCola",
    10: "Sprunk",
    11: "Отмычка для замков",
    12: "Сумка с деньгами",
    13: "Материалы",
    14: "Наркотики",
    15: "Сумка с дрелью",
    16: "Военная отмычка",
    17: "Мешок",
    18: "Стяжки",
    19: "Ключи от машины",
    40: "Подарок",
    41: "Связка ключей",

    20: `"На корке лимона"`,
    21: `"На бруснике"`,
    22: `"Русский стандарт"`,
    23: `"Asahi"`,
    24: `"Midori"`,
    25: `"Yamazaki"`,
    26: `"Martini Asti"`,
    27: `"Sambuca"`,
    28: `"Campari"`,
    29: `"Дживан"`,
    30: `"Арарат"`,
    31: `"Noyan Tapan"`,

    100: "Pistol",
    101: "Combat Pistol",
    102: "Pistol .50",
    103: "SNS Pistol",
    104: "Heavy Pistol",
    105: "Vintage Pistol",
    106: "Marksman Pistol",
    107: "Revolver",
    108: "AP Pistol",
    109: "Stun Gun",
    110: "Flare Gun",
    111: "Double Action",
    112: "Pistol Mk2",
    113: "SNSPistol Mk2",
    114: "Revolver Mk2",

    115: "Micro SMG",
    116: "Machine Pistol",
    117: "SMG",
    118: "Assault SMG",
    119: "Combat PDW",
    120: "MG",
    121: "Combat MG",
    122: "Gusenberg",
    123: "Mini SMG",
    124: "SMG Mk2",
    125: "Combat MG Mk2",

    126: "Assault Rifle",
    127: "Carbine Rifle",
    128: "Advanced Rifle",
    129: "Special Carbine",
    130: "Bullpup Rifle",
    131: "Compact Rifle",
    132: "Assault Rifle Mk2",
    133: "Carbine Rifle Mk2",
    134: "Special Carbine Mk2",
    135: "Bullpup Rifle Mk2",

    136: "Sniper Rifle",
    137: "Heavy Sniper",
    138: "Marksman Rifle",
    139: "Heavy Sniper Mk2",
    140: "Marksman Rifle Mk2",

    141: "Pump Shotgun",
    142: "SawnOff Shotgun",
    143: "Bullpup Shotgun",
    144: "Assault Shotgun",
    145: "Musket",
    146: "Heavy Shotgun",
    147: "Double Barrel Shotgun",
    148: "Sweeper Shotgun",
    149: "Pump Shotgun Mk2",

    180: "Нож",
    181: "Дубинка",
    182: "Молоток",
    183: "Бита",
    184: "Лом",
    185: "Гольф клюшка",
    186: "Бутылка",
    187: "Кинжал",
    188: "Топор",
    189: "Кастет",
    190: "Мачете",
    191: "Фонарик",
    192: "Швейцарский нож",
    193: "Кий",
    194: "Ключ",
    195: "Боевой топор",

    200: "Пистолетный калибр",
    201: "Малый калибр",
    202: "Автоматный калибр",
    203: "Снайперский калибр",
    204: "Дробь",

    // Fishing
    205: "Удочка",
    206: "Улучшенная удочка",
    207: "Удочка MK2",
    208: "Наживка",
    209: "Корюшка",
    210: "Кунджа",
    211: "Лосось",
    212: "Окунь",
    213: "Осётр",
    214: "Скат",
    215: "Тунец",
    216: "Угорь",
    217: "Чёрный амур",
    218: "Щука",

    // AlcoShop
    219: "Martini Asti",
    220: "Sambuca",
    221: "Водка с лимоном",
    222: "Водка на бруснике",
    223: "Русский стандарт",
    224: "Коньяк Дживан",
    225: "Коньяк Арарат",
    226: "Пиво разливное",
    227: "Пиво бутылочное",
    228: "Кальян",
    234: "Урожай",
    235: "Семена",
    777: "Рем.Комплект",

    556: "Сух.Паёк",
}

var itemsInfo = {
    "-1": "Маска - поможет скрыть твою личность.",
    "-3": "Перчатки.",
    "-4": "Штаны.",
    "-5": "Рюкзак.",
    "-6": "Обувь.",
    "-7": "Украшения/Галстук.",
    "-8": "Нижния одежда.",
    "-9": "Бронежилет - поможет получать меньже урона от выстрела.",
    "-10": ".",
    "-11": "Верхняя одежда.",
    "-12": "Головные уборы.",
    "-13": "Очки.",
    "-14": "Часы/Брелки на руку.",
    1:	"Аптечка - поможет реанимировать или восстановить себе здоровье.",
    2: "Конистра бензина - поможет заправить ТС в любом месте.",
    3: "Пачка чипсов - поможет восстановить голод.",
    4:"Без алкольное пиво - дописать.",
    5:"Пицца - поможет восстановить голод.",
    6: "Бургер - поможет восстановить голод.",
    7:  "Хот-Дог - поможет восстановить голод.",
    8:	"Сэндвич - поможет восстановить голод.",
    9:	"Cola - поможет восстановить жажду.",
    10:	"Sprunk - поможет восстановить жажду.",
    11:	"Отмычка - поможет вскрыть замок.",
    12:	"Сумка с деньгами - скорее всего деньги ворованые.",
    13:	"Материалы - помогут скрафтить оружие.",
    14:	"Пакетик марихуаны - поможет восстановить здоровье.",
    15:	"Сумка с дрелью - поможет вскрыть хранилище.",
    16:	"Военная отмычка - поможет скрыть военую технику.",
    17:	"Мешок - надев его на человека он не будет видеть.",
    18:	"Стяжки - помогут связать человека.",
    19:	"Ключи - помогут открыть ваше личное ТС.",
    20:	"Выпивка - даёт алкогольный эффект.",
    21:	"Выпивка - даёт алкогольный эффект.",
    22:	"Выпивка - даёт алкогольный эффект.",
    23:	"Выпивка - даёт алкогольный эффект.",
    24:	"Выпивка - даёт алкогольный эффект.",
    25:	"Выпивка - даёт алкогольный эффект.",
    26:	"Выпивка - даёт алкогольный эффект.",
    27:	"Выпивка - даёт алкогольный эффект.",
    28:	"Выпивка - даёт алкогольный эффект.",
    1:	"Аптечка - поможет реанимировать или восстановить себе здоровье.",
    556: "Сух.паёк (MRE) - Полностью восстанавливает Еду и Воду.",
}

Vue.component('b-item-weapon', {

    template: `
         <img 
            :src="+id ? src : srcelse" 
            alt=""
            :class="{draggable: +id}"
            @dragstart.prevent
            @mousedown="select"
            @click.right.prevent="select"
         />
    `,

    props: ['id', 'index', 'count', 'isactive', 'type', 'subdata', 'weight'],

    data() {
        return {
            src: 'items/' + this.id + '.png',
            name: itemsData[this.id],
            srcelse: 'images/inventory/icons/Carbine-rifle-icon.svg'
        };
    },

    methods: {
        select: function (event) {
            inventory.sType = this.type;
            inventory.sID = this.id;
            inventory.sIndex = this.index;
        }
    }
});


Vue.component('b-item-stuff', {

    template: `
       <img 
            :src="+id ? src : srcelse" 
            alt=""
            :class="{draggable: +id}"
            @dragstart.prevent
            @mousedown="select"
            @click.right.prevent="select"
       />
    `,

    props: ['id', 'index', 'count', 'isactive', 'type', 'subdata', 'weight', 'srcelse'],

    data: function (data) {
        return {
            src: 'items/' + this.id + '.png',
            name: itemsData[this.id]
        };
    },

    methods: {
        select: function (event) {
            inventory.sType = this.type;
            inventory.sID = this.id;
            inventory.sIndex = this.index;
        }
    }
});

Vue.component('b-item-wrap-na', {
    template: `
        <div
            class="non-active b-item-wrap"
            :id="id"
        >
            <b-item-na
                v-if="+item[0] != 0 && +item[3] == 0"
                :id="item[0]"
                :index="id"
                :count="item[1]"
                :isactive="item[3]"
                :subdata="item[4]"
                :weight="item[2]"
            ></b-item-na>
        </div>
    `,

    props: ['item', 'id']
});


Vue.component('b-item-na', {
    template: `
           <div 
                :id="id"
                class="b-item"
           >
                <img @dragstart.prevent class="b-item-image" :src="src" alt="">
                <span class="b-item-text" :title="name">{{name}}</span>
                <span class="b-item-sub">{{subdata}}</span>
                <span class="b-item-amount">{{count}}</span>
                <span class="b-item-weight">{{(+weight).toFixed(2)}} кг</span>
            </div>    
    `,

    props: ['id', 'index', 'count', 'isactive', 'subdata', 'weight'],

    data: function (data) {
        return {
            src: 'items/' + this.id + '.png',
            name: itemsData[this.id]
        };
    }
});




Vue.component('b-item-wrap', {
    template: `
        <div
            :class="{'b-item-wrap': true, 'droppable': true}"
            :id="id"
        >
            <b-item
                v-if="+item[0] != 0 && +item[3] == 0"
                :id="item[0]"
                :index="id"
                :count="item[1]"
                :isactive="item[3]"
                :subdata="item[4]"
                :weight="item[2]"
                :type="type"
            ></b-item>
        </div>
    `,

    props: ['item', 'id', 'type'],

    data() {
        return {

        };
    },
    methods: {

    }
});




Vue.component('b-item', {
    template: `
           <div 
                :id="id"
                :class="{'b-item': true, 'draggable': true}"
                @click.right.prevent="select"
                @mousedown="select"
                :title="subdata"
           >
                <img @dragstart.prevent class="b-item-image" :src="src" alt="">
                <span class="b-item-text" :title="name">{{name}}</span>
                <span class="b-item-sub">{{subdata}}</span>
                <span class="b-item-amount">{{count}}</span>
                <span class="b-item-weight">{{(+weight).toFixed(2)}} кг</span>
            </div>    
    `,

    props: ['id', 'index', 'count', 'isactive', 'type', 'subdata', 'weight'],

    data: function (data) {
        return {
            src: 'items/' + this.id + '.png',
            name: itemsData[this.id]
        };
    },

    methods: {
        select: function (event) {
            inventory.sType = (this.type == 'inv') ? 1 : 0;
            inventory.sID = this.id;
            inventory.sIndex = this.index;
            context.type = (this.type == 'inv') ? 1 : 0;
            context.itemCount = this.count;
        }
    }
});

var inventory = new Vue({
    el: ".board",
    data: {
        active: false,
        outside: false,
        trade: false,
        outType: 0,
        outHead: "Внешний",
        stats: [1, 2, "88005553535", "Admin", 0, 0, "123456789$", "987654321$", "", 9999999, 9999999],
        items: [[200, 5, 1, 1, 1], [1, 5, 1], [5, 10, 0], [10, 500, 0], [11, 100, 1], [-6, 5, 1],[-7, 5, 1],[-8, 5, 1],[-9, 5, 1],[-11, 5, 1],[-12, 5, 1],[-13, 5, 1], [10, 1, 0, 0, 0], [10, 1, 0, 0, 0],[10, 1, 0, 0, 0], [10, 1, 0, 0, 0],[10, 1, 0, 0, 0], [10, 1, 0, 0, 0],[10, 1, 0, 0, 0], [10, 1, 0, 0, 0],[10, 1, 0, 0, 0], [10, 1, 0, 0, 0],[10, 1, 0, 0, 0]],
        outitems: [[1, 5, 1], [5, 10, 0], [10, 500, 0], [11, 100, 0], ],
        sIndex: 0,
        sType: 0,
        sID: 0,
        key: 0,
        tkey: 1,
        wkey: 2,
        tradeMessage: '',
        tradeReady: false,
        checked: false,
        tradeCash: '',
        maxCash: '',
        moneyFrom: 0,
        totalWeight: '0',
        totalWeightOut: '0',
        dragObject: {},
        mood: '',
        hunger: '',
        thirst: '',
        skills: [4, 4, 4, 4, 4],
        tItems: [[1, 1], [1, 1], [1, 1], [1, 1], [1, 1], [1, 1], [1, 1], [1, 1], [1, 1], [1, 1], [1, 1], [1, 1], [1, 1], [1, 1], [1, 1], [1, 1], [1, 1], [1, 1]],
        wItems: [[1, 1], [1, 1], [1, 1], [1, 1], [1, 1], [1, 1], [1, 1], [1, 1], [1, 1], [1, 1], [1, 1], [1, 1]]
    },
    computed: {
        getTotalWeightOut() {
            return this.outitems.reduce((accumulator, currentValue) => accumulator + Number(currentValue[2]), 0);
        },
        getTotalWeight() {
            return this.items.reduce((accumulator, currentValue) => accumulator + Number(currentValue[2]), 0);
        }
    },
    methods: {
        onMouseDown(e) {
            if (e.which !== 1) return;
            const elem = e.target.closest('.draggable');
            if (!elem) return;
            this.dragObject = {
                elem,
                downX: e.pageX,
                downY: e.pageY
            };
        },
        show() {
            this.active = true;
        },
        hideA() {
            if (this.dragObject.avatar) {
                this.dragObject.avatar.remove();
                this.dragObject.avatar = null;
            }

            if (this.dragObject.elem) {
                this.dragObject.elem.remove();
                this.dragObject.elem = null;
            }

            if (this.dragObject.curDropEl) {
                this.dragObject.curDropEl.remove();
                this.dragObject.curDropEl.style.backgroundColor = '';
                this.dragObject.curDropEl = null;
            }

            this.active = false;
            mp.trigger('board', 'close_Board');
        },
        onMouseMove(e) {
            if (!this.dragObject.elem) return;
            if (!this.dragObject.avatar) {
                const moveX = e.pageX - this.dragObject.downX;
                const moveY = e.pageY - this.dragObject.downY;
                if (Math.abs(moveX) < 3 && Math.abs(moveY) < 3) return;
                this.dragObject.avatar = this.createAvatar(e);
                if (!this.dragObject.avatar) {
                    this.dragObject = {};
                    return;
                };
                const coords = this.getCoords(this.dragObject.avatar);
                this.dragObject.shiftX = this.dragObject.downX - coords.left;
                this.dragObject.shiftY = this.dragObject.downY - coords.top;
                this.startDrag(e);
            }

            let underElement = this.findDroppable(e, '.droppable');

            if (underElement) {
                if (!this.dragObject.curDropEl) {
                    this.dragObject.curDropEl = underElement;
                    this.dragObject.curDropEl.style.backgroundColor = 'rgba(255, 255, 255, .2)';
                } else if (this.dragObject.curDropEl != underElement) {
                    this.dragObject.curDropEl.style.backgroundColor = '';
                    this.dragObject.curDropEl = null;
                }
            }

            this.dragObject.avatar.style.left = e.pageX - this.dragObject.shiftX + 'px';
            this.dragObject.avatar.style.top = e.pageY - this.dragObject.shiftY + 'px';
            return false;
        },
        createAvatar(e) {
            const avatar = this.dragObject.elem;
            const old = {
                parent: avatar.parentNode,
                nextSibling: avatar.nextSibling,
                position: avatar.position || '',
                left: avatar.left || '',
                top: avatar.top || '',
                zIndex: avatar.zIndex || ''
            };
            avatar.rollback = () => {
                old.parent.insertBefore(avatar, old.nextSibling);
                avatar.style.position = old.position;
                avatar.style.left = old.left;
                avatar.style.top = old.top;
                avatar.style.zIndex = old.zIndex
                avatar.classList.remove('dragMove');
            };
            avatar.old = old;
            return avatar;
        },
        startDrag(e) {
            const avatar = this.dragObject.avatar;
            document.body.appendChild(avatar);
            this.hide();
            avatar.style.zIndex = 9999;
            avatar.style.position = 'absolute';
            avatar.classList.add('dragMove');
        },
        onMouseUp(e) {
            if (this.dragObject.avatar) {
                this.finishDrag(e);
            }
            this.dragObject = {};
        },
        finishDrag(e) {
            const dropElem = this.findDroppable(e, '.droppable');
            if (dropElem) {
                this.dragObject.elem.style.display = 'none';
                this.dragObject.elem.style.position = '';
                this.dragObject.elem.style.zIndex = '';
                this.dragObject.elem.style.top = '';
                this.dragObject.elem.style.left = '';
                this.dragObject.elem.classList.remove('dragMove');

                let type = (this.sType) ? 0 : this.outType
                let dropType = dropElem.getAttribute('data-type');
                let fromIndex = this.sIndex;
                let toIndex = dropElem.id;
                let data = JSON.stringify([type, fromIndex, +toIndex]);

                //mp.trigger('notify', 1, 2, JSON.stringify([dropType, this.sType, fromIndex, toIndex]), 1500);


                if (this.dragObject.avatar.old.parent == dropElem) {
                    if (this.dragObject.curDropEl) {
                        this.dragObject.curDropEl.style.backgroundColor = '';
                    }
                    this.dragObject.elem.style.display = 'flex';
                    dropElem.appendChild(this.dragObject.elem);
                } else if (dropType == 'tradeInv' && this.sType == 1) {
                    mp.trigger('inventory_transfer', JSON.stringify([type, fromIndex, +toIndex]));
                } else if (dropType == 'wInv' && this.sType == 'wInv') {
                    mp.trigger('inventory_change_weapon_slot', JSON.stringify([fromIndex, +toIndex]));
                    if (this.dragObject.curDropEl) {
                        this.dragObject.curDropEl.style.backgroundColor = '';
                    }
                    return;
                } else if (dropType == 'wInv' && this.sType == 1) {
                    mp.trigger('inventory_use_weapon', JSON.stringify([fromIndex, +toIndex]));
                    if (this.dragObject.curDropEl) {
                        this.dragObject.curDropEl.style.backgroundColor = '';
                    }
                    return;
                } else if (dropType == 'inv' && this.sType == 'wInv') {
                    mp.trigger('inventory_remove_weapon', JSON.stringify([fromIndex, +toIndex]));
                    if (this.dragObject.curDropEl) {
                        this.dragObject.curDropEl.style.backgroundColor = '';
                    }
                    return;
                } else if (dropType == 'tInv' && this.sType == 'tInv' && this.dragObject.avatar.old.parent != dropElem) {
                    if (this.dragObject.curDropEl) {
                        this.dragObject.curDropEl.style.backgroundColor = '';
                    }
                    this.dragObject.elem.style.display = 'inline';
                    this.dragObject.avatar.old.parent.appendChild(this.dragObject.elem);
                } else if (dropType == 'tInv' && this.sType == 1) {
                    mp.trigger('inventory_use_clothes', JSON.stringify([fromIndex, +toIndex]));
                    if (this.dragObject.curDropEl) {
                        this.dragObject.curDropEl.style.backgroundColor = '';
                    }
                    return;
                } else if (dropType == 'inv' && this.sType == 'tInv') {
                    // mp.trigger('notify', 1, 2, JSON.stringify([fromIndex, +toIndex]), 1000);
                    mp.trigger('inventory_stuff', JSON.stringify([fromIndex, +toIndex]));
                    return;
                } else if (dropElem.children.length && type == 0 && dropType == 'inv' || dropElem.children.length && type != 0 && dropType == 'out') {
                    if (dropElem.children[0].id != this.dragObject.avatar.id) {
                        mp.trigger('inventory_swap', data);
                        // this.dragObject.avatar.old.parent.appendChild(dropElem.children[0]);
                        // this.dragObject.curDropEl.style.backgroundColor = '';
                        // this.dragObject.elem.style.display = 'flex';
                        // dropElem.appendChild(this.dragObject.elem);
                        return;
                    }
                    mp.trigger('inventory_stack', data);
                    return;
                } else if (dropType == 'inv' && type == 0 || dropType == 'out' && type != 0) {
                    mp.trigger('inventory_swap', data);
                } else if (dropType == 'out' && type == 0) {
                    mp.trigger('inventory_transfer', data);
                } else if (dropType == 'inv' && type != 0) {
                    mp.trigger('inventory_take', data);
                } else {
                    if (this.dragObject.curDropEl) {
                        this.dragObject.curDropEl.style.backgroundColor = '';
                    }
                    this.dragObject.elem.style.display = '';
                    this.dragObject.avatar.old.parent.appendChild(this.dragObject.elem);
                }

                if (this.dragObject.curDropEl) {
                    this.dragObject.curDropEl.style.backgroundColor = '';
                }

                // dropElem.appendChild(this.dragObject.elem);
                // this.dragObject.elem.style.display = 'flex';
            } else {
                this.dragObject.avatar.rollback();
                if (this.dragObject.curDropEl) {
                    this.dragObject.curDropEl.style.backgroundColor = '';
                }
            }
        },
        findDroppable(e, selector = '.droppable') {
            this.dragObject.avatar.style.pointerEvents = 'none';
            const elem = document.elementFromPoint(e.clientX, e.clientY);
            this.dragObject.avatar.style.pointerEvents = '';
            if (elem == null) return null;

            return elem.closest(selector);
        },
        getCoords(e) {
            const box = e.getBoundingClientRect();
            return {
                top: box.top + pageYOffset,
                left: box.left + pageXOffset
            };
        },
        context(event) {
            const el = clickInsideElement(event, 'b-item');
            if (el) {
                context.show(this.getCoords(el).left, this.getCoords(el).top);
            } else {
                context.hide()
            }
        },
        hide: function (event) {
            context.hide();
            separator.hide();
        },
        tradeSet(json) {
            this.tkey++;
            this.outType = json[0]
            this.outHead = json[1]
            this.outitems = [json[2], json[3]];
            this.maxCash = json[4];
        },
        outSet: function (json) {
            this.key++
            this.outType = json[0]
            this.outHead = json[1]
            this.outitems = json[2]
            //Vue.set(this.outitems, json[2])
        },
        updateItemToPlayer(index, data) {
            this.tkey++;
            Vue.set(this.outitems[0], index, data);
        },
        updateItemFromPlayer(index, data) {
            this.tkey++;
            Vue.set(this.outitems[1], index, data);
        },
        outItemUpdate(index, data) {
            this.key++;
            Vue.set(this.outitems, index, data);
        },
        itemsSet: function (json, info) {
            this.key++;
            this.items = json;

            this.tkey++;
            this.tItems = info;
        },
        weaponsSet(json) {
            this.wkey++;
            this.wItems = json;
        },
        weaponsItemUpdate(index, data) {
            this.wkey++;
            Vue.set(this.wItems, index, data);
        },
        clothesItemUpdate(index, data) {
            this.tkey++;
            Vue.set(this.tItems, index, data);
        },
        tradeCheck() {
            this.checked = !this.checked;
            mp.trigger('trade_check', this.checked, this.tradeCash);
        },
        tradeCancel() {
            mp.trigger('trade_cancel');
        },
        tradeAccept() {
            this.tradeMessage = 'Ожидание игрока...';
            document.querySelector('.trade-btn--success').disabled = true;
            mp.trigger('trade_accept');
        },
        setSkills(json) {
            this.skills = json;
        },
        itemUpd: function (index, data) {
            this.key++;
            Vue.set(this.items, +index, data);
        },
        send: function (id, amount = null) {
            let type = (this.sType) ? 0 : this.outType;
            if (type === 0 && id === 1) {
                if ([-1, -3, -4, -5, -6, -7, -8, -9, -10, -11, -12, -13, -14].some(item => item == this.items[this.sIndex][0])) {
                    id = 6;
                    amount = this.items[this.sIndex][0];
                }
            }

            mp.trigger('boardCB', id, type, this.sIndex, amount)
        },
        close: function () {
            if (this.dragObject.avatar) {
                this.dragObject.avatar.remove();
                this.dragObject.avatar = null;
            }

            if (this.dragObject.curDropEl) {
                this.dragObject.curDropEl.remove();
                this.dragObject.curDropEl.style.backgroundColor = '';
                this.dragObject.curDropEl = null;
            }

            this.hideA();
        },
        clear() {

        }
    }
});

var context = new Vue({
    el: ".context_menu",
    data: {
        active: false,
        style: '',
        type: true,
        itemCount: 0
    },
    methods: {
        show: function (x, y) {
            if (this.type == 0) return;
            this.style = `left:${x}px;top:${y}px;`;
            this.active = true;
        },
        hide: function () {
            this.active = false
        },
        btn: function (id) {
            this.hide()
            //console.log(id)
            inventory.send(id)
        },
        separator() {
            this.hide();
            if (this.itemCount <= 1) return;
            separator.active = true;
            separator.maxVal = this.itemCount;
        }
    }
})

var separator = new Vue({
    el: '.separator',
    data: {
        active: false,
        maxVal: 0,
        input: '1'
    },
    methods: {
        hide() {
            this.active = false;
            this.input = 1;
        },
        split(id) {
            if (+this.input > +this.maxVal) {
                mp.trigger('notify', 1, 2, 'Значение не должно превышать количество', 3000);
                this.input = 1;
            } else {
                inventory.send(id, +this.input);
                this.hide();
            }
        },
        close() {
            this.hide();
        },
        sepOne() {
            this.input = 1;
        },
        sepHalf() {
            this.input = Math.floor(this.maxVal / 2);
        },
        sepAll() {
            this.input = this.maxVal;
        }
    }
});


function clickInsideElement(e, className) {
    var el = e.srcElement || e.target;
    if (el.classList.contains(className)) {
        return el;
    } else {
        while (el = el.parentNode) {
            if (el.classList && el.classList.contains(className)) {
                return el;
            }
        }
    }
    return false;
}
