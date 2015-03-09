(function (wod) {
    var formNS = wod.CLS.getNS("wod.forms");

    var formaterFactory = {
        dateTimeFormater: {
            adaptTo: function (fmt) {
                return fmt &&
                        (fmt.indexOf("yy") >= 0
                    || fmt.indexOf("MM") >= 0
                    || fmt.indexOf("dd") >= 0
                    || fmt.indexOf("hh") >= 0
                    || fmt.indexOf("HH") >= 0
                    || fmt.indexOf("mm") >= 0
                    || fmt.indexOf("ss") >= 0);
            },
            __parseDate: function (dateInput) {
                if ((typeof (dateInput)).toLowerCase() == "string") {
                    var isoExp = [/^\s*(\d{4})-(\d\d)-(\d\d)\s*$/,
                        /^\s*(\d{4})\/(\d\d)\/(\d\d)\s*$/,
                        /^\s*(\d{4})(\d\d)(\d\d)\s*$/],
                        date = new Date(NaN), month;
                    var parts;
                    for (var i = 0,length=isoExp.length; i < length; i++) {
                        parts = isoExp[i].exec(dateInput);
                        if(parts)
                            break;
                    }
                    if (parts) {
                        month = +parts[2];
                        date.setFullYear(parts[1], month - 1, parts[3]);
                        if (month != date.getMonth() + 1) {
                            date.setTime(NaN);
                        }
                    }
                    return date;
                }
                else if ((typeof (dateInput)).toLowerCase() == "number") {
                    return new Date(dateInput);
                }
                return new Date(dateInput);
            },

            __lang: {
                en: {
                    getYear: function (y) { return y; },
                    getMonth: function (m, f) { return (f ? (m < 9 ? "0" : "") : "") + (m + 1).toString(); },
                    getDate: function (d, f) { return (f ? (d < 10 ? "0" : "") : "") + (d).toString(); },
                    getHour: function (h, f, r) {
                        if (r) {
                            var m = h < 12 ? this.am : this.pm;
                            h = h > 12 ? h % 12 : h;
                            return (f ? (h < 10 ? "0" : "") : "") + (h).toString();
                        }
                        else {
                            return (f ? (h < 10 ? "0" : "") : "") + (h).toString();
                        }
                    },
                    getMinute: function (m, f) { return (f ? (m < 10 ? "0" : "") : "") + (m).toString(); },
                    getSecond: function (s, f) { return (f ? (s < 10 ? "0" : "") : "") + (s).toString(); },
                    am: "AM",
                    pm: "PM"
                },
                zh: {
                    zhchars: ["〇", "一", "二", "三", "四", "五", "六", "七", "八", "九", "", "", "", ""],
                    getYear: function (y) {
                        var ystr = "";
                        while (y > 0) {
                            ystr = this.zhchars[y % 10] + ystr;
                            y = (y - y % 10) / 10;
                        }
                        return y;
                    },
                    zhmonths: ["一", "二", "三", "四", "五", "六", "七", "八", "九", "十", "十一", "十二"],
                    getMonth: function (m, f) {
                        return this.zhmonths[m];
                    },
                    getDate: function (d, f) { return (f ? (d < 10 ? "0" : "") : "") + (d).toString(); },
                    getHour: function (h, f, r) {
                        if (r) {
                            var m = h < 12 ? this.am : this.pm;
                            h = h > 12 ? h % 12 : h;
                            return (f ? (h < 10 ? "0" : "") : "") + (h).toString();
                        }
                        else {
                            return f ? (h < 10 ? "0" : "") : "" + (h).toString();
                        }
                    },
                    getMinute: function (m, f) { return (f ? (m < 10 ? "0" : "") : "") + (m).toString(); },
                    getSecond: function (s, f) { return (f ? (s < 10 ? "0" : "") : "") + (s).toString(); },
                    am: "上午",
                    pm: "下午"
                }
            },
            instance: function (fmt) {
                var par = this.__parseDate;
                return {
                    /*
                    "(zh)yyyy年MM月dd日" 中文日期
                    "yyyy-MM-dd" 英文日期
                    "(zh)hh:mm:ss" 中文时间
                    "hh:mm:ss" 英文时间 */
                    lang: fmt[0] == "(" ? this.__lang[fmt.substring(1, 3)] : this.__lang["en"],
                    _fmt: fmt[0] == "(" ? fmt.substring(4) : fmt,
                    format: function (val) {
                        var date, str;
                        if (val && !isNaN((date = this.convert(val)).getTime())) {
                            str = this._fmt;
                            var o = [
                                { k: "yyyy", v: this.lang.getYear(date.getFullYear()) },
                                { k: "yy", v: this.lang.getYear(date.getYear()) },
                                { k: "MM", v: this.lang.getMonth(date.getMonth(), true) },
                                { k: "M", v: this.lang.getMonth(date.getMonth()) },
                                { k: "dd", v: this.lang.getDate(date.getDate(), true) },
                                { k: "d", v: this.lang.getDate(date.getDate()) },
                                { k: "HH", v: this.lang.getHour(date.getHours(), true, true) },
                                { k: "H", v: this.lang.getHour(date.getHours(), false, true) },
                                { k: "hh", v: this.lang.getHour(date.getHours(), true, false) },
                                { k: "h", v: this.lang.getHour(date.getHours(), false, false) },
                                { k: "mm", v: this.lang.getMinute(date.getMinutes(), true) },
                                { k: "m", v: this.lang.getMinute(date.getMinutes(), false) },
                                { k: "ss", v: this.lang.getSecond(date.getSeconds(), true) },
                                { k: "s", v: this.lang.getSecond(date.getSeconds(), false) }
                            ];
                            for (var i = 0; i < o.length; i++) {
                                if (new RegExp("(" + o[i].k + ")").test(str)) {
                                    str = str.replace(RegExp.$1, (o[i].v));
                                }
                            }
                        }
                        else {
                            str = val;
                        }
                        return str;
                    },
                    convert:function (val) {
                        return par(val);
                    }
                };
            }
        },
        numberFormater: {
            adaptTo: function (fmt) {
                return fmt &&
                        (fmt.indexOf("#") >= 0);
            },
            __parseNumber: function (numberInput) {
                if (typeof (numberInput) == "number")
                    return numberInput;
                return parseFloat(numberInput);
            },
            instance: function (fmt) {
                var parse = this.__parseNumber;
                return {
                    /*            
                    "#.##" 数值 
                    "&#.##" 数值  &千分位分隔符
                    "#.##%" 百分比 num*100 %  
                    "$&#.##" 货币（美刀）
                    "￥&#.##" 货币（人民币）
                    "$t&#.##" 会计货币（美刀） $与.都是对齐
                    "￥t&#.##" 会计货币（人民币）*/
                    fmt: fmt.substring(fmt.indexOf("#"), fmt.lastIndexOf("#") + 1),
                    comma: fmt.indexOf('&') >= 0,
                    percent: fmt.indexOf('%') >= 0,
                    money: fmt[0] == '$' || fmt[0] == '￥',
                    moneypix: (fmt[0] == '$' || fmt[0] == '￥') ? fmt[0] : "",
                    align: fmt.indexOf('t') >= 0,
                    format: function (val) {
                        var num = parse(val);
                        var str;
                        if (typeof (num) == "number" && !isNaN(num)) {
                            var dc = 0;
                            if (this.fmt.indexOf(".") >= 0) {
                                dc = this.fmt.split(".").pop().length;
                            }
                            if (this.percent) {
                                num = num * 100;
                            }
                            str = num.toFixed(dc);
                            if (this.comma) {
                                var l = str.indexOf('.');
                                if (l < 0)
                                    l = str.length;
                                if (l > 3) {
                                    var parts = [];
                                    parts.push(str.substring(l - 3));
                                    l -= 3;
                                    for (; l > 0; l -= 3) {
                                        parts.push(str.substring(Math.max(0, l - 3), l));
                                    }
                                    str = parts.reverse().join(",");
                                }
                            }
                            if (this.percent) {
                                str = str + " %";
                            }
                            if (this.money) {
                                if (this.align) {
                                    str = this.moneypix + "\t\t" + str;
                                }
                                else {
                                    str = this.moneypix + " " + str;
                                }
                            }
                        } else {
                            str = val;
                        }
                        return str;
                    },
                    convert: function (val) {
                        if (this.money) {
                            val = val.replace(this.moneypix,"");
                            val = val.replace(new RegExp("/\\t/g"), "");
                            val = val.replace(/ /g, "");
                        }
                        if (this.percent) {
                            val = val.replace("%", "");
                        }
                        if (this.comma) {
                            val = val.replace(/,/g, "");
                        }
                        val = parse(val);
                        if (this.percent) {
                            val = val / 100;
                        }
                        return val;
                    }
                };
            }
        },
        commonFormater: {
            adaptTo: function (fmt) {
                return fmt === "t";
            },
            instance: function (fmt) {
                return {
                    _fmt: fmt,
                    format: function (val) {
                        return val;
                    },
                    convert: function (val) {
                        return val;
                    }
                };
            }
        },
        create: function (format) {
            if (format && typeof (format.format) == "function")
                return format;
            if (format && typeof (format) == "function")
                return { format: format };
            for (var f in this) {
                if (this[f].adaptTo && this[f].adaptTo(format)) {
                    return this[f].instance(format);
                }
            }
            return this.commonFormater.instance(format);
        }
    };

    wod.CLS.getClass({
        formater:null,
        setting: {
            autoSync:true,
            format: "",//,
            align: "left",
            datatype: "string",
            length: { max: 255, min: 0 },
            regex: ""
        },
        defaultFormat:function (datatype) {
            switch (datatype) {
                case "string":
                    return "";
                case "int":
                    return "#";
                case "money":
                    return "&#.#";
                case "number":
                    return "#.#";
                case "date":
                    return "yyyy-MM-dd";
            }
            return "";
        },
        init: function (form, field, setting) {
            this.formater = formaterFactory.create(setting.format || this.setting.format ||
                this.defaultFormat(setting.datatype || this.setting.datatype));
            this.parent(form, field, setting);
            this.update();
        },
        isEquals:function (val1,val2) {
            if (this.setting.datatype == "date") {
                return !(val1 && !val2)
                    || !(val2 && !val1)
                    || (!val1 && !val2)
                    || val1.valueOf() == val2.valueOf();
            }
            else {
                return this.parent(val1, val2);
            }
        },
        formatValue: function (val) {
            return this.formater.format(val);
        },
        _syncValue: function () {
            var val = this._field.getValue();
            if (!val) {
                return null;
            }
            if (this.setting.datatype == "string") {
            }
            else {
                val = this.formater.convert(val);
            }
            return val;
        },
        render: function () {
            if (this.setting.datatype != "string") {
                this._field.css({ "ime-mode": "disabled" });
            }
            if (this.setting.align != "left") {
                this._field.css({ "text-align": this.setting.align });
            }
            if (this.setting.length && this.setting.length.max) {
                this._field.attr({ "maxlength": this.setting.length.max });
            }
        },
        update: function () {
            this._field.setValue(this.formatValue(this._value));
        },
        registAutoSync:function () {
            this._field.on("keyup blur", this.sync.bind(this));
            this._field.on("blur", this.update.bind(this));
        },
        unRegistAutoSync: function () {
            this._field.off("keyup blur");
        }
    }, "wod.forms.wodtext", "wod.forms.FieldBase");
})(wod);


