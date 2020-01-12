<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Program4._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    
    <div class="jumbotron"> <center><h1><b>DJ Wadhwa&#39;s Text Parser</b></h1></center>
        <div>
            <b>
            <asp:TextBox ID="ObjectURL" runat="server" style="font-size: medium" Width="272px" placeholder ="https://prog4storage1.blob.core.windows.net/example/input.txt"></asp:TextBox>
        </b>
            <div>
                <b>
        <asp:Button ID="LoadData" runat="server" OnClick="LoadData_Click" Text="Load Data" Height="44px" Width="134px" style="font-size: medium" />
                <asp:Button ID="ClearData" runat="server" Height="44px" OnClick="ClearData_Click" Text="Clear Data" Width="134px" style="font-size: medium" />
                <asp:Label ID="Message" runat="server" style="font-size: medium"></asp:Label>
                   </b>
                <div>
                <b>
            <asp:TextBox ID="TextBox1" runat="server" Width="134px" placeholder ="First Name" style="font-size: medium"></asp:TextBox>
            <asp:TextBox ID="TextBox2" runat="server" Width="134px" placeholder="Last Name" style="font-size: medium"></asp:TextBox>
        </b>
            <asp:Button ID="Query" runat="server" OnClick="Query_Click" Text="Query" style="font-size: medium" />
                    <br />
                    <asp:Label ID="Output" runat="server" style="font-size: medium; font-weight: 700;"></asp:Label>
                </div>
            </div>
        </div>
    </div>

&nbsp;&nbsp;&nbsp; 

</asp:Content>
