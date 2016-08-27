var pageedit = function () {
    var timuobj = {}; //题目对象
    var revertobj = {};//知识点输入提示下拉框数据源
    var taskItemId = "";//保存任务子项id
    var editBoxId = 0; //编辑框下标
    var oldEditBoxId; //保存切换题目类别之前的编辑框下标
    var choiceIndex = 0; //保存题目选项数量
    var choiceType = ''; //保存题目类别
    var tmType = ''; //保存题型
    //保存当前题目类型
    var timuLeiXing;
    var isValidate = false;
    //生成编辑器需要的参数
    var parameters = { contentId: "editBox", content: "", titleId: null, title: "标题", editType: '', pageType: "topic", editor: "" };
    var tmObj;
    var isCircleFind = false;//是否存在相同知识点
    var reverteJson = { tmid: "", mainId: "", minorIds: "", diff: "", videoCode: "", teacher: "", errorMessage: "" };//被撤回任务，录题页面属性标定json对象

    //
    var inputChoices = []; //当前题目选项数组
    //选项对象
    var choices = { rightAnswer: "", score: "", Sequence: "", inputChoice: inputChoices };
    //当前页面录入的题目信息
    var timu = { timuId: "", tigan: "", analyse: "", explain: "", comment: "", choiceType: "", timuType: "", subjectId: "", choicesIput: choices, timuBookOrder: "", largeNumber: "", smallNumber: "", pageNumber: "" };

    //录题编辑、创建
    var entryQuerys = {
        init: function () {
            //创建题目编辑器
            entryQuerys.bingEditor();
            //加载题目信息
            entryQuerys.initTimu();
            //绑定题型单选事件
            $("input:radio[name='tmType']").click(function () {
                var e = $(this);
                //获取题型
                tmType = (e.context != undefined) ? e.context.value : "";
            });
            //绑定添加选项事件
            $("#addChoice").click(function () {
                var e = $(this);
                entryQuerys.addChoice(e);
            });
            //绑定删除选项事件
            $("#removeChoice").click(function () {
                var e = $(this);
                entryQuerys.removeChoice(e);
            });
            //绑定选项类型radio
            $("input:radio[name='choiceType']").click(function () {
                var e = $(this);
                choiceType = (e.context != undefined) ? e.context.value : "";
                entryQuerys.changeType(e);
            });
            //绑定保存事件
            $("#topicSave").click(function () {
                var url = $(this).attr("data-src");
                entryQuerys.save(url);
            });
            //遍历题型，获取默认选中值
            $("input:radio[name='tmType']").each(function () {
                var e = $(this);
                //var checked = e.attr("checked");
                var checked = e.parent().attr("class");
                if (checked === "checked") {
                    tmType = e.attr("value");
                }
            });
            //遍历类别，获取默认选中值
            $("input:radio[name='choiceType']").each(function () {
                var e = $(this);
                //var checked = e.attr("checked");
                var checked = e.parent().attr("class");
                if (checked === "checked") {
                    choiceType = e.attr("value");
                }
            });
            //添加题目
            $(".addTimu").click(function () {
                var ahref = $(this).attr("data-href");
                var order = parseInt(ahref.substring(ahref.lastIndexOf("=") + 1, ahref.length)) - 1;
                var lastli = $(".entryTopicUl li").last();
                var lia = lastli.find("a");
                if (lia.attr("href").indexOf("timuId") > 0) {
                    // 执行重定向,添加一道题目
                    window.location.href = ahref;
                } else {
                    if (order == 0) {
                        order = 1;
                    }
                    bootbox.alert({
                        title: "提示",
                        message: "请录完第" + order + "道题目再添加!",
                        callback: function () { }
                    });
                }
            });
            //绑定大题号、小题号、页码验证事件
            $("#pageNumber").change(function (e) {
                entryQuerys.entryValidate($(this).context);
            });
            //添加子题目
            $(".addChildTimu").click(function () {
                entryQuerys.addChildTimu($(this));
            });
        },
        //创建编辑器对象
        myEditor: function () {
            return new AFEditor(tmObj);
        },
        //绑定编辑器
        bingEditor: function () {
            $(".article_right_xuanxiang").each(function () {
                var e = $(this);
                var childLength = e.children().length;
                var editId = e.attr("data-edit");
                //如果没有子节点，则创建
                if (childLength <= 0) {
                    parameters.contentId = "editBox_" + editId;
                    parameters.editor = e;
                    //创建题目编辑器
                    var editHtml = entryQuerys.myEditor().init.createTimuEditor(parameters);
                    e.html(editHtml);
                    editBoxId = editId;
                }
            });
        },
        //添加或者改变选项标签
        addChoice: function (e) {
            var html = [];
            var cho;
            var id = (e.context != undefined) ? e.context.id : "";
            if (choiceIndex < 8) {
                if (choiceType === "0") {
                    choiceIndex++;
                    editBoxId++;
                    cho = String.fromCharCode(64 + choiceIndex);
                    html.push('<div id="chContent" data-choiceId="">');
                    html.push(' <div class="article_left_xuanxiang">');
                    html.push('     <input type="radio" id="i_' + tmObj.TMID + '_' + choiceIndex + '" value="' + choiceIndex + '" name="' + tmObj.TMID + '">');
                    html.push('     <label for="i_' + tmObj.TMID + '_' + choiceIndex + '">' + cho + '</label>');
                    html.push(' </div>');
                    html.push(' <div class="Topic-editor-box article_right_xuanxiang" data-edit="' + editBoxId + '"></div>');
                    html.push('</div>');
                    $("#choiceList").append(html.join('\r\n'));
                    entryQuerys.bindInputChoice("radio");
                } else if (choiceType === "1") {
                    choiceIndex++;
                    editBoxId++;
                    cho = String.fromCharCode(64 + choiceIndex);
                    html.push('<div id="chContent" data-choiceId="">');
                    html.push(' <div class="article_left_xuanxiang">');
                    html.push('     <input type="checkbox" id="i_' + tmObj.TMID + '_' + choiceIndex + '" value="' + choiceIndex + '" name="' + tmObj.TMID + '">');
                    html.push('     <label for="i_' + tmObj.TMID + '_' + choiceIndex + '">' + cho + '</label>');
                    html.push(' </div>');
                    html.push(' <div class="Topic-editor-box article_right_xuanxiang" data-edit="' + editBoxId + '"></div>');
                    html.push('</div>');
                    $("#choiceList").append(html.join('\r\n'));
                    entryQuerys.bindInputChoice("checkbox");
                } else if (choiceType === "2") {
                    bootbox.alert({
                        title: "提示",
                        message: "判断题不能添加选项!",
                        callback: function () { }
                    });
                    entryQuerys.bindInputChoice("radio-TF");
                }
                //内容置为空
                parameters.content = "";
                //创建题目编辑器
                entryQuerys.bingEditor();
            } else {
                bootbox.alert({
                    title: "提示",
                    message: "最多支持8个选项!",
                    callback: function () { }
                });
            }
        },
        //更改题目类型
        changeType: function (e) {
            var id = (e.context != undefined) ? e.context.id : "";
            choiceIndex = 0;
            editBoxId = oldEditBoxId; //旧的赋值给新的
            var html = [];
            var cho = String.empty;
            var inputs = timuobj.Inputs[0];
            if (inputs === undefined) {
                return;
            }
            $("#chioce").css("display", "block");
            var inputAnswer = inputs.InputAnswer.split(',');
            var editHtml;
            switch (id) {
                case "rdoRdo":
                    for (var i = 0; i < timuobj.Inputs[0].InputChoice.length; i++) {
                        choiceIndex++;
                        editBoxId++;
                        cho = String.fromCharCode(64 + choiceIndex);
                        html.push('<div id="chContent" data-choiceId="' + timuobj.Inputs[0].InputChoice[i].Id + '">');
                        if (parseInt(inputAnswer[0]) === choiceIndex && timuobj.Inputs[0].InputType === "radio") {
                            html.push(' <div class="article_left_xuanxiang select">');
                        } else {
                            html.push(' <div class="article_left_xuanxiang">');
                        }
                        if (timuobj.Inputs[0].InputType === "radio") {
                            //赋值,填充选项答案
                            parameters.content = timuobj.Inputs[0].InputChoice[i].ChoiceContent;
                            //正确选项
                            $("#choiceAnswer").val(entryQuerys.numToABC(inputAnswer));
                            //得分
                            $("#select_score").attr("value", timuobj.Inputs[0].Score);
                        } else {
                            //赋值,填充选项答案
                            parameters.content = "";
                            //正确选项
                            $("#choiceAnswer").val("");
                            //得分
                            $("#select_score").attr("value", 0);
                        }
                        html.push('     <input type="radio" id="i_' + tmObj.TMID + '_' + choiceIndex + '" value="' + choiceIndex + '" name="' + tmObj.TMID + '">');
                        html.push('     <label for="i_' + tmObj.TMID + '_' + choiceIndex + '">' + cho + '</label>');
                        html.push(' </div>');
                        //创建题目编辑器
                        parameters.contentId = "editBox_" + editBoxId;
                        editHtml = entryQuerys.myEditor().init.createTimuEditor(parameters);
                        html.push(' <div class="Topic-editor-box article_right_xuanxiang" data-edit="' + editBoxId + '">' + editHtml + '</div>');
                        html.push('</div>');
                        $("#choiceList").html(html.join('\r\n'));
                    }
                    if (timuobj.Inputs[0].InputChoice.length === 0) {
                        for (var i = 0; i < 4; i++) {
                            choiceIndex++;
                            editBoxId++;
                            cho = String.fromCharCode(64 + choiceIndex);
                            html.push('<div id="chContent" data-choiceId="">');
                            html.push(' <div class="article_left_xuanxiang">');
                            html.push('     <input type="radio" id="i_' + tmObj.TMID + '_' + choiceIndex + '" value="' + choiceIndex + '" name="' + tmObj.TMID + '">');
                            html.push('     <label for="i_' + tmObj.TMID + '_' + choiceIndex + '">' + cho + '</label>');
                            html.push(' </div>');
                            html.push(' <div class="Topic-editor-box article_right_xuanxiang" data-edit="' + editBoxId + '"></div>');
                            html.push('</div>');
                            $("#choiceList").html(html.join('\r\n'));
                        }
                    }
                    //重新绑定选项单击事件
                    this.bindInputChoice("radio");
                    break;
                case "rdoCK":
                    var rightIndex = 0;
                    for (var i = 0; i < timuobj.Inputs[0].InputChoice.length; i++) {
                        choiceIndex++;
                        editBoxId++;
                        cho = String.fromCharCode(64 + choiceIndex);
                        html.push('<div id="chContent" data-choiceId="' + timuobj.Inputs[0].InputChoice[i].Id + '">');
                        if (parseInt(inputAnswer[rightIndex]) === choiceIndex && rightIndex < inputAnswer.length && timuobj.Inputs[0].InputType === "checkbox-all") {
                            html.push(' <div class="article_left_xuanxiang select">');
                            if (inputAnswer.length > 1) {
                                rightIndex++;
                            }
                        } else {
                            html.push(' <div class="article_left_xuanxiang">');
                        }
                        if (timuobj.Inputs[0].InputType === "checkbox-all") {
                            //赋值,填充选项答案
                            parameters.content = timuobj.Inputs[0].InputChoice[i].ChoiceContent;
                            //正确选项
                            $("#choiceAnswer").val(entryQuerys.numToABC(inputAnswer));
                            //得分
                            $("#select_score").attr("value", timuobj.Inputs[0].Score);
                        } else {
                            //赋值,填充选项答案
                            parameters.content = "";
                            //正确选项
                            $("#choiceAnswer").val("");
                            //得分
                            $("#select_score").attr("value", 0);
                        }
                        html.push('     <input type="checkbox" id="i_' + tmObj.TMID + '_' + choiceIndex + '" value="' + choiceIndex + '" name="' + tmObj.TMID + '">');
                        html.push('     <label for="i_' + tmObj.TMID + '_' + choiceIndex + '">' + cho + '</label>');
                        html.push(' </div>');
                        //创建题目编辑器
                        parameters.contentId = "editBox_" + editBoxId;
                        editHtml = entryQuerys.myEditor().init.createTimuEditor(parameters);
                        html.push(' <div class="Topic-editor-box article_right_xuanxiang" data-edit="' + editBoxId + '">' + editHtml + '</div>');
                        html.push('</div>');
                        $("#choiceList").html(html.join('\r\n'));
                    }
                    if (timuobj.Inputs[0].InputChoice.length === 0) {
                        for (var i = 0; i < 4; i++) {
                            choiceIndex++;
                            editBoxId++;
                            cho = String.fromCharCode(64 + choiceIndex);
                            html.push('<div id="chContent" data-choiceId="">');
                            html.push(' <div class="article_left_xuanxiang">');
                            html.push('     <input type="checkbox" id="i_' + tmObj.TMID + '_' + choiceIndex + '" value="' + choiceIndex + '" name="' + tmObj.TMID + '">');
                            html.push('     <label for="i_' + tmObj.TMID + '_' + choiceIndex + '">' + cho + '</label>');
                            html.push(' </div>');
                            html.push(' <div class="Topic-editor-box article_right_xuanxiang" data-edit="' + editBoxId + '"></div>');
                            html.push('</div>');
                            $("#choiceList").html(html.join('\r\n'));
                        }
                    }
                    //重新绑定选项单击事件
                    entryQuerys.bindInputChoice("checkbox");
                    break;
                case "rdoTF":
                    choiceIndex++;
                    editBoxId++;
                    html.push('<div>');
                    if (parseInt(inputAnswer[0]) === choiceIndex && timuobj.Inputs[0].InputType === "radio-TF") {
                        html.push(' <div class="article_left_xuanxiang select">');
                    } else {
                        html.push(' <div class="article_left_xuanxiang">');
                    }
                    if (timuobj.Inputs[0].InputType === "radio-TF") {
                        //赋值,填充选项答案
                        parameters.content = timuobj.Inputs[0].InputChoice[0].ChoiceContent;
                        //正确选项
                        $("#choiceAnswer").val(entryQuerys.numToABC(inputAnswer));
                        //得分
                        $("#select_score").attr("value", timuobj.Inputs[0].Score);
                    } else {
                        //赋值,填充选项答案
                        parameters.content = "";
                        //正确选项
                        $("#choiceAnswer").val("");
                        //得分
                        $("#select_score").attr("value", 0);
                    }
                    html.push(' <input type="radio" id="i_' + tmObj.TMID + '_1" value="1" name="' + tmObj.TMID + '">');
                    html.push(' <label for="i_' + tmObj.TMID + '_1">T</label>');
                    html.push(' </div>');
                    //创建题目编辑器
                    parameters.contentId = "editBox_" + editBoxId;
                    editHtml = entryQuerys.myEditor().init.createTimuEditor(parameters);
                    html.push(' <div class="Topic-editor-box article_right_xuanxiang" data-edit="' + editBoxId + '" style="width: 98.1%;">' + editHtml + '</div>');
                    html.push('</div>');
                    //创建题目编辑器
                    this.bingEditor();
                    choiceIndex++;
                    editBoxId++;
                    html.push('<div>');
                    if (parseInt(inputAnswer[0]) === choiceIndex && timuobj.Inputs[0].InputType === "radio-TF") {
                        html.push(' <div class="article_left_xuanxiang select">');
                    } else {
                        html.push(' <div class="article_left_xuanxiang">');
                    }
                    if (timuobj.Inputs[0].InputType === "radio-TF") {
                        //赋值,填充选项答案
                        parameters.content = timuobj.Inputs[0].InputChoice[1].ChoiceContent;
                        //正确选项
                        $("#choiceAnswer").val(entryQuerys.numToABC(inputAnswer));
                        //得分
                        $("#select_score").attr("value", timuobj.Inputs[0].Score);
                    } else {
                        //赋值,填充选项答案
                        parameters.content = "";
                        //正确选项
                        $("#choiceAnswer").val("");
                        //得分
                        $("#select_score").attr("value", 0);
                    }
                    html.push(' <input type="radio" id="i_' + tmObj.TMID + '_2" value="2" name="' + tmObj.TMID + '">');
                    html.push(' <label for="i_' + tmObj.TMID + '_2">F</label>');
                    html.push(' </div>');
                    //创建题目编辑器
                    parameters.contentId = "editBox_" + editBoxId;
                    editHtml = entryQuerys.myEditor().init.createTimuEditor(parameters);
                    html.push(' <div class="Topic-editor-box article_right_xuanxiang" data-edit="' + editBoxId + '" style="width: 98.1%;">' + editHtml + '</div>');
                    html.push('</div>');
                    $("#choiceList").html(html.join('\r\n'));
                    //重新绑定选项单击事件
                    entryQuerys.bindInputChoice("radio-TF");
                    break;
                case "rdoText":
                    $("#chioce").css("display", "none");
                    break;
                default:
            }
            //创建题目编辑器
            entryQuerys.bingEditor();
        },
        //数字转字母
        numToABC: function (num) {
            if (num == '' || num == 0) return '';
            var n = num.toString().split(',');
            var abc = '';
            for (var i = 0; i < n.length; i++) {
                abc += String.fromCharCode(64 + parseInt(n[i]));
            }
            return abc;
        },
        //改变选中项颜色
        setChoiceAnswer: function (o, editType) {
            if (o.type === 'radio') {
                $("#choiceList  div").removeClass('select');
                if (o.checked) {
                    $(o).parent().addClass('select');
                }
                if (editType == 'radio-TF') {
                    $("#choiceAnswer").val(o.value == 1 ? 'T' : 'F');
                } else
                    $("#choiceAnswer").val(entryQuerys.numToABC(o.value));
            } else {
                var an = $("#choiceAnswer").val();
                an = an.replace(entryQuerys.numToABC(o.value), '');
                if (o.checked) {
                    $("#choiceAnswer").val(an + entryQuerys.numToABC(o.value));
                    $(o).parent().addClass('select');
                } else {
                    $("#choiceAnswer").val(an);
                    $(o).parent().removeClass('select');
                }
            }
        },
        //绑定单选按钮单击事件
        bindInputChoice: function (type) {
            $("#choiceList input").click(function () {
                entryQuerys.setChoiceAnswer(this, type);
            });
        },
        //删除选项
        removeChoice: function (e) {
            if (choiceIndex <= 2) {
                bootbox.alert({
                    title: "提示",
                    message: "至少支持2个选项!",
                    callback: function () { }
                });
                return;
            }
            bootbox.confirm({
                message: '确定删除一个选项吗?',
                title: "提示",
                callback: function (result) {
                    if (result) {
                        var choiceId = $("#choiceList").children().last().attr("data-choiceId");
                        //删除选项
                        $.ajax({
                            url: '/EntryExamination/DeleteChoice',
                            type: "post",
                            data: { "choiceId": choiceId },
                            success: function (data) {
                                $("#choiceList").children().last().remove();
                                choiceIndex--;
                            }
                        });
                    }
                }
            });
        },
        //保存提交到数据库
        save: function (url) {
            if (tmType === '') {
                bootbox.alert({
                    title: "提示",
                    message: "请选择题型!",
                    callback: function () { }
                });
                return false;
            }
            if (choiceType === '') {
                bootbox.alert({
                    title: "提示",
                    message: "请选择题目类别!",
                    callback: function () { }
                });
                return false;
            }
            //回撤页面不需要验证
            if (revertobj==null) {
                //验证
                entryQuerys.validate();
            }
            if (!isValidate) {
                //题目id
                timu.timuId = timuobj.Id;
                //获取题干
                timu.tigan = $("#TiGan").find(".Topic-editor-section")[0].value;
                //获取分析
                timu.analyse = $("#divAnalyse").find(".Topic-editor-section")[0].value;
                //获取解答
                timu.explain = $("#divExplain").find(".Topic-editor-section")[0].value;
                //获取点评
                timu.comment = $("#divComment").find(".Topic-editor-section")[0].value;
                //题目类别
                timu.choiceType = choiceType;
                //题型
                timu.timuType = tmType;
                //科目id
                timu.subjectId = timuobj.SubjectId;
                //BookTiMu排序
                timu.timuBookOrder = timuobj.Order;
                //大题号
                timu.largeNumber = $(".largeNumber").val();
                //小题号
                timu.smallNumber = $(".smallNumber").val();
                //页码
                timu.pageNumber = $("#pageNumber").val();

                if (timu.tigan.trim() === "" || timu.tigan.trim() === null) {
                    bootbox.alert({
                        title: "提示",
                        message: "请输入题干信息!",
                        callback: function () { }
                    });
                    return false;
                }
                switch (choiceType) {
                    case "0":
                        //获取选项
                        entryQuerys.getChoice();
                        break;
                    case "1":
                        //多选类型题目保存
                        entryQuerys.getChoice();
                        break;
                    case "2":
                        //判断题目类型保存
                        entryQuerys.getChoice();
                        break;
                    default:
                }

                if (revertobj != null) {
                    /*
                 * 组装录题属性json对象，只有在撤回任务时候才有数据
                 */
                    reverteQuerys.reverteSaveToJson();
                    if (reverteJson.mainId === "") {
                        bootbox.alert({
                            title: "提示",
                            message: "主知识点不能为空!",
                            callback: function () { }
                        });
                        return false;
                    }
                    $.ajax({
                        url: '/EntryExamination/SaveTopic',
                        type: "post",
                        data: { "timu": JSON.stringify(timu), "taskItemId": taskItemId, "property": JSON.stringify(reverteJson) },
                        success: function (data) {
                            var json = JSON.parse(data);
                            bootbox.alert({
                                title: "提示",
                                message: json.info,
                                callback: function () {
                                    //刷新当前页
                                    window.location.reload();
                                }
                            });
                        }
                    });

                } else {
                    var parentTimuid = $(".childTimu").attr("data-value");
                    //判断是回撤页面提交,还是录题页面提交
                    var childtype = $(".childTimu").attr("data-type");
                    var reverturl = $(".childTimu").children().attr("href");
                    $.ajax({
                        url: '/EntryExamination/SaveTopic',
                        type: "post",
                        data: { "timu": JSON.stringify(timu), "taskItemId": taskItemId, "parentTmId": parentTimuid === undefined ? "" : parentTimuid },
                        success: function (data) {
                            var json = JSON.parse(data);
                            bootbox.alert({
                                title: "提示",
                                message: json.info,
                                callback: function () {
                                    if (childtype === "revert") {
                                        var nrevert = reverturl.substring(0, reverturl.lastIndexOf("&"));
                                        var nurl = nrevert.replace("create", "edit") + "&timuId=" + json.tmid;
                                        //回撤页面执行重定向
                                        window.location.href = nurl;
                                    } else {
                                        //录题页面执行重定向
                                        var order = url.substring(url.lastIndexOf("=") + 1);
                                        if (order == 0 || order == "") {
                                            url = url.substring(0, url.lastIndexOf("=") + 1) + 1;
                                        }
                                        window.location.href = url + "&timuId=" + json.tmid;
                                    }
                                }
                            });
                        }
                    });
                }
                isValidate = false;
            }
            return false;
        },
        //获取答案选项
        getChoice: function () {
            inputChoices = []; //清空
            $("#choiceList").find(".Topic-editor-section").each(function (i, e) {
                var choice = { choiceContent: "", order: "" };
                choice.choiceContent = e.value;
                //获取选项序号
                choice.order = e.parentNode.previousElementSibling.firstElementChild.value;
                inputChoices.push(choice);
            });
            //获取答案，没有选正确答案，则为""
            choices.rightAnswer = $(".fenzhiLi").find(".answer")[0].value;
            //获取正确答案得分
            choices.score = $("#select_score")[0].value;
            choices.inputChoice = inputChoices;
            timu.choicesIput = choices;
        },
        //初始化加载题目信息
        initTimu: function () {
            //遍历题型赋值
            $("input:radio[name='tmType']").each(function () {
                var e = $(this);
                var value = e.attr("value");
                if (parseInt(value) === parseInt(timuobj.TiMuTypeId)) {
                    //e.attr("checked", "checked");
                    e.parent().addClass("checked");
                    return false;
                }
                return true;
            });
            //遍历类别赋值
            $("input:radio[name='choiceType']").each(function () {
                var e = $(this);
                var id = e.attr("id");
                //判断是否存在用户答案
                if (timuobj.Inputs.length > 0) {
                    timuLeiXing = timuobj.Inputs[0].InputType;
                } else {
                    timuLeiXing = "text";
                };
                if (id === "rdoRdo" && timuLeiXing === "radio") { //单选
                    //e.attr("checked", "checked");
                    e.parent().addClass("checked");
                    return false;
                } else if (id === "rdoCK" && timuLeiXing === "checkbox-all") { //多选
                    //e.attr("checked", "checked");
                    e.parent().addClass("checked");
                    return false;
                } else if (id === "rdoTF" && timuLeiXing === "radio-TF") { //判断
                    //e.attr("checked", "checked");
                    e.parent().addClass("checked");
                    return false;
                } else if (id === "rdoText" && timuLeiXing !== "radio" && timuLeiXing !== "checkbox-all" && timuLeiXing !== "radio-TF") { //填空
                    //e.attr("checked", "checked");
                    e.parent().addClass("checked");
                    return false;
                }
                return true;
            });
            //题干
            $("#TiGan").find(".article_right_xuanxiang").find("#editBox_0").each(function () {
                var e = $(this);
                e.html(timuobj.Trunk);
            });
            //分析赋值
            $("#divAnalyse").find(".article_right_xuanxiang").find("#editBox_1").each(function () {
                var e = $(this);
                e.html(timuobj.Analysis);
            });
            //解答赋值
            $("#divExplain").find(".article_right_xuanxiang").find("#editBox_2").each(function () {
                var e = $(this);
                e.html(timuobj.Answer);
            });
            //点评赋值
            $("#divComment").find(".article_right_xuanxiang").find("#editBox_3").each(function () {
                var e = $(this);
                e.html(timuobj.Comment);
            });
            //大題號 
            $(".largeNumber").val(timuobj.LargeNumber);
            //小題號
            $(".smallNumber").val(timuobj.SmallNumber);
            //頁碼
            $("#pageNumber").val(timuobj.PageNumber);
            //只有单选、多选、判断题才有选项，只有填空题除外
            if (timuobj.Inputs.length === 1) {
                $("#chioce").css("display", "block");
                var html = [];
                var cho = String.empty;
                var inputAnswer = timuobj.Inputs[0].InputAnswer.split(',');
                var editHtml;
                //正确选项
                $("#choiceAnswer").val(entryQuerys.numToABC(inputAnswer));
                //得分
                $("#select_score").attr("value", timuobj.Inputs[0].Score);
                //如果有答案选项
                switch (timuLeiXing) {
                    case "radio":
                        for (var i = 0; i < timuobj.Inputs[0].InputChoice.length; i++) {
                            choiceIndex++;
                            editBoxId++;
                            cho = String.fromCharCode(64 + choiceIndex);
                            html.push('<div id="chContent" data-choiceId="' + timuobj.Inputs[0].InputChoice[i].Id + '">');
                            if (parseInt(inputAnswer[0]) === choiceIndex) {
                                html.push(' <div class="article_left_xuanxiang select">');
                            } else {
                                html.push(' <div class="article_left_xuanxiang">');
                            }
                            html.push('     <input type="radio" id="i_' + tmObj.TMID + '_' + choiceIndex + '" value="' + choiceIndex + '" name="' + tmObj.TMID + '">');
                            html.push('     <label for="i_' + tmObj.TMID + '_' + choiceIndex + '">' + cho + '</label>');
                            html.push(' </div>');
                            //赋值,填充选项答案
                            parameters.content = timuobj.Inputs[0].InputChoice[i].ChoiceContent;
                            //创建题目编辑器
                            parameters.contentId = "editBox_" + editBoxId;
                            editHtml = entryQuerys.myEditor().init.createTimuEditor(parameters);
                            html.push(' <div class="Topic-editor-box article_right_xuanxiang" data-edit="' + editBoxId + '">' + editHtml + '</div>');
                            html.push('</div>');
                            $("#choiceList").html(html.join('\r\n'));
                        }
                        //重新绑定选项单击事件
                        this.bindInputChoice("radio");
                        break;
                    case "checkbox-all":
                        var rightIndex = 0;
                        for (var i = 0; i < timuobj.Inputs[0].InputChoice.length; i++) {
                            choiceIndex++;
                            editBoxId++;
                            cho = String.fromCharCode(64 + choiceIndex);
                            html.push('<div id="chContent" data-choiceId="' + timuobj.Inputs[0].InputChoice[i].Id + '">');
                            if (parseInt(inputAnswer[rightIndex]) === choiceIndex && rightIndex < inputAnswer.length) {
                                html.push(' <div class="article_left_xuanxiang select">');
                                if (inputAnswer.length > 1) {
                                    rightIndex++;
                                }
                            } else {
                                html.push(' <div class="article_left_xuanxiang">');
                            }
                            html.push('     <input type="checkbox" id="i_' + tmObj.TMID + '_' + choiceIndex + '" value="' + choiceIndex + '" name="' + tmObj.TMID + '">');
                            html.push('     <label for="i_' + tmObj.TMID + '_' + choiceIndex + '">' + cho + '</label>');
                            html.push(' </div>');
                            //赋值,填充选项答案
                            parameters.content = timuobj.Inputs[0].InputChoice[i].ChoiceContent;
                            //创建题目编辑器
                            parameters.contentId = "editBox_" + editBoxId;
                            editHtml = entryQuerys.myEditor().init.createTimuEditor(parameters);
                            html.push(' <div class="Topic-editor-box article_right_xuanxiang" data-edit="' + editBoxId + '">' + editHtml + '</div>');
                            html.push('</div>');
                            $("#choiceList").html(html.join('\r\n'));
                        }
                        //重新绑定选项单击事件
                        entryQuerys.bindInputChoice("checkbox");
                        break;
                    case "radio-TF":
                        if (timuobj.Inputs[0].InputChoice.Count <= 1) {
                            choiceIndex++;
                            editBoxId++;
                            cho = String.fromCharCode(64 + choiceIndex);
                            html.push('<div>');
                            if (inputAnswer[0] === choiceIndex) {
                                html.push(' <div class="article_left_xuanxiang select">');
                            } else {
                                html.push(' <div class="article_left_xuanxiang">');
                            }
                            html.push(' <input type="radio" id="i_' + tmObj.TMID + '_1" value="1" name="' + tmObj.TMID + '">');
                            html.push(' <label for="i_' + tmObj.TMID + '_1">T</label>');
                            html.push(' </div>');
                            //赋值,填充选项答案
                            parameters.content = timuobj.Inputs[0].InputChoice[0].ChoiceContent;
                            //创建题目编辑器
                            parameters.contentId = "editBox_" + editBoxId;
                            editHtml = entryQuerys.myEditor().init.createTimuEditor(parameters);
                            html.push(' <div class="Topic-editor-box article_right_xuanxiang" data-edit="' + editBoxId + '" style="width: 98.1%;">' + editHtml + '</div>');
                            html.push('</div>');
                            $("#choiceList").html(html.join('\r\n'));
                        } else {
                            choiceIndex++;
                            editBoxId++;
                            html.push('<div>');
                            if (parseInt(inputAnswer[0]) === choiceIndex) {
                                html.push(' <div class="article_left_xuanxiang select">');
                            } else {
                                html.push(' <div class="article_left_xuanxiang">');
                            }
                            html.push(' <input type="radio" id="i_' + tmObj.TMID + '_1" value="1" name="' + tmObj.TMID + '">');
                            html.push(' <label for="i_' + tmObj.TMID + '_1">T</label>');
                            html.push(' </div>');
                            //赋值,填充选项答案
                            parameters.content = timuobj.Inputs[0].InputChoice[0].ChoiceContent;
                            //创建题目编辑器
                            parameters.contentId = "editBox_" + editBoxId;
                            editHtml = entryQuerys.myEditor().init.createTimuEditor(parameters);
                            html.push(' <div class="Topic-editor-box article_right_xuanxiang" data-edit="' + editBoxId + '" style="width: 98.1%;">' + editHtml + '</div>');
                            html.push('</div>');
                            //赋值,填充选项答案
                            parameters.content = timuobj.Inputs[0].InputChoice[0].ChoiceContent;
                            //创建题目编辑器
                            this.bingEditor();
                            choiceIndex++;
                            editBoxId++;
                            html.push('<div>');
                            if (parseInt(inputAnswer[0]) === choiceIndex) {
                                html.push(' <div class="article_left_xuanxiang select">');
                            } else {
                                html.push(' <div class="article_left_xuanxiang">');
                            }
                            html.push(' <input type="radio" id="i_' + tmObj.TMID + '_2" value="2" name="' + tmObj.TMID + '">');
                            html.push(' <label for="i_' + tmObj.TMID + '_2">F</label>');
                            html.push(' </div>');
                            //赋值,填充选项答案
                            parameters.content = timuobj.Inputs[0].InputChoice[1].ChoiceContent;
                            //创建题目编辑器
                            parameters.contentId = "editBox_" + editBoxId;
                            editHtml = entryQuerys.myEditor().init.createTimuEditor(parameters);
                            html.push(' <div class="Topic-editor-box article_right_xuanxiang" data-edit="' + editBoxId + '" style="width: 98.1%;">' + editHtml + '</div>');
                            html.push('</div>');
                            $("#choiceList").html(html.join('\r\n'));
                        }
                        //重新绑定选项单击事件
                        entryQuerys.bindInputChoice("radio-TF");
                        break;
                    default:
                        $("#chioce").css("display", "none");
                        break;
                }
            }
            oldEditBoxId = editBoxId;
        },
        //大题号、小题号、页码错误提示
        entryValidate: function (e) {
            var value = e.value;
            var tel = /^[1-9]\d*$/;
            var re = tel.test(value);
            if (!re) {
                e.style.borderColor="red";
            } else {
                e.style.borderColor = "";
            }
        },
        //验证
        validate: function () {
            var element = document.getElementById("pageNumber");
            var color = element.style.borderColor;
            if (color === "red") {
                isValidate = true;
            } else {
                isValidate = false;
            }
        },
        //添加子题目
        addChildTimu: function (e) {
            var url = e.attr("data-href");
            var tmid = e.attr("data-timuId");
            if (tmid !== "" && tmid !== undefined) {
                var lastChildLi = $("#" + tmid);
                var childCount = lastChildLi.attr("data-count");
                var ctmid = lastChildLi.find("a").attr("data-timuId");
                if (ctmid === "") {
                    bootbox.alert({
                        title: "提示",
                        message: "请录完第" + childCount + "道子题目再添加!",
                        callback: function () { }
                    });
                } else {
                    // 执行重定向,添加一道子题目
                    window.location.href = url;
                }
            } else {
                bootbox.alert({
                    title: "提示",
                    message: "请录完父题目再添加子题目!",
                    callback: function () { }
                });
            }
        }
    };

    //回撤页面
    var reverteQuerys = {
        init: function () {
            reverteQuerys.downListBind();
            reverteQuerys.initEdit();
            reverteQuerys.circleBtnBind();
            //隐藏下拉框后面的删除符号
            $(".select2-search-choice-close").hide();
        },
        //下拉框事件绑定
        downListBind: function () {
            //绑定主知识点、相关知识点下拉选项
            for (var i = 0; i < revertobj.KnowledgeSource.length; i++) {
                var know = revertobj.KnowledgeSource[i];
                var maink = revertobj.MainTiMuKnowledge;
                if (maink != null) {
                    if (maink.Name !== know.Name) {
                        $("#main").append("<option value='" + know.Id + "'>" + know.Name + "</option>");
                    }
                } else {
                    $("#main").append("<option value='" + know.Id + "'>" + know.Name + "</option>");
                }
                var re = reverteQuerys.knowMinorFind(know.Name);
                if (re === 0) {
                    $("#minor").append("<option value='" + know.Id + "'>" + know.Name + "</option>");
                }
            }
            $("#main").change(function () {
                var text = $("#main option:selected").text();
                var val = $("#main option:selected").val();
                var index = $("#minor option:selected").index();
                var lg = $(".main").find("div").length;
                if (lg === 0) {
                    reverteQuerys.appendHtml(".main", text, val, index, "append");
                } else {
                    reverteQuerys.appendHtml(".main", text, val, index, "html");
                }
                reverteQuerys.circleBtnBind();
            });
            $("#minor").change(function () {
                var text = $("#minor option:selected").text();
                var val = $("#minor option:selected").val();
                var index = $("#minor option:selected").index();
                reverteQuerys.appendHtml(".minor", text, val, index, "append");
                //移除选中项
                $("#minor option[value=" + val + "]").remove();
                reverteQuerys.circleBtnBind();
            });
            //绑定上传视频点击事件
            $(".uploadVideo").click(function () {
                reverteQuerys.reverteSave();
            });
            //绑定保存事件
            $(".markSave").click(function () {
                reverteQuerys.saveMark();
            });
        },
        //遍历查找，是否存在相同知识点属性
        eachIsCircle: function (eClass, mainKnowledge) {
            var lg = $(eClass).find("div").length;
            if (lg === 0) {
                isCircleFind = false;
            } else {
                $(eClass).find("div").each(function () {
                    var e = $(this);
                    var first = e.children(":first");
                    if (mainKnowledge === first.text()) {
                        isCircleFind = true;
                        return false;
                    }
                    isCircleFind = false;
                    return true;
                });
            }
        },
        //拼接标签
        appendHtml: function (nClass, selected, value, index, state) {
            //如果不存在，则添加
            var html = "<div class='btn-group btn-group-circle $type' style='margin-top: 10px; margin-right:10px;' data-value='$value' data-index='$index'>" +
                            "<button type='button' class='btn btn-default'>$name</button>" +
                            "<button type='button' class='btn btn-circle-right btn-default dropdown-toggle1 knowledge' data-toggle='dropdown'>" +
                                "<i class='fa fa-times'></i>" +
                            "</button>" +
                            "</button>" +
                        "</div>";
            html = html.replace("$value", value).replace("$name", selected).replace("$index", index);
            if (nClass === ".minor") {
                html = html.replace("$type", "minorT");
            } else {
                html = html.replace("$type", "");
            }
            if (state === "append") {
                $(nClass).append(html.trim());
            } else if (state === "html") {
                $(nClass).html(html.trim());
            }
        },
        //绑定按钮删除事件
        circleBtnBind: function () {
            $(".knowledge").each(function () {
                var e = $(this);
                e.unbind().click(function () {
                    reverteQuerys.removeBtn(e.parent());
                });
            });
        },
        //删除删除知识点元素
        removeBtn: function (e) {
            e.remove();
            $.ajax({
                url: "/EntryExamination/DeleteKnowledgeById",
                type: "post",
                data: { knowledgeId: e.attr("data-value"), tmid: revertobj.Tmid },
                success: function () {
                    e.remove();
                    //被删掉的相关知识点，又生成到下拉框对应的位置
                    if (e.attr("class").indexOf("minorT") > 0) {
                        var text = e.children(":first").text();
                        var value = e.attr("data-value");
                        var index = e.attr("data-index");
                        //创建选项
                        var option = document.createElement("option");
                        option.value = value;
                        option.innerText = text;
                        if (index === "0") {
                            index = 1;
                        }
                        $("#minor")[0].options.add(option, index);
                    }
                }
            });
        },
        //保存到json对象
        reverteSaveToJson: function () {
            //题目id
            reverteJson.tmid = revertobj.Tmid;
            //获取主知识点
            $(".main").find("div").each(function () {
                var e = $(this);
                reverteJson.mainId = e.attr("data-value");
            });
            var minorkg = "";
            //相关知识点
            $(".minor").find("div").each(function () {
                var e = $(this);
                minorkg += e.attr("data-value") + ",";
            });
            //移除最后一个逗号
            minorkg = minorkg.substring(0, minorkg.lastIndexOf(","));
            reverteJson.minorIds = minorkg;
            //难度等级
            var diff = $(".caption .label").text();
            switch (diff) {
                case "Not Rated": reverteJson.diff = 0; break;
                case "Half Star": reverteJson.diff = 0.5; break;
                case "One Star": reverteJson.diff = 1; break;
                case "One & Half Star": reverteJson.diff = 1.5; break;
                case "Two Stars": reverteJson.diff = 2; break;
                case "Two & Half Stars": reverteJson.diff = 2.5; break;
                case "Three Stars": reverteJson.diff = 3; break;
                case "Three & Half Stars": reverteJson.diff = 3.5; break;
                case "Four Stars": reverteJson.diff = 4; break;
                case "Four & Half Stars": reverteJson.diff = 4.5; break;
                case "Five Stars": reverteJson.diff = 5; break;
                default:
            }
            //微课视频
            //reverteJson.videoCode = $("#topicVideo").val();
            //讲解老师
            reverteJson.teacher = $(".teacher").text();
            //保存标记错误信息
            reverteJson.errorMessage = $(".errorMessage").text();
        },
        //编辑
        initEdit: function () {
            var index = -1;
            //主知识点
            if (revertobj.MainTiMuKnowledge != null) {
                var maink = revertobj.MainTiMuKnowledge;
                index = reverteQuerys.knowIndexof(maink);
                reverteQuerys.appendHtml(".main", maink.Name, maink.Id, index, "html");
            }
            //相关知识点
            var list = revertobj.MinorTiMuKnowledge;
            for (var i = 0; i < list.length; i++) {
                var know = list[i];
                index = reverteQuerys.knowIndexof(know);
                reverteQuerys.appendHtml(".minor", know.Name, know.Id, index, "append");
            }
            //难度等级
            $('#input-7-lg').rating('update', revertobj.Diff);
            //视频id
            //$("#topicVideo").val(revertobj.VideoId);
            //讲解老师
            $(".teacher").val(revertobj.Teacher);
            //标记错误信息
            $(".errorMessage").text(revertobj.ErrorMessage);
            //禁用
            $(".errorMessage").attr("disabled", "disabled");
        },
        //查询当前知识点所在位置下标
        knowIndexof: function (e) {
            var index = -1;
            for (var i = 0; i < revertobj.KnowledgeSource.length; i++) {
                var know = revertobj.KnowledgeSource[i];
                if (know.Name === e.Name) {
                    index = i;
                    return index;
                }
            }
            return index;
        },
        //查找是否存在知识点
        knowMinorFind: function (name) {
            var re = 0;
            var minors = revertobj.MinorTiMuKnowledge;
            for (var i = 0; i < minors.length; i++) {
                if (minors[i].Name === name) {
                    re = 1;
                    return re;
                }
            }
            return re;
        },
        //保存题目标定属性到数据库
        saveMark: function () {
            //保存到json对象
            reverteQuerys.reverteSaveToJson();
            if (reverteJson.mainId === "") {
                bootbox.alert({
                    title: "提示",
                    message: "主知识点不能为空!",
                    callback: function () { }
                });
                return;
            }
            //判断难度
            if (reverteJson.diff === 0) {
                bootbox.alert({
                    title: "提示",
                    message: "难度不能为空!",
                    callback: function () { }
                });
                return;
            }
            //提交到数据库
            $.ajax({
                url: '/EntryExamination/SaveTiMuMark',
                type: "post",
                data: { "property": JSON.stringify(reverteJson) },
                success: function (data) {
                    bootbox.alert({
                        title: "提示",
                        message: data.result,
                        callback: function () { }
                    });
                }
            });
        }
    }

    function init(timuobject, revertobject, taskitemId) {
        timuobj = timuobject;
        tmObj = { TMID: timuobj.Id };
        taskItemId = taskitemId;
        entryQuerys.myEditor();
        entryQuerys.init();
        revertobj = revertobject;
        if (revertobject != null) {
            reverteQuerys.init();
        }
    }
    return { init: init, myEditor: entryQuerys.myEditor };
}();