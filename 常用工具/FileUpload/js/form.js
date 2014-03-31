(function ($) {
    function _getFormData(frm) {
        var data = {};
        function _updateData($dom, pnarr, pn, val) {
            if (pnarr.length > 0) {
                var list = $dom.parents("[list-name='" + pnarr.join(".") + "'][data-form=list]");
                var row = $dom.parents("[list-name='" + pnarr.join(".") + "'][data-form=list] [data-form=row]");
                var subform = $dom.parents("[subform-name='" + pnarr.join(".") + "'][data-form=subform]");
                if (row.length == 1) {
                    var listdata = list.data("formdata");
                    if (listdata) {
                    }
                    else {
                        listdata = [];
                        list.data("formdata", listdata);
                        var lstpnarr = list.attr("list-name").split(".");
                        var lstpn = lstpnarr.pop();
                        _updateData(list, lstpnarr, lstpn, listdata);
                    }
                    listdata = listdata || [];
                    var rowdata = row.data("formdata");
                    if (rowdata) {
                    }
                    else {
                        rowdata = {};
                        listdata.push(rowdata);
                        row.data("formdata", rowdata);
                    }
                    rowdata[pn] = val;
                } else if (subform.length == 1) {
                    var subformdata = subform.data("formdata");
                    if (subformdata) {
                    }
                    else {
                        subformdata = {};
                        subform.data("formdata", subformdata);
                        var sfpnarr = subform.attr("subform-name").split(".");
                        var sfpn = sfpnarr.pop();
                        _updateData(subform, sfpnarr, sfpn, subformdata);
                    }
                    subformdata[pn] = val;
                }
            } else {
                data[pn] = val;
            }
        }
        frm.find("input,textarea,select").each(function () {
            if (this.name) {
                var nameparse = this.name.split(".");
                var pn = nameparse.pop();
                var val = $(this).val();
                _updateData($(this), nameparse, pn, val);
            }
        });
        frm.find("[data-form=list],[data-form=row],[data-form=subform]").each(function () {
            $(this).data("formdata", null);
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
    $.extend({
        achiveForm: function (selector, callback, validate) {
            $(selector).each(function () {
                var frm = $(this);
                var __win;
                var errorCallback = function (name, msg) {
                    _showErrorMsg(frm, name, msg);
                };
                frm.find("[data-form=ok]").click(function () {
                    var data = _getFormData(frm);
                    _clearErrorMsg(frm);
                    if (!validate || validate(data, errorCallback)) {
                        callback(data);
                        if (__win) __win.remove();
                    }
                });
            });
        }
    });
})($);