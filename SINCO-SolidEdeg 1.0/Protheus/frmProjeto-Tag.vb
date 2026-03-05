




Public Class frmProjeto_Tag

    Private idProjeto As Integer
    Private c5_num As String
    Private c6_num As String

    Private Sub TimerdgvProjeto_Tick(sender As Object, e As EventArgs) Handles TimerdgvProjeto.Tick


        Protheus.CarregarPedidosNoGrid(dgvProjeto, txtPesqCliente.Text, Me.txtPesqProjeto.Text, Me)

        TimerdgvProjeto.Enabled = False




    End Sub

    Private Sub frmProjeto_Tag_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        TimerdgvProjeto.Enabled = True


    End Sub

    Private Sub TimerdgvTag_Tick(sender As Object, e As EventArgs) Handles TimerdgvTag.Tick

        Protheus.CarregarItensPedidosNoGrid(dgvTag, c5_num)

        TimerdgvTag.Enabled = False


    End Sub

    Private Sub dgvProjeto_Click(sender As Object, e As EventArgs) Handles dgvProjeto.Click
        Try

            c5_num = dgvProjeto.CurrentRow.Cells("C5_NUM").Value.ToString


        Catch ex As Exception
        Finally

        End Try

        TimerdgvTag.Enabled = True

    End Sub

    Private Sub txtPesqCliente_TextChanged(sender As Object, e As EventArgs) Handles txtPesqCliente.TextChanged
        TimerdgvProjeto.Enabled = True
    End Sub

    Private Sub txtPesqProjeto_TextChanged(sender As Object, e As EventArgs) Handles txtPesqProjeto.TextChanged
        TimerdgvProjeto.Enabled = True
    End Sub

    Private Sub dgvProjeto_CellContentClick(sender As Object, e As Windows.Forms.DataGridViewCellEventArgs) Handles dgvProjeto.CellContentClick

    End Sub

    Private Sub dgvProjeto_DoubleClick(sender As Object, e As EventArgs) Handles dgvProjeto.DoubleClick


    End Sub

    Private Sub dgvTag_CellContentClick(sender As Object, e As Windows.Forms.DataGridViewCellEventArgs) Handles dgvTag.CellContentClick

    End Sub

    Private Sub dgvTag_DoubleClick(sender As Object, e As EventArgs) Handles dgvTag.DoubleClick

    End Sub

    Private Sub btnExportar_Click(sender As Object, e As EventArgs) Handles btnExportar.Click



        cl_BancoDados.RetornaCampoDaPesquisa("select idProjeto FROM projetos where Projeto = '" & c5_num & "' and
                                                                          (D_E_L_E_T_E   is null or D_E_L_E_T_E  = '')", "idProjeto")


        Try
            ' c5_num = VCampo1
            idProjeto = VCampo0
        Catch ex As Exception
            idProjeto = 0
        End Try


        If idProjeto = 0 Then

            Projeto.SalvarDados("", c5_num,
                                             "Dados coletado do Protheus",
                                             Usuario.NomeCompleto.ToUpper,
                                             "",  '   dados relativos ao comercial e financeiro , vem da tela principal
                                             "",
                                             "",
                                             "",
                                             "",
                                             "",
                                             "",
                                             dgvProjeto.CurrentRow.Cells("a1_nome").Value.ToString,
                                             dgvProjeto.CurrentRow.Cells("emissao").Value.ToString,' ClasseProjeto.DataEntrada,
                                             "MG",
                                             "",
                                             "S")



        Else

            Projeto.UpdateDados(idProjeto,
                               c5_num,
                                             "Dados coletado do Protheus",
                                             Usuario.NomeCompleto.ToUpper,
                                             "",  '   dados relativos ao comercial e financeiro , vem da tela principal
                                             "",
                                            dgvProjeto.CurrentRow.Cells("a1_nome").Value.ToString,
                                             "",
                                            dgvProjeto.CurrentRow.Cells("emissao").Value.ToString,' ClasseProjeto.DataEntrada,
                                            "",
                                             "MG")


        End If



        cl_BancoDados.RetornaCampoDaPesquisa("select idProjeto FROM projetos where Projeto = '" & c5_num & "' and
                                                                          (D_E_L_E_T_E   is null or D_E_L_E_T_E  = '')", "idProjeto")


        Try
            idProjeto = VCampo0
        Catch ex As Exception
            idProjeto = 0

        End Try


        If idProjeto > 0 Then

            For i As Integer = 0 To dgvTag.Rows.Count - 1

                Try

                    If dgvTag.Rows(i).Cells("c5_num").Value.ToString <> "" Then

                        TagProjeto.SalvarDadosTags(idProjeto,
                                          dgvTag.Rows(i).Cells("c5_num").Value.ToString,
                                          dgvTag.Rows(i).Cells("c6_produto").Value.ToString, '  Me.txtTag.Text.Trim.ToUpper,
                                          dgvTag.Rows(i).Cells("descricao_produto_pedido").Value.ToString, '  Me.txtDescricao.Text.Trim.ToUpper,
                                          dgvTag.Rows(i).Cells("c6_qtdven").Value.ToString, '  txtQuantidade.Text.Trim.ToUpper,
                                          "", '   ClasseProjeto.IdEmpresa,
                                          dgvTag.Rows(i).Cells("nome_cliente").Value.ToString, '  ClasseProjeto.DescEmpresa,
                                          "PRODUTO", ' cboTipoProduto.Text.Trim.ToUpper,
                                          Date.Now.Date,  'dataatual,
                                          "",
                                          "cj", '"Cbounidade.Text.Trim.ToUpper,
                                          Date.Now.Date,  'DTPImplantacaoPedido.Value.ToString("dd/MM/yyyy"),
                                          "")
                    End If


                Catch ex As Exception
                    Continue For
                End Try

            Next
        End If



        MsgBox("Processo Finalizado!!", vbInformation, "Atenção")


    End Sub
End Class