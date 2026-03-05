Public Class frmListaDesenhoCliente
    Private Sub TimerdgvDesenhosClientesProtheus_Tick(sender As Object, e As EventArgs) Handles TimerdgvDesenhosClientesProtheus.Tick

        Protheus.CarregarDesenhoMetalfisaCliente(dgvDesenhosClientesProtheus,
                                                 DadosArquivoCorrente.NomeArquivoSemExtensao.Replace(".psm", "").Replace(".par", "").Replace(".prt", ""))


        TimerdgvDesenhosClientesProtheus.Enabled = False

    End Sub

    Private Sub frmListaDesenhoCliente_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        TimerdgvDesenhosClientesProtheus.Enabled = True

    End Sub

    Private Sub AssociarItemDesenhoClienteProthuesComDesenhoArquivoToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AssociarItemDesenhoClienteProthuesComDesenhoArquivoToolStripMenuItem.Click

        If MaterialDesenhoCliente.z1_codcli.ToString <> "" Then

            MaterialDesenhoCliente.SalvarDados(MaterialDesenhoCliente.z1_nomecli,
                                               MaterialDesenhoCliente.z1_produto,
                                               MaterialDesenhoCliente.z1_codcli,
                                               MaterialDesenhoCliente.z1_revisao,
                                               MaterialDesenhoCliente.z1_desccli,
                                               MaterialDesenhoCliente.enderecoarquivo_cliente,
                                               MaterialDesenhoCliente.enderecoarquivo,
                                               MaterialDesenhoCliente.desenho)

            ' frmDadosPecaCorrente.TimerdgvGabaritos.Enabled = True

        End If

    End Sub

    Private Sub dgvDesenhosClientesProtheus_Click(sender As Object, e As EventArgs) Handles dgvDesenhosClientesProtheus.Click

        If dgvDesenhosClientesProtheus.CurrentRow.Cells("z1_codcli").Value.ToString <> "" Then

            MaterialDesenhoCliente.desenho = DadosArquivoCorrente.NomeArquivoSemExtensao
            MaterialDesenhoCliente.enderecoarquivo = DadosArquivoCorrente.EnderecoArquivo
            MaterialDesenhoCliente.enderecoarquivo_cliente = ""
            MaterialDesenhoCliente.z1_codcli = dgvDesenhosClientesProtheus.CurrentRow.Cells("z1_codcli").Value.ToString
            MaterialDesenhoCliente.z1_desccli = dgvDesenhosClientesProtheus.CurrentRow.Cells("z1_desccli").Value.ToString
            MaterialDesenhoCliente.z1_nomecli = dgvDesenhosClientesProtheus.CurrentRow.Cells("z1_nomecli").Value.ToString
            MaterialDesenhoCliente.z1_produto = dgvDesenhosClientesProtheus.CurrentRow.Cells("z1_produto").Value.ToString
            MaterialDesenhoCliente.z1_revisao = dgvDesenhosClientesProtheus.CurrentRow.Cells("z1_revisao").Value.ToString

        End If

    End Sub

End Class