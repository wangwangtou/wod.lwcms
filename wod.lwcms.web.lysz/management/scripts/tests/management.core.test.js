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

/* 框架测试 */
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

/* form基础：dom操作封装 */
(function (wod) {
    function domManagerInterfaceTest(managerClass) {
        var manager;
        var formPanel;
        test("domManager测试", function (writeMsg) {
            writeMsg("当前接口:" + managerClass._typename);
            managerClass.$ = window.jQuery;
            var formBody = "<input type='text' name='field1'>";
            formPanel = document.createElement("div");
            formPanel.innerHTML = formBody;
            manager = new managerClass();
            manager.init(formPanel);
            return true;
        });

        test("domManager getByName", function () {
            return manager.getByName("field1") == formPanel.getElementsByTagName("input")[0];
        });
        test("domManager getField", function () {
            var field = manager.getField("field1");
            return !field.getValue() && (field.setValue("abc"), formPanel.getElementsByTagName("input")[0].value == "abc");
        });
    }
    domManagerInterfaceTest(wod.CLS.getClass("wod.dom.jQueryDomManager"));
})(wod);

/* form操作 */
(function (wod, forms) {
    test("Form引用", forms && !!forms.Form);
    var formPanel;
    var form;
    test("Form init & getField", function () {
        var formBody = "<input type='text' name='field1'>";
        formPanel = document.createElement("div");
        formPanel.innerHTML = formBody;
        var fieldsDefine = [
            { name: "field1", autoSync: false }
        ];
        form = new forms.Form();
        form.init(formPanel, fieldsDefine);
        return !!form.getFields()
            && form.getFields().length == 1
            && form.getFields()[0].name == "field1"
            && form.getFields()[0] == form.getField("field1")
    });
    test("Form sync & getData", function () {
        formPanel.getElementsByTagName("input")[0].value = "abc";
        var data1 = form.getData();
        form.sync();
        var data2 = form.getData();
        return !data1.field1
            && data2.field1 == "abc"
    });
    test("Form setData", function () {
        var data = {
            field1: "cde"
        };
        form.setData(data);
        return formPanel.getElementsByTagName("input")[0].value == "cde"
    });
    test("Form validate", function () {
        return form.validate()
    });
    test("Form registValidate", function () {
        form.registValidate(function (data) {
            return false;
        });
        return form.validate() === false;
    });
    test("Form notifyError & clearError", function () {
        form.notifyError("field1", "名称不能重复");
        return formPanel.getElementsByTagName("input")[0].className == "field_error"
            && (form.clearError(), formPanel.getElementsByTagName("input")[0].className == "")
    });
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
