(function ($) {
    function _getFormData(frm) {
        var data = {};
        frm.find("input,textarea,select").each(function () {
            if (this.name) {
                data[this.name] = $(this).val();
            }
        });
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
        getForm: function (title, name, callback, validate) {
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
                        var data = _getFormData(frm);
                        _clearErrorMsg(frm);
                        if (!validate || validate(data, errorCallback)) {
                            callback(data);
                            if (__win) __win.remove();
                        }
                    },
                    loadCallback: function (win) {
                        __win = win;
                    }
                });
            });
        }
    });
})($);