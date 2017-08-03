<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CombineDYToPrint.aspx.cs" Inherits="QQJFroDL_CombineDYToPrint" %>

<!DOCTYPE html>

<html>
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8">
    <title>合并打印</title>
    <script type="text/javascript">
        var webpath = '/QQJFroDL';
    </script>
    <script type="text/javascript" src="<%=ResolveUrl("~") %>QQJFroDL/json2.js"></script>
    <script type="text/javascript" src="<%=ResolveUrl("~") %>QQJFroDL/jquery-1.8.3.js"></script>
    <script type="text/javascript" src="<%=ResolveUrl("~") %>QQJFroDL/LodopFuncs.js"></script>
    <script type="text/javascript" src="<%=ResolveUrl("~") %>QQJFroDL/My97DatePicker/WdatePicker.js"></script>
    <link rel="stylesheet" href="<%=ResolveUrl("~") %>QQJFroDL/My97DatePicker/skin/WdatePicker.css" type="text/css"></link>
    <!--[if !IE]><!-->
    <link type='text/css' href='<%=ResolveUrl("~") %>CSS/InComCSS.css' rel='stylesheet'>
    <!--<![endif]-->
    <!--[if gte IE 8]>
      <link  type='text/css' href='<%=ResolveUrl("~") %>CSS/InComCSS.css' rel='stylesheet' >
      <![endif]-->
    <!--[if IE 7]>
      <link  type='text/css' href='<%=ResolveUrl("~") %>CSS/InComIE7.css' rel='stylesheet' >
      <![endif]-->
    <link type='text/css' href='<%=ResolveUrl("~") %>CSS/SystemUI.css' rel='stylesheet'>
    <link type='text/css' href='<%=ResolveUrl("~") %>QQJFroDL/jquery-ui.css' rel='stylesheet'>
    <script type="text/javascript" src="<%=ResolveUrl("~") %>QQJFroDL/jquery-ui.min.js"></script>
    <object id="LODOP_OB"
        classid="clsid:2105C259-1E0C-4534-8141-A753534CB4CA" width="0" height="0">
        <embed id="LODOP_EM" type="application/x-print-lodop" width="0" height="0"
            pluginspage="'<%=ResolveUrl("~") %>Controls/Activex/install_lodop32.exe"></embed>
    </object>
    <style id="styleMain">
        table, th, td {
            /*border: 1px solid Silver;*/
            border: 1px solid black;

            border-collapse: collapse;
            border-spacing: 0px;
            /*border-color: Silver;*/
            color: #000000;
            padding: 2px;
        }

        td, th {
            font-size: 13px;
        }

        th {
            background-color: #E8E8E8;
        }
    </style>
