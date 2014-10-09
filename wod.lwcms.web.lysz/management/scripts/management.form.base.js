/* controls treesubform */
(function ($, wodForm) {
    function _wodtreesubform(setting) {
        var __feed = 0;
        function getNewTid() {
            return 'tn' + (__feed++).toString();
        }
        var _input = this;
        var _def = {
            edittype: 'node'//'none','content','node'
        };
        setting = $.extend(_def, setting);
        var preHandlerArr = setting.formHandler.preSubmitHandler;

        var inputs = {};
        function _updateInput() {
        }
        var html = [];
        html.push("<div class='f-tree-wrap'>");
        var cmdBar = setting.edittype == "node";
        if (cmdBar) {
            html.push("<div class='tcmd'><a href='javascript:;' class='tadd' data-action='add'>添加</a><a href='javascript:;' class='tadds' data-action='adds'>添加子节点</a><a href='javascript:;' class='tup' data-action='tup'>上移</a><a href='javascript:;' class='tdown' data-action='tdown'>下移</a></div>"); //<a href='javascript:;' class='tlvlup' data-action='tlvlup'>升级</a><a href='javascript:;' class='tlvldown' data-action='tlvldown'>降级</a>
        }
        var conteneEdit = setting.edittype != "none";
        var data = _input.wval();
        function getChildren(item) {
            return item[setting.childrenprop];
        }
        function getOrCreateChildren(item) {
            item[setting.childrenprop] = item[setting.childrenprop] || [];
            return item[setting.childrenprop];
        }
        var preHandlerArr = setting.formHandler.preSubmitHandler;
        preHandlerArr.push(function () {
            var d = $.extend(true, [], data);
            var deltid = function (arr) {
                for (var i = 0, length = arr.length; i < length; i++) {
                    delete arr[i]["tid"];
                    deltid(getChildren(arr[i]));
                }
            };
            deltid(d);
            _input.wval(d);
        });
        function getText(item) {
            return htmlEncode(item[setting.titleprop]);
        }
        function getDataHtml(item) {
            var c = getChildren(item);
            item.tid = getNewTid();
            return "<li class='tnode' data-tid='" + item.tid + "'><span class='tit'>" + getText(item) + "</span><span class='tncmd'>" + (!cmdBar ? "" : "<a href='javascript:;' class='del'  data-action='ndel'>删除</a>") + (!conteneEdit ? "" : "<a href='javascript:;' class='edit' data-action='nedit'>编辑</a>") + "</span>" + (c && c.length ? getListDataHtml(c) : "") + "</li>";
        }
        function getListDataHtml(arr, r) {
            var html = [];
            html.push("<ul class='f-tree" + (r ? " f-root" : "") + "'>");
            $.each(arr, function () {
                html.push(getDataHtml(this));
            });
            html.push("</ul>");
            return html.join('');
        }
        html.push(getListDataHtml(data, true));
        html.push("</div>");
        var editorDom = $(html.join(''));
        var actions = {
            updateLayout: function () {
                editorDom.find(".f-root").remove();
                editorDom.append(getListDataHtml(data, true));
            }
            , add: function () {
                actions.showForm({}, function (d) {
                    if (d && getText(d)) {
                        var currDom = actions.getCurr();
                        if (currDom && currDom.parents(".tnode").length) {
                            var pitem = actions.getData(currDom.parents(".tnode"));
                            getOrCreateChildren(pitem).push(d);
                        } else {
                            data.push(d);
                        }
                        actions.updateLayout();
                    }
                });
            }
            , adds: function () {
                actions.showForm({}, function (d) {
                    if (d && getText(d)) {
                        var curr = actions.getData(actions.getCurr());
                        if (curr) {
                            getOrCreateChildren(curr).push(d);
                        }
                        else {
                            data.push(d);
                        }
                        actions.updateLayout();
                    }
                });
            }
            , tup: function () {
                var currDom = actions.getCurr();
                if (currDom) {
                    var curr = actions.getData(currDom);
                    var arr;
                    if (currDom.parents(".tnode").length) {
                        var pitem = actions.getData(currDom.parents(".tnode"));
                        arr = getOrCreateChildren(pitem);
                    }
                    else {
                        arr = data;
                    }
                    for (var i = 0; i < arr.length; i++) {
                        if (arr[i] == curr) {
                            if (i > 0) {
                                var item = arr.splice(i - 1, 1);
                                arr.splice(i, 0, item[0]);
                            }
                            break;
                        }
                    }
                    actions.updateLayout();
                }
            }
            , tdown: function () {
                var currDom = actions.getCurr();
                if (currDom) {
                    var curr = actions.getData(currDom);
                    var arr;
                    if (currDom.parents(".tnode").length) {
                        var pitem = actions.getData(currDom.parents(".tnode"));
                        arr = getOrCreateChildren(pitem);
                    }
                    else {
                        arr = data;
                    }
                    for (var i = 0; i < arr.length; i++) {
                        if (arr[i] == curr) {
                            if (i < arr.length - 1) {
                                var item = arr.splice(i + 1, 1);
                                arr.splice(i, 0, item[0]);
                            }
                            break;
                        }
                    }
                    actions.updateLayout();
                }
            }
            , tlvlup: function () { }
            , tlvldown: function () { }
            , ndel: function () {
                var currDom = $(this).parents(".tnode:eq(0)");
                if (currDom) {
                    var curr = actions.getData(currDom);
                    var arr;
                    if (currDom.parents(".tnode").length) {
                        var pitem = actions.getData(currDom.parents(".tnode"));
                        arr = getOrCreateChildren(pitem);
                    }
                    else {
                        arr = data;
                    }
                    for (var i = 0; i < arr.length; i++) {
                        if (arr[i] == curr) {
                            arr.splice(i, 1);
                            break;
                        }
                    }
                    actions.updateLayout();
                }
            }
            , nedit: function () {
                var currDom = $(this).parents(".tnode:eq(0)");
                if (currDom) {
                    var curr = actions.getData(currDom);
                    actions.showForm(curr, function (d) {
                        actions.updateLayout();
                    });
                }
            }
            , showForm: function (data, callback) {
                var d = data;
                $.ui.getForm(setting.formTitle || "", setting.formName, d
                    , function (frm) {
                        if (setting.schs) {
                            frm.wodForm({ schema: setting.schs });
                        }
                    }
                    , function (data) {
                        if (data) {
                            callback($.extend(d, data));
                        }
                    }, function (data, errorCallback) {
                        return setting.validate ? setting.validate(data, errorCallback) : true;
                    });
            }
            , getCurr: function () {
                return editorDom.find(".curr");
            }
            , getData: function (dom) {
                if (dom) {
                    var $dom = (dom instanceof $) ? dom : $(dom);
                    var tid = '';
                    if ($dom.hasClass("tnode")) {
                        tid = $dom.data("tid");
                    }
                    else {
                        tid = $dom.parents('.tnode:eq(0)').data("tid");
                    }
                    return actions.getDataByTid(data, tid);
                }
                else
                    return null;
            }
            , getDataByTid: function (arr, tid) {
                for (var i = 0; i < arr.length; i++) {
                    if (arr[i].tid == tid) {
                        return arr[i];
                    }
                    else {
                        var c = getChildren(arr[i]);
                        if (c && c.length) {
                            var snode = actions.getDataByTid(c, tid);
                            if (snode) return snode;
                        }
                    }
                }
                return null;
            }
        };
        editorDom.on("click", '[data-action]', function () {
            actions[$(this).data('action')].call(this);
        });
        editorDom.on("click", ".tnode", function () {
            editorDom.find(".curr").removeClass("curr");
            $(this).addClass("curr");
            return false;
        });
        _input.attr("type", "hidden").hide();
        _input.after(editorDom);
    }
    $.fn.extend({ wodtreesubform: _wodtreesubform });

    var _wodnavitree = function (setting) {
        var _input = this;
        var data = _input.wval();
        var editorDom = $("<div class='f-multitree'/>");
        for (var key in data) {
            editorDom.append("<span class='treename'>" + htmlEncode(setting.names[key] || key) + "</span><br>");
            editorDom.append("<input type='text' wod data-navikey='" + key + "'>");
        }
        editorDom.find("input").each(function () {
            var key = $(this).data("navikey");
            $(this).wval(data[key]);
            $(this).wodtreesubform(setting);
        });
        var preHandlerArr = setting.formHandler.preSubmitHandler;
        preHandlerArr.push(function () {
            var d = {};
            editorDom.find("[data-navikey]").each(function () {
                d[$(this).data("navikey")] = $(this).wval();
            });
            _input.wval(d);
        });
        _input.attr("type", "hidden").hide();
        _input.after(editorDom);
    };
    $.fn.extend({ wodnavitree: _wodnavitree });
})($, _$wod_form);

