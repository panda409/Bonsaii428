$(document).ready(function () {
    $("#longforSave").click(function () {
        $("#longfor_form").submit();
    });

    //针对checkbox设置
    $(".filld-in").change(function () {
        $(this).val($(this).is(":checked"));
    });
    
        $("#refresh").click(function () {
            window.location.reload();
        });

});

function bonsaii_back(path) {
    window.location.href = path;
}