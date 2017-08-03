using ASP;
using SISS.Common;
using SISS.ISHOP4.BLL;
using SISS.ISHOP4.DAL;
using SISS.ISHOP4.MODEL;
using SISS.WebControl;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI.WebControls;

using SISS.ISHOP4.DAL;
using SISS.ISHOP4.MODEL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;


public partial class RptElectronicScaleDataProcessing_qqj : BasePage, ICommPrint, IRequiresSessionState
{
    //protected DropDownList ddlSaleWay;
    //protected GridViewEx gvRpt;
    //protected ImageButton ImageButton2;
    //protected TextBox TextBox2;
    //protected Controls_UCBranchNo txtBranchArea;
    //protected Controls_UCBaseCode txtBrand;
    //protected TextBox txtGMMan;
    //protected TextBox txtItemName;
    //protected TextBox txtItemNo;
    //protected TextBox txtPosNo;
    //protected Controls_UCBaseCode txtSaleman;
    //protected TextBox txtSheetNo;
    //protected TextBox txtVipNo;
    //protected Controls_UCBaseCode ucItemCls;
    //protected Controls_UCOperDateWithTime ucOperDateStart;
    //protected Controls_UCBaseCode ucSuppliert;
    //protected Controls_UCBranchNo ucWarehouse;

    //string sGenerateSheet="";

    private Dictionary<string, string> GetConditions()
    {
        Dictionary<string, string> conditions = new Dictionary<string, string>();
        if (!string.IsNullOrEmpty(this.ucSuppliert.objNo.Text.Trim()))
        {
            conditions.Add("supcust_no", this.ucSuppliert.objNo.Text.Trim());
        }
        if (!string.IsNullOrEmpty(this.ucItemCls.objNo.Text.Trim()))
        {
            conditions.Add("item_clsno", this.ucItemCls.objNo.Text.Trim());
        }
        if (!string.IsNullOrEmpty(this.ucOperDateStart.Text.Trim()))
        {
            conditions.Add("oper_date_start", this.ucOperDateStart.Text.Trim());
        }
        if (!string.IsNullOrEmpty(this.ucOperDateStart.Text2.Trim()))
        {
            conditions.Add("oper_date_end", this.ucOperDateStart.Text2.Trim());
        }
        if (!string.IsNullOrEmpty(this.txtVipNo.Text.Trim()))
        {
            conditions.Add("card_no", this.txtVipNo.Text.Trim());
        }
        if (!string.IsNullOrEmpty(this.txtGMMan.Text.Trim()))
        {
            conditions.Add("oper_id", this.txtGMMan.Text.Trim());
        }
        if (!string.IsNullOrEmpty(this.txtSaleman.objNo.Text.Trim()))
        {
            conditions.Add("sale_man", this.txtSaleman.objNo.Text.Trim());
        }
        if (!string.IsNullOrEmpty(this.ucWarehouse.objNo.Text.Trim()))
        {
            conditions.Add("branch_no", this.ucWarehouse.objNo.Text.Trim());
        }
        if (!string.IsNullOrEmpty(this.txtBrand.objNo.Text.Trim()))
        {
            conditions.Add("item_brand", this.txtBrand.objNo.Text.Trim());
        }
        if (!string.IsNullOrEmpty(this.txtSheetNo.Text.Trim()))
        {
            conditions.Add("flow_no", this.txtSheetNo.Text.Trim());
        }
        if (!string.IsNullOrEmpty(this.txtItemName.Text.Trim()))
        {
            conditions.Add("item_name", this.txtItemName.Text.Trim());
        }
        if (!string.IsNullOrEmpty(this.txtItemNo.Text.Trim()))
        {
            conditions.Add("item_no", this.txtItemNo.Text.Trim());
        }
        if (!string.IsNullOrEmpty(this.ddlSaleWay.SelectedValue))
        {
            conditions.Add("sell_way", this.ddlSaleWay.SelectedValue);
        }
        if (!string.IsNullOrEmpty(this.txtBranchArea.objNo.Text.Trim()))
        {
            conditions.Add("branch_clsno", this.txtBranchArea.objNo.Text.Trim());
        }
        if (!string.IsNullOrEmpty(this.txtPosNo.Text.Trim()))
        {
            conditions.Add("PosNo", this.txtPosNo.Text.Trim());
        }


        //qqjMark
        if (!string.IsNullOrEmpty(sGenerateSheet.Value))
        {
            conditions.Add("sGenerateSheet", sGenerateSheet.Value);
        }

        base.GetDataGrantConditions(ref conditions);
        return conditions;
    }