(function (w,wodForm,$) {
    var catSch = {
        name: {display:"分类名称",type:"wodtext",setting:{}}
        , code: {display:"分类代码",type:"wodtext",setting:{}}
        , description: {display:"描述",type:"wodtext",setting:{}}
        , content: {display:"内容",type:"wodrichtext",setting:{type : "simple"}}
        , keywords: {display:"关键字",type:"wodtext",setting:{}}
        , page: {display:"列表显示类型",type:"wodtext",setting:{}}
        , contentpage: {display:"内容显示类型",type:"wodtext",setting:{}}
        , extendform: {display:"关联数据类型",type:"wodtext",setting:{}}
    };
    var naviSch = {
        name: {display:"导航名称",type:"wodtext",setting:{}}
        , title: {display:"导航提示",type:"wodtext",setting:{}}
        , naviUrl: {display:"链接地址",type:"wodtext",setting:{}}
        , target: {display:"目标窗口",type:"wodtext",setting:{}}
    };
    var baseSch = {
        siteName: {display:"站点名称",type:"wodtext",setting:{}}
        , siteUrl: {display:"站点链接",type:"wodtext",setting:{}}
        , description: {display:"描述",type:"wodtext",setting:{}}
        , copyright: {display:"版权信息",type:"wodrichtext",setting:{type : "simple"}}
        , keywords: {display:"关键字",type:"wodtext",setting:{}}
        , title: {display:"站点标题",type:"wodtext",setting:{}}
        , navis: {display:"导航链接",type:"wodnavitree",setting:{names:{"top":"顶部导航","bottom":"底部导航","main":"主导航"},formName:"naviform",formTitle:"导航",titleprop:"name",childrenprop:"subNavis",validate:function(){return true },schs:naviSch}}
        , allCats: {display:"所有分类",type:"wodtreesubform",setting:{formName:"catform",formTitle:"分类",titleprop:"name",childrenprop:"subCategory",validate:function(){return true },schs:catSch}}
    };
    wodForm.baseSch = baseSch;
})(window,_$wod_form,$);