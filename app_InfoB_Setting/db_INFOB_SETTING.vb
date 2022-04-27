Imports System.Data
Imports System.Data.SqlClient
Imports System.Text

Public Class db_INFOB_SETTING

    Public Shared ReadOnly Property EmptyTable_APP_INFOB_SETTING_WORK_TEMPLATE As DataTable
        Get
            Dim sb As StringBuilder = New StringBuilder(1000)
            sb.AppendLine("SELECT TOP(0) *")
            sb.AppendLine("FROM APP_INFOB_SETTING_WORK_TEMPLATE;")

            Dim cn As SqlConnection = New SqlConnection(SettingConfig.ConnectingString)
            Dim cmd As SqlCommand = New SqlCommand(sb.ToString(), cn)

            Dim adp As SqlDataAdapter = New SqlDataAdapter(cmd)
            Dim tbl As DataTable = New DataTable()

            adp.Fill(tbl)

            adp.Dispose()
            cmd.Dispose()
            cn.Dispose()

            Return tbl
        End Get
    End Property

    Public Shared Function Select_01() As DataTable
        Dim sb As StringBuilder = New StringBuilder(1000)
        sb.AppendLine("SELECT *")
        sb.AppendLine("FROM APP_INFOB_SETTING;")

        Dim cn As SqlConnection = New SqlConnection(SettingConfig.ConnectingString)
        Dim cmd As SqlCommand = New SqlCommand(sb.ToString(), cn)

        Dim adp As SqlDataAdapter = New SqlDataAdapter(cmd)
        Dim tbl As DataTable = New DataTable()
        adp.Fill(tbl)

        adp.Dispose()
        cmd.Dispose()
        cn.Dispose()

        Return tbl
    End Function

    Public Shared ReadOnly Property Table_APP_INFOB_SETTING As DataTable
        Get
            Dim sb As StringBuilder = New StringBuilder(1000)
            sb.AppendLine("SELECT *")
            sb.AppendLine("FROM APP_INFOB_SETTING;")

            Dim cn As SqlConnection = New SqlConnection(SettingConfig.ConnectingString)
            Dim cmd As SqlCommand = New SqlCommand(sb.ToString(), cn)

            Dim adp As SqlDataAdapter = New SqlDataAdapter(cmd)
            Dim tbl As DataTable = New DataTable("CSV_SHOHIN")
            adp.FillSchema(tbl, SchemaType.Mapped)

            adp.Dispose()
            cmd.Dispose()
            cn.Dispose()

            Return tbl
        End Get
    End Property

    Public Shared Function BeginTransaction() As SqlTransaction
        Dim cn As SqlConnection = New SqlConnection(SettingConfig.ConnectingString)
        cn.Open()

        Return cn.BeginTransaction()
    End Function

    Public Shared Sub Reset_APP_INFOB_KAIIN(tbl As DataTable)
        Dim sb As StringBuilder = New StringBuilder(1000), errMsg As String = String.Empty

        Dim trn As SqlTransaction = BeginTransaction()
        Dim cmd As SqlCommand, prm As SqlParameter, adp As SqlDataAdapter

        Do
            sb.Clear()
            sb.AppendLine("SELECT TOP(0) *")
            sb.AppendLine("INTO #wt_BAT_RESET_APP_INFOB_KAIIN_1")
            sb.AppendLine("FROM APP_INFOB_SETTING_WORK_TEMPLATE;")

            cmd = New SqlCommand(sb.ToString(), trn.Connection) With {
                .Transaction = trn
            }

            Try
                cmd.ExecuteNonQuery()
            Catch ex As Exception
                trn.Rollback()
                errMsg = "一時テーブルの作成に失敗しました。"

                Exit Do
            Finally
                cmd.Dispose()
            End Try

            sb.Clear()
            sb.AppendLine("INSERT INTO #wt_BAT_RESET_APP_INFOB_KAIIN_1")
            sb.AppendLine("VALUES (")
            sb.AppendLine(" @FOLDERNM,")
            sb.AppendLine(" @KAIINCD_FROM,")
            sb.AppendLine(" @KAIINCD_TO,")
            sb.AppendLine(" @FLAG")
            sb.AppendLine(");")

            cmd = New SqlCommand(sb.ToString(), trn.Connection) With {
                .Transaction = trn
            }

            prm = cmd.Parameters.Add(New SqlParameter("@FOLDERNM", SqlDbType.VarChar, 10))
            prm.SourceColumn = "FOLDERNM"
            prm = cmd.Parameters.Add(New SqlParameter("@KAIINCD_FROM", SqlDbType.Int))
            prm.SourceColumn = "KAIINCD_FROM"
            prm = cmd.Parameters.Add(New SqlParameter("@KAIINCD_TO", SqlDbType.Int))
            prm.SourceColumn = "KAIINCD_TO"
            prm = cmd.Parameters.Add(New SqlParameter("@FLAG", SqlDbType.TinyInt))
            prm.SourceColumn = "FLAG"

            adp = New SqlDataAdapter With {
                .InsertCommand = cmd
            }

            Try
                adp.Update(tbl)
            Catch ex As Exception
                trn.Rollback()
                errMsg = "一時テーブルへのレコード挿入に失敗しました。"

                Exit Do
            Finally
                cmd.Dispose()
                adp.Dispose()
            End Try

            cmd = New SqlCommand(sb.ToString(), trn.Connection) With {
                .Transaction = trn,
                .CommandType = CommandType.StoredProcedure,
                .CommandText = "BAT_RESET_APP_INFOB_KAIIN"
            }

            Try
                cmd.ExecuteNonQuery()

                trn.Commit()
            Catch ex As Exception
                trn.Rollback()
                errMsg = "ＤＢ更新に失敗しました。（" + ex.Message + "）"
            Finally
                cmd.Dispose()
                adp.Dispose()
            End Try
        Loop While False

        trn.Dispose()

        If errMsg <> String.Empty Then
            Throw New ApplicationException(errMsg)
        End If
    End Sub

End Class
