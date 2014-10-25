(function () {

    var tmpUrl = "/management/artforms/";
    $.extend({
        ui: {
            loadTmp: function (name, callback) {
                this.tmpCache = this.tmpCache || {};
                if (this.tmpCache[name]) {
                    callback($(this.tmpCache[name]));
                }
                else {
                    var _cache = this.tmpCache;
                    $.get(tmpUrl + name + ".html", function (html) {
                        _cache[name] = html;
                        callback($(html));
                    });
                }
            },
            window: function ($content, setting) {
                var _defSet = { defShow: true, width: 480, height: 160 };
                setting = $.extend(_defSet, setting);
                this.loadTmp("window", function (win) {
                    var t = win.find(".u-tt");
                    var cl = win.find(".lyclose");
                    var m = win.find(".lymask");
                    var cnt = win.find(".lyct");
                    var ok = win.find(".lybtns button").first();
                    var cancel = win.find(".lybtns button").last();
                    var info = win.find(".lyother");

                    var lywrap = win.find(".lywrap");
                    if (setting.width) lywrap.css("width", setting.width + "px");
                    if (setting.height) cnt.css("min-height", setting.height + "px");

                    cnt.append($content);
                    if (setting.title) { t.text(setting.title); }
                    if (setting.info) { info.text(setting.info); }

                    $("body").append(win);
                    if (!setting.defShow) {
                        win.hide();
                    }
                    function _closeWin() {
                        win.remove();
                        if (setting.closeCallback) setting.closeCallback();
                        return false;
                    }
                    function _okWin() {
                        if (setting.okCallback) setting.okCallback();
                        return false;
                    }
                    cl.click(_closeWin);
                    m.click(_closeWin);
                    cancel.click(_closeWin);
                    ok.click(_okWin);
                    if (setting.loadCallback) setting.loadCallback(win);
                });
            }
        }
    });


    function _getFormData(frm) {
        var data = {};
        frm.find("input,textarea,select").each(function () {
            if (this.name) {
                data[this.name] = $(this).val();
            }
        });
        return data;
    }
    function _setFormData(frm, data) {
        for (var name in data) {
            frm.find("[name='" + name + "']").val(data[name]);
        }
        return data;
    }
    function _showErrorMsg(frm, name, msg) {
        var label = frm.find("[name='" + name + "']").parents("label");
        label.addClass("err");
        label.append("<span class='errtxt'>" + msg + "</span>");
    }
    function _clearErrorMsg(frm) {
        frm.find("label").removeClass("err");
        frm.find("label .errtxt").remove();
    }
    $.extend($.ui, {
        getForm: function (title, name, loaded, callback, validate) {
            this.loadTmp(name, function (frm) {
                var __win;
                var errorCallback = function (name, msg) {
                    _showErrorMsg(frm, name, msg);
                };
                $.ui.window(frm, {
                    title: title,
                    info: "填写表单信息",
                    closeCallback: function () {
                        callback(undefined);
                    },
                    okCallback: function () {
//                        var data = _getFormData(frm);
                        _clearErrorMsg(frm);
                        if (!validate || validate(frm, errorCallback)) {
                            callback(frm);
                            if (__win) __win.remove();
                        }
                    },
                    loadCallback: function (win) {
                        __win = win;
//                        if (data) {
//                            _setFormData(frm, data);
//                        }
                        loaded(frm);
                    }
                });
            });
        }
    });

    var wodMF = {};
    $.extend({
        wodMF: wodMF
    });
    wodMF.menu = function (p) {
        var $m = $(p);
    };
})($, window);