var currentEditor = "";
var AFEditor = function (TMDOM) {
    var tool = AFEditor.tool;
    this.TMDOM = TMDOM;

    this.bold = function (editorId) {
        tool.setFontStype("b", document.getElementById(editorId));
    }
    this.italic = function (editorId) {
        tool.setFontStype("i", document.getElementById(editorId));
    }
    this.underline = function (editorId) {
        tool.setFontStype("u", document.getElementById(editorId));
    }
    this.sub = function (editorId) {
        tool.setFontStype("sub", document.getElementById(editorId));
    }
    this.sup = function (editorId) {
        tool.setFontStype("sup", document.getElementById(editorId));
    }
    this.yinbiao = function (editorId) {
        tool.setFontStype("yb", document.getElementById(editorId));
    }
    this.choiceNumber = function (editorId) {
        tool.UBB_insertInput(document.getElementById(editorId), 2);
    }
    this.justifyleft = function (editorId) {
        tool.UBB_justifyleft(document.getElementById(editorId));
    }
    this.justifycenter = function (editorId) {
        tool.UBB_justifycenter(document.getElementById(editorId));
    }
    this.justifyright = function (editorId) {
        tool.UBB_justifyright(document.getElementById(editorId));
    }
    this.symbol = function (editorId) {
        currentEditor = editorId;
        tool.UBB_insertsymbol(document.getElementById(editorId));
    }
    this.image = function (editorId) {
        currentEditor = editorId;
        tool.UpFile(editorId, TMDOM.TMID);
    },
    this.multipleResult = function (editorId) {
        currentEditor = editorId;
        tool.UBB_insertMultipleResult(document.getElementById(editorId), 0);
    }
    this.online = function (editorId) {
        currentEditor = editorId;
        tool.UBB_OnLine(document.getElementById(editorId), 0);
    }
    this.shushi = function (editorId) {
        currentEditor = editorId;
        tool.UBB_Shushi(document.getElementById(editorId), this.TMDOM.TMID);
    }
    this.febox = function (editorId) {
        currentEditor = editorId;
        tool.UBB_insertMathML(document.getElementById(editorId));
    }
    this.brace = function (editorId, o) {
        tool.UBB_insertBrace(document.getElementById(editorId));
    },
    this.setImgWH = function (returnValue) {
        var ele = document.getElementById(currentEditor);
        var Arr = [0, 0];
        if (ele == null) {
            return;
        }
        if (returnValue) {
            ele.focus();
            Arr = tool.savePos(ele);
            var pre = ele.value.substr(0, Arr[0]);
            var post = ele.value.substr(Arr[1]);
            ele.value = pre + returnValue + post;
            tool.setPosition(ele, Arr[0], Arr[0] + returnValue.length);
        }
    }
    this.clearHtml = function (editorId) {
        var txtEditor = $("#" + editorId).val();
        var reg = new RegExp("\\[\/?font(=.*?)?\\]|\\[\/?size(=.*?)?\\]|\\[\/?textstyle(=.*?)?\\]|\\[\/?align(=.*?)?\\]", "img");
        txtEditor = txtEditor.replace(reg, '');
        $("#" + editorId).val(txtEditor);
    }
    this.focus = function (editorId) {
        var curObj = document.getElementById(editorId);
        currentEditor = editorId;
        curObj.style.posHeight = curObj.scrollHeight;
    }
    this.fontFamily = function (editorId, o, pageType) {
        if ($("#_editorFactory_").is(":visible") && $("#_editorFactory_").attr('tagid') == 'fontFamily' + editorId) {
            $("#_editorFactory_").hide();
            return;
        }
        var fontF = ['宋体', '仿宋', '楷体', '黑体', '幼圆', '微软雅黑', 'Verdana', 'Arial', 'Times New Roman'];
        var font = ['宋体', ['仿宋', '仿宋_GB2312'], ['楷体', '楷体_GB2312'], '黑体', '幼圆', '微软雅黑', 'Verdana', 'Arial', 'Times New Roman'];
        var html = [];
        html.push("<dl class=\"font-family\">");
        for (var i = 0; i < font.length; i++) {
            var txtFont = '';
            if ($.isArray(font[i])) {
                txtFont = font[i].join("','");
            } else {
                txtFont = font[i];
            }

            html.push("	<dd><a href=\"javascript:;\" style=\"font-family:'" + txtFont + "'\" onclick=\"AFEditor.tool.UBB_CFont('" + font[i] + "','" + editorId + "');$('#_editorFactory_').hide();\">" + fontF[i] + "</a></dd>");
        }
        html.push("</dl>");

        var lt = $(o).offset();
        //获取浏览器显示区域的高度、宽度
        var h = $(window).height();
        var w = $(window).width();
        var ph = $(document).height();
        var pw = $(document).width();
        if (pageType === "topic") {
            $("#_editorFactory_").attr('tagid', 'fontFamily' + editorId).css({ "left": pw * 0.0833, "top": lt.top - (ph * 0.21) }).html(html.join('\r\n')).show();
        } else {
            $("#_editorFactory_").attr('tagid', 'fontFamily' + editorId).css({ "left": lt.left, "top": lt.top + 25 }).html(html.join('\r\n')).show();
        }
    },
    this.fontSize = function (editorId, o) {
        if ($("#_editorFactory_").is(":visible") && $("#_editorFactory_").attr('tagid') == 'fontSize' + editorId) {
            $("#_editorFactory_").hide();
            return;
        }
        var size = [10, 12, 14, 16, 18, 24, 32];
        var html = [];
        html.push("<dl class=\"font-size\">");
        for (var i = 0; i < size.length; i++) {
            html.push("	<dd style=\"height:" + (size[i] + 10) + "px;\"><a href=\"javascript:;\" style=\"font-size:" + size[i] + "px;\" onclick=\"AFEditor.tool.UBB_CFontSize(" + size[i] + ",'" + editorId + "');$('#_editorFactory_').hide();\">" + size[i] + "px</a></dd>");
        }
        html.push("</dl>");
        var lt = $(o).offset();
        $("#_editorFactory_").attr('tagid', 'fontSize' + editorId).css({ "left": lt.left, "top": lt.top + 25 }).html(html.join('\r\n')).show();
    },
    this.fontColor = function (editorId, o) {
        if ($("#_editorFactory_").is(":visible") && $("#_editorFactory_").attr('tagid') == 'fontColor' + editorId) {
            $("#_editorFactory_").hide();
            return;
        }
        var color = ['#E53333', '#E56600', '#FF9900', '#64451D', '#DFC5A4', '#FFE500', '#009900', '#006600', '#FF9900', '#B8D100', '#60D978', '#00D5FF', '#337FE5', '#003399', '#4C33E5', '#9933E5', '#CC33E5', '#EE33EE', '#FFFFFF', '#CCCCCC', '#999999', '#666666', '#333333', '#000000'];
        var html = [];
        html.push("<dl class=\"font-color\">");
        //html.push("<dt><a href=\"javascript:;\">无颜色</a></dt>");
        for (var i = 0; i < color.length; i++) {
            html.push("<dd><a href=\"javascript:;\" style=\"background-color:" + color[i] + ";\" title=\"" + color[i] + "\" onclick=\"AFEditor.tool.UBB_CFontColor('" + color[i] + "','" + editorId + "');$('#_editorFactory_').hide();\"></a></dd>");
        }
        html.push("</dl>");
        var lt = $(o).offset();
        $("#_editorFactory_").attr('tagid', 'fontColor' + editorId).css({ "left": lt.left, "top": lt.top + 25 }).html(html.join('\r\n')).show();
    },
    this.textStyle = function (editorId, o) {
        if ($("#_editorFactory_").is(":visible") && $("#_editorFactory_").attr('tagid') == 'textStyle' + editorId) {
            $("#_editorFactory_").hide();
            return;
        }
        var styleEN = ["dian", "sanjiao", "double", "border"];
        var styleCN = ["加点字", "加三角字", "双横线", "方框文字"];
        var html = [];
        html.push("<dl class=\"font-textstyle\">");
        for (var i = 0; i < styleEN.length; i++) {
            html.push("	<dd><a href=\"javascript:;\" class=\"" + styleEN[i] + "\" onclick=\"AFEditor.tool.UBB_CTextStyle('" + styleEN[i] + "','" + editorId + "');$('#_editorFactory_').hide();\">" + styleCN[i] + "</a></dd>");
        }
        html.push("</dl>");

        var lt = $(o).offset();
        $("#_editorFactory_").attr('tagid', 'textStyle' + editorId).css({ "left": lt.left, "top": lt.top + 25 }).html(html.join('\r\n')).show();
    }
    this.init = AFEditor.init;
};

