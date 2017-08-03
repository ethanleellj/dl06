using SISS.ISHOP4.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;


public partial class QQJFroDL_CombineDYToPrint : System.Web.UI.Page
{
    public string SheetNO, menuId, msg;
    public DataTable dt01;
    public string needbegindate;
    public string needenddate;
    public string itemClsno;
    public string itemBrand;
    public string branchNo;
    public string supcustNo;
    public string rdoCheck, salebegindate, saleenddate, qqjTempAlertString, sWarehouseKeeper;
    

    protected void Page_Load(object sender, EventArgs e)
    {
        SheetNO = String.IsNullOrEmpty(Request.QueryString["sheet_no"]) ? "" : Request.QueryString["sheet_no"].Replace("'", "");

        menuId = String.IsNullOrEmpty(Request.QueryString["menuId"]) ? "" : Request.QueryString["menuId"];

        if (menuId.Contains("A1"))      //拣货向导
        {
            needbegindate = String.IsNullOrEmpty(Request.QueryString["needbegindate"]) ? "" : Request.QueryString["needbegindate"].Replace("'", "");
            needenddate = String.IsNullOrEmpty(Request.QueryString["needenddate"]) ? "" : Request.QueryString["needenddate"].Replace("'", "");
            itemClsno = String.IsNullOrEmpty(Request.QueryString["itemClsno"]) ? "%" : Request.QueryString["itemClsno"].Replace("'", "");
            itemBrand = String.IsNullOrEmpty(Request.QueryString["itemBrand"]) ? "%" : Request.QueryString["itemBrand"].Replace("'", "");
            branchNo = String.IsNullOrEmpty(Request.QueryString["branchNo"]) ? "%" : Request.QueryString["branchNo"].Replace("'", "");
            supcustNo = String.IsNullOrEmpty(Request.QueryString["supcustNo"]) ? "%" : Request.QueryString["supcustNo"].Replace("'", "");
            rdoCheck = String.IsNullOrEmpty(Request.QueryString["rdoCheck"]) ? "1" : Request.QueryString["rdoCheck"].Replace("'", "");


            if (branchNo == "ALL") { branchNo = "%"; }
        }






        if (menuId == "2202" && SheetNO == "")
        {
            //Response.Write("<script>alert('采购订单单号为空！');</script>");
            qqjTempAlertString = "采购订单单号为空！";
        }
        else
        {
            if (!IsPostBack)
            {
               

                StringBuilder tempSQL = new StringBuilder();
                tempSQL.Append(@"
SELECT        t1.branch_no, t1.branch_name + ISNULL(t2.selected, '') AS branch_name
FROM            (SELECT        branch_no, branch_name
                          FROM            t_bd_branch_info
                          WHERE        (LEN(branch_no) = 4)) AS t1 FULL OUTER JOIN


");

                if (menuId == "2202")
                {
                    tempSQL.Append(@"


                             (SELECT DISTINCT t3.branch_no, '@' AS selected
                               FROM            t_pm_sheet_master AS t3 INNER JOIN
                                                         t_pm_sheet_detail AS t4 ON t3.sheet_no = t4.sheet_no
                               WHERE        (t4.item_no IN
                                                             (SELECT        item_no
                                                               FROM            t_pm_sheet_detail
                                                               WHERE        (sheet_no = '" + SheetNO + @"'))) AND (t3.sheet_no IN
                                                             (SELECT        vSheetNO
                                                               FROM            tBoxPurchaseGuideDYDetail
                                                               WHERE        (pktBoxPurchaseGuideMaster =
                                                                                             (SELECT        TOP (1) pktBoxPurchaseGuideMaster
                                                                                               FROM            tBoxPurchaseGuidePODetail
                                                                                               WHERE        (vSheetNO = '" + SheetNO + @"')))))





) AS t2 ON t1.branch_no = t2.branch_no");

                }
                else if (menuId.Contains("A1"))
                {

                    tempSQL.Append(@"



                             (
select distinct b.branch_no, '@' AS selected
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
    and	b.d_branch_no  like left(@branchno,4)+'%'  




) AS t2 ON t1.branch_no = t2.branch_no");

                }



                //dt01 = DBHelper.GetData(tempSQL.ToString());
                dt01 = DBHelper.GetData(tempSQL.ToString(), GetParams_qqj(needbegindate, needenddate, salebegindate, saleenddate, branchNo, itemClsno, itemBrand, supcustNo, rdoCheck));


                CheckBoxListBranch.DataSource = dt01;
                CheckBoxListBranch.DataTextField = "branch_name";
                CheckBoxListBranch.DataValueField = "branch_no";
                CheckBoxListBranch.DataBind();

                bool bGeneraFromPurchaseGuide2 = false;
                foreach (ListItem item in CheckBoxListBranch.Items)
                {
                    item.Selected = item.Text.Contains("@");
                    if (bGeneraFromPurchaseGuide2 == false)
                    {
                        bGeneraFromPurchaseGuide2 = item.Text.Contains("@");
                    }
                }

                if (bGeneraFromPurchaseGuide2 == false && menuId == "2202")
                {
                    //Response.Write("<script>alert('该采购订单并不是通过采购向导2生成！');</script>");
                    qqjTempAlertString = "该采购订单并不是通过采购向导2生成！";
                }






                if (menuId == "2202")
                {
                    DataTable dt03 = DBHelper.GetData(@"
SELECT        sheet_no, t2.sup_name, CONVERT(varchar(100), t1.oper_date, 23) as oper_date, t2.supcust_no
FROM            t_pm_sheet_master  t1 inner join  t_bd_supcust_info t2 on t1.supcust_no = t2.supcust_no and  (t2.supcust_flag = 'S')
WHERE        (sheet_no = '" + SheetNO + "')");

                    foreach (DataRow dr in dt03.Rows)
                    {
                        TextBoxSheetNo.Text = dr["sheet_no"].ToString();
                        TextBoxSupName.Text = dr["sup_name"].ToString();
                        TextBoxSupcustNo.Text = dr["supcust_no"].ToString();
                        TextBoxOperDate.Text = dr["oper_date"].ToString();

                    }
                }


            }
            GeneraData();


        }
    }


    public SqlParameter[] GetParams_qqj(string needbegindate, string needenddate, string salebegindate, string saleenddate, string branchNo, string itemClsno, string itemBrand, string supcustNo, string rdoCheck)
    {
        SqlParameter[] parameterArray = new SqlParameter[] { new SqlParameter("@needbeginDate", SqlDbType.VarChar), new SqlParameter("@needendDate", SqlDbType.VarChar), new SqlParameter("@salebeginDate", SqlDbType.VarChar), new SqlParameter("@saleendDate", SqlDbType.VarChar), new SqlParameter("@branchno", SqlDbType.VarChar, 30), new SqlParameter("@itemClsno", itemClsno), new SqlParameter("@itemBrand", itemBrand), new SqlParameter("@supcustNo", SqlDbType.VarChar), new SqlParameter("@rdoCheck", SqlDbType.VarChar) };
        parameterArray[0].Value = needbegindate;
        parameterArray[1].Value = needenddate;
        parameterArray[2].Value = salebegindate;
        parameterArray[3].Value = saleenddate;
        parameterArray[4].Value = branchNo;
        parameterArray[5].Value = itemClsno;
        parameterArray[6].Value = itemBrand;
        parameterArray[7].Value = supcustNo;
        parameterArray[8].Value = rdoCheck;
        return parameterArray;
    }

    public void GeneraData()
    {
        
        //List<ListItem> liSelected = CheckBoxListBranch.Items.Cast<ListItem>().Where(li => li.Selected).ToList();

        bool bHaveSelectItem = false;

        foreach (ListItem item in CheckBoxListBranch.Items)
        {
            if (item.Selected)
            {
                bHaveSelectItem = true;

            }
        }



        if (bHaveSelectItem == false && false)
        {
            //Response.Write("<script>alert('未选择任何的门店！');</script>");
            qqjTempAlertString = "未选择任何的门店！";

        }
        else
        {

            StringBuilder sbSqlALL = new StringBuilder();
            StringBuilder sbSelectHead = new StringBuilder();
            StringBuilder sbSqlFoot = new StringBuilder();

            StringBuilder sbWarehouseKeeper = new StringBuilder();

            sbSelectHead.Append("[序号_0, 商品名称_0, 货号_0, 参考价_0, 单位_0, 总订量_0, 毛重_0");

            sbSqlALL.Append(@"
declare @sheet_no varchar(50), @branch_no_CombineDYPrintCosider varchar(50)
select @sheet_no =  '" + SheetNO + @"'
SELECT  @branch_no_CombineDYPrintCosider = vValue FROM  tBoxPlanParameter WHERE (pktBoxPlanParameter = 'WarehouseNoCombineDYPrintCosider')");




            if (menuId.Contains("A1") && rdoCheck=="2")
            {


                sbSqlALL.Append(@"
select 序号, 货号 , 商品名称, 单位, 参考价 , 毛重
,  case when 总订量 > isnull([dbo].[fn_get_stock_qty](@branch_no_CombineDYPrintCosider, 货号 ),0) then isnull([dbo].[fn_get_stock_qty](@branch_no_CombineDYPrintCosider, 货号 ),0) else 总订量 end as  总订量

");


                foreach (ListItem item in CheckBoxListBranch.Items)
                {
                    if (item.Selected)
                    {
                        sbSqlALL.Append(", case when 总订量 > isnull([dbo].[fn_get_stock_qty](@branch_no_CombineDYPrintCosider, 货号 ),0) then  [" + item.Text + "]  * isnull([dbo].[fn_get_stock_qty](@branch_no_CombineDYPrintCosider, 货号 ),0) /总订量 else [" + item.Text + "] end as  [" + item.Text + "] ");


                    }
                }
                sbSqlALL.Append(@"

from (

");

            }





            sbSqlALL.Append(@"
		select row_number() over (order by 货号)序号, 货号 , 商品名称, 单位, 参考价 , '' as 毛重,
		sum(总订量) as 总订量
");
            foreach (ListItem item in CheckBoxListBranch.Items)
            {
                if (item.Selected)
                {
                    sbSqlALL.Append(",sum(case 店名 when '" + item.Value + "' then 总订量 else 0 end) as [" + item.Text + "]");
                    sbSqlFoot.Append(",'" + item.Value + "'");
                    sbSelectHead.Append("," + item.Text + "_" + item.Value);

                    sbWarehouseKeeper.Append(item.Text.Replace("@","") + "库管:          ");

                }
            }

            sWarehouseKeeper = sbWarehouseKeeper.ToString();


            sbSqlALL.Append(@"
		from (
		SELECT ii.item_name 商品名称,sm.item_no 货号,ii.unit_no 单位,ii.price
		参考价,bi.branch_no 店名,sum(sm.real_qty) 总订量
		FROM ");






            if (menuId == "2202")
            {
                sbSqlALL.Append(@"


(SELECT branch_no, item_no, real_qty
                               FROM            t_pm_sheet_master AS t3 INNER JOIN
                                                         t_pm_sheet_detail AS t4 ON t3.sheet_no = t4.sheet_no
                               WHERE        (t4.item_no IN
                                                             (SELECT        item_no
                                                               FROM            t_pm_sheet_detail
                                                               WHERE        (sheet_no = @sheet_no ))) AND (t3.sheet_no IN
                                                             (SELECT        vSheetNO
                                                               FROM            tBoxPurchaseGuideDYDetail
                                                               WHERE        (pktBoxPurchaseGuideMaster =
                                                                                             (SELECT        TOP (1) pktBoxPurchaseGuideMaster
                                                                                               FROM            tBoxPurchaseGuidePODetail
                                                                                               WHERE        (vSheetNO = @sheet_no )))))) 




");
            }
            else if (menuId.Contains("A1"))
            {

                sbSqlALL.Append(@"


(

select  b.branch_no,  c.item_no, c.real_qty
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
    and	b.d_branch_no  like left(@branchno,4)+'%'  






) 




");
            }






            sbSqlALL.Append(@"
sm,

		t_bd_item_info ii,
		t_bd_branch_info bi
		where   sm.item_no = ii.item_no
		and sm.branch_no = bi.branch_no
		--and sm.order_id = @sheet_no
        and bi.branch_no in ( ''
");

            sbSqlALL.Append(sbSqlFoot.ToString());

            sbSqlALL.Append(@")
		group by ii.item_name,sm.item_no,ii.unit_no,ii.price,bi.branch_no )t
		group by
		商品名称, 单位, 参考价,货号
");



            if (menuId.Contains("A1") && rdoCheck == "2")
            {


                sbSqlALL.Append(@"  ) t66  ");



            }


            DataTable dt02 = DBHelper.GetData(sbSqlALL.ToString(), GetParams_qqj(needbegindate, needenddate, salebegindate, saleenddate, branchNo, itemClsno, itemBrand, supcustNo, rdoCheck));
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();


            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row;
            foreach (DataRow dr in dt02.Rows)
            {
                row = new Dictionary<string, object>();
                foreach (DataColumn col in dt02.Columns)
                {
                    row.Add(col.ColumnName, dr[col]);
                }
                rows.Add(row);
            }
            TextBoxTableBody.Text = serializer.Serialize(rows);

            if (string.IsNullOrEmpty(TextBoxTableBody.Text))
            { TextBoxTableBody.Text = "['']"; }


            sbSelectHead.Append("]");
            TextBoxSelectHead.Text = sbSelectHead.ToString();

            if (menuId.Contains("A1"))
            {
                TextBoxTableBody.Text = TextBoxTableBody.Text.Replace("总订量","总拣量" );
                TextBoxSelectHead.Text = TextBoxSelectHead.Text.Replace("总订量", "总拣量");

            }

            //Response.Write(sbSqlALL.ToString());


        }

    }
    protected void btnQuery_Click(object sender, EventArgs e)
    {
        GeneraData();
    }
}