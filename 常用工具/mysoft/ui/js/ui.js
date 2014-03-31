(function ($) {
    var tmpUrl = "/ui/tmp/";
    $.extend({
        ui: {
            loadTmp: function (name, callback) {
                $.get(tmpUrl + name + ".html", function (html) {
                    callback($(html));
                });
            },
            window: function ($content, setting) {
                var _defSet = { defShow: true , width  : 480, height:360 };
                setting = $.extend(_defSet, setting);
                this.loadTmp("window", function (win) {
                    var t = win.find(".u-tt");
                    var cl = win.find(".lyclose");
                    var m = win.find(".lymask");
                    var cnt = win.find(".lyct");
                    var ok = win.find(".lybtns button").first();
                    var cancel = win.find(".lybtns button").last();
                    var info = win.find(".lyother");

                    var lywrap = win.find(".lywrap");
                    if (setting.width) lywrap.css("width", setting.width + "px");
                    if (setting.height) cnt.css("height", setting.height + "px");

                    cnt.append($content);
                    if (setting.title) { t.text(setting.title); }
                    if (setting.info) { info.text(setting.info); }

                    $("body").append(win);
                    if (!setting.defShow) {
                        win.hide();
                    }
                    function _closeWin() {
                        win.remove();
                        if (setting.closeCallback) setting.closeCallback();
                        return false;
                    }
                    function _okWin() {
                        if (setting.okCallback) setting.okCallback();
                        return false;
                    }
                    cl.click(_closeWin);
                    m.click(_closeWin);
                    cancel.click(_closeWin);
                    ok.click(_okWin);
                    if (setting.loadCallback) setting.loadCallback(win);
                });
            }
        }
    });
})($);