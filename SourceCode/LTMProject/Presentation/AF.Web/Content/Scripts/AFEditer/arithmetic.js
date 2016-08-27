//<![CDATA[
function generateGuid() {
	var hex = new Array('0','1','2','3','4','5','6','7','8', '9','a','b','c','d','e','f');

	var outB = '';	
	for (count = 0; count < 32; count++) {
		if ((count == 8) || (count == 12) || (count == 16) || (count == 20))
			outB += '-';
			
		outB += hex[Math.floor(Math.random() * 16)];
	}
	
	return outB.toUpperCase();
}

function GetSelectionRange(selectionObject){
	if (selectionObject.getRangeAt)
		return selectionObject.getRangeAt(0);
	else { // 较老版本Safari
		var range = document.createRange();
		range.setStart(selectionObject.anchorNode,selectionObject.anchorOffset);
		range.setEnd(selectionObject.focusNode,selectionObject.focusOffset);
		return range;
	}
}

function GetPos(elem){
	var start = 0,end = 0;
	elem.focus();

	if($.browser.msie){
		var range = document.selection.createRange();
		var srcele = range.parentElement();
		var copy = document.body.createTextRange();
		copy.moveToElementText(srcele);
		var start = 0,end = 0;
		for (start = 0; copy.compareEndPoints("StartToStart", range) < 0; start++) {  
			copy.moveStart("character", 1);
		}  
		for (end = 0; copy.compareEndPoints('StartToEnd', range) < 0; end++)
			copy.moveStart('character', 1);

		end = start + end;
	} else {
		var userSelection = window.getSelection();
		var range = GetSelectionRange(userSelection);
		var copy = document.createRange();
		copy.selectNodeContents(elem);
		
		var length = $(elem).text().length;
		for (start = 0; start < length; start++) {
			copy.setStart(elem.firstChild,start);
			copy.setEnd(elem.firstChild,length);
			if (copy.compareBoundaryPoints(Range.START_TO_START, range) == 0)
			{
				break;
			}
		}
		for (end = 0; end < length; end++){
			copy.setStart(elem.firstChild,end);
			copy.setEnd(elem.firstChild,length);
			if (copy.compareBoundaryPoints(Range.END_TO_START, range) == 0)
			{
				break;
			}
		}
		end = start + (end - start);
	}
	return [start,end];
}

