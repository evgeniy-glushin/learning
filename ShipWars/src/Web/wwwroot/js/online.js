(() => {
    var ss = sessionStorage;

    setInterval(() => {
        $.get('/notifications/ping', (data) => {
            console.log(data)
            if (data) {
                ss.setItem('reload', true);
                location.reload();
            }
        });
    }, 1000)


    if (ss.getItem('reload') == null || ss.getItem('reload') == 'null') {
        $.post('online/yes', (data) => {
            console.log(data);
        });
    }

    ss.setItem('reload', null);

    window.onbeforeunload = function (e) {
        if (ss.getItem('reload') == null || ss.getItem('reload') == 'null') {
            $.ajax({
                type: "POST",
                async: false,
                url: "online/no",
            });
        }
    };
})();
