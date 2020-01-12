<%@ Page Title="About" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="About.aspx.cs" Inherits="Program4.About" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2><%: Title %>.</h2>
    <h3>Load, Clear, and Query Data from Object Storage.</h3>
    <p>This application was built by DJ Wadhwa. It uses the .NET framwork to load data from object storage, query it using a NoSQL database and clear the data by removing it from Object Storage.</p>
<p>&nbsp;</p>
<p>Blob storage information can be accessed here: <a href="https://prog4storage1.blob.core.windows.net/prog4blobcontainer/?restype=container&amp;comp=list">https://prog4storage1.blob.core.windows.net/prog4blobcontainer/?restype=container&amp;comp=list</a></p>
</asp:Content>
