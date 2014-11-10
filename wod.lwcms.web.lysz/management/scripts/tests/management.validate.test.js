(function (wod) {
    var validator;
    test("wod.forms.validator 检测", function () {
        validator = wod.forms.validator;
        return !!validator;
    });

    test("validate 空对象", function () {
        return validator.validate([])
            || validator.validate(undefined);
    });

    test("validate", function () {
        var formValidator = [
            (function () {
                return true;
            })
        ];
        return validator.validate(formValidator);
    });
    test("validate getLength", function (msgWrite, assert) {
        var formValidator = [
            validator.getLength(0, 30).bind(null, "10"),
            validator.getLength(0, 2).bind(null, "1000"),
            validator.getLength(2, 4).bind(null, "1"),
            validator.getLength(2, 4).bind(null, "4343")
        ];
        assert(validator.validate(formValidator[0]), "getLength(0,30) 10");
        assert(!validator.validate(formValidator[1]), "getLength(0,2) 1000");
        assert(!validator.validate(formValidator[2]), "getLength(2,4) 1");
        assert(validator.validate(formValidator[3]), "getLength(2,4) 4343");

        return !validator.validate(formValidator);
    });
    test("validate getRequire", function (msgWrite, assert) {
        var formValidator = [
            validator.getRequire().bind(null, "10"),
            validator.getRequire().bind(null, undefined),
            validator.getRequire().bind(null, 0),
            validator.getRequire().bind(null, 1),
            validator.getRequire().bind(null, null)
        ];
        assert(validator.validate(formValidator[0]), "getRequire() 10");
        assert(!validator.validate(formValidator[1]), "getRequire() undefined");
        assert(validator.validate(formValidator[2]), "getRequire() 0");
        assert(validator.validate(formValidator[3]), "getRequire() 1");
        assert(!validator.validate(formValidator[4]), "getRequire() null");
        return !validator.validate(formValidator);
    });
    test("validate getRegExp", function (msgWrite, assert) {
        var formValidator = [];
        function testhelp(reg, val, valid) {
            var validator1 = validator.getRegExp(reg).bind(null, val);
            formValidator.push(validator1);
            assert(valid === validator.validate(validator1), "getRegExp(" + reg + ") " + val);
        }
        testhelp("date", "2014-11-14", true);
        testhelp("tel", "0712-2987822-013", true);
        testhelp("mobile", "+8615693333521", true);
        testhelp("url", "http://www.baidu.com", true);
        testhelp(/^[0-9]+/g, "a0500", false);
        testhelp(/[0-9]+/g, "a0500", true);
        testhelp(/^[0-9]+/g, "012312500", true);
        testhelp(/^[0-9]+$/g, "0000a", false);
        testhelp(/^[0-0]+$/g, "0000a", false);
        testhelp(/^[0-0]+$/g, "0000", true);
        return !validator.validate(formValidator);
    });
    test("validate getRange", function (msgWrite, assert) {
        var formValidator = [];
        function testhelp(valmin, valmax, margin, val, valid) {
            var validator1 = validator.getRange(valmin, valmax, margin).bind(null, val);
            formValidator.push(validator1);
            assert(valid === validator.validate(validator1), "getRange(" + valmin + "," + valmax + "," + margin + ") " + val);
        }
        testhelp("00", "0a", false, "09", true);
        testhelp(0, 20, false, 19, true);
        testhelp(0, 20, false, 20, true);
        testhelp(0, 20, false, 21, false);
        testhelp(0, 20, false, 0, true);
        testhelp(0, 20, false, -1, false);
        testhelp(0, 20.1, false, 20.9, false);
        testhelp(0, 20.1, false, 20.1, true);
        testhelp(0, 20.1, "min", 20.1, false);
        testhelp(0, 20.1, "max", 20.1, true);
        testhelp(0, 20.1, "none", 20.1, false);
        return !validator.validate(formValidator);
    });

    test("validateForm", function (msgWrite, assert) {
        var formValidator = [
            { name: "name", validator: validator.getLength(0, 30), message: "" },
            { name: "name", validator: validator.getRegExp(), message: "" }
        ];
        var data = { name: "wangx" };
        assert(validator.validateForm(formValidator, data), "validateForm allValid");
        formValidator.push({ name: "age", validator: validator.getRange(0, 20), message: "" });
        data.age = 21;
        assert(!validator.validateForm(formValidator, data), "validateForm oneNotValid");
        return true;
    });

    test("validateForm，带errorCallback", function (msgWrite, assert) {
        var formValidator = [
            { name: "name", validator: validator.getLength(0, 30), message: "" },
            { name: "name", validator: validator.getRegExp(), message: "" }
        ];
        var data = { name: "wangx" };
        var errorCallback = function (name, msg) {
            msgWrite(name + ":" + msg);
        };
        assert(validator.validateForm(formValidator, data, errorCallback), "validateForm allValid");

        formValidator.push({ name: "age", validator: validator.getRange(0, 20), message: "" });
        data.age = 21;
        errorCallback = function (name, msg) {
            assert(name == "age", "errorCallback isNameSuccess");
            assert(msg == "必须在0-20内", "errorCallback defaultMsgSuccess");
        };
        assert(!validator.validateForm(formValidator, data, errorCallback), "validateForm oneNotValid");

        formValidator[2].message = "20岁以内才能注册";
        errorCallback = function (name, msg) {
            assert(name == "age", "errorCallback isNameSuccess");
            assert(msg == formValidator[2].message, "errorCallback customMsgSuccess");
        };
        assert(!validator.validateForm(formValidator, data, errorCallback), "validateForm oneNotValid");

        data.age = 20;
        assert(validator.validateForm(formValidator, data, errorCallback), "validateForm allValid");
        return true;
    });
})(wod);