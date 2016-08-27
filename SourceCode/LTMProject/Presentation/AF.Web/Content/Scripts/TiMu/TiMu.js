//展示题目相关
var TiMu = {

    init: function () {
        this.eventBinding();
    },

    matchingClick: function () {
        var lft = null;
        $(this).parent().parent().parent().find(".matching-left .inline").each(function () {
            if (this.style.color == "blue") {
                lft = this;
            }
        });
        if (lft == null) return;
        if (lft.parentNode.parentNode.parentNode != this.parentNode.parentNode.parentNode) return;

        var rindex = $($(this).parent()).index();
        lft.setAttribute("to", rindex);

        var lobj = $(lft);
        var robj = $(this);
        var lftX = lobj.position().left + lobj.width();
        var lftY = lobj.position().top + lobj.height() / 2;
        var rhtX = robj.position().left;
        var rhtY = robj.position().top + robj.height() / 2;
        OnLine.drawLine(lftX, lftY, rhtX, rhtY, lft, true);
    },

    analytic: function (i, element) {
        var arr = element.id.split('_');
        var id = element.id.substring(element.id.indexOf('_') + 1);
        var tmid = id.substring(0, 15);
        var obj = document.getElementById('root_' + id);
        switch (arr[0]) {
            case "banalyse":
                $(this).unbind().click(function () {
                    if (obj.getAttribute('curObj') == 'banalyse') {
                        if (obj.style.display == 'none') {
                            $('#root_' + id).show();
                        } else {
                            $('#root_' + id).hide();
                        }
                    } else {
                        obj.setAttribute('curObj', 'banalyse');
                        $.get("/EntryExamination/AnswerAnalysis?analyseName=banalyse" + "&tmid=" + id,
                            function (data) {
                                $('#root_' + id).html('');
                                $('#root_' + id).html(data.MuAnalyse).show();
                            });
                    }
                });
                break;
            case "bjudge":
                $(this).unbind().click(function () {
                    if (obj.getAttribute('curObj') == 'bjudge') {
                        if (obj.style.display == 'none') {
                            $('#root_' + id).show();
                        } else {
                            $('#root_' + id).hide();
                        }
                    } else {
                        obj.setAttribute('curObj', 'bjudge');
                        $.get("/EntryExamination/AnswerAnalysis?analyseName=bjudge" + "&tmid=" + id,
                            function (data) {
                                $('#root_' + id).html('');
                                $('#root_' + id).html(data.MuAnalyse).show();
                                if (data.InputType == "online-input") {
                                    OnLine.getonlinePosition(data.Answer, data.InputCode, 2, true);
                                }
                            });
                    }
                });
                break;
            case "bvideo":
                break;
        }
    },

    eventBinding: function () {
        $("div.onlineKong .matching-left .inline").click(function () {
            $(this).parent().parent().find(".inline").each(function () {
                $(this).css("color", "");
            });
            $(this).css("color", "blue");
        });
        $("div.onlineKong .matching-right div").click(this.matchingClick);
        $(".Analytic_Top .button").each(this.analytic);
    }
};

//连线相关
var OnLine = {
    drawLine: function (x1, y1, x2, y2, ele, isRight) {
        var color = isRight ? 'blue' : 'red';
        if (/msie/.test(navigator.userAgent.toLowerCase())) {
            var str = "<v:line class='vline'  from='" + x1 + "," + Math.ceil(y1) + "'";
            str += " to='" + (x2) + "," + (y2)
                + "' right=" + isRight + "  style=\"position:absolute;top:0px;left:0px\">" +
                "<v:stroke endarrow='classic' color='" + color + "'/></v:line>";
            $(ele).find(".vline").remove();
            $(ele).append(str);
        } else {
            var height = $(ele).parent().parent().parent().height();
            height = height <= 0 ? 600 : height;
            var cvsObj = document.createElement("canvas");
            var cvs = cvsObj.getContext("2d");
            cvsObj.style.position = "absolute";
            cvsObj.style.top = "0px";
            cvsObj.style.left = "0px";
            cvsObj.style.zIndex = "-99";
            cvsObj.width = 600;
            cvsObj.height = height;
            cvsObj.right = isRight;
            cvs.strokeStyle = color;
            cvs.lineWidth = 1;
            cvs.beginPath();
            cvs.moveTo(x1, y1);
            cvs.lineTo(x2, y2);
            cvs.closePath();
            cvs.stroke();
            $(ele).find("canvas").remove();
            ele.appendChild(cvsObj);
        }
    },
    getonlinePosition: function (input, inputCode, isRight, isAnswer) {
        if (input == "") return;
        try {
            var answer = $.parseJSON(input).answer;
            for (var i = 0; i < answer.length; i++) {
                var lr = answer[i].split('|');
                var l = lr[0];
                var r = lr[1];
                if (l == -1 || r == -1) continue;
                var ktype = isAnswer ? 'ki' : 'i';
                var lobj = $('#' + ktype + '_' + inputCode + ' ul.matching-left .inline:eq(' + l + ')');
                var robj = $('#' + ktype + '_' + inputCode + ' ul.matching-right .inline:eq(' + r + ')');
                var lftX = lobj.position().left + lobj.width();
                var lftY = lobj.position().top + lobj.height() / 2;
                var rhtX = robj.position().left;
                var rhtY = robj.position().top + robj.height() / 2;

                OnLine.drawLine(lftX, lftY, rhtX, rhtY, lobj[0], isRight == 2 ? true : false);
            }
        } catch (e) {
        }
    }
};
