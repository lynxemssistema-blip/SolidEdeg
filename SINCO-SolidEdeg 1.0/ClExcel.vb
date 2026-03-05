Imports System.Globalization
Imports System.IO
Imports System.Runtime.InteropServices
Imports System.Windows.Forms
Imports ClosedXML.Excel

Public Class ClExcel

    Public Function ExportarOrdemServicoPadrao(dgvGrid As DataGridView, ByVal BarraProgresso As ToolStripProgressBar, Endereco As String, ByVal DescricaoOs As String, ByVal dgvprincipal As DataGridView, ByVal dgvMaterial As DataGridView, Optional rp As Boolean = False) As Boolean

        Try

            BarraProgresso.Minimum = 0
            BarraProgresso.Value = 0
            BarraProgresso.Maximum = dgvGrid.RowCount

            '    Validar condições iniciais
            If dgvGrid Is Nothing OrElse dgvGrid.Rows.Count = 0 Then
                MsgBox("Não há itens a serem liberados para fabricação.", vbInformation, "Atenção")
                Return False
            End If

            If String.IsNullOrEmpty(My.Settings.EnderecoTemplateExcel) OrElse Not File.Exists(My.Settings.EnderecoTemplateExcel) Then
                MsgBox("A planilha template não foi encontrada. Por favor, configure o caminho corretamente.", vbCritical, "Erro")
                Return False
            End If

            Dim ObjetoExcel As Microsoft.Office.Interop.Excel.Application = Nothing
            Dim pasta1 As Microsoft.Office.Interop.Excel.Workbook = Nothing
            Dim pastaLantek As Microsoft.Office.Interop.Excel.Workbook = Nothing
            Dim planilha As Microsoft.Office.Interop.Excel.Worksheet = Nothing
            Dim planilhaMaterial As Microsoft.Office.Interop.Excel.Worksheet = Nothing

            Dim planilhaOmie As Microsoft.Office.Interop.Excel.Worksheet = Nothing


            Dim planilhaLantek As Microsoft.Office.Interop.Excel.Worksheet = Nothing



            'Inicializar o Excel
            ObjetoExcel = New Microsoft.Office.Interop.Excel.Application()
            pasta1 = ObjetoExcel.Workbooks.Open(My.Settings.EnderecoTemplateExcel)

            pastaLantek = ObjetoExcel.Workbooks.Open(My.Settings.PlanilhaModeloLantek)

            ' planilha = CType(pasta1.ActiveSheet, Microsoft.Office.Interop.Excel.Worksheet)

            planilha = CType(pasta1.Sheets("OrdemServico"), Microsoft.Office.Interop.Excel.Worksheet)
            planilhaMaterial = CType(pasta1.Sheets("Material"), Microsoft.Office.Interop.Excel.Worksheet)

            planilhaLantek = CType(pastaLantek.Sheets("Import XLS"), Microsoft.Office.Interop.Excel.Worksheet)


            If My.Settings.ProgramaRM = "#omie" Then

                planilhaOmie = CType(pasta1.Sheets("Omie_Produtos_Estrutura"), Microsoft.Office.Interop.Excel.Worksheet)

            End If

            '            planilha = pasta1.ActiveSheet

            Try
                Dim NovoIdOrdemServicoDB As Integer = Convert.ToInt32(dgvprincipal.CurrentRow.Cells("IdOrdemServico").Value.ToString)

                NovoIdOrdemServico = cl_BancoDados.FormatarPara5Caracteres(NovoIdOrdemServicoDB.ToString())
            Catch ex As Exception

                ' Em caso de erro, atribuir "00001" como valor inicial
                NovoIdOrdemServico = "00001" 'Nothing

            End Try

            NovoIdOrdemServico = cl_BancoDados.FormatarPara5Caracteres(NovoIdOrdemServico.ToString)

            'cabeçalho DA LISTA DE PEÇAS
            planilha.Range("W1").Value = NovoIdOrdemServico.ToString
            planilha.Range("D8").Value = dgvprincipal.CurrentRow.Cells("Projeto").Value.ToString & " - " & dgvprincipal.CurrentRow.Cells("DescEmpresa").Value.ToString
            planilha.Range("D9").Value = dgvprincipal.CurrentRow.Cells("Tag").Value.ToString.Trim.ToUpper
            planilha.Range("T8").Value = dgvprincipal.CurrentRow.Cells("Descricao").Value.ToString.Trim.ToUpper
            planilha.Range("D10").Value = dgvprincipal.CurrentRow.Cells("ENDERECO").Value.ToString.Trim.ToUpper

            planilha.Range("D13").Value = dgvprincipal.CurrentRow.Cells("CriadoPor").Value.ToString.Trim.ToUpper
            planilha.Range("D14").Value = dgvprincipal.CurrentRow.Cells("DataCriacao").Value.ToString.Trim.ToUpper

            'cabeçalho DA LISTA DE MATERIAIS
            planilhaMaterial.Range("P1").Value = NovoIdOrdemServico.ToString
            planilhaMaterial.Range("D8").Value = dgvprincipal.CurrentRow.Cells("Projeto").Value.ToString & " - " & dgvprincipal.CurrentRow.Cells("DescEmpresa").Value.ToString
            planilhaMaterial.Range("D9").Value = dgvprincipal.CurrentRow.Cells("Tag").Value.ToString.Trim.ToUpper
            planilhaMaterial.Range("M8").Value = dgvprincipal.CurrentRow.Cells("Descricao").Value.ToString.Trim.ToUpper
            planilhaMaterial.Range("D10").Value = dgvprincipal.CurrentRow.Cells("ENDERECO").Value.ToString.Trim.ToUpper

            planilhaMaterial.Range("D13").Value = dgvprincipal.CurrentRow.Cells("CriadoPor").Value.ToString.Trim.ToUpper
            planilhaMaterial.Range("D14").Value = dgvprincipal.CurrentRow.Cells("DataCriacao").Value.ToString.Trim.ToUpper

            planilhaLantek.Range("H2").Value = NovoIdOrdemServico.ToString
            planilhaLantek.Range("K2").Value = dgvprincipal.CurrentRow.Cells("DescEmpresa").Value.ToString & " - " &
                                               dgvprincipal.CurrentRow.Cells("Projeto").Value.ToString & " - " &
                                               dgvprincipal.CurrentRow.Cells("Tag").Value.ToString



            ' Função auxiliar para converter valores seguramente em números para o Excel (Double nativo)
            Dim ObterNumeroSeguro As Func(Of Object, Object) =
                Function(v)
                    If v Is Nothing OrElse IsDBNull(v) Then Return ""
                    Dim txt As String = v.ToString().Trim()
                    If txt = "" Then Return ""
                    Dim d As Double
                    If Double.TryParse(txt.Replace(".", ","), NumberStyles.Any, CultureInfo.GetCultureInfo("pt-BR"), d) Then
                        Return d
                    End If
                    Return txt.ToUpper()
                End Function

            'Percorre o grid procurando o item selecionado
            For I As Integer = 0 To dgvGrid.Rows.Count - 1

                Try

                    planilha.Range("A18:W18").Copy(planilha.Range("A" & 19 + I & ":W" & 19 + I))
                    planilha.Range("A" & I + 19).Value = dgvGrid.Rows(I).Cells("IDOrdemServicoItem").Value.ToString.Trim.ToUpper
                    planilha.Range("B" & I + 19).Value = dgvGrid.Rows(I).Cells("CodMatFabricante").Value.ToString.Trim.ToUpper
                    
                    ' 🔢 Envia como número (Double)
                    planilha.Range("I" & I + 19).Value = ObterNumeroSeguro(dgvGrid.Rows(I).Cells("QtdeTotal").Value)
                    
                    planilha.Range("J" & I + 19).Value = dgvGrid.Rows(I).Cells("MaterialSW").Value.ToString.Trim.ToUpper
                    planilha.Range("K" & I + 19).Value = dgvGrid.Rows(I).Cells("Unidade").Value.ToString.Trim.ToUpper
                    
                    ' 🔢 Envia dimensões como número (Double)
                    planilha.Range("L" & I + 19).Value = ObterNumeroSeguro(dgvGrid.Rows(I).Cells("Espessura").Value)
                    planilha.Range("M" & I + 19).Value = ObterNumeroSeguro(dgvGrid.Rows(I).Cells("Altura").Value)
                    planilha.Range("N" & I + 19).Value = ObterNumeroSeguro(dgvGrid.Rows(I).Cells("Largura").Value)
                    
                    planilha.Range("O" & I + 19).Value = dgvGrid.Rows(I).Cells("txtItemEstoque").Value.ToString.Trim.ToUpper
                    planilha.Range("P" & I + 19).Value = dgvGrid.Rows(I).Cells("DescResumo").Value.ToString.Trim.ToUpper
                    planilha.Range("S" & I + 19).Value = dgvGrid.Rows(I).Cells("DescDetal").Value.ToString.Trim.ToUpper
                    planilha.Range("V" & I + 19).Value = dgvGrid.Rows(I).Cells("Acabamento").Value.ToString.Trim.ToUpper
                    planilha.Range("W" & I + 19).Value = dgvGrid.Rows(I).Cells("txtTipoDesenho").Value.ToString.Trim.ToUpper

                    BarraProgresso.Value = I
                Catch ex As Exception

                    Continue For

                End Try

            Next

            planilha.Range("A18:W18").Delete()

            BarraProgresso.Minimum = 0
            BarraProgresso.Value = 0
            BarraProgresso.Maximum = dgvMaterial.RowCount

            ' If My.Settings.ProgramaRM = "#omie" Or My.Settings.BancoDadosAtivo = "lynxlocal" Then

            '   If rp = True Then
            'Percorre o grid procurando o item selecionado MATERIAL
            For I As Integer = 0 To dgvMaterial.Rows.Count - 1

                Try

                    planilhaMaterial.Range("A18:P18").Copy(planilhaMaterial.Range("A" & 19 + I & ":P" & 19 + I))
                    planilhaMaterial.Range("A" & I + 19).Value = dgvMaterial.Rows(I).Cells("CodMatFabricante").Value.ToString.Trim.ToUpper
                    planilhaMaterial.Range("D" & I + 19).Value = dgvMaterial.Rows(I).Cells("NumeroRP").Value.ToString.Trim.ToUpper
                    planilhaMaterial.Range("G" & I + 19).Value = dgvMaterial.Rows(I).Cells("DescResumo").Value.ToString.Trim.ToUpper & vbCrLf & dgvMaterial.Rows(I).Cells("DescDetal").Value.ToString.Trim.ToUpper
                    
                    ' 🔢 Envia como número (Double)
                    planilhaMaterial.Range("N" & I + 19).Value = ObterNumeroSeguro(dgvMaterial.Rows(I).Cells("QtdeTotal").Value)
                    
                    planilhaMaterial.Range("O" & I + 19).Value = dgvMaterial.Rows(I).Cells("Unidade").Value.ToString.Trim.ToUpper
                    
                    ' 🔢 Envia como peso numérico (Double)
                    planilhaMaterial.Range("P" & I + 19).Value = ObterNumeroSeguro(dgvMaterial.Rows(I).Cells("Peso").Value)

                    BarraProgresso.Value = I

                Catch ex As Exception

                msgbox(ex.message)

                    Continue For

                End Try

            Next



            ' End If


            Dim linhaCorte As Integer = 1
            'Percorre o grid procurando o item selecionado
            For I As Integer = 0 To dgvGrid.Rows.Count - 1

                If dgvGrid.Rows(I).Cells("MaterialSW").Value.ToString.Trim.ToUpper <> "" And
                dgvGrid.Rows(I).Cells("Espessura").Value.ToString.Trim.ToUpper <> "" And
                dgvGrid.Rows(I).Cells("txtTipoDesenho").Value.ToString.Trim.ToUpper = "CHAPARIA" Then

                    linhaCorte += 1

                    Dim NomeArquivoReal As String = dgvGrid.Rows(I).Cells("CodMatFabricante").Value.ToString.Trim.ToUpper

                    NomeArquivoReal = NomeArquivoReal.Replace(".par", "").Replace(".psm", "").Replace(".PSM", "").Replace(".PAR", "")

                    '  planilhaLantek.Range("A7:P7").Copy(planilha.Range("A" & 7 + I & ":P" & 7 + I))
                    planilhaLantek.Range("A" & linhaCorte + 6).Value = NomeArquivoReal.ToString
                    planilhaLantek.Range("B" & linhaCorte + 6).Value = ObterNumeroSeguro(dgvGrid.Rows(I).Cells("QtdeTotal").Value)
                    planilhaLantek.Range("F" & linhaCorte + 6).Value = "Metalfisa"
                    planilhaLantek.Range("H" & linhaCorte + 6).Value = "" ' dgvGrid.Rows(I).Cells("MaterialSW").Value.ToString.Trim.ToUpper
                    planilhaLantek.Range("I" & linhaCorte + 6).Value = 0 'dgvGrid.Rows(I).Cells("Espessura").Value.ToString.Trim.ToUpper.Replace(",", ".")

                    planilhaLantek.Range("K" & linhaCorte + 6).Value = dgvGrid.Rows(I).Cells("EnderecoArquivo").Value.ToString.Trim.ToUpper
                    planilhaLantek.Range("p" & linhaCorte + 6).Value = dgvGrid.Rows(I).Cells("NovoRevisao").Value.ToString.Trim.ToUpper

                    ' BarraProgresso.Value = I

                End If

            Next

            planilhaLantek.Range("A7:P7").Delete()

            planilhaMaterial.Range("A18:P18").Delete()

            Dim NomeArquivo As String = Endereco & "\OS_" & NovoIdOrdemServico & ".xlsx"
            Dim contador As Integer = 1


            ' Verifica se o arquivo já existe
            While System.IO.File.Exists(NomeArquivo)
                ' Se o arquivo já existir, adiciona um sufixo numérico
                NomeArquivo = Endereco & "\OS_" & NovoIdOrdemServico & "_" & contador & ".xlsx"
                contador += 1
            End While

            pasta1.SaveCopyAs(NomeArquivo)
            pasta1.Close(False)




            Dim NomeArquivoLantek As String = NomeArquivo.Replace(".xlsx", "") & " - Espelho OS-Corte.xls"
            Dim contadorLantek As Integer = 1

            ' Verifica se o arquivo já existe
            While System.IO.File.Exists(NomeArquivoLantek)
                ' Se o arquivo já existir, adiciona um sufixo numérico
                NomeArquivoLantek = NomeArquivo.Replace(".xlsx", "") & " - Espelho OS-Corte-" & contadorLantek & ".xls"
                contadorLantek += 1

            End While

            pastaLantek.SaveCopyAs(NomeArquivoLantek)
            pastaLantek.Close(False)

            ObjetoExcel.Application.Visible = False


            Process.Start("Explorer", Endereco.ToString)
            BarraProgresso.Value = 0

            MsgBox("Dados exportados com Sucesso!!!", vbInformation, "Atenção!!!!")

        Catch ex As Exception

        Finally

        End Try

    End Function


