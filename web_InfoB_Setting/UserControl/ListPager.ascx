<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ListPager.ascx.vb" Inherits="web_InfoB_Setting.ListPager" %>

<asp:ScriptManagerProxy ID="smp_00" runat="server">
</asp:ScriptManagerProxy>

<table style="table-layout: fixed; border-collapse: separate; padding: 0.2em 0.5em; width: 20em; background-color: transparent;">
    <colgroup>
        <col style="width: 25%;" />
        <col />
        <col style="width: 25%;" />
    </colgroup>
    <tbody>
        <tr>
            <td style="padding-left: 0.2em; text-align: left;">
                <asp:Button ID="btn_ToPrev" runat="server" CssClass="btn02" Text="＜ 戻る" Style="text-decoration: none;" OnClientClick="return Page_ClientValidate();" OnClick="btn_ToPrev_Click" />
            </td>
            <td style="text-align: center;">
                <asp:TextBox ID="tbx_PageNo" runat="server" Width="3em" CssClass="tbx01" MaxLength="5" Style="ime-mode: disabled; text-align: right;"
                    AutoPostBack="true" OnTextChanged="tbx_PageNo_TextChanged" />
                <ajaxToolkit:FilteredTextBoxExtender ID="fte_tbx_PageNo" runat="server" TargetControlID="tbx_PageNo" FilterType="Numbers" />
                &nbsp;／&nbsp;
                <asp:Literal ID="lrl_PageCnt" runat="server" />
            </td>
            <td style="padding-right: 0.2em; text-align: right;">
                <asp:Button ID="btn_ToNext" runat="server" CssClass="btn02" Text="次へ ＞" Style="text-decoration: none;" OnClientClick="return Page_ClientValidate();" OnClick="btn_ToNext_Click" />
            </td>
        </tr>
    </tbody>
</table>

<script type="text/javascript">
<!--
    var pageNo;

    function onFocus_tbx_PageNo() {
        pageNo = $('<% = "#" + tbx_PageNo.ClientID %>').val();
    }

    function onChange_tbx_PageNo() {
        var res = Page_ClientValidate();
        if (!res) {
            $('<% = "#" + tbx_PageNo.ClientID %>').val(pageNo);
        }
    }
//-->
</script>
