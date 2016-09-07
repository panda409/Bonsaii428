(function(){
	var d=document,
		w=window,
		isIE=w.navigator.appVersion.indexOf("MSIE")>-1,
		now=new Date(),
		nowM=now.getMonth();
		nowY=now.getFullYear();
		date=null,
		ids=null,
		oInput=null;
	// document.write('<iframe frameborder=0 style="display:none;position:absolute;" width="500" height="415" scrolling="no" name="sangcalender" id="sangcalender"></iframe>');
	//var f=window.frames['sangcalender'];
	//var ff=d.getElementById('sangcalender');
	var fd=document;
	fd.open();
	//fd.write('<style type="text/css">#yearL, #monthL, #monthR, #yearR, #hoursL, #hoursR, #minL, #minR, #y, #m, #h, #i, #s{cursor:pointer;}.calenderClose a{display:block;width:13px;line-height:13px;border:1px solid #cccccc;background-color:#eeeeee;color:#666; text-decoration:none}.calenderClose a:hover{color:red}td{text-align:center}#calenderDay{cursor:pointer}body{font-size:12px;padding:0;margin:0}.col666{color:#999}.bg9ebdd6{background-color:#9ebdd6}</style>')
	//fd.write('<table class="table table-border" id="table1" style="display:none;table-layout:fixed;">' +
	//		 '<tr><td><table border="0" cellspacing="0" bgcolor="#6699FF" cellpadding="0" width="100%">'+
	//		 '<tr><td width="20" height="25" align="center" id="yearL" title="上一年">&lt;&lt;</td>'+
	//		 '<td width="12" align="center" id="monthL" title="上一月">&lt;</td><td align="center">'+
	//		 '<span id="y" title="点击选择年份"></span>年<span id="m" title="点击选择月份"></span>月</td>'+
	//		 '<td width="12" align="center" id="monthR" title="下一月">&gt;</td>'+
	//		 '<td width="20" align="center" id="yearR" title="下一年">&gt;&gt;</td></tr></table></td></tr>'+
	//		 '<tr bgcolor="#FFFFFF"><td><table class="table table-border">' +
	//		 '<tr bgcolor="#CCCCFF" height="18"><td align="center">日</td><td align="center">一</td><td align="center">二</td><td align="center">三</td><td align="center">四</td><td align="center">五</td><td align="center">六</td></tr></table></td></tr>' +
	//		 '<tr bgcolor="#FFFFFF"><td><div id="calenderDay"></div></td></tr>'+
	//		 '<tr><td><table class="table table-border" hidden="hidden">' +
	//		 '<tr><td align="center" height="20" id="hoursL" title="时减少">&lt;&lt;</td>'+
	//		 '<td align="center" id="minL" title="分减少">&lt;</td>'+
	//		 '<td align="center"><span id="h" title="点击选择小时"></span>:<span id="i" title="点击选择分"></span>:<span id="s" title="点击选择秒"></span></td>'+
	//		 '<td align="center" id="minR" title="分增加">&gt;</td><td align="center" id="hoursR" title="时增加">&gt;&gt;</td></tr>'+
	//		 '</table></td></tr></table>');
	// fd.write('</body></html>');
	fd.close();

	//获取框架里的元素  gids.call(obj,id)
	function gids(idArr){
		var oId=[];
		for(var i=0,len=idArr.length;i<len;i++){
			oId[idArr[i]]=this.getElementById(idArr[i]);
		}
		return oId;
	}
	//需要添加事件的元素的集合
	var idsArr=['yearL','yearR','y','m','monthL','monthR','hoursL','hoursR','minL','minR','calenderClose','calenderDay','h','i','s'];
	if(!ids){ids=gids.call(fd,idsArr)};
	//格式化日历控件
	//function formatDay(timestr){
	//	var t=/(\d+)-(\d+)-(\d+)\s*(\d*):?(\d*):?(\d*)/.exec(timestr);
	//	var da=null;
	//	var dn=getdate(now);
	//	if(t){
	//		da=new Date(t[1],t[2]-1,1,t[4],t[5],t[6]);
	//	}else{
	//		da=new Date(dn['y'],dn['m'],1,dn['h'],dn['i'],dn['s']);
	//	}
	//	date=getdate(da);
	//	var mon=[31,date['y']%4==0?29:28,31,30,31,30,31,31,30,31,30,31];
	//	var str = "", day = 1;
	//	var i = 1
	//	str += '<table class="table table-border">';
	//	for(var md=mon[date['m']-1],wd=md-date['w']+1,n=0;n<6;n++){
	//		str+='<tr bgcolor="#ffffff" height="23">';
	//		for(var nn=0;nn<7;nn++){
	//			if(wd<=md){
	//			    str+='<td class="col666">'+wd+'</td>';
	//			    str += '<td class="col666"></td>';
	//				wd++;
	//			}else {
	//				if(day<=mon[date['m']]){
	//					if(day==dn['d'] && nowM==now.getMonth()&&nowY==now.getFullYear()){
	//						str+='<td class="bg9ebdd6">'+(day++)+'</td>';
	//					}else{
	//					    if (nn % 6 == 0)
	//					    {
                               
	//					        str += '<td>' + (day++) + '<br><div id="id' + (i++) + '">' + '休息' + '</div></td>';
	//					    }
	//						else
	//							str+='<td id="id'+day+'">'+(day++)+'</td>';
	//					}
	//					var day2=1;
	//				}else{
	//				    str += '<td class="col666"></td>';
	//				    str+='<td class="col666">'+(day2++)+'</td>';
	//				}
	//			}
	//		}
	//		str+='</tr>';
	//	}
	//	str+='</table>';
	//	ids['calenderDay'].innerHTML=str;
	//	var dates=[date['y'],fillzero(date['m']+1),fillzero(date['h']),fillzero(date['i']),fillzero(date['s'])];
	//	each.call([ids['y'],ids['m'],ids['h'],ids['i'],ids['s']],function(o,i){o.innerHTML=dates[i]});
	//	each.call(ids['calenderDay'].getElementsByTagName("td"),function(o,i){
	//		addEvent(o,"mouseover",function(e){
	//			o.style.backgroundColor="#9ebdd6";
	//		})
	//		addEvent(o,"mouseout",function(e){
	//			o.style.backgroundColor="";
	//		})
	//		addEvent(o,"click",function(e){
	//			if(o.className=="col666"){return}
	//			oInput.value=ids['y'].innerHTML+"-"+ids['m'].innerHTML+"-"+ fillzero(o.innerHTML) 
	//						+" "+ids['h'].innerHTML+":"+ids['i'].innerHTML+":"+ids['s'].innerHTML;
	//			hide();
	//		})
	//	})
	//}

	function formatDay(week1, week2, week3, week4, week5, week6, week7,timestr) {
	    var t = /(\d+)-(\d+)-(\d+)\s*(\d*):?(\d*):?(\d*)/.exec(timestr);
	    var da = null;
	    var dn = getdate(now);
	   // var temp = new Array(0, 0, 0, 0, 0, 0, 1);
	    var temp = new Array();
	    temp[0]=week7;
	    temp[1]=week1;
	    temp[2]=week2;
	    temp[3]=week3;
	    temp[4]=week4;
	    temp[5]=week5;
	    temp[6]=week6;
	  
	    if (t) {
	        da = new Date(t[1], t[2] - 1, 1, t[4], t[5], t[6]);
	    } else {
	        da = new Date(dn['y'], dn['m'], 1, dn['h'], dn['i'], dn['s']);
	    }
	    date = getdate(da);
	    var mon = [31, date['y'] % 4 == 0 ? 29 : 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31];
	    var str = "", day = 1;
	    var i = 1;
	    str += '<table class="table table-border" align="center" style="table-layout:fixed;">';
	    //if (date['m'] == 0)
	    //{
	    //    var date1 = new Date(2014, 11, '01');
	    //    //设置日期 
	    //    date1.setDate(1);
	    //    //设置月份 
	    //    date1.setMonth(date1.getMonth() + 1);
	    //    //获取本月的最后一天 
	    //    cdate = new Date(date1.getTime() - 1000 * 60 * 60 * 24);
	    //    //打印某年某月的最后一天 
	    //   // alert(cdate.getFullYear() + "年" + (Number(cdate.getMonth()) + 1) + "月最后一天的日期:" + cdate.getDate() + "日" + cdate.getDay());
	       
	    //    date['w'] = cdate.getDay();
	    //}

	    for (var md = mon[date['m']], wd = md - date['w'] + 1, n = 0; n < 6; n++) {
	        str += '<tr bgcolor="#ffffff" height="23">';
	        for (var nn = 0; nn < 7; nn++) {
	            if (wd <= md) {
	                //str+='<td class="col666">'+wd+'</td>';
	                str += '<td class="col666"></td>';
	                wd++;
	            } else {
	                if (day <= mon[date['m']]) {
	                    if (day == dn['d'] && nowM == now.getMonth() && nowY == now.getFullYear()) {
	                        str += '<td align="center" class="bg9ebdd6" id="id' + day + '">' + (day++) + '</td>';
	                    } else {
	                        if (nn % 6 == 0)
	                            str += '<td align="center" id="id' + day + '">' + (day++) + '</td>';
	                        else
	                            str += '<td align="center" id="id' + day + '">' + (day++) + '</td>';
	                        //if (temp[nn]=="1") {

	                        //    str += '<td>' + (day++) + '<br><div id="idid' + (i++) + '">' + '休息' + '</div></td>';
	                        //}
	                        //else
	                        //    str += '<td>' + (day++) + '<br><div id="id' + n+nn + '"></div></td>';
	                            //str += '<td id="id' + day + '">' + (day++) + '</td>';
	                    }
	                    var day2 = 1;
	                } else {
	                    str += '<td align="center" class="col666"></td>';
	                    //str+='<td class="col666">'+(day2++)+'</td>';
	                }
	            }
	        }
	        str += '</tr>';
	    }
	    str += '</table>';
	    ids['calenderDay'].innerHTML = str;
	    var dates = [date['y'], fillzero(date['m'] + 1), fillzero(date['h']), fillzero(date['i']), fillzero(date['s'])];
	    each.call([ids['y'], ids['m'], ids['h'], ids['i'], ids['s']], function (o, i) { o.innerHTML = dates[i] });
	    each.call(ids['calenderDay'].getElementsByTagName("td"), function (o, i) {
	        addEvent(o, "mouseover", function (e) {
	            o.style.backgroundColor = "#9ebdd6";
	        })
	        addEvent(o, "mouseout", function (e) {
	            o.style.backgroundColor = "";
	        })
	        addEvent(o, "click", function (e) {
	            if (o.className == "col666") { return }
	            oInput.value = ids['y'].innerHTML + "-" + ids['m'].innerHTML + "-" + fillzero(o.innerHTML)
							+ " " + ids['h'].innerHTML + ":" + ids['i'].innerHTML + ":" + ids['s'].innerHTML;
	            hide();
	        })
	    })
	}
	
	//为按钮添加事件
	var handlers=[yL,yR,mL,mR,hL,hR,iL,iR];
	each.call([ids['yearL'],ids['yearR'],ids['monthL'],ids['monthR'],ids['hoursL'],ids['hoursR'],ids['minL'],ids['minR']],function(o,i){
		addEvent(o,"click",handlers[i]);
	})
	
	var clicks=[yClick,mClick,hClick,iClick,sClick];
	each.call([ids['y'],ids['m'],ids['h'],ids['i'],ids['s']],function(o,i){
		addEvent(o,"click",clicks[i]);
	})
	
	//获取元素位置
	function getPos(e){
		var x,y,e=typeof e=="string"?d.getElementById(e):e,p=[];
		x=e.offsetLeft;
		y=e.offsetTop;
		while(e=e.offsetParent){
			x+=e.offsetLeft;
			y+=e.offsetTop;
		}
		p['x']=x;p['y']=y;
		return p;
	}
	
	//上一年
	function yL(){
		now.setFullYear(date['y']-1);
		formatDay();
	}
	
	//下一年
	function yR(){
		now.setFullYear(date['y']+1);
		formatDay();
	}
	
	//上一月
	function mL(){
		now.setMonth(date['m']-1);
		formatDay();
	}
	
	//下一月
	function mR(){
		now.setMonth(date['m']+1);
		formatDay();
	}
	
	//时增加
	function hR(){
		now.setHours(date['h']+1);
		formatDay();
	}
	
	//时减少 
	function hL(){
		now.setHours(date['h']-1);
		formatDay();
	}
	
	//分增加
	function iR(){
		now.setMinutes(date['i']+1);
		formatDay();
	}
	
	//分减少
	function iL(){
		now.setMinutes(date['i']-1);
		formatDay();
	}
	
	//为SELECT添加事件
	function addEventForSelect(type){
		function changeInner(){
			ids[type].innerHTML=fillzero(oSelect.value);
			now.setFullYear(ids['y'].innerHTML);
			now.setMonth(Number(ids['m'].innerHTML)-1);
			now.setHours(ids['h'].innerHTML);
			now.setMinutes(ids['i'].innerHTML);
			now.setSeconds(ids['s'].innerHTML);
			formatDay();
		}
		var oSelect=gids.call(fd,['calenderSelect'])['calenderSelect'];
		oSelect.focus();
		addEvent(oSelect,'change',changeInner);
		addEvent(oSelect,"blur",changeInner);
	}
	
	//生成option选项
	function createOption(type,v){
		var str='',str2='',i=0,i2=0;
		function create(i,i2){
			while(i>=i2){
				if(v==i){
					str2+='<option value="'+i+'" selected>'+i+'</option>';
				}else{
					str2+='<option value="'+i+'">'+i+'</option>';
				}
				i--;
			}
			str+=str2+'</select>';
			ids[type].innerHTML=str;
			addEventForSelect(type);
		}
		str+='<select id="calenderSelect">';
		if(type=="y"){
			i=2099;i2=1990;
			create(i,i2);
			return;
		}
		if(type=="m"){
			i=1;i2=12;
		}
		if(type=="h"){
		   i=00;i2=23;
		}
		if(type=="i"){
			i=00;i2=59;
		}
		if(type=="s"){
			i=00;i2=59;
		}
		create(i2,i);
	}
	
	//年鼠标点击
	function yClick(e){
		if(getTarget(e).tagName.toLowerCase()=="span"){
			createOption("y",ids['y'].innerHTML);
		}
	}
	
	//月鼠标点击
	function mClick(e){
		if(getTarget(e).tagName.toLowerCase()=="span"){
			createOption("m",ids['m'].innerHTML);
		}
	}
	
	//时鼠标点击
	function hClick(e){
		if(getTarget(e).tagName.toLowerCase()=="span"){
			createOption("h",ids['h'].innerHTML);
		}
	}
	
	//分鼠标点击
	function iClick(e){
		if(getTarget(e).tagName.toLowerCase()=="span"){
			createOption("i",ids['i'].innerHTML);
		}
	}
	
	//秒鼠标点击
	function sClick(e){
		if(getTarget(e).tagName.toLowerCase()=="span"){
			createOption("s",ids['s'].innerHTML);
		}
	}
	
	//each方法
	function each(handler){
		var o=null;
		for(var i=0,len=this.length;i<len;i++){
			o=typeof this[i]=="string"?fd.getElementById(this[i]):this[i];
			handler(o,i);
		}
	}
	
	//如果日期为一位数，则在前面补零
	function fillzero(str){
		var str=typeof str=="string"?str:str.toString();
		if(str.length==1){
			str="0"+str;
		}
		return str;
	}
	
	//获取时间
	function getdate(da){
		var date=[];
		date['y']=da.getFullYear();
		date['m']=da.getMonth();
		date['d']=da.getDate();
		date['w']=da.getDay();
		date['h']=da.getHours();
		date['i']=da.getMinutes();
		date['s']=da.getSeconds();
		return date;
	}
	
	//阻止默认事件
	function preventDefault(e){
		var e=e||window.event;
		if(e.preventDefault){
			e.preventDefault();
		}else{
			e.returnValue=false;
		}
	}
	
	function getTarget(e){
		var e=e||window.event;
		return e.srcElement||e.target;
	}
	
	//显示日历控件
	function show(week1, week2, week3, week4, week5, week6, week7){
		//var s=o.value;
		//var p=getPos(o);
		if(week7){
		    //formatDay(s);
		    formatDay( week1, week2, week3, week4, week5, week6, week7);
		}else{
			now=new Date();
			formatDay();
		}
		//ff.style.left=p['x']+"px";
		// ff.style.top=p['y'] + o.offsetHeight + "px";
		// ff.style.display="block";
	}
	
	//隐藏日历控件
	function hide(){
		// ff.style.display="none";
	}
	
	//添加事件
	function addEvent(element,event,handler){
		var element=typeof element=="string"?d.getElementById(element):element;
		if(element.addEventListener){
			element.addEventListener(event,handler,false)
		}else if(element.attachEvent){
			element.attachEvent("on"+event,handler);
		}else{
			element["on"+event]=handler;
		}
	}
	
	//获取要实现控件的表单
	function getObj(className){
		var o=d.getElementsByTagName('*'),oArr=[];
		for(var i=0,len=o.length;i<len;i++){
			if(o[i].className==className){
				oArr.push(o[i])
			}
		}
		return oArr;
	}
	
	each.call(getObj("sang_Calender"), function (o, week1, week2, week3, week4, week5, week6, week7,i) {
	    addEvent(o, "click", function (e) { preventDefault(e); oInput = o, show(week1, week2, week3, week4, week5, week6, week7); fd.focus();})
	})
	
	//var iframeObj=isIE?ff:f;
	//addEvent(isIE?ff:f,"blur",function(e){hide()})
})()