    protected void gvRpt_Bind()
    {
        ColumnsSetting.GetSettings(base.Request["menuId"].Trim(), 1, BasePage.GVar.OperId, this.gvRpt);
        PagingBase pagingModel = ColumnsSetting.GetPagingModel(this.gvRpt);
        TextBox box = base.Master.FindControl("txtFiltrate") as TextBox;
        if (!string.IsNullOrEmpty(box.Text))
        {
            pagingModel.FiltrateWhere = base.CreateFiltrateWhere(box.Text);
        }
        Dictionary<string, string> conditions = this.GetConditions();
        new RptQueryBll_qqj().GetItemSaleFlow(pagingModel, conditions);         //qqjMark
        ColumnsSetting.SetPagingModel(pagingModel, this.gvRpt);
    }

    protected void gvRpt_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        //if (e.Row.RowType == DataControlRowType.DataRow)
        //{
        //    int colIndex = -1;
        //    colIndex = this.gvRpt.GetColIndex("sell_way");
        //    if (colIndex >= 0)
        //    {
        //        if (e.Row.Cells[colIndex].Text.Replace(" ", string.Empty).Replace("&nbsp;", string.Empty) == "退货")
        //        {
        //            e.Row.Cells[this.gvRpt.GetColIndex("sell_way")].ForeColor = Color.Red;
        //        }
        //        else if (e.Row.Cells[colIndex].Text.Replace(" ", string.Empty).Replace("&nbsp;", string.Empty) == "赠送")
        //        {
        //            e.Row.Cells[this.gvRpt.GetColIndex("sell_way")].ForeColor = Color.Blue;
        //        }
        //        else
        //        {
        //            e.Row.Cells[this.gvRpt.GetColIndex("sell_way")].ForeColor = Color.Black;
        //        }
        //    }
        //    colIndex = this.gvRpt.GetColIndex("flow_no");
        //    string str = (e.Row.DataItem as DataRowView)["flow_no"].ToString().Trim();
        //    if ((colIndex >= 0) && (str.Trim() != ""))
        //    {
        //        ((HyperLink)e.Row.Cells[colIndex].Controls[0]).NavigateUrl = "javascript:GetPosPreview('" + str + "')";
        //        ((HyperLink)e.Row.Cells[colIndex].Controls[0]).Style.Add("text-decoration", "underline");
        //        ((HyperLink)e.Row.Cells[colIndex].Controls[0]).Attributes.Add("onmouseover", "this.style.color='red';");
        //        ((HyperLink)e.Row.Cells[colIndex].Controls[0]).Attributes.Add("onmouseout", "this.style.color='black';");
        //    }
        //}
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!base.IsPostBack)
        {
            //foreach (BoundFieldEx ex in base.GetItemProColumn())
            //{
            //    this.gvRpt.Columns.Add(ex);
            //}
            this.ddlSaleWay.Items.Add(new ListItem("全部", "%"));
            this.ddlSaleWay.Items.Add(new ListItem("销售", "A"));
            this.ddlSaleWay.Items.Add(new ListItem("退货", "B"));
            this.ddlSaleWay.Items.Add(new ListItem("赠送", "C"));
            this.ddlSaleWay.SelectedValue = "%";
            this.ucOperDateStart.Text = DateTime.Now.AddDays(-7.0).ToString("yyyy-MM-dd") + " 00:00:00";
            this.ucOperDateStart.Text2 = DateTime.Now.ToString("yyyy-MM-dd") + " 23:59:59";
            //if (BasePage.GVar.LoginBranchNo != "0000")
            //{
            //    this.txtBranchArea.ctlEnabled = false;
            //    this.txtBranchArea.objImage.Attributes.Add("onclick", string.Empty);
            //}
            //if (BasePage.GVar.TradeType.Equals("0") || (CommonDAL.GetIsSupVisible(BasePage.GVar.LoginBranchNo) == "1"))
            //{
            //    BoundFieldEx field = new BoundFieldEx
            //    {
            //        HeaderText = "供应商名称"
            //    };
            //    field.HeaderStyle.Width = 120;
            //    field.DataField = "supcust_name";
            //    field.SortExpression = "supcust_name";
            //    field.ColName = "supcust_name";
            //    field.ColVisible = true;
            //    this.gvRpt.Columns.Add(field);
            //}

            string menuId = base.Request["menuId"].Trim();
            if (menuId.StartsWith("22")) { sGenerateSheet.Value = "PI"; CheckBoxGeneratePI.Visible = true; CheckBoxGeneratePI.Checked = true; }
            else if (menuId.StartsWith("43")) { sGenerateSheet.Value = "SaleFlow"; CheckBoxGenerateSaleFlow.Visible = true; CheckBoxGenerateSaleFlow.Checked = true; }
            else if (menuId.StartsWith("A1")) { sGenerateSheet.Value = "DO"; CheckBoxGenerateDO.Visible = true; CheckBoxGenerateDO.Checked = true; }
            else { sGenerateSheet.Value = ""; }
        }
        BasePage.RegisterJs("SetDivHegith(8, '', 'SheetDetailDiv');", this.Page);
    }

    public void PrintSheet(object printModel)
    {
        ((HiddenField)base.Master.FindControl("hdTableID")).Value = "";
        PagingBase pagingModel = ColumnsSetting.GetPagingModel(this.gvRpt);
        pagingModel.GeAllData = true;
        TextBox box = base.Master.FindControl("txtFiltrate") as TextBox;
        if (!string.IsNullOrEmpty(box.Text))
        {
            pagingModel.FiltrateWhere = base.CreateFiltrateWhere(box.Text);
        }
        Dictionary<string, string> conditions = this.GetConditions();
        new RptQueryBll_qqj().GetItemSaleFlow(pagingModel, conditions);     //qqjMark
        (printModel as CommReport).dtdata = pagingModel.ResultDt;
        (printModel as CommReport).OtherShowInfo = "日期:" + conditions["oper_date_start"] + " 至 " + conditions["oper_date_end"];
    }

    //protected global_asax ApplicationInstance
    //{
    //    get
    //    {
    //        return (global_asax)this.Context.ApplicationInstance;
    //    }
    //}

    //protected DefaultProfile Profile
    //{
    //    get
    //    {
    //        return (DefaultProfile)this.Context.Profile;
    //    }
    //}



    protected void btnProcessingData_Click(object sender, System.EventArgs e)
    {
        if (CheckBoxGenerateSaleFlow.Checked || CheckBoxGenerateDO.Checked || CheckBoxGeneratePI.Checked)
        {
            try
            {
                string s = "EXEC	[pBoxElectronicScaleDataProcessing]  @vGenerateSaleFlow ='" + (CheckBoxGenerateSaleFlow.Checked ? "Y" : "N") + "', @vGenerateDO ='" + (CheckBoxGenerateDO.Checked ? "Y" : "N") + "', @vGeneratePI ='" + (CheckBoxGeneratePI.Checked ? "Y" : "N") + "'";
                int i = DBHelper.ExecuteSql(s);
                BasePage.Alert("操作成功！影响" + i.ToString() + "行！", this.Page);

            }
            catch (Exception ex)
            {
                BasePage.Alert(ex.Message.Replace("'","\\'"), this.Page);
            }

        }
        else
        { BasePage.Alert("必须至少选择生成一种单据！", this.Page); }
    }
}




