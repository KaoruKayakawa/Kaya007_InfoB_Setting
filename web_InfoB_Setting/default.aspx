<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="default.aspx.vb" Inherits="web_InfoB_Setting._default" %>

<%@ Register src="~/UserControl/ListPager.ascx" tagname="ListPager" tagprefix="uc1" %>
<%@ Register src="~/PopUp/Express.ascx" tagname="Express" tagprefix="pop1" %>
<%@ Register src="~/PopUp/Confirm.ascx" tagname="Confirm" tagprefix="pop1" %>
<%@ Register src="~/PopUp/InpKaiinCd.ascx" tagname="InpKaiinCd" tagprefix="pop1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    
    <asp:PlaceHolder ID="PlaceHolder1" runat="server">
        <%: System.Web.Optimization.Styles.Render("~/Styles/CSS") %>
        <%: System.Web.Optimization.Scripts.Render("~/Scripts/JS") %>
    </asp:PlaceHolder>
</head>

<body onload="history.forward();" onresize="adjust_divListHeight();">
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="true" AsyncPostBackTimeout="200"></asp:ScriptManager>
        
        <table style="width: 100%;">
            <colgroup>
                <col style="width: 5%;" />
                <col style="width: 20%;" />
                <col />
                <col style="width: 10%;" />
                <col style="width: 15%;" />
                <col style="width: 12%;" />
                <col style="width: 5%;" />
            </colgroup>
            <tr>
                <td colspan="7" style="background-color: linen; border: solid 2px rosybrown; padding: 2px;">
                    <table style="background-color: rosybrown; color: white; width: 100%;">
                        <colgroup>
                            <col />
                            <col style="width: 30%;" />
                        </colgroup>
                        <tr>
                            <td style="padding: 2px 20px; font-size: 17px; font-weight: bold;">
                                <asp:Literal ID="lrl_wndCap" runat="server" />
                            </td>
                            <td style="text-align: right; padding-right: 20px; font-size: 11px;">Ver： <asp:Literal ID="lrl_version" runat="server" /></td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr style="height: 10px;"><td></td></tr>
            <tr>
                <td></td>
                <td style="text-align: left;">
                    <asp:Button ID="btn_Redraw" runat="server" CssClass="btn01" Text="再描画" OnClick="btn_Redraw_Click" />
                </td>
                <td></td>
                <td style="text-align: right;">
                    <asp:Button ID="btn_NewRecord" runat="server" CssClass="btn01" Text="行追加" ValidationGroup="ValGrp1" OnClick="btn_NewRecord_Click" />
                </td>
                <td style="text-align: right;">
                    <asp:Button ID="btn_Update" runat="server" CssClass="btn01" Text="更 新" ValidationGroup="ValGrp1" OnClientClick="return Page_ClientValidate();" OnClick="btn_Update_Click" />
                </td>
                <td style="text-align: right;">
                    <asp:Button ID="btn_Update_Deploy" runat="server" CssClass="btn01" Text="更新して即反映" ValidationGroup="ValGrp1" OnClientClick="return Page_ClientValidate();" OnClick="btn_Update_Deploy_Click" />
                </td>
                <td></td>
            </tr>
        </table>

        <asp:UpdatePanel ID="up_1" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="false">
            <ContentTemplate>
                <div style="margin-top: 5px; background-color: rosybrown; color: Black; border: solid 2px linen; padding: 2px;">
                    <table style="background-color: linen; width: 100%;">
                        <colgroup>
                            <col style="width: 100%;" />
                        </colgroup>
                        <tr>
                            <td style="background-color: lightgrey;">
                                <asp:Repeater id="rep_Header" runat="server" OnItemDataBound="rep_Header_ItemDataBound" OnItemCommand="rep_Header_ItemCommand">
                                    <HeaderTemplate>
                                        <div style="overflow-x: hidden; overflow-y: scroll; width: 100%;">
                                            <table style="table-layout: fixed; border-collapse: separate; border-spacing: 1px; width: 100%; background-color: Gray;">
                                                <colgroup>
                                                    <col style="width: 6%;" />
                                                    <col style="width: 12%;" />
                                                    <col />
                                                    <col style="width: 26%;" />
                                                    <col style="width: 15%;" />
                                                </colgroup>
                                                <tr>
                                                    <th style="padding: 3px; color: Gray; background-color: lightgrey; text-align: center;">削除</th>
                                                    <th style="padding: 3px; color: Gray; background-color: lightgrey; text-align: center;">フォルダ</th>
                                                    <th style="padding: 3px; color: Gray; background-color: lightgrey; text-align: center;">メモ</th>
                                                    <th style="padding: 3px; color: Gray; background-color: lightgrey; text-align: center;">会員</th>
                                                    <th style="padding: 3px; color: Gray; background-color: lightgrey; text-align: center;">表示</th>
                                                </tr>
                                            </table>
                                        </div>
                                        <div id="div_List" style="height: 0; overflow-x: hidden; overflow-y: scroll; width: 100%;" onscroll="save_valScroll();">
                                            <table style="table-layout: fixed; border-collapse: separate; border-spacing: 1px; width: 100%; background-color: Gray;">
                                                <colgroup>
                                                    <col style="width: 6%;" />
                                                    <col style="width: 12%;" />
                                                    <col />
                                                    <col style="width: 26%;" />
                                                    <col style="width: 15%;" />
                                                </colgroup>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                                <tr class="_rep_item">
                                                    <td style="background-color: white; text-align: center; padding: 3px;">
                                                        <asp:CheckBox ID="cbx_delete" runat="server" CssClass="_del_item" />
                                                    </td>
                                                    <td style="background-color: white; text-align: left; padding: 3px;">
                                                        <div style="width: 100%; text-align: center;">
                                                            <asp:TextBox ID="tbx_folder" runat="server" MaxLength="10" Style="width: 80%;" />
                                                        </div>
                                                        <asp:CustomValidator id="tbx_folder_CustomValidator1" runat="server" ControlToValidate="tbx_folder" ValidationGroup="ValGrp1" Display="None" ValidateEmptyText="true"
                                                            ErrorMessage="入力してください" ClientValidationFunction="validateRequiredField" />
                                                        <ajaxToolkit:ValidatorCalloutExtender ID="tbx_folder_CustomValidator1_ValidatorCalloutExtender" PopupPosition="BottomLeft"
                                                            runat="server" Enabled="True" TargetControlID="tbx_folder_CustomValidator1" />
                                                        <asp:CustomValidator id="tbx_folder_CustomValidator2" runat="server" ControlToValidate="tbx_folder" ValidationGroup="ValGrp1" Display="None" ValidateEmptyText="true"
                                                            ErrorMessage="半角 10 文字以内で入力してください" ClientValidationFunction="folder_validateLength" />
                                                        <ajaxToolkit:ValidatorCalloutExtender ID="tbx_folder_CustomValidator2_ValidatorCalloutExtender" PopupPosition="BottomLeft"
                                                            runat="server" Enabled="True" TargetControlID="tbx_folder_CustomValidator2" />
                                                    </td>
                                                    <td style="background-color: white; text-align: left; padding: 3px;">
                                                        <div style="width: 100%; text-align: center;">
                                                            <asp:TextBox ID="tbx_memo" runat="server" TextMode="MultiLine" Rows="3" Style="width: 90%; resize: none;" />
                                                        </div>
                                                        <asp:CustomValidator id="tbx_memo_CustomValidator" runat="server" ControlToValidate="tbx_memo" ValidationGroup="ValGrp1" Display="None" ValidateEmptyText="true"
                                                            ErrorMessage="200 文字以内で入力してください" ClientValidationFunction="memo_validateLength" />
                                                        <ajaxToolkit:ValidatorCalloutExtender ID="tbx_memo_CustomValidator_ValidatorCalloutExtender" PopupPosition="BottomLeft"
                                                            runat="server" Enabled="True" TargetControlID="tbx_memo_CustomValidator" />
                                                    </td>
                                                    <td style="background-color: white; text-align: left; padding: 3px;">
                                                        <div style="width: 100%; text-align: center;">
                                                            <asp:TextBox ID="tbx_kaiincd_disp" runat="server" Style="width: 75%; resize: none; background-color: gainsboro;" TextMode="MultiLine" Rows="3" ReadOnly="true" />
                                                            <asp:ImageButton ID="ibtn_toEdit" runat="server" ImageUrl="~/Images/ASX_Edit_blue_16x.png" AlternateText="edit" CausesValidation="false" CommandName="toEdit_click" Style="width: 16px; height: 16px;"  />
                                                        </div>
                                                        <asp:CustomValidator id="tbx_kaiincd_disp_CustomValidator1" runat="server" ControlToValidate="tbx_kaiincd_disp" ValidationGroup="ValGrp1" Display="None" ValidateEmptyText="true"
                                                            ErrorMessage="入力してください" ClientValidationFunction="validateRequiredField" />
                                                        <ajaxToolkit:ValidatorCalloutExtender ID="tbx_kaiincd_disp_CustomValidator1_ValidatorCalloutExtender" PopupPosition="BottomLeft"
                                                            runat="server" Enabled="True" TargetControlID="tbx_kaiincd_disp_CustomValidator1" />
                                                    </td>
                                                    <td style="background-color: white; text-align: center; padding: 3px;">
                                                        <asp:RadioButtonList ID="rbl_flag" runat="server" RepeatDirection="Horizontal" Style="display: inline-block; height: 1.5em;">
                                                            <asp:ListItem Value="0">なし&nbsp;&nbsp;</asp:ListItem>
                                                            <asp:ListItem Value="1">あり&nbsp;&nbsp;</asp:ListItem>
                                                        </asp:RadioButtonList>
                                                    </td>
                                                </tr>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                            </table>
                                        </div>
                                    </FooterTemplate>
                                </asp:Repeater>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: center;">
                                <div style="position: relative; width: 100%;">
                                    <div style="position: absolute; left: 50%; top: 1em; margin-left: -10em; width: 20em; background-color: palegoldenrod; border: 1px solid Gray;">
                                        <uc1:ListPager ID="uc_ListPager_Item" runat="server" />
                                    </div>
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>

                <pop1:Express id="uc_Express" runat="server" />
                <pop1:Confirm id="uc_Confirm_Redraw" runat="server" />
                <pop1:Confirm id="uc_Confirm_Update" runat="server" />
                <pop1:Confirm id="uc_Confirm_Update_Deploy" runat="server" />
                <pop1:InpKaiinCd id="uc_InpKaiinCd" runat="server" />
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="uc_ListPager_Item" />
                <asp:AsyncPostBackTrigger ControlID="rep_Header" />
                <asp:AsyncPostBackTrigger ControlID="btn_Redraw" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="btn_NewRecord" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="btn_Update" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="btn_Update_Deploy" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="uc_Confirm_Redraw" />
                <asp:AsyncPostBackTrigger ControlID="uc_Confirm_Update" />
                <asp:AsyncPostBackTrigger ControlID="uc_Confirm_Update_Deploy" />
                <asp:AsyncPostBackTrigger ControlID="uc_InpKaiinCd" />
            </Triggers>
        </asp:UpdatePanel>
    </form>
    
    <script type="text/javascript">
    <!--
        var mng;
        function pageLoad() {
            mng = Sys.WebForms.PageRequestManager.getInstance();

            mng.remove_initializeRequest(initializeRequest_mng);
            mng.add_initializeRequest(initializeRequest_mng);

            mng.remove_endRequest(endRequest_mng);
            mng.add_endRequest(endRequest_mng);

            adjust_divListHeight();
        }
        function initializeRequest_mng(sender, args) {
            if (mng.get_isInAsyncPostBack()) {
                args.set_cancel(true);

                return;
            }

            $.sanIndicator();
        }
        function endRequest_mng(sender, args) {
            $.sanIndicator.hide();
        }

        function adjust_divListHeight() {
            var height = document.documentElement.clientHeight - 200;
            if (height < 0) {
                height = 0;
            }

            document.getElementById('div_List').style.height = height + 'px';
        }

        function validateRequiredField(oSrc, args) {
            if (args.Value.length > 0) {
                args.IsValid = true;

                return;
            }

            args.IsValid = $(oSrc).parents('._rep_item').find('._del_item').children(0).prop("checked");
        }

        function folder_validateLength(oSrc, args) {
            if (encodeURI(args.Value).replace(/%../g, "*").length < 11) {
                args.IsValid = true;

                return;
            }

            args.IsValid = $(oSrc).parents('._rep_item').find('._del_item').children(0).prop("checked");
        }

        function memo_validateLength(oSrc, args) {
            if (args.Value.length < 201) {
                args.IsValid = true;

                return;
            }

            args.IsValid = $(oSrc).parents('._rep_item').find('._del_item').children(0).prop("checked");
        }
    //-->
    </script>
</body>
</html>
