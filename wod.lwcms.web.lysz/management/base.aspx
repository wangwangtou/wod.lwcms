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
    base.PreCommand();
  }
</script>
<asp:Content runat="server" ContentPlaceHolderID="head">
  <script type="text/javascript">
    var form;
    $(function () {
      form = new wod.forms.wodform();
      form.init(document.body, siteBaseFields);
      form.registValidate(siteBaseValidate);
      $("#fsubmit").click(form.submit.bind(form,"base.aspx",function(data){
          location.reload();
      }));
      $("#fcancel").click(function () {
          location.reload();
      });
    });
  </script>
    <% wodsite site = PD.getObject<wodsite>("ws");
     List<category> allCats = PD.getObject<List<category>>("allCats"); 
  %>
  <script type="text/javascript">
  var siteData = <%=common.ToJson(site) %>;
  siteData.allCats =  <%=common.ToJson(allCats) %>;
  </script>
  <script type="text/javascript">
    $(function () {
      form.setData(siteData);
    });
  </script>
</asp:Content>
<asp:Content ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
  <div class="g-left">
    <fieldset class="f-form f-form-c2" >
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
        <div class="fr-input"><textarea name="description" id="input4"></textarea></div>
      </div>
      <div class="fr">
        <div class="fr-label"><label for="input6">版权信息：</label></div>
        <div class="fr-input"><textarea name="copyright" id="input6"></textarea></div>
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
        <div class="fr-input"><input type="text" name="navis" value="" id="input9" /></div>
      </div>
    </fieldset>
  </div>
  <div class="g-center">
    <div class="cmd-bar"><input id="fcancel" type="button" value="取消"> <input id="fsubmit" type="button" value="保存"> </div>
    <fieldset class="f-form f-sdoc">
      <legend>所有分类</legend>    
      <div class="fr ">
        <div class="fr-input"><input type="text" name="allCats" value="" id="input91" /></div>
      </div>
    </fieldset>
  </div>
</asp:Content>
