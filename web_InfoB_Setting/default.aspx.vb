Public Class _default
    Inherits System.Web.UI.Page

    Protected _SESSION_REP_DTTBL As String = Me.UniqueID + "_Rep_DtTbl"
    Protected _SESSION_EDIT_RESULT_CTRL As String = Me.UniqueID + "_Edit_ResultCtrl"

    Protected ReadOnly Property _list_tbl As DataTable
        Get
            Return DirectCast(Me.Session(_SESSION_REP_DTTBL), DataTable)
        End Get
    End Property

    Protected ReadOnly Property _isEdited As Boolean
        Get
            For Each row As DataRow In _list_tbl.Rows
                Select Case row.RowState
                    Case DataRowState.Modified
                        Return True
                    Case DataRowState.Added
                        If DirectCast(row("DelFlg"), Integer) = 0 Then
                            Return True
                        End If
                End Select
            Next

            Return False
        End Get
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        setEvh()

        If Not Me.IsPostBack Then
            lrl_version.Text = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString()

            initCtrl()
        End If
    End Sub

    Protected Sub initCtrl()
        Me.lrl_wndCap.Text = SettingConfig.WindowCaption_InfoB()

        selectData()

        Me.uc_Confirm_Redraw.Init_PopUp("編 集", "画面に加えた変更は破棄されます。<br />よろしいですか？")
        Me.uc_Confirm_Update.Init_PopUp("ＤＢ", "データベースを更新します。<br />よろしいですか？")
        Me.uc_Confirm_Update_Deploy.Init_PopUp("ＤＢ", "データベース更新の後、関連データを再作成します。<br />よろしいですか？")
    End Sub

    Protected Sub setEvh()
        AddHandler Me.uc_Confirm_Redraw.Button_OK.Click, AddressOf Me.uc_Confirm_RedrawScreen_OK_Click
        AddHandler Me.uc_Confirm_Update.Button_OK.Click, AddressOf Me.uc_Confirm_UpdateDB_OK_Click
        AddHandler Me.uc_Confirm_Update_Deploy.Button_OK.Click, AddressOf Me.uc_Confirm_UpdateDeployDB_OK_Click

        AddHandler Me.uc_InpKaiinCd.Button_OK.Click, AddressOf Me.uc_InpKaiinCd_OK_Click

        Me.uc_ListPager_Item.Sub_Before_TurnPage = AddressOf Me.pageToDataTable
    End Sub

    Protected Sub initPage(Optional tail As Boolean = False)
        Me.uc_ListPager_Item.Init_ListPager(Me.rep_Header.ID, DirectCast(Me.Session(_SESSION_REP_DTTBL), DataTable), SettingConfig.ListRowCount_InfoB, tail)
    End Sub

    Protected Sub selectData()
        Me.Session(_SESSION_REP_DTTBL) = db_APP_INFOB_SETTING.Select_01()

        initPage()
    End Sub

    Protected Sub pageToDataTable()
        Dim idx As Integer = Me.uc_ListPager_Item.CurrentPosition
        Dim row As DataRow, val_str As String, val_int As Integer, val_byt As Byte

        For Each item As RepeaterItem In Me.rep_Header.Items
            row = _list_tbl.Rows(idx)

            val_str = DirectCast(item.FindControl("tbx_folder"), TextBox).Text
            If val_str = String.Empty Then
                If row("FOLDERNM") IsNot DBNull.Value Then
                    row("FOLDERNM") = DBNull.Value
                End If
            ElseIf row("FOLDERNM") Is DBNull.Value Then
                row("FOLDERNM") = val_str
            ElseIf val_str <> DirectCast(row("FOLDERNM"), String) Then
                row("FOLDERNM") = val_str
            End If

            val_str = DirectCast(item.FindControl("tbx_memo"), TextBox).Text
            If val_str = String.Empty Then
                If row("MEMO") IsNot DBNull.Value Then
                    row("MEMO") = DBNull.Value
                End If
            ElseIf row("MEMO") Is DBNull.Value Then
                row("MEMO") = val_str
            ElseIf val_str <> DirectCast(row("MEMO"), String) Then
                row("MEMO") = val_str
            End If

            val_str = DirectCast(item.FindControl("tbx_kaiincd_disp"), TextBox).Text
            If val_str = String.Empty Then
                If row("KAIINCD") IsNot DBNull.Value Then
                    row("KAIINCD") = DBNull.Value
                End If
            ElseIf row("KAIINCD") Is DBNull.Value Then
                row("KAIINCD") = val_str
            ElseIf val_str <> DirectCast(row("KAIINCD"), String) Then
                row("KAIINCD") = val_str
            End If

            Try
                val_byt = Convert.ToByte(DirectCast(item.FindControl("rbl_flag"), RadioButtonList).SelectedValue)
                If row("FLAG") Is DBNull.Value Then
                    row("FLAG") = val_byt
                ElseIf val_byt <> DirectCast(row("FLAG"), Byte) Then
                    row("FLAG") = val_byt
                End If
            Catch
                If row("FLAG") IsNot DBNull.Value Then
                    row("FLAG") = DBNull.Value
                End If
            End Try

            val_int = If(DirectCast(item.FindControl("cbx_delete"), CheckBox).Checked, 1, 0)
            If val_int <> DirectCast(row("DelFlg"), Integer) Then
                row("DelFlg") = val_int
            End If

            idx += 1
        Next
    End Sub

    Protected Function writeToDB() As String
        Dim tbl As DataTable = _list_tbl.Copy()

        For Each row As DataRow In tbl.Rows
            If DirectCast(row("DelFlg"), Integer) = 1 Then
                If row.RowState = DataRowState.Added Then
                    row.AcceptChanges()
                Else
                    row.Delete()
                End If
            End If
        Next

        Try
            db_APP_INFOB_SETTING.Update_01(tbl)
            _list_tbl.AcceptChanges()
        Catch ex As Exception
            Return "エラーが発生しました。更新に失敗しました。<br />" + ex.Message
        End Try

        Return String.Empty
    End Function

    Protected Function writeToDB_2() As String
        Dim errMsg As String = writeToDB()
        If errMsg <> String.Empty Then
            Return "エラーが発生しました。更新に失敗しました。<br />" + errMsg
        End If

        Try
            Dim psi As ProcessStartInfo = New ProcessStartInfo()
            psi.FileName = SettingConfig.AppInfoBSetting
            psi.CreateNoWindow = True
            psi.UseShellExecute = False
            psi.WorkingDirectory = System.IO.Path.GetDirectoryName(psi.FileName)
            psi.RedirectStandardError = True

            Using p As Process = New Process()
                p.StartInfo = psi
                p.Start()

                p.WaitForExit()
                If p.ExitCode <> 0 Then
                    Throw New ApplicationException(p.StandardError.ReadToEnd())
                End If
            End Using
        Catch ex As Exception
            errMsg = "更新データの反映でエラーが発生しました。"
            errMsg += "<br />"
            errMsg += ex.Message
            errMsg += "<br /><br />"
            errMsg += "<span style=""font-size: 0.9em; color: steelblue;"">※ 画面入力内容は、ＤＢに登録されました。</span>"

            Return errMsg
        End Try

        Return String.Empty
    End Function

    Protected Sub rep_Header_ItemDataBound(sender As Object, e As RepeaterItemEventArgs)
        If e.Item.ItemType <> ListItemType.Item AndAlso e.Item.ItemType <> ListItemType.AlternatingItem Then
            Return
        End If

        DirectCast(e.Item.FindControl("tbx_folder"), TextBox).Text = If(DataBinder.Eval(e.Item.DataItem, "FOLDERNM") Is DBNull.Value, String.Empty, DirectCast(DataBinder.Eval(e.Item.DataItem, "FOLDERNM"), String))
        DirectCast(e.Item.FindControl("tbx_memo"), TextBox).Text = If(DataBinder.Eval(e.Item.DataItem, "MEMO") Is DBNull.Value, String.Empty, DirectCast(DataBinder.Eval(e.Item.DataItem, "MEMO"), String))
        DirectCast(e.Item.FindControl("tbx_kaiincd_disp"), TextBox).Text = If(DataBinder.Eval(e.Item.DataItem, "KAIINCD") Is DBNull.Value, String.Empty, DirectCast(DataBinder.Eval(e.Item.DataItem, "KAIINCD"), String))
        DirectCast(e.Item.FindControl("rbl_flag"), RadioButtonList).SelectedValue = DirectCast(DataBinder.Eval(e.Item.DataItem, "FLAG"), Byte).ToString("d")

        DirectCast(e.Item.FindControl("cbx_delete"), CheckBox).Checked = (DirectCast(DataBinder.Eval(e.Item.DataItem, "DelFlg"), Integer) = 1)
    End Sub

    Protected Sub rep_Header_ItemCommand(source As Object, e As RepeaterCommandEventArgs)
        Select Case e.CommandName
            Case "toEdit_click"
                Dim tbx As TextBox = DirectCast(e.Item.FindControl("tbx_kaiincd_disp"), TextBox)

                Me.Session(_SESSION_EDIT_RESULT_CTRL) = tbx.UniqueID

                Me.uc_InpKaiinCd.TextBox_KaiinCdEdit.Text = DirectCast(e.Item.FindControl("tbx_kaiincd_disp"), TextBox).Text
                Me.uc_InpKaiinCd.ModalPopupExtender_01.Show()
        End Select
    End Sub

    Protected Sub uc_InpKaiinCd_OK_Click(sender As Object, e As EventArgs)
        Dim ctrl As Control = Me.FindControl(DirectCast(Me.Session(_SESSION_EDIT_RESULT_CTRL), String))

        DirectCast(ctrl, TextBox).Text = Me.uc_InpKaiinCd.TextBox_KaiinCdEdit.Text
    End Sub

    Protected Sub btn_Redraw_Click(sender As Object, e As EventArgs)
        pageToDataTable()

        If Me._isEdited Then
            Me.uc_Confirm_Redraw.ModalPopupExtender_01.Show()

            Return
        End If

        uc_Confirm_RedrawScreen_OK_Click(Nothing, Nothing)
    End Sub

    Protected Sub uc_Confirm_RedrawScreen_OK_Click(sender As Object, e As EventArgs)
        initCtrl()
    End Sub

    Protected Sub btn_NewRecord_Click(sender As Object, e As EventArgs)
        pageToDataTable()

        Dim row As DataRow = _list_tbl.NewRow()
        row("FLAG") = 1     ' デフォルトは表示
        row("DelFlg") = 0

        _list_tbl.Rows.Add(row)

        initPage(True)
    End Sub

    Protected Sub btn_Update_Click(sender As Object, e As EventArgs)
        pageToDataTable()

        If Not Me._isEdited Then
            Me.uc_Express.ShowPopup("ＤＢ", "変更されていません。")

            Return
        End If

        Me.uc_Confirm_Update.ModalPopupExtender_01.Show()
    End Sub

    Protected Sub uc_Confirm_UpdateDB_OK_Click(sender As Object, e As EventArgs)
        Dim err As String = writeToDB()
        If err <> String.Empty Then
            Me.uc_Express.ShowPopup("ＤＢ", err)

            Return
        End If

        initCtrl()

        Me.uc_Express.ShowPopup("ＤＢ", "更新しました。")
    End Sub

    Protected Sub btn_Update_Deploy_Click(sender As Object, e As EventArgs)
        pageToDataTable()

        ' 反映のみ行いたい場合を考慮し、変更有無チェックは行わない。
#If False Then
        If Not Me._isEdited Then
            Me.uc_Express.ShowPopup("ＤＢ", "変更されていません。")

            Return
        End If
#End If

        Me.uc_Confirm_Update_Deploy.ModalPopupExtender_01.Show()
    End Sub

    Protected Sub uc_Confirm_UpdateDeployDB_OK_Click(sender As Object, e As EventArgs)
        Dim err As String = writeToDB_2()
        If err <> String.Empty Then
            Me.uc_Express.ShowPopup("ＤＢ", err)

            Return
        End If

        initCtrl()

        Me.uc_Express.ShowPopup("ＤＢ", "更新しました。")
    End Sub
End Class