</head>
<body>
    <div style="height: 100px">
        <form id="queryFormId" class="hpseach_map" runat="server">
            <div id="ToolBar" class="ToolBarInfo">
                <div class="ToolBarBacking" >
                    <a href="#" onclick="createOrderFun();return false;" style="display: none;">
                        <input
                            id="createOrder" style="float: left;" type="image" value="保存采购单"
                            src="<%=ResolveUrl("~") %>Images/flow_Toolbar_Add.png" /><span
                                style="float: left; line-height: 30px;">&nbsp;保存
                  &nbsp;| &nbsp;</span> </a>






                        <button onclick="prn1_print();return false;" id="exportBtn" type="button" class="ToolBarButton" commandname="Print" style="width: 100px;">
                            <span>&nbsp;</span>
                            <img alt="直接打印" src="<%=ResolveUrl("~") %>/Images/flow_Toolbar_Print.png" class="ToolBarImgs"
                                 />
                            <span class="ToolBarFont">直接打印</span> <span style="border-right: solid 1px #ccc">&nbsp;</span>
                        </button>


                        <button onclick="prn1_preview();return false;" id="printBtn" type="button" class="ToolBarButton" commandname="Print" style="width: 100px;">
                            <span>&nbsp;</span>
                            <img alt="打印预览" src="<%=ResolveUrl("~") %>/Images/flow_Toolbar_Print.png" class="ToolBarImgs"
                                 />
                            <span class="ToolBarFont">打印预览</span> <span style="border-right: solid 1px #ccc">&nbsp;</span>
                        </button>


                        <button onclick="prn1_printA();return false;" id="printChoose" type="button" class="ToolBarButton" commandname="Print" style="width: 110px;">
                            <span>&nbsp;</span>
                            <img alt="选择打印机" src="<%=ResolveUrl("~") %>/Images/flow_Toolbar_Set.png" class="ToolBarImgs"
                                 />
                            <span class="ToolBarFont">选择打印机</span> <span style="border-right: solid 1px #ccc">&nbsp;</span>
                        </button>
            

                        <button onclick="btnQuery22_click();" id="btnQuery22" type="button" class="ToolBarButton" commandname="Print" style="width: 80px;">
                            <span>&nbsp;</span>
                            <img alt="查询" src="<%=ResolveUrl("~") %>/Images/flow_Toolbar_Search.png" class="ToolBarImgs"
                                 />
                            <span class="ToolBarFont">查询</span> <span style="border-right: solid 1px #ccc">&nbsp;</span>
                        </button>



                        <button onclick="btnSelectAllBranch_click();" id="btnSelectAllBranch" type="button" class="ToolBarButton" commandname="Print" style="width: 100px;">
                            <span>&nbsp;</span>
                            <img alt="分店全选" src="<%=ResolveUrl("~") %>/Images/flow_Toolbar_Print.png" class="ToolBarImgs"
                                 />
                            <span class="ToolBarFont">分店全选</span> <span style="border-right: solid 1px #ccc">&nbsp;</span>
                        </button>



                      <button onclick="btnReturn_click();" id="btnReturn" type="button" class="ToolBarButton" commandname="Print" style="width: 70px;">
                            <span>&nbsp;</span>
                            <img alt="返回" src="<%=ResolveUrl("~") %>/Images/flow_Toolbar_Return.png" class="ToolBarImgs"
                                 />
                            <span class="ToolBarFont">返回</span> <span style="border-right: solid 1px #ccc">&nbsp;</span>
                        </button>



                </div>
            </div>
            <div id="dicbasic">
                <span id="sp_basic" class="headline">• 选择分店（分店后面的@标志，表明这个分店存在数量）</span>
            </div>

            <div class="divdata">

                <asp:CheckBoxList ID="CheckBoxListBranch" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow"></asp:CheckBoxList>


            </div>




            <div style="display: none;">
                <asp:TextBox ID="TextBoxSelectHead" runat="server"></asp:TextBox>

                <asp:TextBox ID="TextBoxTableBody" runat="server"></asp:TextBox>
                <asp:TextBox ID="TextBoxSheetNo" runat="server" Text="      "></asp:TextBox>
                <asp:TextBox ID="TextBoxSupName" runat="server" Text="      "></asp:TextBox>
                <asp:TextBox ID="TextBoxSupcustNo" runat="server" Text="      "></asp:TextBox>
                <asp:TextBox ID="TextBoxOperDate" runat="server" Text="      "></asp:TextBox>
                <asp:Button ID="btnQuery" runat="server" Text="查询" OnClick="btnQuery_Click" />

                <input id="TextPrintMode" type="text" value="<%=menuId == "2202"?"总订量":"总拣量" %>" />

            </div>
        </form>
    </div>
    <div style="background-color: #E5F2FE; padding: 3px;">
        <span id="sp_basic" class="headline">• 单据信息  （双击单元格，可以临时修改数量，但不影响原有的单据信息。）</span>
    </div>
    <div id="bodyDiv">
        <table
            id="bodyTable">
        </table>
    </div>
    <script language="javascript" type="text/javascript">
        // style=" height:500px ;overflow-x:auto;overflow-y:auto;display:block;"
        var DHSL = "<%=menuId == "2202"?"总订量":"总拣量" %>";
        var CKJJ = "参考价";
        var LODOP; //声明为全局变量 
        var headCellForPage = 6;//行头数
        var childCellForPage = 10;//每页显示店数
        var rowForPage = 30;//每页行数
        var bSelectAllBranch = true;



        function btnReturn_click()
        {
            window.close();
        
        }



        function btnQuery22_click()
        {
            document.getElementById("<%=btnQuery.ClientID%>").click();
        
        }



        function btnSelectAllBranch_click()
        {
      



            var chkBoxList = document.getElementById('<%=CheckBoxListBranch.ClientID%>');
            var chkBoxCount= chkBoxList.getElementsByTagName("input");
            for(var i=0;i<chkBoxCount.length;i++)
            {
                chkBoxCount[i].checked = bSelectAllBranch;
            }



            if(bSelectAllBranch){bSelectAllBranch = false;}
            else{bSelectAllBranch = true;}
        
        }




        // 固定表头
        function FixTable(TableID, FixColumnNumber, width, height) {
            /// <summary>
            ///     锁定表头和列
            ///     <para> sorex.cnblogs.com </para>
            /// </summary>
            /// <param name="TableID" type="String">
            ///     要锁定的Table的ID
            /// </param>
            /// <param name="FixColumnNumber" type="Number">
            ///     要锁定列的个数
            /// </param>
            /// <param name="width" type="Number">
            ///     显示的宽度
            /// </param>
            /// <param name="height" type="Number">
            ///     显示的高度
            /// </param>
            if ($("#" + TableID + "_tableLayout").length != 0) {
                $("#" + TableID + "_tableLayout").before($("#" + TableID));
                $("#" + TableID + "_tableLayout").empty();
            }
            else {
                $("#" + TableID).after("<div id='" + TableID + "_tableLayout' style='overflow:hidden;height:" + height + "px; width:" + width + "px;'></div>");
                //$("#" + TableID).after("<div id='" + TableID + "_tableLayout' style='height:100%; width:100%;'></div>");           //qqjMark
            }
            $('<div id="' + TableID + '_tableFix"></div>'
            + '<div id="' + TableID + '_tableHead"></div>'
            + '<div id="' + TableID + '_tableColumn"></div>'
            + '<div id="' + TableID + '_tableData"></div>').appendTo("#" + TableID + "_tableLayout");
            var oldtable = $("#" + TableID);
            var tableFixClone = oldtable.clone(true);
            tableFixClone.attr("id", TableID + "_tableFixClone");
            $(tableFixClone).children()[0].id = "tableFixCloneHead";
            $(tableFixClone).children()[1].id = "tableFixClonebody";
            $("#" + TableID + "_tableFix").append(tableFixClone);
            var tableHeadClone = oldtable.clone(true);
            tableHeadClone.attr("id", TableID + "_tableHeadClone");
            $(tableHeadClone).children()[0].id = "tableHeadCloneHead";
            $(tableHeadClone).children()[1].id = "tableHeadClonebody";
            $("#" + TableID + "_tableHead").append(tableHeadClone);
            var tableColumnClone = oldtable.clone(true);
            tableColumnClone.attr("id", TableID + "_tableColumnClone");
            $(tableColumnClone).children()[0].id = "tableColumnCloneHead";
            $(tableColumnClone).children()[1].id = "tableColumnClonebody";
            $("#" + TableID + "_tableColumn").append(tableColumnClone);
            $("#" + TableID + "_tableData").append(oldtable);
            $("#" + TableID + "_tableLayout table").each(function () {
                $(this).css("margin", "0");
            });
            var HeadHeight = $("#" + TableID + "_tableHead thead").height();
            HeadHeight += 2;
            $("#" + TableID + "_tableHead").css("height", HeadHeight);
            $("#" + TableID + "_tableFix").css("height", HeadHeight);
            var ColumnsWidth = 0;
            var ColumnsNumber = 0;
            $("#" + TableID + "_tableColumn tr:last td:lt(" + FixColumnNumber + ")").each(function () {
                ColumnsWidth += $(this).outerWidth(true);
                ColumnsNumber++;
            });
            ColumnsWidth += 2;
            if ($.browser.msie) {
                switch ($.browser.version) {
                    case "7.0":
                        if (ColumnsNumber >= 3) ColumnsWidth--;
                        break;
                    case "8.0":
                        if (ColumnsNumber >= 2) ColumnsWidth--;
                        break;
                }
            }
            $("#" + TableID + "_tableColumn").css("width", ColumnsWidth);
            $("#" + TableID + "_tableFix").css("width", ColumnsWidth);
            $("#" + TableID + "_tableData").scroll(function () {
                $("#" + TableID + "_tableHead").scrollLeft($("#" + TableID + "_tableData").scrollLeft());
                $("#" + TableID + "_tableColumn").scrollTop($("#" + TableID + "_tableData").scrollTop());
            });
            //     , "background-color": "Silver"  E8E8E8
            $("#" + TableID + "_tableFix").css({ "overflow": "hidden", "position": "relative", "z-index": "50", "background-color": "#E8E8E8" });
            $("#" + TableID + "_tableHead").css({ "overflow": "hidden", "width": width - 17, "position": "relative", "z-index": "45", "background-color": "white" });
            $("#" + TableID + "_tableColumn").css({ "overflow": "hidden", "height": height - 17, "position": "relative", "z-index": "40" });
            $("#" + TableID + "_tableData").css({ "overflow": "auto", "width": width, "height": height, "position": "relative", "z-index": "35" });
            
            //$("#" + TableID + "_tableData").css({ "overflow": "auto", "width": "100%", "height":  "100%", "position": "relative", "z-index": "35" });     //qqjMark

            if ($("#" + TableID + "_tableHead").width() > $("#" + TableID + "_tableFix table").width()) {
                $("#" + TableID + "_tableHead").css("width", $("#" + TableID + "_tableFix table").width());
                $("#" + TableID + "_tableData").css("width", $("#" + TableID + "_tableFix table").width() + 17);
            }
            if ($("#" + TableID + "_tableColumn").height() > $("#" + TableID + "_tableColumn table").height()) {
                $("#" + TableID + "_tableColumn").css("height", $("#" + TableID + "_tableColumn table").height());
                //         $("#" + TableID + "_tableData").css("height", $("#" + TableID + "_tableColumn table").height() + 17);
                $("#" + TableID + "_tableHead").css("width", $("#" + TableID + "_tableHead").width() + 17);
            } else {

            }
            $("#" + TableID + "_tableFix").offset($("#" + TableID + "_tableLayout").offset());
            $("#" + TableID + "_tableHead").offset($("#" + TableID + "_tableLayout").offset());
            $("#" + TableID + "_tableColumn").offset($("#" + TableID + "_tableLayout").offset());
            $("#" + TableID + "_tableData").offset($("#" + TableID + "_tableLayout").offset());
        }





















        $(document).ready(function () {


            var qqjTempAlertString = "<%=qqjTempAlertString%>";
            if(qqjTempAlertString != "")
            {alert(qqjTempAlertString);}

            //家在图表
            setTimeout(function () { createTable(); }, 100);
            // 	positionTheHead();
            //加载店铺选中状态
            setBranchState();
            //setChooseBranch();
            //加载编辑表格事件
            //document.getElementById("bodyTable").ondblclick = editCell;
            document.getElementById("bodyDiv").ondblclick = editCell;


            //qqjMark
            window.focus();
            window.scroll=true;


        });






















        //设置选了几家店
        function setChooseBranch() {
            var l = $("#branchField").val().split(",").length;
            $("#branchTip").html("(" + l + "家店被选中)")
        }

        function setBranchState() {
            //var branchFields = $("#branchField").val().split(",");
            //var branchlistDiv = $("#bListDiv");
            //for (var i = 0 ; i < branchFields.length; i++) {
            //    var branchField = branchFields[i].split("_")[0];
            //    $(branchlistDiv).find("#" + branchField).attr("checked", true);
            //}
        }
        function checkBoxClick() {
            var checkBoxs = $("#bListDiv").find("input");
            var branchField = "";
            for (var i = 0 ; i < checkBoxs.length; i++) {
                var ck = checkBoxs[i];
                if (ck.checked) {
                    branchField += ck.id + "_" + $(ck).attr("field") + ",";
                }
            }
            $("#branchField").val(branchField.substring(0, branchField.length - 1));
            setChooseBranch();
            //	event.stopPropagation();
        }


        function prn1_preview() {
            //CreateOneFormPage();
            createNewPage()
            LODOP.PREVIEW();
        };
        function prn1_print() {
            createNewPage()
            LODOP.PRINT();
        };
        function prn1_printA() {
            createNewPage()
            LODOP.PRINTA();
        };

        function editCell(event) {
            var currentCell;

            if (event == null) {
                currentCell = window.event.srcElement;
            } else {
                currentCell = event.target;
            }

            if (currentCell.tagName.toLowerCase() == "td" && currentCell.className == "cont") {
            //if (currentCell.tagName.toLowerCase() == "td" ) {         //qqjMark
                var input = document.createElement("input");
                input.type = 'text';
                input.setAttribute('class', 'editable');
                input.value = currentCell.innerHTML;
                input.style.width = "50px";
                input.style.height = "100%";
                input.onblur = function () {
                    //if (isNaN(this.value)) {
                    if (isNaN(this.value)  && currentCell.className == "cont") {            //qqjMark

                        input.style.backgroundColor = "red";

                    } else {
                        currentCell.innerHTML = Number(input.value);
                        //计算订货数量的和
                        var tds = $(currentCell).parent().find(".cont");
                        var sum = 0;
                        for (var i = 0 ; i < tds.length; i++) {
                            sum += Number($(tds[i]).text());
                        }
                        $(currentCell).parent().find("#" + DHSL).text(sum);
                        //bodyTable_tableColumn  改变视觉效果
                        var itemNo = $(currentCell).attr("itemNo");
                        //$(bodyTable_tableColumn).find("td[itemNo='" + itemNo + "'][id='订货数量']").text(sum);
                        $(currentCell).parent().find("[id='总订量']").text(sum);

                    }

                    //currentCell.removeChild(input);
                };
                input.onkeydown = function (e) {
                    var e = e || window.event;
                    if (e.keyCode == 13) {
                        input.blur();
                    }


                };

                currentCell.innerHTML = '';
                currentCell.appendChild(input);
                input.focus();
            }
        }

        function createNewPage() {
            var tbody = document.getElementById("mainBody").childNodes;
            var thead = document.getElementById("mainHead");
            LODOP = getLodop();
            LODOP.PRINT_INIT("单据打印");
            LODOP.SET_PRINT_STYLE("FontSize", 18);
            LODOP.SET_PRINT_STYLE("Bold", 1);
            //按行分页数
            var rowPageNum = Math.ceil(tbody.length / rowForPage);
            var cellNum = tbody[0].childNodes.length;
            //案列分页数
            var cellPageNum = Math.ceil((cellNum - headCellForPage) / childCellForPage);

            var strBodyStyle = "<style>" + $("#styleMain").html() + "</style>";

            for (var i = 0 ; i < rowPageNum; i++) {
                for (var j = 0; j < cellPageNum ; j++) {
                    var thisPageDoc = document.createElement("table");
                    //thisPageDoc.class="table_container" ;
                    thisPageDoc.border = "1";
                    thisPageDoc.cellpadding = "0";
                    thisPageDoc.cellspacing = "0";
                    thisPageDoc.width = "100%"
                    //thisPageDoc.width="100%"
                    //if (i == 0 && j == 0) {
                    LODOP.ADD_PRINT_TEXT(10, '45%', 260, 19, (document.getElementById("TextPrintMode").value =="总拣量"?"商品分拣单" :"商品采购单" ));


                    LODOP.ADD_PRINT_TEXT('10', '75%', 260, 19, "第#页/共&页");
                    LODOP.SET_PRINT_STYLEA(0, "ItemType", 2);
                    LODOP.SET_PRINT_STYLEA(0, "HOrient", 1);
                    LODOP.SET_PRINT_STYLEA(0, "FontSize", 11);

                        // 						LODOP.ADD_PRINT_HTM(30, '2%', '98%','98%',document.getElementById("queryForm").innerHTML);
                    LODOP.ADD_PRINT_TEXT(45, '2%', '98%', '98%', '    供应商：'+ $("#<%=TextBoxSupcustNo.ClientID%>").val() + '  ' + $("#<%=TextBoxSupName.ClientID%>").val() + '    日期：'+ $("#<%=TextBoxOperDate.ClientID%>").val() + '    金额单位：元    重量单位：公斤   单号：' + $("#<%=TextBoxSheetNo.ClientID%>").val());
                        LODOP.SET_PRINT_STYLEA(0, "FontSize", 11);
                        // 						LODOP.SET_PRINT_STYLEA(0,"LinkedItem",-1);
                        // 						此处打印上方说明如 供应商  单号  日期之类的
                    //}
                    var big = headCellForPage + ((j + 1) * childCellForPage) - 1;
                    var small = headCellForPage + (j * childCellForPage);

                    //构建报表头

                    var headData = document.createElement("table");
                    // 						headData.innerHTML = thead.outerHTML ;	
                    $(headData).html(thead.outerHTML);
                    for (var m = cellNum - 1 ; m >= headCellForPage; m--) {
                        if ((m < small || m > big)) {
                            var trs = $(headData).find("tr");
                            for (var n = 0 ; n < trs.length; n++) {

                                var trl = $(trs[n]).children().length;
                                for (var kk = trl - 1 ; kk >= 0 ; kk--) {
                                    var thisEle = $($(trs[n]).children()[kk]);
                                    var posi = "," + thisEle.attr("posi") + ",";
                                    posi = posi.replace(/,,/g, ",");
                                    if (posi.indexOf("," + m + ",") >= 0) {
                                        if (posi == "," + m + ",") {
                                            thisEle.remove();
                                        } else {
                                            posi = posi.replace("," + m + ",", ",");
                                            thisEle.attr("posi", posi);
                                            thisEle.attr("colspan", thisEle.attr("colspan") - 1);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    // 					thisPageDoc.innerHTML +=headData.innerHTML ;
                    var trss = $(headData).find("th");





                    //qqjMark
                    //if (trss.length > (headCellForPage + childCellForPage / 2)) {
                    //    $(trss[headCellForPage + childCellForPage / 2 - 1]).after("<th>商品名称</th>");
                    //}






                    $(thisPageDoc).html(thisPageDoc.innerHTML + headData.innerHTML);

                    var color = "#FFFFFF";      //qqjMark       我添加的，用于真正的隔行显示
                    for (var k = 0; k < rowForPage; k++) {
                        //构建报表体 
                        var rn = i * rowForPage + k;//行数
                        if (rn >= tbody.length) {
                            var trsss = $(thisPageDoc).find("tr");
                            var tempTr = $(trsss[trsss.length - 1]).clone(true);
                            var tdsss = $(tempTr).find("td");


                            

                            for (var w = 0; w < tdsss.length; w++) {
                                if (w != 0) {
                                    $(tdsss[w]).text("");
                                } else {
                                    $(tdsss[0]).text(Number($(tdsss[0]).text()) + 1);
                                }
                                //var color = $(tdsss[w]).css("backgroundColor") == "rgb(242, 242, 242)" ? "#FFFFFF" : "#F2F2F2";
                                

                                $(tdsss[w]).css("backgroundColor", color);
                                
                            }

                            color = color == "#F2F2F2" ? "#FFFFFF" : "#F2F2F2";         //qqjMark


                            $(thisPageDoc).html(thisPageDoc.innerHTML + tempTr[0].outerHTML);
                            continue;
                        }
                        var rowData = document.createElement("table");
                        // 						rowData.innerHTML = tbody[rn].outerHTML ;
                        $(rowData).html(tbody[rn].outerHTML);
                        for (var m = cellNum - 1 ; m >= headCellForPage; m--) {
                            if ((m < small || m > big)) {
                                var trs = $(rowData).find("tr");
                                for (var n = 0 ; n < trs.length; n++) {
                                    $($(trs[n]).children()[m]).remove();
                                }

                            }
                        }
                        var trs = $(rowData).find("tr");


                        //qqjMark
                        //添加商品名称
                        //for (var n = 0 ; n < trs.length; n++) {
                        //    var tdss = $(trs[n]).find("td");
                        //    if (tdss.length > (headCellForPage + childCellForPage / 2)) {
                        //        $(tdss[headCellForPage + childCellForPage / 2 - 1]).after($($(trs[n]).children()[1]).clone(true));
                        //    }
                        //}





                        //	thisPageDoc.innerHTML +=rowData.innerHTML ;
                        $(thisPageDoc).html(thisPageDoc.innerHTML + rowData.innerHTML);
                    }

                    //$(thisPageDoc).css("border","1px solid");           //qqjMark


                    //LODOP.ADD_PRINT_HTM(65, '2%', '96%', '98%',strBodyStyle + "<body>" + thisPageDoc.outerHTML + "</body>");      //qqjMark
                    LODOP.ADD_PRINT_HTM(65, '2%', '90%', '98%',strBodyStyle + "<body>" + thisPageDoc.outerHTML + "</body>");



		

                    if (i != rowPageNum - 1 || j != cellPageNum - 1) {
                        //如果不是最后一页则新起一页
                        LODOP.NEWPAGE();
                    }

            

                }
               

            }


            LODOP.ADD_PRINT_TEXT(10, '6', '98%', '98%', "   制单人:         采购员:         分拣员:         库管员：");

            LODOP.SET_PRINT_STYLEA(0, "FontSize", 12);
            LODOP.SET_PRINT_STYLEA(0, "LinkedItem", -1);



<%--            LODOP.ADD_PRINT_TEXT(10, '6', '90%', '90%', "<%=sWarehouseKeeper%>");

            LODOP.SET_PRINT_STYLEA(0, "FontSize", 12);
            LODOP.SET_PRINT_STYLEA(0, "LinkedItem", -1);--%>


            LODOP.SET_PRINT_PAGESIZE(2, 0, 0, 'A4');      //qqjMark
            //LODOP.SET_PRINT_PAGESIZE(2,2430,2860, 'CreateCustomPage');
            //LODOP.SET_PRINT_PAGESIZE(2, 0, 0, 'LodopCustomPage');







            LODOP.SET_PRINT_STYLEA(0, "TableHeightScope", 1);
            LODOP.SET_PRINT_STYLEA(0, "Top2Offset", -60); //这句可让次页起点向上移
            //LODOP.SET_PRINT_STYLEA(0, "Offset2Left", -200); //这句可让次页起点向上移



            LODOP.SET_SHOW_MODE("LANDSCAPE_DEFROTATED", 1);
        }
        function createOrderFun() {
            var tds = $("#mainBody").find(".cont");
            var dhsls = $("#mainBody").find("#" + DHSL);
            var saveData = [];
            var mainData = [];
            for (var i = 0 ; i < tds.length; i++) {
                saveData.push({ branchNo: $(tds[i]).attr("branchNo"), itemNo: $(tds[i]).attr("itemNo"), num: $(tds[i]).text() });
            }
            // 		CKJJ
            for (var i = 0 ; i < dhsls.length; i++) {
                mainData.push({ itemNo: $(dhsls[i]).attr("itemNo"), largeQty: $(dhsls[i]).text(), realQty: $(dhsls[i]).text(), validPrice: $(dhsls[i]).prev().text() });
            }
            var sUrl = "/eshop/m/print/saveOrderInfo";
            jQuery.ajax({
                url: sUrl,
                type: "post",
                dataType: "json",
                data: { data: JSON.stringify({ saveData: saveData, mainData: mainData, orderNo: $("#orderNo").val() }) },
                async: false,
                success: function (data) {

                    if (data.errorMessage == undefined || data.errorMessage == "SUCCESS") {
                        alert("保存成功！");
                    } else {
                        alert(data.errorMessage);
                    }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert("Failure may be the reason of the network!");
                }
            });
        }

        function createTable() {
            var body = document.getElementById("bodyTable");
            //获取列表头
            var title = document.getElementById("<%=TextBoxSelectHead.ClientID%>").value.replace("[", "").replace("]", "").replace(/ /g, "").split(",");
            //if(document.getElementById("TextPrintMode").value =="总拣量")
            //{
            //    title.replace("总订量", "总拣量");
            //}



            //设置表头宽度
            $(bodyTable).css("width", title.length * 70 + "px");
            // 		定义隐藏列
            //var hiddenList = ["商品编号"];        //qqjMark
            var hiddenList = [""];
            var rs = <%=TextBoxTableBody.Text%> ;
            var html = "<thead id='mainHead'><tr> ";
            var titNum = 0;
            for (var i = 0 ; i < title.length; i++) {
                if (hiddenList[0] != title[i].split("_")[0]) {
                    //非隐藏列
                    //if (i < headCellForPage + hiddenList.length) {
                        html += "<th posi='" + titNum + "'>" + title[i].split("_")[0] + "</th>";
                    //} else {
                    //    html += "<th posi='" + titNum + "'>" + title[i].split("_")[0] + "<br/>" + (titNum - headCellForPage + 1) + (document.getElementById("TextPrintMode").value =="总拣量"?"店拣货</th>":"店订货</th>");
                    //}
                    titNum++;
                }
            }

            html += "</tr></thead>";
            html += "<tbody id='mainBody' >";
            for (var i = 0 ; i < rs.length; i++) {
                html += "<tr style='align:center'>";
                var bgcolor = i % 2 == 1 ? "#F2F2F2" : "#FFFFFF";
                for (var j = 0 ; j < title.length; j++) {

                    if (hiddenList[0] != title[j].split("_")[0]) {
                        //非隐藏列
                        if (j < headCellForPage + hiddenList.length) {
                            //  id = 列名-属于哪个商店   itemNo =属于哪个商品  branchNo= 店铺编号
                            html += "<td class='head'   id='" + title[j].split("_")[0] + "' itemNo='" + rs[i]["商品编号"] + "' branchNo='" + title[j].split("_")[1] + "'  style='text-align:center; background-color:" + bgcolor + ";'>" + (rs[i][title[j].split("_")[0] + ""] == "0" ? "": rs[i][title[j].split("_")[0] + ""] )+ "</td>";
                        } else {
                            html += "<td class='cont' id='" + title[j].split("_")[0] + "' itemNo='" + rs[i]["商品编号"] + "'  branchNo='" + title[j].split("_")[1] + "'   style='text-left:center; background-color:" + bgcolor + ";'>" + (rs[i][title[j].split("_")[0] + ""] == "0"? "": rs[i][title[j].split("_")[0] + ""]) + "</td>";
                        }
                    }
                }
                html += "</tr>";
            }
            html += "</tbody>";
            //body.innerHTML = html;
            $(body).html(html);

            //FixTable("bodyTable", headCellForPage, 1000, 470);
            //showDialog();
        }






    </script>
</body>
</html>
