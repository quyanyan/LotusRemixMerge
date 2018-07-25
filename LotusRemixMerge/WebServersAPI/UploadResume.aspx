<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UploadResume.aspx.cs" Inherits="WebServersAPI.UploadResume" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:TextBox ID="txtUpload" runat="server"></asp:TextBox>
            <asp:TextBox ID="txtUpload1" runat="server"></asp:TextBox>

            <asp:Button ID="Button1" runat="server" Text="upload" OnClick="Button1_Click" />
            <asp:Button ID="Button2" runat="server" Text="reversal" OnClick="Button2_Click" />
        </div>
        <div>

            <input type="file" id="fileSelector" name="fileSelector" runat="server" />
            <input type="button" id="btnUpload" value="上传" />

        </div>
        <div>
            <p>服务器端控件上传</p>
            <asp:FileUpload ID="MyFileUpload" runat="server" />
            <asp:Button ID="FileUploadButton" runat="server" Text="上传" OnClick="FileUploadButton_Click" />
            <asp:Button ID="Button3" runat="server" Text="解析" OnClick="Button3_Click" />
        </div>
        <div>
            <p>使用Html的Input标签上传</p>
            <input type="file" name="MyFileUploadInput" runat="server" />
            <asp:Button ID="InputFileUploadButton" runat="server" Text="上传" OnClick="InputFileUploadButton_Click" />
        </div>

    </form>
</body>
</html>


