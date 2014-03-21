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
        , cat: {display:"分类",type:"woddrop",setting:{}}
        , code: {display:"名称(url名称)",type:"wodtext",setting:{}}
        , content: {display:"内容",type:"wodrichtext",setting:{

        }}
        , preContent: {display:"内容预览",type:"wodtext",setting:{}}
        , page: {display:"版式",type:"wodtext",setting:{}}
        , image: {display:"相关图片",type:"wodtext",setting:{}}
    };
    _$wod_form.artSch = artSch;

    function _wodForm(frm){
        var sch = frm.schema;
        var formHandler={
            preSubmitHandler:[]
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
        var s = frm.submit;
        $(s.selector).bind(s.event,function(){
            formHandler.preSubmit();
            var _def = {
                url : "wodform.ashx?path=/submit"
                , callback : function(){}
                , command : "common_form_cmd"
            };
            var setting = $.extend(_def, s.settings);

            var data = {};

            _fpanel.find("input,textarea").each(function(){
                if($(this).attr("name")){
                    data[$(this).attr("name")] = $(this).val();
                }
            });
            $.ajax({ type:"post" ,url : setting.url , data : data, dataType : "json", error: _$wod_form.ajaxError, success : function(data,statusText){if(_$wod_form.ajaxValidResponse(data)){ setting.callback(data.result); } } });
        });
    }

    function _loadFormData(data) {
        for (var name in data) {
            this.find("[name='"+name+"']").val(data[name]);
        }
    }
    $.fn.extend({
        wodForm:_wodForm,
        loadFormData:_loadFormData
    });
})(window,_$wod_form,$);