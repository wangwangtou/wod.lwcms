/* 字段测试 */
(function (wod, forms) {

    wod.CLS.getClass({
        start: function (type, setting, val1, val2, uiTip, callback, autoConfirm) {
            var field;
            var form;
            var formPanel;
            var context = this;
            test(type + "字段验证", function (msgWrite) {

                var formBody = (uiTip ? encodeHTML(uiTip) : "") + "<input type='text' name='field1'>";
                formPanel = document.createElement("div");
                formPanel.innerHTML = formBody;
                wod.statics.mixin(setting, { name: "field1", type: type });
                form = new forms.Form();
                form.init(formPanel, [setting]);

                field = form.getField("field1");
                msgWrite("字段类型：" + field.constructor._typename);

                field.setValue(val1);
                return !!field;
            });

            testWithUI("UI测试", formPanel, function (msgWrite) {
                if (callback) {
                    return callback.call(field, msgWrite);
                }
                return true;
            }, autoConfirm);
        }
    }, "wtest.fieldTest");

    var wt = new wtest.fieldTest();
    wt.start("FieldBase", {}, 10, 15, "显示的是10，请输入15", function (msgWrite) {
        return this.getValue() == "15";
    });
    wt.start("FieldBase", {}, 25, 25, "显示的是25，请输入25", function (msgWrite) {
        return this.getValue() == "25";
    });
    wt.start("wodtext", {
        format: "",//,
        align: "left",
        datatype: "string",
        length: { max: 100, min: 0 },
        regex: ""
    }, "abc", "25", "显示的是abc，请输入25", function (msgWrite) {
        return this.getValue() === "25";
    });
    //数字格式之前理解有误，#理解为站位符号，其实应为0，后期再调整（其实number类型为#0.00）
    wt.start("wodtext", {
        format: "#",//,
        align: "left",
        datatype: "int",
        length: { max: 100, min: 0 },
        regex: ""
    }, 21.1, 25.0, "显示的是21，请输入25", function (msgWrite) {
        return this.getValue() === 25.0;
    });
    wt.start("wodtext", {
        format: "#.##",//,
        align: "right",
        datatype: "number",
        length: { max: 100, min: 0 },
        regex: ""
    }, 21.1, 25.0, "显示的是21.10，且在右边，请输入25", function (msgWrite) {
        return this.getValue() === 25.0;
    });
    wt.start("wodtext", {
        format: "&#.##",//,
        align: "left",
        datatype: "number",
        length: { max: 100, min: 0 },
        regex: ""
    }, 210000.1, 25.0, "显示的是210,000.10，请输入25", function (msgWrite) {
        return this.getValue() === 25.0;
    });
    wt.start("wodtext", {
        format: "yyyy/MM/dd",//,
        align: "left",
        datatype: "datetime",
        length: { max: 100, min: 0 },
        regex: ""
    }, new Date("2014-11-05"), 25.0, "显示的是2014/11/05，请输入20151105", function (msgWrite) {
        return this.getValue().getTime() === new Date("2015-11-05 00:00:00").getTime();
    });
    wt.start("wodtext", {
        format: "yyyy-MM-d",//,
        align: "left",
        autoSync: false,
        datatype: "datetime",
        length: { max: 100, min: 0 },
        regex: ""
    }, new Date("2014-11-05"), 25.0, "显示的是2014-11-5", null);

    //此类格式(yyMMd)目前不能通过getvalue获取,date格式只能是 yyyy/MM/dd / yyyyMMdd / yyyy-MM-dd中的一种
    wt.start("wodtext", {
        format: "yyMMd",//,
        align: "left",
        autoSync:false,
        datatype: "datetime",
        length: { max: 100, min: 0 },
        regex: ""
    }, new Date("2014-11-05"), 25.0, "显示的是14115", null);
    wt.start("wodtext", {
        format: "yyyy/MM/dd",//,
        align: "left",
        datatype: "datetime",
        length: { max: 100, min: 0 },
        regex: ""
    }, null, "", "显示的是空，返回是null", function (msgWrite) {
        return this.getValue() == null;
    }, true);

    wt.start("wodtext", {
        format: "yyyy/MM/dd",//,
        align: "left",
        datatype: "datetime",
        length: { max: 100, min: 0 },
        regex: ""
    }, new Date("2014-11-05"), 25.0, "显示的是2014/11/05，请输入20151105,autoSync", function (msgWrite) {
        return this._value.getTime() === new Date("2015-11-05 00:00:00").getTime();
    });


    wt.start("wodcommondrop", {
        allownull: false,
        options: [{ name: "aaa", value: "1" }, { name: "ddd", value: "2" }, { name: "ccc", value: "3" }]
    }, "2", "", "显示的是aaa/ddd/ccc，选中的是ddd；请选择ccc", function (msgWrite) {
        return this.getValue() == "3";
    });
    wt.start("wodcommondrop", {
        allownull: true,
        options: [{ name: "aaa", value: "1" }, { name: "ddd", value: "2" }, { name: "ccc", value: "3" }]
    }, "1", "", "显示的是aaa/ddd/ccc，选中的是aaa；请选择空", function (msgWrite) {
        return this.getValue() == null;
    });
})(wod,wod.forms);
