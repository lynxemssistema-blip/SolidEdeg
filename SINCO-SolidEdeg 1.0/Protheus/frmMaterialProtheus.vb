Public Class frmMaterialProtheus
    Private Sub TimerdgvMateriaisProtheus_Tick(sender As Object, e As EventArgs) Handles TimerdgvMateriaisProtheus.Tick


        Protheus.CarregarMaterialProtheus(dgvMateriaisProtheus,
                                                 Me.TxtPesqRP.Text, Me.txtPesqDescricao1.Text, Me.txtPesqDescricao2.Text, Me.txtPesqDescricao3.Text)

        TimerdgvMateriaisProtheus.Enabled = False


    End Sub

    Private Sub frmMaterialProtheus_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        TimerdgvMateriaisProtheus.Enabled = True

    End Sub

    Private Sub TxtPesqRP_TextChanged(sender As Object, e As EventArgs) Handles TxtPesqRP.TextChanged
        TimerdgvMateriaisProtheus.Enabled = True
    End Sub

    Private Sub txtPesqDescricao1_TextChanged(sender As Object, e As EventArgs) Handles txtPesqDescricao1.TextChanged
        TimerdgvMateriaisProtheus.Enabled = True
    End Sub

    Private Sub txtPesqDescricao2_TextChanged(sender As Object, e As EventArgs) Handles txtPesqDescricao2.TextChanged
        TimerdgvMateriaisProtheus.Enabled = True
    End Sub

    Private Sub txtPesqDescricao3_TextChanged(sender As Object, e As EventArgs) Handles txtPesqDescricao3.TextChanged
        TimerdgvMateriaisProtheus.Enabled = True
    End Sub

    Private Sub dgvMateriaisProtheus_CellContentClick(sender As Object, e As Windows.Forms.DataGridViewCellEventArgs) Handles dgvMateriaisProtheus.CellContentClick

    End Sub

    Private Sub dgvMateriaisProtheus_Click(sender As Object, e As EventArgs) Handles dgvMateriaisProtheus.Click

        Try



            MaterialFichaTecnica.CodMatFabricante = dgvMateriaisProtheus.CurrentRow.Cells("b1_cod").Value.ToString()


        Catch ex As Exception

            MaterialFichaTecnica.CodMatFabricante = ""
        Finally



        End Try


    End Sub

    Private Sub SalvarMaterialSelecionadoNoSINCOToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SalvarMaterialSelecionadoNoSINCOToolStripMenuItem.Click

        Dim codmatfabricante As String
        Dim idmaterial As String



        If MaterialFichaTecnica.CodMatFabricante.ToString <> "" Then

            cl_BancoDados.RetornaCampoDaPesquisa("Select CodMatFabricante, idmaterial from material where CodMatFabricante ='" & MaterialFichaTecnica.CodMatFabricante & "'", "CodMatFabricante", "idmaterial")

            codmatfabricante = VCampo0
            idmaterial = VCampo1

            If codmatfabricante = "" Then

                classeMaterial.SalvarDadosMaterial(dgvMateriaisProtheus.CurrentRow.Cells("b1_desc").Value.ToString.ToUpper,
                                                   dgvMateriaisProtheus.CurrentRow.Cells("b1_desc").Value.ToString.ToUpper,
                                                        "0",
                                                        dgvMateriaisProtheus.CurrentRow.Cells("b1_um").Value.ToString.ToUpper,
                                                        dgvMateriaisProtheus.CurrentRow.Cells("b1_cod").Value.ToString.ToUpper,
                                                        "DIVERSOS",
                                                        "0",
                                                        "MATERIAL",
                                                        "0",
                                                        "0",
                                                       "0",
                                                        "0",
                                                        "0",
                                                        Nothing, "MATERIAL",
                                                        dgvMateriaisProtheus.CurrentRow.Cells("b1_cod").Value.ToString.ToUpper,
                                                        Usuario.NomeCompleto,
                                                        Date.Now.Date, "", "", "", "", "", "", "", "",   '   espessura
                                                           "",    ' materialsw
                                                              "",    '   autor
                                                              "")    '  palavrachave

            Else

                classeMaterial.UpdateDados(idmaterial, dgvMateriaisProtheus.CurrentRow.Cells("b1_desc").Value.ToString.ToUpper,
                                                   dgvMateriaisProtheus.CurrentRow.Cells("b1_desc").Value.ToString.ToUpper,
                                                        "0",
                                                        dgvMateriaisProtheus.CurrentRow.Cells("b1_um").Value.ToString.ToUpper,
                                                        dgvMateriaisProtheus.CurrentRow.Cells("b1_cod").Value.ToString.ToUpper,
                                                        "DIVERSOS",
                                                        "0",
                                                        "MATERIAL",
                                                        "0",
                                                        "0",
                                                       "0",
                                                        "0",
                                                        "0",
                                                        Nothing, "MATERIAL",
                                                        dgvMateriaisProtheus.CurrentRow.Cells("b1_cod").Value.ToString.ToUpper,
                                                        "", "", "", "", "", "", "")    '  palavrachave

            End If


            MsgBox("Itens registrado com sucesso", vbInformation, "Atenção!")

        End If

    End Sub

    Private Sub frmMaterialProtheus_Closed(sender As Object, e As EventArgs) Handles Me.Closed

        MaterialFichaTecnica.CodMatFabricante = 0

    End Sub

    Private Sub btnFechar_Click(sender As Object, e As EventArgs) Handles btnFechar.Click
        Me.Close()

    End Sub
End Class