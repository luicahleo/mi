<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<System.Web.Mvc.HandleErrorInfo>" %>

<asp:Content ID="errorHead" ContentPlaceHolderID="head" runat="server">
    <title>Error</title>
</asp:Content>

<asp:Content ID="errorContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2>
        Lo sentimos. Ocurrio un error al procesar su solicitud.
    </h2>
</asp:Content>
