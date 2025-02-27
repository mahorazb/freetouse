var tuning = new Vue({
    el: ".tuning",
    data: {
        active: true,
        title: "Автотюнинг",
        items: [{img: "1", name: "Двигатель",
            submenu: [{name: "Стоковый двигатель", price: 500},{name: "STAGE 1", price: 500},{name: "STAGE 2", price: 500},
                {name: "STAGE 3", price: 500},{name: "STAGE 4", price: 500}]},
            {img: "2", name: "Турбонадув",submenu: [{name: "Нет",price: 500}, {name: "Турбонаддув",price: 500}] },
            {img: "3", name: "Коробка передач", submenu: [{name: "Стандартная",price: 500}, {name: "Улучшенная",price: 500}, {name: "Спортивная",price: 500}, {name: "Гоночная",price: 500}]},
            {img: "4", name: "Тормоза", submenu: [{name: "Стандартные",price: 500}, {name: "Улучшенные",price: 500}, {name: "Спортивные",price: 500}, {name: "Гоночные",price: 500}]},
            {img: "5", name: "Подвеска", range: true,submenu: [{name: "Стандартная",price: 500}, {name: "Спортивная",price: 500}, {name: "Гоночная",price: 500}, {name: "Раллийная",price: 500}, {name: "Заниженная",price: 500}]},
            {img: "6", name: "Покраска",submenu: [
                {name: "Основной цвет", submenu: [{name: "Хромированный",price: 500}, {name: "Классический",price: 500}, {name: "Матовый",price: 500},{name: "Металлик",price: 500},{name: "Металл",price: 500},{name: "Перламутровый",price: 500}]},
                    {name: "Дополнительный цвет", submenu: [{name: "Хромированный",price: 500}, {name: "Классический",price: 500}, {name: "Матовый",price: 500},{name: "Металлик",price: 500},{name: "Металл",price: 500},{name: "Перламутровый",price: 500}]},
                    {name: "Неоновая подсветка", submenu: [{name: "Неоновая подсветка 1",price: 500}, {name: "Неоновая подсветка 2",price: 500}]}
                ]},
            {img: "7", name: "Колёса",submenu: [{name: "Эксклюзивные диски",price: 500}, {name: "Лоурайдер",price: 500}, {name: "Внедорожник",price: 500}, {name: "Спорт",price: 500}, {name: "Вездеход",price: 500}, {name: "Тюннер",price: 500}, {name: "Маслкар",price: 500}, {name: "AMG",price: 500}]},
            {img: "8", name: "Клаксоны", submenu: [{name: "Стандартный",price: 500}, {name: "Грузовик",price: 500}, {name: "Клоун",price: 500}, {name: "Данг-данг",price: 500}, {name: "Автобус",price: 500}, {name: "QSSD (AZ) ",price: 500}, {name: "Прочь с дороги (RUS) ",price: 500}]},
            {img: "9", name: "Тонировка", submenu: [{name: "Нет",price: 500}, {name: "35%",price: 500}, {name: "15%",price: 500}, {name: "5%",price: 500}, {name: "Бункер",price: 500}]},
            {img: "10", name: "Глушитель", submenu: [{name: "Стандартный",price: 500},{name: "Глушитель 1",price: 500},{name: "Глушитель 2",price: 500},{name: "Глушитель 3",price: 500},{name: "Глушитель 4",price: 500}]},
            {img: "11", name: "Бамперы",  submenu: [{name: "Передний бампер", submenu: [{name: "Стандартный",price: 500}, {name: "Бампер 1",price: 500}, {name: "Бампер 2",price: 500}, {name: "Бампер 3",price: 500}, {name: "Бампер 4",price: 500}]}, {name: "Задний бампер", submenu: [{name: "Стандартный",price: 500}, {name: "Бампер 1",price: 500}, {name: "Бампер 2",price: 500}, {name: "Бампер 3",price: 500}, {name: "Бампер 4",price: 500}]}]},
            {img: "12", name: "Пороги", submenu: [{name: "Стандартный",price: 500}, {name: "Пороги 1",price: 500}, {name: "Пороги 2",price: 500}, {name: "Пороги 3",price: 500}, {name: "Пороги 4",price: 500}]},
            {img: "13", name: "Капот", submenu: [{name: "Стандартный",price: 500}, {name: "Капот 1",price: 500}, {name: "Капот 2",price: 500}, {name: "Капот 3",price: 500}, {name: "Капот 4",price: 500}]},
            {img: "14", name: "Крыша", submenu: [{name: "Стандартный",price: 500}, {name: "Крыша 1",price: 500}, {name: "Крыша 2",price: 500}, {name: "Крыша 3",price: 500}, {name: "Крыша 4",price: 500}]},
            {img: "15", name: "Спойлер", submenu: [{name: "Стандартный",price: 500}, {name: "Спойлер 1",price: 500}, {name: "Спойлер 2",price: 500}, {name: "Спойлер 3",price: 500}, {name: "Спойлер 4",price: 500}]},
            {img: "16", name: "Фары", submenu: [{name: "Стандартные",price: 500}, {name: "Ксеноновые",price: 500}, {name: "Синие",price: 500}, {name: "Голубые",price: 500}, {name: "Бирюзовые",price: 500}]},
            {img: "17", name: "Номера", submenu: [{name: "Синий на белом",price: 500}, {name: "Чёрный на белом",price: 500}]}],
        speed: 250,
        boost: "2.3/100",
        braking: 60,
        clutch: 50,
        submenu_index: 0,
        submenu1_index: 0,
        submenu2_index: 0
    },
    methods: {
        show: function () {
            this.active = true;
        },
        hide: function () {
            this.active = false;
        },
        submenu: function (i) {
            this.submenu_index = i;
            if (this.items[this.submenu_index].submenu) {
                $(this.$el).find(".menu").hide();
                $(this.$el).find(".submenu").show();
            }
        },
        buy: function (level) {
            if (level == 1) {
                mp.trigger('tuning-buy', this.submenu1_index);
            }
            if (level == 2) {
                mp.trigger('tuning-buy', this.submenu1_index,  this.submenu2_index);
            }
        },
        select: function(i, e){
            this.submenu1_index = i;
            if (typeof this.items[this.submenu_index].submenu[i].submenu !== "undefined") {
                $(this.$el).find(".submenu").hide();
                $(this.$el).find(".submenu2").show();
            } else {
                $(this.$el).find(".submenu .active").removeClass("active");
                $(e.target).addClass("active");
            }

        },
        select_podveska: function (i, e) {
            this.submenu1_index = i;
            $(this.$el).find(".submenu .active").removeClass("active");
            $(e.target).addClass("active");
        },
        select2: function (i, e) {
            this.submenu2_index = i;
            $(this.$el).find(".submenu2 .active").removeClass("active");
            $(e.target).addClass("active");
        },
        back: function () {
            $(this.$el).find(".menu").show();
            $(this.$el).find(".submenu").hide();
        },
        back2: function () {
            $(this.$el).find(".submenu").show();
            $(this.$el).find(".submenu2").hide();
        }
    }
});