Imports System.IO
Imports System.Windows.Forms

Public Class frmFichaTecnica
    Private Sub btnBuscarArquivo_Click(sender As Object, e As EventArgs) Handles btnBuscarArquivo.Click

        ' 1) Arquivo PDF
        Dim caminhoPdf As String = Nothing
        Dim nomepdf As String
        Using ofd As New OpenFileDialog()
            ofd.Title = "Selecione o arquivo de desenho (PDF)"
            ofd.Filter = "Arquivos PDF (*.pdf)|*.pdf"
            ofd.Multiselect = False
            If ofd.ShowDialog() <> DialogResult.OK Then Exit Sub
            caminhoPdf = ofd.FileName
            nomepdf = ofd.SafeFileName
        End Using
        txtEnderecoArquivo.Text = caminhoPdf.ToUpper
        txtnomeArquivo.Text = nomepdf.ToUpper

        MaterialFichaTecnica.NomeArquivo = txtnomeArquivo.Text
        MaterialFichaTecnica.EnderecoArquivo = txtEnderecoArquivo.Text




    End Sub

    Private Sub btnSalvar_Click(sender As Object, e As EventArgs) Handles btnSalvar.Click

        If MaterialFichaTecnica.NomeArquivo = "" Then
            MessageBox.Show("Selecione um arquivo para salvar.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If


        If MaterialFichaTecnica.EnderecoArquivo = "" Then

            MessageBox.Show("Não há arquivo valido para salvar.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub

        End If


        If Me.txtDescricaoArquivo.Text = "" Then

            MsgBox("Informe uma descrição para o arquivo.", MsgBoxStyle.Exclamation, "Atenção")

            Exit Sub


        End If

        Try




            If MaterialFichaTecnica.idmaterial_fichatecnica.ToString = "" Or MaterialFichaTecnica.idmaterial_fichatecnica.ToString = 0 Then

                MaterialFichaTecnica.SalvarDadosMaterial(MaterialFichaTecnica.idmaterial, Me.txtDescricaoArquivo.Text,
                MaterialFichaTecnica.EnderecoArquivo, MaterialFichaTecnica.NomeArquivo, MaterialFichaTecnica.CodMatFabricante,
                MaterialFichaTecnica.DescResumo, MaterialFichaTecnica.DescDetal, MaterialFichaTecnica.CodMatFabricanteNumeroRP)


            Else

                MaterialFichaTecnica.UpdateDados(MaterialFichaTecnica.idmaterial_fichatecnica,
                                                 MaterialFichaTecnica.idmaterial, Me.txtDescricaoArquivo.Text,
                MaterialFichaTecnica.EnderecoArquivo, MaterialFichaTecnica.NomeArquivo, MaterialFichaTecnica.CodMatFabricante,
                MaterialFichaTecnica.DescResumo, MaterialFichaTecnica.DescDetal, MaterialFichaTecnica.CodMatFabricanteNumeroRP)


            End If




            MessageBox.Show("Ficha técnica salva com sucesso.", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information)

            TimerdgvDados.Enabled = True


        Catch ex As Exception
            Exit Sub

        End Try


    End Sub

    Private Sub dgvDados_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvDados.CellContentClick

    End Sub

    Private Sub dgvDados_Click(sender As Object, e As EventArgs) Handles dgvDados.Click

        MaterialFichaTecnica.idmaterial_fichatecnica = dgvDados.CurrentRow.Cells("idmaterial_fichatecnica").Value.ToString

        MaterialFichaTecnica.NomeArquivo = dgvDados.CurrentRow.Cells("NomeArquivo").Value.ToString
        Me.txtnomeArquivo.Text = MaterialFichaTecnica.NomeArquivo


        MaterialFichaTecnica.EnderecoArquivo = dgvDados.CurrentRow.Cells("EnderecoArquivo").Value.ToString
        Me.txtEnderecoArquivo.Text = MaterialFichaTecnica.EnderecoArquivo

        MaterialFichaTecnica.Descricao = dgvDados.CurrentRow.Cells("Descricao").Value.ToString
        Me.txtDescricaoArquivo.Text = MaterialFichaTecnica.Descricao


    End Sub

    Private Sub btnNovo_Click(sender As Object, e As EventArgs) Handles btnNovo.Click

        Limpar()


    End Sub



    Public Sub Limpar()

        MaterialFichaTecnica.idmaterial_fichatecnica = ""

        Me.txtDescricaoArquivo.Clear()
        Me.txtEnderecoArquivo.Clear()
        Me.txtnomeArquivo.Clear()




    End Sub

    Private Sub btnExcluir_Click(sender As Object, e As EventArgs) Handles btnExcluir.Click

        If MaterialFichaTecnica.idmaterial_fichatecnica.ToString = "" Then
            MessageBox.Show("Selecione um registro para excluir.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub

        Else

            cl_BancoDados.Salvar("DELETE FROM material_fichatecnica WHERE idmaterial_fichatecnica = " & MaterialFichaTecnica.idmaterial_fichatecnica.ToString)
            MessageBox.Show("Registro excluído com sucesso.", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information)
            TimerdgvDados.Enabled = True
            Limpar()


        End If

        TimerdgvDados.Enabled = True

    End Sub

    Private Sub frmFichaTecnica_Load(sender As Object, e As EventArgs) Handles MyBase.Load


        TimerdgvDados.Enabled = True
    End Sub

    Private Sub TimerdgvDados_Tick(sender As Object, e As EventArgs) Handles TimerdgvDados.Tick


        TimerdgvDados.Enabled = False
        Dim sql As String
        sql = "SELECT idmaterial_fichatecnica, NomeArquivo, EnderecoArquivo, Descricao " &
              "FROM material_fichatecnica where idmaterial = '" & MaterialFichaTecnica.idmaterial.ToString & "' " &
              " ORDER BY idmaterial_fichatecnica DESC"

        dgvDados.DataSource = cl_BancoDados.CarregarDados(sql)

    End Sub

    Private Sub btnCancelar_Click(sender As Object, e As EventArgs) Handles btnCancelar.Click

        Me.Close()

    End Sub

    Private Sub dgvDados_DoubleClick(sender As Object, e As EventArgs) Handles dgvDados.DoubleClick

        Dim Endereco As String = dgvDados.CurrentRow.Cells("EnderecoArquivo").Value.ToString()

        If File.Exists(Endereco) Then

            Process.Start(Endereco)

        Else

            MessageBox.Show("Arquivo não encontrado: " & Endereco, "SINCO",
                            MessageBoxButtons.OK, MessageBoxIcon.Exclamation)

        End If

    End Sub

End Class