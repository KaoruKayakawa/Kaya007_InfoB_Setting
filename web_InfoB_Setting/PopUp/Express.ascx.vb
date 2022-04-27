Public Class Express
    Inherits System.Web.UI.UserControl

    Public Sub ShowPopup(cap As String, msg As String, width_popup As Integer?, width_msg As Integer?, textAlign_msg As String)
        If width_popup Is Nothing Then
            width_popup = Const_App.PopUp.Default_Width_PopUp
        End If

        If width_msg Is Nothing Then
            width_msg = Const_App.PopUp.Default_Width_Msg
        End If

        If textAlign_msg Is Nothing Then
            textAlign_msg = Const_App.PopUp.Default_TextAlign_Msg
        End If

        Me.pnl_00.Width = Unit.Pixel(width_popup.Value)
        Me.lrl_Caption.Text = cap
        Me.lrl_Message.Text = String.Format(Const_App.PopUp.Layout_Message, width_msg, textAlign_msg, msg)

        Me.ModalPopupExtender1.Show()
    End Sub

    Public Sub ShowPopup(cap As String, msg As String)
        ShowPopup(cap, msg, Nothing, Nothing, Nothing)
    End Sub

    Protected Sub btn_OK_Click(sender As Object, e As EventArgs)
        Me.ModalPopupExtender1.Hide()
    End Sub

End Class