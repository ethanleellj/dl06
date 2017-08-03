using AjaxPro;
using ASP;
using Aspose.Cells;
using SISS.Common;
using SISS.ISHOP4.BLL;
using SISS.ISHOP4.DAL;
using SISS.ISHOP4.MODEL;
using SISS.WebControl;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class ProCode_Pur_PurchaseGuide2_qqj : BasePage, IRequiresSessionState
{
    private SheetManageBLL _commSheetBll = new SheetManageBLL();
    private BDBranchInfoBll _objBranchBll = new BDBranchInfoBll();
    private PMSheetMasterBll _sheetMasterBll = new PMSheetMasterBll();
    private SYSMenuBll _sysMenuBll = new SYSMenuBll();
    private const string _transNo = "PO";
    //protected Button btnClose;
    //protected Button btnDelect;
    //protected Button btnExport;
    //protected Button btnNext;
    //protected Button btnNextEnd;
    //protected Button btnPre;
    //protected HtmlInputButton btnSet;
    //protected HtmlGenericControl DivSetup;
    //protected HtmlGenericControl DivSetup1;
    //protected GridViewEx gvDetail;
    //protected GridViewEx gvOrderList;
    //protected GridViewEx gvRptAnalysis;
    //protected TextBox hidCurrSelectedRowId;
    //protected HiddenField hidInfo;
    //protected TextBox hidItemBarcode;
    //protected TextBox hidSelectedRowId;
    //protected TextBox hidSHEET_NO;
    //protected Image Image8;
    //protected Label lblHead;
    //protected RadioButtonList rdoCheck;
    //protected Controls_UCCalendar txtBranchDate;
    //protected Controls_UCBranchNo txtBranchNo;
    //protected TextBox txtlevel;
    //protected Controls_UCCalendar txtSaleDate;
    //protected Controls_UCBaseCode txtSupNo;
    //protected Controls_UCBaseCode ucItemBrand;
    //protected Controls_UCBaseCode ucItemCls;
    //protected UpdatePanel UpdatePanel1;
    //protected UpdatePanel UpdatePanel2;

    protected void btnClose_Click(object sender, EventArgs e)
    {
        this.hidSHEET_NO.Text = "";
        this.setButtonsStatus("1");
    }

    protected void btnDelect_Click(object sender, EventArgs e)
    {
        if ((this.hidSelectedRowId.Text.Trim() != "") && (this.hidItemBarcode.Text.Trim() != ""))
        {
            int index = Convert.ToInt32(this.hidSelectedRowId.Text.Trim()) - 1;
            if ((this.rdoCheck.SelectedValue.Trim() != "") && (this.rdoCheck.SelectedValue == "按补货分析采购"))
            {
                this.gvRptAnalysis.DtSource.Rows.RemoveAt(index);
                this.gvRptAnalysis.DataTableBing();
                this.hidItemBarcode.Text = "";
                this.hidSelectedRowId.Text = "";
            }
            else
            {
                this.gvDetail.DtSource.Rows.RemoveAt(index);
                this.gvDetail.DataTableBing();
                this.hidItemBarcode.Text = "";
                this.hidSelectedRowId.Text = "";
            }
        }
    }

    protected void btnExport_Click(object sender, EventArgs e)
    {
        if (this.rdoCheck.SelectedValue.Equals("按补货分析采购"))
        {
            if ((this.gvRptAnalysis.DtSource == null) || (this.gvRptAnalysis.DtSource.Rows.Count <= 0))
            {
                BasePage.Alert("没有任何数据！", this.Page);
                return;
            }
        }
        else if ((this.gvDetail.DtSource == null) || (this.gvDetail.DtSource.Rows.Count <= 0))
        {
            BasePage.Alert("没有任何数据！", this.Page);
            return;
        }
        Workbook workbook = new Workbook();
        Aspose.Cells.Style defaultStyle = workbook.DefaultStyle;
        defaultStyle.Font.Name = "Tahoma";
        workbook.DefaultStyle = defaultStyle;
        if (this.rdoCheck.SelectedValue.Equals("按补货分析采购"))
        {
            this.CreateStaticData(workbook, this.gvRptAnalysis.DtSource);
        }
        else
        {
            this.CreateStaticData(workbook, this.gvDetail.DtSource);
        }
        workbook.Save("Information" + BasePage.GVar.OperId + ".xlsx", FileFormatType.Xlsx, SaveType.OpenInExcel, base.Response);
    }

    protected void btnNext_Click(object sender, EventArgs e)
    {
        if (((this.txtBranchDate.Text.Trim() != "") && (this.txtBranchDate.Text2.Trim() != "")) && ((this.txtSaleDate.Text.Trim() != "") && (this.txtSaleDate.Text2.Trim() != "")))
        {
            if (BasePage.GVar.IsAreaOper && !this._objBranchBll.ExistBranchArear(BasePage.GVar.CBranchAearNo))
            {
                BasePage.Alert("必须为总部做采购向导,该区域不包含总部机构，请重新登录总部！", this.Page);
            }
            else
            {
                this.setButtonsStatus("2");
                DataTable table = this.SelectOrderDetailMessage();
                if ((table != null) && (table.Rows.Count > 0))
                {
                    if ((this.rdoCheck.SelectedValue.Trim() != "") && (this.rdoCheck.SelectedValue == "按补货分析采购"))
                    {
                        this.gvRptAnalysis.DtSource = table;
                        this.gvRptAnalysis.DataTableBing();
                    }
                    else
                    {
                        this.gvDetail.DtSource = table;
                        this.gvDetail.DataTableBing();
                    }
                }
                else
                {
                    this.btnNextEnd.Enabled = false;
                    BasePage.Alert("此向导无数据，请重新选择", this.Page);
                    this.setButtonsStatus("1");
                }
            }
        }
        else
        {
            BasePage.Alert("日期不能为空!", this.Page);
            this.setButtonsStatus("1");
        }
    }

    protected void btnNextEnd_Click(object sender, EventArgs e)
    {
        this.hidSHEET_NO.Text = "";
        if ((this.rdoCheck.SelectedValue.Trim() != "") && (this.rdoCheck.SelectedValue == "按补货分析采购"))
        {
            if ((this.gvRptAnalysis.DtSource == null) || (this.gvRptAnalysis.DtSource.Rows.Count <= 0))
            {
                BasePage.Alert("详细列表中无商品信息，请点击[上一步]重新选择!", this.Page);
                return;
            }
        }
        else if ((this.gvDetail.DtSource == null) || (this.gvDetail.DtSource.Rows.Count <= 0))
        {
            BasePage.Alert("详细列表中无商品信息，请点击[上一步]重新选择!", this.Page);
            return;
        }
        this.DateGridQuery();
        if (this.hidInfo.Value != "")
        {
            string[] strArray = this.hidInfo.Value.Split(new char[] { '|' });
            string needbegindate = strArray[0];
            string needenddate = strArray[1];
            string salebegindate = strArray[2];
            string saleenddate = strArray[3];
            string itemClsno = strArray[4];
            string itemBrand = strArray[5];
            string branchNo = strArray[6];
            string supcustNo = strArray[7];
            this._sheetMasterBll.UpdateDYComflag(needbegindate, needenddate, salebegindate, saleenddate, branchNo, itemClsno, itemBrand, supcustNo);
        }
    }

    protected void btnPre_Click(object sender, EventArgs e)
    {
        this.setButtonsStatus("1");       
    }

    protected void btnSet_OnClick(object sender, EventArgs e)
    {
        this.SheetSetting();
    }

    private void CreateStaticData(Workbook workbook, DataTable dtExcel)
    {
        Worksheet worksheet = workbook.Worksheets[0];
        worksheet.Name = "采购向导数据";
        Aspose.Cells.Cells cells = worksheet.Cells;
        try
        {
            DataTable dataTable = new DataTable();
            DataTable table2 = dtExcel;
            List<string> list = new List<string>();
            if (this.rdoCheck.SelectedValue.Equals("按补货分析采购"))
            {
                foreach (DataControlField field in this.gvRptAnalysis.Columns)
                {
                    IDataField field2 = field as IDataField;
                    if (((field2 != null) && !(field2.ColName == "row_number")) && field2.ColVisible)
                    {
                        list.Add(field.HeaderText);
                        dataTable.Columns.Add(field2.ColName);
                    }
                }
            }
            else
            {
                foreach (DataControlField field3 in this.gvDetail.Columns)
                {
                    IDataField field4 = field3 as IDataField;
                    if (((field4 != null) && !(field4.ColName == "row_number")) && field4.ColVisible)
                    {
                        list.Add(field3.HeaderText);
                        dataTable.Columns.Add(field4.ColName);
                    }
                }
            }
            DataRow row = dataTable.NewRow();
            row.ItemArray = list.ToArray();
            dataTable.Rows.InsertAt(row, 0);
            int pos = 1;
            foreach (DataRow row2 in table2.Rows)
            {
                list = new List<string>();
                row = dataTable.NewRow();
                for (int i = 0; i < dataTable.Columns.Count; i++)
                {
                    list.Add(SIString.TryStr(row2[dataTable.Columns[i].ColumnName]));
                }
                if ((this.rdoCheck.SelectedValue.Equals("按补货分析采购") && (Convert.ToDecimal(list[12]) != 0M)) && (Convert.ToDecimal(list[13]) == 0M))
                {
                    list[13] = list[12];
                }
                row.ItemArray = list.ToArray();
                dataTable.Rows.InsertAt(row, pos);
                pos++;
            }
            cells.ImportDataTable(dataTable, false, 0, 0);
        }
        catch (Exception exception)
        {
            BasePage.Alert(exception.Message, this.Page);
        }
    }

    private void DateGridQuery()
    {
        string validday = "";
        if (PubClass.GetSystemValue("Order_Valid_Day").Trim() != "")
        {
            validday = PubClass.FormatDate(DateTime.Now.AddDays((double) int.Parse(PubClass.GetSystemValue("Order_Valid_Day").Trim())));
        }
        else
        {
            validday = PubClass.FormatDate(DateTime.Now);
        }
        DataTable dtSource = null;
        if ((this.rdoCheck.SelectedValue.Trim() != "") && (this.rdoCheck.SelectedValue == "按补货分析采购"))
        {
            dtSource = this.gvRptAnalysis.DtSource;
        }
        else
        {
            dtSource = this.gvDetail.DtSource;
        }
        if (dtSource != null)
        {
            int num = 0;
            string str2 = "";
            string str3 = "";
            string str4 = "";
            string str5 = "";
            string str6 = "";
            string str7 = "";
            string str8 = "";
            string str9 = "";
            decimal num2 = 0M;
            string str10 = "";
            int num3 = 0;
            int num4 = 0;
            string str11 = "";
            string str12 = "";
            string sheetNo = "";
            string str14 = "";
            string str15 = "";
            string str16 = "";
            string str17 = "";
            num = dtSource.Select(" item_no <> '' ").Count<DataRow>();
            if (num == 0)
            {
                BasePage.Alert("此向导无数据，请重新选择", this.Page);
                this.setButtonsStatus("2");
                return;
            }
            string[] strArray = new string[num];
            string[] selitems = new string[strArray.Length];
            string str18 = "";
            string str19 = "";
            if (num == 1)
            {
                num4 = 1;
                num3 = 1;
                str12 = dtSource.Rows[0]["main_supcust"].ToString().Trim();
                str3 = dtSource.Rows[0]["real_qty"].ToString().Trim();
                str5 = dtSource.Rows[0]["valid_price"].ToString().Trim();
                str11 = dtSource.Rows[0]["tax"].ToString().Trim();
                str7 = dtSource.Rows[0]["sale_price"].ToString().Trim();
                str6 = dtSource.Rows[0]["sub_amt"].ToString().Trim();
                str4 = dtSource.Rows[0]["LARGE_QTY"].ToString().Trim();
                str2 = dtSource.Rows[0]["item_no"].ToString().Trim();
                num2 = Convert.ToDecimal(dtSource.Rows[0]["qty"].ToString().Trim());
                str15 = dtSource.Rows[0]["main_supcusts"].ToString().Trim();
                str16 = dtSource.Rows[0]["branch_no"].ToString().Trim();
                strArray[0] = string.Concat(new object[] { 
                    str2, "^", num2, "^", str12, "^", str4, "^", str3, "^", str8, "^", str5, "^", str6, "^", 
                    str11, "^", str7, "^", str15, "^", str16
                 });
                sheetNo = PubClass.GetSheetNo("PO");
                str14 = (str14 == "") ? ("'" + sheetNo + "'") : (str14 + ",'" + sheetNo + "'");
                selitems[0] = strArray[0] + "^" + sheetNo;
            }
            else
            {
                for (int i = 0; i < num; i++)
                {
                    if (i != (num - 1))
                    {
                        str2 = dtSource.Rows[i + 1]["item_no"].ToString();
                        str16 = dtSource.Rows[i + 1]["branch_no"].ToString();
                    }
                    if ((str2 == dtSource.Rows[i]["item_no"].ToString()) && (str16 == dtSource.Rows[i]["branch_no"].ToString()))
                    {
                        if (i != (num - 1))
                        {
                            str10 = dtSource.Rows[i + 1]["item_no"].ToString();
                            str17 = dtSource.Rows[i + 1]["branch_no"].ToString();
                        }
                        str12 = dtSource.Rows[i]["main_supcust"].ToString().Trim();
                        str4 = dtSource.Rows[i]["LARGE_QTY"].ToString().Trim();
                        str5 = dtSource.Rows[i]["valid_price"].ToString().Trim();
                        str11 = dtSource.Rows[i]["tax"].ToString().Trim();
                        str7 = dtSource.Rows[i]["sale_price"].ToString().Trim();
                        str6 = dtSource.Rows[i]["sub_amt"].ToString().Trim();
                        str2 = dtSource.Rows[i]["item_no"].ToString().Trim();
                        str9 = dtSource.Rows[i]["qty"].ToString().Trim();
                        num2 += Convert.ToDecimal(str9);
                        str3 = dtSource.Rows[i]["real_qty"].ToString().Trim();
                        str15 = dtSource.Rows[i]["main_supcusts"].ToString().Trim();
                        str16 = dtSource.Rows[i]["branch_no"].ToString().Trim();
                        if ((str10 != dtSource.Rows[i]["item_no"].ToString()) || (str17 != dtSource.Rows[i]["branch_no"].ToString()))
                        {
                            num4++;
                            strArray[i] = string.Concat(new object[] { 
                                str2, "^", num2, "^", str12, "^", str4, "^", str3, "^", str8, "^", str5, "^", str6, "^", 
                                str11, "^", str7, "^", str15, "^", str16
                             });
                        }
                    }
                    else
                    {
                        num4++;
                        str12 = dtSource.Rows[i]["main_supcust"].ToString().Trim();
                        str3 = dtSource.Rows[i]["real_qty"].ToString().Trim();
                        str4 = dtSource.Rows[i]["LARGE_QTY"].ToString().Trim();
                        str5 = dtSource.Rows[i]["valid_price"].ToString().Trim();
                        str11 = dtSource.Rows[i]["tax"].ToString().Trim();
                        str7 = dtSource.Rows[i]["sale_price"].ToString().Trim();
                        str6 = dtSource.Rows[i]["sub_amt"].ToString().Trim();
                        str2 = dtSource.Rows[i]["item_no"].ToString().Trim();
                        str16 = dtSource.Rows[i]["branch_no"].ToString();
                        num2 = Convert.ToDecimal(dtSource.Rows[i]["qty"].ToString().Trim());
                        str15 = dtSource.Rows[i]["main_supcusts"].ToString().Trim();
                        strArray[i] = string.Concat(new object[] { 
                            str2, "^", num2, "^", str12, "^", str4, "^", str3, "^", str8, "^", str5, "^", str6, "^", 
                            str11, "^", str7, "^", str15, "^", str16
                         });
                    }
                }
                for (int j = 0; j < strArray.Length; j++)
                {
                    if (strArray[j] != null)
                    {
                        string[] strArray3 = strArray[j].Trim().Split(new char[] { '^' });
                        if (!(str18 != strArray3[10]) && !(str19 != strArray3[11]))
                        {
                            selitems[j] = strArray[j] + "^" + sheetNo;
                        }
                        else
                        {
                            num3++;
                            str18 = strArray3[10].Trim();
                            str19 = strArray3[11].Trim();
                            sheetNo = PubClass.GetSheetNo("PO");
                            str14 = (str14 == "") ? ("'" + sheetNo + "'") : (str14 + ",'" + sheetNo + "'");
                            selitems[j] = strArray[j] + "^" + sheetNo;
                        }
                    }
                }
            }
            if (this._sheetMasterBll.InsertOrderDetail(BasePage.GVar.LoginBranchNo, BasePage.GVar.OperId, selitems, validday) > 0)
            {
                BasePage.Alert("自动生成订单" + num3.ToString() + "张,订货" + num4.ToString() + "种", this.Page);
                this.lblHead.Text = "订单列表";
            }
            this.hidSHEET_NO.Text = str14;
            this.gvOrderList.DataTableBing();
            InsertPurchaseGuideData_qqj();      //qqjMark       记录生成单据的参数，与生成的采购订单单号
        }
        this.setButtonsStatus("3");
    }






    public void InsertPurchaseGuideData_qqj()
    {
        string[] strArray = this.hidInfo.Value.Split(new char[] { '|' });
        string needbegindate = strArray[0];
        string needenddate = strArray[1];
        string salebegindate = strArray[2];
        string saleenddate = strArray[3];
        string itemClsno = strArray[4];
        string itemBrand = strArray[5];
        string branchNo = strArray[6];
        string supcustNo = strArray[7];

        string strGenSheet = string.IsNullOrEmpty(PubClass.GetSystemValue("gen_sheet")) ? "0" : PubClass.GetSystemValue("gen_sheet");
        string str2 = "";
        if (strGenSheet == "1")
        {
            str2 = " and b.com_flag = '0'";
        }


        StringBuilder builder = new StringBuilder();
        builder.Append(@"
        
declare  @pktBoxPurchaseGuideMaster  varchar(50)
SELECT @pktBoxPurchaseGuideMaster =  CONVERT(varchar(100), GETDATE(), 21)


INSERT  INTO   tBoxPurchaseGuideMaster(pktBoxPurchaseGuideMaster, dNeedBeginDate, dNeedEndDate, vBranchNo, vItemClsno, vItemBrand, vSupcustNo)
VALUES       (@pktBoxPurchaseGuideMaster, @needbegindate, @needenddate, @branchNo, @itemClsno, @itemBrand, @supcustNo)



INSERT  INTO    [tBoxPurchaseGuideDYDetail]( pktBoxPurchaseGuideMaster, vSheetNO)
select distinct @pktBoxPurchaseGuideMaster,  b.sheet_no 
FROM t_bd_item_info a,                
        t_pm_sheet_master b,               
        t_pm_sheet_detail c
WHERE b.sheet_no=c.sheet_no and a.item_stock='1' and    
        c.item_no=a.item_no and   ( (dbo.fn_get_item_saleflag(a.item_no,'sale_flag',b.d_branch_no  )) not in ('3', '2') ) AND                                   
        b.approve_flag = '1'   and                                       
        (a.item_clsno like  @itemClsno + '%'  ) and 
        (a.item_brand like @itemBrand +'%' ) and 
        (a.main_supcust like @supcustNo +'%' ) and 
        ( b.trans_no = 'DY') and (b.order_status <> '5') and                                                                 
        convert(char(10), b.valid_date, 112)  >= convert(char(10), getdate(), 112) and      
        isnull(c.real_qty,0) > isnull(c.order_qty,0)  
        and(convert(char(10),b.work_date,20) between @needbeginDate and @needendDate  )   
    and	b.d_branch_no  like left(@branchno,4)+'%'   {0} 





INSERT  INTO    [tBoxPurchaseGuidePODetail]( pktBoxPurchaseGuideMaster, vSheetNO)
select @pktBoxPurchaseGuideMaster, sheet_no
FROM            t_pm_sheet_master 
where sheet_no in (" + hidSHEET_NO.Text + @")

UPDATE t_pm_sheet_master SET  memo = SUBSTRING(CAST(('由采购向导2生成：' + @pktBoxPurchaseGuideMaster) AS text), 1, COL_LENGTH('t_pm_sheet_master', 'memo')) 
where sheet_no in (" + hidSHEET_NO.Text + @")

");

        DBHelper.GetData(string.Format(builder.ToString(),  str2), GetParams_qqj(needbegindate, needenddate, salebegindate, saleenddate, branchNo, itemClsno, itemBrand, supcustNo));



    }









    public string getopen(string menuid, string sheetNo)
    {
        t_sys_menu menuByids = this._sysMenuBll.GetMenuByids(menuid);
        string str2 = "采购订单";
        string str3 = "";
        string str4 = base.ResolveUrl(menuByids.menu_code);
        string str5 = str4.Substring(0, str4.LastIndexOf("/") + 1) + "POSheet.aspx";
        string str6 = str5 + "?menuId=" + menuid + "&SHEET_NO=" + sheetNo + "&general=" + str3;
        return ("try{ if(ProCode_Pur_PurchaseGuide.isExitsBllNO('" + sheetNo.Trim() + "').value=='0'){   alert('采购订单[" + sheetNo.Trim() + "]已删除,请重新生成！');}else {  var objre=parent.AddMenu('" + menuid + "', '" + str2 + "', '" + str6 + "', 'BROW'); if(objre!='undefined'){ if(objre==false)alert('如已打开采购订单页面，请先关闭后再打开！');}}}catch(e){} ");
    }

    public PagingBase GetOrderList(string sheetno, PagingBase pageModel)
    {
        pageModel = this._sheetMasterBll.GetOrderList(sheetno, pageModel);
        return pageModel;
    }

    protected void gvDetail_Bind()
    {
        ColumnsSetting.GetSettings(base.Request["menuId"].Trim(), 1, BasePage.GVar.OperId, this.gvDetail);
        ColumnsSetting.SetGeneralColunms(this.gvDetail);
    }

    protected void gvDetail_CellEdit(object sender, GridViewCellEventArgs e)
    {
        DataRow currentRow = e.CurrentRow;
        object obj1 = currentRow[e.ColumnName];
        e.ColumnName.ToLower();
        string str = e.ColumnName.ToLower();
        if (str != null)
        {
            if (str == "qty")
            {
                decimal num;
                if (SIString.TryDec(currentRow["purchase_spec"], 1M) == 0M)
                {
                    num = 1M;
                }
                else
                {
                    num = SIString.TryDec(currentRow["purchase_spec"], 1M);
                }
                currentRow["large_qty"] = SIString.TryDec(currentRow["qty"], 0M) / num;
                currentRow["sub_amt"] = SIString.TryDec(currentRow["valid_price"], 0M) * SIString.TryDec(currentRow["real_qty"], 0M);
            }
            else if (str == "large_qty")
            {
                decimal num2;
                if (SIString.TryDec(currentRow["purchase_spec"], 1M) == 0M)
                {
                    num2 = 1M;
                }
                else
                {
                    num2 = SIString.TryDec(currentRow["purchase_spec"], 1M);
                }
                currentRow["qty"] = SIString.TryDec(currentRow["large_qty"], 0M) * num2;
                currentRow["sub_amt"] = SIString.TryDec(currentRow["valid_price"], 0M) * SIString.TryDec(currentRow["real_qty"], 0M);
            }
        }
    }

    public void gvOrderList_Bind()
    {
        string sheetno = this.hidSHEET_NO.Text.Trim();
        ColumnsSetting.GetSettings(base.Request["menuId"].Trim(), 1, BasePage.GVar.OperId, this.gvOrderList);
        PagingBase pagingModel = ColumnsSetting.GetPagingModel(this.gvOrderList);
        ColumnsSetting.SetPagingModel(this.GetOrderList(sheetno, pagingModel), this.gvOrderList);
    }

    protected void gvOrderList_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            int colIndex = this.gvOrderList.GetColIndex("SHEET_NO");
            string menuid = "2202";
            string str2 = "";
            string sheetNo = (e.Row.DataItem as DataRowView)["SHEET_NO"].ToString();
            if ((colIndex >= 0) && (sheetNo != ""))
            {
                str2 = this.getopen(menuid, sheetNo);
                ((HyperLink) e.Row.Cells[colIndex].Controls[0]).NavigateUrl = "javascript:" + str2;
                ((HyperLink) e.Row.Cells[colIndex].Controls[0]).Style.Add("TEXT-DECORATION", "underline");
                ((HyperLink) e.Row.Cells[colIndex].Controls[0]).Attributes.Add("onmouseover", "this.style.color='red';");
                ((HyperLink) e.Row.Cells[colIndex].Controls[0]).Attributes.Add("onmouseout", "this.style.color='black';");
            }
        }
    }

    protected void gvRptAnalysis_Bind()
    {
        ColumnsSetting.GetSettings(base.Request["menuId"].Trim(), 2, BasePage.GVar.OperId, this.gvRptAnalysis);
        ColumnsSetting.SetGeneralColunms(this.gvRptAnalysis);
    }

    protected void gvRptAnalysis_CellEdit(object sender, GridViewCellEventArgs e)
    {
        DataRow currentRow = e.CurrentRow;
        object obj1 = currentRow[e.ColumnName];
        e.ColumnName.ToLower();
        string str = e.ColumnName.ToLower();
        if (str != null)
        {
            if (str == "qty")
            {
                decimal num;
                if (SIString.TryDec(currentRow["purchase_spec"], 1M) == 0M)
                {
                    num = 1M;
                }
                else
                {
                    num = SIString.TryDec(currentRow["purchase_spec"], 1M);
                }
                currentRow["large_qty"] = SIString.TryDec(currentRow["qty"], 0M) / num;
                currentRow["sub_amt"] = SIString.TryDec(currentRow["valid_price"], 0M) * SIString.TryDec(currentRow["qty"], 0M);
            }
            else if (str == "large_qty")
            {
                decimal num2;
                if (SIString.TryDec(currentRow["purchase_spec"], 1M) == 0M)
                {
                    num2 = 1M;
                }
                else
                {
                    num2 = SIString.TryDec(currentRow["purchase_spec"], 1M);
                }
                currentRow["qty"] = SIString.TryDec(currentRow["large_qty"], 0M) * num2;
                currentRow["sub_amt"] = SIString.TryDec(currentRow["valid_price"], 0M) * SIString.TryDec(currentRow["qty"], 0M);
            }
        }
    }

    protected void gvRptAnalysis_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            int colIndex = this.gvRptAnalysis.GetColIndex("large_qty");
            int num2 = this.gvRptAnalysis.GetColIndex("qty");
            string str = (e.Row.DataItem as DataRowView)["large_qty"].ToString();
            if ((colIndex >= 0) && (str != ""))
            {
                string text = ((TextBox) e.Row.Cells[num2].Controls[0]).Text;
                ((TextBox) e.Row.Cells[colIndex].Controls[0]).Text = SIString.TryDec(text).ToString();
            }
        }
    }

    [AjaxMethod]
    public string isExitsBllNO(string sheetNo)
    {
        if (!this._sheetMasterBll.Exists(sheetNo))
        {
            return "0";
        }
        return "1";
    }

    protected void lbitem_barcode_Click(object sender, EventArgs e)
    {
        int rowIndex = ((GridViewRow) ((LinkButton) sender).NamingContainer).RowIndex;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        Utility.RegisterTypeForAjax(typeof(ProCode_Pur_PurchaseGuide), this.Page);
        if (!base.IsPostBack)
        {
            this.btnDelect.Attributes.Add("onclick", "if(!checkDetete()) return false;");
            this.txtBranchDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            this.txtBranchDate.Text2 = DateTime.Now.AddDays(1.0).ToString("yyyy-MM-dd");
            this.txtSaleDate.Text = DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd");
            this.txtSaleDate.Text2 = DateTime.Now.AddDays(1.0).ToString("yyyy-MM-dd");
            //this.txtBranchNo.objNo.Text = BasePage.GVar.LoginBranchNo;        //qqjMark
            this.txtBranchNo.objNo.Text = "ALL";        //qqjMark
            if (PubClass.GetSystemValue("trans_pur_guide").Equals("1"))
            {
                this.rdoCheck.Items.Add("按分店要货量生成分店订单");
            }
            ColumnsSetting.DefaultSettings(base.Request["menuId"].Trim(), 1, BasePage.GVar.OperId, this.gvDetail);
            ColumnsSetting.GetSettings(base.Request["menuId"].Trim(), 1, BasePage.GVar.OperId, this.gvDetail);
            ColumnsSetting.SetPagingModel(ColumnsSetting.GetPagingModel(this.gvDetail), this.gvDetail);
            this.gvDetail.ResetEmptyData();
        }
        string str = base.Request.Params.Get("__EVENTTARGET");
        base.Request.Params.Get("__EVENTARGUMENT");
        if (str == "btnNext")
        {
            this.btnNext_Click(sender, e);
        }
        BasePage.RegisterJs("SetDivHegith(1, '', 'DivSetup1');", this.Page);
    }

    protected void rdoCheck_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.txtBranchDate.ctlEnabled = !this.rdoCheck.SelectedValue.Equals("按补货分析采购");
        this.txtSaleDate.ctlEnabled = !this.rdoCheck.SelectedValue.Equals("按补货分析采购");
    }

    protected DataTable SelectOrderDetailMessage()
    {
        string needbegindate = this.txtBranchDate.Text.Trim();
        string needenddate = this.txtBranchDate.Text2.Trim();
        string salebegindate = this.txtSaleDate.Text.Trim();
        string saleenddate = this.txtSaleDate.Text2.Trim();
        string itemClsno = this.ucItemCls.objNo.Text.Trim();
        string itemBrand = this.ucItemBrand.objNo.Text.Trim();
        string branchNo = this.txtBranchNo.objNo.Text.Trim();
        string supcustNo = this.txtSupNo.objNo.Text.Trim();
        if (branchNo == "ALL")
        {
            branchNo = "%";
        }
        this.hidInfo.Value = "";
        DataTable table = null;
        if ((this.rdoCheck.SelectedValue.Trim() != "") && (this.rdoCheck.SelectedValue == "按库存存量指标采购"))
        {
            return this._sheetMasterBll.SelectOrderDetailByStorTarget(needbegindate, needenddate, salebegindate, saleenddate, branchNo, itemClsno, itemBrand, supcustNo);
        }
        if ((this.rdoCheck.SelectedValue.Trim() != "") && (this.rdoCheck.SelectedValue == "按分店要货量采购"))
        {
            string strGenSheet = string.IsNullOrEmpty(PubClass.GetSystemValue("gen_sheet")) ? "0" : PubClass.GetSystemValue("gen_sheet");
            //table = this._sheetMasterBll.SelectOrderDetailByBranchNo(needbegindate, needenddate, salebegindate, saleenddate, branchNo, itemClsno, itemBrand, supcustNo, strGenSheet);    //qqjMark
            table = this.SelectOrderDetailByBranchNo_qqj(needbegindate, needenddate, salebegindate, saleenddate, branchNo, itemClsno, itemBrand, supcustNo, strGenSheet);  





            if (table.Rows.Count > 0)
            {
                this.hidInfo.Value = needbegindate + "|" + needenddate + "|" + salebegindate + "|" + saleenddate + "|" + itemClsno + "|" + itemBrand + "|" + branchNo + "|" + supcustNo;
            }
            return table;
        }
        if ((this.rdoCheck.SelectedValue.Trim() != "") && (this.rdoCheck.SelectedValue == "按销量采购"))
        {
            return this._sheetMasterBll.SelectOrderDetailBySales(needbegindate, needenddate, salebegindate, saleenddate, branchNo, itemClsno, itemBrand, supcustNo);
        }
        if ((this.rdoCheck.SelectedValue.Trim() != "") && (this.rdoCheck.SelectedValue == "按批发订单采购"))
        {
            return this._sheetMasterBll.SelectOrderDetailByWsm(needbegindate, needenddate, salebegindate, saleenddate, branchNo, itemClsno, itemBrand, supcustNo);
        }
        if ((this.rdoCheck.SelectedValue.Trim() != "") && (this.rdoCheck.SelectedValue == "按分店要货量生成分店订单"))
        {
            return this._sheetMasterBll.SelectOrderDIDetailByBranchNo(needbegindate, needenddate, salebegindate, saleenddate, branchNo, itemClsno, itemBrand, supcustNo);
        }
        if ((this.rdoCheck.SelectedValue.Trim() != "") && (this.rdoCheck.SelectedValue == "按补货分析采购"))
        {
            table = this._sheetMasterBll.SelectOrderDetailByAnalyse(branchNo, itemClsno, itemBrand, supcustNo);
        }
        return table;
    }




    public DataTable SelectOrderDetailByBranchNo_qqj(string needbegindate, string needenddate, string salebegindate, string saleenddate, string branchNo, string itemClsno, string itemBrand, string supcustNo, string strGenSheet)
    {
        string str2 = "";
        if (strGenSheet == "1")
        {
            str2 = " and b.com_flag = '0'";
        }
        return DBHelper.GetData(string.Format(@"

            SELECT item_no,item_size,item_name, 
            purchase_spec, 
            unit_no,  
            --LARGE_QTY=((sum(real_qty)  - allstock_qty )/isnull(avg(purchase_spec),1)), 			--qqjMark
            LARGE_QTY=((sum(real_qty) )/isnull(avg(purchase_spec),1)), 			--qqjMark		直接为要货单数量，不考虑库存量
            branch_no,  
            sum(real_qty) as real_qty,main_supcusts, 
            main_supcust ,
            d_branch_no,
            allstock_qty ,    
            --(sum(real_qty)  - allstock_qty) as qty,			--qqjMark
			(sum(real_qty)  ) as qty,			--qqjMark		直接为要货单数量，不考虑库存量
            valid_price,sale_price,  
            (sum(real_qty)  - allstock_qty) * valid_price as  sub_amt, 
            tax,   
            sum(order_qty) as order_qty,
            stock_qty,               
           	sale_qnty
            FROM(
	select item_no,item_size,item_name,purchase_spec, unit_no,  
	LARGE_QTY=((sum(real_qty) - allstock_qty )/isnull(avg(purchase_spec),1)),
	branch_no,sum(real_qty) as real_qty,main_supcusts, main_supcust ,d_branch_no,allstock_qty ,    
	(sum(real_qty) - allstock_qty) as qty,valid_price,sale_price,  
	(sum(real_qty) - allstock_qty) * valid_price as  sub_amt, 
	tax, sum(order_qty) as order_qty,stock_qty,               
	sale_qnty from (
           SELECT c.item_no as item_no,  a.item_size,  a.item_name, 
            purchase_spec=isnull(avg(a.purchase_spec),1), 
            a.unit_no,  
            LARGE_QTY=(((sum(c.real_qty-isnull(c.order_qty,0))-isnull((Select isnull(sum(stock_qty),0)from t_im_branch_stock st where st.item_no = a.item_no 
           and st.branch_no in (select branch_no from t_bd_branch_info where branch_no like  left(b.d_branch_no,4)+'%' and trade_type='5')),0))/isnull(avg(a.purchase_spec),1))), 
            branch_no=(select top 1 branch_no from t_bd_branch_info where branch_no  like left(b.d_branch_no,4)+'%'   and trade_type='5'),
            sum(c.real_qty-isnull(c.order_qty,0)) as real_qty,a.main_supcust as main_supcusts, 
            main_supcust = (select sup_name from t_bd_supcust_info where supcust_no=a.main_supcust and supcust_flag='S'),
            b.d_branch_no,
            allstock_qty = (Select isnull(sum(stock_qty),0)from t_im_branch_stock st where st.item_no = a.item_no 
           and st.branch_no in (select branch_no from t_bd_branch_info where branch_no like  left(b.d_branch_no,4)+'%' and trade_type='5')) ,    
           (sum(isnull(c.real_qty,0)-isnull(c.order_qty,0))-isnull((Select isnull(sum(stock_qty),0)from t_im_branch_stock st where st.item_no = a.item_no 
           and st.branch_no in (select branch_no from t_bd_branch_info where branch_no like  left(b.d_branch_no,4)+'%' and trade_type='5')),0))
            as qty,
            a.price as valid_price,a.sale_price,  
            round( a.price * (sum(c.real_qty-isnull(c.order_qty,0))-isnull((Select isnull(sum(stock_qty),0) 
            from t_im_branch_stock st where st.item_no = a.item_no and st.branch_no like left(b.d_branch_no,4)+'%'),0)),2)as sub_amt, 
            a.purchase_tax as tax,   
            isnull(c.order_qty,0) as order_qty ,
            stock_qty=isnull((select sum(isnull(target.max_qty,0))
            from t_im_stock_target target where  target.item_no =c.item_no and target.branch_no like left(b.d_branch_no,4)+'%' ),0),               
           	(SELECT  isnull(sum(isnull(d.so_qty + d.pos_qty - d.pos_ret_qty+ d.pos_send_qty,0)),0)
           	 FROM t_da_jxc_daysum d
           		WHERE
           		d.item_no = a.item_no  and 
           		CONVERT(varchar(10), d.oper_date, 20) BETWEEN  @salebeginDate and @saleendDate   and
           		d.branch_no = b.d_branch_no	 and
           		d.item_no =c.item_no )as sale_qnty
                FROM t_bd_item_info a,                
                     t_pm_sheet_master b,               
                     t_pm_sheet_detail c
                WHERE b.sheet_no=c.sheet_no and a.item_stock='1' and    
                     c.item_no=a.item_no and   ( (dbo.fn_get_item_saleflag(a.item_no,'sale_flag',b.d_branch_no  )) not in ('3', '2') ) AND                                   
            		 b.approve_flag = '1'   and                                       
                     (a.item_clsno like  @itemClsno + '%'  ) and 
                     (a.item_brand like @itemBrand +'%' ) and 
                     (a.main_supcust like @supcustNo +'%' ) and 
                     ( b.trans_no = 'DY') and (b.order_status <> '5') and                                                                 
                     convert(char(10), b.valid_date, 112)  >= convert(char(10), getdate(), 112) and      
                     isnull(c.real_qty,0) > isnull(c.order_qty,0)  
            	     and(convert(char(10),b.work_date,20) between @needbeginDate and @needendDate  )   
             		and	b.d_branch_no  like left(@branchno,4)+'%'     {0}                        
             group by c.item_no, a.item_no , a.unit_no, a.item_size,                            
                      a.item_name, b.branch_no,b.d_branch_no,
                    --b.sheet_no,     --按店汇总数量时不能把单号分组
                      c.order_qty,c.order_qty,a.main_supcust,a.price, a.sale_price,a.purchase_tax        
            /* having               --qqjMark       直接为要货单数量，不考虑库存量
         --Ceiling((sum(isnull(c.real_qty,0)-isnull(c.order_qty,0))-isnull((Select isnull(sum(stock_qty),0) 
         --from t_im_branch_stock st where st.item_no = a.item_no and st.branch_no like left(b.branch_no,4)+'%'),0)))>0       
         --and 
        ((sum(isnull(c.real_qty,0)-isnull(c.order_qty,0))-isnull((Select isnull(sum(stock_qty),0)from t_im_branch_stock st where st.item_no = a.item_no 
          and st.branch_no in (select branch_no from t_bd_branch_info where branch_no like  left(b.d_branch_no,4)+'%' and trade_type='5' and len(branch_no)=6 )),0))) >0   */                           
) tbs1 group by item_no,item_size,item_name,purchase_spec,unit_no,branch_no,main_supcusts, 
	main_supcust ,d_branch_no,allstock_qty,valid_price,sale_price,tax,stock_qty,sale_qnty

   UNION 
select item_no,item_size,item_name,purchase_spec, unit_no,  
	--LARGE_QTY=(( (sum(real_qty) - allstock_qty )/isnull(avg(purchase_spec),1))), 		--qqjMark
	LARGE_QTY=(( (sum(real_qty) )/isnull(avg(purchase_spec),1))), 		--qqjMark			直接为要货单数量，不考虑库存量
	branch_no,sum(real_qty) as real_qty,main_supcusts, main_supcust ,d_branch_no,allstock_qty ,    
	--(sum(real_qty) - allstock_qty) as qty,		 		--qqjMark
	(sum(real_qty) ) as qty,		 		--qqjMark				直接为要货单数量，不考虑库存量
	valid_price,sale_price,  
	(sum(real_qty) - allstock_qty) * valid_price as  sub_amt, 
	tax, sum(order_qty) as order_qty,stock_qty,               
	sale_qnty from (                                                                                     
   SELECT c.item_no as item_no,  a.item_size,  a.item_name, 
            purchase_spec=isnull(avg(a.purchase_spec),1), 
            a.unit_no,  
            LARGE_QTY=(((sum(c.real_qty-isnull(c.order_qty,0))-isnull((Select isnull(sum(stock_qty),0)from t_im_branch_stock st where st.item_no = a.item_no 	
           and st.branch_no in (select branch_no from t_bd_branch_info where branch_no like  left(b.d_branch_no,4)+'%' and trade_type='5')),0))/isnull(avg(a.purchase_spec),1))), 
            branch_no=(select top 1 branch_no from t_bd_branch_info where branch_no  like left(b.d_branch_no,4)+'%'   and trade_type='5'),  
            ((sum(c.real_qty-isnull(c.order_qty,0))))as real_qty,a.main_supcust as main_supcusts, 
            main_supcust = (select sup_name from t_bd_supcust_info where supcust_no=a.main_supcust and supcust_flag='S'),
            b.d_branch_no,
            allstock_qty = (Select isnull(sum(stock_qty),0)from t_im_branch_stock st where st.item_no = a.item_no 
           and st.branch_no in (select branch_no from t_bd_branch_info where branch_no like  left(b.d_branch_no,4)+'%' and trade_type='5')) ,    
           ((sum(isnull(c.real_qty,0)-isnull(c.order_qty,0))-isnull((Select isnull(sum(stock_qty),0)from t_im_branch_stock st where st.item_no = a.item_no 
           and st.branch_no in (select branch_no from t_bd_branch_info where branch_no like  left(b.d_branch_no,4)+'%' and trade_type='5')),0)))
            as qty,
            a.price as valid_price,a.sale_price,  
            round( a.price * ((sum(c.real_qty-isnull(c.order_qty,0))-isnull((Select isnull(sum(stock_qty),0) 
            from t_im_branch_stock st where st.item_no = a.item_no and st.branch_no like left(b.d_branch_no,4)+'%'),0))),2)as sub_amt, 
            a.purchase_tax as tax,   
            isnull(c.order_qty,0) as order_qty ,
            stock_qty=isnull((select sum(isnull(target.max_qty,0))
            from t_im_stock_target target where  target.item_no =c.item_no and target.branch_no like left(b.d_branch_no,4)+'%' ),0),               
           	(SELECT  isnull(sum(isnull(d.so_qty + d.pos_qty - d.pos_ret_qty+ d.pos_send_qty,0)),0)
           	 FROM t_da_jxc_daysum d
           		WHERE
           		d.item_no = a.item_no  and 
           		CONVERT(varchar(10), d.oper_date, 20) BETWEEN  @salebeginDate and @saleendDate   and
           		d.branch_no = b.d_branch_no	 and
           		d.item_no =c.item_no )as sale_qnty
                FROM t_bd_item_info a,                
                     t_pm_sheet_master b,               
                     t_pm_sheet_detail c
                WHERE b.sheet_no=c.sheet_no and a.item_stock='1' and    
                     c.item_no=a.item_no and   ( (dbo.fn_get_item_saleflag(a.item_no,'sale_flag',b.d_branch_no  )) not in ('3', '2') ) AND                                   
            		 b.approve_flag = '1'   and                                       
                     (a.item_clsno like  @itemClsno + '%'  ) and 
                     (a.item_brand like @itemBrand +'%' ) and 
                     ( b.trans_no = 'DY') and (b.order_status <> '5') and                                                                    
                     convert(char(10), b.valid_date, 112)  >= convert(char(10), getdate(), 112) and      
                     isnull(c.real_qty,0) > isnull(c.order_qty,0) 
            	     and(convert(char(10),b.work_date,20) between @needbeginDate and @needendDate  )   
             		 and b.d_branch_no  like left(@branchno,4)+'%'   


                     --and a.main_supcust  like left(@supcustNo,4)+'%'      --qqjMark       思讯的bug
                     and a.main_supcust  like @supcustNo +'%'


                     and (c.item_no not in (select item_no from t_im_branch_stock where branch_no like left(b.d_branch_no,4)+'%' and len(branch_no)=6))     {0}                             
             group by c.item_no, a.item_no , a.unit_no, a.item_size,                            
                      a.item_name, branch_no,b.d_branch_no, 
                    --b.sheet_no,       --按店汇总数量时不能把单号分组
                      c.order_qty,c.order_qty,a.main_supcust,a.price, a.sale_price,a.purchase_tax    
) tbs1 group by item_no,item_size,item_name,purchase_spec,unit_no,branch_no,main_supcusts, 
	main_supcust ,d_branch_no,allstock_qty,valid_price,sale_price,tax,stock_qty,sale_qnty
            )aa 
            group by item_no,item_size,item_name, 
            purchase_spec, 
            unit_no,    
            branch_no,   main_supcusts, 
            main_supcust ,
            d_branch_no,
            allstock_qty ,   
            valid_price,sale_price,   
            tax,    
            stock_qty,               
           	sale_qnty
            order by   main_supcust,branch_no, item_no

"

            , str2, str2).ToString(), this.GetParams_qqj(needbegindate, needenddate, salebegindate, saleenddate, branchNo, itemClsno, itemBrand, supcustNo));
    }


    public SqlParameter[] GetParams_qqj(string needbegindate, string needenddate, string salebegindate, string saleenddate, string branchNo, string itemClsno, string itemBrand, string supcustNo)
    {
        SqlParameter[] parameterArray = new SqlParameter[] { new SqlParameter("@needbeginDate", SqlDbType.VarChar), new SqlParameter("@needendDate", SqlDbType.VarChar), new SqlParameter("@salebeginDate", SqlDbType.VarChar), new SqlParameter("@saleendDate", SqlDbType.VarChar), new SqlParameter("@branchno", SqlDbType.VarChar, 30), new SqlParameter("@itemClsno", itemClsno), new SqlParameter("@itemBrand", itemBrand), new SqlParameter("@supcustNo", SqlDbType.VarChar) };
        parameterArray[0].Value = needbegindate;
        parameterArray[1].Value = needenddate;
        parameterArray[2].Value = salebegindate;
        parameterArray[3].Value = saleenddate;
        parameterArray[4].Value = branchNo;
        parameterArray[5].Value = itemClsno;
        parameterArray[6].Value = itemBrand;
        parameterArray[7].Value = supcustNo;
        return parameterArray;
    }











    private void setButtonsStatus(string status)
    {
        switch (status)
        {
            case "1":
                this.btnNext.Enabled = true;
                this.btnPre.Visible = false;
                this.btnClose.Visible = false;
                this.DivSetup.Visible = true;
                this.DivSetup1.Visible = false;
                this.btnDelect.Visible = false;
                this.btnExport.Visible = false;
                this.txtlevel.Text = "1";
                this.btnNextEnd.Visible = false;
                this.btnNext.Visible = true;
                //this.txtBranchDate.Text = DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd");
                //this.txtBranchDate.Text2 = DateTime.Now.AddDays(1.0).ToString("yyyy-MM-dd");
                //this.txtSaleDate.Text = DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd");
                //this.txtSaleDate.Text2 = DateTime.Now.AddDays(1.0).ToString("yyyy-MM-dd");
                this.rdoCheck.SelectedIndex = 0;    
                this.gvDetail.DtSource = null;
                this.gvDetail.ResetEmptyData();
                this.gvOrderList.DtSource = null;
                this.gvOrderList.ResetEmptyData();
                this.btnNextEnd.Enabled = true;
                this.lblHead.Text = "采购向导";
                this.txtBranchDate.ctlEnabled = !this.rdoCheck.SelectedValue.Equals("按补货分析采购");
                this.txtSaleDate.ctlEnabled = !this.rdoCheck.SelectedValue.Equals("按补货分析采购");
                this.btnSet.Visible = false;
                return;

            case "2":
                this.btnNextEnd.Visible = true;
                this.btnNext.Visible = false;
                if ((this.rdoCheck.SelectedValue.Trim() != "") && (this.rdoCheck.SelectedValue == "按补货分析采购"))
                {
                    this.gvRptAnalysis.Visible = true;
                    this.gvDetail.Visible = false;
                }
                else
                {
                    this.gvDetail.Visible = true;
                    this.gvRptAnalysis.Visible = false;
                }
                this.DivSetup.Visible = false;
                this.DivSetup1.Visible = true;
                this.btnPre.Visible = true;
                this.btnClose.Visible = true;
                this.gvOrderList.Visible = false;
                this.btnDelect.Visible = true;
                this.btnExport.Visible = true;
                this.txtlevel.Text = "2";
                this.btnNextEnd.Enabled = true;
                this.lblHead.Text = "商品采购列表";
                this.gvOrderList.DtSource = null;
                this.gvOrderList.ResetEmptyData();
                this.btnSet.Visible = true;
                break;

            case "3":
                this.gvOrderList.Visible = true;
                this.gvDetail.Visible = false;
                this.gvRptAnalysis.Visible = false;
                this.btnPre.Visible = false;
                this.btnNext.Enabled = false;
                this.btnNextEnd.Visible = false;
                this.btnDelect.Visible = false;
                this.btnExport.Visible = false;
                this.btnClose.Visible = true;
                this.txtlevel.Text = "3";
                this.btnNextEnd.Enabled = true;
                this.gvDetail.DtSource = null;
                this.gvDetail.ResetEmptyData();
                this.gvRptAnalysis.DtSource = null;
                this.gvRptAnalysis.ResetEmptyData();
                this.btnSet.Visible = false;
                break;
        }
    }

    public bool SheetSetting()
    {
        string frmId = base.Request.QueryString["menuId"].ToString();
        string operId = BasePage.GVar.OperId;
        GridViewEx gvDetail = this.gvDetail;
        ColumnsSetting.CheckSettings(frmId, 1, operId, gvDetail);
        ScriptManager.RegisterStartupScript(this.btnSet, base.GetType(), "SHOW", "<script>var iWidth=550;var iHeight=430;var iTop = (window.screen.availHeight-30-iHeight)/2;var iLeft = (window.screen.availWidth-10-iWidth)/2; var val=window.showModalDialog('" + string.Concat(new object[] { "../../CommonPage/ColumnSet.aspx?sheetflg=1&t=", DateTime.Now.Ticks.ToString(), "&frm_id=", frmId, "&table_id=", 1, "&oper_id=", operId }) + "', window, 'dialogHeight='+iHeight+'px; dialogWidth='+iWidth+'px; center=yes; scroll=no; resizable=no; status=no');var setColumnsCallback = function(val){if(val){bind();}}</script>", false);
        return true;
    }

    //protected global_asax ApplicationInstance
    //{
    //    get
    //    {
    //        return (global_asax) this.Context.ApplicationInstance;
    //    }
    //}

    //protected DefaultProfile Profile
    //{
    //    get
    //    {
    //        return (DefaultProfile) this.Context.Profile;
    //    }
    //}
}

