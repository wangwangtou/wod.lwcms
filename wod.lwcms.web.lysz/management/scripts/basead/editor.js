function autherEdit(){
    this.canEdit = true;
    this.canDelete = true;
    var tmp;
    var pre = {};
    this.edit = function($d){
        var $div = $("<div>");
        var data = {name:$d.text(),title:$d.text(),target:$d.attr("target"),naviUrl:$d.attr("href")};
        if(tmp){
            $div.append(tmp.clone(true));
            $div.setFormData(data);
        }
        else{
            pre.div=$div;
            pre.data=data;
        }
        return $div; 
    };
    this.delete = function($d){
        return $d.parent().remove();  
    };
    this.getCommands = function ($d) {
        if($d.is(".nav>a")){
            var $addSub = $("<a href='javascript:void(0);'>添加子导航</a>");
            $addSub.click(function(){
                if(!$d.next().is("p.subs")){
                    $d.after("<p class='subs'><span class='subsplit'>|</span><span class='subsplit'>|</span></p>")
                }
                $d.next().find(".subsplit:eq(1)").before("<a href='http://'>新导航</a>");
            })
            return $addSub;
        }
    };
    this.commit = function($d,$f){
        $f.syncForm();
        var data = $f.getFormData();
        $d.text(data.name)
            .attr("title",data.title)
            .attr("target",data.target)
            .attr("href",data.naviUrl);
    };
    this.unbind=function(){
       pre = {};
    };
    this.tmpLoaded = function($dom){
        tmp = $dom;
        if(pre.div){
            pre.div.append(tmp.clone(true));
            pre.div.setFormData(data);
        }
    };   
    $.ui.loadTmp("naviform",this.tmpLoaded.bind(this));
}
function navListEdit(){
    this.canEdit = false;
    this.canDelete = false;
    this.cmdposition = "pointer";
    this.edit = function($d){
    };
    this.delete = function($d){
    };
    this.getCommands = function ($d) {
        var $add = $("<a href='javascript:void(0);'>添加导航</a>");
        $add.click(function(){
            if($d.children().length){
                $d.append("<span class='split'>|</span>")
            }
            $d.append("<a href='http://'>新导航</a>");
        })
        return $add;
    };
    this.commit = function($d,$f){
    };
    this.unbind=function(){
    };
}
function textEdit() {
    this.canEdit = true;
    this.canDelete = false;
    this.cmdposition = "pointer";
    var tmp;
    var pre = {};
    this.edit = function($d){
        var $div = $("<div>");
        var data = {text:$d.text()};
        if(tmp){
            $div.append(tmp.clone(true));
            $div.setFormData(data);
        }
        else{
            pre.div=$div;
            pre.data=data;
        }
        return $div; 
    };
    this.delete = function($d){
      return $d.parent().remove();  
    };
    this.commit = function($d,$f){
        $f.syncForm();
        var data = $f.getFormData();
        $d.text(data.text);
    };
    this.unbind=function(){
       pre = {};
    };
    this.tmpLoaded = function($dom){
        tmp = $dom;
        if(pre.div){
            pre.div.append(tmp.clone(true));
            pre.div.setFormData(data);
        }
    };   
    $.ui.loadTmp("textarea",this.tmpLoaded.bind(this));
}

function htmlEdit() {
    this.canEdit = true;
    this.canDelete = false;
    this.cmdposition = "pointer";
    var tmp;
    var pre = {};
    this.edit = function($d){
        var $div = $("<div>");
        var data = {html:$d.html()};
        if(tmp){
            $div.append(tmp.clone(true));
            $div.setFormData(data);
            setTimeout(function(){
                $div.wodForm({schema:{html:{display:"内容",type:"wodrichtext",setting:{type:"simple"}}}});
            },0);
        }
        else{
            pre.div=$div;
            pre.data=data;
        }
        return $div; 
    };
    this.delete = function($d){
      return $d.parent().remove();  
    };
    this.commit = function($d,$f){
        $f.syncForm();
        var data = $f.getFormData();
        $d.html(data.html);
    };
    this.unbind=function(){
       pre = {};
    };
    this.tmpLoaded = function($dom){
        tmp = $dom;
        if(pre.div){
            pre.div.append(tmp.clone(true));
            pre.div.setFormData(data);
            pre.div.wodForm({schema:{html:{display:"内容",type:"wodrichtext",setting:{type:"simple"}}}});
        }
    };   
    $.ui.loadTmp("richtext",this.tmpLoaded.bind(this));
}

