(function () {
    'use strict';

    angular
        .module('ship-wars-app')
        .controller('active_war_controller', active_war_controller);

    active_war_controller.$inject = ['$location', '$http', '$window', '$timeout'];

    function active_war_controller($location, $http, $window, $timeout) {
        var vm = this;
        var socket = new WebSocket("ws://" + window.location.host);
        configureSocket();

        function configureSocket(onopen, onclose, onmessage, onerror) {
            socket.onopen = onopen ? onopen : (e) => console.log('onOpen ' + e);
            socket.onclose = onclose ? onclose : (e) => console.log('onClose ' + e);
            socket.onmessage = onmessage ? onmessage : (e) => {
                console.log('onMessage ' + e.data);
                $timeout(() => {
                    var resp = JSON.parse(e.data);
                    var success = 1;
                    if (resp.state == success)
                        vm.war = resp.content;
                    else
                        alert(resp.errorMessage);
                }, 0);
            };
            socket.onerror = onerror ? onerror : (e) => console.log('onError ' + e);
        }
     
        vm.shot = (row, col) => {
            socket.close();
            socket = new WebSocket("ws://" + window.location.host);
            configureSocket((e) => socket.send(JSON.stringify({ warId: vm.war.id, row: row, col: col })),
                null, null, null);
        };

        (() => {
            $http.get('/wars/last')
            .then((resp) => {
                if (resp.data) {
                    vm.war = {};
                    angular.copy(resp.data, vm.war);
                }
                else
                    $window.location.href = '/';
            }, (err) => {
                console.log(err);
            }).finally(() => {

            });
        })();
    }
})();







//vm.init = (war) =>
//    vm.war = betterJson(war);

//function betterJson(war) {
//    return {
//        id: war.Id,
//        rows: war.Rows,
//        cols: war.Cols,
//        score1: war.Score1,
//        score2: war.Score2,
//        field: war.Field //JSON.parse(war.FieldModel)
//    };
//}