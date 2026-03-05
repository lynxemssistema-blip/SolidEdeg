Public Class frmOpcaoLiberacaoOrdemServico

    Private Sub frmOpcaoLiberacaoOrdemServico_Load(sender As Object, e As EventArgs) Handles MyBase.Load


        If My.Settings.TravarEmissaoOS.ToString = "Sim" Then

            optNaoBaixaSaldoTag.Enabled = False

        Else

            optNaoBaixaSaldoTag.Enabled = True

        End If

    End Sub

    Private Sub btnCANCELAR_Click(sender As Object, e As EventArgs) Handles btnCANCELAR.Click

        ' Exit Sub 'Cancela a operação
        Me.Close()

        TipoLiberacaoOrdemServico = "Sair"

    End Sub

    Private Sub optBaixaSaldoTag_CheckedChanged(sender As Object, e As EventArgs) Handles optBaixaSaldoTag.CheckedChanged

        TipoLiberacaoOrdemServico = "Total"

    End Sub

    Private Sub optNaoBaixaSaldoTag_CheckedChanged(sender As Object, e As EventArgs) Handles optNaoBaixaSaldoTag.CheckedChanged
        TipoLiberacaoOrdemServico = "Parcial"
    End Sub

    Private Sub btnLiberarOrdemServico_Click(sender As Object, e As EventArgs) Handles btnLiberarOrdemServico.Click

        If optBaixaSaldoTag.Checked = True Then
            TipoLiberacaoOrdemServico = "Total"
        Else
            TipoLiberacaoOrdemServico = "Parcial"
        End If
        Me.Close()

    End Sub

    Private Sub btnAjuda_Click(sender As Object, e As EventArgs) Handles btnAjuda.Click

        If Me.Height = 180 Then
            Me.Height = 400
        Else
            Me.Height = 180
        End If
    End Sub

End Class