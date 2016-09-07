/**
 * qiao.util.js
 * 1.qser
 * 2.qdata
 * 3.qiao.on
 * 4.qiao.is
 * 5.qiao.ajax
 * 6.qiao.totop
 * 7.qiao.end
 * 8.qiao.cookie
 * 9.qiao.search
 * 10.qiao.juicer 
 */
define(function (require, exports, module) {
    'use strict';
    
    /**
	 * 将表单转为js对象
	 */
	$.fn.qser = function(){
		var obj = {};
		
		var objs = $(this).serializeArray();
		if(objs.length != 0){
			for(var i=0; i<objs.length; i++){
				obj[objs[i].name] = objs[i].value;
			}
		}
	
		return obj;
	};
	
	/** 
	 * 将data属性转为js对象
	 */
	$.fn.qdata = function(){
		var res = {};
		
		var data = $(this).attr('data');
		if(data){
			var options = data.split(';');
			for(var i=0; i<options.length; i++){
				if(options[i]){
					var opt = options[i].split(':');
					res[opt[0]] = opt[1];
				}
			}
		}
		
		return res;
	};
	
	/**
	 * qiao.on
	 * 事件绑定
	 */
	exports.on = function(obj, event, func){
		$(document).off(event, obj).on(event, obj, func);
	};
	
	/**
	 * qiao.is
	 * 一些常用的判断，例如数字，手机号等
	 */
	exports.is = function(str, type){
		if(str && type){
			if(type == 'number') return /^\d+$/g.test(str);
			if(type == 'mobile') return /^1\d{10}$/g.test(str);
		}
	};
	
	/**
	 * qiao.ajax
	 * 对$.ajax的封装
	 */
	exports.ajaxoptions = {
		url 		: '',
		data 		: {},
		type 		: 'post',
		dataType	: 'json',
		async 		: true,
		crossDomain	: false
	};
	exports.ajaxopt = function(options){
		var opt = $.extend({}, exports.ajaxoptions);
		if(typeof options == 'string'){
			opt.url = options;
		}else{
			$.extend(opt, options);
		}
		
		return opt;
	};
	exports.ajax = function(options, success, fail){
		if(options){
			var opt = exports.ajaxopt(options);
			
			$.ajax(opt).done(function(obj){
				if(success) success(obj);
			}).fail(function(e){
				if(fail){
					fail(e);
				}else{
					alert('数据传输失败，请重试！');
				}
			});
		}
	};
	
	/**
	 * qiao.totop
	 * 返回顶部的方法
	 * 可以参考：plugins/_01_qtotop/qtotop.html
	 */
	exports.totop = function(el){
		var $el = $(el);
		$el.hide().click(function(){
			$('body, html').animate({scrollTop : '0px'});
		});
		
		var $window = $(window);
		$window.scroll(function(){
			if ($window.scrollTop()>0){
				$el.fadeIn();
			}else{
				$el.fadeOut();
			}
		});
	};
	
	/**
	 * qiao.end
	 * 到达页面底部后自动加载内容
	 * end：到达底部后的回调函数
	 * $d：容器，默认是$(window)
	 * $c：内容，默认是$(document)
	 * 可以参考：plugins/_04_qend/qend.html
	 */
	exports.end = function(end, $d, $c){
		if(end){
			var $d = $d || $(window);
			var $c = $c || $(document);
			
			$d.scroll(function(){if($d.scrollTop() + $d.height() >= $c.height()) end();});
		}else{
			$(window).scroll(null);
		}
	};
	
	/**
	 * qiao.cookie
	 * 对jquery.cookie.js稍作封装
	 * 注：需要引入jquery.cookie.js，<script src="http://cdn.bootcss.com/jquery-cookie/1.4.1/jquery.cookie.min.js"></script>
	 * qiao.cookie(key)：返回key对应的value
	 * qiao.cookie(key, null)： 删除key对应的cookie
	 * qiao.cookie(key, value)：设置key-value的cookie
	 * 可以参考：plugins/_05_qcookie/qcookie.html
	 */
	exports.cookie = function(key, value){
		if(typeof value == 'undefined'){
			return $.cookie(key);
		}else if(value == null){
			$.cookie(key, value, {path:'/', expires: -1});
		}else{
			$.cookie(key, value, {path:'/', expires:1});
		}
	};
	
	/**
	 * qiao.search
	 * 获取url后参数中的value
	 * qiao.search(key)：返回参数中key对应的value
	 * 可以参考：plugins/_06_qsearch/qsearch.html
	 */
	exports.search = function(key){
		var res;
		
		var s = location.search;
		if(s){
			s = s.substr(1);
			if(s){
				var ss = s.split('&');
				for(var i=0; i<ss.length; i++){
					var sss = ss[i].split('=');
					if(sss && sss[0] == key) res = sss[1]; 
				}
			}
		}
		
		return res;
	};
	
	/**
	 * qiao.juicer
	 * 对juicer进行封装
	 */
	exports.juicer = function(el, data, callback){
		if(el){
			var $tpl = $(el);
			$tpl.after(juicer($tpl.html(), data));
			if(callback) callback();
		}
	};
});