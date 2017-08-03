<%@ page title="" language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" inherits="ProCode_Pur_PurchaseGuide2_qqj" CodeFile="~/ProCode/Pur/PurchaseGuide2_qqj.aspx.cs" %>

<%@ Register Assembly="SISS.WebControl" Namespace="SISS.WebControl" TagPrefix="cc1" %>
<%@ Register Src="~/Controls/UCBranchNo.ascx" TagName="UCBranchNo" TagPrefix="uc1" %>
<%@ Register Src="~/Controls/UCBaseCode.ascx" TagName="UCBaseCode" TagPrefix="uc2" %>
<%@ Register Src="~/Controls/UCCalendar.ascx" TagName="UCCalendar" TagPrefix="uc3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MPContent" runat="Server">
    <script src="<%=ResolveUrl("~") %>JS/UCCommon.js" type="text/javascript"></script>
    <script src="<%=ResolveUrl("~") %>JS/Calender/WdatePicker.js" type="text/javascript"></script>
    <script src="../../JS/Pur/PurchaseGuide.js" type="text/javascript"></script>
    <script type="text/javascript">
        function bind() {
            __doPostBack('btnNext', '');
        }
    </script>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="tbl">
                <asp:TextBox ID="txtlevel" runat="server" Text="1" Style="display: none;"></asp:TextBox>
                <asp:TextBox ID="hidSHEET_NO" Style="display: none" runat="server"></asp:TextBox>
                <asp:TextBox ID="hidCurrSelectedRowId" runat="server" Style="display: none;"></asp:TextBox>
                <asp:TextBox ID="hidSelectedRowId" runat="server" Style="display: none;"></asp:TextBox>
                <asp:TextBox ID="hidItemBarcode" runat="server" Style="display: none;"></asp:TextBox>
                <div id="dicbasic">
                    <asp:Image ID="Image8" runat="server" ImageUrl="~/Images/flow_SmlToolbar_blackicon.gif"
                        CssClass="sheetTitilImg" />
                    <span style="font-weight: bold">&nbsp;&nbsp;&nbsp;<asp:Label ID="lblHead" runat="server"
                        Text="采购向导"></asp:Label></span></div>
                <div id="DivSetup" runat="server">
                    <div class="MessageList" style="height: 450px">
                        <table>
                            <tr>
                                <td align="right" class="TdName">
                                    分店要货日期：
                                </td>
                                <td align="left" class="TdValue">
                                    <uc3:UCCalendar ID="txtBranchDate" runat="server" TwoDate="true" BeginWidth="80"
                                        EndWidth="80" />
                                </td>
                            </tr>
                            <tr style="display:none;">
                                <td align="right" class="TdName">
                                    销&nbsp; 售&nbsp; 日&nbsp; 期 ：
                                </td>
                                <td align="left" class="TdValue">
                                    <uc3:UCCalendar ID="txtSaleDate" runat="server" TwoDate="true" BeginWidth="80" EndWidth="80" />
                                </td>
                            </tr>
                            <tr>
                                <td align="right" class="TdName">
                                    发&nbsp;货&nbsp;分&nbsp;店：
                                </td>
                                <td align="left" class="TdValue">
                                    <uc1:UCBranchNo ID="txtBranchNo" runat="server" BranchCls="AllBranchOrAll" nameWidth="96"
                                        noWidth="70" PageName="Connect" UCBaseType="Branch" />
                                </td>
                                <tr>
                                    <td align="right" class="TdName">
                                        类&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;别：
                                    </td>
                                    <td align="left" class="TdValue">
                                        <uc2:UCBaseCode ID="ucItemCls" runat="server" nameWidth="96" noWidth="70" UCBaseType="ItemCls" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" class="TdName">
                                        品&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;牌：
                                    </td>
                                    <td align="left" class="TdValue">
                                        <uc2:UCBaseCode ID="ucItemBrand" runat="server" nameWidth="96" noWidth="70" UCBaseType="TrandMark" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" class="TdName">
                                        供&nbsp;&nbsp;&nbsp;应&nbsp;&nbsp;&nbsp;商：
                                    </td>
                                    <td align="left" class="TdValue">
                                        <uc2:UCBaseCode ID="txtSupNo" runat="server" UCBaseType="Supplier" noWidth="70" nameWidth="96" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" class="TdName">
                                        采购数量默认参考值：
                                    </td>
                                    <td align="left" class="TdValue">
                                        <span>
                                            <asp:RadioButtonList ID="rdoCheck" runat="server" AutoPostBack="true" OnSelectedIndexChanged="rdoCheck_SelectedIndexChanged"
                                                RepeatDirection="Horizontal">
                                                <%--<asp:ListItem>按库存存量指标采购</asp:ListItem>--%>
                                                <asp:ListItem Selected="True">按分店要货量采购</asp:ListItem>       <%--qqjMark--%>
                                                <%--<asp:ListItem>按销量采购</asp:ListItem>--%>
                                                <%--<asp:ListItem>按批发订单采购</asp:ListItem>--%>
                                                <%--<asp:ListItem>按补货分析采购</asp:ListItem>--%>
                                            </asp:RadioButtonList>
                                        </span>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                    </td>
                                    <td style="color: Blue;">
                                        <%--<span>按库存存量指标采购：根据当前库存量低于库存存量指标的下限进行补货</span><br />--%>
                                        <span>按门店要货采购：根据根据门店要货单的要货数量，直接生成采购订单的采购数量，而不考虑当前库存量</span><br />
                                      <%--  <span>按销量采购：根据当前销售数量大于0，且库存低于销售数量进行补货</span><br />
                                        <span>按批发订单采购：根据按未销售的销售订单进行补货</span><br />
                                        <span>按补货分析采购：根据补货分析报表里建议数量大于0的商品数量进行补货</span>--%>
                                    </td>
                                </tr>
                            </tr>
                        </table>
                    </div>
                </div>
                <div id="DivSetup1" class="MessageList" visible="false" runat="server" style="overflow: auto;">
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div id="div_RootHeader" style="">
                            </div>
                            <div class="MessageList" id="SheetDetailDiv" style="width: 99.7%; height: 407px">
                                <div style="float: left; width: 99.7%;">
                                    <!--商品详细信息-->
                                    <cc1:GridViewEx ID="gvDetail" runat="server" AutoGenerateColumns="False" AllowSorting="true"
                                        PageSize="8" IsEidt="true" CellPadding="0" GridLines="None" DivID="gvDiv" CssClassHeaderCell="gvHeaderRow"
                                        CssClassMouseOver="OverRow" CssClass="detalist" IsRootHeader="true" Height="100%"
                                        RaiseRowDataBound="True" RowClentClick="fn_RowClient1" OnCellEdit="gvDetail_CellEdit"
                                        OnBind="gvDetail_Bind">
                                        <RowStyle CssClass="Row" />
                                        <SelectedRowStyle CssClass="OverRow" />
                                        <AlternatingRowStyle CssClass="AlternatingRow" />
                                        <PagerStyle CssClass="PagerRow" />
                                        <Columns>
                                            <cc1:TemplateFieldEx ColName="row_number" HeaderStyle-Width="35px" ColFlag="1" HeaderText="行号">
                                            </cc1:TemplateFieldEx>
                                            <cc1:BoundFieldEx ColName="branch_no" DataField="branch_no" HeaderStyle-Width="120px"
                                                HeaderText="分店仓库" SortExpression="branch_no" ItemStyle-HorizontalAlign="Left">
                                            </cc1:BoundFieldEx>
                                            <cc1:BoundFieldEx ColName="item_no" DataField="item_no" HeaderStyle-Width="180px"
                                                HeaderText="货号" SortExpression="item_no" ItemStyle-HorizontalAlign="Left">
                                            </cc1:BoundFieldEx>
                                            <cc1:BoundFieldEx ColName="item_name" DataField="item_name" HeaderStyle-Width="130px"
                                                HeaderText="品名" SortExpression="item_name" ItemStyle-HorizontalAlign="Left">
                                            </cc1:BoundFieldEx>
                                            <cc1:BoundFieldEx ColName="unit_no" DataField="unit_no" HeaderStyle-Width="60px"
                                                HeaderText="单位" SortExpression="unit_no" ItemStyle-HorizontalAlign="Left">
                                                <HeaderStyle Width="60px"></HeaderStyle>
                                            </cc1:BoundFieldEx>
                                            <cc1:BoundFieldEx ColName="item_size" DataField="item_size" HeaderStyle-Width="60px"
                                                HeaderText="规格" SortExpression="item_size" ItemStyle-HorizontalAlign="Left">
                                                <HeaderStyle Width="60px"></HeaderStyle>
                                            </cc1:BoundFieldEx>
                                            <cc1:EditCellField ColName="qty" DataField="qty" HeaderText="采购数量" ItemStyle-HorizontalAlign="Right"
                                                HeaderStyle-Width="90px" SortExpression="qty" GlobalFormat="Quantity">
                                            </cc1:EditCellField>
                                            <cc1:EditCellField ColName="large_qty" DataField="large_qty" HeaderText="采购箱数" ItemStyle-HorizontalAlign="Right"
                                                HeaderStyle-Width="90px" SortExpression="large_qty" GlobalFormat="Quantity">
                                            </cc1:EditCellField>
                                            <cc1:BoundFieldEx ColName="real_qty" DataField="real_qty" HeaderStyle-Width="90px"
                                                HeaderText="要货数量" SortExpression="real_qty" ItemStyle-HorizontalAlign="Right"
                                                GlobalFormat="Quantity">
                                                <HeaderStyle Width="90px"></HeaderStyle>
                                            </cc1:BoundFieldEx>
                                            <cc1:BoundFieldEx ColName="stock_qty" DataField="stock_qty" HeaderStyle-Width="90px"
                                                HeaderText="库存指标" SortExpression="stock_qty" ItemStyle-HorizontalAlign="Right"
                                                GlobalFormat="Quantity">
                                                <HeaderStyle Width="90px"></HeaderStyle>
                                            </cc1:BoundFieldEx>
                                            <cc1:BoundFieldEx ColName="sale_qnty" DataField="sale_qnty" HeaderStyle-Width="90px"
                                                HeaderText="销售数量" SortExpression="sale_qnty" ItemStyle-HorizontalAlign="Right"
                                                GlobalFormat="Quantity">
                                                <HeaderStyle Width="90px"></HeaderStyle>
                                            </cc1:BoundFieldEx>
                                            <cc1:BoundFieldEx ColName="allstock_qty" DataField="allstock_qty" HeaderStyle-Width="90px"
                                                HeaderText="本机构库存" SortExpression="allstock_qty" ItemStyle-HorizontalAlign="Right"
                                                GlobalFormat="Quantity">
                                                <HeaderStyle Width="90px"></HeaderStyle>
                                            </cc1:BoundFieldEx>
                                            <cc1:BoundFieldEx ColName="main_supcusts" DataField="main_supcusts" HeaderStyle-Width="120px"
                                                HeaderText="供应商编码" SortExpression="main_supcusts" ItemStyle-HorizontalAlign="Left"
                                                ColVisible="false">
                                                <HeaderStyle Width="120px"></HeaderStyle>
                                            </cc1:BoundFieldEx>
                                            <cc1:BoundFieldEx ColName="main_supcust" DataField="main_supcust" HeaderStyle-Width="120px"
                                                HeaderText="供应商名称" SortExpression="main_supcust" ItemStyle-HorizontalAlign="Left">
                                                <HeaderStyle Width="120px"></HeaderStyle>
                                            </cc1:BoundFieldEx>
                                            <cc1:BoundFieldEx ColName="valid_price" DataField="valid_price" HeaderStyle-Width="90px"
                                                ColVisible="false" HeaderText="进价" SortExpression="valid_price" ItemStyle-HorizontalAlign="Right"
                                                PriceGrant="InPrice" GlobalFormat="Money">
                                                <HeaderStyle Width="90px"></HeaderStyle>
                                            </cc1:BoundFieldEx>
                                            <cc1:BoundFieldEx ColName="sale_price" DataField="sale_price" HeaderStyle-Width="90px"
                                                ColVisible="false" HeaderText="售价" SortExpression="sale_price" ItemStyle-HorizontalAlign="Right"
                                                PriceGrant="SalePrice" GlobalFormat="Money">
                                                <HeaderStyle Width="90px"></HeaderStyle>
                                            </cc1:BoundFieldEx>
                                            <cc1:BoundFieldEx ColName="sub_amt" DataField="sub_amt" HeaderStyle-Width="90px"
                                                PriceGrant="InPrice" ColVisible="false" HeaderText="单据金额" SortExpression="sub_amt"
                                                ItemStyle-HorizontalAlign="Right" GlobalFormat="Money">
                                                <HeaderStyle Width="90px"></HeaderStyle>
                                            </cc1:BoundFieldEx>
                                            <cc1:BoundFieldEx ColName="purchase_spec" DataField="purchase_spec" HeaderStyle-Width="90px"
                                                ColVisible="false" HeaderText="规格" SortExpression="purchase_spec" ItemStyle-HorizontalAlign="Right">
                                                <HeaderStyle Width="90px"></HeaderStyle>
                                            </cc1:BoundFieldEx>
                                        </Columns>
                                    </cc1:GridViewEx>
                                    <!--订单信息列表-->
                                    <cc1:GridViewEx ID="gvOrderList" runat="server" AutoGenerateColumns="False" AllowPaging="True"
                                        AllowSorting="true" PageSize="15" IsEidt="false" CellPadding="4" GridLines="None"
                                        DivID="gvDiv" CssClassHeaderCell="gvHeaderRow" CssClassMouseOver="OverRow" IsRootHeader="true"
                                        CssClass="detalist" RowClentClick="fn_RowClient" PriceGrantColumns="" Height="100%"
                                        RaiseRowDataBound="True" OnBind="gvOrderList_Bind" OnRowDataBound="gvOrderList_RowDataBound">
                                        <RowStyle CssClass="Row" />
                                        <SelectedRowStyle CssClass="OverRow" />
                                        <AlternatingRowStyle CssClass="AlternatingRow" />
                                        <PagerStyle CssClass="PagerRow" />
                                        <Columns>
                                            <cc1:TemplateFieldEx ColName="row_id" ColFlag="1" HeaderStyle-Width="35px" HeaderText="行号">
                                            </cc1:TemplateFieldEx>
                                            <cc1:HyperLinkFieldEx ColName="SHEET_NO" DataTextField='SHEET_NO' HeaderText="单号"
                                                HeaderStyle-Width="130px" SortExpression="SHEET_NO" ItemStyle-HorizontalAlign="Left"
                                                ItemStyle-Font-Bold="true">
                                            </cc1:HyperLinkFieldEx>
                                            <cc1:BoundFieldEx ColName="BRANCH_NO" DataField="BRANCH_NO" HeaderText="仓库" HeaderStyle-Width="120px"
                                                ItemStyle-HorizontalAlign="Left" />
                                            <cc1:BoundFieldEx ColName="SUPCUST_NO" DataField="SUPCUST_NO" HeaderText="供应商" ItemStyle-HorizontalAlign="Left">
                                                <HeaderStyle Width="120px"></HeaderStyle>
                                            </cc1:BoundFieldEx>
                                            <cc1:BoundFieldEx ColName="SHEET_AMT" DataField="SHEET_AMT" IsSum="true" HeaderText="单据金额"
                                                PriceGrant="InPrice" ItemStyle-HorizontalAlign="Right" GlobalFormat="Money">
                                                <HeaderStyle Width="120px"></HeaderStyle>
                                            </cc1:BoundFieldEx>
                                            <cc1:BoundFieldEx ColName="OPER_ID" DataField="OPER_ID" HeaderText="操作员" ItemStyle-HorizontalAlign="Left">
                                                <HeaderStyle Width="120px"></HeaderStyle>
                                            </cc1:BoundFieldEx>
                                            <cc1:BoundFieldEx ColName="OPER_DATE" DataField="OPER_DATE" HeaderText="操作日期" GlobalFormat="Date">
                                                <HeaderStyle Width="120px"></HeaderStyle>
                                            </cc1:BoundFieldEx>
                                        </Columns>
                                    </cc1:GridViewEx>
                                    <!--补货分析-->
                                    <cc1:GridViewEx ID="gvRptAnalysis" runat="server" AutoGenerateColumns="False" AllowPaging="false"
                                        AllowSorting="true" PageSize="10" IsEidt="true" CellPadding="0" GridLines="None"
                                        DivID="gvDiv" ShowFooter="true" CssClassHeaderCell="gvHeaderRow" CssClassMouseOver="OverRow"
                                        OnCellEdit="gvRptAnalysis_CellEdit" CssClass="detalist" RowClentClick="fn_RowClient2"
                                        OnRowDataBound="gvRptAnalysis_RowDataBound" RaiseRowDataBound="True" OnBind="gvRptAnalysis_Bind">
                                        <RowStyle CssClass="Row" />
                                        <SelectedRowStyle CssClass="OverRow" />
                                        <AlternatingRowStyle CssClass="AlternatingRow" />
                                        <PagerStyle CssClass="PagerRow" />
                                        <Columns>
                                            <cc1:TemplateFieldEx ColName="row_number" HeaderStyle-Width="35px" ColFlag="1" HeaderText="行号">
                                            </cc1:TemplateFieldEx>
                                            <cc1:BoundFieldEx ColName="branch_no" DataField="branch_no" HeaderStyle-Width="120px"
                                                HeaderText="分店仓库" SortExpression="branch_no" ItemStyle-HorizontalAlign="Left">
                                            </cc1:BoundFieldEx>
                                            <cc1:BoundFieldEx ColName="item_no" DataField="item_no" HeaderStyle-Width="110px"
                                                HeaderText="货号" SortExpression="item_no" ItemStyle-HorizontalAlign="Left">
                                            </cc1:BoundFieldEx>
                                            <cc1:BoundFieldEx ColName="item_name" DataField="item_name" HeaderStyle-Width="150px"
                                                HeaderText="品名" SortExpression="item_name" ItemStyle-HorizontalAlign="Left">
                                            </cc1:BoundFieldEx>
                                            <cc1:BoundFieldEx ColName="main_supcust" DataField="main_supcust" HeaderStyle-Width="80px"
                                                HeaderText="主供应商" ColVisible="false" SortExpression="main_supcust" ItemStyle-HorizontalAlign="Left">
                                            </cc1:BoundFieldEx>
                                            <cc1:BoundFieldEx ColName="sup_name" DataField="sup_name" HeaderStyle-Width="120px"
                                                HeaderText="供应商名称" SortExpression="sup_name" ItemStyle-HorizontalAlign="Left">
                                            </cc1:BoundFieldEx>
                                            <cc1:BoundFieldEx ColName="po_cycle" DataField="po_cycle" HeaderStyle-Width="80px"
                                                HeaderText="送货周期" SortExpression="po_cycle" ItemStyle-HorizontalAlign="Left">
                                            </cc1:BoundFieldEx>
                                            <cc1:BoundFieldEx ColName="stock_qty" DataField="stock_qty" HeaderStyle-Width="80px"
                                                HeaderText="当前存量" SortExpression="stock_qty" ItemStyle-HorizontalAlign="right"
                                                GlobalFormat="Quantity">
                                            </cc1:BoundFieldEx>
                                            <%--      <cc1:BoundFieldEx ColName="cost_price" DataField="cost_price" HeaderStyle-Width="80px"
                                                HeaderText="成本价" SortExpression="cost_price" ItemStyle-HorizontalAlign="right"
                                                GlobalFormat="Price" PriceGrant="CostPrice">
                                            </cc1:BoundFieldEx>
                                            <cc1:BoundFieldEx ColName="stock_amt" DataField="stock_amt" HeaderStyle-Width="80px"
                                                HeaderText="金额" SortExpression="stock_amt" ItemStyle-HorizontalAlign="right"
                                                GlobalFormat="Money" IsSum="true" PriceGrant="CostPrice">
                                            </cc1:BoundFieldEx>--%>
                                            <cc1:BoundFieldEx ColName="prev_week_saleqty" DataField="prev_week_saleqty" HeaderStyle-Width="80px"
                                                HeaderText="上周销量" SortExpression="prev_week_saleqty" ItemStyle-HorizontalAlign="right"
                                                GlobalFormat="Quantity">
                                            </cc1:BoundFieldEx>
                                            <cc1:BoundFieldEx ColName="avg_4week_saleqty" DataField="avg_4week_saleqty" HeaderStyle-Width="80px"
                                                HeaderText="四周日均" SortExpression="avg_4week_saleqty" ItemStyle-HorizontalAlign="right"
                                                GlobalFormat="Quantity">
                                            </cc1:BoundFieldEx>
                                            <cc1:BoundFieldEx ColName="cur_week_saleqty" DataField="cur_week_saleqty" HeaderStyle-Width="80px"
                                                HeaderText="本周销量" SortExpression="cur_week_saleqty" ItemStyle-HorizontalAlign="right"
                                                GlobalFormat="Quantity">
                                            </cc1:BoundFieldEx>
                                            <cc1:BoundFieldEx ColName="route_qty" DataField="route_qty" HeaderStyle-Width="90px"
                                                HeaderText="在途量" SortExpression="route_qty" ItemStyle-HorizontalAlign="right"
                                                GlobalFormat="Quantity">
                                            </cc1:BoundFieldEx>
                                            <cc1:BoundFieldEx ColName="safe_qty" DataField="safe_qty" HeaderStyle-Width="90px"
                                                HeaderText="安全库存" SortExpression="safe_qty" ItemStyle-HorizontalAlign="right"
                                                GlobalFormat="Quantity">
                                            </cc1:BoundFieldEx>
                                            <cc1:BoundFieldEx ColName="saleDays" DataField="saleDays" HeaderStyle-Width="80px"
                                                HeaderText="可售天数" SortExpression="saleDays" ItemStyle-HorizontalAlign="right"
                                                GlobalFormat="Quantity">
                                            </cc1:BoundFieldEx>
                                            <cc1:EditCellField ColName="qty" DataField="qty" HeaderStyle-Width="90px" HeaderText="建议采购数量"
                                                SortExpression="qty" ItemStyle-HorizontalAlign="right" GlobalFormat="Quantity">
                                            </cc1:EditCellField>
                                            <cc1:EditCellField ColName="large_qty" DataField="large_qty" HeaderText="建议采购箱数"
                                                ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="90px" SortExpression="large_qty"
                                                GlobalFormat="Quantity">
                                            </cc1:EditCellField>
                                            <cc1:BoundFieldEx ColName="real_qty" DataField="real_qty" HeaderStyle-Width="90px"
                                                HeaderText="要货数量" SortExpression="real_qty" ItemStyle-HorizontalAlign="Right"
                                                ColVisible="false" GlobalFormat="Quantity">
                                                <HeaderStyle Width="90px"></HeaderStyle>
                                            </cc1:BoundFieldEx>
                                            <cc1:BoundFieldEx ColName="valid_price" DataField="valid_price" HeaderStyle-Width="90px"
                                                ColVisible="false" HeaderText="进价" SortExpression="valid_price" ItemStyle-HorizontalAlign="Right">
                                                <HeaderStyle Width="90px"></HeaderStyle>
                                            </cc1:BoundFieldEx>
                                            <cc1:BoundFieldEx ColName="sale_price" DataField="sale_price" HeaderStyle-Width="90px"
                                                ColVisible="false" HeaderText="售价" SortExpression="sale_price" ItemStyle-HorizontalAlign="Right">
                                                <HeaderStyle Width="90px"></HeaderStyle>
                                            </cc1:BoundFieldEx>
                                            <cc1:BoundFieldEx ColName="sub_amt" DataField="sub_amt" HeaderStyle-Width="90px"
                                                PriceGrant="InPrice" ColVisible="false" HeaderText="单据金额" SortExpression="sub_amt"
                                                ItemStyle-HorizontalAlign="Right">
                                                <HeaderStyle Width="90px"></HeaderStyle>
                                            </cc1:BoundFieldEx>
                                            <cc1:BoundFieldEx ColName="purchase_spec" DataField="purchase_spec" HeaderStyle-Width="90px"
                                                ColVisible="false" HeaderText="规格" SortExpression="purchase_spec" ItemStyle-HorizontalAlign="Right">
                                                <HeaderStyle Width="90px"></HeaderStyle>
                                            </cc1:BoundFieldEx>
                                        </Columns>
                                    </cc1:GridViewEx>
                                </div>
                            </div>
                        </ContentTemplate>
                        <Triggers>
                            <asp:PostBackTrigger ControlID="btnClose" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
                <div class="ToolBarInfo" style="margin-top: 2px">
                    <div class="ToolBarBacking" style="line-height: 34px; text-align: left;">
                        <table width="100%">
                            <tr>
                                <td>
                                    <asp:HiddenField ID="hidInfo" runat="server" Value="" />
                                    <asp:Button ID="btnPre" runat="server" Text="上一步" CssClass="PtButton" Visible="False"
                                        OnClick="btnPre_Click" />
                                    <asp:Button ID="btnNext" runat="server" Text="下一步" CssClass="PtButton" OnClick="btnNext_Click" />
                                    <asp:Button ID="btnNextEnd" runat="server" CssClass="PtButton" Text="下一步" Visible="False"
                                        OnClick="btnNextEnd_Click" />
                                    <asp:Button ID="btnDelect" runat="server" Text="删除" CssClass="PtButton" Visible="false"
                                        OnClick="btnDelect_Click" />
                                    <asp:Button ID="btnExport" runat="server" Text="导出" CssClass="PtButton" 
                                        Visible="false" onclick="btnExport_Click"/>
                                    <asp:Button ID="btnClose" runat="server" Text="退出" CssClass="PtButton" Visible="false"
                                        OnClick="btnClose_Click" />
                                    <input type="button" id="btnSet" name="btnSet" value="设置" visible="false" class="PtButton"
                                        runat="server" onserverclick="btnSet_OnClick" />
                                    <input type="hidden" id="ctl00_hidSheetState" value="0" />
                                    <input type="hidden" id="ctl00_hidIsChange" value="0" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnClose" />
            <asp:PostBackTrigger ControlID="btnExport" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
