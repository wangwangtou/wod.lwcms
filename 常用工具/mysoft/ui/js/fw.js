Array.prototype.remove = function (item) {
    for (var i = 0; i < this.length; i++) {
        if (item == this[i]) {
            return this.splice(i, 1);
        }
    }
};
(function ($) {
    $.extend({
        fw: {
            initApp:function(){
                var albumsList = $("#fw-albums")
                albumsList.change(function () {
                    $.fw.loadAlbum($(this).val());
                });
                var ap = $("#fw-album");
                ap.delegate(".imglst li","click",function(){
                    if($(this).hasClass("active")){
                        $(this).removeClass("active");
                    }
                    else {
                        $(this).addClass("active");
                    }
                });
                ap.delegate(".imglst li .desc","click",function(){
                    var $li = $(this).parent();
                    var pic = $li.data("_s_data");
                    $.fw.editPic(pic,ap.data("_s_data").photos);
                    return false;
                });
                this.loadAlbums();
            },
            loadAlbums: function () {
                $.getData("data", function (data, err) {
                    if (err) {
                        alert("程序错误！" + err);
                    }
                    else {
                        var sel = $("#fw-albums").html("");
                        var html = [];
                        for (var name in data) {
                            var item = data[name];
                            html.push("<option value=" + item.code + ">" + item.name + "</option>")
                        }
                        sel.append(html.join());
                        sel.change();
                        $.fw.data = data;
                    }
                });
            },
            addAlbum: function () {
                $.ui.getForm("填写相册信息", "albumform", function (album) {
                    if (album) {
                        album.create = new Date()
                        $.fw.data[album.code] = album;
                        $.setData("data", $.fw.data, function (data, err) {
                            if (err) {
                                alert(err);
                            } else {
                                album.syncState = { finished: false, lastmodify: album.create };
                                album.photos = [];
                                $.setData(album.code, album, function (data, err) {
                                    if (err) {
                                        alert(err);
                                    }
                                    else {
                                        $.fw.loadAlbums();
                                    }
                                });
                            }
                        });
                    }
                }, function (album, callback) {
                    if (!album.name) {
                        callback("name", "不能为空");
                        return false;
                    }
                    if (!album.code) {
                        callback("code", "不能为空");
                        return false;
                    }
                    if ($.fw.data[album.code]) {
                        callback("code", "不能重复");
                        return false;
                    }
                    if (!album.sizeType) {
                        callback("sizeType", "不能为空");
                        return false;
                    }
                    return true;
                });
            },
            loadAlbum: function (code) {
                var ap = $("#fw-album");
                ap.attr("ablum", code);
                var _picSource = "/api/imgres/";
                $.getData(code, function (data, err) {
                    if (err) {
                        alert("程序错误！");
                    }
                    else {
                        ap.data("_s_data", data);
                        ap.find("h1").text(data.name);
                        ap.find(".description").text(data.description);
                        ap.find(".code").text(data.code);
                        var imglst = ap.find(".imglst");
                        imglst.html("");
                        $.each(data.photos, function (index) {
                            var desc = "<div class=\"desc\"><p><span class=\"no\">"+(index+1)+"</span>"+this.description+"</p></div>";
                            imglst.append($("<li></li>").data("_s_data", this).append($("<img/>").attr("src", _picSource + encodeURI(this.path))).append(desc));
                        });
                    }
                });
            },
            saveAlbum : function(){
                var ap = $("#fw-album");
                var data = ap.data("_s_data");
                $.setData(data.code, data, function (r,err) {
                    if (err) {
                        alert("程序错误！");
                    }
                    $.fw.loadAlbum(data.code);
                });
            },
            addPic: function () {
                $.ui.getPic(function (selPic) {
                    var ap = $("#fw-album");
                    var data = ap.data("_s_data");
                    $.each(selPic,function(){
                        data.photos.push({path:this,description:""});
                    });
                    $.fw.saveAlbum();
                });
            },
            deletePic: function () {
                var ap = $("#fw-album");
                var data = ap.data("_s_data");
                ap.find(".active").each(function () {
                    var p = $(this).data("_s_data");
                    data.photos.remove(p);
                });
                $.fw.saveAlbum();
            },
            editPic :function(pic,array){
                $.ui.picDesc(pic,array,function(){
                    $.fw.saveAlbum();
                });
            },
            upload:function(){
                var ap = $("#fw-album");
                var code = ap.attr("ablum");
                _uploadApi = "/api/upload/";

                $.ajax({ url: _uploadApi + code, type: "post", dataType: "json"
                    , success: function (data) {
                        alert("正在上传，请耐心等待，不要关闭窗口！");
                    }, error: function (textStatus) {
                        alert("上传出错！");
                    }
                });
            }
        }
    });
    $(function () { $.fw.initApp(); });
})($);