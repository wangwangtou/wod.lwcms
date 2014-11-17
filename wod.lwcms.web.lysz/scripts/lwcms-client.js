; (function () { 
    window.lwcms = {};
    window.lwcms.viewArticle = function(id){
        $.post("/index.aspx?path=/common/view&aid="+id);
    };
    window.lwcms.getComments = function(id,container,ps,callback){
        function pageLinkClick()
        {
        var href =$(this).attr("href");
            if(href.indexOf("javascript")>=0){
            return false;}
            else{
            $.get(href,function(html){
                $(container).html(html);
                $(container).find(".m-page a").click(pageLinkClick);
                if(callback) callback();
            });
            return false;}
        }
        $.get("/index.aspx?path=/common/getcomment&pageSize="+(ps||10)+"&aid="+id +"&rdcode="+Math.random(),function(html){
            $(container).html(html);
            $(container).find(".m-page a").click(pageLinkClick);
            if(callback) callback();
        });
    };
    var handleerror = function(data){
        if(!data.status && data.message){alert(data.message);}
    };
    window.lwcms.commentArticle = function(id,data,callback){
        $.post("/index.aspx?path=/common/addcomment&aid="+id,data,function(data){
            handleerror(data);
            if(callback) callback(data);
        });
    };
    window.lwcms.regist = function(data,callback){
        $.post("/index.aspx?path=/common/regist",data,function(data){
            handleerror(data);
            if(callback) callback(data);
        });
    };
    window.lwcms.login = function(data,callback){
        $.post("/index.aspx?path=/common/login",data,function(data){
            handleerror(data);
            if(callback) callback(data);
        });
    };
})();