(function (wod,$) {
    var formNS = wod.CLS.getNS("wod.forms");

    wod.CLS.getClass({
        setting: {
            autoSync: true,
            allownull: true,
            options: []
        },
        _renderring:false,
        _asyncValue: "",
        _hasAsyncValue:false,
        _swap_body: "<select size=1></select>",
        _emptyText: "-请选择-",
        _emptyVal:"-1",
        render: function () {
            this._field.swap(this._swap_body);
            this._renderring = true;
            this.getSource(this.renderDrop.bind(this));
        },
        renderDrop:function (options) {
            this.setting.options = options;
            var html = [];
            if (this.setting.allownull) {
                html.push("<option value=\"" + encodeHTML(this._emptyVal) + "\">" + encodeHTML(this._emptyText) + "</option>");
            }
            for (var i = 0, length = options.length; i < length; i++) {
                html.push("<option value=\"" + encodeHTML(options[i].value) + "\">" + encodeHTML(options[i].name) + "</option>");
            }
            this._field.innerHTML(html.join(""));
            this._renderring = false;
            this.update();
        },
        getSource:function (callback) {
            callback(this.setting.options);
        },
        _syncValue: function () {
            var val = this._field.getValue();
            if (val === this._emptyVal) {
                val = null;
            }
            return val;
        },
        _innerSetValue: function (val) {
            if (this._renderring) {
                this._asyncValue = val;
                this._hasAsyncValue = true;
            }
            else {
                this._hasAsyncValue = false;
                return this.parent(val);
            }
        },
        _correctValue: function () {
            function isInArray(val, array) {
                for (var i = 0,length= array.length; i < length; i++) {
                    if (array[i].value == val) {
                        return true;
                    }
                }
                return false;
            }
            if (this._hasAsyncValue) {
                if (isInArray(this._asyncValue, this.setting.options)) {
                    this._value = this._asyncValue;
                }
            }
            if (!isInArray(this._value, this.setting.options)) {
                if (this.setting.allownull || this.setting.options.length == 0) {
                    this._value = null;
                }
                else {
                    this._value = this.setting.options[0].value;
                }
            }
        },
        update: function () {
            this._correctValue();
            if (this._value) {
                this._field.setValue(this._value);
            }
            else {
                this._field.setValue(this._emptyVal);
            }
        },
        registAutoSync: function () {
            this._field.on("click", this.sync.bind(this));
        },
        unRegistAutoSync: function () {
            this._field.off("click");
        }
    }, "wod.forms.wodcommondrop", "wod.forms.FieldBase");
})(wod);

