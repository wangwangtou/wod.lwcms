(function ($) {
    var reloadurl = "/reload";
    //http://127.0.0.1:8762/reload
    $.extend({
        serverReload: function (callback) {
            $.ajax({ url: reloadurl, type: "post", dataType: "json"
                , success: function (data) {
                    if (typeof (callback) == "function") {
                        callback(data);
                    }
                    else {
                        alert("刷新成功！");
                    }
                }
            });
        },
        getData: function (name, callback) {
            var geturl = "/api/getdata/" + name;
            $.ajax({ url: geturl, type: "post", dataType: "json"
                , success: function (data) {
                    if (typeof (callback) == "function") {
                        callback(data);
                    }
                    else {
                        alert($.toJSON(data));
                    }
                }, error: function (textStatus) {
                    if (typeof (callback) == "function") {
                        callback(null, textStatus);
                    }
                    else {
                    }
                }
            });
        },
        setData: function (name, data, callback) {
            var seturl = "/api/setdata/" + name;
            $.ajax({ url: seturl, type: "post", dataType: "json"
                , success: function (data) {
                    if (typeof (callback) == "function") {
                        callback(data);
                    }
                    else {
                        alert($.toJSON(data));
                    }
                }, error: function (textStatus) {
                    if (typeof (callback) == "function") {
                        callback(null, textStatus);
                    }
                    else {
                    }
                }
                , data: { jsonstring: $.toJSON(data) }
            });
        }
    });
})($);