End Class

Public Class clPadraoMetta


    Public Function ExportarOrdemServicoPadraoMetta(
    ByVal dgvGrid As DataGridView,
    ByVal BarraProgresso As ProgressBar,
    ByVal Endereco As String,
    ByVal DescricaoOs As String,
    ByVal dgvPrincipal As DataGridView,
    ByVal TipoExcel As Boolean,
    ByVal dgvMaterial As DataGridView
) As Boolean

        ' Validar condições iniciais
        If Not ValidarEntrada(dgvGrid, dgvMaterial) Then
            Return False
        End If

        Dim ObjetoExcel As Microsoft.Office.Interop.Excel.Application = Nothing
        Dim pasta1 As Microsoft.Office.Interop.Excel.Workbook = Nothing
        Dim planilha As Microsoft.Office.Interop.Excel.Worksheet = Nothing

        Try
            ' Inicializar Excel e abrir template
            ObjetoExcel = New Microsoft.Office.Interop.Excel.Application()
            pasta1 = ObjetoExcel.Workbooks.Open(My.Settings.EnderecoTemplateExcel)
            planilha = CType(pasta1.ActiveSheet, Microsoft.Office.Interop.Excel.Worksheet)

            ' Configurar barra de progresso
            ConfigurarBarraProgresso(BarraProgresso, dgvGrid.Rows.Count, dgvMaterial.Rows.Count)

            ' Obter ID da Ordem de Serviço
            Dim NovoIdOrdemServico As String = ObterIdOrdemServico(dgvGrid)

            ' Preencher cabeçalho
            PreencherCabecalho(planilha, dgvPrincipal, NovoIdOrdemServico, TipoExcel)

            ' Preencher dados do Grid
            PreencherGrid(planilha, dgvGrid, 23, BarraProgresso)

            ' Preencher dados de material
            Dim linhaInicialMaterial = dgvGrid.Rows.Count + 23
            PreencherGrid(planilha, dgvMaterial, linhaInicialMaterial, BarraProgresso, "material")

            ' Remover linha modelo
            planilha.Range("A22:U22").Delete()

            ' Salvar planilha
            Dim caminhoArquivo = Path.Combine(Endereco, $"OS_{NovoIdOrdemServico}.xlsx")
            pasta1.SaveCopyAs(caminhoArquivo)
            pasta1.Close(False)

            ' Abrir pasta destino
            Process.Start("Explorer", Endereco)

            ' Resetar progresso e mostrar mensagem de sucesso
            BarraProgresso.Value = 0
            MsgBox("Dados exportados com sucesso!", vbInformation, "Sucesso")

            Return True
        Catch ex As Exception
            MsgBox("Erro durante a exportação: " & ex.Message, vbCritical, "Erro")
            Return False
        Finally
            ' Liberar recursos COM
            If pasta1 IsNot Nothing Then Marshal.ReleaseComObject(pasta1)
            If planilha IsNot Nothing Then Marshal.ReleaseComObject(planilha)
            If ObjetoExcel IsNot Nothing Then
                ObjetoExcel.Quit()
                Marshal.ReleaseComObject(ObjetoExcel)
            End If

            GC.Collect()
            GC.WaitForPendingFinalizers()
        End Try
    End Function

    ' Método para validar entrada
    Private Function ValidarEntrada(dgvGrid As DataGridView, dgvMaterial As DataGridView) As Boolean
        If dgvGrid Is Nothing OrElse dgvGrid.Rows.Count = 0 Then
            MsgBox("Não há itens a serem exportados.", vbInformation, "Atenção")
            Return False
        End If
        If String.IsNullOrEmpty(My.Settings.EnderecoTemplateExcel) OrElse Not File.Exists(My.Settings.EnderecoTemplateExcel) Then
            MsgBox("Template Excel não encontrado. Verifique as configurações.", vbCritical, "Erro")
            Return False
        End If
        Return True
    End Function

    ' Método para configurar a barra de progresso
    Private Sub ConfigurarBarraProgresso(BarraProgresso As ProgressBar, totalGrid As Integer, totalMaterial As Integer)
        BarraProgresso.Minimum = 0
        BarraProgresso.Value = 0
        BarraProgresso.Maximum = totalGrid + totalMaterial - 1
    End Sub

    ' Método para obter o ID da Ordem de Serviço
    Private Function ObterIdOrdemServico(dgvGrid As DataGridView) As String
        Try
            Dim id As Integer = Convert.ToInt32(dgvGrid.CurrentRow.Cells("IdOrdemServico").Value)
            Return cl_BancoDados.FormatarPara5Caracteres(id.ToString())
        Catch
            Return cl_BancoDados.FormatarPara5Caracteres("1")
        End Try
    End Function

    ' Método para preencher cabeçalho
    Private Sub PreencherCabecalho(
    planilha As Microsoft.Office.Interop.Excel.Worksheet,
    dgvPrincipal As DataGridView,
    ordemServico As String,
    TipoExcel As Boolean
)
        planilha.Range("R2").Value = ordemServico
        planilha.Range("D10").Value = $"{dgvPrincipal.CurrentRow.Cells("Projeto").Value} - {dgvPrincipal.CurrentRow.Cells("DescEmpresa").Value}"
        planilha.Range("D11").Value = dgvPrincipal.CurrentRow.Cells("Tag").Value
        planilha.Range("N10").Value = dgvPrincipal.CurrentRow.Cells("Descricao").Value
        planilha.Range("D12").Value = dgvPrincipal.CurrentRow.Cells("ENDERECO").Value
        planilha.Range("D16").Value = dgvPrincipal.CurrentRow.Cells("USUARIO").Value
        planilha.Range("D17").Value = dgvPrincipal.CurrentRow.Cells("Data").Value
        If TipoExcel Then
            planilha.Range("N16").Value = dgvPrincipal.CurrentRow.Cells("Data_Liberacao_Engenharia").Value
        End If
        planilha.Range("Q16").Value = My.Computer.Name.ToUpper()
        planilha.Range("Q17").Value = Date.Now.ToShortDateString
    End Sub

    ' Método para preencher dados do grid
    Private Sub PreencherGrid(
    planilha As Microsoft.Office.Interop.Excel.Worksheet,
    dgv As DataGridView,
    linhaInicial As Integer,
    BarraProgresso As ProgressBar,
    Optional tipoMaterial As String = ""
)
        For i As Integer = 0 To dgv.Rows.Count - 1
            Dim linhaAtual = linhaInicial + i
            Try
                planilha.Range("A22:U22").Copy(planilha.Range("A" & linhaAtual & ":U" & linhaAtual))

                Dim row = dgv.Rows(i)
                planilha.Range("A" & linhaAtual).Value = row.Cells("IDOrdemServicoItem").Value
                planilha.Range("B" & linhaAtual).Value = row.Cells("DescResumo").Value
                planilha.Range("I" & linhaAtual).Value = row.Cells("DescDetal").Value
                planilha.Range("L" & linhaAtual).Value = row.Cells("CodMatFabricante").Value

                ' 🔢 Parse Decimal Seguro para a Quantidade Total
                Dim objQtde As Object = row.Cells("QtdeTotal").Value
                If objQtde IsNot Nothing Then
                    Dim qtdeTexto As String = objQtde.ToString().Trim()
                    If qtdeTexto <> "" Then
                        Dim valorQtde As Double
                        If Double.TryParse(qtdeTexto, NumberStyles.Any, CultureInfo.InvariantCulture, valorQtde) Then
                            planilha.Range("N" & linhaAtual).Value = valorQtde
                        Else
                            planilha.Range("N" & linhaAtual).Value = qtdeTexto
                        End If
                    End If
                End If

                planilha.Range("P" & linhaAtual).Value = If(String.IsNullOrEmpty(tipoMaterial), row.Cells("Unidade").Value, tipoMaterial)
                BarraProgresso.Value += 1
            Catch
                Continue For
            End Try
        Next
    End Sub

End Class