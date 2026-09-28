Imports System
Imports System.Runtime.InteropServices

''' <summary>
''' Implementação do filtro de mensagens OLE (IOleMessageFilter) para o Solid Edge.
''' Gerencia automaticamente quando o Solid Edge está ocupado (0x8001010A RPC_E_SERVERCALL_RETRYLATER)
''' e quando chamadas COM são rejeitadas temporariamente (0x80010001 RPC_E_CALL_REJECTED).
''' </summary>
Public Class OleMessageFilter
    Implements IOleMessageFilter

    Private Shared _registered As Boolean = False

    Public Shared Sub Register()
        If _registered Then Return
        Dim newFilter As IOleMessageFilter = New OleMessageFilter()
        Dim oldFilter As IOleMessageFilter = Nothing
        CoRegisterMessageFilter(newFilter, oldFilter)
        _registered = True
    End Sub

    Public Shared Sub Revoke()
        If Not _registered Then Return
        Dim oldFilter As IOleMessageFilter = Nothing
        CoRegisterMessageFilter(Nothing, oldFilter)
        _registered = False
    End Sub

    Public Function HandleInComingCall(dwCallType As UInteger, htaskCaller As IntPtr, dwTickCount As UInteger, lpInterfaceInfo As IntPtr) As Integer Implements IOleMessageFilter.HandleInComingCall
        Return 0 ' SERVERCALL_ISHANDLED
    End Function

    Public Function RetryRejectedCall(htaskCallee As IntPtr, dwTickCount As UInteger, dwRejectType As Integer) As Integer Implements IOleMessageFilter.RetryRejectedCall
        If dwRejectType = 2 Then ' SERVERCALL_RETRYLATER (Solid Edge ocupado recalculando ou abrindo arquivo)
            Return 150 ' Tenta novamente em 150 milissegundos
        End If
        Return -1 ' Cancela a chamada se for rejeição definitiva
    End Function

    Public Function MessagePending(htaskCallee As IntPtr, dwTickCount As UInteger, dwPendingType As UInteger) As Integer Implements IOleMessageFilter.MessagePending
        Return 2 ' PENDINGMSG_WAITDEFPROCESS (continua aguardando o Solid Edge responder)
    End Function

    <DllImport("Ole32.dll")>
    Private Shared Function CoRegisterMessageFilter(newFilter As IOleMessageFilter, ByRef oldFilter As IOleMessageFilter) As Integer
    End Function
End Class

<ComImport(), Guid("00000016-0000-0000-C000-000000000046"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)>
Interface IOleMessageFilter
    <PreserveSig> Function HandleInComingCall(dwCallType As UInteger, htaskCaller As IntPtr, dwTickCount As UInteger, lpInterfaceInfo As IntPtr) As Integer
    <PreserveSig> Function RetryRejectedCall(htaskCallee As IntPtr, dwTickCount As UInteger, dwRejectType As Integer) As Integer
    <PreserveSig> Function MessagePending(htaskCallee As IntPtr, dwTickCount As UInteger, dwPendingType As UInteger) As Integer
End Interface
