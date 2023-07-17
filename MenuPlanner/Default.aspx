<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="MenuPlanner._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <link rel="stylesheet" href="Calender.css" />

    <div class="calender">
        <asp:Calendar ID="Calendar1" runat="server" Height="80%" Width="60%" OnSelectionChanged="Calendar1_SelectionChanged"></asp:Calendar>
    </div>
    <div id="PersonListContainer">
        <div id="PersonMark" class="personContainer">
            <asp:Label ID="PersonNameMark" runat="server" Text="Mark"></asp:Label>
            <asp:Label ID="MealMark" runat="server" Text="Meal"></asp:Label>
        </div>        
        <div id="PersonSarah" class="personContainer">
            <asp:Label ID="PersonNameSarah" runat="server" Text="Sarah"></asp:Label>
            <asp:Label ID="MealSarah" runat="server" Text="Meal"></asp:Label>
        </div>        
        <div id="PersonThomas" class="personContainer">
            <asp:Label ID="PersonNameThomas" runat="server" Text="Thomas"></asp:Label>
            <asp:Label ID="MealThomas" runat="server" Text="Meal"></asp:Label>
        </div>        
        <div id="PersonWill" class="personContainer">
            <asp:Label ID="PersonNameWill" runat="server" Text="Will"></asp:Label>
            <asp:Label ID="MealWill" runat="server" Text="Meal"></asp:Label>
        </div>        
        <div id="PersonEd" class="personContainer">
            <asp:Label ID="PersonNameEd" runat="server" Text="Ed"></asp:Label>
            <asp:Label ID="MealEd" runat="server" Text="Meal"></asp:Label>
        </div>
    </div>
</asp:Content>
