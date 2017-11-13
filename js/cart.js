$(document).ready(function () {

    UpdateTotalCount();

    $("#button-cart").click(function () {
        $.post('/Cart/AddItem',
        {
            ItemURL: $("#relativeurl").attr("href"),
            count: $("#input-quantity").val(),
            update: false
        },
        function (data) {
            alert(data.Message);
            UpdateTotalCount();
        }, "json");
    })

    $(document).on("change", ".item_quatity", function () {
        var parent = $(this).parents(".btn-block.quantity");
        parent.find(".cartitemupdate").click();
    })

    $(document).on("click", ".cartitemupdate", function () {
        var parent = $(this).parents(".btn-block.quantity");
        var cart_item = parent.find(".relative_url").val();
        var count = parent.find(".item_quatity").val();
        $.post('/Cart/AddItem',
        {
            ItemURL: cart_item,
            count: count,
            update: true
        },
        function (data) {
            UpdateTotalCount();
            UpdateTotalAmount();
            if (data.Result) {
                parent.parents("tr").find(".cartitemsubtotal").html(data.Object);
            }
        }, "json");
    })

    $(document).on("click", ".cartitemdelete", function () {
        var parent = $(this).parents(".btn-block.quantity");
        var cart_item = parent.find(".relative_url").val();
        $.post('/Cart/DelItem',
        {
            ItemURL: cart_item
        },
        function (data) {
            alert(data.Message);
            UpdateTotalCount();
            UpdateTotalAmount();
            if (data.Result) {
                parent.parents("tr").remove();
            }
        }, "json");
    })

    $("<style>")
    .prop("type", "text/css")
        .html(".error {border: 1px solid #eb4848 !important; box-shadow: 0 0 10px #f00 !important;}")
        .appendTo("head");
    $(".SendOrder").click(function () {

        if ($(".btn-block.quantity").length == 0) {
            alert("Не выбраны товары");
        }
        else {

            isError = false;

            nameField = $("#username");
            nameField.removeClass("error");

            nameChars = " -ёйцукенгшщзхъфывапролджэячсмитьбюЁЙЦУКЕНГШЩЗХЪФЫВАПРОЛДЖЭЯЧСМИТЬБЮ";;
            nameString = nameField.val();
            if (nameString.length < 3) {
                nameField.addClass("error");
                isError = true;
            }
            else {
                i = 0;
                while (ch = nameString.substr(i, 1)) {
                    if (nameChars.indexOf(ch) == -1) {
                        nameField.addClass("error");
                        isError = true;
                        break;
                    }
                    i++;
                }
            }


            phoneField = $("#userphone");
            phoneField.removeClass("error");

            phoneChars = " +-()1234567890";
            phoneString = phoneField.val();
            if (phoneString.length < 10) {
                phoneField.addClass("error");
                isError = true;
            }
            else {
                i = 0;
                while (ch = phoneString.substr(i, 1)) {
                    if (phoneChars.indexOf(ch) == -1) {
                        phoneField.addClass("error");
                        isError = true;
                        break;
                    }
                    i++;
                }
            }

            emailField = $("#useremail");
            emailField.removeClass("error");

            emailChars = "_-.@~qwertyuiopasdfghjklzxcvbnmQWERTYUIOPASDFGHJKLZXCVBNM0123456789";
            emailString = emailField.val();
            if (emailString.length < 6) {
                emailField.addClass("error");
                isError = true;
            }
            else {
                i = 0;
                HasAt = false;
                while (ch = emailString.substr(i, 1)) {
                    if (emailChars.indexOf(ch) == -1) {
                        emailField.addClass("error");
                        isError = true;
                        break;
                    }
                    if (ch == "@") {
                        HasAt = true;
                    }
                    i++;
                }
                if (!HasAt) {
                    emailField.addClass("error");
                    isError = true;
                }
            }

            if (isError) {
                alert("Пожалуйста, заполните все поля корректно");
            } else {
                $.post('/Cart/SendOrder',
                {
                    name: nameString,
                    phone: phoneString,
                    email: emailString
                },
                function (data) {
                    if (data.Result) {
                        alert("Заказ отправлен. Ожидайте ответа нашего сотрудника по указанным вами данным для обратной связи");
                    }
                    else {
                        alert(data.Message);
                    }
                }, "json");
            }
        }
    })
});

function UpdateTotalCount() {
    $.post('/Cart/GetTotalCount',
    {
    },
    function (data) {
        $("#cart-total").html(data.Object);
    }, "json");
}

function UpdateTotalAmount() {
    $.post('/Cart/GetTotalAmount',
    {
    },
    function (data) {
        $("#total_amount").html(data.Object);
    }, "json");
};