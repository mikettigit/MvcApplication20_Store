$(document).ready(function () {

    var latests_title = $(".latests_title");
    if (latests_title.length > 0) {

        var history_queue = $.cookie("history_queue");
        if (history_queue == null) {
            latests_title.hide();
        }
        else {
            latests_title.show();
            history_queue = JSON.parse(history_queue);
            for (var i = 0; i < history_queue.length; i++) {
                var current_item = $(history_queue[i]["content"]);
                latests_title.after(current_item);
            }
        }
    }

    if ($(".LastItemTemplate").length > 0) {

        var item = {
            key: $(".LastItemTemplate").attr("relativeurl"),
            content: $(".LastItemTemplate").html()
        }

        var history_queue = $.cookie("history_queue");
        if (history_queue == null) {
            history_queue = [];
        }
        else
        {
            history_queue = JSON.parse(history_queue);
        }

        for (var i = 0; i < history_queue.length; i++) {
            if (item["key"] == history_queue[i]["key"]) {
                return;
            }
        }

        history_queue.push(item);
        if (history_queue.length > 2) {
            history_queue.shift();
        }

        $.cookie("history_queue", JSON.stringify(history_queue), {
            expires: 7,
            path: '/'
        });
    }


})