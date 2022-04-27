Module Module1

    Sub Main()
        Try
            Console.WriteLine("■ [APP_INFOB_KAIIN] テーブル レコードを再設定中　…　（ ver：" + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString() + "）")
            Console.WriteLine("")

            Dim tbl_src As DataTable = db_INFOB_SETTING.Select_01()
            Dim tbl_dest As DataTable = db_INFOB_SETTING.EmptyTable_APP_INFOB_SETTING_WORK_TEMPLATE
            Dim row As DataRow
            Dim ls_line As String(), ls_cd As String()

            For Each row_src As DataRow In tbl_src.Rows
                ls_line = DirectCast(row_src("KAIINCD"), String).Split(vbLf)

                For Each line As String In ls_line
                    row = tbl_dest.NewRow()
                    row("FOLDERNM") = row_src("FOLDERNM")
                    row("FLAG") = row_src("FLAG")

                    ls_cd = line.Split("-"c)

                    ' 不正な行は無視する
                    Try
                        Select Case ls_cd.Length
                            Case 1
                                row("KAIINCD_FROM") = Integer.Parse(ls_cd(0))
                                row("KAIINCD_TO") = row("KAIINCD_FROM")
                            Case 2
                                row("KAIINCD_FROM") = Integer.Parse(ls_cd(0))
                                row("KAIINCD_TO") = Integer.Parse(ls_cd(1))
                            Case Else
                                Continue For
                        End Select
                    Catch
                        Continue For
                    End Try

                    tbl_dest.Rows.Add(row)
                Next
            Next

            db_INFOB_SETTING.Reset_APP_INFOB_KAIIN(tbl_dest)

            Console.WriteLine("…　処理が正常に終了しました。")
            Console.WriteLine("")
        Catch ex As Exception
            Console.Error.WriteLine("** エラー **" + vbCrLf + ex.Message)
            Console.WriteLine("")

            System.Environment.ExitCode = 1
        End Try
    End Sub

End Module
