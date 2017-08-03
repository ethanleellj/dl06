<%@ page title="" language="C#" masterpagefile="~/ProCode/RptPage.master" autoeventwireup="true" inherits="RptElectronicScaleDataProcessing_qqj" CodeFile="~/ProCode/Rtm/RptElectronicScaleDataProcessing_qqj.aspx.cs" %>

<%@ Register Src="~/Controls/UCBranchNo.ascx" TagName="UCBranchNo" TagPrefix="uc1" %>
<%@ Register Src="~/Controls/UCBaseCode.ascx" TagName="UCBaseCode" TagPrefix="uc2" %>
<%@ Register Src="~/Controls/UCCalendar.ascx" TagName="UCCalendar" TagPrefix="uc3" %>
<%@ Register Src="~/Controls/UCBranchNo.ascx" TagName="UCBranchNo" TagPrefix="uc4" %>
<%@ Register Src="~/Controls/UCOperDateWithTime.ascx" TagName="UCOperDateWithTime"
    TagPrefix="uc3" %>
<%@ Register Assembly="SISS.WebControl" Namespace="SISS.WebControl" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="rptQuery" runat="Server">
    <script src="<%=ResolveUrl("~") %>JS/Calender/WdatePicker.js" type="text/javascript"></script>
    <script src="<%=ResolveUrl("~") %>JS/UCCommon.js" type="text/javascript"></script>
    <script src="<%=ResolveUrl("~") %>JS/Stm/RptStockQuery.js" type="text/javascript"></script>

    <fieldset>
    <legend style="font-weight: 700;"></legend>
       &nbsp;&nbsp;&nbsp;&nbsp; <asp:Button ID="btnProcessingData" runat="server" Text="数据处理" CssClass="PtButton"  OnClick="btnProcessingData_Click" />

     &nbsp;&nbsp;&nbsp;&nbsp; <asp:CheckBox ID="CheckBoxGenerateSaleFlow" runat="server" Text="生成零售流水" Visible="false"  Checked="false"/>
     &nbsp;&nbsp;&nbsp;&nbsp; <asp:CheckBox ID="CheckBoxGenerateDO" runat="server" Text="生成直调出库单"   Visible="false"  Checked="false"/>
     &nbsp;&nbsp;&nbsp;&nbsp; <asp:CheckBox ID="CheckBoxGeneratePI" runat="server" Text="生成采购收货单"  Visible="false"  Checked="false" />
        <asp:HiddenField ID="sGenerateSheet" runat="server" />
    </fieldset>

    <table style11="display:none" id="SheetWhere" border="0" cellpadding="0" cellspacing="0">
        <tr>
            <td align="right" style="width: 70px;">
                日&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;期：
            </td>
            <td align="left" colspan="7">
                <uc3:UCOperDateWithTime ID="ucOperDateStart" runat="server" TwoDate="true" />
            </td>
        </tr>
        <tr  style="display:none" >
            <td align="right" style="width: 70px;">
                供&nbsp;&nbsp;应&nbsp;&nbsp;&nbsp;商：
            </td>
            <td align="left">
                <uc2:UCBaseCode ID="ucSuppliert" runat="server" UCBaseType="Supplier" TwoShow="true"
                    noWidth="70" nameWidth="172" />
            </td>
            <td align="right" style="width: 70px;" colspan="2">
                收&nbsp;&nbsp;银&nbsp;&nbsp;员：<asp:TextBox ID="txtGMMan" runat="server" Width="40px"
                    class="TextBox"></asp:TextBox>
                POS机号：<asp:TextBox ID="txtPosNo" runat="server" Width="40px" class="TextBox"></asp:TextBox>
            </td>
            <td align="right" style="width: 70px;">
                营&nbsp;&nbsp;业&nbsp;&nbsp;员：
            </td>
            <td align="left">
                <uc2:UCBaseCode ID="txtSaleman" runat="server" UCBaseType="Saleman" TwoShow="false"
                    noWidth="125" nameWidth="112" />
            </td>
            <td align="right" style="width: 70px;">
            </td>
            <td align="left">
            </td>
        </tr>
        <tr style="display:none" >
            <td align="right" style="width: 70px;">
                分店/仓库：
            </td>
            <td align="left">
                <uc1:UCBranchNo ID="ucWarehouse" runat="server" BranchCls="All" UCBaseType="Branch"
                    noWidth="70" nameWidth="172" />
            </td>
            <td align="right" style="width: 70px;">
                会员卡号：
            </td>
            <td align="left">
                <asp:TextBox ID="txtVipNo" runat="server" Width="148px" class="TextBox"></asp:TextBox>
            </td>
            <td align="right" style="width: 70px;">
                区&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;域：
            </td>
            <td align="left">
                <uc4:UCBranchNo ID="txtBranchArea" runat="server" BranchCls="All" UCBaseType="BranchArea"
                    TwoShow="false" noWidth="125" nameWidth="0" />
            </td>
            <td align="right" style="width: 70px;">
            </td>
            <td align="left">
            </td>
        </tr>
        <tr style="display:none" >
            <td align="right" style="width: 70px;">
                单&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;号：
            </td>
            <td align="left">
                <asp:TextBox ID="txtSheetNo" runat="server" Width="269px" class="TextBox"></asp:TextBox>
            </td>
            <td align="right" style="width: 70px;">
                类&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;别：
            </td>
            <td align="left">
                <uc2:UCBaseCode ID="ucItemCls" runat="server" UCBaseType="ItemCls" TwoShow="false"
                    noWidth="126" nameWidth="0" />
            </td>
            <td align="right" style="width: 70px;">
                品&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;牌：
            </td>
            <td align="left">
                <uc2:UCBaseCode ID="txtBrand" runat="server" UCBaseType="TrandMark" TwoShow="false"
                    noWidth="125" nameWidth="70" />
            </td>
            <td align="right" style="width: 70px;">
            </td>
            <td align="left">
            </td>
        </tr>
        <tr style="display:none" >
            <td align="right" style="width: 70px;">
                货&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;号：
            </td>
            <td align="left">
                <asp:TextBox ID="txtItemNo" runat="server" CssClass="sheettextno" Width="247"></asp:TextBox><asp:ImageButton
                    ID="ImageButton2" ImageUrl="~/Images/comm_Search.png" class="ImgShow" onmouseover="this.style.cursor='hand'"
                    OnClientClick="ShowItemSelectWindow('txtItemNo','txtItemName');return false;"
                    runat="server" /><asp:TextBox ID="TextBox2" Style="display: none" runat="server"
                        CssClass="sheettextname" Width="110" ReadOnly="true" ForeColor="Silver"></asp:TextBox>
            </td>
            <td align="right" style="width: 70px;">
                商品名称：
            </td>
            <td align="left">
                <asp:TextBox ID="txtItemName" runat="server" Width="148px" class="TextBox"></asp:TextBox>
            </td>
            <td align="right" style="width: 70px;">
                销售方式：
            </td>
            <td align="left">
                <asp:DropDownList ID="ddlSaleWay" Width="154px" runat="server">
                </asp:DropDownList>
            </td>
            <td align="right" style="width: 70px;">
            </td>
            <td align="left">
            </td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="rptDetail" runat="Server">
    <div id="div_RootHeader">
    </div>
    <div style="width: auto; overflow: auto; height: inherit;" id="SheetDetailDiv">
        <cc1:GridViewEx ID="gvRpt" runat="server" AutoGenerateColumns="False" AllowPaging="True"
            AllowSorting="true" PageSize="50" IsEidt="false" CellPadding="0" GridLines="None"
            DivID="gvDiv" ShowFooter="true" CssClassHeaderCell="gvHeaderRow" CssClassMouseOver="OverRow"
            CssClass="detalist" RowClentClick="fn_RowClient" RaiseRowDataBound="True" OnBind="gvRpt_Bind"
            IsRootHeader="true" OnRowDataBound="gvRpt_RowDataBound">
            <RowStyle CssClass="Row" />
            <SelectedRowStyle CssClass="OverRow" />
            <AlternatingRowStyle CssClass="AlternatingRow" />
            <PagerStyle CssClass="PagerRow" />
            <Columns>
                <cc1:TemplateFieldEx ColName="row_number" HeaderStyle-Width="35px" ColFlag="1" HeaderText="行号">
                </cc1:TemplateFieldEx>



               <cc1:BoundFieldEx ColName="sheet_no_qqj" DataField="sheet_no_qqj" HeaderStyle-Width="140px"
                    HeaderText="生成单据号" SortExpression="sheet_no_qqj" ItemStyle-HorizontalAlign="Left">
                </cc1:BoundFieldEx> 

                <cc1:BoundFieldEx ColName="StoreTypeName" DataField="StoreTypeName" HeaderStyle-Width="60px"
                    HeaderText="业务类型" SortExpression="StoreTypeName" ItemStyle-HorizontalAlign="Left">
                </cc1:BoundFieldEx> 

                <cc1:BoundFieldEx ColName="TradeTime" DataField="TradeTime" HeaderStyle-Width="130px"
                    HeaderText="交易时间"  GlobalFormat="DateTime" SortExpression="TradeTime" ItemStyle-HorizontalAlign="Left">
                </cc1:BoundFieldEx> 
                <cc1:BoundFieldEx ColName="SubShopNo" DataField="SubShopNo" HeaderStyle-Width="100px"
                    HeaderText="门店 / 供应商" SortExpression="SubShopNo" ItemStyle-HorizontalAlign="Left">
                </cc1:BoundFieldEx> 
                <cc1:BoundFieldEx ColName="PluNo" DataField="PluNo" HeaderStyle-Width="80px"
                    HeaderText="货号" SortExpression="PluNo" ItemStyle-HorizontalAlign="Left">
                </cc1:BoundFieldEx> 
                <cc1:BoundFieldEx ColName="PluName" DataField="PluName" HeaderStyle-Width="120px"
                    HeaderText="品名" SortExpression="PluName" ItemStyle-HorizontalAlign="Left">
                </cc1:BoundFieldEx> 
                <cc1:BoundFieldEx ColName="UnitPrice" DataField="UnitPrice" HeaderStyle-Width="60px"
                    HeaderText="单价" SortExpression="UnitPrice" ItemStyle-HorizontalAlign="right"
                    GlobalFormat="Price" >
                </cc1:BoundFieldEx>
                <cc1:BoundFieldEx ColName="Quantity" DataField="Quantity" HeaderStyle-Width="60px"
                    HeaderText="数量" SortExpression="Quantity" ItemStyle-HorizontalAlign="right"
                    GlobalFormat="Quantity" IsSum="true">
                </cc1:BoundFieldEx>
                <cc1:BoundFieldEx ColName="Weight" DataField="Weight" HeaderStyle-Width="60px"
                    HeaderText="重量" SortExpression="Weight" ItemStyle-HorizontalAlign="right"
                    GlobalFormat="Quantity" IsSum="true">
                </cc1:BoundFieldEx>

                <cc1:BoundFieldEx ColName="TotalPrice" DataField="TotalPrice" HeaderStyle-Width="70px"
                    HeaderText="小计金额" SortExpression="TotalPrice" ItemStyle-HorizontalAlign="right"
                    GlobalFormat="Money"   IsSum="true">
                </cc1:BoundFieldEx>


                <cc1:BoundFieldEx ColName="TicketNo" DataField="TicketNo" HeaderStyle-Width="60px"
                    HeaderText="单据号" SortExpression="TicketNo" ItemStyle-HorizontalAlign="Left">
                </cc1:BoundFieldEx> 
                <cc1:BoundFieldEx ColName="SubNo" DataField="SubNo" HeaderStyle-Width="40px"
                    HeaderText="序号" SortExpression="SubNo" ItemStyle-HorizontalAlign="Left">
                </cc1:BoundFieldEx> 

                <cc1:BoundFieldEx ColName="OperatorNo" DataField="OperatorNo" HeaderStyle-Width="70px"
                    HeaderText="操作员编号" SortExpression="OperatorNo" ItemStyle-HorizontalAlign="Left">
                </cc1:BoundFieldEx> 

                <cc1:BoundFieldEx ColName="IPPoint" DataField="IPPoint" HeaderStyle-Width="120px"
                    HeaderText="IP地址" SortExpression="IPPoint" ItemStyle-HorizontalAlign="Left">
                </cc1:BoundFieldEx>

                <cc1:BoundFieldEx ColName="ScaleType" DataField="ScaleType" HeaderStyle-Width="80px" 
                    HeaderText="电子秤类型" SortExpression="ScaleType" ItemStyle-HorizontalAlign="Left">
                </cc1:BoundFieldEx>  

                <cc1:BoundFieldEx ColName="FlowNo" DataField="FlowNo" HeaderStyle-Width="70px"   ColVisible="false"
                    HeaderText="FlowNo" SortExpression="FlowNo" ItemStyle-HorizontalAlign="Right">
                </cc1:BoundFieldEx>





                <cc1:BoundFieldEx ColName="DeviceID" DataField="DeviceID" HeaderStyle-Width="120px" ColVisible="false"
                    HeaderText="DeviceID" SortExpression="DeviceID" ItemStyle-HorizontalAlign="Left">
                </cc1:BoundFieldEx>          

                <cc1:BoundFieldEx ColName="ScaleNo" DataField="ScaleNo" HeaderStyle-Width="120px" ColVisible="false"
                    HeaderText="ScaleNo" SortExpression="ScaleNo" ItemStyle-HorizontalAlign="Left">
                </cc1:BoundFieldEx>          
   



                <cc1:BoundFieldEx ColName="ArticleNo" DataField="ArticleNo" HeaderStyle-Width="120px" ColVisible="false"
                    HeaderText="ArticleNo" SortExpression="ArticleNo" ItemStyle-HorizontalAlign="Left">
                </cc1:BoundFieldEx> 







               <cc1:BoundFieldEx ColName="memo_qqj" DataField="memo_qqj" HeaderStyle-Width="120px" ColVisible="false"
                    HeaderText="memo_qqj" SortExpression="memo_qqj" ItemStyle-HorizontalAlign="Left">
                </cc1:BoundFieldEx> 
   

           
            </Columns>
        </cc1:GridViewEx>
    </div>
  
  <!--小票预览 引入小票预览页面-->
    <!--#include File="../../posPreview.htm"-->
</asp:Content>
