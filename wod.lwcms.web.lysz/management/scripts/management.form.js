var _$wod_form = {};
_$wod_form.ajaxError = function (status) { alert(status); };
_$wod_form.ajaxValidResponse = function (data) {
    if (!data.status) { alert(data.message); }
    return data.status;
};

/* controls wodtext woddrop */
(function ($, K) {
    function _wodtext(setting) {
        var _def = {
            minlength: 0
            , maxlength: 255
            , trim: true
            , type: "str" // en-str , num
            , regexp: ""
        };
        setting = $.extend(_def, setting);
        this.attr("maxlength", setting.maxlength);

        if (setting.type != "str") {
            this.css("ime-mode", "disabled");
        }
        this.keyup(function (event) { });
        this.blur(function (event) { });
    }
    $.fn.extend({ wodtext: _wodtext });

    function _woddrop(setting) {
        var _def = {
            allownull: true
            , options: []
            , optionscmd: "op_category"
            , optionscmddata: {}
        };
        setting = $.extend(_def, setting);
        
        var preHandlerArr = setting.formHandler.preSubmitHandler;
        function createOption($input,options){
            var sel = $("<select></select>");
            sel.addClass("woddrop");
            $input.after(sel);
            $input.attr("type","hidden").hide();
            preHandlerArr.push(function(){ $input.val(sel.val()); });
            $.each(options,function(i,item){
                sel.append($("<option></option>").text(item.name).val(item.value));
            });
            sel.val($input.val());
            $input.val(sel.val());
        }

        if(setting.options.length){
            createOption(this,setting.options);
        }
        else{
            var _selfInput = this;
            $.ajax({
                url:"common.ashx?command="+setting.optionscmd,
                data:setting.optionscmddata,
                type:"post",
                dataType:"json",
                success:function(data){
                    if(_$wod_form.ajaxValidResponse(data)){
                        createOption(_selfInput,data.result);
                    }
                },
                error:_$wod_form.ajaxError
            });
        }
    }
    $.fn.extend({ woddrop: _woddrop });

    function _wodrichtext(setting) {
        var _def = {
        };
        setting = $.extend(_def, setting);
        var preHandlerArr = setting.formHandler.preSubmitHandler;
        var editor1 = K.create(this.selector, {
            cssPath: 'editor/plugins/code/prettify.css',
            uploadJson: 'editor/upload_json.ashx',
            fileManagerJson: 'editor/file_manager_json.ashx',
            allowFileManager: true,
            afterCreate: function () {
                var self = this;
                preHandlerArr.push(function(){
                    self.sync();
                });
            }
        });
    }
    $.fn.extend({ wodrichtext: _wodrichtext });

    var siteImagSeed = 1;
    function _wodsiteImage(setting){
        var _input = this;
        var _def = {
            small:{width:180,height:120,description:"小图片(建议尺寸180*120)"},
            normal:{width:500,height:200,description:"普通图片(建议尺寸500*200)"},
            big:{width:960,height:350,description:"大图片(建议尺寸960*350)"}
        };
        setting.small = $.extend(_def.small, setting.small);
        setting.normal = $.extend(_def.normal, setting.normal);
        setting.big = $.extend(_def.big, setting.big);
        var preHandlerArr = setting.formHandler.preSubmitHandler;
        
        var editor1 = K.editor({
            allowFileManager : true ,
            uploadJson: 'editor/upload_json.ashx',
            fileManagerJson: 'editor/file_manager_json.ashx'
        });
        var imageTypes = ["small","normal","big"];
        var inputs = {};
        function _updateInput(){
            var data = {};
            for (var i in inputs) {
                data[+i+"Img"] = inputs[i].val();
            }
            _input.val($.toJSON(data));
        }
        var editorDom = $("<div class=\"f-form-mimg\"></div>");
        var oldData = _input.val() ? $.evalJSON(_input.val()) : {};
        for (var i = 0,length = imageTypes.length; i < length; i++) {
            var imgSetting =  setting[imageTypes[i]];
            if(imgSetting&&imgSetting.canset){
                var iptID = "f-form-mimg_" + siteImagSeed++;
                inputs[imageTypes[i]] = $("<input type=text readonly id=\""+iptID+"\">")
                    .val(oldData[imageTypes[i] +"Img"])
                    .attr("imgType",imageTypes[i]).focus(
                    function(){
                        var imgType = $(this).attr("imgType");
                        editor1.loadPlugin('image', 
                            function() {
					            editor1.plugin.imageDialog({
						            imageUrl : inputs[imgType].val(),
                                    imageHeight:setting[imgType].height,
                                    imageWidth:setting[imgType].width,
                                    //imageTitle:setting[imgType].description,
						            clickFn : function(url, title, width, height, border, align) {
							            inputs[imgType].val(url);
                                        _updateInput();
							            editor1.hideDialog();
						            }
					            });
				            });
                    });
                editorDom.append("<div class=\"f-form-mimg-label\"><label for=\""+iptID+"\">"+imgSetting.description+"</label></div><div class=\"f-form-mimg-input\"></div>").find(".f-form-mimg-input:last").append(inputs[imageTypes[i]]);
            }
        }
        _input.attr("type","hidden").hide();
        _input.after(editorDom);
    }
    $.fn.extend({ wodsiteImage: _wodsiteImage });
})($, KindEditor);

