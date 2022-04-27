Imports System.Data
Imports System.Data.SqlClient
Imports System.Text

Public Class db_APP_INFOB_SETTING

    Public Shared Function Select_01() As DataTable
        Dim sb As StringBuilder = New StringBuilder(1000)
        sb.AppendLine("SELECT *,")
        sb.AppendLine(" CAST(0 AS int) AS DelFlg")
        sb.AppendLine("FROM APP_INFOB_SETTING")
        sb.AppendLine("ORDER BY FOLDERNM;")

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

    Public Shared Sub Update_01(tbl As DataTable)
        Dim kousinYmd As DateTime = DateTime.Now

        Dim cn As SqlConnection = New SqlConnection(SettingConfig.ConnectingString)
        Dim adp As SqlDataAdapter = New SqlDataAdapter()

        Dim sb As StringBuilder = New StringBuilder(1000)
        sb.AppendLine("UPDATE APP_INFOB_SETTING")
        sb.AppendLine("SET")
        sb.AppendLine(" FOLDERNM = @FOLDERNM,")
        sb.AppendLine(" KAIINCD = @KAIINCD,")
        sb.AppendLine(" FLAG = @FLAG,")
        sb.AppendLine(" MEMO = @MEMO,")
        sb.AppendLine(" UPDAT = @UPDAT")
        sb.AppendLine("WHERE FOLDERNM = @org_FOLDERNM;")

        Dim cmd As SqlCommand = New SqlCommand(sb.ToString(), cn)
        Dim prm As SqlParameter
        prm = cmd.Parameters.Add(New SqlParameter("@FOLDERNM", SqlDbType.VarChar, 10))
        prm.SourceColumn = "FOLDERNM"
        prm = cmd.Parameters.Add(New SqlParameter("@KAIINCD", SqlDbType.NVarChar))
        prm.SourceColumn = "KAIINCD"
        prm = cmd.Parameters.Add(New SqlParameter("@FLAG", SqlDbType.TinyInt))
        prm.SourceColumn = "FLAG"
        prm = cmd.Parameters.Add(New SqlParameter("@MEMO", SqlDbType.NVarChar, 200))
        prm.SourceColumn = "MEMO"
        prm = cmd.Parameters.Add(New SqlParameter("@UPDAT", SqlDbType.DateTime))
        prm.Value = kousinYmd

        prm = cmd.Parameters.Add(New SqlParameter("@org_FOLDERNM", SqlDbType.VarChar, 10))
        prm.SourceColumn = "FOLDERNM"
        prm.SourceVersion = DataRowVersion.Original

        adp.UpdateCommand = cmd

        sb.Clear()
        sb.AppendLine("INSERT INTO APP_INFOB_SETTING (")
        sb.AppendLine(" FOLDERNM,")
        sb.AppendLine(" KAIINCD,")
        sb.AppendLine(" FLAG,")
        sb.AppendLine(" MEMO,")
        sb.AppendLine(" INSDAT,")
        sb.AppendLine(" UPDAT")
        sb.AppendLine(")")
        sb.AppendLine("VALUES (")
        sb.AppendLine(" @FOLDERNM,")
        sb.AppendLine(" @KAIINCD,")
        sb.AppendLine(" @FLAG,")
        sb.AppendLine(" @MEMO,")
        sb.AppendLine(" @UPDAT,")
        sb.AppendLine(" @UPDAT")
        sb.AppendLine(");")

        cmd = New SqlCommand(sb.ToString(), cn)
        prm = cmd.Parameters.Add(New SqlParameter("@FOLDERNM", SqlDbType.VarChar, 10))
        prm.SourceColumn = "FOLDERNM"
        prm = cmd.Parameters.Add(New SqlParameter("@KAIINCD", SqlDbType.NVarChar))
        prm.SourceColumn = "KAIINCD"
        prm = cmd.Parameters.Add(New SqlParameter("@FLAG", SqlDbType.TinyInt))
        prm.SourceColumn = "FLAG"
        prm = cmd.Parameters.Add(New SqlParameter("@MEMO", SqlDbType.NVarChar, 200))
        prm.SourceColumn = "MEMO"
        prm = cmd.Parameters.Add(New SqlParameter("@UPDAT", SqlDbType.DateTime))
        prm.Value = kousinYmd

        adp.InsertCommand = cmd

        sb.Clear()
        sb.AppendLine("DELETE FROM APP_INFOB_SETTING")
        sb.AppendLine("WHERE FOLDERNM = @org_FOLDERNM;")

        cmd = New SqlCommand(sb.ToString(), cn)
        prm = cmd.Parameters.Add(New SqlParameter("@org_FOLDERNM", SqlDbType.VarChar, 10))
        prm.SourceColumn = "FOLDERNM"
        prm.SourceVersion = DataRowVersion.Original

        adp.DeleteCommand = cmd

        Try
            adp.Update(tbl)
        Catch ex As Exception
            Throw New ApplicationException(ex.Message, ex)
        Finally
            adp.Dispose()
            cn.Dispose()
        End Try
    End Sub

    Public Shared Sub ExecuteProcedure_01()
        Dim cn As SqlConnection = New SqlConnection(SettingConfig.ConnectingString)

        Dim cmd As SqlCommand = New SqlCommand()
        cmd.Connection = cn
        cmd.CommandType = CommandType.StoredProcedure
        cmd.CommandText = "BAT_RESET_APP_INFOB_KAIIN"

        Try
            cn.Open()
            cmd.ExecuteNonQuery()
        Catch ex As Exception
            Throw New ApplicationException(ex.Message, ex)
        Finally
            cmd.Dispose()
            cn.Dispose()
        End Try
    End Sub
End Class