AFEditor.tool = {
    selectAllow: function () {
        //if (window.event.srcElement.tagName=="INPUT") {SelectAllow=false;return}
        //if (window.event.srcElement!=UBBTextArea[0]) {SelectAllow=false;return false}
        return true;
    },
    savePos: function (textBox) {
        var start = 0, end = 0;
        if (textBox.tagName == 'INPUT') {
            if (typeof (textBox.selectionStart) == "number") {
                start = textBox.selectionStart;
                end = start;
            }
            else if (document.selection) {
                var s = document.selection.createRange();
                s.setEndPoint("StartToStart", textBox.createTextRange());
                start = s.text.length;
                end = start;
            }
        }
        else {
            if (typeof (textBox.selectionStart) == "number") {
                start = textBox.selectionStart;
                end = textBox.selectionEnd;
            }
            else if (document.selection) {
                var range = document.selection.createRange();
                if (range.parentElement().id == textBox.id) {
                    var range_all = document.body.createTextRange();
                    range_all.moveToElementText(textBox);
                    for (start = 0; range_all.compareEndPoints("StartToStart", range) < 0; start++)
                        range_all.moveStart('character', 1);
                    for (var i = 0; i <= start; i++) {
                        if (textBox.value.charAt(i) == '\n')
                            start++;
                    }
                    var range_all = document.body.createTextRange();
                    range_all.moveToElementText(textBox);
                    for (end = 0; range_all.compareEndPoints('StartToEnd', range) < 0; end++)
                        range_all.moveStart('character', 1);
                    for (var i = 0; i <= end; i++) {
                        if (textBox.value.charAt(i) == '\n')
                            end++;
                    }
                }
            }
        }
        return [start, end];
    },
    setPosition: function (oInput, oStart, oEnd) {
        if (oInput.setSelectionRange) {
            oInput.setSelectionRange(oStart, oEnd);
        }
        else if (oInput.createTextRange) {
            for (var i = 0; i <= oStart; i++) {
                if (oInput.value.charAt(i) == '\n') {
                    oStart--;
                    oEnd--;
                }
            }
            var range = oInput.createTextRange();
            range.moveEnd('character', -oInput.value.length);
            range.moveStart('character', -oInput.value.length);
            range.collapse(true);
            range.moveEnd('character', oEnd);
            range.moveStart('character', oStart);
            range.select();
        }
    },
    //向左对齐
    UBB_justifyleft: function (ele) {
        var txt = AFEditor.editor.getSelectText(ele);
        if (txt !== "") {
            txt = "[align=left]" + txt + "[/align]";
            AFEditor.editor.setSelectText(ele, txt);
        }
    },
    //居中
    UBB_justifycenter: function (ele) {
        var txt = AFEditor.editor.getSelectText(ele);
        if (txt !== "") {
            txt = "[align=center]" + txt + "[/align]";
            AFEditor.editor.setSelectText(ele, txt);
        }
    },
    //向右对齐
    UBB_justifyright: function (ele) {
        var txt = AFEditor.editor.getSelectText(ele);
        if (txt !== "") {
            txt = "[align=right]" + txt + "[/align]";
            AFEditor.editor.setSelectText(ele, txt);
        }
    },
    UBB_insertInput: function (ele, type, l) {
        if (!l) l = 0;
        var width = 460;
        var height = 450;
        var Arr = [0, 0];
        ele.focus();
        Arr = AFEditor.tool.savePos(ele);
        if (type == 2) {
            var pre = ele.value.substr(0, Arr[0]);
            var post = ele.value.substr(Arr[1]);

            var arr = ele.value.match(/<strong><u>/img);
            var index = arr ? arr.length : 0;

            ele.value = pre + " <strong><u>" + (index + 1) + "</u></strong> " + post;
            AFEditor.tool.setPosition(ele, Arr[0], Arr[0] + " <strong><u>" + (index + 1) + "</u></strong> ".length);
            return;
        }

    },
    UBB_insertsymbol: function (ele) {
        var width = 380;
        var height = 285;
        var left = (screen.width - width) / 2;
        var top = (screen.height - height) / 2;

        var url = "/Html/symbol.htm";
        if (navigator.userAgent.indexOf("Chrome") > 0) {
            window.open(url, window, "top=" + top + ",left=" + left + ",width=" + width + "px,height=" + height + "px,status=0");
        }
        else {
            var returnValue = window.showModalDialog(url, window, "dialogWidth:" + width + "px;dialogHeight:" + height + "px,status:0; help:0");
            AFEditor.tool.Indo(returnValue, document.getElementById(currentEditor));
        }
    },
    UpFile: function (editorId, tmId) {
        var f = document.getElementById('ifm1');
        //f.src = "/TestDebug/UploadImage?tmid=" + tmId;
        //设置子页面name="tmid"的input标签value为tmId
        f.getElementsByTagName("tmid").value = tmId;
        if (editorId == "") {
            alert("参数错误！");
            return;
        }
        var doc = f.contentDocument || f.contentWindow.document;
        var upFile = doc.getElementById('file');

        upFile.value = '';
        upFile.click();
    },
    UBB_insertMultipleResult: function (ele) {
        var width = 770;
        var height = 560;
        var left = (screen.width - width) / 2;
        var top = (screen.height - height) / 2;

        var url = "/Html/multipleResult.html";
        if (navigator.userAgent.indexOf("Chrome") > 0) {
            window.open(url, window, "top=" + top + ",left=" + left + ",width=" + width + "px,height=" + height + "px,status=0");
        }
        else {
            var returnValue = window.showModalDialog(url, window, "dialogWidth:" + width + "px;dialogHeight:" + height + "px,status:0; help:0");
            AFEditor.tool.Indo(returnValue, document.getElementById(currentEditor));
        }
    },
    UBB_OnLine: function (ele) {
        var width = 450;
        var height = 500;
        var left = (screen.width - width) / 2;
        var top = (screen.height - height) / 2;

        var url = "/Html/onLine.html";
        if (navigator.userAgent.indexOf("Chrome") > 0) {
            window.open(url, window, "top=" + top + ",left=" + left + ",width=" + width + "px,height=" + height + "px,status=0");
        }
        else {
            var returnValue = window.showModalDialog(url, window, "dialogWidth:" + width + "px;dialogHeight:" + height + "px,status:0; help:0");
            AFEditor.tool.Indo(returnValue, document.getElementById(currentEditor));
        }
    },
    UBB_Shushi: function (ele, tmid) {
        var width = 450;
        var height = 500;
        var left = (screen.width - width) / 2;
        var top = (screen.height - height) / 2;

        var url = "/Html/arithmetic.html?tmid=" + tmid + "&r=" + Math.random();
        if (navigator.userAgent.indexOf("Chrome") > 0) {
            window.open(url, window, "top=" + top + ",left=" + left + ",width=" + width + "px,height=" + height + "px,status=0");
        }
        else {
            var returnValue = window.showModalDialog(url, window, "dialogWidth:" + width + "px;dialogHeight:" + height + "px,status:0; help:0");
            AFEditor.tool.Indo(returnValue, document.getElementById(currentEditor));
        }
    },
    UBB_insertMathML: function (ele) {
        var width = 600;
        var height = 400;
        var left = (screen.width - width) / 2;
        var top = (screen.height - height) / 2;

        var url = "/Html/FE.html";
        if (navigator.userAgent.indexOf("Chrome") > 0) {
            window.open(url, window, "top=" + top + ",left=" + left + ",width=" + width + "px,height=" + height + "px,status=0");
        }
        else {
            var returnValue = window.showModalDialog(url, window, "dialogWidth:" + width + "px;dialogHeight:" + height + "px,status:0; help:0");
            AFEditor.tool.Indo(returnValue, document.getElementById(currentEditor));
        }
    },
    UBB_insertBrace: function (ele) {
        var width = 580;
        var height = 460;
        var left = (screen.width - width) / 2;
        var top = (screen.height - height) / 2;

        var returnValue = window.showModalDialog('/LEditor/brace.html', window, "dialogWidth:" + width + "px;dialogHeight:" + height + "px,status:0; help:0");
        AFEditor.tool.Indo(returnValue, ele);
    },
    Indo: function (v, ele) {

        if (ele == null) return;
        if (!v) return;
        var Arr = [0, 0];
        //_Ele = document.getElementById(_Ele);
        ele.focus();
        Arr = AFEditor.tool.savePos(ele);

        var pre = ele.value.substr(0, Arr[0]);
        var post = ele.value.substr(Arr[1]);
        ele.value = pre + v + post;
        AFEditor.tool.setPosition(ele, Arr[0], Arr[0] + v.length);
    },
    setFontStype: function (type, ele) {
        if (type === "")
            return;
        var lf = "<", rf = ">";
        if (type === 'yb') {
            lf = "[";
            rf = "]";
        }
        var txt = AFEditor.editor.getSelectText(ele);
        if (txt !== "") {
            txt = lf + type + rf + txt + lf + "/" + type + rf;
            AFEditor.editor.setSelectText(ele, txt);
        }
    },
    //更改字体颜色
    UBB_CFontColor: function (fontColor, editorId) {
        var ele = document.getElementById(editorId);
        var txt = AFEditor.editor.getSelectText(ele);
        if (txt !== "") {
            txt = "[color=" + fontColor + "]" + txt + "[/color]";
            AFEditor.editor.setSelectText(ele, txt);
        }
    },
    //更改字体
    UBB_CFont: function (font, editorId) {
        var ele = document.getElementById(editorId);
        var txt = AFEditor.editor.getSelectText(ele);
        if (txt !== "") {
            txt = "[font=" + font + "]" + txt + "[/font]";
            AFEditor.editor.setSelectText(ele, txt);
        }
    },
    //更改字体大小
    UBB_CFontSize: function (fontSize, editorId) {
        var ele = document.getElementById(editorId);
        var txt = AFEditor.editor.getSelectText(ele);
        if (txt !== "") {
            txt = "[size=" + fontSize + "]" + txt + "[/size]";
            AFEditor.editor.setSelectText(ele, txt);
        }
    },
    //更改文本样式
    UBB_CTextStyle: function (textStyle, editorId) {
        var ele = document.getElementById(editorId);
        var txt = AFEditor.editor.getSelectText(ele);
        if (txt !== "") {
            txt = "[textstyle=" + textStyle + "]" + txt + "[/textstyle]";
            AFEditor.editor.setSelectText(ele, txt);
        }
    },
    setOpenConent: function (txt) {
        AFEditor.tool.Indo(txt, document.getElementById(currentEditor));
    }
}

