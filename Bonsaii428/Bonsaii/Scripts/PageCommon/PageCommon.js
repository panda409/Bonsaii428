/*չ�����۵�Ч��*/
$(document).ready(function () {
$("#downchevron").click(function () {//���2
$("#downchevron").hide();//2����
$('#upchevron').show();//1��ʾ
});
$("#upchevron").click(function () {//����۵�1
$("#upchevron").hide();       //�۵�Ϊ1��1���أ�2��ʾ
$('#downchevron').show();   //չ��Ϊ2��
});
});
