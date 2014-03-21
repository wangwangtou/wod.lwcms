<%@ Page Language="C#" AutoEventWireup="true" 
MasterPageFile="~/management/managepage.master"%>
<script runat="server" type="text/C#">
    public override string loadCmd { get { return string.IsNullOrEmpty(Request.QueryString["id"]) ? "" : "artload"; } set { } }
    public override string ajaxCmd { get { return string.IsNullOrEmpty(Request.QueryString["id"]) ? "artadd" : "artupdate"; } set { } }

    protected override void PreLoadCommand()
    {
        if (!string.IsNullOrEmpty(Request.QueryString["id"]))
        {
            ServerParams.Add("artid", Request.QueryString["id"]);
        }
        base.PreLoadCommand();
    }
    
    protected override void PreCommand()
    {
        if (!string.IsNullOrEmpty(Request.QueryString["id"]))
        {
            ServerParams.Add("artid", Request.QueryString["id"]);
        }
        ServerParams.Add("newGUID", Guid.NewGuid().ToString());
        ServerParams.Add("w_now", DateTime.Now);
        ServerParams.Add("w_nowstring", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
        ServerParams.Add("w_today", DateTime.Today);
        ServerParams.Add("curUser", "xvycn.com");
        base.PreCommand();
    }
</script>
<asp:Content runat="server" ContentPlaceHolderID="head">
</asp:Content>
<asp:Content ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<%article art = PD==null? null : PD.getObject<article>("art");
  if (art != null)
  {%>
    <script type="text/javascript">var artData = <%=wod.lwcms.common.ToJson(art) %>;
    artData.cat = artData.category.fullpath;</script>
    <script type="text/javascript">
        $(function () {
            $("body").loadFormData(artData);
        });
    </script>
<% }%>
    <script type="text/javascript">
        $(function () {
            $("body").wodForm({
                schema: _$wod_form.artSch
                , submit: { selector: "#fsubmit"
                    , event: "click"
                    , settings: { url: "artadd.aspx?id=<%=Request.QueryString["id"] %>", callback : function(){ alert("保存成功");} }
                }
            });
            $("#fcancel").click(function () {
                location.reload();
            });
        });
    </script>
<div class="g-left">
    <fieldset class="f-form f-form-c2">
        <legend>基本信息</legend>
        <div class="fr">
                <div class="fr-label"><label for="input1">标题：</label></div>
                <div class="fr-input"><input type="text" name="name" value="" id="input1" /></div>
            </div>
            <div class="fr">
                <div class="fr-label"><label for="input2">代码：</label></div>
                <div class="fr-input"><input type="text" name="code" value=""  id="input2" /></div>
            </div>
        <div class="fr">
                <div class="fr-label"><label for="input4">概要：</label></div>
                <div class="fr-input"><textarea type="text" name="preContent"  id="input4"></textarea></div>
        </div>
        <div class="fr">
                <div class="fr-label"><label for="input6">描述：</label></div>
                <div class="fr-input"><textarea type="text" name="description"  id="input6"></textarea></div>
        </div>
        <div class="fr">
                <div class="fr-label"><label for="input5">分类：</label></div>
                <div class="fr-input"><input type="text" name="cat" value="" id="input5" /></div>
            </div>
            <div class="fr">
                <div class="fr-label"><label for="input7">关键字：</label></div>
                <div class="fr-input"><input type="text" name="keywords" value="" id="input7" /></div>
            </div>
        <div class="fr">
                <div class="fr-label"><label for="input8">版式：</label></div>
                <div class="fr-input"><input type="text" name="page" value="" id="input8" /></div>
        </div>
        <div class="fr ">
                <div class="fr-label"><label for="input9">关联图片：</label></div>
                <div class="fr-input"><input type="text" name="image" value="" id="input9" /></div>
        </div>
    </fieldset>
</div>
<div class="g-center">
    <div class="cmd-bar">
        <input id="fcancel" type="button" value="取消">
        <input id="fsubmit" type="button" value="保存">
    </div>
    <fieldset class="f-form f-sdoc">
        <legend>内容</legend>
        <textarea name="content"></textarea>
    </fieldset>
</div>
</asp:Content>