AFEditor.editor = {
    __calcBookmark: function (bookmark) {
        return (bookmark.charCodeAt(0) - 1) + (bookmark.charCodeAt(3) - 1) * 65536 + (bookmark.charCodeAt(2) - 1);
    },
    __getSelectPos: function (editor, end) {
        if (!editor) return;
        if (typeof editor.selectionStart != "undefined")
            return end ? editor.selectionEnd : editor.selectionStart;
        if (!editor.createTextRange) return;
        editor.focus();
        var range = document.selection.createRange().duplicate();
        if (!end) range.collapse(true);
        range.setEndPoint("StartToEnd", range);
        var start = document.body.createTextRange();
        start.moveToElementText(editor);
        var start = this.__calcBookmark(range.getBookmark()) - this.__calcBookmark(start.getBookmark());
        var value = editor.value;
        return start;
    },
    getSelectStart: function (editor) {
        return this.__getSelectPos(editor);
    },
    getSelectEnd: function (editor) {
        return this.__getSelectPos(editor, true);
    },
    getSelectRange: function (editor) {
        return [this.getSelectStart(editor), this.getSelectEnd(editor)];
    },
    getSelectText: function (editor) {
        editor.focus();
        if (typeof document.selection != "undefined") {
            return document.selection.createRange().text;
        } else {
            return editor.value.substr(editor.selectionStart, editor.selectionEnd - editor.selectionStart);
        }
    },
    setSelectRange: function (editor, range) {
        if (!editor) return;
        if (range[0] > range[1]) return;
        editor.focus();
        if (editor.setSelectionRange) {
            editor.setSelectionRange(range[0], range[1]);
        } else if (editor.createTextRange) {
            var value = editor.value;
            var textRange = editor.createTextRange();
            textRange.collapse(true);
            textRange.moveEnd("character", range[1]);
            textRange.moveStart("character", range[0]);
            textRange.select();
        }
    },
    textPos: function (editor, pos) {
        if (!editor) return;
        if (!editor.createTextRange) return pos;
        var value = editor.value;
        for (var i = 0; i <= pos; i++) if (value.charAt(i) == "\n") pos++;
        return pos;
    },
    cartPos: function (editor, pos) {
        if (!editor) return;
        if (!editor.createTextRange) return pos;
        var value = editor.value;
        var j = 0;
        for (var i = 0; i <= pos; i++) if (value.charAt(i) == "\n") j++;
        return pos - j;
    },
    selectEx: function (editor, startPattern, endPattern, include) {
        if (!editor) return;
        startPattern = "" + startPattern;
        endPattern = "" + endPattern;
        var range = this.getSelectRange(editor);
        var value = editor.value;
        var textRange = [this.textPos(editor, range[0]), this.textPos(editor, range[1])];
        var startStr = value.substr(0, textRange[0]);
        var endStr = value.substring(textRange[1], value.length);
        var i = startStr.lastIndexOf(startPattern);
        if (i < 0) return;
        var j = endStr.indexOf(endPattern);
        if (j < 0) return;
        j += textRange[1];
        if (include)
            j += endPattern.length;
        else i += startPattern.length;
        this.setSelectRange(editor, [this.cartPos(editor, i), this.cartPos(editor, j)]);
    },
    setSelectText: function (editor, value, range) {
        if (!editor) return;
        editor.focus();
        if (range && range.parentElement() == editor) {
            range.text = value;
            range.select();
            range = null;
        }
        else if (document.selection) {
            document.selection.createRange().text = value;
        }
        else if (typeof editor.selectionStart != "undefined") { // firefox
            var str = editor.value;
            var start = editor.selectionStart;
            var scroll = editor.scrollTop;
            editor.value = str.substr(0, start) + value +
                str.substring(editor.selectionEnd, str.length);
            editor.selectionStart = start + value.length;
            editor.selectionEnd = start + value.length;
            editor.scrollTop = scroll;
        }
    }
};