var cfg = {
    ".nav a": new autherEdit(),
    ".nav": new navListEdit(),
    ".wodhead .seotitle": new textEdit(),
    ".wodhead .keywords": new textEdit(),
    ".wodhead .description": new textEdit(),
    ".m-copyright": new htmlEdit()
};
var css = document.createElement("div");
var cssStr = "_<style>.__e_bd{border:2px solid #6d6;background:#eee;}";
cssStr += ".__e_cmd{}";
cssStr += ".__e_cmd a{display:block;background:#eee;margin:0 0 2px 0;}";
cssStr += ".__e_frm{background: #eee;padding: 10px;border-radius: 3px;}";
//cssStr += ".__e_frm input{border: #999 1px solid;padding: 2px 6px;}";
cssStr += ".__e_frm .__e_frm_cmd{text-align:center;}";
cssStr += ".__e_frm .__e_frm_cmd a{display:inline-block;background:#eee;margin:2px 4px;}";
cssStr += ".opacity{opacity:.4}</style>";
css.innerHTML = cssStr;
$("head").append(css.childNodes[1])
var untilSelecter = 'body';
$(function(){
    function matchSel(sel,$d){
        return $d.is(sel) || $d.parents(sel).length;
    }
    function getSel(sel, $d) {
        if ($d.is(sel))
            return $d;
        else {
            return $d.parents(sel);
        }
    }
    function resetCur(){
        if(cur){
            cur.unbind();
            cur_d.parentsUntil(untilSelecter).last().removeClass("opacity");
           
            cur = null;
            cur_d = null;
        }
        $(".__e_bd").remove();
        $(".__e_cmd").remove();        
        $(".__e_frm").remove();
        state = null;
    }
    var cur;
    var cur_d;
    var state;
    $(document).bind("mousemove",function(evt){
        if(!state){
            resetCur();
            var $d = $(evt.target);
            for(var sel in cfg){
                if (matchSel(sel, $d)) {
                    $d = getSel(sel,$d);
                    cur = cfg[sel];
                    cur_d = $d;
                    displayEdirot($d);
                    break;
                }
            }
        }
    });
    $(document).bind("keyup",function(evt){
       if(state && evt.which==27){
           resetCur();
       }
    });
    function editcurr() {
        if(cur){
            var $d = cur_d;
            var $f = cur.edit($d);  
            var frm = $("<div class='__e_frm'>");

            frm.append($f);
            frm.append("<div class='__e_frm_cmd'><a href='javascript:void(0)' class='__e_frm_s'>确定</a><a href='javascript:void(0)' class='__e_frm_c'>取消</a></div>")
            
            if(cur.cmdposition == "pointer"){
                frm.css({"position":"absolute",left:($('.__e_cmd').offset().left + $('.__e_cmd').outerWidth())+"px",top:($('.__e_cmd').offset().top) +"px",zIndex:9999999});
            }
            else{
                frm.css({"position":"absolute",left:$d.offset().left+"px",top:($d.offset().top+$d.outerHeight()+2) +"px",zIndex:9999999});
            }
            frm.find(".__e_frm_s").click(function(){
                cur.commit($d,$f);
                resetCur();
            });
            frm.find(".__e_frm_c").click(resetCur);
            $(document.body).append(frm);
        }
    }
    function deletecurr() {
        var $d = cur_d;
        if(cur)cur.delete($d);
        resetCur();
    }
    function bindcurr(evt){
        state = "bind";
        var $d=cur_d;
        var cmd = $("<div class='__e_cmd'>");    
        if(cur.canEdit){
            cmd.append("<a href='javascript:void(0);' class='__e_cmd_e'>编辑</a>");
        }
        if(cur.canDelete){
            cmd.append("<a href='javascript:void(0);' class='__e_cmd_d'>删除</a>");
        }
        if(cur.getCommands){
            var cmds = cur.getCommands($d);
            if(cmds) cmd.append(cmds);
        }
        cmd.append("<a href='javascript:void(0);' class='__e_cmd_c'>取消</a>");

        $(".__e_bd").css({"z-index":9999999}).addClass("opacity");
       
        cmd.find('.__e_cmd_e').click(editcurr);        
        cmd.find('.__e_cmd_d').click(deletecurr);    
        cmd.find('.__e_cmd_c').click(resetCur);
        if(cur.cmdposition == "pointer"){
            cmd.css({"position":"absolute",left:evt.pageX+"px",top:evt.pageY+"px",zIndex:9999999});
        }
        else{
            cmd.css({"position":"absolute",left:($d.offset().left + 2 + $d.outerWidth())+"px",top:$d.offset().top+"px",zIndex:9999999});
        }
        $(document.body).append(cmd);  
       
        $d.unbind("mousedown",bindcurr);
        cur_d.parentsUntil(untilSelecter).last().removeClass("opacity");
        return false;
    }
    function displayEdirot($d){
        var bd = $("<div class='__e_bd'>");
        bd.css({width:$d.outerWidth(),height:$d.outerHeight(), "position":"absolute",left:$d.offset().left - 2+"px",top:$d.offset().top - 2+"px",zIndex:-1});
       
        //bd.css({left:$d.postion().left - 2+"px",top:$d.postion().top - 2+"px"});  
        //$d.parent().append(bd);
        $(document.body).append(bd);    

        cur_d.parentsUntil(untilSelecter).last().addClass("opacity");
       
        $d.unbind("mousedown",bindcurr);
        $d.bind("mousedown",bindcurr);
    }
})