Imports AjaxControlToolkit

Public Class InpKaiinCd
    Inherits System.Web.UI.UserControl

    Public ReadOnly Property ModalPopupExtender_01() As ModalPopupExtender
        Get
            Return Me.ModalPopupExtender1
        End Get
    End Property

    Public ReadOnly Property TextBox_KaiinCdEdit() As TextBox
        Get
            Return Me.tbx_kaiincd_edit
        End Get
    End Property

    Public ReadOnly Property Button_OK() As Button
        Get
            Return Me.btn_OK
        End Get
    End Property

    Private Sub InpKaiinCd_Load(sender As Object, e As EventArgs) Handles Me.Load
        tbx_kaiincd_edit_FilteredTextBoxExtender.ValidChars = tbx_kaiincd_edit_FilteredTextBoxExtender.ValidChars + vbLf
    End Sub
End Class