/*drop*/
(function (wod,$) {
    /*到时候提取出放到外面*/
    wod.CLS.getClass({
        setting: {
            autoSync: true,
            allownull: false,
            options: [],
            optionscmd: "op_category",
            optionscmddata: null
        },
        getSource: function (callback) {
            if (this.setting.options.length) {
                this.parent(callback);
            }
            else {
                $.ajax({
                    url: "common.ashx?command=" + this.setting.optionscmd,
                    data: this.setting.optionscmddata || {},
                    type: "post",
                    dataType: "json",
                    success: (function (data) {
                        //if (_$wod_form.ajaxValidResponse(data)) {
                        if (data && data.result && data.result.length) {
                            this.setting.options = data.result;
                        }
                        callback(this.setting.options);
                        //}
                    }).bind(this),
                    error: function () {
                        callback(this.setting.options);
                    }//_$wod_form.ajaxError
                });

            }
        }
    }, "wod.forms.woddrop", "wod.forms.wodcommondrop");
})(wod, jQuery);
/*wodrichtext,siteImage*/
(function (wod, $, K) {
    wod.CLS.getClass({
        setting:{
            cssPath: 'editor/plugins/code/prettify.css',
            uploadJson: 'editor/upload_json.ashx',
            fileManagerJson: 'editor/file_manager_json.ashx',
            allowFileManager: true,
            autoSync: false,
            richtype: ""//simple
        },
        _editor:null,
        render: function () {
            var editorSetting = wod.statics.mixin({},this.setting);
            if (this.setting.richtype == "simple") {
                editorSetting.items = [
                    'source', '|', 'fontname', 'fontsize', '|', 'forecolor', 'hilitecolor', 'bold', '|',
                    'italic', 'underline', 'strikethrough', 'lineheight',
                    'link', 'unlink', '|', 'about'];
                editorSetting.minWidth = "200px";
                editorSetting.resizeType = 0;
            }
            this._editor = K.create(this._field.dom, editorSetting);
        },
        sync: function () {
            this._editor.sync();
            this.parent();
        }
    }, "wod.forms.wodrichtext", "wod.forms.FieldBase");
    wod.CLS.getClass({
        setting: {
            autoSync:true,
            allowFileManager: true,
            uploadJson: 'editor/upload_json.ashx',
            fileManagerJson: 'editor/file_manager_json.ashx',
            width: 180,
            height: 120,
            description: "小图片(建议尺寸180*120)"
        },
        _editor: null,
        render: function () {
            this._editor = K.editor(this.setting);
            this._field.attr({"readonly":true});
            this._field.on("click focus", this._showDialog.bind(this));
        },
        _showDialog:function () {
            this._editor.loadPlugin('image',this._imagePluginLoaded.bind(this));
        },
        _imagePluginLoaded: function () {
            this._editor.plugin.imageDialog({
                imageUrl: this.getValue(),
                imageHeight: this.setting.height,
                imageWidth: this.setting.width,
                imageTitle: this.setting.description,
                clickFn: this._selectImage.bind(this)
            });
        },
        _selectImage: function (url, title, width, height, border, align) {
            if (this.setting.autoSync) {
                this.sync();
            }
            this._field.setValue(url);
            this._editor.hideDialog();
        },
        registAutoSync: function () {
        },
        unRegistAutoSync: function () {
        }
    }, "wod.forms.wodimage", "wod.forms.FieldBase");

    wod.CLS.getClass({
        setting: {
            autoSync:false,
            datatype: "json"//"object","json"
        },
        _objValue: null,
        _canSync:false,
        _setJsonValue:function(jsonVal){
            try {
                this._objValue = $.parseJSON(jsonVal);
                if (!this._objValue) {
                    this._objValue = wod.statics.mixin({},this.setting.defaultObjValue);
                }
            } catch (e) {
                this._objValue = wod.statics.mixin({}, this.setting.defaultObjValue);
            }
            this.update();
            if (this.setting.datatype == "object") {
                this._value = this._objValue;
            }
            else{
                this._value = jsonVal;
            }
        },
        _innerSetValue:function(val){
            if(this.setting.datatype == "object"){
                if( typeof(val) == "object"){
                    this.parent(val);
                    this._objValue = val;
                    this.update();
                }
                else{
                    this._setJsonValue(val);
                }
            }
            else{
                if( typeof(val) == "object"){
                    this._objValue = val;
                    this.parent($.toJSON(this._objValue));
                    this.update();
                }
                else{
                    this._setJsonValue(val);
                }
            }
        },
        _innerGetObject:function(){
            return this._objValue;
        },
        _syncValue: function () {
            if(this._canSync){
                this._objValue = this._innerGetObject();
                if (this.setting.datatype == "object") {
                    return this._objValue;
                }
                else {
                    return $.toJSON(this._objValue);
                }
            }
            else{
                return this._value;
            }
        }
    },"wod.forms.ObjectField", "wod.forms.FieldBase");

    wod.CLS.getClass({
        setting: {
            autoSync:false,
            datatype: "json"//"object","json"
        },
        _form: null,
        _objValue: null,
        render: function () {
            var fields = this.initFields();
            var formBody = this.renderFormBody(fields);
            this._form = new wod.forms.Form();
            this._form.init(formBody, fields);
            this._field.swap(formBody);
            this._canSync = true;
            this._setJsonValue(this._value);
        },
        update:function (){
            this._form.setData(this._objValue);
        },
        initFields: function () {
            return [];
        },
        renderFormBody:function (fields) {
            return document.createElement("div");
        },
        _innerGetObject:function(){
            this._form.sync();
            return this._form.getData();
        }
    }, "wod.forms.wodformfield", "wod.forms.ObjectField");

    wod.CLS.getClass({
        setting: {
            datatype: "json",//"object","json"
            small: {
                allowFileManager: true,
                uploadJson: 'editor/upload_json.ashx',
                fileManagerJson: 'editor/file_manager_json.ashx',
                width: 180,
                height: 120,
                description: "小图片(建议尺寸180*120)"
            },
            normal: {
                allowFileManager: true,
                uploadJson: 'editor/upload_json.ashx',
                fileManagerJson: 'editor/file_manager_json.ashx',
                width: 500,
                height: 200,
                description: "普通图片(建议尺寸500*200)"
            },
            big: {
                allowFileManager: true,
                uploadJson: 'editor/upload_json.ashx',
                fileManagerJson: 'editor/file_manager_json.ashx',
                width: 960,
                height: 350,
                description: "大图片(建议尺寸960*350)"
            }
        },
        render: function () {
            this.parent();
        },
        initFields: function () {
            var type = "wodimage";
            var fields = [{ name: "smallImg", type: type },{ name: "normalImg", type: type },{ name: "bigImg", type: type }];
            wod.statics.mixin(fields[0], this.setting.small);
            wod.statics.mixin(fields[1], this.setting.normal);
            wod.statics.mixin(fields[2], this.setting.big);
            return fields;
        },
        renderFormBody: function (fields) {
            var formPanel = document.createElement("div");
            var formBody = [];
            for (var i = 0, length = fields.length; i < length; i++) {
                formBody.push("<label>" + encodeHTML(fields[i].description) + "</label><br>");
                formBody.push("<input type=\"text\" name=\"" + encodeHTML(fields[i].name) + "\">");
            }
            formPanel.innerHTML = formBody.join("");
            return formPanel;
        }
    }, "wod.forms.wodsiteImage", "wod.forms.wodformfield");
})(wod, jQuery, KindEditor);

