﻿Imports System.Web.SessionState

Public Class Global_asax
    Inherits System.Web.HttpApplication

    Sub Application_Start(ByVal sender As Object, ByVal e As EventArgs)
        ' アプリケーションの起動時に呼び出されます

        BundleOperation.RegisterBundles(System.Web.Optimization.BundleTable.Bundles)
        System.Web.Optimization.BundleTable.EnableOptimizations = True

        ' jQueryの登録
        Dim srd As New ScriptResourceDefinition()
        srd.Path = "~/Scripts/JS/jquery-3.6.0.min.js"
        srd.DebugPath = "~/Scripts/JS/jquery-3.6.0.min.js"
        ScriptManager.ScriptResourceMapping.AddDefinition("jquery", Nothing, srd)
    End Sub

    Sub Session_Start(ByVal sender As Object, ByVal e As EventArgs)
        ' セッションの開始時に呼び出されます
    End Sub

    Sub Application_BeginRequest(ByVal sender As Object, ByVal e As EventArgs)
        ' 各要求の開始時に呼び出されます
    End Sub

    Sub Application_AuthenticateRequest(ByVal sender As Object, ByVal e As EventArgs)
        ' 使用の認証時に呼び出されます
    End Sub

    Sub Application_Error(ByVal sender As Object, ByVal e As EventArgs)
        ' エラーの発生時に呼び出されます
    End Sub

    Sub Session_End(ByVal sender As Object, ByVal e As EventArgs)
        ' セッションの終了時に呼び出されます
    End Sub

    Sub Application_End(ByVal sender As Object, ByVal e As EventArgs)
        ' アプリケーションの終了時に呼び出されます
    End Sub

End Class