AFEditor.init = {
    //创建题目编辑器
    createTimuEditor: function myfunction(parameters) {
        var html = [];
        html.push('<div class="Topic-editor-title">');
        html.push(' <a class="jiacu" href="javascript:;" onclick="myEditor.bold(\'' + parameters.contentId + '\');" title="加粗" tabindex="-1"><dfn></dfn></a>');
        html.push(' <a class="xieti" href="javascript:;" onclick="myEditor.italic(\'' + parameters.contentId + '\');" title="斜体" tabindex="-1"><dfn></dfn></a>');
        html.push(' <a class="xiahuaxian" href="javascript:;" onclick="myEditor.underline(\'' + parameters.contentId + '\');" title="下划线" tabindex="-1"><dfn></dfn></a>');
        html.push(' <a class="sub" href="javascript:;" title="下标" tabindex="-1" style="width: 21px;" onclick="myEditor.sub(\'' + parameters.contentId + '\', this);"><dfn></dfn></a>');
        html.push(' <a class="sup" href="javascript:;" title="上标" tabindex="-1" style="width: 21px;" onclick="myEditor.sup(\'' + parameters.contentId + '\', this);"><dfn></dfn></a>');
        html.push(' <a class="ziti" href="javascript:;" title="字体" tabindex="-1" style="width: 21px;" onclick="myEditor.fontFamily(\'' + parameters.contentId + '\', this,\'' + parameters.pageType + '\');"><dfn></dfn></a>');
        html.push(' <a class="zihao" href="javascript:;" title="文字大小" tabindex="-1" style="width: 23px;" onclick="myEditor.fontSize(\'' + parameters.contentId + '\', this,\'' + parameters.pageType + '\');"><dfn></dfn></a>');
        html.push(' <a class="yanse" href="javascript:;" title="字体颜色" tabindex="-1" style="width: 23px;" onclick="myEditor.fontColor(\'' + parameters.contentId + '\', this,\'' + parameters.pageType + '\');"><dfn></dfn></a>');
        html.push(' <a class="textstyle" href="javascript:;" title="文本样式" tabindex="-1" style="width: 23px;" onclick="myEditor.textStyle(\'' + parameters.contentId + '\', this,\'' + parameters.pageType + '\');"><dfn></dfn></a>');
        html.push(' <a class="clearHtml" href="javascript:;" onclick="myEditor.clearHtml(\'' + parameters.contentId + '\');" title="清除Html样式" tabindex="-1"><dfn></dfn></a>');
        html.push(' <a tabIndex="-1" title="选中音标，加标签" class="yinbiao" onclick="myEditor.yinbiao(\'' + parameters.contentId + '\');" href="javascript:;"><dfn></dfn></a>');
        html.push(' <a tabIndex="-1" title="序号" onclick="myEditor.choiceNumber(\'' + parameters.contentId + '\');" href="javascript:;"><dfn></dfn></a>');
        html.push(' <a class="jvzuo" href="javascript:;" onclick="myEditor.justifyleft(\'' + parameters.contentId + '\');" title="居左" tabindex="-1"><dfn></dfn></a>');
        html.push(' <a class="jvzhong" href="javascript:;" onclick="myEditor.justifycenter(\'' + parameters.contentId + '\');" title="居中" tabindex="-1"><dfn></dfn></a>');
        html.push(' <a class="jvyou" href="javascript:;" onclick="myEditor.justifyright(\'' + parameters.contentId + '\');" title="居右" tabindex="-1"><dfn></dfn></a>');

        html.push(' <a class="tupian" href="javascript:;" onclick="myEditor.image(\'' + parameters.contentId + '\');" title="图片" tabindex="-1"><dfn></dfn></a>');
        html.push(' <a class="fuhao" href="javascript:;" onclick="myEditor.symbol(\'' + parameters.contentId + '\');" title="符号" tabindex="-1"><dfn></dfn></a>');
        html.push(' <a tabIndex="-1" title="多果空答案" class="kongdanan" onclick="myEditor.multipleResult(\'' + parameters.contentId + '\');" href="javascript:;"><dfn></dfn></a>');
        html.push(' <a tabIndex="-1" title="连线" class="shushi" onclick="myEditor.online(\'' + parameters.contentId + '\');" href="javascript:;"><dfn></dfn></a>');
        html.push(' <a tabIndex="-1" title="数学竖式" class="shushi" onclick="myEditor.shushi(\'' + parameters.contentId + '\');" href="javascript:;"><dfn></dfn></a>');
        html.push(' <a class="gongshi" href="javascript:;" onclick="myEditor.febox(\'' + parameters.contentId + '\');" title="公式编辑器" tabindex="-1"><dfn></dfn></a>');
        html.push('</div>');
        html.push('<textarea class="Topic-editor-section" id=' + parameters.contentId + ' name=' + parameters.contentId + ' onpropertychange="this.style.posHeight=this.scrollHeight;" oninput="this.style.height=this.scrollHeight;" onfocus="myEditor.focus(\'' + parameters.contentId + '\');">' + parameters.content + '</textarea>');
        //$(".article_right_xuanxiang").html(html.join('\r\n'));
        //parameters.editor.html(html.join('\r\n'));
        return html.join('\r\n');
    }
}