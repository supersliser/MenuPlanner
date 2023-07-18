<%@ Page Async="true" Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="MenuPlanner._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <link rel="stylesheet" href="Calender.css" />

    <div class="calender">
        <asp:Calendar ID="Calendar1" runat="server" Height="80%" Width="60%" OnSelectionChanged="Calendar1_SelectionChanged" />
    </div>
    <div id="MealContainer">
            <asp:Label ID="MealItem" runat="server" Text="Meal"></asp:Label>
    </div>
</asp:Content>
