<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/management/managepage.master" %>
<script runat="server" type="text/C#">
  public override string ajaxCmd
  {
    get
    {
      return "attributeupdate";
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
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="head">
</asp:Content>
<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="ContentPlaceHolder1">
</asp:Content>