Public Class Const_App

    Public Class UrlParam
        Enum VisitPage
            VisitNew
            VisitRelative
            VisitBack
        End Enum

        Public Const Session_VisitPage As String = "Const_App_Session_PageTrans"

        Public Shared Function GetVisitPage(val As Object) As VisitPage
            If val Is Nothing Then
                Return VisitPage.VisitNew
            End If

            Return DirectCast(val, VisitPage)
        End Function

        Public Const Session_Param As String = "Const_App_Session_Param"
    End Class

    Public Class PopUp
        Public Const Default_Width_PopUp As Integer = 350
        Public Const Default_Width_Msg As Integer = 280
        Public Const Default_TextAlign_Msg As String = "left"

        Public Const Layout_Message As String = "<table style=""width: 100%;"">" _
                + "<colgroup><col /><col style=""width: {0:D}px;"" /><col /></colgroup>" _
                + "<tr style=""height: 10px;""><td></td></tr>" _
                + "<tr><td>&nbsp;</td><td style=""text-align: {1};"">{2}</td><td>&nbsp;</td></tr>" _
                + "</table>"
    End Class

End Class
