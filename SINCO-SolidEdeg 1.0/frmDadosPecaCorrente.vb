Imports System.Data
Imports System.Drawing
Imports System.Globalization
Imports System.IO
Imports System.Linq
Imports System.Runtime.InteropServices
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Threading
Imports System.Windows.Forms
Imports iText.IO.Font.Constants
Imports iText.Kernel.Font
Imports iText.Kernel.Geom
Imports iText.Kernel.Pdf
Imports iText.Kernel.Pdf.Canvas
Imports Microsoft.Extensions.Logging
Imports MySql.Data.MySqlClient
Imports Mysqlx.Datatypes
Imports SolidEdgeAssembly
Imports SolidEdgeFramework
Imports SolidEdgeGeometry
'Imports SolidEdgeFramework
Imports SolidEdgePart
Imports UglyToad.PdfPig.Graphics.Operations.SpecialGraphicsState
Imports Environment = System.Environment
Imports Font = System.Drawing.Font
Imports Path = System.IO.Path
Imports Thread = System.Threading.Thread



Public Class frmDadosPecaCorrente



#Region "🔧 VARIÁVEIS GLOBAIS"

    Private WithEvents timerAtualizacao As New System.Windows.Forms.Timer()



    ' Controle de edição pendente
    Private editando As Boolean = False
    Private nomePropriedadeEditando As String = ""
    Private valorEditado As String = ""

    Private Salvar As Boolean = False


    Private dtproc As New System.Data.DataTable()





    ' Variáveis de controle para o drag & drop
    Private dragIndex As Integer
    Private dragRow As DataGridViewRow
    Private dropIndex As Integer



#End Region


    Private Sub btnDxf_Click(sender As Object, e As EventArgs) Handles btnDxf.Click


        '  DxfArquivoCorrente()

        'ExportarPlanificacaoDXF_Final()

        '  ExportarDXFPlanificadoAuto()

        ExportarDXFPlanificadoAuto(0)



    End Sub

    Private Const CAMINHO_ALVO As String =
    "\\192.168.1.253\Engenharia\desenhos\ACESS DO  SOLID\PERFIS SOLID EDGE"

    Private Sub dgvDadosPecas_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs) Handles dgvDadosPecas.CellFormatting
        If e.RowIndex >= 0 AndAlso dgvDadosPecas.Columns.Contains("CaminhoCompleto") Then
            Dim caminhoObj = dgvDadosPecas.Rows(e.RowIndex).Cells("CaminhoCompleto").Value
            Dim caminho As String = If(caminhoObj IsNot Nothing, caminhoObj.ToString(), "")

            If caminho.IndexOf(CAMINHO_ALVO, StringComparison.OrdinalIgnoreCase) >= 0 Then
                e.CellStyle.BackColor = Color.Pink
                e.CellStyle.ForeColor = Color.Black
            End If
        End If
    End Sub

    Private Sub frmDadosPecaCorrente_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Try

            ' seApp = Marshal.GetActiveObject("SolidEdge.Application")

            dgvDadosPecas.Columns("dgvdxf").Visible = False
            dgvDadosPecas.Columns("dgvpdf").Visible = False
            DadosArquivoCorrente.soldagem = "NÃO"
            DadosArquivoCorrente.ItemEstoque = "NÃO"

        Catch ex As Exception
        Finally
        End Try



        Try

            AtualziarDadosSinco()

            CarregarPropriedadesArquivoCorrente()
            '  VerificaCadastroArquivo()


        Catch ex As Exception
        Finally
        End Try

        ' ListarTodasPropriedadesDoArquivo(TextBox1)



    End Sub




    Public Function VerificaCadastroArquivo()

        'Verifica se o arquivo existe no banco da dados
        cl_BancoDados.RetornaCampoDaPesquisa("Select CodMatFabricante, IdMaterial,txtTipoDesenho, Acabamento from material 
    where CodMatFabricante = '" & DadosArquivoCorrente.NomeArquivoSemExtensao & "'",
                                             "CodMatFabricante", 'VCampo0
                                             "IdMaterial", 'VCampo1
"txtTipoDesenho", 'VCampo2
"Acabamento") 'VCampo2

        Try
            DadosArquivoCorrente.IdMaterial = VCampo1
        Catch ex As Exception
            DadosArquivoCorrente.IdMaterial = 0
        End Try



        If VCampo0 <> "" Then

            ' Me.chkBoxAcabamento.Enabled = True
            ' Me.chkBoxTipoDesenho.Enabled = True
            Me.dgvProcessos.Enabled = True

            DadosArquivoCorrente.IdMaterial = VCampo1
            DadosArquivoCorrente.TipoDesenho = VCampo2
            DadosArquivoCorrente.Acabamento = VCampo3

            gpbMateriaisProtheus.Enabled = True

            gpbMateriaiDesenho.Enabled = True

            dgvGabaritos.Enabled = True
            TimerdgvGabaritos.Enabled = True


        Else

            ' Me.chkBoxAcabamento.Enabled = False
            ' Me.chkBoxTipoDesenho.Enabled = False
            Me.dgvProcessos.Enabled = False
            '  TimerdgvProcessoMaterial.Enabled = True

            gpbMateriaisProtheus.Enabled = False

            gpbMateriaiDesenho.Enabled = False


            TimerdgvGabaritos.Enabled = True
            dgvGabaritos.Enabled = True




            DadosArquivoCorrente.IdMaterial = 0
            DadosArquivoCorrente.TipoDesenho = ""
            DadosArquivoCorrente.Acabamento = ""

            Me.txtPalavraChave.Clear()

        End If

        TimerdgvProcessoMaterial.Enabled = True

        TimerManufaturada.Enabled = True

    End Function



    Private Sub SelecionarItemCheckedListBox(chk As CheckedListBox, valorReferencia As String)


        Try
            Dim valorBusca As String = valorReferencia?.Trim().ToUpper()

            ' 🔹 Se o valor está vazio, desmarca tudo e sai imediatamente
            If String.IsNullOrEmpty(valorBusca) Then
                chk.BeginUpdate()
                For i As Integer = 0 To chk.Items.Count - 1
                    chk.SetItemChecked(i, False)
                Next
                chk.EndUpdate()
                Exit Sub
            End If

            ' 🔹 Percorre e marca/desmarca em um único loop
            chk.BeginUpdate()
            Dim temMarcado As Boolean = False

            For i As Integer = 0 To chk.Items.Count - 1
                Dim isMatch As Boolean = (chk.Items(i).ToString().Trim().ToUpper() = valorBusca)
                chk.SetItemChecked(i, isMatch)
                If isMatch Then temMarcado = True
            Next

            chk.EndUpdate()

            ' 🔹 Garante que, se nada for marcado, todos fiquem desmarcados
            If Not temMarcado Then
                chk.BeginUpdate()
                For i As Integer = 0 To chk.Items.Count - 1
                    chk.SetItemChecked(i, False)
                Next
                chk.EndUpdate()
            End If

        Catch ex As Exception
            Debug.WriteLine("Erro ao selecionar item: " & ex.Message)
        End Try

    End Sub

    Private Sub AtualziarDadosSinco()

        ' --- medição opcional de desempenho ---
        Dim sw As Stopwatch = Stopwatch.StartNew()

        ' Guarda UI
        Cursor.Current = Cursors.WaitCursor
        Me.Enabled = False

        Try
            Dim prefixo As String = If(ComplementoTipoBanco, String.Empty)

            '  ElseIf tipo = "MYSQL" Then
            ' Projeto

            ' Teste banco da dos Protheus - 09-12-2025
            cl_BancoDados.SafeComboBoxFill(
             Sub()
                 cl_BancoDados.ComboBoxDataSet("projetos", "idProjeto", "Projeto", cboProjeto, " WHERE (D_E_L_E_T_E Is NULL Or D_E_L_E_T_E = '') AND (Finalizado = '' OR Finalizado Is NULL) AND (Liberado = 'S')")
             End Sub, "cboProjeto (MySQL)", Me)


            '  Protheus.CarregarPedidosNoCombo(cboProjeto, Me)



            ' Acabamento
            cl_BancoDados.SafeComboBoxFill(
             Sub()
                 cl_BancoDados.ComboBoxDataSet("acabamento", "IdAcabamento", "DescAcabamento", cboOpcoesAcabamento, "WHERE (D_E_L_E_T_E IS NULL OR D_E_L_E_T_E = '')")
             End Sub, "cboOpcoesAcabamento (MySQL)", Me)

            ' Acabamento aplicado no desenho
            cl_BancoDados.SafeComboBoxFill(
             Sub()
                 cl_BancoDados.ComboBoxDataSet("acabamento", "IdAcabamento", "DescAcabamento", cboAcabamento, "WHERE (D_E_L_E_T_E IS NULL OR D_E_L_E_T_E = '')")
             End Sub, "cboAcabamento (MySQL)", Me)


            ' tipo de desenho aplicado no desenho
            cl_BancoDados.SafeComboBoxFill(
             Sub()
                 cl_BancoDados.ComboBoxDataSet("familia", "Idfamilia", "DescFamilia", cboTipoDesenho, "WHERE (D_E_L_E_T_E IS NULL OR D_E_L_E_T_E = '')")
             End Sub, "cboTipoDesenho (MySQL)", Me)

            ' 5) Versão na StatusStrip (sem estourar em ambientes sem AssemblyInfo)
            Dim version As Version = Nothing
            Try
                version = Reflection.Assembly.GetExecutingAssembly().GetName().Version
            Catch
                ' ignora
            End Try
            Dim versaoTxt As String = If(version IsNot Nothing, version.ToString(), "v?")

            TimerdgvProcesso.Enabled = True

            TimerdgvMateriaisProtheus.Enabled = True

            dgvProcessoMaterial.AllowDrop = True
            dgvProcessoMaterial.ReadOnly = True
            dgvProcessoMaterial.SelectionMode = DataGridViewSelectionMode.FullRowSelect
            dgvProcessoMaterial.MultiSelect = False
            dgvProcessoMaterial.AllowUserToAddRows = False
            dgvProcessoMaterial.AllowUserToDeleteRows = False

        Catch ex As Exception
            ' Log detalhado + mensagem amigável
            Debug.WriteLine($"AtualziarDadosSinco: {ex}")
            MessageBox.Show("Falha ao atualizar dados do SINCO: " & ex.Message, "Atualização de dados", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            Me.Enabled = True
            Cursor.Current = Cursors.Default
            sw.Stop()
            Debug.WriteLine($"AtualziarDadosSinco concluído em {sw.ElapsedMilliseconds} ms")
        End Try

        Timerdgvos.Enabled = True

    End Sub


    Private Sub Button1_Click()
        DadosDesenhoCorrente()

    End Sub

    Private Sub Button2_Click()

        PlanificarDesenhoCorrente()

    End Sub

    Private Sub btnGerarPdf_Click(sender As Object, e As EventArgs) Handles btnGerarPdf.Click
        '  ExportarPDFDetalhamento()
        ExportarPDFDetalhamentoLote(0)

    End Sub

    Private Sub btnListaConjunto_Click(sender As Object, e As EventArgs) Handles btnListaConjunto.Click

        TabelaViewMontaPeca = cl_BancoDados.CarregarDados("SELECT * FROM  " & ComplementoTipoBanco & "viewmontapeca where D_E_L_E_T_E = '' OR D_E_L_E_T_E IS NULL")


        ' ModListarComponentesMontagem.ListarComponentesMontagem(Me.dgvComponentes)
        '  ModListarComponentesMontagem.ListarBOMPropriedadesRecursiva(Me.DataGridView1)
        ' ModListarComponentesMontagem.ListarEstruturaComTodasPropriedades(DataGridView1)
        'ModListarComponentesMontagem.ListarEstruturaComTodasPropriedadesBlank(dgvDadosPecas, ProgressBar1, lblProgresso, lblResumo)
        '  ModListarComponentesMontagem.ListarEstruturaBom(dgvDadosPecas, ProgressBar1, lblProgresso, lblResumo)
        '
        ModListarComponentesMontagem.ListarEstruturaBomMateriais(dgvDadosPecas, ProgressBar1, lblProgresso, lblResumo)


        Me.lblProgresso.Text = "0%"
    End Sub

    Private Sub btnCriarEstruturaProduto_Click(sender As Object, e As EventArgs) Handles btnCriarEstruturaProduto.Click
        ' Parar timers para evitar que fechem a conexão no meio do processo (race condition)
        timerAtualizacao.Stop()
        TimerDGVListaMaterialSW.Stop()
        Timerdgvos.Stop()
        TimerdgvProcessoMaterial.Stop()
        TimerdgvProcesso.Stop()
        TimerdgvMateriaisProtheus.Stop()
        TimerManufaturada.Stop()
        TimerdgvGabaritos.Stop()

        Try
            If dgvDadosPecas.Rows.Count = 0 Then
                MessageBox.Show("A grade de peças/conjuntos está vazia. Carregue a estrutura primeiro.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Exit Sub
            End If

            Dim codProdutoPai As String = ""
            Dim temItemNaoCadastrado As Boolean = False

            cl_BancoDados.AbrirBanco()

            For i As Integer = 0 To dgvDadosPecas.Rows.Count - 1
                Dim nivelStr As String = ""
                If dgvDadosPecas.Rows(i).Cells("Nivel").Value IsNot Nothing Then nivelStr = dgvDadosPecas.Rows(i).Cells("Nivel").Value.ToString()
                Dim tipoLinha As String = ""
                If dgvDadosPecas.Rows(i).Cells("TipoLinha").Value IsNot Nothing Then tipoLinha = dgvDadosPecas.Rows(i).Cells("TipoLinha").Value.ToString().ToUpper()

                If nivelStr = "0" AndAlso tipoLinha = "PECA" AndAlso codProdutoPai = "" Then
                    If dgvDadosPecas.Rows(i).Cells("Arquivo").Value IsNot Nothing Then codProdutoPai = dgvDadosPecas.Rows(i).Cells("Arquivo").Value.ToString()
                End If

                If tipoLinha <> "MATERIAL" Then
                    Dim fileNameWithExtension As String = ""
                    If dgvDadosPecas.Rows(i).Cells("Arquivo").Value IsNot Nothing Then fileNameWithExtension = dgvDadosPecas.Rows(i).Cells("Arquivo").Value.ToString()

                    Dim checkCadastrado As String = ""
                    Try
                        ' Nota: RetornaCampoDaPesquisa abre e fecha a conexão internamente.
                        cl_BancoDados.RetornaCampoDaPesquisa("SELECT CodMatFabricante FROM " & ComplementoTipoBanco & "material WHERE CodMatFabricante = '" & fileNameWithExtension & "' AND (D_E_L_E_T_E = '' OR D_E_L_E_T_E IS NULL)", "CodMatFabricante")
                        checkCadastrado = VCampo0

                    Catch ex As Exception
                    End Try

                    If String.IsNullOrWhiteSpace(checkCadastrado) Then
                        temItemNaoCadastrado = True
                        If dgvDadosPecas.Columns.Contains("Arquivo") Then dgvDadosPecas.Rows(i).Cells("Arquivo").Style.BackColor = Color.LightPink
                        If dgvDadosPecas.Columns.Contains("CodMatFabricante") Then dgvDadosPecas.Rows(i).Cells("CodMatFabricante").Style.BackColor = Color.LightPink
                        If dgvDadosPecas.Columns.Contains("Número do documento") Then dgvDadosPecas.Rows(i).Cells("Número do documento").Style.BackColor = Color.LightPink
                    Else
                        If dgvDadosPecas.Columns.Contains("Arquivo") Then dgvDadosPecas.Rows(i).Cells("Arquivo").Style.BackColor = Color.Empty
                        If dgvDadosPecas.Columns.Contains("CodMatFabricante") Then dgvDadosPecas.Rows(i).Cells("CodMatFabricante").Style.BackColor = Color.Empty
                        If dgvDadosPecas.Columns.Contains("Número do documento") Then dgvDadosPecas.Rows(i).Cells("Número do documento").Style.BackColor = Color.Empty
                    End If
                End If
            Next

            If String.IsNullOrWhiteSpace(codProdutoPai) Then
                MessageBox.Show("Não foi possível identificar o item Pai de Nível 0. Verifique se a estrutura está montada corretamente no grid.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Exit Sub
            End If

            If temItemNaoCadastrado Then
                MessageBox.Show("Um ou mais itens do conjunto não estão devidamente cadastrados no sistema (sinalizados em rosa claro)." & vbCrLf & "Por favor, selecione e cadastre-os antes de gerar a estrutura padrão.", "Itens Não Cadastrados", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Exit Sub
            End If

            If MessageBox.Show("Deseja gravar a estrutura padrão do produto [" & codProdutoPai & "] no banco de dados?", "Confirmação", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.No Then
                Exit Sub
            End If

            ProgressBar1.Visible = True
            ProgressBar1.Value = 0
            ProgressBar1.Maximum = dgvDadosPecas.Rows.Count
            lblProgresso.Visible = True

            ' Garante que a conexão esteja aberta para o bloco de execução
            cl_BancoDados.AbrirBanco()

            Dim createTableQuery As String = "CREATE TABLE IF NOT EXISTS `" & ComplementoTipoBanco & "produto_estrutura_item` (" &
                  "`IdProdutoEstruturaItem` INT NOT NULL AUTO_INCREMENT," &
                  "`CodProdutoPai` VARCHAR(255) NULL COMMENT 'Codigo do material pai/produto (Nivel 0)'," &
                  "`nivel` INT NULL," &
                  "`Estatus_OrdemServico` VARCHAR(50) DEFAULT NULL," &
                  "`DescResumo` VARCHAR(250) DEFAULT NULL," &
                  "`DescDetal` VARCHAR(250) DEFAULT NULL," &
                  "`Autor` VARCHAR(150) DEFAULT NULL," &
                  "`PalavraChave` VARCHAR(150) DEFAULT NULL," &
                  "`Notas` TEXT," &
                  "`Espessura` VARCHAR(50) DEFAULT NULL," &
                  "`AreaPintura` DECIMAL(18,6) DEFAULT NULL," &
                  "`NumeroDobras` INT DEFAULT NULL," &
                  "`Peso` DECIMAL(18,6) DEFAULT NULL," &
                  "`Unidade` VARCHAR(10) DEFAULT NULL," &
                  "`UnidadeSW` VARCHAR(10) DEFAULT NULL," &
                  "`Altura` VARCHAR(50) DEFAULT NULL," &
                  "`Largura` VARCHAR(50) DEFAULT NULL," &
                  "`CodMatFabricante` VARCHAR(250) DEFAULT NULL," &
                  "`DtCad` DATETIME DEFAULT NULL," &
                  "`UsuarioCriacao` VARCHAR(100) DEFAULT NULL," &
                  "`UsuarioAlteracao` VARCHAR(100) DEFAULT NULL," &
                  "`DtAlteracao` DATETIME DEFAULT NULL," &
                  "`EnderecoArquivo` VARCHAR(500) DEFAULT NULL," &
                  "`MaterialSW` VARCHAR(150) DEFAULT NULL," &
                  "`QtdeTotal` DECIMAL(18,6) DEFAULT NULL," &
                  "`CriadoPor` VARCHAR(150) DEFAULT NULL," &
                  "`DataCriacao` DATETIME DEFAULT NULL," &
                  "`Estatus` VARCHAR(50) DEFAULT NULL," &
                  "`Acabamento` VARCHAR(150) DEFAULT NULL," &
                  "`D_E_L_E_T_E` VARCHAR(1) DEFAULT NULL," &
                  "`Fator` DECIMAL(18,6) DEFAULT NULL," &
                  "`Qtde` DECIMAL(18,6) DEFAULT NULL," &
                  "`txtSoldagem` VARCHAR(50) DEFAULT NULL," &
                  "`txtTipoDesenho` VARCHAR(50) DEFAULT NULL," &
                  "`txtCorte` VARCHAR(50) DEFAULT NULL," &
                  "`txtDobra` VARCHAR(50) DEFAULT NULL," &
                  "`txtSolda` VARCHAR(50) DEFAULT NULL," &
                  "`txtPintura` VARCHAR(50) DEFAULT NULL," &
                  "`txtMontagem` VARCHAR(50) DEFAULT NULL," &
                  "`ComprimentoCaixaDelimitadora` VARCHAR(50) DEFAULT NULL," &
                  "`LarguraCaixaDelimitadora` VARCHAR(50) DEFAULT NULL," &
                  "`EspessuraCaixaDelimitadora` VARCHAR(50) DEFAULT NULL," &
                  "`AreaPinturaUnitario` DECIMAL(18,6) DEFAULT NULL," &
                  "`PesoUnitario` DECIMAL(18,6) DEFAULT NULL," &
                  "`txtItemEstoque` VARCHAR(50) DEFAULT NULL," &
                  "`DataPrevisao` DATE DEFAULT NULL," &
                  "`sttxtCorte` VARCHAR(50) DEFAULT NULL," &
                  "`sttxtDobra` VARCHAR(50) DEFAULT NULL," &
                  "`sttxtSolda` VARCHAR(50) DEFAULT NULL," &
                  "`sttxtPintura` VARCHAR(50) DEFAULT NULL," &
                  "`sttxtMontagem` VARCHAR(50) DEFAULT NULL," &
                  "`descempresa` VARCHAR(150) DEFAULT NULL," &
                  "`IdEmpresa` INT DEFAULT NULL," &
                  "`DescTag` VARCHAR(150) DEFAULT NULL," &
                  "PRIMARY KEY (`IdProdutoEstruturaItem`)" &
                  ") ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;"

            Using cmdCreate As New MySqlCommand(createTableQuery, myconect)
                cmdCreate.ExecuteNonQuery()
            End Using

            ' OPCIONAL: Limpa a estrutura existente desse produto para não duplicar, caso o usuário gere 2 vezes.
            Dim deleteQuery As String = "DELETE FROM `" & ComplementoTipoBanco & "produto_estrutura_item` WHERE CodProdutoPai = @CodPai"
            Using cmdDel As New MySqlCommand(deleteQuery, myconect)
                cmdDel.Parameters.AddWithValue("@CodPai", codProdutoPai)
                cmdDel.ExecuteNonQuery()
            End Using

            ' Vamos iterar a BOM e gravar na nova tabela
            For b As Integer = 0 To dgvDadosPecas.Rows.Count - 1
                ProgressBar1.Value = b
                lblProgresso.Text = "Gravando estrutura: " & b + 1 & " de " & dgvDadosPecas.Rows.Count

                Dim tipoLinha As String = ""
                If dgvDadosPecas.Rows(b).Cells("TipoLinha").Value IsNot Nothing Then tipoLinha = dgvDadosPecas.Rows(b).Cells("TipoLinha").Value.ToString().ToUpper()

                ' ====================================================================================
                ' REGRA NEGÓCIO: Ignorar insumos/matérias-primas e focar na árvore da engenharia
                ' ====================================================================================
                If tipoLinha = "MATERIAL" Then Continue For

                Dim nivel As Integer = 0
                If dgvDadosPecas.Rows(b).Cells("Nivel").Value IsNot Nothing Then Integer.TryParse(dgvDadosPecas.Rows(b).Cells("Nivel").Value.ToString(), nivel)

                Dim arquivo As String = ""
                If dgvDadosPecas.Rows(b).Cells("Arquivo").Value IsNot Nothing Then arquivo = dgvDadosPecas.Rows(b).Cells("Arquivo").Value.ToString()
                Dim codMatFab As String = arquivo

                ' Faz o insert. Montado num padrão idêntico ao que é gerado ao salvar uma 'ordemservicoitem'.
                Dim query As String = "INSERT INTO `" & ComplementoTipoBanco & "produto_estrutura_item` (" &
                        " CodProdutoPai, nivel, DescResumo, DescDetal, Autor, Espessura, " &
                        " Peso, Unidade, CodMatFabricante, EnderecoArquivo, MaterialSW, QtdeTotal, " &
                        " Qtde, txtTipoDesenho, AreaPinturaUnitario, DataCriacao, CriadoPor " &
                        ") VALUES (" &
                        " @CodProdutoPai, @Nivel, @DescResumo, @DescDetal, @Autor, @Espessura, " &
                        " @Peso, @Unidade, @CodMatFabricante, @EnderecoArquivo, @MaterialSW, @QtdeTotal, " &
                        " @Qtde, @txtTipoDesenho, @AreaPinturaUnitario, @DataCriacao, @CriadoPor)"

                ' Garante que a conexão ainda está aberta (devido aos timers estarem parados, deve estar)
                If myconect.State <> ConnectionState.Open Then cl_BancoDados.AbrirBanco()

                Using command As New MySqlCommand(query, myconect)
                    command.Parameters.Clear() ' Segurança contra reuso de Command interno
                    command.Parameters.AddWithValue("@CodProdutoPai", codProdutoPai)
                    command.Parameters.AddWithValue("@Nivel", nivel)

                    Dim descResumo As String = ""
                    If dgvDadosPecas.Columns.Contains("Título") AndAlso dgvDadosPecas.Rows(b).Cells("Título").Value IsNot Nothing Then descResumo = Convert.ToString(dgvDadosPecas.Rows(b).Cells("Título").Value)
                    command.Parameters.AddWithValue("@DescResumo", descResumo)

                    Dim descDetal As String = ""
                    If dgvDadosPecas.Columns.Contains("Assunto") AndAlso dgvDadosPecas.Rows(b).Cells("Assunto").Value IsNot Nothing Then descDetal = Convert.ToString(dgvDadosPecas.Rows(b).Cells("Assunto").Value)
                    command.Parameters.AddWithValue("@DescDetal", descDetal)

                    Dim autor As String = ""
                    If dgvDadosPecas.Columns.Contains("Autor") AndAlso dgvDadosPecas.Rows(b).Cells("Autor").Value IsNot Nothing Then autor = Convert.ToString(dgvDadosPecas.Rows(b).Cells("Autor").Value)
                    command.Parameters.AddWithValue("@Autor", autor)

                    Dim espessura As String = ""
                    If dgvDadosPecas.Columns.Contains("Thickness") AndAlso dgvDadosPecas.Rows(b).Cells("Thickness").Value IsNot Nothing Then espessura = Convert.ToString(dgvDadosPecas.Rows(b).Cells("Thickness").Value)
                    command.Parameters.AddWithValue("@Espessura", espessura)

                    Dim pesoTot As Double = 0
                    If dgvDadosPecas.Columns.Contains("PesoTotal") AndAlso dgvDadosPecas.Rows(b).Cells("PesoTotal").Value IsNot Nothing Then
                        Double.TryParse(Convert.ToString(dgvDadosPecas.Rows(b).Cells("PesoTotal").Value), Globalization.NumberStyles.Any, Globalization.CultureInfo.InvariantCulture, pesoTot)
                    End If
                    command.Parameters.AddWithValue("@Peso", pesoTot.ToString().Replace(",", "."))

                    command.Parameters.AddWithValue("@Unidade", "UN")
                    command.Parameters.AddWithValue("@CodMatFabricante", codMatFab)

                    Dim caminho As String = ""
                    If dgvDadosPecas.Columns.Contains("CaminhoCompleto") AndAlso dgvDadosPecas.Rows(b).Cells("CaminhoCompleto").Value IsNot Nothing Then caminho = Convert.ToString(dgvDadosPecas.Rows(b).Cells("CaminhoCompleto").Value)
                    command.Parameters.AddWithValue("@EnderecoArquivo", caminho)

                    Dim materialsw As String = ""
                    If dgvDadosPecas.Columns.Contains("Material") AndAlso dgvDadosPecas.Rows(b).Cells("Material").Value IsNot Nothing Then materialsw = Convert.ToString(dgvDadosPecas.Rows(b).Cells("Material").Value)
                    command.Parameters.AddWithValue("@MaterialSW", materialsw)

                    Dim qtdTot As Double = 0
                    If dgvDadosPecas.Columns.Contains("QtdeTotal") AndAlso dgvDadosPecas.Rows(b).Cells("QtdeTotal").Value IsNot Nothing Then Double.TryParse(Convert.ToString(dgvDadosPecas.Rows(b).Cells("QtdeTotal").Value), Globalization.NumberStyles.Any, Globalization.CultureInfo.InvariantCulture, qtdTot)
                    command.Parameters.AddWithValue("@QtdeTotal", qtdTot.ToString().Replace(",", "."))

                    Dim qtdUnit As Double = 0
                    If dgvDadosPecas.Columns.Contains("Qtde") AndAlso dgvDadosPecas.Rows(b).Cells("Qtde").Value IsNot Nothing Then Double.TryParse(Convert.ToString(dgvDadosPecas.Rows(b).Cells("Qtde").Value), Globalization.NumberStyles.Any, Globalization.CultureInfo.InvariantCulture, qtdUnit)
                    command.Parameters.AddWithValue("@Qtde", qtdUnit.ToString().Replace(",", "."))

                    command.Parameters.AddWithValue("@txtTipoDesenho", tipoLinha)

                    Dim areaPint As Double = 0
                    If dgvDadosPecas.Columns.Contains("AreaPinturaTotal") AndAlso dgvDadosPecas.Rows(b).Cells("AreaPinturaTotal").Value IsNot Nothing Then Double.TryParse(Convert.ToString(dgvDadosPecas.Rows(b).Cells("AreaPinturaTotal").Value), Globalization.NumberStyles.Any, Globalization.CultureInfo.InvariantCulture, areaPint)
                    command.Parameters.AddWithValue("@AreaPinturaUnitario", areaPint.ToString().Replace(",", "."))

                    command.Parameters.AddWithValue("@DataCriacao", Now)
                    command.Parameters.AddWithValue("@CriadoPor", System.Environment.UserName)

                    command.ExecuteNonQuery()
                End Using
            Next

            ProgressBar1.Visible = False
            lblProgresso.Visible = False
            MessageBox.Show("A estrutura do produto [" & codProdutoPai & "] foi gerada com sucesso e já está pronta para gerar futuras Ordens de Serviço!", "Produto Estruturado", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception
            ProgressBar1.Visible = False
            lblProgresso.Visible = False
            MessageBox.Show("Erro ao gravar a estrutura do produto:" & vbCrLf & ex.Message, "SINCO - Erro", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            ' Reativar timers
            timerAtualizacao.Start()
            Timerdgvos.Enabled = True
            TimerdgvProcessoMaterial.Enabled = True
            TimerdgvProcesso.Enabled = True
            TimerdgvMateriaisProtheus.Enabled = True
            TimerManufaturada.Enabled = True
            TimerdgvGabaritos.Enabled = True
            cl_BancoDados.FecharBanco()
        End Try
    End Sub



    Private Sub BtnGeraArquivos_Click(sender As Object, e As EventArgs) Handles BtnGeraArquivos.Click

        Try



            ModListarComponentesMontagem.ExportarPorGrid(dgvDadosPecas, chkdxf, chkPdf, chkiges, ProgressBar1, lblProgresso, chkOpcaodePasta)
            MsgBox("Processo Finalizado!!!", vbInformation, "Finalização")

        Catch ex As Exception
        Finally
        End Try

    End Sub


    Private Sub DataGridView1_DataError(sender As Object, e As DataGridViewDataErrorEventArgs) Handles dgvDadosPecas.DataError
        Try



        Catch ex As Exception
        Finally
        End Try
    End Sub

    Private Sub AbrirArquivoToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AbrirArquivoToolStripMenuItem.Click

        Dim EnderecoArquivo As String = dgvDadosPecas.CurrentRow.Cells("CaminhoCompleto").Value.ToString()

        If File.Exists(EnderecoArquivo) Then
            Process.Start(EnderecoArquivo)
        Else
            MessageBox.Show("Arquivo não encontrado: " & EnderecoArquivo, "SINCO - Solid Edge",
                            MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End If
    End Sub

    Private Sub btnLimpar_Click(sender As Object, e As EventArgs) Handles btnLimpar.Click

        Try
            ' 🔹 Remove o DataSource — limpa completamente os dados
            dgvDadosPecas.DataSource = Nothing
            dgvDadosPecas.Rows.Clear()
            dgvDadosPecas.Columns.Clear()

            '' 🔹 (Re)Cria suas colunas fixas de controle, se necessário
            'Dim colDXF As New DataGridViewImageColumn()
            'colDXF.Name = "dgvdxf"
            'colDXF.HeaderText = "DXF"
            'colDXF.ImageLayout = DataGridViewImageCellLayout.Zoom
            ''dgvDadosPecas.Columns.Add(colDXF)

            'Dim colPDF As New DataGridViewImageColumn()
            'colPDF.Name = "dgvpdf"
            'colPDF.HeaderText = "PDF"
            'colPDF.ImageLayout = DataGridViewImageCellLayout.Zoom
            '' dgvDadosPecas.Columns.Add(colPDF)

            ' 🔹 Atualiza interface
            dgvDadosPecas.Refresh()
            lblResumo.Text = ""

            ' 🔹 Zera a variável global da tabela (se estiver usando)
            tabelaOriginalBOM = Nothing

            'MessageBox.Show("✅ Grid limpo. Pronto para nova leitura.",
            '            "SINCO - Solid Edge",
            '            MessageBoxButtons.OK, MessageBoxIcon.Information)
            Me.lblProgresso.Text = "0%"

        Catch ex As Exception
            MessageBox.Show("Erro ao limpar grid: " & ex.Message,
                        "SINCO - Solid Edge",
                        MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

    End Sub

    Private Sub AbrirDetalhamentoToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AbrirDetalhamentoToolStripMenuItem.Click

        Dim Endereco As String = dgvDadosPecas.CurrentRow.Cells("CaminhoCompleto").Value.ToString()

        Endereco = Replace(Endereco, ".SLDPRT", ".dft")
        Endereco = Replace(Endereco, ".SLDASM", ".dft")
        Endereco = Replace(Endereco, ".sldprt", ".dft")
        Endereco = Replace(Endereco, ".sldasm", ".dft")
        Endereco = Replace(Endereco, ".asm", ".dft")
        Endereco = Replace(Endereco, ".ASM", ".dft")
        Endereco = Replace(Endereco, ".psm", ".dft")
        Endereco = Replace(Endereco, ".PSM", ".dft")
        Endereco = Replace(Endereco, ".par", ".dft")
        Endereco = Replace(Endereco, ".PAR", ".dft")

        If File.Exists(Endereco) Then
            Process.Start(Endereco)
        Else
            MessageBox.Show("Arquivo não encontrado: " & Endereco, "SINCO - Solid Edge",
                            MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End If

    End Sub

    Private Sub AbrirPdfToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AbrirPdfToolStripMenuItem.Click


        Dim Endereco As String = dgvDadosPecas.CurrentRow.Cells("CaminhoCompleto").Value.ToString()

        Endereco = Replace(Endereco, ".SLDPRT", ".pdf")
        Endereco = Replace(Endereco, ".SLDASM", ".pdf")
        Endereco = Replace(Endereco, ".sldprt", ".pdf")
        Endereco = Replace(Endereco, ".sldasm", ".pdf")
        Endereco = Replace(Endereco, ".asm", ".pdf")
        Endereco = Replace(Endereco, ".ASM", ".pdf")
        Endereco = Replace(Endereco, ".psm", ".pdf")
        Endereco = Replace(Endereco, ".PSM", ".pdf")
        Endereco = Replace(Endereco, ".par", ".pdf")
        Endereco = Replace(Endereco, ".PAR", ".pdf")

        If File.Exists(Endereco) Then
            Process.Start(Endereco)
        Else
            MessageBox.Show("Arquivo não encontrado: " & Endereco, "SINCO - Solid Edge",
                            MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End If

    End Sub

    Private Sub btnAbriDetalhamentoCorrente_Click(sender As Object, e As EventArgs) Handles btnAbriDetalhamentoCorrente.Click

        AbriDetalhamentoDesenhoCorrente()


    End Sub


    Private Sub timerAtualizacao_Tick(sender As Object, e As EventArgs) Handles timerAtualizacao.Tick


        Try
            ' 🔹 Se o Solid Edge não está aberto ou não há documentos, sai
            If app Is Nothing OrElse app.Documents.Count = 0 Then Exit Sub

            Dim doc As Object = Nothing
            Try
                doc = app.ActiveDocument
            Catch
                Exit Sub
            End Try

            Static nomeAnterior As String = ""

            ' 🔸 Se o documento atual é o mesmo que o anterior, não faz nada
            If doc.FullName = nomeAnterior Then Exit Sub

            ' 🔸 Se o documento mudou e havia edição pendente, salva antes de trocar
            If nomeAnterior <> "" AndAlso doc.FullName <> nomeAnterior AndAlso editando Then
                If Not String.IsNullOrEmpty(nomePropriedadeEditando) Then
                    SalvarPropriedadeAlterada(nomePropriedadeEditando, valorEditado)
                End If
                editando = False
                nomePropriedadeEditando = ""
                valorEditado = ""
            End If

            ' 🔸 Atualiza propriedades apenas quando um novo documento é detectado
            If Not String.IsNullOrWhiteSpace(doc.FullName) Then
                nomeAnterior = doc.FullName
                CarregarPropriedadesArquivoCorrente()
                VerificaCadastroArquivo()
                TimerDGVListaMaterialSW.Enabled = True
                TimerdgvProcessoMaterial.Enabled = True
            End If

        Catch ex As Exception
            ' Falha silenciosa (não trava a UI)
            ' Debug.WriteLine($"Erro em timerAtualizacao_Tick: {ex.Message}")
        End Try
    End Sub


    Private bloqueiaEventosUI As Boolean = False

    Public Sub CarregarPropriedadesArquivoCorrente()

        'tenta ativa o maximo as variaveis do solid edge
        AtivarVariaveisESincronizarPropriedades()



        Try
            timerAtualizacao.Interval = 2000
            timerAtualizacao.Start()

            If app Is Nothing OrElse app.Documents.Count = 0 Then Exit Sub

            Dim doc As Object = app.ActiveDocument
            If doc Is Nothing Then Exit Sub

            Dim caminho As String = doc.FullName
            Dim ext As String = IO.Path.GetExtension(caminho).ToLower()
            If Not (ext = ".par" Or ext = ".psm" Or ext = ".asm") Then Exit Sub

            Dim NomeArquivo As String = IO.Path.GetFileNameWithoutExtension(caminho).ToUpper()

            ' 🚀 Suspende repintura
            Me.SuspendLayout()
            'dgvPropriedades.SuspendLayout()
            bloqueiaEventosUI = True

            txtNumeroDesenho.Text = NomeArquivo & ext
            txtendereco.Text = caminho


            ' 👉 Nenhuma leitura suja aqui! Delega para o Reader Service:
            Dim leitor As New SolidEdgeReaderService()
            leitor.ExtrairPropriedades(doc, DadosArquivoCorrente)

            ' Retorna e Atualiza UI
            txtTitulo.Text = DadosArquivoCorrente.Titulo
            cboAcabamento.Text = DadosArquivoCorrente.Comentarios
            txtPalavraChave.Text = DadosArquivoCorrente.PalavraChave
            txtAutor.Text = DadosArquivoCorrente.Author
            txtEmpresa.Text = DadosArquivoCorrente.CodigoJuridicoMat
            txtCategoria.Text = DadosArquivoCorrente.Verificado
            txtGerente.Text = DadosArquivoCorrente.Aprovado
            txtMaterialSw.Text = DadosArquivoCorrente.material
            cboTipoDesenho.Text = DadosArquivoCorrente.TipoDesenho

            txtData.Text = DadosArquivoCorrente.DataCriacaDesenho
            txtDataR.Text = DadosArquivoCorrente.DataUltimoSalvamento

            txtCutSizex.Text = DadosArquivoCorrente.ComprimentoBlank
            txtCutSizey.Text = DadosArquivoCorrente.LarguraBlank
            txtEspessura.Text = DadosArquivoCorrente.Espessura
            txtPesoKg.Text = DadosArquivoCorrente.Massa
            txtAreametroquadr.Text = DadosArquivoCorrente.AreaPintura


            ' 🔹 Atualiza apenas no final
            bloqueiaEventosUI = False
            Me.ResumeLayout()



            ' 🔹 Roda verificação em background
            VerificaCadastroArquivo()

        Catch ex As Exception

        Finally
            'MessageBox.Show("Erro ao carregar propriedades: " & ex.Message,
            '            "SINCO - Solid Edge", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub LimaprCampos()

        DadosArquivoCorrente.Titulo = ""
        txtTitulo.Clear()

        DadosArquivoCorrente.AssuntoSubiTitulo = ""
        DadosArquivoCorrente.Comentarios = ""
        Me.cboAcabamento.Text = ""

        DadosArquivoCorrente.PalavraChave = ""
        txtPalavraChave.Clear()

        DadosArquivoCorrente.Author = ""
        txtAutor.Clear()


        txtEmpresa.Clear()

        DadosArquivoCorrente.Verificado = ""
        txtCategoria.Clear()

        DadosArquivoCorrente.Aprovado = ""
        txtGerente.Clear()

        DadosArquivoCorrente.material = ""
        txtMaterialSw.Clear()
        '──────────────────────────────
        ' 📅 Datas
        '──────────────────────────────

        DadosArquivoCorrente.DataCriacaDesenho = ""
        txtData.Clear()


        DadosArquivoCorrente.DataUltimoSalvamento = ""
        txtDataR.Clear()


        txtData1.Clear()

        '──────────────────────────────
        ' 🧾 Revisão
        '──────────────────────────────

        txtRevisao.Clear()

        '──────────────────────────────
        ' 📐 Dimensões
        '──────────────────────────────

        DadosArquivoCorrente.ComprimentoBlank = ""
        txtCutSizex.Clear()


        DadosArquivoCorrente.LarguraBlank = ""
        txtCutSizey.Clear()


        DadosArquivoCorrente.Espessura = ""
        txtEspessura.Clear()



        DadosArquivoCorrente.Massa = ""
        txtPesoKg.Clear()


        '──────────────────────────────
        ' 📏 Área (mm² → m²)
        '──────────────────────────────



        DadosArquivoCorrente.AreaPintura = 0
        txtAreametroquadr.Clear()


    End Sub

    Public Sub ListarTodasPropriedadesDoArquivo(txtDestino As TextBox)
        Try
            ' ==========================================================
            ' 🔹 Obtém a instância ativa do Solid Edge
            ' ==========================================================
            ' Dim seApp As SolidEdgeFramework.Application =
            ' Marshal.GetActiveObject("SolidEdge.Application")

            If app Is Nothing OrElse app.Documents.Count = 0 Then
                MessageBox.Show("Nenhum documento aberto no Solid Edge.",
                            "SINCO - Solid Edge", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Exit Sub
            End If

            Dim doc As Object = app.ActiveDocument
            If doc Is Nothing Then Exit Sub

            ' ==========================================================
            ' 🔹 Builder de texto
            ' ==========================================================
            Dim sb As New StringBuilder()
            sb.AppendLine("🧾 LISTAGEM COMPLETA DE PROPRIEDADES DO ARQUIVO")
            sb.AppendLine(New String("="c, 80))
            sb.AppendLine($"📄 Arquivo: {doc.FullName}")
            sb.AppendLine($"📁 Tipo: {IO.Path.GetExtension(doc.FullName).ToUpper()}")
            sb.AppendLine(New String("="c, 80))
            sb.AppendLine()

            ' ==========================================================
            ' 🔹 Percorre todos os grupos e propriedades
            ' ==========================================================
            Dim propSets As Object = doc.Properties
            Dim totalProps As Integer = 0

            For Each propSet As Object In propSets
                Dim nomeGrupo As String = propSet.Name

                For Each prop As Object In propSet
                    Try
                        Dim nomeProp As String = prop.Name
                        Dim valor As String = ""

                        Try
                            valor = If(prop.Value IsNot Nothing, prop.Value.ToString(), "")
                        Catch
                            valor = "(erro ao ler)"
                        End Try

                        ' Verifica se é editável
                        Dim editavel As String = "Sim"
                        Try
                            Dim temp = prop.Value
                            prop.Value = temp
                        Catch
                            editavel = "Não"
                        End Try

                        ' 🧩 Linha formatada no estilo 'Grupo - Propriedade: Valor”
                        sb.AppendLine($"{nomeGrupo} - {nomeProp}: {valor}")
                        sb.AppendLine($"    ↳ Editável: {editavel}")
                        sb.AppendLine()
                        totalProps += 1

                    Catch ex As Exception
                        sb.AppendLine($"{nomeGrupo} - (Erro ao acessar propriedade): {ex.Message}")
                        sb.AppendLine()
                    End Try
                Next
            Next

            sb.AppendLine(New String("="c, 80))
            sb.AppendLine($"Total de propriedades: {totalProps}")
            sb.AppendLine("✅ Fim da listagem.")

            ' ==========================================================
            ' 🔹 Exibe no TextBox
            ' ==========================================================
            txtDestino.Text = sb.ToString()
            txtDestino.ScrollBars = ScrollBars.Both
            txtDestino.Multiline = True
            txtDestino.WordWrap = False
            txtDestino.ReadOnly = True
            txtDestino.SelectionStart = 0
            txtDestino.ScrollToCaret()

        Catch ex As Exception
            MessageBox.Show("Erro ao listar propriedades: " & ex.Message,
                        "SINCO - Solid Edge", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub


    Private Function LimparUnidades(texto As String) As String
        If String.IsNullOrWhiteSpace(texto) Then Return ""
        Return texto _
        .Replace("mm²", "") _
        .Replace("MM²", "") _
        .Replace("mm", "") _
        .Replace("MM", "") _
        .Replace("^2", "") _
         .Replace("KG", "") _
                 .Replace("kg", "") _
        .Trim()
    End Function

    Public Sub SalvarPropriedadeAlterada(nomeCompleto As String, novoValor As String)
        Try
            ' Obter instância ativa do Solid Edge
            'Dim seApp As SolidEdgeFramework.Application =
            ' Marshal.GetActiveObject("SolidEdge.Application")
            If app Is Nothing OrElse app.Documents.Count = 0 Then Exit Sub

            Dim doc As Object = app.ActiveDocument
            If doc Is Nothing Then Exit Sub

            ' Separar grupo e nome
            Dim partes = nomeCompleto.Split(New String() {" - "}, StringSplitOptions.None)
            If partes.Length <> 2 Then Exit Sub

            Dim grupo As String = partes(0).Trim()
            Dim nome As String = partes(1).Trim()

            Dim propSets As Object = doc.Properties
            Dim alterou As Boolean = False

            For Each propSet As Object In propSets
                If String.Equals(propSet.Name, grupo, StringComparison.OrdinalIgnoreCase) Then
                    For Each prop As Object In propSet
                        If String.Equals(prop.Name, nome, StringComparison.OrdinalIgnoreCase) Then
                            prop.Value = novoValor
                            alterou = True
                            Exit For
                        End If
                    Next
                End If
            Next

            If alterou Then
                ' 🔹 Força o Solid Edge a aplicar e sincronizar
                Try : doc.UpdatePropertyTextDisplay() : Catch : End Try
                Try : doc.Save() : Catch : End Try
                Try : doc.Properties.Save() : Catch : End Try


                'MessageBox.Show($"✅ Propriedade '{nomeCompleto}' atualizada com sucesso!",
                '            "SINCO - Solid Edge", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                MessageBox.Show($"⚠️ Propriedade '{nomeCompleto}' não encontrada.",
                            "SINCO - Solid Edge", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            End If

        Catch ex As Exception
            MessageBox.Show("Erro ao salvar propriedade: " & ex.Message,
                        "SINCO - Solid Edge", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    Private Sub btnLimparFiltro_Click(sender As Object, e As EventArgs) Handles btnLimparFiltro.Click

        txtFiltroCaminho1.Clear()
        ' txtFiltroCaminho = TextBox para caminho
        ' txtFiltroEmpresa = TextBox para empresa
        FiltrarPorCaminho(dgvDadosPecas, "", "")


    End Sub

    Private Sub btnFiltrar_Click(sender As Object, e As EventArgs) Handles btnFiltrar.Click

        ' txtFiltroCaminho = TextBox para caminho
        ' txtFiltroEmpresa = TextBox para empresa
        FiltrarPorCaminho(dgvDadosPecas, txtFiltroCaminho1.Text, txtFiltroEmpresa.Text)


    End Sub

    Private Sub cboProjeto_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboProjeto.SelectedIndexChanged

        Try

            'OrdemServico.Projeto = cboProjeto.Text
            'OrdemServico.idProjeto = Convert.ToInt32(cboProjeto.SelectedValue)

            ' Verificar se o combo box contém algum valor selecionado
            If cboProjeto.SelectedItem Is Nothing Then
                Throw New Exception("Nenhum Projeto selecionado. Por favor, selecione um Projeto válido.")
            End If

            ' Garantir que o texto do combo box não está vazio ou nulo
            If String.IsNullOrEmpty(cboProjeto.Text) Then
                Throw New Exception("O nome do Projeto não pode estar vazio. Selecione um Projeto válido.")
            End If

            ' Atribuir valores ao objeto OrdemServico
            OrdemServico.Projeto = cboProjeto.Text
            OrdemServico.idProjeto = cboProjeto.SelectedValue

            Me.txtCliente.Text = cboProjeto.Text

            Try

                ' Tentar converter o valor selecionado para inteiro
                ' Dim idProjeto As Integer
                If Integer.TryParse(cboProjeto.SelectedValue?.ToString(), Convert.ToInt32(OrdemServico.idProjeto)) Then

                    If My.Settings.TipoConexao = "MYSQL" Then

                        '	OrdemServico.idProjeto = OrdemServico.idProjeto

                        ' Preenchendo o ComboBox com os dados do banco
                        cl_BancoDados.ComboBoxDataSet("tags", "idTag", "Tag", cboTag, " where (D_E_L_E_T_E IS NULL OR D_E_L_E_T_E = '')   and (Finalizado = '' OR Finalizado Is NULL) AND idProjeto = '" & OrdemServico.idProjeto & "'")

                        ' Tentativa de retornar o nome da empresa
                        cl_BancoDados.RetornaCampoDaPesquisa("SELECT DescEmpresa,idempresa FROM  " & ComplementoTipoBanco & "projetos where idProjeto  = " & OrdemServico.idProjeto, "DescEmpresa", "idempresa")
                        txtCliente.Text = VCampo0

                        OrdemServico.DescEmpresa = VCampo0

                        If VCampo1 = "" Then

                            VCampo1 = 0

                        End If

                        OrdemServico.idempresa = VCampo1

                    ElseIf My.Settings.TipoConexao = "SQL" Then

                        'codificação codigo protheus
                        OrdemServico.idProjeto = cl_BancoDados.FormatarPara6Caracteres(OrdemServico.idProjeto)

                        ' Preenchendo o ComboBox com os dados do banco
                        cl_BancoDados.ComboBoxDataSet("View_SZ2010_GESTAO01", "Z2_PRODUTO", "Z2_DESC", cboTag, " where Z2_NUM = '" & OrdemServico.idProjeto & "'", "[MP12OFICIAL].[dbo].")

                        ' Tentativa de retornar o nome da empresa
                        cl_BancoDados.RetornaCampoDaPesquisa("SELECT Z1_DESC FROM  " & "[MP12OFICIAL].[dbo].[View_SZ1010_GESTAO] where Z1_NUM  = " & OrdemServico.idProjeto, "Z1_DESC")
                        txtCliente.Text = VCampo0

                    End If

                End If

                OrdemServico.Tag = cboTag.Text

                OrdemServico.DescEmpresa = txtCliente.Text
            Catch ex As Exception
                Me.txtCliente.Clear()
                OrdemServico.Projeto = Nothing
                OrdemServico.Tag = Nothing
                OrdemServico.DescEmpresa = Nothing

            End Try
        Catch ex As Exception
            ' MsgBox(ex.Message)
        Finally
        End Try

    End Sub

    Private Sub cboTag_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboTag.SelectedIndexChanged

        Try

            'OrdemServico.Tag = cboTag.Text
            'OrdemServico.idTag = cboTag.SelectedValue

            ' Verificar se o combo box contém algum valor selecionado
            If cboTag.SelectedItem Is Nothing Then
                Throw New Exception("Nenhuma Tag selecionada. Por favor, selecione uma Tag válida.")
            End If

            ' Garantir que o texto do combo box não está vazio ou nulo
            If String.IsNullOrEmpty(cboTag.Text) Then
                Throw New Exception("O nome da Tag não pode estar vazio. Selecione uma Tag válida.")
            End If

            ' Atribuir o texto da Tag ao objeto OrdemServico
            OrdemServico.Tag = cboTag.Text
            OrdemServico.idTag = cboTag.SelectedValue

            ' Tentar converter o valor selecionado para inteiro
            ' Dim idTag As Integer
            If Integer.TryParse(cboTag.SelectedValue?.ToString(), Convert.ToInt32(OrdemServico.idTag)) Then

                OrdemServico.DescTag = cboTag.Text

                ' OrdemServico.idTag = cl_BancoDados.FormatarPara7Caracteres(OrdemServico.idTag)

                '	OrdemServico.idTag = OrdemServico.idTag

                '    OrdemServico.idTag = idTag
                ' Else
                '   Throw New Exception("O ID da Tag selecionada não é válido. Por favor, verifique.")
            End If

            If My.Settings.TipoConexao = "SQL" Then

                ' Tenta retornar a descrição da Tag
                cl_BancoDados.RetornaCampoDaPesquisa("SELECT DISTINCT(Z2_DESC) FROM [MP12OFICIAL].[dbo].[View_SZ2010_GESTAO01] where Z2_PRODUTO = '" & OrdemServico.idTag & "'", "Z2_DESC")
                txtDescricaoTag.Text = VCampo0
                ' Tenta retornar a descrição da Tag
                cl_BancoDados.RetornaCampoDaPesquisa("SELECT DISTINCT(QtdeTag) FROM [MP12OFICIAL].[dbo].[View_SZ2010_GESTAO] where Z2_PRODUTO = '" & OrdemServico.idTag & "'", "QtdeTag")
                lbltxtQtdeTag.Text = VCampo0

                Try
                    cl_BancoDados.RetornaCampoDaPesquisa("SELECT DataPrevisao FROM [MP12OFICIAL].[dbo].[View_SZ2010_GESTAO] where Z2_PRODUTO = '" & OrdemServico.idTag & "'", "DataPrevisao")
                    OrdemServico.DataPrevisao = VCampo0
                Catch ex As Exception

                    OrdemServico.DataPrevisao = ""

                End Try

            ElseIf My.Settings.TipoConexao = "MYSQL" Then

                Me.lbltxtQtdeLiberada.Text = ""
                Me.lbltxtQtdeTag.Text = ""
                Me.lbltxtSaldoTag.Text = ""
                txtDescricaoTag.Clear()

                cl_BancoDados.VerificaSaldoTag(OrdemServico.idTag)

                txtDescricaoTag.Text = OrdemServico.Descricao
                Me.lbltxtQtdeTag.Text = OrdemServico.QtdeTag
                Me.lbltxtQtdeLiberada.Text = OrdemServico.QtdeLiberada
                Me.lbltxtSaldoTag.Text = OrdemServico.SaldoTag


            End If

            ' My.Settings.TipoConexao = "SQL"
        Catch ex As Exception
            ' Em caso de erro, limpa o campo de descrição
            Me.txtDescricaoTag.Clear()
            lbltxtQtdeTag.Text = ""

            OrdemServico.Tag = Nothing
            OrdemServico.idTag = Nothing

            ' Opcional: Registrar o erro em um log
            ' LogError(ex) ' Função fictícia para logar erros
            ' MessageBox.Show("Erro ao carregar a descrição da Tag.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

    End Sub

    Private Sub ToolStripButton1_Click(sender As Object, e As EventArgs) Handles ToolStripButton1.Click

        OrdemServico.IdOrdemServico = Nothing
        OrdemServico.Projeto = Nothing
        OrdemServico.Tag = Nothing
        OrdemServico.Descricao = Nothing
        OrdemServico.Estatus = Nothing
        OrdemServico.idTag = Nothing
        OrdemServico.idProjeto = Nothing
        OrdemServico.DescEmpresa = Nothing
        TSBSalvarOrdemServico.Enabled = True
        OrdemServico.DataPrevisao = Nothing
        OrdemServico.Liberado_Engenharia = Nothing

        Me.lblOrdemServicoAtiva.Text = ""

        Me.cboProjeto.DropDownStyle = ComboBoxStyle.DropDownList
        Me.cboTag.DropDownStyle = ComboBoxStyle.DropDownList

        Me.cboProjeto.Text = ""
        Me.cboTag.Text = ""
        Me.txtCliente.Clear()
        Me.txtDescricaoTag.Clear()
        Me.txtDescricao.Clear()
        Me.cboProjeto.Enabled = True
        Me.cboTag.Enabled = True

        Me.cboProjeto.Focus()

        TimerDGVListaMaterialSW.Enabled = True
    End Sub

    Private Async Sub TimerDGVListaMaterialSW_Tick(sender As Object, e As EventArgs) Handles TimerDGVListaMaterialSW.Tick

        Try
            TimerDGVListaMaterialSW.Enabled = False

            Dim pesqNumeroDesenho As String = Me.txtPesqNumeroDesenho.Text
            Dim pesqAcabamento As String = Me.txtPesqAcabamentoDesenho.Text
            Dim idOS As String = OrdemServico.IdOrdemServico.ToString()
            Dim compTb As String = ComplementoTipoBanco
            Dim conStr As String = conexao

            Dim dt1 As System.Data.DataTable = Nothing
            Dim dt2 As System.Data.DataTable = Nothing

            ' Ativa a barra de progresso em modo contínuo animado ("Marquee")
            ProgressBarOSM.Style = ProgressBarStyle.Marquee
            ProgressBarOSM.MarqueeAnimationSpeed = 30
            ProgressBarOSM.Visible = True

            Await Task.Run(Sub()
                               Try
                                   If My.Settings.TipoConexao.ToUpper() = "MYSQL" AndAlso Not String.IsNullOrWhiteSpace(conStr) Then
                                       Using con As New MySql.Data.MySqlClient.MySqlConnection(conStr)
                                           con.Open()

                                           Dim sql1 As String = "SELECT
            			IdOrdemServicoItem, IdOrdemServico, Projeto, Tag, Nivel, CodMatFabricante, Fator, Qtde, QtdeTotal, DescResumo, DescDetal, Estatus_OrdemServico, IdMaterial, CriadoPor, DataCriacao, Estatus, Acabamento, D_E_L_E_T_E, OrdemServicoItemFinalizado, IdEmpresa, IdProjeto, IdTag, Autor, PalavraChave, Notas, Espessura, MaterialSW, AreaPintura, NumeroDobras, Peso, Unidade, UnidadeSW, ValorSW, Altura, Largura, DtCad, UsuarioCriacao, UsuarioAlteracao, DtAlteracao, EnderecoArquivo, txtSoldagem, txtTipoDesenho, txtCorte, txtDobra, txtSolda, txtPintura, txtMontagem, QtdeRomaneio, Liberado_Engenharia, Data_Liberacao_Engenharia, DescEmpresa, ProdutoPrincipal, RNC, ComprimentoCaixaDelimitadora, LarguraCaixaDelimitadora, EspessuraCaixaDelimitadora, txtItemEstoque, AreaPinturaUnitario, PesoUnitario, DataPrevisao, EnderecoArquivoItemOrdemServico, NovoRevisao
            						   FROM " & compTb & "ordemservicoitem
            				WHERE (D_E_L_E_T_E <> '*') AND (IdOrdemServico = '" & idOS & "') AND (CodMatFabricante LIKE '%" & pesqNumeroDesenho & "%') AND (Acabamento LIKE '%" & pesqAcabamento & "%') ORDER BY IdOrdemServicoItem"

                                           Using cmd1 As New MySql.Data.MySqlClient.MySqlCommand(sql1, con)
                                               Using da1 As New MySql.Data.MySqlClient.MySqlDataAdapter(cmd1)
                                                   dt1 = New System.Data.DataTable()
                                                   da1.Fill(dt1)
                                               End Using
                                           End Using

                                           'Dim sql2 As String = "SELECT o.PROJETO AS Projeto, 
                                           '            o.TAG as Tag, o.Nivel as Nivel, 
                                           '            o.CodMatFabricante as CodMatFabricante, 
                                           '            o.CodMatFabricante as NumeroRP, o.DescResumo as DescResumo, 
                                           '            o.DescDetal as DescDetal, SUM(o.QtdeTotal) AS QtdeTotal, 
                                           '            o.Unidade as Unidade, SUM(o.Peso) AS Peso, 
                                           '            o.IDOrdemServico as IDOrdemServico 
                                           '            FROM " & compTb & "ordemservicoitem o 
                                           '            WHERE (o.D_E_L_E_T_E <> '*' or o.D_E_L_E_T_E is null) AND 
                                           '            (IdOrdemServico = '" & idOS & "') and 
                                           '            enderecoarquivo = '' GROUP BY o.PROJETO, 
                                           '            o.TAG, o.Nivel, 
                                           '            o.CodMatFabricante, o.DescResumo, 
                                           '            o.DescDetal, o.Unidade, o.IDOrdemServico"

                                           Dim sql2 As String = "SELECT o.PROJETO AS Projeto, 
                                                       o.TAG as Tag, o.Nivel as Nivel, 
                                                       o.CodMatFabricante as CodMatFabricante, 
                                                       o.CodMatFabricante as NumeroRP, o.DescResumo as DescResumo, 
                                                       o.DescDetal as DescDetal, SUM(o.QtdeTotal) AS QtdeTotal, 
                                                       o.Unidade as Unidade, SUM(o.Peso) AS Peso, 
                                                       o.IDOrdemServico as IDOrdemServico 
                                                       FROM " & compTb & "ordemservicoitem o 
                                                       WHERE (o.D_E_L_E_T_E <> '*' or o.D_E_L_E_T_E is null) AND 
                                                       (IdOrdemServico = '" & idOS & "') and 
                                                       enderecoarquivo = '' GROUP BY o.CodMatFabricante"
                                           Using cmd2 As New MySql.Data.MySqlClient.MySqlCommand(sql2, con)
                                               Using da2 As New MySql.Data.MySqlClient.MySqlDataAdapter(cmd2)
                                                   dt2 = New System.Data.DataTable()
                                                   da2.Fill(dt2)
                                               End Using
                                           End Using
                                       End Using
                                   End If
                               Catch ex As Exception
                               End Try
                           End Sub)

            ' Esconde a barra de progresso após finalizado
            ProgressBarOSM.Style = ProgressBarStyle.Blocks
            ProgressBarOSM.Visible = False

            If dt1 IsNot Nothing Then DGVListaMaterialSW.DataSource = dt1
            If dt2 IsNot Nothing Then DGVListaMaterialSWMateriais.DataSource = dt2

        Catch ex As Exception
            ProgressBarOSM.Style = ProgressBarStyle.Blocks
            ProgressBarOSM.Visible = False
        End Try

    End Sub

    Private Sub Timerdgvos_Tick(sender As Object, e As EventArgs) Handles Timerdgvos.Tick

        'CarregarDadosAgrupados()

        Dim Filtro As String

        ' Verifica se a checkbox está marcada e ajusta o filtro
        If chkMostraLiberadasPelaEngenharia.Checked = False Then
            Filtro = " AND (Liberado_Engenharia = '' OR Liberado_Engenharia IS NULL )"
        End If

        dgvos.DataSource = cl_BancoDados.CarregarDados("SELECT IdOrdemServico,
												idProjeto,
												Projeto,
                                                Fator,
												Tag,
                                                DescTag,
												idTag,
												Descricao,
												DescEmpresa,
                                                replace(EnderecoOrdemServico,'##','\\') as ENDERECO,
												CriadoPor as CriadoPor,
												DataCriacao as DataCriacao,
												Liberado_Engenharia,
												Data_Liberacao_Engenharia,
												Estatus,
												DataPrevisao,
                                                ProdutoPadrao,
                                                format(PesoTotal,2) as PesoTotal,
                                                format(AreaPinturaTotal,2) as AreaPinturaTotal,
                                                idempresa,NumeroOpOmie
												FROM  " & ComplementoTipoBanco & "ordemservico WHERE (D_E_L_E_T_E <> '*' or D_E_L_E_T_E is null)" & Filtro &
                                                " AND CriadoPor LIKE '%" & Me.txtPesqCriadoPor.Text & "%' order by IdOrdemServico desc")

        Timerdgvos.Enabled = False

        '   cl_BancoDados.FormatarDataGridView(dgvos, "SIM")

    End Sub


    Dim Formatadgvos As Boolean = False
    Private Sub dgvos_DataBindingComplete(sender As Object, e As DataGridViewBindingCompleteEventArgs) Handles dgvos.DataBindingComplete

        If dgvos Is Nothing OrElse dgvos.Rows Is Nothing Then Exit Sub



        ' Configuração estrutural (uma vez)
        PrepararColunasDgv()



        ' Visibilidades seguras
        OcultarColunas("ENDERECO", "idTag", "idProjeto", "Estatus",
                   "DataPrevisao", "Data_Liberacao_Engenharia",
                   "Liberado_Engenharia", "ProdutoPadrao", "idEmpresa")

        FixarColuna("idProjeto")



    End Sub

    ''' ' Coloque no Form (campo para evitar reconfigurar a cada bind)
    Private _gridConfigured As Boolean = False

    Private Sub PrepararColunasDgv()
        If _gridConfigured Then Exit Sub

        ' Garante que a coluna de status exista e seja de imagem
        Dim colStatus As DataGridViewImageColumn = TryCast(dgvos.Columns("dgvStatus"), DataGridViewImageColumn)
        If colStatus Is Nothing Then
            colStatus = New DataGridViewImageColumn() With {
                .Name = "dgvStatus",
                .HeaderText = "Status",
                .ImageLayout = DataGridViewImageCellLayout.Zoom,
                .Width = 28,
                .ReadOnly = True
            }
            ' Opcional: fixa como primeira coluna
            dgvos.Columns.Insert(0, colStatus)
        End If

        ' Ajustes gerais de estilo (uma vez só)
        dgvos.AutoGenerateColumns = True
        dgvos.RowHeadersVisible = False
        dgvos.AllowUserToAddRows = False
        dgvos.AllowUserToDeleteRows = False
        dgvos.SelectionMode = DataGridViewSelectionMode.FullRowSelect

        _gridConfigured = True
    End Sub

    Private Sub OcultarColunas(ParamArray nomes() As String)
        For Each nome In nomes
            If dgvos.Columns.Contains(nome) Then
                dgvos.Columns(nome).Visible = False
            End If
        Next
    End Sub

    Private Sub FixarColuna(nome As String)
        If dgvos.Columns.Contains(nome) Then
            dgvos.Columns(nome).Frozen = True
        End If
    End Sub

    ' >>> NOVO: Usa CellFormatting para renderizar a imagem sem percorrer todas as linhas
    Private Sub dgvos_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs) Handles dgvos.CellFormatting
        If dgvos.Columns(e.ColumnIndex).Name = "dgvStatus" Then
            ' Lê o valor da coluna de origem com segurança (pode ser DBNull/Nothing)
            Dim valor As Object = Nothing
            If dgvos.Columns.Contains("Liberado_Engenharia") Then
                valor = dgvos.Rows(e.RowIndex).Cells("Liberado_Engenharia").Value
            End If

            Dim texto As String = If(valor Is Nothing OrElse valor Is DBNull.Value, "", Convert.ToString(valor))
            e.Value = If(texto.Equals("S", StringComparison.OrdinalIgnoreCase),
                         My.Resources.verificado,
                         My.Resources.atencao)
            e.FormattingApplied = True
        End If
    End Sub

    Private Sub txtTitulo_Leave(sender As Object, e As EventArgs) Handles txtTitulo.Leave

        SalvarPropriedadeAlterada(txtTitulo.Tag, txtTitulo.Text.ToUpper)

    End Sub

    Private Sub txtPalavraChave_Leave(sender As Object, e As EventArgs) Handles txtPalavraChave.Leave

        SalvarPropriedadeAlterada(txtPalavraChave.Tag, txtPalavraChave.Text.ToUpper)

    End Sub

    Private Sub txtEmpresa_Leave(sender As Object, e As EventArgs) Handles txtEmpresa.Leave
        SalvarPropriedadeAlterada(txtEmpresa.Tag, txtEmpresa.Text.ToUpper)
    End Sub


    Private Sub txtAutor_Leave(sender As Object, e As EventArgs) Handles txtAutor.Leave
        SalvarPropriedadeAlterada(txtAutor.Tag, txtAutor.Text.ToUpper)
    End Sub


    Private Sub txtData_Leave(sender As Object, e As EventArgs) Handles txtData.Leave
        SalvarPropriedadeAlterada(txtData.Tag, txtData.Text.ToUpper)
    End Sub


    Private Sub txtMaterialSw_Leave(sender As Object, e As EventArgs) Handles txtMaterialSw.Leave
        SalvarPropriedadeAlterada(txtMaterialSw.Tag, txtMaterialSw.Text.ToUpper)
    End Sub


    Private Sub txtCategoria_Leave(sender As Object, e As EventArgs) Handles txtCategoria.Leave
        SalvarPropriedadeAlterada(txtCategoria.Tag, txtCategoria.Text.ToUpper)
    End Sub


    Private Sub txtDataR_Leave(sender As Object, e As EventArgs) Handles txtDataR.Leave
        SalvarPropriedadeAlterada(txtDataR.Tag, txtDataR.Text.ToUpper)
    End Sub


    Private Sub txtEspessura_Leave(sender As Object, e As EventArgs) Handles txtEspessura.Leave
        SalvarPropriedadeAlterada(txtEspessura.Tag, txtEspessura.Text.ToUpper)
    End Sub


    Private Sub txtCutSizex_Leave(sender As Object, e As EventArgs) Handles txtCutSizex.Leave
        SalvarPropriedadeAlterada(txtCutSizex.Tag, txtCutSizex.Text.ToUpper)
    End Sub


    Private Sub txtCutSizey_Leave(sender As Object, e As EventArgs) Handles txtCutSizey.Leave
        SalvarPropriedadeAlterada(txtCutSizey.Tag, txtCutSizey.Text.ToUpper)
    End Sub

    Private Sub txtAreametroquadr_Leave(sender As Object, e As EventArgs) Handles txtAreametroquadr.Leave
        SalvarPropriedadeAlterada(txtAreametroquadr.Tag, txtAreametroquadr.Text.ToUpper)
    End Sub


    Private Sub txtGerente_Leave(sender As Object, e As EventArgs) Handles txtGerente.Leave
        SalvarPropriedadeAlterada(txtGerente.Tag, txtGerente.Text.ToUpper)
    End Sub


    Private Sub txtData1_Leave(sender As Object, e As EventArgs) Handles txtData1.Leave
        SalvarPropriedadeAlterada(txtData1.Tag, txtData1.Text.ToUpper)
    End Sub


    Private Sub txtRevisao_Leave(sender As Object, e As EventArgs) Handles txtRevisao.Leave
        SalvarPropriedadeAlterada(txtRevisao.Tag, txtRevisao.Text.ToUpper)
    End Sub

    ' No formulário:
    Private WithEvents _comboEditing As ComboBox

    ' Garante que o SelectedIndexChanged do combo seja capturado
    Private Sub dgvDadosPdf_EditingControlShowing(sender As Object, e As DataGridViewEditingControlShowingEventArgs) _
    Handles dgvDadosPdf.EditingControlShowing

        If dgvDadosPdf.CurrentCell Is Nothing Then Exit Sub

        ' Só quando a célula atual é da coluna ComboBox
        If dgvDadosPdf.Columns(dgvDadosPdf.CurrentCell.ColumnIndex).Name = "DGVDadosExtracaoPDF" Then
            ' Desassocia handler antigo (se existir) e associa o novo
            If _comboEditing IsNot Nothing Then
                RemoveHandler _comboEditing.SelectedIndexChanged, AddressOf Combo_SelectedIndexChanged
            End If

            _comboEditing = TryCast(e.Control, ComboBox)
            If _comboEditing IsNot Nothing Then
                AddHandler _comboEditing.SelectedIndexChanged, AddressOf Combo_SelectedIndexChanged
            End If
        End If
    End Sub

    Private Sub Combo_SelectedIndexChanged(sender As Object, e As EventArgs)
        If dgvDadosPdf.CurrentCell Is Nothing Then Exit Sub

        Dim colNome As String = dgvDadosPdf.Columns(dgvDadosPdf.CurrentCell.ColumnIndex).Name
        If colNome <> "DGVDadosExtracaoPDF" Then Exit Sub

        Dim cb As ComboBox = DirectCast(sender, ComboBox)
        Dim valor As Object = If(cb.SelectedValue, cb.SelectedItem)

        ' Escreve na coluna de destino da MESMA linha
        Dim linha = dgvDadosPdf.CurrentCell.RowIndex
        dgvDadosPdf.Rows(linha).Cells("dgvDadosSelecionado").Value = If(valor, "")
    End Sub

    ' Opcional: faz o DataGridView “commitar” a edição do combo imediatamente
    Private Sub dgvDadosPdf_CurrentCellDirtyStateChanged(sender As Object, e As EventArgs) _
    Handles dgvDadosPdf.CurrentCellDirtyStateChanged
        If dgvDadosPdf.IsCurrentCellDirty AndAlso
       dgvDadosPdf.Columns(dgvDadosPdf.CurrentCell.ColumnIndex).Name = "DGVDadosExtracaoPDF" Then
            dgvDadosPdf.CommitEdit(DataGridViewDataErrorContexts.Commit)
        End If
    End Sub
    Private Sub btnSalvar_Click(sender As Object, e As EventArgs) Handles btnSalvar.Click


        Try

            Try
                DadosArquivoCorrente.SalvarCorrente()

                VerificaCadastroArquivo()

                cl_BancoDados.FormataBtnMsg("Registro Salvo Com Sucesso!", 1, btnMsg, TimerbtnMsg)

            Catch ex As Exception

            End Try




        Catch ex As Exception

        Finally

        End Try

    End Sub


    Private Sub dgvListaProcessos_Click(sender As Object, e As EventArgs) Handles dgvProcessos.Click

        Try

            ProcessosPadrao.IdProcesso = dgvProcessos.CurrentRow.Cells("Idprocessofabricacao").Value.ToString()

        Catch ex As Exception
            ProcessosPadrao.IdMaterial = 0
        Finally
        End Try

    End Sub

    Private Sub dgvProcessos_DoubleClick(sender As Object, e As EventArgs) Handles dgvProcessos.DoubleClick


        Dim processoNome As String = ""

        processoNome = dgvProcessos.CurrentRow.Cells("processofabricacao").Value.ToString

        If processoNome.ToString = "" Or processoNome.ToString = "TODOS" Then

            Exit Sub

        End If



        Try
            ' Busca a MAIOR sequência existente para o material atual
            cl_BancoDados.RetornaCampoDaPesquisa("
            SELECT IFNULL(MAX(SequenciaExecucao), 0) AS UltimaSequencia 
            FROM material_processo 
            WHERE IdMaterial = '" & DadosArquivoCorrente.IdMaterial & "'", "UltimaSequencia")

            Dim Sequencia As Integer = 1
            If IsNumeric(VCampo0) Then Sequencia = CInt(VCampo0) + 1

            ' Chama a função que grava o processo no banco


            ProcessosPadrao.SalvarOrdeMServicoBanco(
            ProcessosPadrao.IdProcesso,
            DadosArquivoCorrente.IdMaterial,
            Sequencia,
            DadosArquivoCorrente.NomeArquivoSemExtensao
        )

            '  MsgBox("✅ Processo adicionado ao material com sucesso!", vbInformation, "Sucesso")

            Me.dgvProcessos.CurrentRow.DefaultCellStyle.BackColor = System.Drawing.Color.LightGreen


            TimerdgvProcessoMaterial.Enabled = True

        Catch ex As Exception
            cl_BancoDados.FormataBtnMsg("Erro ao salvar processo: " & ex.Message, 1, btnMsg, TimerbtnMsg)
            ' MsgBox("Erro ao salvar processo: " & ex.Message, vbCritical, "Erro")
        End Try

    End Sub

    Private Sub TimerdgvProcessoMaterial_Tick(sender As Object, e As EventArgs) Handles TimerdgvProcessoMaterial.Tick
        Try
            TimerdgvProcessoMaterial.Enabled = False

            If DadosArquivoCorrente Is Nothing OrElse String.IsNullOrEmpty(DadosArquivoCorrente.NomeArquivoSemExtensao) Then Exit Sub
            If cl_BancoDados Is Nothing Then Exit Sub

            Dim sql As String =
        "SELECT mp.IdMaterialProcesso, mp.IdMaterial, mp.IdProcesso,
                    p.processofabricacao AS processofabricacao,
                    mp.SequenciaExecucao, mp.TempoPadraoMin, mp.codmatfabricante
             FROM material_processo mp
             LEFT JOIN processofabricacao p ON p.idprocessofabricacao = mp.IdProcesso
             WHERE mp.codmatfabricante = '" & DadosArquivoCorrente.NomeArquivoSemExtensao & "'
             ORDER BY mp.SequenciaExecucao"

            dtproc = cl_BancoDados.CarregarDados(sql)

            dgvProcessoMaterial.DataSource = Nothing
            dgvProcessoMaterial.DataSource = dtproc
            ConfigurarGridProcesso()

        Catch ex As Exception
            Debug.WriteLine("[TimerdgvProcessoMaterial] Erro: " & ex.Message)
        End Try

    End Sub



    ' =====================================================================
#Region "⚙️ CONFIGURAÇÃO VISUAL DO GRID"
    Private Sub ConfigurarGridProcesso()
        Try
            Dim ocultarColunas = {
            "IdMaterialProcesso", "IdMaterial", "IdProcesso",
            "TempoPadraoMin", "codmatfabricante"
        }

            For Each nomeColuna In ocultarColunas
                If dgvProcessoMaterial.Columns.Contains(nomeColuna) Then
                    dgvProcessoMaterial.Columns(nomeColuna).Visible = False
                End If
            Next

            With dgvProcessoMaterial
                .AllowDrop = True
                .ReadOnly = True
                .AllowUserToAddRows = False
                .AllowUserToDeleteRows = False
                .SelectionMode = DataGridViewSelectionMode.FullRowSelect
                .MultiSelect = False
            End With
        Catch ex As Exception
            Debug.WriteLine("[ConfigurarGridProcesso] Erro: " & ex.Message)
        End Try
    End Sub
#End Region

    ' =====================================================================
#Region "🗑️ EXCLUSÃO DE PROCESSO"
    Private Sub dgvProcessoMaterial_KeyDown(sender As Object, e As KeyEventArgs) Handles dgvProcessoMaterial.KeyDown
        If e.KeyCode = Keys.Delete Then
            ExcluirProcessodoMaterial()
            TimerdgvProcesso.Enabled = True
        End If
    End Sub

    Private Sub ExcluirProcessodoMaterial()
        Try
            If dgvProcessoMaterial.CurrentRow Is Nothing Then
                MsgBox("Selecione um processo para excluir.", vbExclamation, "Aviso")
                Exit Sub
            End If

            Dim idMatProc As Integer = CInt(dgvProcessoMaterial.CurrentRow.Cells("IdMaterialProcesso").Value)
            Dim nomeProc As String = dgvProcessoMaterial.CurrentRow.Cells("processofabricacao").Value.ToString()

            Dim resp = MsgBox("Deseja realmente excluir o processo """ & nomeProc & """ ?", vbYesNo + vbQuestion, "Confirmação")
            If resp = vbNo Then Exit Sub

            Dim sqlDel As String = "DELETE FROM material_processo WHERE IdMaterialProcesso = " & idMatProc
            cl_BancoDados.Salvar(sqlDel)

            ReorganizarSequencias(DadosArquivoCorrente.NomeArquivoSemExtensao)
            TimerdgvProcessoMaterial.Enabled = True

            cl_BancoDados.FormataBtnMsg("Processo excluído e sequência atualizada!", 1, btnMsg, TimerbtnMsg)

            ' MsgBox("Processo excluído e sequência atualizada!", vbInformation, "Sucesso")

        Catch ex As Exception
            MsgBox("Erro ao excluir processo: " & ex.Message, vbCritical, "Erro")
        End Try
    End Sub


    Private Sub ExcluirGabaritoMaterial()
        Try
            If dgvGabaritos.CurrentRow Is Nothing Then
                MsgBox("Selecione um Gabarito para excluir.", vbExclamation, "Aviso")
                Exit Sub
            End If

            Dim idMatProc As Integer = CInt(dgvGabaritos.CurrentRow.Cells("Id").Value)
            Dim nomeProc As String = dgvGabaritos.CurrentRow.Cells("Desenho_Gabarito").Value.ToString()

            Dim resp = MsgBox("Deseja realmente excluir o Desenho """ & nomeProc & """ ?", vbYesNo + vbQuestion, "Confirmação")
            If resp = vbNo Then Exit Sub

            Dim sqlDel As String = "DELETE FROM material_gabarito WHERE idmaterial_gabarito = " & idMatProc
            cl_BancoDados.Salvar(sqlDel)

            ' TimerdgvGabaritos.Enabled = True

            ' MsgBox("Processo excluído e sequência atualizada!", vbInformation, "Sucesso")

        Catch ex As Exception
            MsgBox("Erro ao excluir Gabarito: " & ex.Message, vbCritical, "Erro")
        End Try
    End Sub


    Private Sub ExcluirDesenhoMaterial()
        Try
            If dgvDesenhoCliente.CurrentRow Is Nothing Then
                MsgBox("Selecione um Desenho para excluir.", vbExclamation, "Aviso")
                Exit Sub
            End If

            Dim idMatProc As Integer = CInt(dgvDesenhoCliente.CurrentRow.Cells("idmaterial_desenho").Value)
            Dim nomeProc As String = dgvDesenhoCliente.CurrentRow.Cells("z1_codcli").Value.ToString()

            Dim resp = MsgBox("Deseja realmente excluir o Desenho """ & nomeProc & """ ?", vbYesNo + vbQuestion, "Confirmação")
            If resp = vbNo Then Exit Sub

            Dim sqlDel As String = "DELETE FROM material_desenho WHERE idmaterial_desenho = " & idMatProc
            cl_BancoDados.Salvar(sqlDel)

            ' TimerdgvGabaritos.Enabled = True

            ' MsgBox("Processo excluído e sequência atualizada!", vbInformation, "Sucesso")

        Catch ex As Exception
            MsgBox("Erro ao excluir Gabarito: " & ex.Message, vbCritical, "Erro")
        End Try
    End Sub


    Private Sub ExcluirMaterialProtheus()
        Try
            If dgvMaterialPeca.CurrentRow Is Nothing Then
                MsgBox("Selecione um Material para excluir.", vbExclamation, "Aviso")
                Exit Sub
            End If

            Dim idMatProc As Integer = CInt(dgvMaterialPeca.CurrentRow.Cells("IdMontaPeca").Value.ToString)
            Dim nomeProc As String = dgvMaterialPeca.CurrentRow.Cells("DescDetal").Value.ToString()

            Dim resp = MsgBox("Deseja realmente excluir o processo " & nomeProc & " ?", vbYesNo + vbQuestion, "Confirmação")
            If resp = vbNo Then Exit Sub

            Dim sqlDel As String = "DELETE FROM montapeca WHERE IdMontaPeca = " & idMatProc
            cl_BancoDados.Salvar(sqlDel)

            TimerdgvProcessoMaterial.Enabled = True

            'cl_BancoDados.FormataBtnMsg("Processo excluído e sequência atualizada!", 1, btnMsg, TimerbtnMsg)

            ' MsgBox("Processo excluído e sequência atualizada!", vbInformation, "Sucesso")

        Catch ex As Exception
            MsgBox("Erro ao excluir Material da Peça: " & ex.Message, vbCritical, "Erro")
        End Try
    End Sub


    Private Sub ReorganizarSequencias(codMat As String)
        Try
            Dim dt As System.Data.DataTable = cl_BancoDados.CarregarDados("
            SELECT IdMaterialProcesso 
            FROM material_processo 
            WHERE codmatfabricante = '" & codMat & "'
            ORDER BY SequenciaExecucao")

            Dim seq As Integer = 1
            For Each r As DataRow In dt.Rows
                Dim id As Integer = CInt(r("IdMaterialProcesso"))
                Dim sqlUpd As String =
                "UPDATE material_processo SET SequenciaExecucao = " & seq &
                " WHERE IdMaterialProcesso = " & id
                cl_BancoDados.Salvar(sqlUpd)
                seq += 1
            Next

        Catch ex As Exception
            MsgBox("Erro ao reorganizar sequência: " & ex.Message, vbCritical, "Erro")
        End Try
    End Sub
#End Region

    ' =====================================================================
#Region "🖱️ DRAG & DROP PARA REORDENAR PROCESSOS"

    Private Sub dgvProcessoMaterial_MouseDown(sender As Object, e As MouseEventArgs) Handles dgvProcessoMaterial.MouseDown
        dragIndex = dgvProcessoMaterial.HitTest(e.X, e.Y).RowIndex
        If dragIndex < 0 OrElse dragIndex >= dgvProcessoMaterial.Rows.Count Then dragIndex = -1
    End Sub

    Private Sub dgvProcessoMaterial_MouseMove(sender As Object, e As MouseEventArgs) Handles dgvProcessoMaterial.MouseMove
        If e.Button = MouseButtons.Left AndAlso dragIndex >= 0 Then
            dgvProcessoMaterial.DoDragDrop(dgvProcessoMaterial.Rows(dragIndex), DragDropEffects.Move)
        End If
    End Sub

    Private Sub dgvProcessoMaterial_DragOver(sender As Object, e As DragEventArgs) Handles dgvProcessoMaterial.DragOver
        e.Effect = DragDropEffects.Move
    End Sub

    Private Sub dgvProcessoMaterial_DragDrop(sender As Object, e As DragEventArgs) Handles dgvProcessoMaterial.DragDrop
        Try
            If dragIndex < 0 Then Exit Sub
            If dtproc Is Nothing OrElse dtproc.Rows.Count = 0 Then Exit Sub

            Dim clientPoint = dgvProcessoMaterial.PointToClient(New System.Drawing.Point(e.X, e.Y))
            Dim dropIndex = dgvProcessoMaterial.HitTest(clientPoint.X, clientPoint.Y).RowIndex
            If dropIndex < 0 Then dropIndex = dgvProcessoMaterial.Rows.Count - 1

            ReordenarDataTable(dtproc, dragIndex, dropIndex)
            AtualizarSequenciaBancoAPartirDoDataTable()

            dgvProcessoMaterial.DataSource = Nothing
            dgvProcessoMaterial.DataSource = dtproc
            ConfigurarGridProcesso()

        Catch ex As Exception
            MsgBox("Erro ao mover processo: " & ex.Message, vbCritical, "Erro")
        Finally
            dragIndex = -1
        End Try
    End Sub
#End Region

    ' =====================================================================
#Region "🧱 MOVE LINHAS NO DATATABLE"
    Private Sub ReordenarDataTable(ByRef dt As System.Data.DataTable, fromIndex As Integer, toIndex As Integer)
        Try
            If dt Is Nothing Then Exit Sub
            If fromIndex = toIndex Then Exit Sub
            If fromIndex < 0 OrElse fromIndex >= dt.Rows.Count Then Exit Sub
            If toIndex < 0 Then toIndex = 0
            If toIndex >= dt.Rows.Count Then toIndex = dt.Rows.Count - 1

            Dim rowToMove As DataRow = dt.Rows(fromIndex)
            Dim newTable As System.Data.DataTable = dt.Clone()

            For i As Integer = 0 To dt.Rows.Count - 1
                If i = toIndex Then newTable.ImportRow(rowToMove)
                If i <> fromIndex Then newTable.ImportRow(dt.Rows(i))
            Next

            If toIndex = newTable.Rows.Count Then newTable.ImportRow(rowToMove)

            dt.Clear()
            For Each r As DataRow In newTable.Rows
                dt.ImportRow(r)
            Next
        Catch ex As Exception
            Debug.WriteLine("[ReordenarDataTable] " & ex.Message)
        End Try
    End Sub
#End Region

    ' =====================================================================
#Region "💾 ATUALIZA SEQUÊNCIA NO BANCO"
    Private Sub AtualizarSequenciaBancoAPartirDoDataTable()
        Try
            Dim seq As Integer = 1
            For Each r As DataRow In dtproc.Rows
                If r.RowState = DataRowState.Deleted Then Continue For
                Dim idMatProc As Integer = Convert.ToInt32(r("IdMaterialProcesso"))
                r("SequenciaExecucao") = seq

                Dim sql As String = "UPDATE material_processo " &
                                "SET SequenciaExecucao = " & seq &
                                " WHERE IdMaterialProcesso = " & idMatProc
                cl_BancoDados.Salvar(sql)
                seq += 1
            Next
        Catch ex As Exception
            Debug.WriteLine("[AtualizarSequenciaBancoAPartirDoDataTable] " & ex.Message)
        End Try
    End Sub


    Private _salvandoOS As Boolean = False
    Private Sub TSBSalvarOrdemServico_Click(sender As Object, e As EventArgs) Handles TSBSalvarOrdemServico.Click


        If OrdemServico.Liberado_Engenharia <> "" Then

            MsgBox("Ordem de serviço já liberada, não e possivel fazer alterações!", vbCritical, "Atenção")

            Exit Sub

        End If



        If _salvandoOS Then Exit Sub

        ' 1) Validação de usuário logado
        If String.IsNullOrWhiteSpace(Usuario.NomeCompleto) Then
            MsgBox("Você não está logado com um usuário válido. Não é possível criar OS.", vbCritical, "Atenção")
            Exit Sub
        End If

        ' 2) Validação básica de campos obrigatórios
        If String.IsNullOrWhiteSpace(Me.txtDescricao.Text) Then
            MsgBox("Descrição é obrigatória.", vbExclamation, "Atenção")
            Exit Sub
        End If

        ' 3) Leitura segura dos IDs dos comboboxes
        Dim idTag As Integer
        Dim idProjeto As Integer

        If Not TryGetSelectedInt(Me.cboTag, idTag) OrElse idTag <= 0 Then
            MsgBox("Selecione uma Tag válida.", vbExclamation, "Atenção")
            Exit Sub
        End If

        If Not TryGetSelectedInt(Me.cboProjeto, idProjeto) OrElse idProjeto <= 0 Then
            MsgBox("Selecione um Projeto válido.", vbExclamation, "Atenção")
            Exit Sub
        End If

        ' (Opcional) se houver data de entrega em algum controle:
        'Dim prevDataEntrega As DateTime? = Nothing
        'If DateTime.TryParse(Me.dtpPrevEntrega.Text, Nothing) Then prevDataEntrega = Me.dtpPrevEntrega.Value

        _salvandoOS = True
        TSBSalvarOrdemServico.Enabled = False
        Cursor.Current = Cursors.WaitCursor

        Try
            ' 4) Atribuições no objeto de domínio
            OrdemServico.Descricao = Me.txtDescricao.Text.Trim()
            OrdemServico.idTag = idTag
            OrdemServico.idProjeto = idProjeto
            'OrdemServico.PrevDataEntrega = prevDataEntrega

            ' 5) Pausa timers que podem competir com o salvamento (evita reentrância)
            Dim t1State = Timerdgvos.Enabled
            Dim t2State = TimerDGVListaMaterialSW.Enabled
            Timerdgvos.Enabled = False
            TimerDGVListaMaterialSW.Enabled = False

            Try
                ' 6) Chamada da rotina existente (mantida)
                OrdemServico.CriarOsCompleta(dgvos, Timerdgvos, TimerDGVListaMaterialSW)

                ' (Opcional) feedback de sucesso
                'MsgBox("Ordem de Serviço criada com sucesso.", vbInformation, "OK")
            Finally
                ' 7) Restaura timers ao estado anterior
                Timerdgvos.Enabled = t1State
                TimerDGVListaMaterialSW.Enabled = t2State
            End Try
        Catch ex As Exception
            ' Mostra a mensagem real para facilitar suporte
            MsgBox("Erro ao criar OS: " & ex.Message, vbCritical, "Erro")
        Finally
            Cursor.Current = Cursors.Default
            TSBSalvarOrdemServico.Enabled = True
            _salvandoOS = False
        End Try

    End Sub
#End Region


    ' =============================
    ' Helper para obter SelectedValue como Integer com segurança
    ' =============================
    Private Function TryGetSelectedInt(cbo As ComboBox, ByRef valueOut As Integer) As Boolean
        valueOut = 0
        If cbo Is Nothing Then Return False

        Dim v As Object = Nothing
        Try
            v = cbo.SelectedValue
        Catch
            ' Se o DataSource mudou durante o acesso
            Return False
        End Try

        If v Is Nothing OrElse Convert.IsDBNull(v) Then Return False

        ' Quando o DataSource está em edição, às vezes vem DataRowView
        If TypeOf v Is DataRowView Then
            Dim drv = DirectCast(v, DataRowView)
            ' Tenta pela DisplayMember/ValueMember
            Dim colName = If(String.IsNullOrWhiteSpace(cbo.ValueMember), cbo.DisplayMember, cbo.ValueMember)
            If Not String.IsNullOrWhiteSpace(colName) AndAlso drv.Row.Table.Columns.Contains(colName) Then
                Dim raw = drv(colName)
                If raw IsNot Nothing AndAlso Not Convert.IsDBNull(raw) Then
                    Return Integer.TryParse(Convert.ToString(raw), valueOut)
                End If
            End If
            Return False
        End If

        ' Normal: SelectedValue é escalar
        Return Integer.TryParse(Convert.ToString(v), valueOut)
    End Function



    Dim idOrdemServicoAnterior As Integer
    Private Async Sub dgvos_Click(sender As Object, e As EventArgs) Handles dgvos.Click



        Try
            OrdemServico.Liberado_Engenharia = dgvos.CurrentRow.Cells("Liberado_Engenharia").Value.ToString

        Catch ex As Exception
            OrdemServico.Liberado_Engenharia = ""
        End Try

        ' Se não houver linha atual, sai
        If dgvos.CurrentRow Is Nothing Then Exit Sub

        If idOrdemServicoAnterior <> Convert.ToInt32(dgvos.CurrentRow.Cells("idordemservico").Value) Then

            idOrdemServicoAnterior = Convert.ToInt32(dgvos.CurrentRow.Cells("idordemservico").Value)
        Else
            Exit Sub
        End If

        ' Carrega itens da OS (preenche OrdemServico.*)
        CarregarItemsOs()

        ' Lê idTag com segurança
        Dim vIdTag As Object = dgvos.CurrentRow.Cells("idTag").Value
        Dim idTag As Integer
        If vIdTag IsNot Nothing AndAlso Not Convert.IsDBNull(vIdTag) AndAlso Integer.TryParse(vIdTag.ToString(), idTag) Then
            OrdemServico.idTag = idTag
        Else
            OrdemServico.idTag = 0
        End If

        ' Lê IdOrdemServico com segurança
        Dim vIdOs As Object = dgvos.CurrentRow.Cells("IdOrdemServico").Value
        Dim idOs As Integer
        If vIdOs IsNot Nothing AndAlso Not Convert.IsDBNull(vIdOs) AndAlso Integer.TryParse(vIdOs.ToString(), idOs) Then
            OrdemServico.IdOrdemServico = idOs
        Else
            OrdemServico.IdOrdemServico = 0
        End If

        Dim vIdDescEmpresa As Object = dgvos.CurrentRow.Cells("descempresa").Value.ToString
        OrdemServico.DescEmpresa = vIdDescEmpresa

        Dim vIdDescTag As Object = dgvos.CurrentRow.Cells("DescTag").Value.ToString
        OrdemServico.DescTag = vIdDescTag


        OrdemServico.idempresa = dgvos.CurrentRow.Cells("idempresa").Value.ToString

        Dim strIdOS As String = OrdemServico.IdOrdemServico.ToString()
        Dim strIdTag As String = OrdemServico.idTag.ToString()
        Dim dtQuery As System.Data.DataTable = Nothing
        Dim conStr As String = conexao
        Dim compTb As String = ComplementoTipoBanco

        ' Variáveis locais para armazenar o resultado da leitura da Tag no background
        Dim bgQtdeTag As Integer = 0
        Dim bgSaldoTag As Integer = 0
        Dim bgQtdeLiberada As Integer = 0
        Dim bgDescricao As String = ""
        Dim bgDataPrevisao As String = ""
        Dim tagFound As Boolean = False

        ' Ativa a barra de progresso em modo contínuo animado ("Marquee")
        ProgressBarOSM.Style = ProgressBarStyle.Marquee
        ProgressBarOSM.MarqueeAnimationSpeed = 30
        ProgressBarOSM.Visible = True

        Await Task.Run(Sub()
                           Try
                               If My.Settings.TipoConexao.ToUpper() = "MYSQL" AndAlso Not String.IsNullOrWhiteSpace(conStr) Then
                                   Using con As New MySql.Data.MySqlClient.MySqlConnection(conStr)
                                       con.Open()

                                       ' 1. Consulta dos M2 e Pesos
                                       Dim sql As String = "SELECT  SUM((areapinturaunitario * qtdetotal) / FATOR) as areaPorEquipamento, SUM((areapinturaunitario * qtdetotal)) as areaTotal, SUM((pesounitario * qtdetotal) / FATOR) as PesoPorEquipamento, SUM((pesounitario * qtdetotal)) as PesoTotal FROM ordemservicoitem where (idordemservico = '" & strIdOS & "') AND (areapinturaunitario > 0) and (D_E_L_E_T_E = '' OR D_E_L_E_T_E IS NULL) and enderecoarquivo <> '' and tXttipodesenho <> 'MATERIAL';"
                                       Using cmd As New MySql.Data.MySqlClient.MySqlCommand(sql, con)
                                           Using dadd As New MySql.Data.MySqlClient.MySqlDataAdapter(cmd)
                                               Dim dt As New System.Data.DataTable()
                                               dadd.Fill(dt)
                                               dtQuery = dt
                                           End Using
                                       End Using

                                       ' 2. Consulta dos Saldos e Status da Tag (antigo VerificaSaldoTag - Syncro Freeze)
                                       Dim sqlTag As String = "SELECT QtdeTag, SaldoTag, QtdeLiberada, desctag, DataPrevisao FROM " & compTb & "tags where IdTag = '" & strIdTag & "'"
                                       Using cmdTag As New MySql.Data.MySqlClient.MySqlCommand(sqlTag, con)
                                           Using drTag As MySql.Data.MySqlClient.MySqlDataReader = cmdTag.ExecuteReader()
                                               If drTag.HasRows Then
                                                   drTag.Read()
                                                   Integer.TryParse(drTag("QtdeTag").ToString(), bgQtdeTag)
                                                   Integer.TryParse(drTag("SaldoTag").ToString(), bgSaldoTag)
                                                   Integer.TryParse(drTag("QtdeLiberada").ToString(), bgQtdeLiberada)
                                                   bgDescricao = drTag("desctag").ToString()
                                                   bgDataPrevisao = drTag("DataPrevisao").ToString()
                                                   tagFound = True
                                               End If
                                           End Using
                                       End Using

                                   End Using
                               End If
                           Catch ex As Exception
                               Debug.WriteLine("Erro Background Task OS_Click: " & ex.Message)
                           End Try
                       End Sub)

        ' Esconde a barra de progresso após finalizado
        ProgressBarOSM.Style = ProgressBarStyle.Blocks
        ProgressBarOSM.Visible = False

        ' Atualiza a Model OrdemServico se os dados da Tag foram encontrados
        If tagFound Then
            OrdemServico.QtdeTag = bgQtdeTag
            OrdemServico.SaldoTag = bgSaldoTag
            OrdemServico.QtdeLiberada = bgQtdeLiberada
            OrdemServico.Descricao = bgDescricao
            OrdemServico.DataPrevisao = bgDataPrevisao
        Else
            OrdemServico.QtdeTag = 0
            OrdemServico.SaldoTag = 0
            OrdemServico.QtdeLiberada = 0
            OrdemServico.Descricao = ""
            OrdemServico.DataPrevisao = ""
        End If

        Try
            ' Atualiza os rótulos de Quantidade e Saldos
            Me.lbltxtQtdeTag.Text = If(OrdemServico.QtdeTag = Nothing, "0", Convert.ToString(OrdemServico.QtdeTag))
            Me.lbltxtQtdeLiberada.Text = If(OrdemServico.QtdeLiberada = Nothing, "0", Convert.ToString(OrdemServico.QtdeLiberada))
            Me.lbltxtSaldoTag.Text = If(OrdemServico.SaldoTag = Nothing, "0", Convert.ToString(OrdemServico.SaldoTag))
        Catch ex As Exception
        End Try

        If dtQuery IsNot Nothing AndAlso dtQuery.Rows.Count > 0 Then
            Try
                Me.txtAreaPintM2.Text = Convert.ToString(dtQuery.Rows(0)("areaPorEquipamento"))
            Catch ex As Exception
                Me.txtAreaPintM2.Text = ""
            End Try

            Try
                Me.txtAreaPintM2Total.Text = Convert.ToString(dtQuery.Rows(0)("areaTotal"))
            Catch ex As Exception
                Me.txtAreaPintM2Total.Text = ""
            End Try

            Try
                Me.txtPesoTotalKg.Text = Convert.ToString(dtQuery.Rows(0)("PesoPorEquipamento"))
            Catch ex As Exception
                Me.txtPesoTotalKg.Text = ""
            End Try

            Try
                Me.txtPesoTotalKgTotal.Text = Convert.ToString(dtQuery.Rows(0)("PesoTotal"))
            Catch ex As Exception
                Me.txtPesoTotalKgTotal.Text = ""
            End Try
        Else
            Me.txtAreaPintM2.Text = ""
            Me.txtAreaPintM2Total.Text = ""
            Me.txtPesoTotalKg.Text = ""
            Me.txtPesoTotalKgTotal.Text = ""
        End If

    End Sub



    Private Function CarregarItemsOs()

        Cursor.Current = Cursors.WaitCursor

        Try
            Dim row As DataGridViewRow = dgvos.CurrentRow
            If row Is Nothing Then Return Nothing

            ' Garante que a célula-chave existe e não é nula/DBNull
            Dim vIdOs As Object = row.Cells("IdOrdemServico").Value
            If vIdOs Is Nothing OrElse Convert.IsDBNull(vIdOs) Then Return Nothing

            ' Campos numéricos com TryParse
            Dim tmpInt As Integer

            ' Liberado_Engenharia (string)
            OrdemServico.Liberado_Engenharia =
            If(Convert.IsDBNull(row.Cells("Liberado_Engenharia").Value), "",
               Convert.ToString(row.Cells("Liberado_Engenharia").Value))

            ' IdOrdemServico
            If Integer.TryParse(Convert.ToString(vIdOs), tmpInt) Then
                OrdemServico.IdOrdemServico = tmpInt
            Else
                OrdemServico.IdOrdemServico = 0
            End If

            ' idProjeto
            Dim vIdProj As Object = row.Cells("idProjeto").Value
            If vIdProj IsNot Nothing AndAlso Not Convert.IsDBNull(vIdProj) AndAlso Integer.TryParse(vIdProj.ToString(), tmpInt) Then
                OrdemServico.idProjeto = tmpInt
            Else
                OrdemServico.idProjeto = 0
            End If

            ' Projeto / Tag / Estatus / ENDERECO / DataPrevisao / Descricao / DescEmpresa
            OrdemServico.Projeto =
            If(Convert.IsDBNull(row.Cells("Projeto").Value), "", Convert.ToString(row.Cells("Projeto").Value))

            OrdemServico.Tag =
            If(Convert.IsDBNull(row.Cells("Tag").Value), "", Convert.ToString(row.Cells("Tag").Value))

            OrdemServico.Estatus =
            If(Convert.IsDBNull(row.Cells("Estatus").Value), "", Convert.ToString(row.Cells("Estatus").Value))

            OrdemServico.EnderecoOrdemServico =
            If(Convert.IsDBNull(row.Cells("ENDERECO").Value), "", Convert.ToString(row.Cells("ENDERECO").Value))

            ' DataPrevisao pode vir como Date/DBNull/String — normaliza para string
            If Convert.IsDBNull(row.Cells("DataPrevisao").Value) OrElse row.Cells("DataPrevisao").Value Is Nothing Then
                OrdemServico.DataPrevisao = ""
            Else
                Dim dt As DateTime
                Dim s As String = Convert.ToString(row.Cells("DataPrevisao").Value)
                If DateTime.TryParse(s, dt) Then
                    OrdemServico.DataPrevisao = dt.ToString("dd/MM/yyyy HH:mm")
                Else
                    ' Mantém como string se não parsear
                    OrdemServico.DataPrevisao = s
                End If
            End If

            ' Fator (numérico)
            Dim vFator As Object = row.Cells("Fator").Value
            If vFator IsNot Nothing AndAlso Not Convert.IsDBNull(vFator) AndAlso Integer.TryParse(vFator.ToString(), tmpInt) Then
                OrdemServico.Fator = tmpInt
            Else
                OrdemServico.Fator = 1
            End If

            ' Controles da UI (sem chamar ToString em DBNull)
            Me.cboProjeto.Text = OrdemServico.Projeto
            Me.txtCliente.Text =
            If(Convert.IsDBNull(row.Cells("DescEmpresa").Value), "", Convert.ToString(row.Cells("DescEmpresa").Value))
            Me.cboTag.Text = OrdemServico.Tag
            Me.txtDescricaoTag.Text = OrdemServico.Tag
            Me.txtDescricao.Text =
            If(Convert.IsDBNull(row.Cells("Descricao").Value), "", Convert.ToString(row.Cells("Descricao").Value))

            Me.lblOrdemServicoAtiva.Text =
            $"Projeto: {OrdemServico.Projeto} - Tag: {OrdemServico.Tag} - OS: {OrdemServico.IdOrdemServico}"
        Catch ex As Exception
            ' Se algo deu errado, evita deixar lixo no Id
            OrdemServico.IdOrdemServico = 0
            Debug.WriteLine("CarregarItemsOs: " & ex.Message)
        Finally
            ' Dispara a atualização da outra grid (se é isso que você precisa)
            TimerDGVListaMaterialSW.Enabled = True
            Cursor.Current = Cursors.Default
        End Try

        Return Nothing ' (mantendo assinatura como Function, sem alterar)
    End Function

    Private Sub btnInserirnaOS_Click(sender As Object, e As EventArgs) Handles btnInserirnaOS.Click

        Try



            If ValidacaoItens() = False Then

                Exit Sub

            End If

            Try


                If OrdemServico.IdOrdemServico = 0 Or OrdemServico.IdOrdemServico = Nothing Or OrdemServico.IdOrdemServico = "" Then
                    MsgBox("Nenhuma Ordem de Serviço Ativa para Inserção de Itens!", vbCritical, "Atenção")
                    Exit Sub
                End If

                If OrdemServico.Liberado_Engenharia.ToString <> "" Then
                    MsgBox("Ordem de Serviço " & OrdemServico.IdOrdemServico & " já Liberada para Produção, não pode mais ser modificada!", vbCritical, "Atenção")
                    Exit Sub
                End If

                If dgvDadosPecas.Rows.Count <= 0 Then

                    Exit Sub

                End If

                Cursor.Current = Cursors.WaitCursor

                ' Dim verificaRnc As Boolean = False

                Dim result As DialogResult = MessageBox.Show("Deseja Realmente Inserir os itens da lista BOM na OS: " & Me.lblOrdemServicoAtiva.Text, "Inserção de Itens da OS", MessageBoxButtons.YesNo)

                If result = DialogResult.Yes Then
                    ' Parar timers para evitar que fechem a conexão no meio do processo (race condition)
                    timerAtualizacao.Stop()
                    TimerDGVListaMaterialSW.Stop()
                    Timerdgvos.Stop()
                    TimerdgvProcessoMaterial.Stop()
                    TimerdgvProcesso.Stop()
                    TimerdgvMateriaisProtheus.Stop()
                    TimerManufaturada.Stop()
                    TimerdgvGabaritos.Stop()

                    '    Try
                    Dim rnc As String
                    Dim Fator As Double

                    Try
                        '            ' Recupera o valor do saldo da tag, com fallback padrão
                        '            ' Nota: RetornaCampoDaPesquisa agora é seguro pois timers estão parados e usa contador
                        '            cl_BancoDados.RetornaCampoDaPesquisa(
                        '"SELECT SaldoTag FROM " & ComplementoTipoBanco & " tags WHERE IdProjeto = '" & OrdemServico.idProjeto & "' AND IdTag = '" & OrdemServico.idTag & "'",
                        '"SaldoTag")

                        '            Dim resultado As Object = VCampo0
                        '            OrdemServico.Fator = If(resultado IsNot Nothing AndAlso IsNumeric(resultado), Convert.ToDouble(resultado), 1.0)
                        '        Catch ex As Exception
                        OrdemServico.Fator = 1
                        ' End Try

                        ' Solicita ao usuário o valor do fator de multiplicação
                        Dim inputFator As String = InputBox(
                            "Informe o Fator de Multiplicação para fabricação:" & vbCrLf &
                            "O valor padrão é a quantidade solicitada pelo PCP no ato do cadastro da Tag." & vbCrLf & vbCrLf &
                            "Exemplo: 1 = 1x, 2 = 2x, 1.5 = 1.5x",
                            "SINCO - Fator de Multiplicação",
                            OrdemServico.Fator
                        )

                        ' Se o usuário clicou em Cancelar
                        If String.IsNullOrWhiteSpace(inputFator) Then
                            MsgBox("Operação cancelada pelo usuário.", vbInformation, "SINCO")
                            Exit Sub
                        End If

                        ' Valida se é um número válido (permite decimais)
                        Dim fatorDigitado As Double
                        If Not Double.TryParse(inputFator.Replace(",", "."), Globalization.NumberStyles.Any, Globalization.CultureInfo.InvariantCulture, fatorDigitado) _
                        OrElse fatorDigitado <= 0 Then
                            MsgBox("O Fator de Multiplicação deve ser um número maior que zero.", vbCritical, "Atenção")
                            Exit Sub
                        End If

                        ' Atribui valor validado
                        OrdemServico.Fator = fatorDigitado

                        ' ============================================================
                        ' 🔹 Processa cada linha do DataGridView
                        ' ============================================================
                        ProgressBar1.Value = 0
                        ProgressBar1.Maximum = dgvDadosPecas.Rows.Count
                        ProgressBar1.Visible = True
                        ProgressBar1.Minimum = 0

                        cl_BancoDados.AbrirBanco()

                        For b As Integer = 0 To dgvDadosPecas.Rows.Count - 1
                            Try
                                If dgvDadosPecas.Rows(b).IsNewRow Then Continue For

                                ProgressBar1.Value = b + 1
                                Me.lblProgresso.Text = "Inserindo: " & (b + 1) & " de " & dgvDadosPecas.Rows.Count

                                ' Define fator
                                DadosArquivoCorrente.Fator = OrdemServico.Fator

                                ' 🔸 Lê e converte valores das colunas
                                Try
                                    DadosArquivoCorrente.Massa = CDbl(If(String.IsNullOrWhiteSpace(dgvDadosPecas.Rows(b).Cells("Mass").Value?.ToString()), "0", dgvDadosPecas.Rows(b).Cells("Mass").Value.ToString().Replace(".", ",")))
                                Catch
                                    DadosArquivoCorrente.Massa = 0
                                End Try

                                Try
                                    DadosArquivoCorrente.qtde = CDbl(If(String.IsNullOrWhiteSpace(dgvDadosPecas.Rows(b).Cells("Qtde").Value?.ToString()), "0", dgvDadosPecas.Rows(b).Cells("Qtde").Value.ToString().Replace(".", ",")))
                                Catch
                                    DadosArquivoCorrente.qtde = 0
                                End Try

                                Try
                                    DadosArquivoCorrente.AreaPintura = CDbl(If(String.IsNullOrWhiteSpace(dgvDadosPecas.Rows(b).Cells("Área_de_superfície").Value?.ToString()), "0", dgvDadosPecas.Rows(b).Cells("Área_de_superfície").Value.ToString().Replace(".", ",")))
                                Catch
                                    DadosArquivoCorrente.AreaPintura = 0
                                End Try

                                Dim qtdeDaArvore As Double = 0
                                Try
                                    qtdeDaArvore = CDbl(If(String.IsNullOrWhiteSpace(dgvDadosPecas.Rows(b).Cells("QtdeTotal").Value?.ToString()), "0", dgvDadosPecas.Rows(b).Cells("QtdeTotal").Value.ToString().Replace(".", ",")))
                                Catch
                                    qtdeDaArvore = 0
                                End Try

                                ' Cálculos de totais
                                DadosArquivoCorrente.qtdeTotal = qtdeDaArvore * DadosArquivoCorrente.Fator
                                DadosArquivoCorrente.PesoTotal = DadosArquivoCorrente.Massa * DadosArquivoCorrente.qtdeTotal
                                DadosArquivoCorrente.AreaPinturaTotal = DadosArquivoCorrente.AreaPintura * DadosArquivoCorrente.qtdeTotal

                                ' Atualiza o DataGridView
                                dgvDadosPecas.Rows(b).Cells("qtdeTotal").Value = Math.Round(DadosArquivoCorrente.qtdeTotal, 2)
                                dgvDadosPecas.Rows(b).Cells("PesoTotal").Value = Math.Round(DadosArquivoCorrente.PesoTotal, 4)
                                dgvDadosPecas.Rows(b).Cells("AreaPinturaTotal").Value = Math.Round(DadosArquivoCorrente.AreaPinturaTotal, 4)
                                dgvDadosPecas.Rows(b).Cells("Fator").Value = Math.Round(DadosArquivoCorrente.Fator, 4)

                                OrdemServico.nivel = dgvDadosPecas.Rows(b).Cells("Nivel").Value.ToString()

                                ' Nome e caminho
                                DadosArquivoCorrente.NomeArquivoSemExtensao = If(dgvDadosPecas.Rows(b).Cells("Arquivo").Value Is Nothing, "", dgvDadosPecas.Rows(b).Cells("Arquivo").Value.ToString())
                                DadosArquivoCorrente.EnderecoArquivo = If(dgvDadosPecas.Rows(b).Cells("CaminhoCompleto").Value Is Nothing, "", dgvDadosPecas.Rows(b).Cells("CaminhoCompleto").Value.ToString())
                                DadosArquivoCorrente.Titulo = If(dgvDadosPecas.Rows(b).Cells("Título").Value Is Nothing, "", dgvDadosPecas.Rows(b).Cells("Título").Value.ToString())
                                DadosArquivoCorrente.AssuntoSubiTitulo = If(dgvDadosPecas.Rows(b).Cells("Assunto").Value Is Nothing, "", dgvDadosPecas.Rows(b).Cells("Assunto").Value.ToString())

                                Dim comentario As String = If(dgvDadosPecas.Rows(b).Cells("Coment").Value Is Nothing, "", dgvDadosPecas.Rows(b).Cells("Coment").Value.ToString())
                                Dim palavras As String = If(dgvDadosPecas.Rows(b).Cells("Palavras-").Value Is Nothing, "", dgvDadosPecas.Rows(b).Cells("Palavras-").Value.ToString())
                                DadosArquivoCorrente.Acabamento = $"{comentario}-{palavras}".Trim("-"c)
                                DadosArquivoCorrente.Author = If(dgvDadosPecas.Rows(b).Cells("Autor").Value Is Nothing, "", dgvDadosPecas.Rows(b).Cells("Autor").Value.ToString())
                                DadosArquivoCorrente.material = If(dgvDadosPecas.Rows(b).Cells("Material").Value Is Nothing, "", dgvDadosPecas.Rows(b).Cells("Material").Value.ToString())
                                DadosArquivoCorrente.TipoDesenho = If(dgvDadosPecas.Rows(b).Cells("Tipo de Desenho").Value Is Nothing, "", dgvDadosPecas.Rows(b).Cells("Tipo de Desenho").Value.ToString())

                                Dim toDouble As Func(Of Object, Double) = Function(v)
                                                                              If v Is Nothing Then Return 0
                                                                              Dim s = v.ToString().Trim().Replace(".", ",")
                                                                              Dim d As Double
                                                                              If Double.TryParse(s, Globalization.NumberStyles.Any, Globalization.CultureInfo.GetCultureInfo("pt-BR"), d) Then Return d
                                                                              Return 0
                                                                          End Function

                                DadosArquivoCorrente.ComprimentoBlank = toDouble(dgvDadosPecas.Rows(b).Cells("CutSizeX").Value).ToString("0.##")
                                DadosArquivoCorrente.LarguraBlank = toDouble(dgvDadosPecas.Rows(b).Cells("CutSizeY").Value).ToString("0.##")
                                DadosArquivoCorrente.Espessura = toDouble(dgvDadosPecas.Rows(b).Cells("Thickness").Value).ToString("0.##")

                                Dim query As String = "INSERT INTO " & ComplementoTipoBanco & " ordemservicoitem (IdOrdemServico, IdProjeto, Projeto, IdTag, Tag, Estatus_OrdemServico, DescResumo, DescDetal, Autor, PalavraChave, Notas, Espessura, AreaPintura, NumeroDobras, Peso, Unidade, UnidadeSW, Altura, Largura, CodMatFabricante, DtCad, UsuarioCriacao, UsuarioAlteracao, DtAlteracao, EnderecoArquivo, MaterialSW, QtdeTotal, CriadoPor, DataCriacao, Estatus, Acabamento, D_E_L_E_T_E, Fator, Qtde, txtSoldagem, txtTipoDesenho, txtCorte, txtDobra, txtSolda, txtPintura, txtMontagem, ComprimentoCaixaDelimitadora, LarguraCaixaDelimitadora, EspessuraCaixaDelimitadora, AreaPinturaUnitario, PesoUnitario, txtItemEstoque, DataPrevisao, sttxtCorte, sttxtDobra, sttxtSolda, sttxtPintura, sttxtMontagem, descempresa, IdEmpresa, DescTag, nivel) " &
                                                      "VALUES (@IdOrdemServico, @idProjeto, @Projeto, @idTag, @Tag, @ESTATUS_OrdemServico, @DescResumo, @DescDetal, @Autor, @PalavraChave, @Notas, @Espessura, @AreaPintura, @NumeroDobras, @Peso, @Unidade, @UnidadeSW, @Altura, @Largura, @CodMatFabricante, @DtCad, @UsuarioCriacao, @UsuarioAlteracao, @DtAlteracao, @EnderecoArquivo, @MaterialSW, @QtdeTotal, @CriadoPor, @DataCriacao, @Estatus, @Acabamento, @D_E_L_E_T_E, @fator, @qtde, @txtSoldagem, @txtTipoDesenho, @txtCorte, @txtDobra, @txtSolda, @txtPintura, @txtMontagem, @ComprimentoCaixaDelimitadora, @LarguraCaixaDelimitadora, @EspessuraCaixaDelimitadora, @AreaPinturaUnitario, @PesoUnitario, @txtItemEstoque, @DataPrevisao, @sttxtCorte, @sttxtDobra, @sttxtSolda, @sttxtPintura, @sttxtMontagem, @descempresa, @IdEmpresa, @DescTag, @nivel)"

                                Using command As New MySqlCommand(query, myconect)
                                    ' Limpar parâmetros para garantir que não haja sobras de iterações anteriores (apesar do New, por segurança extrema)
                                    command.Parameters.Clear()

                                    command.Parameters.AddWithValue("@IdOrdemServico", OrdemServico.IdOrdemServico)
                                    command.Parameters.AddWithValue("@idProjeto", OrdemServico.idProjeto)
                                    command.Parameters.AddWithValue("@Projeto", OrdemServico.Projeto)
                                    command.Parameters.AddWithValue("@idTag", OrdemServico.idTag)
                                    command.Parameters.AddWithValue("@Tag", OrdemServico.Tag)
                                    command.Parameters.AddWithValue("@ESTATUS_OrdemServico", OrdemServico.Estatus)
                                    command.Parameters.AddWithValue("@DescResumo", DadosArquivoCorrente.Titulo)
                                    command.Parameters.AddWithValue("@DescDetal", DadosArquivoCorrente.AssuntoSubiTitulo)
                                    command.Parameters.AddWithValue("@Autor", DadosArquivoCorrente.Author)
                                    command.Parameters.AddWithValue("@PalavraChave", "")
                                    command.Parameters.AddWithValue("@Notas", "")
                                    command.Parameters.AddWithValue("@Espessura", DadosArquivoCorrente.Espessura)
                                    command.Parameters.AddWithValue("@AreaPintura", DadosArquivoCorrente.AreaPinturaTotal)
                                    command.Parameters.AddWithValue("@NumeroDobras", 0)
                                    command.Parameters.AddWithValue("@Peso", DadosArquivoCorrente.PesoTotal.ToString().Replace(",", "."))

                                    'cl_BancoDados.RetornaCampoDaPesquisa("Select unidade from material where CodMatFabricante = '" & DadosArquivoCorrente.NomeArquivoSemExtensao & "'", "unidade")
                                    command.Parameters.AddWithValue("@Unidade", "UN")
                                    command.Parameters.AddWithValue("@UnidadeSW", "UN")
                                    command.Parameters.AddWithValue("@Altura", "")
                                    command.Parameters.AddWithValue("@Largura", "")
                                    command.Parameters.AddWithValue("@CodMatFabricante", DadosArquivoCorrente.NomeArquivoSemExtensao)
                                    command.Parameters.AddWithValue("@DtCad", Date.Now)
                                    command.Parameters.AddWithValue("@UsuarioCriacao", Usuario.NomeCompleto)
                                    command.Parameters.AddWithValue("@UsuarioAlteracao", "")
                                    command.Parameters.AddWithValue("@DtAlteracao", DBNull.Value)
                                    command.Parameters.AddWithValue("@EnderecoArquivo", DadosArquivoCorrente.EnderecoArquivo)
                                    command.Parameters.AddWithValue("@MaterialSW", DadosArquivoCorrente.material)
                                    command.Parameters.AddWithValue("@QtdeTotal", DadosArquivoCorrente.qtdeTotal.ToString().Replace(",", "."))
                                    command.Parameters.AddWithValue("@CriadoPor", Usuario.NomeCompleto)
                                    command.Parameters.AddWithValue("@DataCriacao", Date.Now)
                                    command.Parameters.AddWithValue("@Estatus", "A")
                                    command.Parameters.AddWithValue("@Acabamento", DadosArquivoCorrente.Acabamento)
                                    command.Parameters.AddWithValue("@D_E_L_E_T_E", "")
                                    command.Parameters.AddWithValue("@fator", OrdemServico.Fator.ToString().Replace(",", "."))
                                    command.Parameters.AddWithValue("@qtde", DadosArquivoCorrente.qtde.ToString().Replace(",", "."))
                                    command.Parameters.AddWithValue("@txtSoldagem", DadosArquivoCorrente.soldagem)
                                    command.Parameters.AddWithValue("@txtTipoDesenho", DadosArquivoCorrente.TipoDesenho)
                                    command.Parameters.AddWithValue("@txtCorte", If(DadosArquivoCorrente.Espessura <> "" And DadosArquivoCorrente.material <> "", "1", ""))
                                    command.Parameters.AddWithValue("@txtDobra", "")
                                    command.Parameters.AddWithValue("@txtSolda", "")
                                    command.Parameters.AddWithValue("@txtPintura", "")
                                    command.Parameters.AddWithValue("@txtMontagem", "")
                                    command.Parameters.AddWithValue("@ComprimentoCaixaDelimitadora", "")
                                    command.Parameters.AddWithValue("@LarguraCaixaDelimitadora", "")
                                    command.Parameters.AddWithValue("@EspessuraCaixaDelimitadora", "")
                                    command.Parameters.AddWithValue("@AreaPinturaUnitario", DadosArquivoCorrente.AreaPintura.ToString().Replace(",", "."))
                                    command.Parameters.AddWithValue("@PesoUnitario", DadosArquivoCorrente.Massa.ToString().Replace(",", "."))
                                    command.Parameters.AddWithValue("@txtItemEstoque", DadosArquivoCorrente.ItemEstoque)
                                    command.Parameters.AddWithValue("@sttxtCorte", "")
                                    command.Parameters.AddWithValue("@sttxtDobra", "")
                                    command.Parameters.AddWithValue("@sttxtSolda", "")
                                    command.Parameters.AddWithValue("@sttxtPintura", "")
                                    command.Parameters.AddWithValue("@sttxtMontagem", "")
                                    command.Parameters.AddWithValue("@descempresa", OrdemServico.DescEmpresa)
                                    command.Parameters.AddWithValue("@IdEmpresa", OrdemServico.idempresa)
                                    command.Parameters.AddWithValue("@DescTag", OrdemServico.DescTag)
                                    command.Parameters.AddWithValue("@nivel", OrdemServico.nivel)
                                    command.Parameters.AddWithValue("@DataPrevisao", If(OrdemServico.DataPrevisao IsNot Nothing, OrdemServico.DataPrevisao, DBNull.Value))

                                    '  cl_BancoDados.AbrirBanco()


                                    command.ExecuteNonQuery()




                                End Using

                                ' Obtém o ID e copia processos
                                Dim idGerado As Integer = Convert.ToInt32(New MySqlCommand("SELECT LAST_INSERT_ID()", myconect).ExecuteScalar())
                                If idGerado > 0 Then CopiarProcessosDoMaterialParaOSItem(DadosArquivoCorrente.NomeArquivoSemExtensao, idGerado, DadosArquivoCorrente.qtdeTotal)

                                dgvDadosPecas.Rows(b).Cells("Nivel").Style.BackColor = Color.LightSeaGreen

                                ' cl_BancoDados.FecharBanco()

                            Catch ex As Exception
                                Debug.WriteLine("Erro na linha " & b & ": " & ex.Message)
                            End Try
                        Next

                        MsgBox("Itens Inseridos com Sucesso na Ordem de Serviço: " & OrdemServico.IdOrdemServico, vbInformation, "SINCO")

                    Catch ex As Exception
                        MsgBox("Erro ao processar inserção: " & ex.Message, vbCritical, "Erro")
                    Finally
                        ' Reativar timers e limpar UI
                        timerAtualizacao.Start()
                        Timerdgvos.Enabled = True
                        TimerdgvProcessoMaterial.Enabled = True
                        TimerdgvProcesso.Enabled = True
                        TimerdgvMateriaisProtheus.Enabled = True
                        TimerManufaturada.Enabled = True
                        TimerdgvGabaritos.Enabled = True

                        ProgressBar1.Visible = False
                        lblProgresso.Text = ""
                        cl_BancoDados.FecharBanco()
                        Cursor.Current = Cursors.Default
                    End Try
                End If



            Catch ex As Exception
            Finally
            End Try



        Catch ex As Exception

        Finally

        End Try

    End Sub


    Private Function ValidacaoItens() As Boolean

        ValidacaoItens = True

        ProgressBar1.Visible = True

        ProgressBar1.Maximum = dgvDadosPecas.Rows.Count - 1

        ProgressBar1.Minimum = 0


        For b As Integer = 0 To dgvDadosPecas.Rows.Count - 1



            If dgvDadosPecas.Rows(b).Cells("RNC").Value.ToString <> "" Then

                dgvDadosPecas.Rows(b).Cells("RNC").Style.BackColor = Color.LightSkyBlue
                ValidacaoItens = False

            End If


            If dgvDadosPecas.Rows(b).Cells("Tipo de Desenho").Value.ToString() = "" Then

                dgvDadosPecas.Rows(b).Cells("Tipo de Desenho").Style.BackColor = Color.LightSkyBlue
                dgvDadosPecas.Rows(b).Cells("Tipo de Desenho").Value = "Obrigatorio"
                ValidacaoItens = False

            End If



            If dgvDadosPecas.Rows(b).Cells("Tipo de Desenho").Value.ToString = "CHAPARIA" And
                                     (dgvDadosPecas.Rows(b).Cells("Material").Value.ToString() = "" Or dgvDadosPecas.Rows(b).Cells("Thickness").Value.ToString() = "") Then

                If dgvDadosPecas.Rows(b).Cells("Material").Value.ToString() = "" Then
                    dgvDadosPecas.Rows(b).Cells("Material").Style.BackColor = Color.LightSkyBlue
                    dgvDadosPecas.Rows(b).Cells("Material").Value = "Sem tem chaparia o Material e Obrigatorio"

                End If

                If dgvDadosPecas.Rows(b).Cells("Thickness").Value.ToString() = "" Then
                    dgvDadosPecas.Rows(b).Cells("Thickness").Style.BackColor = Color.LightSkyBlue
                    dgvDadosPecas.Rows(b).Cells("Thickness").Value = "Sem tem chaparia o Material e Obrigatorio"

                End If


                'dgvDadosPecas.Rows(b).DefaultCellStyle.BackColor = Color.LightSalmon
                ValidacaoItens = False

            End If


            If dgvDadosPecas.Rows(b).Cells("Tipo de Desenho").Value.ToString <> "MATERIAL" Then

                'cl_BancoDados.RetornaCampoDaPesquisa("Select codmatfabricante from material where codmatfabricante = '" & dgvDadosPecas.Rows(b).Cells("Arquivo").Value.ToString() & "'", "codmatfabricante")
                'If VCampo0.ToString = "" Then
                '    dgvDadosPecas.Rows(b).Cells("Arquivo").Style.BackColor = Color.LightSkyBlue
                '    ValidacaoItens = False
                'End If

                cl_BancoDados.RetornaCampoDaPesquisa("Select count(codmatfabricante) as qtdeProcesso, codmatfabricante from material_processo where codmatfabricante = '" & dgvDadosPecas.Rows(b).Cells("Arquivo").Value.ToString() & "'", "qtdeProcesso", "codmatfabricante")
                If VCampo0 = 0 Then
                    dgvDadosPecas.Rows(b).Cells("Assunto").Style.BackColor = Color.LightSkyBlue
                    dgvDadosPecas.Rows(b).Cells("Assunto").Value = "Sem Processo"
                    ValidacaoItens = False
                End If

                If VCampo1.ToString = "" Then
                    dgvDadosPecas.Rows(b).Cells("Arquivo").Style.BackColor = Color.LightSkyBlue
                    ValidacaoItens = False
                End If

            End If


            ProgressBar1.Value = b

            Me.lblProgresso.Text = "Processado arquivo: " & dgvDadosPecas.Rows(b).Cells("Arquivo").Value.ToString

        Next

        ProgressBar1.Value = 0
        Me.lblProgresso.Text = ""
        ProgressBar1.Visible = False



    End Function

    Private Sub CopiarProcessosDoMaterialParaOSItem(codMatFabricante As String, idOSItem As Integer, qtdePrevista As Decimal)
        Try
            ' 🔹 Obter o IdMaterial a partir do código
            Dim idMaterial As Integer = ObterIdMaterialPorCodigo(codMatFabricante)
            If idMaterial = 0 Then
                MsgBox("⚠️ Nenhum material encontrado para o código: " & codMatFabricante, vbExclamation, "Aviso")
                Exit Sub
            End If

            '  cl_BancoDados.AbrirBanco()

            ' 🔹 Buscar os processos padrão do material
            Dim sqlBusca As String = "SELECT IdProcesso, SequenciaExecucao FROM material_processo WHERE IdMaterial = @IdMat ORDER BY SequenciaExecucao"
            Dim cmdBusca As New MySqlCommand(sqlBusca, myconect)
            cmdBusca.Parameters.AddWithValue("@IdMat", idMaterial)

            Dim reader As MySqlDataReader = cmdBusca.ExecuteReader()

            Dim processos As New List(Of Tuple(Of Integer, Integer))
            While reader.Read()
                processos.Add(New Tuple(Of Integer, Integer)(reader("IdProcesso"), reader("SequenciaExecucao")))
            End While
            reader.Close()

            ' 🔹 Inserir cada processo vinculado ao item da OS
            For Each proc In processos
                Dim sqlInsert As String = "
                INSERT INTO ordemservicoitem_processo
                    (IdOrdemServicoItem, IdProcesso, SequenciaExecucao, QtdePrevista, QtdeProduzida, Situacao)
                VALUES
                    (@IdOSItem, @IdProcesso, @Seq, @QtdePrevista, 0, 'PENDENTE');"

                Using cmdInsert As New MySqlCommand(sqlInsert, myconect)
                    cmdInsert.Parameters.AddWithValue("@IdOSItem", idOSItem)
                    cmdInsert.Parameters.AddWithValue("@IdProcesso", proc.Item1)
                    cmdInsert.Parameters.AddWithValue("@Seq", proc.Item2)
                    cmdInsert.Parameters.AddWithValue("@QtdePrevista", qtdePrevista)
                    cmdInsert.ExecuteNonQuery()
                End Using
            Next

            ' cl_BancoDados.FecharBanco()

        Catch ex As Exception
            MsgBox("Erro ao copiar processos para o código " & codMatFabricante & ": " & ex.Message, vbCritical)
        End Try
    End Sub


    Private Function ObterIdMaterialPorCodigo(codMatFabricante As String) As Integer
        Try
            Dim sql As String = "SELECT IdMaterial FROM material WHERE CodMatFabricante = @Cod"
            Using cmd As New MySqlCommand(sql, myconect)
                cmd.Parameters.AddWithValue("@Cod", codMatFabricante)
                Dim result = cmd.ExecuteScalar()
                If result IsNot Nothing Then
                    Return Convert.ToInt32(result)
                Else
                    Return 0
                End If
            End Using
        Catch ex As Exception
            MsgBox("Erro ao obter IdMaterial do código " & codMatFabricante & ": " & ex.Message, vbCritical)
            Return 0
        End Try
    End Function

    Private Sub AbrirPastaDaOrdemDeServiçoToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AbrirPastaDaOrdemDeServiçoToolStripMenuItem.Click
        Try
            ' Verifica se a célula "Endereco" não está vazia ou nula
            If IsNothing(dgvos.CurrentRow.Cells("Endereco").Value) OrElse IsDBNull(dgvos.CurrentRow.Cells("Endereco").Value) Then
                MsgBox("O endereço não foi informado!", vbExclamation, "Atenção")
                Exit Sub
            End If

            ' Obtém o endereço da célula e tenta abrir o Explorer
            OrdemServico.EnderecoOrdemServico = dgvos.CurrentRow.Cells("Endereco").Value.ToString()
            If Not String.IsNullOrWhiteSpace(OrdemServico.EnderecoOrdemServico) Then
                Process.Start("Explorer", OrdemServico.EnderecoOrdemServico)
            Else
                MsgBox("O endereço está vazio ou não foi informado corretamente!", vbExclamation, "Atenção")
            End If
        Catch ex As Exception
            ' Exibe uma mensagem de erro detalhada
            MsgBox($"Ocorreu um erro ao tentar abrir o endereço: {ex.Message}", vbCritical, "Erro")
        Finally
            ' Código opcional que pode ser executado mesmo após uma exceção
            ' (e.g., limpar variáveis, liberar recursos)
        End Try
    End Sub

    Private Sub LiberarOrdemDeServiçoParaProduçãoToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles LiberarOrdemDeServiçoParaProduçãoToolStripMenuItem.Click

        If Usuario.NomeCompleto = "" Then
            MsgBox("Você não está logado com um usuario valido, não e possivel cria um OS", vbCritical, "Atenção")
            Exit Sub
        End If

        If OrdemServico.Liberado_Engenharia.ToString <> "" Then
            MsgBox("A OS já está liberada!", vbCritical, "Atenção")
            Exit Sub
        End If


        ' Verifica se o usuário clicou em "Cancelar" (Fator será uma string vazia)
        If OrdemServico.Fator <= 0 Or OrdemServico.Fator = Nothing Then

            MsgBox("A operação foi cancelada, pois o fator está definido como ''0''. Altere o valor do fator para liberar a OS.", vbInformation, "Atenação")

            Exit Sub ' Sai do procedimento

        End If

        cl_BancoDados.VerificaSaldoTag(OrdemServico.idTag)
        Dim totalItens As Integer
        Dim txtacabamento As String
        Dim dxf As String
        Dim espessura As String
        Dim material As String
        Dim Valido As Boolean = True
        Dim txtTipoDesenho As String
        Dim principal As String

        ' totalItens = Convert.ToInt32(cl_BancoDados.RetornaCampoDaPesquisa("Select Max(IdOrdemServico) as IdOrdemServico from ordemservicoitem where idordemservico = '" & OrdemServico.IdOrdemServico & "')", "IdOrdemServico"))

        If DGVListaMaterialSW.Rows.Count <= 0 Then
            MsgBox("Não há itens inseridos da Ordem de Serviço", vbInformation, "Atenção")
            Exit Sub
        ElseIf DGVListaMaterialSW.Rows.Count >= 0 Then
            ProgressBarProcessoLiberacaoOrdemServico.Minimum = 0

            ProgressBarProcessoLiberacaoOrdemServico.Maximum = DGVListaMaterialSW.Rows.Count - 1

            For i As Integer = 0 To DGVListaMaterialSW.Rows.Count - 1

                '''
                 'Verifica se há um produto principal para Ordem de serviço
                Try

                    principal = DGVListaMaterialSW.Rows(i).Cells("ProdutoPrincipal").Value.ToString

                    If principal = "SIM" Then
                        '  principal = DGVListaMaterialSW.Rows(i).Cells("ProdutoPrincipal").Value.ToString
                        ProdutoPrincipal = DGVListaMaterialSW.Rows(i).Cells("CodMatFabricante").Value.ToString

                        Exit For

                    ElseIf principal = "" Then

                        principal = ""
                        ProdutoPrincipal = ""
                        MsgBox("Para gerar a Lista de material e obrigatorio que o produto 'Principal'da Ordem de Serviço já esteja selecionado.", vbInformation, "Atenção")
                        Exit Sub
                    End If
                Catch ex As Exception

                    principal = ""
                    ProdutoPrincipal = ""
                    MsgBox("Para gerar a Lista de material e obrigatorio que o produto 'Principal'da Ordem de Serviço já esteja selecionado.", vbInformation, "Atenção")

                    Exit Sub

                End Try

                Try
                    dxf = Path.ChangeExtension(DGVListaMaterialSW.Rows(i).Cells("EnderecoArquivo").Value.ToString, ".dxf")
                Catch ex As Exception
                    dxf = ""
                End Try

                Try
                    espessura = DGVListaMaterialSW.Rows(i).Cells("espessura").Value.ToString
                Catch ex As Exception
                    espessura = ""
                End Try

                Try
                    material = DGVListaMaterialSW.Rows(i).Cells("materialsw").Value.ToString
                Catch ex As Exception
                    material = ""
                End Try

                If espessura <> "" And material <> "" And dxf <> "" Then
                    ' Verifica se o arquivo DXF existe
                    If File.Exists(dxf) = False Then
                        DGVListaMaterialSW.Rows(i).DefaultCellStyle.BackColor = Color.LightSalmon
                        Valido = False
                    End If

                End If

                ProgressBarProcessoLiberacaoOrdemServico.Value = i

            Next

            If Valido = False Then

                MsgBox("Itens marcados em rosa indicam que essas peças deveriam ter o respectivo DFx vinculado ao seu desenho.", vbInformation, "Atenção.")
            End If

        End If
        ProgressBarProcessoLiberacaoOrdemServico.Value = 0

        If My.Settings.chkAcabamentoObrigatorio = "SIM" Then

            For i As Integer = 0 To DGVListaMaterialSW.Rows.Count - 1
                'Verifica se há acabamento indicado para as peças
                Try
                    txtacabamento = DGVListaMaterialSW.Rows(i).Cells("Acabamento").Value.ToString
                Catch ex As Exception
                    txtacabamento = ""
                End Try

                Try
                    txtTipoDesenho = DGVListaMaterialSW.Rows(i).Cells("txtTipoDesenho").Value.ToString
                Catch ex As Exception
                    txtTipoDesenho = ""
                End Try

                If txtacabamento = "" And txtTipoDesenho <> "Material" Then

                    DGVListaMaterialSW.Rows(i).DefaultCellStyle.BackColor = Color.LightSalmon
                End If

            Next

            MsgBox("Há itens na OS que devem ter seus 'Acabamentos' preenchido, favor verificar os itens destacados!", vbInformation, "Atenção")

            Exit Sub

        End If

        Cursor.Current = Cursors.WaitCursor
        '
        Try

            txtPesqNumeroDesenho.Clear()

            txtPesqTipoDesenho.Clear()

            txtPesqAcabamentoDesenho.Clear()

            '  TimerDGVListaMaterialSW.Enabled = True

            ' Verifica se há uma linha selecionada no DataGridView
            If dgvos.CurrentRow IsNot Nothing Then

                frmOpcaoLiberacaoOrdemServico.ShowDialog()

                If TipoLiberacaoOrdemServico.ToString() = "Total" Then

                    If OrdemServico.SaldoTag < OrdemServico.Fator Then

                        MsgBox("A operação foi cancelada. O valor informado é maior que o saldo disponível,
                    altere o fator multiplicador e ou solicite ao PCP para inserir saldo na Tag! o Valor Multiplicador é :" & OrdemServico.Fator &
                    " é o saldo da tag é : " & OrdemServico.SaldoTag & "!", vbInformation, "Atenção")

                        Exit Sub

                    End If

                    If OrdemServico.QtdeTag >= OrdemServico.Fator + OrdemServico.QtdeLiberada Then



                        ' Verifica se o valor inserido é numérico e maior que 0
                        If Not IsNumeric(OrdemServico.Fator) OrElse Convert.ToDouble(OrdemServico.Fator) <= 0 Then

                            MsgBox("A operação foi cancelada. O valor informado não é um número válido!", vbInformation, "Atenação")

                            Exit Sub
                            'End If

                        End If

                        '						cl_BancoDados.AlteracaoEspecifica("OrdemServico", "Fator", OrdemServico.Fator, "IdOrdemServico", OrdemServico.IdOrdemServico)

                        cl_BancoDados.AlteracaoEspecifica("Tags", "QtdeLiberada", (OrdemServico.QtdeLiberada + OrdemServico.Fator), "IdTag", OrdemServico.idTag)
                        cl_BancoDados.AlteracaoEspecifica("Tags", "SaldoTag", (OrdemServico.SaldoTag - OrdemServico.Fator), "IdTag", OrdemServico.idTag)
                        cl_BancoDados.AlteracaoEspecifica("Ordemservico", "TipoLiberacaoOrdemServico", "Total", "IdOrdemServico", OrdemServico.IdOrdemServico)


                    Else

                        MsgBox("A operação foi cancelada. O valor informado é maior que o saldo disponível!", vbInformation, "Atenação")
                        Exit Sub

                    End If

                ElseIf TipoLiberacaoOrdemServico.ToString() = "Parcial" Then

                    cl_BancoDados.AlteracaoEspecifica("Ordemservico", "TipoLiberacaoOrdemServico", "Parcial", "IdOrdemServico", OrdemServico.IdOrdemServico)

                ElseIf TipoLiberacaoOrdemServico.ToString() = "Sair" Then

                    MsgBox("A operação foi cancelada. O processo de liberação foi cancelada!", vbInformation, "Atenção")

                    Exit Sub

                End If

                ' Verifica e obtém o valor da célula "Estatus"
                If dgvos.CurrentRow.Cells("Estatus") IsNot Nothing AndAlso dgvos.CurrentRow.Cells("Estatus").Value IsNot DBNull.Value Then
                    OrdemServico.Estatus = dgvos.CurrentRow.Cells("Estatus").Value.ToString()
                Else
                    OrdemServico.Estatus = String.Empty ' Valor padrão em caso de ausência
                End If

                ' Verifica e obtém o valor da célula "ENDERECO"
                If dgvos.CurrentRow.Cells("ENDERECO") IsNot Nothing AndAlso dgvos.CurrentRow.Cells("ENDERECO").Value IsNot DBNull.Value Then
                    OrdemServico.EnderecoOrdemServico = dgvos.CurrentRow.Cells("ENDERECO").Value.ToString()
                Else
                    OrdemServico.EnderecoOrdemServico = String.Empty ' Valor padrão em caso de ausência
                End If

                ' Verifica e obtém o valor da célula "Liberado_Engenharia"
                If dgvos.CurrentRow.Cells("Liberado_Engenharia") IsNot Nothing AndAlso dgvos.CurrentRow.Cells("Liberado_Engenharia").Value IsNot DBNull.Value Then
                    OrdemServico.Liberado_Engenharia = dgvos.CurrentRow.Cells("Liberado_Engenharia").Value.ToString()
                Else
                    OrdemServico.Liberado_Engenharia = String.Empty ' Valor padrão em caso de ausência
                End If

                ' Verifica e obtém o valor da célula "Descricao"
                If dgvos.CurrentRow.Cells("Descricao") IsNot Nothing AndAlso dgvos.CurrentRow.Cells("Descricao").Value IsNot DBNull.Value Then
                    OrdemServico.Descricao = dgvos.CurrentRow.Cells("Descricao").Value.ToString()
                Else
                    OrdemServico.Descricao = String.Empty ' Valor padrão em caso de ausência
                End If
            Else
                ' Tratar caso não exista uma linha selecionada
                Throw New Exception("Nenhuma linha selecionada no DataGridView.")
            End If

            If OrdemServico.Liberado_Engenharia = "S" Then

                MsgBox("OS Já liberada, não e possivel liberar novamente!", vbInformation, "Atenção")

                Exit Sub
            Else

                Dim diretorio As String = OrdemServico.EnderecoOrdemServico

                ''Libera pasata para edição
                'cl_BancoDados.PermitirEscritaNaPasta(diretorio & "\PDF")
                'cl_BancoDados.PermitirEscritaNaPasta(diretorio & "\DXF")
                'cl_BancoDados.PermitirEscritaNaPasta(diretorio & "\DFT")

                LimparDiretorio(diretorio & "\PDF")
                LimparDiretorio(diretorio & "\DXF")
                LimparDiretorio(diretorio & "\DFT")
                LimparDiretorio(diretorio & "\LXDS")
                LimparDiretorio(diretorio & "\IGES")

                ImportarLXDSParaOS(DGVListaMaterialSW, "DXF", ProgressBarProcessoLiberacaoOrdemServico, "")
                ImportarLXDSParaOS(DGVListaMaterialSW, "PDF", ProgressBarProcessoLiberacaoOrdemServico, "IdOrdemServicoItem")
                ImportarLXDSParaOS(DGVListaMaterialSW, "DFT", ProgressBarProcessoLiberacaoOrdemServico, "")
                ImportarLXDSParaOS(DGVListaMaterialSW, "LXDS", ProgressBarProcessoLiberacaoOrdemServico, "")
                ImportarLXDSParaOS(DGVListaMaterialSW, "IGES", ProgressBarProcessoLiberacaoOrdemServico, "")

                ' inicio teste query unica 25/09/2025
                cl_BancoDados.Salvar("Update ordemservicoitem set Liberado_Engenharia = 'S',
						Data_Liberacao_Engenharia = '" & Date.Now & "'
						where (D_E_L_E_T_E IS NULL OR D_E_L_E_T_E = '') and IdOrdemServico = '" & OrdemServico.IdOrdemServico & "';
                        Update ordemservico set Liberado_Engenharia = 'S',
						Data_Liberacao_Engenharia = '" & Date.Now & "'
						where (D_E_L_E_T_E IS NULL OR D_E_L_E_T_E = '') and IdOrdemServico = '" & OrdemServico.IdOrdemServico & "';")
                dgvos.CurrentRow.Cells("Liberado_Engenharia").Value = "S"
                dgvos.CurrentRow.Cells("Data_Liberacao_Engenharia").Value = Date.Now
                dgvos.CurrentRow.Cells("dgvStatus").Value = My.Resources.verificado
                ' dgvos.Refresh()

                cl_BancoDados.CalcularordemservicoitemFatorOS_TAG_PROJETO()

                ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

                cl_BancoDados.VerificaSaldoTag(OrdemServico.idTag)

                Me.lbltxtQtdeTag.Text = OrdemServico.QtdeTag
                Me.lbltxtQtdeLiberada.Text = OrdemServico.QtdeLiberada
                Me.lbltxtSaldoTag.Text = OrdemServico.SaldoTag

                Try


                    TemplatesExcel.ExportarOrdemServicoPadrao(DGVListaMaterialSW, ProgressBarProcessoLiberacaoOrdemServico, OrdemServico.EnderecoOrdemServico, Me.txtDescricao.Text.Trim.ToUpper, dgvos, DGVListaMaterialSWMateriais)


                Catch ex As Exception
                Finally

                End Try

                'If Usuario.EnviarEmailLiberacaoOS <> "" Then

                '    Dim resultado As MsgBoxResult = MessageBox.Show("Deseja enviar o e-mail para o PCP, de comunicado de Liberação da Ordem de Serviço: " & OrdemServico.IdOrdemServico, "Liberação", MessageBoxButtons.YesNo)

                '    If resultado = DialogResult.Yes Then

                '        ClasseEmail.EmailLiberacaoOS()

                '    End If

                'End If

            End If

        Catch ex As Exception

        Finally

        End Try

        ''''  Email.EnviarEmailComOs01(OrdemServico.EnderecoOrdemServico, OrdemServico.Descricao, Date.Now)
        ' Retornar o cursor ao normal
        Cursor.Current = Cursors.Default

    End Sub


    'Sub LimparDiretorio(ByVal diretorio As String)
    '    ' Verifica se o diretório existe
    '    If Directory.Exists(diretorio) Then
    '        ' Remove o atributo somente leitura do diretório
    '        Dim dirInfo As New DirectoryInfo(diretorio)
    '        dirInfo.Attributes = dirInfo.Attributes And Not FileAttributes.ReadOnly

    '        ' Limpa todos os arquivos no diretório
    '        For Each arquivo As String In Directory.GetFiles(diretorio)
    '            ' Remove o atributo somente leitura antes de excluir
    '            Dim fileInfo As New FileInfo(arquivo)
    '            fileInfo.Attributes = fileInfo.Attributes And Not FileAttributes.ReadOnly
    '            File.Delete(arquivo)
    '        Next

    '        ' Limpa todos os subdiretórios, exceto aqueles chamados "Projeto"
    '        For Each subdiretorio As String In Directory.GetDirectories(diretorio)
    '            ' Verifica se o nome do subdiretório corresponde a um dos nomes especificados
    '            If Path.GetFileName(subdiretorio).Equals("DXF", StringComparison.OrdinalIgnoreCase) Or
    '           Path.GetFileName(subdiretorio).Equals("PDF", StringComparison.OrdinalIgnoreCase) Or
    '           Path.GetFileName(subdiretorio).Equals("DFT", StringComparison.OrdinalIgnoreCase) Or
    '           Path.GetFileName(subdiretorio).Equals("PUNC", StringComparison.OrdinalIgnoreCase) Or
    '           Path.GetFileName(subdiretorio).Equals("LASER", StringComparison.OrdinalIgnoreCase) Then

    '                ' Remove o atributo somente leitura do subdiretório antes de limpar
    '                Dim subDirInfo As New DirectoryInfo(subdiretorio)
    '                subDirInfo.Attributes = subDirInfo.Attributes And Not FileAttributes.ReadOnly

    '                LimparDiretorio(subdiretorio)
    '            End If
    '        Next

    '    End If
    'End Sub

    Sub LimparDiretorio(ByVal diretorio As String)

        If String.IsNullOrWhiteSpace(diretorio) Then Exit Sub
        If Not Directory.Exists(diretorio) Then Exit Sub

        ' Lista de pastas permitidas para recursão (mesma regra sua)
        Dim pastasPermitidas As New HashSet(Of String)(StringComparer.OrdinalIgnoreCase) From {
        "DXF", "PDF", "DFT", "PUNC", "LASER"
    }

        ' Acumula avisos para mostrar uma vez ao final
        Dim log As New StringBuilder()

        ' Função local: remove atributos problemáticos (quando possível)
        Dim RemoverAtributos As Action(Of FileSystemInfo) =
        Sub(fsi As FileSystemInfo)
            Try
                Dim at As FileAttributes = fsi.Attributes
                at = at And Not FileAttributes.ReadOnly
                at = at And Not FileAttributes.Hidden
                at = at And Not FileAttributes.System
                fsi.Attributes = at
            Catch ex As UnauthorizedAccessException
                log.AppendLine("SEM PERMISSÃO p/ alterar atributos: " & fsi.FullName)
            Catch ex As Exception
                log.AppendLine("Falha ao alterar atributos: " & fsi.FullName & " | " & ex.Message)
            End Try
        End Sub

        ' Função local: tenta deletar arquivo sem quebrar o processo
        Dim DeletarArquivo As Action(Of String) =
        Sub(arquivo As String)
            Try
                Dim fi As New FileInfo(arquivo)
                RemoverAtributos(fi)
                File.Delete(arquivo)
            Catch ex As UnauthorizedAccessException
                log.AppendLine("ACESSO NEGADO ao excluir arquivo: " & arquivo)
            Catch ex As IOException
                log.AppendLine("ARQUIVO EM USO/IO ao excluir: " & arquivo & " | " & ex.Message)
            Catch ex As Exception
                log.AppendLine("Erro ao excluir arquivo: " & arquivo & " | " & ex.Message)
            End Try
        End Sub

        ' Função local recursiva (mantém o comportamento original)
        Dim LimparRec As Action(Of String) = Nothing
        LimparRec =
        Sub(dir As String)

            If Not Directory.Exists(dir) Then Exit Sub

            ' Tenta “destravar” atributos da pasta
            Try
                Dim di As New DirectoryInfo(dir)
                RemoverAtributos(di)
            Catch ex As Exception
                log.AppendLine("Erro ao acessar pasta: " & dir & " | " & ex.Message)
                ' Mesmo assim tenta seguir para arquivos se conseguir listar
            End Try

            ' Limpa arquivos do diretório atual (como sua função original)
            Dim arquivos As String() = {}
            Try
                arquivos = Directory.GetFiles(dir)
            Catch ex As UnauthorizedAccessException
                log.AppendLine("SEM PERMISSÃO p/ listar arquivos: " & dir)
                arquivos = {}
            Catch ex As Exception
                log.AppendLine("Erro ao listar arquivos: " & dir & " | " & ex.Message)
                arquivos = {}
            End Try

            For Each arq In arquivos
                DeletarArquivo(arq)
            Next

            ' Recursão apenas em pastas permitidas (igual sua regra)
            Dim subdirs As String() = {}
            Try
                subdirs = Directory.GetDirectories(dir)
            Catch ex As UnauthorizedAccessException
                log.AppendLine("SEM PERMISSÃO p/ listar subpastas: " & dir)
                subdirs = {}
            Catch ex As Exception
                log.AppendLine("Erro ao listar subpastas: " & dir & " | " & ex.Message)
                subdirs = {}
            End Try

            For Each subdir In subdirs
                Dim nome As String = ""
                Try
                    nome = Path.GetFileName(subdir)
                Catch
                    nome = ""
                End Try

                If pastasPermitidas.Contains(nome) Then
                    LimparRec(subdir)
                End If
            Next

        End Sub

        ' Executa
        LimparRec(diretorio)

        ' Mostra apenas se teve algum problema
        If log.Length > 0 Then
            MessageBox.Show(
            "Alguns itens não puderam ser limpos:" & Environment.NewLine & Environment.NewLine &
            log.ToString(),
            "Limpeza de diretório (avisos)",
            MessageBoxButtons.OK,
            MessageBoxIcon.Warning
        )
        End If

    End Sub

    Private Function ImportarLXDSParaOS(ByVal ObjetoDgv As DataGridView, ByVal Pasta As String, ByVal BarraProgresso As ToolStripProgressBar, ByVal NomeidColuna As String)

        Dim Origem, Destino, Prefixo, MaterialSW, QtdeTotal, Espessura, tipoDesenho, Acabamento As String
        Dim caminhoArquivoDestino As String

        BarraProgresso.Minimum = 0
        BarraProgresso.Maximum = ObjetoDgv.Rows.Count - 1

        Destino = dgvos.CurrentRow.Cells("Endereco").Value.ToString
        Destino = Path.Combine(Destino, Pasta)

        For i As Integer = 0 To ObjetoDgv.Rows.Count - 1

            Try

                Try
                    ' Acessando os valores diretamente da linha
                    Origem = ObjetoDgv.Rows(i).Cells("EnderecoArquivo").Value.ToString
                    Origem = Replace(Origem, ".SLDPRT", "." & Pasta, , , CompareMethod.Text)
                    Origem = Replace(Origem, ".SLDASM", "." & Pasta, , , CompareMethod.Text)
                    Origem = Replace(Origem, ".psm", "." & Pasta, , , CompareMethod.Text)
                    Origem = Replace(Origem, ".asm", "." & Pasta, , , CompareMethod.Text)
                    Origem = Replace(Origem, ".par", "." & Pasta, , , CompareMethod.Text)



                Catch ex As Exception
                    Origem = ""
                Finally
                End Try

                Try
                    MaterialSW = ObjetoDgv.Rows(i).Cells("MaterialSW").Value.ToString
                Catch ex As Exception
                    MaterialSW = "Sem material"
                Finally
                End Try

                Try
                    QtdeTotal = ObjetoDgv.Rows(i).Cells("QtdeTotal").Value.ToString
                Catch ex As Exception
                    QtdeTotal = "Sem Quantidade"
                Finally
                End Try

                Try
                    Espessura = ObjetoDgv.Rows(i).Cells("Espessura").Value.ToString
                Catch ex As Exception
                    Espessura = "Sem Espessura"
                Finally
                End Try

                Try
                    tipoDesenho = ObjetoDgv.Rows(i).Cells("txtTipoDesenho").Value.ToString
                Catch ex As Exception
                    tipoDesenho = "Sem Tipo Desenho"
                Finally
                End Try

                Try
                    Acabamento = ObjetoDgv.Rows(i).Cells("Acabamento").Value.ToString
                Catch ex As Exception
                    Acabamento = "Sem Acabamento"
                Finally
                End Try

                Prefixo = Espessura & " - " & MaterialSW & " - " & QtdeTotal & " - "

                ' Verifica se o arquivo de origem existe
                If File.Exists(Origem) Then

                    ' Obtém o nome do arquivo sem a extensão
                    Dim nomeArquivoSemExtensao As String = Path.GetFileNameWithoutExtension(Origem)

                    ' Obtém a extensão do arquivo
                    Dim extensaoArquivo As String = Path.GetExtension(Origem)

                    Dim novoNomeArquivo As String

                    Dim invalidChars = Path.GetInvalidFileNameChars()
                    Espessura = String.Concat(Espessura.Where(Function(c) Not invalidChars.Contains(c)))

                    ' Dim invalidChars = Path.GetInvalidFileNameChars()
                    MaterialSW = String.Concat(MaterialSW.Where(Function(c) Not invalidChars.Contains(c)))

                    'Dim invalidChars = Path.GetInvalidFileNameChars()
                    QtdeTotal = String.Concat(QtdeTotal.Where(Function(c) Not invalidChars.Contains(c)))

                    ' Dim invalidChars = Path.GetInvalidFileNameChars()
                    nomeArquivoSemExtensao = String.Concat(nomeArquivoSemExtensao.Where(Function(c) Not invalidChars.Contains(c)))

                    ' Dim invalidChars = Path.GetInvalidFileNameChars()
                    extensaoArquivo = String.Concat(extensaoArquivo.Where(Function(c) Not invalidChars.Contains(c)))

                    ' Verifica qual formato de exportação foi selecionado
                    If My.Settings.ParametroExportarDXF = "1" Then
                        novoNomeArquivo = $"OS_{OrdemServico.IdOrdemServico} - {Espessura} - {MaterialSW} - {QtdeTotal} - {nomeArquivoSemExtensao}{extensaoArquivo}"
                    ElseIf My.Settings.ParametroExportarDXF = "2" Then
                        novoNomeArquivo = $"OS_{OrdemServico.IdOrdemServico} -{QtdeTotal} - {nomeArquivoSemExtensao} - {MaterialSW} - {Espessura}{extensaoArquivo}"
                    Else
                        MessageBox.Show("Nenhuma opção de exportação de LXDS selecionada. Vá nas configurações e selecione a opção desejada!")
                        Exit For ' Saída antecipada se não houver configuração válida
                    End If

                    If My.Settings.BancoDadosAtivo = "mettapaineis" Then

                        If ObjetoDgv.Rows(i).Cells("EnderecoArquivo").Value.ToString.IndexOf(".SLDASM", StringComparison.OrdinalIgnoreCase) >= 0 Then
                            novoNomeArquivo = $"CPF_{OrdemServico.IdOrdemServico} - {tipoDesenho} - {QtdeTotal} - {nomeArquivoSemExtensao}{extensaoArquivo}"
                        End If


                    ElseIf My.Settings.BancoDadosAtivo = "metalfisa" Then

                        If ObjetoDgv.Rows(i).Cells("EnderecoArquivo").Value.ToString.IndexOf(".asm", StringComparison.OrdinalIgnoreCase) >= 0 Then
                            novoNomeArquivo = $"OS_{OrdemServico.IdOrdemServico} - {tipoDesenho} - {QtdeTotal} - {nomeArquivoSemExtensao}{extensaoArquivo}"
                        End If

                    Else

                        If ObjetoDgv.Rows(i).Cells("EnderecoArquivo").Value.ToString.IndexOf(".SLDASM", StringComparison.OrdinalIgnoreCase) >= 0 Then
                            novoNomeArquivo = $"OS_{OrdemServico.IdOrdemServico} - {tipoDesenho} - {QtdeTotal} - {nomeArquivoSemExtensao}{extensaoArquivo}"
                        End If

                    End If

                    caminhoArquivoDestino = Path.Combine(Destino, novoNomeArquivo)

                    If Pasta = "PDF" Then

                        Dim id As String

                        If NomeidColuna <> "" Then

                            id = ObjetoDgv.Rows(i).Cells(NomeidColuna).Value

                        End If

                        If My.Settings.BancoDadosAtivo = "metalfisa" Then
                            EditarPDFParaOsFormatoMetalfisa(Origem, caminhoArquivoDestino, novoNomeArquivo, QtdeTotal, OrdemServico.IdOrdemServico, Acabamento, "OS")
                        Else



                            EditarPDFParaOsFormatoLynx(Origem, caminhoArquivoDestino, novoNomeArquivo, QtdeTotal, OrdemServico.IdOrdemServico, Acabamento, "OS")
                        End If

                        cl_BancoDados.AlteracaoEspecifica("OrdemServicoItem", "EnderecoArquivoItemOrdemServico", caminhoArquivoDestino, "IdOrdemServicoItem", id)

                        ObjetoDgv.Rows(i).Cells("EnderecoArquivoItemOrdemServico").Value = caminhoArquivoDestino
                    Else

                        '' Copia o arquivo para o destino diretamente se não for PDF
                        'Origem = Path.GetFullPath(Origem)
                        'caminhoArquivoDestino = Path.GetFullPath(caminhoArquivoDestino)

                        'File.Copy(Origem, caminhoArquivoDestino, True)

                        cl_BancoDados.CopiarArquivoInteligente(Origem, caminhoArquivoDestino)

                    End If

                End If

                Origem = ""
                Prefixo = ""
                MaterialSW = ""
                QtdeTotal = ""
                Espessura = ""
                tipoDesenho = ""
            Catch ex As Exception
                Continue For
            End Try

            BarraProgresso.Value = i

        Next

        BarraProgresso.Value = 0

    End Function


    Public Sub EditarPDFParaOsFormatoLynx(Origem As String, caminhoArquivoDestino As String, novoNomeArquivo As String, QtdeTotal As String, Identificado As String, Acabamento As String, TipoIdentidicado As String)

        Origem = System.Text.RegularExpressions.Regex.Replace(Origem, ".sldprt$|.sldasm$|.asm$|.psm$|.par$", ".pdf", System.Text.RegularExpressions.RegexOptions.IgnoreCase)

        Origem = Path.GetFullPath(Origem)

        If System.IO.File.Exists(Origem) Then
            Try
                Dim pdfReader As New PdfReader(Origem)
                Dim pdfWriter As New PdfWriter(caminhoArquivoDestino)
                Dim pdfDocument As New PdfDocument(pdfReader, pdfWriter)
                Dim page As PdfPage = pdfDocument.GetPage(1)
                Dim canvas As New PdfCanvas(page)

                ' Tamanho da página
                Dim largura As Single = pdfDocument.GetDefaultPageSize().GetWidth()
                Dim altura As Single = pdfDocument.GetDefaultPageSize().GetHeight()

                ' Fonte padrão
                Dim fonte = iText.Kernel.Font.PdfFontFactory.CreateFont()

                canvas.BeginText()
                canvas.SetFontAndSize(fonte, 8)

                ' 🧾 5. Data de emissão no canto inferior direito
                canvas.SetTextMatrix(900, 15)
                canvas.ShowText(Date.Now.Date)

                ' 🧾 3. Qtde no meio da página
                canvas.SetTextMatrix(972, 15)
                canvas.ShowText(QtdeTotal.ToString)

                ' 🧾 2. Identificado no topo direito
                canvas.SetTextMatrix(1022, 15)
                canvas.ShowText(Identificado.ToString)

                ' 🧾 4. Acabamento no canto inferior esquerdo
                canvas.SetTextMatrix(1085, 15)
                canvas.ShowText(Acabamento.ToString)

                ' 🧾 1. TipoIdentificado no topo esquerdo
                canvas.SetTextMatrix(1200, 15)
                canvas.ShowText(TipoIdentidicado.ToString)

                canvas.EndText()

                pdfDocument.Close()
            Catch ex As Exception

                'Origem = Path.GetFullPath(Origem)
                'caminhoArquivoDestino = Path.GetFullPath(caminhoArquivoDestino)

                'File.Delete(caminhoArquivoDestino)
                'File.Copy(Origem, caminhoArquivoDestino, True)

                cl_BancoDados.CopiarArquivoInteligente(Origem, caminhoArquivoDestino)

            End Try
        End If

    End Sub

    Public Sub EditarPDFParaOsFormatoMetalfisa(Origem As String,
                                           caminhoArquivoDestino As String,
                                           novoNomeArquivo As String,
                                           QtdeTotal As String,
                                           Identificado As String,
                                           Acabamento As String,
                                           TipoIdentificado As String)

        Origem = System.Text.RegularExpressions.Regex.Replace(Origem,
        ".sldprt$|.sldasm$|.asm$|.psm$|.par$", ".pdf",
        System.Text.RegularExpressions.RegexOptions.IgnoreCase)

        Origem = Path.GetFullPath(Origem)
        If Not System.IO.File.Exists(Origem) Then Exit Sub

        Try
            Dim pdfReader As New PdfReader(Origem)
            Dim pdfWriter As New PdfWriter(caminhoArquivoDestino)
            Dim pdfDocument As New PdfDocument(pdfReader, pdfWriter)
            Dim page As PdfPage = pdfDocument.GetPage(1)
            Dim canvas As New PdfCanvas(page)
            Dim fonte = PdfFontFactory.CreateFont()

            ' 📏 Dimensões
            Dim pageSize As iText.Kernel.Geom.Rectangle = pdfDocument.GetDefaultPageSize()
            Dim largura As Single = pageSize.GetWidth()
            Dim altura As Single = pageSize.GetHeight()
            Dim rotacao As Integer = page.GetRotation()

            ' ============================================================
            ' 🔹 Corrige rotação para que o texto sempre fique horizontal
            ' ============================================================
            Select Case rotacao
                Case 0
                    canvas.ConcatMatrix(1, 0, 0, 1, 0, 0)
                Case 90
                    ' Folha girada em pé: gira texto 90° anti-horário
                    canvas.ConcatMatrix(0, 1, -1, 0, altura, 0)
                Case 180
                    ' Cabeça pra baixo: inverte texto
                    canvas.ConcatMatrix(-1, 0, 0, -1, largura, altura)
                Case 270
                    ' Deitado (Solid Edge): gira texto 90° horário
                    canvas.ConcatMatrix(0, -1, 1, 0, 0, largura)
            End Select

            ' ============================================================
            ' 🔹 Define formato e posição base
            ' ============================================================
            Dim paisagem As Boolean = largura > altura
            Dim yBase As Single = 10
            Dim xBase As Single = 100
            Dim espacamento As Single = If(paisagem, 200, 150)

            ' ============================================================
            ' ✍️ Escreve texto horizontal no rodapé
            ' ============================================================
            canvas.BeginText()
            canvas.SetFontAndSize(fonte, 10)

            ' 🔸 Garante rotação zero do texto (sempre de pé)
            Dim matrizHorizontal As New iText.Kernel.Geom.AffineTransform()
            matrizHorizontal.Translate(xBase, yBase)
            matrizHorizontal.Rotate(0) ' sem rotação
            canvas.ConcatMatrix(matrizHorizontal)

            ' 🔸 Escreve as informações em linha

            'Dim xPos As Single = 0
            'canvas.SetTextMatrix(xPos, 0)
            'canvas.ShowText($"Tipo: {TipoIdentificado}")
            'xPos += espacamento


            'canvas.SetTextMatrix(xPos, 0)
            'canvas.ShowText($"Acabamento: {Acabamento}")
            'xPos += espacamento


            'canvas.SetTextMatrix(xPos, 0)
            'canvas.ShowText($"ID: {Identificado}")
            'xPos += espacamento

            'canvas.SetTextMatrix(xPos, 0)
            'canvas.ShowText($"Qtde: {QtdeTotal}")
            'xPos += espacamento

            'canvas.SetTextMatrix(xPos, 0)
            'canvas.ShowText($"Data: {Date.Now:dd/MM/yyyy}")

            ' 🔸 Escreve as informações em linha

            Dim xPos As Single = 0
            canvas.SetTextMatrix(xPos, 0)
            canvas.ShowText($"Tipo: {TipoIdentificado}" & ($" ID: {Identificado}") & ($" Acabamento: {Acabamento}") & ($" Qtde: {QtdeTotal}") & ($" Data: {Date.Now:dd/MM/yyyy}"))

            canvas.EndText()

            pdfDocument.Close()

        Catch ex As Exception
            cl_BancoDados.CopiarArquivoInteligente(Origem, caminhoArquivoDestino)
        End Try

    End Sub







    ''Public Sub EditarPDFParaOsFormatoLynx(Origem As String,
    ''                                  caminhoArquivoDestino As String,
    ''                                  novoNomeArquivo As String,
    ''                                  QtdeTotal As String,
    ''                                  Identificado As String,
    ''                                  Acabamento As String,
    ''                                  TipoIdentidicado As String)

    ''    Origem = System.Text.RegularExpressions.Regex.Replace(Origem, ".sldprt$|.sldasm$|.asm$|.psm$|.par$", ".pdf", System.Text.RegularExpressions.RegexOptions.IgnoreCase)
    ''    Origem = Path.GetFullPath(Origem)

    ''    If System.IO.File.Exists(Origem) Then
    ''        Try
    ''            Dim pdfReader As New PdfReader(Origem)
    ''            Dim pdfWriter As New PdfWriter(caminhoArquivoDestino)
    ''            Dim pdfDocument As New PdfDocument(pdfReader, pdfWriter)
    ''            Dim page As PdfPage = pdfDocument.GetPage(1)
    ''            Dim canvas As New PdfCanvas(page)

    ''            ' =========================================================
    ''            ' 🔹 Dimensões da página
    ''            ' =========================================================
    ''            Dim largura As Single = pdfDocument.GetDefaultPageSize().GetWidth()
    ''            Dim altura As Single = pdfDocument.GetDefaultPageSize().GetHeight()

    ''            ' =========================================================
    ''            ' 🔹 Fonte padrão
    ''            ' =========================================================
    ''            Dim fonte = iText.Kernel.Font.PdfFontFactory.CreateFont()

    ''            ' ============================================================
    ''            ' 🧾 Textos informativos no rodapé (horizontal — canto inferior esquerdo)
    ''            ' ============================================================

    ''            canvas.BeginText()
    ''            canvas.SetFontAndSize(fonte, 8)

    ''            ' 🔹 Reset da matriz de transformação (para garantir texto horizontal)
    ''            canvas.SetTextMatrix(1, 0, 0, 1, 0, 0)

    ''            ' 🔹 Define margens e espaçamento proporcional à largura da folha
    ''            Dim margemEsquerda As Single = 30
    ''            Dim margemInferior As Single = 20
    ''            Dim larguraPagina As Single = pdfDocument.GetDefaultPageSize().GetWidth()
    ''            Dim larguraUtil As Single = larguraPagina - (margemEsquerda * 2)
    ''            Dim espacoHorizontal As Single = larguraUtil / 5.5

    ''            Dim posX As Single = margemEsquerda
    ''            Dim posY As Single = margemInferior

    ''            ' Move para a primeira posição
    ''            canvas.MoveText(posX, posY)

    ''            ' 1️⃣ Tipo Identificado
    ''            canvas.ShowText($"Tipo: {TipoIdentidicado}")
    ''            canvas.MoveText(espacoHorizontal, 0)

    ''            ' 2️⃣ Acabamento
    ''            canvas.ShowText($"Acabamento: {Acabamento}")
    ''            canvas.MoveText(espacoHorizontal, 0)

    ''            ' 3️⃣ Identificado
    ''            canvas.ShowText($"Identificado: {Identificado}")
    ''            canvas.MoveText(espacoHorizontal, 0)

    ''            ' 4️⃣ Qtde
    ''            canvas.ShowText($"Qtde: {QtdeTotal}")
    ''            canvas.MoveText(espacoHorizontal, 0)

    ''            ' 5️⃣ Data
    ''            canvas.ShowText($"Data: {Date.Now:dd/MM/yyyy}")

    ''            canvas.EndText()


    ''            ' =========================================================
    ''            ' 🩶 ADICIONA MARCA D'ÁGUA GRANDE CENTRAL
    ''            ' =========================================================
    ''            Dim canvasWatermark As New PdfCanvas(page)
    ''            Dim fonteWatermark = iText.Kernel.Font.PdfFontFactory.CreateFont(iText.IO.Font.Constants.StandardFonts.HELVETICA_BOLD)

    ''            canvasWatermark.SaveState()
    ''            canvasWatermark.BeginText()
    ''            canvasWatermark.SetFontAndSize(fonteWatermark, 120)

    ''            '' 🔸 Define cor e transparência da marca
    ''            'Dim gs As New iText.Kernel.Pdf.Extgstate()
    ''            'gs.SetFillOpacity(0.1F)
    ''            'canvasWatermark.SetExtGState(gs)

    ''            ' 🔸 Posição e rotação (diagonal)
    ''            canvasWatermark.SetTextMatrix(1, 0, -0.5F, 1, largura / 6, altura / 2)

    ''            ' 🔸 Texto da marca d’água
    ''            canvasWatermark.ShowText("SINCO - LYNX FORMAT TEST")

    ''            canvasWatermark.EndText()
    ''            canvasWatermark.RestoreState()

    ''            ' =========================================================
    ''            pdfDocument.Close()

    ''        Catch ex As Exception
    ''            cl_BancoDados.CopiarArquivoInteligente(Origem, caminhoArquivoDestino)
    ''        End Try
    ''    End If
    ''End Sub

    'Public Sub EditarPDFParaOsFormatoLynx(Origem As String,
    '                                  caminhoArquivoDestino As String,
    '                                  novoNomeArquivo As String,
    '                                  QtdeTotal As String,
    '                                  Identificado As String,
    '                                  Acabamento As String,
    '                                  TipoIdentidicado As String)

    '    ' 🔸 Ajusta extensão e caminho da origem
    '    Origem = System.Text.RegularExpressions.Regex.Replace(
    '    Origem, ".sldprt$|.sldasm$|.asm$|.psm$|.par$",
    '    ".pdf", System.Text.RegularExpressions.RegexOptions.IgnoreCase)

    '    Origem = Path.GetFullPath(Origem)
    '    If Not System.IO.File.Exists(Origem) Then Exit Sub

    '    Try
    '        ' 🔸 Abre o PDF existente
    '        Dim pdfReader As New PdfReader(Origem)
    '        Dim pdfWriter As New PdfWriter(caminhoArquivoDestino)
    '        Dim pdfDocument As New PdfDocument(pdfReader, pdfWriter)
    '        Dim page As PdfPage = pdfDocument.GetPage(1)

    '        ' ============================================================
    '        ' 🧾 RODAPÉ HORIZONTAL — canto inferior esquerdo
    '        ' ============================================================
    '        Dim canvas As New PdfCanvas(page)
    '        Dim fonte As iText.Kernel.Font.PdfFont = PdfFontFactory.CreateFont()

    '        canvas.SaveState()
    '        canvas.BeginText()
    '        canvas.SetFontAndSize(fonte, 8)
    '        canvas.SetTextMatrix(1, 0, 0, 1, 0, 0) ' garante texto horizontal

    '        ' 🔹 Define margens e espaçamento proporcional à largura da folha
    '        Dim margemEsquerda As Single = 30
    '        Dim margemInferior As Single = 20
    '        Dim larguraPagina As Single = pdfDocument.GetDefaultPageSize().GetWidth()
    '        Dim larguraUtil As Single = larguraPagina - (margemEsquerda * 2)
    '        Dim espacoHorizontal As Single = larguraUtil / 5.5

    '        ' 🔹 Posição inicial
    '        Dim posX As Single = margemEsquerda
    '        Dim posY As Single = margemInferior
    '        canvas.MoveText(posX, posY)

    '        ' 🔹 Textos horizontais lado a lado
    '        canvas.ShowText($"Tipo: {TipoIdentidicado}")
    '        canvas.MoveText(espacoHorizontal, 0)

    '        canvas.ShowText($"Acabamento: {Acabamento}")
    '        canvas.MoveText(espacoHorizontal, 0)

    '        canvas.ShowText($"Identificado: {Identificado}")
    '        canvas.MoveText(espacoHorizontal, 0)

    '        canvas.ShowText($"Qtde: {QtdeTotal}")
    '        canvas.MoveText(espacoHorizontal, 0)

    '        canvas.ShowText($"Data: {Date.Now:dd/MM/yyyy}")

    '        canvas.EndText()
    '        canvas.RestoreState()

    '        ' 🔹 Finaliza o documento
    '        pdfDocument.Close()

    '    Catch ex As Exception
    '        ' Em caso de erro, copia o original
    '        cl_BancoDados.CopiarArquivoInteligente(Origem, caminhoArquivoDestino)
    '    End Try

    'End Sub





    Private Sub MarcarTodosToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles MarcarTodosToolStripMenuItem.Click

        For i As Integer = 0 To DGVListaMaterialSW.Rows.Count - 1

            If DGVListaMaterialSW.Rows(i).Cells("dgvSelecao").Value = False Then

                DGVListaMaterialSW.Rows(i).Cells("dgvSelecao").Value = True

            End If

        Next

    End Sub

    Private Sub DesmarcarTodosToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DesmarcarTodosToolStripMenuItem.Click

        For i As Integer = 0 To DGVListaMaterialSW.Rows.Count - 1

            If DGVListaMaterialSW.Rows(i).Cells("dgvSelecao").Value = True Then

                DGVListaMaterialSW.Rows(i).Cells("dgvSelecao").Value = False

            End If

        Next

    End Sub

    Private Sub InverterSeleçãoToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles InverterSeleçãoToolStripMenuItem.Click

        For i As Integer = 0 To DGVListaMaterialSW.Rows.Count - 1

            If DGVListaMaterialSW.Rows(i).Cells("dgvSelecao").Value = False Then

                DGVListaMaterialSW.Rows(i).Cells("dgvSelecao").Value = True

            ElseIf DGVListaMaterialSW.Rows(i).Cells("dgvSelecao").Value = True Then

                DGVListaMaterialSW.Rows(i).Cells("dgvSelecao").Value = False

            End If

        Next

    End Sub
    Private Sub CancelarLiberaçãoDaOSToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CancelarLiberaçãoDaOSToolStripMenuItem.Click


        If OrdemServico.Liberado_Engenharia = "" Then
            MsgBox("A OS não está liberada!", vbCritical, "Atenção")
            Exit Sub
        End If

        If OrdemServico.IdOrdemServico.ToString = Nothing Or OrdemServico.IdOrdemServico.ToString = "" Or OrdemServico.IdOrdemServico.ToString <= 0 Then

            MsgBox("Não há OS Selecionada!", vbCritical, "Atenção")
            Exit Sub
        End If
        Dim result As DialogResult = MessageBox.Show("Deseja Realmente Cancelar a Liberação da Ordem de Serviço: " & OrdemServico.IdOrdemServico, "Cancelando Ordem de Serviço", MessageBoxButtons.YesNo)
        Dim totalExecutado As Integer
        Try

            cl_BancoDados.RetornaCampoDaPesquisa("SELECT  count(idplanodecorte) +
				   count(CorteTotalExecutado) + count(DobraTotalExecutado)+ count(SoldaTotalExecutado) +
				   count(PinturaTotalExecutado) +  count(MontagemTotalExecutado) as totalExecutado
				   FROM  " & ComplementoTipoBanco & "ordemservicoitem where IdOrdemServico  ='" & OrdemServico.IdOrdemServico & "'
				   and (idplanodecorte > 0) AND (D_E_L_E_T_E IS NULL OR D_E_L_E_T_E = '');", "totalExecutado")
            totalExecutado = Convert.ToInt32(VCampo0)
        Catch ex As Exception

            totalExecutado = 0
        Finally

        End Try

        If result = DialogResult.Yes Then

            If totalExecutado > 0 Then

                Dim dtTabelaPlanoCorte As New System.Data.DataTable()

                dtTabelaPlanoCorte = cl_BancoDados.CarregarDados("SELECT  idplanodecorte, CodMatFabricante
					 FROM  " & ComplementoTipoBanco & "ordemservicoitem where IdOrdemServico  = '" & OrdemServico.IdOrdemServico & "' and (idplanodecorte > 0) AND (D_E_L_E_T_E IS NULL OR D_E_L_E_T_E = '');")

                Dim MessagemItens As String

                For I As Integer = 0 To dtTabelaPlanoCorte.Rows.Count - 1

                    MessagemItens = MessagemItens & "PlanoCorte = " & dtTabelaPlanoCorte.Rows(I).Item("idplanodecorte").ToString &
                    " Numero Desenho: = " & dtTabelaPlanoCorte.Rows(I).Item("CodMatFabricante").ToString & vbCrLf

                Next

                MsgBox("A OS Numero: " & OrdemServico.IdOrdemServico & " contem processos em andamento, por este motivo não pode ser cancelada, ver plano de corte's: " & vbCrLf & MessagemItens, vbCritical, "Atenção!")

                Exit Sub

            End If

            OrdemServico.EnderecoOrdemServico = dgvos.CurrentRow.Cells("Endereco").Value.ToString

            '       cl_BancoDados.AlteracaoEspecifica("ordemservicoitem", "D_E_L_E_T_E", "*", "IdOrdemServico", OrdemServico.IdOrdemServico)

            Dim diretorio As String = OrdemServico.EnderecoOrdemServico

            LimparDiretorio(diretorio & "\PDF")
            LimparDiretorio(diretorio & "\DXF")
            LimparDiretorio(diretorio & "\DFT")
            LimparDiretorio(diretorio & "\LXDS")
            LimparDiretorio(diretorio & "\IGES")

            cl_BancoDados.Salvar("Update ordemservico set Liberado_Engenharia = '',
						Data_Liberacao_Engenharia = ''
						where IdOrdemServico = '" & OrdemServico.IdOrdemServico & "'")

            cl_BancoDados.Salvar("Update ordemservicoitem set Liberado_Engenharia = '',
						Data_Liberacao_Engenharia = ''
						where IdOrdemServico = '" & OrdemServico.IdOrdemServico & "'")

            dgvos.CurrentRow.Cells("Liberado_Engenharia").Value = ""
            dgvos.CurrentRow.Cells("Data_Liberacao_Engenharia").Value = ""
            dgvos.CurrentRow.Cells("dgvStatus").Value = My.Resources.atencao

            OrdemServico.Liberado_Engenharia = ""
            OrdemServico.Data_Liberacao_Engenharia = ""


            'OrdemServico.SaldoTag
            'OrdemServico.QtdeLiberada
            'OrdemServico.Fator
            cl_BancoDados.RetornaCampoDaPesquisa("Select TipoLiberacaoOrdemServico from ordemservico where idordemservico = '" & OrdemServico.IdOrdemServico & "'", "TipoLiberacaoOrdemServico")

            Dim total As String = VCampo0

            If total.ToString = "Total" Then

                cl_BancoDados.Salvar("Update tags set QtdeLiberada = '" & (OrdemServico.QtdeLiberada - OrdemServico.Fator) & "',
                                                  SaldoTag = '" & (OrdemServico.SaldoTag + OrdemServico.Fator) & "'
												where IdTag = '" & OrdemServico.idTag & "'")

                cl_BancoDados.VerificaSaldoTag(OrdemServico.idTag)

                Me.lbltxtQtdeTag.Text = OrdemServico.QtdeTag
                Me.lbltxtQtdeLiberada.Text = OrdemServico.QtdeLiberada
                Me.lbltxtSaldoTag.Text = OrdemServico.SaldoTag

            End If

            ' Timerdgvos.Enabled = True


            'dgvos.Refresh()

            'If Usuario.EnviarEmailLiberacaoOS <> "" Then

            '    Dim resultado As MsgBoxResult = MessageBox.Show("Deseja enviar o e-mail para o PCP, de cancelamento da Ordem de Serviço: " & OrdemServico.IdOrdemServico, "Cancelamento", MessageBoxButtons.YesNo)

            '    If resultado = DialogResult.Yes Then

            '        ClasseEmail.EmailCancelamentoOS()

            '    End If

            'End If

            'TimerFiltroPecaAtivaOS.Enabled = True
        Else

            MsgBox("Esta operação não é valida para OS: " & OrdemServico.IdOrdemServico & ", há processos executados!", vbCritical, "Atenção")

        End If

        cl_BancoDados.CalcularordemservicoitemFatorOS_TAG_PROJETO()

    End Sub

    Private Sub AtualizarPDFsEDXFsNaPastaDaOSMToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AtualizarPDFsEDXFsNaPastaDaOSMToolStripMenuItem.Click


        Dim resultado As MsgBoxResult = MessageBox.Show("Deseja Atualizar os arquivos da OS, esta ação ira apagar todos os arquivo da pasta da Ordem de Serviço " & OrdemServico.IdOrdemServico, "Atualização", MessageBoxButtons.YesNo)

        If resultado = DialogResult.Yes Then

            ' Verifica e obtém o valor da célula "Liberado_Engenharia"
            If dgvos.CurrentRow.Cells("Liberado_Engenharia") IsNot Nothing AndAlso dgvos.CurrentRow.Cells("Liberado_Engenharia").Value IsNot DBNull.Value Then
                OrdemServico.Liberado_Engenharia = dgvos.CurrentRow.Cells("Liberado_Engenharia").Value.ToString()
            Else
                OrdemServico.Liberado_Engenharia = String.Empty ' Valor padrão em caso de ausência
            End If

            If OrdemServico.Liberado_Engenharia = "S" Then

                MsgBox("OS Já liberada, não e possivel liberar novamente!", vbInformation, "Atenção")

                Exit Sub
            Else

                Try

                    If DGVListaMaterialSW.Rows.Count > 0 Then

                        Dim diretorio As String = OrdemServico.EnderecoOrdemServico

                        LimparDiretorio(diretorio & "\PDF")
                        LimparDiretorio(diretorio & "\DXF")
                        LimparDiretorio(diretorio & "\DFT")
                        LimparDiretorio(diretorio & "\LXDS")

                        'OrdemServico.IDOrdemServicoItem = DGVListaMaterialSW.Rows(1).Cells("IdOrdemServicoItem").Value

                        ImportarLXDSParaOS(DGVListaMaterialSW, "DXF", ProgressBarProcessoLiberacaoOrdemServico, "")
                        ImportarLXDSParaOS(DGVListaMaterialSW, "PDF", ProgressBarProcessoLiberacaoOrdemServico, "IdOrdemservicoItem")
                        ImportarLXDSParaOS(DGVListaMaterialSW, "DFT", ProgressBarProcessoLiberacaoOrdemServico, "")
                        ImportarLXDSParaOS(DGVListaMaterialSW, "LXDS", ProgressBarProcessoLiberacaoOrdemServico, "")

                        MsgBox("Documentos enviado com sucesso!", vbInformation, "Atenção")
                    Else

                        MsgBox("Não ha dados a serem atualizado", vbCritical, "Atenção!")

                    End If
                Catch ex As Exception
                    MsgBox(ex.Message & " ERRO: Transferencia de arquivo - Atualização de documento da OS")
                Finally

                End Try

                'Me.lblOrdemServicoAtiva.Text = ""

                'TimerDGVListaMaterialSW.Enabled = True
                'TimerFiltroPecaAtivaOS.Enabled = True

            End If
        Else

            MsgBox("Operação Cancelada", vbCritical, "Atenção")

        End If

    End Sub

    Private Sub AlterarOFatorMultipçlicadorDaOSToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AlterarOFatorMultipçlicadorDaOSToolStripMenuItem.Click

        If OrdemServico.Liberado_Engenharia = "S" Then

            MsgBox("Ordem de Serviço já Liberada para Produção, não pode mais ser modificada!", vbCritical, "Atenção")

            Exit Sub

        End If

        If DGVListaMaterialSW.Rows.Count <= 0 Then

            MsgBox("Não há itens na Ordem de Serviço!", vbInformation, "Atenção")

            Exit Sub

        End If

        Try

            OrdemServico.Fator = InputBox("Informe o Valor Multiplicador para fabricação, o Valor informado como
                                              padrão é a quantidade de
                                        conjuntos solicitados pelo PCP no ato do cadasro da Tag", "Fator de Multiplicação", lbltxtSaldoTag.Text)

            If OrdemServico.Fator <= 0 Then

                MsgBox("A operação foi cancelada. O valor informado não é válido!", vbInformation, "Atenção")
                Exit Sub

            End If

            If IsNumeric(OrdemServico.Fator) And OrdemServico.Fator > 0 Then

                cl_BancoDados.AbrirBanco()
                cl_BancoDados.CalcularordemservicoitemFator(OrdemServico.IdOrdemServico, OrdemServico.Fator)
                cl_BancoDados.FecharBanco()
                TimerDGVListaMaterialSW.Enabled = True

                dgvos.CurrentRow.Cells("Fator").Value = OrdemServico.Fator

                Dim diretorio As String = OrdemServico.EnderecoOrdemServico

                LimparDiretorio(diretorio & "\PDF")
                LimparDiretorio(diretorio & "\DXF")
                LimparDiretorio(diretorio & "\DFT")
                LimparDiretorio(diretorio & "\LXDS")

                ' cl_BancoDados.AlteracaoEspecifica("OrdemServico", "Fator", Replace(OrdemServico.Fator, ".", ","), "IdOrdemServico", OrdemServico.IdOrdemServico)

                Me.lblFator.Text = OrdemServico.Fator
                MsgBox("Fator alterado com sucesso!", vbInformation, "Atenção")
            Else
                MsgBox("O valor informado não e um numero Valido")
            End If
        Catch ex As Exception
        Finally
        End Try

        cl_BancoDados.CalcularordemservicoitemFatorOS_TAG_PROJETO()
    End Sub

    Private Sub GeralExcelDaOSToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles GeralExcelDaOSToolStripMenuItem.Click


        Dim principal As String = ""

        If DGVListaMaterialSW.Rows.Count >= 0 Then

            For i As Integer = 0 To DGVListaMaterialSW.Rows.Count - 1

                'Verifica se há um produto principal para Ordem de serviço
                Try

                    principal = DGVListaMaterialSW.Rows(i).Cells("ProdutoPrincipal").Value.ToString

                    If principal = "SIM" Then
                        '  principal = DGVListaMaterialSW.Rows(i).Cells("ProdutoPrincipal").Value.ToString
                        ProdutoPrincipal = DGVListaMaterialSW.Rows(i).Cells("CodMatFabricante").Value.ToString

                        Exit For

                    ElseIf principal = "" Then

                        principal = ""
                        ProdutoPrincipal = ""
                        MsgBox("Para gerar a Lista de material e obrigatorio que o produto 'Principal'da Ordem de Serviço já esteja selecionado.", vbInformation, "Atenção")
                        Exit Sub
                    End If
                Catch ex As Exception

                    principal = ""
                    ProdutoPrincipal = ""
                    MsgBox("Para gerar a Lista de material e obrigatorio que o produto 'Principal'da Ordem de Serviço já esteja selecionado.", vbInformation, "Atenção")

                    Exit Sub

                End Try

            Next

        End If

        Try

            txtPesqNumeroDesenho.Clear()

            txtPesqTipoDesenho.Clear()

            txtPesqAcabamentoDesenho.Clear()

            TimerDGVListaMaterialSW.Enabled = True


            TemplatesExcel.ExportarOrdemServicoPadrao(DGVListaMaterialSW, ProgressBarProcessoLiberacaoOrdemServico, OrdemServico.EnderecoOrdemServico, Me.txtDescricao.Text.Trim.ToUpper, dgvos, DGVListaMaterialSWMateriais)

        Catch ex As Exception

            MsgBox("Erro ao gerar o Excel da OS: " & ex.Message, MsgBoxStyle.Critical, "Erro")
        Finally

        End Try

    End Sub

    Private Sub LimparPastaOrdemDeServiçoSelecionadaToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles LimparPastaOrdemDeServiçoSelecionadaToolStripMenuItem.Click

        If OrdemServico.IdOrdemServico = Nothing Then

            MsgBox("Não há Ordem de Serviço selecionada", vbCritical, "Atenção")
        Else

            If dgvos.CurrentRow.Cells("Liberado_Engenharia").Value.ToString <> "S" Then

                OrdemServico.EnderecoOrdemServico = dgvos.CurrentRow.Cells("Endereco").Value.ToString

                Dim resultado As MsgBoxResult = MessageBox.Show("Deseja limpar a Ordem de Serviço, esta ação ira apagar todos os arquivo da pasta da Ordem de Serviço " & OrdemServico.IdOrdemServico, "Exclusão", MessageBoxButtons.YesNo)

                If resultado = DialogResult.Yes Then



                    Dim sql As String = "DELETE FROM ordemservicoitem WHERE idordemservico = @id"

                    Dim parametros As New List(Of MySqlParameter) From {
              New MySqlParameter("@id", OrdemServico.IdOrdemServico)
                }

                    cl_BancoDados.SalvarParametros(sql, parametros)



                    '  cl_BancoDados.AlteracaoEspecificaDelete("ordemservicoitem", "idordemservico", OrdemServico.IdOrdemServico)

                    ' cl_BancoDados.AlteracaoEspecifica("ordemservicoitem", "D_E_L_E_T_E", "*", "IdOrdemServico", OrdemServico.IdOrdemServico)

                    Dim diretorio As String = OrdemServico.EnderecoOrdemServico

                    LimparDiretorio(diretorio)

                    Me.lblOrdemServicoAtiva.Text = ""

                    TimerDGVListaMaterialSW.Enabled = True
                    ' TimerFiltroPecaAtivaOS.Enabled = True
                Else

                    MsgBox("Operação Cancelada", vbCritical, "Atenção")

                End If
            Else

                MsgBox("Esta operação não e valida a OS: " & OrdemServico.EnderecoOrdemServico & ", já foi liberada anteriormente!", vbInformation, "Atenção")

            End If

        End If

        cl_BancoDados.CalcularordemservicoitemFatorOS_TAG_PROJETO()

    End Sub

    Private Sub AbrirPDFDaLinhaSelecionadaToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AbrirPDFDaLinhaSelecionadaToolStripMenuItem.Click


        Try

            Dim ArquivoPdf As String = DGVListaMaterialSW.CurrentRow.Cells("EnderecoArquivo").Value.ToString()

            ' Substitui extensões ".SLDASM" e ".SLDPRT" por ".DDF"
            ArquivoPdf = Path.ChangeExtension(ArquivoPdf, ".PDF")

            ' Obtém o caminho completo
            ArquivoPdf = Path.GetFullPath(ArquivoPdf)

            ' Verifica se o arquivo existe e o abre
            If File.Exists(ArquivoPdf) Then
                Using p As New Diagnostics.Process
                    p.StartInfo = New ProcessStartInfo(ArquivoPdf)

                    p.Start()
                    'p.WaitForExit()

                    DGVListaMaterialSW.CurrentRow.DefaultCellStyle.BackColor = Color.LightCyan
                End Using
            End If
        Catch ex As Exception
            MsgBox("Arquivo não encontrado!", vbCritical, "Atenção")
        Finally

        End Try

    End Sub

    Private Sub ExcluirODocumentoDaLinhaSelecionadaToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExcluirODocumentoDaLinhaSelecionadaToolStripMenuItem.Click


        If OrdemServico.Liberado_Engenharia <> "" Then

            MsgBox("Ordem de Serviço já Liberada para Produção, não pode mais ser modificada!", vbCritical, "Atenção")
            Exit Sub
        Else

            If MsgBox("Tem certeza que deseja desabilitar o Desenho Selecionado?", MsgBoxStyle.YesNo, "Confirmar desabilitação!") = MsgBoxResult.No Then

                Exit Sub
            Else

                ' Atualiza o status no banco de dados
                cl_BancoDados.Salvar("UPDATE ordemservicoitem SET D_E_L_E_T_E = '*' WHERE CodMatFabricante = '" & DGVListaMaterialSW.CurrentRow.Cells("CodMatFabricante").Value.ToString() & "'")

                ' Remove a linha corrente do DataGridView
                If DGVListaMaterialSW.CurrentRow IsNot Nothing Then
                    DGVListaMaterialSW.Rows.Remove(DGVListaMaterialSW.CurrentRow)
                End If
                ' MsgBox("Desenho desabilitado e removido da lista.")
            End If

        End If

    End Sub

    Private Sub MarcarComoConjuntoPrincipalDaOrdemDeServiçoToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles MarcarComoConjuntoPrincipalDaOrdemDeServiçoToolStripMenuItem.Click

        For i As Integer = 0 To DGVListaMaterialSW.Rows.Count - 1

            OrdemServico.ProdutoPrincipal = DGVListaMaterialSW.Rows(i).Cells("ProdutoPrincipal").Value.ToString

            If OrdemServico.ProdutoPrincipal <> "" Then

                MsgBox("Esta ordem de serviço já possui um produto principal", vbCritical, "Atenção")

                Exit Sub

            End If

        Next

        Try
            OrdemServico.IDOrdemServicoItem = DGVListaMaterialSW.CurrentRow.Cells("IDOrdemServicoItem").Value.ToString

            cl_BancoDados.AlteracaoEspecifica("ordemservicoitem", "ProdutoPrincipal", "SIM", "IDOrdemServicoItem", OrdemServico.IDOrdemServicoItem)

            DGVListaMaterialSW.CurrentRow.Cells("ProdutoPrincipal").Value = "SIM".ToUpper

            DGVListaMaterialSW.CurrentRow.Cells("dgvIconeItemOS").Value = My.Resources.IconeswPrincipal

            ' DadosArquivoCorrente.NomeArquivoSemExtensao = DGVListaMaterialSW.CurrentRow.Cells("CodMatFabricante").Value.ToString


            cl_BancoDados.RetornaCampoDaPesquisa("Select EnderecoImagem from material where CodMatFabricante = '" & OrdemServico.CodMatFabricante & "'", "EnderecoImagem")

            cl_BancoDados.AlteracaoEspecifica("ordemservico", "EnderecoImagem", VCampo0, "idordemservico", OrdemServico.IdOrdemServico)

            cl_BancoDados.AlteracaoEspecifica("material", "ProdutoPrincipal", "SIM", "CodMatFabricante", OrdemServico.CodMatFabricante)

            cl_BancoDados.AlteracaoEspecifica("ordemservico", "CodDesenhoProduto", OrdemServico.CodMatFabricante, "idordemservico", OrdemServico.IdOrdemServico)

            cl_BancoDados.AlteracaoEspecifica("ordemservico", "DescricaoProduto", OrdemServico.DescResumo & "-" & OrdemServico.DescDetal, "idordemservico", OrdemServico.IdOrdemServico)


        Catch ex As Exception

            OrdemServico.IDOrdemServicoItem = Nothing

            MsgBox("Item da Ordem de Serviço não Valido", vbCritical, "Atenção")

        End Try

    End Sub

    Private Sub DesmarcarComoConjuntoPrincipalToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DesmarcarComoConjuntoPrincipalToolStripMenuItem.Click


        Try
            OrdemServico.IDOrdemServicoItem = DGVListaMaterialSW.CurrentRow.Cells("IDOrdemServicoItem").Value.ToString

            cl_BancoDados.AlteracaoEspecifica("ordemservicoitem", "ProdutoPrincipal", "", "IDOrdemServicoItem", OrdemServico.IDOrdemServicoItem)

            cl_BancoDados.AlteracaoEspecifica("material", "ProdutoPrincipal", "", "CodMatFabricante", DadosArquivoCorrente.NomeArquivoSemExtensao)

            DGVListaMaterialSW.CurrentRow.Cells("ProdutoPrincipal").Value = "SIM".ToUpper

            DGVListaMaterialSW.CurrentRow.Cells("dgvIconeItemOS").Value = My.Resources.IcopneMontagemSW
        Catch ex As Exception

            OrdemServico.IDOrdemServicoItem = Nothing

            MsgBox("Item da Ordem de Serviço não Valido", vbCritical, "Atenção")

        End Try

    End Sub

    Private Sub AlterarAQuantidadeDePeçasFabricaçãoDaLinhaSelecionadaToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AlterarAQuantidadeDePeçasFabricaçãoDaLinhaSelecionadaToolStripMenuItem.Click


        Try

            If OrdemServico.Liberado_Engenharia <> "" Then

                MsgBox("Ordem de Serviço já Liberada para Produção, não pode mais ser modificada!", vbCritical, "Atenção")

                Exit Sub
            Else

                Dim novaqtde As Double

                novaqtde = InputBox("Informe a Nova Quantidade", "Alteração de Quantidade", DGVListaMaterialSW.CurrentRow.Cells("QtdeTotal").Value.ToString)

                If IsNumeric(novaqtde) Then

                    Dim Peso, AreaPintura As String

                    Peso = (DGVListaMaterialSW.CurrentRow.Cells("Peso").Value / DGVListaMaterialSW.CurrentRow.Cells("QtdeTotal").Value) * novaqtde

                    AreaPintura = (DGVListaMaterialSW.CurrentRow.Cells("AreaPintura").Value / DGVListaMaterialSW.CurrentRow.Cells("QtdeTotal").Value) * novaqtde

                    cl_BancoDados.AlteracaoEspecifica("ordemservicoitem", "QtdeTotal", novaqtde, "IDOrdemServicoItem", DGVListaMaterialSW.CurrentRow.Cells("IDOrdemServicoItem").Value.ToString)
                    cl_BancoDados.AlteracaoEspecifica("ordemservicoitem", "qtde", novaqtde, "IDOrdemServicoItem", DGVListaMaterialSW.CurrentRow.Cells("IDOrdemServicoItem").Value.ToString)

                    cl_BancoDados.AlteracaoEspecifica("ordemservicoitem", "AreaPintura", AreaPintura, "IDOrdemServicoItem", DGVListaMaterialSW.CurrentRow.Cells("IDOrdemServicoItem").Value.ToString)

                    cl_BancoDados.AlteracaoEspecifica("ordemservicoitem", "Peso", Peso, "IDOrdemServicoItem", DGVListaMaterialSW.CurrentRow.Cells("IDOrdemServicoItem").Value.ToString)

                    DGVListaMaterialSW.CurrentRow.Cells("QtdeTotal").Value = novaqtde
                    DGVListaMaterialSW.CurrentRow.Cells("qtde").Value = novaqtde

                    DGVListaMaterialSW.CurrentRow.Cells("AreaPintura").Value = AreaPintura

                    DGVListaMaterialSW.CurrentRow.Cells("Peso").Value = Peso

                    DGVListaMaterialSW.CurrentRow.Cells("QtdeTotal").Style.BackColor = Color.LightGreen

                    DGVListaMaterialSW.CurrentRow.Cells("qtde").Style.BackColor = Color.LightGreen

                    DGVListaMaterialSW.CurrentRow.Cells("AreaPintura").Style.BackColor = Color.LightGreen

                    DGVListaMaterialSW.CurrentRow.Cells("Peso").Style.BackColor = Color.LightGreen
                Else

                    MsgBox("O valor informado não e um numero Valido")

                End If

            End If
        Catch ex As Exception
        Finally
        End Try

    End Sub

    Private Sub DGVListaMaterialSW_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DGVListaMaterialSW.CellContentClick

    End Sub

    Dim FormataDGVListaMaterialSW As Boolean = False

    Private Sub DGVListaMaterialSW_DataBindingComplete(sender As Object, e As DataGridViewBindingCompleteEventArgs) Handles DGVListaMaterialSW.DataBindingComplete



        If DGVListaMaterialSW IsNot Nothing AndAlso DGVListaMaterialSW.Rows IsNot Nothing AndAlso DGVListaMaterialSW.Rows.Count > 0 Then

            ' Otimizações de Perfomance: 
            ' 1. Cache para evitar ler o disco (File.Exists) repetidamente pelo mesmo caminho.
            ' 2. Carregar imagens do My.Resources para memória uma vez, em vez de ler do assembly cada ciclo.
            Dim cacheDXF As New Dictionary(Of String, Boolean)(StringComparer.OrdinalIgnoreCase)
            Dim cachePDF As New Dictionary(Of String, Boolean)(StringComparer.OrdinalIgnoreCase)

            Dim imgDXF As Image = My.Resources.dxf
            Dim imgPDF As Image = My.Resources.pdf
            Dim imgSemIcone As Image = My.Resources.sem_icone
            Dim imgMontagemSW As Image = My.Resources.IcopneMontagemSW
            Dim imgSWPrincipal As Image = My.Resources.IconeswPrincipal
            Dim imgMontagemPRT As Image = My.Resources.IcopneMontagemPRT
            Dim imgDiversos As Image = My.Resources.sinco_Diversos

            ' Configura e exibe a barra de progresso do preenchimento de ícones
            ProgressBarOSM.Style = ProgressBarStyle.Continuous
            ProgressBarOSM.Minimum = 0
            ProgressBarOSM.Maximum = DGVListaMaterialSW.Rows.Count
            ProgressBarOSM.Value = 0
            ProgressBarOSM.Visible = True

            ' Pausa o desenho do DataGridView para acelerar
            DGVListaMaterialSW.SuspendLayout()

            Dim count_prog As Integer = 0

            For Each row As DataGridViewRow In DGVListaMaterialSW.Rows

                Dim valorEnderecoArquivo As String = If(row.Cells("EnderecoArquivo").Value, "").ToString()
                Dim valorProdutoPrincipal As String = If(row.Cells("ProdutoPrincipal").Value, "").ToString()

                Dim iconeParaOS As Image = imgSemIcone
                Dim iconeParaDXF As Image = imgSemIcone
                Dim iconeParaPDF As Image = imgSemIcone

                If valorEnderecoArquivo.EndsWith(".ASM", StringComparison.OrdinalIgnoreCase) Then
                    iconeParaOS = imgMontagemSW
                End If

                If valorProdutoPrincipal.IndexOf("SIM", StringComparison.OrdinalIgnoreCase) >= 0 Then
                    iconeParaOS = imgSWPrincipal
                ElseIf valorEnderecoArquivo.EndsWith(".PAR", StringComparison.OrdinalIgnoreCase) OrElse
                       valorEnderecoArquivo.EndsWith(".PSM", StringComparison.OrdinalIgnoreCase) Then
                    iconeParaOS = imgMontagemPRT
                ElseIf valorEnderecoArquivo = "" Then
                    iconeParaOS = imgDiversos
                End If

                If valorEnderecoArquivo <> "" Then
                    ' ----------- DXF Check -----------
                    If valorEnderecoArquivo.EndsWith(".PRA", StringComparison.OrdinalIgnoreCase) OrElse
                       valorEnderecoArquivo.EndsWith(".PSM", StringComparison.OrdinalIgnoreCase) OrElse
                       valorEnderecoArquivo.EndsWith(".PAR", StringComparison.OrdinalIgnoreCase) Then

                        Dim strDxf As String = Path.ChangeExtension(valorEnderecoArquivo, ".dxf")
                        Dim dExiste As Boolean

                        If Not cacheDXF.TryGetValue(strDxf, dExiste) Then
                            dExiste = File.Exists(strDxf)
                            cacheDXF(strDxf) = dExiste
                        End If

                        If dExiste Then iconeParaDXF = imgDXF
                    End If

                    ' ----------- PDF Check -----------
                    If valorEnderecoArquivo.EndsWith(".PAR", StringComparison.OrdinalIgnoreCase) OrElse
                       valorEnderecoArquivo.EndsWith(".PSM", StringComparison.OrdinalIgnoreCase) OrElse
                       valorEnderecoArquivo.EndsWith(".ASM", StringComparison.OrdinalIgnoreCase) Then

                        Dim strPdf As String = Path.ChangeExtension(valorEnderecoArquivo, ".pdf")
                        Dim pExiste As Boolean

                        If Not cachePDF.TryGetValue(strPdf, pExiste) Then
                            pExiste = File.Exists(strPdf)
                            cachePDF(strPdf) = pExiste
                        End If

                        If pExiste Then iconeParaPDF = imgPDF
                    End If
                End If

                row.Cells("dgvIconeItemOS").Value = iconeParaOS
                row.Cells("DGVDXF1").Value = iconeParaDXF
                row.Cells("DGVPDF1").Value = iconeParaPDF

                ' Atualização da barra de progressão
                count_prog += 1
                ProgressBarOSM.Value = count_prog
                If count_prog Mod 20 = 0 Then
                    ProgressBarOSM.Refresh() ' Garante que a barra não congele visualmente
                End If
            Next

            DGVListaMaterialSW.ResumeLayout()

            ' Finaliza a barra de progresso
            ProgressBarOSM.Visible = False

            If FormataDGVListaMaterialSW = False Then
                DGVListaMaterialSW.Columns("IdOrdemServicoItem").Visible = False
                DGVListaMaterialSW.Columns("IdOrdemServico").Visible = False
                DGVListaMaterialSW.Columns("Projeto").Visible = False
                DGVListaMaterialSW.Columns("Tag").Visible = False
                DGVListaMaterialSW.Columns("Estatus_OrdemServico").Visible = False
                DGVListaMaterialSW.Columns("IdMaterial").Visible = False
                DGVListaMaterialSW.Columns("QtdeTotal").Visible = True
                DGVListaMaterialSW.Columns("CriadoPor").Visible = False
                DGVListaMaterialSW.Columns("DataCriacao").Visible = False
                DGVListaMaterialSW.Columns("Estatus").Visible = False
                DGVListaMaterialSW.Columns("Acabamento").Visible = True
                DGVListaMaterialSW.Columns("D_E_L_E_T_E").Visible = False
                DGVListaMaterialSW.Columns("OrdemServicoItemFinalizado").Visible = False
                DGVListaMaterialSW.Columns("IdEmpresa").Visible = False
                DGVListaMaterialSW.Columns("idProjeto").Visible = False
                DGVListaMaterialSW.Columns("IdTag").Visible = False
                DGVListaMaterialSW.Columns("DescResumo").Visible = True
                DGVListaMaterialSW.Columns("DescDetal").Visible = False
                DGVListaMaterialSW.Columns("Autor").Visible = False
                DGVListaMaterialSW.Columns("Palavrachave").Visible = False
                DGVListaMaterialSW.Columns("Notas").Visible = False
                DGVListaMaterialSW.Columns("Espessura").Visible = True
                DGVListaMaterialSW.Columns("AreaPintura").Visible = False
                DGVListaMaterialSW.Columns("NumeroDobras").Visible = False
                DGVListaMaterialSW.Columns("Peso").Visible = False
                DGVListaMaterialSW.Columns("Unidade").Visible = False
                DGVListaMaterialSW.Columns("UnidadeSW").Visible = False
                DGVListaMaterialSW.Columns("ValorSW").Visible = False
                DGVListaMaterialSW.Columns("Altura").Visible = False
                DGVListaMaterialSW.Columns("Largura").Visible = False
                DGVListaMaterialSW.Columns("CodMatFabricante").Visible = True
                DGVListaMaterialSW.Columns("DtCad").Visible = False
                DGVListaMaterialSW.Columns("UsuarioCriacao").Visible = False
                DGVListaMaterialSW.Columns("UsuarioAlteracao").Visible = False
                DGVListaMaterialSW.Columns("DtAlteracao").Visible = False
                DGVListaMaterialSW.Columns("MaterialSW").Visible = False
                DGVListaMaterialSW.Columns("Fator").Visible = True
                DGVListaMaterialSW.Columns("qtde").Visible = True
                DGVListaMaterialSW.Columns("txtSoldagem").Visible = False
                DGVListaMaterialSW.Columns("txtTipoDesenho").Visible = False
                DGVListaMaterialSW.Columns("txtCorte").Visible = False
                DGVListaMaterialSW.Columns("txtDobra").Visible = False
                DGVListaMaterialSW.Columns("txtSolda").Visible = False
                DGVListaMaterialSW.Columns("txtPintura").Visible = False
                DGVListaMaterialSW.Columns("txtMontagem").Visible = False
                DGVListaMaterialSW.Columns("QtdeRomaneio").Visible = False
                DGVListaMaterialSW.Columns("Liberado_Engenharia").Visible = False
                DGVListaMaterialSW.Columns("Data_Liberacao_Engenharia").Visible = False
                DGVListaMaterialSW.Columns("descempresa").Visible = False
                DGVListaMaterialSW.Columns("ProdutoPrincipal").Visible = False
                DGVListaMaterialSW.Columns("RNC").Visible = False
                DGVListaMaterialSW.Columns("Comprimentocaixadelimitadora").Visible = False
                DGVListaMaterialSW.Columns("Larguracaixadelimitadora").Visible = False
                DGVListaMaterialSW.Columns("Espessuracaixadelimitadora").Visible = False
                DGVListaMaterialSW.Columns("txtItemEstoque").Visible = False
                DGVListaMaterialSW.Columns("AreaPinturaUnitario").Visible = False
                DGVListaMaterialSW.Columns("PesoUnitario").Visible = False
                DGVListaMaterialSW.Columns("DataPrevisao").Visible = False
                DGVListaMaterialSW.Columns("EnderecoArquivoItemOrdemServico").Visible = False
                DGVListaMaterialSW.Columns("EnderecoArquivo").Visible = False

                Cursor.Current = Cursors.Default
            End If
        End If
        FormataDGVListaMaterialSW = True

    End Sub

    Private Sub DGVListaMaterialSW_DoubleClick(sender As Object, e As EventArgs) Handles DGVListaMaterialSW.DoubleClick

        Dim ArquivoListaBom As String = DGVListaMaterialSW.CurrentRow.Cells("EnderecoArquivo").Value.ToString

        ' Obtém o caminho completo
        ArquivoListaBom = Path.GetFullPath(ArquivoListaBom)

        ' Verifica se o arquivo existe e o abre
        If File.Exists(ArquivoListaBom) Then
            Process.Start(ArquivoListaBom)

        End If

    End Sub



    Private Sub btnConfiguracoes_Click(sender As Object, e As EventArgs) Handles btnConfiguracoes.Click

        cl_BancoDados.BuscarArquivoconf()


    End Sub

    Private Sub chkMostraLiberadasPelaEngenharia_CheckedChanged(sender As Object, e As EventArgs) Handles chkMostraLiberadasPelaEngenharia.CheckedChanged
        Timerdgvos.Enabled = True
    End Sub

    Private Sub btnAplicarAcabamento_Click(sender As Object, e As EventArgs) Handles btnAplicarAcabamento.Click

        If DGVListaMaterialSW.Rows.Count > 0 Then

            For i As Integer = 0 To DGVListaMaterialSW.Rows.Count - 1

                If Convert.ToBoolean(DGVListaMaterialSW.Rows(i).Cells("dgvSelecao").Value) = True Then

                    Try

                        cl_BancoDados.AlteracaoEspecifica("ordemservicoitem", "Acabamento", cboOpcoesAcabamento.Text.ToUpper.Trim, "IDOrdemServicoItem", DGVListaMaterialSW.Rows(i).Cells("IDOrdemServicoItem").Value.ToString)

                        DGVListaMaterialSW.Rows(i).Cells("Acabamento").Value = cboOpcoesAcabamento.Text.ToUpper.Trim

                        DGVListaMaterialSW.Rows(i).Cells("dgvSelecao").Value = False
                    Catch ex As Exception
                        '  MsgBox(ex.Message)
                    Finally
                    End Try
                End If

            Next
        End If
    End Sub

    Private Sub txtPesqNumeroDesenho_TextChanged(sender As Object, e As EventArgs) Handles txtPesqNumeroDesenho.TextChanged
        TimerDGVListaMaterialSW.Enabled = True
    End Sub

    Private Sub txtPesqTipoDesenho_TextChanged(sender As Object, e As EventArgs) Handles txtPesqTipoDesenho.TextChanged
        TimerDGVListaMaterialSW.Enabled = True
    End Sub

    Private Sub txtPesqAcabamentoDesenho_TextChanged(sender As Object, e As EventArgs) Handles txtPesqAcabamentoDesenho.TextChanged
        TimerDGVListaMaterialSW.Enabled = True
    End Sub

    Private Sub txtPesqCriadoPor_TextChanged(sender As Object, e As EventArgs) Handles txtPesqCriadoPor.TextChanged

        Timerdgvos.Enabled = True
    End Sub

    Private Sub btnAtualizacoesDados_Click(sender As Object, e As EventArgs) Handles btnAtualizacoesDados.Click
        AtualziarDadosSinco()
    End Sub



    Private Sub TimerdgvProcesso_Tick(sender As Object, e As EventArgs) Handles TimerdgvProcesso.Tick




        dgvProcessos.DataSource = cl_BancoDados.CarregarDados("SELECT IdProcessoFabricacao, CodigoProcessoFabricacao, processofabricacao, CriadoPor, 
DataCriacao, D_E_L_E_T_E, DataD_E_L_E_T_E,
UsuarioD_E_L_E_T_E, DataLiberada, Fabrica FROM processofabricacao
where  (D_E_L_E_T_E = '' or D_E_L_E_T_E is null) and processofabricacao like '%" & txtPesqProcesso.Text & "%' order by CodigoProcessoFabricacao")

        dgvProcessos.Columns("IdProcessoFabricacao").Visible = False
        dgvProcessos.Columns("CriadoPor").Visible = False
        dgvProcessos.Columns("DataCriacao").Visible = False
        dgvProcessos.Columns("D_E_L_E_T_E").Visible = False
        dgvProcessos.Columns("DataD_E_L_E_T_E").Visible = False
        dgvProcessos.Columns("UsuarioD_E_L_E_T_E").Visible = False
        dgvProcessos.Columns("DataLiberada").Visible = False
        dgvProcessos.Columns("Fabrica").Visible = False

        TimerdgvProcesso.Enabled = False

    End Sub

    Private Sub txtPesqProcesso_TextChanged(sender As Object, e As EventArgs) Handles txtPesqProcesso.TextChanged


        TimerdgvProcesso.Enabled = True

    End Sub


    Private Sub ToolStripTextBox1_TextChanged(sender As Object, e As EventArgs) Handles ToolStripTextBox1.TextChanged


        cl_BancoDados.ComboBoxDataSet("projetos", "idProjeto", "Projeto",
                                      cboProjeto, " WHERE (D_E_L_E_T_E Is NULL Or D_E_L_E_T_E = '') and
                                                                            (Finalizado = '' OR Finalizado Is NULL) and (Liberado = 'S') and Projeto like '%" & ToolStripTextBox1.Text & "%'")
    End Sub

    Private Sub DGVListaMaterialSW_Click(sender As Object, e As EventArgs) Handles DGVListaMaterialSW.Click

        Try

            If DGVListaMaterialSW.CurrentRow.Cells("dgvSelecao").Value = False Then

                DGVListaMaterialSW.CurrentRow.Cells("dgvSelecao").Value = True

            ElseIf DGVListaMaterialSW.CurrentRow.Cells("dgvSelecao").Value = True Then

                DGVListaMaterialSW.CurrentRow.Cells("dgvSelecao").Value = False

            End If

            OrdemServico.CodMatFabricante = DGVListaMaterialSW.CurrentRow.Cells("CodMatFabricante").Value.ToString

            OrdemServico.DescDetal = DGVListaMaterialSW.CurrentRow.Cells("DescDetal").Value.ToString
            OrdemServico.DescResumo = DGVListaMaterialSW.CurrentRow.Cells("DescResumo").Value.ToString




        Catch ex As Exception
        Finally

        End Try

    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles btnCriaPropriedadePadroes.Click

        AtivarVariaveisESincronizarPropriedades()

        CarregarPropriedadesArquivoCorrente()


    End Sub


    'Public Sub AtivarVariaveisESincronizarPropriedades()
    '    Try
    '        '────────────────────────────────────────────
    '        ' 🔹 Obter Solid Edge ativo e documento corrente
    '        '────────────────────────────────────────────
    '        ' Dim seApp As SolidEdgeFramework.Application =
    '        ' Marshal.GetActiveObject("SolidEdge.Application")

    '        If app Is Nothing OrElse app.Documents.Count = 0 Then Exit Sub
    '        Dim doc As Object = app.ActiveDocument
    '        If doc Is Nothing Then Exit Sub

    '        Dim caminho As String = doc.FullName
    '        Dim extensao As String = Path.GetExtension(caminho).ToLower()

    '        '────────────────────────────────────────────
    '        ' ⚙️ VARIÁVEIS A SEREM GARANTIDAS
    '        '────────────────────────────────────────────
    '        Dim varsNecessarias As New List(Of String) From {"Custom - Mass_0"}

    '        If extensao = ".psm" OrElse extensao = ".par" Then
    '            varsNecessarias.Add("Flat_Pattern_Model_CutSizeX")
    '            varsNecessarias.Add("Flat_Pattern_Model_CutSizeY")
    '        End If

    '        '────────────────────────────────────────────
    '        ' 🧩 ACESSA VARIÁVEIS DO DOCUMENTO
    '        '────────────────────────────────────────────
    '        Dim variables As SolidEdgeFramework.Variables = Nothing

    '        Try
    '            variables = doc.Variables
    '        Catch ex As Exception
    '            MessageBox.Show("Não foi possível acessar a lista de variáveis do documento." & vbCrLf & ex.Message,
    '                            "SINCO - Solid Edge", MessageBoxButtons.OK, MessageBoxIcon.Error)
    '            Exit Sub
    '        End Try

    '        Dim encontrouEAtivou As Boolean = False

    '        '────────────────────────────────────────────
    '        ' 🔍 VERIFICA E ATIVA VARIÁVEIS NECESSÁRIAS
    '        '────────────────────────────────────────────
    '        For Each varObj As Object In variables
    '            Try
    '                Dim nomeVar As String = ""
    '                Try
    '                    nomeVar = varObj.Name
    '                Catch
    '                    Continue For
    '                End Try

    '                If varsNecessarias.Contains(nomeVar) Then
    '                    Dim estadoAtivo As Boolean = False

    '                    ' 🔹 Tenta ler o estado atual (IsActive)
    '                    Try
    '                        estadoAtivo = CBool(CallByName(varObj, "IsActive", CallType.Get))
    '                    Catch
    '                        ' A propriedade IsActive pode não existir ou não ser acessível
    '                        Debug.WriteLine($"⚠️ '{nomeVar}' não possui propriedade IsActive (provavelmente variável calculada).")
    '                        Continue For
    '                    End Try

    '                    ' 🔹 Se não estiver ativa, ativa
    '                    If Not estadoAtivo Then
    '                        Try
    '                            CallByName(varObj, "IsActive", CallType.Let, True)
    '                            encontrouEAtivou = True
    '                            Debug.WriteLine($"🔧 Variável '{nomeVar}' ativada com sucesso.")
    '                        Catch ex2 As Exception
    '                            Debug.WriteLine($"⚠️ Falha ao ativar '{nomeVar}': {ex2.Message}")
    '                        End Try
    '                    Else
    '                        Debug.WriteLine($"✅ Variável '{nomeVar}' já ativa.")
    '                    End If
    '                End If

    '            Catch ex As Exception
    '                Debug.WriteLine($"⚠️ Erro ao verificar variável: {ex.Message}")
    '            End Try
    '        Next


    '        '────────────────────────────────────────────
    '        ' 🧠 APLICA AS MUDANÇAS NAS VARIÁVEIS
    '        '────────────────────────────────────────────
    '        'If encontrouEAtivou Then

    '        If encontrouEAtivou Then
    '                Try
    '                    ' ❌ NÃO USE variables.Edit() sem parâmetros
    '                    variables.Update()
    '                    variables.Apply()
    '                    Debug.WriteLine("✅ Variáveis aplicadas e atualizadas com sucesso.")
    '                Catch ex As Exception
    '                    Debug.WriteLine($"⚠️ Falha ao aplicar variáveis: {ex.Message}")
    '                End Try
    '            End If


    '        ' End If

    '        '────────────────────────────────────────────
    '        ' ⚙️ ATUALIZA PROPRIEDADES FÍSICAS (MASSA, ÁREA, ETC)
    '        '────────────────────────────────────────────
    '        Try
    '            doc.UpdatePhysicalProperties()
    '            Debug.WriteLine("📏 Propriedades físicas recalculadas.")
    '        Catch ex As Exception
    '            Debug.WriteLine($"⚠️ Falha ao atualizar propriedades físicas: {ex.Message}")
    '        End Try

    '        '────────────────────────────────────────────
    '        ' 🧾 GARANTE PROPRIEDADE CUSTOM: Tipo de desenho
    '        '────────────────────────────────────────────
    '        Dim propSetsCustom As Object = doc.Properties("Custom")
    '        Dim nomePropCustom As String = "Tipo de desenho"
    '        Dim valorPadrao As String = If(extensao = ".asm", "CONJUNTO DE MONTAGEM", "CHAPARIA")

    '        Dim propExiste As Boolean = False
    '        Dim propValorAtual As String = ""

    '        For Each prop As Object In propSetsCustom
    '            If String.Equals(prop.Name, nomePropCustom, StringComparison.OrdinalIgnoreCase) Then
    '                propExiste = True
    '                propValorAtual = If(prop.Value IsNot Nothing, prop.Value.ToString().Trim(), "")
    '                Exit For
    '            End If
    '        Next

    '        If propExiste Then
    '            If String.IsNullOrWhiteSpace(propValorAtual) Then
    '                For Each prop As Object In propSetsCustom
    '                    If String.Equals(prop.Name, nomePropCustom, StringComparison.OrdinalIgnoreCase) Then
    '                        prop.Value = valorPadrao
    '                        Debug.WriteLine($"🔧 Propriedade '{nomePropCustom}' atualizada para '{valorPadrao}'.")
    '                        Exit For
    '                    End If
    '                Next
    '            Else
    '                Debug.WriteLine($"✅ '{nomePropCustom}' já preenchida com '{propValorAtual}'.")
    '            End If
    '        Else
    '            propSetsCustom.Add(nomePropCustom, valorPadrao)
    '            Debug.WriteLine($"➕ '{nomePropCustom}' criada com valor '{valorPadrao}'.")
    '        End If

    '        '────────────────────────────────────────────
    '        ' 💾 SALVA E SINCRONIZA
    '        '────────────────────────────────────────────
    '        Try : doc.UpdatePropertyTextDisplay() : Catch : End Try
    '        Try : doc.Properties.Save() : Catch : End Try
    '        Try : doc.Save() : Catch : End Try

    '        '────────────────────────────────────────────
    '        ' 📋 LOG FINAL
    '        '────────────────────────────────────────────
    '        Debug.WriteLine("═══════════════════════════════════════════════")
    '        Debug.WriteLine($"📄 Documento: {Path.GetFileName(caminho)}")
    '        Debug.WriteLine($"Extensão: {extensao}")
    '        Debug.WriteLine($"Variáveis garantidas: {String.Join(", ", varsNecessarias)}")
    '        Debug.WriteLine("✅ Ativação concluída com sucesso.")
    '        Debug.WriteLine("═══════════════════════════════════════════════")

    '    Catch ex As Exception
    '        MessageBox.Show("Erro ao ativar variáveis e propriedades: " & ex.Message,
    '                        "SINCO - Solid Edge", MessageBoxButtons.OK, MessageBoxIcon.Error)
    '    End Try
    'End Sub



    '======================== FUNÇÃO PRINCIPAL ========================



    Public Sub AtivarVariaveisESincronizarPropriedades(Optional valorBloqueado As String = Nothing)
        Try

            ' 1) Garante que app aponta para a instância correta do Solid Edge
            If app Is Nothing Then
                Try
                    app = CType(Marshal.GetActiveObject("SolidEdge.Application"),
                            SolidEdgeFramework.Application)
                Catch ex As Exception
                    MessageBox.Show("Não foi possível localizar uma instância do Solid Edge aberta." &
                                Environment.NewLine &
                                "Detalhe: " & ex.Message,
                                "Solid Edge",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error)
                    Exit Sub
                End Try
            End If

            ' 2) Verifica se há documentos abertos
            Dim qtdDocs As Integer

            Try
                qtdDocs = app.Documents.Count
            Catch ex As Exception
                MessageBox.Show("Erro ao acessar app.Documents.Count: " & ex.Message,
                            "Solid Edge",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error)
                Exit Sub
            End Try

            If qtdDocs = 0 Then
                MessageBox.Show("Nenhum documento de Solid Edge está aberto.",
                            "Solid Edge",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information)
                Exit Sub
            End If

            ' 3) Pega o documento ativo
            Dim doc As Object = Nothing

            Try
                doc = app.ActiveDocument
            Catch ex As Exception
                MessageBox.Show("Erro ao acessar o documento ativo: " & ex.Message,
                            "Solid Edge",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error)
                Exit Sub
            End Try

            If doc Is Nothing Then
                MessageBox.Show("Nenhum documento ativo encontrado no Solid Edge.",
                            "Solid Edge",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information)
                Exit Sub
            End If

            ' 4) Daqui pra baixo você já tem um documento ativo garantido
            '    coloque aqui a lógica de sincronizar variáveis / propriedades
            'Exemplo:
            'SincronizarVariaveis(doc, valorBloqueado)

            Dim caminho As String = doc.FullName
            Dim extensao As String = Path.GetExtension(caminho).ToLowerInvariant()

            ' ---------- LISTA DE VARIÁVEIS CALCULADAS A GARANTIR ----------
            ' Mass_0 = para todos os tipos
            ' Área_de_superfície = mesmo tratamento de Mass_0 (todos os tipos)
            ' CutSizeX / CutSizeY = apenas .psm / .par
            Dim varsAlvos As New List(Of String) From {
                "Mass_0",
                "Área_de_superfície",      ' nome com acento
                "Area_de_superficie"       ' sinônimo sem acento
            }
            If extensao = ".psm" OrElse extensao = ".par" Then
                varsAlvos.Add("Flat_Pattern_Model_CutSizeX")
                varsAlvos.Add("Flat_Pattern_Model_CutSizeY")
            End If

            ' ---------- ACESSA VARIÁVEIS ----------
            Dim variables As Object = Nothing
            Try
                variables = doc.Variables
            Catch ex As Exception
                MessageBox.Show("Não foi possível acessar a lista de variáveis do documento." & vbCrLf & ex.Message,
                                "SINCO - Solid Edge", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub
            End Try

            ' ---------- Helper: achar variável por nome normalizado / sinônimos ----------
            Dim TryGetVariable As Func(Of IEnumerable(Of String), Object) =
                Function(nomes As IEnumerable(Of String)) As Object
                    Dim alvoNorms = nomes.Select(Function(x) NormalizeName(x)).Distinct().ToArray()
                    Try
                        For Each v As Object In variables
                            Try
                                Dim n As String = ""
                                Try : n = CStr(CallByName(v, "Name", CallType.Get)) : Catch : n = "" : End Try
                                If String.IsNullOrEmpty(n) Then Continue For
                                Dim vn = NormalizeName(n)
                                If alvoNorms.Contains(vn) Then Return v
                            Catch
                            End Try
                        Next
                    Catch
                    End Try
                    Return Nothing
                End Function

            ' ---------- se .psm: garanta Flat Pattern (para CutSizeX/Y aparecerem) ----------
            If String.Equals(extensao, ".psm", StringComparison.OrdinalIgnoreCase) Then
                Try
                    Dim sheetDoc = TryCast(doc, SolidEdgePart.SheetMetalDocument)
                    If sheetDoc IsNot Nothing Then
                        Dim fps = sheetDoc.FlatPatternModels
                        If fps IsNot Nothing AndAlso fps.Count = 0 Then
                            app.StartCommand(1494) ' Create Flat Pattern
                            Threading.Thread.Sleep(500)
                            app.DoIdle()
                        End If
                    End If
                Catch
                End Try
            End If

            ' ---------- ativa/exponhe cada variável alvo ----------
            Dim ativouAlgo As Boolean = False

            ' mapa de sinônimos para melhorar a busca
            Dim sinonimos As New Dictionary(Of String, String()) From {
                {"Mass_0", New String() {"Mass_0", "Mass0"}},
                {"Área_de_superfície", New String() {"Área_de_superfície", "Area_de_superficie", "Area de superficie", "SurfaceArea"}},
                {"Flat_Pattern_Model_CutSizeX", New String() {"Flat_Pattern_Model_CutSizeX", "FlatPatternModelCutSizeX", "CutSizeX"}},
                {"Flat_Pattern_Model_CutSizeY", New String() {"Flat_Pattern_Model_CutSizeY", "FlatPatternModelCutSizeY", "CutSizeY"}}
            }

            For Each alvo In varsAlvos
                Dim nomesBusca As IEnumerable(Of String) =
                    If(sinonimos.ContainsKey(alvo), sinonimos(alvo), New String() {alvo})

                Dim v As Object = TryGetVariable(nomesBusca)

                ' tente atualizar o doc e procurar de novo (caso de CutSize/Area aparecer após update)
                If v Is Nothing Then
                    Try : CallByName(doc, "Update", CallType.Method) : Catch : End Try
                    app.DoIdle()
                    v = TryGetVariable(nomesBusca)
                End If

                If v Is Nothing Then
                    Debug.WriteLine($"⚠️ Variável '{alvo}' não encontrada (agora).")
                    Continue For
                End If

                Dim mudou As Boolean = False

                ' Tenta IsActive
                Try
                    Dim atual As Boolean = True
                    Try : atual = CBool(CallByName(v, "IsActive", CallType.Get)) : Catch : atual = True : End Try
                    If Not atual Then
                        CallByName(v, "IsActive", CallType.Let, True)
                        mudou = True
                    End If
                Catch
                End Try

                ' Tenta Expose
                Try
                    Dim exposto As Boolean = True
                    Try : exposto = CBool(CallByName(v, "Expose", CallType.Get)) : Catch : exposto = True : End Try
                    If Not exposto Then
                        CallByName(v, "Expose", CallType.Let, True)
                        mudou = True
                    End If
                Catch
                End Try

                ' Tenta IsIncluded
                Try
                    Dim inc As Boolean = True
                    Try : inc = CBool(CallByName(v, "IsIncluded", CallType.Get)) : Catch : inc = True : End Try
                    If Not inc Then
                        CallByName(v, "IsIncluded", CallType.Let, True)
                        mudou = True
                    End If
                Catch
                End Try

                If mudou Then ativouAlgo = True
                Debug.WriteLine($"✅ Variável '{alvo}' disponível. Mudou estado: {mudou}")
            Next

            If ativouAlgo Then
                Try : CallByName(variables, "Update", CallType.Method) : Catch : End Try
                Try : CallByName(variables, "Apply", CallType.Method) : Catch : End Try
            End If

            ' ---------- recalc properties ----------
            Try : CallByName(doc, "UpdatePhysicalProperties", CallType.Method) : Catch : End Try

            ' ---------- customs: Tipo de desenho ----------
            Dim propSetsCustom As Object = doc.Properties("Custom")
            Dim nomePropTipo As String = "Tipo de desenho"
            Dim valorTipo As String = If(extensao = ".asm", "CONJUNTO DE MONTAGEM", "CHAPARIA")
            GarantirPropriedadeCustom(propSetsCustom, nomePropTipo, valorTipo, soSeVazio:=True)

            ' ---------- custom: Bloqueado (SIM/NÃO) ----------
            If String.IsNullOrWhiteSpace(valorBloqueado) Then
                GarantirPropriedadeCustom(propSetsCustom, "Bloqueado", "NÃO", soSeVazio:=True)
            Else
                Dim vBloq = If(valorBloqueado.Trim().ToUpperInvariant() = "SIM", "SIM", "NÃO")
                GarantirPropriedadeCustom(propSetsCustom, "Bloqueado", vBloq, soSeVazio:=False)
            End If

            ' ---------- sincroniza ----------
            Try : CallByName(doc, "UpdatePropertyTextDisplay", CallType.Method) : Catch : End Try
            Try : doc.Properties.Save() : Catch : End Try
            Try : doc.Save() : Catch : End Try

            Debug.WriteLine("═══════════════════════════════════════════════")
            Debug.WriteLine($"📄 {Path.GetFileName(caminho)}  → variáveis garantidas (Mass_0 / Área_de_superfície / CutSizeX/Y se aplicável)")
            Debug.WriteLine("✅ Rotina concluída.")
            Debug.WriteLine("═══════════════════════════════════════════════")

        Catch ex As Exception
            MessageBox.Show("Erro ao ativar variáveis/propriedades: " & ex.Message,
                            "SINCO - Solid Edge", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub




    '------------------------ helpers de normalização ------------------------
    Private Function StripDiacritics(s As String) As String
        If String.IsNullOrEmpty(s) Then Return ""
        Dim norm = s.Normalize(NormalizationForm.FormD)
        Dim sb As New StringBuilder(norm.Length)
        For Each ch In norm
            If CharUnicodeInfo.GetUnicodeCategory(ch) <> UnicodeCategory.NonSpacingMark Then
                sb.Append(ch)
            End If
        Next
        Return sb.ToString().Normalize(NormalizationForm.FormC)
    End Function



    'Private Function NormalizeName(s As String) As String
    '    If String.IsNullOrEmpty(s) Then Return ""
    '    Dim t = s.Trim()

    '    ' remove prefixo "Custom - "
    '    If t.StartsWith("Custom - ", StringComparison.OrdinalIgnoreCase) Then
    '        t = t.Substring(9).Trim()
    '    End If

    '    t = StripDiacritics(t)        ' tira acentos
    '    t = t.Replace("_", "").Replace("-", "").Replace(" ", "")
    '    Return t.ToLowerInvariant()
    'End Function

    '------------------------ propriedade custom genérica ------------------------
    Private Sub GarantirPropriedadeCustom(customSet As Object,
                                          nome As String,
                                          valor As String,
                                          Optional soSeVazio As Boolean = True)
        Try
            Dim existe As Boolean = False
            For Each p As Object In customSet
                Try
                    If String.Equals(CStr(p.Name), nome, StringComparison.OrdinalIgnoreCase) Then
                        existe = True
                        Dim atual As String = ""
                        Try : atual = If(p.Value IsNot Nothing, p.Value.ToString().Trim(), "") : Catch : atual = "" : End Try
                        If soSeVazio Then
                            If String.IsNullOrWhiteSpace(atual) Then p.Value = valor
                        Else
                            p.Value = valor
                        End If
                        Exit For
                    End If
                Catch
                End Try
            Next
            If Not existe Then
                Try
                    customSet.Add(nome, valor)
                Catch ex As Exception
                    Debug.WriteLine($"⚠️ Falha ao criar propriedade '{nome}': {ex.Message}")
                End Try
            End If
        Catch
        End Try
    End Sub

    Private Sub btnConverterChapa_Click(sender As Object, e As EventArgs) Handles btnConverterChapa.Click
        ConverterParOuPsm_AjustarDobras()
    End Sub

    Public Sub ConverterParOuPsm_AjustarDobras(Optional kFactorDesejado As Double = 0.485)
        Dim app As SolidEdgeFramework.Application = Nothing
        Dim docObj As Object = Nothing

        ' IDs de comando (podem variar por versão/linguagem – tentamos vários)
        Dim CMD_CONVERTER_PARA_CHAPA() As Integer = {11045, 11044, 11046} ' "Peça para Peça em Chapa"
        Dim CMD_MODO_SINCRONO() As Integer = {18946, 18945, 18944}       ' Alternar para Síncrono

        Try
            ' 1) Solid Edge + documento ativo
            app = CType(Marshal.GetActiveObject("SolidEdge.Application"), SolidEdgeFramework.Application)
            If app Is Nothing OrElse app.Documents.Count = 0 Then
                MsgBox("Nenhum documento aberto.", vbExclamation, "SINCO")
                Exit Sub
            End If

            docObj = app.ActiveDocument
            Dim fullName As String = CStr(CallByName(docObj, "FullName", CallType.Get))
            Dim ext As String = IO.Path.GetExtension(fullName).ToLowerInvariant()

            ' 2) Se for PAR (peça “sólida”), tentar converter para chapa
            Dim sheet As SheetMetalDocument = TryCast(docObj, SheetMetalDocument)
            If sheet Is Nothing AndAlso (TypeOf docObj Is PartDocument OrElse ext = ".par") Then
                For Each id In CMD_CONVERTER_PARA_CHAPA
                    Try : app.StartCommand(id) : app.DoIdle() : Threading.Thread.Sleep(500) : Exit For : Catch : End Try
                Next
                ' Reobtém doc ativo (pode ter trocado após converter)
                sheet = TryCast(app.ActiveDocument, SheetMetalDocument)
            End If

            ' Se ainda não é chapa, não dá pra ajustar dobras
            If sheet Is Nothing Then
                MsgBox("Este arquivo não é de chapa. Converta para .PSM e tente novamente.", vbExclamation, "SINCO")
                Exit Sub
            End If

            ' 3) (Opcional) forçar modo Síncrono
            For Each id In CMD_MODO_SINCRONO
                Try : app.StartCommand(id) : app.DoIdle() : Threading.Thread.Sleep(250) : Exit For : Catch : End Try
            Next

            ' 4) Descobrir ESPESSURA (tenta variáveis comuns e SheetMetalData)
            Dim variables As Object = Nothing
            Try : variables = sheet.Variables : Catch : variables = Nothing : End Try

            Dim espessuraMm As Double = 0
            Dim nomeVarEsp As String = ""

            Dim candidatosEsp As String() = {
            "Material Thickness", "Espessura do Material",
            "Thickness", "Espessura", "SheetMetal.Thickness"
        }

            If variables IsNot Nothing Then
                For Each v As Object In variables
                    Dim n As String = SafeGetName(v)
                    If n = "" Then Continue For
                    If candidatosEsp.Any(Function(c) String.Equals(n, c, StringComparison.OrdinalIgnoreCase)) Then
                        Dim val = SafeGetValue(v)
                        Dim num As Double
                        If TryParseNumero(val, num) Then
                            espessuraMm = num
                            nomeVarEsp = n
                            Exit For
                        End If
                    End If
                Next
            End If

            If espessuraMm <= 0 Then
                Try
                    Dim sm = sheet.SheetMetalData
                    ' Muitas instalações trazem em metros; se for bem pequeno, converte p/ mm
                    espessuraMm = sm.Thickness
                    If espessuraMm < 0.01 Then espessuraMm *= 1000.0R
                    nomeVarEsp = "SheetMetalData.Thickness"
                Catch
                End Try
            End If

            If espessuraMm <= 0 Then
                ' MsgBox("Não foi possível identificar a espessura da chapa.", vbExclamation, "SINCO")

                espessuraMm = InputBox("Não foi possível identificar a espessura da chapa, Informe a espessura da chapa em mm:", "SINCO - Espessura da Chapa", "1.0")

            End If


            If espessuraMm <= 0 Then
                MsgBox("Não foi possível identificar a espessura da chapa.", vbExclamation, "SINCO")
                Exit Sub
            End If


            ' 5) Definir BendRadius = espessura e K-Factor = 0,485
            Dim alterou As Boolean = False
            Dim exprEsp As String =
            If(String.IsNullOrWhiteSpace(nomeVarEsp),
               espessuraMm.ToString("0.###", CultureInfo.InvariantCulture),
               nomeVarEsp)

            ' (a) Padrões globais
            alterou = SetOrCreateVar(variables, "BendRadius", exprEsp) Or alterou
            alterou = SetOrCreateVar(variables, "NeutralFactor", kFactorDesejado.ToString(CultureInfo.InvariantCulture)) Or alterou
            alterou = SetOrCreateVar(variables, "Fator_Neutro", kFactorDesejado.ToString(CultureInfo.InvariantCulture)) Or alterou

            ' (b) Heurística: ajustar variáveis de raio por feature
            If variables IsNot Nothing Then
                For Each v As Object In variables
                    Dim n As String = SafeGetName(v)
                    If n = "" Then Continue For
                    Dim nn As String = RemoveDiacritics(n).ToLowerInvariant()
                    If nn.Contains("bendradius") OrElse nn.Contains("raio") AndAlso nn.Contains("dobra") Then
                        If Not SafeEditOrValue(v, n, exprEsp, espessuraMm) Then
                            ' ignora falha silenciosamente
                        Else
                            alterou = True
                        End If
                    End If
                Next
            End If

            ' 6) Aplicar/atualizar
            If alterou Then
                Try : CallByName(variables, "Update", CallType.Method) : Catch : End Try
                Try : CallByName(variables, "Apply", CallType.Method) : Catch : End Try
            End If

            Try : sheet.Update() : Catch : End Try
            Try : sheet.UpdatePhysicalProperties() : Catch : End Try
            app.DoIdle()

            MsgBox(
            $"✅ Ajustes aplicados:" & vbCrLf &
            $"• Espessura: {espessuraMm:0.###} mm ({If(nomeVarEsp = "", "valor numérico", nomeVarEsp)})" & vbCrLf &
            $"• BendRadius = {If(nomeVarEsp = "", espessuraMm.ToString("0.###"), nomeVarEsp)}" & vbCrLf &
            $"• K-factor = {kFactorDesejado:0.###}",
            vbInformation, "SINCO")

        Catch ex As Exception
            MsgBox("Erro ao converter/ajustar dobras: " & ex.Message, vbCritical, "SINCO")
        End Try
    End Sub

    '==================== Helpers ====================

    'Private Function SafeGetName(o As Object) As String
    '    Try : Return CStr(CallByName(o, "Name", CallType.Get)) : Catch : Return "" : End Try
    'End Function

    'Private Function SafeGetValue(o As Object) As String
    '    Try : Return CStr(CallByName(o, "Value", CallType.Get)) : Catch : Return "" : End Try
    'End Function
    Private Function TryParseNumero(txt As String, ByRef num As Double) As Boolean
        If String.IsNullOrWhiteSpace(txt) Then Return False

        ' Normaliza para minúsculas e remove unidades/espacos
        Dim s As String = txt.Trim().ToLowerInvariant()
        s = s.Replace("mm²", "").Replace("mm2", "")
        s = s.Replace("mm", "")
        s = s.Replace("kg", "")
        s = s.Replace(" ", "")

        ' tenta pt-BR e Invariant
        If Double.TryParse(s, Globalization.NumberStyles.Any, New Globalization.CultureInfo("pt-BR"), num) Then
            Return True
        End If
        s = s.Replace(",", ".")
        Return Double.TryParse(s, Globalization.NumberStyles.Any, Globalization.CultureInfo.InvariantCulture, num)
    End Function

    Private Function SafeGetName(o As Object) As String
        Try : Return CStr(CallByName(o, "Name", CallType.Get)) : Catch : Return "" : End Try
    End Function

    Private Function SafeGetValue(o As Object) As String
        Try : Return CStr(CallByName(o, "Value", CallType.Get)) : Catch : Return "" : End Try
    End Function

    'Private Function RemoveDiacritics(s As String) As String
    '    If String.IsNullOrEmpty(s) Then Return ""
    '    Dim norm = s.Normalize(NormalizationForm.FormD)
    '    Dim sb As New System.Text.StringBuilder(norm.Length)
    '    For Each ch As Char In norm
    '        If Globalization.CharUnicodeInfo.GetUnicodeCategory(ch) <> Globalization.UnicodeCategory.NonSpacingMark Then
    '            sb.Append(ch)
    '        End If
    '    Next
    '    Return sb.ToString().Normalize(NormalizationForm.FormC)
    'End Function

    ' Cria (se não existir) ou atualiza uma variável de documento (fórmula ou valor)
    Private Function SetOrCreateVar(variables As Object, nome As String, formulaOuValor As String) As Boolean
        If variables Is Nothing Then Return False
        Try
            Dim alvo As Object = Nothing
            For Each v As Object In variables
                If String.Equals(SafeGetName(v), nome, StringComparison.OrdinalIgnoreCase) Then
                    alvo = v : Exit For
                End If
            Next
            If alvo Is Nothing Then
                ' Add(nome, formula) – o Solid Edge cria como variável de usuário
                CallByName(variables, "Add", CallType.Method, nome, formulaOuValor)
                Return True
            Else
                Return SafeEditOrValue(alvo, nome, formulaOuValor, Nothing)
            End If
        Catch
            Return False
        End Try
    End Function

    ' Tenta .Edit(nome, formula). Se falhar, tenta atribuir .Value
    Private Function SafeEditOrValue(varObj As Object, nome As String, formula As String, Optional valorNumerico As Double? = Nothing) As Boolean
        Try
            CallByName(varObj, "Edit", CallType.Method, nome, formula)
            Return True
        Catch
            Try
                If valorNumerico.HasValue Then
                    CallByName(varObj, "Value", CallType.Let, valorNumerico.Value)
                    Return True
                Else
                    Dim num As Double
                    If TryParseNumero(formula, num) Then
                        CallByName(varObj, "Value", CallType.Let, num)
                        Return True
                    End If
                End If
            Catch
            End Try
        End Try
        Return False
    End Function

    '' Versão única para evitar colisões com outros helpers
    'Private Function RemoveDiacritics(s As String) As String
    '    If String.IsNullOrEmpty(s) Then Return ""
    '    Dim norm = s.Normalize(NormalizationForm.FormD)
    '    Dim sb As New System.Text.StringBuilder(norm.Length)
    '    For Each ch As Char In norm
    '        If Globalization.CharUnicodeInfo.GetUnicodeCategory(ch) <> Globalization.UnicodeCategory.NonSpacingMark Then
    '            sb.Append(ch)
    '        End If
    '    Next
    '    Return sb.ToString().Normalize(NormalizationForm.FormC)
    'End Function


    Private Sub Button5_Click_1(sender As Object, e As EventArgs) Handles Button5.Click


        Dim vals = LerFisicosProfundos(app.ActiveDocument)
        ' vals.MassaKg, vals.AreaM2, vals.CutXmm, vals.CutYmm

        txtPesoKg.Text = vals.MassaKg.ToString("0.###")
        txtAreametroquadr.Text = vals.AreaM2.ToString("0.0000")
        txtCutSizex.Text = vals.CutXmm.ToString("0.###")
        txtCutSizey.Text = vals.CutYmm.ToString("0.###")


    End Sub

    ' ============================================================
    ' 📦 Leitura PROFUNDA de Massa(kg), Área(m²) e CutSizeX/Y(mm)
    '  - Funciona para PAR/PSM (e ASM com soma recursiva)
    '  - Não depende de variáveis custom estarem ativas/visíveis
    '  - Usa múltiplos fallbacks (API geométrica e DXF temporário)
    ' ============================================================
    Public Function LerFisicosProfundos(doc As Object) As (MassaKg As Double, AreaM2 As Double, CutXmm As Double, CutYmm As Double)
        If doc Is Nothing Then Return (0, 0, 0, 0)

        Dim ext As String = System.IO.Path.GetExtension(CStr(CallByName(doc, "FullName", CallType.Get))).ToLowerInvariant()
        Dim massaKg As Double = 0.0
        Dim areaM2 As Double = 0.0
        Dim cutX As Double = 0.0
        Dim cutY As Double = 0.0

        ' ---------- 0) Força recálculo ----------
        Try : CallByName(doc, "UpdatePhysicalProperties", CallType.Method) : Catch : End Try
        Try : CallByName(doc, "Update", CallType.Method) : Catch : End Try

        ' ---------- 1) Massa / Área do documento corrente ----------
        Try
            Dim r = TryLerMassaAreaDoDocumento(doc)
            massaKg = r.MassaKg
            areaM2 = r.AreaM2
        Catch
        End Try

        ' ---------- 2) Se for ASM, soma recursivamente das ocorrências ----------
        If ext = ".asm" Then
            Dim somaAsm = SomarFisicosDeAssembly(doc)
            ' Só substitui se não veio nada do doc
            If massaKg <= 0 Then massaKg = somaAsm.MassaKg
            If areaM2 <= 0 Then areaM2 = somaAsm.AreaM2
        End If

        ' ---------- 3) CutSizeX/Y para PSM/PAR ----------
        If ext = ".psm" OrElse ext = ".par" Then
            ' 3A: tentar RangeBox do FlatPattern real
            Dim bb = TryGetFlatPatternBoundingBoxMm(doc)
            cutX = bb.WidthMm
            cutY = bb.HeightMm

            ' 3B: se falhar, gerar DXF temporário e ler extents
            If cutX <= 0 OrElse cutY <= 0 Then
                Dim dxfDims = TryGetBlankFromTempDXF(doc)
                If dxfDims.WidthMm > 0 AndAlso dxfDims.HeightMm > 0 Then
                    cutX = dxfDims.WidthMm
                    cutY = dxfDims.HeightMm
                End If
            End If
        End If

        Return (Math.Round(massaKg, 6), Math.Round(areaM2, 6), Math.Round(cutX, 3), Math.Round(cutY, 3))
    End Function

    ' ============================================================
    ' Massa/Área via propriedades internas + API geométrica (modelo)
    ' ============================================================
    Private Function TryLerMassaAreaDoDocumento(doc As Object) As (MassaKg As Double, AreaM2 As Double)
        Dim massaKg As Double = 0
        Dim areaM2 As Double = 0

        ' 1) Propriedades internas (ExtendedSummaryInformation costuma ter)
        massaKg = TryGetPropAsDouble(doc, {("ExtendedSummaryInformation", "Mass"), ("ExtendedSummaryInformation", "Massa")}, 1.0)
        areaM2 = TryGetPropAsDouble(doc, {("ExtendedSummaryInformation", "SurfaceArea"), ("ExtendedSummaryInformation", "Área de superfície")}, 1.0)

        ' 2) Se ainda zerado, tenta obter do Body
        If massaKg <= 0 OrElse areaM2 <= 0 Then
            Try
                Dim models As Object = CallByName(doc, "Models", CallType.Get)
                If models IsNot Nothing AndAlso CInt(CallByName(models, "Count", CallType.Get)) > 0 Then
                    Dim model As Object = CallByName(models, "Item", CallType.Method, 1)
                    Dim body As Object = CallByName(model, "Body", CallType.Get)

                    If body IsNot Nothing Then
                        ' (a) método direto de propriedades de massa
                        Dim m As Double = 0, a As Double = 0, v As Double = 0
                        If GetBodyMassAreaVolume(body, m, a, v) Then
                            If massaKg <= 0 Then massaKg = m
                            If areaM2 <= 0 Then areaM2 = a
                        End If

                        ' (b) Se ainda não tiver área, soma áreas de faces
                        If areaM2 <= 0 Then
                            areaM2 = TryComputeTotalAreaM2_FromBody(body)
                        End If
                    End If
                End If
            Catch
            End Try
        End If

        ' 3) Fallback duro: massa pelo volume * densidade (quando disponível)
        If massaKg <= 0 Then
            Try
                Dim densKgM3 As Double = TryGetDensityKgM3(doc)   ' ex.: variável "Propriedades_Físicas_Densidade"
                Dim volM3 As Double = TryGetBodyVolumeM3(doc)
                If densKgM3 > 0 AndAlso volM3 > 0 Then massaKg = densKgM3 * volM3
            Catch
            End Try
        End If

        Return (massaKg, areaM2)
    End Function

    Private Function GetBodyMassAreaVolume(body As Object, ByRef massKg As Double, ByRef areaM2 As Double, ByRef volumeM3 As Double) As Boolean
        massKg = 0 : areaM2 = 0 : volumeM3 = 0
        Try
            ' Muitos builds oferecem GetMassProperties(mass, area, volume, cgX, cgY, cgZ, ixx, iyy, izz, ixy, ixz, iyz)
            CallByName(body, "GetMassProperties", CallType.Method, massKg, areaM2, volumeM3, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)
            Return (massKg > 0 OrElse areaM2 > 0 OrElse volumeM3 > 0)
        Catch
            ' Alternativas: GetArea / GetVolume
            Try : areaM2 = CDbl(CallByName(body, "GetArea", CallType.Method)) : Catch : End Try
            Try : volumeM3 = CDbl(CallByName(body, "GetVolume", CallType.Method)) : Catch : End Try
            Return (areaM2 > 0 OrElse volumeM3 > 0)
        End Try
    End Function

    Private Function TryComputeTotalAreaM2_FromBody(body As Object) As Double
        Try
            Dim faces As Object = CallByName(body, "Faces", CallType.Get, 0) ' 0 = igQueryAll
            Dim cnt As Integer = CInt(CallByName(faces, "Count", CallType.Get))
            Dim areaTotalMm2 As Double = 0
            For i = 1 To cnt
                Try
                    Dim f As Object = CallByName(faces, "Item", CallType.Method, i)
                    Dim a As Double = CDbl(CallByName(f, "Area", CallType.Get))
                    ' Heurística de unidade
                    If a > 10000 Then
                        areaTotalMm2 += a            ' já parece mm²
                    Else
                        areaTotalMm2 += a * 1000000  ' veio em m²
                    End If
                Catch
                End Try
            Next
            Return areaTotalMm2 / 1000000.0R
        Catch
            Return 0
        End Try
    End Function

    Private Function TryGetBodyVolumeM3(doc As Object) As Double
        Try
            Dim models As Object = CallByName(doc, "Models", CallType.Get)
            If models Is Nothing OrElse CInt(CallByName(models, "Count", CallType.Get)) = 0 Then Return 0
            Dim model As Object = CallByName(models, "Item", CallType.Method, 1)
            Dim body As Object = CallByName(model, "Body", CallType.Get)
            Dim v As Double = 0
            Try : v = CDbl(CallByName(body, "GetVolume", CallType.Method)) : Catch : v = 0 : End Try
            Return v
        Catch
            Return 0
        End Try
    End Function

    Private Function TryGetDensityKgM3(doc As Object) As Double
        ' tenta em Custom e em Variáveis conhecidas (print do usuário mostra "Propriedades_Físicas_Densidade")
        Dim dens As Double = TryGetPropAsDouble(doc, {("Custom", "Propriedades_Físicas_Densidade"), ("Custom", "Density")}, 1.0)
        If dens > 0 Then Return dens

        ' Procura por variável
        Try
            Dim vars As Object = CallByName(doc, "Variables", CallType.Get)
            For Each v As Object In vars
                Dim n As String = ""
                Try : n = CStr(CallByName(v, "Name", CallType.Get)) : Catch : Continue For : End Try
                If NormalizeName(n).Contains("densidade") OrElse NormalizeName(n).Contains("density") Then
                    Dim val As Object = Nothing
                    Try : val = CallByName(v, "Value", CallType.Get) : Catch : End Try
                    Dim d As Double
                    If TryParseNumberWithUnits(If(val Is Nothing, "", val.ToString()), d) Then Return d
                End If
            Next
        Catch
        End Try
        Return 0
    End Function

    ' ============================================================
    ' Soma recursiva ASM (massa/área) por ocorrências
    ' ============================================================
    Private Function SomarFisicosDeAssembly(asmDoc As Object) As (MassaKg As Double, AreaM2 As Double)
        Dim m As Double = 0, a As Double = 0
        Try
            Dim occs As Object = CallByName(asmDoc, "Occurrences", CallType.Get)
            Dim cnt As Integer = 0
            Try : cnt = CInt(CallByName(occs, "Count", CallType.Get)) : Catch : cnt = 0 : End Try
            For i = 1 To cnt
                Try
                    Dim occ As Object = CallByName(occs, "Item", CallType.Method, i)
                    Dim childDoc As Object = CallByName(occ, "OccurrenceDocument", CallType.Get)
                    Dim childName As String = CStr(CallByName(occ, "OccurrenceFileName", CallType.Get))
                    Dim q As Integer = 1
                    Try : q = CInt(CallByName(occ, "Quantity", CallType.Get)) : Catch : q = 1 : End Try

                    Dim ext As String = System.IO.Path.GetExtension(childName).ToLowerInvariant()
                    If ext = ".asm" Then
                        Dim rAsm = SomarFisicosDeAssembly(childDoc)
                        m += rAsm.MassaKg * q
                        a += rAsm.AreaM2 * q
                    Else
                        Dim r = TryLerMassaAreaDoDocumento(childDoc)
                        m += r.MassaKg * q
                        a += r.AreaM2 * q
                    End If
                Catch
                End Try
            Next
        Catch
        End Try
        Return (m, a)
    End Function

    ' ============================================================
    ' FlatPattern → RangeBox (m → mm)
    ' ============================================================
    Private Function TryGetFlatPatternBoundingBoxMm(doc As Object) As (WidthMm As Double, HeightMm As Double)
        Try
            Dim sheet As Object = TryCast(doc, Object)
            Dim fps As Object = Nothing
            Try : fps = CallByName(sheet, "FlatPatternModels", CallType.Get) : Catch : fps = Nothing : End Try

            ' cria flat se necessário
            If fps Is Nothing OrElse CInt(CallByName(fps, "Count", CallType.Get)) = 0 Then
                Try : CallByName(AppDomain.CurrentDomain.GetData("SE_APP"), "StartCommand", CallType.Method, 1494) : Catch : End Try ' Create Flat Pattern
                Threading.Thread.Sleep(300) : System.Windows.Forms.Application.DoEvents()
                Try : fps = CallByName(sheet, "FlatPatternModels", CallType.Get) : Catch : fps = Nothing : End Try
                If fps Is Nothing OrElse CInt(CallByName(fps, "Count", CallType.Get)) = 0 Then Return (0, 0)
            End If

            Dim fp As Object = CallByName(fps, "Item", CallType.Method, 1)
            Dim body As Object = CallByName(fp, "Body", CallType.Get)
            Dim minX As Double, minY As Double, minZ As Double, maxX As Double, maxY As Double, maxZ As Double
            CallByName(body, "GetRangeBox", CallType.Method, minX, minY, minZ, maxX, maxY, maxZ)

            Dim dx = Math.Abs(maxX - minX) * 1000.0R
            Dim dy = Math.Abs(maxY - minY) * 1000.0R
            Return (Math.Round(dx, 3), Math.Round(dy, 3))
        Catch
            Return (0, 0)
        End Try
    End Function

    ' ============================================================
    ' Plano B: gera DXF TEMP e lê $EXTMIN/$EXTMAX do cabeçalho
    ' ============================================================
    Private Function TryGetBlankFromTempDXF(doc As Object) As (WidthMm As Double, HeightMm As Double)
        Try
            Dim full As String = CStr(CallByName(doc, "FullName", CallType.Get))
            Dim pastaTmp As String = System.IO.Path.GetTempPath()
            Dim nomeBase As String = System.IO.Path.GetFileNameWithoutExtension(full)
            Dim arqDxf As String = System.IO.Path.Combine(pastaTmp, nomeBase & "_tmp_flat.dxf")

            ' Remove se existir
            Try : If System.IO.File.Exists(arqDxf) Then System.IO.File.Delete(arqDxf) : 
            Catch : End Try

            ' Tenta exportar via SaveAsFlatDXFEx (passa qualquer face/edge válidas)
            Dim models As Object = CallByName(doc, "Models", CallType.Get)
            If models Is Nothing OrElse CInt(CallByName(models, "Count", CallType.Get)) = 0 Then Return (0, 0)
            Dim model As Object = CallByName(models, "Item", CallType.Method, 1)
            Dim body As Object = CallByName(model, "Body", CallType.Get)
            Dim faces As Object = CallByName(body, "Faces", CallType.Get, 0)
            Dim face As Object = CallByName(faces, "Item", CallType.Method, 1)
            Dim edges As Object = CallByName(face, "Edges", CallType.Get)
            Dim edge As Object = CallByName(edges, "Item", CallType.Method, 1)
            ' vertex pode ser Nothing
            CallByName(models, "SaveAsFlatDXFEx", CallType.Method, arqDxf, face, edge, Nothing, True)

            ' Espera o arquivo surgir
            Dim tries = 0
            Do While Not System.IO.File.Exists(arqDxf) AndAlso tries < 20
                Threading.Thread.Sleep(150) : tries += 1
            Loop
            If Not System.IO.File.Exists(arqDxf) Then Return (0, 0)

            ' Lê extents do DXF
            Dim minX As Double, minY As Double, maxX As Double, maxY As Double
            If ParseDxfExtents(arqDxf, minX, minY, maxX, maxY) Then
                Dim dx = Math.Abs(maxX - minX) ' DXF geralmente em mm
                Dim dy = Math.Abs(maxY - minY)
                ' limpa temporário
                Try : System.IO.File.Delete(arqDxf) : Catch : End Try
                Return (Math.Round(dx, 3), Math.Round(dy, 3))
            End If

            Try : System.IO.File.Delete(arqDxf) : Catch : End Try
        Catch
        End Try
        Return (0, 0)
    End Function

    Private Function ParseDxfExtents(caminho As String, ByRef minX As Double, ByRef minY As Double, ByRef maxX As Double, ByRef maxY As Double) As Boolean
        minX = 0 : minY = 0 : maxX = 0 : maxY = 0
        Try
            Dim lines = System.IO.File.ReadAllLines(caminho)
            ' No HEADER:
            '  $EXTMIN
            '  10
            '  <x>
            '  20
            '  <y>
            '  $EXTMAX
            '  10
            '  <x>
            '  20
            '  <y>
            Dim i As Integer = 0
            Dim foundMin As Boolean = False, foundMax As Boolean = False
            While i < lines.Length - 1
                Dim ln = lines(i).Trim()
                If ln = "$EXTMIN" Then
                    ' espera: 10, x / 20, y
                    If i + 4 < lines.Length AndAlso lines(i + 1).Trim() = "10" AndAlso lines(i + 3).Trim() = "20" Then
                        Double.TryParse(lines(i + 2).Trim().Replace(",", "."), Globalization.NumberStyles.Any, Globalization.CultureInfo.InvariantCulture, minX)
                        Double.TryParse(lines(i + 4).Trim().Replace(",", "."), Globalization.NumberStyles.Any, Globalization.CultureInfo.InvariantCulture, minY)
                        foundMin = True
                    End If
                ElseIf ln = "$EXTMAX" Then
                    If i + 4 < lines.Length AndAlso lines(i + 1).Trim() = "10" AndAlso lines(i + 3).Trim() = "20" Then
                        Double.TryParse(lines(i + 2).Trim().Replace(",", "."), Globalization.NumberStyles.Any, Globalization.CultureInfo.InvariantCulture, maxX)
                        Double.TryParse(lines(i + 4).Trim().Replace(",", "."), Globalization.NumberStyles.Any, Globalization.CultureInfo.InvariantCulture, maxY)
                        foundMax = True
                    End If
                End If
                i += 1
            End While
            Return foundMin AndAlso foundMax
        Catch
            Return False
        End Try
    End Function

    ' ============================================================
    ' Utilitários já usados
    ' ============================================================
    Private Function TryGetPropAsDouble(doc As Object,
                                        candidates As IEnumerable(Of (SetName As String, PropName As String)),
                                        Optional unitMultiplier As Double = 1.0) As Double
        For Each c In candidates
            Try
                Dim sets As Object = CallByName(doc, "Properties", CallType.Get, c.SetName)
                If sets Is Nothing Then Continue For
                For Each p As Object In sets
                    Dim n As String = ""
                    Try : n = CStr(CallByName(p, "Name", CallType.Get)) : Catch : Continue For : End Try
                    If NormalizeName(n) <> NormalizeName(c.PropName) Then Continue For
                    Dim raw As String = ""
                    Try : raw = CStr(CallByName(p, "Value", CallType.Get)) : Catch : raw = "" : End Try
                    Dim value As Double
                    If TryParseNumberWithUnits(raw, value) Then
                        Return value * unitMultiplier
                    End If
                Next
            Catch
            End Try
        Next
        Return 0.0
    End Function

    Private Function TryParseNumberWithUnits(s As String, ByRef value As Double) As Boolean
        value = 0
        If String.IsNullOrWhiteSpace(s) Then Return False
        Dim t As String = s.Trim().ToLowerInvariant()
        t = t.Replace("mm²", "").Replace("mm2", "").Replace("mm", "")
        t = t.Replace("kg", "").Replace(" m^2", "").Replace("m²", "").Replace("m2", "")
        t = t.Replace(" ", "")
        If Double.TryParse(t, Globalization.NumberStyles.Any, New Globalization.CultureInfo("pt-BR"), value) Then Return True
        t = t.Replace(",", ".")
        Return Double.TryParse(t, Globalization.NumberStyles.Any, Globalization.CultureInfo.InvariantCulture, value)
    End Function

    Private Function NormalizeName(n As String) As String
        If String.IsNullOrEmpty(n) Then Return ""
        n = RemoveDiacritics(n).Trim().ToLowerInvariant()
        n = n.Replace(" ", "").Replace("_", "")
        Return n
    End Function

    Private Function RemoveDiacritics(s As String) As String
        If String.IsNullOrEmpty(s) Then Return ""
        Dim norm = s.Normalize(NormalizationForm.FormD)
        Dim sb As New System.Text.StringBuilder(norm.Length)
        For Each ch As Char In norm
            If Globalization.CharUnicodeInfo.GetUnicodeCategory(ch) <> Globalization.UnicodeCategory.NonSpacingMark Then
                sb.Append(ch)
            End If
        Next
        Return sb.ToString().Normalize(NormalizationForm.FormC)
    End Function




    Private Sub cboAcabamento_Leave(sender As Object, e As EventArgs) Handles cboAcabamento.Leave

        SalvarPropriedadeAlterada(cboAcabamento.Tag, cboAcabamento.Text.ToUpper)
        DadosArquivoCorrente.Acabamento = Me.cboAcabamento.Text


    End Sub

    Private Sub cboTipoDesenho_Leave(sender As Object, e As EventArgs) Handles cboTipoDesenho.Leave


        DadosArquivoCorrente.TipoDesenho = Me.cboTipoDesenho.Text
        SalvarPropriedadeAlterada(cboTipoDesenho.Tag, cboTipoDesenho.Text.ToUpper)


    End Sub

    Private Sub cboBloqueado_Leave(sender As Object, e As EventArgs) Handles cboBloqueado.Leave

        SalvarPropriedadeAlterada(cboBloqueado.Tag, cboBloqueado.Text.ToUpper)
        DadosArquivoCorrente.Bloqueado = Me.cboBloqueado.Text

    End Sub


    Private Async Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click


        Try
            dgvDadosPdf.AllowUserToAddRows = False ' desabilita a linha em branco

            ' Adiciona as linhas manualmente, após configurar o DataGridView
            dgvDadosPdf.Rows.Clear()  ' limpa linhas existentes para evitar duplicatas

            dgvDadosPdf.Rows.Add("numero_desenho") '0
            dgvDadosPdf.Rows.Add("descricao_desenho") '1
            dgvDadosPdf.Rows.Add("titulo") '2
            dgvDadosPdf.Rows.Add("material") '3
            dgvDadosPdf.Rows.Add("acabamento") '4
            dgvDadosPdf.Rows.Add("peso_valor") '5
            dgvDadosPdf.Rows.Add("peso_unidade") '6
            dgvDadosPdf.Rows.Add("Area_de_Pintura") '7
            dgvDadosPdf.Rows.Add("cliente") '8
            dgvDadosPdf.Rows.Add("codigo_cliente") '9
            dgvDadosPdf.Rows.Add("caminho_arquivo") '10
            dgvDadosPdf.Rows.Add("revisao") '11
            dgvDadosPdf.Rows.Add("espessura") '12
            dgvDadosPdf.Rows.Add("largurablank") '13
            dgvDadosPdf.Rows.Add("comprimentoblank") '14
            dgvDadosPdf.Rows.Add("tipo_de_desenho") '15


            dgvDadosPdf.Rows.Add("Endereco Real PDF") '16
            dgvDadosPdf.Rows.Add("Nome Real PDF") '17



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

            ' 2) Extrai
            Dim doc As DocumentoDesenho = Await AgentesIA.LerDadosDesenhoComIA(caminhoPdf)


            Dim msg As New StringBuilder()
            msg.AppendLine(If(doc.numero_desenho, "")) _
               .AppendLine(If(doc.descricao_desenho, "")) _
               .AppendLine(If(doc.titulo, "")) _
               .AppendLine(If(doc.material, "")) _
               .AppendLine(If(doc.acabamento, "")) _
               .AppendLine(If(doc.peso_valor, "")) _
               .AppendLine(If(doc.peso_unidade, "")) _
               .AppendLine(If(doc.Area_de_Pintura, "")) _
            .AppendLine(If(doc.cliente, "")) _
               .AppendLine(If(doc.codigo_cliente, "")) _
               .AppendLine(If(doc.caminho_arquivo, "")) _
               .AppendLine(If(doc.revisao, "")) _
            .AppendLine(If(doc.espessura, "")) _
            .AppendLine(If(doc.largurablank, "")) _
            .AppendLine(If(doc.comprimentoblank, "")) _
                   .AppendLine(If(doc.tipo_de_desenho, ""))

            ' 3) Preenche o Combo da coluna "DGVDadosExtracaoPDF"
            PreencherComboComDadosPdf(dgvDadosPdf, "DGVDadosExtracaoPDF", msg.ToString())


            dgvDadosPdf.Rows(0).Cells("dgvDadosSelecionado").Value = If(doc.numero_desenho, "")
            dgvDadosPdf.Rows(1).Cells("dgvDadosSelecionado").Value = If(doc.descricao_desenho, "")
            dgvDadosPdf.Rows(2).Cells("dgvDadosSelecionado").Value = If(doc.titulo, "")
            dgvDadosPdf.Rows(3).Cells("dgvDadosSelecionado").Value = If(doc.material, "")
            dgvDadosPdf.Rows(4).Cells("dgvDadosSelecionado").Value = If(doc.acabamento, "")
            dgvDadosPdf.Rows(5).Cells("dgvDadosSelecionado").Value = If(doc.peso_valor, "")
            dgvDadosPdf.Rows(6).Cells("dgvDadosSelecionado").Value = If(doc.peso_unidade, "")
            dgvDadosPdf.Rows(7).Cells("dgvDadosSelecionado").Value = If(doc.Area_de_Pintura, "")
            dgvDadosPdf.Rows(8).Cells("dgvDadosSelecionado").Value = If(doc.cliente, "")
            dgvDadosPdf.Rows(9).Cells("dgvDadosSelecionado").Value = If(doc.codigo_cliente, "")
            dgvDadosPdf.Rows(10).Cells("dgvDadosSelecionado").Value = If(doc.caminho_arquivo, "")
            dgvDadosPdf.Rows(11).Cells("dgvDadosSelecionado").Value = If(doc.revisao, "")

            dgvDadosPdf.Rows(12).Cells("dgvDadosSelecionado").Value = If(doc.espessura, "")
            dgvDadosPdf.Rows(13).Cells("dgvDadosSelecionado").Value = If(doc.largurablank, "")
            dgvDadosPdf.Rows(14).Cells("dgvDadosSelecionado").Value = If(doc.comprimentoblank, "")
            dgvDadosPdf.Rows(15).Cells("dgvDadosSelecionado").Value = If(doc.tipo_de_desenho, "")


            dgvDadosPdf.Rows(16).Cells("dgvDadosSelecionado").Value = caminhoPdf
            dgvDadosPdf.Rows(17).Cells("dgvDadosSelecionado").Value = nomepdf


            msg.AppendLine(If("numero_desenho: " & doc.numero_desenho, "")) _
               .AppendLine(If("descricao_desenho: " & doc.descricao_desenho, "")) _
               .AppendLine(If("titulo: " & doc.titulo, "")) _
               .AppendLine(If("material: " & doc.material, "")) _
               .AppendLine(If("acabamento: " & doc.acabamento, "")) _
               .AppendLine(If("peso_valor: " & doc.peso_valor, "")) _
               .AppendLine(If("peso_unidade: " & doc.peso_unidade, "")) _
               .AppendLine(If("Area_de_Pintura: " & doc.Area_de_Pintura, "")) _
            .AppendLine(If("cliente: " & doc.cliente, "")) _
               .AppendLine(If("codigo_cliente: " & doc.codigo_cliente, "")) _
               .AppendLine(If("caminho_arquivo: " & doc.caminho_arquivo, "")) _
               .AppendLine(If("revisao: " & doc.revisao, "")) _
            .AppendLine(If("espessura: " & doc.espessura, "")) _
            .AppendLine(If("largurablank: " & doc.largurablank, "")) _
            .AppendLine(If("comprimentoblank: " & doc.comprimentoblank, "")) _
                   .AppendLine(If("tipo_de_desenho: " & doc.tipo_de_desenho, ""))

            RichTextBox1.Text = msg.ToString()



        Catch ex As Exception

            MessageBox.Show("Erro " & ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Public Sub PreencherComboComDadosPdf(dgv As DataGridView,
                                     nomeColunaCombo As String,
                                     conteudo As String)

        If dgv Is Nothing OrElse String.IsNullOrWhiteSpace(nomeColunaCombo) Then Exit Sub

        ' Obtem a coluna ComboBox existente ou cria se não existir
        Dim col As DataGridViewComboBoxColumn =
        TryCast(dgv.Columns.Cast(Of DataGridViewColumn)().
            FirstOrDefault(Function(c) String.Equals(c.Name, nomeColunaCombo, StringComparison.OrdinalIgnoreCase)),
            DataGridViewComboBoxColumn)

        If col Is Nothing Then
            col = New DataGridViewComboBoxColumn() With {
            .Name = nomeColunaCombo,
            .HeaderText = "Opção de Dados Extraídos PDF",
            .ValueType = GetType(String),
            .DisplayStyle = DataGridViewComboBoxDisplayStyle.DropDownButton,
            .FlatStyle = FlatStyle.Standard,
            .Width = 250
        }
            dgv.Columns.Add(col)
        End If

        ' Quebra o texto em linhas (qualquer tipo de newline) e filtra
        Dim texto As String = If(conteudo, String.Empty)
        Dim linhas As List(Of String) =
        Regex.Split(texto, "\r\n|\n|\r").
        Select(Function(s) s.Trim()).
        Where(Function(s) s.Length > 0 AndAlso Not s.StartsWith("====")).
        Distinct(StringComparer.OrdinalIgnoreCase).
        ToList()

        If linhas.Count = 0 Then linhas.Add("(sem dados)")

        ' Para evitar conflitos com linhas existentes no grid, 
        ' é preferível limpar o DataSource e definir um novo
        ' Se não usar DataSource, limpar os itens e adicionar manualmente
        col.DataSource = Nothing
        col.Items.Clear()

        ' Adiciona as linhas como itens no combobox da coluna
        For Each item In linhas
            col.Items.Add(item)
        Next

        col.MaxDropDownItems = Math.Min(30, linhas.Count)
        col.AutoComplete = True

        ' Caso não tenha linhas no grid, adiciona uma para poder usar o combo
        'If dgv.Rows.Count = 0 Then dgv.Rows.Add()

        ' Define o valor na primeira linha da coluna para o primeiro item do combo (opcional)
        ' dgv.Rows(0).Cells(col.Index).Value = linhas(0)

        dgv.EditMode = DataGridViewEditMode.EditOnEnter

    End Sub


    Private Sub TimerbtnMsg_Tick(sender As Object, e As EventArgs) Handles TimerbtnMsg.Tick



        btnMsg.BackColor = ColorTranslator.FromHtml("#007ACC")
        btnMsg.Text = "Status Mensagem"

        TimerbtnMsg.Enabled = False


    End Sub




    Private Sub dgvProcessoMaterial_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvProcessoMaterial.CellContentClick

    End Sub

    Private Sub dgvDadosPdf_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvDadosPdf.CellContentClick

    End Sub

    Private Sub dgvDadosPdf_CellEndEdit(sender As Object, e As DataGridViewCellEventArgs) Handles dgvDadosPdf.CellEndEdit

    End Sub

    Private Sub Button7_Click(sender As Object, e As EventArgs) Handles Button7.Click

        If app Is Nothing OrElse app.Documents.Count > 0 Then

            MsgBox("Feche todos os documentos do Solid Edge antes de continuar.", MsgBoxStyle.Exclamation, "SINCO - Solid Edge")

            Exit Sub

        Else


            txtTitulo.Text = dgvDadosPdf.Rows(2).Cells("dgvDadosSelecionado").Value.ToString.ToUpper.Trim   '= If(doc.titulo, "")
            DadosArquivoCorrente.Titulo = txtTitulo.Text

            DadosArquivoCorrente.Bloqueado = "NÃO"

            DadosArquivoCorrente.AssuntoSubiTitulo = dgvDadosPdf.Rows(2).Cells("dgvDadosSelecionado").Value.ToString.ToUpper.Trim  '= If(doc.titulo, "")
            DadosArquivoCorrente.AssuntoSubiTitulo = txtTitulo.Text

            DadosArquivoCorrente.material = dgvDadosPdf.Rows(3).Cells("dgvDadosSelecionado").Value.ToString.ToUpper.Trim
            txtMaterialSw.Text = DadosArquivoCorrente.material

            DadosArquivoCorrente.Comentarios = dgvDadosPdf.Rows(4).Cells("dgvDadosSelecionado").Value.ToString.ToUpper.Trim
            Me.cboAcabamento.Text = DadosArquivoCorrente.Comentarios

            Dim limpo As String

            limpo = LimparUnidades(dgvDadosPdf.Rows(5).Cells("dgvDadosSelecionado").Value).ToString.ToUpper.Trim
            DadosArquivoCorrente.Massa = limpo
            txtPesoKg.Text = limpo


            limpo = LimparUnidades(dgvDadosPdf.Rows(7).Cells("dgvDadosSelecionado").Value).ToString.ToUpper.Trim
            DadosArquivoCorrente.AreaPintura = limpo
            txtAreametroquadr.Text = limpo


            DadosArquivoCorrente.CodigoJuridicoMat = dgvDadosPdf.Rows(8).Cells("dgvDadosSelecionado").Value.ToString.ToUpper.Trim & "/" & dgvDadosPdf.Rows(2).Cells("dgvDadosSelecionado").Value.ToString.ToUpper.Trim  '& dgvDadosPdf.Rows(9).Cells("dgvDadosSelecionado").Value
            txtEmpresa.Text = DadosArquivoCorrente.CodigoJuridicoMat

            Me.txtendereco.Text = dgvDadosPdf.Rows(10).Cells("dgvDadosSelecionado").Value.ToString.ToUpper.Trim  ' = If(doc.caminho_arquivo, "")
            '  dgvDadosPdf.Rows(11).Cells("dgvDadosSelecionado").Value = If(doc.revisao, "")

            limpo = LimparUnidades(dgvDadosPdf.Rows(12).Cells("dgvDadosSelecionado").Value.ToString.ToUpper.Trim)
            DadosArquivoCorrente.Espessura = limpo
            txtEspessura.Text = limpo


            limpo = LimparUnidades(dgvDadosPdf.Rows(13).Cells("dgvDadosSelecionado").Value.ToString.ToUpper.Trim)
            DadosArquivoCorrente.ComprimentoBlank = limpo
            txtCutSizex.Text = limpo


            limpo = LimparUnidades(dgvDadosPdf.Rows(14).Cells("dgvDadosSelecionado").Value.ToString.ToUpper.Trim)
            DadosArquivoCorrente.LarguraBlank = limpo
            txtCutSizey.Text = limpo

            DadosArquivoCorrente.TipoDesenho = dgvDadosPdf.Rows(15).Cells("dgvDadosSelecionado").Value.ToString.ToUpper.Trim
            cboTipoDesenho.Text = DadosArquivoCorrente.TipoDesenho

            DadosArquivoCorrente.EnderecoArquivo = dgvDadosPdf.Rows(16).Cells("dgvDadosSelecionado").Value.ToString.ToUpper.Trim
            txtNumeroDesenho.Text = DadosArquivoCorrente.EnderecoArquivo

            DadosArquivoCorrente.NomeArquivoComExtensao = dgvDadosPdf.Rows(17).Cells("dgvDadosSelecionado").Value.ToString.ToUpper.Trim

            DadosArquivoCorrente.NomeArquivoSemExtensao = dgvDadosPdf.Rows(17).Cells("dgvDadosSelecionado").Value.ToString.ToUpper.Trim

            ' Atualiza o campo de acabamento
            DadosArquivoCorrente.Acabamento = $"{DadosArquivoCorrente.Comentarios} - {DadosArquivoCorrente.PalavraChave}"


            TabPage1.Show()


            TabPage1.BringToFront()

        End If

    End Sub

    Private Sub TimerdgvMateriaisProtheus_Tick(sender As Object, e As EventArgs) Handles TimerdgvMateriaisProtheus.Tick

        If chkGabarito.Checked = False Then

            dgvMateriaisProtheus.DataSource = cl_BancoDados.CarregarDados("SELECT
    IdMaterial,
    CodMatFabricante,
    NumeroRP,
    DescDetal,
    EnderecoArquivo,
    FamiliaMat AS Familia,
    ConfiguracaoArquivo,
    Unidade,
    Peso,
    Valor,
    Qtde,
    IdEmpresa,
    d_e_l_e_t_e
FROM material
WHERE
    (d_e_l_e_t_e IS NULL OR d_e_l_e_t_e = '')  -- Filtrando onde d_e_l_e_t_e é NULL ou vazio
    AND (EnderecoArquivo IS NULL)  -- Filtrando onde EnderecoArquivo é NULL
    AND DescDetal LIKE '%" & Me.txtPesqDescricao1.Text & "%'  -- Filtrando onde DescDetal contém 'valor' (ajuste conforme necessário)
        AND DescDetal LIKE '%" & Me.txtPesqDescricao2.Text & "%'  -- Filtrando onde DescDetal contém 'valor' (ajuste conforme necessário)
    AND CodMatFabricante LIKE '%" & txtPesqCodigo.Text & "%'  -- Filtrando onde CodMatFabricante contém 'valor_cod' (ajuste conforme necessário)
    AND NumeroRP LIKE '%" & TxtPesqRP.Text & "%'  -- Filtrando onde NumeroRP contém 'valor_num' (ajuste conforme necessário)
ORDER BY CodMatFabricante limit 100")

        ElseIf chkGabarito.Checked = True Then


            dgvMateriaisProtheus.DataSource = cl_BancoDados.CarregarDados("SELECT
    IdMaterial,
    CodMatFabricante,
    DescDetal,
    EnderecoArquivo,
    FamiliaMat AS Familia,
    Unidade,
    Peso,
    Valor,
    Qtde,
    IdEmpresa,
    d_e_l_e_t_e
FROM material
WHERE
    (d_e_l_e_t_e IS NULL OR d_e_l_e_t_e = '')  -- Filtrando onde d_e_l_e_t_e é NULL ou vazio
    AND (EnderecoArquivo is not null)
     AND DescDetal LIKE '%" & Me.txtPesqDescricao1.Text & "%'  -- Filtrando onde DescDetal contém 'valor' (ajuste conforme necessário)
     AND DescDetal LIKE '%" & Me.txtPesqDescricao2.Text & "%'  -- Filtrando onde DescDetal contém 'valor' (ajuste conforme necessário)
    AND CodMatFabricante LIKE '%" & txtPesqCodigo.Text & "%'  -- Filtrando onde CodMatFabricante contém 'valor_cod' (ajuste conforme necessário)
    ORDER BY CodMatFabricante limit 100")

        End If

        TimerdgvMateriaisProtheus.Enabled = False


    End Sub

    Private Sub txtPesqCodigo_TextChanged(sender As Object, e As EventArgs) Handles txtPesqCodigo.TextChanged
        TimerdgvMateriaisProtheus.Enabled = True
    End Sub

    Private Sub dgvMateriaisProtheus_DataBindingComplete(sender As Object, e As DataGridViewBindingCompleteEventArgs) Handles dgvMateriaisProtheus.DataBindingComplete


        Try


            dgvMateriaisProtheus.Columns("DescDetal").HeaderText = "Descrição"
            dgvMateriaisProtheus.Columns("CodMatFabricante").HeaderText = "Cod.Mat."
            ' DGVMaterial.Columns("IdMaterial").Visible = False
            dgvMateriaisProtheus.Columns("ConfiguracaoArquivo").Visible = False
            dgvMateriaisProtheus.Columns("Familia").Visible = False
            dgvMateriaisProtheus.Columns("Peso").Visible = False
            dgvMateriaisProtheus.Columns("Qtde").Visible = False
            dgvMateriaisProtheus.Columns("d_e_l_e_t_e").Visible = False

            dgvMateriaisProtheus.Columns("IdEmpresa").Visible = False

            dgvMateriaisProtheus.Columns("CodMatFabricante").DefaultCellStyle.Font = New Font("Microsoft Sans Serif", 9, FontStyle.Bold)
            ' Alinha o texto da coluna "idplanodecorte" ao centro
            dgvMateriaisProtheus.Columns("CodMatFabricante").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            dgvMateriaisProtheus.Columns("CodMatFabricante").DefaultCellStyle.BackColor = Color.LightGray


        Catch ex As Exception
        Finally

        End Try

    End Sub

    Private Sub dgvMateriaisProtheus_DoubleClick(sender As Object, e As EventArgs) Handles dgvMateriaisProtheus.DoubleClick
        If chkGabarito.Checked = False Then

            AdicionarPeca()

        Else

            MsgBox("A INserção de material na peça corrente so e possivel com a opção gabarito desmarcada!", vbInformation, "Atenção!!")

        End If



        TimerManufaturada.Enabled = True

    End Sub



    Private Sub AdicionarPeca()

        '--- 1) Validação da linha selecionada no grid -----------------------------
        Dim linhaAtual As DataGridViewRow = dgvMateriaisProtheus.CurrentRow

        If linhaAtual Is Nothing Then
            MsgBox("Nenhum material selecionado.", vbExclamation, "Atenção")
            Exit Sub
        End If

        Dim codMatFabricante As String = ""
        Try
            codMatFabricante = Convert.ToString(linhaAtual.Cells("CodMatFabricante").Value).Trim()
        Catch ex As Exception
            codMatFabricante = ""
        End Try

        If String.IsNullOrWhiteSpace(codMatFabricante) Then
            MsgBox("Selecione um material válido para adicionar.", vbExclamation, "Atenção")
            Exit Sub
        End If

        '--- 2) Entrada da quantidade (decimal, ex: 2,3) --------------------------
        Dim inputQtde As String = InputBox("Informe a quantidade de peças a serem adicionadas:", "Monta Peça", "1")

        'Usuário cancelou ou deixou vazio
        If String.IsNullOrWhiteSpace(inputQtde) Then
            Exit Sub
        End If

        inputQtde = inputQtde.Trim()

        'Aceita ponto e vírgula (converte ponto para vírgula)
        inputQtde = inputQtde.Replace(".", ",")

        Dim qtde As Decimal

        If Not Decimal.TryParse(inputQtde,
                            NumberStyles.Number,
                            New CultureInfo("pt-BR"),
                            qtde) _
       OrElse qtde <= 0D Then

            MsgBox("O valor informado não é válido. Informe um número maior que zero. Ex: 1, 2,5, 10,75", vbCritical, "Atenção")
            Exit Sub
        End If

        classePecaManufaturada.PecaQtde = qtde

        '--- 3) Validação do IdMaterial do arquivo corrente -----------------------
        If DadosArquivoCorrente Is Nothing Then
            MsgBox("Não há arquivo corrente carregado.", vbExclamation, "Atenção")
            Exit Sub
        End If

        Dim idMaterialAtual As Integer
        idMaterialAtual = linhaAtual.Cells("idMaterial").Value.ToString()

        '--- 4) Montagem dos objetos e acesso ao banco ----------------------------
        Try


            classePecaManufaturada.IdMaterialPeca = linhaAtual.Cells("IdMaterial").Value

            classePecaManufaturada.PecaQtde = qtde

            'classePecaManufaturada.IdMaterial = idMaterialAtual
            classePecaManufaturada.D_E_L_E_T_E = ""
            classePecaManufaturada.Peso = ""
            classePecaManufaturada.Valor = ""
            classePecaManufaturada.UsuarioD_E_L_E_T_E = ""
            classePecaManufaturada.DataD_E_L_E_T_E = ""
            classePecaManufaturada.CodMatFabricante = codMatFabricante
            classePecaManufaturada.UsuarioCriacao = Environment.UserName
            classePecaManufaturada.DataCriacao = Date.Now.Date

            classePecaManufaturada.SalvarDados("",
                                            classePecaManufaturada.TipoPeca,
                                                 classePecaManufaturada.IdMaterialPeca,
                                            classePecaManufaturada.PecaQtde,
                                                             DadosArquivoCorrente.IdMaterial,
                                            classePecaManufaturada.IdEmpresa,
                                            classePecaManufaturada.D_E_L_E_T_E,
                                            classePecaManufaturada.Peso,
                                            classePecaManufaturada.Valor,
                                            classePecaManufaturada.UsuarioD_E_L_E_T_E,
                                            classePecaManufaturada.DataD_E_L_E_T_E,
                                            classePecaManufaturada.CodMatFabricante,
                                            classePecaManufaturada.UsuarioCriacao,
                                            classePecaManufaturada.DataCriacao)

            TimerManufaturada.Enabled = True



        Catch ex As Exception
            classePecaManufaturada.PecaQtde = 0D
            MsgBox("Ocorreu um erro ao adicionar a peça:" & vbCrLf &
               ex.Message, vbCritical, "Erro")
            'Aqui seria o ponto para logar o erro (arquivo, banco, etc.)
        End Try


    End Sub


    Private Sub AdicionarPecaGabaritos()

        '--- 1) Validação da linha selecionada no grid -----------------------------
        Dim linhaAtual As DataGridViewRow = dgvMateriaisProtheus.CurrentRow

        If linhaAtual Is Nothing Then
            MsgBox("Nenhum material selecionado.", vbExclamation, "Atenção")
            Exit Sub
        End If

        Dim codMatFabricante As String = ""
        Try
            codMatFabricante = Convert.ToString(linhaAtual.Cells("CodMatFabricante").Value).Trim()
        Catch ex As Exception
            codMatFabricante = ""
        End Try

        If String.IsNullOrWhiteSpace(codMatFabricante) Then
            MsgBox("Selecione um material válido para adicionar.", vbExclamation, "Atenção")
            Exit Sub
        End If


        '--- 3) Validação do IdMaterial do arquivo corrente -----------------------
        If DadosArquivoCorrente Is Nothing Then
            MsgBox("Não há arquivo corrente carregado.", vbExclamation, "Atenção")
            Exit Sub
        End If

        Dim idMaterialAtual As Integer
        idMaterialAtual = linhaAtual.Cells("idMaterial").Value.ToString()

        '--- 4) Montagem dos objetos e acesso ao banco ----------------------------
        Try


            classePecaManufaturada.IdMaterialPeca = linhaAtual.Cells("IdMaterial").Value

            classePecaManufaturada.CodMatFabricante = codMatFabricante

            classePecaManufaturada.SalvarDadosGabarito(DadosArquivoCorrente.IdMaterial,
                                            MaterialFichaTecnica.idmaterial,
                                                 InputBox("Observações", "Associação de Gabarito", ""))

            'TimerManufaturada.Enabled = True

            TimerdgvGabaritos.Enabled = True

        Catch ex As Exception
            classePecaManufaturada.PecaQtde = 0D
            MsgBox("Ocorreu um erro ao adicionar a peça:" & vbCrLf &
               ex.Message, vbCritical, "Erro")
            'Aqui seria o ponto para logar o erro (arquivo, banco, etc.)
        End Try

    End Sub


    Private Sub dgvMaterialPeca_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvMaterialPeca.CellContentClick

    End Sub

    Private Sub dgvMaterialPeca_KeyDown(sender As Object, e As KeyEventArgs) Handles dgvMaterialPeca.KeyDown

        If e.KeyCode = Keys.Delete Then
            ExcluirMaterialProtheus()
            TimerManufaturada.Enabled = True
        End If

    End Sub

    Private Sub TimerManufaturada_Tick(sender As Object, e As EventArgs) Handles TimerManufaturada.Tick

        Dim query As String = "SELECT IdMontaPeca, 
CodigoJuridicoMat,
DescDetal, 
PecaQtde,
Unidade, 
NomeArquivoSemExtensao, 
IdMaterialpeca, 
DescFamilia, 
CodMatFabricante, 
IdMaterial, 
Peso, 
D_E_L_E_T_E, 
Valor, 
 Total, 
txtItemEstoque
    FROM viewmontapeca
    WHERE (d_e_l_e_t_e IS NULL OR d_e_l_e_t_e = '')
      AND NomeArquivoSemExtensao = '" & DadosArquivoCorrente.NomeArquivoSemExtensao & "'
        ORDER BY CodMatFabricante;"

        dgvMaterialPeca.DataSource = cl_BancoDados.CarregarDados(query)


        If dgvMaterialPeca.Rows.Count > 0 Then
            dgvMaterialPeca.Columns("DescDetal").HeaderText = "Descrição"
            dgvMaterialPeca.Columns("CodMatFabricante").HeaderText = "Cod.Mat."
            dgvMaterialPeca.Columns("txtItemEstoque").HeaderText = "Estoque"
            dgvMaterialPeca.Columns("PecaQtde").HeaderText = "Qtde"
            dgvMaterialPeca.Columns("NomeArquivoSemExtensao").HeaderText = "Nome Arquivo"
            dgvMaterialPeca.Columns("IdMaterial").Visible = False
            dgvMaterialPeca.Columns("pecaqtde").Visible = True
            dgvMaterialPeca.Columns("IdMontaPeca").Visible = True
            dgvMaterialPeca.Columns("IdMaterialpeca").Visible = False
            dgvMaterialPeca.Columns("CodigoJuridicoMat").Visible = False
            dgvMaterialPeca.Columns("DescFamilia").Visible = False
            dgvMaterialPeca.Columns("D_E_L_E_T_E").Visible = False
            dgvMaterialPeca.Columns("txtItemEstoque").Visible = False
            dgvMaterialPeca.Columns("NomeArquivoSemExtensao").Visible = False
            dgvMaterialPeca.Columns("Peso").Visible = False
            dgvMaterialPeca.Columns("Valor").Visible = False
            'dgvMaterialPeca.Columns("qtdetotal").Visible = False


        Else
            dgvMaterialPeca.DataSource = ""
        End If
        TimerManufaturada.Enabled = False


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

    Private Sub txtPesqCodigo_DoubleClick(sender As Object, e As EventArgs) Handles txtPesqCodigo.DoubleClick

        If DadosArquivoCorrente.NomeArquivoComExtensao.Contains("MTA").ToString Then

            Me.txtPesqCodigo.Text = DadosArquivoCorrente.NomeArquivoComExtensao.Replace(".psm", "").Replace(".par", "").Replace(".asm", "")

        End If


    End Sub

    Private Sub dgvDadosPecas_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvDadosPecas.CellContentClick

    End Sub

    Private Sub dgvos_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvos.CellContentClick

    End Sub

    Private Sub MarcarDesenhoComoRevisãoToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles MarcarDesenhoComoRevisãoToolStripMenuItem.Click


        Try

            If OrdemServico.Liberado_Engenharia <> "" Then

                MsgBox("Ordem de Serviço já Liberada para Produção, não pode mais ser modificada!", vbCritical, "Atenção")

                Exit Sub

            Else

                For i As Integer = 0 To DGVListaMaterialSW.Rows.Count - 1

                    Try


                        If DGVListaMaterialSW.Rows(i).Cells("dgvSelecao").Value = True Then
                            Dim nomeArquivo As String = DGVListaMaterialSW.Rows(i).Cells("CodMatFabricante").Value.ToString()

                            ' Condição: só pode marcar se contiver a extensão
                            If System.IO.Path.HasExtension(nomeArquivo) Then
                                cl_BancoDados.AlteracaoEspecifica("ordemservicoitem", "NovoRevisao", "X", "IDOrdemServicoItem", DGVListaMaterialSW.Rows(i).Cells("IDOrdemServicoItem").Value.ToString)

                                DGVListaMaterialSW.Rows(i).Cells("NovoRevisao").Style.BackColor = Color.LightPink
                                DGVListaMaterialSW.Rows(i).Cells("NovoRevisao").Value = "X"
                                DGVListaMaterialSW.Rows(i).Cells("dgvSelecao").Value = False
                            End If

                        End If

                    Catch ex As Exception
                        Continue For

                    End Try

                Next



            End If
        Catch ex As Exception
        Finally
        End Try
    End Sub

    Private Sub DesmarcarDesenhoComoRevisãoToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DesmarcarDesenhoComoRevisãoToolStripMenuItem.Click




        Try

            If OrdemServico.Liberado_Engenharia <> "" Then

                MsgBox("Ordem de Serviço já Liberada para Produção, não pode mais ser modificada!", vbCritical, "Atenção")

                Exit Sub

            Else

                For i As Integer = 0 To DGVListaMaterialSW.Rows.Count - 1

                    Try


                        If DGVListaMaterialSW.Rows(i).Cells("dgvSelecao").Value = True Then
                            cl_BancoDados.AlteracaoEspecifica("ordemservicoitem", "NovoRevisao", "", "IDOrdemServicoItem", DGVListaMaterialSW.Rows(i).Cells("IDOrdemServicoItem").Value.ToString)

                            DGVListaMaterialSW.Rows(i).Cells("NovoRevisao").Style.BackColor = Color.LightGreen
                            DGVListaMaterialSW.Rows(i).Cells("NovoRevisao").Value = ""
                            DGVListaMaterialSW.Rows(i).Cells("dgvSelecao").Value = False

                        End If

                    Catch ex As Exception
                        Continue For

                    End Try

                Next



            End If
        Catch ex As Exception
        Finally
        End Try


    End Sub


    Private Sub dgvMateriaisProtheus_Click(sender As Object, e As EventArgs) Handles dgvMateriaisProtheus.Click

        Try

            MaterialFichaTecnica.idmaterial = dgvMateriaisProtheus.CurrentRow.Cells("IdMaterial").Value.ToString()
            MaterialFichaTecnica.CodMatFabricanteNumeroRP = dgvMateriaisProtheus.CurrentRow.Cells("CodMatFabricante").Value.ToString()
            MaterialFichaTecnica.DescResumo = dgvMateriaisProtheus.CurrentRow.Cells("DescDetal").Value.ToString()
            MaterialFichaTecnica.DescDetal = dgvMateriaisProtheus.CurrentRow.Cells("DescDetal").Value.ToString()

            MaterialFichaTecnica.CodMatFabricante = DadosArquivoCorrente.NomeArquivoSemExtensao


        Catch ex As Exception
        Finally
        End Try

    End Sub

    Private Sub AssociarFichaTecnicaToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AssociarFichaTecnicaToolStripMenuItem.Click

        If MaterialFichaTecnica.idmaterial.ToString = "" Or MaterialFichaTecnica.idmaterial = 0 Then

            Exit Sub

        Else

            frmFichaTecnica.ShowDialog()

        End If

    End Sub


    Private Sub chkGabarito_Click(sender As Object, e As EventArgs) Handles chkGabarito.Click

        TimerdgvMateriaisProtheus.Enabled = True


    End Sub

    Private Sub AssociarGabaritoAPeçaCorrenteToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AssociarGabaritoAPeçaCorrenteToolStripMenuItem.Click

        If chkGabarito.Checked = True Then



            If MaterialFichaTecnica.idmaterial.ToString = "" Or MaterialFichaTecnica.idmaterial = 0 Then

                Exit Sub
            End If


            AdicionarPecaGabaritos()


        Else

            MsgBox("Opção Invalida!")

        End If

    End Sub

    Private Sub mnudgvMateriaisProtheus_Opening(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles mnudgvMateriaisProtheus.Opening


        If chkGabarito.Checked = True Then

            AssociarGabaritoAPeçaCorrenteToolStripMenuItem.Enabled = True

        Else

            AssociarGabaritoAPeçaCorrenteToolStripMenuItem.Enabled = False

        End If

    End Sub


    Private Sub TimerdgvGabaritos_Tick(sender As Object, e As EventArgs) Handles TimerdgvGabaritos.Tick

        Try

            dgvGabaritos.DataSource = cl_BancoDados.CarregarDados("select * from view_material_gabarito where Desenho = '" & DadosArquivoCorrente.NomeArquivoSemExtensao & "'")

            dgvGabaritos.Columns("id").Visible = False
            dgvGabaritos.Columns("Desenho").Visible = False



            dgvDesenhoCliente.DataSource = cl_BancoDados.CarregarDados("SELECT
  TRIM(COALESCE(idmaterial_desenho,''))        AS idmaterial_desenho,
  TRIM(COALESCE(z1_nomecli,''))                AS z1_nomecli,
  TRIM(COALESCE(z1_produto,''))                AS z1_produto,
  TRIM(COALESCE(z1_codcli,''))                 AS z1_codcli,
  TRIM(COALESCE(z1_revisao,''))                AS z1_revisao,
  TRIM(COALESCE(z1_desccli,''))                AS z1_desccli,
  TRIM(COALESCE(enderecoarquivo_cliente,''))   AS enderecoarquivo_cliente,
  TRIM(COALESCE(enderecoarquivo,''))           AS enderecoarquivo,
  TRIM(COALESCE(desenho,''))                   AS desenho
            FROM material_desenho where desenho = '" & DadosArquivoCorrente.NomeArquivoSemExtensao & "'")

            dgvDesenhoCliente.Columns("idmaterial_desenho").Visible = False
            dgvDesenhoCliente.Columns("Desenho").Visible = False
            dgvDesenhoCliente.Columns("enderecoarquivo_cliente").Visible = False
            dgvDesenhoCliente.Columns("enderecoarquivo").Visible = False

        Catch ex As Exception

        End Try

        TimerdgvGabaritos.Enabled = False

    End Sub

    Private Sub dgvGabaritos_KeyDown(sender As Object, e As KeyEventArgs) Handles dgvGabaritos.KeyDown

        If e.KeyCode = Keys.Delete Then

            ExcluirGabaritoMaterial()

            TimerdgvGabaritos.Enabled = True

        End If


    End Sub

    Private Sub dgvGabaritos_DoubleClick(sender As Object, e As EventArgs) Handles dgvGabaritos.DoubleClick

        Try

            Dim ArquivoPdf As String = dgvGabaritos.CurrentRow.Cells("EnderecoArquivo").Value.ToString()

            ' Substitui extensões ".SLDASM" e ".SLDPRT" por ".DDF"
            ArquivoPdf = Path.ChangeExtension(ArquivoPdf, ".PDF")

            ' Obtém o caminho completo
            ArquivoPdf = Path.GetFullPath(ArquivoPdf)

            ' Verifica se o arquivo existe e o abre
            If File.Exists(ArquivoPdf) Then
                Using p As New Diagnostics.Process
                    p.StartInfo = New ProcessStartInfo(ArquivoPdf)

                    p.Start()
                    'p.WaitForExit()

                    dgvGabaritos.CurrentRow.DefaultCellStyle.BackColor = Color.LightCyan
                End Using
            End If
        Catch ex As Exception
            MsgBox("Arquivo pdf não encontrado!", vbCritical, "Atenção")
        Finally

        End Try


    End Sub

    Private Sub btnProjetoProtheus_Click(sender As Object, e As EventArgs) Handles btnProjetoProtheus.Click

        frmProjeto_Tag.ShowDialog()

    End Sub

    Private Sub ToolStripButton3_Click(sender As Object, e As EventArgs) Handles ToolStripButton3.Click

        AtualziarDadosSinco()

    End Sub

    Private Sub BuscarDesenhoDeReferenciaToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles BuscarDesenhoDeReferenciaToolStripMenuItem.Click


        frmListaDesenhoCliente.ShowDialog()

        TimerdgvGabaritos.Enabled = True


    End Sub

    Private Sub BuscarArquivoPDFDeReferenciaToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles BuscarArquivoPDFDeReferenciaToolStripMenuItem.Click

        Dim ArquivoPdf As String = dgvDesenhoCliente.CurrentRow.Cells("EnderecoArquivo").Value.ToString()

        If ArquivoPdf.ToString = "" Then Exit Sub

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

        cl_BancoDados.AlteracaoEspecifica("material_desenho", "enderecoarquivo_cliente", caminhoPdf, "desenho", DadosArquivoCorrente.NomeArquivoSemExtensao)

        TimerdgvGabaritos.Enabled = True


    End Sub

    Private Sub AbrirArquivoPDFToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AbrirArquivoPDFToolStripMenuItem.Click

        Dim Endereco As String = dgvDesenhoCliente.CurrentRow.Cells("enderecoarquivo_cliente").Value.ToString()

        If File.Exists(Endereco) Then
            Process.Start(Endereco)
        Else
            MessageBox.Show("Arquivo não encontrado: " & Endereco, "SINCO - Solid Edge",
                            MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End If


    End Sub

    Private Sub dgvDesenhoCliente_KeyDown(sender As Object, e As KeyEventArgs) Handles dgvDesenhoCliente.KeyDown


        If e.KeyCode = Keys.Delete Then

            ExcluirDesenhoMaterial()

            TimerdgvGabaritos.Enabled = True

        End If

    End Sub

    Private Sub BuscarMaterialNoProtheusToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles BuscarMaterialNoProtheusToolStripMenuItem.Click

        frmMaterialProtheus.ShowDialog()

        TimerdgvMateriaisProtheus.Enabled = True

        MaterialFichaTecnica.idmaterial = 0

    End Sub

    Private Sub Panel3_Paint(sender As Object, e As PaintEventArgs) Handles Panel3.Paint

    End Sub

    Private Sub BuscarArquivoDoClienteDiretemanteNoDiretorioSemPassarPeloProtheusToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles BuscarArquivoDoClienteDiretemanteNoDiretorioSemPassarPeloProtheusToolStripMenuItem.Click



        '  Dim ArquivoPdf As String = dgvDesenhoCliente.CurrentRow.Cells("EnderecoArquivo").Value.ToString()

        If txtNumeroDesenho.Text.ToString = "" Then Exit Sub

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

        MaterialDesenhoCliente.SalvarDados(txtEmpresa.Text,'MaterialDesenhoCliente.z1_nomecli,
                                           txtNumeroDesenho.Text, 'MaterialDesenhoCliente.z1_produto,
                                           nomepdf,  'MaterialDesenhoCliente.z1_codcli,
                                           "",'MaterialDesenhoCliente.z1_revisao,
                                           "",'MaterialDesenhoCliente.z1_desccli,
                                           caminhoPdf,'MaterialDesenhoCliente.enderecoarquivo_cliente,
                                           txtendereco.Text,'MaterialDesenhoCliente.enderecoarquivo,
                                           txtNumeroDesenho.Text) 'MaterialDesenhoCliente.desenho)

        ' frmDadosPecaCorrente.TimerdgvGabaritos.Enabled = True


        ' cl_BancoDados.AlteracaoEspecifica("material_desenho", "enderecoarquivo_cliente", caminhoPdf, "desenho", DadosArquivoCorrente.NomeArquivoSemExtensao)

        TimerdgvGabaritos.Enabled = True


    End Sub

    Private Sub dgvDadosPecas_KeyPress(sender As Object, e As KeyPressEventArgs) Handles dgvDadosPecas.KeyPress

    End Sub

    Private Sub dgvDadosPecas_KeyDown(sender As Object, e As KeyEventArgs) Handles dgvDadosPecas.KeyDown

        If e.KeyCode = Keys.Delete Then
            ' se não tiver linha selecionada, sai
            If dgvDadosPecas.CurrentRow Is Nothing Then Exit Sub

            ' evita tentar remover a linha "nova" (a última do grid quando AllowUserToAddRows=True)
            If dgvDadosPecas.CurrentRow.IsNewRow Then Exit Sub

            dgvDadosPecas.Rows.Remove(dgvDadosPecas.CurrentRow)

            e.Handled = True
        End If

    End Sub

    Private Sub CancelarAFabricaçãoDaOSToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CancelarAFabricaçãoDaOSToolStripMenuItem.Click

    End Sub

    Private Sub dgvProcessos_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvProcessos.CellContentClick

    End Sub

    Private Sub btnBuscarOperacaoProtheus_Click(sender As Object, e As EventArgs) Handles btnBuscarOperacaoProtheus.Click

        ' DadosArquivoCorrente.NomeArquivoSemExtensao = DadosArquivoCorrente.NomeArquivoComExtensao.Replace(".par", "").Replace(".psm", "").Replace(".asm", "")


        Protheus.GerarTabelaTempProcesso(DadosArquivoCorrente.NomeArquivoComExtensao, "")


        Me.dgvProcessos.CurrentRow.DefaultCellStyle.BackColor = System.Drawing.Color.LightGreen


        TimerdgvProcessoMaterial.Enabled = True



    End Sub

    Private Sub dgvMateriaisProtheus_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvMateriaisProtheus.CellContentClick

    End Sub

    Private Sub btnEstrutraMaterialProtheus_Click(sender As Object, e As EventArgs) Handles btnEstrutraMaterialProtheus.Click

        Dim dt = Protheus.GerarTabelaTempEstrutura("MT4-28433",
                                  DadosArquivoCorrente.IdMaterial,
                                  0,
                                  Usuario.NomeCompleto)

        For Each row As DataRow In dt.Rows
            classePecaManufaturada.SalvarDados("",
        row("TipoPeca").ToString(),
        CInt(row("IdMaterial")),
        CDec(row("PecaQtde")),
        CInt(row("IdMaterialPeca")),
        CInt(row("IdEmpresa")),
        "", "", "", "", "",
        row("CodMatFabricante").ToString(),
        row("UsuarioCriacao").ToString(),
        CDate(row("DataCriacao"))
    )
        Next

        TimerManufaturada.Enabled = True



    End Sub

    Private Sub AlterarQtdeToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AlterarQtdeToolStripMenuItem.Click


        ' 1. PRIMEIRO: Garante que existe uma linha selecionada no Grid
        If dgvDadosPecas.CurrentRow Is Nothing Then
            MessageBox.Show("Por favor, selecione uma peça na lista para alterar a quantidade.",
                            "Nenhuma seleção", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        ' 2. Captura a entrada como String
        Dim inputString As String = InputBox("Informe Nova Qtde", "Qtde", "1")

        ' 3. Verifica se cancelou ou deixou vazio
        If String.IsNullOrWhiteSpace(inputString) Then
            Exit Sub
        End If

        ' 4. Prepara conversão (aceita ponto ou vírgula)
        Dim qtde As Double
        Dim valorLimpo As String = inputString.Replace(",", ".")

        ' 5. Valida se é número e se é maior que zero
        If Double.TryParse(valorLimpo, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, qtde) AndAlso qtde > 0 Then

            Try
                ' 6. Pega o ID da linha selecionada com segurança
                Dim idParaAlterar As String = dgvDadosPecas.CurrentRow.Cells("IdMontaPeca").Value.ToString()

                ' 7. Executa atualização no Banco
                cl_BancoDados.AlteracaoEspecifica("montapeca", "pecaqtde", qtde, "IdMontaPeca", idParaAlterar)

                ' 8. Atualiza o visual do Grid
                dgvDadosPecas.CurrentRow.Cells("pecaqtde").Value = qtde

            Catch ex As Exception
                MessageBox.Show("Erro ao gravar no banco: " & ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try

        Else
            MessageBox.Show("Quantidade inválida!" & vbCrLf &
                            "Informe um número maior que zero.",
                            "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End If

    End Sub

    Private Sub btnPdfLote_Click(sender As Object, e As EventArgs) Handles btnPdfLote.Click
        ' 1. Tenta sugerir a pasta do documento ativo (se houver)
        Dim sugestaoCaminho As String = ""
        Try
            Dim seApp As SolidEdgeFramework.Application = Marshal.GetActiveObject("SolidEdge.Application")
            If seApp.Documents.Count > 0 Then
                sugestaoCaminho = IO.Path.GetDirectoryName(seApp.ActiveDocument.FullName)
            End If
        Catch
        End Try

        ' 2. Abre uma caixa de texto para colar o caminho
        Dim caminhoEscolhido As String = InputBox("Cole o caminho da pasta completa aqui:" & vbCrLf & vbCrLf &
                                                  "Exemplo: C:\Projetos\Maquina01\Des",
                                                  "Seleção Direta de Pasta",
                                                  sugestaoCaminho)

        ' 3. Verifica se o usuário cancelou (string vazia)
        If String.IsNullOrWhiteSpace(caminhoEscolhido) Then Exit Sub

        ' 4. Valida se a pasta realmente existe
        If IO.Directory.Exists(caminhoEscolhido) Then

            If MessageBox.Show($"Pasta validada: {caminhoEscolhido}" & vbCrLf & "Iniciar conversão?",
                               "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then

                ConverterTodosDftEmPasta(caminhoEscolhido, 0)

            End If
        Else
            MessageBox.Show("O caminho informado não existe ou está incorreto.",
                            "Pasta não encontrada", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If

    End Sub

    Public Sub ConverterTodosDftEmPasta(ByVal pastaRaiz As String, Optional modoDestino As Integer = 0)
        Dim seApp As SolidEdgeFramework.Application = Nothing

        Try
            ' 1. Verifica se a pasta existe
            If Not Directory.Exists(pastaRaiz) Then
                MessageBox.Show("A pasta raiz não existe.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub
            End If

            ' 2. Conecta ou Inicia o Solid Edge (Necessário para abrir os arquivos)
            Try
                seApp = Marshal.GetActiveObject("SolidEdge.Application")
            Catch
                seApp = Activator.CreateInstance(Type.GetTypeFromProgID("SolidEdge.Application"))
                seApp.Visible = True
            End Try

            ' Desativa atualizações de tela para acelerar o processo
            seApp.ScreenUpdating = False
            seApp.DisplayAlerts = False

            ' 3. Busca todos os arquivos .dft em todos os subdiretórios
            Dim arquivosDFT As String() = Directory.GetFiles(pastaRaiz, "*.dft", SearchOption.AllDirectories)

            If arquivosDFT.Length = 0 Then
                MessageBox.Show("Nenhum arquivo .dft encontrado.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information)
                GoTo Finalizar
            End If

            ' 4. Loop pelos arquivos encontrados
            For Each arquivo As String In arquivosDFT
                Try
                    ' Abre o arquivo no Solid Edge
                    Dim doc As Object = seApp.Documents.Open(arquivo)

                    ' ====================================================
                    ' 🔹 CHAMA SUA FUNÇÃO EXISTENTE AQUI
                    ' ====================================================
                    ' Sua função pega o "ActiveDocument", então o arquivo precisa estar aberto
                    ExportarPDFDetalhamentoLote(modoDestino)

                    ' Fecha o documento se sua função não fechar (segurança)
                    ' Nota: Sua função atual já fecha o draftDoc, mas verificamos apenas para garantir
                    Try
                        ' Verifica se o documento ainda está aberto antes de tentar fechar
                        ' (Isso evita erro se sua função já tiver fechado)
                        Dim docAberto As Object = seApp.ActiveDocument
                        If docAberto IsNot Nothing AndAlso docAberto.FullName = arquivo Then
                            docAberto.Close(False)
                        End If
                    Catch
                    End Try

                Catch ex As Exception
                    Debug.WriteLine($"Erro ao processar arquivo {arquivo}: {ex.Message}")
                End Try
            Next

            MessageBox.Show("Processamento em lote concluído!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information)

Finalizar:
            ' 5. Restaura configurações do Solid Edge
            seApp.ScreenUpdating = True
            seApp.DisplayAlerts = True

        Catch ex As Exception
            MessageBox.Show($"Erro geral: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error)
            If seApp IsNot Nothing Then
                seApp.ScreenUpdating = True
                seApp.DisplayAlerts = True
            End If
        End Try

    End Sub
End Class




Module ModListarComponentesMontagem

#Region "VERSÃO ANTERIOR - 26-11-2025"
    'Public Sub ListarEstruturaComTodasPropriedadesBlank(ByVal dgv As DataGridView,
    '                                                ByVal pgb As ProgressBar,
    '                                                ByVal lbl As Label, lblResumo As Label)

    '    ' Lista que acumula erros
    '    Dim erros As New List(Of String)

    '    ' Contador de documentos que deram RPC_E_DISCONNECTED
    '    Dim qtdRpcDesconectado As Integer = 0

    '    ' Helper para registrar erros com contexto
    '    Dim RegistrarErro As Action(Of String, Exception) =
    '    Sub(contexto As String, ex As Exception)
    '        Try
    '            If ex Is Nothing Then Return

    '            Dim msg As String

    '            ' Detecta erro COM de objeto desconectado (RPC_E_DISCONNECTED)
    '            Dim comEx = TryCast(ex, System.Runtime.InteropServices.COMException)
    '            If comEx IsNot Nothing AndAlso comEx.ErrorCode = &H80010108 Then
    '                qtdRpcDesconectado += 1
    '                msg = $"{contexto}: [RPC_E_DISCONNECTED] O objeto do Solid Edge foi desconectado. Detalhe: {ex.Message}"
    '            Else
    '                msg = $"{contexto}: {ex.Message}"
    '            End If

    '            erros.Add(msg)
    '        Catch
    '            ' não deixa o log quebrar o fluxo
    '        End Try
    '    End Sub

    '    Try
    '        '============================================================
    '        ' 🔹 Sessão do Solid Edge e documento ativo
    '        '============================================================
    '        If app Is Nothing OrElse app.Documents.Count = 0 Then
    '            MessageBox.Show("Nenhum documento aberto no Solid Edge.",
    '                        "SINCO - Solid Edge",
    '                        MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
    '            Exit Sub
    '        End If

    '        Dim doc As Object = app.ActiveDocument
    '        If doc Is Nothing Then Exit Sub

    '        '============================================================
    '        ' 🔹 UI - estado inicial
    '        '============================================================
    '        pgb.Visible = True : pgb.Style = ProgressBarStyle.Marquee : pgb.Value = 0
    '        lbl.Visible = True : lbl.Text = "Lendo estrutura... aguarde."

    '        '============================================================
    '        ' 🔹 Tabela destino
    '        '============================================================
    '        Dim tabela As New DataTable("BOM_Completa")

    '        ' Colunas fixas
    '        tabela.Columns.Add("Nivel", GetType(Integer))
    '        tabela.Columns.Add("Arquivo", GetType(String))          ' com extensão
    '        tabela.Columns.Add("TipoArquivo", GetType(String))      ' PAR/PSM/ASM...
    '        tabela.Columns.Add("Qtde", GetType(Integer))            ' quantidade efetiva no Nivel
    '        tabela.Columns.Add("CaminhoCompleto", GetType(String))

    '        ' Propriedades de interesse como colunas próprias
    '        Dim propsInteressantes As New List(Of String) From {
    '        "Título", "Assunto", "Coment", "Tipo de Desenho", "Palavras-", "Autor", "Empresa",
    '        "Categoria", "Gerente", "Material", "Data", "DataR", "Data1",
    '        "Revision", "CutSizeX", "CutSizeY", "Thickness",
    '        "Mass", "Área_de_superfície", "Area_de_superficie", "Bloqueado"
    '    }
    '        For Each p In propsInteressantes
    '            If Not tabela.Columns.Contains(p) Then tabela.Columns.Add(p, GetType(String))
    '        Next

    '        ' Colunas de totais
    '        tabela.Columns.Add("RNC", GetType(String))
    '        tabela.Columns.Add("Fator", GetType(Double))
    '        tabela.Columns.Add("QtdeTotal", GetType(Double))
    '        tabela.Columns.Add("PesoTotal", GetType(Double))
    '        tabela.Columns.Add("AreaPinturaTotal", GetType(Double))

    '        ' Índice para agrupar por (Nivel|Arquivo)
    '        Dim indexPorChave As New Dictionary(Of String, DataRow)(StringComparer.OrdinalIgnoreCase)

    '        Dim totalLidos As Integer = 0
    '        Dim totalEstimado As Integer = 1

    '        '============================================================
    '        ' 🔹 Helpers
    '        '============================================================
    '        Dim LimparUnidades As Func(Of String, String) =
    '        Function(txt As String) As String
    '            If String.IsNullOrWhiteSpace(txt) Then Return ""
    '            Return txt _
    '                .Replace("mm²", "") _
    '                .Replace("MM²", "") _
    '                .Replace("mm", "") _
    '                .Replace("MM", "") _
    '                .Replace("^2", "") _
    '                .Replace("KG", "") _
    '                .Replace("kg", "") _
    '                .Trim()
    '        End Function

    '        Dim FormatarValor As Func(Of String, String, String) =
    '        Function(propNome As String, valorBruto As String) As String
    '            If String.IsNullOrWhiteSpace(valorBruto) Then Return ""
    '            Dim nome = propNome.Trim().ToUpperInvariant()
    '            Dim valor = LimparUnidades(valorBruto)

    '            ' Datas
    '            If nome = "DATA" OrElse nome = "DATAR" OrElse nome = "DATA1" Then
    '                Try : Return cl_BancoDados.FormatarData(valor) : Catch : Return valor : End Try
    '            End If

    '            ' Revisão
    '            If nome.Contains("REVISION") Then Return valor.ToUpperInvariant()

    '            ' CutSize / Espessura / Massa -> ponto decimal
    '            If nome.Contains("CUTSIZEX") Then Return valor.Replace(",", ".")
    '            If nome.Contains("CUTSIZEY") Then Return valor.Replace(",", ".")
    '            If nome.Contains("THICKNESS") Then Return valor.Replace(",", ".")
    '            If nome.Contains("MASS") Then Return valor.Replace(",", ".")

    '            ' Área mm² -> m²
    '            If nome.Contains("ÁREA") OrElse nome.Contains("AREA") Then
    '                Dim num As Double
    '                If Double.TryParse(valor.Replace(",", "."), NumberStyles.Any,
    '                                   CultureInfo.InvariantCulture, num) Then
    '                    Dim m2 = num / 1_000_000.0
    '                    Return m2.ToString("N4", CultureInfo.GetCultureInfo("pt-BR"))
    '                End If
    '            End If

    '            Return valor
    '        End Function

    '        Dim PreencherPropriedades As Action(Of Object, DataRow) =
    '        Sub(docObj As Object, linha As DataRow)
    '            If docObj Is Nothing Then Exit Sub
    '            Try
    '                Dim propSets As Object = docObj.Properties
    '                For Each propSet As Object In propSets
    '                    For Each prop As Object In propSet
    '                        If prop Is Nothing Then Continue For

    '                        Dim nomeProp As String = ""
    '                        Try : nomeProp = CStr(prop.Name) : Catch : Continue For : End Try
    '                        If String.IsNullOrWhiteSpace(nomeProp) Then Continue For

    '                        ' 1) Preenche colunas de interesse
    '                        For Each alvo In propsInteressantes
    '                            If nomeProp.IndexOf(alvo, StringComparison.OrdinalIgnoreCase) >= 0 Then
    '                                Dim valorBruto As String = ""
    '                                Try
    '                                    Dim v = prop.Value
    '                                    If v IsNot Nothing Then valorBruto = v.ToString()
    '                                Catch ex As Exception
    '                                    valorBruto = "(erro)"
    '                                    RegistrarErro($"Lendo propriedade '{nomeProp}'", ex)
    '                                End Try
    '                                linha(alvo) = FormatarValor(nomeProp, valorBruto)
    '                                Exit For
    '                            End If
    '                        Next

    '                        ' 2) Mapa de espessura -> Thickness
    '                        If nomeProp.IndexOf("Material Thickness", StringComparison.OrdinalIgnoreCase) >= 0 _
    '                           OrElse nomeProp.IndexOf("Espessura Do Material", StringComparison.OrdinalIgnoreCase) >= 0 _
    '                           OrElse nomeProp.Equals("Thickness", StringComparison.OrdinalIgnoreCase) Then

    '                            Dim valorBruto As String = ""
    '                            Try
    '                                Dim v = prop.Value
    '                                If v IsNot Nothing Then valorBruto = v.ToString()
    '                            Catch ex As Exception
    '                                RegistrarErro($"Lendo espessura em '{nomeProp}'", ex)
    '                            End Try

    '                            If Not String.IsNullOrWhiteSpace(valorBruto) Then
    '                                Dim limpo = LimparUnidades(valorBruto).Replace(",", ".")
    '                                linha("Thickness") = limpo
    '                            End If
    '                        End If

    '                    Next
    '                Next
    '            Catch ex As Exception
    '                RegistrarErro("PreencherPropriedades", ex)
    '            End Try
    '        End Sub

    '        Dim AdicionarOuSomarLinha As Action(Of Integer, String, String, Integer, String, Object) =
    '        Sub(nivel As Integer, nomeComExt As String, tipoArquivo As String, qtdeEfetiva As Integer, caminho As String, docObj As Object)
    '            Try
    '                Dim chave = $"{nivel}|{nomeComExt}".ToUpperInvariant()

    '                If Not indexPorChave.ContainsKey(chave) Then
    '                    Dim linha = tabela.NewRow()
    '                    linha("Nivel") = nivel
    '                    linha("Arquivo") = nomeComExt
    '                    linha("TipoArquivo") = tipoArquivo
    '                    linha("Qtde") = qtdeEfetiva
    '                    linha("CaminhoCompleto") = caminho
    '                    For Each p In propsInteressantes : linha(p) = "" : Next
    '                    linha("RNC") = "" : linha("Fator") = 1.0
    '                    linha("QtdeTotal") = CDbl(qtdeEfetiva)
    '                    linha("PesoTotal") = 0.0
    '                    linha("AreaPinturaTotal") = 0.0

    '                    PreencherPropriedades(docObj, linha)

    '                    Dim mass As Double = 0
    '                    Double.TryParse(Convert.ToString(linha("Mass")).Replace(",", "."),
    '                                    NumberStyles.Any, CultureInfo.InvariantCulture, mass)

    '                    Dim area As Double = 0
    '                    Double.TryParse(Convert.ToString(linha("Área_de_superfície")).Replace(",", "."),
    '                                    NumberStyles.Any, CultureInfo.InvariantCulture, area)

    '                    linha("PesoTotal") = mass * qtdeEfetiva
    '                    linha("AreaPinturaTotal") = area * qtdeEfetiva

    '                    tabela.Rows.Add(linha)
    '                    indexPorChave(chave) = linha

    '                Else
    '                    Dim linha = indexPorChave(chave)

    '                    Dim qtAnt As Integer = 0
    '                    Integer.TryParse(linha("Qtde").ToString(), qtAnt)
    '                    Dim qtNova = qtAnt + qtdeEfetiva
    '                    linha("Qtde") = qtNova
    '                    linha("QtdeTotal") = CDbl(qtNova)

    '                    Dim massUnit As Double = 0
    '                    Double.TryParse(Convert.ToString(linha("Mass")).Replace(",", "."),
    '                                    NumberStyles.Any, CultureInfo.InvariantCulture, massUnit)

    '                    Dim areaUnit As Double = 0
    '                    Double.TryParse(Convert.ToString(linha("Área_de_superfície")).Replace(",", "."),
    '                                    NumberStyles.Any, CultureInfo.InvariantCulture, areaUnit)

    '                    Dim pesoTot As Double = 0
    '                    Double.TryParse(linha("PesoTotal").ToString().Replace(",", "."),
    '                                    NumberStyles.Any, CultureInfo.InvariantCulture, pesoTot)

    '                    Dim areaTot As Double = 0
    '                    Double.TryParse(linha("AreaPinturaTotal").ToString().Replace(",", "."),
    '                                    NumberStyles.Any, CultureInfo.InvariantCulture, areaTot)

    '                    linha("PesoTotal") = pesoTot + (massUnit * qtdeEfetiva)
    '                    linha("AreaPinturaTotal") = areaTot + (areaUnit * qtdeEfetiva)
    '                End If
    '            Catch ex As Exception
    '                RegistrarErro($"AdicionarOuSomarLinha [{nomeComExt}]", ex)
    '            End Try
    '        End Sub

    '        '============================================================
    '        ' 🔹 Recursão (propaga multiplicador do pai)
    '        '============================================================
    '        Dim Ler As Action(Of Object, String, Integer, Integer) =
    '        Sub(docObj As Object, caminhoArquivo As String, nivel As Integer, multiplicadorPai As Integer)
    '            Try
    '                If docObj Is Nothing OrElse String.IsNullOrWhiteSpace(caminhoArquivo) Then Exit Sub

    '                totalLidos += 1
    '                lbl.Text = $"Lendo {IO.Path.GetFileName(caminhoArquivo)} ({totalLidos})"
    '                ''  System.Windows.Forms.Application.DoEvents()

    '                If totalEstimado > 1 Then
    '                    pgb.Style = ProgressBarStyle.Continuous
    '                    pgb.Value = Math.Min(100, CInt((totalLidos / Math.Max(1, totalEstimado)) * 100))
    '                End If

    '                Dim ext As String = IO.Path.GetExtension(caminhoArquivo).ToLower()
    '                Dim nomeComExt As String = IO.Path.GetFileName(caminhoArquivo)
    '                Dim tipoUpper As String = ext.Replace(".", "").ToUpperInvariant()

    '                ' Adiciona/Acumula a linha do próprio documento
    '                AdicionarOuSomarLinha(nivel, nomeComExt, tipoUpper, multiplicadorPai, caminhoArquivo, docObj)

    '                ' Apenas ASM tem filhos
    '                If ext <> ".asm" Then Exit Sub

    '                ' Ocorrências
    '                Dim occsObj As Object = Nothing
    '                Try
    '                    occsObj = CallByName(docObj, "Occurrences", CallType.Get)
    '                Catch ex As Exception
    '                    RegistrarErro($"Lendo Occurrences de {nomeComExt}", ex)
    '                    occsObj = Nothing
    '                End Try
    '                If occsObj Is Nothing Then Exit Sub

    '                Dim count As Integer = 0
    '                Try
    '                    count = CInt(CallByName(occsObj, "Count", CallType.Get))
    '                Catch ex As Exception
    '                    RegistrarErro($"Lendo Count de {nomeComExt}", ex)
    '                    count = 0
    '                End Try
    '                If count <= 0 Then Exit Sub

    '                totalEstimado += count

    '                ' Agrupa por arquivo dentro do Nivel
    '                Dim grupo As New Dictionary(Of String, (Caminho As String, QtdeLocal As Integer, DocObj As Object))(StringComparer.OrdinalIgnoreCase)

    '                For i As Integer = 1 To count
    '                    Try
    '                        Dim occ As Object = CallByName(occsObj, "Item", CallType.Method, i)

    '                        Dim subPath As String = ""
    '                        Try
    '                            subPath = CStr(CallByName(occ, "OccurrenceFileName", CallType.Get))
    '                        Catch ex As Exception
    '                            RegistrarErro($"Lendo OccurrenceFileName (#{i}) de {nomeComExt}", ex)
    '                            subPath = ""
    '                        End Try

    '                        Dim subDoc As Object = Nothing
    '                        Try
    '                            subDoc = CallByName(occ, "OccurrenceDocument", CallType.Get)
    '                        Catch ex As Exception
    '                            RegistrarErro($"Lendo OccurrenceDocument (#{i}) de {nomeComExt}", ex)
    '                            subDoc = Nothing
    '                        End Try

    '                        If subDoc Is Nothing AndAlso Not String.IsNullOrWhiteSpace(subPath) AndAlso IO.File.Exists(subPath) Then
    '                            Try
    '                                subDoc = app.Documents.Open(subPath)
    '                            Catch ex As Exception
    '                                RegistrarErro($"Abrindo subdocumento '{subPath}'", ex)
    '                                subDoc = Nothing
    '                            End Try
    '                        End If

    '                        Dim nomeAgrup As String = If(String.IsNullOrWhiteSpace(subPath), $"ITEM_{i}", IO.Path.GetFileName(subPath)).ToUpperInvariant()

    '                        Dim qLocal As Integer = 1
    '                        Try
    '                            Dim qtdProp As Object = CallByName(occ, "Quantity", CallType.Get)
    '                            If qtdProp IsNot Nothing Then
    '                                Dim qi As Integer
    '                                If Integer.TryParse(qtdProp.ToString(), qi) AndAlso qi > 0 Then qLocal = qi
    '                            End If
    '                        Catch ex As Exception
    '                            RegistrarErro($"Lendo Quantity (#{i}) de {nomeComExt}", ex)
    '                        End Try

    '                        If grupo.ContainsKey(nomeAgrup) Then
    '                            Dim t = grupo(nomeAgrup)
    '                            grupo(nomeAgrup) = (t.Caminho, t.QtdeLocal + qLocal, If(t.DocObj Is Nothing, subDoc, t.DocObj))
    '                        Else
    '                            grupo.Add(nomeAgrup, (If(String.IsNullOrWhiteSpace(subPath), "", subPath), qLocal, subDoc))
    '                        End If
    '                    Catch ex As Exception
    '                        RegistrarErro($"Processando ocorrência (#{i}) de {nomeComExt}", ex)
    '                    End Try
    '                Next

    '                For Each kv In grupo

    '                    Dim caminhoSub = kv.Value.Caminho
    '                    Dim qLocal = kv.Value.QtdeLocal
    '                    Dim qEfetivaFilho As Integer = qLocal * multiplicadorPai
    '                    Dim subDoc = kv.Value.DocObj

    '                    ' Se ainda não temos caminho mas temos documento, tenta pegar o FullName
    '                    If String.IsNullOrWhiteSpace(caminhoSub) AndAlso subDoc IsNot Nothing Then
    '                        Try
    '                            caminhoSub = CStr(CallByName(subDoc, "FullName", CallType.Get))
    '                        Catch ex As Exception
    '                            RegistrarErro("Lendo FullName de subdocumento", ex)
    '                            caminhoSub = ""
    '                        End Try
    '                    End If

    '                    ' Nome e tipo do filho (mesmo sem caminho)
    '                    Dim nomeFilho As String
    '                    Dim tipoFilho As String

    '                    If Not String.IsNullOrWhiteSpace(caminhoSub) Then
    '                        nomeFilho = IO.Path.GetFileName(caminhoSub)
    '                        tipoFilho = IO.Path.GetExtension(caminhoSub).Replace(".", "").ToUpperInvariant()
    '                    Else
    '                        ' fallback: usa a chave do grupo como nome
    '                        nomeFilho = kv.Key
    '                        tipoFilho = ""
    '                    End If

    '                    If subDoc IsNot Nothing AndAlso Not String.IsNullOrWhiteSpace(caminhoSub) Then
    '                        ' ✅ Caso NORMAL: temos o documento
    '                        '    → NÃO adiciona aqui, deixa o Ler(...) do filho fazer o AdicionarOuSomarLinha
    '                        Ler(subDoc, caminhoSub, nivel + 1, qEfetivaFilho)

    '                        ' Fecha se abrimos temporariamente
    '                        Try
    '                            Dim isActive As Boolean = False
    '                            Try : isActive = (TryCast(subDoc, Object) Is app.ActiveDocument) : Catch : End Try
    '                            If Not isActive AndAlso IO.File.Exists(caminhoSub) Then
    '                                Try : CallByName(subDoc, "Close", CallType.Method, False) : Catch : End Try
    '                            End If
    '                        Catch ex As Exception
    '                            RegistrarErro($"Fechando subdocumento '{caminhoSub}'", ex)
    '                        End Try

    '                    Else
    '                        ' ⚠ Caso EXCEÇÃO: não há documento carregado,
    '                        '    mas precisamos que a peça apareça na lista.
    '                        '    → adiciona a linha AQUI, uma única vez.
    '                        AdicionarOuSomarLinha(
    '                            nivel + 1,
    '                            nomeFilho,
    '                            tipoFilho,
    '                            qEfetivaFilho,
    '                            caminhoSub,
    '                            subDoc
    '                        )
    '                    End If

    '                Next

    '            Catch ex As Exception
    '                RegistrarErro($"Ler('{caminhoArquivo}')", ex)
    '            End Try
    '        End Sub

    '        '============================================================
    '        ' 🔹 Chamada inicial da recursão (multiplicador raiz = 1)
    '        '============================================================
    '        Ler(doc, doc.FullName, 0, 1)

    '        '============================================================
    '        ' 🔹 Limpeza de linhas “fantasmas” (sem arquivo/caminho)
    '        '============================================================
    '        For i As Integer = tabela.Rows.Count - 1 To 0 Step -1
    '            Dim arq As String = Convert.ToString(tabela.Rows(i)("Arquivo"))
    '            Dim cam As String = Convert.ToString(tabela.Rows(i)("CaminhoCompleto"))

    '            If String.IsNullOrWhiteSpace(arq) AndAlso String.IsNullOrWhiteSpace(cam) Then
    '                tabela.Rows.RemoveAt(i)
    '            End If
    '        Next

    '        '============================================================
    '        ' 🔹 Cálculo dos TOTAIS
    '        '============================================================
    '        Dim totalQtdSolicitada As Double = 0
    '        Dim arquivosUnicos As New HashSet(Of String)(StringComparer.OrdinalIgnoreCase)

    '        For Each row As DataRow In tabela.Rows
    '            ' 1) Soma da QtdeTotal
    '            Dim qtd As Double
    '            Double.TryParse(
    '            Convert.ToString(row("QtdeTotal")).Replace(",", "."),
    '            NumberStyles.Any,
    '            CultureInfo.InvariantCulture,
    '            qtd
    '        )
    '            totalQtdSolicitada += qtd

    '            ' 2) Arquivo distinto
    '            Dim arq As String = Convert.ToString(row("Arquivo"))
    '            If Not String.IsNullOrWhiteSpace(arq) Then
    '                arquivosUnicos.Add(arq)
    '            End If
    '        Next

    '        Dim totalPecasDiferentes As Integer = arquivosUnicos.Count
    '        Dim totalPecas As Integer = tabela.Rows.Count

    '        '============================================================
    '        ' 🔹 Bind no grid
    '        '============================================================
    '        dgv.DataSource = tabela
    '        dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells
    '        dgv.AllowUserToAddRows = False
    '        dgv.ColumnHeadersDefaultCellStyle.Font = New Font(dgv.Font, FontStyle.Bold)

    '        If dgv.Columns.Contains("Arquivo") Then dgv.Columns("Arquivo").Frozen = True

    '        tabelaOriginalBOM = tabela.Copy()

    '        Dim ocultar() As String = {"TipoArquivo", "CaminhoCompleto", "Categoria", "Gerente",
    '                               "Autor", "Empresa", "Data", "DataR", "Data1",
    '                               "Revision", "CutSizeX", "CutSizeY", "Mass", "Área_de_superfície", "Area_de_superficie", "Bloqueado",
    '                               "RNC", "PesoTotal", "AreaPinturaTotal"}

    '        For Each c In ocultar
    '            If dgv.Columns.Contains(c) Then dgv.Columns(c).Visible = False
    '        Next

    '        '============================================================
    '        ' 🔹 UI - final (incluindo os totais)
    '        '============================================================
    '        pgb.Style = ProgressBarStyle.Continuous
    '        pgb.Value = 100

    '        lblResumo.Text =
    '        "✅ Estrutura concluída." & vbCrLf &
    '        $"• Total de peças solicitadas (QtdeTotal): {totalQtdSolicitada}" & vbCrLf &
    '        $"• Total de peças diferentes (arquivos únicos): {totalPecasDiferentes}" & vbCrLf &
    '        $"• Total de peças na estrutura (linhas): {totalPecas}"


    '        '' System.Windows.Forms.Application.DoEvents()
    '        Threading.Thread.Sleep(550)

    '    Catch ex As Exception
    '        ' Erro geral do processo
    '        RegistrarErro("Erro geral em ListarEstruturaComTodasPropriedadesBlank", ex)

    '    Finally
    '        pgb.Visible = False

    '        ' Se houve erros, mostra relatório organizado
    '        If erros IsNot Nothing AndAlso erros.Count > 0 Then
    '            Dim sb As New StringBuilder()
    '            sb.AppendLine("Foram encontrados " & erros.Count & " erro(s) durante a leitura da estrutura:")
    '            sb.AppendLine()

    '            Dim i As Integer = 1
    '            For Each msg In erros
    '                sb.AppendLine(i.ToString("00") & " - " & msg)
    '                i += 1
    '            Next

    '            If qtdRpcDesconectado > 0 Then
    '                sb.AppendLine()
    '                sb.AppendLine(qtdRpcDesconectado.ToString() &
    '                          " erro(s) são do tipo RPC_E_DISCONNECTED (necessário revisar/ajustar o arquivo no Solid Edge).")
    '            End If

    '            MessageBox.Show(sb.ToString(),
    '                        "Relatório de erros - Estrutura",
    '                        MessageBoxButtons.OK,
    '                        MessageBoxIcon.Warning)

    '        End If

    '    End Try


    'End Sub
#End Region


#Region "VERSÃO ANTERIOR - 26-11-2025 rev 00"
    Public Sub ListarEstruturaComTodasPropriedadesBlank(ByVal dgv As DataGridView,
                                                    ByVal pgb As ProgressBar,
                                                    ByVal lbl As Label, lblResumo As Label)

        ' Lista que acumula erros
        Dim erros As New List(Of String)

        ' Contador de documentos que deram RPC_E_DISCONNECTED
        Dim qtdRpcDesconectado As Integer = 0

        ' Helper para registrar erros com contexto
        Dim RegistrarErro As Action(Of String, Exception) =
    Sub(contexto As String, ex As Exception)
        Try
            If ex Is Nothing Then Return

            Dim msg As String

            ' Detecta erro COM de objeto desconectado (RPC_E_DISCONNECTED)
            Dim comEx = TryCast(ex, System.Runtime.InteropServices.COMException)
            If comEx IsNot Nothing AndAlso comEx.ErrorCode = &H80010108 Then
                qtdRpcDesconectado += 1
                msg = $"{contexto}: [RPC_E_DISCONNECTED] O objeto do Solid Edge foi desconectado. Detalhe: {ex.Message}"
            Else
                msg = $"{contexto}: {ex.Message}"
            End If

            erros.Add(msg)
        Catch
            ' não deixa o log quebrar o fluxo
        End Try
    End Sub

        Try
            '============================================================
            ' 🔹 Sessão do Solid Edge e documento ativo
            '============================================================
            If app Is Nothing OrElse app.Documents.Count = 0 Then
                MessageBox.Show("Nenhum documento aberto no Solid Edge.",
                        "SINCO - Solid Edge",
                        MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Exit Sub
            End If

            Dim doc As Object = app.ActiveDocument
            If doc Is Nothing Then Exit Sub

            '============================================================
            ' 🔹 UI - estado inicial
            '============================================================
            pgb.Visible = True : pgb.Style = ProgressBarStyle.Marquee : pgb.Value = 0
            lbl.Visible = True : lbl.Text = "Lendo estrutura... aguarde."

            '============================================================
            ' 🔹 Tabela destino
            '============================================================
            Dim tabela As New DataTable("BOM_Completa")

            ' Colunas fixas
            tabela.Columns.Add("Nivel", GetType(Integer))
            tabela.Columns.Add("Arquivo", GetType(String))          ' com extensão
            tabela.Columns.Add("TipoArquivo", GetType(String))      ' PAR/PSM/ASM...
            tabela.Columns.Add("Qtde", GetType(Integer))            ' quantidade efetiva no Nivel
            tabela.Columns.Add("CaminhoCompleto", GetType(String))

            ' Propriedades de interesse como colunas próprias
            Dim propsInteressantes As New List(Of String) From {
            "Título", "Assunto", "Coment", "Tipo de Desenho", "Palavras-", "Autor", "Empresa",
            "Categoria", "Gerente", "Material", "Data", "DataR", "Data1",
            "Revision", "CutSizeX", "CutSizeY", "Thickness",
            "Mass", "Área_de_superfície", "Area_de_superficie", "Bloqueado"
        }
            For Each p In propsInteressantes
                If Not tabela.Columns.Contains(p) Then tabela.Columns.Add(p, GetType(String))
            Next

            ' Colunas de totais
            tabela.Columns.Add("RNC", GetType(String))
            tabela.Columns.Add("Fator", GetType(Double))
            tabela.Columns.Add("QtdeTotal", GetType(Double))
            tabela.Columns.Add("PesoTotal", GetType(Double))
            tabela.Columns.Add("AreaPinturaTotal", GetType(Double))

            ' Índice para agrupar por (Nivel|Arquivo)
            Dim indexPorChave As New Dictionary(Of String, DataRow)(StringComparer.OrdinalIgnoreCase)

            Dim totalLidos As Integer = 0
            Dim totalEstimado As Integer = 1

            '============================================================
            ' 🔹 Helpers
            '============================================================
            Dim LimparUnidades As Func(Of String, String) =
        Function(txt As String) As String
            If String.IsNullOrWhiteSpace(txt) Then Return ""
            Return txt _
                .Replace("mm²", "") _
                .Replace("MM²", "") _
                .Replace("mm", "") _
                .Replace("MM", "") _
                .Replace("^2", "") _
                .Replace("KG", "") _
                .Replace("kg", "") _
                .Trim()
        End Function

            Dim FormatarValor As Func(Of String, String, String) =
        Function(propNome As String, valorBruto As String) As String
            If String.IsNullOrWhiteSpace(valorBruto) Then Return ""
            Dim nome = propNome.Trim().ToUpperInvariant()
            Dim valor = LimparUnidades(valorBruto)

            ' Datas
            If nome = "DATA" OrElse nome = "DATAR" OrElse nome = "DATA1" Then
                Try : Return cl_BancoDados.FormatarData(valor) : Catch : Return valor : End Try
            End If

            ' Revisão
            If nome.Contains("REVISION") Then Return valor.ToUpperInvariant()

            ' CutSize / Espessura / Massa -> ponto decimal
            If nome.Contains("CUTSIZEX") Then Return valor.Replace(",", ".")
            If nome.Contains("CUTSIZEY") Then Return valor.Replace(",", ".")
            If nome.Contains("THICKNESS") Then Return valor.Replace(",", ".")
            If nome.Contains("MASS") Then Return valor.Replace(",", ".")

            ' Área mm² -> m²
            If nome.Contains("ÁREA") OrElse nome.Contains("AREA") Then
                Dim num As Double
                If Double.TryParse(valor.Replace(",", "."), NumberStyles.Any,
                                   CultureInfo.InvariantCulture, num) Then
                    Dim m2 = num / 1_000_000.0
                    Return m2.ToString("N4", CultureInfo.GetCultureInfo("pt-BR"))
                End If
            End If

            Return valor
        End Function

            ' ============================================================
            ' 🔹 PreencherPropriedades com retry + fechar docs abertos aqui
            ' ============================================================
            Dim PreencherPropriedades As Action(Of Object, DataRow) =
            Sub(docObj As Object, linha As DataRow)
                If linha Is Nothing Then Exit Sub

                Dim tentativas As Integer = 0
                Dim abriuTemp As Boolean = False   ' <-- controla se foi aberto aqui

                While tentativas < 2
                    Try
                        ' Se o objeto estiver Nothing (após erro), tenta reabrir pelo caminho da linha
                        If docObj Is Nothing Then
                            Dim caminhoArquivo As String = ""
                            Try
                                caminhoArquivo = Convert.ToString(linha("CaminhoCompleto"))
                            Catch
                                caminhoArquivo = ""
                            End Try

                            If Not String.IsNullOrWhiteSpace(caminhoArquivo) AndAlso IO.File.Exists(caminhoArquivo) Then
                                Try
                                    docObj = app.Documents.Open(caminhoArquivo)
                                    abriuTemp = True           ' <-- marcamos que abrimos aqui
                                Catch exOpen As Exception
                                    RegistrarErro($"Reabrindo documento '{caminhoArquivo}'", exOpen)
                                    Exit While
                                End Try
                            Else
                                ' Sem caminho válido, não há o que fazer
                                Exit While
                            End If
                        End If

                        ' --- CÓDIGO ORIGINAL DE LEITURA DAS PROPRIEDADES ---
                        Dim propSets As Object = docObj.Properties
                        For Each propSet As Object In propSets
                            For Each prop As Object In propSet
                                If prop Is Nothing Then Continue For

                                Dim nomeProp As String = ""
                                Try : nomeProp = CStr(prop.Name) : Catch : Continue For : End Try
                                If String.IsNullOrWhiteSpace(nomeProp) Then Continue For

                                ' 1) Preenche colunas de interesse
                                For Each alvo In propsInteressantes
                                    If nomeProp.IndexOf(alvo, StringComparison.OrdinalIgnoreCase) >= 0 Then
                                        Dim valorBruto As String = ""
                                        Try
                                            Dim v = prop.Value
                                            If v IsNot Nothing Then valorBruto = v.ToString()
                                        Catch ex As Exception
                                            valorBruto = "(erro)"
                                            RegistrarErro($"Lendo propriedade '{nomeProp}'", ex)
                                        End Try
                                        linha(alvo) = FormatarValor(nomeProp, valorBruto)
                                        Exit For
                                    End If
                                Next

                                ' 2) Mapa de espessura -> Thickness
                                If nomeProp.IndexOf("Material Thickness", StringComparison.OrdinalIgnoreCase) >= 0 _
                                   OrElse nomeProp.IndexOf("Espessura Do Material", StringComparison.OrdinalIgnoreCase) >= 0 _
                                   OrElse nomeProp.Equals("Thickness", StringComparison.OrdinalIgnoreCase) Then

                                    Dim valorBruto As String = ""
                                    Try
                                        Dim v = prop.Value
                                        If v IsNot Nothing Then valorBruto = v.ToString()
                                    Catch ex As Exception
                                        RegistrarErro($"Lendo espessura em '{nomeProp}'", ex)
                                    End Try

                                    If Not String.IsNullOrWhiteSpace(valorBruto) Then
                                        Dim limpo = LimparUnidades(valorBruto).Replace(",", ".")
                                        linha("Thickness") = limpo
                                    End If
                                End If

                            Next
                        Next
                        ' ---------------------------------------------------

                        Exit While ' sucesso, sai do loop

                    Catch comEx As System.Runtime.InteropServices.COMException
                        If comEx.ErrorCode = &H80010108 Then
                            ' RPC_E_DISCONNECTED → tenta reabrir e repetir
                            tentativas += 1
                            RegistrarErro("PreencherPropriedades - RPC_E_DISCONNECTED, tentando reabrir documento", comEx)
                            docObj = Nothing
                            Threading.Thread.Sleep(200)
                            Continue While
                        Else
                            RegistrarErro("PreencherPropriedades", comEx)
                            Exit While
                        End If

                    Catch ex As Exception
                        RegistrarErro("PreencherPropriedades", ex)
                        Exit While
                    End Try
                End While

                ' 🔻 Se abrimos o documento aqui, tentamos fechar no final
                If abriuTemp AndAlso docObj IsNot Nothing Then
                    Try
                        Dim isActive As Boolean = False
                        Try
                            isActive = (TryCast(docObj, Object) Is app.ActiveDocument)
                        Catch
                            isActive = False
                        End Try

                        If Not isActive Then
                            Try
                                CallByName(docObj, "Close", CallType.Method, False)
                            Catch ex As Exception
                                RegistrarErro("Fechando documento aberto em PreencherPropriedades", ex)
                            End Try
                        End If
                    Catch
                        ' não deixa o fechamento quebrar o fluxo
                    End Try
                End If
            End Sub
            ' ============================================================
            ' ============================================================

            Dim AdicionarOuSomarLinha As Action(Of Integer, String, String, Integer, String, Object) =
        Sub(nivel As Integer, nomeComExt As String, tipoArquivo As String, qtdeEfetiva As Integer, caminho As String, docObj As Object)
            Try
                Dim chave = $"{nivel}|{nomeComExt}".ToUpperInvariant()

                If Not indexPorChave.ContainsKey(chave) Then
                    Dim linha = tabela.NewRow()
                    linha("Nivel") = nivel
                    linha("Arquivo") = nomeComExt
                    linha("TipoArquivo") = tipoArquivo
                    linha("Qtde") = qtdeEfetiva
                    linha("CaminhoCompleto") = caminho
                    For Each p In propsInteressantes : linha(p) = "" : Next
                    linha("RNC") = "" : linha("Fator") = 1.0
                    linha("QtdeTotal") = CDbl(qtdeEfetiva)
                    linha("PesoTotal") = 0.0
                    linha("AreaPinturaTotal") = 0.0

                    PreencherPropriedades(docObj, linha)

                    Dim mass As Double = 0
                    Double.TryParse(Convert.ToString(linha("Mass")).Replace(",", "."),
                                    NumberStyles.Any, CultureInfo.InvariantCulture, mass)

                    Dim area As Double = 0
                    Double.TryParse(Convert.ToString(linha("Área_de_superfície")).Replace(",", "."),
                                    NumberStyles.Any, CultureInfo.InvariantCulture, area)

                    linha("PesoTotal") = mass * qtdeEfetiva
                    linha("AreaPinturaTotal") = area * qtdeEfetiva

                    tabela.Rows.Add(linha)
                    indexPorChave(chave) = linha

                Else
                    Dim linha = indexPorChave(chave)

                    Dim qtAnt As Integer = 0
                    Integer.TryParse(linha("Qtde").ToString(), qtAnt)
                    Dim qtNova = qtAnt + qtdeEfetiva
                    linha("Qtde") = qtNova
                    linha("QtdeTotal") = CDbl(qtNova)

                    Dim massUnit As Double = 0
                    Double.TryParse(Convert.ToString(linha("Mass")).Replace(",", "."),
                                    NumberStyles.Any, CultureInfo.InvariantCulture, massUnit)

                    Dim areaUnit As Double = 0
                    Double.TryParse(Convert.ToString(linha("Área_de_superfície")).Replace(",", "."),
                                    NumberStyles.Any, CultureInfo.InvariantCulture, areaUnit)

                    Dim pesoTot As Double = 0
                    Double.TryParse(linha("PesoTotal").ToString().Replace(",", "."),
                                    NumberStyles.Any, CultureInfo.InvariantCulture, pesoTot)

                    Dim areaTot As Double = 0
                    Double.TryParse(linha("AreaPinturaTotal").ToString().Replace(",", "."),
                                    NumberStyles.Any, CultureInfo.InvariantCulture, areaTot)

                    linha("PesoTotal") = pesoTot + (massUnit * qtdeEfetiva)
                    linha("AreaPinturaTotal") = areaTot + (areaUnit * qtdeEfetiva)
                End If
            Catch ex As Exception
                RegistrarErro($"AdicionarOuSomarLinha [{nomeComExt}]", ex)
            End Try
        End Sub

            '============================================================
            ' 🔹 Recursão (propaga multiplicador do pai)
            '============================================================
            Dim Ler As Action(Of Object, String, Integer, Integer) =
        Sub(docObj As Object, caminhoArquivo As String, nivel As Integer, multiplicadorPai As Integer)
            Try
                If docObj Is Nothing OrElse String.IsNullOrWhiteSpace(caminhoArquivo) Then Exit Sub

                totalLidos += 1
                lbl.Text = $"Lendo {IO.Path.GetFileName(caminhoArquivo)} ({totalLidos})"
                ''  System.Windows.Forms.Application.DoEvents()

                If totalEstimado > 1 Then
                    pgb.Style = ProgressBarStyle.Continuous
                    pgb.Value = Math.Min(100, CInt((totalLidos / Math.Max(1, totalEstimado)) * 100))
                End If

                Dim ext As String = IO.Path.GetExtension(caminhoArquivo).ToLower()
                Dim nomeComExt As String = IO.Path.GetFileName(caminhoArquivo)
                Dim tipoUpper As String = ext.Replace(".", "").ToUpperInvariant()

                ' Adiciona/Acumula a linha do próprio documento
                AdicionarOuSomarLinha(nivel, nomeComExt, tipoUpper, multiplicadorPai, caminhoArquivo, docObj)

                ' Apenas ASM tem filhos
                If ext <> ".asm" Then Exit Sub

                ' Ocorrências
                Dim occsObj As Object = Nothing
                Try
                    occsObj = CallByName(docObj, "Occurrences", CallType.Get)
                Catch ex As Exception
                    RegistrarErro($"Lendo Occurrences de {nomeComExt}", ex)
                    occsObj = Nothing
                End Try
                If occsObj Is Nothing Then Exit Sub

                Dim count As Integer = 0
                Try
                    count = CInt(CallByName(occsObj, "Count", CallType.Get))
                Catch ex As Exception
                    RegistrarErro($"Lendo Count de {nomeComExt}", ex)
                    count = 0
                End Try
                If count <= 0 Then Exit Sub

                totalEstimado += count

                ' Agrupa por arquivo dentro do Nivel
                Dim grupo As New Dictionary(Of String, (Caminho As String, QtdeLocal As Integer, DocObj As Object))(StringComparer.OrdinalIgnoreCase)

                For i As Integer = 1 To count
                    Try
                        Dim occ As Object = CallByName(occsObj, "Item", CallType.Method, i)

                        Dim subPath As String = ""
                        Try
                            subPath = CStr(CallByName(occ, "OccurrenceFileName", CallType.Get))
                        Catch ex As Exception
                            RegistrarErro($"Lendo OccurrenceFileName (#{i}) de {nomeComExt}", ex)
                            subPath = ""
                        End Try

                        Dim subDoc As Object = Nothing
                        Try
                            subDoc = CallByName(occ, "OccurrenceDocument", CallType.Get)
                        Catch ex As Exception
                            RegistrarErro($"Lendo OccurrenceDocument (#{i}) de {nomeComExt}", ex)
                            subDoc = Nothing
                        End Try

                        If subDoc Is Nothing AndAlso Not String.IsNullOrWhiteSpace(subPath) AndAlso IO.File.Exists(subPath) Then
                            Try
                                subDoc = app.Documents.Open(subPath)
                            Catch ex As Exception
                                RegistrarErro($"Abrindo subdocumento '{subPath}'", ex)
                                subDoc = Nothing
                            End Try
                        End If

                        Dim nomeAgrup As String = If(String.IsNullOrWhiteSpace(subPath), $"ITEM_{i}", IO.Path.GetFileName(subPath)).ToUpperInvariant()

                        Dim qLocal As Integer = 1
                        Try
                            Dim qtdProp As Object = CallByName(occ, "Quantity", CallType.Get)
                            If qtdProp IsNot Nothing Then
                                Dim qi As Integer
                                If Integer.TryParse(qtdProp.ToString(), qi) AndAlso qi > 0 Then qLocal = qi
                            End If
                        Catch ex As Exception
                            RegistrarErro($"Lendo Quantity (#{i}) de {nomeComExt}", ex)
                        End Try

                        If grupo.ContainsKey(nomeAgrup) Then
                            Dim t = grupo(nomeAgrup)
                            grupo(nomeAgrup) = (t.Caminho, t.QtdeLocal + qLocal, If(t.DocObj Is Nothing, subDoc, t.DocObj))
                        Else
                            grupo.Add(nomeAgrup, (If(String.IsNullOrWhiteSpace(subPath), "", subPath), qLocal, subDoc))
                        End If
                    Catch ex As Exception
                        RegistrarErro($"Processando ocorrência (#{i}) de {nomeComExt}", ex)
                    End Try
                Next

                For Each kv In grupo

                    Dim caminhoSub = kv.Value.Caminho
                    Dim qLocal = kv.Value.QtdeLocal
                    Dim qEfetivaFilho As Integer = qLocal * multiplicadorPai
                    Dim subDoc = kv.Value.DocObj

                    ' Se ainda não temos caminho mas temos documento, tenta pegar o FullName
                    If String.IsNullOrWhiteSpace(caminhoSub) AndAlso subDoc IsNot Nothing Then
                        Try
                            caminhoSub = CStr(CallByName(subDoc, "FullName", CallType.Get))
                        Catch ex As Exception
                            RegistrarErro("Lendo FullName de subdocumento", ex)
                            caminhoSub = ""
                        End Try
                    End If

                    ' Nome e tipo do filho (mesmo sem caminho)
                    Dim nomeFilho As String
                    Dim tipoFilho As String

                    If Not String.IsNullOrWhiteSpace(caminhoSub) Then
                        nomeFilho = IO.Path.GetFileName(caminhoSub)
                        tipoFilho = IO.Path.GetExtension(caminhoSub).Replace(".", "").ToUpperInvariant()
                    Else
                        ' fallback: usa a chave do grupo como nome
                        nomeFilho = kv.Key
                        tipoFilho = ""
                    End If

                    If subDoc IsNot Nothing AndAlso Not String.IsNullOrWhiteSpace(caminhoSub) Then
                        ' ✅ Caso NORMAL: temos o documento
                        '    → NÃO adiciona aqui, deixa o Ler(...) do filho fazer o AdicionarOuSomarLinha
                        Ler(subDoc, caminhoSub, nivel + 1, qEfetivaFilho)

                        ' Fecha se abrimos temporariamente
                        Try
                            Dim isActive As Boolean = False
                            Try : isActive = (TryCast(subDoc, Object) Is app.ActiveDocument) : Catch : End Try
                            If Not isActive AndAlso IO.File.Exists(caminhoSub) Then
                                Try : CallByName(subDoc, "Close", CallType.Method, False) : Catch : End Try
                            End If
                        Catch ex As Exception
                            RegistrarErro($"Fechando subdocumento '{caminhoSub}'", ex)
                        End Try

                    Else
                        ' ⚠ Caso EXCEÇÃO: não há documento carregado,
                        '    mas precisamos que a peça apareça na lista.
                        '    → adiciona a linha AQUI, uma única vez.
                        AdicionarOuSomarLinha(
                            nivel + 1,
                            nomeFilho,
                            tipoFilho,
                            qEfetivaFilho,
                            caminhoSub,
                            subDoc
                        )
                    End If

                Next

            Catch ex As Exception
                RegistrarErro($"Ler('{caminhoArquivo}')", ex)
            End Try
        End Sub

            '============================================================
            ' 🔹 Chamada inicial da recursão (multiplicador raiz = 1)
            '============================================================
            Ler(doc, doc.FullName, 0, 1)

            '============================================================
            ' 🔹 Limpeza de linhas “fantasmas” (sem arquivo/caminho)
            '============================================================
            For i As Integer = tabela.Rows.Count - 1 To 0 Step -1
                Dim arq As String = Convert.ToString(tabela.Rows(i)("Arquivo"))
                Dim cam As String = Convert.ToString(tabela.Rows(i)("CaminhoCompleto"))

                If String.IsNullOrWhiteSpace(arq) AndAlso String.IsNullOrWhiteSpace(cam) Then
                    tabela.Rows.RemoveAt(i)
                End If
            Next

            '============================================================
            ' 🔹 Cálculo dos TOTAIS
            '============================================================
            Dim totalQtdSolicitada As Double = 0
            Dim arquivosUnicos As New HashSet(Of String)(StringComparer.OrdinalIgnoreCase)

            For Each row As DataRow In tabela.Rows
                ' 1) Soma da QtdeTotal
                Dim qtd As Double
                Double.TryParse(
                Convert.ToString(row("QtdeTotal")).Replace(",", "."),
                NumberStyles.Any,
                CultureInfo.InvariantCulture,
                qtd
            )
                totalQtdSolicitada += qtd

                ' 2) Arquivo distinto
                Dim arq As String = Convert.ToString(row("Arquivo"))
                If Not String.IsNullOrWhiteSpace(arq) Then
                    arquivosUnicos.Add(arq)
                End If
            Next

            Dim totalPecasDiferentes As Integer = arquivosUnicos.Count
            Dim totalPecas As Integer = tabela.Rows.Count

            '============================================================
            ' 🔹 Bind no grid
            '============================================================
            dgv.DataSource = tabela
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells
            dgv.AllowUserToAddRows = False
            dgv.ColumnHeadersDefaultCellStyle.Font = New Font(dgv.Font, FontStyle.Bold)

            If dgv.Columns.Contains("Arquivo") Then dgv.Columns("Arquivo").Frozen = True

            tabelaOriginalBOM = tabela.Copy()

            Dim ocultar() As String = {
    "TipoArquivo", "CaminhoCompleto", "Categoria", "Gerente",
    "Autor", "Empresa", "Data", "DataR", "Data1",
    "Revision", "CutSizeX", "CutSizeY", "Mass",
    "Área_de_superfície", "Area_de_superficie", "Bloqueado",
    "RNC", "PesoTotal", "AreaPinturaTotal",
    "TipoLinha", "CodMatFabricante", "DescDetal", "QtdeMaterial", "PesoMaterial"
}


            For Each c In ocultar
                If dgv.Columns.Contains(c) Then dgv.Columns(c).Visible = False
            Next

            '============================================================
            ' 🔹 UI - final (incluindo os totais)
            '============================================================
            pgb.Style = ProgressBarStyle.Continuous
            pgb.Value = 100

            lblResumo.Text =
        "✅ Estrutura concluída." & vbCrLf &
        $"• Total de peças solicitadas (QtdeTotal): {totalQtdSolicitada}" & vbCrLf &
        $"• Total de peças diferentes (arquivos únicos): {totalPecasDiferentes}" & vbCrLf &
        $"• Total de peças na estrutura (linhas): {totalPecas}"

            '' System.Windows.Forms.Application.DoEvents()
            Threading.Thread.Sleep(550)

        Catch ex As Exception
            ' Erro geral do processo
            RegistrarErro("Erro geral em ListarEstruturaComTodasPropriedadesBlank", ex)

        Finally
            pgb.Visible = False

            ' Se houve erros, mostra relatório organizado
            If erros IsNot Nothing AndAlso erros.Count > 0 Then
                Dim sb As New StringBuilder()
                sb.AppendLine("Foram encontrados " & erros.Count & " erro(s) durante a leitura da estrutura:")
                sb.AppendLine()

                Dim i As Integer = 1
                For Each msg In erros
                    sb.AppendLine(i.ToString("00") & " - " & msg)
                    i += 1
                Next

                If qtdRpcDesconectado > 0 Then
                    sb.AppendLine()
                    sb.AppendLine(qtdRpcDesconectado.ToString() &
                          " erro(s) são do tipo RPC_E_DISCONNECTED (necessário revisar/ajustar o arquivo no Solid Edge).")
                End If

                MessageBox.Show(sb.ToString(),
                        "Relatório de erros - Estrutura",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning)

            End If

        End Try

    End Sub

#End Region


#Region "VERSÃO ANTERIOR - 26-11-2025 rev 00"
    ' somando os totais por nivel
    Public Sub ListarEstruturaBom(ByVal dgv As DataGridView,
                              ByVal pgb As ProgressBar,
                              ByVal lbl As Label,
                              ByVal lblResumo As Label)

        ' Lista que acumula erros
        Dim erros As New List(Of String)

        ' Contador de documentos que deram RPC_E_DISCONNECTED
        Dim qtdRpcDesconectado As Integer = 0

        ' Helper para registrar erros com contexto
        Dim RegistrarErro As Action(Of String, Exception) =
        Sub(contexto As String, ex As Exception)
            Try
                If ex Is Nothing Then Return

                Dim msg As String

                ' Detecta erro COM de objeto desconectado (RPC_E_DISCONNECTED)
                Dim comEx = TryCast(ex, System.Runtime.InteropServices.COMException)
                If comEx IsNot Nothing AndAlso comEx.ErrorCode = &H80010108 Then
                    qtdRpcDesconectado += 1
                    msg = $"{contexto}: [RPC_E_DISCONNECTED] O objeto do Solid Edge foi desconectado. Detalhe: {ex.Message}"
                Else
                    msg = $"{contexto}: {ex.Message}"
                End If

                erros.Add(msg)
            Catch
                ' não deixa o log quebrar o fluxo
            End Try
        End Sub

        Try
            '============================================================
            ' 🔹 Sessão do Solid Edge e documento ativo
            '============================================================
            If app Is Nothing OrElse app.Documents.Count = 0 Then
                MessageBox.Show("Nenhum documento aberto no Solid Edge.",
                            "SINCO - Solid Edge",
                            MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Exit Sub
            End If

            Dim doc As Object = app.ActiveDocument
            If doc Is Nothing Then Exit Sub

            '============================================================
            ' 🔹 UI - estado inicial
            '============================================================
            pgb.Visible = True : pgb.Style = ProgressBarStyle.Marquee : pgb.Value = 0
            lbl.Visible = True : lbl.Text = "Lendo estrutura... aguarde."

            '============================================================
            ' 🔹 Tabela destino
            '============================================================
            Dim tabela As New DataTable("BOM_Completa")

            ' Colunas fixas
            tabela.Columns.Add("Nivel", GetType(Integer))
            tabela.Columns.Add("Arquivo", GetType(String))          ' com extensão
            tabela.Columns.Add("TipoArquivo", GetType(String))      ' PAR/PSM/ASM...
            tabela.Columns.Add("Qtde", GetType(Integer))            ' quantidade efetiva no Nivel
            tabela.Columns.Add("CaminhoCompleto", GetType(String))

            ' Propriedades de interesse como colunas próprias
            Dim propsInteressantes As New List(Of String) From {
            "Título", "Assunto", "Coment", "Tipo de Desenho", "Palavras-", "Autor", "Empresa",
            "Categoria", "Gerente", "Material", "Data", "DataR", "Data1",
            "Revision", "CutSizeX", "CutSizeY", "Thickness",
            "Mass", "Área_de_superfície", "Area_de_superficie", "Bloqueado"
        }
            For Each p In propsInteressantes
                If Not tabela.Columns.Contains(p) Then tabela.Columns.Add(p, GetType(String))
            Next

            ' Colunas de totais
            tabela.Columns.Add("RNC", GetType(String))
            tabela.Columns.Add("Fator", GetType(Double))
            tabela.Columns.Add("QtdeTotal", GetType(Double))
            tabela.Columns.Add("PesoTotal", GetType(Double))
            tabela.Columns.Add("AreaPinturaTotal", GetType(Double))

            Dim totalLidos As Integer = 0
            Dim totalEstimado As Integer = 1

            '============================================================
            ' 🔹 Helpers
            '============================================================
            Dim LimparUnidades As Func(Of String, String) =
            Function(txt As String) As String
                If String.IsNullOrWhiteSpace(txt) Then Return ""
                Return txt _
                    .Replace("mm²", "") _
                    .Replace("MM²", "") _
                    .Replace("mm", "") _
                    .Replace("MM", "") _
                    .Replace("^2", "") _
                    .Replace("KG", "") _
                    .Replace("kg", "") _
                    .Trim()
            End Function

            Dim FormatarValor As Func(Of String, String, String) =
            Function(propNome As String, valorBruto As String) As String
                If String.IsNullOrWhiteSpace(valorBruto) Then Return ""
                Dim nome = propNome.Trim().ToUpperInvariant()
                Dim valor = LimparUnidades(valorBruto)

                ' Datas
                If nome = "DATA" OrElse nome = "DATAR" OrElse nome = "DATA1" Then
                    Try : Return cl_BancoDados.FormatarData(valor) : Catch : Return valor : End Try
                End If

                ' Revisão
                If nome.Contains("REVISION") Then Return valor.ToUpperInvariant()

                ' CutSize / Espessura / Massa -> ponto decimal
                If nome.Contains("CUTSIZEX") Then Return valor.Replace(",", ".")
                If nome.Contains("CUTSIZEY") Then Return valor.Replace(",", ".")
                If nome.Contains("THICKNESS") Then Return valor.Replace(",", ".")
                If nome.Contains("MASS") Then Return valor.Replace(",", ".")

                ' Área mm² -> m²
                If nome.Contains("ÁREA") OrElse nome.Contains("AREA") Then
                    Dim num As Double
                    If Double.TryParse(valor.Replace(",", "."), NumberStyles.Any,
                                       CultureInfo.InvariantCulture, num) Then
                        Dim m2 = num / 1_000_000.0
                        Return m2.ToString("N4", CultureInfo.GetCultureInfo("pt-BR"))
                    End If
                End If

                Return valor
            End Function

            ' ============================================================
            ' 🔹 PreencherPropriedades com retry + fechar docs abertos aqui
            ' ============================================================
            Dim PreencherPropriedades As Action(Of Object, DataRow) =
            Sub(docObj As Object, linha As DataRow)
                If linha Is Nothing Then Exit Sub

                Dim tentativas As Integer = 0
                Dim abriuTemp As Boolean = False   ' controla se foi aberto aqui

                While tentativas < 2
                    Try
                        ' Se o objeto estiver Nothing (após erro), tenta reabrir pelo caminho da linha
                        If docObj Is Nothing Then
                            Dim caminhoArquivo As String = ""
                            Try
                                caminhoArquivo = Convert.ToString(linha("CaminhoCompleto"))
                            Catch
                                caminhoArquivo = ""
                            End Try

                            If Not String.IsNullOrWhiteSpace(caminhoArquivo) AndAlso IO.File.Exists(caminhoArquivo) Then
                                Try
                                    docObj = app.Documents.Open(caminhoArquivo)
                                    abriuTemp = True
                                Catch exOpen As Exception
                                    RegistrarErro($"Reabrindo documento '{caminhoArquivo}'", exOpen)
                                    Exit While
                                End Try
                            Else
                                Exit While
                            End If
                        End If

                        ' --- Leitura das propriedades ---
                        Dim propSets As Object = docObj.Properties
                        For Each propSet As Object In propSets
                            For Each prop As Object In propSet
                                If prop Is Nothing Then Continue For

                                Dim nomeProp As String = ""
                                Try : nomeProp = CStr(prop.Name) : Catch : Continue For : End Try
                                If String.IsNullOrWhiteSpace(nomeProp) Then Continue For

                                ' 1) Preenche colunas de interesse
                                For Each alvo In propsInteressantes
                                    If nomeProp.IndexOf(alvo, StringComparison.OrdinalIgnoreCase) >= 0 Then
                                        Dim valorBruto As String = ""
                                        Try
                                            Dim v = prop.Value
                                            If v IsNot Nothing Then valorBruto = v.ToString()
                                        Catch ex As Exception
                                            valorBruto = "(erro)"
                                            RegistrarErro($"Lendo propriedade '{nomeProp}'", ex)
                                        End Try
                                        linha(alvo) = FormatarValor(nomeProp, valorBruto)
                                        Exit For
                                    End If
                                Next

                                ' 2) Mapa de espessura -> Thickness
                                If nomeProp.IndexOf("Material Thickness", StringComparison.OrdinalIgnoreCase) >= 0 _
                                   OrElse nomeProp.IndexOf("Espessura Do Material", StringComparison.OrdinalIgnoreCase) >= 0 _
                                   OrElse nomeProp.Equals("Thickness", StringComparison.OrdinalIgnoreCase) Then

                                    Dim valorBruto As String = ""
                                    Try
                                        Dim v = prop.Value
                                        If v IsNot Nothing Then valorBruto = v.ToString()
                                    Catch ex As Exception
                                        RegistrarErro($"Lendo espessura em '{nomeProp}'", ex)
                                    End Try

                                    If Not String.IsNullOrWhiteSpace(valorBruto) Then
                                        Dim limpo = LimparUnidades(valorBruto).Replace(",", ".")
                                        linha("Thickness") = limpo
                                    End If
                                End If

                            Next
                        Next

                        Exit While ' sucesso

                    Catch comEx As System.Runtime.InteropServices.COMException
                        If comEx.ErrorCode = &H80010108 Then
                            ' RPC_E_DISCONNECTED → tenta reabrir
                            tentativas += 1
                            RegistrarErro("PreencherPropriedades - RPC_E_DISCONNECTED, tentando reabrir documento", comEx)
                            docObj = Nothing
                            Threading.Thread.Sleep(200)
                            Continue While
                        Else
                            RegistrarErro("PreencherPropriedades", comEx)
                            Exit While
                        End If

                    Catch ex As Exception
                        RegistrarErro("PreencherPropriedades", ex)
                        Exit While
                    End Try
                End While

                ' Se abrimos o documento aqui, tentamos fechar
                If abriuTemp AndAlso docObj IsNot Nothing Then
                    Try
                        Dim isActive As Boolean = False
                        Try
                            isActive = (TryCast(docObj, Object) Is app.ActiveDocument)
                        Catch
                            isActive = False
                        End Try

                        If Not isActive Then
                            Try
                                CallByName(docObj, "Close", CallType.Method, False)
                            Catch ex As Exception
                                RegistrarErro("Fechando documento aberto em PreencherPropriedades", ex)
                            End Try
                        End If
                    Catch
                        ' não deixa o fechamento quebrar o fluxo
                    End Try
                End If
            End Sub
            ' ============================================================

            ' ============================================================
            ' 🔹 AdicionarOuSomarLinha SEM agrupamento global
            '     → respeita a árvore; agrupamento só por pai/nivel via "grupo"
            ' ============================================================
            Dim AdicionarOuSomarLinha As Action(Of Integer, String, String, Integer, String, Object) =
            Sub(nivel As Integer,
                nomeComExt As String,
                tipoArquivo As String,
                qtdeEfetiva As Integer,
                caminho As String,
                docObj As Object)

                Try
                    Dim linha = tabela.NewRow()

                    linha("Nivel") = nivel
                    linha("Arquivo") = nomeComExt
                    linha("TipoArquivo") = tipoArquivo
                    linha("Qtde") = qtdeEfetiva
                    linha("CaminhoCompleto") = caminho

                    For Each p In propsInteressantes
                        linha(p) = ""
                    Next

                    linha("RNC") = ""
                    linha("Fator") = 1.0
                    linha("QtdeTotal") = CDbl(qtdeEfetiva)
                    linha("PesoTotal") = 0.0
                    linha("AreaPinturaTotal") = 0.0

                    ' Preenche propriedades do arquivo
                    PreencherPropriedades(docObj, linha)

                    Dim mass As Double = 0
                    Double.TryParse(
                        Convert.ToString(linha("Mass")).Replace(",", "."),
                        NumberStyles.Any,
                        CultureInfo.InvariantCulture,
                        mass
                    )

                    Dim area As Double = 0
                    Double.TryParse(
                        Convert.ToString(linha("Área_de_superfície")).Replace(",", "."),
                        NumberStyles.Any,
                        CultureInfo.InvariantCulture,
                        area
                    )

                    linha("PesoTotal") = mass * qtdeEfetiva
                    linha("AreaPinturaTotal") = area * qtdeEfetiva

                    ' ✅ Sem agrupamento global → cada chamada gera uma linha
                    tabela.Rows.Add(linha)


                    ' ====================================================
                    ' 🔹 NOVO: para cada Arquivo, carrega materiais
                    '     – sem mexer na lógica da estrutura
                    ' ====================================================
                    Try
                        ' Prepara o "contexto" para LerDadosViewMontaPeca
                        DadosArquivoCorrente.NomeArquivoSemExtensao = nomeComExt
                        '     Path.GetFileNameWithoutExtension(nomeComExt)

                        DadosArquivoCorrente.qtde = qtdeEfetiva

                        ' Chama a função que preenche o grid de materiais
                        LerDadosViewMontaPeca(dgv)

                    Catch exMat As Exception
                        RegistrarErro($"LerDadosViewMontaPeca() para '{nomeComExt}'", exMat)
                    End Try
                    ' ====================================================





                Catch ex As Exception
                    RegistrarErro($"AdicionarOuSomarLinha [{nomeComExt}]", ex)
                End Try
            End Sub

            '============================================================
            ' 🔹 Recursão (propaga multiplicador do pai)
            '============================================================
            Dim Ler As Action(Of Object, String, Integer, Integer) =
            Sub(docObj As Object, caminhoArquivo As String, nivel As Integer, multiplicadorPai As Integer)
                Try
                    If docObj Is Nothing OrElse String.IsNullOrWhiteSpace(caminhoArquivo) Then Exit Sub

                    totalLidos += 1
                    lbl.Text = $"Lendo {IO.Path.GetFileName(caminhoArquivo)} ({totalLidos})"
                    ' System.Windows.Forms.Application.DoEvents()

                    If totalEstimado > 1 Then
                        pgb.Style = ProgressBarStyle.Continuous
                        pgb.Value = Math.Min(100, CInt((totalLidos / Math.Max(1, totalEstimado)) * 100))
                    End If

                    Dim ext As String = IO.Path.GetExtension(caminhoArquivo).ToLower()
                    Dim nomeComExt As String = IO.Path.GetFileName(caminhoArquivo)
                    Dim tipoUpper As String = ext.Replace(".", "").ToUpperInvariant()

                    ' Adiciona/Acumula a linha do próprio documento
                    AdicionarOuSomarLinha(nivel, nomeComExt, tipoUpper, multiplicadorPai, caminhoArquivo, docObj)

                    ' Apenas ASM tem filhos
                    If ext <> ".asm" Then Exit Sub

                    ' Ocorrências
                    Dim occsObj As Object = Nothing
                    Try
                        occsObj = CallByName(docObj, "Occurrences", CallType.Get)
                    Catch ex As Exception
                        RegistrarErro($"Lendo Occurrences de {nomeComExt}", ex)
                        occsObj = Nothing
                    End Try
                    If occsObj Is Nothing Then Exit Sub

                    Dim count As Integer = 0
                    Try
                        count = CInt(CallByName(occsObj, "Count", CallType.Get))
                    Catch ex As Exception
                        RegistrarErro($"Lendo Count de {nomeComExt}", ex)
                        count = 0
                    End Try
                    If count <= 0 Then Exit Sub

                    totalEstimado += count

                    ' Agrupa por arquivo dentro do mesmo pai
                    Dim grupo As New Dictionary(Of String, (Caminho As String, QtdeLocal As Integer, DocObj As Object))(StringComparer.OrdinalIgnoreCase)

                    For i As Integer = 1 To count
                        Try
                            Dim occ As Object = CallByName(occsObj, "Item", CallType.Method, i)

                            Dim subPath As String = ""
                            Try
                                subPath = CStr(CallByName(occ, "OccurrenceFileName", CallType.Get))
                            Catch ex As Exception
                                RegistrarErro($"Lendo OccurrenceFileName (#{i}) de {nomeComExt}", ex)
                                subPath = ""
                            End Try

                            Dim subDoc As Object = Nothing
                            Try
                                subDoc = CallByName(occ, "OccurrenceDocument", CallType.Get)
                            Catch ex As Exception
                                RegistrarErro($"Lendo OccurrenceDocument (#{i}) de {nomeComExt}", ex)
                                subDoc = Nothing
                            End Try

                            If subDoc Is Nothing AndAlso Not String.IsNullOrWhiteSpace(subPath) AndAlso IO.File.Exists(subPath) Then
                                Try
                                    subDoc = app.Documents.Open(subPath)
                                Catch ex As Exception
                                    RegistrarErro($"Abrindo subdocumento '{subPath}'", ex)
                                    subDoc = Nothing
                                End Try
                            End If

                            Dim nomeAgrup As String = If(String.IsNullOrWhiteSpace(subPath), $"ITEM_{i}", IO.Path.GetFileName(subPath)).ToUpperInvariant()

                            Dim qLocal As Integer = 1
                            Try
                                Dim qtdProp As Object = CallByName(occ, "Quantity", CallType.Get)
                                If qtdProp IsNot Nothing Then
                                    Dim qi As Integer
                                    If Integer.TryParse(qtdProp.ToString(), qi) AndAlso qi > 0 Then qLocal = qi
                                End If
                            Catch ex As Exception
                                RegistrarErro($"Lendo Quantity (#{i}) de {nomeComExt}", ex)
                            End Try

                            If grupo.ContainsKey(nomeAgrup) Then
                                Dim t = grupo(nomeAgrup)
                                grupo(nomeAgrup) = (t.Caminho, t.QtdeLocal + qLocal, If(t.DocObj Is Nothing, subDoc, t.DocObj))
                            Else
                                grupo.Add(nomeAgrup, (If(String.IsNullOrWhiteSpace(subPath), "", subPath), qLocal, subDoc))
                            End If
                        Catch ex As Exception
                            RegistrarErro($"Processando ocorrência (#{i}) de {nomeComExt}", ex)
                        End Try
                    Next

                    For Each kv In grupo

                        Dim caminhoSub = kv.Value.Caminho
                        Dim qLocal = kv.Value.QtdeLocal
                        Dim qEfetivaFilho As Integer = qLocal * multiplicadorPai
                        Dim subDoc = kv.Value.DocObj

                        ' Se ainda não temos caminho mas temos documento, tenta pegar o FullName
                        If String.IsNullOrWhiteSpace(caminhoSub) AndAlso subDoc IsNot Nothing Then
                            Try
                                caminhoSub = CStr(CallByName(subDoc, "FullName", CallType.Get))
                            Catch ex As Exception
                                RegistrarErro("Lendo FullName de subdocumento", ex)
                                caminhoSub = ""
                            End Try
                        End If

                        ' Nome e tipo do filho (mesmo sem caminho)
                        Dim nomeFilho As String
                        Dim tipoFilho As String

                        If Not String.IsNullOrWhiteSpace(caminhoSub) Then
                            nomeFilho = IO.Path.GetFileName(caminhoSub)
                            tipoFilho = IO.Path.GetExtension(caminhoSub).Replace(".", "").ToUpperInvariant()
                        Else
                            ' fallback: usa a chave do grupo como nome
                            nomeFilho = kv.Key
                            tipoFilho = ""
                        End If

                        If subDoc IsNot Nothing AndAlso Not String.IsNullOrWhiteSpace(caminhoSub) Then
                            ' Caso normal: temos o documento → Ler cuida de adicionar
                            Ler(subDoc, caminhoSub, nivel + 1, qEfetivaFilho)

                            ' Fecha se abrimos temporariamente
                            Try
                                Dim isActive As Boolean = False
                                Try : isActive = (TryCast(subDoc, Object) Is app.ActiveDocument) : Catch : End Try
                                If Not isActive AndAlso IO.File.Exists(caminhoSub) Then
                                    Try : CallByName(subDoc, "Close", CallType.Method, False) : Catch : End Try
                                End If
                            Catch ex As Exception
                                RegistrarErro($"Fechando subdocumento '{caminhoSub}'", ex)
                            End Try

                        Else
                            ' Caso exceção: não há documento carregado, mas queremos listar mesmo assim
                            AdicionarOuSomarLinha(
                                nivel + 1,
                                nomeFilho,
                                tipoFilho,
                                qEfetivaFilho,
                                caminhoSub,
                                subDoc
                            )
                        End If

                    Next

                Catch ex As Exception
                    RegistrarErro($"Ler('{caminhoArquivo}')", ex)
                End Try
            End Sub

            '============================================================
            ' 🔹 Chamada inicial da recursão (multiplicador raiz = 1)
            '============================================================
            Ler(doc, doc.FullName, 0, 1)

            '============================================================
            ' 🔹 Limpeza de linhas “fantasmas” (sem arquivo/caminho)
            '============================================================
            For i As Integer = tabela.Rows.Count - 1 To 0 Step -1
                Dim arq As String = Convert.ToString(tabela.Rows(i)("Arquivo"))
                ' Dim cam As String = Convert.ToString(tabela.Rows(i)("CaminhoCompleto"))

                If String.IsNullOrWhiteSpace(arq) Then 'AndAlso String.IsNullOrWhiteSpace(cam) Then
                    tabela.Rows.RemoveAt(i)
                End If
            Next

            '============================================================
            ' 🔹 Cálculo dos TOTAIS
            '============================================================
            Dim totalQtdSolicitada As Double = 0
            Dim arquivosUnicos As New HashSet(Of String)(StringComparer.OrdinalIgnoreCase)

            For Each row As DataRow In tabela.Rows
                ' 1) Soma da QtdeTotal
                Dim qtd As Double
                Double.TryParse(
                Convert.ToString(row("QtdeTotal")).Replace(",", "."),
                NumberStyles.Any,
                CultureInfo.InvariantCulture,
                qtd
            )
                totalQtdSolicitada += qtd

                ' 2) Arquivo distinto
                Dim arq As String = Convert.ToString(row("Arquivo"))
                If Not String.IsNullOrWhiteSpace(arq) Then
                    arquivosUnicos.Add(arq)
                End If
            Next

            Dim totalPecasDiferentes As Integer = arquivosUnicos.Count
            Dim totalPecas As Integer = tabela.Rows.Count

            '============================================================
            ' 🔹 Bind no grid
            '============================================================
            dgv.DataSource = tabela
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells
            dgv.AllowUserToAddRows = False
            dgv.ColumnHeadersDefaultCellStyle.Font = New Font(dgv.Font, FontStyle.Bold)

            If dgv.Columns.Contains("Arquivo") Then dgv.Columns("Arquivo").Frozen = True

            tabelaOriginalBOM = tabela.Copy()

            Dim ocultar() As String = {
            "TipoArquivo", "CaminhoCompleto", "Categoria", "Gerente",
            "Autor", "Empresa", "Data", "DataR", "Data1",
            "Revision", "CutSizeX", "CutSizeY", "Mass",
            "Área_de_superfície", "Area_de_superficie", "Bloqueado",
            "RNC", "PesoTotal", "AreaPinturaTotal"
        }

            For Each c In ocultar
                If dgv.Columns.Contains(c) Then dgv.Columns(c).Visible = False
            Next

            '============================================================
            ' 🔹 UI - final (incluindo os totais)
            '============================================================
            pgb.Style = ProgressBarStyle.Continuous
            pgb.Value = 100

            lblResumo.Text =
            "✅ Estrutura concluída." & vbCrLf &
            $"• Total de peças solicitadas (QtdeTotal): {totalQtdSolicitada}" & vbCrLf &
            $"• Total de peças diferentes (arquivos únicos): {totalPecasDiferentes}" & vbCrLf &
            $"• Total de peças na estrutura (linhas): {totalPecas}"

            ' System.Windows.Forms.Application.DoEvents()
            Threading.Thread.Sleep(550)

        Catch ex As Exception
            ' Erro geral do processo
            RegistrarErro("Erro geral em ListarEstruturaBom", ex)

        Finally
            pgb.Visible = False

            ' Se houve erros, mostra relatório organizado
            If erros IsNot Nothing AndAlso erros.Count > 0 Then
                Dim sb As New StringBuilder()
                sb.AppendLine("Foram encontrados " & erros.Count & " erro(s) durante a leitura da estrutura:")
                sb.AppendLine()

                Dim i As Integer = 1
                For Each msg In erros
                    sb.AppendLine(i.ToString("00") & " - " & msg)
                    i += 1
                Next

                If qtdRpcDesconectado > 0 Then
                    sb.AppendLine()
                    sb.AppendLine(qtdRpcDesconectado.ToString() &
                              " erro(s) são do tipo RPC_E_DISCONNECTED (necessário revisar/ajustar o arquivo no Solid Edge).")
                End If

                MessageBox.Show(sb.ToString(),
                            "Relatório de erros - Estrutura",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning)
            End If

        End Try

    End Sub



    Public Function LerDadosViewMontaPeca(dgv As DataGridView)

        Dim dv As New System.Data.DataView(TabelaViewMontaPeca)
        ' dv.RowFilter = "NomeArquivoSemExtensao = '" & DadosArquivoCorrente.NomeArquivoSemExtensao & "' AND D_E_L_E_T_E <> '*' OR D_E_L_E_T_E IS NULL"
        dv.RowFilter = "NomeArquivoSemExtensao = '" & DadosArquivoCorrente.NomeArquivoSemExtensao & "'"

        Dim dtFiltrado As System.Data.DataTable = dv.ToTable()



        For Each linha As DataRow In dtFiltrado.Rows

            Try
                If String.Equals(
        Trim(linha("NomeArquivoSemExtensao").ToString),
        DadosArquivoCorrente.NomeArquivoSemExtensao,
        StringComparison.OrdinalIgnoreCase) Then
                    iconeTipoArquivo = My.Resources.sinco_Diversos  'My.Resources.material_escolar_32
                    Dim peso As Double
                    Dim PecaQtde As Double
                    ' Try
                    ' Obtém o valor da célula "PecaQtde" e remove espaços em branco
                    Dim pecaQtdeStr As String

                    Try

                        pecaQtdeStr = linha.Item("PecaQtde").ToString().Trim().Replace(",", ".")

                    Catch ex As Exception
                        pecaQtdeStr = 0
                    End Try

                    PecaQtde = cl_BancoDados.converteStringParaDouble(pecaQtdeStr)



                    Dim pecaPeso As String

                    Try
                        pecaPeso = linha.Item("Peso").ToString().Trim().Replace(",", ".")

                    Catch ex As Exception

                        pecaPeso = 0

                    End Try

                    peso = cl_BancoDados.converteStringParaDouble(pecaPeso)
                    ' MsgBox(peso)


                    Dim PesoMaterial As Double

                    Try
                        PesoMaterial = CDbl(peso) * CDbl(DadosArquivoCorrente.qtde)
                    Catch ex As Exception
                        PesoMaterial = 0
                    End Try

                    Dim QtdeMaterial As Double

                    Try
                        QtdeMaterial = CDbl(PecaQtde) * CDbl(DadosArquivoCorrente.qtde)
                    Catch ex As Exception
                        QtdeMaterial = 0
                    End Try

                    ' Preencher o DataGridView com os dados da peça
                    dgv.Rows.Add("",
                                                Trim(linha.Item("CodMatFabricante").ToString.ToUpper),
                                                QtdeMaterial.ToString,
                                                Trim(linha.Item("DescDetal").ToString.ToUpper),'Autor,
                                                "",'assunto
                                                "",'coment
                                                 "MATERIAL")



                End If
            Catch ex As Exception
                Continue For
            Finally
            End Try

        Next

        'removento o tratamento de erro 04/04/2025
        'Catch ex As Exception
        'Finally
        'End Try

    End Function


#End Region

    ' somando os totais por nivel

#Region "VERSÃO NOVA - 27-11-2025 rev 02"

    '    '================================================================================
    '    ' Monta estrutura BOM + MATERIAIS no MESMO DataGridView
    '    '================================================================================
    '    Public Sub ListarEstruturaBomMateriais(ByVal dgv As DataGridView,
    '                                       ByVal pgb As ProgressBar,
    '                                       ByVal lbl As Label,
    '                                       ByVal lblResumo As Label)

    '        ' Lista que acumula erros
    '        Dim erros As New List(Of String)
    '        Dim qtdRpcDesconectado As Integer = 0

    '        ' Helper para registrar erros com contexto
    '        Dim RegistrarErro As Action(Of String, Exception) =
    '        Sub(contexto As String, ex As Exception)
    '            Try
    '                If ex Is Nothing Then Return

    '                Dim msg As String
    '                Dim comEx = TryCast(ex, System.Runtime.InteropServices.COMException)

    '                If comEx IsNot Nothing AndAlso comEx.ErrorCode = &H80010108 Then
    '                    qtdRpcDesconectado += 1
    '                    msg = $"{contexto}: [RPC_E_DISCONNECTED] O objeto do Solid Edge foi desconectado. Detalhe: {ex.Message}"
    '                Else
    '                    msg = $"{contexto}: {ex.Message}"
    '                End If

    '                erros.Add(msg)
    '            Catch
    '                ' não deixa o log quebrar o fluxo
    '            End Try
    '        End Sub

    '        Try
    '            '============================================================
    '            ' 🔹 Sessão do Solid Edge e documento ativo
    '            '============================================================
    '            If app Is Nothing OrElse app.Documents.Count = 0 Then
    '                MessageBox.Show("Nenhum documento aberto no Solid Edge.",
    '                            "SINCO - Solid Edge",
    '                            MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
    '                Exit Sub
    '            End If

    '            Dim doc As Object = app.ActiveDocument
    '            If doc Is Nothing Then Exit Sub

    '            '============================================================
    '            ' 🔹 UI - estado inicial
    '            '============================================================
    '            pgb.Visible = True
    '            pgb.Style = ProgressBarStyle.Marquee
    '            pgb.Value = 0
    '            lbl.Visible = True
    '            lbl.Text = "Lendo estrutura... aguarde."

    '            '============================================================
    '            ' 🔹 Tabela destino
    '            '============================================================
    '            Dim tabela As New DataTable("BOM_Completa")

    '            ' Colunas fixas
    '            tabela.Columns.Add("Nivel", GetType(Integer))
    '            tabela.Columns.Add("Arquivo", GetType(String))          ' com extensão
    '            tabela.Columns.Add("TipoArquivo", GetType(String))      ' PAR/PSM/ASM...
    '            tabela.Columns.Add("Qtde", GetType(Double))               ' quantidade efetiva no Nivel
    '            tabela.Columns.Add("CaminhoCompleto", GetType(String))

    '            ' Tipo de linha (PECA / MATERIAL)
    '            tabela.Columns.Add("TipoLinha", GetType(String))

    '            ' Propriedades de interesse
    '            Dim propsInteressantes As New List(Of String) From {
    '            "Título", "Assunto", "Coment", "Tipo de Desenho", "Palavras-", "Autor", "Empresa",
    '            "Categoria", "Gerente", "Material", "Data", "DataR", "Data1",
    '            "Revision", "CutSizeX", "CutSizeY", "Thickness",
    '            "Mass", "Área_de_superfície", "Area_de_superficie", "Bloqueado"
    '        }
    '            For Each p In propsInteressantes
    '                If Not tabela.Columns.Contains(p) Then
    '                    tabela.Columns.Add(p, GetType(String))
    '                End If
    '            Next

    '            ' Colunas de totais de peça
    '            tabela.Columns.Add("RNC", GetType(String))
    '            tabela.Columns.Add("Fator", GetType(Double))
    '            tabela.Columns.Add("QtdeTotal", GetType(Double))
    '            tabela.Columns.Add("PesoTotal", GetType(Double))
    '            tabela.Columns.Add("AreaPinturaTotal", GetType(Double))

    '            ' Colunas específicas para MATERIAL
    '            tabela.Columns.Add("CodMatFabricante", GetType(String))
    '            tabela.Columns.Add("DescDetal", GetType(String))
    '            tabela.Columns.Add("QtdeMaterial", GetType(Double))
    '            tabela.Columns.Add("PesoMaterial", GetType(Double))

    '            Dim totalLidos As Integer = 0
    '            Dim totalEstimado As Integer = 1

    '            '============================================================
    '            ' 🔹 Helpers
    '            '============================================================
    '            Dim LimparUnidades As Func(Of String, String) =
    '            Function(txt As String) As String
    '                If String.IsNullOrWhiteSpace(txt) Then Return ""
    '                Return txt _
    '                    .Replace("mm²", "") _
    '                    .Replace("MM²", "") _
    '                    .Replace("mm", "") _
    '                    .Replace("MM", "") _
    '                    .Replace("^2", "") _
    '                    .Replace("KG", "") _
    '                    .Replace("kg", "") _
    '                    .Trim()
    '            End Function

    '            Dim FormatarValor As Func(Of String, String, String) =
    '            Function(propNome As String, valorBruto As String) As String
    '                If String.IsNullOrWhiteSpace(valorBruto) Then Return ""
    '                Dim nome = propNome.Trim().ToUpperInvariant()
    '                Dim valor = LimparUnidades(valorBruto)

    '                ' Datas
    '                If nome = "DATA" OrElse nome = "DATAR" OrElse nome = "DATA1" Then
    '                    Try
    '                        Return cl_BancoDados.FormatarData(valor)
    '                    Catch
    '                        Return valor
    '                    End Try
    '                End If

    '                ' Revisão
    '                If nome.Contains("REVISION") Then Return valor.ToUpperInvariant()

    '                ' CutSize / Espessura / Massa -> ponto decimal
    '                If nome.Contains("CUTSIZEX") Then Return valor.Replace(",", ".")
    '                If nome.Contains("CUTSIZEY") Then Return valor.Replace(",", ".")
    '                If nome.Contains("THICKNESS") Then Return valor.Replace(",", ".")
    '                If nome.Contains("MASS") Then Return valor.Replace(",", ".")

    '                ' Área mm² -> m²
    '                If nome.Contains("ÁREA") OrElse nome.Contains("AREA") Then
    '                    Dim num As Double
    '                    If Double.TryParse(valor.Replace(",", "."),
    '                                       NumberStyles.Any,
    '                                       CultureInfo.InvariantCulture,
    '                                       num) Then
    '                        Dim m2 = num / 1_000_000.0
    '                        Return m2.ToString("N4", CultureInfo.GetCultureInfo("pt-BR"))
    '                    End If
    '                End If

    '                Return valor
    '            End Function

    '            '------------------------------------------------------------
    '            ' PreencherPropriedades
    '            '------------------------------------------------------------
    '            Dim PreencherPropriedades As Action(Of Object, DataRow) =
    '            Sub(docObj As Object, linha As DataRow)
    '                If linha Is Nothing Then Exit Sub

    '                Dim tentativas As Integer = 0
    '                Dim abriuTemp As Boolean = False

    '                While tentativas < 2
    '                    Try
    '                        ' Reabrir se necessário
    '                        If docObj Is Nothing Then
    '                            Dim caminhoArquivo As String = ""
    '                            Try
    '                                caminhoArquivo = Convert.ToString(linha("CaminhoCompleto"))
    '                            Catch
    '                                caminhoArquivo = ""
    '                            End Try

    '                            If Not String.IsNullOrWhiteSpace(caminhoArquivo) AndAlso IO.File.Exists(caminhoArquivo) Then
    '                                Try
    '                                    docObj = app.Documents.Open(caminhoArquivo)
    '                                    abriuTemp = True
    '                                Catch exOpen As Exception
    '                                    RegistrarErro($"Reabrindo documento '{caminhoArquivo}'", exOpen)
    '                                    Exit While
    '                                End Try
    '                            Else
    '                                Exit While
    '                            End If
    '                        End If

    '                        Dim propSets As Object = docObj.Properties
    '                        For Each propSet As Object In propSets
    '                            For Each prop As Object In propSet
    '                                If prop Is Nothing Then Continue For

    '                                Dim nomeProp As String = ""
    '                                Try
    '                                    nomeProp = CStr(prop.Name)
    '                                Catch
    '                                    Continue For
    '                                End Try
    '                                If String.IsNullOrWhiteSpace(nomeProp) Then Continue For

    '                                ' Propriedades de interesse
    '                                For Each alvo In propsInteressantes
    '                                    If nomeProp.IndexOf(alvo, StringComparison.OrdinalIgnoreCase) >= 0 Then
    '                                        Dim valorBruto As String = ""
    '                                        Try
    '                                            Dim v = prop.Value
    '                                            If v IsNot Nothing Then valorBruto = v.ToString()
    '                                        Catch ex As Exception
    '                                            valorBruto = "(erro)"
    '                                            RegistrarErro($"Lendo propriedade '{nomeProp}'", ex)
    '                                        End Try
    '                                        linha(alvo) = FormatarValor(nomeProp, valorBruto)
    '                                        Exit For
    '                                    End If
    '                                Next

    '                                ' Espessura → Thickness
    '                                If nomeProp.IndexOf("Material Thickness", StringComparison.OrdinalIgnoreCase) >= 0 _
    '                                   OrElse nomeProp.IndexOf("Espessura Do Material", StringComparison.OrdinalIgnoreCase) >= 0 _
    '                                   OrElse nomeProp.Equals("Thickness", StringComparison.OrdinalIgnoreCase) Then

    '                                    Dim valorBruto As String = ""
    '                                    Try
    '                                        Dim v = prop.Value
    '                                        If v IsNot Nothing Then valorBruto = v.ToString()
    '                                    Catch ex As Exception
    '                                        RegistrarErro($"Lendo espessura em '{nomeProp}'", ex)
    '                                    End Try

    '                                    If Not String.IsNullOrWhiteSpace(valorBruto) Then
    '                                        Dim limpo = LimparUnidades(valorBruto).Replace(",", ".")
    '                                        linha("Thickness") = limpo
    '                                    End If
    '                                End If

    '                            Next
    '                        Next

    '                        Exit While ' sucesso

    '                    Catch comEx As System.Runtime.InteropServices.COMException
    '                        If comEx.ErrorCode = &H80010108 Then
    '                            tentativas += 1
    '                            RegistrarErro("PreencherPropriedades - RPC_E_DISCONNECTED, tentando reabrir documento", comEx)
    '                            docObj = Nothing
    '                            Threading.Thread.Sleep(200)
    '                            Continue While
    '                        Else
    '                            RegistrarErro("PreencherPropriedades", comEx)
    '                            Exit While
    '                        End If

    '                    Catch ex As Exception
    '                        RegistrarErro("PreencherPropriedades", ex)
    '                        Exit While
    '                    End Try
    '                End While

    '                ' Fecha doc aberto temporariamente
    '                If abriuTemp AndAlso docObj IsNot Nothing Then
    '                    Try
    '                        Dim isActive As Boolean = False
    '                        Try
    '                            isActive = (TryCast(docObj, Object) Is app.ActiveDocument)
    '                        Catch
    '                            isActive = False
    '                        End Try

    '                        If Not isActive Then
    '                            Try
    '                                CallByName(docObj, "Close", CallType.Method, False)
    '                            Catch ex As Exception
    '                                RegistrarErro("Fechando documento aberto em PreencherPropriedades", ex)
    '                            End Try
    '                        End If
    '                    Catch
    '                        ' ignora
    '                    End Try
    '                End If
    '            End Sub

    '            '------------------------------------------------------------
    '            ' Adiciona linha de PEÇA + materiais abaixo
    '            '------------------------------------------------------------
    '            Dim AdicionarPeca As Action(Of Integer, String, String, Integer, String, Object) =
    '            Sub(nivel As Integer,
    '                nomeComExt As String,
    '                tipoArquivo As String,
    '                qtdeEfetiva As Integer,
    '                caminho As String,
    '                docObj As Object)

    '                Try
    '                    Dim linha = tabela.NewRow()

    '                    linha("TipoLinha") = "PECA"
    '                    linha("Nivel") = nivel
    '                    linha("Arquivo") = nomeComExt
    '                    linha("TipoArquivo") = tipoArquivo
    '                    linha("Qtde") = qtdeEfetiva
    '                    linha("CaminhoCompleto") = caminho

    '                    For Each p In propsInteressantes
    '                        linha(p) = ""
    '                    Next

    '                    linha("RNC") = ""
    '                    linha("Fator") = 1.0
    '                    linha("QtdeTotal") = CDbl(qtdeEfetiva)
    '                    linha("PesoTotal") = 0.0
    '                    linha("AreaPinturaTotal") = 0.0

    '                    ' Campos de material vazios na linha de peça
    '                    linha("CodMatFabricante") = ""
    '                    linha("DescDetal") = ""
    '                    linha("QtdeMaterial") = 0.0
    '                    linha("PesoMaterial") = 0.0

    '                    ' Propriedades do arquivo
    '                    PreencherPropriedades(docObj, linha)

    '                    Dim mass As Double = 0
    '                    Double.TryParse(
    '                        Convert.ToString(linha("Mass")).Replace(",", "."),
    '                        NumberStyles.Any,
    '                        CultureInfo.InvariantCulture,
    '                        mass)

    '                    Dim area As Double = 0
    '                    Double.TryParse(
    '                        Convert.ToString(linha("Área_de_superfície")).Replace(",", "."),
    '                        NumberStyles.Any,
    '                        CultureInfo.InvariantCulture,
    '                        area)

    '                    linha("PesoTotal") = mass * qtdeEfetiva
    '                    linha("AreaPinturaTotal") = area * qtdeEfetiva

    '                    ' Adiciona a linha da peça
    '                    tabela.Rows.Add(linha)

    '                    ' Adiciona os materiais para essa peça
    '                    Try
    '                        LerDadosViewMontaPecaParaTabela(tabela, nivel, nomeComExt, qtdeEfetiva)
    '                    Catch exMat As Exception
    '                        RegistrarErro($"Materiais para '{nomeComExt}'", exMat)
    '                    End Try

    '                Catch ex As Exception
    '                    RegistrarErro($"AdicionarPeca [{nomeComExt}]", ex)
    '                End Try
    '            End Sub

    '            '------------------------------------------------------------
    '            ' Recursão (propaga multiplicador do pai)
    '            '------------------------------------------------------------
    '            Dim Ler As Action(Of Object, String, Integer, Integer) =
    '            Sub(docObj As Object, caminhoArquivo As String, nivel As Integer, multiplicadorPai As Integer)
    '                Try
    '                    If docObj Is Nothing OrElse String.IsNullOrWhiteSpace(caminhoArquivo) Then Exit Sub

    '                    totalLidos += 1
    '                    lbl.Text = $"Lendo {IO.Path.GetFileName(caminhoArquivo)} ({totalLidos})"

    '                    If totalEstimado > 1 Then
    '                        pgb.Style = ProgressBarStyle.Continuous
    '                        pgb.Value = Math.Min(100, CInt((totalLidos / Math.Max(1, totalEstimado)) * 100))
    '                    End If

    '                    Dim ext As String = IO.Path.GetExtension(caminhoArquivo).ToLower()
    '                    Dim nomeComExt As String = IO.Path.GetFileName(caminhoArquivo)
    '                    Dim tipoUpper As String = ext.Replace(".", "").ToUpperInvariant()

    '                    ' Linha da própria peça
    '                    AdicionarPeca(nivel, nomeComExt, tipoUpper, multiplicadorPai, caminhoArquivo, docObj)

    '                    ' Apenas ASM tem filhos
    '                    If ext <> ".asm" Then Exit Sub

    '                    ' Ocorrências
    '                    Dim occsObj As Object = Nothing
    '                    Try
    '                        occsObj = CallByName(docObj, "Occurrences", CallType.Get)
    '                    Catch ex As Exception
    '                        RegistrarErro($"Lendo Occurrences de {nomeComExt}", ex)
    '                        occsObj = Nothing
    '                    End Try
    '                    If occsObj Is Nothing Then Exit Sub

    '                    Dim count As Integer = 0
    '                    Try
    '                        count = CInt(CallByName(occsObj, "Count", CallType.Get))
    '                    Catch ex As Exception
    '                        RegistrarErro($"Lendo Count de {nomeComExt}", ex)
    '                        count = 0
    '                    End Try
    '                    If count <= 0 Then Exit Sub

    '                    totalEstimado += count

    '                    ' Agrupa por arquivo dentro do mesmo pai
    '                    Dim grupo As New Dictionary(Of String, (Caminho As String, QtdeLocal As Integer, DocObj As Object))(StringComparer.OrdinalIgnoreCase)

    '                    For i As Integer = 1 To count
    '                        Try
    '                            Dim occ As Object = CallByName(occsObj, "Item", CallType.Method, i)

    '                            Dim subPath As String = ""
    '                            Try
    '                                subPath = CStr(CallByName(occ, "OccurrenceFileName", CallType.Get))
    '                            Catch ex As Exception
    '                                RegistrarErro($"Lendo OccurrenceFileName (#{i}) de {nomeComExt}", ex)
    '                            End Try

    '                            Dim subDoc As Object = Nothing
    '                            Try
    '                                subDoc = CallByName(occ, "OccurrenceDocument", CallType.Get)
    '                            Catch ex As Exception
    '                                RegistrarErro($"Lendo OccurrenceDocument (#{i}) de {nomeComExt}", ex)
    '                            End Try

    '                            If subDoc Is Nothing AndAlso Not String.IsNullOrWhiteSpace(subPath) AndAlso IO.File.Exists(subPath) Then
    '                                Try
    '                                    subDoc = app.Documents.Open(subPath)
    '                                Catch ex As Exception
    '                                    RegistrarErro($"Abrindo subdocumento '{subPath}'", ex)
    '                                    subDoc = Nothing
    '                                End Try
    '                            End If

    '                            Dim nomeAgrup As String = If(String.IsNullOrWhiteSpace(subPath),
    '                                                         $"ITEM_{i}",
    '                                                         IO.Path.GetFileName(subPath)).ToUpperInvariant()

    '                            Dim qLocal As Integer = 1
    '                            Try
    '                                Dim qtdProp As Object = CallByName(occ, "Quantity", CallType.Get)
    '                                If qtdProp IsNot Nothing Then
    '                                    Dim qi As Integer
    '                                    If Integer.TryParse(qtdProp.ToString(), qi) AndAlso qi > 0 Then qLocal = qi
    '                                End If
    '                            Catch ex As Exception
    '                                RegistrarErro($"Lendo Quantity (#{i}) de {nomeComExt}", ex)
    '                            End Try

    '                            If grupo.ContainsKey(nomeAgrup) Then
    '                                Dim t = grupo(nomeAgrup)
    '                                grupo(nomeAgrup) = (t.Caminho,
    '                                                    t.QtdeLocal + qLocal,
    '                                                    If(t.DocObj Is Nothing, subDoc, t.DocObj))
    '                            Else
    '                                grupo.Add(nomeAgrup,
    '                                          (If(String.IsNullOrWhiteSpace(subPath), "", subPath),
    '                                           qLocal,
    '                                           subDoc))
    '                            End If
    '                        Catch ex As Exception
    '                            RegistrarErro($"Processando ocorrência (#{i}) de {nomeComExt}", ex)
    '                        End Try
    '                    Next

    '                    For Each kv In grupo
    '                        Dim caminhoSub = kv.Value.Caminho
    '                        Dim qLocal = kv.Value.QtdeLocal
    '                        Dim qEfetivaFilho As Integer = qLocal * multiplicadorPai
    '                        Dim subDoc = kv.Value.DocObj

    '                        If String.IsNullOrWhiteSpace(caminhoSub) AndAlso subDoc IsNot Nothing Then
    '                            Try
    '                                caminhoSub = CStr(CallByName(subDoc, "FullName", CallType.Get))
    '                            Catch ex As Exception
    '                                RegistrarErro("Lendo FullName de subdocumento", ex)
    '                                caminhoSub = ""
    '                            End Try
    '                        End If

    '                        Dim nomeFilho As String
    '                        Dim tipoFilho As String

    '                        If Not String.IsNullOrWhiteSpace(caminhoSub) Then
    '                            nomeFilho = IO.Path.GetFileName(caminhoSub)
    '                            tipoFilho = IO.Path.GetExtension(caminhoSub).Replace(".", "").ToUpperInvariant()
    '                        Else
    '                            nomeFilho = kv.Key
    '                            tipoFilho = ""
    '                        End If

    '                        If subDoc IsNot Nothing AndAlso Not String.IsNullOrWhiteSpace(caminhoSub) Then
    '                            Ler(subDoc, caminhoSub, nivel + 1, qEfetivaFilho)

    '                            ' Fecha se abrimos temporariamente
    '                            Try
    '                                Dim isActive As Boolean = False
    '                                Try
    '                                    isActive = (TryCast(subDoc, Object) Is app.ActiveDocument)
    '                                Catch
    '                                    isActive = False
    '                                End Try

    '                                If Not isActive AndAlso IO.File.Exists(caminhoSub) Then
    '                                    Try
    '                                        CallByName(subDoc, "Close", CallType.Method, False)
    '                                    Catch
    '                                    End Try
    '                                End If
    '                            Catch ex As Exception
    '                                RegistrarErro($"Fechando subdocumento '{caminhoSub}'", ex)
    '                            End Try

    '                        Else
    '                            ' Sem doc carregado, mas queremos listar
    '                            AdicionarPeca(nivel + 1,
    '                                          nomeFilho,
    '                                          tipoFilho,
    '                                          qEfetivaFilho,
    '                                          caminhoSub,
    '                                          subDoc)
    '                        End If

    '                    Next

    '                Catch ex As Exception
    '                    RegistrarErro($"Ler('{caminhoArquivo}')", ex)
    '                End Try
    '            End Sub

    '            '============================================================
    '            ' 🔹 Chamada inicial da recursão
    '            '============================================================
    '            Ler(doc, doc.FullName, 0, 1)

    '            '============================================================
    '            ' 🔹 Limpeza de peças “fantasmas”
    '            '============================================================
    '            For i As Integer = tabela.Rows.Count - 1 To 0 Step -1
    '                Dim arq As String = Convert.ToString(tabela.Rows(i)("Arquivo"))
    '                If String.IsNullOrWhiteSpace(arq) AndAlso
    '               String.Equals(Convert.ToString(tabela.Rows(i)("TipoLinha")),
    '                             "PECA",
    '                             StringComparison.OrdinalIgnoreCase) Then
    '                    tabela.Rows.RemoveAt(i)
    '                End If
    '            Next

    '            '============================================================
    '            ' 🔹 Cálculo dos TOTAIS (apenas peças)
    '            '============================================================
    '            Dim totalQtdSolicitada As Double = 0
    '            Dim arquivosUnicos As New HashSet(Of String)(StringComparer.OrdinalIgnoreCase)

    '            For Each row As DataRow In tabela.Rows
    '                If Not String.Equals(Convert.ToString(row("TipoLinha")), "PECA", StringComparison.OrdinalIgnoreCase) Then
    '                    Continue For
    '                End If

    '                Dim qtd As Double
    '                Double.TryParse(
    '                Convert.ToString(row("QtdeTotal")).Replace(",", "."),
    '                NumberStyles.Any,
    '                CultureInfo.InvariantCulture,
    '                qtd)
    '                totalQtdSolicitada += qtd

    '                Dim arq As String = Convert.ToString(row("Arquivo"))
    '                If Not String.IsNullOrWhiteSpace(arq) Then
    '                    arquivosUnicos.Add(arq)
    '                End If
    '            Next

    '            Dim totalPecasDiferentes As Integer = arquivosUnicos.Count
    '            Dim totalPecas As Integer =
    '            tabela.Rows.Cast(Of DataRow)().
    '                   Count(Function(r) String.Equals(Convert.ToString(r("TipoLinha")),
    '                                                   "PECA",
    '                                                   StringComparison.OrdinalIgnoreCase))

    '            '============================================================
    '            ' 🔹 Bind no grid
    '            '============================================================
    '            dgv.DataSource = tabela
    '            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells
    '            dgv.AllowUserToAddRows = False
    '            dgv.ColumnHeadersDefaultCellStyle.Font = New Font(dgv.Font, FontStyle.Bold)

    '            If dgv.Columns.Contains("Arquivo") Then
    '                dgv.Columns("Arquivo").Frozen = True
    '            End If

    '            tabelaOriginalBOM = tabela.Copy()

    '            Dim ocultar() As String = {
    '    "TipoArquivo", "CaminhoCompleto", "Categoria", "Gerente",
    '    "Autor", "Empresa", "Data", "DataR", "Data1",
    '    "Revision", "CutSizeX", "CutSizeY", "Mass",
    '    "Área_de_superfície", "Area_de_superficie", "Bloqueado",
    '    "RNC", "PesoTotal", "AreaPinturaTotal",
    '    "QtdeMaterial", "PesoMaterial", "CodMatFabricante", "DescDetal"
    '}


    '            For Each c In ocultar
    '                If dgv.Columns.Contains(c) Then
    '                    dgv.Columns(c).Visible = False
    '                End If
    '            Next

    '            '============================================================
    '            ' 🔹 UI - final
    '            '============================================================
    '            pgb.Style = ProgressBarStyle.Continuous
    '            pgb.Value = 100

    '            lblResumo.Text =
    '            "✅ Estrutura concluída." & vbCrLf &
    '            $"• Total de peças solicitadas (QtdeTotal): {totalQtdSolicitada}" & vbCrLf &
    '            $"• Total de peças diferentes (arquivos únicos): {totalPecasDiferentes}" & vbCrLf &
    '            $"• Total de peças na estrutura (linhas de peça): {totalPecas}"

    '            Threading.Thread.Sleep(550)

    '        Catch ex As Exception
    '            RegistrarErro("Erro geral em ListarEstruturaBomMateriais", ex)

    '        Finally
    '            pgb.Visible = False

    '            If erros IsNot Nothing AndAlso erros.Count > 0 Then
    '                Dim sb As New StringBuilder()
    '                sb.AppendLine("Foram encontrados " & erros.Count & " erro(s) durante a leitura da estrutura:")
    '                sb.AppendLine()

    '                Dim i As Integer = 1
    '                For Each msg In erros
    '                    sb.AppendLine(i.ToString("00") & " - " & msg)
    '                    i += 1
    '                Next

    '                If qtdRpcDesconectado > 0 Then
    '                    sb.AppendLine()
    '                    sb.AppendLine(qtdRpcDesconectado.ToString() &
    '                              " erro(s) são do tipo RPC_E_DISCONNECTED (necessário revisar/ajustar o arquivo no Solid Edge).")
    '                End If

    '                MessageBox.Show(sb.ToString(),
    '                            "Relatório de erros - Estrutura",
    '                            MessageBoxButtons.OK,
    '                            MessageBoxIcon.Warning)
    '            End If

    '        End Try

    '    End Sub

    '    ' Lê os materiais de TabelaViewMontaPeca para uma PEÇA
    '    ' e adiciona linhas do tipo MATERIAL na mesma DataTable da BOM
    '    Public Sub LerDadosViewMontaPecaParaTabela(tabelaDest As DataTable,
    '                                           nivelPeca As Integer,
    '                                           nomeArquivoComExt As String,
    '                                           qtdeEfetivaPeca As Integer)

    '        If TabelaViewMontaPeca Is Nothing Then Exit Sub

    '        ' 👉 Usa o nome COM extensão, igual está em NomeArquivoSemExtensao na view
    '        Dim chaveArquivo As String = IO.Path.GetFileName(nomeArquivoComExt)

    '        DadosArquivoCorrente.NomeArquivoSemExtensao = chaveArquivo
    '        DadosArquivoCorrente.qtde = qtdeEfetivaPeca

    '        ' Case-insensitive
    '        TabelaViewMontaPeca.CaseSensitive = False

    '        Dim dv As New DataView(TabelaViewMontaPeca)
    '        Dim filtro As String = chaveArquivo.Replace("'", "''")
    '        dv.RowFilter = $"NomeArquivoSemExtensao = '{filtro}'"

    '        Dim dtFiltrado As DataTable = dv.ToTable()

    '        'For Each linha As DataRow In dtFiltrado.Rows
    '        '    Try
    '        '        ' Confirma novamente ignorando maiúsc/minúsc (por segurança)
    '        '        If Not String.Equals(
    '        '    linha("NomeArquivoSemExtensao").ToString().Trim(),
    '        '    chaveArquivo.Trim(),
    '        '    StringComparison.OrdinalIgnoreCase) Then

    '        '            Continue For
    '        '        End If

    '        '        iconeTipoArquivo = My.Resources.sinco_Diversos

    '        '        Dim PecaQtde As Double
    '        '        Dim peso As Double

    '        '        ' Quantidade por peça (view)
    '        '        Dim pecaQtdeStr As String
    '        '        Try
    '        '            pecaQtdeStr = linha.Item("PecaQtde").ToString().Trim().Replace(",", ".")
    '        '        Catch
    '        '            pecaQtdeStr = "0"
    '        '        End Try
    '        '        PecaQtde = cl_BancoDados.converteStringParaDouble(pecaQtdeStr)

    '        '        ' Peso unitário (view)
    '        '        Dim pecaPesoStr As String
    '        '        Try
    '        '            pecaPesoStr = linha.Item("Peso").ToString().Trim().Replace(",", ".")
    '        '        Catch
    '        '            pecaPesoStr = "0"
    '        '        End Try
    '        '        peso = cl_BancoDados.converteStringParaDouble(pecaPesoStr)

    '        '        ' Totais para o conjunto (peça * QtdeEfetivaPeca)
    '        '        Dim PesoMaterial As Double
    '        '        Try
    '        '            PesoMaterial = CDbl(peso) * CDbl(qtdeEfetivaPeca)
    '        '        Catch
    '        '            PesoMaterial = 0
    '        '        End Try

    '        '        Dim QtdeMaterial As Double
    '        '        Try
    '        '            QtdeMaterial = CDbl(PecaQtde) * CDbl(qtdeEfetivaPeca)
    '        '        Catch
    '        '            QtdeMaterial = 0
    '        '        End Try

    '        '        ' ----------------------------------------------
    '        '        ' MONTA LINHA DE MATERIAL NA MESMA TABELA
    '        '        ' ----------------------------------------------
    '        '        Dim drMat As DataRow = tabelaDest.NewRow()

    '        '        drMat("TipoLinha") = "MATERIAL"
    '        '        drMat("Nivel") = nivelPeca + 1

    '        '        ' 🔹 Arquivo → mostra o CodMatFabricante
    '        '        Dim cod As String = linha.Item("CodMatFabricante").ToString().Trim().ToUpper()
    '        '        drMat("Arquivo") = cod
    '        '        drMat("CodMatFabricante") = cod

    '        '        ' TipoArquivo pode ficar vazio ou "MAT"
    '        '        drMat("TipoArquivo") = "MAT"

    '        '        ' 🔹 QtdeMaterial deve aparecer na MESMA coluna de Qtde
    '        '        drMat("Qtde") = QtdeMaterial
    '        '        drMat("QtdeMaterial") = QtdeMaterial

    '        '        drMat("CaminhoCompleto") = ""

    '        '        ' 🔹 DescDetal + Material na coluna Título
    '        '        Dim desc As String = linha.Item("DescDetal").ToString().Trim().ToUpper()
    '        '        Dim matView As String = ""
    '        '        Try
    '        '            matView = linha.Item("Material").ToString().Trim().ToUpper()
    '        '        Catch
    '        '            matView = ""
    '        '        End Try

    '        '        Dim titulo As String = desc
    '        '        If matView <> "" Then
    '        '            titulo &= " - " & matView
    '        '        End If

    '        '        drMat("Título") = titulo

    '        '        ' Opcional: também preenchendo coluna Material da peça
    '        '        Try
    '        '            If matView <> "" Then drMat("Material") = matView
    '        '        Catch
    '        '        End Try

    '        '        ' 🔹 TipoLinha na mesma coluna de "Tipo de Desenho"
    '        '        Try
    '        '            drMat("Tipo de Desenho") = "MATERIAL"
    '        '        Catch
    '        '        End Try

    '        '        ' Campos de peça que não fazem sentido para material
    '        '        drMat("RNC") = ""
    '        '        drMat("Fator") = 1.0
    '        '        drMat("QtdeTotal") = 0
    '        '        drMat("PesoTotal") = 0
    '        '        drMat("AreaPinturaTotal") = 0

    '        '        ' Mantém PesoMaterial guardado, mas vamos ocultar a coluna no grid
    '        '        drMat("PesoMaterial") = PesoMaterial

    '        '        tabelaDest.Rows.Add(drMat)

    '        '    Catch
    '        '        Continue For
    '        '    End Try
    '        'Next

    '        For Each linha As DataRow In dtFiltrado.Rows
    '            Try
    '                ' Confirma novamente ignorando maiúsc/minúsc (por segurança)
    '                If Not String.Equals(
    '            linha("NomeArquivoSemExtensao").ToString().Trim(),
    '            chaveArquivo.Trim(),
    '            StringComparison.OrdinalIgnoreCase) Then

    '                    Continue For
    '                End If

    '                iconeTipoArquivo = My.Resources.sinco_Diversos

    '                ' ============================================
    '                ' 1) Quantidades base
    '                '    PecaQtde  -> qtde de material POR PEÇA
    '                '    qtdeEfetivaPeca -> qtde da peça pai na estrutura
    '                ' ============================================
    '                Dim PecaQtde As Double
    '                Dim peso As Double

    '                Dim pecaQtdeStr As String
    '                Try
    '                    pecaQtdeStr = linha.Item("PecaQtde").ToString().Trim().Replace(",", ".")
    '                Catch
    '                    pecaQtdeStr = "0"
    '                End Try
    '                PecaQtde = cl_BancoDados.converteStringParaDouble(pecaQtdeStr)

    '                Dim pecaPesoStr As String
    '                Try
    '                    pecaPesoStr = linha.Item("Peso").ToString().Trim().Replace(",", ".")
    '                Catch
    '                    pecaPesoStr = "0"
    '                End Try
    '                peso = cl_BancoDados.converteStringParaDouble(pecaPesoStr)

    '                ' qtde da peça pai na estrutura (veio do caller)
    '                Dim qtdPecaPai As Double = qtdeEfetivaPeca

    '                ' fator (pode ser usado depois; por enquanto 1)
    '                Dim fator As Double = 1.0

    '                ' ============================================
    '                ' 2) Cálculos conforme regra pedida
    '                '    QtdeMaterial  -> qtde de material POR PEÇA
    '                '    Qtde (visível) -> material total = PecaQtde * qtdPecaPai
    '                '    QtdeTotal     -> PecaQtde * qtdPecaPai * fator
    '                ' ============================================
    '                Dim QtdeMaterial As Double = PecaQtde
    '                Dim QtdeVisivel As Double = PecaQtde * qtdPecaPai
    '                Dim QtdeTotalMaterial As Double = PecaQtde * qtdPecaPai * fator

    '                ' Peso total de material (se quiser manter)
    '                Dim PesoMaterial As Double
    '                Try
    '                    PesoMaterial = CDbl(peso) * qtdPecaPai
    '                Catch
    '                    PesoMaterial = 0
    '                End Try

    '                ' ============================================
    '                ' 3) Monta linha de MATERIAL na tabela
    '                ' ============================================
    '                Dim drMat As DataRow = tabelaDest.NewRow()

    '                drMat("TipoLinha") = "MATERIAL"
    '                drMat("Nivel") = nivelPeca + 1

    '                ' Arquivo → mostra o CodMatFabricante
    '                Dim cod As String = linha.Item("CodMatFabricante").ToString().Trim().ToUpper()
    '                drMat("Arquivo") = cod
    '                drMat("CodMatFabricante") = cod

    '                drMat("TipoArquivo") = "MAT"

    '                ' 🔹 Qtde (coluna visível) = qtde(material) * qtde(peça pai)
    '                drMat("Qtde") = QtdeVisivel

    '                ' 🔹 QtdeMaterial (interna) = qtde(material POR peça)
    '                drMat("QtdeMaterial") = QtdeMaterial

    '                drMat("CaminhoCompleto") = ""

    '                ' Título = DescDetal + Material
    '                Dim desc As String = linha.Item("DescDetal").ToString().Trim().ToUpper()
    '                Dim matView As String = ""
    '                Try
    '                    matView = linha.Item("Material").ToString().Trim().ToUpper()
    '                Catch
    '                    matView = ""
    '                End Try

    '                Dim titulo As String = desc
    '                If matView <> "" Then
    '                    titulo &= " - " & matView
    '                End If

    '                drMat("Título") = titulo

    '                ' Também preenche Material, se existir
    '                Try
    '                    If matView <> "" Then drMat("Material") = matView
    '                Catch
    '                End Try

    '                ' Tipo de Desenho = MATERIAL
    '                Try
    '                    drMat("Tipo de Desenho") = "MATERIAL"
    '                Catch
    '                End Try

    '                ' Campos de controle
    '                drMat("RNC") = ""
    '                drMat("Fator") = fator

    '                ' 🔹 QtdeTotal = qtde(material) * qtde(peça pai) * fator
    '                drMat("QtdeTotal") = QtdeTotalMaterial

    '                drMat("PesoTotal") = 0
    '                drMat("AreaPinturaTotal") = 0

    '                ' Mantém PesoMaterial guardado (coluna oculta)
    '                drMat("PesoMaterial") = PesoMaterial

    '                tabelaDest.Rows.Add(drMat)

    '            Catch
    '                Continue For
    '            End Try
    '        Next



    '    End Sub


#End Region


#Region "VERSÃO NOVA - 27-11-2025 rev 03"



    ''================================================================================
    '' Monta estrutura BOM + MATERIAIS no MESMO DataGridView
    ''================================================================================
    'Public Sub ListarEstruturaBomMateriais(ByVal dgv As DataGridView,
    '                                   ByVal pgb As ProgressBar,
    '                                   ByVal lbl As Label,
    '                                   ByVal lblResumo As Label)

    '    ' Lista que acumula erros
    '    Dim erros As New List(Of String)
    '    Dim qtdRpcDesconectado As Integer = 0

    '    ' Helper para registrar erros com contexto
    '    Dim RegistrarErro As Action(Of String, Exception) =
    '    Sub(contexto As String, ex As Exception)
    '        Try
    '            If ex Is Nothing Then Return

    '            Dim msg As String
    '            Dim comEx = TryCast(ex, System.Runtime.InteropServices.COMException)

    '            If comEx IsNot Nothing AndAlso comEx.ErrorCode = &H80010108 Then
    '                qtdRpcDesconectado += 1
    '                msg = $"{contexto}: [RPC_E_DISCONNECTED] O objeto do Solid Edge foi desconectado. Detalhe: {ex.Message}"
    '            Else
    '                msg = $"{contexto}: {ex.Message}"
    '            End If

    '            erros.Add(msg)
    '        Catch
    '            ' não deixa o log quebrar o fluxo
    '        End Try
    '    End Sub

    '    Try
    '        '============================================================
    '        ' 🔹 Sessão do Solid Edge e documento ativo
    '        '============================================================
    '        If app Is Nothing OrElse app.Documents.Count = 0 Then
    '            MessageBox.Show("Nenhum documento aberto no Solid Edge.",
    '                        "SINCO - Solid Edge",
    '                        MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
    '            Exit Sub
    '        End If

    '        Dim doc As Object = app.ActiveDocument
    '        If doc Is Nothing Then Exit Sub

    '        '============================================================
    '        ' 🔹 UI - estado inicial
    '        '============================================================
    '        pgb.Visible = True
    '        pgb.Style = ProgressBarStyle.Marquee
    '        pgb.Value = 0
    '        lbl.Visible = True
    '        lbl.Text = "Lendo estrutura... aguarde."

    '        '============================================================
    '        ' 🔹 Tabela destino
    '        '============================================================
    '        Dim tabela As New DataTable("BOM_Completa")

    '        ' Colunas fixas
    '        tabela.Columns.Add("Nivel", GetType(Integer))
    '        tabela.Columns.Add("Arquivo", GetType(String))          ' com extensão
    '        tabela.Columns.Add("TipoArquivo", GetType(String))      ' PAR/PSM/ASM...
    '        tabela.Columns.Add("Qtde", GetType(Double))             ' quantidade efetiva no Nivel
    '        tabela.Columns.Add("CaminhoCompleto", GetType(String))

    '        ' Tipo de linha (PECA / MATERIAL)
    '        tabela.Columns.Add("TipoLinha", GetType(String))

    '        ' Propriedades de interesse
    '        Dim propsInteressantes As New List(Of String) From {
    '        "Título", "Assunto", "Coment", "Tipo de Desenho", "Palavras-", "Autor", "Empresa",
    '        "Categoria", "Gerente", "Material", "Data", "DataR", "Data1",
    '        "Revision", "CutSizeX", "CutSizeY", "Thickness",
    '        "Mass", "Área_de_superfície", "Area_de_superficie", "Bloqueado"
    '    }
    '        For Each p In propsInteressantes
    '            If Not tabela.Columns.Contains(p) Then
    '                tabela.Columns.Add(p, GetType(String))
    '            End If
    '        Next

    '        ' Colunas de totais de peça
    '        tabela.Columns.Add("RNC", GetType(String))
    '        tabela.Columns.Add("Fator", GetType(Double))
    '        tabela.Columns.Add("QtdeTotal", GetType(Double))
    '        tabela.Columns.Add("PesoTotal", GetType(Double))
    '        tabela.Columns.Add("AreaPinturaTotal", GetType(Double))

    '        ' Colunas específicas para MATERIAL
    '        tabela.Columns.Add("CodMatFabricante", GetType(String))
    '        tabela.Columns.Add("DescDetal", GetType(String))
    '        tabela.Columns.Add("QtdeMaterial", GetType(Double))
    '        tabela.Columns.Add("PesoMaterial", GetType(Double))

    '        Dim totalLidos As Integer = 0
    '        Dim totalEstimado As Integer = 1

    '        '============================================================
    '        ' 🔹 Helpers
    '        '============================================================
    '        Dim LimparUnidades As Func(Of String, String) =
    '        Function(txt As String) As String
    '            If String.IsNullOrWhiteSpace(txt) Then Return ""
    '            Return txt _
    '                .Replace("mm²", "") _
    '                .Replace("MM²", "") _
    '                .Replace("mm", "") _
    '                .Replace("MM", "") _
    '                .Replace("^2", "") _
    '                .Replace("KG", "") _
    '                .Replace("kg", "") _
    '                .Trim()
    '        End Function

    '        Dim FormatarValor As Func(Of String, String, String) =
    '        Function(propNome As String, valorBruto As String) As String
    '            If String.IsNullOrWhiteSpace(valorBruto) Then Return ""
    '            Dim nome = propNome.Trim().ToUpperInvariant()
    '            Dim valor = LimparUnidades(valorBruto)

    '            ' Datas
    '            If nome = "DATA" OrElse nome = "DATAR" OrElse nome = "DATA1" Then
    '                Try
    '                    Return cl_BancoDados.FormatarData(valor)
    '                Catch
    '                    Return valor
    '                End Try
    '            End If

    '            ' Revisão
    '            If nome.Contains("REVISION") Then Return valor.ToUpperInvariant()

    '            ' CutSize / Espessura / Massa -> ponto decimal
    '            If nome.Contains("CUTSIZEX") Then Return valor.Replace(",", ".")
    '            If nome.Contains("CUTSIZEY") Then Return valor.Replace(",", ".")
    '            If nome.Contains("THICKNESS") Then Return valor.Replace(",", ".")
    '            If nome.Contains("MASS") Then Return valor.Replace(",", ".")

    '            ' Área mm² -> m²
    '            If nome.Contains("ÁREA") OrElse nome.Contains("AREA") Then
    '                Dim num As Double
    '                If Double.TryParse(valor.Replace(",", "."),
    '                                   NumberStyles.Any,
    '                                   CultureInfo.InvariantCulture,
    '                                   num) Then
    '                    Dim m2 = num / 1_000_000.0
    '                    Return m2.ToString("N4", CultureInfo.GetCultureInfo("pt-BR"))
    '                End If
    '            End If

    '            Return valor
    '        End Function

    '        '------------------------------------------------------------
    '        ' PreencherPropriedades
    '        '------------------------------------------------------------
    '        Dim PreencherPropriedades As Action(Of Object, DataRow) =
    '        Sub(docObj As Object, linha As DataRow)
    '            If linha Is Nothing Then Exit Sub

    '            Dim tentativas As Integer = 0
    '            Dim abriuTemp As Boolean = False

    '            While tentativas < 2
    '                Try
    '                    ' Reabrir se necessário
    '                    If docObj Is Nothing Then
    '                        Dim caminhoArquivo As String = ""
    '                        Try
    '                            caminhoArquivo = Convert.ToString(linha("CaminhoCompleto"))
    '                        Catch
    '                            caminhoArquivo = ""
    '                        End Try

    '                        If Not String.IsNullOrWhiteSpace(caminhoArquivo) AndAlso IO.File.Exists(caminhoArquivo) Then
    '                            Try
    '                                docObj = app.Documents.Open(caminhoArquivo)
    '                                abriuTemp = True
    '                            Catch exOpen As Exception
    '                                RegistrarErro($"Reabrindo documento '{caminhoArquivo}'", exOpen)
    '                                Exit While
    '                            End Try
    '                        Else
    '                            Exit While
    '                        End If
    '                    End If

    '                    Dim propSets As Object = docObj.Properties
    '                    For Each propSet As Object In propSets
    '                        For Each prop As Object In propSet
    '                            If prop Is Nothing Then Continue For

    '                            Dim nomeProp As String = ""
    '                            Try
    '                                nomeProp = CStr(prop.Name)
    '                            Catch
    '                                Continue For
    '                            End Try
    '                            If String.IsNullOrWhiteSpace(nomeProp) Then Continue For

    '                            ' Propriedades de interesse
    '                            For Each alvo In propsInteressantes
    '                                If nomeProp.IndexOf(alvo, StringComparison.OrdinalIgnoreCase) >= 0 Then
    '                                    Dim valorBruto As String = ""
    '                                    Try
    '                                        Dim v = prop.Value
    '                                        If v IsNot Nothing Then valorBruto = v.ToString()
    '                                    Catch ex As Exception
    '                                        valorBruto = "(erro)"
    '                                        RegistrarErro($"Lendo propriedade '{nomeProp}'", ex)
    '                                    End Try
    '                                    linha(alvo) = FormatarValor(nomeProp, valorBruto)
    '                                    Exit For
    '                                End If
    '                            Next

    '                            ' Espessura → Thickness
    '                            If nomeProp.IndexOf("Material Thickness", StringComparison.OrdinalIgnoreCase) >= 0 _
    '                               OrElse nomeProp.IndexOf("Espessura Do Material", StringComparison.OrdinalIgnoreCase) >= 0 _
    '                               OrElse nomeProp.Equals("Thickness", StringComparison.OrdinalIgnoreCase) Then

    '                                Dim valorBruto As String = ""
    '                                Try
    '                                    Dim v = prop.Value
    '                                    If v IsNot Nothing Then valorBruto = v.ToString()
    '                                Catch ex As Exception
    '                                    RegistrarErro($"Lendo espessura em '{nomeProp}'", ex)
    '                                End Try

    '                                If Not String.IsNullOrWhiteSpace(valorBruto) Then
    '                                    Dim limpo = LimparUnidades(valorBruto).Replace(",", ".")
    '                                    linha("Thickness") = limpo
    '                                End If
    '                            End If

    '                        Next
    '                    Next

    '                    Exit While ' sucesso

    '                Catch comEx As System.Runtime.InteropServices.COMException
    '                    If comEx.ErrorCode = &H80010108 Then
    '                        tentativas += 1
    '                        RegistrarErro("PreencherPropriedades - RPC_E_DISCONNECTED, tentando reabrir documento", comEx)
    '                        docObj = Nothing
    '                        Threading.Thread.Sleep(200)
    '                        Continue While
    '                    Else
    '                        RegistrarErro("PreencherPropriedades", comEx)
    '                        Exit While
    '                    End If

    '                Catch ex As Exception
    '                    RegistrarErro("PreencherPropriedades", ex)
    '                    Exit While
    '                End Try
    '            End While

    '            ' Fecha doc aberto temporariamente
    '            If abriuTemp AndAlso docObj IsNot Nothing Then
    '                Try
    '                    Dim isActive As Boolean = False
    '                    Try
    '                        isActive = (TryCast(docObj, Object) Is app.ActiveDocument)
    '                    Catch
    '                        isActive = False
    '                    End Try

    '                    If Not isActive Then
    '                        Try
    '                            CallByName(docObj, "Close", CallType.Method, False)
    '                        Catch ex As Exception
    '                            RegistrarErro("Fechando documento aberto em PreencherPropriedades", ex)
    '                        End Try
    '                    End If
    '                Catch
    '                    ' ignora
    '                End Try
    '            End If
    '        End Sub

    '        '------------------------------------------------------------
    '        ' Adiciona linha de PEÇA + materiais abaixo
    '        '------------------------------------------------------------
    '        Dim AdicionarPeca As Action(Of Integer, String, String, Integer, String, Object) =
    '        Sub(nivel As Integer,
    '            nomeComExt As String,
    '            tipoArquivo As String,
    '            qtdeEfetiva As Integer,
    '            caminho As String,
    '            docObj As Object)

    '            Try
    '                Dim linha = tabela.NewRow()

    '                linha("TipoLinha") = "PECA"
    '                linha("Nivel") = nivel
    '                linha("Arquivo") = nomeComExt
    '                linha("TipoArquivo") = tipoArquivo
    '                linha("Qtde") = CDbl(qtdeEfetiva)
    '                linha("CaminhoCompleto") = caminho

    '                For Each p In propsInteressantes
    '                    linha(p) = ""
    '                Next

    '                linha("RNC") = ""
    '                linha("Fator") = 1.0
    '                linha("QtdeTotal") = CDbl(qtdeEfetiva)
    '                linha("PesoTotal") = 0.0
    '                linha("AreaPinturaTotal") = 0.0

    '                ' Campos de material vazios na linha de peça
    '                linha("CodMatFabricante") = ""
    '                linha("DescDetal") = ""
    '                linha("QtdeMaterial") = 0.0
    '                linha("PesoMaterial") = 0.0

    '                ' Propriedades do arquivo
    '                PreencherPropriedades(docObj, linha)

    '                Dim mass As Double = 0
    '                Double.TryParse(
    '                    Convert.ToString(linha("Mass")).Replace(",", "."),
    '                    NumberStyles.Any,
    '                    CultureInfo.InvariantCulture,
    '                    mass)

    '                Dim area As Double = 0
    '                Double.TryParse(
    '                    Convert.ToString(linha("Área_de_superfície")).Replace(",", "."),
    '                    NumberStyles.Any,
    '                    CultureInfo.InvariantCulture,
    '                    area)

    '                linha("PesoTotal") = mass * qtdeEfetiva
    '                linha("AreaPinturaTotal") = area * qtdeEfetiva

    '                ' Adiciona a linha da peça
    '                tabela.Rows.Add(linha)

    '                ' Adiciona os materiais para essa peça
    '                Try
    '                    LerDadosViewMontaPecaParaTabela(tabela, nivel, nomeComExt, qtdeEfetiva)
    '                Catch exMat As Exception
    '                    RegistrarErro($"Materiais para '{nomeComExt}'", exMat)
    '                End Try

    '            Catch ex As Exception
    '                RegistrarErro($"AdicionarPeca [{nomeComExt}]", ex)
    '            End Try
    '        End Sub

    '        '------------------------------------------------------------
    '        ' Recursão (propaga multiplicador do pai)
    '        '------------------------------------------------------------
    '        Dim Ler As Action(Of Object, String, Integer, Integer) =
    '        Sub(docObj As Object, caminhoArquivo As String, nivel As Integer, multiplicadorPai As Integer)
    '            Try
    '                If docObj Is Nothing OrElse String.IsNullOrWhiteSpace(caminhoArquivo) Then Exit Sub

    '                totalLidos += 1
    '                lbl.Text = $"Lendo {IO.Path.GetFileName(caminhoArquivo)} ({totalLidos})"

    '                If totalEstimado > 1 Then
    '                    pgb.Style = ProgressBarStyle.Continuous
    '                    pgb.Value = Math.Min(100, CInt((totalLidos / Math.Max(1, totalEstimado)) * 100))
    '                End If

    '                Dim ext As String = IO.Path.GetExtension(caminhoArquivo).ToLower()
    '                Dim nomeComExt As String = IO.Path.GetFileName(caminhoArquivo)
    '                Dim tipoUpper As String = ext.Replace(".", "").ToUpperInvariant()

    '                ' Linha da própria peça
    '                AdicionarPeca(nivel, nomeComExt, tipoUpper, multiplicadorPai, caminhoArquivo, docObj)

    '                ' Apenas ASM tem filhos
    '                If ext <> ".asm" Then Exit Sub

    '                ' Ocorrências
    '                Dim occsObj As Object = Nothing
    '                Try
    '                    occsObj = CallByName(docObj, "Occurrences", CallType.Get)
    '                Catch ex As Exception
    '                    RegistrarErro($"Lendo Occurrences de {nomeComExt}", ex)
    '                    occsObj = Nothing
    '                End Try
    '                If occsObj Is Nothing Then Exit Sub

    '                Dim count As Integer = 0
    '                Try
    '                    count = CInt(CallByName(occsObj, "Count", CallType.Get))
    '                Catch ex As Exception
    '                    RegistrarErro($"Lendo Count de {nomeComExt}", ex)
    '                    count = 0
    '                End Try
    '                If count <= 0 Then Exit Sub

    '                totalEstimado += count

    '                ' Agrupa por arquivo dentro do mesmo pai
    '                Dim grupo As New Dictionary(Of String, (Caminho As String, QtdeLocal As Integer, DocObj As Object))(StringComparer.OrdinalIgnoreCase)

    '                For i As Integer = 1 To count
    '                    Try
    '                        Dim occ As Object = CallByName(occsObj, "Item", CallType.Method, i)

    '                        Dim subPath As String = ""
    '                        Try
    '                            subPath = CStr(CallByName(occ, "OccurrenceFileName", CallType.Get))
    '                        Catch ex As Exception
    '                            RegistrarErro($"Lendo OccurrenceFileName (#{i}) de {nomeComExt}", ex)
    '                        End Try

    '                        Dim subDoc As Object = Nothing
    '                        Try
    '                            subDoc = CallByName(occ, "OccurrenceDocument", CallType.Get)
    '                        Catch ex As Exception
    '                            RegistrarErro($"Lendo OccurrenceDocument (#{i}) de {nomeComExt}", ex)
    '                        End Try

    '                        If subDoc Is Nothing AndAlso Not String.IsNullOrWhiteSpace(subPath) AndAlso IO.File.Exists(subPath) Then
    '                            Try
    '                                subDoc = app.Documents.Open(subPath)
    '                            Catch ex As Exception
    '                                RegistrarErro($"Abrindo subdocumento '{subPath}'", ex)
    '                                subDoc = Nothing
    '                            End Try
    '                        End If

    '                        Dim nomeAgrup As String = If(String.IsNullOrWhiteSpace(subPath),
    '                                                     $"ITEM_{i}",
    '                                                     IO.Path.GetFileName(subPath)).ToUpperInvariant()

    '                        Dim qLocal As Integer = 1
    '                        Try
    '                            Dim qtdProp As Object = CallByName(occ, "Quantity", CallType.Get)
    '                            If qtdProp IsNot Nothing Then
    '                                Dim qi As Integer
    '                                If Integer.TryParse(qtdProp.ToString(), qi) AndAlso qi > 0 Then qLocal = qi
    '                            End If
    '                        Catch ex As Exception
    '                            RegistrarErro($"Lendo Quantity (#{i}) de {nomeComExt}", ex)
    '                        End Try

    '                        If grupo.ContainsKey(nomeAgrup) Then
    '                            Dim t = grupo(nomeAgrup)
    '                            grupo(nomeAgrup) = (t.Caminho,
    '                                                t.QtdeLocal + qLocal,
    '                                                If(t.DocObj Is Nothing, subDoc, t.DocObj))
    '                        Else
    '                            grupo.Add(nomeAgrup,
    '                                      (If(String.IsNullOrWhiteSpace(subPath), "", subPath),
    '                                       qLocal,
    '                                       subDoc))
    '                        End If
    '                    Catch ex As Exception
    '                        RegistrarErro($"Processando ocorrência (#{i}) de {nomeComExt}", ex)
    '                    End Try
    '                Next

    '                For Each kv In grupo
    '                    Dim caminhoSub = kv.Value.Caminho
    '                    Dim qLocal = kv.Value.QtdeLocal
    '                    Dim qEfetivaFilho As Integer = qLocal * multiplicadorPai
    '                    Dim subDoc = kv.Value.DocObj

    '                    If String.IsNullOrWhiteSpace(caminhoSub) AndAlso subDoc IsNot Nothing Then
    '                        Try
    '                            caminhoSub = CStr(CallByName(subDoc, "FullName", CallType.Get))
    '                        Catch ex As Exception
    '                            RegistrarErro("Lendo FullName de subdocumento", ex)
    '                            caminhoSub = ""
    '                        End Try
    '                    End If

    '                    Dim nomeFilho As String
    '                    Dim tipoFilho As String

    '                    If Not String.IsNullOrWhiteSpace(caminhoSub) Then
    '                        nomeFilho = IO.Path.GetFileName(caminhoSub)
    '                        tipoFilho = IO.Path.GetExtension(caminhoSub).Replace(".", "").ToUpperInvariant()
    '                    Else
    '                        nomeFilho = kv.Key
    '                        tipoFilho = ""
    '                    End If

    '                    If subDoc IsNot Nothing AndAlso Not String.IsNullOrWhiteSpace(caminhoSub) Then
    '                        Ler(subDoc, caminhoSub, nivel + 1, qEfetivaFilho)

    '                        ' Fecha se abrimos temporariamente
    '                        Try
    '                            Dim isActive As Boolean = False
    '                            Try
    '                                isActive = (TryCast(subDoc, Object) Is app.ActiveDocument)
    '                            Catch
    '                                isActive = False
    '                            End Try

    '                            If Not isActive AndAlso IO.File.Exists(caminhoSub) Then
    '                                Try
    '                                    CallByName(subDoc, "Close", CallType.Method, False)
    '                                Catch
    '                                End Try
    '                            End If
    '                        Catch ex As Exception
    '                            RegistrarErro($"Fechando subdocumento '{caminhoSub}'", ex)
    '                        End Try

    '                    Else
    '                        ' Sem doc carregado, mas queremos listar
    '                        AdicionarPeca(nivel + 1,
    '                                      nomeFilho,
    '                                      tipoFilho,
    '                                      qEfetivaFilho,
    '                                      caminhoSub,
    '                                      subDoc)
    '                    End If

    '                Next

    '            Catch ex As Exception
    '                RegistrarErro($"Ler('{caminhoArquivo}')", ex)
    '            End Try
    '        End Sub

    '        '============================================================
    '        ' 🔹 Chamada inicial da recursão
    '        '============================================================
    '        Ler(doc, doc.FullName, 0, 1)

    '        '============================================================
    '        ' 🔹 Limpeza de peças “fantasmas”
    '        '============================================================
    '        For i As Integer = tabela.Rows.Count - 1 To 0 Step -1
    '            Dim arq As String = Convert.ToString(tabela.Rows(i)("Arquivo"))
    '            If String.IsNullOrWhiteSpace(arq) AndAlso
    '           String.Equals(Convert.ToString(tabela.Rows(i)("TipoLinha")),
    '                         "PECA",
    '                         StringComparison.OrdinalIgnoreCase) Then
    '                tabela.Rows.RemoveAt(i)
    '            End If
    '        Next

    '        '============================================================
    '        ' 🔹 Cálculo dos TOTAIS (apenas peças)
    '        '============================================================
    '        Dim totalQtdSolicitada As Double = 0
    '        Dim arquivosUnicos As New HashSet(Of String)(StringComparer.OrdinalIgnoreCase)

    '        For Each row As DataRow In tabela.Rows
    '            If Not String.Equals(Convert.ToString(row("TipoLinha")), "PECA", StringComparison.OrdinalIgnoreCase) Then
    '                Continue For
    '            End If

    '            Dim qtd As Double
    '            Double.TryParse(
    '            Convert.ToString(row("QtdeTotal")).Replace(",", "."),
    '            NumberStyles.Any,
    '            CultureInfo.InvariantCulture,
    '            qtd)
    '            totalQtdSolicitada += qtd

    '            Dim arq As String = Convert.ToString(row("Arquivo"))
    '            If Not String.IsNullOrWhiteSpace(arq) Then
    '                arquivosUnicos.Add(arq)
    '            End If
    '        Next

    '        Dim totalPecasDiferentes As Integer = arquivosUnicos.Count
    '        Dim totalPecas As Integer =
    '        tabela.Rows.Cast(Of DataRow)().
    '               Count(Function(r) String.Equals(Convert.ToString(r("TipoLinha")),
    '                                               "PECA",
    '                                               StringComparison.OrdinalIgnoreCase))

    '        '============================================================
    '        ' 🔹 Bind no grid
    '        '============================================================
    '        dgv.DataSource = tabela
    '        dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells
    '        dgv.AllowUserToAddRows = False
    '        dgv.ColumnHeadersDefaultCellStyle.Font = New Font(dgv.Font, FontStyle.Bold)

    '        If dgv.Columns.Contains("Arquivo") Then
    '            dgv.Columns("Arquivo").Frozen = True
    '        End If

    '        tabelaOriginalBOM = tabela.Copy()

    '        Dim ocultar() As String = {
    '        "TipoArquivo", "CaminhoCompleto", "Categoria", "Gerente",
    '        "Autor", "Empresa", "Data", "DataR", "Data1",
    '        "Revision", "CutSizeX", "CutSizeY", "Mass",
    '        "Área_de_superfície", "Area_de_superficie", "Bloqueado",
    '        "RNC", "PesoTotal", "AreaPinturaTotal",
    '        "QtdeMaterial", "PesoMaterial", "CodMatFabricante", "DescDetal"
    '    }

    '        For Each c In ocultar
    '            If dgv.Columns.Contains(c) Then
    '                dgv.Columns(c).Visible = False
    '            End If
    '        Next

    '        '============================================================
    '        ' 🔹 UI - final
    '        '============================================================
    '        pgb.Style = ProgressBarStyle.Continuous
    '        pgb.Value = 100

    '        lblResumo.Text =
    '        "✅ Estrutura concluída." & vbCrLf &
    '        $"• Total de peças solicitadas (QtdeTotal): {totalQtdSolicitada}" & vbCrLf &
    '        $"• Total de peças diferentes (arquivos únicos): {totalPecasDiferentes}" & vbCrLf &
    '        $"• Total de peças na estrutura (linhas de peça): {totalPecas}"

    '        Threading.Thread.Sleep(550)

    '    Catch ex As Exception
    '        RegistrarErro("Erro geral em ListarEstruturaBomMateriais", ex)

    '    Finally
    '        pgb.Visible = False

    '        If erros IsNot Nothing AndAlso erros.Count > 0 Then
    '            Dim sb As New StringBuilder()
    '            sb.AppendLine("Foram encontrados " & erros.Count & " erro(s) durante a leitura da estrutura:")
    '            sb.AppendLine()

    '            Dim i As Integer = 1
    '            For Each msg In erros
    '                sb.AppendLine(i.ToString("00") & " - " & msg)
    '                i += 1
    '            Next

    '            If qtdRpcDesconectado > 0 Then
    '                sb.AppendLine()
    '                sb.AppendLine(qtdRpcDesconectado.ToString() &
    '                          " erro(s) são do tipo RPC_E_DISCONNECTED (necessário revisar/ajustar o arquivo no Solid Edge).")
    '            End If

    '            MessageBox.Show(sb.ToString(),
    '                        "Relatório de erros - Estrutura",
    '                        MessageBoxButtons.OK,
    '                        MessageBoxIcon.Warning)
    '        End If

    '    End Try

    'End Sub



    ''===============================================================================
    '' Lê os materiais de TabelaViewMontaPeca para uma PEÇA
    '' e adiciona linhas do tipo MATERIAL na mesma DataTable da BOM
    ''===============================================================================
    'Public Sub LerDadosViewMontaPecaParaTabela(tabelaDest As DataTable,
    '                                       nivelPeca As Integer,
    '                                       nomeArquivoComExt As String,
    '                                       qtdeEfetivaPeca As Integer)

    '    If TabelaViewMontaPeca Is Nothing Then Exit Sub

    '    ' 👉 Usa o nome COM extensão, igual está em NomeArquivoSemExtensao na view
    '    Dim chaveArquivo As String = IO.Path.GetFileName(nomeArquivoComExt)

    '    DadosArquivoCorrente.NomeArquivoSemExtensao = chaveArquivo
    '    DadosArquivoCorrente.qtde = qtdeEfetivaPeca

    '    ' Case-insensitive
    '    TabelaViewMontaPeca.CaseSensitive = False

    '    Dim dv As New DataView(TabelaViewMontaPeca)
    '    Dim filtro As String = chaveArquivo.Replace("'", "''")
    '    dv.RowFilter = $"NomeArquivoSemExtensao = '{filtro}'"

    '    Dim dtFiltrado As DataTable = dv.ToTable()

    '    For Each linha As DataRow In dtFiltrado.Rows
    '        Try
    '            ' Confirma novamente ignorando maiúsc/minúsc (por segurança)
    '            If Not String.Equals(
    '            linha("NomeArquivoSemExtensao").ToString().Trim(),
    '            chaveArquivo.Trim(),
    '            StringComparison.OrdinalIgnoreCase) Then

    '                Continue For
    '            End If

    '            iconeTipoArquivo = My.Resources.sinco_Diversos

    '            ' ============================================
    '            ' 1) Quantidades base
    '            '    PecaQtde  -> qtde de material POR PEÇA
    '            '    qtdeEfetivaPeca -> qtde da peça pai (fallback)
    '            ' ============================================
    '            Dim PecaQtde As Double
    '            Dim peso As Double

    '            Dim pecaQtdeStr As String
    '            Try
    '                pecaQtdeStr = linha.Item("PecaQtde").ToString().Trim().Replace(",", ".")
    '            Catch
    '                pecaQtdeStr = "0"
    '            End Try
    '            PecaQtde = cl_BancoDados.converteStringParaDouble(pecaQtdeStr)

    '            Dim pecaPesoStr As String
    '            Try
    '                pecaPesoStr = linha.Item("Peso").ToString().Trim().Replace(",", ".")
    '            Catch
    '                pecaPesoStr = "0"
    '            End Try
    '            peso = cl_BancoDados.converteStringParaDouble(pecaPesoStr)

    '            ' qtde da peça pai: tenta pegar da própria tabela (linha PECA);
    '            ' se não achar, usa qtdeEfetivaPeca recebido
    '            Dim qtdPecaPai As Double = qtdeEfetivaPeca

    '            Try
    '                Dim filtroPai As String =
    '                "TipoLinha = 'PECA' AND Arquivo = '" &
    '                chaveArquivo.Replace("'", "''") &
    '                "' AND Nivel = " &
    '                nivelPeca.ToString()

    '                Dim rowsPeca() As DataRow = tabelaDest.Select(filtroPai)

    '                If rowsPeca IsNot Nothing AndAlso rowsPeca.Length > 0 Then
    '                    Dim tmp As Double
    '                    If Double.TryParse(
    '                    Convert.ToString(rowsPeca(rowsPeca.Length - 1)("Qtde")),
    '                    NumberStyles.Any,
    '                    CultureInfo.InvariantCulture,
    '                    tmp) Then

    '                        qtdPecaPai = tmp
    '                    End If
    '                End If
    '            Catch
    '                ' se der erro, continua com qtdeEfetivaPeca
    '            End Try

    '            ' fator (por enquanto 1, mas já separado)
    '            Dim fator As Double = 1.0

    '            ' ============================================
    '            ' 2) Cálculos conforme a REGRA:
    '            '    QtdeMaterial  -> qtde de material POR PEÇA (da view)
    '            '    Qtde          -> mesma QtdeMaterial (por peça)
    '            '    QtdeTotal     -> Qtde * QtdePecaPai * fator
    '            ' ============================================
    '            Dim QtdeMaterial As Double = PecaQtde                        ' por peça
    '            Dim QtdeVisivel As Double = QtdeMaterial                     ' coluna Qtde
    '            Dim QtdeTotalMaterial As Double = QtdeMaterial * qtdPecaPai * fator

    '            ' Peso total de material (opcional, interno)
    '            Dim PesoMaterial As Double
    '            Try
    '                PesoMaterial = CDbl(peso) * QtdeTotalMaterial
    '            Catch
    '                PesoMaterial = 0
    '            End Try

    '            ' ============================================
    '            ' 3) Monta linha de MATERIAL na tabela
    '            ' ============================================
    '            Dim drMat As DataRow = tabelaDest.NewRow()

    '            drMat("TipoLinha") = "MATERIAL"
    '            drMat("Nivel") = nivelPeca + 1

    '            ' Arquivo → mostra o CodMatFabricante
    '            Dim cod As String = linha.Item("CodMatFabricante").ToString().Trim().ToUpper()
    '            drMat("Arquivo") = cod
    '            drMat("CodMatFabricante") = cod

    '            drMat("TipoArquivo") = "MAT"

    '            ' 🔹 Qtde (VISÍVEL) = Qtde de material POR PEÇA
    '            drMat("Qtde") = QtdeVisivel

    '            ' 🔹 QtdeMaterial (INTERNA) = mesma coisa, se quiser usar depois
    '            drMat("QtdeMaterial") = QtdeMaterial

    '            drMat("CaminhoCompleto") = ""

    '            ' Título = DescDetal + Material
    '            Dim desc As String = linha.Item("DescDetal").ToString().Trim().ToUpper()
    '            Dim matView As String = ""
    '            Try
    '                matView = linha.Item("Material").ToString().Trim().ToUpper()
    '            Catch
    '                matView = ""
    '            End Try

    '            Dim titulo As String = desc
    '            If matView <> "" Then
    '                titulo &= " - " & matView
    '            End If

    '            drMat("Título") = titulo

    '            ' Também preenche Material, se existir
    '            Try
    '                If matView <> "" Then drMat("Material") = matView
    '            Catch
    '            End Try

    '            ' Tipo de Desenho = MATERIAL
    '            Try
    '                drMat("Tipo de Desenho") = "MATERIAL"
    '            Catch
    '            End Try

    '            ' Campos de controle
    '            drMat("RNC") = ""
    '            drMat("Fator") = fator

    '            ' 🔹 QtdeTotal = Qtde (por peça) * Qtde da peça pai * fator
    '            drMat("QtdeTotal") = QtdeTotalMaterial

    '            drMat("PesoTotal") = 0
    '            drMat("AreaPinturaTotal") = 0

    '            ' Mantém PesoMaterial guardado (coluna oculta)
    '            drMat("PesoMaterial") = PesoMaterial

    '            tabelaDest.Rows.Add(drMat)

    '        Catch
    '            Continue For
    '        End Try
    '    Next

    'End Sub



#End Region


#Region "VERSÃO NOVA - 04-02-2026 rev 00"


    ''================================================================================
    '' Monta estrutura BOM + MATERIAIS no MESMO DataGridView
    ''================================================================================
    'Public Sub ListarEstruturaBomMateriais(ByVal dgv As DataGridView,
    '                                   ByVal pgb As ProgressBar,
    '                                   ByVal lbl As Label,
    '                                   ByVal lblResumo As Label)

    '    ' Lista que acumula erros
    '    Dim erros As New List(Of String)
    '    Dim qtdRpcDesconectado As Integer = 0

    '    ' Helper para registrar erros com contexto
    '    Dim RegistrarErro As Action(Of String, Exception) =
    'Sub(contexto As String, ex As Exception)
    '    Try
    '        If ex Is Nothing Then Return

    '        Dim msg As String
    '        Dim comEx = TryCast(ex, System.Runtime.InteropServices.COMException)

    '        If comEx IsNot Nothing AndAlso comEx.ErrorCode = &H80010108 Then
    '            qtdRpcDesconectado += 1
    '            msg = $"{contexto}: [RPC_E_DISCONNECTED] O objeto do Solid Edge foi desconectado. Detalhe: {ex.Message}"
    '        Else
    '            msg = $"{contexto}: {ex.Message}"
    '        End If

    '        erros.Add(msg)
    '    Catch
    '        ' não deixa o log quebrar o fluxo
    '    End Try
    'End Sub

    '    '  Try
    '    Try
    '            '============================================================
    '            ' 🔹 Garante conexão com o Solid Edge
    '            '============================================================
    '            If app Is Nothing Then
    '                app = ConectarSolidEdge()
    '            End If

    '            ' Verifica se há documentos abertos com proteção
    '            Dim doc As Object = Nothing
    '            Try
    '                If app Is Nothing OrElse app.Documents.Count = 0 Then
    '                    MessageBox.Show("Nenhum documento aberto no Solid Edge.",
    '                                    "SINCO - Solid Edge",
    '                                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
    '                    Exit Sub
    '                End If
    '                doc = app.ActiveDocument
    '                If doc Is Nothing Then
    '                    MessageBox.Show("Não foi possível obter o documento ativo.",
    '                                    "SINCO - Solid Edge",
    '                                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
    '                    Exit Sub
    '                End If
    '            Catch ex As Exception
    '                MessageBox.Show("Erro ao acessar o Solid Edge: " & ex.Message,
    '                                "Erro de conexão",
    '                                MessageBoxButtons.OK, MessageBoxIcon.Error)
    '                Exit Sub
    '            End Try

    '            ' Dim doc As Object = app.ActiveDocument
    '            If doc Is Nothing Then Exit Sub

    '        '============================================================
    '        ' 🔹 UI - estado inicial
    '        '============================================================
    '        pgb.Visible = True
    '        pgb.Style = ProgressBarStyle.Marquee
    '        pgb.Value = 0
    '        lbl.Visible = True
    '        lbl.Text = "Lendo estrutura... aguarde."

    '        '============================================================
    '        ' 🔹 Tabela destino
    '        '============================================================
    '        Dim tabela As New DataTable("BOM_Completa")

    '        ' Colunas fixas
    '        tabela.Columns.Add("Nivel", GetType(Integer))
    '        tabela.Columns.Add("Arquivo", GetType(String))          ' com extensão
    '        tabela.Columns.Add("TipoArquivo", GetType(String))      ' PAR/PSM/ASM...
    '        tabela.Columns.Add("Qtde", GetType(Double))             ' quantidade efetiva no Nivel
    '        tabela.Columns.Add("CaminhoCompleto", GetType(String))

    '        ' Tipo de linha (PECA / MATERIAL)
    '        tabela.Columns.Add("TipoLinha", GetType(String))

    '        ' Propriedades de interesse
    '        Dim propsInteressantes As New List(Of String) From {
    '        "Título", "Assunto", "Coment", "Tipo de Desenho", "Palavras-", "Autor", "Empresa",
    '        "Categoria", "Gerente", "Material", "Data", "DataR", "Data1",
    '        "Revision", "CutSizeX", "CutSizeY", "Thickness",
    '        "Mass", "Área_de_superfície", "Area_de_superficie", "Bloqueado"
    '    }
    '        For Each p In propsInteressantes
    '            If Not tabela.Columns.Contains(p) Then
    '                tabela.Columns.Add(p, GetType(String))
    '            End If
    '        Next

    '        ' Colunas de totais de peça
    '        tabela.Columns.Add("RNC", GetType(String))
    '        tabela.Columns.Add("Fator", GetType(Double))
    '        tabela.Columns.Add("QtdeTotal", GetType(Double))
    '        tabela.Columns.Add("PesoTotal", GetType(Double))
    '        tabela.Columns.Add("AreaPinturaTotal", GetType(Double))

    '        ' Colunas específicas para MATERIAL
    '        tabela.Columns.Add("CodMatFabricante", GetType(String))
    '        tabela.Columns.Add("DescDetal", GetType(String))
    '        tabela.Columns.Add("QtdeMaterial", GetType(Double))
    '        tabela.Columns.Add("PesoMaterial", GetType(Double))

    '        Dim totalLidos As Integer = 0
    '        Dim totalEstimado As Integer = 1

    '        '============================================================
    '        ' 🔹 Helpers
    '        '============================================================
    '        Dim LimparUnidades As Func(Of String, String) =
    '        Function(txt As String) As String
    '            If String.IsNullOrWhiteSpace(txt) Then Return ""
    '            Return txt _
    '                .Replace("mm²", "") _
    '                .Replace("MM²", "") _
    '                .Replace("mm", "") _
    '                .Replace("MM", "") _
    '                .Replace("^2", "") _
    '                .Replace("KG", "") _
    '                .Replace("kg", "") _
    '                .Trim()
    '        End Function

    '        Dim FormatarValor As Func(Of String, String, String) =
    '        Function(propNome As String, valorBruto As String) As String
    '            If String.IsNullOrWhiteSpace(valorBruto) Then Return ""
    '            Dim nome = propNome.Trim().ToUpperInvariant()
    '            Dim valor = LimparUnidades(valorBruto)

    '            ' Datas
    '            If nome = "DATA" OrElse nome = "DATAR" OrElse nome = "DATA1" Then
    '                Try
    '                    Return cl_BancoDados.FormatarData(valor)
    '                Catch
    '                    Return valor
    '                End Try
    '            End If

    '            ' Revisão
    '            If nome.Contains("REVISION") Then Return valor.ToUpperInvariant()

    '            ' CutSize / Espessura / Massa -> ponto decimal
    '            If nome.Contains("CUTSIZEX") Then Return valor.Replace(",", ".")
    '            If nome.Contains("CUTSIZEY") Then Return valor.Replace(",", ".")
    '            If nome.Contains("THICKNESS") Then Return valor.Replace(",", ".")
    '            If nome.Contains("MASS") Then Return valor.Replace(",", ".")

    '            ' Área mm² -> m²
    '            If nome.Contains("ÁREA") OrElse nome.Contains("AREA") Then
    '                Dim num As Double
    '                If Double.TryParse(valor.Replace(",", "."),
    '                                   NumberStyles.Any,
    '                                   CultureInfo.InvariantCulture,
    '                                   num) Then
    '                    Dim m2 = num / 1_000_000.0
    '                    Return m2.ToString("N4", CultureInfo.GetCultureInfo("pt-BR"))
    '                End If
    '            End If

    '            Return valor
    '        End Function

    '        '------------------------------------------------------------
    '        ' PreencherPropriedades
    '        '------------------------------------------------------------
    '        Dim PreencherPropriedades As Action(Of Object, DataRow) =
    '        Sub(docObj As Object, linha As DataRow)
    '            If linha Is Nothing Then Exit Sub

    '            Dim tentativas As Integer = 0
    '            Dim abriuTemp As Boolean = False

    '            While tentativas < 2
    '                Try
    '                    ' Reabrir se necessário
    '                    If docObj Is Nothing Then
    '                        Dim caminhoArquivo As String = ""
    '                        Try
    '                            caminhoArquivo = Convert.ToString(linha("CaminhoCompleto"))
    '                        Catch
    '                            caminhoArquivo = ""
    '                        End Try

    '                        If Not String.IsNullOrWhiteSpace(caminhoArquivo) AndAlso IO.File.Exists(caminhoArquivo) Then
    '                            Try
    '                                docObj = app.Documents.Open(caminhoArquivo)
    '                                abriuTemp = True
    '                            Catch exOpen As Exception
    '                                RegistrarErro($"Reabrindo documento '{caminhoArquivo}'", exOpen)
    '                                Exit While
    '                            End Try
    '                        Else
    '                            Exit While
    '                        End If
    '                    End If

    '                    Dim propSets As Object = docObj.Properties
    '                    For Each propSet As Object In propSets
    '                        For Each prop As Object In propSet
    '                            If prop Is Nothing Then Continue For

    '                            Dim nomeProp As String = ""
    '                            Try
    '                                nomeProp = CStr(prop.Name)
    '                            Catch
    '                                Continue For
    '                            End Try
    '                            If String.IsNullOrWhiteSpace(nomeProp) Then Continue For

    '                            ' Propriedades de interesse
    '                            For Each alvo In propsInteressantes
    '                                If nomeProp.IndexOf(alvo, StringComparison.OrdinalIgnoreCase) >= 0 Then
    '                                    Dim valorBruto As String = ""
    '                                    Try
    '                                        Dim v = prop.Value
    '                                        If v IsNot Nothing Then valorBruto = v.ToString()
    '                                    Catch ex As Exception
    '                                        valorBruto = "(erro)"
    '                                        RegistrarErro($"Lendo propriedade '{nomeProp}'", ex)
    '                                    End Try
    '                                    linha(alvo) = FormatarValor(nomeProp, valorBruto)
    '                                    Exit For
    '                                End If
    '                            Next

    '                            ' Espessura → Thickness
    '                            If nomeProp.IndexOf("Material Thickness", StringComparison.OrdinalIgnoreCase) >= 0 _
    '                               OrElse nomeProp.IndexOf("Espessura Do Material", StringComparison.OrdinalIgnoreCase) >= 0 _
    '                               OrElse nomeProp.Equals("Thickness", StringComparison.OrdinalIgnoreCase) Then

    '                                Dim valorBruto As String = ""
    '                                Try
    '                                    Dim v = prop.Value
    '                                    If v IsNot Nothing Then valorBruto = v.ToString()
    '                                Catch ex As Exception
    '                                    RegistrarErro($"Lendo espessura em '{nomeProp}'", ex)
    '                                End Try

    '                                If Not String.IsNullOrWhiteSpace(valorBruto) Then
    '                                    Dim limpo = LimparUnidades(valorBruto).Replace(",", ".")
    '                                    linha("Thickness") = limpo
    '                                End If
    '                            End If

    '                        Next
    '                    Next

    '                    Exit While ' sucesso

    '                Catch comEx As System.Runtime.InteropServices.COMException
    '                    If comEx.ErrorCode = &H80010108 Then
    '                        tentativas += 1
    '                        RegistrarErro("PreencherPropriedades - RPC_E_DISCONNECTED, tentando reabrir documento", comEx)
    '                        docObj = Nothing
    '                        Threading.Thread.Sleep(200)
    '                        Continue While
    '                    Else
    '                        RegistrarErro("PreencherPropriedades", comEx)
    '                        Exit While
    '                    End If

    '                Catch ex As Exception
    '                    RegistrarErro("PreencherPropriedades", ex)
    '                    Exit While
    '                End Try
    '            End While

    '            ' Fecha doc aberto temporariamente
    '            If abriuTemp AndAlso docObj IsNot Nothing Then
    '                Try
    '                    Dim isActive As Boolean = False
    '                    Try
    '                        isActive = (TryCast(docObj, Object) Is app.ActiveDocument)
    '                    Catch
    '                        isActive = False
    '                    End Try

    '                    If Not isActive Then
    '                        Try
    '                            CallByName(docObj, "Close", CallType.Method, False)
    '                        Catch ex As Exception
    '                            RegistrarErro("Fechando documento aberto em PreencherPropriedades", ex)
    '                        End Try
    '                    End If
    '                Catch
    '                    ' ignora
    '                End Try
    '            End If
    '        End Sub

    '        '------------------------------------------------------------
    '        ' Adiciona linha de PEÇA + materiais abaixo
    '        '------------------------------------------------------------
    '        Dim AdicionarPeca As Action(Of Integer, String, String, Integer, String, Object) =
    '        Sub(nivel As Integer,
    '            nomeComExt As String,
    '            tipoArquivo As String,
    '            qtdeEfetiva As Integer,
    '            caminho As String,
    '            docObj As Object)

    '            Try
    '                Dim linha = tabela.NewRow()

    '                linha("TipoLinha") = "PECA"
    '                linha("Nivel") = nivel
    '                linha("Arquivo") = nomeComExt
    '                linha("TipoArquivo") = tipoArquivo
    '                linha("Qtde") = CDbl(qtdeEfetiva)
    '                linha("CaminhoCompleto") = caminho

    '                For Each p In propsInteressantes
    '                    linha(p) = ""
    '                Next

    '                linha("RNC") = ""
    '                linha("Fator") = 1.0
    '                linha("QtdeTotal") = CDbl(qtdeEfetiva)
    '                linha("PesoTotal") = 0.0
    '                linha("AreaPinturaTotal") = 0.0

    '                ' Campos de material vazios na linha de peça
    '                linha("CodMatFabricante") = ""
    '                linha("DescDetal") = ""
    '                linha("QtdeMaterial") = 0.0
    '                linha("PesoMaterial") = 0.0

    '                ' Propriedades do arquivo
    '                PreencherPropriedades(docObj, linha)

    '                Dim mass As Double = 0
    '                Double.TryParse(
    '                    Convert.ToString(linha("Mass")).Replace(",", "."),
    '                    NumberStyles.Any,
    '                    CultureInfo.InvariantCulture,
    '                    mass)

    '                Dim area As Double = 0
    '                Double.TryParse(
    '                    Convert.ToString(linha("Área_de_superfície")).Replace(",", "."),
    '                    NumberStyles.Any,
    '                    CultureInfo.InvariantCulture,
    '                    area)

    '                linha("PesoTotal") = mass * qtdeEfetiva
    '                linha("AreaPinturaTotal") = area * qtdeEfetiva

    '                ' Adiciona a linha da peça
    '                tabela.Rows.Add(linha)

    '                ' Adiciona os materiais para essa peça
    '                Try
    '                    LerDadosViewMontaPecaParaTabela(tabela, nivel, nomeComExt, qtdeEfetiva)
    '                Catch exMat As Exception
    '                    RegistrarErro($"Materiais para '{nomeComExt}'", exMat)
    '                End Try

    '            Catch ex As Exception
    '                RegistrarErro($"AdicionarPeca [{nomeComExt}]", ex)
    '            End Try
    '        End Sub

    '        '------------------------------------------------------------
    '        ' Recursão (propaga multiplicador do pai)
    '        '------------------------------------------------------------
    '        Dim Ler As Action(Of Object, String, Integer, Integer) =
    '        Sub(docObj As Object, caminhoArquivo As String, nivel As Integer, multiplicadorPai As Integer)
    '            Try
    '                If docObj Is Nothing OrElse String.IsNullOrWhiteSpace(caminhoArquivo) Then Exit Sub

    '                totalLidos += 1
    '                lbl.Text = $"Lendo {IO.Path.GetFileName(caminhoArquivo)} ({totalLidos})"

    '                If totalEstimado > 1 Then
    '                    pgb.Style = ProgressBarStyle.Continuous
    '                    pgb.Value = Math.Min(100, CInt((totalLidos / Math.Max(1, totalEstimado)) * 100))
    '                End If

    '                Dim ext As String = IO.Path.GetExtension(caminhoArquivo).ToLower()
    '                Dim nomeComExt As String = IO.Path.GetFileName(caminhoArquivo)
    '                Dim tipoUpper As String = ext.Replace(".", "").ToUpperInvariant()

    '                ' Linha da própria peça
    '                AdicionarPeca(nivel, nomeComExt, tipoUpper, multiplicadorPai, caminhoArquivo, docObj)

    '                ' Apenas ASM tem filhos
    '                If ext <> ".asm" Then Exit Sub

    '                ' Ocorrências
    '                Dim occsObj As Object = Nothing
    '                Try
    '                    occsObj = CallByName(docObj, "Occurrences", CallType.Get)
    '                Catch ex As Exception
    '                    RegistrarErro($"Lendo Occurrences de {nomeComExt}", ex)
    '                    occsObj = Nothing
    '                End Try
    '                If occsObj Is Nothing Then Exit Sub

    '                Dim count As Integer = 0
    '                Try
    '                    count = CInt(CallByName(occsObj, "Count", CallType.Get))
    '                Catch ex As Exception
    '                    RegistrarErro($"Lendo Count de {nomeComExt}", ex)
    '                    count = 0
    '                End Try
    '                If count <= 0 Then Exit Sub

    '                totalEstimado += count

    '                ' Agrupa por arquivo dentro do mesmo pai
    '                Dim grupo As New Dictionary(Of String, (Caminho As String, QtdeLocal As Integer, DocObj As Object))(StringComparer.OrdinalIgnoreCase)

    '                For i As Integer = 1 To count
    '                    Try
    '                        Dim occ As Object = CallByName(occsObj, "Item", CallType.Method, i)

    '                        Dim subPath As String = ""
    '                        Try
    '                            subPath = CStr(CallByName(occ, "OccurrenceFileName", CallType.Get))
    '                        Catch ex As Exception
    '                            RegistrarErro($"Lendo OccurrenceFileName (#{i}) de {nomeComExt}", ex)
    '                        End Try

    '                        Dim subDoc As Object = Nothing
    '                        Try
    '                            subDoc = CallByName(occ, "OccurrenceDocument", CallType.Get)
    '                        Catch ex As Exception
    '                            RegistrarErro($"Lendo OccurrenceDocument (#{i}) de {nomeComExt}", ex)
    '                        End Try

    '                        If subDoc Is Nothing AndAlso Not String.IsNullOrWhiteSpace(subPath) AndAlso IO.File.Exists(subPath) Then
    '                            Try
    '                                subDoc = app.Documents.Open(subPath)
    '                            Catch ex As Exception
    '                                RegistrarErro($"Abrindo subdocumento '{subPath}'", ex)
    '                                subDoc = Nothing
    '                            End Try
    '                        End If

    '                        Dim nomeAgrup As String = If(String.IsNullOrWhiteSpace(subPath),
    '                                                     $"ITEM_{i}",
    '                                                     IO.Path.GetFileName(subPath)).ToUpperInvariant()

    '                        Dim qLocal As Integer = 1
    '                        Try
    '                            Dim qtdProp As Object = CallByName(occ, "Quantity", CallType.Get)
    '                            If qtdProp IsNot Nothing Then
    '                                Dim qi As Integer
    '                                If Integer.TryParse(qtdProp.ToString(), qi) AndAlso qi > 0 Then qLocal = qi
    '                            End If
    '                        Catch ex As Exception
    '                            RegistrarErro($"Lendo Quantity (#{i}) de {nomeComExt}", ex)
    '                        End Try

    '                        If grupo.ContainsKey(nomeAgrup) Then
    '                            Dim t = grupo(nomeAgrup)
    '                            grupo(nomeAgrup) = (t.Caminho,
    '                                                t.QtdeLocal + qLocal,
    '                                                If(t.DocObj Is Nothing, subDoc, t.DocObj))
    '                        Else
    '                            grupo.Add(nomeAgrup,
    '                                      (If(String.IsNullOrWhiteSpace(subPath), "", subPath),
    '                                       qLocal,
    '                                       subDoc))
    '                        End If
    '                    Catch ex As Exception
    '                        RegistrarErro($"Processando ocorrência (#{i}) de {nomeComExt}", ex)
    '                    End Try
    '                Next

    '                For Each kv In grupo
    '                    Dim caminhoSub = kv.Value.Caminho
    '                    Dim qLocal = kv.Value.QtdeLocal
    '                    Dim qEfetivaFilho As Integer = qLocal * multiplicadorPai
    '                    Dim subDoc = kv.Value.DocObj

    '                    If String.IsNullOrWhiteSpace(caminhoSub) AndAlso subDoc IsNot Nothing Then
    '                        Try
    '                            caminhoSub = CStr(CallByName(subDoc, "FullName", CallType.Get))
    '                        Catch ex As Exception
    '                            RegistrarErro("Lendo FullName de subdocumento", ex)
    '                            caminhoSub = ""
    '                        End Try
    '                    End If

    '                    Dim nomeFilho As String
    '                    Dim tipoFilho As String

    '                    If Not String.IsNullOrWhiteSpace(caminhoSub) Then
    '                        nomeFilho = IO.Path.GetFileName(caminhoSub)
    '                        tipoFilho = IO.Path.GetExtension(caminhoSub).Replace(".", "").ToUpperInvariant()
    '                    Else
    '                        nomeFilho = kv.Key
    '                        tipoFilho = ""
    '                    End If

    '                    If subDoc IsNot Nothing AndAlso Not String.IsNullOrWhiteSpace(caminhoSub) Then
    '                        Ler(subDoc, caminhoSub, nivel + 1, qEfetivaFilho)

    '                        ' Fecha se abrimos temporariamente
    '                        Try
    '                            Dim isActive As Boolean = False
    '                            Try
    '                                isActive = (TryCast(subDoc, Object) Is app.ActiveDocument)
    '                            Catch
    '                                isActive = False
    '                            End Try

    '                            If Not isActive AndAlso IO.File.Exists(caminhoSub) Then
    '                                Try
    '                                    CallByName(subDoc, "Close", CallType.Method, False)
    '                                Catch
    '                                End Try
    '                            End If
    '                        Catch ex As Exception
    '                            RegistrarErro($"Fechando subdocumento '{caminhoSub}'", ex)
    '                        End Try

    '                    Else
    '                        ' Sem doc carregado, mas queremos listar
    '                        AdicionarPeca(nivel + 1,
    '                                      nomeFilho,
    '                                      tipoFilho,
    '                                      qEfetivaFilho,
    '                                      caminhoSub,
    '                                      subDoc)
    '                    End If

    '                Next

    '            Catch ex As Exception
    '                RegistrarErro($"Ler('{caminhoArquivo}')", ex)
    '            End Try
    '        End Sub

    '        '============================================================
    '        ' 🔹 Chamada inicial da recursão
    '        '============================================================
    '        Ler(doc, doc.FullName, 0, 1)

    '        '============================================================
    '        ' 🔹 Limpeza de peças “fantasmas”
    '        '============================================================
    '        For i As Integer = tabela.Rows.Count - 1 To 0 Step -1
    '            Dim arq As String = Convert.ToString(tabela.Rows(i)("Arquivo"))
    '            If String.IsNullOrWhiteSpace(arq) AndAlso
    '           String.Equals(Convert.ToString(tabela.Rows(i)("TipoLinha")),
    '                         "PECA",
    '                         StringComparison.OrdinalIgnoreCase) Then
    '                tabela.Rows.RemoveAt(i)
    '            End If
    '        Next

    '        '============================================================
    '        ' 🔹 Cálculo dos TOTAIS (apenas peças)
    '        '============================================================
    '        Dim totalQtdSolicitada As Double = 0
    '        Dim arquivosUnicos As New HashSet(Of String)(StringComparer.OrdinalIgnoreCase)

    '        For Each row As DataRow In tabela.Rows
    '            If Not String.Equals(Convert.ToString(row("TipoLinha")), "PECA", StringComparison.OrdinalIgnoreCase) Then
    '                Continue For
    '            End If

    '            Dim qtd As Double
    '            Double.TryParse(
    '            Convert.ToString(row("QtdeTotal")).Replace(",", "."),
    '            NumberStyles.Any,
    '            CultureInfo.InvariantCulture,
    '            qtd)
    '            totalQtdSolicitada += qtd

    '            Dim arq As String = Convert.ToString(row("Arquivo"))
    '            If Not String.IsNullOrWhiteSpace(arq) Then
    '                arquivosUnicos.Add(arq)
    '            End If
    '        Next

    '        Dim totalPecasDiferentes As Integer = arquivosUnicos.Count
    '        Dim totalPecas As Integer =
    '        tabela.Rows.Cast(Of DataRow)().
    '               Count(Function(r) String.Equals(Convert.ToString(r("TipoLinha")),
    '                                               "PECA",
    '                                               StringComparison.OrdinalIgnoreCase))

    '        '============================================================
    '        ' 🔹 Bind no grid
    '        '============================================================
    '        dgv.DataSource = tabela
    '        dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells
    '        dgv.AllowUserToAddRows = False
    '        dgv.ColumnHeadersDefaultCellStyle.Font = New Font(dgv.Font, FontStyle.Bold)

    '        If dgv.Columns.Contains("Arquivo") Then
    '            dgv.Columns("Arquivo").Frozen = True
    '        End If

    '        tabelaOriginalBOM = tabela.Copy()

    '        Dim ocultar() As String = {
    '        "TipoArquivo", "CaminhoCompleto", "Categoria", "Gerente",
    '        "Autor", "Empresa", "Data", "DataR", "Data1",
    '        "Revision", "CutSizeX", "CutSizeY", "Mass",
    '        "Área_de_superfície", "Area_de_superficie", "Bloqueado",
    '        "RNC", "PesoTotal", "AreaPinturaTotal",
    '        "QtdeMaterial", "PesoMaterial", "CodMatFabricante", "DescDetal"
    '    }

    '        For Each c In ocultar
    '            If dgv.Columns.Contains(c) Then
    '                dgv.Columns(c).Visible = False
    '            End If
    '        Next

    '        '============================================================
    '        ' 🔹 UI - final
    '        '============================================================
    '        pgb.Style = ProgressBarStyle.Continuous
    '        pgb.Value = 100

    '        lblResumo.Text =
    '        "✅ Estrutura concluída." & vbCrLf &
    '        $"• Total de peças solicitadas (QtdeTotal): {totalQtdSolicitada}" & vbCrLf &
    '        $"• Total de peças diferentes (arquivos únicos): {totalPecasDiferentes}" & vbCrLf &
    '        $"• Total de peças na estrutura (linhas de peça): {totalPecas}"

    '        Threading.Thread.Sleep(550)

    '    Catch ex As Exception
    '        RegistrarErro("Erro geral em ListarEstruturaBomMateriais", ex)

    '    Finally
    '        pgb.Visible = False

    '        If erros IsNot Nothing AndAlso erros.Count > 0 Then
    '            Dim sb As New StringBuilder()
    '            sb.AppendLine("Foram encontrados " & erros.Count & " erro(s) durante a leitura da estrutura:")
    '            sb.AppendLine()

    '            Dim i As Integer = 1
    '            For Each msg In erros
    '                sb.AppendLine(i.ToString("00") & " - " & msg)
    '                i += 1
    '            Next

    '            If qtdRpcDesconectado > 0 Then
    '                sb.AppendLine()
    '                sb.AppendLine(qtdRpcDesconectado.ToString() &
    '                          " erro(s) são do tipo RPC_E_DISCONNECTED (necessário revisar/ajustar o arquivo no Solid Edge).")
    '            End If

    '            MessageBox.Show(sb.ToString(),
    '                        "Relatório de erros - Estrutura",
    '                        MessageBoxButtons.OK,
    '                        MessageBoxIcon.Warning)
    '        End If

    '    End Try

    'End Sub



    ''===============================================================================
    '' Lê os materiais de TabelaViewMontaPeca para uma PEÇA
    '' e adiciona linhas do tipo MATERIAL na mesma DataTable da BOM
    ''===============================================================================

    'Public Sub LerDadosViewMontaPecaParaTabela(tabelaDest As DataTable,
    '                                       nivelPeca As Integer,
    '                                       nomeArquivoComExt As String,
    '                                       qtdeEfetivaPeca As Integer)

    '    If TabelaViewMontaPeca Is Nothing Then Exit Sub

    '    Dim chaveArquivo As String = IO.Path.GetFileName(nomeArquivoComExt)

    '    DadosArquivoCorrente.NomeArquivoSemExtensao = chaveArquivo
    '    DadosArquivoCorrente.qtde = qtdeEfetivaPeca

    '    TabelaViewMontaPeca.CaseSensitive = False

    '    Dim dv As New DataView(TabelaViewMontaPeca)
    '    Dim filtro As String = chaveArquivo.Replace("'", "''")
    '    dv.RowFilter = $"NomeArquivoSemExtensao = '{filtro}'"

    '    Dim dtFiltrado As DataTable = dv.ToTable()

    '    For Each linha As DataRow In dtFiltrado.Rows
    '        Try
    '            If Not String.Equals(
    '            linha("NomeArquivoSemExtensao").ToString().Trim(),
    '            chaveArquivo.Trim(),
    '            StringComparison.OrdinalIgnoreCase) Then

    '                Continue For
    '            End If

    '            iconeTipoArquivo = My.Resources.sinco_Diversos

    '            ' 1) Quantidades base
    '            Dim PecaQtde As Double
    '            Dim peso As Double

    '            Dim pecaQtdeStr As String = linha.Item("PecaQtde").ToString().Trim().Replace(",", ".")
    '            PecaQtde = cl_BancoDados.converteStringParaDouble(pecaQtdeStr)

    '            Dim pecaPesoStr As String = linha.Item("Peso").ToString().Trim().Replace(",", ".")
    '            peso = cl_BancoDados.converteStringParaDouble(pecaPesoStr)

    '            ' Usa diretamente a qtdeEfetivaPeca passada como parâmetro
    '            Dim qtdPecaPai As Double = qtdeEfetivaPeca

    '            Dim fator As Double = 1.0

    '            ' 2) Cálculo da quantidade total de material
    '            Dim QtdeMaterial As Double = PecaQtde
    '            Dim QtdeVisivel As Double = QtdeMaterial
    '            Dim QtdeTotalMaterial As Double = QtdeMaterial * qtdPecaPai * fator

    '            Dim PesoMaterial As Double = 0
    '            Try
    '                PesoMaterial = CDbl(peso) * QtdeTotalMaterial
    '            Catch
    '            End Try

    '            ' 3) Monta linha de MATERIAL na tabela
    '            Dim drMat As DataRow = tabelaDest.NewRow()

    '            drMat("TipoLinha") = "MATERIAL"
    '            drMat("Nivel") = nivelPeca + 1

    '            Dim cod As String = linha.Item("CodMatFabricante").ToString().Trim().ToUpper()
    '            drMat("Arquivo") = cod
    '            drMat("CodMatFabricante") = cod

    '            drMat("TipoArquivo") = "MAT"
    '            drMat("Qtde") = QtdeVisivel
    '            drMat("QtdeMaterial") = QtdeMaterial
    '            drMat("CaminhoCompleto") = ""

    '            Dim desc As String = linha.Item("DescDetal").ToString().Trim().ToUpper()
    '            Dim matView As String = ""
    '            Try
    '                matView = linha.Item("Material").ToString().Trim().ToUpper()
    '            Catch
    '            End Try

    '            Dim titulo As String = desc
    '            If matView <> "" Then titulo &= " - " & matView
    '            drMat("Título") = titulo

    '            If matView <> "" Then drMat("Material") = matView
    '            drMat("Tipo de Desenho") = "MATERIAL"

    '            drMat("RNC") = ""
    '            drMat("Fator") = fator
    '            drMat("QtdeTotal") = QtdeTotalMaterial
    '            drMat("PesoTotal") = 0
    '            drMat("AreaPinturaTotal") = 0
    '            drMat("PesoMaterial") = PesoMaterial

    '            tabelaDest.Rows.Add(drMat)

    '        Catch
    '            Continue For
    '        End Try
    '    Next

    'End Sub



#End Region



#Region "VERSÃO NOVA - 04-02-2026 rev 00 (REVISADA - QTDE ACUMULADA CORRETA)"

    ''================================================================================
    '' Monta estrutura BOM + MATERIAIS no MESMO DataGridView
    '' - QtdeLocal  = quantidade do Nivel (dentro do pai)
    '' - QtdeTotal  = quantidade acumulada (multiplica todos os níveis acima)
    '' - Materiais  = multiplicam SEMPRE pela QtdeTotal da peça
    ''================================================================================
    'Public Sub ListarEstruturaBomMateriais(ByVal dgv As DataGridView,
    '                                   ByVal pgb As ProgressBar,
    '                                   ByVal lbl As Label,
    '                                   ByVal lblResumo As Label)

    '    Dim erros As New List(Of String)
    '    Dim qtdRpcDesconectado As Integer = 0

    '    Dim RegistrarErro As Action(Of String, Exception) =
    '    Sub(contexto As String, ex As Exception)
    '        Try
    '            If ex Is Nothing Then Return

    '            Dim msg As String
    '            Dim comEx = TryCast(ex, System.Runtime.InteropServices.COMException)

    '            If comEx IsNot Nothing AndAlso comEx.ErrorCode = &H80010108 Then
    '                qtdRpcDesconectado += 1
    '                msg = $"{contexto}: [RPC_E_DISCONNECTED] O objeto do Solid Edge foi desconectado. Detalhe: {ex.Message}"
    '            Else
    '                msg = $"{contexto}: {ex.Message}"
    '            End If

    '            erros.Add(msg)
    '        Catch
    '        End Try
    '    End Sub

    '    ' Parse Decimal robusto (aceita 2,5 e 2.5)
    '    Dim ParseDecimalFlex As Func(Of Object, Decimal) =
    '    Function(v As Object) As Decimal
    '        Try
    '            If v Is Nothing OrElse v Is DBNull.Value Then Return 0D
    '            Dim s As String = v.ToString().Trim()
    '            If s = "" Then Return 0D
    '            s = s.Replace(",", ".")
    '            Dim d As Decimal
    '            If Decimal.TryParse(s, Globalization.NumberStyles.Any, Globalization.CultureInfo.InvariantCulture, d) Then
    '                Return d
    '            End If
    '            Return 0D
    '        Catch
    '            Return 0D
    '        End Try
    '    End Function

    '    Try
    '        '============================================================
    '        ' 🔹 Garante conexão com o Solid Edge
    '        '============================================================
    '        If app Is Nothing Then
    '            app = ConectarSolidEdge()
    '        End If

    '        Dim doc As Object = Nothing
    '        Try
    '            If app Is Nothing OrElse app.Documents.Count = 0 Then
    '                MessageBox.Show("Nenhum documento aberto no Solid Edge.",
    '                            "SINCO - Solid Edge",
    '                            MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
    '                Exit Sub
    '            End If

    '            doc = app.ActiveDocument
    '            If doc Is Nothing Then
    '                MessageBox.Show("Não foi possível obter o documento ativo.",
    '                            "SINCO - Solid Edge",
    '                            MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
    '                Exit Sub
    '            End If
    '        Catch ex As Exception
    '            MessageBox.Show("Erro ao acessar o Solid Edge: " & ex.Message,
    '                        "Erro de conexão",
    '                        MessageBoxButtons.OK, MessageBoxIcon.Error)
    '            Exit Sub
    '        End Try

    '        If doc Is Nothing Then Exit Sub

    '        '============================================================
    '        ' 🔹 UI - estado inicial
    '        '============================================================
    '        pgb.Visible = True
    '        pgb.Style = ProgressBarStyle.Marquee
    '        pgb.Value = 0
    '        lbl.Visible = True
    '        lbl.Text = "Lendo estrutura... aguarde."

    '        '============================================================
    '        ' 🔹 Tabela destino (mantém colunas originais)
    '        '============================================================
    '        Dim tabela As New DataTable("BOM_Completa")

    '        tabela.Columns.Add("Nivel", GetType(Integer))
    '        tabela.Columns.Add("Arquivo", GetType(String))
    '        tabela.Columns.Add("TipoArquivo", GetType(String))
    '        tabela.Columns.Add("Qtde", GetType(Double))                 ' mantém original
    '        tabela.Columns.Add("CaminhoCompleto", GetType(String))
    '        tabela.Columns.Add("TipoLinha", GetType(String))

    '        Dim propsInteressantes As New List(Of String) From {
    '        "Título", "Assunto", "Coment", "Tipo de Desenho", "Palavras-", "Autor", "Empresa",
    '        "Categoria", "Gerente", "Material", "Data", "DataR", "Data1",
    '        "Revision", "CutSizeX", "CutSizeY", "Thickness",
    '        "Mass", "Área_de_superfície", "Area_de_superficie", "Bloqueado"
    '    }
    '        For Each p In propsInteressantes
    '            If Not tabela.Columns.Contains(p) Then tabela.Columns.Add(p, GetType(String))
    '        Next

    '        tabela.Columns.Add("RNC", GetType(String))
    '        tabela.Columns.Add("Fator", GetType(Double))
    '        tabela.Columns.Add("QtdeTotal", GetType(Double))            ' mantém original
    '        tabela.Columns.Add("PesoTotal", GetType(Double))
    '        tabela.Columns.Add("AreaPinturaTotal", GetType(Double))

    '        tabela.Columns.Add("CodMatFabricante", GetType(String))
    '        tabela.Columns.Add("DescDetal", GetType(String))
    '        tabela.Columns.Add("QtdeMaterial", GetType(Double))
    '        tabela.Columns.Add("PesoMaterial", GetType(Double))

    '        Dim totalLidos As Integer = 0
    '        Dim totalEstimado As Integer = 1

    '        '============================================================
    '        ' 🔹 Helpers de propriedades (mantidos)
    '        '============================================================
    '        Dim LimparUnidades As Func(Of String, String) =
    '        Function(txt As String) As String
    '            If String.IsNullOrWhiteSpace(txt) Then Return ""
    '            Return txt _
    '                .Replace("mm²", "") _
    '                .Replace("MM²", "") _
    '                .Replace("mm", "") _
    '                .Replace("MM", "") _
    '                .Replace("^2", "") _
    '                .Replace("KG", "") _
    '                .Replace("kg", "") _
    '                .Trim()
    '        End Function

    '        Dim FormatarValor As Func(Of String, String, String) =
    '        Function(propNome As String, valorBruto As String) As String
    '            If String.IsNullOrWhiteSpace(valorBruto) Then Return ""
    '            Dim nome = propNome.Trim().ToUpperInvariant()
    '            Dim valor = LimparUnidades(valorBruto)

    '            If nome = "DATA" OrElse nome = "DATAR" OrElse nome = "DATA1" Then
    '                Try
    '                    Return cl_BancoDados.FormatarData(valor)
    '                Catch
    '                    Return valor
    '                End Try
    '            End If

    '            If nome.Contains("REVISION") Then Return valor.ToUpperInvariant()

    '            If nome.Contains("CUTSIZEX") Then Return valor.Replace(",", ".")
    '            If nome.Contains("CUTSIZEY") Then Return valor.Replace(",", ".")
    '            If nome.Contains("THICKNESS") Then Return valor.Replace(",", ".")
    '            If nome.Contains("MASS") Then Return valor.Replace(",", ".")

    '            If nome.Contains("ÁREA") OrElse nome.Contains("AREA") Then
    '                Dim num As Double
    '                If Double.TryParse(valor.Replace(",", "."),
    '                                   Globalization.NumberStyles.Any,
    '                                   Globalization.CultureInfo.InvariantCulture,
    '                                   num) Then
    '                    Dim m2 = num / 1_000_000.0R
    '                    Return m2.ToString("N4", Globalization.CultureInfo.GetCultureInfo("pt-BR"))
    '                End If
    '            End If

    '            Return valor
    '        End Function

    '        Dim PreencherPropriedades As Action(Of Object, DataRow) =
    '        Sub(docObj As Object, linha As DataRow)
    '            If linha Is Nothing Then Exit Sub

    '            Dim tentativas As Integer = 0
    '            Dim abriuTemp As Boolean = False

    '            While tentativas < 2
    '                Try
    '                    If docObj Is Nothing Then
    '                        Dim caminhoArquivo As String = ""
    '                        Try : caminhoArquivo = Convert.ToString(linha("CaminhoCompleto")) : Catch : caminhoArquivo = "" : End Try

    '                        If Not String.IsNullOrWhiteSpace(caminhoArquivo) AndAlso IO.File.Exists(caminhoArquivo) Then
    '                            Try
    '                                docObj = app.Documents.Open(caminhoArquivo)
    '                                abriuTemp = True
    '                            Catch exOpen As Exception
    '                                RegistrarErro($"Reabrindo documento '{caminhoArquivo}'", exOpen)
    '                                Exit While
    '                            End Try
    '                        Else
    '                            Exit While
    '                        End If
    '                    End If

    '                    Dim propSets As Object = docObj.Properties
    '                    For Each propSet As Object In propSets
    '                        For Each prop As Object In propSet
    '                            If prop Is Nothing Then Continue For

    '                            Dim nomeProp As String = ""
    '                            Try : nomeProp = CStr(prop.Name) : Catch : Continue For : End Try
    '                            If String.IsNullOrWhiteSpace(nomeProp) Then Continue For

    '                            For Each alvo In propsInteressantes
    '                                If nomeProp.IndexOf(alvo, StringComparison.OrdinalIgnoreCase) >= 0 Then
    '                                    Dim valorBruto As String = ""
    '                                    Try
    '                                        Dim v = prop.Value
    '                                        If v IsNot Nothing Then valorBruto = v.ToString()
    '                                    Catch ex As Exception
    '                                        valorBruto = "(erro)"
    '                                        RegistrarErro($"Lendo propriedade '{nomeProp}'", ex)
    '                                    End Try
    '                                    linha(alvo) = FormatarValor(nomeProp, valorBruto)
    '                                    Exit For
    '                                End If
    '                            Next

    '                            If nomeProp.IndexOf("Material Thickness", StringComparison.OrdinalIgnoreCase) >= 0 _
    '                               OrElse nomeProp.IndexOf("Espessura Do Material", StringComparison.OrdinalIgnoreCase) >= 0 _
    '                               OrElse nomeProp.Equals("Thickness", StringComparison.OrdinalIgnoreCase) Then

    '                                Dim valorBruto As String = ""
    '                                Try
    '                                    Dim v = prop.Value
    '                                    If v IsNot Nothing Then valorBruto = v.ToString()
    '                                Catch ex As Exception
    '                                    RegistrarErro($"Lendo espessura em '{nomeProp}'", ex)
    '                                End Try

    '                                If Not String.IsNullOrWhiteSpace(valorBruto) Then
    '                                    Dim limpo = LimparUnidades(valorBruto).Replace(",", ".")
    '                                    linha("Thickness") = limpo
    '                                End If
    '                            End If
    '                        Next
    '                    Next

    '                    Exit While

    '                Catch comEx As System.Runtime.InteropServices.COMException
    '                    If comEx.ErrorCode = &H80010108 Then
    '                        tentativas += 1
    '                        RegistrarErro("PreencherPropriedades - RPC_E_DISCONNECTED, tentando reabrir documento", comEx)
    '                        docObj = Nothing
    '                        Threading.Thread.Sleep(200)
    '                        Continue While
    '                    Else
    '                        RegistrarErro("PreencherPropriedades", comEx)
    '                        Exit While
    '                    End If

    '                Catch ex As Exception
    '                    RegistrarErro("PreencherPropriedades", ex)
    '                    Exit While
    '                End Try
    '            End While

    '            If abriuTemp AndAlso docObj IsNot Nothing Then
    '                Try
    '                    Dim isActive As Boolean = False
    '                    Try : isActive = (TryCast(docObj, Object) Is app.ActiveDocument) : Catch : isActive = False : End Try
    '                    If Not isActive Then
    '                        Try
    '                            CallByName(docObj, "Close", CallType.Method, False)
    '                        Catch ex As Exception
    '                            RegistrarErro("Fechando documento aberto em PreencherPropriedades", ex)
    '                        End Try
    '                    End If
    '                Catch
    '                End Try
    '            End If
    '        End Sub

    '        '------------------------------------------------------------
    '        ' Adiciona linha PEÇA + materiais abaixo (funcionalidade original)
    '        ' - qtdeLocal = quantidade no Nivel (dentro do pai)
    '        ' - qtdeTotal = acumulado (multiplica níveis acima)
    '        ' - Qtde (visual) mantém o que você usava no grid (aqui: qtdeLocal)
    '        ' - QtdeTotal guarda o acumulado correto
    '        '------------------------------------------------------------
    '        Dim AdicionarPeca As Action(Of Integer, String, String, Double, Double, String, Object) =
    '        Sub(nivel As Integer,
    '            nomeComExt As String,
    '            tipoArquivo As String,
    '            qtdeLocal As Double,
    '            qtdeTotal As Double,
    '            caminho As String,
    '            docObj As Object)

    '            Try
    '                Dim linha = tabela.NewRow()

    '                linha("TipoLinha") = "PECA"
    '                linha("Nivel") = nivel
    '                linha("Arquivo") = nomeComExt
    '                linha("TipoArquivo") = tipoArquivo

    '                ' Mantém a "Qtde" como quantidade do Nivel (como a sua original fazia)
    '                linha("Qtde") = qtdeLocal

    '                ' QtdeTotal = acumulado correto (pai * filho * neto...)
    '                linha("QtdeTotal") = qtdeTotal

    '                linha("CaminhoCompleto") = caminho

    '                For Each p In propsInteressantes
    '                    linha(p) = ""
    '                Next

    '                linha("RNC") = ""
    '                linha("Fator") = 1.0R
    '                linha("PesoTotal") = 0.0R
    '                linha("AreaPinturaTotal") = 0.0R

    '                linha("CodMatFabricante") = ""
    '                linha("DescDetal") = ""
    '                linha("QtdeMaterial") = 0.0R
    '                linha("PesoMaterial") = 0.0R

    '                PreencherPropriedades(docObj, linha)

    '                ' Peso/Área devem usar o ACUMULADO (qtdeTotal), senão fica errado nos níveis
    '                Dim mass As Double = 0
    '                Double.TryParse(Convert.ToString(linha("Mass")).Replace(",", "."),
    '                                Globalization.NumberStyles.Any,
    '                                Globalization.CultureInfo.InvariantCulture,
    '                                mass)

    '                Dim area As Double = 0
    '                Double.TryParse(Convert.ToString(linha("Área_de_superfície")).Replace(",", "."),
    '                                Globalization.NumberStyles.Any,
    '                                Globalization.CultureInfo.InvariantCulture,
    '                                area)

    '                linha("PesoTotal") = mass * qtdeTotal
    '                linha("AreaPinturaTotal") = area * qtdeTotal

    '                tabela.Rows.Add(linha)

    '                ' Materiais: abaixo da peça, com qtde baseada NO ACUMULADO da peça
    '                Try
    '                    LerDadosViewMontaPecaParaTabela(tabela, nivel, nomeComExt, qtdeTotal)
    '                Catch exMat As Exception
    '                    RegistrarErro($"Materiais para '{nomeComExt}'", exMat)
    '                End Try

    '            Catch ex As Exception
    '                RegistrarErro($"AdicionarPeca [{nomeComExt}]", ex)
    '            End Try
    '        End Sub

    '        '------------------------------------------------------------
    '        ' Recursão (mantém árvore original)
    '        ' Parâmetros:
    '        ' - qtdeLocalAtual: quantidade dentro do pai
    '        ' - qtdeTotalAtual: acumulado (multiplica níveis acima)
    '        '------------------------------------------------------------
    '        Dim Ler As Action(Of Object, String, Integer, Double, Double) =
    '        Sub(docObj As Object, caminhoArquivo As String, nivel As Integer, qtdeLocalAtual As Double, qtdeTotalAtual As Double)
    '            Try
    '                If docObj Is Nothing OrElse String.IsNullOrWhiteSpace(caminhoArquivo) Then Exit Sub

    '                totalLidos += 1
    '                lbl.Text = $"Lendo {IO.Path.GetFileName(caminhoArquivo)} ({totalLidos})"

    '                If totalEstimado > 1 Then
    '                    pgb.Style = ProgressBarStyle.Continuous
    '                    pgb.Value = Math.Min(100, CInt((totalLidos / Math.Max(1, totalEstimado)) * 100))
    '                End If

    '                Dim ext As String = IO.Path.GetExtension(caminhoArquivo).ToLower()
    '                Dim nomeComExt As String = IO.Path.GetFileName(caminhoArquivo)
    '                Dim tipoUpper As String = ext.Replace(".", "").ToUpperInvariant()

    '                ' Adiciona a peça (e materiais logo abaixo)
    '                AdicionarPeca(nivel, nomeComExt, tipoUpper, qtdeLocalAtual, qtdeTotalAtual, caminhoArquivo, docObj)

    '                If ext <> ".asm" Then Exit Sub

    '                Dim occsObj As Object = Nothing
    '                Try
    '                    occsObj = CallByName(docObj, "Occurrences", CallType.Get)
    '                Catch ex As Exception
    '                    RegistrarErro($"Lendo Occurrences de {nomeComExt}", ex)
    '                    occsObj = Nothing
    '                End Try
    '                If occsObj Is Nothing Then Exit Sub

    '                Dim count As Integer = 0
    '                Try
    '                    count = CInt(CallByName(occsObj, "Count", CallType.Get))
    '                Catch ex As Exception
    '                    RegistrarErro($"Lendo Count de {nomeComExt}", ex)
    '                    count = 0
    '                End Try
    '                If count <= 0 Then Exit Sub

    '                totalEstimado += count

    '                ' Agrupa por caminho (evita somar itens diferentes com mesmo nome)
    '                Dim grupo As New Dictionary(Of String, (Caminho As String, QtdeLocal As Double, DocObj As Object))(StringComparer.OrdinalIgnoreCase)

    '                For i As Integer = 1 To count
    '                    Try
    '                        Dim occ As Object = CallByName(occsObj, "Item", CallType.Method, i)

    '                        Dim subPath As String = ""
    '                        Try : subPath = CStr(CallByName(occ, "OccurrenceFileName", CallType.Get)) : Catch ex As Exception : RegistrarErro($"Lendo OccurrenceFileName (#{i}) de {nomeComExt}", ex) : End Try

    '                        Dim subDoc As Object = Nothing
    '                        Try : subDoc = CallByName(occ, "OccurrenceDocument", CallType.Get) : Catch ex As Exception : RegistrarErro($"Lendo OccurrenceDocument (#{i}) de {nomeComExt}", ex) : End Try

    '                        If subDoc Is Nothing AndAlso Not String.IsNullOrWhiteSpace(subPath) AndAlso IO.File.Exists(subPath) Then
    '                            Try
    '                                subDoc = app.Documents.Open(subPath)
    '                            Catch ex As Exception
    '                                RegistrarErro($"Abrindo subdocumento '{subPath}'", ex)
    '                                subDoc = Nothing
    '                            End Try
    '                        End If

    '                        ' Quantity robusto (decimal) -> evita cair pra 1
    '                        Dim qLocalDec As Decimal = 1D
    '                        Try
    '                            Dim qtdProp As Object = CallByName(occ, "Quantity", CallType.Get)
    '                            Dim qParsed As Decimal = ParseDecimalFlex(qtdProp)
    '                            If qParsed > 0D Then qLocalDec = qParsed
    '                        Catch ex As Exception
    '                            RegistrarErro($"Lendo Quantity (#{i}) de {nomeComExt}", ex)
    '                        End Try

    '                        Dim qLocal As Double = CDbl(qLocalDec)

    '                        Dim chaveAgrup As String = If(String.IsNullOrWhiteSpace(subPath), $"ITEM_{i}", subPath).ToUpperInvariant()

    '                        If grupo.ContainsKey(chaveAgrup) Then
    '                            Dim t = grupo(chaveAgrup)
    '                            grupo(chaveAgrup) = (t.Caminho,
    '                                                t.QtdeLocal + qLocal,
    '                                                If(t.DocObj Is Nothing, subDoc, t.DocObj))
    '                        Else
    '                            grupo.Add(chaveAgrup, (subPath, qLocal, subDoc))
    '                        End If

    '                    Catch ex As Exception
    '                        RegistrarErro($"Processando ocorrência (#{i}) de {nomeComExt}", ex)
    '                    End Try
    '                Next

    '                For Each kv In grupo
    '                    Dim caminhoSub As String = kv.Value.Caminho
    '                    Dim qtdeLocalFilho As Double = kv.Value.QtdeLocal
    '                    Dim subDoc As Object = kv.Value.DocObj

    '                    If String.IsNullOrWhiteSpace(caminhoSub) AndAlso subDoc IsNot Nothing Then
    '                        Try : caminhoSub = CStr(CallByName(subDoc, "FullName", CallType.Get)) : Catch ex As Exception : RegistrarErro("Lendo FullName de subdocumento", ex) : caminhoSub = "" : End Try
    '                    End If

    '                    ' >>>>>>> AQUI É A CORREÇÃO PRINCIPAL <<<<<<<<
    '                    ' QtdeTotal do filho = QtdeTotal do pai * QtdeLocal do filho
    '                    Dim qtdeTotalFilho As Double = qtdeTotalAtual * qtdeLocalFilho

    '                    Dim nomeFilho As String
    '                    Dim tipoFilho As String

    '                    If Not String.IsNullOrWhiteSpace(caminhoSub) Then
    '                        nomeFilho = IO.Path.GetFileName(caminhoSub)
    '                        tipoFilho = IO.Path.GetExtension(caminhoSub).Replace(".", "").ToUpperInvariant()
    '                    Else
    '                        nomeFilho = kv.Key
    '                        tipoFilho = ""
    '                    End If

    '                    If subDoc IsNot Nothing AndAlso Not String.IsNullOrWhiteSpace(caminhoSub) Then
    '                        Ler(subDoc, caminhoSub, nivel + 1, qtdeLocalFilho, qtdeTotalFilho)

    '                        ' Fecha se abrimos temporariamente
    '                        Try
    '                            Dim isActive As Boolean = False
    '                            Try : isActive = (TryCast(subDoc, Object) Is app.ActiveDocument) : Catch : isActive = False : End Try

    '                            If Not isActive AndAlso IO.File.Exists(caminhoSub) Then
    '                                Try : CallByName(subDoc, "Close", CallType.Method, False) : Catch : End Try
    '                            End If
    '                        Catch ex As Exception
    '                            RegistrarErro($"Fechando subdocumento '{caminhoSub}'", ex)
    '                        End Try

    '                    Else
    '                        ' Sem doc carregado, mas queremos listar mantendo a árvore
    '                        AdicionarPeca(nivel + 1, nomeFilho, tipoFilho, qtdeLocalFilho, qtdeTotalFilho, caminhoSub, subDoc)
    '                    End If
    '                Next

    '            Catch ex As Exception
    '                RegistrarErro($"Ler('{caminhoArquivo}')", ex)
    '            End Try
    '        End Sub

    '        ' Root: local = 1 / total = 1
    '        Ler(doc, doc.FullName, 0, 1.0R, 1.0R)

    '        '============================================================
    '        ' 🔹 Limpeza de peças “fantasmas”
    '        '============================================================
    '        For i As Integer = tabela.Rows.Count - 1 To 0 Step -1
    '            Dim arq As String = Convert.ToString(tabela.Rows(i)("Arquivo"))
    '            If String.IsNullOrWhiteSpace(arq) AndAlso
    '           String.Equals(Convert.ToString(tabela.Rows(i)("TipoLinha")),
    '                         "PECA",
    '                         StringComparison.OrdinalIgnoreCase) Then
    '                tabela.Rows.RemoveAt(i)
    '            End If
    '        Next

    '        '============================================================
    '        ' 🔹 Totais (apenas peças)
    '        '============================================================
    '        Dim totalQtdSolicitada As Double = 0
    '        Dim arquivosUnicos As New HashSet(Of String)(StringComparer.OrdinalIgnoreCase)

    '        For Each row As DataRow In tabela.Rows
    '            If Not String.Equals(Convert.ToString(row("TipoLinha")), "PECA", StringComparison.OrdinalIgnoreCase) Then Continue For

    '            Dim qtd As Double = 0
    '            Double.TryParse(Convert.ToString(row("QtdeTotal")).Replace(",", "."),
    '                        Globalization.NumberStyles.Any,
    '                        Globalization.CultureInfo.InvariantCulture,
    '                        qtd)
    '            totalQtdSolicitada += qtd

    '            Dim arq As String = Convert.ToString(row("Arquivo"))
    '            If Not String.IsNullOrWhiteSpace(arq) Then arquivosUnicos.Add(arq)
    '        Next

    '        Dim totalPecasDiferentes As Integer = arquivosUnicos.Count
    '        Dim totalPecas As Integer =
    '        tabela.Rows.Cast(Of DataRow)().
    '               Count(Function(r) String.Equals(Convert.ToString(r("TipoLinha")), "PECA", StringComparison.OrdinalIgnoreCase))

    '        '============================================================
    '        ' 🔹 Bind no grid
    '        '============================================================
    '        dgv.DataSource = tabela
    '        dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells
    '        dgv.AllowUserToAddRows = False
    '        dgv.ColumnHeadersDefaultCellStyle.Font = New Font(dgv.Font, FontStyle.Bold)

    '        If dgv.Columns.Contains("Arquivo") Then dgv.Columns("Arquivo").Frozen = True

    '        tabelaOriginalBOM = tabela.Copy()

    '        Dim ocultar() As String = {
    '        "TipoArquivo", "CaminhoCompleto", "Categoria", "Gerente",
    '        "Autor", "Empresa", "Data", "DataR", "Data1",
    '        "Revision", "CutSizeX", "CutSizeY", "Mass",
    '        "Área_de_superfície", "Area_de_superficie", "Bloqueado",
    '        "RNC", "PesoTotal", "AreaPinturaTotal",
    '        "QtdeMaterial", "PesoMaterial", "CodMatFabricante", "DescDetal"
    '    }

    '        For Each c In ocultar
    '            If dgv.Columns.Contains(c) Then dgv.Columns(c).Visible = False
    '        Next

    '        '============================================================
    '        ' 🔹 UI - final
    '        '============================================================
    '        pgb.Style = ProgressBarStyle.Continuous
    '        pgb.Value = 100

    '        lblResumo.Text =
    '        "✅ Estrutura concluída." & vbCrLf &
    '        $"• Total de peças solicitadas (QtdeTotal): {totalQtdSolicitada}" & vbCrLf &
    '        $"• Total de peças diferentes (arquivos únicos): {totalPecasDiferentes}" & vbCrLf &
    '        $"• Total de peças na estrutura (linhas de peça): {totalPecas}"

    '        Threading.Thread.Sleep(550)

    '    Catch ex As Exception
    '        RegistrarErro("Erro geral em ListarEstruturaBomMateriais", ex)

    '    Finally
    '        pgb.Visible = False

    '        If erros IsNot Nothing AndAlso erros.Count > 0 Then
    '            Dim sb As New StringBuilder()
    '            sb.AppendLine("Foram encontrados " & erros.Count & " erro(s) durante a leitura da estrutura:")
    '            sb.AppendLine()

    '            Dim i As Integer = 1
    '            For Each msg In erros
    '                sb.AppendLine(i.ToString("00") & " - " & msg)
    '                i += 1
    '            Next

    '            If qtdRpcDesconectado > 0 Then
    '                sb.AppendLine()
    '                sb.AppendLine(qtdRpcDesconectado.ToString() &
    '                          " erro(s) são do tipo RPC_E_DISCONNECTED (necessário revisar/ajustar o arquivo no Solid Edge).")
    '            End If

    '            MessageBox.Show(sb.ToString(),
    '                        "Relatório de erros - Estrutura",
    '                        MessageBoxButtons.OK,
    '                        MessageBoxIcon.Warning)
    '        End If
    '    End Try

    'End Sub


    ''===============================================================================
    '' Lê os materiais de TabelaViewMontaPeca para uma PEÇA
    '' - RECEBE qtdeTotalPeca (acumulada) e multiplica corretamente
    ''===============================================================================
    'Public Sub LerDadosViewMontaPecaParaTabela(tabelaDest As DataTable,
    '                                           nivelPeca As Integer,
    '                                           nomeArquivoComExt As String,
    '                                           qtdeTotalPeca As Double)

    '    If TabelaViewMontaPeca Is Nothing Then Exit Sub

    '    ' A coluna na view é NomeArquivoSemExtensao -> usar SEM extensão
    '    Dim chaveSemExt As String = IO.Path.GetFileNameWithoutExtension(nomeArquivoComExt).Trim()

    '    DadosArquivoCorrente.NomeArquivoSemExtensao = chaveSemExt
    '    DadosArquivoCorrente.qtde = qtdeTotalPeca

    '    TabelaViewMontaPeca.CaseSensitive = False

    '    Dim dv As New DataView(TabelaViewMontaPeca)
    '    Dim filtro As String = chaveSemExt.Replace("'", "''")
    '    dv.RowFilter = $"NomeArquivoSemExtensao = '{filtro}'"

    '    Dim dtFiltrado As DataTable = dv.ToTable()

    '    For Each linha As DataRow In dtFiltrado.Rows
    '        Try
    '            If Not String.Equals(linha("NomeArquivoSemExtensao").ToString().Trim(),
    '                             chaveSemExt,
    '                             StringComparison.OrdinalIgnoreCase) Then
    '                Continue For
    '            End If

    '            iconeTipoArquivo = My.Resources.sinco_Diversos

    '            ' 1) Quantidade por peça e peso unitário
    '            Dim pecaQtde As Double
    '            Dim pesoUnit As Double

    '            Dim pecaQtdeStr As String = linha("PecaQtde").ToString().Trim().Replace(",", ".")
    '            pecaQtde = cl_BancoDados.converteStringParaDouble(pecaQtdeStr)

    '            Dim pecaPesoStr As String = linha("Peso").ToString().Trim().Replace(",", ".")
    '            pesoUnit = cl_BancoDados.converteStringParaDouble(pecaPesoStr)

    '            Dim fator As Double = 1.0R

    '            ' 2) TOTAL do material = (qtde por peça) * (qtde TOTAL acumulada da peça) * fator
    '            ' O parâmetro qtdeTotalPeca JÁ TRAZ o cálculo dos níveis da recursão principal.
    '            Dim qtdeTotalMaterial As Double = pecaQtde * qtdeTotalPeca * fator

    '            Dim pesoTotalMaterial As Double = 0.0R
    '            Try
    '                pesoTotalMaterial = pesoUnit * qtdeTotalMaterial
    '            Catch
    '            End Try

    '            ' 3) Linha do material abaixo da peça
    '            Dim drMat As DataRow = tabelaDest.NewRow()

    '            drMat("TipoLinha") = "MATERIAL"
    '            drMat("Nivel") = nivelPeca + 1

    '            Dim cod As String = linha("CodMatFabricante").ToString().Trim().ToUpperInvariant()
    '            drMat("Arquivo") = cod
    '            drMat("CodMatFabricante") = cod

    '            drMat("TipoArquivo") = "MAT"

    '            ' ==========================================================================
    '            ' CORREÇÃO AQUI:
    '            ' "Qtde" deve exibir a Quantidade Unitária (por peça pai)
    '            ' "QtdeTotal" deve exibir a Quantidade Acumulada (considerando níveis)
    '            ' ==========================================================================
    '            drMat("Qtde") = pecaQtde                ' ANTES estava: qtdeTotalMaterial (Erro Visual)
    '            drMat("QtdeTotal") = qtdeTotalMaterial  ' Mantém o cálculo correto do total

    '            drMat("QtdeMaterial") = pecaQtde        ' Mantém backup do unitário

    '            drMat("CaminhoCompleto") = ""

    '            Dim desc As String = linha("DescDetal").ToString().Trim().ToUpperInvariant()
    '            Dim matView As String = ""
    '            Try : matView = linha("Material").ToString().Trim().ToUpperInvariant() : Catch : End Try

    '            Dim titulo As String = desc
    '            If matView <> "" Then titulo &= " - " & matView
    '            drMat("Título") = titulo

    '            If matView <> "" Then drMat("Material") = matView
    '            drMat("Tipo de Desenho") = "MATERIAL"

    '            drMat("RNC") = ""
    '            drMat("Fator") = fator
    '            drMat("PesoTotal") = 0.0R
    '            drMat("AreaPinturaTotal") = 0.0R

    '            drMat("PesoMaterial") = pesoTotalMaterial

    '            tabelaDest.Rows.Add(drMat)

    '        Catch
    '            Continue For
    '        End Try
    '    Next

    'End Sub

#End Region

#Region "VERSÃO CORRIGIDA - CALCULO NIVEIS E TOTAIS"

    ''================================================================================
    '' Monta estrutura BOM + MATERIAIS no MESMO DataGridView
    ''================================================================================
    'Public Sub ListarEstruturaBomMateriais(ByVal dgv As DataGridView,
    '                                       ByVal pgb As ProgressBar,
    '                                       ByVal lbl As Label,
    '                                       ByVal lblResumo As Label)

    '    Dim erros As New List(Of String)
    '    Dim qtdRpcDesconectado As Integer = 0

    '    ' Helper para registrar erros
    '    Dim RegistrarErro As Action(Of String, Exception) =
    '    Sub(contexto As String, ex As Exception)
    '        Try
    '            If ex Is Nothing Then Return
    '            Dim msg As String
    '            Dim comEx = TryCast(ex, System.Runtime.InteropServices.COMException)
    '            If comEx IsNot Nothing AndAlso comEx.ErrorCode = &H80010108 Then
    '                qtdRpcDesconectado += 1
    '                msg = $"{contexto}: [RPC_E_DISCONNECTED] O objeto do Solid Edge foi desconectado. Detalhe: {ex.Message}"
    '            Else
    '                msg = $"{contexto}: {ex.Message}"
    '            End If
    '            erros.Add(msg)
    '        Catch
    '        End Try
    '    End Sub

    '    ' Helper de Parse Decimal (Original)
    '    Dim ParseDecimalFlex As Func(Of Object, Decimal) =
    '    Function(v As Object) As Decimal
    '        Try
    '            If v Is Nothing OrElse v Is DBNull.Value Then Return 0D
    '            Dim s As String = v.ToString().Trim()
    '            If s = "" Then Return 0D
    '            s = s.Replace(",", ".")
    '            Dim d As Decimal
    '            If Decimal.TryParse(s, Globalization.NumberStyles.Any, Globalization.CultureInfo.InvariantCulture, d) Then
    '                Return d
    '            End If
    '            Return 0D
    '        Catch
    '            Return 0D
    '        End Try
    '    End Function

    '    Try
    '        '============================================================
    '        ' 🔹 Garante conexão com o Solid Edge
    '        '============================================================
    '        If app Is Nothing Then
    '            ' app = ConectarSolidEdge() ' (Se tiver sua rotina de conexão, descomente)
    '            Try
    '                app = System.Runtime.InteropServices.Marshal.GetActiveObject("SolidEdge.Application")
    '            Catch
    '            End Try
    '        End If

    '        Dim doc As Object = Nothing
    '        Try
    '            If app Is Nothing OrElse app.Documents.Count = 0 Then
    '                MessageBox.Show("Nenhum documento aberto no Solid Edge.", "SINCO - Solid Edge", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
    '                Exit Sub
    '            End If

    '            doc = app.ActiveDocument
    '            If doc Is Nothing Then
    '                MessageBox.Show("Não foi possível obter o documento ativo.", "SINCO - Solid Edge", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
    '                Exit Sub
    '            End If
    '        Catch ex As Exception
    '            MessageBox.Show("Erro ao acessar o Solid Edge: " & ex.Message, "Erro de conexão", MessageBoxButtons.OK, MessageBoxIcon.Error)
    '            Exit Sub
    '        End Try

    '        If doc Is Nothing Then Exit Sub

    '        '============================================================
    '        ' 🔹 UI - estado inicial
    '        '============================================================
    '        pgb.Visible = True
    '        pgb.Style = ProgressBarStyle.Marquee
    '        pgb.Value = 0
    '        lbl.Visible = True
    '        lbl.Text = "Lendo estrutura... aguarde."

    '        '============================================================
    '        ' 🔹 Tabela destino
    '        '============================================================
    '        Dim tabela As New DataTable("BOM_Completa")

    '        tabela.Columns.Add("Nivel", GetType(Integer))
    '        tabela.Columns.Add("Arquivo", GetType(String))
    '        tabela.Columns.Add("TipoArquivo", GetType(String))
    '        tabela.Columns.Add("Qtde", GetType(Double))             ' Originalmente mantida como Double
    '        tabela.Columns.Add("CaminhoCompleto", GetType(String))
    '        tabela.Columns.Add("TipoLinha", GetType(String))

    '        Dim propsInteressantes As New List(Of String) From {
    '        "Título", "Assunto", "Coment", "Tipo de Desenho", "Palavras-", "Autor", "Empresa",
    '        "Categoria", "Gerente", "Material", "Data", "DataR", "Data1",
    '        "Revision", "CutSizeX", "CutSizeY", "Thickness",
    '        "Mass", "Área_de_superfície", "Area_de_superficie", "Bloqueado"
    '    }
    '        For Each p In propsInteressantes
    '            If Not tabela.Columns.Contains(p) Then tabela.Columns.Add(p, GetType(String))
    '        Next

    '        tabela.Columns.Add("RNC", GetType(String))
    '        tabela.Columns.Add("Fator", GetType(Double))
    '        tabela.Columns.Add("QtdeTotal", GetType(Double))
    '        tabela.Columns.Add("PesoTotal", GetType(Double))
    '        tabela.Columns.Add("AreaPinturaTotal", GetType(Double))

    '        tabela.Columns.Add("CodMatFabricante", GetType(String))
    '        tabela.Columns.Add("DescDetal", GetType(String))
    '        tabela.Columns.Add("QtdeMaterial", GetType(Double))
    '        tabela.Columns.Add("PesoMaterial", GetType(Double))

    '        Dim totalLidos As Integer = 0
    '        Dim totalEstimado As Integer = 1

    '        '============================================================
    '        ' 🔹 Helpers Originais (Formatadores)
    '        '============================================================
    '        Dim LimparUnidades As Func(Of String, String) =
    '        Function(txt As String) As String
    '            If String.IsNullOrWhiteSpace(txt) Then Return ""
    '            Return txt.Replace("mm²", "").Replace("MM²", "").Replace("mm", "").Replace("MM", "").Replace("^2", "").Replace("KG", "").Replace("kg", "").Trim()
    '        End Function

    '        Dim FormatarValor As Func(Of String, String, String) =
    '        Function(propNome As String, valorBruto As String) As String
    '            If String.IsNullOrWhiteSpace(valorBruto) Then Return ""
    '            Dim nome = propNome.Trim().ToUpperInvariant()
    '            Dim valor = LimparUnidades(valorBruto)

    '            If nome = "DATA" OrElse nome = "DATAR" OrElse nome = "DATA1" Then
    '                Try
    '                    Return cl_BancoDados.FormatarData(valor)
    '                Catch
    '                    Return valor
    '                End Try
    '            End If

    '            If nome.Contains("REVISION") Then Return valor.ToUpperInvariant()

    '            If nome.Contains("CUTSIZEX") Then Return valor.Replace(",", ".")
    '            If nome.Contains("CUTSIZEY") Then Return valor.Replace(",", ".")
    '            If nome.Contains("THICKNESS") Then Return valor.Replace(",", ".")
    '            If nome.Contains("MASS") Then Return valor.Replace(",", ".")

    '            If nome.Contains("ÁREA") OrElse nome.Contains("AREA") Then
    '                Dim num As Double
    '                If Double.TryParse(valor.Replace(",", "."), Globalization.NumberStyles.Any, Globalization.CultureInfo.InvariantCulture, num) Then
    '                    Dim m2 = num / 1_000_000.0R
    '                    Return m2.ToString("N4", Globalization.CultureInfo.GetCultureInfo("pt-BR"))
    '                End If
    '            End If

    '            Return valor
    '        End Function

    '        '============================================================
    '        ' 🔹 PreencherPropriedades (Lógica ORIGINAL Restaurada)
    '        '============================================================
    '        Dim PreencherPropriedades As Action(Of Object, DataRow) =
    '        Sub(docObj As Object, linha As DataRow)
    '            If linha Is Nothing Then Exit Sub

    '            Dim tentativas As Integer = 0
    '            Dim abriuTemp As Boolean = False

    '            While tentativas < 2
    '                Try
    '                    If docObj Is Nothing Then
    '                        Dim caminhoArquivo As String = ""
    '                        Try : caminhoArquivo = Convert.ToString(linha("CaminhoCompleto")) : Catch : caminhoArquivo = "" : End Try

    '                        If Not String.IsNullOrWhiteSpace(caminhoArquivo) AndAlso IO.File.Exists(caminhoArquivo) Then
    '                            Try
    '                                docObj = app.Documents.Open(caminhoArquivo)
    '                                abriuTemp = True
    '                            Catch exOpen As Exception
    '                                RegistrarErro($"Reabrindo documento '{caminhoArquivo}'", exOpen)
    '                                Exit While
    '                            End Try
    '                        Else
    '                            Exit While
    '                        End If
    '                    End If

    '                    Dim propSets As Object = docObj.Properties
    '                    For Each propSet As Object In propSets
    '                        For Each prop As Object In propSet
    '                            If prop Is Nothing Then Continue For

    '                            Dim nomeProp As String = ""
    '                            Try : nomeProp = CStr(prop.Name) : Catch : Continue For : End Try
    '                            If String.IsNullOrWhiteSpace(nomeProp) Then Continue For

    '                            For Each alvo In propsInteressantes
    '                                If nomeProp.IndexOf(alvo, StringComparison.OrdinalIgnoreCase) >= 0 Then
    '                                    Dim valorBruto As String = ""
    '                                    Try
    '                                        Dim v = prop.Value
    '                                        If v IsNot Nothing Then valorBruto = v.ToString()
    '                                    Catch ex As Exception
    '                                        valorBruto = "(erro)"
    '                                        RegistrarErro($"Lendo propriedade '{nomeProp}'", ex)
    '                                    End Try
    '                                    linha(alvo) = FormatarValor(nomeProp, valorBruto)
    '                                    Exit For
    '                                End If
    '                            Next

    '                            If nomeProp.IndexOf("Material Thickness", StringComparison.OrdinalIgnoreCase) >= 0 _
    '                               OrElse nomeProp.IndexOf("Espessura Do Material", StringComparison.OrdinalIgnoreCase) >= 0 _
    '                               OrElse nomeProp.Equals("Thickness", StringComparison.OrdinalIgnoreCase) Then

    '                                Dim valorBruto As String = ""
    '                                Try
    '                                    Dim v = prop.Value
    '                                    If v IsNot Nothing Then valorBruto = v.ToString()
    '                                Catch ex As Exception
    '                                    RegistrarErro($"Lendo espessura em '{nomeProp}'", ex)
    '                                End Try

    '                                If Not String.IsNullOrWhiteSpace(valorBruto) Then
    '                                    Dim limpo = LimparUnidades(valorBruto).Replace(",", ".")
    '                                    linha("Thickness") = limpo
    '                                End If
    '                            End If
    '                        Next
    '                    Next

    '                    Exit While

    '                Catch comEx As System.Runtime.InteropServices.COMException
    '                    If comEx.ErrorCode = &H80010108 Then
    '                        tentativas += 1
    '                        RegistrarErro("PreencherPropriedades - RPC_E_DISCONNECTED, tentando reabrir documento", comEx)
    '                        docObj = Nothing
    '                        Threading.Thread.Sleep(200)
    '                        Continue While
    '                    Else
    '                        RegistrarErro("PreencherPropriedades", comEx)
    '                        Exit While
    '                    End If

    '                Catch ex As Exception
    '                    RegistrarErro("PreencherPropriedades", ex)
    '                    Exit While
    '                End Try
    '            End While

    '            If abriuTemp AndAlso docObj IsNot Nothing Then
    '                Try
    '                    Dim isActive As Boolean = False
    '                    Try : isActive = (TryCast(docObj, Object) Is app.ActiveDocument) : Catch : isActive = False : End Try
    '                    If Not isActive Then
    '                        Try
    '                            CallByName(docObj, "Close", CallType.Method, False)
    '                        Catch ex As Exception
    '                            RegistrarErro("Fechando documento aberto em PreencherPropriedades", ex)
    '                        End Try
    '                    End If
    '                Catch
    '                End Try
    '            End If
    '        End Sub

    '        '============================================================
    '        ' 🔹 AdicionarPeca (Com cálculo corrigido + Leitura Original)
    '        '============================================================
    '        Dim AdicionarPeca As Action(Of Integer, String, String, Double, Double, String, Object) =
    '        Sub(nivel As Integer,
    '            nomeComExt As String,
    '            tipoArquivo As String,
    '            qtdeLocal As Double,
    '            qtdeTotal As Double,
    '            caminho As String,
    '            docObj As Object)

    '            Try
    '                Dim linha = tabela.NewRow()

    '                linha("TipoLinha") = "PECA"
    '                linha("Nivel") = nivel
    '                linha("Arquivo") = nomeComExt
    '                linha("TipoArquivo") = tipoArquivo

    '                ' >>> CORREÇÃO DO CÁLCULO MANTIDA AQUI <<<
    '                linha("Qtde") = qtdeLocal           ' Quantidade unitária (local)
    '                linha("QtdeTotal") = qtdeTotal      ' Quantidade total acumulada

    '                linha("CaminhoCompleto") = caminho

    '                For Each p In propsInteressantes
    '                    linha(p) = ""
    '                Next

    '                linha("RNC") = ""
    '                linha("Fator") = 1.0R
    '                linha("PesoTotal") = 0.0R
    '                linha("AreaPinturaTotal") = 0.0R

    '                linha("CodMatFabricante") = ""
    '                linha("DescDetal") = ""
    '                linha("QtdeMaterial") = 0.0R
    '                linha("PesoMaterial") = 0.0R

    '                ' >>> LEITURA DAS PROPRIEDADES ORIGINAL RESTAURADA AQUI <<<
    '                PreencherPropriedades(docObj, linha)

    '                ' Peso/Área usando o ACUMULADO (qtdeTotal) e valores lidos das propriedades
    '                Dim mass As Double = 0
    '                Double.TryParse(Convert.ToString(linha("Mass")).Replace(",", "."), Globalization.NumberStyles.Any, Globalization.CultureInfo.InvariantCulture, mass)

    '                Dim area As Double = 0
    '                Double.TryParse(Convert.ToString(linha("Área_de_superfície")).Replace(",", "."), Globalization.NumberStyles.Any, Globalization.CultureInfo.InvariantCulture, area)

    '                linha("PesoTotal") = mass * qtdeTotal
    '                linha("AreaPinturaTotal") = area * qtdeTotal

    '                tabela.Rows.Add(linha)

    '                ' Materiais:
    '                Try
    '                    LerDadosViewMontaPecaParaTabela(tabela, nivel, nomeComExt, qtdeTotal)
    '                Catch exMat As Exception
    '                    RegistrarErro($"Materiais para '{nomeComExt}'", exMat)
    '                End Try

    '            Catch ex As Exception
    '                RegistrarErro($"AdicionarPeca [{nomeComExt}]", ex)
    '            End Try
    '        End Sub

    '        '============================================================
    '        ' 🔹 Recursão Ler (Lógica corrigida de Local/Total)
    '        '============================================================
    '        Dim Ler As Action(Of Object, String, Integer, Double, Double) =
    '        Sub(docObj As Object, caminhoArquivo As String, nivel As Integer, qtdeLocalAtual As Double, qtdeTotalAtual As Double)
    '            Try
    '                If docObj Is Nothing OrElse String.IsNullOrWhiteSpace(caminhoArquivo) Then Exit Sub

    '                totalLidos += 1
    '                lbl.Text = $"Lendo {IO.Path.GetFileName(caminhoArquivo)} ({totalLidos})"

    '                If totalEstimado > 1 Then
    '                    pgb.Style = ProgressBarStyle.Continuous
    '                    pgb.Value = Math.Min(100, CInt((totalLidos / Math.Max(1, totalEstimado)) * 100))
    '                End If

    '                Dim ext As String = IO.Path.GetExtension(caminhoArquivo).ToLower()
    '                Dim nomeComExt As String = IO.Path.GetFileName(caminhoArquivo)
    '                Dim tipoUpper As String = ext.Replace(".", "").ToUpperInvariant()

    '                ' Adiciona a peça com as propriedades lidas
    '                AdicionarPeca(nivel, nomeComExt, tipoUpper, qtdeLocalAtual, qtdeTotalAtual, caminhoArquivo, docObj)

    '                If ext <> ".asm" Then Exit Sub

    '                Dim occsObj As Object = Nothing
    '                Try : occsObj = CallByName(docObj, "Occurrences", CallType.Get) : Catch ex As Exception : RegistrarErro($"Lendo Occurrences de {nomeComExt}", ex) : occsObj = Nothing : End Try
    '                If occsObj Is Nothing Then Exit Sub

    '                Dim count As Integer = 0
    '                Try : count = CInt(CallByName(occsObj, "Count", CallType.Get)) : Catch ex As Exception : RegistrarErro($"Lendo Count de {nomeComExt}", ex) : count = 0 : End Try
    '                If count <= 0 Then Exit Sub

    '                totalEstimado += count

    '                Dim grupo As New Dictionary(Of String, (Caminho As String, QtdeLocal As Double, DocObj As Object))(StringComparer.OrdinalIgnoreCase)

    '                For i As Integer = 1 To count
    '                    Try
    '                        Dim occ As Object = CallByName(occsObj, "Item", CallType.Method, i)

    '                        Dim subPath As String = ""
    '                        Try : subPath = CStr(CallByName(occ, "OccurrenceFileName", CallType.Get)) : Catch ex As Exception : RegistrarErro($"Lendo OccurrenceFileName (#{i}) de {nomeComExt}", ex) : End Try

    '                        Dim subDoc As Object = Nothing
    '                        Try : subDoc = CallByName(occ, "OccurrenceDocument", CallType.Get) : Catch ex As Exception : RegistrarErro($"Lendo OccurrenceDocument (#{i}) de {nomeComExt}", ex) : End Try

    '                        If subDoc Is Nothing AndAlso Not String.IsNullOrWhiteSpace(subPath) AndAlso IO.File.Exists(subPath) Then
    '                            Try : subDoc = app.Documents.Open(subPath) : Catch ex As Exception : RegistrarErro($"Abrindo subdocumento '{subPath}'", ex) : subDoc = Nothing : End Try
    '                        End If

    '                        Dim qLocalDec As Decimal = 1D
    '                        Try
    '                            Dim qtdProp As Object = CallByName(occ, "Quantity", CallType.Get)
    '                            Dim qParsed As Decimal = ParseDecimalFlex(qtdProp)
    '                            If qParsed > 0D Then qLocalDec = qParsed
    '                        Catch ex As Exception
    '                            RegistrarErro($"Lendo Quantity (#{i}) de {nomeComExt}", ex)
    '                        End Try

    '                        Dim qLocal As Double = CDbl(qLocalDec)
    '                        Dim chaveAgrup As String = If(String.IsNullOrWhiteSpace(subPath), $"ITEM_{i}", subPath).ToUpperInvariant()

    '                        If grupo.ContainsKey(chaveAgrup) Then
    '                            Dim t = grupo(chaveAgrup)
    '                            grupo(chaveAgrup) = (t.Caminho, t.QtdeLocal + qLocal, If(t.DocObj Is Nothing, subDoc, t.DocObj))
    '                        Else
    '                            grupo.Add(chaveAgrup, (subPath, qLocal, subDoc))
    '                        End If

    '                    Catch ex As Exception
    '                        RegistrarErro($"Processando ocorrência (#{i}) de {nomeComExt}", ex)
    '                    End Try
    '                Next

    '                For Each kv In grupo
    '                    Dim caminhoSub As String = kv.Value.Caminho
    '                    Dim qtdeLocalFilho As Double = kv.Value.QtdeLocal
    '                    Dim subDoc As Object = kv.Value.DocObj

    '                    If String.IsNullOrWhiteSpace(caminhoSub) AndAlso subDoc IsNot Nothing Then
    '                        Try : caminhoSub = CStr(CallByName(subDoc, "FullName", CallType.Get)) : Catch ex As Exception : RegistrarErro("Lendo FullName de subdocumento", ex) : caminhoSub = "" : End Try
    '                    End If

    '                    ' CÁLCULO DOS NÍVEIS
    '                    Dim qtdeTotalFilho As Double = qtdeTotalAtual * qtdeLocalFilho

    '                    Dim nomeFilho As String
    '                    Dim tipoFilho As String

    '                    If Not String.IsNullOrWhiteSpace(caminhoSub) Then
    '                        nomeFilho = IO.Path.GetFileName(caminhoSub)
    '                        tipoFilho = IO.Path.GetExtension(caminhoSub).Replace(".", "").ToUpperInvariant()
    '                    Else
    '                        nomeFilho = kv.Key
    '                        tipoFilho = ""
    '                    End If

    '                    If subDoc IsNot Nothing AndAlso Not String.IsNullOrWhiteSpace(caminhoSub) Then
    '                        Ler(subDoc, caminhoSub, nivel + 1, qtdeLocalFilho, qtdeTotalFilho)

    '                        Try
    '                            Dim isActive As Boolean = False
    '                            Try : isActive = (TryCast(subDoc, Object) Is app.ActiveDocument) : Catch : isActive = False : End Try
    '                            If Not isActive AndAlso IO.File.Exists(caminhoSub) Then
    '                                Try : CallByName(subDoc, "Close", CallType.Method, False) : Catch : End Try
    '                            End If
    '                        Catch ex As Exception
    '                            RegistrarErro($"Fechando subdocumento '{caminhoSub}'", ex)
    '                        End Try
    '                    Else
    '                        AdicionarPeca(nivel + 1, nomeFilho, tipoFilho, qtdeLocalFilho, qtdeTotalFilho, caminhoSub, subDoc)
    '                    End If
    '                Next

    '            Catch ex As Exception
    '                RegistrarErro($"Ler('{caminhoArquivo}')", ex)
    '            End Try
    '        End Sub

    '        '============================================================
    '        ' 🔹 Chamada Inicial
    '        '============================================================
    '        Ler(doc, doc.FullName, 0, 1.0R, 1.0R)

    '        '============================================================
    '        ' 🔹 Limpeza e Totais (Original)
    '        '============================================================
    '        For i As Integer = tabela.Rows.Count - 1 To 0 Step -1
    '            Dim arq As String = Convert.ToString(tabela.Rows(i)("Arquivo"))
    '            If String.IsNullOrWhiteSpace(arq) AndAlso String.Equals(Convert.ToString(tabela.Rows(i)("TipoLinha")), "PECA", StringComparison.OrdinalIgnoreCase) Then
    '                tabela.Rows.RemoveAt(i)
    '            End If
    '        Next

    '        Dim totalQtdSolicitada As Double = 0
    '        Dim arquivosUnicos As New HashSet(Of String)(StringComparer.OrdinalIgnoreCase)

    '        For Each row As DataRow In tabela.Rows
    '            If Not String.Equals(Convert.ToString(row("TipoLinha")), "PECA", StringComparison.OrdinalIgnoreCase) Then Continue For

    '            Dim qtd As Double = 0
    '            Double.TryParse(Convert.ToString(row("QtdeTotal")).Replace(",", "."), Globalization.NumberStyles.Any, Globalization.CultureInfo.InvariantCulture, qtd)
    '            totalQtdSolicitada += qtd

    '            Dim arq As String = Convert.ToString(row("Arquivo"))
    '            If Not String.IsNullOrWhiteSpace(arq) Then arquivosUnicos.Add(arq)
    '        Next ' Corrigido de End For para Next

    '        Dim totalPecasDiferentes As Integer = arquivosUnicos.Count
    '        Dim totalPecas As Integer = tabela.Rows.Cast(Of DataRow)().Count(Function(r) String.Equals(Convert.ToString(r("TipoLinha")), "PECA", StringComparison.OrdinalIgnoreCase))

    '        dgv.DataSource = tabela
    '        dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells
    '        dgv.AllowUserToAddRows = False
    '        dgv.ColumnHeadersDefaultCellStyle.Font = New Font(dgv.Font, FontStyle.Bold)

    '        If dgv.Columns.Contains("Arquivo") Then dgv.Columns("Arquivo").Frozen = True

    '        ' tabelaOriginalBOM = tabela.Copy() ' (Descomente se usar a variável global)

    '        Dim ocultar() As String = {
    '        "TipoArquivo", "CaminhoCompleto", "Categoria", "Gerente",
    '        "Autor", "Empresa", "Data", "DataR", "Data1",
    '        "Revision", "CutSizeX", "CutSizeY", "Mass",
    '        "Área_de_superfície", "Area_de_superficie", "Bloqueado",
    '        "RNC", "PesoTotal", "AreaPinturaTotal",
    '        "QtdeMaterial", "PesoMaterial", "CodMatFabricante", "DescDetal"
    '    }

    '        For Each c In ocultar
    '            If dgv.Columns.Contains(c) Then dgv.Columns(c).Visible = False
    '        Next

    '        pgb.Style = ProgressBarStyle.Continuous
    '        pgb.Value = 100

    '        lblResumo.Text = "✅ Estrutura concluída." & vbCrLf &
    '        $"• Total de peças solicitadas (QtdeTotal): {totalQtdSolicitada}" & vbCrLf &
    '        $"• Total de peças diferentes (arquivos únicos): {totalPecasDiferentes}" & vbCrLf &
    '        $"• Total de peças na estrutura (linhas de peça): {totalPecas}"

    '        Threading.Thread.Sleep(550)

    '    Catch ex As Exception
    '        RegistrarErro("Erro geral em ListarEstruturaBomMateriais", ex)

    '    Finally
    '        pgb.Visible = False

    '        If erros IsNot Nothing AndAlso erros.Count > 0 Then
    '            Dim sb As New StringBuilder()
    '            sb.AppendLine("Foram encontrados " & erros.Count & " erro(s) durante a leitura da estrutura:")
    '            sb.AppendLine()
    '            Dim i As Integer = 1
    '            For Each msg In erros
    '                sb.AppendLine(i.ToString("00") & " - " & msg)
    '                i += 1
    '            Next
    '            If qtdRpcDesconectado > 0 Then
    '                sb.AppendLine()
    '                sb.AppendLine(qtdRpcDesconectado.ToString() & " erro(s) são do tipo RPC_E_DISCONNECTED (necessário revisar/ajustar o arquivo no Solid Edge).")
    '            End If
    '            MessageBox.Show(sb.ToString(), "Relatório de erros - Estrutura", MessageBoxButtons.OK, MessageBoxIcon.Warning)
    '        End If
    '    End Try

    'End Sub


    ''===============================================================================
    '' 🔴 CORREÇÃO 3: Materiais agora recebe Double e o cálculo já vem pronto
    ''===============================================================================
    'Public Sub LerDadosViewMontaPecaParaTabela(tabelaDest As DataTable,
    '                                           nivelPeca As Integer,
    '                                           nomeArquivoComExt As String,
    '                                           qtdeTotalAcumuladaPeca As Double) ' Alterado para Double

    '    If TabelaViewMontaPeca Is Nothing Then Exit Sub

    '    Dim chaveArquivo As String = IO.Path.GetFileName(nomeArquivoComExt)

    '    ' Variável global ou de contexto (mantida como no original)
    '    Try : DadosArquivoCorrente.NomeArquivoSemExtensao = chaveArquivo : Catch : End Try
    '    Try : DadosArquivoCorrente.qtde = CInt(qtdeTotalAcumuladaPeca) : Catch : End Try

    '    TabelaViewMontaPeca.CaseSensitive = False
    '    Dim dv As New DataView(TabelaViewMontaPeca)
    '    Dim filtro As String = chaveArquivo.Replace("'", "''")
    '    dv.RowFilter = $"NomeArquivoSemExtensao = '{filtro}'"
    '    Dim dtFiltrado As DataTable = dv.ToTable()

    '    For Each linha As DataRow In dtFiltrado.Rows
    '        Try
    '            If Not String.Equals(linha("NomeArquivoSemExtensao").ToString().Trim(), chaveArquivo.Trim(), StringComparison.OrdinalIgnoreCase) Then Continue For

    '            iconeTipoArquivo = My.Resources.sinco_Diversos

    '            Dim PecaQtde As Double
    '            Dim peso As Double
    '            Dim pecaQtdeStr As String = linha.Item("PecaQtde").ToString().Trim().Replace(",", ".")
    '            PecaQtde = cl_BancoDados.converteStringParaDouble(pecaQtdeStr)

    '            Dim pecaPesoStr As String = linha.Item("Peso").ToString().Trim().Replace(",", ".")
    '            peso = cl_BancoDados.converteStringParaDouble(pecaPesoStr)

    '            Dim fator As Double = 1.0R

    '            ' CÁLCULO DO MATERIAL:
    '            ' O parâmetro qtdeTotalAcumuladaPeca JÁ É o total acumulado vindo da recursão.
    '            ' Portanto: Total Material = (Qtd de material por peça) * (Total de Peças na Maquina)
    '            Dim QtdeMaterialPorPeca As Double = PecaQtde
    '            Dim QtdeTotalMaterial As Double = QtdeMaterialPorPeca * qtdeTotalAcumuladaPeca * fator

    '            Dim PesoMaterial As Double = 0
    '            Try : PesoMaterial = peso * QtdeTotalMaterial : Catch : End Try

    '            Dim drMat As DataRow = tabelaDest.NewRow()

    '            drMat("TipoLinha") = "MATERIAL"
    '            drMat("Nivel") = nivelPeca + 1

    '            Dim cod As String = linha.Item("CodMatFabricante").ToString().Trim().ToUpper()
    '            drMat("Arquivo") = cod
    '            drMat("CodMatFabricante") = cod
    '            drMat("TipoArquivo") = "MAT"

    '            ' 🔴 Visualização Correta:
    '            ' Qtde = Quanto gasta em UMA peça
    '            ' QtdeTotal = Quanto gasta no PROJETO todo
    '            drMat("Qtde") = QtdeMaterialPorPeca       ' Unitário
    '            drMat("QtdeTotal") = QtdeTotalMaterial    ' Acumulado (Total)

    '            drMat("QtdeMaterial") = QtdeMaterialPorPeca
    '            drMat("CaminhoCompleto") = ""

    '            Dim desc As String = linha.Item("DescDetal").ToString().Trim().ToUpper()
    '            Dim matView As String = ""
    '            Try : matView = linha.Item("Material").ToString().Trim().ToUpper() : Catch : End Try

    '            Dim titulo As String = desc
    '            If matView <> "" Then titulo &= " - " & matView
    '            drMat("Título") = titulo

    '            If matView <> "" Then drMat("Material") = matView
    '            drMat("Tipo de Desenho") = "MATERIAL"
    '            drMat("RNC") = ""
    '            drMat("Fator") = fator

    '            drMat("PesoTotal") = 0 ' Peso do material geralmente não soma no peso da peça estrutural no grid BOM, manter 0 ou implementar logica
    '            drMat("AreaPinturaTotal") = 0
    '            drMat("PesoMaterial") = PesoMaterial

    '            tabelaDest.Rows.Add(drMat)

    '        Catch
    '            Continue For
    '        End Try
    '    Next
    'End Sub



#End Region

#Region "VERSÃO FINAL CORRIGIDA - SEM ERROS DE COMPILAÇÃO"

    ''================================================================================
    '' Monta estrutura BOM + MATERIAIS no MESMO DataGridView
    ''================================================================================
    'Public Sub ListarEstruturaBomMateriais(ByVal dgv As DataGridView,
    '                                       ByVal pgb As ProgressBar,
    '                                       ByVal lbl As Label,
    '                                       ByVal lblResumo As Label)

    '    Dim erros As New List(Of String)
    '    Dim qtdRpcDesconectado As Integer = 0

    '    ' Helper para registrar erros
    '    Dim RegistrarErro As Action(Of String, Exception) =
    '    Sub(contexto As String, ex As Exception)
    '        Try
    '            If ex Is Nothing Then Return
    '            Dim msg As String
    '            Dim comEx = TryCast(ex, System.Runtime.InteropServices.COMException)
    '            If comEx IsNot Nothing AndAlso comEx.ErrorCode = &H80010108 Then
    '                qtdRpcDesconectado += 1
    '                msg = $"{contexto}: [RPC_E_DISCONNECTED] O objeto do Solid Edge foi desconectado. Detalhe: {ex.Message}"
    '            Else
    '                msg = $"{contexto}: {ex.Message}"
    '            End If
    '            erros.Add(msg)
    '        Catch
    '        End Try
    '    End Sub

    '    ' Helper de Parse Decimal
    '    Dim ParseDecimalFlex As Func(Of Object, Decimal) =
    '    Function(v As Object) As Decimal
    '        Try
    '            If v Is Nothing OrElse v Is DBNull.Value Then Return 0D
    '            Dim s As String = v.ToString().Trim()
    '            If s = "" Then Return 0D
    '            s = s.Replace(",", ".")
    '            Dim d As Decimal
    '            If Decimal.TryParse(s, Globalization.NumberStyles.Any, Globalization.CultureInfo.InvariantCulture, d) Then
    '                Return d
    '            End If
    '            Return 0D
    '        Catch
    '            Return 0D
    '        End Try
    '    End Function

    '    Try
    '        ' 1. Conexão com Solid Edge
    '        If app Is Nothing Then
    '            Try
    '                app = System.Runtime.InteropServices.Marshal.GetActiveObject("SolidEdge.Application")
    '            Catch
    '            End Try
    '        End If

    '        Dim doc As Object = Nothing
    '        Try
    '            If app Is Nothing OrElse app.Documents.Count = 0 Then
    '                MessageBox.Show("Nenhum documento aberto no Solid Edge.", "SINCO", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
    '                Exit Sub
    '            End If
    '            doc = app.ActiveDocument
    '        Catch ex As Exception
    '            MessageBox.Show("Erro ao acessar o Solid Edge: " & ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error)
    '            Exit Sub
    '        End Try

    '        If doc Is Nothing Then Exit Sub

    '        ' 2. UI
    '        pgb.Visible = True
    '        pgb.Style = ProgressBarStyle.Marquee
    '        pgb.Value = 0
    '        lbl.Visible = True
    '        lbl.Text = "Lendo estrutura... aguarde."

    '        ' 3. Tabela
    '        Dim tabela As New DataTable("BOM_Completa")
    '        tabela.Columns.Add("Nivel", GetType(Integer))
    '        tabela.Columns.Add("Arquivo", GetType(String))
    '        tabela.Columns.Add("TipoArquivo", GetType(String))
    '        tabela.Columns.Add("Qtde", GetType(Double))             ' Unitário
    '        tabela.Columns.Add("CaminhoCompleto", GetType(String))
    '        tabela.Columns.Add("TipoLinha", GetType(String))

    '        Dim propsInteressantes As New List(Of String) From {
    '        "Título", "Assunto", "Coment", "Tipo de Desenho", "Palavras-", "Autor", "Empresa",
    '        "Categoria", "Gerente", "Material", "Data", "DataR", "Data1",
    '        "Revision", "CutSizeX", "CutSizeY", "Thickness",
    '        "Mass", "Área_de_superfície", "Area_de_superficie", "Bloqueado"
    '    }
    '        For Each p In propsInteressantes
    '            If Not tabela.Columns.Contains(p) Then tabela.Columns.Add(p, GetType(String))
    '        Next

    '        tabela.Columns.Add("RNC", GetType(String))
    '        tabela.Columns.Add("Fator", GetType(Double))
    '        tabela.Columns.Add("QtdeTotal", GetType(Double))        ' Acumulado
    '        tabela.Columns.Add("PesoTotal", GetType(Double))
    '        tabela.Columns.Add("AreaPinturaTotal", GetType(Double))
    '        tabela.Columns.Add("CodMatFabricante", GetType(String))
    '        tabela.Columns.Add("DescDetal", GetType(String))
    '        tabela.Columns.Add("QtdeMaterial", GetType(Double))
    '        tabela.Columns.Add("PesoMaterial", GetType(Double))

    '        Dim totalLidos As Integer = 0
    '        Dim totalEstimado As Integer = 1

    '        ' Helpers de Formatação
    '        Dim LimparUnidades As Func(Of String, String) =
    '        Function(txt As String) As String
    '            If String.IsNullOrWhiteSpace(txt) Then Return ""
    '            Return txt.Replace("mm²", "").Replace("MM²", "").Replace("mm", "").Replace("MM", "").Replace("^2", "").Replace("KG", "").Replace("kg", "").Trim()
    '        End Function

    '        Dim FormatarValor As Func(Of String, String, String) =
    '        Function(propNome As String, valorBruto As String) As String
    '            If String.IsNullOrWhiteSpace(valorBruto) Then Return ""
    '            Dim nome = propNome.Trim().ToUpperInvariant()
    '            Dim valor = LimparUnidades(valorBruto)
    '            If nome = "DATA" OrElse nome = "DATAR" OrElse nome = "DATA1" Then
    '                Try
    '                    Return cl_BancoDados.FormatarData(valor)
    '                Catch
    '                    Return valor
    '                End Try
    '            End If
    '            If nome.Contains("REVISION") Then Return valor.ToUpperInvariant()
    '            If nome.Contains("CUTSIZEX") OrElse nome.Contains("CUTSIZEY") OrElse nome.Contains("THICKNESS") OrElse nome.Contains("MASS") Then
    '                Return valor.Replace(",", ".")
    '            End If
    '            If nome.Contains("ÁREA") OrElse nome.Contains("AREA") Then
    '                Dim num As Double
    '                If Double.TryParse(valor.Replace(",", "."), Globalization.NumberStyles.Any, Globalization.CultureInfo.InvariantCulture, num) Then
    '                    Dim m2 = num / 1_000_000.0R
    '                    Return m2.ToString("N4", Globalization.CultureInfo.GetCultureInfo("pt-BR"))
    '                End If
    '            End If
    '            Return valor
    '        End Function

    '        ' PreencherPropriedades (Expandido para evitar erro BC30030)
    '        Dim PreencherPropriedades As Action(Of Object, DataRow) =
    '        Sub(docObjP As Object, linhaP As DataRow)
    '            If linhaP Is Nothing Then Exit Sub
    '            Dim tentativas As Integer = 0
    '            Dim abriuTemp As Boolean = False

    '            While tentativas < 2
    '                Try
    '                    If docObjP Is Nothing Then
    '                        Dim caminhoArquivo As String = ""
    '                        Try
    '                            caminhoArquivo = Convert.ToString(linhaP("CaminhoCompleto"))
    '                        Catch
    '                            caminhoArquivo = ""
    '                        End Try

    '                        If Not String.IsNullOrWhiteSpace(caminhoArquivo) AndAlso IO.File.Exists(caminhoArquivo) Then
    '                            Try
    '                                docObjP = app.Documents.Open(caminhoArquivo)
    '                                abriuTemp = True
    '                            Catch exOpen As Exception
    '                                RegistrarErro($"Reabrindo doc '{caminhoArquivo}'", exOpen)
    '                                Exit While
    '                            End Try
    '                        Else
    '                            Exit While
    '                        End If
    '                    End If

    '                    Dim propSets As Object = docObjP.Properties
    '                    For Each propSet As Object In propSets
    '                        For Each prop As Object In propSet
    '                            If prop Is Nothing Then Continue For
    '                            Dim nomeProp As String = ""
    '                            Try
    '                                nomeProp = CStr(prop.Name)
    '                            Catch
    '                                Continue For
    '                            End Try
    '                            If String.IsNullOrWhiteSpace(nomeProp) Then Continue For

    '                            For Each alvo In propsInteressantes
    '                                If nomeProp.IndexOf(alvo, StringComparison.OrdinalIgnoreCase) >= 0 Then
    '                                    Dim valorBruto As String = ""
    '                                    Try
    '                                        Dim v = prop.Value
    '                                        If v IsNot Nothing Then valorBruto = v.ToString()
    '                                    Catch ex As Exception
    '                                        valorBruto = "(erro)"
    '                                    End Try
    '                                    linhaP(alvo) = FormatarValor(nomeProp, valorBruto)
    '                                    Exit For
    '                                End If
    '                            Next
    '                            If nomeProp.IndexOf("Material Thickness", StringComparison.OrdinalIgnoreCase) >= 0 OrElse nomeProp.IndexOf("Espessura Do Material", StringComparison.OrdinalIgnoreCase) >= 0 OrElse nomeProp.Equals("Thickness", StringComparison.OrdinalIgnoreCase) Then
    '                                Dim valorBruto As String = ""
    '                                Try
    '                                    Dim v = prop.Value
    '                                    If v IsNot Nothing Then valorBruto = v.ToString()
    '                                Catch
    '                                End Try
    '                                If Not String.IsNullOrWhiteSpace(valorBruto) Then
    '                                    Dim limpo = LimparUnidades(valorBruto).Replace(",", ".")
    '                                    linhaP("Thickness") = limpo
    '                                End If
    '                            End If
    '                        Next
    '                    Next
    '                    Exit While
    '                Catch comEx As System.Runtime.InteropServices.COMException
    '                    If comEx.ErrorCode = &H80010108 Then
    '                        tentativas += 1
    '                        docObjP = Nothing
    '                        Threading.Thread.Sleep(200)
    '                        Continue While
    '                    Else
    '                        Exit While
    '                    End If
    '                Catch ex As Exception
    '                    Exit While
    '                End Try
    '            End While

    '            If abriuTemp AndAlso docObjP IsNot Nothing Then
    '                Try
    '                    Dim isActive As Boolean = False
    '                    Try
    '                        isActive = (TryCast(docObjP, Object) Is app.ActiveDocument)
    '                    Catch
    '                        isActive = False
    '                    End Try
    '                    If Not isActive Then
    '                        Try
    '                            CallByName(docObjP, "Close", CallType.Method, False)
    '                        Catch
    '                        End Try
    '                    End If
    '                Catch
    '                End Try
    '            End If
    '        End Sub

    '        ' AdicionarPeca
    '        Dim AdicionarPeca As Action(Of Integer, String, String, Double, Double, String, Object) =
    '        Sub(nivel As Integer,
    '            nomeComExt As String,
    '            tipoArquivo As String,
    '            qtdeLocal As Double,
    '            qtdeTotal As Double,
    '            caminho As String,
    '            docObjA As Object)

    '            Try
    '                Dim linha = tabela.NewRow()
    '                linha("TipoLinha") = "PECA"
    '                linha("Nivel") = nivel
    '                linha("Arquivo") = nomeComExt
    '                linha("TipoArquivo") = tipoArquivo

    '                ' >>> CÁLCULO NÍVEIS <<<
    '                linha("Qtde") = qtdeLocal           ' Unitário
    '                linha("QtdeTotal") = qtdeTotal      ' Total Acumulado

    '                linha("CaminhoCompleto") = caminho
    '                For Each p In propsInteressantes
    '                    linha(p) = ""
    '                Next
    '                linha("RNC") = ""
    '                linha("Fator") = 1.0R
    '                linha("PesoTotal") = 0.0R
    '                linha("AreaPinturaTotal") = 0.0R
    '                linha("CodMatFabricante") = ""
    '                linha("DescDetal") = ""
    '                linha("QtdeMaterial") = 0.0R
    '                linha("PesoMaterial") = 0.0R

    '                PreencherPropriedades(docObjA, linha)

    '                Dim mass As Double = 0
    '                Double.TryParse(Convert.ToString(linha("Mass")).Replace(",", "."), Globalization.NumberStyles.Any, Globalization.CultureInfo.InvariantCulture, mass)
    '                Dim area As Double = 0
    '                Double.TryParse(Convert.ToString(linha("Área_de_superfície")).Replace(",", "."), Globalization.NumberStyles.Any, Globalization.CultureInfo.InvariantCulture, area)

    '                linha("PesoTotal") = mass * qtdeTotal
    '                linha("AreaPinturaTotal") = area * qtdeTotal
    '                tabela.Rows.Add(linha)

    '                ' Chama Materiais com a lógica Original
    '                Try
    '                    LerDadosViewMontaPecaParaTabela(tabela, nivel, nomeComExt, qtdeTotal)
    '                Catch exMat As Exception
    '                    RegistrarErro($"Materiais para '{nomeComExt}'", exMat)
    '                End Try

    '            Catch ex As Exception
    '                RegistrarErro($"AdicionarPeca [{nomeComExt}]", ex)
    '            End Try
    '        End Sub

    '        ' Recursão Ler (Blocos Try expandidos)
    '        Dim Ler As Action(Of Object, String, Integer, Double, Double) =
    '        Sub(docObjL As Object, caminhoArquivo As String, nivel As Integer, qtdeLocalAtual As Double, qtdeTotalAtual As Double)
    '            Try
    '                If docObjL Is Nothing OrElse String.IsNullOrWhiteSpace(caminhoArquivo) Then Exit Sub
    '                totalLidos += 1
    '                lbl.Text = $"Lendo {IO.Path.GetFileName(caminhoArquivo)} ({totalLidos})"
    '                If totalEstimado > 1 Then
    '                    pgb.Style = ProgressBarStyle.Continuous
    '                    pgb.Value = Math.Min(100, CInt((totalLidos / Math.Max(1, totalEstimado)) * 100))
    '                End If

    '                Dim ext As String = IO.Path.GetExtension(caminhoArquivo).ToLower()
    '                Dim nomeComExt As String = IO.Path.GetFileName(caminhoArquivo)
    '                Dim tipoUpper As String = ext.Replace(".", "").ToUpperInvariant()

    '                AdicionarPeca(nivel, nomeComExt, tipoUpper, qtdeLocalAtual, qtdeTotalAtual, caminhoArquivo, docObjL)

    '                If ext <> ".asm" Then Exit Sub

    '                Dim occsObj As Object = Nothing
    '                Try
    '                    occsObj = CallByName(docObjL, "Occurrences", CallType.Get)
    '                Catch
    '                End Try
    '                If occsObj Is Nothing Then Exit Sub

    '                Dim count As Integer = 0
    '                Try
    '                    count = CInt(CallByName(occsObj, "Count", CallType.Get))
    '                Catch
    '                End Try
    '                If count <= 0 Then Exit Sub
    '                totalEstimado += count

    '                Dim grupo As New Dictionary(Of String, (Caminho As String, QtdeLocal As Double, DocObj As Object))(StringComparer.OrdinalIgnoreCase)

    '                For i As Integer = 1 To count
    '                    Try
    '                        Dim occ As Object = CallByName(occsObj, "Item", CallType.Method, i)
    '                        Dim subPath As String = ""
    '                        Try
    '                            subPath = CStr(CallByName(occ, "OccurrenceFileName", CallType.Get))
    '                        Catch
    '                        End Try
    '                        Dim subDoc As Object = Nothing
    '                        Try
    '                            subDoc = CallByName(occ, "OccurrenceDocument", CallType.Get)
    '                        Catch
    '                        End Try

    '                        If subDoc Is Nothing AndAlso Not String.IsNullOrWhiteSpace(subPath) AndAlso IO.File.Exists(subPath) Then
    '                            Try
    '                                subDoc = app.Documents.Open(subPath)
    '                            Catch
    '                                subDoc = Nothing
    '                            End Try
    '                        End If

    '                        Dim qLocalDec As Decimal = 1D
    '                        Try
    '                            Dim qtdProp As Object = CallByName(occ, "Quantity", CallType.Get)
    '                            Dim qParsed As Decimal = ParseDecimalFlex(qtdProp)
    '                            If qParsed > 0D Then qLocalDec = qParsed
    '                        Catch
    '                        End Try
    '                        Dim qLocal As Double = CDbl(qLocalDec)
    '                        Dim chaveAgrup As String = If(String.IsNullOrWhiteSpace(subPath), $"ITEM_{i}", subPath).ToUpperInvariant()

    '                        If grupo.ContainsKey(chaveAgrup) Then
    '                            Dim t = grupo(chaveAgrup)
    '                            grupo(chaveAgrup) = (t.Caminho, t.QtdeLocal + qLocal, If(t.DocObj Is Nothing, subDoc, t.DocObj))
    '                        Else
    '                            grupo.Add(chaveAgrup, (subPath, qLocal, subDoc))
    '                        End If
    '                    Catch ex As Exception
    '                        RegistrarErro($"Ocorrência #{i}", ex)
    '                    End Try
    '                Next

    '                For Each kv In grupo
    '                    Dim caminhoSub As String = kv.Value.Caminho
    '                    Dim qtdeLocalFilho As Double = kv.Value.QtdeLocal
    '                    Dim subDoc As Object = kv.Value.DocObj
    '                    If String.IsNullOrWhiteSpace(caminhoSub) AndAlso subDoc IsNot Nothing Then
    '                        Try
    '                            caminhoSub = CStr(CallByName(subDoc, "FullName", CallType.Get))
    '                        Catch
    '                            caminhoSub = ""
    '                        End Try
    '                    End If

    '                    ' >>> CÁLCULO RECURSIVO DOS NÍVEIS <<<
    '                    Dim qtdeTotalFilho As Double = qtdeTotalAtual * qtdeLocalFilho

    '                    Dim nomeFilho As String
    '                    Dim tipoFilho As String
    '                    If Not String.IsNullOrWhiteSpace(caminhoSub) Then
    '                        nomeFilho = IO.Path.GetFileName(caminhoSub)
    '                        tipoFilho = IO.Path.GetExtension(caminhoSub).Replace(".", "").ToUpperInvariant()
    '                    Else
    '                        nomeFilho = kv.Key
    '                        tipoFilho = ""
    '                    End If

    '                    If subDoc IsNot Nothing AndAlso Not String.IsNullOrWhiteSpace(caminhoSub) Then
    '                        Ler(subDoc, caminhoSub, nivel + 1, qtdeLocalFilho, qtdeTotalFilho)

    '                        ' Fechamento seguro expandido
    '                        Try
    '                            Dim isActive As Boolean = False
    '                            Try
    '                                isActive = (TryCast(subDoc, Object) Is app.ActiveDocument)
    '                            Catch
    '                                isActive = False
    '                            End Try

    '                            If Not isActive AndAlso IO.File.Exists(caminhoSub) Then
    '                                Try
    '                                    CallByName(subDoc, "Close", CallType.Method, False)
    '                                Catch
    '                                End Try
    '                            End If
    '                        Catch
    '                        End Try
    '                    Else
    '                        AdicionarPeca(nivel + 1, nomeFilho, tipoFilho, qtdeLocalFilho, qtdeTotalFilho, caminhoSub, subDoc)
    '                    End If
    '                Next
    '            Catch ex As Exception
    '                RegistrarErro($"Ler('{caminhoArquivo}')", ex)
    '            End Try
    '        End Sub

    '        Ler(doc, doc.FullName, 0, 1.0R, 1.0R)

    '        For i As Integer = tabela.Rows.Count - 1 To 0 Step -1
    '            Dim arq As String = Convert.ToString(tabela.Rows(i)("Arquivo"))
    '            If String.IsNullOrWhiteSpace(arq) AndAlso String.Equals(Convert.ToString(tabela.Rows(i)("TipoLinha")), "PECA", StringComparison.OrdinalIgnoreCase) Then
    '                tabela.Rows.RemoveAt(i)
    '            End If
    '        Next

    '        Dim totalQtdSolicitada As Double = 0
    '        Dim arquivosUnicos As New HashSet(Of String)(StringComparer.OrdinalIgnoreCase)
    '        For Each row As DataRow In tabela.Rows
    '            If Not String.Equals(Convert.ToString(row("TipoLinha")), "PECA", StringComparison.OrdinalIgnoreCase) Then Continue For
    '            Dim qtd As Double = 0
    '            Double.TryParse(Convert.ToString(row("QtdeTotal")).Replace(",", "."), Globalization.NumberStyles.Any, Globalization.CultureInfo.InvariantCulture, qtd)
    '            totalQtdSolicitada += qtd
    '            Dim arq As String = Convert.ToString(row("Arquivo"))
    '            If Not String.IsNullOrWhiteSpace(arq) Then arquivosUnicos.Add(arq)
    '        Next

    '        Dim totalPecasDiferentes As Integer = arquivosUnicos.Count
    '        Dim totalPecas As Integer = tabela.Rows.Cast(Of DataRow)().Count(Function(r) String.Equals(Convert.ToString(r("TipoLinha")), "PECA", StringComparison.OrdinalIgnoreCase))

    '        dgv.DataSource = tabela
    '        dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells
    '        dgv.AllowUserToAddRows = False
    '        dgv.ColumnHeadersDefaultCellStyle.Font = New Font(dgv.Font, FontStyle.Bold)
    '        If dgv.Columns.Contains("Arquivo") Then dgv.Columns("Arquivo").Frozen = True
    '        Dim ocultar() As String = {"TipoArquivo", "CaminhoCompleto", "Categoria", "Gerente", "Autor", "Empresa", "Data", "DataR", "Data1", "Revision", "CutSizeX", "CutSizeY", "Mass", "Área_de_superfície", "Area_de_superficie", "Bloqueado", "RNC", "PesoTotal", "AreaPinturaTotal", "QtdeMaterial", "PesoMaterial", "CodMatFabricante", "DescDetal"}
    '        For Each c In ocultar
    '            If dgv.Columns.Contains(c) Then dgv.Columns(c).Visible = False
    '        Next

    '        pgb.Style = ProgressBarStyle.Continuous
    '        pgb.Value = 100
    '        lblResumo.Text = "✅ Estrutura concluída." & vbCrLf & $"• Total Qtde: {totalQtdSolicitada}" & vbCrLf & $"• Arquivos Únicos: {totalPecasDiferentes}"
    '        Threading.Thread.Sleep(550)

    '    Catch ex As Exception
    '        RegistrarErro("Geral", ex)
    '    Finally
    '        pgb.Visible = False
    '        If erros IsNot Nothing AndAlso erros.Count > 0 Then MessageBox.Show($"Erros encontrados: {erros.Count}", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning)
    '    End Try
    'End Sub

    ''===============================================================================
    '' 🔴 LÊ MATERIAIS PARA TABELA - LÓGICA ORIGINAL RESTAURADA + SEGURA
    ''===============================================================================
    'Public Sub LerDadosViewMontaPecaParaTabela(tabelaDest As DataTable,
    '                                           nivelPeca As Integer,
    '                                           nomeArquivoComExt As String,
    '                                           qtdeTotalAcumuladaPeca As Double)

    '    If TabelaViewMontaPeca Is Nothing Then Exit Sub

    '    ' 👉 Usa o nome COM extensão, igual está em NomeArquivoSemExtensao na view (conforme seu código original)
    '    Dim chaveArquivo As String = IO.Path.GetFileName(nomeArquivoComExt)

    '    Try
    '        DadosArquivoCorrente.NomeArquivoSemExtensao = chaveArquivo
    '    Catch
    '    End Try
    '    Try
    '        DadosArquivoCorrente.qtde = CInt(qtdeTotalAcumuladaPeca)
    '    Catch
    '    End Try

    '    TabelaViewMontaPeca.CaseSensitive = False
    '    Dim dv As New DataView(TabelaViewMontaPeca)
    '    Dim filtro As String = chaveArquivo.Replace("'", "''")
    '    dv.RowFilter = $"NomeArquivoSemExtensao = '{filtro}'"

    '    Dim dtFiltrado As DataTable = dv.ToTable()

    '    For Each linha As DataRow In dtFiltrado.Rows
    '        Try
    '            ' Confirma novamente ignorando maiúsc/minúsc (por segurança)
    '            If Not String.Equals(linha("NomeArquivoSemExtensao").ToString().Trim(), chaveArquivo.Trim(), StringComparison.OrdinalIgnoreCase) Then Continue For

    '            iconeTipoArquivo = My.Resources.sinco_Diversos

    '            ' 1) Quantidades base
    '            Dim PecaQtde As Double
    '            Dim peso As Double
    '            Dim pecaQtdeStr As String = linha.Item("PecaQtde").ToString().Trim().Replace(",", ".")
    '            PecaQtde = cl_BancoDados.converteStringParaDouble(pecaQtdeStr)
    '            Dim pecaPesoStr As String = linha.Item("Peso").ToString().Trim().Replace(",", ".")
    '            peso = cl_BancoDados.converteStringParaDouble(pecaPesoStr)

    '            ' qtde da peça pai: usa qtdeTotalAcumuladaPeca recebido (que é o acumulado correto da árvore)
    '            Dim qtdPecaPai As Double = qtdeTotalAcumuladaPeca

    '            ' Fallback (mantido do original, busca na tabela se necessário, embora qtdPecaPai já deva vir correta)
    '            Try
    '                Dim filtroPai As String = "TipoLinha = 'PECA' AND Arquivo = '" & chaveArquivo.Replace("'", "''") & "' AND Nivel = " & nivelPeca.ToString()
    '                Dim rowsPeca() As DataRow = tabelaDest.Select(filtroPai)
    '                If rowsPeca IsNot Nothing AndAlso rowsPeca.Length > 0 Then
    '                    Dim tmp As Double
    '                    If Double.TryParse(Convert.ToString(rowsPeca(rowsPeca.Length - 1)("QtdeTotal")), Globalization.NumberStyles.Any, Globalization.CultureInfo.InvariantCulture, tmp) Then
    '                        qtdPecaPai = tmp
    '                    End If
    '                End If
    '            Catch
    '            End Try

    '            Dim fator As Double = 1.0R

    '            ' 2) Cálculos conforme a REGRA ORIGINAL:
    '            Dim QtdeMaterial As Double = PecaQtde                     ' por peça
    '            Dim QtdeVisivel As Double = QtdeMaterial                  ' coluna Qtde (Unitário)

    '            ' Ajuste aqui: QtdeTotalMaterial deve usar a qtdPecaPai que vem acumulada da recursão
    '            Dim QtdeTotalMaterial As Double = QtdeMaterial * qtdPecaPai * fator

    '            Dim PesoMaterial As Double = 0
    '            Try
    '                PesoMaterial = peso * QtdeTotalMaterial
    '            Catch
    '            End Try

    '            ' 3) Monta linha de MATERIAL na tabela
    '            Dim drMat As DataRow = tabelaDest.NewRow()
    '            drMat("TipoLinha") = "MATERIAL"
    '            drMat("Nivel") = nivelPeca + 1

    '            Dim cod As String = linha.Item("CodMatFabricante").ToString().Trim().ToUpper()
    '            drMat("Arquivo") = cod
    '            drMat("CodMatFabricante") = cod
    '            drMat("TipoArquivo") = "MAT"

    '            ' 🔹 Qtde (VISÍVEL) = Qtde de material POR PEÇA
    '            drMat("Qtde") = QtdeVisivel

    '            ' 🔹 QtdeMaterial (INTERNA)
    '            drMat("QtdeMaterial") = QtdeMaterial
    '            drMat("CaminhoCompleto") = ""

    '            Dim desc As String = linha.Item("DescDetal").ToString().Trim().ToUpper()
    '            Dim matView As String = ""
    '            Try
    '                matView = linha.Item("Material").ToString().Trim().ToUpper()
    '            Catch
    '                matView = ""
    '            End Try

    '            Dim titulo As String = desc
    '            If matView <> "" Then titulo &= " - " & matView
    '            drMat("Título") = titulo
    '            If matView <> "" Then drMat("Material") = matView
    '            drMat("Tipo de Desenho") = "MATERIAL"
    '            drMat("RNC") = ""
    '            drMat("Fator") = fator

    '            ' 🔹 QtdeTotal = Qtde (por peça) * Qtde da peça pai * fator
    '            drMat("QtdeTotal") = QtdeTotalMaterial
    '            drMat("PesoTotal") = 0.0R
    '            drMat("AreaPinturaTotal") = 0.0R
    '            drMat("PesoMaterial") = PesoMaterial

    '            tabelaDest.Rows.Add(drMat)

    '        Catch
    '            Continue For
    '        End Try
    '    Next
    'End Sub

#End Region

#Region "VERSÃO DEFINITIVA - DADOS RESTAURADOS + CÁLCULOS CORRETOS"


    '================================================================================
    ' Função Global de Conversão Segura (Movemos para fora para ser compartilhada)
    '================================================================================
    Private Function ParseSeguro(ByVal v As Object) As Double
        Try
            If v Is Nothing OrElse IsDBNull(v) Then Return 0.0R
            Dim s As String = v.ToString().Trim()
            If s = "" Then Return 0.0R
            ' Força ponto como separador decimal para garantir o TryParse Invariant
            s = s.Replace(",", ".")
            Dim d As Double
            If Double.TryParse(s, Globalization.NumberStyles.Any, Globalization.CultureInfo.InvariantCulture, d) Then
                Return d
            End If
            Return 0.0R
        Catch
            Return 0.0R
        End Try
    End Function

    '================================================================================
    ' Monta estrutura BOM + MATERIAIS no MESMO DataGridView
    '================================================================================
    Public Sub ListarEstruturaBomMateriais(ByVal dgv As DataGridView,
                                           ByVal pgb As ProgressBar,
                                           ByVal lbl As Label,
                                           ByVal lblResumo As Label)

        Dim erros As New List(Of String)
        Dim qtdRpcDesconectado As Integer = 0

        ' Helper para registrar erros
        Dim RegistrarErro As Action(Of String, Exception) =
        Sub(contexto As String, ex As Exception)
            Try
                If ex Is Nothing Then Return
                Dim msg As String = $"{contexto}: {ex.Message}"
                If ex.Message.Contains("RPC_E_DISCONNECTED") OrElse ex.HResult = &H80010108 Then
                    qtdRpcDesconectado += 1
                    msg = $"{contexto}: [Solid Edge Desconectado] {ex.Message}"
                End If
                erros.Add(msg)
            Catch
            End Try
        End Sub

        ' Helper de Parse Decimal Local
        Dim ParseDecimalFlex As Func(Of Object, Decimal) =
        Function(v As Object) As Decimal
            Try
                If v Is Nothing OrElse IsDBNull(v) Then Return 0D
                Dim s As String = v.ToString().Trim()
                If s = "" Then Return 0D
                s = s.Replace(",", ".")
                Dim d As Decimal
                If Decimal.TryParse(s, Globalization.NumberStyles.Any, Globalization.CultureInfo.InvariantCulture, d) Then
                    Return d
                End If
                Return 0D
            Catch
                Return 0D
            End Try
        End Function

        Try
            ' 1. Conexão com Solid Edge
            If app Is Nothing Then
                Try
                    app = System.Runtime.InteropServices.Marshal.GetActiveObject("SolidEdge.Application")
                Catch
                End Try
            End If

            Dim doc As Object = Nothing
            Try
                If app Is Nothing OrElse app.Documents.Count = 0 Then
                    MessageBox.Show("Nenhum documento aberto no Solid Edge.", "SINCO", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    Exit Sub
                End If
                doc = app.ActiveDocument
            Catch ex As Exception
                MessageBox.Show("Erro conexão SE: " & ex.Message)
                Exit Sub
            End Try

            If doc Is Nothing Then Exit Sub

            ' 2. UI
            pgb.Visible = True
            pgb.Style = ProgressBarStyle.Marquee
            pgb.Value = 0
            lbl.Visible = True
            lbl.Text = "Lendo estrutura..."

            ' 3. Tabela
            Dim tabela As New DataTable("BOM_Completa")
            tabela.Columns.Add("Nivel", GetType(Integer))
            tabela.Columns.Add("Arquivo", GetType(String))
            tabela.Columns.Add("TipoArquivo", GetType(String))
            tabela.Columns.Add("Qtde", GetType(Double))             ' Unitário
            tabela.Columns.Add("CaminhoCompleto", GetType(String))
            tabela.Columns.Add("TipoLinha", GetType(String))

            Dim propsInteressantes As New List(Of String) From {
                "Título", "Assunto", "Coment", "Tipo de Desenho", "Palavras-", "Autor", "Empresa",
                "Categoria", "Gerente", "Material", "Data", "DataR", "Data1",
                "Revision", "CutSizeX", "CutSizeY", "Thickness",
                "Mass", "Área_de_superfície", "Area_de_superficie", "Bloqueado"
            }

            For Each p In propsInteressantes
                If Not tabela.Columns.Contains(p) Then
                    tabela.Columns.Add(p, GetType(String))
                End If
            Next

            tabela.Columns.Add("RNC", GetType(String))
            tabela.Columns.Add("Fator", GetType(Double))
            tabela.Columns.Add("QtdeTotal", GetType(Double))        ' Acumulado
            tabela.Columns.Add("PesoTotal", GetType(Double))
            tabela.Columns.Add("AreaPinturaTotal", GetType(Double))
            tabela.Columns.Add("CodMatFabricante", GetType(String))
            tabela.Columns.Add("DescDetal", GetType(String))
            tabela.Columns.Add("QtdeMaterial", GetType(Double))
            tabela.Columns.Add("PesoMaterial", GetType(Double))
            '   tabela.Columns.Add("NovoRevisao", GetType(Double))

            Dim totalLidos As Integer = 0
            Dim totalEstimado As Integer = 1

            ' Helper Formatação Original
            Dim LimparUnidades As Func(Of String, String) =
            Function(txt)
                If String.IsNullOrWhiteSpace(txt) Then Return ""
                Return txt.Replace("mm²", "").Replace("MM²", "").Replace("mm", "").Replace("MM", "").Replace("^2", "").Replace("KG", "").Replace("kg", "").Trim()
            End Function

            Dim FormatarValor As Func(Of String, String, String) =
            Function(propNome As String, valorBruto As String) As String
                If String.IsNullOrWhiteSpace(valorBruto) Then Return ""
                Dim nome = propNome.Trim().ToUpperInvariant()
                Dim valor = LimparUnidades(valorBruto)

                If nome = "DATA" OrElse nome = "DATAR" OrElse nome = "DATA1" Then
                    Try
                        Return cl_BancoDados.FormatarData(valor)
                    Catch
                        Return valor
                    End Try
                End If
                If nome.Contains("REVISION") Then Return valor.ToUpperInvariant()
                If nome.Contains("CUTSIZEX") OrElse nome.Contains("CUTSIZEY") OrElse nome.Contains("THICKNESS") OrElse nome.Contains("MASS") Then
                    Return valor.Replace(",", ".")
                End If
                If nome.Contains("ÁREA") OrElse nome.Contains("AREA") Then
                    Dim num As Double
                    If Double.TryParse(valor.Replace(",", "."), Globalization.NumberStyles.Any, Globalization.CultureInfo.InvariantCulture, num) Then
                        Dim m2 = num / 1_000_000.0R
                        Return m2.ToString("N4", Globalization.CultureInfo.GetCultureInfo("pt-BR"))
                    End If
                End If
                Return valor
            End Function

            ' ====================================================================
            ' PreencherPropriedades (RESTAURADA A ORIGINAL QUE LÊ TUDO)
            ' ====================================================================
            Dim PreencherPropriedades As Action(Of Object, DataRow) =
            Sub(docObjP, linhaP)
                If linhaP Is Nothing Then Exit Sub
                Dim tentativas As Integer = 0
                Dim abriuTemp As Boolean = False

                While tentativas < 2
                    Try
                        If docObjP Is Nothing Then
                            Dim caminhoArquivo As String = ""
                            Try
                                caminhoArquivo = Convert.ToString(linhaP("CaminhoCompleto"))
                            Catch
                            End Try

                            If Not String.IsNullOrWhiteSpace(caminhoArquivo) AndAlso IO.File.Exists(caminhoArquivo) Then
                                Try
                                    docObjP = app.Documents.Open(caminhoArquivo)
                                    abriuTemp = True
                                Catch exOpen As Exception
                                    RegistrarErro($"Reabrindo doc '{caminhoArquivo}'", exOpen)
                                    Exit While
                                End Try
                            Else
                                Exit While
                            End If
                        End If

                        ' --- LOOP ORIGINAL RESTAURADO ---
                        Dim propSets As Object = docObjP.Properties
                        For Each propSet As Object In propSets
                            For Each prop As Object In propSet
                                If prop Is Nothing Then Continue For

                                Dim nomeProp As String = ""
                                Try
                                    nomeProp = CStr(prop.Name)
                                Catch
                                    Continue For
                                End Try

                                If String.IsNullOrWhiteSpace(nomeProp) Then Continue For

                                ' Loop pelas propriedades de interesse
                                For Each alvo In propsInteressantes
                                    If nomeProp.IndexOf(alvo, StringComparison.OrdinalIgnoreCase) >= 0 Then
                                        Dim valorBruto As String = ""
                                        Try
                                            Dim v = prop.Value
                                            If v IsNot Nothing Then valorBruto = v.ToString()
                                        Catch ex As Exception
                                            valorBruto = "(erro)"
                                            RegistrarErro($"Lendo propriedade '{nomeProp}'", ex)
                                        End Try

                                        ' Aplica formatação
                                        linhaP(alvo) = FormatarValor(nomeProp, valorBruto)
                                        Exit For
                                    End If
                                Next

                                ' Loop específico para Espessura
                                If nomeProp.IndexOf("Material Thickness", StringComparison.OrdinalIgnoreCase) >= 0 OrElse
                                   nomeProp.IndexOf("Espessura Do Material", StringComparison.OrdinalIgnoreCase) >= 0 OrElse
                                   nomeProp.Equals("Thickness", StringComparison.OrdinalIgnoreCase) Then

                                    Dim valorBruto As String = ""
                                    Try
                                        Dim v = prop.Value
                                        If v IsNot Nothing Then valorBruto = v.ToString()
                                    Catch ex As Exception
                                        RegistrarErro($"Lendo espessura em '{nomeProp}'", ex)
                                    End Try

                                    If Not String.IsNullOrWhiteSpace(valorBruto) Then
                                        linhaP("Thickness") = LimparUnidades(valorBruto).Replace(",", ".")
                                    End If
                                End If
                            Next
                        Next
                        ' -------------------------------

                        Exit While
                    Catch comEx As System.Runtime.InteropServices.COMException
                        If comEx.ErrorCode = &H80010108 Then
                            tentativas += 1
                            docObjP = Nothing
                            Threading.Thread.Sleep(200)
                            Continue While
                        Else
                            RegistrarErro("PreencherPropriedades COM", comEx)
                            Exit While
                        End If
                    Catch ex As Exception
                        RegistrarErro("PreencherPropriedades", ex)
                        Exit While
                    End Try
                End While

                If abriuTemp AndAlso docObjP IsNot Nothing Then
                    Try
                        Dim isActive As Boolean = False
                        Try
                            isActive = (TryCast(docObjP, Object) Is app.ActiveDocument)
                        Catch
                            isActive = False
                        End Try
                        If Not isActive Then
                            CallByName(docObjP, "Close", CallType.Method, False)
                        End If
                    Catch
                    End Try
                End If
            End Sub

            ' ====================================================================
            ' Adicionar Processos da Peça
            ' ====================================================================
            Dim AdicionarProcessos As Action(Of Integer, String, Double) =
            Sub(nivelProc, codPecaProc, qTotProc)
                Try
                    Dim nomeLimpo As String = IO.Path.GetFileNameWithoutExtension(codPecaProc)
                    Dim sqlProc As String = "SELECT mp.IdProcesso, p.processofabricacao AS processofabricacao, mp.SequenciaExecucao " &
                                            "FROM material_processo mp " &
                                            "LEFT JOIN processofabricacao p ON p.idprocessofabricacao = mp.IdProcesso " &
                                            "WHERE mp.codmatfabricante = '" & nomeLimpo.Replace("'", "''") & "' " &
                                            "ORDER BY mp.SequenciaExecucao"

                    Dim dtProc As DataTable = cl_BancoDados.CarregarDados(sqlProc)
                    If dtProc IsNot Nothing AndAlso dtProc.Rows.Count > 0 Then
                        For Each linhaProc As DataRow In dtProc.Rows
                            Dim nomeProc As String = ""
                            Try : nomeProc = Convert.ToString(linhaProc("processofabricacao")) : Catch : End Try

                            Dim seqProc As String = ""
                            Try : seqProc = Convert.ToString(linhaProc("SequenciaExecucao")) : Catch : End Try

                            Dim drProc As DataRow = tabela.NewRow()
                            drProc("TipoLinha") = "PROCESSO"
                            drProc("Nivel") = nivelProc

                            Dim finalName As String = "[" & seqProc & "] " & nomeProc
                            drProc("Arquivo") = finalName
                            drProc("Título") = finalName
                            drProc("TipoArquivo") = "PROC"
                            drProc("CaminhoCompleto") = ""

                            For Each pCol In propsInteressantes
                                If drProc.Table.Columns.Contains(pCol) Then
                                    drProc(pCol) = ""
                                End If
                            Next

                            drProc("Tipo de Desenho") = "PROCESSO"
                            drProc("Qtde") = 0.0R
                            drProc("QtdeTotal") = qTotProc
                            drProc("Fator") = 1.0R
                            drProc("RNC") = ""
                            drProc("PesoTotal") = 0.0R
                            drProc("AreaPinturaTotal") = 0.0R
                            drProc("PesoMaterial") = 0.0R
                            drProc("QtdeMaterial") = 0.0R
                            drProc("CodMatFabricante") = ""
                            drProc("DescDetal") = ""

                            tabela.Rows.Add(drProc)
                        Next
                    End If
                Catch ex As Exception
                    RegistrarErro("Erro Add Processos " & codPecaProc, ex)
                End Try
            End Sub

            ' ====================================================================
            ' AdicionarPeca
            ' ====================================================================
            Dim AdicionarPeca As Action(Of Integer, String, String, Double, Double, String, Object) =
            Sub(nivel, nome, tipo, qLocal, qTotal, caminho, objDoc)
                Try
                    Dim linha = tabela.NewRow()
                    linha("TipoLinha") = "PECA"
                    linha("Nivel") = nivel
                    linha("Arquivo") = nome
                    linha("TipoArquivo") = tipo

                    linha("Qtde") = qLocal
                    linha("QtdeTotal") = qTotal

                    linha("CaminhoCompleto") = caminho
                    For Each p In propsInteressantes
                        linha(p) = ""
                    Next
                    linha("Fator") = 1.0R
                    linha("RNC") = ""
                    linha("PesoTotal") = 0.0R
                    linha("AreaPinturaTotal") = 0.0R
                    linha("CodMatFabricante") = ""
                    linha("DescDetal") = ""
                    linha("QtdeMaterial") = 0.0R
                    linha("PesoMaterial") = 0.0R

                    ' Aqui lê os dados (Título, etc)
                    PreencherPropriedades(objDoc, linha)

                    ' Recalcula Peso/Área com os dados que acabaram de ser lidos
                    Dim m As Double = ParseDecimalFlex(linha("Mass"))
                    Dim a As Double = ParseDecimalFlex(linha("Área_de_superfície"))
                    linha("PesoTotal") = m * qTotal
                    linha("AreaPinturaTotal") = a * qTotal
                    '   linha("NovoRevisao") = ""

                    tabela.Rows.Add(linha)

                    Try
                        LerDadosViewMontaPecaParaTabela(tabela, nivel, nome, qTotal)
                    Catch ex As Exception
                        RegistrarErro("Erro Materiais " & nome, ex)
                    End Try

                    Try
                        AdicionarProcessos(nivel + 1, nome, qTotal)
                    Catch ex As Exception
                        RegistrarErro("Erro Processos " & nome, ex)
                    End Try

                Catch ex As Exception
                    RegistrarErro("AdicionarPeca " & nome, ex)
                End Try
            End Sub

            ' ====================================================================
            ' Recursão Ler
            ' ====================================================================
            Dim Ler As Action(Of Object, String, Integer, Double, Double) =
            Sub(obj, path, niv, qLoc, qTot)
                Try
                    If obj Is Nothing Then Exit Sub
                    totalLidos += 1
                    lbl.Text = "Lendo: " & IO.Path.GetFileName(path)

                    Dim ext As String = IO.Path.GetExtension(path).ToLower()
                    Dim nome As String = IO.Path.GetFileName(path)
                    Dim tipo As String = ext.Replace(".", "").ToUpper()

                    AdicionarPeca(niv, nome, tipo, qLoc, qTot, path, obj)

                    If ext <> ".asm" Then Exit Sub

                    Dim occs As Object = Nothing
                    Try
                        occs = CallByName(obj, "Occurrences", CallType.Get)
                    Catch
                    End Try
                    If occs Is Nothing Then Exit Sub

                    Dim count As Integer = 0
                    Try
                        count = CInt(CallByName(occs, "Count", CallType.Get))
                    Catch
                    End Try
                    totalEstimado += count

                    Dim grupo As New Dictionary(Of String, (Caminho As String, Qtde As Double, Obj As Object))

                    For i As Integer = 1 To count
                        Try
                            Dim occ = CallByName(occs, "Item", CallType.Method, i)
                            Dim subPath As String = ""
                            Try
                                subPath = CStr(CallByName(occ, "OccurrenceFileName", CallType.Get))
                            Catch
                            End Try

                            Dim subDoc As Object = Nothing
                            Try
                                subDoc = CallByName(occ, "OccurrenceDocument", CallType.Get)
                            Catch
                            End Try

                            If subDoc Is Nothing AndAlso IO.File.Exists(subPath) Then
                                Try
                                    subDoc = app.Documents.Open(subPath)
                                Catch
                                End Try
                            End If

                            Dim qItem As Double = 1.0
                            Try
                                qItem = ParseDecimalFlex(CallByName(occ, "Quantity", CallType.Get))
                            Catch
                            End Try

                            If qItem <= 0 Then qItem = 1.0

                            Dim key As String = ""
                            If String.IsNullOrEmpty(subPath) Then
                                key = "ITEM" & i
                            Else
                                key = subPath
                            End If

                            If grupo.ContainsKey(key) Then
                                Dim t = grupo(key)
                                grupo(key) = (t.Caminho, t.Qtde + qItem, If(t.Obj Is Nothing, subDoc, t.Obj))
                            Else
                                grupo.Add(key, (subPath, qItem, subDoc))
                            End If
                        Catch
                        End Try
                    Next

                    For Each kv In grupo
                        Dim qFilhoLocal = kv.Value.Qtde
                        Dim qFilhoTotal = qTot * qFilhoLocal

                        Dim sPath = kv.Value.Caminho
                        Dim sObj = kv.Value.Obj

                        If sObj IsNot Nothing Then
                            Ler(sObj, sPath, niv + 1, qFilhoLocal, qFilhoTotal)
                            Try
                                Dim active As Boolean = False
                                Try
                                    active = (TryCast(sObj, Object) Is app.ActiveDocument)
                                Catch
                                End Try

                                If Not active Then
                                    CallByName(sObj, "Close", CallType.Method, False)
                                End If
                            Catch
                            End Try
                        Else
                            Dim sNome = IO.Path.GetFileName(sPath)
                            Dim sTipo = IO.Path.GetExtension(sPath).Replace(".", "").ToUpper()
                            AdicionarPeca(niv + 1, sNome, sTipo, qFilhoLocal, qFilhoTotal, sPath, Nothing)
                        End If
                    Next
                Catch ex As Exception
                    RegistrarErro("Recursão " & path, ex)
                End Try
            End Sub

            Ler(doc, doc.FullName, 0, 1.0R, 1.0R)

            ' Limpeza e Totais
            Dim qtdTotalGeral As Double = 0
            For i As Integer = tabela.Rows.Count - 1 To 0 Step -1
                Dim row = tabela.Rows(i)
                Dim arq As String = row("Arquivo").ToString()

                If row("TipoLinha") = "PECA" AndAlso String.IsNullOrEmpty(arq) Then
                    tabela.Rows.RemoveAt(i)
                ElseIf row("TipoLinha") = "PECA" Then
                    qtdTotalGeral += CDbl(row("QtdeTotal"))
                End If
            Next

            dgv.DataSource = tabela
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells

            Dim ocultar() As String = {"TipoArquivo", "CaminhoCompleto", "Categoria", "Gerente", "Autor", "Empresa", "Data", "DataR", "Data1", "Revision", "CutSizeX", "CutSizeY", "Mass", "Área_de_superfície", "Area_de_superficie", "Bloqueado", "RNC", "PesoTotal", "AreaPinturaTotal", "QtdeMaterial", "PesoMaterial", "CodMatFabricante", "DescDetal"}

            For Each c In ocultar
                If dgv.Columns.Contains(c) Then
                    dgv.Columns(c).Visible = False
                End If
            Next

            lblResumo.Text = $"Total de Peças: {qtdTotalGeral}"
            pgb.Visible = False

        Catch ex As Exception
            MessageBox.Show("Erro Fatal: " & ex.Message)
        End Try
    End Sub

    '===============================================================================
    ' LÊ MATERIAIS PARA TABELA - CORREÇÃO VISUAL DA COLUNA QTDE
    '===============================================================================
    Public Sub LerDadosViewMontaPecaParaTabela(tabelaDest As DataTable,
                                               nivelPeca As Integer,
                                               nomeArquivoComExt As String,
                                               qtdeTotalAcumuladaPeca As Double)

        If TabelaViewMontaPeca Is Nothing Then Exit Sub

        Dim chaveArquivo As String = IO.Path.GetFileName(nomeArquivoComExt)

        TabelaViewMontaPeca.CaseSensitive = False
        Dim dv As New DataView(TabelaViewMontaPeca)
        Dim filtro As String = chaveArquivo.Replace("'", "''")
        dv.RowFilter = $"NomeArquivoSemExtensao = '{filtro}'"

        For Each linha As DataRow In dv.ToTable().Rows
            Try
                If Not String.Equals(linha("NomeArquivoSemExtensao").ToString().Trim(), chaveArquivo.Trim(), StringComparison.OrdinalIgnoreCase) Then
                    Continue For
                End If

                iconeTipoArquivo = My.Resources.sinco_Diversos

                ' 1) PARSE (Lê os dados do banco)
                Dim PecaQtde As Double = ParseSeguro(linha("PecaQtde"))
                Dim peso As Double = ParseSeguro(linha("Peso"))

                ' 2) CÁLCULO DO MATERIAL
                ' QtdeMaterialUnitario = Quanto gasta em 1 peça (ex: 0,125)
                ' QtdeMaterialTotal    = Quanto gasta em TODAS as peças (ex: 0,125 * 4 = 0,5)
                Dim QtdeMaterialUnitario As Double = PecaQtde
                Dim QtdeMaterialTotal As Double = QtdeMaterialUnitario * qtdeTotalAcumuladaPeca

                Dim PesoMaterialTotal As Double = peso * QtdeMaterialTotal

                ' 3) PREENCHE A LINHA
                Dim drMat As DataRow = tabelaDest.NewRow()
                drMat("TipoLinha") = "MATERIAL"
                drMat("Nivel") = nivelPeca + 1

                Dim cod As String = linha("CodMatFabricante").ToString().Trim().ToUpper()
                drMat("Arquivo") = cod
                drMat("CodMatFabricante") = cod
                drMat("TipoArquivo") = "MAT"

                ' -----------------------------------------------------------------------
                ' 🔴 CORREÇÃO AQUI:
                ' Antes estava: drMat("Qtde") = QtdeMaterialUnitario  (Mostrava 0,125)
                ' Agora fica:   drMat("Qtde") = QtdeMaterialTotal     (Mostra 0,5)
                ' Isso faz com que a coluna visual 'Qtde' já mostre o valor multiplicado pela quantidade da peça pai.
                ' -----------------------------------------------------------------------
                drMat("Qtde") = QtdeMaterialTotal

                ' Coluna de totais (mantém igual para cálculo)
                drMat("QtdeTotal") = QtdeMaterialTotal

                ' Guarda o unitário apenas na coluna interna se precisar depois
                drMat("QtdeMaterial") = QtdeMaterialUnitario
                drMat("CaminhoCompleto") = ""

                Dim desc As String = linha("DescDetal").ToString().Trim().ToUpper()
                Dim matView As String = ""
                Try : matView = linha("Material").ToString().Trim().ToUpper() : Catch : End Try

                Dim titulo As String = desc
                If matView <> "" Then titulo &= " - " & matView
                drMat("Título") = titulo
                If matView <> "" Then drMat("Material") = matView
                drMat("Tipo de Desenho") = "MATERIAL"
                drMat("Fator") = 1.0R
                drMat("PesoTotal") = 0.0R
                drMat("AreaPinturaTotal") = 0.0R
                drMat("PesoMaterial") = PesoMaterialTotal

                tabelaDest.Rows.Add(drMat)
            Catch
                Continue For
            End Try
        Next
    End Sub

#End Region


    'Public Sub ExportarPorGrid(dgv As DataGridView, chkDXF As CheckBox, chkPDF As CheckBox, chkIGES As CheckBox, BarraProgresso As ProgressBar, lbl As Label, chkOpcaodePasta As CheckBox)

    '    ' Evita processar o mesmo arquivo mais de uma vez nesta execução
    '    Dim arquivosProcessados As New HashSet(Of String)(StringComparer.OrdinalIgnoreCase)

    '    Try


    '        If dgv.Rows.Count = 0 Then
    '            MessageBox.Show("Nenhum item na lista de peças.", "SINCO - Solid Edge",
    '                        MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
    '            Exit Sub
    '        End If

    '        '──────────────────────────────────────────────
    '        ' 🔹 Configurações iniciais
    '        '──────────────────────────────────────────────
    '        Dim total As Integer = dgv.Rows.Count
    '        Dim processados As Integer = 0
    '        Dim exportadosDXF As Integer = 0
    '        Dim exportadosPDF As Integer = 0
    '        Dim primeiroArquivo As String = ""

    '        Try

    '            primeiroArquivo = dgv.Rows(0).Cells("CaminhoCompleto").Value?.ToString()?.Trim()

    '        Catch ex As Exception

    '            primeiroArquivo = ""

    '        Finally

    '        End Try


    '        app.DisplayAlerts = False
    '        BarraProgresso.Visible = True
    '        BarraProgresso.Minimum = 0
    '        BarraProgresso.Maximum = total
    '        BarraProgresso.Value = 0

    '        'Notas de regra
    '        'Para gera blank vou descartar aquivo com inicio MT1-MT2-MT7-MT8-MTA

    '        '──────────────────────────────────────────────
    '        ' 🔁 Loop principal
    '        '──────────────────────────────────────────────
    '        For Each row As DataGridViewRow In dgv.Rows
    '            Try
    '                Dim Bloqueado As String = row.Cells("Bloqueado").Value?.ToString()?.Trim().ToUpperInvariant()

    '                If Bloqueado = "SIM" Then

    '                    row.Cells("Bloqueado").Style.BackColor = Color.LightGreen

    '                    Continue For

    '                Else


    '                    Dim caminho As String = row.Cells("CaminhoCompleto").Value?.ToString()?.Trim()
    '                    Dim txttipodesenho As String = row.Cells("Tipo de Desenho").Value?.ToString()?.Trim()
    '                    If String.IsNullOrWhiteSpace(caminho) OrElse Not File.Exists(caminho) Then Continue For

    '                    Dim extensao As String = IO.Path.GetExtension(caminho).ToLower()
    '                    Dim nomeArquivo As String = IO.Path.GetFileNameWithoutExtension(caminho)
    '                    Dim pasta As String = IO.Path.GetDirectoryName(caminho)





    '                    ' Normaliza para caminho absoluto e checa duplicidade
    '                    Dim caminhoFull = IO.Path.GetFullPath(caminho)

    '                    ' Se já vimos este arquivo, apenas atualiza ícones (se desejar) e segue
    '                    If arquivosProcessados.Contains(caminhoFull) Then
    '                        '' Opcional: marcar ícones se os arquivos já existem
    '                        Try
    '                            Dim pastaDup = IO.Path.GetDirectoryName(caminhoFull)
    '                            Dim nomeDup = IO.Path.GetFileNameWithoutExtension(caminhoFull)
    '                            If chkDXF.Checked Then
    '                                row.Cells("dgvdxf").Value = If(File.Exists(IO.Path.Combine(pastaDup, nomeDup & ".dxf")),
    '                                           My.Resources.dxf, My.Resources.sem_icone)
    '                            End If
    '                            If chkPDF.Checked Then
    '                                row.Cells("dgvpdf").Value = If(File.Exists(IO.Path.Combine(pastaDup, nomeDup & ".pdf")),
    '                                           My.Resources.pdf, My.Resources.sem_icone)
    '                            End If
    '                        Catch
    '                        End Try
    '                        Continue For
    '                    End If

    '                    ' Marca como em processamento para não repetir mais à frente
    '                    arquivosProcessados.Add(caminhoFull)

    '                    '──────────────────────────────────────────────
    '                    ' 🔹 Fecha documentos abertos
    '                    '──────────────────────────────────────────────
    '                    Try
    '                        For Each d As Object In app.Documents
    '                            Try : d.Close(False) : Catch : End Try
    '                        Next
    '                    Catch
    '                    End Try

    '                    '──────────────────────────────────────────────
    '                    ' 🔹 Abre o arquivo atual
    '                    '──────────────────────────────────────────────
    '                    Dim doc As Object = Nothing
    '                    Try
    '                        doc = app.Documents.Open(caminho)
    '                        doc.Activate()
    '                        app.DoIdle()
    '                        Threading.Thread.Sleep(200)
    '                    Catch
    '                        Continue For
    '                    End Try

    '                    '──────────────────────────────────────────────
    '                    ' 🔹 Exportar DXF conforme o tipo de arquivo
    '                    '──────────────────────────────────────────────
    '                    If chkDXF.Checked Then

    '                        If caminho.Contains("MT1-") Or caminho.Contains("MT2-") Or caminho.Contains("MT7-") Or caminho.Contains("MT8-") Or caminho.Contains("MTA") Then

    '                            Continue For

    '                        Else

    '                            Select Case extensao
    '                                Case ".psm", ".par"
    '                                    Try

    '                                        If chkOpcaodePasta.Checked = False Then

    '                                            ExportarDXFPlanificadoAuto(1)

    '                                        Else

    '                                            ExportarDXFPlanificadoAuto(0)
    '                                        End If


    '                                        Dim dxfDestino As String = IO.Path.Combine(pasta, nomeArquivo & ".dxf")

    '                                        ' Aguarda até o arquivo existir (máx 5s)
    '                                        Dim tentativas As Integer = 0
    '                                        Do Until File.Exists(dxfDestino) OrElse tentativas > 20
    '                                            Threading.Thread.Sleep(250)
    '                                            tentativas += 1
    '                                        Loop

    '                                        If File.Exists(dxfDestino) Then
    '                                            row.Cells("dgvdxf").Value = My.Resources.dxf
    '                                            exportadosDXF += 1
    '                                        Else
    '                                            row.Cells("dgvdxf").Value = My.Resources.sem_icone
    '                                        End If

    '                                    Catch
    '                                        row.Cells("dgvdxf").Value = My.Resources.sem_icone
    '                                    End Try

    '                                Case Else
    '                                    row.Cells("dgvdxf").Value = My.Resources.sem_icone
    '                            End Select

    '                        End If

    '                    End If

    '                    '──────────────────────────────────────────────
    '                    ' 🔹 Exportar PDF (para qualquer tipo suportado)
    '                    '──────────────────────────────────────────────
    '                    If chkPDF.Checked Then


    '                        Try


    '                            If chkOpcaodePasta.Checked = False Then
    '                                ExportarPDFDetalhamentoLote(1)
    '                            Else
    '                                ExportarPDFDetalhamentoLote(0)
    '                            End If


    '                            Dim pdfDestino As String = IO.Path.Combine(pasta, nomeArquivo & ".pdf")

    '                            Dim tentativas As Integer = 0
    '                            Do Until File.Exists(pdfDestino) OrElse tentativas > 20
    '                                Threading.Thread.Sleep(250)
    '                                tentativas += 1
    '                            Loop

    '                            If File.Exists(pdfDestino) Then
    '                                row.Cells("dgvpdf").Value = My.Resources.pdf
    '                                exportadosPDF += 1
    '                            Else
    '                                row.Cells("dgvpdf").Value = My.Resources.sem_icone
    '                            End If

    '                        Catch
    '                            row.Cells("dgvpdf").Value = My.Resources.sem_icone
    '                        End Try
    '                    End If


    '                    '──────────────────────────────────────────────
    '                    ' 🔹 Exportar PDF (para qualquer tipo suportado)
    '                    '──────────────────────────────────────────────

    '                    If chkIGES.Checked And txttipodesenho = "CORTE LASER TUBO" Then

    '                        Try

    '                            If chkOpcaodePasta.Checked = False Then
    '                                ExportarIGESAuto(1)
    '                            Else
    '                                ExportarIGESAuto(0)
    '                            End If


    '                            Dim pdfDestino As String = IO.Path.Combine(pasta, nomeArquivo & ".iges")

    '                            Dim tentativas As Integer = 0
    '                            Do Until File.Exists(pdfDestino) OrElse tentativas > 20
    '                                Threading.Thread.Sleep(250)
    '                                tentativas += 1
    '                            Loop

    '                        Catch

    '                        End Try

    '                    End If


    '                    '──────────────────────────────────────────────
    '                    ' 🔹 Fecha documento sem salvar
    '                    '──────────────────────────────────────────────
    '                    Try : doc.Close(False) : Catch : End Try

    '                    processados += 1
    '                    BarraProgresso.Value = processados

    '                    lbl.Text = processados
    '                    '' System.Windows.Forms.Application.DoEvents()

    '                End If


    '            Catch
    '                ' Continua o loop mesmo que uma linha falhe
    '            End Try


    '        Next

    '        '──────────────────────────────────────────────
    '        ' 🔹 Reabre o primeiro desenho
    '        '──────────────────────────────────────────────


    '        ' Dim EnderecoArquivo As String = dgvDadosPecas.CurrentRow.Cells("CaminhoCompleto").Value.ToString()

    '        'If File.Exists(primeiroArquivo) Then

    '        '    Process.Start(primeiroArquivo)

    '        '    'Else

    '        '    '    MessageBox.Show("Arquivo não encontrado " & primeiroArquivo, "SINCO - Solid Edge",
    '        '    '   MessageBoxButtons.OK, MessageBoxIcon.Exclamation)

    '        'End If

    '        Try



    '            dgv.Columns("dgvdxf").Visible = True
    '            dgv.Columns("dgvpdf").Visible = True



    '        Catch ex As Exception
    '        Finally
    '        End Try



    '        Try
    '            If Not String.IsNullOrWhiteSpace(primeiroArquivo) AndAlso File.Exists(primeiroArquivo) Then
    '                Threading.Thread.Sleep(500)
    '                Dim docPrimeiro = app.Documents.Open(primeiroArquivo)
    '                docPrimeiro.Activate()
    '                app.DoIdle()
    '            End If
    '        Catch ex As Exception
    '            MessageBox.Show("Não foi possível reabrir o primeiro desenho " & ex.Message,
    '                        "SINCO - Solid Edge", MessageBoxButtons.OK, MessageBoxIcon.Warning)
    '        End Try

    '        '──────────────────────────────────────────────
    '        ' 🔹 Finalização
    '        '──────────────────────────────────────────────
    '        app.DisplayAlerts = True
    '        BarraProgresso.Visible = False
    '        BarraProgresso.Value = 0

    '        lbl.Text = ""

    '        'MessageBox.Show($"✅ Exportação concluída!" & vbCrLf &
    '        '            $"Peças processadas {processados}" & vbCrLf &
    '        '            $"DXFs gerados {exportadosDXF}" & vbCrLf &
    '        '            $"PDFs gerados {exportadosPDF}",
    '        '            "SINCO - Solid Edge",
    '        '            MessageBoxButtons.OK, MessageBoxIcon.Information)

    '    Catch ex As Exception
    '    Finally
    '        'MessageBox.Show("Erro geral na exportação em lote " & ex.Message,
    '        '            "SINCO - Solid Edge", MessageBoxButtons.OK, MessageBoxIcon.Error)
    '    End Try

    'End Sub


    Public Sub ExportarPorGrid(dgv As DataGridView, chkDXF As CheckBox, chkPDF As CheckBox, chkIGES As CheckBox, BarraProgresso As ProgressBar, lbl As Label, chkOpcaodePasta As CheckBox)

        ' Evita processar o mesmo arquivo mais de uma vez nesta execução
        Dim arquivosProcessados As New HashSet(Of String)(StringComparer.OrdinalIgnoreCase)

        Try
            If dgv.Rows.Count = 0 Then
                MessageBox.Show("Nenhum item na lista de peças.", "SINCO - Solid Edge", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Exit Sub
            End If

            '──────────────────────────────────────────────
            ' 📁 LÓGICA DE PASTA CORRIGIDA (Pergunta antes do loop)
            '──────────────────────────────────────────────
            Dim pastaDestinoLote As String = ""

            ' Se chkOpcaodePasta = False (Ex: Salvar tudo em uma pasta selecionada)
            If chkOpcaodePasta.Checked = False Then
                Using fbd As New FolderBrowserDialog()
                    fbd.Description = "Selecione a pasta de destino para salvar TODOS os arquivos exportados:"
                    If fbd.ShowDialog() = DialogResult.OK Then
                        pastaDestinoLote = fbd.SelectedPath
                    Else
                        Exit Sub ' Usuário cancelou, aborta o processo
                    End If
                End Using
            End If

            '──────────────────────────────────────────────
            ' 🔹 Configurações iniciais
            '──────────────────────────────────────────────
            Dim total As Integer = dgv.Rows.Count
            Dim processados As Integer = 0
            Dim exportadosDXF As Integer = 0
            Dim exportadosPDF As Integer = 0
            Dim primeiroArquivo As String = ""

            Try : primeiroArquivo = dgv.Rows(0).Cells("CaminhoCompleto").Value?.ToString()?.Trim() : Catch : End Try

            app.DisplayAlerts = False
            BarraProgresso.Visible = True
            BarraProgresso.Minimum = 0
            BarraProgresso.Maximum = total
            BarraProgresso.Value = 0

            '──────────────────────────────────────────────
            ' 🔁 Loop principal
            '──────────────────────────────────────────────
            For Each row As DataGridViewRow In dgv.Rows
                Try
                    Dim Bloqueado As String = row.Cells("Bloqueado").Value?.ToString()?.Trim().ToUpperInvariant()

                    If Bloqueado = "SIM" Then
                        row.Cells("Bloqueado").Style.BackColor = Color.LightGreen
                        Continue For
                    End If

                    Dim caminho As String = row.Cells("CaminhoCompleto").Value?.ToString()?.Trim()
                    Dim txttipodesenho As String = row.Cells("Tipo de Desenho").Value?.ToString()?.Trim()
                    If String.IsNullOrWhiteSpace(caminho) OrElse Not File.Exists(caminho) Then Continue For

                    Dim extensao As String = IO.Path.GetExtension(caminho).ToLower()
                    Dim nomeArquivo As String = IO.Path.GetFileNameWithoutExtension(caminho)
                    Dim pastaOriginal As String = IO.Path.GetDirectoryName(caminho)

                    ' Define a pasta final para este arquivo
                    Dim pastaDestinoFinal As String = pastaOriginal
                    If chkOpcaodePasta.Checked = False Then
                        pastaDestinoFinal = pastaDestinoLote ' Usa a pasta selecionada no FBD
                    End If

                    Dim caminhoFull = IO.Path.GetFullPath(caminho)

                    If arquivosProcessados.Contains(caminhoFull) Then
                        Try
                            If chkDXF.Checked Then
                                row.Cells("dgvdxf").Value = If(File.Exists(IO.Path.Combine(pastaDestinoFinal, nomeArquivo & ".dxf")), My.Resources.dxf, My.Resources.sem_icone)
                            End If
                            If chkPDF.Checked Then
                                row.Cells("dgvpdf").Value = If(File.Exists(IO.Path.Combine(pastaDestinoFinal, nomeArquivo & ".pdf")), My.Resources.pdf, My.Resources.sem_icone)
                            End If
                        Catch
                        End Try
                        Continue For
                    End If

                    arquivosProcessados.Add(caminhoFull)

                    ' Fecha documentos abertos
                    Try
                        For Each d As Object In app.Documents
                            Try : d.Close(False) : Catch : End Try
                        Next
                    Catch
                    End Try

                    ' Abre o arquivo atual
                    Dim doc As Object = Nothing
                    Try
                        doc = app.Documents.Open(caminho)
                        doc.Activate()
                        app.DoIdle()
                        Threading.Thread.Sleep(200)
                    Catch
                        Continue For
                    End Try

                    '──────────────────────────────────────────────
                    ' 🔹 Exportar DXF
                    '──────────────────────────────────────────────
                    If chkDXF.Checked Then
                        'If caminho.Contains("MT1-") Or caminho.Contains("MT2-") Or caminho.Contains("MT7-") Or caminho.Contains("MT8-") Or caminho.Contains("MTA") Or caminho.Contains("MPT") Then
                        'Continue For
                        'Else
                        Select Case extensao
                                Case ".psm", ".par"
                                    Try
                                        Dim dxfDestino As String = IO.Path.Combine(pastaDestinoFinal, nomeArquivo & ".dxf")

                                        ' Chama a função passando O CAMINHO COMPLETO E CORRETO
                                        ExportarDXFPlanificadoAuto(dxfDestino)

                                        ' Aguarda até o arquivo existir
                                        Dim tentativas As Integer = 0
                                        Do Until File.Exists(dxfDestino) OrElse tentativas > 20
                                            Threading.Thread.Sleep(250)
                                            tentativas += 1
                                        Loop

                                        If File.Exists(dxfDestino) Then
                                            row.Cells("dgvdxf").Value = My.Resources.dxf
                                            exportadosDXF += 1
                                        Else
                                            row.Cells("dgvdxf").Value = My.Resources.sem_icone
                                        End If
                                    Catch
                                        row.Cells("dgvdxf").Value = My.Resources.sem_icone
                                    End Try
                                Case Else
                                    row.Cells("dgvdxf").Value = My.Resources.sem_icone
                            End Select
                        End If
                    ' End If

                    '──────────────────────────────────────────────
                    ' 🔹 Exportar PDF (Ajuste suas chamadas de PDF/IGES da mesma forma)
                    '──────────────────────────────────────────────
                    ' Exemplo de como ficará o PDF:
                    If chkPDF.Checked Then
                        Try
                            Dim pdfDestino As String = IO.Path.Combine(pastaDestinoFinal, nomeArquivo & ".pdf")

                            ' Nota: Altere sua função ExportarPDFDetalhamentoLote para receber o pdfDestino como parâmetro
                            ExportarPDFDetalhamentoLote(pdfDestino)

                            Dim tentativas As Integer = 0
                            Do Until File.Exists(pdfDestino) OrElse tentativas > 20
                                Threading.Thread.Sleep(250)
                                tentativas += 1
                            Loop

                            If File.Exists(pdfDestino) Then
                                row.Cells("dgvpdf").Value = My.Resources.pdf
                                exportadosPDF += 1
                            Else
                                row.Cells("dgvpdf").Value = My.Resources.sem_icone
                            End If
                        Catch
                            row.Cells("dgvpdf").Value = My.Resources.sem_icone
                        End Try
                    End If

                    If chkIGES.Checked And txttipodesenho = "CORTE LASER TUBO" Then
                        Try
                            Dim igesDestino As String = IO.Path.Combine(pastaDestinoFinal, nomeArquivo & ".iges")
                            ExportarIGESAuto(igesDestino)
                        Catch
                        End Try
                    End If

                    ' Fecha documento sem salvar
                    Try : doc.Close(False) : Catch : End Try

                    processados += 1
                    BarraProgresso.Value = processados
                    lbl.Text = processados.ToString()

                Catch
                End Try
            Next

            ' Reabre o primeiro
            Try
                dgv.Columns("dgvdxf").Visible = True
                dgv.Columns("dgvpdf").Visible = True
            Catch : End Try

            Try
                If Not String.IsNullOrWhiteSpace(primeiroArquivo) AndAlso File.Exists(primeiroArquivo) Then
                    Threading.Thread.Sleep(500)
                    Dim docPrimeiro = app.Documents.Open(primeiroArquivo)
                    docPrimeiro.Activate()
                    app.DoIdle()
                End If
            Catch : End Try

            ' Finalização
            app.DisplayAlerts = True
            BarraProgresso.Visible = False
            BarraProgresso.Value = 0
            lbl.Text = ""

        Catch ex As Exception
        End Try
    End Sub



    Public Sub FiltrarPorCaminho(ByVal dgv As DataGridView,
                                 ByVal termo As String,
                                 ByVal empresa As String)
        Try
            If tabelaOriginalBOM Is Nothing Then Exit Sub

            ' Cria uma DataView baseada na tabela original
            Dim view As New DataView(tabelaOriginalBOM)

            Dim filtros As New List(Of String)

            ' 🔹 Trata texto de filtro por caminho
            If Not String.IsNullOrWhiteSpace(termo) Then
                termo = termo.Replace("'", "''") ' evita erro por aspas simples

                If view.Table.Columns.Contains("CaminhoCompleto") Then
                    filtros.Add($"CaminhoCompleto LIKE '%{termo}%'")
                End If
            End If

            ' 🔹 Trata texto de filtro por empresa
            If Not String.IsNullOrWhiteSpace(empresa) Then
                empresa = empresa.Replace("'", "''")

                If view.Table.Columns.Contains("Empresa") Then
                    filtros.Add($"Empresa LIKE '%{empresa}%'")
                End If
            End If

            ' 🔹 Monta o RowFilter
            If filtros.Count > 0 Then
                ' Se tiver mais de um filtro, junta com AND
                view.RowFilter = String.Join(" AND ", filtros)
            Else
                ' Sem filtros → mostra tudo
                view.RowFilter = ""
            End If

            ' Atualiza o grid
            dgv.DataSource = view

        Catch ex As Exception
            MessageBox.Show("Erro ao filtrar: " & ex.Message,
                            "Filtro", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub






End Module
