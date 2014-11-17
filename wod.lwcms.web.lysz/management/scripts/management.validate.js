(function (wod) {
	wod.CLS.getClass("wod.forms.validator");

	wod.statics.mixin(wod.forms.validator, {
	    getLength: function (min,max) {
	        validate = function (val) {
	            var l = !val ? 0 : (function len(s) {
	                var l = 0;
	                var a = s.split("");
	                for (var i = 0; i < a.length; i++) {
	                    if (a[i].charCodeAt(0) < 299) {
	                        l++;
	                    } else {
	                        l += 2;
	                    }
	                }
	                return l;
	            })(val);
	            return l >= min && l <= max;
	        };
	        validate.message = "长度必须在" + min + "-" + max + "内";
	        return validate;
	    },
	    getRequire : function() {
	        validate = function (val) {
	            return val === 0 || val === false || (!!val &&
                    (typeof (val) != "string" ? true : !!val.replace(/(^\s*)|(\s*$)/g, "")));
	        };
	        validate.message = "不能为空";
	        return validate;
	    },
	    getRegExp : function(regExp) {
	        if (typeof (regExp) == "string") {
	            switch (regExp) {
	                case "date": regExp = /^(\d{1,4})(-|\/)(\d{1,2})\2(\d{1,2})$/; break;
	                case "tel": regExp = /^((\(\d{2,3}\))|(\d{3}\-))?(\(0\d{2,3}\)|0\d{2,3}-)?[1-9]\d{6,7}(\-\d{1,4})?$/; break;
	                case "email": regExp = /^\w+((-\w+)|(\.\w+))*\@[A-Za-z0-9]+((\.|-)[A-Za-z0-9]+)*\.[A-Za-z0-9]+$/; break;
	                case "mobile": regExp = /^(\+?[0-9]{2})?0?(1[3-8])[0-9]{9}$/; break;
	                case "url": regExp = /^http:\/\/[A-Za-z0-9]+\.[A-Za-z0-9]+[\/=\?%\-&_~`@[\]\':+!]*([^<>\"\"])*$/; break;
	                default:
	                    regExp = new RegExp(regExp)
	                    break;
	            }
	        }
	        validate = function (val) {
	            return !(val && !val.match(regExp));
	        };
	        validate.message = "应为"+regExp+"格式";
	        return validate;
	    },
	    getRange: function (min, max, margin) {
	        var validate;
	        if (!margin || margin == "all") {
	            validate = function (val) {
	                return val >= min && val <= max;
	            };
	            validate.message = "必须在" + min + "-" + max + "内";
            }
	        else {
	            switch (margin) {
	                case "max":
	                    validate = function (val) {
	                        return val > min && val <= max;
	                    };
	                    validate.message = "必须在(" + min + "-" + max + "]内";
	                    break;
	                case "min":
	                    validate = function (val) {
	                        return val >= min && val < max;
	                    };
	                    validate.message = "必须在[" + min + "-" + max + ")内";
	                    break;
	                case "none":
	                default:
	                    validate = function (val) {
	                        return val > min && val < max;
	                    };
	                    validate.message = "必须在(" + min + "-" + max + ")内";
	                    break;
	            }
	        }
	        return validate;
	    }
	});
	wod.statics.mixin(wod.forms.validator, {
	    validate: function (validates) {
	        if (!validates)
	            return true;
	        if (validates.length) {
	            for (var i = 0, length = validates.length; i < length; i++) {
	                var result = validates[i]();
	                if (!result)
	                    return false;
	            }
	            return true;
	        }
	        else if (typeof (validates) == "function") {
	            return validates();
	        }
	    },
	    validateForm: function (formValidator, data, errorCallback) {
	        var validates = this._convertFormValidator(formValidator, data, errorCallback);
	        return this.validate(validates);
	    },
	    _convertFormValidator: function (formValidator, data, errorCallback) {
	        //{ name: "name", validator: validator.getLength(0, 30), message: "" }
	        if (!formValidator)
	            return undefined;
	        if (formValidator.length) {
	            var validates = [];
	            for (var i = 0, length = formValidator.length; i < length; i++) {
	                validates.push(this._convertSingleFormValidator(formValidator[i], data[formValidator[i].name], errorCallback))
	            }
	            return validates;
	        }
	        else {
	            return this._convertSingleFormValidator(formValidator[i], data[formValidator[i].name], errorCallback);
	        }
	    },
	    _convertSingleFormValidator: function (formValidator, val, errorCallback) {
	        return function () {
	            if (formValidator.validator(val)) {
	                return true;
	            }
	            else {
	                if (errorCallback) {
	                    errorCallback(formValidator.name, formValidator.message || formValidator.validator.message);
	                }
	                return false;
	            }
	        }
	    }
	});
})(wod);