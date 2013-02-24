<%@ Control Language="C#" Inherits="BeenJammin.SharePoint.WebControls.ChildDropDownListFieldEditControl,BeenJammin.SharePoint.CascadingLookups, Version=1.0.0.0, Culture=neutral, PublicKeyToken=882d387e76532c02"   AutoEventWireup="false" %>
<%@ Register TagPrefix="wssuc" TagName="InputFormControl" src="~/_controltemplates/InputFormControl.ascx" %>
<%@ Register TagPrefix="wssuc" TagName="InputFormSection" src="~/_controltemplates/InputFormSection.ascx" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Import Namespace="Microsoft.SharePoint" %>
<wssuc:InputFormSection runat="server" id="MySections" Title="Child Drop Down List Details">
       <Template_InputFormControls>
             <wssuc:InputFormControl runat="server"
                    LabelText="Enter a SharePoint site URL (in this Farm)then click the Load Lists button. Select a List and Column in the remaining dropdowns">
                    <Template_Control>
                        <asp:Label ID="lblSiteURL" runat="server" Text="Site URL" Width="120px">
                        </asp:Label>                           
                        <br />
                        <asp:TextBox ID="txtSiteURL" runat="server"  Width="350px" Text="">
                        </asp:TextBox>  
                        <br />
                        <asp:Button ID="btnLoadLists" runat="server" Text="Load Lists" />                              
                        <br />
                        <asp:Label ID="lblList" runat="server" Text="List Name" Width="120px" >
                        </asp:Label>
                        <br />
                        <asp:DropDownList ID="ddlLists" runat="server" Width="170px" >
                        </asp:DropDownList>
                        <br />
                        <asp:Label ID="lblColumnValue" runat="server" Text="Column For Value" Width="120px">
                        </asp:Label>
                        <br />
                        <asp:DropDownList ID="ddlColumnValue" runat="server" Width="170px" >
                        </asp:DropDownList>
                        <br />
                        <asp:Label ID="lblColumnText" runat="server" Text="Column For Text" Width="120px">
                        </asp:Label>
                        <br />
                        <asp:DropDownList ID="ddlColumnText" runat="server" Width="170px" >
                        </asp:DropDownList>
                        <br />
                        <asp:Label ID="lblColumnJoin" runat="server" Text="Column to Join to Parent" Width="120px">
                        </asp:Label>
                        <br />
                        <asp:DropDownList ID="ddlColumnJoin" runat="server" Width="170px" >
                        </asp:DropDownList>
                        <br />
                    </Template_Control>
             </wssuc:InputFormControl>
       </Template_InputFormControls>
</wssuc:InputFormSection>

