/**
 * qmask遮罩
 * @version 0.0.1.20141020
 * @history
 * 	0.0.1.20141020
 */
function Qmask(options){
	var o = new Object();
	
	o.background = document.createElement('div');
	o.background.style.cssText = 'display:none;position:absolute;top:0;left:0;width:100%;height:100%;background-color:white;z-index:2001;';
	
	o.loading = document.createElement('img');
	o.loading.src = 'qmask-1.0.gif';
	o.loading.style.cssText = 'display:none;position:absolute;top:50%;left:50%;width:200px;height:200px;margin:-100px 0 0 -100px;z-index:2002;overflow:auto;';
	
	o.show = function(){
		this.background.style.display = 'block';
		this.loading.style.display = 'block';
	};
	o.hide = function(){
		this.background.style.display = 'none';
		this.loading.style.display = 'none';
	};
	
	if(options){
		if(options.hasOwnProperty('bgcss')) o.background.style.cssText = options.bgcss;
		if(options.hasOwnProperty('loadingcss')) o.loading.style.cssText = options.loadingcss;
		if(options.hasOwnProperty('loadingimg')) o.loading.src = base + options.loadingimg;
	}
	
	document.body.appendChild(o.background);
	document.body.appendChild(o.loading);
	
	return o;
}
var qmask = new Qmask(); 
qmask.show();