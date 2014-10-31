var test = (function (window, body) {
    var trp = window.document.createElement("div");
    body.appendChild(trp);
    function writeResult(panel,msg) {
        var result = window.document.createElement("div");
        result.innerHTML = encodeHTML(msg);
        panel.appendChild(result);
    }
    return function (item, arg) {
        var itemPanel = window.document.createElement("div");
        itemPanel.innerHTML = "<p>" +encodeHTML(item)+"</p>";
        trp.appendChild(itemPanel);
        var msgPanel = window.document.createElement("div");
        itemPanel.appendChild(msgPanel);
        if (typeof (arg) == "boolean") {
        }
        else if (typeof (arg) == "function") {
            arg = arg(function (msg) {
                writeResult(msgPanel, msg);
            });
        }
        writeResult(msgPanel, arg ? "成功" : "失败");
        if (arg) {
            itemPanel.style["background"] = "#0f0";
        }
        else {
            itemPanel.style["background"] = "#f00";
        }
    };
})(window, window.document.body);

test("Function.bind",
            (function () {
                return 1;
            }).bind({})() == 1
            && (function () {
                return this.name;
            }).bind({ name: 1 })() == 1
            && (function (a) {
                return this.name + a;
            }).bind({ name: 1 },1)() == 2);

(function (wod) {
    test("框架注册", !!wod && !!wod.CLS);
    test("注册名称空间", !!wod.CLS.getNS("mini.ui") && !!mini.ui && wod.CLS.getNS("mini.ui") == mini.ui);
    test("注册类", !!wod.CLS.getClass("mini.ui.TextBox") && !!mini.ui.TextBox);

    mini.ui.TextBox.prototype.getText = function () {
        return "abc";
    };

    test("继承类", !!wod.CLS.getClass("mini.ui.SubTextBox", "mini.ui.TextBox")
        && !!mini.ui.SubTextBox
        && mini.ui.SubTextBox.isSubclassOf(mini.ui.TextBox));

    test("继承类-继承方法验证1", function (msgWrite) {
        var sub = new mini.ui.SubTextBox();
        return sub.getText && sub.getText() == "abc";
    });

    test("继承类-继承方法验证2", function (msgWrite) {
        var subCLS = wod.CLS.getClass({
            getText: function () {
                return "h" + this.parent();
            },
            hh: function () {
                return this.getText();
            }
        }, "mini.ui.SubTextBox1", "mini.ui.SubTextBox");
        var sub = new subCLS();
        return sub.getText && sub.getText() == "habc" && sub.hh;
    });
})(wod);


(function (wod, forms) {
    test("Form引用", forms && !!forms.Form);


    var fieldsDefine = {

    };
    var data = {

    };
    var form = new forms.Form();
    form.init("body", fieldsDefine);
    form.sync();
    var data1 = form.getData();
    form.setData(data);
    var url = "";
    form.registValidate(function (data) {
        return false;
    });
    form.validate();
    form.tipError("name", "名称不能重复");
    form.submit(url);
})(wod,wod.CLS.getNS("wod.forms"));



//$("body").wodForm({
//    schema: _$wod_form.artSch
//    , submit: { selector: "#fsubmit"
//        , event: "click"
//        , posts: { url: "artadd.aspx?id=<%=Request.QueryString["id"] %>", callback : function(){ alert("保存成功");} }
//        , validate : function(data,errorCallback){
//            errorCallback("name","不能为空");
//            return true;
//        }
//        , preSubmit : function(){
//        }
//}
//});
