<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%
    if (Request.IsAuthenticated) {
%>
        Bienvenido <b><%= Html.Encode(Page.User.Identity.Name) %></b>!
        [ <%= Html.ActionLink("Salir", "LogOff", "Account") %> ][<%= Html.ActionLink("Cambiar Contraseña", "ChangePassword", "Account")%>]
<%
    }
    else {
%> 
        [ <%= Html.ActionLink("Entrar", "LogOn", "Account") %> ]
<%
    }
%>
