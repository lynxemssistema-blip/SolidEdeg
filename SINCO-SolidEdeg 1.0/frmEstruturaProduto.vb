Imports System.Data
Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Globalization
Imports System.IO
Imports System.Net
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Windows.Forms
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq
Imports Npgsql

''' <summary>
''' SINCO - Solid Edge | Formulário de Engenharia e Estrutura de Produtos
''' Réplica fiel com 4 Pilares (SG1010, SZ1010, SG2010, SZ0010) e Árvore Completa Multinível
''' </summary>
Public Class frmEstruturaProduto
    Inherits Form

    ' =========================================================================
    ' 1. CONTROLES DA TOP BAR
    ' =========================================================================
    Private pnlTopBar As Panel
    Private lblLogo As Label
    Private lblBadgeUI As Label
    Private lblSep1 As Label
    Private lblArquivoCorrenteTitle As Label
    Private lblBadgeArquivoStatus As Label
    Private lblArquivoCorrenteValor As Label
    Private btnRefresh As Button
    Private txtBuscaERP As TextBox
    Private btnBuscarERP As Button
    Private btnGerarPDF As Button
    Private btnSalvarProtheus As Button

    ' =========================================================================
    ' 2. CONTROLES DA TABS BAR
    ' =========================================================================
    Private pnlTabsBar As Panel
    Private btnTabFormulario As Button
    Private btnTabArvore As Button
    Private lblSubtituloVisao As Label
    Private lblBadgePecaTop As Label

    ' =========================================================================
    ' 3. CONTEÚDO PRINCIPAL (TABS)
    ' =========================================================================
    Private pnlTabContent As Panel
    Private pnlContentFormulario As Panel
    Private pnlContentArvore As Panel
    Private pnlSecaoPilares As SplitContainer

    ' =========================================================================
    ' 4. DADOS GERAIS DO PRODUTO (CABEÇALHO)
    ' =========================================================================
    Private pnlDadosGerais As Panel
    Private lblTituloDados As Label
    Private lblBadgeModo As Label
    Private btnEditarCabecalho As Button

    ' Linha 1 de campos
    Private txtTipo As TextBox
    Private txtUnidade As TextBox
    Private txtGrupo As TextBox
    Private txtNumDoc As TextBox
    Private txtRev As TextBox
    Private txtRevCliente As TextBox
    Private txtNCM As TextBox
    Private txtTituloProd As TextBox

    ' Linha 2 de campos
    Private txtDensidade As TextBox
    Private txtEspessura As TextBox
    Private txtLargura As TextBox
    Private txtComprimento As TextBox
    Private txtArea As TextBox
    Private txtPeso As TextBox
    Private txtComentarios As TextBox

    ' =========================================================================
    ' 5. PILAR 1: ÁRVORE DO PRODUTO / MATERIAIS (SG1010)
    ' =========================================================================
    Private pnlColMateriais As Panel
    Private lblBadgeQtdMateriais As Label
    Private btnEditarMateriais As Button
    Private btnInserirMateriais As Button
    Private lblBadgePecaMateriais As Label
    Private dgvMateriais As DataGridView

    ' =========================================================================
    ' 6. PILAR 2: PRODUTO METALFISA / CLIENTE (SZ1010)
    ' =========================================================================
    Private pnlColCliente As Panel
    Private lblBadgeQtdCliente As Label
    Private btnEditarCliente As Button
    Private btnInserirCliente As Button
    Private lblBadgePecaCliente As Label
    Private dgvCliente As DataGridView

    ' =========================================================================
    ' 7. PILAR 3: OPERAÇÕES / ROTEIRO (SG2010 + SH1010)
    ' =========================================================================
    Private pnlColRoteiro As Panel
    Private lblBadgeQtdRoteiro As Label
    Private btnEditarRoteiro As Button
    Private btnInserirRoteiro As Button
    Private lblBadgePecaRoteiro As Label
    Private dgvRoteiro As DataGridView

    ' =========================================================================
    ' 8. PILAR 4: RESTRIÇÕES DE ENGENHARIA (SZ0010)
    ' =========================================================================
    Private pnlSecaoRestricoes As Panel
    Private lblBadgeQtdRestricoes As Label
    Private btnNovaRestricao As Button
    Private lblBadgePecaRestricao As Label
    Private dgvRestricoes As DataGridView
    Private lblSemRestricoes As Label

    ' =========================================================================
    ' 9. TAB 2: ESTRUTURA EM ÁRVORE COMPLETA (MULTINÍVEL)
    ' =========================================================================
    Private splitArvore As SplitContainer
    Private tvwArvoreBOM As TreeView
    Private dgvArvoreCompleta As DataGridView
    Private btnExpandirArvore As Button
    Private btnRecolherArvore As Button
    Private btnExportarExcelArvore As Button
    Private lblTotalNosArvore As Label

    ' =========================================================================
    ' 10. STATUS BAR
    ' =========================================================================
    Private pnlStatusBar As Panel
    Private lblStatus As Label
    Private progressBar As ProgressBar

    ' =========================================================================
    ' PROPRIEDADES E CONFIGURAÇÕES DE BANCO / API
    ' =========================================================================
    Public Property CodigoProduto As String = "MT6-5351"
    Public Property DescricaoProduto As String = ""
    Public Property DocumentoAtivoCAD As Object = Nothing

    Private ReadOnly connStringProtheus As String = "Host=192.168.1.61;Port=5432;Username=sinco2;Password=sinco25;Database=p12prd;"
    Private ReadOnly apiProtheusBase As String = "https://192.168.1.60:47500"

    ' Cores Oficiais do Design System da Tela
    Private ReadOnly corSlate900 As Color = Color.FromArgb(15, 23, 42)
    Private ReadOnly corSlate800 As Color = Color.FromArgb(30, 41, 59)
    Private ReadOnly corSlate700 As Color = Color.FromArgb(51, 65, 85)
    Private ReadOnly corSlate600 As Color = Color.FromArgb(71, 85, 105)
    Private ReadOnly corSlate200 As Color = Color.FromArgb(226, 232, 240)
    Private ReadOnly corSlate100 As Color = Color.FromArgb(241, 245, 249)
    Private ReadOnly corSlate50 As Color = Color.FromArgb(248, 250, 252)

    Private ReadOnly corVerdeBOM As Color = Color.FromArgb(5, 150, 105)
    Private ReadOnly corVerdeFundo As Color = Color.FromArgb(236, 253, 245)

    Private ReadOnly corAmbarCliente As Color = Color.FromArgb(217, 119, 6)
    Private ReadOnly corAmbarFundo As Color = Color.FromArgb(254, 243, 199)

    Private ReadOnly corAzulRoteiro As Color = Color.FromArgb(30, 58, 138)
    Private ReadOnly corAzulFundo As Color = Color.FromArgb(239, 246, 255)

    Private ReadOnly corVermelhoRestricao As Color = Color.FromArgb(220, 38, 38)
    Private ReadOnly corVermelhoFundo As Color = Color.FromArgb(254, 242, 242)

    Public Sub New(Optional codProd As String = "MT6-5351", Optional descProd As String = "", Optional docCAD As Object = Nothing)
        CodigoProduto = ObterCodigoProtheusLimpo(If(String.IsNullOrWhiteSpace(codProd), "MT6-5351", codProd))
        DescricaoProduto = If(descProd, "").Trim()
        DocumentoAtivoCAD = docCAD

        InicializarComponentes()
        ConfigurarEstilos()

        ' Extrai dados do arquivo CAD diretamente se fornecido
        TratarArquivoCADAtivo()

        txtBuscaERP.Text = CodigoProduto
        txtNumDoc.Text = CodigoProduto
        If Not String.IsNullOrEmpty(DescricaoProduto) Then
            txtTituloProd.Text = DescricaoProduto
        End If

        ' Carregamento automático e imediato ao exibir a janela
        AddHandler Me.Shown, Sub(s, e)
                                Application.DoEvents()
                                CarregarTodosModulos(CodigoProduto)
                            End Sub
    End Sub

    ''' <summary>
    ''' Trata o documento ativo do Solid Edge extraindo nome real do arquivo e propriedades físicas/blank
    ''' </summary>
    Private Sub TratarArquivoCADAtivo()
        Try
            If DocumentoAtivoCAD IsNot Nothing Then
                Dim caminhoCAD As String = ""
                Try
                    caminhoCAD = DocumentoAtivoCAD.FullName
                Catch
                End Try

                If Not String.IsNullOrEmpty(caminhoCAD) Then
                    Dim nomeArquivo As String = IO.Path.GetFileName(caminhoCAD)
                    lblArquivoCorrenteValor.Text = nomeArquivo
                    lblArquivoCorrenteValor.ForeColor = Color.FromArgb(5, 150, 105)
                    lblBadgeArquivoStatus.Text = "CAD ATIVO"
                    lblBadgeArquivoStatus.ForeColor = Color.FromArgb(5, 150, 105)
                    lblBadgeArquivoStatus.BackColor = Color.FromArgb(236, 253, 245)

                    ' Se o código fornecido for genérico, tenta extrair do arquivo CAD
                    If String.IsNullOrEmpty(CodigoProduto) OrElse CodigoProduto = "MT6-5351" Then
                        Dim codCAD = ObterCodigoProtheusLimpo(nomeArquivo)
                        If Not String.IsNullOrEmpty(codCAD) Then
                            CodigoProduto = codCAD
                        End If
                    End If

                    ' Tenta extrair propriedades físicas do CAD
                    Try
                        Dim dados As New clDadosArquivoCorrente()
                        Dim reader As New SolidEdgeReaderService()
                        reader.ExtrairPropriedades(DocumentoAtivoCAD, dados)

                        If Not String.IsNullOrEmpty(dados.Titulo) AndAlso String.IsNullOrEmpty(DescricaoProduto) Then
                            DescricaoProduto = dados.Titulo
                        End If
                        If Not String.IsNullOrEmpty(dados.material) Then
                            txtDensidade.Text = dados.material
                        End If
                        If Not String.IsNullOrEmpty(dados.Espessura) Then
                            txtEspessura.Text = dados.Espessura
                        End If
                        If Not String.IsNullOrEmpty(dados.LarguraBlank) Then
                            txtLargura.Text = dados.LarguraBlank
                        End If
                        If Not String.IsNullOrEmpty(dados.ComprimentoBlank) Then
                            txtComprimento.Text = dados.ComprimentoBlank
                        End If
                        If dados.AreaPintura IsNot Nothing AndAlso IsNumeric(dados.AreaPintura) Then
                            txtArea.Text = Convert.ToDouble(dados.AreaPintura).ToString("N4")
                        End If
                        If Not String.IsNullOrEmpty(dados.Massa) Then
                            txtPeso.Text = dados.Massa
                        End If
                    Catch
                    End Try
                End If
            Else
                lblArquivoCorrenteValor.Text = "Busca Protheus Direta"
                lblArquivoCorrenteValor.ForeColor = Color.FromArgb(37, 99, 235)
                lblBadgeArquivoStatus.Text = "CONSULTA ERP"
                lblBadgeArquivoStatus.ForeColor = Color.FromArgb(71, 85, 105)
                lblBadgeArquivoStatus.BackColor = Color.FromArgb(241, 245, 249)
            End If
        Catch
        End Try
    End Sub

    ''' <summary>
    ''' Extrai o código puro do Protheus sem diretórios, sem extensões (.asm, .par, .psm) e sem sufixos residuais como " asm"
    ''' </summary>
    Public Shared Function ObterCodigoProtheusLimpo(input As String) As String
        If String.IsNullOrWhiteSpace(input) Then Return ""
        Dim s As String = input.Trim()
        If s.Contains("\"c) OrElse s.Contains("/"c) Then
            s = IO.Path.GetFileNameWithoutExtension(s)
        End If
        s = System.Text.RegularExpressions.Regex.Replace(s, "\.(par|psm|asm|prt|dft|dwg|dxf|pdf)$", "", System.Text.RegularExpressions.RegexOptions.IgnoreCase).Trim()
        s = System.Text.RegularExpressions.Regex.Replace(s, "\s+(par|psm|asm|prt|dft)$", "", System.Text.RegularExpressions.RegexOptions.IgnoreCase).Trim()
        Return s.Trim(" "c, """"c, "'"c).ToUpper()
    End Function

    ''' <summary>
    ''' Converte texto numérico de forma ultra segura para Double, suportando tanto formato brasileiro quanto americano
    ''' </summary>
    Public Shared Function ConverterParaDecimalSeguro(valorTexto As String) As Double
        If String.IsNullOrWhiteSpace(valorTexto) Then Return 0.0
        Dim s As String = valorTexto.Trim()
        s = System.Text.RegularExpressions.Regex.Replace(s, "(?i)[^\d,\.\-]", "")
        If String.IsNullOrEmpty(s) Then Return 0.0

        If s.Contains("."c) AndAlso s.Contains(","c) Then
            Dim lastDot = s.LastIndexOf("."c)
            Dim lastComma = s.LastIndexOf(","c)
            If lastComma > lastDot Then
                s = s.Replace(".", "").Replace(",", ".")
            Else
                s = s.Replace(",", "")
            End If
        ElseIf s.Contains(","c) Then
            s = s.Replace(",", ".")
        End If

        Dim res As Double = 0.0
        Double.TryParse(s, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, res)
        Return res
    End Function

    ' =========================================================================
    ' INICIALIZAÇÃO DE COMPONENTES
    ' =========================================================================
    Private Sub InicializarComponentes()
        Me.Text = "SINCO - Solid Edge | Engenharia Protheus (SG1, SZ1, SG2, SZ0)"
        Me.Size = New Size(1340, 850)
        Me.MinimumSize = New Size(1100, 720)
        Me.StartPosition = FormStartPosition.CenterScreen
        Me.Font = New Font("Segoe UI", 9.0!, FontStyle.Regular)
        Me.BackColor = Color.FromArgb(248, 250, 252)

        ' 1. TOP BAR
        CriarTopBar()

        ' 2. TABS BAR
        CriarTabsBar()

        ' 3. STATUS BAR
        CriarStatusBar()

        ' 4. TAB CONTENT CONTAINER
        pnlTabContent = New Panel() With {
            .Dock = DockStyle.Fill,
            .BackColor = Color.FromArgb(248, 250, 252)
        }

        ' Tab 1: Formulário Solid Edge
        CriarTabFormulario()

        ' Tab 2: Estrutura em Árvore Completa (instanciada em memória para compatibilidade, oculta da tela)
        CriarTabArvore()
        If pnlContentArvore IsNot Nothing Then
            pnlContentArvore.Visible = False
        End If

        pnlTabContent.Controls.Add(pnlContentFormulario)
        If pnlContentArvore IsNot Nothing Then
            pnlTabContent.Controls.Add(pnlContentArvore)
        End If

        Me.Controls.Add(pnlTabContent)
        Me.Controls.Add(pnlTabsBar)
        Me.Controls.Add(pnlTopBar)
        Me.Controls.Add(pnlStatusBar)
    End Sub

    ' -------------------------------------------------------------------------
    ' 1. TOP BAR
    ' -------------------------------------------------------------------------
    Private Sub CriarTopBar()
        pnlTopBar = New Panel() With {
            .Dock = DockStyle.Top,
            .Height = 52,
            .BackColor = Color.White,
            .Padding = New Padding(14, 8, 14, 8)
        }

        lblLogo = New Label() With {
            .Text = "SINCO | Engenharia Protheus",
            .Font = New Font("Segoe UI", 11.0!, FontStyle.Bold),
            .ForeColor = corSlate900,
            .Location = New Point(12, 14),
            .AutoSize = True
        }

        lblBadgeUI = New Label() With {
            .Text = "v5.2",
            .Font = New Font("Segoe UI", 7.5!, FontStyle.Bold),
            .ForeColor = corSlate600,
            .BackColor = corSlate200,
            .Location = New Point(235, 16),
            .Size = New Size(36, 18),
            .TextAlign = ContentAlignment.MiddleCenter
        }

        lblSep1 = New Label() With {
            .Text = "|",
            .Font = New Font("Segoe UI", 11.0!),
            .ForeColor = Color.FromArgb(203, 213, 225),
            .Location = New Point(280, 14),
            .AutoSize = True
        }

        lblBadgeArquivoStatus = New Label() With {
            .Text = "CAD ATIVO",
            .Font = New Font("Segoe UI", 7.5!, FontStyle.Bold),
            .ForeColor = Color.FromArgb(5, 150, 105),
            .BackColor = Color.FromArgb(236, 253, 245),
            .Location = New Point(296, 16),
            .Size = New Size(95, 20),
            .TextAlign = ContentAlignment.MiddleCenter
        }

        lblArquivoCorrenteValor = New Label() With {
            .Text = "MT6-5351.asm",
            .Font = New Font("Segoe UI", 9.0!, FontStyle.Bold),
            .ForeColor = Color.FromArgb(15, 23, 42),
            .Location = New Point(400, 16),
            .AutoSize = True,
            .Cursor = Cursors.Hand
        }

        ' Botões e Busca da Direita (Adicionados na ordem da direita para esquerda)
        btnSalvarProtheus = New Button() With {
            .Text = "💾  Salvar no Protheus",
            .Dock = DockStyle.Right,
            .Width = 170,
            .BackColor = Color.FromArgb(5, 150, 105),
            .ForeColor = Color.White,
            .Font = New Font("Segoe UI", 9.0!, FontStyle.Bold),
            .FlatStyle = FlatStyle.Flat,
            .Cursor = Cursors.Hand
        }
        btnSalvarProtheus.FlatAppearance.BorderSize = 0
        AddHandler btnSalvarProtheus.Click, AddressOf btnSalvarProtheus_Click

        btnGerarPDF = New Button() With {
            .Text = "📄  Relatório PDF",
            .Dock = DockStyle.Right,
            .Width = 125,
            .BackColor = Color.White,
            .ForeColor = Color.FromArgb(220, 38, 38),
            .Font = New Font("Segoe UI", 9.0!, FontStyle.Bold),
            .FlatStyle = FlatStyle.Flat,
            .Cursor = Cursors.Hand
        }
        btnGerarPDF.FlatAppearance.BorderColor = Color.FromArgb(254, 202, 202)
        AddHandler btnGerarPDF.Click, AddressOf btnGerarPDF_Click

        btnBuscarERP = New Button() With {
            .Text = "🔍 Buscar",
            .Dock = DockStyle.Right,
            .Width = 90,
            .BackColor = Color.FromArgb(30, 41, 59),
            .ForeColor = Color.White,
            .Font = New Font("Segoe UI", 9.0!, FontStyle.Bold),
            .FlatStyle = FlatStyle.Flat,
            .Cursor = Cursors.Hand
        }
        btnBuscarERP.FlatAppearance.BorderSize = 0
        AddHandler btnBuscarERP.Click, Sub(s, e) CarregarTodosModulos(txtBuscaERP.Text.Trim().ToUpper())

        Dim pnlInputWrapper As New Panel() With {
            .Dock = DockStyle.Right,
            .Width = 180,
            .Padding = New Padding(6, 4, 6, 4)
        }

        txtBuscaERP = New TextBox() With {
            .Dock = DockStyle.Fill,
            .Font = New Font("Segoe UI", 9.5!, FontStyle.Bold),
            .ForeColor = Color.FromArgb(30, 41, 59),
            .CharacterCasing = CharacterCasing.Upper
        }
        AddHandler txtBuscaERP.KeyDown, Sub(s, e)
                                            If e.KeyCode = Keys.Enter Then
                                                CarregarTodosModulos(txtBuscaERP.Text.Trim().ToUpper())
                                                e.SuppressKeyPress = True
                                            End If
                                        End Sub

        btnRefresh = New Button() With {
            .Text = "🔄",
            .Dock = DockStyle.Right,
            .Width = 32,
            .BackColor = Color.White,
            .FlatStyle = FlatStyle.Flat,
            .Cursor = Cursors.Hand,
            .Font = New Font("Segoe UI", 9.0!)
        }
        btnRefresh.FlatAppearance.BorderColor = Color.FromArgb(203, 213, 225)
        AddHandler btnRefresh.Click, Sub(s, e) CarregarTodosModulos(txtBuscaERP.Text.Trim().ToUpper())

        pnlInputWrapper.Controls.Add(txtBuscaERP)
        pnlInputWrapper.Controls.Add(btnRefresh)

        pnlTopBar.Controls.AddRange({lblLogo, lblBadgeUI, lblSep1, lblBadgeArquivoStatus, lblArquivoCorrenteValor, pnlInputWrapper, btnBuscarERP, btnGerarPDF, btnSalvarProtheus})
    End Sub

    ' -------------------------------------------------------------------------
    ' 2. TABS BAR
    ' -------------------------------------------------------------------------
    Private Sub CriarTabsBar()
        pnlTabsBar = New Panel() With {
            .Dock = DockStyle.Top,
            .Height = 44,
            .BackColor = Color.White,
            .Padding = New Padding(12, 5, 12, 5)
        }

        btnTabFormulario = New Button() With {
            .Text = "📋  Formulário Solid Edge",
            .Location = New Point(12, 6),
            .Size = New Size(200, 32),
            .BackColor = corSlate900,
            .ForeColor = Color.White,
            .Font = New Font("Segoe UI", 9.0!, FontStyle.Bold),
            .FlatStyle = FlatStyle.Flat,
            .Cursor = Cursors.Hand
        }
        btnTabFormulario.FlatAppearance.BorderSize = 0
        AddHandler btnTabFormulario.Click, Sub(s, e) AlternarAba(True)

        ' Instanciado internamente para evitar NullReferenceException, mantido invisível (removido da barra de abas)
        btnTabArvore = New Button() With {
            .Visible = False
        }

        lblBadgePecaTop = New Label() With {
            .Text = "MT6-5351",
            .Font = New Font("Segoe UI", 8.0!, FontStyle.Bold),
            .ForeColor = corSlate700,
            .BackColor = corSlate200,
            .Dock = DockStyle.Right,
            .Width = 85,
            .TextAlign = ContentAlignment.MiddleCenter
        }

        lblSubtituloVisao = New Label() With {
            .Text = "Visão clássica de cadastro e sub-tabelas",
            .Font = New Font("Segoe UI", 8.5!, FontStyle.Regular),
            .ForeColor = Color.FromArgb(100, 116, 139),
            .Dock = DockStyle.Right,
            .Width = 230,
            .TextAlign = ContentAlignment.MiddleRight
        }

        pnlTabsBar.Controls.AddRange({btnTabFormulario, lblBadgePecaTop, lblSubtituloVisao})
    End Sub

    Private Sub AlternarAba(mostrarFormulario As Boolean)
        pnlContentFormulario.Visible = True
        If pnlContentArvore IsNot Nothing Then pnlContentArvore.Visible = False

        btnTabFormulario.BackColor = corSlate900
        btnTabFormulario.ForeColor = Color.White
    End Sub

    ' -------------------------------------------------------------------------
    ' 3. TAB 1: FORMULÁRIO SOLID EDGE COM OS 4 PILARES
    ' -------------------------------------------------------------------------
    Private Sub CriarTabFormulario()
        pnlContentFormulario = New Panel() With {
            .Dock = DockStyle.Fill,
            .BackColor = Color.FromArgb(248, 250, 252),
            .Padding = New Padding(12, 10, 12, 10)
        }

        ' 1. Card Superior: DADOS GERAIS DO PRODUTO (Dock = Top, Height = 175)
        CriarCardDadosGerais()

        ' 2. SplitContainer Central para os 4 Pilares (Dock = Fill)
        pnlSecaoPilares = New SplitContainer() With {
            .Dock = DockStyle.Fill,
            .Orientation = Orientation.Horizontal,
            .SplitterWidth = 6,
            .BackColor = Color.FromArgb(226, 232, 240)
        }

        Dim tblPilares As New TableLayoutPanel() With {
            .Dock = DockStyle.Fill,
            .ColumnCount = 3,
            .RowCount = 1,
            .Margin = New Padding(0),
            .Padding = New Padding(0)
        }
        tblPilares.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 36.0!))
        tblPilares.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 32.0!))
        tblPilares.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 32.0!))

        CriarPilarMateriais()
        CriarPilarCliente()
        CriarPilarRoteiro()

        tblPilares.Controls.Add(pnlColMateriais, 0, 0)
        tblPilares.Controls.Add(pnlColCliente, 1, 0)
        tblPilares.Controls.Add(pnlColRoteiro, 2, 0)

        pnlSecaoPilares.Panel1.Controls.Add(tblPilares)
        pnlSecaoPilares.Panel1.Padding = New Padding(0, 4, 0, 4)

        ' Pilar 4: Restrições
        CriarSecaoRestricoes()
        pnlSecaoPilares.Panel2.Controls.Add(pnlSecaoRestricoes)
        pnlSecaoPilares.Panel2.Padding = New Padding(0, 4, 0, 0)

        pnlSecaoPilares.SplitterDistance = 330

        ' Adiciona primeiro o SplitContainer e depois o Header para Dock = Top ficar no topo
        pnlContentFormulario.Controls.Add(pnlSecaoPilares)
        pnlContentFormulario.Controls.Add(pnlDadosGerais)
    End Sub

    ' -------------------------------------------------------------------------
    ' CARD SUPERIOR: DADOS GERAIS DO PRODUTO
    ' -------------------------------------------------------------------------
    Private Sub CriarCardDadosGerais()
        pnlDadosGerais = New Panel() With {
            .Dock = DockStyle.Top,
            .Height = 175,
            .BackColor = Color.White,
            .Margin = New Padding(0, 0, 0, 8),
            .Padding = New Padding(12, 8, 12, 8)
        }

        Dim pnlHeaderDados As New Panel() With {.Dock = DockStyle.Top, .Height = 28}
        lblTituloDados = New Label() With {
            .Text = "📋 DADOS CADASTRAIS DO PRODUTO (PROTHEUS SB1010 & GEOMETRIA SOLID EDGE)",
            .Font = New Font("Segoe UI", 9.0!, FontStyle.Bold),
            .ForeColor = corSlate900,
            .Location = New Point(0, 4),
            .AutoSize = True
        }

        lblBadgeModo = New Label() With {
            .Text = "CAD INTEGRADO",
            .Font = New Font("Segoe UI", 7.0!, FontStyle.Bold),
            .ForeColor = Color.FromArgb(5, 150, 105),
            .BackColor = Color.FromArgb(236, 253, 245),
            .Location = New Point(520, 4),
            .Size = New Size(125, 20),
            .TextAlign = ContentAlignment.MiddleCenter
        }

        btnEditarCabecalho = New Button() With {
            .Text = "✏️  EDITAR",
            .Dock = DockStyle.Right,
            .Width = 90,
            .BackColor = Color.White,
            .ForeColor = corSlate700,
            .Font = New Font("Segoe UI", 8.0!, FontStyle.Bold),
            .FlatStyle = FlatStyle.Flat,
            .Cursor = Cursors.Hand
        }
        btnEditarCabecalho.FlatAppearance.BorderColor = corSlate200
        AddHandler btnEditarCabecalho.Click, AddressOf btnEditarCabecalho_Click

        pnlHeaderDados.Controls.AddRange({lblTituloDados, lblBadgeModo, btnEditarCabecalho})

        ' Tabela com 2 Colunas para os Grupos de Campos
        Dim tblGrupos As New TableLayoutPanel() With {
            .Dock = DockStyle.Fill,
            .ColumnCount = 2,
            .RowCount = 1,
            .Margin = New Padding(0),
            .Padding = New Padding(0)
        }
        tblGrupos.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 55.0!))
        tblGrupos.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 45.0!))

        ' GRUPO 1: Classificação Protheus (SB1010)
        Dim grpProtheusCab As New GroupBox() With {
            .Text = " Classificação Protheus (SB1010) ",
            .Dock = DockStyle.Fill,
            .Font = New Font("Segoe UI", 8.0!, FontStyle.Bold),
            .ForeColor = corSlate700,
            .Margin = New Padding(0, 2, 4, 2)
        }

        Dim lblTipo As New Label() With {.Text = "TIPO:", .Font = New Font("Segoe UI", 7.5!), .ForeColor = corSlate600, .Location = New Point(10, 18), .AutoSize = True}
        txtTipo = New TextBox() With {.Location = New Point(10, 34), .Size = New Size(48, 22), .Font = New Font("Segoe UI", 8.5!, FontStyle.Bold), .TextAlign = HorizontalAlignment.Center, .Text = "PI"}

        Dim lblGrupo As New Label() With {.Text = "GRUPO:", .Font = New Font("Segoe UI", 7.5!), .ForeColor = corSlate600, .Location = New Point(64, 18), .AutoSize = True}
        txtGrupo = New TextBox() With {.Location = New Point(64, 34), .Size = New Size(75, 22), .Font = New Font("Segoe UI", 8.5!, FontStyle.Bold), .TextAlign = HorizontalAlignment.Center, .Text = "MT6"}

        Dim lblUM As New Label() With {.Text = "UM:", .Font = New Font("Segoe UI", 7.5!), .ForeColor = corSlate600, .Location = New Point(145, 18), .AutoSize = True}
        txtUnidade = New TextBox() With {.Location = New Point(145, 34), .Size = New Size(48, 22), .Font = New Font("Segoe UI", 8.5!, FontStyle.Bold), .TextAlign = HorizontalAlignment.Center, .Text = "CJ"}

        Dim lblNumDoc As New Label() With {.Text = "CÓD. PROTHEUS:", .Font = New Font("Segoe UI", 7.5!, FontStyle.Bold), .ForeColor = Color.FromArgb(3, 105, 161), .Location = New Point(199, 18), .AutoSize = True}
        txtNumDoc = New TextBox() With {.Location = New Point(199, 34), .Size = New Size(130, 22), .Font = New Font("Segoe UI", 8.5!, FontStyle.Bold), .ForeColor = Color.FromArgb(3, 105, 161), .BackColor = Color.FromArgb(241, 245, 249), .TextAlign = HorizontalAlignment.Center, .ReadOnly = True, .Text = "MT6-5351"}

        Dim lblRev As New Label() With {.Text = "REV. MF:", .Font = New Font("Segoe UI", 7.5!), .ForeColor = corSlate600, .Location = New Point(335, 18), .AutoSize = True}
        txtRev = New TextBox() With {.Location = New Point(335, 34), .Size = New Size(52, 22), .Font = New Font("Segoe UI", 8.5!, FontStyle.Bold), .TextAlign = HorizontalAlignment.Center, .Text = "02"}

        Dim lblRevCli As New Label() With {.Text = "REV. CLI:", .Font = New Font("Segoe UI", 7.5!), .ForeColor = corSlate600, .Location = New Point(393, 18), .AutoSize = True}
        txtRevCliente = New TextBox() With {.Location = New Point(393, 34), .Size = New Size(55, 22), .Font = New Font("Segoe UI", 8.5!, FontStyle.Bold), .TextAlign = HorizontalAlignment.Center, .Text = "02"}

        Dim lblNCM As New Label() With {.Text = "NCM:", .Font = New Font("Segoe UI", 7.5!, FontStyle.Bold), .ForeColor = corSlate600, .Location = New Point(454, 18), .AutoSize = True}
        txtNCM = New TextBox() With {.Location = New Point(454, 34), .Size = New Size(110, 22), .Font = New Font("Segoe UI", 8.5!, FontStyle.Bold), .TextAlign = HorizontalAlignment.Center, .ReadOnly = True, .BackColor = Color.FromArgb(241, 245, 249), .ForeColor = Color.FromArgb(51, 65, 85), .Text = "00000000"}

        Dim lblTitulo As New Label() With {.Text = "TÍTULO / DESCRIÇÃO PROTHEUS:", .Font = New Font("Segoe UI", 7.5!), .ForeColor = corSlate600, .Location = New Point(10, 62), .AutoSize = True}
        txtTituloProd = New TextBox() With {.Location = New Point(10, 78), .Size = New Size(554, 22), .Font = New Font("Segoe UI", 8.5!, FontStyle.Bold), .ForeColor = corSlate900, .Text = "TORRADOR - 02KG GOLD PLUS TESTE01", .Anchor = AnchorStyles.Left Or AnchorStyles.Right}

        grpProtheusCab.Controls.AddRange({lblTipo, txtTipo, lblGrupo, txtGrupo, lblUM, txtUnidade, lblNumDoc, txtNumDoc, lblRev, txtRev, lblRevCli, txtRevCliente, lblNCM, txtNCM, lblTitulo, txtTituloProd})

        ' GRUPO 2: Propriedades Físicas CAD & Blank
        Dim grpCadCab As New GroupBox() With {
            .Text = " Propriedades Físicas CAD & Blank (Solid Edge) ",
            .Dock = DockStyle.Fill,
            .Font = New Font("Segoe UI", 8.0!, FontStyle.Bold),
            .ForeColor = corSlate700,
            .Margin = New Padding(4, 2, 0, 2)
        }

        Dim lblDens As New Label() With {.Text = "MATERIAL:", .Font = New Font("Segoe UI", 7.5!), .ForeColor = corSlate600, .Location = New Point(10, 18), .AutoSize = True}
        txtDensidade = New TextBox() With {.Location = New Point(10, 34), .Size = New Size(100, 22), .Font = New Font("Segoe UI", 8.5!), .Text = "AÇO INOX"}

        Dim lblEsp As New Label() With {.Text = "ESPESSURA:", .Font = New Font("Segoe UI", 7.5!), .ForeColor = corSlate600, .Location = New Point(116, 18), .AutoSize = True}
        txtEspessura = New TextBox() With {.Location = New Point(116, 34), .Size = New Size(65, 22), .Font = New Font("Segoe UI", 8.5!), .TextAlign = HorizontalAlignment.Right, .Text = "0"}

        Dim lblLarg As New Label() With {.Text = "LARG. (MM):", .Font = New Font("Segoe UI", 7.5!), .ForeColor = corSlate600, .Location = New Point(187, 18), .AutoSize = True}
        txtLargura = New TextBox() With {.Location = New Point(187, 34), .Size = New Size(65, 22), .Font = New Font("Segoe UI", 8.5!), .TextAlign = HorizontalAlignment.Right, .Text = "0"}

        Dim lblComp As New Label() With {.Text = "COMPR. (MM):", .Font = New Font("Segoe UI", 7.5!), .ForeColor = corSlate600, .Location = New Point(258, 18), .AutoSize = True}
        txtComprimento = New TextBox() With {.Location = New Point(258, 34), .Size = New Size(65, 22), .Font = New Font("Segoe UI", 8.5!), .TextAlign = HorizontalAlignment.Right, .Text = "0"}

        Dim lblArea As New Label() With {.Text = "ÁREA (M²):", .Font = New Font("Segoe UI", 7.5!), .ForeColor = corSlate600, .Location = New Point(329, 18), .AutoSize = True}
        txtArea = New TextBox() With {.Location = New Point(329, 34), .Size = New Size(65, 22), .Font = New Font("Segoe UI", 8.5!), .TextAlign = HorizontalAlignment.Right, .Text = "0"}

        Dim lblPeso As New Label() With {.Text = "PESO (KG):", .Font = New Font("Segoe UI", 7.5!), .ForeColor = corSlate600, .Location = New Point(400, 18), .AutoSize = True}
        txtPeso = New TextBox() With {.Location = New Point(400, 34), .Size = New Size(70, 22), .Font = New Font("Segoe UI", 8.5!), .TextAlign = HorizontalAlignment.Right, .Text = "0"}

        Dim lblComent As New Label() With {.Text = "ACABAMENTO / COMENTÁRIOS:", .Font = New Font("Segoe UI", 7.5!), .ForeColor = corSlate600, .Location = New Point(10, 62), .AutoSize = True}
        txtComentarios = New TextBox() With {.Location = New Point(10, 78), .Size = New Size(460, 22), .Font = New Font("Segoe UI", 8.5!), .Text = "CINZA N6,5 TEXTURIZADO", .Anchor = AnchorStyles.Left Or AnchorStyles.Right}

        grpCadCab.Controls.AddRange({lblDens, txtDensidade, lblEsp, txtEspessura, lblLarg, txtLargura, lblComp, txtComprimento, lblArea, txtArea, lblPeso, txtPeso, lblComent, txtComentarios})

        tblGrupos.Controls.Add(grpProtheusCab, 0, 0)
        tblGrupos.Controls.Add(grpCadCab, 1, 0)

        pnlDadosGerais.Controls.Add(tblGrupos)
        pnlDadosGerais.Controls.Add(pnlHeaderDados)
    End Sub

    ' -------------------------------------------------------------------------
    ' PILAR 1: ÁRVORE DO PRODUTO (MATERIAIS) - SG1010
    ' -------------------------------------------------------------------------
    Private Sub CriarPilarMateriais()
        pnlColMateriais = New Panel() With {
            .Dock = DockStyle.Fill,
            .BackColor = Color.White,
            .Margin = New Padding(0, 0, 4, 0),
            .Padding = New Padding(8)
        }

        Dim pnlHeader As New Panel() With {.Dock = DockStyle.Top, .Height = 32}
        Dim lblTitulo As New Label() With {
            .Text = "ÁRVORE DO PRODUTO (MATERIAIS)",
            .Font = New Font("Segoe UI", 8.25!, FontStyle.Bold),
            .ForeColor = corVerdeBOM,
            .Location = New Point(0, 6),
            .AutoSize = True
        }

        lblBadgeQtdMateriais = New Label() With {
            .Text = "4",
            .Font = New Font("Segoe UI", 7.5!, FontStyle.Bold),
            .ForeColor = corVerdeBOM,
            .BackColor = corVerdeFundo,
            .Location = New Point(205, 6),
            .Size = New Size(22, 18),
            .TextAlign = ContentAlignment.MiddleCenter
        }

        btnEditarMateriais = New Button() With {
            .Text = "✏️ Editar",
            .Location = New Point(232, 4),
            .Size = New Size(62, 24),
            .BackColor = Color.White,
            .ForeColor = corVerdeBOM,
            .Font = New Font("Segoe UI", 7.5!, FontStyle.Bold),
            .FlatStyle = FlatStyle.Flat,
            .Cursor = Cursors.Hand
        }
        btnEditarMateriais.FlatAppearance.BorderColor = Color.FromArgb(167, 243, 208)
        AddHandler btnEditarMateriais.Click, AddressOf btnEditarMateriais_Click

        btnInserirMateriais = New Button() With {
            .Text = "+ Inserir Novo",
            .Location = New Point(298, 4),
            .Size = New Size(85, 24),
            .BackColor = corVerdeBOM,
            .ForeColor = Color.White,
            .Font = New Font("Segoe UI", 7.5!, FontStyle.Bold),
            .FlatStyle = FlatStyle.Flat,
            .Cursor = Cursors.Hand
        }
        btnInserirMateriais.FlatAppearance.BorderSize = 0
        AddHandler btnInserirMateriais.Click, AddressOf btnInserirMateriais_Click

        lblBadgePecaMateriais = New Label() With {
            .Text = "MT6-5351",
            .Dock = DockStyle.Right,
            .Width = 60,
            .Font = New Font("Segoe UI", 7.0!, FontStyle.Bold),
            .ForeColor = corVerdeBOM,
            .BackColor = corVerdeFundo,
            .TextAlign = ContentAlignment.MiddleCenter
        }

        pnlHeader.Controls.AddRange({lblTitulo, lblBadgeQtdMateriais, btnEditarMateriais, btnInserirMateriais, lblBadgePecaMateriais})

        dgvMateriais = New DataGridView() With {
            .Dock = DockStyle.Fill,
            .BackgroundColor = Color.White,
            .BorderStyle = BorderStyle.None,
            .AllowUserToAddRows = False,
            .AllowUserToDeleteRows = False,
            .SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            .RowHeadersVisible = False,
            .EnableHeadersVisualStyles = False,
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        }

        Dim colNiv As New DataGridViewTextBoxColumn() With {.Name = "Nivel", .HeaderText = "NÍV.", .FillWeight = 12}
        colNiv.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter

        Dim colCod As New DataGridViewTextBoxColumn() With {.Name = "Codigo", .HeaderText = "CÓDIGO", .FillWeight = 26}
        colCod.DefaultCellStyle.Font = New Font("Segoe UI", 8.5!, FontStyle.Bold)

        Dim colDesc As New DataGridViewTextBoxColumn() With {.Name = "Descricao", .HeaderText = "DESCRIÇÃO", .FillWeight = 42}

        Dim colQtd As New DataGridViewTextBoxColumn() With {.Name = "Qtd", .HeaderText = "QTDE", .FillWeight = 16}
        colQtd.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        colQtd.DefaultCellStyle.Format = "N4"

        Dim colUM As New DataGridViewTextBoxColumn() With {.Name = "UM", .HeaderText = "UN", .FillWeight = 10}
        colUM.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter

        dgvMateriais.Columns.AddRange({colNiv, colCod, colDesc, colQtd, colUM})

        pnlColMateriais.Controls.Add(dgvMateriais)
        pnlColMateriais.Controls.Add(pnlHeader)
    End Sub

    ' -------------------------------------------------------------------------
    ' PILAR 2: PRODUTO METALFISA / CLIENTE - SZ1010
    ' -------------------------------------------------------------------------
    Private Sub CriarPilarCliente()
        pnlColCliente = New Panel() With {
            .Dock = DockStyle.Fill,
            .BackColor = Color.White,
            .Margin = New Padding(4, 0, 4, 0),
            .Padding = New Padding(8)
        }

        Dim pnlHeader As New Panel() With {.Dock = DockStyle.Top, .Height = 32}
        Dim lblTitulo As New Label() With {
            .Text = "PRODUTO METALFISA/CLIENTE (SZ1)",
            .Font = New Font("Segoe UI", 8.25!, FontStyle.Bold),
            .ForeColor = corAmbarCliente,
            .Location = New Point(0, 6),
            .AutoSize = True
        }

        lblBadgeQtdCliente = New Label() With {
            .Text = "1",
            .Font = New Font("Segoe UI", 7.5!, FontStyle.Bold),
            .ForeColor = corAmbarCliente,
            .BackColor = corAmbarFundo,
            .Location = New Point(208, 6),
            .Size = New Size(20, 18),
            .TextAlign = ContentAlignment.MiddleCenter
        }

        btnEditarCliente = New Button() With {
            .Text = "✏️ Editar",
            .Location = New Point(234, 4),
            .Size = New Size(62, 24),
            .BackColor = Color.White,
            .ForeColor = corAmbarCliente,
            .Font = New Font("Segoe UI", 7.5!, FontStyle.Bold),
            .FlatStyle = FlatStyle.Flat,
            .Cursor = Cursors.Hand
        }
        btnEditarCliente.FlatAppearance.BorderColor = Color.FromArgb(253, 230, 138)
        AddHandler btnEditarCliente.Click, AddressOf btnEditarCliente_Click

        btnInserirCliente = New Button() With {
            .Text = "+ Inserir Novo",
            .Location = New Point(300, 4),
            .Size = New Size(85, 24),
            .BackColor = corAmbarCliente,
            .ForeColor = Color.White,
            .Font = New Font("Segoe UI", 7.5!, FontStyle.Bold),
            .FlatStyle = FlatStyle.Flat,
            .Cursor = Cursors.Hand
        }
        btnInserirCliente.FlatAppearance.BorderSize = 0
        AddHandler btnInserirCliente.Click, AddressOf btnInserirCliente_Click

        lblBadgePecaCliente = New Label() With {
            .Text = "MT6-5351",
            .Dock = DockStyle.Right,
            .Width = 60,
            .Font = New Font("Segoe UI", 7.0!, FontStyle.Bold),
            .ForeColor = corAmbarCliente,
            .BackColor = corAmbarFundo,
            .TextAlign = ContentAlignment.MiddleCenter
        }

        pnlHeader.Controls.AddRange({lblTitulo, lblBadgeQtdCliente, btnEditarCliente, btnInserirCliente, lblBadgePecaCliente})

        dgvCliente = New DataGridView() With {
            .Dock = DockStyle.Fill,
            .BackgroundColor = Color.White,
            .BorderStyle = BorderStyle.None,
            .AllowUserToAddRows = False,
            .AllowUserToDeleteRows = False,
            .SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            .RowHeadersVisible = False,
            .EnableHeadersVisualStyles = False,
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        }

        Dim colCli As New DataGridViewTextBoxColumn() With {.Name = "Cliente", .HeaderText = "CLIENTE", .FillWeight = 24}
        Dim colNome As New DataGridViewTextBoxColumn() With {.Name = "Nome", .HeaderText = "NOME", .FillWeight = 42}
        Dim colCodCl As New DataGridViewTextBoxColumn() With {.Name = "CodCl", .HeaderText = "CÓD CL.", .FillWeight = 22}
        Dim colRev As New DataGridViewTextBoxColumn() With {.Name = "Rev", .HeaderText = "REV", .FillWeight = 12}
        colRev.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter

        dgvCliente.Columns.AddRange({colCli, colNome, colCodCl, colRev})

        pnlColCliente.Controls.Add(dgvCliente)
        pnlColCliente.Controls.Add(pnlHeader)
    End Sub

    ' -------------------------------------------------------------------------
    ' PILAR 3: OPERAÇÕES / ROTEIRO - SG2010 + SH1010
    ' -------------------------------------------------------------------------
    Private Sub CriarPilarRoteiro()
        pnlColRoteiro = New Panel() With {
            .Dock = DockStyle.Fill,
            .BackColor = Color.White,
            .Margin = New Padding(4, 0, 0, 0),
            .Padding = New Padding(8)
        }

        Dim pnlHeader As New Panel() With {.Dock = DockStyle.Top, .Height = 32}
        Dim lblTitulo As New Label() With {
            .Text = "OPERAÇÕES (ROTEIRO)",
            .Font = New Font("Segoe UI", 8.25!, FontStyle.Bold),
            .ForeColor = corAzulRoteiro,
            .Location = New Point(0, 6),
            .AutoSize = True
        }

        lblBadgeQtdRoteiro = New Label() With {
            .Text = "2",
            .Font = New Font("Segoe UI", 7.5!, FontStyle.Bold),
            .ForeColor = corAzulRoteiro,
            .BackColor = corAzulFundo,
            .Location = New Point(145, 6),
            .Size = New Size(20, 18),
            .TextAlign = ContentAlignment.MiddleCenter
        }

        btnEditarRoteiro = New Button() With {
            .Text = "✏️ Editar",
            .Location = New Point(175, 4),
            .Size = New Size(62, 24),
            .BackColor = Color.White,
            .ForeColor = corAzulRoteiro,
            .Font = New Font("Segoe UI", 7.5!, FontStyle.Bold),
            .FlatStyle = FlatStyle.Flat,
            .Cursor = Cursors.Hand
        }
        btnEditarRoteiro.FlatAppearance.BorderColor = Color.FromArgb(191, 219, 254)
        AddHandler btnEditarRoteiro.Click, AddressOf btnEditarRoteiro_Click

        btnInserirRoteiro = New Button() With {
            .Text = "+ Inserir Novo",
            .Location = New Point(241, 4),
            .Size = New Size(85, 24),
            .BackColor = corAzulRoteiro,
            .ForeColor = Color.White,
            .Font = New Font("Segoe UI", 7.5!, FontStyle.Bold),
            .FlatStyle = FlatStyle.Flat,
            .Cursor = Cursors.Hand
        }
        btnInserirRoteiro.FlatAppearance.BorderSize = 0
        AddHandler btnInserirRoteiro.Click, AddressOf btnInserirRoteiro_Click

        lblBadgePecaRoteiro = New Label() With {
            .Text = "MT6-5351",
            .Dock = DockStyle.Right,
            .Width = 60,
            .Font = New Font("Segoe UI", 7.0!, FontStyle.Bold),
            .ForeColor = corAzulRoteiro,
            .BackColor = corAzulFundo,
            .TextAlign = ContentAlignment.MiddleCenter
        }

        pnlHeader.Controls.AddRange({lblTitulo, lblBadgeQtdRoteiro, btnEditarRoteiro, btnInserirRoteiro, lblBadgePecaRoteiro})

        dgvRoteiro = New DataGridView() With {
            .Dock = DockStyle.Fill,
            .BackgroundColor = Color.White,
            .BorderStyle = BorderStyle.None,
            .AllowUserToAddRows = False,
            .AllowUserToDeleteRows = False,
            .SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            .RowHeadersVisible = False,
            .EnableHeadersVisualStyles = False,
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        }

        Dim colOp As New DataGridViewTextBoxColumn() With {.Name = "Operacao", .HeaderText = "OPERAÇÃO", .FillWeight = 16}
        colOp.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter

        Dim colDesc As New DataGridViewTextBoxColumn() With {.Name = "Descricao", .HeaderText = "DESCRIÇÃO", .FillWeight = 42}
        Dim colRec As New DataGridViewTextBoxColumn() With {.Name = "Recurso", .HeaderText = "RECURSO", .FillWeight = 18}

        Dim colSetup As New DataGridViewTextBoxColumn() With {.Name = "Setup", .HeaderText = "SETUP (MIN)", .FillWeight = 18}
        colSetup.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight

        Dim colPadrao As New DataGridViewTextBoxColumn() With {.Name = "TempoPadrao", .HeaderText = "TEMPO PADRÃO", .FillWeight = 20}
        colPadrao.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight

        dgvRoteiro.Columns.AddRange({colOp, colDesc, colRec, colSetup, colPadrao})

        pnlColRoteiro.Controls.Add(dgvRoteiro)
        pnlColRoteiro.Controls.Add(pnlHeader)
    End Sub

    ' -------------------------------------------------------------------------
    ' PILAR 4: RESTRIÇÕES DE ENGENHARIA - SZ0010
    ' -------------------------------------------------------------------------
    Private Sub CriarSecaoRestricoes()
        pnlSecaoRestricoes = New Panel() With {
            .Dock = DockStyle.Fill,
            .BackColor = Color.White,
            .Margin = New Padding(0),
            .Padding = New Padding(8)
        }

        Dim pnlHeader As New Panel() With {.Dock = DockStyle.Top, .Height = 32}
        Dim lblTitulo As New Label() With {
            .Text = "RESTRIÇÕES DE ENGENHARIA (SZ0)",
            .Font = New Font("Segoe UI", 8.25!, FontStyle.Bold),
            .ForeColor = corVermelhoRestricao,
            .Location = New Point(0, 6),
            .AutoSize = True
        }

        lblBadgeQtdRestricoes = New Label() With {
            .Text = "0",
            .Font = New Font("Segoe UI", 7.5!, FontStyle.Bold),
            .ForeColor = corVermelhoRestricao,
            .BackColor = corVermelhoFundo,
            .Location = New Point(210, 6),
            .Size = New Size(22, 18),
            .TextAlign = ContentAlignment.MiddleCenter
        }

        btnNovaRestricao = New Button() With {
            .Text = "+ Nova",
            .Location = New Point(240, 4),
            .Size = New Size(65, 24),
            .BackColor = corVermelhoRestricao,
            .ForeColor = Color.White,
            .Font = New Font("Segoe UI", 7.5!, FontStyle.Bold),
            .FlatStyle = FlatStyle.Flat,
            .Cursor = Cursors.Hand
        }
        btnNovaRestricao.FlatAppearance.BorderSize = 0
        AddHandler btnNovaRestricao.Click, AddressOf btnNovaRestricao_Click

        lblBadgePecaRestricao = New Label() With {
            .Text = "MT6-5351",
            .Dock = DockStyle.Right,
            .Width = 60,
            .Font = New Font("Segoe UI", 7.0!, FontStyle.Bold),
            .ForeColor = corVermelhoRestricao,
            .BackColor = corVermelhoFundo,
            .TextAlign = ContentAlignment.MiddleCenter
        }

        pnlHeader.Controls.AddRange({lblTitulo, lblBadgeQtdRestricoes, btnNovaRestricao, lblBadgePecaRestricao})

        dgvRestricoes = New DataGridView() With {
            .Dock = DockStyle.Fill,
            .BackgroundColor = Color.White,
            .BorderStyle = BorderStyle.None,
            .AllowUserToAddRows = False,
            .AllowUserToDeleteRows = False,
            .SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            .RowHeadersVisible = False,
            .EnableHeadersVisualStyles = False,
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        }

        Dim colItem As New DataGridViewTextBoxColumn() With {.Name = "Item", .HeaderText = "ITEM", .FillWeight = 8}
        colItem.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter

        Dim colStatus As New DataGridViewTextBoxColumn() With {.Name = "Status", .HeaderText = "STATUS", .FillWeight = 10}
        colStatus.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter

        Dim colResumo As New DataGridViewTextBoxColumn() With {.Name = "Resumo", .HeaderText = "RESUMO", .FillWeight = 24}
        Dim colDescRestric As New DataGridViewTextBoxColumn() With {.Name = "Descricao", .HeaderText = "DESCRIÇÃO DA RESTRIÇÃO *", .FillWeight = 32}

        Dim colRev As New DataGridViewTextBoxColumn() With {.Name = "Revisao", .HeaderText = "REVISÃO", .FillWeight = 10}
        colRev.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter

        Dim colResp As New DataGridViewTextBoxColumn() With {.Name = "Responsavel", .HeaderText = "RESPONSÁVEL", .FillWeight = 14}
        Dim colDtHr As New DataGridViewTextBoxColumn() With {.Name = "UltAlteracao", .HeaderText = "ÚLT. ALTERAÇÃO", .FillWeight = 16}
        colDtHr.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter

        dgvRestricoes.Columns.AddRange({colItem, colStatus, colResumo, colDescRestric, colRev, colResp, colDtHr})

        lblSemRestricoes = New Label() With {
            .Text = "Nenhuma restrição SZ0 encontrada para este produto",
            .Dock = DockStyle.Fill,
            .Font = New Font("Segoe UI", 9.5!, FontStyle.Italic),
            .ForeColor = Color.FromArgb(148, 163, 184),
            .TextAlign = ContentAlignment.MiddleCenter,
            .BackColor = Color.White,
            .Visible = False
        }

        pnlSecaoRestricoes.Controls.Add(lblSemRestricoes)
        pnlSecaoRestricoes.Controls.Add(dgvRestricoes)
        pnlSecaoRestricoes.Controls.Add(pnlHeader)
    End Sub

    ' -------------------------------------------------------------------------
    ' TAB 2: ESTRUTURA EM ÁRVORE COMPLETA (MULTINÍVEL - RECURSIVA CTE)
    ' -------------------------------------------------------------------------
    Private Sub CriarTabArvore()
        pnlContentArvore = New Panel() With {
            .Dock = DockStyle.Fill,
            .Visible = False,
            .BackColor = Color.FromArgb(248, 250, 252),
            .Padding = New Padding(12, 10, 12, 10)
        }

        Dim pnlBarraArvore As New Panel() With {.Dock = DockStyle.Top, .Height = 40, .BackColor = Color.White, .Padding = New Padding(10, 6, 10, 6)}
        Dim lblTituloArvore As New Label() With {
            .Text = "🌲  Árvore Completa Multinível (Explosão SG1010 até Nível 12)",
            .Font = New Font("Segoe UI", 9.5!, FontStyle.Bold),
            .ForeColor = corSlate900,
            .Location = New Point(10, 10),
            .AutoSize = True
        }

        lblTotalNosArvore = New Label() With {
            .Text = "41 nós no total",
            .Font = New Font("Segoe UI", 8.0!, FontStyle.Bold),
            .ForeColor = corVerdeBOM,
            .BackColor = corVerdeFundo,
            .Location = New Point(420, 10),
            .Size = New Size(110, 20),
            .TextAlign = ContentAlignment.MiddleCenter
        }

        btnExportarExcelArvore = New Button() With {
            .Text = "📊  Exportar Excel",
            .Dock = DockStyle.Right,
            .Width = 130,
            .BackColor = Color.White,
            .ForeColor = corSlate700,
            .Font = New Font("Segoe UI", 8.5!, FontStyle.Bold),
            .FlatStyle = FlatStyle.Flat,
            .Cursor = Cursors.Hand
        }
        btnExportarExcelArvore.FlatAppearance.BorderColor = corSlate200
        AddHandler btnExportarExcelArvore.Click, AddressOf btnExportarExcelArvore_Click

        btnRecolherArvore = New Button() With {
            .Text = "➖  Recolher",
            .Dock = DockStyle.Right,
            .Width = 95,
            .BackColor = Color.White,
            .ForeColor = corSlate700,
            .Font = New Font("Segoe UI", 8.5!, FontStyle.Regular),
            .FlatStyle = FlatStyle.Flat,
            .Cursor = Cursors.Hand
        }
        btnRecolherArvore.FlatAppearance.BorderColor = corSlate200
        AddHandler btnRecolherArvore.Click, Sub(s, e) tvwArvoreBOM.CollapseAll()

        btnExpandirArvore = New Button() With {
            .Text = "➕  Expandir Tudo",
            .Dock = DockStyle.Right,
            .Width = 120,
            .BackColor = Color.White,
            .ForeColor = corSlate700,
            .Font = New Font("Segoe UI", 8.5!, FontStyle.Regular),
            .FlatStyle = FlatStyle.Flat,
            .Cursor = Cursors.Hand
        }
        btnExpandirArvore.FlatAppearance.BorderColor = corSlate200
        AddHandler btnExpandirArvore.Click, Sub(s, e) tvwArvoreBOM.ExpandAll()

        pnlBarraArvore.Controls.AddRange({lblTituloArvore, lblTotalNosArvore, btnExpandirArvore, btnRecolherArvore, btnExportarExcelArvore})

        splitArvore = New SplitContainer() With {
            .Dock = DockStyle.Fill,
            .Orientation = Orientation.Vertical,
            .SplitterDistance = 420,
            .BackColor = corSlate200,
            .Padding = New Padding(0, 8, 0, 0)
        }

        ' TreeView à Esquerda
        tvwArvoreBOM = New TreeView() With {
            .Dock = DockStyle.Fill,
            .BorderStyle = BorderStyle.None,
            .Font = New Font("Segoe UI", 9.0!, FontStyle.Regular),
            .FullRowSelect = True,
            .ShowLines = True,
            .ShowPlusMinus = True,
            .ShowRootLines = True
        }

        ' Grid à Direita
        dgvArvoreCompleta = New DataGridView() With {
            .Dock = DockStyle.Fill,
            .BackgroundColor = Color.White,
            .BorderStyle = BorderStyle.None,
            .AllowUserToAddRows = False,
            .AllowUserToDeleteRows = False,
            .SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            .RowHeadersVisible = False,
            .EnableHeadersVisualStyles = False,
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        }

        Dim colLvl As New DataGridViewTextBoxColumn() With {.Name = "Level", .HeaderText = "NÍVEL", .FillWeight = 8}
        colLvl.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter

        Dim colCompPai As New DataGridViewTextBoxColumn() With {.Name = "Pai", .HeaderText = "PAI", .FillWeight = 16}
        Dim colCompFilho As New DataGridViewTextBoxColumn() With {.Name = "Componente", .HeaderText = "COMPONENTE", .FillWeight = 18}
        colCompFilho.DefaultCellStyle.Font = New Font("Segoe UI", 8.5!, FontStyle.Bold)

        Dim colDescFull As New DataGridViewTextBoxColumn() With {.Name = "Descricao", .HeaderText = "DESCRIÇÃO OFICIAL", .FillWeight = 34}
        Dim colTipoFull As New DataGridViewTextBoxColumn() With {.Name = "Tipo", .HeaderText = "TIPO", .FillWeight = 8}
        colTipoFull.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter

        Dim colUMFull As New DataGridViewTextBoxColumn() With {.Name = "UM", .HeaderText = "UN", .FillWeight = 7}
        colUMFull.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter

        Dim colQtdFull As New DataGridViewTextBoxColumn() With {.Name = "Quantidade", .HeaderText = "QTDE ACUMULADA", .FillWeight = 15}
        colQtdFull.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        colQtdFull.DefaultCellStyle.Format = "N4"

        Dim colPath As New DataGridViewTextBoxColumn() With {.Name = "Caminho", .HeaderText = "CAMINHO NA ÁRVORE", .FillWeight = 40}

        dgvArvoreCompleta.Columns.AddRange({colLvl, colCompPai, colCompFilho, colDescFull, colTipoFull, colUMFull, colQtdFull, colPath})

        splitArvore.Panel1.Controls.Add(tvwArvoreBOM)
        splitArvore.Panel2.Controls.Add(dgvArvoreCompleta)

        pnlContentArvore.Controls.Add(splitArvore)
        pnlContentArvore.Controls.Add(pnlBarraArvore)
    End Sub

    ' -------------------------------------------------------------------------
    ' STATUS BAR
    ' -------------------------------------------------------------------------
    Private Sub CriarStatusBar()
        pnlStatusBar = New Panel() With {
            .Dock = DockStyle.Bottom,
            .Height = 28,
            .BackColor = Color.White,
            .Padding = New Padding(12, 4, 12, 4)
        }

        lblStatus = New Label() With {
            .Dock = DockStyle.Fill,
            .Text = "Pronto.",
            .ForeColor = corSlate600,
            .Font = New Font("Segoe UI", 8.5!, FontStyle.Regular),
            .TextAlign = ContentAlignment.MiddleLeft
        }

        progressBar = New ProgressBar() With {
            .Dock = DockStyle.Right,
            .Width = 180,
            .Visible = False
        }

        pnlStatusBar.Controls.Add(lblStatus)
        pnlStatusBar.Controls.Add(progressBar)
    End Sub

    ' -------------------------------------------------------------------------
    ' CONFIGURAÇÃO VISUAL DE TABELAS
    ' -------------------------------------------------------------------------
    Private Sub ConfigurarEstilos()
        ' Materiais
        dgvMateriais.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(241, 245, 249)
        dgvMateriais.ColumnHeadersDefaultCellStyle.ForeColor = corSlate700
        dgvMateriais.ColumnHeadersDefaultCellStyle.Font = New Font("Segoe UI", 8.25!, FontStyle.Bold)
        dgvMateriais.ColumnHeadersHeight = 28
        dgvMateriais.RowTemplate.Height = 26
        dgvMateriais.GridColor = corSlate200
        dgvMateriais.DefaultCellStyle.SelectionBackColor = Color.FromArgb(224, 242, 254)
        dgvMateriais.DefaultCellStyle.SelectionForeColor = corSlate900
        dgvMateriais.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 250, 252)

        ' Cliente
        dgvCliente.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(241, 245, 249)
        dgvCliente.ColumnHeadersDefaultCellStyle.ForeColor = corSlate700
        dgvCliente.ColumnHeadersDefaultCellStyle.Font = New Font("Segoe UI", 8.25!, FontStyle.Bold)
        dgvCliente.ColumnHeadersHeight = 28
        dgvCliente.RowTemplate.Height = 26
        dgvCliente.GridColor = corSlate200
        dgvCliente.DefaultCellStyle.SelectionBackColor = Color.FromArgb(224, 242, 254)
        dgvCliente.DefaultCellStyle.SelectionForeColor = corSlate900
        dgvCliente.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 250, 252)

        ' Roteiro
        dgvRoteiro.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(241, 245, 249)
        dgvRoteiro.ColumnHeadersDefaultCellStyle.ForeColor = corSlate700
        dgvRoteiro.ColumnHeadersDefaultCellStyle.Font = New Font("Segoe UI", 8.25!, FontStyle.Bold)
        dgvRoteiro.ColumnHeadersHeight = 28
        dgvRoteiro.RowTemplate.Height = 26
        dgvRoteiro.GridColor = corSlate200
        dgvRoteiro.DefaultCellStyle.SelectionBackColor = Color.FromArgb(224, 242, 254)
        dgvRoteiro.DefaultCellStyle.SelectionForeColor = corSlate900
        dgvRoteiro.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 250, 252)

        ' Restrições
        dgvRestricoes.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(241, 245, 249)
        dgvRestricoes.ColumnHeadersDefaultCellStyle.ForeColor = corSlate700
        dgvRestricoes.ColumnHeadersDefaultCellStyle.Font = New Font("Segoe UI", 8.25!, FontStyle.Bold)
        dgvRestricoes.ColumnHeadersHeight = 28
        dgvRestricoes.RowTemplate.Height = 26
        dgvRestricoes.GridColor = corSlate200
        dgvRestricoes.DefaultCellStyle.SelectionBackColor = Color.FromArgb(224, 242, 254)
        dgvRestricoes.DefaultCellStyle.SelectionForeColor = corSlate900
        dgvRestricoes.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 250, 252)

        ' Árvore Completa
        If dgvArvoreCompleta IsNot Nothing Then
            dgvArvoreCompleta.ColumnHeadersDefaultCellStyle.BackColor = corSlate100
            dgvArvoreCompleta.ColumnHeadersDefaultCellStyle.ForeColor = corSlate800
            dgvArvoreCompleta.ColumnHeadersDefaultCellStyle.Font = New Font("Segoe UI", 8.5!, FontStyle.Bold)
            dgvArvoreCompleta.ColumnHeadersHeight = 30
            dgvArvoreCompleta.RowTemplate.Height = 26
            dgvArvoreCompleta.GridColor = corSlate200
        End If
    End Sub

    ' =========================================================================
    ' CARREGAMENTO COMPLETO DE TODOS OS MÓDULOS (ESTUDO MT6-5351)
    ' =========================================================================
    Public Sub CarregarTodosModulos(codigo As String)
        codigo = ObterCodigoProtheusLimpo(codigo)
        If String.IsNullOrWhiteSpace(codigo) Then Exit Sub

        CodigoProduto = codigo
        txtBuscaERP.Text = codigo
        txtNumDoc.Text = codigo
        lblBadgePecaTop.Text = codigo
        lblBadgePecaMateriais.Text = codigo
        lblBadgePecaCliente.Text = codigo
        lblBadgePecaRoteiro.Text = codigo
        lblBadgePecaRestricao.Text = codigo

        Me.Cursor = Cursors.WaitCursor
        lblStatus.Text = $"Consultando Protheus para {codigo}..."
        progressBar.Visible = True
        progressBar.Value = 15
        Application.DoEvents()

        Try
            Using conn As New NpgsqlConnection(connStringProtheus)
                conn.Open()

                ' 1. SB1010 - Dados Gerais do Produto
                CarregarDadosGerais(codigo, conn)
                progressBar.Value = 35

                ' 2. SG1010 - Árvore de Materiais (Nível 1)
                CarregarMateriais(codigo, conn)
                progressBar.Value = 55

                ' 3. SZ1010 - Produto x Cliente
                CarregarProdutoCliente(codigo, conn)
                progressBar.Value = 75

                ' 4. SG2010 - Roteiro de Operações
                CarregarRoteiroOperacoes(codigo, conn)
                progressBar.Value = 85

                ' 5. SZ0010 - Restrições de Engenharia
                CarregarRestricoes(codigo, conn)
                progressBar.Value = 95

                ' 6. Total de Nós na Árvore Completa (CTE)
                AtualizarBadgeArvore(codigo, conn)
                progressBar.Value = 100
            End Using

            lblStatus.Text = $"Produto {codigo} carregado com sucesso no padrão SINCO/Protheus."

        Catch ex As Exception
            MessageBox.Show("Erro ao carregar dados do produto do Protheus: " & ex.Message, "Erro Protheus", MessageBoxButtons.OK, MessageBoxIcon.Error)
            lblStatus.Text = "Erro ao consultar Protheus."
        Finally
            Me.Cursor = Cursors.Default
            progressBar.Visible = False
            progressBar.Value = 0
        End Try
    End Sub

    ' -------------------------------------------------------------------------
    ' 1. CONSULTA SB1010 (DADOS GERAIS DO PRODUTO)
    ' -------------------------------------------------------------------------
    Private Sub CarregarDadosGerais(codigo As String, conn As NpgsqlConnection)
        Dim sql As String = "SELECT b1_cod, b1_desc, b1_tipo, b1_um, b1_grupo, b1_posipi, b1_xrevm, b1_xrevc, b1_xcoddes, b1_peso, b1_locpad " &
                            "FROM public.sb1010 " &
                            "WHERE (TRIM(b1_cod) = @Cod OR TRIM(b1_cod) = @CodPadded) " &
                            "  AND COALESCE(d_e_l_e_t_, '') NOT IN ('*', '  *') LIMIT 1;"

        Using cmd As New NpgsqlCommand(sql, conn)
            cmd.Parameters.AddWithValue("@Cod", codigo)
            cmd.Parameters.AddWithValue("@CodPadded", codigo.PadRight(15, " "c))

            Using rdr = cmd.ExecuteReader()
                If rdr.Read() Then
                    txtTipo.Text = If(IsDBNull(rdr("b1_tipo")), "PI", rdr("b1_tipo").ToString().Trim())
                    txtUnidade.Text = If(IsDBNull(rdr("b1_um")), "CJ", rdr("b1_um").ToString().Trim())
                    txtGrupo.Text = If(IsDBNull(rdr("b1_grupo")), "MT6", rdr("b1_grupo").ToString().Trim())
                    txtTituloProd.Text = If(IsDBNull(rdr("b1_desc")), "", rdr("b1_desc").ToString().Trim())

                    ' ═══════════════════════════════════════════════════════════
                    ' REGRA INVIOLÁVEL: SEMPRE prioriza os dados existentes do banco!
                    ' ═══════════════════════════════════════════════════════════
                    ' 1. Revisão MetalFisa (B1_XREVM)
                    Dim revMFBanco As String = If(IsDBNull(rdr("b1_xrevm")), "", rdr("b1_xrevm").ToString().Trim())
                    txtRev.Text = If(Not String.IsNullOrEmpty(revMFBanco), revMFBanco, "00")

                    ' 2. Revisão Cliente (B1_XREVC)
                    Dim revCliBanco As String = If(IsDBNull(rdr("b1_xrevc")), "", rdr("b1_xrevc").ToString().Trim())
                    txtRevCliente.Text = revCliBanco

                    ' 3. NCM (B1_POSIPI) - Bloqueia para edição apenas se já existe código fiscal real e válido no banco Protheus
                    Dim ncmBanco As String = If(IsDBNull(rdr("b1_posipi")), "", rdr("b1_posipi").ToString().Trim())
                    Dim ncmBancoLimpo As String = ncmBanco.Replace("."c, "").Replace("-"c, "").Trim()
                    Dim temNCMValidoNoBanco As Boolean = Not String.IsNullOrEmpty(ncmBancoLimpo) AndAlso 
                                                         ncmBancoLimpo <> "00000000" AndAlso 
                                                         ncmBancoLimpo.Replace("0"c, "").Length > 0 AndAlso 
                                                         ncmBancoLimpo.Length >= 8

                    If temNCMValidoNoBanco Then
                        txtNCM.Text = ncmBanco
                        txtNCM.ReadOnly = True
                        txtNCM.BackColor = Color.FromArgb(241, 245, 249)
                        txtNCM.ForeColor = Color.FromArgb(51, 65, 85)
                    Else
                        txtNCM.Text = If(Not String.IsNullOrEmpty(ncmBanco) AndAlso ncmBanco <> "00000000", ncmBanco, "")
                        txtNCM.ReadOnly = False
                        txtNCM.BackColor = Color.White
                        txtNCM.ForeColor = Color.FromArgb(15, 23, 42)
                    End If

                    Dim pesoVal As Double = If(IsDBNull(rdr("b1_peso")), 0, Convert.ToDouble(rdr("b1_peso")))
                    If pesoVal > 0 Then
                        txtPeso.Text = pesoVal.ToString("N3")
                    End If
                Else
                    txtNCM.Text = ""
                    txtNCM.ReadOnly = False
                    txtNCM.BackColor = Color.White
                    txtNCM.ForeColor = Color.FromArgb(15, 23, 42)
                End If
            End Using
        End Using

        ' Se a Revisão do Cliente veio vazia da SB1010, busca na amarração SZ1010
        If String.IsNullOrEmpty(txtRevCliente.Text) Then
            Try
                Dim sqlRevCli As String = "SELECT z1_revisao FROM public.sz1010 WHERE TRIM(z1_produto) = @Cod AND COALESCE(d_e_l_e_t_, '') = ' ' LIMIT 1;"
                Using cmdCli As New NpgsqlCommand(sqlRevCli, conn)
                    cmdCli.Parameters.AddWithValue("@Cod", codigo)
                    Dim cliRevObj = cmdCli.ExecuteScalar()
                    If cliRevObj IsNot Nothing AndAlso Not IsDBNull(cliRevObj) Then
                        txtRevCliente.Text = cliRevObj.ToString().Trim()
                    End If
                End Using
            Catch
            End Try
        End If
    End Sub

    ' -------------------------------------------------------------------------
    ' 2. CONSULTA SG1010 (ÁRVORE DE MATERIAIS - NÍVEL 1)
    ' -------------------------------------------------------------------------
    Private Sub CarregarMateriais(codigo As String, conn As NpgsqlConnection)
        Dim sql As String = "SELECT " &
                            "    TRIM(g.g1_niv)    AS nivel, " &
                            "    TRIM(g.g1_comp)   AS componente, " &
                            "    g.g1_quant        AS quantidade, " &
                            "    TRIM(g.g1_trt)    AS trt, " &
                            "    TRIM(b.b1_desc)   AS descricao, " &
                            "    TRIM(b.b1_um)     AS unidade, " &
                            "    TRIM(b.b1_tipo)   AS tipo " &
                            "FROM public.sg1010 g " &
                            "LEFT JOIN public.sb1010 b " &
                            "    ON TRIM(b.b1_cod) = TRIM(g.g1_comp) " &
                            "    AND COALESCE(b.d_e_l_e_t_, '') NOT IN ('*', '  *') " &
                            "WHERE TRIM(g.g1_cod) = @Cod " &
                            "  AND COALESCE(g.d_e_l_e_t_, '') NOT IN ('*', '  *') " &
                            "ORDER BY g.g1_niv, g.g1_comp;"

        dgvMateriais.Rows.Clear()
        Using cmd As New NpgsqlCommand(sql, conn)
            cmd.Parameters.AddWithValue("@Cod", codigo)
            Using da As New NpgsqlDataAdapter(cmd)
                Dim dt As New DataTable()
                da.Fill(dt)

                For Each r As DataRow In dt.Rows
                    Dim niv = If(IsDBNull(r("nivel")), "02", r("nivel").ToString().Trim())
                    If String.IsNullOrEmpty(niv) Then niv = "02"
                    Dim comp = If(IsDBNull(r("componente")), "", r("componente").ToString().Trim())
                    Dim desc = If(IsDBNull(r("descricao")), "", r("descricao").ToString().Trim())
                    Dim qtd = If(IsDBNull(r("quantidade")), 1.0, Convert.ToDouble(r("quantidade")))
                    Dim um = If(IsDBNull(r("unidade")), "PC", r("unidade").ToString().Trim())

                    dgvMateriais.Rows.Add(niv, comp, desc, qtd, um)
                Next

                lblBadgeQtdMateriais.Text = dt.Rows.Count.ToString()
            End Using
        End Using
    End Sub

    ' -------------------------------------------------------------------------
    ' 3. CONSULTA SZ1010 + SA1010 (PRODUTO X CLIENTE)
    ' -------------------------------------------------------------------------
    Private Sub CarregarProdutoCliente(codigo As String, conn As NpgsqlConnection)
        Dim sql As String = "SELECT " &
                            "    TRIM(z1.z1_cliente) AS cliente, " &
                            "    TRIM(COALESCE(a1.a1_nome, '')) AS nome, " &
                            "    TRIM(z1.z1_codcli)  AS codcli, " &
                            "    TRIM(z1.z1_revisao) AS revisao " &
                            "FROM public.sz1010 z1 " &
                            "LEFT JOIN public.sa1010 a1 " &
                            "    ON TRIM(a1.a1_cod) = TRIM(z1.z1_cliente) " &
                            "    AND COALESCE(a1.d_e_l_e_t_, '') = ' ' " &
                            "WHERE TRIM(z1.z1_produto) = @Cod " &
                            "  AND COALESCE(z1.d_e_l_e_t_, '') = ' ' " &
                            "ORDER BY z1.z1_cliente;"

        dgvCliente.Rows.Clear()
        Using cmd As New NpgsqlCommand(sql, conn)
            cmd.Parameters.AddWithValue("@Cod", codigo)
            Using da As New NpgsqlDataAdapter(cmd)
                Dim dt As New DataTable()
                da.Fill(dt)

                For Each r As DataRow In dt.Rows
                    Dim cli = If(IsDBNull(r("cliente")), "", r("cliente").ToString().Trim())
                    Dim nome = If(IsDBNull(r("nome")), "", r("nome").ToString().Trim())
                    Dim codcl = If(IsDBNull(r("codcli")), "S/CODIGO", r("codcli").ToString().Trim())
                    Dim rev = If(IsDBNull(r("revisao")), "02", r("revisao").ToString().Trim())

                    dgvCliente.Rows.Add(cli, nome, codcl, rev)
                Next

                lblBadgeQtdCliente.Text = dt.Rows.Count.ToString()
            End Using
        End Using
    End Sub

    ' -------------------------------------------------------------------------
    ' 4. CONSULTA SG2010 + SH1010 (OPERAÇÕES E CENTROS DE TRABALHO)
    ' -------------------------------------------------------------------------
    Private Sub CarregarRoteiroOperacoes(codigo As String, conn As NpgsqlConnection)
        Dim sql As String = "SELECT " &
                            "    TRIM(g2.g2_operac)                                          AS operacao, " &
                            "    TRIM(COALESCE(NULLIF(g2.g2_descri, ''), h1.h1_descri, '')) AS descricao, " &
                            "    TRIM(g2.g2_recurso)                                         AS recurso, " &
                            "    ROUND(g2.g2_tempad::numeric, 3)                             AS tempo, " &
                            "    COALESCE(g2.g2_setup, 0)                                    AS setup " &
                            "FROM public.sg2010 g2 " &
                            "LEFT JOIN public.sh1010 h1 " &
                            "    ON h1.h1_codigo = g2.g2_recurso " &
                            "    AND COALESCE(h1.d_e_l_e_t_, '') IN ('', ' ') " &
                            "WHERE TRIM(g2.g2_produto) = @Cod " &
                            "  AND COALESCE(g2.d_e_l_e_t_, '') NOT IN ('*', '  *') " &
                            "  AND TRIM(g2.g2_codigo) = ( " &
                            "      SELECT MAX(TRIM(g2_sub.g2_codigo)) " &
                            "      FROM public.sg2010 g2_sub " &
                            "      WHERE TRIM(g2_sub.g2_produto) = TRIM(g2.g2_produto) " &
                            "      AND COALESCE(g2_sub.d_e_l_e_t_, '') NOT IN ('*', '  *') " &
                            "  ) " &
                            "ORDER BY g2.g2_operac;"

        dgvRoteiro.Rows.Clear()
        Using cmd As New NpgsqlCommand(sql, conn)
            cmd.Parameters.AddWithValue("@Cod", codigo)
            Using da As New NpgsqlDataAdapter(cmd)
                Dim dt As New DataTable()
                da.Fill(dt)

                For Each r As DataRow In dt.Rows
                    Dim op = If(IsDBNull(r("operacao")), "10", r("operacao").ToString().Trim())
                    Dim desc = If(IsDBNull(r("descricao")), "", r("descricao").ToString().Trim())
                    Dim rec = If(IsDBNull(r("recurso")), "", r("recurso").ToString().Trim())
                    Dim setupVal As Double = If(IsDBNull(r("setup")), 0, Convert.ToDouble(r("setup")))
                    Dim tempoVal As Double = If(IsDBNull(r("tempo")), 1, Convert.ToDouble(r("tempo")))

                    dgvRoteiro.Rows.Add(op, desc, rec, $"{setupVal:N2} min", $"{tempoVal:N3} min")
                Next

                lblBadgeQtdRoteiro.Text = dt.Rows.Count.ToString()
            End Using
        End Using
    End Sub

    ' -------------------------------------------------------------------------
    ' 5. CONSULTA SZ0010 (RESTRIÇÕES DE ENGENHARIA COM SUPORTE A BYTEA)
    ' -------------------------------------------------------------------------
    Private Sub CarregarRestricoes(codigo As String, conn As NpgsqlConnection)
        Dim sql As String = "SELECT " &
                            "    Z0.z0_item           AS item, " &
                            "    Z0.z0_status         AS status, " &
                            "    Z0.z0_resumo         AS resumo, " &
                            "    Z0.z0_restric        AS restricao_raw, " &
                            "    Z0.z0_respons        AS responsavel, " &
                            "    Z0.z0_dtultal        AS dt_ultima_alt, " &
                            "    Z0.z0_hrultal        AS hr_ultima_alt, " &
                            "    Z0.z0_xrevm          AS revisao " &
                            "FROM public.sz0010 Z0 " &
                            "WHERE TRIM(Z0.z0_produto) = @Cod " &
                            "  AND COALESCE(Z0.d_e_l_e_t_, '') NOT IN ('*', '  *') " &
                            "  AND COALESCE(Z0.z0_status, '') <> 'L' " &
                            "ORDER BY Z0.z0_item;"

        dgvRestricoes.Rows.Clear()
        Using cmd As New NpgsqlCommand(sql, conn)
            cmd.Parameters.AddWithValue("@Cod", codigo)
            Using da As New NpgsqlDataAdapter(cmd)
                Dim dt As New DataTable()
                da.Fill(dt)

                For Each r As DataRow In dt.Rows
                    Dim item = If(IsDBNull(r("item")), "001", r("item").ToString().Trim())
                    Dim status = If(IsDBNull(r("status")), "P", r("status").ToString().Trim())
                    Dim resumo = If(IsDBNull(r("resumo")), "", r("resumo").ToString().Trim())

                    ' Decodificação cuidadosa de bytea sem terminador nulo
                    Dim descRestricao As String = ""
                    If Not IsDBNull(r("restricao_raw")) Then
                        Dim rawBytes As Byte() = TryCast(r("restricao_raw"), Byte())
                        If rawBytes IsNot Nothing AndAlso rawBytes.Length > 0 Then
                            ' Filtra bytes \0
                            Dim cleanBytes = rawBytes.Where(Function(b) b <> 0).ToArray()
                            Dim strUtf8 = Encoding.UTF8.GetString(cleanBytes)
                            If strUtf8.Contains(ChrW(&HFFFD)) Then
                                descRestricao = Encoding.GetEncoding("ISO-8859-1").GetString(cleanBytes).Trim()
                            Else
                                descRestricao = strUtf8.Trim()
                            End If
                        End If
                    End If

                    Dim rev = If(IsDBNull(r("revisao")), "02", r("revisao").ToString().Trim())
                    Dim resp = If(IsDBNull(r("responsavel")), "ENGENHARIA", r("responsavel").ToString().Trim())
                    Dim dtAlt = If(IsDBNull(r("dt_ultima_alt")), "", r("dt_ultima_alt").ToString().Trim())
                    Dim hrAlt = If(IsDBNull(r("hr_ultima_alt")), "", r("hr_ultima_alt").ToString().Trim())
                    Dim dataHora = If(String.IsNullOrEmpty(dtAlt), "-", $"{dtAlt} {hrAlt}")

                    dgvRestricoes.Rows.Add(item, status, resumo, descRestricao, rev, resp, dataHora)
                Next

                lblBadgeQtdRestricoes.Text = dt.Rows.Count.ToString()

                ' Se vazio, mostra label amigável idêntica à imagem de referência
                If dt.Rows.Count = 0 Then
                    lblSemRestricoes.Visible = True
                    dgvRestricoes.Visible = False
                Else
                    lblSemRestricoes.Visible = False
                    dgvRestricoes.Visible = True
                End If
            End Using
        End Using
    End Sub

    ' -------------------------------------------------------------------------
    ' 6. ÁRVORE COMPLETA MULTINÍVEL (RECURSIVIDADE CTE ATÉ NÍVEL 12)
    ' -------------------------------------------------------------------------
    Private Sub AtualizarBadgeArvore(codigo As String, conn As NpgsqlConnection)
        Dim sql As String = "WITH RECURSIVE bom_tree AS ( " &
                            "  SELECT 1 as level, TRIM(g.g1_cod) as pai, TRIM(g.g1_comp) as componente, ARRAY[TRIM(g.g1_cod)] as path " &
                            "  FROM public.sg1010 g " &
                            "  WHERE TRIM(g.g1_cod) = @Cod AND COALESCE(g.d_e_l_e_t_, '') NOT IN ('*', '  *') " &
                            "  UNION ALL " &
                            "  SELECT t.level + 1, TRIM(g.g1_cod), TRIM(g.g1_comp), t.path || TRIM(g.g1_cod) " &
                            "  FROM public.sg1010 g " &
                            "  INNER JOIN bom_tree t ON TRIM(g.g1_cod) = t.componente " &
                            "  WHERE t.level < 12 AND NOT (TRIM(g.g1_comp) = ANY(t.path)) AND COALESCE(g.d_e_l_e_t_, '') NOT IN ('*', '  *') " &
                            ") " &
                            "SELECT COUNT(*) as total FROM bom_tree;"

        Using cmd As New NpgsqlCommand(sql, conn)
            cmd.Parameters.AddWithValue("@Cod", codigo)
            Dim totalObj = cmd.ExecuteScalar()
            Dim totalSubItens As Integer = If(totalObj IsNot Nothing, Convert.ToInt32(totalObj), 0)
            ' + 1 para o produto raiz (exatamente 41 nós para MT6-5351)
            Dim totalNos As Integer = totalSubItens + 1

            If btnTabArvore IsNot Nothing Then
                btnTabArvore.Text = $"🌲  Estrutura em Árvore Completa   {totalNos} "
            End If
            If lblTotalNosArvore IsNot Nothing Then
                lblTotalNosArvore.Text = $"{totalNos} nós no total"
            End If
        End Using
    End Sub

    Public Sub CarregarArvoreMultinivel(codigo As String)
        Try
            Me.Cursor = Cursors.WaitCursor
            lblStatus.Text = $"Explodindo árvore multinível recursiva de {codigo}..."

            Dim sql As String = "WITH RECURSIVE bom_tree AS ( " &
                                "  SELECT " &
                                "    1 as level, " &
                                "    TRIM(g.g1_cod) as pai, " &
                                "    TRIM(g.g1_comp) as componente, " &
                                "    g.g1_quant::numeric as quantidade, " &
                                "    TRIM(g.g1_trt) as trt, " &
                                "    TRIM(b.b1_desc) as descricao, " &
                                "    TRIM(b.b1_tipo) as tipo, " &
                                "    TRIM(b.b1_um) as um, " &
                                "    ARRAY[TRIM(g.g1_cod)] as path, " &
                                "    TRIM(g.g1_comp) as path_str " &
                                "  FROM public.sg1010 g " &
                                "  LEFT JOIN public.sb1010 b ON TRIM(b.b1_cod) = TRIM(g.g1_comp) AND COALESCE(b.d_e_l_e_t_, '') NOT IN ('*', '  *') " &
                                "  WHERE TRIM(g.g1_cod) = @Cod AND COALESCE(g.d_e_l_e_t_, '') NOT IN ('*', '  *') " &
                                "  UNION ALL " &
                                "  SELECT " &
                                "    t.level + 1, " &
                                "    TRIM(g.g1_cod), " &
                                "    TRIM(g.g1_comp), " &
                                "    (t.quantidade * g.g1_quant)::numeric, " &
                                "    TRIM(g.g1_trt), " &
                                "    TRIM(b.b1_desc), " &
                                "    TRIM(b.b1_tipo), " &
                                "    TRIM(b.b1_um), " &
                                "    t.path || TRIM(g.g1_cod), " &
                                "    t.path_str || ' > ' || TRIM(g.g1_comp) " &
                                "  FROM public.sg1010 g " &
                                "  INNER JOIN bom_tree t ON TRIM(g.g1_cod) = t.componente " &
                                "  LEFT JOIN public.sb1010 b ON TRIM(b.b1_cod) = TRIM(g.g1_comp) AND COALESCE(b.d_e_l_e_t_, '') NOT IN ('*', '  *') " &
                                "  WHERE t.level < 12 AND NOT (TRIM(g.g1_comp) = ANY(t.path)) AND COALESCE(g.d_e_l_e_t_, '') NOT IN ('*', '  *') " &
                                ") " &
                                "SELECT level, pai, componente, quantidade, trt, descricao, tipo, um, path_str " &
                                "FROM bom_tree " &
                                "ORDER BY path_str;"

            Using conn As New NpgsqlConnection(connStringProtheus)
                conn.Open()
                Using cmd As New NpgsqlCommand(sql, conn)
                    cmd.Parameters.AddWithValue("@Cod", codigo)
                    Using da As New NpgsqlDataAdapter(cmd)
                        Dim dt As New DataTable()
                        da.Fill(dt)

                        ' 1. Popula DataGridView da Árvore
                        dgvArvoreCompleta.Rows.Clear()
                        For Each r As DataRow In dt.Rows
                            Dim lvl = Convert.ToInt32(r("level"))
                            Dim pai = r("pai").ToString().Trim()
                            Dim comp = r("componente").ToString().Trim()
                            Dim desc = If(IsDBNull(r("descricao")), "", r("descricao").ToString().Trim())
                            Dim tipo = If(IsDBNull(r("tipo")), "", r("tipo").ToString().Trim())
                            Dim um = If(IsDBNull(r("um")), "", r("um").ToString().Trim())
                            Dim qtd = If(IsDBNull(r("quantidade")), 1.0, Convert.ToDouble(r("quantidade")))
                            Dim pathStr = r("path_str").ToString().Trim()

                            dgvArvoreCompleta.Rows.Add(lvl, pai, comp, desc, tipo, um, qtd, pathStr)
                        Next

                        ' 2. Popula TreeView Hierárquico
                        tvwArvoreBOM.BeginUpdate()
                        tvwArvoreBOM.Nodes.Clear()

                        Dim rootNode As New TreeNode($"📦 [{txtTipo.Text}] {codigo} - {txtTituloProd.Text}") With {
                            .Tag = codigo
                        }
                        rootNode.NodeFont = New Font("Segoe UI", 9.0!, FontStyle.Bold)
                        tvwArvoreBOM.Nodes.Add(rootNode)

                        ' Indexador de nós para montagem em árvore rápida
                        Dim nodeMap As New Dictionary(Of String, TreeNode)(StringComparer.OrdinalIgnoreCase)
                        nodeMap(codigo) = rootNode

                        For Each r As DataRow In dt.Rows
                            Dim pai = r("pai").ToString().Trim()
                            Dim comp = r("componente").ToString().Trim()
                            Dim desc = If(IsDBNull(r("descricao")), "", r("descricao").ToString().Trim())
                            Dim tipo = If(IsDBNull(r("tipo")), "", r("tipo").ToString().Trim())
                            Dim um = If(IsDBNull(r("um")), "", r("um").ToString().Trim())
                            Dim qtd = If(IsDBNull(r("quantidade")), 1.0, Convert.ToDouble(r("quantidade")))

                            Dim textoNo As String = $"[{tipo}] {comp} - {desc} ({qtd:N4} {um})"
                            Dim childNode As New TreeNode(textoNo) With {.Tag = comp}

                            If nodeMap.ContainsKey(pai) Then
                                nodeMap(pai).Nodes.Add(childNode)
                            Else
                                rootNode.Nodes.Add(childNode)
                            End If

                            nodeMap(comp) = childNode
                        Next

                        rootNode.Expand()
                        For Each n As TreeNode In rootNode.Nodes
                            n.Expand()
                        Next
                        tvwArvoreBOM.EndUpdate()

                        If lblTotalNosArvore IsNot Nothing Then
                            lblTotalNosArvore.Text = $"{dt.Rows.Count + 1} nós no total"
                        End If
                        If btnTabArvore IsNot Nothing Then
                            btnTabArvore.Text = $"🌲  Estrutura em Árvore Completa   {dt.Rows.Count + 1} "
                        End If
                        lblStatus.Text = $"Árvore completa de {codigo} explodida com {dt.Rows.Count + 1} nós."
                    End Using
                End Using
            End Using

        Catch ex As Exception
            MessageBox.Show("Erro ao explodir árvore: " & ex.Message, "Erro Árvore", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    ' =========================================================================
    ' EVENTOS E AÇÕES DOS BOTÕES
    ' =========================================================================
    Private Sub btnEditarCabecalho_Click(sender As Object, e As EventArgs)
        txtTituloProd.ReadOnly = Not txtTituloProd.ReadOnly
        txtDensidade.ReadOnly = Not txtDensidade.ReadOnly
        txtComentarios.ReadOnly = Not txtComentarios.ReadOnly
        MessageBox.Show("Modo de edição do cabeçalho alternado.", "Edição", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    Private Sub btnEditarMateriais_Click(sender As Object, e As EventArgs)
        dgvMateriais.ReadOnly = Not dgvMateriais.ReadOnly
        MessageBox.Show(If(dgvMateriais.ReadOnly, "Grid de Materiais bloqueado para edição.", "Grid de Materiais liberado para edição direta."), "Materiais", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    Private Sub btnInserirMateriais_Click(sender As Object, e As EventArgs)
        Dim rawComp = InputBox("Informe o código do novo componente a incluir na estrutura (ex: MTA-1826):", "Inserir Componente (SG1)", "")
        If String.IsNullOrWhiteSpace(rawComp) Then Exit Sub
        Dim novoComp = ObterCodigoProtheusLimpo(rawComp)
        If String.IsNullOrEmpty(novoComp) Then Exit Sub

        ' 1. Validação de Referência Circular
        If novoComp.Equals(CodigoProduto, StringComparison.OrdinalIgnoreCase) Then
            MessageBox.Show("Um produto não pode conter a si próprio como componente na estrutura (referência circular proibida).", "Validação de Estrutura", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        ' 2. Validação de Duplicidade no Grid
        For Each row As DataGridViewRow In dgvMateriais.Rows
            If Not row.IsNewRow AndAlso row.Cells("Codigo").Value IsNot Nothing Then
                If row.Cells("Codigo").Value.ToString().Trim().ToUpper().Equals(novoComp, StringComparison.OrdinalIgnoreCase) Then
                    MessageBox.Show($"O componente '{novoComp}' já consta na estrutura.", "Componente Duplicado", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    Exit Sub
                End If
            End If
        Next

        ' 3. Validação e Consulta no Banco Protheus (SB1010)
        Dim descComp As String = "COMPONENTE MANUAL"
        Dim umComp As String = "PC"
        Dim compCadastrado As Boolean = False

        Try
            Using conn As New NpgsqlConnection(connStringProtheus)
                conn.Open()
                Dim sql As String = "SELECT b1_desc, b1_um FROM public.sb1010 WHERE (TRIM(b1_cod) = @Cod OR TRIM(b1_cod) = @CodPad) AND COALESCE(d_e_l_e_t_, '') NOT IN ('*', '  *') LIMIT 1;"
                Using cmd As New NpgsqlCommand(sql, conn)
                    cmd.Parameters.AddWithValue("@Cod", novoComp)
                    cmd.Parameters.AddWithValue("@CodPad", novoComp.PadRight(15, " "c))
                    Using rdr = cmd.ExecuteReader()
                        If rdr.Read() Then
                            compCadastrado = True
                            descComp = If(IsDBNull(rdr("b1_desc")), "", rdr("b1_desc").ToString().Trim())
                            umComp = If(IsDBNull(rdr("b1_um")), "PC", rdr("b1_um").ToString().Trim())
                        End If
                    End Using
                End Using
            End Using
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine("Erro ao consultar componente no Protheus: " & ex.Message)
        End Try

        If Not compCadastrado Then
            Dim conf = MessageBox.Show($"O componente '{novoComp}' não foi localizado no cadastro de produtos (SB1010) do Protheus." & vbCrLf & vbCrLf &
                                       "Deseja adicioná-lo à estrutura mesmo assim como item pendente de cadastro?",
                                       "Componente Não Encontrado", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
            If conf <> DialogResult.Yes Then Exit Sub
        End If

        ' 4. Validação de Quantidade
        Dim qtdStr = InputBox($"Informe a quantidade líquida utilizada de '{novoComp}' ({umComp}):", "Quantidade do Componente", "1.0000")
        If String.IsNullOrWhiteSpace(qtdStr) Then Exit Sub
        Dim qtd As Double = ConverterParaDecimalSeguro(qtdStr)
        If qtd <= 0 Then
            MessageBox.Show("A quantidade do componente deve ser um valor numérico positivo maior que zero.", "Quantidade Inválida", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        dgvMateriais.Rows.Add("02", novoComp, descComp, qtd, umComp)
        lblBadgeQtdMateriais.Text = dgvMateriais.Rows.Count.ToString()
        lblStatus.Text = $"Componente {novoComp} ({qtd:N4} {umComp}) adicionado à estrutura."
    End Sub

    Private Sub btnEditarCliente_Click(sender As Object, e As EventArgs)
        dgvCliente.ReadOnly = Not dgvCliente.ReadOnly
        MessageBox.Show(If(dgvCliente.ReadOnly, "Grid de Clientes bloqueado.", "Grid de Clientes liberado para edição."), "Clientes", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    Private Sub btnInserirCliente_Click(sender As Object, e As EventArgs)
        Dim rawCli = InputBox("Informe o Código do Cliente no Protheus (ex: 00000157):", "Novo Cliente (SZ1)", "00000157")
        If String.IsNullOrWhiteSpace(rawCli) Then Exit Sub
        Dim cli = rawCli.Trim().ToUpper()
        If IsNumeric(cli) AndAlso cli.Length < 8 Then
            cli = cli.PadLeft(8, "0"c)
        End If

        Dim codcl = InputBox("Informe o Código da Peça no Cliente (ex: S/CODIGO):", "Código do Cliente", "S/CODIGO")
        If String.IsNullOrWhiteSpace(codcl) Then codcl = "S/CODIGO"
        codcl = codcl.Trim().ToUpper()

        Dim rev = InputBox("Informe a Revisão do Cliente (obrigatória, ex: 02):", "Revisão", If(Not String.IsNullOrEmpty(txtRevCliente.Text), txtRevCliente.Text, "02"))
        If String.IsNullOrWhiteSpace(rev) Then rev = "01"
        rev = rev.Trim().ToUpper()

        ' 1. Validação de Duplicidade
        For Each row As DataGridViewRow In dgvCliente.Rows
            If Not row.IsNewRow AndAlso row.Cells("Cliente").Value IsNot Nothing Then
                If row.Cells("Cliente").Value.ToString().Trim().ToUpper().Equals(cli, StringComparison.OrdinalIgnoreCase) Then
                    MessageBox.Show($"O cliente '{cli}' já está vinculado a esta peça.", "Vínculo Duplicado", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    Exit Sub
                End If
            End If
        Next

        ' 2. Consulta no Cadastro de Clientes (SA1010)
        Dim nomeCli As String = "CLIENTE VINCULADO"
        Dim clienteExiste As Boolean = False
        Try
            Using conn As New NpgsqlConnection(connStringProtheus)
                conn.Open()
                Dim sql As String = "SELECT a1_nome FROM public.sa1010 WHERE (TRIM(a1_cod) = @Cli OR TRIM(a1_cod) = @CliPad) AND COALESCE(d_e_l_e_t_, '') = ' ' LIMIT 1;"
                Using cmd As New NpgsqlCommand(sql, conn)
                    cmd.Parameters.AddWithValue("@Cli", cli)
                    cmd.Parameters.AddWithValue("@CliPad", cli.PadRight(8, " "c))
                    Dim res = cmd.ExecuteScalar()
                    If res IsNot Nothing AndAlso Not IsDBNull(res) Then
                        clienteExiste = True
                        nomeCli = res.ToString().Trim()
                    End If
                End Using
            End Using
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine("Erro ao consultar cliente no SA1010: " & ex.Message)
        End Try

        If Not clienteExiste Then
            Dim conf = MessageBox.Show($"O cliente '{cli}' não foi encontrado na tabela de clientes (SA1010) do Protheus." & vbCrLf & vbCrLf &
                                       "Deseja vincular este código mesmo assim?", "Cliente Não Localizado", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
            If conf <> DialogResult.Yes Then Exit Sub
        End If

        dgvCliente.Rows.Add(cli, nomeCli, codcl, rev)
        lblBadgeQtdCliente.Text = dgvCliente.Rows.Count.ToString()
        lblStatus.Text = $"Vínculo com cliente {cli} ({nomeCli}) adicionado."
    End Sub

    Private Sub btnEditarRoteiro_Click(sender As Object, e As EventArgs)
        dgvRoteiro.ReadOnly = Not dgvRoteiro.ReadOnly
        MessageBox.Show(If(dgvRoteiro.ReadOnly, "Grid de Roteiro bloqueado.", "Grid de Roteiro liberado para edição."), "Roteiro", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    Private Sub btnInserirRoteiro_Click(sender As Object, e As EventArgs)
        ' Calcula próxima operação automática (10, 20, 30...)
        Dim proxOp As Integer = 10
        For Each row As DataGridViewRow In dgvRoteiro.Rows
            If Not row.IsNewRow AndAlso row.Cells("Operacao").Value IsNot Nothing Then
                Dim valOp As Integer
                If Integer.TryParse(row.Cells("Operacao").Value.ToString().Trim(), valOp) Then
                    If valOp >= proxOp Then proxOp = valOp + 10
                End If
            End If
        Next

        Dim rawOp = InputBox("Sequência da Operação Produtiva (ex: 10, 20, 30):", "Nova Operação (SG2)", proxOp.ToString("D2"))
        If String.IsNullOrWhiteSpace(rawOp) Then Exit Sub
        Dim op = rawOp.Trim()

        Dim rawRec = InputBox("Código do Recurso / Centro de Trabalho (SH1, ex: MO0011, CT0001):", "Recurso / Centro de Trabalho", "MO0011")
        If String.IsNullOrWhiteSpace(rawRec) Then Exit Sub
        Dim rec = rawRec.Trim().ToUpper()

        Dim desc As String = ""
        Dim recursoExiste As Boolean = False
        Try
            Using conn As New NpgsqlConnection(connStringProtheus)
                conn.Open()
                Dim sql As String = "SELECT h1_descri FROM public.sh1010 WHERE TRIM(h1_codigo) = @Rec AND COALESCE(h1_d_e_l_e_t_, d_e_l_e_t_, '') IN ('', ' ') LIMIT 1;"
                Using cmd As New NpgsqlCommand(sql, conn)
                    cmd.Parameters.AddWithValue("@Rec", rec)
                    Dim res = cmd.ExecuteScalar()
                    If res IsNot Nothing AndAlso Not IsDBNull(res) Then
                        recursoExiste = True
                        desc = res.ToString().Trim()
                    End If
                End Using
            End Using
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine("Erro ao consultar recurso no SH1010: " & ex.Message)
        End Try

        Dim rawDesc = InputBox("Descrição da Operação:", "Descrição", If(Not String.IsNullOrEmpty(desc), desc, "PROCESSO PRODUTIVO"))
        If Not String.IsNullOrWhiteSpace(rawDesc) Then desc = rawDesc.Trim()

        Dim rawSetup = InputBox("Tempo de Setup / Preparação em minutos (ex: 0.00):", "Tempo de Setup", "0.00")
        Dim setupMin As Double = ConverterParaDecimalSeguro(rawSetup)
        If setupMin < 0 Then setupMin = 0.0

        Dim rawCiclo = InputBox("Tempo Padrão / Ciclo da Peça em minutos (ex: 1.000):", "Tempo Padrão (Ciclo)", "1.000")
        Dim cicloMin As Double = ConverterParaDecimalSeguro(rawCiclo)
        If cicloMin <= 0 Then cicloMin = 1.0

        dgvRoteiro.Rows.Add(op, desc, rec, $"{setupMin:N2} min", $"{cicloMin:N3} min")
        lblBadgeQtdRoteiro.Text = dgvRoteiro.Rows.Count.ToString()
        lblStatus.Text = $"Operação {op} ({desc} - {rec}) adicionada ao roteiro."
    End Sub

    Private Sub btnNovaRestricao_Click(sender As Object, e As EventArgs)
        Dim resumo = InputBox("Título / Resumo da Restrição Técnica (máx 40 caracteres):", "Nova Restrição de Engenharia (SZ0)", "")
        If String.IsNullOrWhiteSpace(resumo) Then Exit Sub
        resumo = resumo.Trim()
        If resumo.Length > 40 Then resumo = resumo.Substring(0, 40)

        Dim desc = InputBox("Descrição Técnica Detalhada:", "Detalhes da Restrição", "")
        If desc Is Nothing Then desc = ""
        desc = desc.Trim()

        ' Calcula próximo número sequencial (001, 002, 003...)
        Dim proxItem As Integer = 1
        For Each row As DataGridViewRow In dgvRestricoes.Rows
            If Not row.IsNewRow AndAlso row.Cells("Item").Value IsNot Nothing Then
                Dim valIt As Integer
                If Integer.TryParse(row.Cells("Item").Value.ToString().Trim(), valIt) Then
                    If valIt >= proxItem Then proxItem = valIt + 1
                End If
            End If
        Next

        Dim itemSeq As String = proxItem.ToString("D3")
        Dim revProd As String = If(Not String.IsNullOrEmpty(txtRev.Text), txtRev.Text.Trim(), "00")

        dgvRestricoes.Rows.Add(itemSeq, "P", resumo, desc, revProd, "ENGENHARIA", DateTime.Now.ToString("yyyyMMdd HH:mm"))
        lblBadgeQtdRestricoes.Text = dgvRestricoes.Rows.Count.ToString()
        lblSemRestricoes.Visible = False
        dgvRestricoes.Visible = True
        lblStatus.Text = $"Restrição {itemSeq} ({resumo}) registrada."
    End Sub

    Private Sub btnGerarPDF_Click(sender As Object, e As EventArgs)
        MessageBox.Show($"Gerando relatório técnico e ficha de produto para {CodigoProduto}...", "PDF SINCO", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    Private Sub btnExportarExcelArvore_Click(sender As Object, e As EventArgs)
        Try
            Dim sfd As New SaveFileDialog() With {
                .Filter = "Arquivo CSV (*.csv)|*.csv",
                .FileName = $"Estrutura_{CodigoProduto}_{DateTime.Now:yyyyMMdd}.csv"
            }
            If sfd.ShowDialog() = DialogResult.OK Then
                Dim sb As New StringBuilder()
                sb.AppendLine("Nível;Pai;Componente;Descrição;Tipo;UM;Quantidade;Caminho")
                For Each r As DataGridViewRow In dgvArvoreCompleta.Rows
                    If r.IsNewRow Then Continue For
                    sb.AppendLine($"{r.Cells("Level").Value};{r.Cells("Pai").Value};{r.Cells("Componente").Value};""{r.Cells("Descricao").Value}"";{r.Cells("Tipo").Value};{r.Cells("UM").Value};{r.Cells("Quantidade").Value};""{r.Cells("Caminho").Value}""")
                Next
                File.WriteAllText(sfd.FileName, sb.ToString(), Encoding.UTF8)
                MessageBox.Show("Estrutura exportada com sucesso!", "Exportação Concluída", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        Catch ex As Exception
            MessageBox.Show("Erro ao exportar: " & ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' =========================================================================
    ' GRAVAÇÃO INTEGRADA NO PROTHEUS VIA API REST TLPP
    ' =========================================================================
    Private Sub btnSalvarProtheus_Click(sender As Object, e As EventArgs)
        Dim conf As DialogResult = MessageBox.Show(
            $"Deseja sincronizar e gravar a engenharia completa de {CodigoProduto} no TOTVS Protheus?" & vbCrLf & vbCrLf &
            $"• Árvore de Materiais (SG1010): {dgvMateriais.Rows.Count} itens" & vbCrLf &
            $"• Amarração Cliente (SZ1010): {dgvCliente.Rows.Count} vínculos" & vbCrLf &
            $"• Roteiro Produtivo (SG2010): {dgvRoteiro.Rows.Count} operações" & vbCrLf &
            $"• Restrições (SZ0010): {dgvRestricoes.Rows.Count} itens",
            "Confirmar Gravação Protheus", MessageBoxButtons.YesNo, MessageBoxIcon.Question)

        If conf <> DialogResult.Yes Then Exit Sub

        progressBar.Visible = True
        progressBar.Value = 10
        lblStatus.Text = "Autenticando no Protheus TLPP..."
        Me.Cursor = Cursors.WaitCursor
        Application.DoEvents()

        Try
            ' 1. Configurar SSL e Protocolo TLS
            ServicePointManager.SecurityProtocol = DirectCast(3072, SecurityProtocolType) Or SecurityProtocolType.Tls11 Or SecurityProtocolType.Tls
            ServicePointManager.ServerCertificateValidationCallback = Function(s, cert, chain, sslPolicyErrors) True

            ' 2. Obter Token OAuth2
            Dim tokenUrl As String = $"{apiProtheusBase}/tlpp/oauth2/token?grant_type=password&username=sinco&password=Metal1120"
            Dim tokenReq As HttpWebRequest = CType(WebRequest.Create(tokenUrl), HttpWebRequest)
            tokenReq.Method = "GET"
            tokenReq.Timeout = 25000

            Dim accessToken As String = ""
            Using tokenResp As HttpWebResponse = CType(tokenReq.GetResponse(), HttpWebResponse)
                Using rdr As New StreamReader(tokenResp.GetResponseStream())
                    Dim respText As String = rdr.ReadToEnd()
                    Dim m = Regex.Match(respText, """access_token""\s*:\s*""([^""]+)""")
                    If m.Success Then accessToken = m.Groups(1).Value
                End Using
            End Using

            If String.IsNullOrEmpty(accessToken) Then
                Throw New Exception("Falha ao obter token OAuth2 do Protheus.")
            End If

            progressBar.Value = 35
            lblStatus.Text = "Enviando Estrutura de Materiais (SG1010)..."
            Application.DoEvents()

            ' 3. Gravar Estrutura BOM (SG1010)
            SalvarEstruturaBOM(accessToken)
            progressBar.Value = 70

            ' 4. Gravar Produto x Cliente (SZ1010)
            SalvarProdutoCliente(accessToken)
            progressBar.Value = 85

            ' 5. Gravar Roteiro (SG2010)
            SalvarRoteiroProdutivo(accessToken)
            progressBar.Value = 100

            lblStatus.Text = "Engenharia sincronizada com sucesso no Protheus!"
            MessageBox.Show($"✅ Produto {CodigoProduto} e módulos de engenharia salvos com sucesso no Protheus!", "Protheus Sincronizado", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception
            MessageBox.Show("Erro ao enviar dados para a API do Protheus: " & ex.Message, "Erro Protheus", MessageBoxButtons.OK, MessageBoxIcon.Error)
            lblStatus.Text = "Erro na gravação Protheus."
        Finally
            Me.Cursor = Cursors.Default
            progressBar.Visible = False
            progressBar.Value = 0
        End Try
    End Sub

    Private Sub SalvarEstruturaBOM(token As String)
        Dim arrEstrutura As New JArray()
        For Each r As DataGridViewRow In dgvMateriais.Rows
            If r.IsNewRow Then Continue For
            Dim comp = If(r.Cells("Codigo").Value, "").ToString().Trim().ToUpper()
            If String.IsNullOrEmpty(comp) Then Continue For

            Dim qtd As Double = ConverterParaDecimalSeguro(If(r.Cells("Qtd").Value, "1").ToString())
            If qtd <= 0 Then qtd = 1.0

            Dim item As New JObject()
            item("G1_COD") = CodigoProduto
            item("G1_COMP") = comp
            item("G1_TRT") = "   "
            item("G1_QUANT") = qtd
            item("G1_PERDA") = 0
            item("G1_INI") = "19000101"
            item("G1_FIM") = "20491231"
            item("OPCAO") = "ALTERAR"
            arrEstrutura.Add(item)
        Next

        Dim root As New JObject()
        Dim dataObj As New JObject()
        dataObj("NOPER") = 4
        dataObj("EMPRESA") = "01"
        dataObj("CFILANT") = "0101"
        dataObj("G1_COD") = CodigoProduto
        dataObj("G1_QUANT") = 1.0
        dataObj("AUTOREV") = False
        dataObj("estrutura") = arrEstrutura
        root("data") = dataObj

        EnviarPostApi($"{apiProtheusBase}/estrutura-produtos/editar", root.ToString(Formatting.None), token)
    End Sub

    Private Sub SalvarProdutoCliente(token As String)
        If dgvCliente.Rows.Count = 0 Then Exit Sub

        Dim arr As New JArray()
        For Each r As DataGridViewRow In dgvCliente.Rows
            If r.IsNewRow Then Continue For
            Dim cli = If(r.Cells("Cliente").Value, "").ToString().Trim()
            Dim codcl = If(r.Cells("CodCl").Value, "").ToString().Trim()
            Dim rev = If(r.Cells("Rev").Value, "02").ToString().Trim()
            If String.IsNullOrEmpty(cli) Then Continue For

            Dim item As New JObject()
            item("Z1_CLIENTE") = cli.PadLeft(8, "0"c)
            item("Z1_LOJA") = "0001"
            item("Z1_PRODUTO") = CodigoProduto
            item("Z1_CODCLI") = codcl
            item("Z1_DESCCLI") = txtTituloProd.Text.Trim()
            item("Z1_REVISAO") = rev
            arr.Add(item)
        Next

        Dim root As New JObject()
        Dim dataObj As New JObject()
        dataObj("EMPRESA") = "01"
        dataObj("CFILANT") = "0101"
        dataObj("NOPER") = 4
        dataObj("produtos_clientes") = arr
        root("data") = dataObj

        Try
            EnviarPostApi($"{apiProtheusBase}/produto-cliente/editar", root.ToString(Formatting.None), token)
        Catch
            ' Fallback para inclusão se não existir
            dataObj("NOPER") = 3
            EnviarPostApi($"{apiProtheusBase}/produto-cliente/editar", root.ToString(Formatting.None), token)
        End Try
    End Sub

    Private Sub SalvarRoteiroProdutivo(token As String)
        If dgvRoteiro.Rows.Count = 0 Then Exit Sub

        Dim arr As New JArray()
        For Each r As DataGridViewRow In dgvRoteiro.Rows
            If r.IsNewRow Then Continue For
            Dim op = If(r.Cells("Operacao").Value, "10").ToString().Trim()
            Dim desc = If(r.Cells("Descricao").Value, "").ToString().Trim()
            Dim rec = If(r.Cells("Recurso").Value, "").ToString().Trim()

            Dim setupMin As Double = ConverterParaDecimalSeguro(If(r.Cells("Setup").Value, "0").ToString())
            Dim cicloMin As Double = ConverterParaDecimalSeguro(If(r.Cells("TempoPadrao").Value, "1").ToString())
            If cicloMin <= 0 Then cicloMin = 1.0

            Dim item As New JObject()
            item("G2_OPERAC") = op
            item("G2_DESCRI") = desc
            item("G2_RECURSO") = rec
            item("G2_TEMPAD") = cicloMin
            item("G2_SETUP") = setupMin
            item("G2_LOTEPAD") = 1
            item("G2_MAOOBRA") = 1
            item("G2_TPOPER") = "1"
            item("OPERACAO") = "ALTERAR"
            arr.Add(item)
        Next

        Dim root As New JObject()
        Dim dataObj As New JObject()
        dataObj("NOPER") = 4
        dataObj("EMPRESA") = "01"
        dataObj("CFILANT") = "0101"
        dataObj("G2_CODIGO") = "01"
        dataObj("G2_PRODUTO") = CodigoProduto
        dataObj("roteiros") = arr
        root("data") = dataObj

        EnviarPostApi($"{apiProtheusBase}/roteiro-produtivo/editar", root.ToString(Formatting.None), token)
    End Sub

    Private Function EnviarPostApi(url As String, jsonBody As String, token As String) As String
        Dim postReq As HttpWebRequest = CType(WebRequest.Create(url), HttpWebRequest)
        postReq.Method = "POST"
        postReq.ContentType = "application/json; charset=utf-8"
        postReq.Headers.Add("Authorization", "Bearer " & token)
        postReq.Timeout = 40000

        Dim bodyBytes As Byte() = Encoding.UTF8.GetBytes(jsonBody)
        postReq.ContentLength = bodyBytes.Length
        Using s = postReq.GetRequestStream()
            s.Write(bodyBytes, 0, bodyBytes.Length)
        End Using

        Try
            Using postResp As HttpWebResponse = CType(postReq.GetResponse(), HttpWebResponse)
                Using rdr As New StreamReader(postResp.GetResponseStream())
                    Return rdr.ReadToEnd()
                End Using
            End Using
        Catch webEx As WebException
            Dim detalhe As String = webEx.Message
            If webEx.Response IsNot Nothing Then
                Try
                    Using rdrErr As New StreamReader(webEx.Response.GetResponseStream())
                        detalhe = rdrErr.ReadToEnd()
                    End Using
                Catch
                End Try
            End If
            Throw New Exception($"Erro na API Protheus ({url}): {detalhe}", webEx)
        End Try
    End Function

End Class
