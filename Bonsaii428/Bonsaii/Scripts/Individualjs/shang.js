$("#code").click(function () {
    var formParam = $("#verifyphone").serialize();//序列化表格内容为字符串

    alert("LINUX");
    $.ajax({

        type: 'POST',

        url: 'Account/GenerateVerifyCode',

        data: formParam,

        success: function (data) {
            alert(data);
        },

        dataType: 'json'

    });

    alert("STOP HER");
});