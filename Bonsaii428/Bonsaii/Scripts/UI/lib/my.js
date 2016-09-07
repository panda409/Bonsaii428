/*
    jQuery插件开发分为两种，一种是类级别的，就像$.ajax(...),相当于静态方法
    开发扩展方法是使用$.extend方法，即jQuery.extend(object);
    例子如下：
*/
$.extend({
    myFunc: function (str) {
        alert("FUCKING DAY = " + str);
    }
});
/*
    另一种是对象级别的，对象级别可以理解为基于对象的拓展，如$("#table").changeColor(...);这里这个changeColor,就是基于对象的拓展
    基于对象的拓展使用$.fn.extend方法，也就是jQuery.fn.extend(object);
*/
$.fn.extend({
    myCheck: function () {
        alert((this).attr("class"));
    }
});