/* wodform submit*/
(function (wod,$) {
    wod.CLS.getClass({
        submit:function (url,callback){
            if(this.validate()){
                $.ajax({
                    data:this.getData(),
                    url:url,
                    type:"post",
                    success:function(data){
                        if (data && data.status) {
                            callback(data);
                        }
                        else{
                            alert(data.message);
                        }
                    }
                });
            }
        }
    },"wod.forms.wodform","wod.forms.Form");
})(wod,jQuery);

/* wodtreesubform */
(function (wod,$) {
    wod.CLS.getClass({
        setting:{
            edittype: 'node',//'none','content','node'
            formname: "subform", 
            formtitle: "子表单", 
            titleprop: "name", 
            childrenprop: "children",
            validate: null,
            fields: []
        },
        body:null,
        _lineActionHTML:"",
        _tidFeed:0,
        _cache:{},
        render:function(){
            var html = [];
            html.push("<div class='f-tree-wrap'>");
            if (this.setting.edittype == "node") {
                html.push("<div class='tcmd'><a href='javascript:;' class='tadd' data-action='add'>添加</a><a href='javascript:;' class='tadds' data-action='addsub'>添加子节点</a><a href='javascript:;' class='tup' data-action='moveup'>上移</a><a href='javascript:;' class='tdown' data-action='movedown'>下移</a></div>"); 
                //<a href='javascript:;' class='tlvlup' data-action='tlvlup'>升级</a><a href='javascript:;' class='tlvldown' data-action='tlvldown'>降级</a>

                this._lineActionHTML = "<a href='javascript:;' class='del'  data-action='remove'>删除</a><a href='javascript:;' class='edit' data-action='edit'>编辑</a>";
            }
            html.push("<ul class='f-tree  f-root'>");
            html.push("</ul>");
            html.push("</div>");
            this.body = $(html.join(""));
            this.root = this.body.find(".f-root");
            this._field.swap(this.body);

            var context = this;
            this.body.on("click", "[data-action]",function () {
                var tid = $(this).parents(".tnode:eq(0)").attr("data-tid");
                if(!tid){
                    tid = context.body.find(".curr").attr("data-tid");
                }
                var data = context._cache[tid];
                var node = data ?data.node:null;
                var pnode = data ?context._cache[data.ptid]:null;
                var callback = context["action_"+ $(this).attr("data-action")].bind(context,node,pnode);
                callback();
                return false;
            });
            this.body.on("click", ".tnode",function () {
                context.body.find(".curr").removeClass("curr");
                $(this).addClass("curr");
                return false;
            });
            
            this._canSync = true;
            this._setJsonValue(this._value);
        },
        update:function (){
            this.root.empty();
            this.root.html(this._getTreeNodesHTML(this._objValue));
        },
        _getTreeNodesHTML:function(nodes,ptid){
            var html = [];
            for (var i = 0,length = nodes && nodes.length; i < length; i++) {
                var item = nodes[i];
                var tid = this._getNewTid();
                this._cache[tid] = {node:item,ptid:ptid};
                var child = this._getNodeChild(item);
                var text = encodeHTML( this._getNodeText(item));
                html.push("<li class='tnode' data-tid='" + tid + "'>"
                    +"<span class='tit'>" + text + "</span>"
                    +"<span class='tncmd'>" + this._lineActionHTML + "</span>" 
                    + (child && child.length ? "<ul class='f-tree'>" + this._getTreeNodesHTML(child,tid) +"</ul>" : "") 
                    + "</li>");
            }
            return html.join("");
        },
        _getNewTid:function(){
            return 'tn' + (this._tidFeed++).toString()
        },
        _getNodeChild:function(node){
            return node[this.setting.childrenprop];
        },
        _getOrCreateNodeChild:function(node){
            var child = node[this.setting.childrenprop];
            if(!child){
                child = [];
                node[this.setting.childrenprop] = child;
            }
            return child;
        },
        _getNodeText:function(node){
            return node[this.setting.titleprop];
        },
        _innerGetObject:function(){
            return this._objValue;
        },
        _showForm:function(data,callback){
            var form = new wod.forms.wodform();
            var fields = this.setting.fields;
            var validate = this.setting.validate;
            $.ui.getForm(this.setting.formtitle || "", this.setting.formname
                , function (frm) {
                    form.init(frm[0],fields);
                    form.registValidate(validate);
                    form.setData(data);
                }
                , function (frm) {
                    if(frm){
                        callback(form.getData());
                    }
                }, function (frm, errorCallback) {
                    return form.validate();
                });
        },
        action_add:function(node,pnode){
            var arr = pnode ? this._getOrCreateNodeChild(pnode) : this._objValue;
            var context = this;
            this._showForm({},function (newNode) {
                if(!newNode)return;
                if(!node){
                    arr.push(newNode);
                }
                else{
                    for (var i = 0,length=arr.length; i < length; i++) {
                        if(node == arr[i]){
                            arr.splice(i,0,newNode);
                            break;
                        }
                    }
                }
                context.update();
            });
        },
        action_addsub:function(node,pnode){
            var arr = node ? this._getOrCreateNodeChild(node) : this._objValue;
            var context = this;
            this._showForm({},function (newNode) {
                if(!newNode)return;
                arr.push(newNode);
                context.update();
            });
        },
        action_moveup:function(node,pnode){
            if(node){
                var arr = pnode ? this._getOrCreateNodeChild(pnode) : this._objValue;
                for (var i = 0,length=arr.length; i < length; i++) {
                    if(node == arr[i]){
                        arr.splice(i,1);
                        arr.splice(Math.max(0,i-1),0,node);
                        break;
                    }
                }
            }
        },
        action_movedown:function(node,pnode){
            if(node){
                var arr = pnode ? this._getOrCreateNodeChild(pnode) : this._objValue;
                for (var i = 0,length=arr.length; i < length; i++) {
                    if(node == arr[i]){
                        arr.splice(i,1);
                        arr.splice(Math.min(length,i+1),0,node);
                        break;
                    }
                }
                this.update();
            }
        },
        action_remove:function(node,pnode){
            if(node){
                var arr = pnode ? this._getOrCreateNodeChild(pnode) : this._objValue;
                for (var i = 0,length=arr.length; i < length; i++) {
                    if(node == arr[i]){
                        arr.splice(i,1);
                        break;
                    }
                }
                this.update();
            }
        },
        action_edit:function(node,pnode){
            if(node){
                var context = this;
                this._showForm(node,function (newNode) {
                    wod.statics.mixin(node,newNode);
                    context.update();
                });
            }
        }
    }, "wod.forms.wodtreesubform", "wod.forms.ObjectField");

    
    wod.CLS.getClass({
        setting: {
            datatype: "json",//"object","json"
            names: { },
            formname: "naviform", formtitle: "导航", titleprop: "name", childrenprop: "subNavis",
            validate: null,
            fields: null
        },
        render: function () {
            this.parent();
        },
        initFields: function () {
            var type = "wodtreesubform";
            var fields = [];
            for (var key in this.setting.names) {
                fields.push({name:key,type:type,description:this.setting.names[key]});
            }
            var setting = {
                formname: this.setting.formname, 
                formtitle: this.setting.formtitle, 
                titleprop: this.setting.titleprop, 
                childrenprop: this.setting.childrenprop,
                validate: this.setting.validate,
                fields: this.setting.fields,
                datatype: "object"
            }
            wod.statics.mixin(fields[0], setting);
            wod.statics.mixin(fields[1], setting);
            wod.statics.mixin(fields[2], setting);
            return fields;
        },
        renderFormBody: function (fields) {
            var formPanel = document.createElement("div");
            formPanel.className="f-multitree";
            var formBody = [];
            for (var i = 0, length = fields.length; i < length; i++) {
                formBody.push("<label class='treename'>" + encodeHTML(fields[i].description) + "</label><br>");
                formBody.push("<input type=\"text\" name=\"" + encodeHTML(fields[i].name) + "\">");
            }
            formPanel.innerHTML = formBody.join("");
            return formPanel;
        }
    }, "wod.forms.wodnavitree", "wod.forms.wodformfield");
})(wod,jQuery);