(function (w) {
    var artSch = {
        name: {display:"内容标题",type:"wodtext",setting:{
            allownull : true
            , options : []
            , optionscmd : "op_category"
            , optionscmddata : {}
        }}
        , description: {display:"SEO描述",type:"wodtext",setting:{}}
        , keywords: {display:"SEO关键字",type:"wodtext",setting:{}}
        , category: {display:"分类",type:"woddrop",setting:{}}
        , code: {display:"名称(url名称)",type:"wodtext",setting:{}}
        , content: {display:"内容",type:"wodrichtext",setting:{

        }}
        , preContent: {display:"内容预览",type:"wodtext",setting:{}}
        , page: {display:"版式",type:"wodtext",setting:{}}
        , image: {display:"相关图片",type:"wodsiteImage",setting:{small:{canset:true},normal:{canset:true},big:{canset:true}}}
    };
    _$wod_form.artSch = artSch;
    
    function _getFormData(frm) {
        var data = {};
        function _updateData($dom, pnarr, pn, val) {
            if (pnarr.length > 0) {
                var list = $dom.parents("[list-name='" + pnarr.join(".") + "'][data-form=list]");
                var row = $dom.parents("[list-name='" + pnarr.join(".") + "'][data-form=list] [data-form=row]");
                var subform = $dom.parents("[subform-name='" + pnarr.join(".") + "'][data-form=subform]");
                if (row.length == 1) {
                    var listdata = list.data("formdata");
                    if (listdata) {
                    }
                    else {
                        listdata = [];
                        list.data("formdata", listdata);
                        var lstpnarr = list.attr("list-name").split(".");
                        var lstpn = lstpnarr.pop();
                        _updateData(list, lstpnarr, lstpn, listdata);
                    }
                    listdata = listdata || [];
                    var rowdata = row.data("formdata");
                    if (rowdata) {
                    }
                    else {
                        rowdata = {};
                        listdata.push(rowdata);
                        row.data("formdata", rowdata);
                    }
                    rowdata[pn] = val;
                } else if (subform.length == 1) {
                    var subformdata = subform.data("formdata");
                    if (subformdata) {
                    }
                    else {
                        subformdata = {};
                        subform.data("formdata", subformdata);
                        var sfpnarr = subform.attr("subform-name").split(".");
                        var sfpn = sfpnarr.pop();
                        _updateData(subform, sfpnarr, sfpn, subformdata);
                    }
                    subformdata[pn] = val;
                }
            } else {
                data[pn] = val;
            }
        }
        frm.find("input,textarea,select").each(function () {
            if (this.name) {
                var nameparse = this.name.split(".");
                var pn = nameparse.pop();
                var val = $(this).val();
                _updateData($(this), nameparse, pn, val);
            }
        });
        frm.find("[data-form=list],[data-form=row],[data-form=subform]").each(function () {
            $(this).data("formdata", null);
        });
        return data;
    }
    function _setFormData(data) {
        for (var name in data) {
            this.find("[name='"+name+"']").val(data[name]);
        }
    }


    function _wodForm(setting){
        var sch = setting.schema;
        var formHandler={
            preSubmitHandler: []
            , preSubmit:function(){
                for (var i=0;i<this.preSubmitHandler.length;i++) {
                    this.preSubmitHandler[i]();
                }
            }
        };
        var _fpanel = this; 
        for (var sn in sch) {
            var s = sch[sn];
            s.setting.formHandler = formHandler;
            _fpanel.find("[name='"+sn+"']")[s.type](s.setting);
        }
        var submitSet = setting.submit;
        $(submitSet.selector).bind(submitSet.event,function(){
            formHandler.preSubmit();
            var _def = {
                url : "wodform.ashx?path=/submit"
                , callback : function(){}
                , command : "common_form_cmd"
            };
            var posts = $.extend(_def, submitSet.posts);

            var data = _getFormData(_fpanel);
            var errorCallback = function(){};
            var validate = submitSet.validate;
            if (!validate || validate(data, errorCallback)){
                $.ajax({ type:"post" ,url : posts.url , data : data, dataType : "json", error: _$wod_form.ajaxError, success : function(data,statusText){if(_$wod_form.ajaxValidResponse(data)){ posts.callback(data.result); } } });
            }
        });
    }

    function _setFormData(data) {
        for (var name in data) {
            this.find("[name='"+name+"']").val(data[name]);
        }
    }
    $.fn.extend({
        wodForm:_wodForm,
        setFormData:_setFormData
    });
})(window,_$wod_form,$);