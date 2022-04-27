Imports System.Drawing

Public Class ListPager
    Inherits System.Web.UI.UserControl

    Delegate Sub delegate_TurnPage()
    Public Sub_Before_TurnPage As delegate_TurnPage

    Protected _SESSION_REP_CONTROLID As String = Me.UniqueID + "_Rep_ControlId"
    Protected _SESSION_REP_DTTBL As String = Me.UniqueID + "_Rep_DtTbl"
    Protected _SESSION_PAGE_CNT As String = Me.UniqueID + "_Page_Cnt"
    Protected _SESSION_PAGE_LINECNT As String = Me.UniqueID + "_Page_LineCnt"
    Protected _SESSION_PAGE_CURRENT As String = Me.UniqueID + "_Page_Current"

    Private Sub ListPager_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not Me.IsPostBack Then
            Me.tbx_PageNo.Attributes.Add("onfocus", "onFocus_tbx_PageNo();")
            Me.tbx_PageNo.Attributes.Add("onchange", "onChange_tbx_PageNo();")
        End If
    End Sub

    Public ReadOnly Property CurrentPageNo As Integer
        Get
            Return DirectCast(Me.Session(_SESSION_PAGE_CURRENT), Integer)
        End Get
    End Property

    Public ReadOnly Property CurrentPosition As Integer
        Get
            Return DirectCast(Me.Session(_SESSION_PAGE_LINECNT), Integer) * DirectCast(Me.Session(_SESSION_PAGE_CURRENT), Integer)
        End Get
    End Property

    Public ReadOnly Property CtrlID_tbx_PageNo As String
        Get
            Return Me.tbx_PageNo.ID
        End Get
    End Property

    Public Sub Init_ListPager(id_ctrl As String, tbl As DataTable, pageLineCnt As Integer, Optional tail As Boolean = False)
        Me.Session(_SESSION_REP_CONTROLID) = id_ctrl
        Me.Session(_SESSION_REP_DTTBL) = tbl
        Me.Session(_SESSION_PAGE_CNT) = Convert.ToInt32(Math.Ceiling(tbl.Rows.Count / pageLineCnt))
        Me.Session(_SESSION_PAGE_LINECNT) = pageLineCnt

        Me.Session(_SESSION_PAGE_CURRENT) = Nothing     ' 初期状態判定に使用するので、この設定は必要

        Me.lrl_PageCnt.Text = DirectCast(Me.Session(_SESSION_PAGE_CNT), Integer).ToString("d")

        If tail Then
            TurnPage(DirectCast(Me.Session(_SESSION_PAGE_CNT), Integer) - 1)
        Else
            TurnPage(0)
        End If
    End Sub

    Public Sub ReDraw_List()
        TurnPage(DirectCast(Me.Session(_SESSION_PAGE_CURRENT), Integer))
    End Sub

    Public Sub TurnPage(pageNo As Integer)
        If Me.Session(_SESSION_PAGE_CURRENT) IsNot Nothing Then
            Sub_Before_TurnPage()
        End If

        Dim rep As Repeater = DirectCast(Me.Parent.FindControl(DirectCast(Me.Session(_SESSION_REP_CONTROLID), String)), Repeater)
        Dim tbl As DataTable = DirectCast(Me.Session(_SESSION_REP_DTTBL), DataTable)
        Dim pageCnt As Integer = DirectCast(Me.Session(_SESSION_PAGE_CNT), Integer)
        Dim pageLineCnt As Integer = DirectCast(Me.Session(_SESSION_PAGE_LINECNT), Integer)

        If pageNo >= pageCnt Then
            pageNo = pageCnt - 1
        End If

        If pageNo > 0 Then
            Me.btn_ToPrev.Enabled = True
            Me.btn_ToPrev.ForeColor = Color.Blue
        Else
            Me.btn_ToPrev.Enabled = False
            Me.btn_ToPrev.ForeColor = Color.Gray
        End If
        If pageNo < pageCnt - 1 Then
            Me.btn_ToNext.Enabled = True
            Me.btn_ToNext.ForeColor = Color.Blue
        Else
            Me.btn_ToNext.Enabled = False
            Me.btn_ToNext.ForeColor = Color.Gray
        End If
        Me.tbx_PageNo.Text = (pageNo + 1).ToString("d")

        If pageCnt = 0 Then
            rep.DataSource = tbl
        Else
            Dim ls_row_01(tbl.Rows.Count) As DataRow
            tbl.Rows.CopyTo(ls_row_01, 0)

            Dim pos As Integer = pageLineCnt * pageNo
            Dim cnt As Integer = tbl.Rows.Count - pos
            If cnt > pageLineCnt Then
                cnt = pageLineCnt
            End If

            Dim ls_row_02(cnt) As DataRow
            Array.Copy(ls_row_01, pos, ls_row_02, 0, cnt)

            rep.DataSource = ls_row_02.CopyToDataTable()
        End If

        Me.Session(_SESSION_PAGE_CURRENT) = pageNo
        rep.DataBind()
    End Sub

    Protected Sub btn_ToPrev_Click(sender As Object, e As EventArgs)
        TurnPage(DirectCast(Me.Session(_SESSION_PAGE_CURRENT), Integer) - 1)
    End Sub

    Protected Sub btn_ToNext_Click(sender As Object, e As EventArgs)
        TurnPage(DirectCast(Me.Session(_SESSION_PAGE_CURRENT), Integer) + 1)
    End Sub

    Protected Sub tbx_PageNo_TextChanged(sender As Object, e As EventArgs)
        Dim pageNo As Integer
        If Me.tbx_PageNo.Text = String.Empty Then
            pageNo = 0
        Else
            pageNo = Convert.ToInt32(Me.tbx_PageNo.Text)

            If pageNo > 0 Then
                pageNo -= 1
            End If
        End If

        TurnPage(pageNo)
    End Sub

End Class
