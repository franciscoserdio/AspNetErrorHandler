<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="row">
        <h2>Error chain example, processing an unhandled exception</h2>
        <p>
            Press the button to generate an unhandled exception
        </p>
        <p>
            <asp:Button runat="server" ID="btn_exception" Text="Generate Exception" OnClick="btn_exception_Click" />
        </p>
    </div>
</asp:Content>
