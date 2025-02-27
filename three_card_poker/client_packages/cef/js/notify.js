var notifys = [];

function notify(type, layout, message, time) {
    if (notifys.length > 3){
        notifys.shift().close();
    }
    var types = ['alert', 'error', 'success', 'information', 'warning'];
    var titles = ['Уведомление', 'Ошибка', 'Успешно', 'Информация', 'Предупреждение'];
    var layouts = ['bottomCenter', 'bottomCenter', 'bottomCenter', 'bottomCenter', 'bottomCenter', 'bottomCenter', 'bottomCenter', 'bottomCenter', 'bottomCenter', 'bottomCenter', 'bottomCenter'];
    var icons = [];
    message = '<div class="text">'+'<img src="'+ 'images/noty/'+ types[type] + '.svg" />'  + `<div><div class="noty__title">${titles[type]}</div>` + message+'</div></div>';
    let noty = new Noty({
        type: types[type],
        layout: layouts[layout],
        theme: 'fivestar',
        text: message,
        timeout: time,
        progressBar: true,
        animation: {
            open: 'noty_effects_open',
            close: 'noty_effects_close'
        }
    });
    notifys.push(noty);
    noty.show();

}
//Noty.setMaxVisible(10);
//https://ned.im/noty/#/api