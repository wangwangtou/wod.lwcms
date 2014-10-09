<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/management/managepage.master" %>
<script runat="server" type="text/C#">
  public override string ajaxCmd
  {
    get
    {
      return "baseupdate";
    }
    set { }
  }

  protected override void PreLoadCommand()
  {
    base.PreLoadCommand();
  }

  protected override void PreCommand()
  {
    //ServerParams.Add("curUser", "xvycn.com");
    base.PreCommand();
  }
</script>
<asp:Content runat="server" ContentPlaceHolderID="head">
  <script src="scripts/management.form.base.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
  <% wodsite site = PD.getObject<wodsite>("ws");
     List<category> allCats = PD.getObject<List<category>>("allCats"); 
  %>
  <script type="text/javascript">
  var siteData = <%=common.ToJson(site) %>;
  siteData.allCats =  <%=common.ToJson(allCats) %>;
  </script>
  <script type="text/javascript">
      $(function () {
        $("body").setFormData(siteData);
      });
  </script>
    <script type="text/javascript">
        $(function () {
            $("body").wodForm({
                schema: _$wod_form.baseSch
                , submit: { selector: "#fsubmit"
                    , event: "click"
                    , posts: { url: "base.aspx", callback : function(){ alert("保存成功");} }
                    , validate : function(data,errorCallback){
                        errorCallback("name","不能为空");
                        return true;
                    }
                    , preSubmit : function(){
                    }
                }
            });
            $("#fcancel").click(function () {
                location.reload();
            });
        });
    </script>
  <div class="g-left">
    <fieldset class="f-form f-form-c2" <%--data-form="subform" subform-name="site" str--%>>
      <legend>基本信息</legend>
      <div class="fr">
        <div class="fr-label"><label for="input1">站点名称：</label></div>
        <div class="fr-input"><input type="text" name="siteName" value="" id="input1" /></div>
      </div>
      <div class="fr">
        <div class="fr-label"><label for="input2">站点链接：</label></div>
        <div class="fr-input"><input type="text" name="siteUrl" value="" id="input2" /></div>
      </div>
      <div class="fr">
        <div class="fr-label"><label for="input4">描述：</label></div>
        <div class="fr-input"><textarea type="text" name="description" id="input4"></textarea></div>
      </div>
      <div class="fr">
        <div class="fr-label"><label for="input6">版权信息：</label></div>
        <div class="fr-input"><textarea type="text" name="copyright" id="input6"></textarea></div>
      </div>
      <div class="fr">
        <div class="fr-label"><label for="input7">关键字：</label></div>
        <div class="fr-input"><input type="text" name="keywords" value="" id="input7" /></div>
      </div>
      <div class="fr">
        <div class="fr-label"><label for="input8">标题：</label></div>
        <div class="fr-input"><input type="text" name="title" value="" id="input8" /></div>
      </div>
      <div class="fr ">
        <div class="fr-label"><label for="input9">导航链接：</label></div>
        <div class="fr-input"><input type="text" wod str name="navis" value="" id="input9" /></div>
      </div>
    </fieldset>
  </div>
  <div class="g-center">
    <div class="cmd-bar"><input id="fcancel" type="button" value="取消"> <input id="fsubmit" type="button" value="保存"> </div>
    <fieldset class="f-form f-sdoc">
      <legend>所有分类</legend>    
      <div class="fr ">
        <div class="fr-input"><input type="text" wod str name="allCats" value="" id="input91" /></div>
      </div>
    </fieldset>
  </div>
</asp:Content>
