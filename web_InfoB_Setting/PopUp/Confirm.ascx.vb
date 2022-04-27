Imports AjaxControlToolkit

Public Class Confirm
    Inherits System.Web.UI.UserControl

    Public ReadOnly Property ModalPopupExtender_01() As ModalPopupExtender
        Get
            Return Me.ModalPopupExtender1
        End Get
    End Property

    Public ReadOnly Property Button_OK() As Button
        Get
            Return Me.btn_OK
        End Get
    End Property

    Public Sub Init_PopUp(cap As String, msg As String, width_popup As Integer?, width_msg As Integer?, textAlign_msg As String)
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
    End Sub

    Public Sub Init_PopUp(cap As String, msg As String)
        Init_PopUp(cap, msg, Nothing, Nothing, Nothing)
    End Sub

End Class