//下面这堆代码从  eshop4_20160726  版本中复制过来，因为  V20170425  的壳脱不掉        qqjMark


public class RptQueryBll_qqj
{
    private RptQueryDal_qqj query = new RptQueryDal_qqj();


    public PagingBase GetItemSaleFlow(PagingBase pageModel, Dictionary<string, string> conditions)
    {
        return this.query.GetItemSaleFlow(pageModel, conditions);
    }


}






public class RptQueryDal_qqj
{
    public PagingBase GetItemSaleFlow(PagingBase pageModel, Dictionary<string, string> conditions)
    {
        DateTime time;
        DateTime time2;
        List<SqlParameter> list = new List<SqlParameter>();
        Dictionary<string, string> dicWith = new Dictionary<string, string>();
        string format = @" 

SELECT      * , CASE StoreType WHEN 0 THEN '正常销售' WHEN 1 THEN '入库供应商' WHEN 2 THEN '配送门店' ELSE '其他' END AS StoreTypeName
FROM            Details
{0}
        ";



        StringBuilder builder = new StringBuilder();
        if (((conditions.ContainsKey("oper_date_start") && conditions.ContainsKey("oper_date_end")) && (conditions.ContainsKey("oper_date_start") && DateTime.TryParse(conditions["oper_date_start"], out time))) && DateTime.TryParse(conditions["oper_date_end"], out time2))
        {
            time2 = DateTime.Parse(time2.ToString("yyyy-MM-dd HH:mm:ss") + ".997");
            builder.Append(" AND ( TradeTime between @oper_date_start and @oper_date_end) ");
            list.Add(new SqlParameter("@oper_date_start", time));
            list.Add(new SqlParameter("@oper_date_end", time2));
        }

        if (conditions.ContainsKey("sGenerateSheet"))
        {
            builder.Append(@"  AND (
    (@sGenerateSheet = 'PI') AND (ScaleType = N'BlackEagle') AND (StoreType = 1) 
OR  (@sGenerateSheet = 'DO') AND (ScaleType = N'BlackEagle') AND (StoreType = 2) 
OR  (@sGenerateSheet = 'SaleFlow') AND (ScaleType = N'bTwin') AND (StoreType = 0)  AND (SellType IN (0, 2))  
)
");
            list.Add(new SqlParameter("@sGenerateSheet", conditions["sGenerateSheet"] ));


        }


        //if (conditions.ContainsKey("flow_no"))
        //{
        //    builder.Append(" AND a.flow_no LIKE @flow_no");
        //    list.Add(new SqlParameter("@flow_no", conditions["flow_no"] + "%"));
        //}
        //if (conditions.ContainsKey("oper_id"))
        //{
        //    builder.Append(" AND c.oper_id LIKE @oper_id");
        //    list.Add(new SqlParameter("@oper_id", "%" + conditions["oper_id"] + "%"));
        //}
        //if (conditions.ContainsKey("sale_man"))
        //{
        //    builder.Append(" AND isnull(a.sale_man,'') LIKE @sale_man ");
        //    list.Add(new SqlParameter("@sale_man", "%" + conditions["sale_man"] + "%"));
        //}
        //if (conditions.ContainsKey("card_no"))
        //{
        //    builder.Append(" AND isnull(c.card_no,'') LIKE @card_no");
        //    list.Add(new SqlParameter("@card_no", conditions["card_no"] + "%"));
        //}
        //if (conditions.ContainsKey("branch_no"))
        //{
        //    builder.Append(" AND a.branch_no LIKE @branch_no");
        //    list.Add(new SqlParameter("@branch_no", conditions["branch_no"] + "%"));
        //}
        //if (conditions.ContainsKey("sell_way"))
        //{
        //    builder.Append(" AND a.sell_way LIKE @sell_way");
        //    list.Add(new SqlParameter("@sell_way", conditions["sell_way"] + "%"));
        //}
        //if (conditions.ContainsKey("item_no"))
        //{
        //    builder.Append(" AND (a.item_no LIKE @item_no or b.item_subno LIKE @item_no)");
        //    list.Add(new SqlParameter("@item_no", "%" + conditions["item_no"] + "%"));
        //}
        //if (conditions.ContainsKey("PosNo"))
        //{
        //    builder.Append(" AND right(left(a.flow_no,8),4) LIKE @PosNo");
        //    list.Add(new SqlParameter("@PosNo", conditions["PosNo"]));
        //}
        //if (conditions.ContainsKey("branch_clsno"))
        //{
        //    builder.Append(" AND isnull(d.branch_clsno,'') LIKE @branch_clsno");
        //    list.Add(new SqlParameter("@branch_clsno", conditions["branch_clsno"] + "%"));
        //}
        //if (conditions.ContainsKey("supcust_no"))
        //{
        //    builder.Append(" AND isnull(b.main_supcust,'') LIKE @supcust_no");
        //    list.Add(new SqlParameter("@supcust_no", conditions["supcust_no"] + "%"));
        //}
        //if (conditions.ContainsKey("item_brand"))
        //{
        //    builder.Append(" AND isnull(b.item_brand,'') LIKE @item_brand");
        //    list.Add(new SqlParameter("@item_brand", conditions["item_brand"] + "%"));
        //}
        //if (conditions.ContainsKey("item_clsno"))
        //{
        //    builder.Append(" AND isnull(b.item_clsno,'') LIKE @item_clsno");
        //    list.Add(new SqlParameter("@item_clsno", conditions["item_clsno"] + "%"));
        //}
        //if (conditions.ContainsKey("item_name"))
        //{
        //    builder.Append(" AND b.item_name LIKE @item_name");
        //    list.Add(new SqlParameter("@item_name", "%" + conditions["item_name"] + "%"));
        //}
        //if (conditions.ContainsKey("DataGrantBranch"))
        //{
        //    builder.Append(" AND left(a.branch_no,4) IN(" + conditions["DataGrantBranch"] + ")");
        //}
        //if (conditions.ContainsKey("DataGrantType"))
        //{
        //    if (conditions["DataGrantType"].Equals("0"))
        //    {
        //        if (conditions.ContainsKey("ItemClsDataGrant"))
        //        {
        //            builder.AppendFormat(" AND LEFT(b.item_clsno,2) IN({0}) ", conditions["ItemClsDataGrant"]);
        //        }
        //    }
        //    else if (conditions["DataGrantType"].Equals("1") && conditions.ContainsKey("ItemBrandDataGrant"))
        //    {
        //        builder.AppendFormat(" AND b.item_brand IN({0})", conditions["ItemBrandDataGrant"]);
        //    }
        //}
        string str2 = (builder.Length > 0) ? string.Format(" WHERE 1=1 {0}", builder.ToString()) : "";
        string str3 = string.Format(format, str2);
        //dicWith.Add("mainWith", str3);
        return CommonDAL.PagingData(str3, " FlowNo DESC ", pageModel, list.ToArray());
    }

}
