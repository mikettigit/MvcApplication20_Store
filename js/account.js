$(document).ready(function () {

    $("<style>")
    .prop("type", "text/css")
        .html(".error {border: 1px solid #eb4848 !important; box-shadow: 0 0 10px #f00 !important;}")
        .appendTo("head");
    $("#RegisterButton").click(function () {

        isError = false;

        nameField = $("#Name");
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


        emailField = $("#Email");
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


        PasswordField = $("#Password");
        PasswordField.removeClass("error");

        passwordString = PasswordField.val();
        if (passwordString.length < 6) {
            PasswordField.addClass("error");
            isError = true;
        }


        PasswordConfirmField = $("#PasswordConfirm");
        PasswordConfirmField.removeClass("error");

        passwordConfirmString = PasswordConfirmField.val();
        if (passwordString != passwordConfirmString) {
            PasswordConfirmField.addClass("error");
            isError = true;
        }

        if (isError) {
            alert("Пожалуйста, заполните все поля корректно");
        } else {
            $.post('/Account/RegisterNew',
            {
                UserName: nameString,
                Email: emailString,
                Password: passwordString
            },
            function (data) {
                alert(data.Message);
                if (data.Result) {
                    document.location.href = "/" 
                }
            }, "json");
        }
    })

    $("#LoginButton").click(function () {

        isError = false;

        emailField = $("#Email");
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
            $.post('/Account/Logon',
            {
                Email: emailString,
                Password: $("#Password").val()
            },
            function (data) {
                if (data.Result) {
                    document.location.href = "/"
                }
                else {
                    alert(data.Message);
                }
            }, "json");
        }
    })


    $("#LogoutButton").click(function () {
        $.post('/Account/Logout',
            {              
            },
            function (data) {
                if (data.Result) {
                    document.location.href = "/"
                }
                else {
                    alert(data.Message);
                }
            }, "json");
        return false;
    })

});
