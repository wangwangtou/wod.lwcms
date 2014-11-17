(function (window) {
    /* 开始：基础函数，对js的扩展及兼容性适配 */
    if (!Function.prototype.bind) {
        function convet2Array(arg) {
            var args = [];
            for (var i = 0, length = arg.length; i < length; i++) {
                args.push(arg[i]);
            }
            return args;
        }
        Function.prototype.bind = function (thisArgs) {
            var context = this;
            var args = convet2Array(arguments).slice(1, arguments.length);
            return function () {
                return context.apply(thisArgs, args.concat(convet2Array(arguments).slice(args.length, arguments.length)));
            };
        };
    }
    if (!window.console) {
        window.console = { log: function () { } };
    }
    encodeHTML = function (text) {
        var d = document.createElement("div");
        d.appendChild(document.createTextNode(text));
        var html = d.innerHTML;
        return html.replace(/"/g, "&quot;");
    };
    decodeHTML = function (html) {
        var d = document.createElement("div");
        d.innerHTML = html;
        html = d.innerText;
        return html;
    };
    /* 结束：基础函数，对js的扩展及兼容性适配 */

    /* 开始：实现名称空间和类的注册 */
    var _clone = function (obj) {
        var objClone;
        if (typeof (obj) == "object") {
            objClone = new obj.constructor();
        } else {
            objClone = new obj.constructor(obj.valueOf());
        }
        for (var key in obj) {
            if (objClone[key] != obj[key]) {
                if (typeof (obj[key]) == 'object') {
                    objClone[key] = _clone(obj[key]);
                } else {
                    objClone[key] = obj[key];
                }
            }
        }
        objClone.toString = obj.toString;
        objClone.valueOf = obj.valueOf;
        return objClone;
    };
    var _mixin = (function () {
        var extraNames, extraLen, empty = {};
        for (var i in { toString: 1 }) { extraNames = []; break; }
        extraNames = extraNames || ["hasOwnProperty", "valueOf", "isPrototypeOf",
            "propertyIsEnumerable", "toLocaleString", "toString", "constructor"];
        extraLen = extraNames.length;
        return function (target, source) {
            var name, s, i;
            for (name in source) {
                s = source[name];
                if (!(name in target) || (target[name] !== s && (!(name in empty) || empty[name] !== s))) {
                    target[name] = s;
                }
            }
            if (extraLen && source) {
                for (i = 0; i < extraLen; ++i) {
                    name = extraNames[i];
                    s = source[name];
                    if (!(name in target) || (target[name] !== s && (!(name in empty) || empty[name] !== s))) {
                        target[name] = s;
                    }
                }
            }
            return target;
        }
    })();

    var wod = new Object();
    Function.prototype.mixin = function (base) {
        _mixin(this.prototype, base || this._superpt);
        return this;
    };
    Function.prototype.isSubclassOf = function (base) {
        if (this._super == null)
            return false;
        if (this._super === base)
            return true;
        else {
            return this._super.isSubclassOf(base);
        }
    };
    wod.CLS = function () {
        this.parent = function () {
            var cls = this.constructor;
            var caller = arguments.callee.caller;
            var finded = false;
            var result;
            while (!finded) {
                for (var tmp in cls.prototype) {
                    if (cls.prototype[tmp] == caller) {
                        if (cls._superpt[tmp] && cls._superpt[tmp] != caller) {
                            result = cls._superpt[tmp].apply(this, arguments);
                            finded = true;
                        }
                        break;
                    }
                }
                if (!finded) {
                    cls = cls._super;
                }
            }
            return result;
        };
        //将prototype中的Object类型进行拷贝，避免修改；如果需要，各子类在new之后，使用之前调用
        this._init = function () {
            for (var p in this) {
                if (typeof (this[p]) == "object" && this[p]) {
                    this[p] = _clone(this[p]);
                }
            }
        };
    };
    Function.prototype.isCLS = function () {
        return this.isSubclassOf(wod.CLS);
    }
    _mixin(wod.CLS, {
        _classes : {},
        getRandomName :(function () {
            var i = 0;
            return function () {
                return "__Class" + i++;
            };
        })(),
        getAllTypes: function () {
            return _mixin({}, this._classes);
        },
        getNS: function (nsName) {
            var arr;
            if (!nsName || ((arr = nsName.split(".")) && arr.length == 0))
                return undefined;
            var base = window;
            for (var i = 0, length = arr.length; i < length; i++) {
                if (base[arr[i]]) {
                }
                else {
                    base[arr[i]] = new Object();
                }
                base = base[arr[i]];
            }
            return base;
        },
        //getClass(clsName)
        //getClass(attrs,clsName)
        //getClass(clsName,baseClsName)
        //getClass(newAttrs,clsName,baseClsName)
        getClass: function () {
            var getClassMethod;
            if (arguments.length == 1) {
                getClassMethod = this._getClassCLS;
            }
            else if (arguments.length == 2) {
                if (typeof (arguments[0]) == "string") {
                    getClassMethod = this._getClassBaseTo;
                }
                else {
                    getClassMethod = this._getClassCLSWithAttr;
                }
            }
            else if (arguments.length == 3) {
                getClassMethod = this._getClassWithAttrBaseTo;
            }
            if (getClassMethod)
                return getClassMethod.apply(this, arguments);
            return this;
        },
        getClassNS:function (clsName) {
            var arr;
            if (!clsName || ((arr = clsName.split(".")) && arr.length < 1))
                return undefined;
            arr.pop();
            return this.getNS(arr.join("."));
        },
        _getClassCLS: function (clsName) {
            return this._getClassBaseTo(clsName, this);
        },
        _getClassCLSWithAttr: function (attrs, clsName) {
            var cls = this._getClassCLS(clsName);
            cls.mixin(attrs);
            return cls;
        },
        _getClassBaseTo: function (clsName, baseClsName) {
            var ns = this.getClassNS(clsName);
            if (ns) {
                var name = clsName.split(".").pop();
                if (ns[name])
                    return ns[name];
                else {
                    var baseClass;
                    if (typeof (baseClsName) == "string") {
                        baseClass = this._getClassCLS(baseClsName);
                    }
                    else {
                        baseClass = baseClsName;
                    }
                    var construct = eval("(" + clsName + " = function() {this._init.apply(this,arguments);})");
                    construct._super = baseClass;
                    construct._superpt = new baseClass();
                    construct.mixin();
                    this._classes[clsName] = construct;
                    construct._typename = clsName;
                    ns[name] = construct;
                    return construct;
                }
            }
            return undefined;
        },
        _getClassWithAttrBaseTo: function (newAttrs, clsName, baseClsName) {
            var cls = this._getClassBaseTo(clsName, baseClsName);
            cls.mixin(newAttrs);
            return cls;
        }
    });
    window.wod = wod;

    var statics = wod.CLS.getNS("wod.statics");
    _mixin(statics, {
        mixin: _mixin,
        clone: _clone
    });
    /* 结束：实现名称空间和类的注册 */
})(window);

/*form主体，无jQuery依赖*/
(function (wod) {
    var formNS = wod.CLS.getNS("wod.forms");
    wod.CLS.getClass({
        _components:{},
        notify: function (name, field/*arguments*/) {
            var components = this._components[name];
            if (components) {
                for (var i = 0, length = components.length; i < length; i++) {
                    components[i].notify.apply(components[i], arguments);
                }
            }
        },
        registComponent: function (notifyTypes,component) {
            for (var i = 0, length = notifyTypes.length; i < length; i++) {
                var components = this._components[notifyTypes[i]];
                if (!components) {
                    components = [];
                    this._components[notifyTypes[i]] = components;
                }
                components.push(component);
            }
        },
        unRegistComponent: function (notifyTypes) {
            for (var i = 0, length = notifyTypes.length; i < length; i++) {
                var components = this._components[notifyTypes[i]];
                if (components) {
                    delete this._components[notifyTypes[i]];
                }
            }
        }
    }, "wod.forms.FieldComponents");

    wod.CLS.getClass({
        notify: function (name, field/*arguments*/) {
            if (this[name]) {
                var args = [];
                for (var i = 1, length = arguments.length; i < length; i++) {
                    args.push(arguments[i]);
                }
                this[name].apply(this, args);
            }
        }
    }, "wod.forms.FieldComponentBase");

    /* 开始，字段基类 */
    wod.CLS.getClass({
        form: null,
        name: "",
        setting: { autoSync: true },
        components:null,
        _field: null,
        _value:"",
        _init: function (form, field, setting) {
            this.parent();
            if (arguments.length) {
                this.init(form, field, setting);
            }
        },
        getForm: function () {
            return this.form;
        },
        init: function (form, field, setting) {
            this.form = form;
            this._field = field;
            this.name = setting.name;
            wod.statics.mixin(this.setting, setting);
            this._value = this._syncValue();
            this.render();
            if (this.setting.autoSync) {
                this.registAutoSync();
            }
        },
        isEquals:function (val1,val2) {
            return val1 == val2;
        },
        _innerSetValue: function (val) {
            if (!this.isEquals(this._value, val)) {
                this._value = val;
                //this.raiseEvent("valueChanged");
                return true;
            }
            return false;
        },
        setValue: function (val) {
            if (this._innerSetValue(val)) {
                this.update();
            }
        },
        getValue: function () {
            if (this.setting.autoSync) {
                this.sync();
            }
            return this._value;
        },
        _syncValue: function () {
            var val = this._field.getValue();
            return val;
        },
        sync: function () {
            var val = this._syncValue();
            this._innerSetValue(val);
        },
        registAutoSync: function () {
        },
        unRegistAutoSync: function () {
        },
        notifyError: function (msg) {
            (this.components || formNS.FieldBase.components).notify("notifyError", this, msg);
        },
        clearError: function () {
            (this.components || formNS.FieldBase.components).notify("clearError", this);
        },
        render: function () {
        },
        update: function () {
            this._field.setValue(this._value);
        }
    }, "wod.forms.FieldBase");
    formNS.FieldBase.components = new formNS.FieldComponents();
    /* 结束，字段基类 */

    /* 开始，表单类 */
    wod.CLS.getClass({
        _domMgr:null,
        _getDomManager: function (dom) {
            return new Object();
        },
        _getFieldType: function (type) {
            for (var typeName in formNS) {
                if (typeName == type
                    && formNS[typeName].isSubclassOf(formNS.FieldBase)) {
                    return formNS[typeName];
                }
            }
            return formNS.FieldBase;
        },
        fields: [],
        customValidates:[],
        name: "",
        _init: function (formBody, setting) {
            this.parent();
            if (arguments.length) {
                this.init(formBody, setting);
            }
        },
        init: function (formBody, fieldsDefine) {
            this._domMgr = this._getDomManager(formBody);
            for (var i = 0, length = fieldsDefine.length; i < length; i++) {
                var fieldDef = fieldsDefine[i];
                var fieldType = this._getFieldType(fieldDef.type);
                var field = new fieldType(this,this._domMgr.getField(fieldDef.name),fieldDef);
                this.fields.push(field);
            }
        },
        sync: function () {
            for (var i = 0, length=this.fields.length; i < length; i++) {
                this.fields[i].sync();
            }
        },
        getFields:function () {
            return this.fields;
        },
        getField: function (name) {
            for (var i = 0, length = this.fields.length; i < length; i++) {
                if (name == this.fields[i].name) {
                    return this.fields[i];
                }
            }
        },
        getData: function () {
            var data = {};
            for (var i = 0, length = this.fields.length; i < length; i++) {
                data[this.fields[i].name] = this.fields[i].getValue();
            }
            return data;
        },
        setData: function (data) {
            for (var i = 0, length = this.fields.length; i < length; i++) {
                if (data[this.fields[i].name] != undefined) {
                    this.fields[i].setValue(data[this.fields[i].name]);
                }
            }
        },
        registValidate: function (validator) {
            this.customValidates.push(validator);
        },
        unRegistValidate: function (validator) {
            for (var i = 0,length =this.customValidates.length; i < length; i++) {
                if (this.customValidates[i] == validator) {
                    this.customValidates.splice(i, 1);
                    break;
                }
            }
        },
        validate: function () {
            this.sync();
            var data = this.getData();
            for (var i = 0, length = this.customValidates.length; i < length; i++) {
                if (!this.customValidates[i].call(this, data)) {
                    return false;
                }
            }
            return true;
        },
        notifyError: function (name, msg) {
            var field = this.getField(name);
            field.notifyError(msg);
        },
        clearError: function () {
            for (var i = 0, length = this.fields.length; i < length; i++) {
                this.fields[i].clearError();
            }
        }
    }, "wod.forms.FormBase");
    /* 结束，表单类 */
})(wod);

/*此段必须引用在jquery之后*/
(function (wod, $) {
    var formNS = wod.CLS.getNS("wod.forms");
    /* 开始，dom封装 */
    wod.CLS.getClass({
        dom: null,
        _init: function (dom) {
            this.parent();
            if (arguments.length) {
                this.init(dom);
            }
        },
        init: function (dom) {
            this.dom = dom;
        },
        getValue: function () {
            var val;
            switch (this.dom.tagName.toUpperCase()) {
                case "select":
                    val = $(this.dom).find(":selected").val();
                    break;
                default:
                    val = this.dom.value;
                    break;
            }
            return val;
        },
        setValue: function (val) {
            switch (this.dom.tagName.toUpperCase()) {
                case "select":
                    val = $(this.dom).find("[value=\"" + val + "\"]").attr("selected", true);
                    break;
                default:
                    this.dom.value = val;
                    break;
            }
        },
        on: function (event,callback) {
            $(this.dom).on(event, callback);
        },
        off: function (event) {
            $(this.dom).off(event);
        },
        css: function (data) {
            $(this.dom).css(data);
        },
        attr: function (data) {
            $(this.dom).attr(data);
        },
        swap: function (body) {
            var $sdom = $(body);
            $(this.dom).hide().after($sdom);
            this.dom = $sdom[0];
        },
        innerHTML: function (html) {
            $(this.dom).html(html);
        }
    }, "wod.dom.fields");

    wod.CLS.getClass({
        $: null,
        _init: function (body) {
            this.parent();
            this.$ = wod.dom.jQueryDomManager.$;
            if (arguments.length) {
                this.init(body);
            }
        },
        init: function (body) {
            this.body = body;
        },
        getByName: function (name) {
            return this.$(this.body).find("[name=\"" + encodeHTML(name) + "\"]")[0];
        },
        getField: function (name) {
            return new wod.dom.fields(this.getByName(name));
        }
    }, "wod.dom.jQueryDomManager").$ = jQuery;

    wod.CLS.getClass({
        $: null,
        errorClass: "field_error",
        _init: function () {
            this.parent();
            this.$ = wod.dom.jQueryDomManager.$;
        },
        notifyError: function (field, msg) {
            this.$(field._field.dom).addClass(this.errorClass)
                .attr("title", msg);
        },
        clearError: function (field) {
            this.$(field._field.dom).removeClass(this.errorClass)
                .attr("title", "");
        }
    }, "wod.dom.components.CommonNotifyErrorComponent", "wod.forms.FieldComponentBase")
        .notifyTypes = ["notifyError", "clearError"];
    /* 结束，dom封装 */

    wod.CLS.getClass({
        _getDomManager: function (body) {
            return new wod.dom.jQueryDomManager(body);
        }
    }, "wod.forms.Form", "wod.forms.FormBase");
    formNS.FieldBase.components.registComponent(wod.dom.components.CommonNotifyErrorComponent.notifyTypes,
        new wod.dom.components.CommonNotifyErrorComponent());
})(wod, jQuery);