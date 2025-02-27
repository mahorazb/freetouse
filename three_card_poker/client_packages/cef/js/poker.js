var poker = new Vue({
    el: '.casinop__main',
    data: {
        active: false,
        players: [
            {
                active: false,
                now: false,
                chips: 0,
                name: "",
                lastAction: "CALL 250",
                winner: false,
            },
            {
                active: false,
                now: false,
                chips: 0,
                name: "",
                lastAction: "",
                winner: false,
            },
            {
                active: false,
                now: false,
                chips: 0,
                name: "Bot Imran",
                lastAction: "",
                winner: false,
            },
            {
                active: false,
                now: false,
                chips: 0,
                name: "Bot Dima",
                lastAction: "",
                winner: false,
            },
            {
                active: false,
                now: false,
                chips: 0,
                name: "Bot Marik",
                lastAction: "",
                winner: false,
            }
        ],
        pkey: 0,
        bank: 0,
        rise: 100,
        mySeat: -1,
        val: 0,
        chips: 1000,
        panel: false,
        tableBet: 0,
        maxBet: 0,
        button2Name: "CALL",
        button3Name: "RISE UP",
        timerOn: true,
        timer: '15',
    },

    methods: {
        show (){
            this.active = true;
        },
        showPanel(){
            this.maxBet -= this.players[this.mySeat].chips;
            if(this.maxBet > this.tableBet) this.rise = this.tableBet;
            else this.rise = 0;

            this.panel = true;
            this.pkey++;

        },
        setButtons(buttons){
            var res = JSON.parse(buttons);
            this.button2Name = res[0];
            this.button3Name = res[1];

            this.pkey++;
        },
        hidePanel(){
            this.panel = false;
            this.pkey++;
        },
        setMySeat(seat){
            this.mySeat = seat;
        },
        setPlayers(info){
            var res = JSON.parse(info);

            /*this.players[0].active = res[0]["active"];
            this.players[0].name = res[0]["name"];*/


            //this.bank = bank;

            /*this.players[0] = Object.assign({}, this.players[0], res[0]);
            this.players[1] = Object.assign({}, this.players[1], res[1]);
            this.players[2] = Object.assign({}, this.players[2], res[2]);
            this.players[3] = Object.assign({}, this.players[3], res[3]);
            this.players[4] = Object.assign({}, this.players[4], res[4]);*/


            Vue.set(this.players, 0, res[0]);
            Vue.set(this.players, 1, res[1]);
            Vue.set(this.players, 2, res[2]);
            Vue.set(this.players, 3, res[3]);
            Vue.set(this.players, 4, res[4]);
            /*this.players[1] = res[1];
            this.players[2] = res[2];
            this.players[3] = res[3];
            this.players[4] = res[4];*/

            this.bank = parseInt(res[0].chips) + parseInt(res[1].chips) + parseInt(res[2].chips) + parseInt(res[3].chips) + parseInt(res[4].chips);
            //mp.trigger('outputP',res[0].toString());

            this.pkey++;

      
        },
        hide (){
            this.active = false;
        },   
        button1(){
            mp.trigger('pockerAction', 'fold', 0);
        },
        button2(){
            mp.trigger('pockerAction', 'call', 0);
        },
        button3(){
            mp.trigger('pockerAction', 'bet', this.rise);
        },
        close() {
            mp.trigger('exitPokerUi');
        }
    }
});
