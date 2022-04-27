<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="InpKaiinCd.ascx.vb" Inherits="web_InfoB_Setting.InpKaiinCd" %>

<asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server"></asp:ScriptManagerProxy>

<asp:Panel ID="pnl_00" runat="server" Style="display: none; width: 300px; height: 400px;" CssClass="modalPopup">
    <asp:Panel ID="pnl_01" runat="server" Style="width: 100%; background-color: #FFCFAF; border: solid 1px Gray; color: Black; cursor: pointer;">
        <div style="text-align: center; padding: 2px 0px;">
            [会員コード] 入力
        </div>
    </asp:Panel>
    
    <div style="padding-top: 10px; width: 100%; text-align: center;">
        <p style="color: steelblue;">
            ※ 範囲指定例：10001-10100
        </p>
        <asp:TextBox ID="tbx_kaiincd_edit" runat="server" Style="width: 90%; resize: none;" TextMode="MultiLine" Rows="20" />
        <ajaxToolkit:FilteredTextBoxExtender ID="tbx_kaiincd_edit_FilteredTextBoxExtender" runat="server"
            TargetControlID="tbx_kaiincd_edit" FilterType="Custom, Numbers" ValidChars="-" />
    </div>

    <table style="width: 100%;">
        <colgroup>
            <col style="width: 100%;"/>
        </colgroup>
        <tr style="height: 10px;"><td></td></tr>
        <tr>
            <td style="text-align: center;">
                <asp:Button ID="btn_OK" runat="server" CausesValidation="false" Text="OK" Style="cursor: pointer;" />
                <asp:Button ID="btn_Cancel" runat="server" CausesValidation="false" Text="Cancel" Style="cursor: pointer;" />
            </td>
        </tr>
    </table>
</asp:Panel>

<ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender1" runat="server"
    TargetControlID="hdn_dummyTarget"
    PopupControlID="pnl_00" 
    BackgroundCssClass="modalBackground"
    OkControlID=""
    OnOkScript=""
    CancelControlID="btn_Cancel"
    DropShadow="true"
    PopupDragHandleControlID="pnl_01" />

<asp:HiddenField ID="hdn_dummyTarget" runat="server" />
