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
            return this.getNS(arr.slice(1,arr.length).join("."));
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

(function (wod) {
    var formNS = wod.CLS.getNS("wod.forms");
    /* 开始，字段基类 */
    wod.CLS.getClass({
        name: function () {

        },
        _init: function () {
            this.parent();
        },
        getForm: function () {
            
        },
        init: function (fieldBody, setting) {

        }
    }, "wod.forms.FieldBase");
    /* 结束，字段基类 */

    /* 开始，表单类 */
    wod.CLS.getClass({
        fields:[],
        name:"",
        init: function (formBody, fieldsDefine) {

        },
        sync: function () {

        },
        getData: function () {
            this.sync();
        },
        setData: function (data) {

        },
        registValidate: function (validator) {

        },
        validate: function () {

        },
        tipError: function (fname, msg) {

        }
    }, "wod.forms.Form");
    /* 结束，表单类 */
})(wod);