function Arithmetic(){
	this.track = [];
	this.focusElem = null;
	this.container = null;
	this.containerId = '';
	this.scoreId = '';
	if (typeof Arithmetic._initialized == 'undefined'){
		//c: 加题容器元素ID
		//s: 本题分数元素ID
		Arithmetic.prototype.Init = function(c,s){ 
			this.containerId = c;
			this.scoreId = s;
			this.container = $('#'+this.containerId);		
			this.track = [];
		}

		//operate: c:Create, d:Delete
		Arithmetic.prototype.Track = function(Operation, guid){
			if (guid == null){
				guid = generateGuid();
			}
			arithmetic.track.push({'guid':guid,'Operation':Operation});

			return guid;
		}

		Arithmetic.prototype.Bind = function(){
			$('#'+arithmetic.containerId+' div').unbind()
				.bind("focus", function(){arithmetic.focusElem=this;})
				.bind("keydown", function(event){arithmetic.KeyDown(event);})
				.bind("keyup", function(event){arithmetic.RejectSpecialChar(event,this);});

		}

		Arithmetic.prototype.DFN = function(){
			var guid = generateGuid();
			return '<div guid="'+guid+'" contentEditable="true"></div>';
		}

		Arithmetic.prototype.ModelOne = function(){
			if (arithmetic.track.length > 0){
				return false;
			}

			var guid = arithmetic.Track('c');

			var html = [];
			html.push('<table guid="'+guid+'" model="model_1" class="ShuShi bianji" cellpadding="0" cellspacing="0"><tbody>');
			html.push('<tr><td>'+arithmetic.DFN()+'</td></tr>');
			html.push('<tr><td>'+arithmetic.DFN()+arithmetic.DFN()+'</td></tr>');
			html.push('<tr class="line"><td>'+arithmetic.DFN()+'</td></tr>');
			html.push('</tbody></table>');

			arithmetic.container.append(html.join(''));
			arithmetic.Bind();
		}

		Arithmetic.prototype.ModelTwo = function(){
			if (arithmetic.track.length > 0){
				return false;
			}

			var guid = arithmetic.Track('c');

			var html = [];
			html.push('<table guid="'+guid+'" model="model_2" class="ShuShi bianji" cellpadding="0" cellspacing="0"><tbody>');
			html.push('<tr><td class="ShuShi-left"></td><td>'+arithmetic.DFN()+'</td></tr>');
			html.push('<tr class="line"><td class="ShuShi-chufa">'+arithmetic.DFN()+'</td><td>'+arithmetic.DFN()+'</td></tr>');
			html.push('<tr><td class="ShuShi-left"></td><td>'+arithmetic.DFN()+'</td></tr>');
			html.push('<tr class="line"><td class="ShuShi-left"></td><td>'+arithmetic.DFN()+'</td></tr>');
			html.push('</tbody></table>');

			arithmetic.container.append(html.join(''));
			arithmetic.Bind();
		}

		Arithmetic.prototype.LeftCell = function(){
			var dfn = arithmetic.focusElem;
			if (dfn != null){				
				var guid = arithmetic.Track('c');
				var html = '<div guid="'+guid+'" contentEditable="true"></div>';

				$(dfn).before(html);
				arithmetic.Bind();

				$(dfn).focus();
			}
		}

		Arithmetic.prototype.AnswerCell = function(){
			var dfn = arithmetic.focusElem;
			if (dfn != null){
				if ($(dfn).hasClass('daankuang')){
					$(dfn).removeClass('daankuang');
				} else {
					$(dfn).addClass('daankuang');
				}

				$(dfn).focus();
			}
		}

		Arithmetic.prototype.IncreaseRows = function(){
			var tbl = arithmetic.container.find('table');
			if (tbl.length > 0){
				tbl = $(tbl[0]);
				var model = tbl.attr('model');
				
				var guid = arithmetic.Track('c');

				var html = [];
				if (model == 'model_1') {
					html.push('<tr guid="'+guid+'"><td>'+arithmetic.DFN()+arithmetic.DFN()+'</td></tr>');
					html.push('<tr class="line" guid="'+guid+'"><td>'+arithmetic.DFN()+'</td></tr>');
				} else {
					html.push('<tr guid="'+guid+'"><td class="ShuShi-left"></td><td>'+arithmetic.DFN()+'</td></tr>');
					html.push('<tr class="line" guid="'+guid+'"><td class="ShuShi-left"></td><td>'+arithmetic.DFN()+'</td></tr>');
				}
				tbl.append(html.join(''));
				arithmetic.Bind();

				var dfn = arithmetic.focusElem;			
				if (dfn != null){
					$(dfn).focus();
				}
			}			
		}

		Arithmetic.prototype.UnDo = function(){
			if (arithmetic.track.length > 0){
				var obj = arithmetic.track[arithmetic.track.length-1];
				var guid = obj.guid;
				var operate = obj.Operation;

				var dfn = arithmetic.focusElem;
				if (operate == 'c') {				
					if (dfn != null){
						$('#'+arithmetic.containerId+' [guid="'+guid+'"]').each(function(i){
							var elem = $(this);
							if (elem.is('div')){
								if (elem.attr('guid') == dfn.getAttribute('guid')){
									arithmetic.focusElem = null;
								}
							} else {
								var dfns = elem.find('div');
								for(var i=0; i<dfns.length; i++){
									if (dfns[i].getAttribute('guid') == dfn.getAttribute('guid')){
										arithmetic.focusElem = null;
										break;
									}
								}
							}
						});
					}
					
					$('#'+arithmetic.containerId+' [guid="'+guid+'"]').remove();
				} else {
					$('#'+arithmetic.containerId+' [guid="'+guid+'"]').removeClass('delete');
				}

				if (dfn != null){
					$(dfn).focus();
				}

				arithmetic.track = $.grep(arithmetic.track, function(n,i){
					return i < arithmetic.track.length-1;
				});
			}			
		}

		Arithmetic.prototype.RejectNaN = function(input){
			input.value = input.value.replace(/\D/g,'');
		}

		Arithmetic.prototype.RejectSpecialChar = function(e,dfn){
			var oEvent = e || window.event;

			if (oEvent.keyCode >= 37 && oEvent.keyCode <= 40) // arrow key
				return;
			

			var tbl = $(dfn).parent().parent().parent().parent();			
			var model = tbl.attr('model');

			var n = '';
			if (model == 'model_1')
				n = dfn.innerHTML.replace(/[^0-9\+\-×\.]/g,'');
			else
				n = dfn.innerHTML.replace(/[^0-9\.]/g,'');

			if (n.length > 1){
				n = n.substring(0,1);
			}

			dfn.innerHTML = n;
			
			if(n.length == 1){
				if (!$.browser.msie){
					var selection = window.getSelection();
					selection.collapse(dfn, 1);
				}
				$(dfn).addClass('no-empty');
			} else {
				$(dfn).removeClass('no-empty');
			}
		}

		Arithmetic.prototype.KeyDown = function(e){
			var oEvent = e || window.event;
			
			switch(oEvent.keyCode){
				case 8:
					if($.browser.msie){
						oEvent.target = oEvent.srcElement;
					}
					var dfn = $(oEvent.target);
					
					var position = GetPos(dfn);
					if (position[0] == 0 && position[1] == 0){
						while (true){
							var prevDfn = dfn.prev();
							if (prevDfn != null){
								if (! prevDfn.hasClass('delete')){
									prevDfn.addClass('delete');
									arithmetic.Track('d', prevDfn.attr('guid'));
									break;
								} else {
									dfn = prevDfn;
								}								
							} else {
								break;
							}
						}						
					}
					
					break;
				case 13:
					if($.browser.msie){
						oEvent.target = oEvent.srcElement;
					}
					var dfn = $(oEvent.target);
					
					var tr = dfn.parent().parent();
					var tbl = tr.parent().parent();
					
					var model = tbl.attr('model');
					if (model == 'model_1'){
						var guid = arithmetic.Track('c');
						var	html = '<tr guid="'+guid+'"><td><div contentEditable="true"></div></td></tr>';
						
						tr.after(html);
						arithmetic.Bind();
					}
				break;
			}
		}

		Arithmetic.prototype.Parser = function(d){
			//var d = '{"score":0,"track":["48D4578A-5C30-CB9A-E18E-CB19CFD6FE59"],"html":{"g":"48D4578A-5C30-CB9A-E18E-CB19CFD6FE59","m":"model_1","trList":[{"tdList":[{"dfnList":[{"c":"no-empty","text":"1","isAnswer":0}]}]},{"tdList":[{"dfnList":[{"c":"no-empty","text":"+","isAnswer":0},{"c":"no-empty","text":"2","isAnswer":0}]}]},{"c":"line","tdList":[{"dfnList":[{"c":"no-empty daankuang","text":"3","isAnswer":1}]}]}]}}';			
			var data = jQuery.parseJSON(d);
			
			$('#'+arithmetic.scoreId).val(data.score);
			arithmetic.track = data.track;

			var htmlObject = data.html;
			var html = [];			
			html.push('<table guid="'+htmlObject.g+'" model="'+htmlObject.m+'" class="ShuShi bianji" cellpadding="0" cellspacing="0"><tbody>');
			var trList = htmlObject.trList;
			for(var i=0; i<trList.length; i++){
				var tr = trList[i];
				html.push('<tr ');
				if (tr.hasOwnProperty("c")) html.push('class="'+tr.c+'" ');
				if (tr.hasOwnProperty("g")) html.push('guid="'+tr.g+'" ');
				html.push('>');

				var tdList = tr.tdList;
				for(var j=0; j<tdList.length; j++){
					var td = tdList[j];
					html.push('<td ');
					if (td.hasOwnProperty("c")) html.push('class="'+td.c+'" ');
					html.push('>');

					var dfnList = td.dfnList;
					for (var k=0; k<dfnList.length; k++){
						var dfn = dfnList[k];
						html.push('<div guid="'+dfn.g+'" ');
						if (dfn.hasOwnProperty("c")) html.push('class="'+dfn.c+'" ');
						html.push();
						html.push('contentEditable="true" >');
						if (dfn.text != ''){
							html.push(dfn.text);
						}
						html.push('</div>');
					}

					html.push('</td>');
				}
				
				html.push('</tr>');
			}
			html.push('</tbody></table>');

			$('#'+arithmetic.containerId).html(html.join(''));
			arithmetic.Bind();			
		}

		Arithmetic.prototype.Store = function(){			
			var data = new Object();

			var tbl = arithmetic.container.find('table');
			if (tbl.length > 0){
				var score = $('#'+arithmetic.scoreId).val();
				if (score == ''){
					data.score = 0;
				} else {					
					data.score = score;
				}
				data.track = arithmetic.track;
				data.inputList = [];
				
				//html
				var counter=0;
				var htmlObject = new Object();

				var table = $(tbl[0]);
				htmlObject.g = table.attr('guid');
				htmlObject.m = table.attr('model');
				htmlObject.trList = [];

				var trList = table.find('tr');
				for(var i=0; i<trList.length; i++){
					var trObject = new Object();
					
					var tr = $(trList[i]);
					if(typeof(tr.attr("guid")) != "undefined"){
						trObject.g = tr.attr("guid");
					}

					if(typeof(tr.attr("class")) != "undefined"){
						trObject.c = tr.attr("class");												
					}

					trObject.tdList = [];
					//td list
					var tdList = tr.find('td');
					for(var j=0; j<tdList.length; j++){
						var tdObject = new Object();
						
						var td = $(tdList[j]);
						if(typeof(td.attr("class")) != "undefined"){
							tdObject.c = td.attr("class");
						}

						tdObject.dfnList = [];
						//dfn list						
						var dfnList = td.find('div');
						for (var k=0; k<dfnList.length; k++){
							var dfnObject = new Object();

							var dfn = $(dfnList[k]);
							dfnObject.g = dfn.attr("guid");

							if(typeof(dfn.attr("class")) != "undefined"){
								dfnObject.c = dfn.attr("class");												
							}

							dfnObject.text = dfn.html();
							
							if (dfn.hasClass('daankuang')){
								dfnObject.isAnswer = 1;
							} else {
								dfnObject.isAnswer = 0;
							}

							if (dfn.hasClass('delete')){
								dfnObject.isDelete = 1;
							} else {
								dfnObject.isDelete = 0;
							}

							tdObject.dfnList.push(dfnObject);

							if (dfnObject.isAnswer == 1 && dfnObject.isDelete == 0){
								data.inputList.push({'index':++counter,'answer':dfnObject.text});
							}						
						}
						//dfn end

						trObject.tdList.push(tdObject);
					}
					//td end

					htmlObject.trList.push(trObject);
				}
				data.html = htmlObject;
				//html end
			}

			return JSON.stringify(data);
		}
		
		Arithmetic._initialized = true;
	}
}
var arithmetic = new Arithmetic();
//]]>