$(document).ready(function () {

    $(".down").click(function () {
        var span = $(this);
        var ul = span.next("ul");
        if (ul.find("li").length == 0) {
            $.post('/Catalog/CatalogTreeBranch',
            {
                id: span.attr("data-id")
            },
            function (data) {
                if (data.Result) {
                    ul.html(data.Object)
                }
                else {
                    alert(data.Message);
                }
            }, "json");
        }
    })

    $(".file-for-viewer a").click(function () {

        var fileurl = location.host + $(this).attr("href");
        var filename = $(this).attr("title");

        var altercontent = $("<h1 class='title'>" + filename + "</h1><iframe src='//docs.google.com/viewer?url=" + fileurl + "&embedded=true' style='width: 100%; height: 600px;' frameborder='0'>Ваш браузер не поддерживает фреймы</iframe>");

        $("#altercontent").html(altercontent).show();
        $("#content").hide();

        return false;
    })
});

