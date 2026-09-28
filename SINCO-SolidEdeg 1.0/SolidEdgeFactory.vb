Imports System.Runtime.InteropServices

Public Module SolidEdgeFactory
    Private _app As Object

    Public Function GetApp(Optional startIfNotRunning As Boolean = True) As Object
        If _app IsNot Nothing Then Return _app

        ' 0x800401E3 = operação não disponível (ex.: não existe instância no ROT)
        Try
            _app = Marshal.GetActiveObject("SolidEdge.Application")
            Return _app
        Catch ex As COMException When ex.ErrorCode = &H800401E3
            If Not startIfNotRunning Then Return Nothing
        End Try

        ' Tenta abrir nova instância
        Dim t = Type.GetTypeFromProgID("SolidEdge.Application")
        If t Is Nothing Then
            Throw New InvalidOperationException("Solid Edge não está instalado/registrado (ProgID não encontrado).")
        End If

        Try
            _app = Activator.CreateInstance(t)
            Try : CallByName(_app, "Visible", CallType.Let, True) : Catch : End Try
            Return _app
        Catch ex As COMException
            Throw New InvalidOperationException("Falha ao iniciar Solid Edge via COM: " & ex.Message, ex)
        End Try
    End Function

    ''' <summary>
    ''' Garante que o Solid Edge esteja 100% interativo, visível e acessível ao usuário
    ''' </summary>
    Public Sub AtivarSolidEdge()
        Try
            Dim se = GetApp(False)
            If se IsNot Nothing Then
                Try : CallByName(se, "Visible", CallType.Let, True) : Catch : End Try
                Try : CallByName(se, "Interactive", CallType.Let, True) : Catch : End Try
                Try : CallByName(se, "ScreenUpdating", CallType.Let, True) : Catch : End Try
                Try : CallByName(se, "DisplayAlerts", CallType.Let, True) : Catch : End Try
                Try
                    Dim hwnd As IntPtr = CType(CallByName(se, "hWnd", CallType.Get), IntPtr)
                    If hwnd <> IntPtr.Zero Then
                        SetForegroundWindow(hwnd)
                    End If
                Catch
                End Try
            End If
        Catch
        End Try
    End Sub

    <DllImport("user32.dll")>
    Private Function SetForegroundWindow(hWnd As IntPtr) As Boolean
    End Function
End Module

