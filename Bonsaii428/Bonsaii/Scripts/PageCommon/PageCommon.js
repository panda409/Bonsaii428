/*展开、折叠效果*/
$(document).ready(function () {
$("#downchevron").click(function () {//点击2
$("#downchevron").hide();//2隐藏
$('#upchevron').show();//1显示
});
$("#upchevron").click(function () {//点击折叠1
$("#upchevron").hide();       //折叠为1。1隐藏，2显示
$('#downchevron').show();   //展开为2。
});
});
