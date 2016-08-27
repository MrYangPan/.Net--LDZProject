//试卷相关
var Sheet = {

    //数学答案格式化
    formatMathMlString: function (val) {
        if (val.indexOf("root") > -1 || val.indexOf("sqrt") > -1 ||
            val.indexOf("^") > -1 || val.indexOf("_") > -1 || val.indexOf("**") > -1 ||
            val.indexOf("oparent") > -1 || val.indexOf("/") > -1 ||
            val.indexOf("bar") > -1 || val.indexOf("{") > -1 || val.indexOf(">\\-") > -1 ||
            val.indexOf("}") > -1 || val.indexOf("stackrel") > -1 ||
            val.indexOf("ulul") > -1 || val.indexOf("rec") > -1 || val.indexOf("vec") > -1 ||
            val.indexOf("[:") > -1 || val.indexOf("dot") > -1) {
            return val;
        } else {
            return val.replace(/`/g, '').replace(/lt/g, '&lt;');
        }
    },

    //ajax异步（请求路径，被请求method所需的参数，请求成功的回调函数，回调函数所需参数）
    ajaxRequest: function (urlpath, databoj, successFunc, param) {
        $.ajax({
            type: "POST",
            url: urlpath,
            data: databoj,
            timeout: 30000,
            success: function (obj) {
                successFunc(obj, param);
            },
            error: function (xmlRequest, textStatus, errorThrown) {
            }
        });
    },

    //提交试卷页面成功的回调函数
    ajaxTestResult: function (obj) {
        window.location.href = "/TestSheet/TestSheetResult?TestSheetId=" + obj;
    },

    //提交试卷
    submitTest: function () {
        $(".submitmorepractice").text("提交中");
        $(".submitmorepractice").attr("disabled", "true");
        var elements = new Array();
        $(":input,.FEBox,.daankuang,.onlineKong").each(function (i, o) {
            if (!o.disabled && !o.readOnly) {
                elements.push(o);
            }
        });
        var testsheetid = $("#testsheetid").val();
        var timuid = $("#timuid").val();
        var timuidList = timuid.split(',');
        var inputcode = $("#inputcode").val();
        var arrList = [];
        var timuList = "";
        for (var i = 0; i < elements.length; i++) {
            var element = elements[i];
            var id = element.id.replace("i_", "");
            id = id.replace(/[_][0-9]{1}[_][0-9]{1,}$/, '');
            if (inputcode.split("^").indexOf(id) > -1) {
                if (element.tagName.toLowerCase() == "input") {
                    if (element.type == "radio" || element.type == "checkbox") {
                        var timu = element.name;
                        if (timuList.indexOf(timu) > -1) { //本选择题答案已组装
                            if (element.checked) {
                                for (var j = 0; j < arrList.length; j++) {
                                    if (arrList[j].InputCode == timu) {
                                        arrList[j].InputAnswer = arrList[j].InputAnswer + "," + element.value;
                                        var re = new RegExp(",", "i");
                                        arrList[j].InputAnswer = arrList[j].InputAnswer.replace(re, "");
                                    }
                                }
                            }
                        } else { //本选择题答案未组装且该题未作答则给默认值Answer=null
                            if (element.checked) {
                                var obj0 = { "InputCode": element.name.replace("i_", ""), "InputAnswer": element.value };
                                timuList = timuList + timu + "^";
                                arrList.push(obj0);
                            } else {
                                var obj1 = { "InputCode": element.name.replace("i_", ""), "InputAnswer": "" };
                                timuList = timuList + timu + "^";
                                arrList.push(obj1);
                            }
                        }
                    } else if (element.type == "submit" || element.type == "hidden" || element.type == "password" || element.type == "text") {
                        var obj2 = { "InputCode": element.name.replace("i_", ""), "InputAnswer": element.value };
                        arrList.push(obj2);
                    } else {
                        var uanswer = parser.Analyse(element);
                        var obj3 = { "InputCode": element.id.replace("i_", ""), "InputAnswer": uanswer };
                        arrList.push(obj3);
                    }
                } else if (element.tagName.toLowerCase() == "select") {
                    var value = '';
                    if (element.type == 'select-one') {
                        var index = element.selectedIndex;
                        if (index >= 0)
                            value = element.options[index].value || element.options[index].text;
                    } else {
                        value = new Array();
                        for (var i = 0; i < element.length; i++) {
                            var opt = element.options[i];
                            if (opt.selected)
                                value.push(opt.value || opt.text);
                        }
                    }
                    var obj4 = { "InputCode": element.name.replace("i_", ""), "InputAnswer": value };
                    arrList.push(obj4);
                } else {
                    var text = '';
                    if (typeof parser == 'undefined' && $(element).attr('class').indexOf("daankuang") == -1 && $(element).attr('class').indexOf("onlineKong") == -1) {
                        text = $(element).text();
                    } else {
                        if ($(element).attr('class').indexOf("daankuang") > -1) {
                            text = element.innerHTML;
                            text = text.replace('<br>', '');
                        } else if ($(element).attr('class').indexOf("onlineKong") > -1) {
                            var obj = {};
                            var answer = [];
                            $(element).find('.matching-left li').each(function (index) {
                                answer.push(index + '|' + ($(this).find('div.inline').attr('to') || -1));
                            });
                            obj['answer'] = answer;
                            text = JSON.stringify(obj);
                        } else {
                            try {
                                text = parser.Analyse(element);
                            } catch (e) {
                            }
                            text = this.formatMathMlString(text);
                        }
                    }
                    var elementName = $(element).attr("name");
                    text = text.replace('请将解题步骤写在草稿纸上，完成全部答题提交试卷后，对照系统提供的解析逐题自我判卷。', '');
                    var obj5 = { "InputCode": elementName.replace("i_", ""), "InputAnswer": text };
                    arrList.push(obj5);
                }
            }
        }
        var urlpath = "/TestSheet/SubmitAnswer";
        var dataobj = { "userAnswerModel": arrList, "timuidList": timuidList, testsheetId: testsheetid };
        ajaxRequest(urlpath, dataobj, this.ajaxTestResult);
    },

    //关闭窗口
    closeWindow: function () {
        window.opener = null;
        window.open('', "_self");
        window.close();
    },

    //题型筛选
    submitType: function () {
        var chkvalue = [];
        $('input[type="checkbox"]:checked').each(function () {
            var tmtype = $(this).attr("data-tmtype");
            chkvalue.push(tmtype);
        });
        window.location.href = "/ErrorSets/CreateTimeClassify?timuType=" + chkvalue;
    },

    //获取错题列表成功的回调函数
    ajaxGetData: function (json, currentnext) {
        var html = "";
        var htmltemp = "<div class='cuoti'><div class='suolue'><img src='#RelatePath#' class='img-responsive' alt='' /></div>\
                                                   <div class='Questions'><span>#PingTiMuHtml#</span></div>\
                                                   <div class='button'><a target='_blank' href='/ErrorSets/GetMorePractice?uploadId=#uploadId#'><li class='tuozhan'><span></span>拓展练习</li></a>\
                                                   <a class='getmore'  data-video='#VideoID#'><li class='weike'><span></span>微课视频</li></a></div></div>";
        if (json.length > 0) {
            for (var i = 0; i < json.length; i++) {
                var upload = json[i];
                var htmli = htmltemp;
                htmli = htmli.replace('#RelatePath#', upload.RelatePath);
                htmli = htmli.replace('#PingTiMuHtml#', upload.PingTiMuHtml);
                htmli = htmli.replace('#uploadId#', upload.Id);
                htmli = htmli.replace('#VideoID#', upload.VideoId);
                html += htmli;
            }
            currentnext.html(html);
        } else {
            html += "<div class='Questions'>当前日期没有收藏所选题型的题目</div>";
            currentnext.html(html);
        }
    },

    //获取某日期错题列表
    getData: function () {
        var currentnext = $(this).next();
        var date = $(this).attr("data-shorttime");
        var toggleid = "Toggle+" + date;
        $("[data-toggle='" + toggleid + "']").toggleClass("close");
        $("[data-toggle='" + toggleid + "']").toggleClass("open");
        var chkvalue = [];
        $('input[type="checkbox"]:checked').each(function () {
            var tmtype = $(this).attr("data-tmtype");
            chkvalue.push(tmtype);
        });
        if (currentnext.is(":hidden")) {
            var urlpath = "/ErrorSets/GetTiMuInfoByDate";
            var dataobj = { "DateTime": date, "timuType": chkvalue };
            this.ajaxRequest(urlpath, dataobj, this.ajaxGetData, currentnext);
            currentnext.toggle();
        } else {
            currentnext.toggle();
        }
    },

    init: function () {
        this.eventBinding();
        $(".FEBoxTextArea").each(function () {
            if (typeof box != 'undefined') {
                box.Disable(this);
            }
            this.style.fontSize = '14px';
            this.innerHTML = '请将解题步骤写在草稿纸上，完成全部答题提交试卷后，对照系统提供的解析逐题自我判卷。';
        });

        $(".errorsetsdate:first").click();
    },

    eventBinding: function () {
        $(".button[id='queding']").click(this.submitType);
        $(".closetestsheet").click(this.closeWindow);
        $(".submittestsheet").click(this.submitTest);
        $(".errorsetsdate").click(this.getData);
    }
};

