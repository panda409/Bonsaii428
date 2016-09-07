
$(function(){

// 	$.get("home.html",function(data){ //初始將home.html include div#iframe
// 　　　　$("#iframe").html(data);
// 　　});
// 　　$('.sider .nav li a.link').click(function() {
//   　　// 找出 li 中的超链接 href(#id)
//  　　　　var $this = $(this),
//  　　　　    _clickTab = $this.attr('target'); // 找到链接a中的targer的值
//  　　　　$.get(_clickTab,function(data){
//  　　　　　　$("#iframe").html(data);
//  　　　　});
// 　　});


// 滚动条
	$('.scroll-wrap').slimScroll({
        height: '100%',
        size: '5px'
    });

	$("[data-toggle='tooltip']").tooltip();
	
	$('ul.left-nav li:has(ul)').addClass('has-subnav');
	$('ul.nav > li').click(function () {
		$(this).addClass('active').siblings('li').removeClass('active');
	});

	$('ul.left-nav > li > a').click(function () {
		$(this).parent().siblings('li').find('ul').slideUp('fast');
		$(this).siblings('ul.sub').slideToggle('fast');

	});

	

	  $(".tree li:not(:has(ul))").find('> a > i').addClass('fa-square');
	    
	  $('.tree li:has(ul)').addClass('parent_li');
	
	  $('.parent_li > ul:visible').siblings('a').find('i').addClass('fa-minus-square');     //当ul可见的时候a前面加“-”
	
	  $('.parent_li > ul:hidden').siblings('a').find('i').addClass('fa-plus-square');      //当ul不可见的时候a前面加“+”
	
	  $('.tree li.parent_li > a').on('click', function (e) {
	    var children = $(this).parent('li.parent_li').find(' > ul');
	
	    if (children.is(":visible")) {
	      children.hide('fast');
	      $(this).find(' > i').addClass('fa-plus-square').removeClass('fa-minus-square');
	    } else {
	      children.show('fast');
	      $(this).find(' > i').addClass('fa-minus-square').removeClass('fa-plus-square');
	    }
	    e.stopPropagation();
	  });
	    
	    
	  $('.tree li a').click(function(){
	      $('.tree li a').removeClass("active")
	      $(this).addClass("active");
	  });
	
	  $('.tree li > a > i.fa-square').click(function(){
	      $(this).toggleClass("fa-check-square");
	      $('#btn2 #num-d').text('(' + $('.fa-check-square').length + ')')
	      $(this).siblings('span').text('选中了第' + $('.fa-check-square').length + '个')    //测试
	  });


});

