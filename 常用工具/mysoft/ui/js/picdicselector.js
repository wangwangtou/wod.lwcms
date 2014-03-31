(function ($) {
    var _folderAPI = "/api/getdir";
    //http://127.0.0.1:8762/api/getdir/d:%5C
    var _picSource = "/api/imgres/";
    function _picSelector($pic) {
        var folder = $pic.find(".folder");
        var foldertitle = $pic.find(".foldertitle");
        var imglst = $pic.find(".imglst");
        var _c = {
            picTypes: {
                "gif": "image/gif",
                "jpeg": "image/jpeg",
                "jpg": "image/jpeg",
                "png": "image/png"
            },
            isFolder: function (p) { return p.type == "folder"; },
            isImage: function (p) { return !!this.picTypes[p.type]; }
        };
        function _createPath(path) {
            var panel = $("<span></span>");
            var root = $("<a></a>").text("我的设备").attr("fullpath", "");
            panel.append(root);
            panel.append($(document.createTextNode(" > ")));
            var ps = path ? path.split("\\") : [];
            var pt = "";
            for (var i = 0; i < ps.length; i++) {
                if (!ps[i])
                    break;
                pt += ps[i];
                var p = $("<a></a>").text(ps[i]).attr("fullpath", pt);
                pt += "\\";
                panel.append(p);
                panel.append(document.createTextNode(" > "));
            }
            return panel;
        }
        function _load(path) {
            var url = _folderAPI;
            if (path) {
                url += "/" + encodeURI(path);
            }
            foldertitle.html("").append(_createPath(path));
            $.ajax({ url: url, type: "post", dataType: "json"
                , success: function (data) {
                    folder.find("li").remove();
                    imglst.find("li").remove();
                    $.each(data, function () {
                        if (_c.isFolder(this)) {
                            folder.append($("<li></li>").text(this.name).attr("fullpath", this.fullpath));
                        }
                        else if (_c.isImage(this)) {
                            imglst.append($("<li></li>").attr("fullpath", this.fullpath).append($("<img/>").attr("src", _picSource + encodeURI(this.fullpath))));
                        }
                    });
                }
            });
        }
        folder.delegate("li", "click", function () {
            var path = $(this).attr("fullpath");
            _load(path);
        })
        imglst.delegate("li", "click", function () {
            if ($(this).hasClass("active")) {
                $(this).removeClass("active");
            }
            else {
                $(this).addClass("active");
            }
        })
        foldertitle.delegate("a", "click", function () {
            var path = $(this).attr("fullpath");
            _load(path);
        })
        _load();
    }
    function _getPicSelector($pic) {
        var pics = [];
        $pic.find(".imglst li.active").each(function () {
            pics.push($(this).attr("fullpath"));
        });
        return pics;
    }
    $.extend($.ui, {
        getPic: function (callback) {
            this.loadTmp("picdicselector", function (pic) {
                var __win;
                $.ui.window(pic, {
                    title: "选择图片",
                    info: "从电脑选择图片文件",
                    width: 600,
                    height: 360,
                    closeCallback: function () {
                        callback([]);
                    },
                    okCallback: function () {
                        var data = _getPicSelector(pic);
                        callback(data);
                        if (__win) __win.remove();
                    },
                    loadCallback: function (win) {
                        __win = win;
                        _picSelector(pic);
                    }
                });
            });
        }
    });
})($);