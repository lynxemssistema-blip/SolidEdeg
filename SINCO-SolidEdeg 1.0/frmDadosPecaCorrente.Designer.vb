<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmDadosPecaCorrente
    Inherits System.Windows.Forms.Form

    'Descartar substituições de formulário para limpar a lista de componentes.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Exigido pelo Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'OBSERVAÇÃO: o procedimento a seguir é exigido pelo Windows Form Designer
    'Pode ser modificado usando o Windows Form Designer.  
    'Não o modifique usando o editor de códigos.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmDadosPecaCorrente))
        Me.btnListaConjunto = New System.Windows.Forms.Button()
        Me.btnCriarEstruturaProduto = New System.Windows.Forms.Button()
        Me.dgvDadosPecas = New System.Windows.Forms.DataGridView()
        Me.dgvDXF = New System.Windows.Forms.DataGridViewImageColumn()
        Me.dgvpdf = New System.Windows.Forms.DataGridViewImageColumn()
        Me.mnudgvDadosPecas = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.AbrirArquivoToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator()
        Me.AbrirDetalhamentoToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.AbrirPdfToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.MarcarComoNovoRevisaoToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.chkdxf = New System.Windows.Forms.CheckBox()
        Me.chkPdf = New System.Windows.Forms.CheckBox()
        Me.ProgressBar1 = New System.Windows.Forms.ProgressBar()
        Me.btnLimpar = New System.Windows.Forms.Button()
        Me.TabControlPrincipal = New System.Windows.Forms.TabControl()
        Me.TabPage1 = New System.Windows.Forms.TabPage()
        Me.btnSalvarCadProtheus = New System.Windows.Forms.Button()
        Me.txtB1_XREVM = New System.Windows.Forms.TextBox()
        Me.Label50 = New System.Windows.Forms.Label()
        Me.cboB1_GRUPO = New System.Windows.Forms.ComboBox()
        Me.Label49 = New System.Windows.Forms.Label()
        Me.cboB1_UM = New System.Windows.Forms.ComboBox()
        Me.Label48 = New System.Windows.Forms.Label()
        Me.cboB1_TIPO = New System.Windows.Forms.ComboBox()
        Me.Label47 = New System.Windows.Forms.Label()
        Me.btnPdfLote = New System.Windows.Forms.Button()
        Me.btnEstrutraMaterialProtheus = New System.Windows.Forms.Button()
        Me.btnBuscarOperacaoProtheus = New System.Windows.Forms.Button()
        Me.TcpAcessorios = New System.Windows.Forms.TabControl()
        Me.tpgGabaritos = New System.Windows.Forms.TabPage()
        Me.dgvGabaritos = New System.Windows.Forms.DataGridView()
        Me.tpgCliente = New System.Windows.Forms.TabPage()
        Me.dgvDesenhoCliente = New System.Windows.Forms.DataGridView()
        Me.mnudgvDesenhoCliente = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.BuscarDesenhoDeReferenciaToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.BuscarArquivoPDFDeReferenciaToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.AbrirArquivoPDFToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator4 = New System.Windows.Forms.ToolStripSeparator()
        Me.BuscarArquivoDoClienteDiretemanteNoDiretorioSemPassarPeloProtheusToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.btnProjetoProtheus = New System.Windows.Forms.Button()
        Me.gpbMateriaisProtheus = New System.Windows.Forms.GroupBox()
        Me.chkGabarito = New System.Windows.Forms.CheckBox()
        Me.TxtPesqRP = New System.Windows.Forms.TextBox()
        Me.Label40 = New System.Windows.Forms.Label()
        Me.dgvMateriaisProtheus = New System.Windows.Forms.DataGridView()
        Me.mnudgvMateriaisProtheus = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.AssociarFichaTecnicaToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.AssociarGabaritoAPeçaCorrenteToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.BuscarMaterialNoProtheusToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.txtPesqDescricao3 = New System.Windows.Forms.TextBox()
        Me.txtPesqDescricao2 = New System.Windows.Forms.TextBox()
        Me.txtPesqDescricao1 = New System.Windows.Forms.TextBox()
        Me.Label34 = New System.Windows.Forms.Label()
        Me.txtPesqCodigo = New System.Windows.Forms.TextBox()
        Me.Label33 = New System.Windows.Forms.Label()
        Me.gpbMateriaiDesenho = New System.Windows.Forms.GroupBox()
        Me.dgvMaterialPeca = New System.Windows.Forms.DataGridView()
        Me.mnudgvMaterialPeca = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.AlterarQtdeToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.btnMsg = New System.Windows.Forms.Button()
        Me.Button5 = New System.Windows.Forms.Button()
        Me.cboAcabamento = New System.Windows.Forms.ComboBox()
        Me.cboTipoDesenho = New System.Windows.Forms.ComboBox()
        Me.btnConverterChapa = New System.Windows.Forms.Button()
        Me.cboBloqueado = New System.Windows.Forms.ComboBox()
        Me.Label39 = New System.Windows.Forms.Label()
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        Me.btnCriaPropriedadePadroes = New System.Windows.Forms.Button()
        Me.Button4 = New System.Windows.Forms.Button()
        Me.Button3 = New System.Windows.Forms.Button()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.Label32 = New System.Windows.Forms.Label()
        Me.Label31 = New System.Windows.Forms.Label()
        Me.dgvProcessoMaterial = New System.Windows.Forms.DataGridView()
        Me.dgvProcessos = New System.Windows.Forms.DataGridView()
        Me.txtPesqProcesso = New System.Windows.Forms.TextBox()
        Me.Label23 = New System.Windows.Forms.Label()
        Me.btnAtualizacoesDados = New System.Windows.Forms.Button()
        Me.btnConfiguracoes = New System.Windows.Forms.Button()
        Me.txtPesoKg = New System.Windows.Forms.TextBox()
        Me.Label22 = New System.Windows.Forms.Label()
        Me.btnSalvar = New System.Windows.Forms.Button()
        Me.txtEspessura = New System.Windows.Forms.TextBox()
        Me.Label19 = New System.Windows.Forms.Label()
        Me.txtMaterialSw = New System.Windows.Forms.TextBox()
        Me.txtAreametroquadr = New System.Windows.Forms.TextBox()
        Me.Label18 = New System.Windows.Forms.Label()
        Me.txtCutSizey = New System.Windows.Forms.TextBox()
        Me.Label17 = New System.Windows.Forms.Label()
        Me.txtCutSizex = New System.Windows.Forms.TextBox()
        Me.Label16 = New System.Windows.Forms.Label()
        Me.Label15 = New System.Windows.Forms.Label()
        Me.txtEmpresa = New System.Windows.Forms.TextBox()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.txtRevisao = New System.Windows.Forms.TextBox()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.txtPalavraChave = New System.Windows.Forms.TextBox()
        Me.txtData1 = New System.Windows.Forms.TextBox()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.txtDataR = New System.Windows.Forms.TextBox()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.txtData = New System.Windows.Forms.TextBox()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.txtendereco = New System.Windows.Forms.TextBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.txtGerente = New System.Windows.Forms.TextBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.txtCategoria = New System.Windows.Forms.TextBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.txtAutor = New System.Windows.Forms.TextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.txtNumeroDesenho = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.txtTitulo = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.btnAbriDetalhamentoCorrente = New System.Windows.Forms.Button()
        Me.btnGerarPdf = New System.Windows.Forms.Button()
        Me.btnDxf = New System.Windows.Forms.Button()
        Me.TabPage2 = New System.Windows.Forms.TabPage()
        Me.chkiges = New System.Windows.Forms.CheckBox()
        Me.lblResumo = New System.Windows.Forms.Label()
        Me.chkOpcaodePasta = New System.Windows.Forms.CheckBox()
        Me.Label21 = New System.Windows.Forms.Label()
        Me.txtFiltroEmpresa = New System.Windows.Forms.TextBox()
        Me.btnInserirnaOS = New System.Windows.Forms.Button()
        Me.lblOrdemServicoAtiva = New System.Windows.Forms.Label()
        Me.btnLimparFiltro = New System.Windows.Forms.Button()
        Me.lblProgresso = New System.Windows.Forms.Label()
        Me.btnFiltrar = New System.Windows.Forms.Button()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.txtFiltroCaminho1 = New System.Windows.Forms.TextBox()
        Me.BtnGeraArquivos = New System.Windows.Forms.Button()
        Me.TabPage5 = New System.Windows.Forms.TabPage()
        Me.Panel4 = New System.Windows.Forms.Panel()
        Me.TabControlOS = New System.Windows.Forms.TabControl()
        Me.TabPage6 = New System.Windows.Forms.TabPage()
        Me.DGVListaMaterialSW = New System.Windows.Forms.DataGridView()
        Me.dgvSelecao = New System.Windows.Forms.DataGridViewCheckBoxColumn()
        Me.dgvIconeItemOS = New System.Windows.Forms.DataGridViewImageColumn()
        Me.dgvDXF1 = New System.Windows.Forms.DataGridViewImageColumn()
        Me.dgvPDF1 = New System.Windows.Forms.DataGridViewImageColumn()
        Me.mnuDGVListaMaterialSW = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ToolStripSeparator3 = New System.Windows.Forms.ToolStripSeparator()
        Me.MarcarTodosToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.DesmarcarTodosToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.InverterSeleçãoToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator6 = New System.Windows.Forms.ToolStripSeparator()
        Me.AbrirPDFDaLinhaSelecionadaToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator15 = New System.Windows.Forms.ToolStripSeparator()
        Me.ExcluirODocumentoDaLinhaSelecionadaToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator7 = New System.Windows.Forms.ToolStripSeparator()
        Me.MarcarComoConjuntoPrincipalDaOrdemDeServiçoToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.DesmarcarComoConjuntoPrincipalToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator8 = New System.Windows.Forms.ToolStripSeparator()
        Me.AlterarAQuantidadeDePeçasFabricaçãoDaLinhaSelecionadaToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator9 = New System.Windows.Forms.ToolStripSeparator()
        Me.MarcarDesenhoComoRevisãoToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.DesmarcarDesenhoComoRevisãoToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.TabPage7 = New System.Windows.Forms.TabPage()
        Me.DGVListaMaterialSWMateriais = New System.Windows.Forms.DataGridView()
        Me.txtPesqNumeroDesenho = New System.Windows.Forms.TextBox()
        Me.Label27 = New System.Windows.Forms.Label()
        Me.cboOpcoesAcabamento = New System.Windows.Forms.ComboBox()
        Me.Label26 = New System.Windows.Forms.Label()
        Me.txtPesqAcabamentoDesenho = New System.Windows.Forms.TextBox()
        Me.btnAplicarAcabamento = New System.Windows.Forms.Button()
        Me.Label28 = New System.Windows.Forms.Label()
        Me.Label29 = New System.Windows.Forms.Label()
        Me.txtPesqTipoDesenho = New System.Windows.Forms.TextBox()
        Me.Panel3 = New System.Windows.Forms.Panel()
        Me.ProgressBarOSM = New System.Windows.Forms.ProgressBar()
        Me.Label44 = New System.Windows.Forms.Label()
        Me.txtPesoTotalKgTotal = New System.Windows.Forms.TextBox()
        Me.Label45 = New System.Windows.Forms.Label()
        Me.txtAreaPintM2Total = New System.Windows.Forms.TextBox()
        Me.Label46 = New System.Windows.Forms.Label()
        Me.Label43 = New System.Windows.Forms.Label()
        Me.txtPesoTotalKg = New System.Windows.Forms.TextBox()
        Me.Label42 = New System.Windows.Forms.Label()
        Me.txtAreaPintM2 = New System.Windows.Forms.TextBox()
        Me.Label41 = New System.Windows.Forms.Label()
        Me.lbltxtSaldoTag = New System.Windows.Forms.Label()
        Me.lbltxtQtdeLiberada = New System.Windows.Forms.Label()
        Me.lbltxtQtdeTag = New System.Windows.Forms.Label()
        Me.dgvos = New System.Windows.Forms.DataGridView()
        Me.dgvStatus = New System.Windows.Forms.DataGridViewImageColumn()
        Me.mnudgvos = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.AbrirPastaDaOrdemDeServiçoToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator10 = New System.Windows.Forms.ToolStripSeparator()
        Me.LiberarOrdemDeServiçoParaProduçãoToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.CancelarLiberaçãoDaOSToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator11 = New System.Windows.Forms.ToolStripSeparator()
        Me.AtualizarPDFsEDXFsNaPastaDaOSMToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator12 = New System.Windows.Forms.ToolStripSeparator()
        Me.AlterarOFatorMultipçlicadorDaOSToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator14 = New System.Windows.Forms.ToolStripSeparator()
        Me.GeralExcelDaOSToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator13 = New System.Windows.Forms.ToolStripSeparator()
        Me.LimparPastaOrdemDeServiçoSelecionadaToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator17 = New System.Windows.Forms.ToolStripSeparator()
        Me.CancelarAFabricaçãoDaOSToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.lblFator = New System.Windows.Forms.TextBox()
        Me.Label37 = New System.Windows.Forms.Label()
        Me.chkMostraLiberadasPelaEngenharia = New System.Windows.Forms.CheckBox()
        Me.Label38 = New System.Windows.Forms.Label()
        Me.Label36 = New System.Windows.Forms.Label()
        Me.Label30 = New System.Windows.Forms.Label()
        Me.txtPesqCriadoPor = New System.Windows.Forms.TextBox()
        Me.Label35 = New System.Windows.Forms.Label()
        Me.txtDescricao = New System.Windows.Forms.TextBox()
        Me.Label25 = New System.Windows.Forms.Label()
        Me.txtCliente = New System.Windows.Forms.TextBox()
        Me.Label20 = New System.Windows.Forms.Label()
        Me.cboProjeto = New System.Windows.Forms.ComboBox()
        Me.Label24 = New System.Windows.Forms.Label()
        Me.cboTag = New System.Windows.Forms.ComboBox()
        Me.txtDescricaoTag = New System.Windows.Forms.TextBox()
        Me.BindingNavigator1 = New System.Windows.Forms.BindingNavigator(Me.components)
        Me.ToolStripButton10 = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripButton1 = New System.Windows.Forms.ToolStripButton()
        Me.TSBSalvarOrdemServico = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripButton3 = New System.Windows.Forms.ToolStripButton()
        Me.ProgressBarProcessoLiberacaoOrdemServico = New System.Windows.Forms.ToolStripProgressBar()
        Me.ToolStripSeparator29 = New System.Windows.Forms.ToolStripSeparator()
        Me.ToolStripTextBox1 = New System.Windows.Forms.ToolStripTextBox()
        Me.TabPagePDF = New System.Windows.Forms.TabPage()
        Me.Button7 = New System.Windows.Forms.Button()
        Me.dgvDadosPdf = New System.Windows.Forms.DataGridView()
        Me.dgvDadosDesenhoCampoDados = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.dgvDadosSelecionado = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.RichTextBox1 = New System.Windows.Forms.RichTextBox()
        Me.Button6 = New System.Windows.Forms.Button()
        Me.TimerDGVListaMaterialSW = New System.Windows.Forms.Timer(Me.components)
        Me.Timerdgvos = New System.Windows.Forms.Timer(Me.components)
        Me.TimerdgvProcessoMaterial = New System.Windows.Forms.Timer(Me.components)
        Me.TimerdgvProcesso = New System.Windows.Forms.Timer(Me.components)
        Me.ToolTipAjuda = New System.Windows.Forms.ToolTip(Me.components)
        Me.TimerbtnMsg = New System.Windows.Forms.Timer(Me.components)
        Me.TimerdgvMateriaisProtheus = New System.Windows.Forms.Timer(Me.components)
        Me.TimerManufaturada = New System.Windows.Forms.Timer(Me.components)
        Me.TimerdgvGabaritos = New System.Windows.Forms.Timer(Me.components)
        Me.DataGridViewImageColumn1 = New System.Windows.Forms.DataGridViewImageColumn()
        Me.DataGridViewImageColumn2 = New System.Windows.Forms.DataGridViewImageColumn()
        Me.Button8 = New System.Windows.Forms.Button()
        CType(Me.dgvDadosPecas, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.mnudgvDadosPecas.SuspendLayout()
        Me.TabControlPrincipal.SuspendLayout()
        Me.TabPage1.SuspendLayout()
        Me.TcpAcessorios.SuspendLayout()
        Me.tpgGabaritos.SuspendLayout()
        CType(Me.dgvGabaritos, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tpgCliente.SuspendLayout()
        CType(Me.dgvDesenhoCliente, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.mnudgvDesenhoCliente.SuspendLayout()
        Me.gpbMateriaisProtheus.SuspendLayout()
        CType(Me.dgvMateriaisProtheus, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.mnudgvMateriaisProtheus.SuspendLayout()
        Me.gpbMateriaiDesenho.SuspendLayout()
        CType(Me.dgvMaterialPeca, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.mnudgvMaterialPeca.SuspendLayout()
        CType(Me.dgvProcessoMaterial, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dgvProcessos, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabPage2.SuspendLayout()
        Me.TabPage5.SuspendLayout()
        Me.Panel4.SuspendLayout()
        Me.TabControlOS.SuspendLayout()
        Me.TabPage6.SuspendLayout()
        CType(Me.DGVListaMaterialSW, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.mnuDGVListaMaterialSW.SuspendLayout()
        Me.TabPage7.SuspendLayout()
        CType(Me.DGVListaMaterialSWMateriais, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel3.SuspendLayout()
        CType(Me.dgvos, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.mnudgvos.SuspendLayout()
        CType(Me.BindingNavigator1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.BindingNavigator1.SuspendLayout()
        Me.TabPagePDF.SuspendLayout()
        CType(Me.dgvDadosPdf, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'btnListaConjunto
        '
        Me.btnListaConjunto.Location = New System.Drawing.Point(11, 10)
        Me.btnListaConjunto.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.btnListaConjunto.Name = "btnListaConjunto"
        Me.btnListaConjunto.Size = New System.Drawing.Size(147, 44)
        Me.btnListaConjunto.TabIndex = 5
        Me.btnListaConjunto.Text = "BOM"
        Me.ToolTipAjuda.SetToolTip(Me.btnListaConjunto, resources.GetString("btnListaConjunto.ToolTip"))
        Me.btnListaConjunto.UseVisualStyleBackColor = True
        '
        'btnCriarEstruturaProduto
        '
        Me.btnCriarEstruturaProduto.Location = New System.Drawing.Point(160, 10)
        Me.btnCriarEstruturaProduto.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.btnCriarEstruturaProduto.Name = "btnCriarEstruturaProduto"
        Me.btnCriarEstruturaProduto.Size = New System.Drawing.Size(147, 44)
        Me.btnCriarEstruturaProduto.TabIndex = 600
        Me.btnCriarEstruturaProduto.Text = "Criar Estrutura Padrão"
        Me.btnCriarEstruturaProduto.UseVisualStyleBackColor = True
        '
        'dgvDadosPecas
        '
        Me.dgvDadosPecas.AllowUserToAddRows = False
        Me.dgvDadosPecas.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dgvDadosPecas.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvDadosPecas.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.dgvDXF, Me.dgvpdf})
        Me.dgvDadosPecas.ContextMenuStrip = Me.mnudgvDadosPecas
        Me.dgvDadosPecas.Location = New System.Drawing.Point(5, 172)
        Me.dgvDadosPecas.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.dgvDadosPecas.Name = "dgvDadosPecas"
        Me.dgvDadosPecas.ReadOnly = True
        Me.dgvDadosPecas.RowHeadersWidth = 51
        Me.dgvDadosPecas.RowTemplate.Height = 24
        Me.dgvDadosPecas.Size = New System.Drawing.Size(1559, 508)
        Me.dgvDadosPecas.TabIndex = 6
        '
        'dgvDXF
        '
        Me.dgvDXF.FillWeight = 25.0!
        Me.dgvDXF.Frozen = True
        Me.dgvDXF.HeaderText = "dxf"
        Me.dgvDXF.Image = Global.SINCO_SolidEdeg_1._0.My.Resources.Resources.sem_icone
        Me.dgvDXF.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Zoom
        Me.dgvDXF.MinimumWidth = 6
        Me.dgvDXF.Name = "dgvDXF"
        Me.dgvDXF.ReadOnly = True
        Me.dgvDXF.Width = 25
        '
        'dgvpdf
        '
        Me.dgvpdf.FillWeight = 25.0!
        Me.dgvpdf.Frozen = True
        Me.dgvpdf.HeaderText = "pdf"
        Me.dgvpdf.Image = Global.SINCO_SolidEdeg_1._0.My.Resources.Resources.sem_icone
        Me.dgvpdf.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Zoom
        Me.dgvpdf.MinimumWidth = 6
        Me.dgvpdf.Name = "dgvpdf"
        Me.dgvpdf.ReadOnly = True
        Me.dgvpdf.Width = 25
        '
        'mnudgvDadosPecas
        '
        Me.mnudgvDadosPecas.ImageScalingSize = New System.Drawing.Size(20, 20)
        Me.mnudgvDadosPecas.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.AbrirArquivoToolStripMenuItem, Me.ToolStripSeparator2, Me.AbrirDetalhamentoToolStripMenuItem, Me.ToolStripSeparator1, Me.AbrirPdfToolStripMenuItem, Me.MarcarComoNovoRevisaoToolStripMenuItem})
        Me.mnudgvDadosPecas.Name = "mnudgvDadosPecas"
        Me.mnudgvDadosPecas.Size = New System.Drawing.Size(268, 120)
        '
        'AbrirArquivoToolStripMenuItem
        '
        Me.AbrirArquivoToolStripMenuItem.Image = Global.SINCO_SolidEdeg_1._0.My.Resources.Resources.psm
        Me.AbrirArquivoToolStripMenuItem.Name = "AbrirArquivoToolStripMenuItem"
        Me.AbrirArquivoToolStripMenuItem.Size = New System.Drawing.Size(267, 26)
        Me.AbrirArquivoToolStripMenuItem.Text = "Abrir Arquivo"
        '
        'ToolStripSeparator2
        '
        Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
        Me.ToolStripSeparator2.Size = New System.Drawing.Size(264, 6)
        '
        'AbrirDetalhamentoToolStripMenuItem
        '
        Me.AbrirDetalhamentoToolStripMenuItem.Image = Global.SINCO_SolidEdeg_1._0.My.Resources.Resources.par
        Me.AbrirDetalhamentoToolStripMenuItem.Name = "AbrirDetalhamentoToolStripMenuItem"
        Me.AbrirDetalhamentoToolStripMenuItem.Size = New System.Drawing.Size(267, 26)
        Me.AbrirDetalhamentoToolStripMenuItem.Text = "Abrir Detalhamento"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(264, 6)
        '
        'AbrirPdfToolStripMenuItem
        '
        Me.AbrirPdfToolStripMenuItem.Image = Global.SINCO_SolidEdeg_1._0.My.Resources.Resources.pdf
        Me.AbrirPdfToolStripMenuItem.Name = "AbrirPdfToolStripMenuItem"
        Me.AbrirPdfToolStripMenuItem.Size = New System.Drawing.Size(267, 26)
        Me.AbrirPdfToolStripMenuItem.Text = "Abrir Pdf"
        '
        'MarcarComoNovoRevisaoToolStripMenuItem
        '
        Me.MarcarComoNovoRevisaoToolStripMenuItem.Name = "MarcarComoNovoRevisaoToolStripMenuItem"
        Me.MarcarComoNovoRevisaoToolStripMenuItem.Size = New System.Drawing.Size(267, 26)
        Me.MarcarComoNovoRevisaoToolStripMenuItem.Text = "Marcar como Novo/Revisao"
        '
        'chkdxf
        '
        Me.chkdxf.AutoSize = True
        Me.chkdxf.Location = New System.Drawing.Point(782, 46)
        Me.chkdxf.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.chkdxf.Name = "chkdxf"
        Me.chkdxf.Size = New System.Drawing.Size(112, 20)
        Me.chkdxf.TabIndex = 7
        Me.chkdxf.Text = "Converte DXF"
        Me.chkdxf.UseVisualStyleBackColor = True
        '
        'chkPdf
        '
        Me.chkPdf.AutoSize = True
        Me.chkPdf.Location = New System.Drawing.Point(916, 46)
        Me.chkPdf.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.chkPdf.Name = "chkPdf"
        Me.chkPdf.Size = New System.Drawing.Size(117, 20)
        Me.chkPdf.TabIndex = 8
        Me.chkPdf.Text = "Converter PDF"
        Me.chkPdf.UseVisualStyleBackColor = True
        '
        'ProgressBar1
        '
        Me.ProgressBar1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ProgressBar1.ForeColor = System.Drawing.Color.Green
        Me.ProgressBar1.Location = New System.Drawing.Point(5, 686)
        Me.ProgressBar1.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.ProgressBar1.Name = "ProgressBar1"
        Me.ProgressBar1.Size = New System.Drawing.Size(1559, 50)
        Me.ProgressBar1.TabIndex = 10
        '
        'btnLimpar
        '
        Me.btnLimpar.Location = New System.Drawing.Point(458, 10)
        Me.btnLimpar.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.btnLimpar.Name = "btnLimpar"
        Me.btnLimpar.Size = New System.Drawing.Size(163, 44)
        Me.btnLimpar.TabIndex = 11
        Me.btnLimpar.Text = "Limpar Grid"
        Me.ToolTipAjuda.SetToolTip(Me.btnLimpar, "🧹 Limpa o grid para realizar uma nova leitura ou atualizar os dados exibidos.")
        Me.btnLimpar.UseVisualStyleBackColor = True
        '
        'TabControlPrincipal
        '
        Me.TabControlPrincipal.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TabControlPrincipal.Controls.Add(Me.TabPage1)
        Me.TabControlPrincipal.Controls.Add(Me.TabPage2)
        Me.TabControlPrincipal.Controls.Add(Me.TabPage5)
        Me.TabControlPrincipal.Controls.Add(Me.TabPagePDF)
        Me.TabControlPrincipal.Location = New System.Drawing.Point(3, 4)
        Me.TabControlPrincipal.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.TabControlPrincipal.Name = "TabControlPrincipal"
        Me.TabControlPrincipal.SelectedIndex = 0
        Me.TabControlPrincipal.Size = New System.Drawing.Size(1578, 778)
        Me.TabControlPrincipal.TabIndex = 13
        '
        'TabPage1
        '
        Me.TabPage1.Controls.Add(Me.Button8)
        Me.TabPage1.Controls.Add(Me.btnSalvarCadProtheus)
        Me.TabPage1.Controls.Add(Me.txtB1_XREVM)
        Me.TabPage1.Controls.Add(Me.Label50)
        Me.TabPage1.Controls.Add(Me.cboB1_GRUPO)
        Me.TabPage1.Controls.Add(Me.Label49)
        Me.TabPage1.Controls.Add(Me.cboB1_UM)
        Me.TabPage1.Controls.Add(Me.Label48)
        Me.TabPage1.Controls.Add(Me.cboB1_TIPO)
        Me.TabPage1.Controls.Add(Me.Label47)
        Me.TabPage1.Controls.Add(Me.btnPdfLote)
        Me.TabPage1.Controls.Add(Me.btnEstrutraMaterialProtheus)
        Me.TabPage1.Controls.Add(Me.btnBuscarOperacaoProtheus)
        Me.TabPage1.Controls.Add(Me.TcpAcessorios)
        Me.TabPage1.Controls.Add(Me.btnProjetoProtheus)
        Me.TabPage1.Controls.Add(Me.gpbMateriaisProtheus)
        Me.TabPage1.Controls.Add(Me.gpbMateriaiDesenho)
        Me.TabPage1.Controls.Add(Me.btnMsg)
        Me.TabPage1.Controls.Add(Me.Button5)
        Me.TabPage1.Controls.Add(Me.cboAcabamento)
        Me.TabPage1.Controls.Add(Me.cboTipoDesenho)
        Me.TabPage1.Controls.Add(Me.btnConverterChapa)
        Me.TabPage1.Controls.Add(Me.cboBloqueado)
        Me.TabPage1.Controls.Add(Me.Label39)
        Me.TabPage1.Controls.Add(Me.TextBox1)
        Me.TabPage1.Controls.Add(Me.btnCriaPropriedadePadroes)
        Me.TabPage1.Controls.Add(Me.Button4)
        Me.TabPage1.Controls.Add(Me.Button3)
        Me.TabPage1.Controls.Add(Me.Button2)
        Me.TabPage1.Controls.Add(Me.Button1)
        Me.TabPage1.Controls.Add(Me.Label32)
        Me.TabPage1.Controls.Add(Me.Label31)
        Me.TabPage1.Controls.Add(Me.dgvProcessoMaterial)
        Me.TabPage1.Controls.Add(Me.dgvProcessos)
        Me.TabPage1.Controls.Add(Me.txtPesqProcesso)
        Me.TabPage1.Controls.Add(Me.Label23)
        Me.TabPage1.Controls.Add(Me.btnAtualizacoesDados)
        Me.TabPage1.Controls.Add(Me.btnConfiguracoes)
        Me.TabPage1.Controls.Add(Me.txtPesoKg)
        Me.TabPage1.Controls.Add(Me.Label22)
        Me.TabPage1.Controls.Add(Me.btnSalvar)
        Me.TabPage1.Controls.Add(Me.txtEspessura)
        Me.TabPage1.Controls.Add(Me.Label19)
        Me.TabPage1.Controls.Add(Me.txtMaterialSw)
        Me.TabPage1.Controls.Add(Me.txtAreametroquadr)
        Me.TabPage1.Controls.Add(Me.Label18)
        Me.TabPage1.Controls.Add(Me.txtCutSizey)
        Me.TabPage1.Controls.Add(Me.Label17)
        Me.TabPage1.Controls.Add(Me.txtCutSizex)
        Me.TabPage1.Controls.Add(Me.Label16)
        Me.TabPage1.Controls.Add(Me.Label15)
        Me.TabPage1.Controls.Add(Me.txtEmpresa)
        Me.TabPage1.Controls.Add(Me.Label14)
        Me.TabPage1.Controls.Add(Me.txtRevisao)
        Me.TabPage1.Controls.Add(Me.Label13)
        Me.TabPage1.Controls.Add(Me.Label12)
        Me.TabPage1.Controls.Add(Me.txtPalavraChave)
        Me.TabPage1.Controls.Add(Me.txtData1)
        Me.TabPage1.Controls.Add(Me.Label9)
        Me.TabPage1.Controls.Add(Me.txtDataR)
        Me.TabPage1.Controls.Add(Me.Label10)
        Me.TabPage1.Controls.Add(Me.txtData)
        Me.TabPage1.Controls.Add(Me.Label11)
        Me.TabPage1.Controls.Add(Me.txtendereco)
        Me.TabPage1.Controls.Add(Me.Label8)
        Me.TabPage1.Controls.Add(Me.txtGerente)
        Me.TabPage1.Controls.Add(Me.Label7)
        Me.TabPage1.Controls.Add(Me.txtCategoria)
        Me.TabPage1.Controls.Add(Me.Label6)
        Me.TabPage1.Controls.Add(Me.txtAutor)
        Me.TabPage1.Controls.Add(Me.Label5)
        Me.TabPage1.Controls.Add(Me.Label4)
        Me.TabPage1.Controls.Add(Me.txtNumeroDesenho)
        Me.TabPage1.Controls.Add(Me.Label3)
        Me.TabPage1.Controls.Add(Me.txtTitulo)
        Me.TabPage1.Controls.Add(Me.Label1)
        Me.TabPage1.Controls.Add(Me.btnAbriDetalhamentoCorrente)
        Me.TabPage1.Controls.Add(Me.btnGerarPdf)
        Me.TabPage1.Controls.Add(Me.btnDxf)
        Me.TabPage1.Location = New System.Drawing.Point(4, 25)
        Me.TabPage1.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Padding = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.TabPage1.Size = New System.Drawing.Size(1570, 749)
        Me.TabPage1.TabIndex = 0
        Me.TabPage1.Text = "Dados Arquivo Corrente"
        Me.TabPage1.UseVisualStyleBackColor = True
        '
        'btnSalvarCadProtheus
        '
        Me.btnSalvarCadProtheus.Image = Global.SINCO_SolidEdeg_1._0.My.Resources.Resources.salvar
        Me.btnSalvarCadProtheus.Location = New System.Drawing.Point(166, 6)
        Me.btnSalvarCadProtheus.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.btnSalvarCadProtheus.Name = "btnSalvarCadProtheus"
        Me.btnSalvarCadProtheus.Size = New System.Drawing.Size(135, 50)
        Me.btnSalvarCadProtheus.TabIndex = 96
        Me.btnSalvarCadProtheus.Text = "Buscar Cod. - Protheus"
        Me.btnSalvarCadProtheus.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.btnSalvarCadProtheus.UseVisualStyleBackColor = True
        '
        'txtB1_XREVM
        '
        Me.txtB1_XREVM.Location = New System.Drawing.Point(1311, 134)
        Me.txtB1_XREVM.Name = "txtB1_XREVM"
        Me.txtB1_XREVM.Size = New System.Drawing.Size(100, 22)
        Me.txtB1_XREVM.TabIndex = 95
        Me.txtB1_XREVM.Text = "00"
        '
        'Label50
        '
        Me.Label50.AutoSize = True
        Me.Label50.Location = New System.Drawing.Point(1312, 115)
        Me.Label50.Name = "Label50"
        Me.Label50.Size = New System.Drawing.Size(95, 16)
        Me.Label50.TabIndex = 94
        Me.Label50.Text = "Rev. Metalfisa:"
        '
        'cboB1_GRUPO
        '
        Me.cboB1_GRUPO.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboB1_GRUPO.FormattingEnabled = True
        Me.cboB1_GRUPO.Location = New System.Drawing.Point(487, 134)
        Me.cboB1_GRUPO.Name = "cboB1_GRUPO"
        Me.cboB1_GRUPO.Size = New System.Drawing.Size(322, 24)
        Me.cboB1_GRUPO.TabIndex = 93
        '
        'Label49
        '
        Me.Label49.AutoSize = True
        Me.Label49.Location = New System.Drawing.Point(488, 116)
        Me.Label49.Name = "Label49"
        Me.Label49.Size = New System.Drawing.Size(47, 16)
        Me.Label49.TabIndex = 92
        Me.Label49.Text = "Grupo:"
        '
        'cboB1_UM
        '
        Me.cboB1_UM.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboB1_UM.FormattingEnabled = True
        Me.cboB1_UM.Location = New System.Drawing.Point(260, 134)
        Me.cboB1_UM.Name = "cboB1_UM"
        Me.cboB1_UM.Size = New System.Drawing.Size(221, 24)
        Me.cboB1_UM.TabIndex = 91
        '
        'Label48
        '
        Me.Label48.AutoSize = True
        Me.Label48.Location = New System.Drawing.Point(262, 116)
        Me.Label48.Name = "Label48"
        Me.Label48.Size = New System.Drawing.Size(62, 16)
        Me.Label48.TabIndex = 90
        Me.Label48.Text = "Unidade:"
        '
        'cboB1_TIPO
        '
        Me.cboB1_TIPO.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboB1_TIPO.FormattingEnabled = True
        Me.cboB1_TIPO.Location = New System.Drawing.Point(11, 134)
        Me.cboB1_TIPO.Name = "cboB1_TIPO"
        Me.cboB1_TIPO.Size = New System.Drawing.Size(243, 24)
        Me.cboB1_TIPO.TabIndex = 89
        '
        'Label47
        '
        Me.Label47.AutoSize = True
        Me.Label47.Location = New System.Drawing.Point(8, 116)
        Me.Label47.Name = "Label47"
        Me.Label47.Size = New System.Drawing.Size(38, 16)
        Me.Label47.TabIndex = 88
        Me.Label47.Text = "Tipo:"
        '
        'btnPdfLote
        '
        Me.btnPdfLote.Image = Global.SINCO_SolidEdeg_1._0.My.Resources.Resources.pdf
        Me.btnPdfLote.Location = New System.Drawing.Point(1084, 7)
        Me.btnPdfLote.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.btnPdfLote.Name = "btnPdfLote"
        Me.btnPdfLote.Size = New System.Drawing.Size(135, 50)
        Me.btnPdfLote.TabIndex = 87
        Me.btnPdfLote.Text = "Gerar PDF Lote"
        Me.btnPdfLote.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.ToolTipAjuda.SetToolTip(Me.btnPdfLote, "🧾 Clique aqui para gerar o PDF do desenho atual." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "⚠️ O PDF só será gerado se o d" &
        "etalhamento existir." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "        ")
        Me.btnPdfLote.UseVisualStyleBackColor = True
        '
        'btnEstrutraMaterialProtheus
        '
        Me.btnEstrutraMaterialProtheus.Location = New System.Drawing.Point(426, 497)
        Me.btnEstrutraMaterialProtheus.Name = "btnEstrutraMaterialProtheus"
        Me.btnEstrutraMaterialProtheus.Size = New System.Drawing.Size(167, 35)
        Me.btnEstrutraMaterialProtheus.TabIndex = 86
        Me.btnEstrutraMaterialProtheus.Text = "Estrutura Material"
        Me.btnEstrutraMaterialProtheus.UseVisualStyleBackColor = True
        '
        'btnBuscarOperacaoProtheus
        '
        Me.btnBuscarOperacaoProtheus.Location = New System.Drawing.Point(642, 230)
        Me.btnBuscarOperacaoProtheus.Name = "btnBuscarOperacaoProtheus"
        Me.btnBuscarOperacaoProtheus.Size = New System.Drawing.Size(285, 41)
        Me.btnBuscarOperacaoProtheus.TabIndex = 85
        Me.btnBuscarOperacaoProtheus.Text = "Operação do Protheus"
        Me.btnBuscarOperacaoProtheus.UseVisualStyleBackColor = True
        '
        'TcpAcessorios
        '
        Me.TcpAcessorios.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TcpAcessorios.Controls.Add(Me.tpgGabaritos)
        Me.TcpAcessorios.Controls.Add(Me.tpgCliente)
        Me.TcpAcessorios.Location = New System.Drawing.Point(1280, 230)
        Me.TcpAcessorios.Name = "TcpAcessorios"
        Me.TcpAcessorios.SelectedIndex = 0
        Me.TcpAcessorios.Size = New System.Drawing.Size(287, 295)
        Me.TcpAcessorios.TabIndex = 84
        '
        'tpgGabaritos
        '
        Me.tpgGabaritos.Controls.Add(Me.dgvGabaritos)
        Me.tpgGabaritos.Location = New System.Drawing.Point(4, 25)
        Me.tpgGabaritos.Name = "tpgGabaritos"
        Me.tpgGabaritos.Padding = New System.Windows.Forms.Padding(3)
        Me.tpgGabaritos.Size = New System.Drawing.Size(279, 266)
        Me.tpgGabaritos.TabIndex = 0
        Me.tpgGabaritos.Text = "Gabaritos"
        Me.tpgGabaritos.UseVisualStyleBackColor = True
        '
        'dgvGabaritos
        '
        Me.dgvGabaritos.AllowUserToAddRows = False
        Me.dgvGabaritos.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        Me.dgvGabaritos.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedHeaders
        Me.dgvGabaritos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvGabaritos.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgvGabaritos.Location = New System.Drawing.Point(3, 3)
        Me.dgvGabaritos.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.dgvGabaritos.Name = "dgvGabaritos"
        Me.dgvGabaritos.ReadOnly = True
        Me.dgvGabaritos.RowHeadersWidth = 51
        Me.dgvGabaritos.RowTemplate.Height = 24
        Me.dgvGabaritos.Size = New System.Drawing.Size(273, 260)
        Me.dgvGabaritos.TabIndex = 81
        '
        'tpgCliente
        '
        Me.tpgCliente.Controls.Add(Me.dgvDesenhoCliente)
        Me.tpgCliente.Location = New System.Drawing.Point(4, 25)
        Me.tpgCliente.Name = "tpgCliente"
        Me.tpgCliente.Padding = New System.Windows.Forms.Padding(3)
        Me.tpgCliente.Size = New System.Drawing.Size(279, 266)
        Me.tpgCliente.TabIndex = 1
        Me.tpgCliente.Text = "Desenho do Cliente"
        Me.tpgCliente.UseVisualStyleBackColor = True
        '
        'dgvDesenhoCliente
        '
        Me.dgvDesenhoCliente.AllowUserToAddRows = False
        Me.dgvDesenhoCliente.AllowUserToDeleteRows = False
        Me.dgvDesenhoCliente.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCellsExceptHeader
        Me.dgvDesenhoCliente.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCells
        Me.dgvDesenhoCliente.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvDesenhoCliente.ContextMenuStrip = Me.mnudgvDesenhoCliente
        Me.dgvDesenhoCliente.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgvDesenhoCliente.Location = New System.Drawing.Point(3, 3)
        Me.dgvDesenhoCliente.Name = "dgvDesenhoCliente"
        Me.dgvDesenhoCliente.ReadOnly = True
        Me.dgvDesenhoCliente.RowHeadersWidth = 51
        Me.dgvDesenhoCliente.RowTemplate.Height = 24
        Me.dgvDesenhoCliente.Size = New System.Drawing.Size(273, 260)
        Me.dgvDesenhoCliente.TabIndex = 0
        '
        'mnudgvDesenhoCliente
        '
        Me.mnudgvDesenhoCliente.ImageScalingSize = New System.Drawing.Size(20, 20)
        Me.mnudgvDesenhoCliente.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.BuscarDesenhoDeReferenciaToolStripMenuItem, Me.BuscarArquivoPDFDeReferenciaToolStripMenuItem, Me.AbrirArquivoPDFToolStripMenuItem, Me.ToolStripSeparator4, Me.BuscarArquivoDoClienteDiretemanteNoDiretorioSemPassarPeloProtheusToolStripMenuItem})
        Me.mnudgvDesenhoCliente.Name = "mnudgvDesenhoCliente"
        Me.mnudgvDesenhoCliente.Size = New System.Drawing.Size(590, 106)
        '
        'BuscarDesenhoDeReferenciaToolStripMenuItem
        '
        Me.BuscarDesenhoDeReferenciaToolStripMenuItem.Name = "BuscarDesenhoDeReferenciaToolStripMenuItem"
        Me.BuscarDesenhoDeReferenciaToolStripMenuItem.Size = New System.Drawing.Size(589, 24)
        Me.BuscarDesenhoDeReferenciaToolStripMenuItem.Text = "Buscar Desenho de Referencia - Lista Protheus"
        '
        'BuscarArquivoPDFDeReferenciaToolStripMenuItem
        '
        Me.BuscarArquivoPDFDeReferenciaToolStripMenuItem.Name = "BuscarArquivoPDFDeReferenciaToolStripMenuItem"
        Me.BuscarArquivoPDFDeReferenciaToolStripMenuItem.Size = New System.Drawing.Size(589, 24)
        Me.BuscarArquivoPDFDeReferenciaToolStripMenuItem.Text = "Buscar Arquivo PDF de Referencia Para Associar Protheus"
        '
        'AbrirArquivoPDFToolStripMenuItem
        '
        Me.AbrirArquivoPDFToolStripMenuItem.Name = "AbrirArquivoPDFToolStripMenuItem"
        Me.AbrirArquivoPDFToolStripMenuItem.Size = New System.Drawing.Size(589, 24)
        Me.AbrirArquivoPDFToolStripMenuItem.Text = "Abrir Arquivo PDF"
        '
        'ToolStripSeparator4
        '
        Me.ToolStripSeparator4.Name = "ToolStripSeparator4"
        Me.ToolStripSeparator4.Size = New System.Drawing.Size(586, 6)
        '
        'BuscarArquivoDoClienteDiretemanteNoDiretorioSemPassarPeloProtheusToolStripMenuItem
        '
        Me.BuscarArquivoDoClienteDiretemanteNoDiretorioSemPassarPeloProtheusToolStripMenuItem.Name = "BuscarArquivoDoClienteDiretemanteNoDiretorioSemPassarPeloProtheusToolStripMenuIte" &
    "m"
        Me.BuscarArquivoDoClienteDiretemanteNoDiretorioSemPassarPeloProtheusToolStripMenuItem.Size = New System.Drawing.Size(589, 24)
        Me.BuscarArquivoDoClienteDiretemanteNoDiretorioSemPassarPeloProtheusToolStripMenuItem.Text = "Buscar Arquivo do Cliente diretemante no diretorio sem passar pelo Protheus"
        '
        'btnProjetoProtheus
        '
        Me.btnProjetoProtheus.Location = New System.Drawing.Point(936, 7)
        Me.btnProjetoProtheus.Name = "btnProjetoProtheus"
        Me.btnProjetoProtheus.Size = New System.Drawing.Size(128, 49)
        Me.btnProjetoProtheus.TabIndex = 83
        Me.btnProjetoProtheus.Text = "Projeto Protheus"
        Me.btnProjetoProtheus.UseVisualStyleBackColor = True
        '
        'gpbMateriaisProtheus
        '
        Me.gpbMateriaisProtheus.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.gpbMateriaisProtheus.Controls.Add(Me.chkGabarito)
        Me.gpbMateriaisProtheus.Controls.Add(Me.TxtPesqRP)
        Me.gpbMateriaisProtheus.Controls.Add(Me.Label40)
        Me.gpbMateriaisProtheus.Controls.Add(Me.dgvMateriaisProtheus)
        Me.gpbMateriaisProtheus.Controls.Add(Me.txtPesqDescricao3)
        Me.gpbMateriaisProtheus.Controls.Add(Me.txtPesqDescricao2)
        Me.gpbMateriaisProtheus.Controls.Add(Me.txtPesqDescricao1)
        Me.gpbMateriaisProtheus.Controls.Add(Me.Label34)
        Me.gpbMateriaisProtheus.Controls.Add(Me.txtPesqCodigo)
        Me.gpbMateriaisProtheus.Controls.Add(Me.Label33)
        Me.gpbMateriaisProtheus.Enabled = False
        Me.gpbMateriaisProtheus.Location = New System.Drawing.Point(936, 531)
        Me.gpbMateriaisProtheus.Name = "gpbMateriaisProtheus"
        Me.gpbMateriaisProtheus.Size = New System.Drawing.Size(627, 210)
        Me.gpbMateriaisProtheus.TabIndex = 80
        Me.gpbMateriaisProtheus.TabStop = False
        Me.gpbMateriaisProtheus.Text = "Materiais Protheus"
        '
        'chkGabarito
        '
        Me.chkGabarito.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.chkGabarito.AutoSize = True
        Me.chkGabarito.Location = New System.Drawing.Point(421, 18)
        Me.chkGabarito.Name = "chkGabarito"
        Me.chkGabarito.Size = New System.Drawing.Size(195, 20)
        Me.chkGabarito.TabIndex = 55
        Me.chkGabarito.Text = "Mostrar Desenhos/Gabarito"
        Me.chkGabarito.UseVisualStyleBackColor = True
        '
        'TxtPesqRP
        '
        Me.TxtPesqRP.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.TxtPesqRP.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.TxtPesqRP.Location = New System.Drawing.Point(137, 40)
        Me.TxtPesqRP.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.TxtPesqRP.Name = "TxtPesqRP"
        Me.TxtPesqRP.Size = New System.Drawing.Size(116, 22)
        Me.TxtPesqRP.TabIndex = 51
        Me.TxtPesqRP.Tag = "SummaryInformation - Título"
        '
        'Label40
        '
        Me.Label40.AutoSize = True
        Me.Label40.Location = New System.Drawing.Point(134, 22)
        Me.Label40.Name = "Label40"
        Me.Label40.Size = New System.Drawing.Size(60, 16)
        Me.Label40.TabIndex = 7
        Me.Label40.Text = "Cód. RP."
        '
        'dgvMateriaisProtheus
        '
        Me.dgvMateriaisProtheus.AllowUserToAddRows = False
        Me.dgvMateriaisProtheus.AllowUserToDeleteRows = False
        Me.dgvMateriaisProtheus.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dgvMateriaisProtheus.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCellsExceptHeader
        Me.dgvMateriaisProtheus.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders
        Me.dgvMateriaisProtheus.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvMateriaisProtheus.ContextMenuStrip = Me.mnudgvMateriaisProtheus
        Me.dgvMateriaisProtheus.Location = New System.Drawing.Point(15, 67)
        Me.dgvMateriaisProtheus.Name = "dgvMateriaisProtheus"
        Me.dgvMateriaisProtheus.ReadOnly = True
        Me.dgvMateriaisProtheus.RowHeadersWidth = 51
        Me.dgvMateriaisProtheus.RowTemplate.Height = 24
        Me.dgvMateriaisProtheus.Size = New System.Drawing.Size(604, 137)
        Me.dgvMateriaisProtheus.TabIndex = 6
        '
        'mnudgvMateriaisProtheus
        '
        Me.mnudgvMateriaisProtheus.ImageScalingSize = New System.Drawing.Size(20, 20)
        Me.mnudgvMateriaisProtheus.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.AssociarFichaTecnicaToolStripMenuItem, Me.AssociarGabaritoAPeçaCorrenteToolStripMenuItem, Me.BuscarMaterialNoProtheusToolStripMenuItem})
        Me.mnudgvMateriaisProtheus.Name = "mnudgvMateriaisProtheus"
        Me.mnudgvMateriaisProtheus.Size = New System.Drawing.Size(305, 76)
        '
        'AssociarFichaTecnicaToolStripMenuItem
        '
        Me.AssociarFichaTecnicaToolStripMenuItem.Name = "AssociarFichaTecnicaToolStripMenuItem"
        Me.AssociarFichaTecnicaToolStripMenuItem.Size = New System.Drawing.Size(304, 24)
        Me.AssociarFichaTecnicaToolStripMenuItem.Text = "Associar/Ver Ficha Tecnica"
        '
        'AssociarGabaritoAPeçaCorrenteToolStripMenuItem
        '
        Me.AssociarGabaritoAPeçaCorrenteToolStripMenuItem.Name = "AssociarGabaritoAPeçaCorrenteToolStripMenuItem"
        Me.AssociarGabaritoAPeçaCorrenteToolStripMenuItem.Size = New System.Drawing.Size(304, 24)
        Me.AssociarGabaritoAPeçaCorrenteToolStripMenuItem.Text = "Associar Gabarito a peça Corrente"
        '
        'BuscarMaterialNoProtheusToolStripMenuItem
        '
        Me.BuscarMaterialNoProtheusToolStripMenuItem.Name = "BuscarMaterialNoProtheusToolStripMenuItem"
        Me.BuscarMaterialNoProtheusToolStripMenuItem.Size = New System.Drawing.Size(304, 24)
        Me.BuscarMaterialNoProtheusToolStripMenuItem.Text = "Buscar Material no Protheus"
        '
        'txtPesqDescricao3
        '
        Me.txtPesqDescricao3.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.txtPesqDescricao3.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtPesqDescricao3.Location = New System.Drawing.Point(501, 40)
        Me.txtPesqDescricao3.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.txtPesqDescricao3.Name = "txtPesqDescricao3"
        Me.txtPesqDescricao3.Size = New System.Drawing.Size(115, 22)
        Me.txtPesqDescricao3.TabIndex = 54
        Me.txtPesqDescricao3.Tag = "SummaryInformation - Título"
        '
        'txtPesqDescricao2
        '
        Me.txtPesqDescricao2.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.txtPesqDescricao2.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtPesqDescricao2.Location = New System.Drawing.Point(380, 40)
        Me.txtPesqDescricao2.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.txtPesqDescricao2.Name = "txtPesqDescricao2"
        Me.txtPesqDescricao2.Size = New System.Drawing.Size(115, 22)
        Me.txtPesqDescricao2.TabIndex = 53
        Me.txtPesqDescricao2.Tag = "SummaryInformation - Título"
        '
        'txtPesqDescricao1
        '
        Me.txtPesqDescricao1.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.txtPesqDescricao1.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtPesqDescricao1.Location = New System.Drawing.Point(259, 40)
        Me.txtPesqDescricao1.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.txtPesqDescricao1.Name = "txtPesqDescricao1"
        Me.txtPesqDescricao1.Size = New System.Drawing.Size(115, 22)
        Me.txtPesqDescricao1.TabIndex = 52
        Me.txtPesqDescricao1.Tag = "SummaryInformation - Título"
        '
        'Label34
        '
        Me.Label34.AutoSize = True
        Me.Label34.Location = New System.Drawing.Point(256, 22)
        Me.Label34.Name = "Label34"
        Me.Label34.Size = New System.Drawing.Size(72, 16)
        Me.Label34.TabIndex = 2
        Me.Label34.Text = "Descrição."
        '
        'txtPesqCodigo
        '
        Me.txtPesqCodigo.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.txtPesqCodigo.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtPesqCodigo.Location = New System.Drawing.Point(15, 40)
        Me.txtPesqCodigo.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.txtPesqCodigo.Name = "txtPesqCodigo"
        Me.txtPesqCodigo.Size = New System.Drawing.Size(116, 22)
        Me.txtPesqCodigo.TabIndex = 50
        Me.txtPesqCodigo.Tag = "SummaryInformation - Título"
        '
        'Label33
        '
        Me.Label33.AutoSize = True
        Me.Label33.Location = New System.Drawing.Point(12, 22)
        Me.Label33.Name = "Label33"
        Me.Label33.Size = New System.Drawing.Size(35, 16)
        Me.Label33.TabIndex = 0
        Me.Label33.Text = "Cód."
        '
        'gpbMateriaiDesenho
        '
        Me.gpbMateriaiDesenho.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.gpbMateriaiDesenho.Controls.Add(Me.dgvMaterialPeca)
        Me.gpbMateriaiDesenho.Location = New System.Drawing.Point(12, 531)
        Me.gpbMateriaiDesenho.Name = "gpbMateriaiDesenho"
        Me.gpbMateriaiDesenho.Size = New System.Drawing.Size(918, 213)
        Me.gpbMateriaiDesenho.TabIndex = 79
        Me.gpbMateriaiDesenho.TabStop = False
        Me.gpbMateriaiDesenho.Text = "Lista de Materiais Inseridos no desenho"
        '
        'dgvMaterialPeca
        '
        Me.dgvMaterialPeca.AllowUserToAddRows = False
        Me.dgvMaterialPeca.AllowUserToDeleteRows = False
        Me.dgvMaterialPeca.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCellsExceptHeader
        Me.dgvMaterialPeca.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders
        Me.dgvMaterialPeca.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvMaterialPeca.ContextMenuStrip = Me.mnudgvMaterialPeca
        Me.dgvMaterialPeca.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgvMaterialPeca.Location = New System.Drawing.Point(3, 18)
        Me.dgvMaterialPeca.Name = "dgvMaterialPeca"
        Me.dgvMaterialPeca.ReadOnly = True
        Me.dgvMaterialPeca.RowHeadersWidth = 51
        Me.dgvMaterialPeca.RowTemplate.Height = 24
        Me.dgvMaterialPeca.Size = New System.Drawing.Size(912, 192)
        Me.dgvMaterialPeca.TabIndex = 78
        '
        'mnudgvMaterialPeca
        '
        Me.mnudgvMaterialPeca.ImageScalingSize = New System.Drawing.Size(20, 20)
        Me.mnudgvMaterialPeca.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.AlterarQtdeToolStripMenuItem})
        Me.mnudgvMaterialPeca.Name = "mnudgvMaterialPeca"
        Me.mnudgvMaterialPeca.Size = New System.Drawing.Size(161, 28)
        '
        'AlterarQtdeToolStripMenuItem
        '
        Me.AlterarQtdeToolStripMenuItem.Name = "AlterarQtdeToolStripMenuItem"
        Me.AlterarQtdeToolStripMenuItem.Size = New System.Drawing.Size(160, 24)
        Me.AlterarQtdeToolStripMenuItem.Text = "Alterar Qtde"
        '
        'btnMsg
        '
        Me.btnMsg.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnMsg.BackColor = System.Drawing.SystemColors.Info
        Me.btnMsg.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnMsg.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.btnMsg.Location = New System.Drawing.Point(9, 66)
        Me.btnMsg.Margin = New System.Windows.Forms.Padding(4)
        Me.btnMsg.Name = "btnMsg"
        Me.btnMsg.Size = New System.Drawing.Size(1554, 35)
        Me.btnMsg.TabIndex = 77
        Me.btnMsg.UseVisualStyleBackColor = False
        '
        'Button5
        '
        Me.Button5.Location = New System.Drawing.Point(935, 32)
        Me.Button5.Margin = New System.Windows.Forms.Padding(4)
        Me.Button5.Name = "Button5"
        Me.Button5.Size = New System.Drawing.Size(145, 50)
        Me.Button5.TabIndex = 76
        Me.Button5.Text = "Dados Especificos"
        Me.Button5.UseVisualStyleBackColor = True
        Me.Button5.Visible = False
        '
        'cboAcabamento
        '
        Me.cboAcabamento.FormattingEnabled = True
        Me.cboAcabamento.Location = New System.Drawing.Point(48, 188)
        Me.cboAcabamento.Name = "cboAcabamento"
        Me.cboAcabamento.Size = New System.Drawing.Size(503, 24)
        Me.cboAcabamento.TabIndex = 75
        Me.cboAcabamento.Tag = "SummaryInformation - Comentários"
        '
        'cboTipoDesenho
        '
        Me.cboTipoDesenho.FormattingEnabled = True
        Me.cboTipoDesenho.Location = New System.Drawing.Point(601, 188)
        Me.cboTipoDesenho.Name = "cboTipoDesenho"
        Me.cboTipoDesenho.Size = New System.Drawing.Size(257, 24)
        Me.cboTipoDesenho.TabIndex = 74
        Me.cboTipoDesenho.Tag = "Custom - Tipo de desenho"
        '
        'btnConverterChapa
        '
        Me.btnConverterChapa.Enabled = False
        Me.btnConverterChapa.Location = New System.Drawing.Point(935, 6)
        Me.btnConverterChapa.Margin = New System.Windows.Forms.Padding(4)
        Me.btnConverterChapa.Name = "btnConverterChapa"
        Me.btnConverterChapa.Size = New System.Drawing.Size(131, 50)
        Me.btnConverterChapa.TabIndex = 73
        Me.btnConverterChapa.Text = "Tentar Converter em Chapa"
        Me.btnConverterChapa.UseVisualStyleBackColor = True
        Me.btnConverterChapa.Visible = False
        '
        'cboBloqueado
        '
        Me.cboBloqueado.FormattingEnabled = True
        Me.cboBloqueado.Items.AddRange(New Object() {"SIM", "NÃO"})
        Me.cboBloqueado.Location = New System.Drawing.Point(1109, 178)
        Me.cboBloqueado.Name = "cboBloqueado"
        Me.cboBloqueado.Size = New System.Drawing.Size(121, 24)
        Me.cboBloqueado.TabIndex = 72
        Me.cboBloqueado.Tag = "Custom - Bloqueado"
        Me.cboBloqueado.Text = "NÃO"
        Me.ToolTipAjuda.SetToolTip(Me.cboBloqueado, "SIM")
        '
        'Label39
        '
        Me.Label39.AutoSize = True
        Me.Label39.Location = New System.Drawing.Point(1110, 157)
        Me.Label39.Name = "Label39"
        Me.Label39.Size = New System.Drawing.Size(77, 16)
        Me.Label39.TabIndex = 71
        Me.Label39.Text = "Bloqueado:"
        '
        'TextBox1
        '
        Me.TextBox1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TextBox1.Location = New System.Drawing.Point(1259, 10)
        Me.TextBox1.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.TextBox1.Multiline = True
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.Size = New System.Drawing.Size(73, 43)
        Me.TextBox1.TabIndex = 61
        Me.TextBox1.Visible = False
        '
        'btnCriaPropriedadePadroes
        '
        Me.btnCriaPropriedadePadroes.Location = New System.Drawing.Point(783, 6)
        Me.btnCriaPropriedadePadroes.Margin = New System.Windows.Forms.Padding(4)
        Me.btnCriaPropriedadePadroes.Name = "btnCriaPropriedadePadroes"
        Me.btnCriaPropriedadePadroes.Size = New System.Drawing.Size(131, 50)
        Me.btnCriaPropriedadePadroes.TabIndex = 70
        Me.btnCriaPropriedadePadroes.Text = "Ativar/Criar Propriedades"
        Me.btnCriaPropriedadePadroes.UseVisualStyleBackColor = True
        '
        'Button4
        '
        Me.Button4.Image = Global.SINCO_SolidEdeg_1._0.My.Resources.Resources.Ajuda_16x16
        Me.Button4.Location = New System.Drawing.Point(604, 230)
        Me.Button4.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.Button4.Name = "Button4"
        Me.Button4.Size = New System.Drawing.Size(32, 34)
        Me.Button4.TabIndex = 69
        Me.Button4.Text = resources.GetString("Button4.Text")
        Me.ToolTipAjuda.SetToolTip(Me.Button4, resources.GetString("Button4.ToolTip"))
        Me.Button4.UseVisualStyleBackColor = True
        '
        'Button3
        '
        Me.Button3.Image = Global.SINCO_SolidEdeg_1._0.My.Resources.Resources.Ajuda_16x16
        Me.Button3.Location = New System.Drawing.Point(10, 182)
        Me.Button3.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.Button3.Name = "Button3"
        Me.Button3.Size = New System.Drawing.Size(32, 34)
        Me.Button3.TabIndex = 68
        Me.ToolTipAjuda.SetToolTip(Me.Button3, resources.GetString("Button3.ToolTip"))
        Me.Button3.UseVisualStyleBackColor = True
        '
        'Button2
        '
        Me.Button2.Image = Global.SINCO_SolidEdeg_1._0.My.Resources.Resources.Ajuda_16x16
        Me.Button2.Location = New System.Drawing.Point(566, 178)
        Me.Button2.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(32, 34)
        Me.Button2.TabIndex = 67
        Me.ToolTipAjuda.SetToolTip(Me.Button2, "🧩 Lista de tipos de desenho para auxiliar na organização e padronização do proce" &
        "sso de produção.")
        Me.Button2.UseVisualStyleBackColor = True
        '
        'Button1
        '
        Me.Button1.Image = Global.SINCO_SolidEdeg_1._0.My.Resources.Resources.Ajuda_16x16
        Me.Button1.Location = New System.Drawing.Point(940, 196)
        Me.Button1.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(32, 34)
        Me.Button1.TabIndex = 66
        Me.ToolTipAjuda.SetToolTip(Me.Button1, resources.GetString("Button1.ToolTip"))
        Me.Button1.UseVisualStyleBackColor = True
        '
        'Label32
        '
        Me.Label32.AutoSize = True
        Me.Label32.Location = New System.Drawing.Point(645, 240)
        Me.Label32.Name = "Label32"
        Me.Label32.Size = New System.Drawing.Size(228, 16)
        Me.Label32.TabIndex = 63
        Me.Label32.Text = "Lista de Processos Aplicado a Peça:"
        '
        'Label31
        '
        Me.Label31.AutoSize = True
        Me.Label31.Location = New System.Drawing.Point(977, 209)
        Me.Label31.Name = "Label31"
        Me.Label31.Size = New System.Drawing.Size(192, 16)
        Me.Label31.TabIndex = 62
        Me.Label31.Text = "Lista de Processos Diponiveis:"
        '
        'dgvProcessoMaterial
        '
        Me.dgvProcessoMaterial.AllowUserToAddRows = False
        Me.dgvProcessoMaterial.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        Me.dgvProcessoMaterial.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders
        Me.dgvProcessoMaterial.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvProcessoMaterial.Location = New System.Drawing.Point(604, 275)
        Me.dgvProcessoMaterial.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.dgvProcessoMaterial.Name = "dgvProcessoMaterial"
        Me.dgvProcessoMaterial.ReadOnly = True
        Me.dgvProcessoMaterial.RowHeadersWidth = 51
        Me.dgvProcessoMaterial.RowTemplate.Height = 24
        Me.dgvProcessoMaterial.Size = New System.Drawing.Size(323, 241)
        Me.dgvProcessoMaterial.TabIndex = 0
        '
        'dgvProcessos
        '
        Me.dgvProcessos.AllowUserToAddRows = False
        Me.dgvProcessos.AllowUserToDeleteRows = False
        Me.dgvProcessos.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        Me.dgvProcessos.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders
        Me.dgvProcessos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvProcessos.Location = New System.Drawing.Point(940, 276)
        Me.dgvProcessos.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.dgvProcessos.Name = "dgvProcessos"
        Me.dgvProcessos.ReadOnly = True
        Me.dgvProcessos.RowHeadersWidth = 51
        Me.dgvProcessos.RowTemplate.Height = 24
        Me.dgvProcessos.Size = New System.Drawing.Size(334, 241)
        Me.dgvProcessos.TabIndex = 16
        Me.ToolTipAjuda.SetToolTip(Me.dgvProcessos, "Lista de processos disponíveis para associação à peça. " & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Dê um duplo clique no pr" &
        "ocesso desejado para vinculá-lo à peça atual.")
        '
        'txtPesqProcesso
        '
        Me.txtPesqProcesso.Location = New System.Drawing.Point(940, 238)
        Me.txtPesqProcesso.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.txtPesqProcesso.Name = "txtPesqProcesso"
        Me.txtPesqProcesso.Size = New System.Drawing.Size(334, 22)
        Me.txtPesqProcesso.TabIndex = 17
        '
        'Label23
        '
        Me.Label23.AutoSize = True
        Me.Label23.Location = New System.Drawing.Point(598, 169)
        Me.Label23.Name = "Label23"
        Me.Label23.Size = New System.Drawing.Size(96, 16)
        Me.Label23.TabIndex = 60
        Me.Label23.Text = "Tipo Desenho:"
        '
        'btnAtualizacoesDados
        '
        Me.btnAtualizacoesDados.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnAtualizacoesDados.Location = New System.Drawing.Point(1242, 6)
        Me.btnAtualizacoesDados.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.btnAtualizacoesDados.Name = "btnAtualizacoesDados"
        Me.btnAtualizacoesDados.Size = New System.Drawing.Size(157, 50)
        Me.btnAtualizacoesDados.TabIndex = 58
        Me.btnAtualizacoesDados.Text = "Buscar Por Atualizaçoes de dados"
        Me.btnAtualizacoesDados.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.ToolTipAjuda.SetToolTip(Me.btnAtualizacoesDados, "🔄 Em caso de novos cadastros realizados no SINCO (ex.: Acabamento, Tipo de Desen" &
        "ho, Processo ou Projeto)," & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & " clique aqui para atualizar e disponibilizar os novos" &
        " dados.")
        Me.btnAtualizacoesDados.UseVisualStyleBackColor = True
        '
        'btnConfiguracoes
        '
        Me.btnConfiguracoes.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnConfiguracoes.Location = New System.Drawing.Point(1414, 6)
        Me.btnConfiguracoes.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.btnConfiguracoes.Name = "btnConfiguracoes"
        Me.btnConfiguracoes.Size = New System.Drawing.Size(149, 50)
        Me.btnConfiguracoes.TabIndex = 57
        Me.btnConfiguracoes.Text = "Buscar Configurações"
        Me.btnConfiguracoes.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.ToolTipAjuda.SetToolTip(Me.btnConfiguracoes, "⚙️ Clique aqui para carregar as configurações de integração com o SINCO.")
        Me.btnConfiguracoes.UseVisualStyleBackColor = True
        '
        'txtPesoKg
        '
        Me.txtPesoKg.AcceptsTab = True
        Me.txtPesoKg.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtPesoKg.Enabled = False
        Me.txtPesoKg.Location = New System.Drawing.Point(396, 364)
        Me.txtPesoKg.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.txtPesoKg.Name = "txtPesoKg"
        Me.txtPesoKg.Size = New System.Drawing.Size(103, 22)
        Me.txtPesoKg.TabIndex = 55
        '
        'Label22
        '
        Me.Label22.AutoSize = True
        Me.Label22.Location = New System.Drawing.Point(396, 342)
        Me.Label22.Name = "Label22"
        Me.Label22.Size = New System.Drawing.Size(60, 16)
        Me.Label22.TabIndex = 56
        Me.Label22.Text = "Peso kg:"
        '
        'btnSalvar
        '
        Me.btnSalvar.Image = Global.SINCO_SolidEdeg_1._0.My.Resources.Resources.salvar
        Me.btnSalvar.Location = New System.Drawing.Point(11, 6)
        Me.btnSalvar.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.btnSalvar.Name = "btnSalvar"
        Me.btnSalvar.Size = New System.Drawing.Size(135, 50)
        Me.btnSalvar.TabIndex = 54
        Me.btnSalvar.Text = "Salvar - SINCO"
        Me.btnSalvar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.ToolTipAjuda.SetToolTip(Me.btnSalvar, "🧩 Clique aqui para cadastrar a peça ou montagem no SINCO." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "⚠️ Sem este cadastro," &
        " não será possível criar a Ordem de Serviço.")
        Me.btnSalvar.UseVisualStyleBackColor = True
        '
        'txtEspessura
        '
        Me.txtEspessura.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtEspessura.Enabled = False
        Me.txtEspessura.Location = New System.Drawing.Point(284, 426)
        Me.txtEspessura.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.txtEspessura.Name = "txtEspessura"
        Me.txtEspessura.Size = New System.Drawing.Size(105, 22)
        Me.txtEspessura.TabIndex = 12
        '
        'Label19
        '
        Me.Label19.AutoSize = True
        Me.Label19.Location = New System.Drawing.Point(8, 283)
        Me.Label19.Name = "Label19"
        Me.Label19.Size = New System.Drawing.Size(58, 16)
        Me.Label19.TabIndex = 53
        Me.Label19.Text = "Material:"
        Me.Label19.Visible = False
        '
        'txtMaterialSw
        '
        Me.txtMaterialSw.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtMaterialSw.Enabled = False
        Me.txtMaterialSw.Location = New System.Drawing.Point(11, 304)
        Me.txtMaterialSw.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.txtMaterialSw.Name = "txtMaterialSw"
        Me.txtMaterialSw.Size = New System.Drawing.Size(538, 22)
        Me.txtMaterialSw.TabIndex = 11
        Me.txtMaterialSw.Visible = False
        '
        'txtAreametroquadr
        '
        Me.txtAreametroquadr.AcceptsTab = True
        Me.txtAreametroquadr.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtAreametroquadr.Enabled = False
        Me.txtAreametroquadr.Location = New System.Drawing.Point(287, 364)
        Me.txtAreametroquadr.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.txtAreametroquadr.Name = "txtAreametroquadr"
        Me.txtAreametroquadr.Size = New System.Drawing.Size(103, 22)
        Me.txtAreametroquadr.TabIndex = 15
        '
        'Label18
        '
        Me.Label18.AutoSize = True
        Me.Label18.Location = New System.Drawing.Point(284, 342)
        Me.Label18.Name = "Label18"
        Me.Label18.Size = New System.Drawing.Size(57, 16)
        Me.Label18.TabIndex = 50
        Me.Label18.Text = "Area M²:"
        '
        'txtCutSizey
        '
        Me.txtCutSizey.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtCutSizey.Enabled = False
        Me.txtCutSizey.Location = New System.Drawing.Point(512, 426)
        Me.txtCutSizey.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.txtCutSizey.Name = "txtCutSizey"
        Me.txtCutSizey.Size = New System.Drawing.Size(81, 22)
        Me.txtCutSizey.TabIndex = 14
        Me.txtCutSizey.Tag = "Flat_Pattern_Model_CutSizeY"
        '
        'Label17
        '
        Me.Label17.AutoSize = True
        Me.Label17.Location = New System.Drawing.Point(508, 403)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(90, 16)
        Me.Label17.TabIndex = 48
        Me.Label17.Text = "Comprimento:"
        '
        'txtCutSizex
        '
        Me.txtCutSizex.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtCutSizex.Enabled = False
        Me.txtCutSizex.Location = New System.Drawing.Point(396, 426)
        Me.txtCutSizex.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.txtCutSizex.Name = "txtCutSizex"
        Me.txtCutSizex.Size = New System.Drawing.Size(103, 22)
        Me.txtCutSizex.TabIndex = 13
        Me.txtCutSizex.Tag = "Flat_Pattern_Model_CutSizeX"
        '
        'Label16
        '
        Me.Label16.AutoSize = True
        Me.Label16.Location = New System.Drawing.Point(400, 403)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(56, 16)
        Me.Label16.TabIndex = 46
        Me.Label16.Text = "Largura:"
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.Location = New System.Drawing.Point(284, 403)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(75, 16)
        Me.Label15.TabIndex = 44
        Me.Label15.Text = "Espessura:"
        '
        'txtEmpresa
        '
        Me.txtEmpresa.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtEmpresa.Location = New System.Drawing.Point(277, 249)
        Me.txtEmpresa.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.txtEmpresa.Name = "txtEmpresa"
        Me.txtEmpresa.Size = New System.Drawing.Size(272, 22)
        Me.txtEmpresa.TabIndex = 4
        Me.txtEmpresa.Tag = "DocumentSummaryInformation - Empresa"
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.Location = New System.Drawing.Point(274, 227)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(65, 16)
        Me.Label14.TabIndex = 42
        Me.Label14.Text = "Empresa:"
        '
        'txtRevisao
        '
        Me.txtRevisao.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtRevisao.Location = New System.Drawing.Point(291, 486)
        Me.txtRevisao.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.txtRevisao.Name = "txtRevisao"
        Me.txtRevisao.Size = New System.Drawing.Size(129, 22)
        Me.txtRevisao.TabIndex = 16
        Me.txtRevisao.Tag = "SummaryInformation - Número da Revisão"
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Location = New System.Drawing.Point(291, 463)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(61, 16)
        Me.Label13.TabIndex = 40
        Me.Label13.Text = "Revisao:"
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Location = New System.Drawing.Point(9, 227)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(152, 16)
        Me.Label12.TabIndex = 39
        Me.Label12.Text = "PalavraChave-Camada:"
        '
        'txtPalavraChave
        '
        Me.txtPalavraChave.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtPalavraChave.Enabled = False
        Me.txtPalavraChave.Location = New System.Drawing.Point(11, 249)
        Me.txtPalavraChave.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.txtPalavraChave.Name = "txtPalavraChave"
        Me.txtPalavraChave.Size = New System.Drawing.Size(260, 22)
        Me.txtPalavraChave.TabIndex = 3
        Me.txtPalavraChave.Tag = "SummaryInformation - Palavras-chave"
        '
        'txtData1
        '
        Me.txtData1.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtData1.Location = New System.Drawing.Point(146, 486)
        Me.txtData1.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.txtData1.Name = "txtData1"
        Me.txtData1.Size = New System.Drawing.Size(129, 22)
        Me.txtData1.TabIndex = 10
        Me.txtData1.Tag = "Custom - data1"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(146, 463)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(46, 16)
        Me.Label9.TabIndex = 36
        Me.Label9.Text = "Data1:"
        '
        'txtDataR
        '
        Me.txtDataR.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtDataR.Location = New System.Drawing.Point(146, 426)
        Me.txtDataR.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.txtDataR.Name = "txtDataR"
        Me.txtDataR.Size = New System.Drawing.Size(129, 22)
        Me.txtDataR.TabIndex = 8
        Me.txtDataR.Tag = "Custom - datar"
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(146, 403)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(49, 16)
        Me.Label10.TabIndex = 34
        Me.Label10.Text = "DataR:"
        '
        'txtData
        '
        Me.txtData.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtData.Location = New System.Drawing.Point(146, 364)
        Me.txtData.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.txtData.Name = "txtData"
        Me.txtData.Size = New System.Drawing.Size(129, 22)
        Me.txtData.TabIndex = 6
        Me.txtData.Tag = "Custom - data"
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(146, 342)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(39, 16)
        Me.Label11.TabIndex = 32
        Me.Label11.Text = "Data:"
        '
        'txtendereco
        '
        Me.txtendereco.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtendereco.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtendereco.Enabled = False
        Me.txtendereco.Location = New System.Drawing.Point(1422, 135)
        Me.txtendereco.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.txtendereco.Name = "txtendereco"
        Me.txtendereco.Size = New System.Drawing.Size(141, 22)
        Me.txtendereco.TabIndex = 19
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(1419, 115)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(69, 16)
        Me.Label8.TabIndex = 30
        Me.Label8.Text = "Endereço:"
        '
        'txtGerente
        '
        Me.txtGerente.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtGerente.Location = New System.Drawing.Point(10, 486)
        Me.txtGerente.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.txtGerente.Name = "txtGerente"
        Me.txtGerente.Size = New System.Drawing.Size(129, 22)
        Me.txtGerente.TabIndex = 9
        Me.txtGerente.Tag = "DocumentSummaryInformation - Categoria"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(6, 463)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(135, 16)
        Me.Label7.TabIndex = 28
        Me.Label7.Text = "Gerente - Aprovação:"
        '
        'txtCategoria
        '
        Me.txtCategoria.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtCategoria.Location = New System.Drawing.Point(10, 426)
        Me.txtCategoria.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.txtCategoria.Name = "txtCategoria"
        Me.txtCategoria.Size = New System.Drawing.Size(129, 22)
        Me.txtCategoria.TabIndex = 7
        Me.txtCategoria.Tag = "DocumentSummaryInformation - Categoria"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(6, 403)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(130, 16)
        Me.Label6.TabIndex = 26
        Me.Label6.Text = "Categoria - Revisão:"
        '
        'txtAutor
        '
        Me.txtAutor.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtAutor.Location = New System.Drawing.Point(10, 364)
        Me.txtAutor.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.txtAutor.Name = "txtAutor"
        Me.txtAutor.Size = New System.Drawing.Size(129, 22)
        Me.txtAutor.TabIndex = 5
        Me.txtAutor.Tag = "SummaryInformation - Autor"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(10, 342)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(41, 16)
        Me.Label5.TabIndex = 24
        Me.Label5.Text = "Autor:"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(46, 166)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(164, 16)
        Me.Label4.TabIndex = 22
        Me.Label4.Text = "Comentarios-Acabamento"
        '
        'txtNumeroDesenho
        '
        Me.txtNumeroDesenho.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtNumeroDesenho.Enabled = False
        Me.txtNumeroDesenho.Location = New System.Drawing.Point(1110, 135)
        Me.txtNumeroDesenho.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.txtNumeroDesenho.Name = "txtNumeroDesenho"
        Me.txtNumeroDesenho.Size = New System.Drawing.Size(195, 22)
        Me.txtNumeroDesenho.TabIndex = 1
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(1110, 116)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(130, 16)
        Me.Label3.TabIndex = 20
        Me.Label3.Text = "Numero Documento:"
        '
        'txtTitulo
        '
        Me.txtTitulo.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtTitulo.Font = New System.Drawing.Font("Arial Narrow", 7.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTitulo.Location = New System.Drawing.Point(815, 135)
        Me.txtTitulo.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.txtTitulo.Name = "txtTitulo"
        Me.txtTitulo.Size = New System.Drawing.Size(289, 22)
        Me.txtTitulo.TabIndex = 0
        Me.txtTitulo.Tag = "SummaryInformation - Título"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(801, 116)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(43, 16)
        Me.Label1.TabIndex = 18
        Me.Label1.Text = "Titulo:"
        '
        'btnAbriDetalhamentoCorrente
        '
        Me.btnAbriDetalhamentoCorrente.Image = Global.SINCO_SolidEdeg_1._0.My.Resources.Resources.par
        Me.btnAbriDetalhamentoCorrente.Location = New System.Drawing.Point(396, 6)
        Me.btnAbriDetalhamentoCorrente.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.btnAbriDetalhamentoCorrente.Name = "btnAbriDetalhamentoCorrente"
        Me.btnAbriDetalhamentoCorrente.Size = New System.Drawing.Size(54, 50)
        Me.btnAbriDetalhamentoCorrente.TabIndex = 12
        Me.btnAbriDetalhamentoCorrente.Text = "Abrir DFT"
        Me.btnAbriDetalhamentoCorrente.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.ToolTipAjuda.SetToolTip(Me.btnAbriDetalhamentoCorrente, "📄 Caso haja detalhamento para a peça atual, o desenho será aberto automaticament" &
        "e.")
        Me.btnAbriDetalhamentoCorrente.UseVisualStyleBackColor = True
        '
        'btnGerarPdf
        '
        Me.btnGerarPdf.Image = Global.SINCO_SolidEdeg_1._0.My.Resources.Resources.pdf
        Me.btnGerarPdf.Location = New System.Drawing.Point(473, 6)
        Me.btnGerarPdf.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.btnGerarPdf.Name = "btnGerarPdf"
        Me.btnGerarPdf.Size = New System.Drawing.Size(135, 50)
        Me.btnGerarPdf.TabIndex = 3
        Me.btnGerarPdf.Text = "Gerar PDF"
        Me.btnGerarPdf.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.ToolTipAjuda.SetToolTip(Me.btnGerarPdf, "🧾 Clique aqui para gerar o PDF do desenho atual." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "⚠️ O PDF só será gerado se o d" &
        "etalhamento existir." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "        ")
        Me.btnGerarPdf.UseVisualStyleBackColor = True
        '
        'btnDxf
        '
        Me.btnDxf.Image = Global.SINCO_SolidEdeg_1._0.My.Resources.Resources.dxf
        Me.btnDxf.Location = New System.Drawing.Point(627, 6)
        Me.btnDxf.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.btnDxf.Name = "btnDxf"
        Me.btnDxf.Size = New System.Drawing.Size(135, 50)
        Me.btnDxf.TabIndex = 0
        Me.btnDxf.Text = "Gerar dxf"
        Me.btnDxf.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.ToolTipAjuda.SetToolTip(Me.btnDxf, "🧾 Clique aqui para gerar o DXF do desenho atual." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "⚠️ O DXF só será gerado se o d" &
        "etalhamento existir.")
        Me.btnDxf.UseVisualStyleBackColor = True
        '
        'TabPage2
        '
        Me.TabPage2.Controls.Add(Me.chkiges)
        Me.TabPage2.Controls.Add(Me.lblResumo)
        Me.TabPage2.Controls.Add(Me.chkOpcaodePasta)
        Me.TabPage2.Controls.Add(Me.Label21)
        Me.TabPage2.Controls.Add(Me.txtFiltroEmpresa)
        Me.TabPage2.Controls.Add(Me.btnInserirnaOS)
        Me.TabPage2.Controls.Add(Me.lblOrdemServicoAtiva)
        Me.TabPage2.Controls.Add(Me.btnLimparFiltro)
        Me.TabPage2.Controls.Add(Me.lblProgresso)
        Me.TabPage2.Controls.Add(Me.btnFiltrar)
        Me.TabPage2.Controls.Add(Me.ProgressBar1)
        Me.TabPage2.Controls.Add(Me.Label2)
        Me.TabPage2.Controls.Add(Me.txtFiltroCaminho1)
        Me.TabPage2.Controls.Add(Me.dgvDadosPecas)
        Me.TabPage2.Controls.Add(Me.btnCriarEstruturaProduto)
        Me.TabPage2.Controls.Add(Me.btnListaConjunto)
        Me.TabPage2.Controls.Add(Me.BtnGeraArquivos)
        Me.TabPage2.Controls.Add(Me.btnLimpar)
        Me.TabPage2.Controls.Add(Me.chkPdf)
        Me.TabPage2.Controls.Add(Me.chkdxf)
        Me.TabPage2.Location = New System.Drawing.Point(4, 25)
        Me.TabPage2.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.TabPage2.Name = "TabPage2"
        Me.TabPage2.Padding = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.TabPage2.Size = New System.Drawing.Size(1570, 749)
        Me.TabPage2.TabIndex = 1
        Me.TabPage2.Text = "Trabalhando com Lista BOM"
        Me.TabPage2.UseVisualStyleBackColor = True
        '
        'chkiges
        '
        Me.chkiges.AutoSize = True
        Me.chkiges.Location = New System.Drawing.Point(1048, 46)
        Me.chkiges.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.chkiges.Name = "chkiges"
        Me.chkiges.Size = New System.Drawing.Size(121, 20)
        Me.chkiges.TabIndex = 24
        Me.chkiges.Text = "Converter IGES"
        Me.chkiges.UseVisualStyleBackColor = True
        '
        'lblResumo
        '
        Me.lblResumo.AutoSize = True
        Me.lblResumo.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblResumo.Location = New System.Drawing.Point(1179, 13)
        Me.lblResumo.Name = "lblResumo"
        Me.lblResumo.Size = New System.Drawing.Size(44, 20)
        Me.lblResumo.TabIndex = 23
        Me.lblResumo.Text = "-----"
        Me.ToolTipAjuda.SetToolTip(Me.lblResumo, "🧾 Exibe a Ordem de Serviço atualmente selecionada.")
        '
        'chkOpcaodePasta
        '
        Me.chkOpcaodePasta.AutoSize = True
        Me.chkOpcaodePasta.Location = New System.Drawing.Point(782, 13)
        Me.chkOpcaodePasta.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.chkOpcaodePasta.Name = "chkOpcaodePasta"
        Me.chkOpcaodePasta.Size = New System.Drawing.Size(243, 20)
        Me.chkOpcaodePasta.TabIndex = 22
        Me.chkOpcaodePasta.Text = "Salvar na Pasta corrente do arquivo"
        Me.ToolTipAjuda.SetToolTip(Me.chkOpcaodePasta, "1 - Opção desmarcada: permite que o usuário escolha, separadamente, a pasta onde " &
        "serão salvos os arquivos DXF e PDF." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "2 - Opção marcada: salva cada arquivo na su" &
        "a pasta de origem.")
        Me.chkOpcaodePasta.UseVisualStyleBackColor = True
        '
        'Label21
        '
        Me.Label21.AutoSize = True
        Me.Label21.Location = New System.Drawing.Point(262, 78)
        Me.Label21.Name = "Label21"
        Me.Label21.Size = New System.Drawing.Size(65, 16)
        Me.Label21.TabIndex = 21
        Me.Label21.Text = "Empresa:"
        '
        'txtFiltroEmpresa
        '
        Me.txtFiltroEmpresa.Location = New System.Drawing.Point(262, 101)
        Me.txtFiltroEmpresa.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.txtFiltroEmpresa.Name = "txtFiltroEmpresa"
        Me.txtFiltroEmpresa.Size = New System.Drawing.Size(240, 22)
        Me.txtFiltroEmpresa.TabIndex = 20
        '
        'btnInserirnaOS
        '
        Me.btnInserirnaOS.Location = New System.Drawing.Point(309, 10)
        Me.btnInserirnaOS.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.btnInserirnaOS.Name = "btnInserirnaOS"
        Me.btnInserirnaOS.Size = New System.Drawing.Size(147, 44)
        Me.btnInserirnaOS.TabIndex = 19
        Me.btnInserirnaOS.Text = "Inserir Itens da Ordem de Serviço"
        Me.ToolTipAjuda.SetToolTip(Me.btnInserirnaOS, "📦 Após selecionar a Ordem de Serviço e garantir que todas as peças da lista este" &
        "jam " & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "devidamente cadastradas, clique aqui para inseri-las em uma Ordem de Servi" &
        "ço válida.")
        Me.btnInserirnaOS.UseVisualStyleBackColor = True
        '
        'lblOrdemServicoAtiva
        '
        Me.lblOrdemServicoAtiva.AutoSize = True
        Me.lblOrdemServicoAtiva.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblOrdemServicoAtiva.Location = New System.Drawing.Point(11, 137)
        Me.lblOrdemServicoAtiva.Name = "lblOrdemServicoAtiva"
        Me.lblOrdemServicoAtiva.Size = New System.Drawing.Size(214, 20)
        Me.lblOrdemServicoAtiva.TabIndex = 18
        Me.lblOrdemServicoAtiva.Text = "Ordem de Serviço Ativa:"
        Me.ToolTipAjuda.SetToolTip(Me.lblOrdemServicoAtiva, "🧾 Exibe a Ordem de Serviço atualmente selecionada.")
        '
        'btnLimparFiltro
        '
        Me.btnLimparFiltro.Location = New System.Drawing.Point(907, 80)
        Me.btnLimparFiltro.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.btnLimparFiltro.Name = "btnLimparFiltro"
        Me.btnLimparFiltro.Size = New System.Drawing.Size(149, 50)
        Me.btnLimparFiltro.TabIndex = 15
        Me.btnLimparFiltro.Text = "Limpar Filtro"
        Me.btnLimparFiltro.UseVisualStyleBackColor = True
        '
        'lblProgresso
        '
        Me.lblProgresso.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.lblProgresso.AutoSize = True
        Me.lblProgresso.BackColor = System.Drawing.Color.Transparent
        Me.lblProgresso.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblProgresso.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.lblProgresso.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblProgresso.Location = New System.Drawing.Point(652, 698)
        Me.lblProgresso.Name = "lblProgresso"
        Me.lblProgresso.Size = New System.Drawing.Size(43, 27)
        Me.lblProgresso.TabIndex = 16
        Me.lblProgresso.Text = "0%"
        Me.lblProgresso.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'btnFiltrar
        '
        Me.btnFiltrar.Location = New System.Drawing.Point(753, 80)
        Me.btnFiltrar.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.btnFiltrar.Name = "btnFiltrar"
        Me.btnFiltrar.Size = New System.Drawing.Size(149, 50)
        Me.btnFiltrar.TabIndex = 14
        Me.btnFiltrar.Text = "Filtrar"
        Me.btnFiltrar.UseVisualStyleBackColor = True
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(12, 78)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(63, 16)
        Me.Label2.TabIndex = 13
        Me.Label2.Text = "Caminho:"
        '
        'txtFiltroCaminho1
        '
        Me.txtFiltroCaminho1.Location = New System.Drawing.Point(11, 101)
        Me.txtFiltroCaminho1.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.txtFiltroCaminho1.Name = "txtFiltroCaminho1"
        Me.txtFiltroCaminho1.Size = New System.Drawing.Size(240, 22)
        Me.txtFiltroCaminho1.TabIndex = 12
        '
        'BtnGeraArquivos
        '
        Me.BtnGeraArquivos.Location = New System.Drawing.Point(623, 10)
        Me.BtnGeraArquivos.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.BtnGeraArquivos.Name = "BtnGeraArquivos"
        Me.BtnGeraArquivos.Size = New System.Drawing.Size(149, 44)
        Me.BtnGeraArquivos.TabIndex = 9
        Me.BtnGeraArquivos.Text = "Gerar"
        Me.ToolTipAjuda.SetToolTip(Me.BtnGeraArquivos, "📄 Selecione uma das opções laterais (PDF ou DXF) para que o SINCO gere o arquivo" &
        " " & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "automaticamente, seguindo as regras principais do sistema.")
        Me.BtnGeraArquivos.UseVisualStyleBackColor = True
        '
        'TabPage5
        '
        Me.TabPage5.Controls.Add(Me.Panel4)
        Me.TabPage5.Controls.Add(Me.Panel3)
        Me.TabPage5.Controls.Add(Me.BindingNavigator1)
        Me.TabPage5.Location = New System.Drawing.Point(4, 25)
        Me.TabPage5.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.TabPage5.Name = "TabPage5"
        Me.TabPage5.Padding = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.TabPage5.Size = New System.Drawing.Size(1570, 749)
        Me.TabPage5.TabIndex = 2
        Me.TabPage5.Text = "Ordem de Serviço"
        Me.TabPage5.UseVisualStyleBackColor = True
        '
        'Panel4
        '
        Me.Panel4.Controls.Add(Me.TabControlOS)
        Me.Panel4.Controls.Add(Me.txtPesqNumeroDesenho)
        Me.Panel4.Controls.Add(Me.Label27)
        Me.Panel4.Controls.Add(Me.cboOpcoesAcabamento)
        Me.Panel4.Controls.Add(Me.Label26)
        Me.Panel4.Controls.Add(Me.txtPesqAcabamentoDesenho)
        Me.Panel4.Controls.Add(Me.btnAplicarAcabamento)
        Me.Panel4.Controls.Add(Me.Label28)
        Me.Panel4.Controls.Add(Me.Label29)
        Me.Panel4.Controls.Add(Me.txtPesqTipoDesenho)
        Me.Panel4.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel4.Location = New System.Drawing.Point(3, 498)
        Me.Panel4.Margin = New System.Windows.Forms.Padding(4)
        Me.Panel4.Name = "Panel4"
        Me.Panel4.Size = New System.Drawing.Size(1564, 249)
        Me.Panel4.TabIndex = 39
        '
        'TabControlOS
        '
        Me.TabControlOS.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TabControlOS.Controls.Add(Me.TabPage6)
        Me.TabControlOS.Controls.Add(Me.TabPage7)
        Me.TabControlOS.Location = New System.Drawing.Point(0, 108)
        Me.TabControlOS.Margin = New System.Windows.Forms.Padding(4)
        Me.TabControlOS.Name = "TabControlOS"
        Me.TabControlOS.SelectedIndex = 0
        Me.TabControlOS.Size = New System.Drawing.Size(1561, 137)
        Me.TabControlOS.TabIndex = 24
        '
        'TabPage6
        '
        Me.TabPage6.Controls.Add(Me.DGVListaMaterialSW)
        Me.TabPage6.Location = New System.Drawing.Point(4, 25)
        Me.TabPage6.Margin = New System.Windows.Forms.Padding(4)
        Me.TabPage6.Name = "TabPage6"
        Me.TabPage6.Padding = New System.Windows.Forms.Padding(4)
        Me.TabPage6.Size = New System.Drawing.Size(1553, 108)
        Me.TabPage6.TabIndex = 0
        Me.TabPage6.Text = "Desenhos"
        Me.TabPage6.UseVisualStyleBackColor = True
        '
        'DGVListaMaterialSW
        '
        Me.DGVListaMaterialSW.AllowUserToAddRows = False
        Me.DGVListaMaterialSW.AllowUserToDeleteRows = False
        Me.DGVListaMaterialSW.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DGVListaMaterialSW.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.dgvSelecao, Me.dgvIconeItemOS, Me.dgvDXF1, Me.dgvPDF1})
        Me.DGVListaMaterialSW.ContextMenuStrip = Me.mnuDGVListaMaterialSW
        Me.DGVListaMaterialSW.Dock = System.Windows.Forms.DockStyle.Fill
        Me.DGVListaMaterialSW.Location = New System.Drawing.Point(4, 4)
        Me.DGVListaMaterialSW.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.DGVListaMaterialSW.Name = "DGVListaMaterialSW"
        Me.DGVListaMaterialSW.ReadOnly = True
        Me.DGVListaMaterialSW.RowHeadersWidth = 51
        Me.DGVListaMaterialSW.RowTemplate.Height = 24
        Me.DGVListaMaterialSW.Size = New System.Drawing.Size(1545, 100)
        Me.DGVListaMaterialSW.TabIndex = 9
        '
        'dgvSelecao
        '
        Me.dgvSelecao.Frozen = True
        Me.dgvSelecao.HeaderText = "dgvSelecao"
        Me.dgvSelecao.MinimumWidth = 25
        Me.dgvSelecao.Name = "dgvSelecao"
        Me.dgvSelecao.ReadOnly = True
        Me.dgvSelecao.Width = 25
        '
        'dgvIconeItemOS
        '
        Me.dgvIconeItemOS.Frozen = True
        Me.dgvIconeItemOS.HeaderText = "dgvIconeItemOS"
        Me.dgvIconeItemOS.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Zoom
        Me.dgvIconeItemOS.MinimumWidth = 25
        Me.dgvIconeItemOS.Name = "dgvIconeItemOS"
        Me.dgvIconeItemOS.ReadOnly = True
        Me.dgvIconeItemOS.Width = 25
        '
        'dgvDXF1
        '
        Me.dgvDXF1.HeaderText = "DXF"
        Me.dgvDXF1.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Zoom
        Me.dgvDXF1.MinimumWidth = 25
        Me.dgvDXF1.Name = "dgvDXF1"
        Me.dgvDXF1.ReadOnly = True
        Me.dgvDXF1.Width = 25
        '
        'dgvPDF1
        '
        Me.dgvPDF1.HeaderText = "PDF"
        Me.dgvPDF1.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Zoom
        Me.dgvPDF1.MinimumWidth = 25
        Me.dgvPDF1.Name = "dgvPDF1"
        Me.dgvPDF1.ReadOnly = True
        Me.dgvPDF1.Width = 25
        '
        'mnuDGVListaMaterialSW
        '
        Me.mnuDGVListaMaterialSW.ImageScalingSize = New System.Drawing.Size(20, 20)
        Me.mnuDGVListaMaterialSW.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripSeparator3, Me.MarcarTodosToolStripMenuItem, Me.DesmarcarTodosToolStripMenuItem, Me.InverterSeleçãoToolStripMenuItem, Me.ToolStripSeparator6, Me.AbrirPDFDaLinhaSelecionadaToolStripMenuItem, Me.ToolStripSeparator15, Me.ExcluirODocumentoDaLinhaSelecionadaToolStripMenuItem, Me.ToolStripSeparator7, Me.MarcarComoConjuntoPrincipalDaOrdemDeServiçoToolStripMenuItem, Me.DesmarcarComoConjuntoPrincipalToolStripMenuItem, Me.ToolStripSeparator8, Me.AlterarAQuantidadeDePeçasFabricaçãoDaLinhaSelecionadaToolStripMenuItem, Me.ToolStripSeparator9, Me.MarcarDesenhoComoRevisãoToolStripMenuItem, Me.DesmarcarDesenhoComoRevisãoToolStripMenuItem})
        Me.mnuDGVListaMaterialSW.Name = "ContextMenuStrip1"
        Me.mnuDGVListaMaterialSW.Size = New System.Drawing.Size(500, 300)
        '
        'ToolStripSeparator3
        '
        Me.ToolStripSeparator3.Name = "ToolStripSeparator3"
        Me.ToolStripSeparator3.Size = New System.Drawing.Size(496, 6)
        '
        'MarcarTodosToolStripMenuItem
        '
        Me.MarcarTodosToolStripMenuItem.Image = Global.SINCO_SolidEdeg_1._0.My.Resources.Resources.marcado
        Me.MarcarTodosToolStripMenuItem.Name = "MarcarTodosToolStripMenuItem"
        Me.MarcarTodosToolStripMenuItem.Size = New System.Drawing.Size(499, 26)
        Me.MarcarTodosToolStripMenuItem.Text = "Marcar Todos"
        '
        'DesmarcarTodosToolStripMenuItem
        '
        Me.DesmarcarTodosToolStripMenuItem.Image = Global.SINCO_SolidEdeg_1._0.My.Resources.Resources.desmarcado
        Me.DesmarcarTodosToolStripMenuItem.Name = "DesmarcarTodosToolStripMenuItem"
        Me.DesmarcarTodosToolStripMenuItem.Size = New System.Drawing.Size(499, 26)
        Me.DesmarcarTodosToolStripMenuItem.Text = "Desmarcar Todos"
        '
        'InverterSeleçãoToolStripMenuItem
        '
        Me.InverterSeleçãoToolStripMenuItem.Image = Global.SINCO_SolidEdeg_1._0.My.Resources.Resources.alterar
        Me.InverterSeleçãoToolStripMenuItem.Name = "InverterSeleçãoToolStripMenuItem"
        Me.InverterSeleçãoToolStripMenuItem.Size = New System.Drawing.Size(499, 26)
        Me.InverterSeleçãoToolStripMenuItem.Text = "Inverter Seleção"
        '
        'ToolStripSeparator6
        '
        Me.ToolStripSeparator6.Name = "ToolStripSeparator6"
        Me.ToolStripSeparator6.Size = New System.Drawing.Size(496, 6)
        '
        'AbrirPDFDaLinhaSelecionadaToolStripMenuItem
        '
        Me.AbrirPDFDaLinhaSelecionadaToolStripMenuItem.Image = Global.SINCO_SolidEdeg_1._0.My.Resources.Resources.pdf
        Me.AbrirPDFDaLinhaSelecionadaToolStripMenuItem.Name = "AbrirPDFDaLinhaSelecionadaToolStripMenuItem"
        Me.AbrirPDFDaLinhaSelecionadaToolStripMenuItem.Size = New System.Drawing.Size(499, 26)
        Me.AbrirPDFDaLinhaSelecionadaToolStripMenuItem.Text = "Abrir PDF da Linha Selecionada"
        '
        'ToolStripSeparator15
        '
        Me.ToolStripSeparator15.Name = "ToolStripSeparator15"
        Me.ToolStripSeparator15.Size = New System.Drawing.Size(496, 6)
        '
        'ExcluirODocumentoDaLinhaSelecionadaToolStripMenuItem
        '
        Me.ExcluirODocumentoDaLinhaSelecionadaToolStripMenuItem.Image = Global.SINCO_SolidEdeg_1._0.My.Resources.Resources.excluir
        Me.ExcluirODocumentoDaLinhaSelecionadaToolStripMenuItem.Name = "ExcluirODocumentoDaLinhaSelecionadaToolStripMenuItem"
        Me.ExcluirODocumentoDaLinhaSelecionadaToolStripMenuItem.Size = New System.Drawing.Size(499, 26)
        Me.ExcluirODocumentoDaLinhaSelecionadaToolStripMenuItem.Text = "Excluir o Documento da Linha Selecionada"
        '
        'ToolStripSeparator7
        '
        Me.ToolStripSeparator7.Name = "ToolStripSeparator7"
        Me.ToolStripSeparator7.Size = New System.Drawing.Size(496, 6)
        '
        'MarcarComoConjuntoPrincipalDaOrdemDeServiçoToolStripMenuItem
        '
        Me.MarcarComoConjuntoPrincipalDaOrdemDeServiçoToolStripMenuItem.Image = Global.SINCO_SolidEdeg_1._0.My.Resources.Resources.IconeswPrincipal
        Me.MarcarComoConjuntoPrincipalDaOrdemDeServiçoToolStripMenuItem.Name = "MarcarComoConjuntoPrincipalDaOrdemDeServiçoToolStripMenuItem"
        Me.MarcarComoConjuntoPrincipalDaOrdemDeServiçoToolStripMenuItem.Size = New System.Drawing.Size(499, 26)
        Me.MarcarComoConjuntoPrincipalDaOrdemDeServiçoToolStripMenuItem.Text = "Marcar como Conjunto Principal da Ordem de Serviço"
        '
        'DesmarcarComoConjuntoPrincipalToolStripMenuItem
        '
        Me.DesmarcarComoConjuntoPrincipalToolStripMenuItem.Image = Global.SINCO_SolidEdeg_1._0.My.Resources.Resources.IcopneMontagemSW
        Me.DesmarcarComoConjuntoPrincipalToolStripMenuItem.Name = "DesmarcarComoConjuntoPrincipalToolStripMenuItem"
        Me.DesmarcarComoConjuntoPrincipalToolStripMenuItem.Size = New System.Drawing.Size(499, 26)
        Me.DesmarcarComoConjuntoPrincipalToolStripMenuItem.Text = "Desmarcar Como Conjunto Principal da Ordem de Serviço"
        '
        'ToolStripSeparator8
        '
        Me.ToolStripSeparator8.Name = "ToolStripSeparator8"
        Me.ToolStripSeparator8.Size = New System.Drawing.Size(496, 6)
        '
        'AlterarAQuantidadeDePeçasFabricaçãoDaLinhaSelecionadaToolStripMenuItem
        '
        Me.AlterarAQuantidadeDePeçasFabricaçãoDaLinhaSelecionadaToolStripMenuItem.Image = Global.SINCO_SolidEdeg_1._0.My.Resources.Resources.multiply
        Me.AlterarAQuantidadeDePeçasFabricaçãoDaLinhaSelecionadaToolStripMenuItem.Name = "AlterarAQuantidadeDePeçasFabricaçãoDaLinhaSelecionadaToolStripMenuItem"
        Me.AlterarAQuantidadeDePeçasFabricaçãoDaLinhaSelecionadaToolStripMenuItem.Size = New System.Drawing.Size(499, 26)
        Me.AlterarAQuantidadeDePeçasFabricaçãoDaLinhaSelecionadaToolStripMenuItem.Text = "Alterar a quantidade de peças/Fabricação da linha selecionada"
        '
        'ToolStripSeparator9
        '
        Me.ToolStripSeparator9.Name = "ToolStripSeparator9"
        Me.ToolStripSeparator9.Size = New System.Drawing.Size(496, 6)
        '
        'MarcarDesenhoComoRevisãoToolStripMenuItem
        '
        Me.MarcarDesenhoComoRevisãoToolStripMenuItem.Image = Global.SINCO_SolidEdeg_1._0.My.Resources.Resources.NadaConsta
        Me.MarcarDesenhoComoRevisãoToolStripMenuItem.Name = "MarcarDesenhoComoRevisãoToolStripMenuItem"
        Me.MarcarDesenhoComoRevisãoToolStripMenuItem.Size = New System.Drawing.Size(499, 26)
        Me.MarcarDesenhoComoRevisãoToolStripMenuItem.Text = "Marcar Desenho como Revisão"
        '
        'DesmarcarDesenhoComoRevisãoToolStripMenuItem
        '
        Me.DesmarcarDesenhoComoRevisãoToolStripMenuItem.Name = "DesmarcarDesenhoComoRevisãoToolStripMenuItem"
        Me.DesmarcarDesenhoComoRevisãoToolStripMenuItem.Size = New System.Drawing.Size(499, 26)
        Me.DesmarcarDesenhoComoRevisãoToolStripMenuItem.Text = "Desmarcar Desenho como Revisão"
        '
        'TabPage7
        '
        Me.TabPage7.Controls.Add(Me.DGVListaMaterialSWMateriais)
        Me.TabPage7.Location = New System.Drawing.Point(4, 25)
        Me.TabPage7.Margin = New System.Windows.Forms.Padding(4)
        Me.TabPage7.Name = "TabPage7"
        Me.TabPage7.Padding = New System.Windows.Forms.Padding(4)
        Me.TabPage7.Size = New System.Drawing.Size(1553, 108)
        Me.TabPage7.TabIndex = 1
        Me.TabPage7.Text = "Materiais"
        Me.TabPage7.UseVisualStyleBackColor = True
        '
        'DGVListaMaterialSWMateriais
        '
        Me.DGVListaMaterialSWMateriais.AllowUserToAddRows = False
        Me.DGVListaMaterialSWMateriais.AllowUserToDeleteRows = False
        Me.DGVListaMaterialSWMateriais.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DGVListaMaterialSWMateriais.Dock = System.Windows.Forms.DockStyle.Fill
        Me.DGVListaMaterialSWMateriais.Location = New System.Drawing.Point(4, 4)
        Me.DGVListaMaterialSWMateriais.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.DGVListaMaterialSWMateriais.Name = "DGVListaMaterialSWMateriais"
        Me.DGVListaMaterialSWMateriais.ReadOnly = True
        Me.DGVListaMaterialSWMateriais.RowHeadersWidth = 51
        Me.DGVListaMaterialSWMateriais.RowTemplate.Height = 24
        Me.DGVListaMaterialSWMateriais.Size = New System.Drawing.Size(1545, 100)
        Me.DGVListaMaterialSWMateriais.TabIndex = 10
        '
        'txtPesqNumeroDesenho
        '
        Me.txtPesqNumeroDesenho.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtPesqNumeroDesenho.Location = New System.Drawing.Point(19, 78)
        Me.txtPesqNumeroDesenho.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.txtPesqNumeroDesenho.Name = "txtPesqNumeroDesenho"
        Me.txtPesqNumeroDesenho.Size = New System.Drawing.Size(196, 22)
        Me.txtPesqNumeroDesenho.TabIndex = 19
        '
        'Label27
        '
        Me.Label27.AutoSize = True
        Me.Label27.Location = New System.Drawing.Point(19, 55)
        Me.Label27.Name = "Label27"
        Me.Label27.Size = New System.Drawing.Size(116, 16)
        Me.Label27.TabIndex = 18
        Me.Label27.Text = "Numero Desenho:"
        '
        'cboOpcoesAcabamento
        '
        Me.cboOpcoesAcabamento.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboOpcoesAcabamento.FormattingEnabled = True
        Me.cboOpcoesAcabamento.Location = New System.Drawing.Point(108, 17)
        Me.cboOpcoesAcabamento.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.cboOpcoesAcabamento.Name = "cboOpcoesAcabamento"
        Me.cboOpcoesAcabamento.Size = New System.Drawing.Size(1045, 24)
        Me.cboOpcoesAcabamento.TabIndex = 18
        '
        'Label26
        '
        Me.Label26.AutoSize = True
        Me.Label26.Location = New System.Drawing.Point(15, 22)
        Me.Label26.Name = "Label26"
        Me.Label26.Size = New System.Drawing.Size(87, 16)
        Me.Label26.TabIndex = 17
        Me.Label26.Text = "Acabamento:"
        '
        'txtPesqAcabamentoDesenho
        '
        Me.txtPesqAcabamentoDesenho.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtPesqAcabamentoDesenho.Location = New System.Drawing.Point(443, 78)
        Me.txtPesqAcabamentoDesenho.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.txtPesqAcabamentoDesenho.Name = "txtPesqAcabamentoDesenho"
        Me.txtPesqAcabamentoDesenho.Size = New System.Drawing.Size(211, 22)
        Me.txtPesqAcabamentoDesenho.TabIndex = 23
        '
        'btnAplicarAcabamento
        '
        Me.btnAplicarAcabamento.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnAplicarAcabamento.Location = New System.Drawing.Point(1169, 14)
        Me.btnAplicarAcabamento.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.btnAplicarAcabamento.Name = "btnAplicarAcabamento"
        Me.btnAplicarAcabamento.Size = New System.Drawing.Size(139, 31)
        Me.btnAplicarAcabamento.TabIndex = 19
        Me.btnAplicarAcabamento.Text = "Aplicar"
        Me.btnAplicarAcabamento.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.btnAplicarAcabamento.UseVisualStyleBackColor = True
        '
        'Label28
        '
        Me.Label28.AutoSize = True
        Me.Label28.Location = New System.Drawing.Point(223, 55)
        Me.Label28.Name = "Label28"
        Me.Label28.Size = New System.Drawing.Size(96, 16)
        Me.Label28.TabIndex = 20
        Me.Label28.Text = "Tipo Desenho:"
        '
        'Label29
        '
        Me.Label29.AutoSize = True
        Me.Label29.Location = New System.Drawing.Point(440, 55)
        Me.Label29.Name = "Label29"
        Me.Label29.Size = New System.Drawing.Size(118, 16)
        Me.Label29.TabIndex = 22
        Me.Label29.Text = "Tipo Acabamento:"
        '
        'txtPesqTipoDesenho
        '
        Me.txtPesqTipoDesenho.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtPesqTipoDesenho.Location = New System.Drawing.Point(224, 78)
        Me.txtPesqTipoDesenho.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.txtPesqTipoDesenho.Name = "txtPesqTipoDesenho"
        Me.txtPesqTipoDesenho.Size = New System.Drawing.Size(211, 22)
        Me.txtPesqTipoDesenho.TabIndex = 21
        '
        'Panel3
        '
        Me.Panel3.Controls.Add(Me.ProgressBarOSM)
        Me.Panel3.Controls.Add(Me.Label44)
        Me.Panel3.Controls.Add(Me.txtPesoTotalKgTotal)
        Me.Panel3.Controls.Add(Me.Label45)
        Me.Panel3.Controls.Add(Me.txtAreaPintM2Total)
        Me.Panel3.Controls.Add(Me.Label46)
        Me.Panel3.Controls.Add(Me.Label43)
        Me.Panel3.Controls.Add(Me.txtPesoTotalKg)
        Me.Panel3.Controls.Add(Me.Label42)
        Me.Panel3.Controls.Add(Me.txtAreaPintM2)
        Me.Panel3.Controls.Add(Me.Label41)
        Me.Panel3.Controls.Add(Me.lbltxtSaldoTag)
        Me.Panel3.Controls.Add(Me.lbltxtQtdeLiberada)
        Me.Panel3.Controls.Add(Me.lbltxtQtdeTag)
        Me.Panel3.Controls.Add(Me.dgvos)
        Me.Panel3.Controls.Add(Me.lblFator)
        Me.Panel3.Controls.Add(Me.Label37)
        Me.Panel3.Controls.Add(Me.chkMostraLiberadasPelaEngenharia)
        Me.Panel3.Controls.Add(Me.Label38)
        Me.Panel3.Controls.Add(Me.Label36)
        Me.Panel3.Controls.Add(Me.Label30)
        Me.Panel3.Controls.Add(Me.txtPesqCriadoPor)
        Me.Panel3.Controls.Add(Me.Label35)
        Me.Panel3.Controls.Add(Me.txtDescricao)
        Me.Panel3.Controls.Add(Me.Label25)
        Me.Panel3.Controls.Add(Me.txtCliente)
        Me.Panel3.Controls.Add(Me.Label20)
        Me.Panel3.Controls.Add(Me.cboProjeto)
        Me.Panel3.Controls.Add(Me.Label24)
        Me.Panel3.Controls.Add(Me.cboTag)
        Me.Panel3.Controls.Add(Me.txtDescricaoTag)
        Me.Panel3.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel3.Location = New System.Drawing.Point(3, 65)
        Me.Panel3.Margin = New System.Windows.Forms.Padding(4)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(1564, 433)
        Me.Panel3.TabIndex = 38
        '
        'ProgressBarOSM
        '
        Me.ProgressBarOSM.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ProgressBarOSM.ForeColor = System.Drawing.Color.Green
        Me.ProgressBarOSM.Location = New System.Drawing.Point(5, 400)
        Me.ProgressBarOSM.Name = "ProgressBarOSM"
        Me.ProgressBarOSM.Size = New System.Drawing.Size(1552, 23)
        Me.ProgressBarOSM.TabIndex = 50
        '
        'Label44
        '
        Me.Label44.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label44.AutoSize = True
        Me.Label44.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.8!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Italic), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label44.Location = New System.Drawing.Point(1412, 109)
        Me.Label44.Name = "Label44"
        Me.Label44.Size = New System.Drawing.Size(95, 16)
        Me.Label44.TabIndex = 49
        Me.Label44.Text = "Dados totais"
        '
        'txtPesoTotalKgTotal
        '
        Me.txtPesoTotalKgTotal.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtPesoTotalKgTotal.Enabled = False
        Me.txtPesoTotalKgTotal.Location = New System.Drawing.Point(1450, 164)
        Me.txtPesoTotalKgTotal.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.txtPesoTotalKgTotal.Name = "txtPesoTotalKgTotal"
        Me.txtPesoTotalKgTotal.Size = New System.Drawing.Size(89, 22)
        Me.txtPesoTotalKgTotal.TabIndex = 48
        '
        'Label45
        '
        Me.Label45.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label45.AutoSize = True
        Me.Label45.Location = New System.Drawing.Point(1383, 167)
        Me.Label45.Name = "Label45"
        Me.Label45.Size = New System.Drawing.Size(61, 16)
        Me.Label45.TabIndex = 47
        Me.Label45.Text = "Peso Kg:"
        '
        'txtAreaPintM2Total
        '
        Me.txtAreaPintM2Total.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtAreaPintM2Total.Enabled = False
        Me.txtAreaPintM2Total.Location = New System.Drawing.Point(1450, 132)
        Me.txtAreaPintM2Total.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.txtAreaPintM2Total.Name = "txtAreaPintM2Total"
        Me.txtAreaPintM2Total.Size = New System.Drawing.Size(89, 22)
        Me.txtAreaPintM2Total.TabIndex = 46
        '
        'Label46
        '
        Me.Label46.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label46.AutoSize = True
        Me.Label46.Location = New System.Drawing.Point(1359, 135)
        Me.Label46.Name = "Label46"
        Me.Label46.Size = New System.Drawing.Size(85, 16)
        Me.Label46.TabIndex = 45
        Me.Label46.Text = "Area Pint. m²:"
        '
        'Label43
        '
        Me.Label43.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label43.AutoSize = True
        Me.Label43.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.8!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Italic), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label43.Location = New System.Drawing.Point(1366, 10)
        Me.Label43.Name = "Label43"
        Me.Label43.Size = New System.Drawing.Size(175, 16)
        Me.Label43.TabIndex = 44
        Me.Label43.Text = "Dados por Equipamento"
        '
        'txtPesoTotalKg
        '
        Me.txtPesoTotalKg.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtPesoTotalKg.Enabled = False
        Me.txtPesoTotalKg.Location = New System.Drawing.Point(1452, 68)
        Me.txtPesoTotalKg.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.txtPesoTotalKg.Name = "txtPesoTotalKg"
        Me.txtPesoTotalKg.Size = New System.Drawing.Size(89, 22)
        Me.txtPesoTotalKg.TabIndex = 43
        '
        'Label42
        '
        Me.Label42.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label42.AutoSize = True
        Me.Label42.Location = New System.Drawing.Point(1385, 71)
        Me.Label42.Name = "Label42"
        Me.Label42.Size = New System.Drawing.Size(61, 16)
        Me.Label42.TabIndex = 42
        Me.Label42.Text = "Peso Kg:"
        '
        'txtAreaPintM2
        '
        Me.txtAreaPintM2.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtAreaPintM2.Enabled = False
        Me.txtAreaPintM2.Location = New System.Drawing.Point(1452, 36)
        Me.txtAreaPintM2.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.txtAreaPintM2.Name = "txtAreaPintM2"
        Me.txtAreaPintM2.Size = New System.Drawing.Size(89, 22)
        Me.txtAreaPintM2.TabIndex = 41
        '
        'Label41
        '
        Me.Label41.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label41.AutoSize = True
        Me.Label41.Location = New System.Drawing.Point(1361, 39)
        Me.Label41.Name = "Label41"
        Me.Label41.Size = New System.Drawing.Size(85, 16)
        Me.Label41.TabIndex = 40
        Me.Label41.Text = "Area Pint. m²:"
        '
        'lbltxtSaldoTag
        '
        Me.lbltxtSaldoTag.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lbltxtSaldoTag.BackColor = System.Drawing.Color.White
        Me.lbltxtSaldoTag.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lbltxtSaldoTag.Location = New System.Drawing.Point(1268, 82)
        Me.lbltxtSaldoTag.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lbltxtSaldoTag.Name = "lbltxtSaldoTag"
        Me.lbltxtSaldoTag.Size = New System.Drawing.Size(80, 23)
        Me.lbltxtSaldoTag.TabIndex = 39
        Me.lbltxtSaldoTag.Text = "0"
        Me.lbltxtSaldoTag.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lbltxtQtdeLiberada
        '
        Me.lbltxtQtdeLiberada.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lbltxtQtdeLiberada.BackColor = System.Drawing.Color.White
        Me.lbltxtQtdeLiberada.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lbltxtQtdeLiberada.Location = New System.Drawing.Point(1268, 43)
        Me.lbltxtQtdeLiberada.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lbltxtQtdeLiberada.Name = "lbltxtQtdeLiberada"
        Me.lbltxtQtdeLiberada.Size = New System.Drawing.Size(80, 23)
        Me.lbltxtQtdeLiberada.TabIndex = 38
        Me.lbltxtQtdeLiberada.Text = "0"
        Me.lbltxtQtdeLiberada.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lbltxtQtdeTag
        '
        Me.lbltxtQtdeTag.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lbltxtQtdeTag.BackColor = System.Drawing.Color.White
        Me.lbltxtQtdeTag.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lbltxtQtdeTag.Location = New System.Drawing.Point(1268, 7)
        Me.lbltxtQtdeTag.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lbltxtQtdeTag.Name = "lbltxtQtdeTag"
        Me.lbltxtQtdeTag.Size = New System.Drawing.Size(80, 23)
        Me.lbltxtQtdeTag.TabIndex = 37
        Me.lbltxtQtdeTag.Text = "0"
        Me.lbltxtQtdeTag.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'dgvos
        '
        Me.dgvos.AllowUserToAddRows = False
        Me.dgvos.AllowUserToDeleteRows = False
        Me.dgvos.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dgvos.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCellsExceptHeader
        Me.dgvos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvos.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.dgvStatus})
        Me.dgvos.ContextMenuStrip = Me.mnudgvos
        Me.dgvos.Location = New System.Drawing.Point(5, 212)
        Me.dgvos.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.dgvos.Name = "dgvos"
        Me.dgvos.ReadOnly = True
        Me.dgvos.RowHeadersWidth = 51
        Me.dgvos.RowTemplate.Height = 24
        Me.dgvos.Size = New System.Drawing.Size(1556, 177)
        Me.dgvos.TabIndex = 7
        '
        'dgvStatus
        '
        Me.dgvStatus.Frozen = True
        Me.dgvStatus.HeaderText = "Status"
        Me.dgvStatus.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Zoom
        Me.dgvStatus.MinimumWidth = 6
        Me.dgvStatus.Name = "dgvStatus"
        Me.dgvStatus.ReadOnly = True
        Me.dgvStatus.Width = 6
        '
        'mnudgvos
        '
        Me.mnudgvos.ImageScalingSize = New System.Drawing.Size(20, 20)
        Me.mnudgvos.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.AbrirPastaDaOrdemDeServiçoToolStripMenuItem, Me.ToolStripSeparator10, Me.LiberarOrdemDeServiçoParaProduçãoToolStripMenuItem, Me.CancelarLiberaçãoDaOSToolStripMenuItem, Me.ToolStripSeparator11, Me.AtualizarPDFsEDXFsNaPastaDaOSMToolStripMenuItem, Me.ToolStripSeparator12, Me.AlterarOFatorMultipçlicadorDaOSToolStripMenuItem, Me.ToolStripSeparator14, Me.GeralExcelDaOSToolStripMenuItem, Me.ToolStripSeparator13, Me.LimparPastaOrdemDeServiçoSelecionadaToolStripMenuItem, Me.ToolStripSeparator17, Me.CancelarAFabricaçãoDaOSToolStripMenuItem})
        Me.mnudgvos.Name = "mnudgvos"
        Me.mnudgvos.Size = New System.Drawing.Size(432, 248)
        '
        'AbrirPastaDaOrdemDeServiçoToolStripMenuItem
        '
        Me.AbrirPastaDaOrdemDeServiçoToolStripMenuItem.Image = Global.SINCO_SolidEdeg_1._0.My.Resources.Resources.pasta
        Me.AbrirPastaDaOrdemDeServiçoToolStripMenuItem.Name = "AbrirPastaDaOrdemDeServiçoToolStripMenuItem"
        Me.AbrirPastaDaOrdemDeServiçoToolStripMenuItem.Size = New System.Drawing.Size(431, 26)
        Me.AbrirPastaDaOrdemDeServiçoToolStripMenuItem.Text = "Abrir Pasta da Ordem de Serviço"
        Me.AbrirPastaDaOrdemDeServiçoToolStripMenuItem.ToolTipText = "Abre a Pasta da OS Selecionada!"
        '
        'ToolStripSeparator10
        '
        Me.ToolStripSeparator10.Name = "ToolStripSeparator10"
        Me.ToolStripSeparator10.Size = New System.Drawing.Size(428, 6)
        '
        'LiberarOrdemDeServiçoParaProduçãoToolStripMenuItem
        '
        Me.LiberarOrdemDeServiçoParaProduçãoToolStripMenuItem.Image = Global.SINCO_SolidEdeg_1._0.My.Resources.Resources.verificado
        Me.LiberarOrdemDeServiçoParaProduçãoToolStripMenuItem.Name = "LiberarOrdemDeServiçoParaProduçãoToolStripMenuItem"
        Me.LiberarOrdemDeServiçoParaProduçãoToolStripMenuItem.Size = New System.Drawing.Size(431, 26)
        Me.LiberarOrdemDeServiçoParaProduçãoToolStripMenuItem.Text = "Liberar Ordem de Serviço para Produção"
        Me.LiberarOrdemDeServiçoParaProduçãoToolStripMenuItem.ToolTipText = "Aqui você libera a OS para o processo de fabricação pelo SINCO e inportas os arqu" &
    "ivo em PDF's e DXF's para as pastas da OS."
        '
        'CancelarLiberaçãoDaOSToolStripMenuItem
        '
        Me.CancelarLiberaçãoDaOSToolStripMenuItem.Image = Global.SINCO_SolidEdeg_1._0.My.Resources.Resources.cancelar
        Me.CancelarLiberaçãoDaOSToolStripMenuItem.Name = "CancelarLiberaçãoDaOSToolStripMenuItem"
        Me.CancelarLiberaçãoDaOSToolStripMenuItem.Size = New System.Drawing.Size(431, 26)
        Me.CancelarLiberaçãoDaOSToolStripMenuItem.Text = "Cancelar Liberação da Ordem de Serviço"
        '
        'ToolStripSeparator11
        '
        Me.ToolStripSeparator11.Name = "ToolStripSeparator11"
        Me.ToolStripSeparator11.Size = New System.Drawing.Size(428, 6)
        '
        'AtualizarPDFsEDXFsNaPastaDaOSMToolStripMenuItem
        '
        Me.AtualizarPDFsEDXFsNaPastaDaOSMToolStripMenuItem.Image = Global.SINCO_SolidEdeg_1._0.My.Resources.Resources.atualizar
        Me.AtualizarPDFsEDXFsNaPastaDaOSMToolStripMenuItem.Name = "AtualizarPDFsEDXFsNaPastaDaOSMToolStripMenuItem"
        Me.AtualizarPDFsEDXFsNaPastaDaOSMToolStripMenuItem.Size = New System.Drawing.Size(431, 26)
        Me.AtualizarPDFsEDXFsNaPastaDaOSMToolStripMenuItem.Text = "Atualizar PDF's, DXF's/LXDS's e DTF's  na pasta da OS"
        Me.AtualizarPDFsEDXFsNaPastaDaOSMToolStripMenuItem.ToolTipText = "Atenção: Esta função exclui todos os arquivos da OS e os atualizas com os documen" &
    "to da lista de materiais da OS."
        '
        'ToolStripSeparator12
        '
        Me.ToolStripSeparator12.Name = "ToolStripSeparator12"
        Me.ToolStripSeparator12.Size = New System.Drawing.Size(428, 6)
        '
        'AlterarOFatorMultipçlicadorDaOSToolStripMenuItem
        '
        Me.AlterarOFatorMultipçlicadorDaOSToolStripMenuItem.Image = Global.SINCO_SolidEdeg_1._0.My.Resources.Resources.multiply
        Me.AlterarOFatorMultipçlicadorDaOSToolStripMenuItem.Name = "AlterarOFatorMultipçlicadorDaOSToolStripMenuItem"
        Me.AlterarOFatorMultipçlicadorDaOSToolStripMenuItem.Size = New System.Drawing.Size(431, 26)
        Me.AlterarOFatorMultipçlicadorDaOSToolStripMenuItem.Text = "Alterar o Fator Multiplicador da OS"
        Me.AlterarOFatorMultipçlicadorDaOSToolStripMenuItem.ToolTipText = "A alteração do Fator multiplicador irá ajustar todas as quantidades de totas as p" &
    "eças da OS, é com isso irá atualizar todos os arquivos da pasta da OS."
        '
        'ToolStripSeparator14
        '
        Me.ToolStripSeparator14.Name = "ToolStripSeparator14"
        Me.ToolStripSeparator14.Size = New System.Drawing.Size(428, 6)
        '
        'GeralExcelDaOSToolStripMenuItem
        '
        Me.GeralExcelDaOSToolStripMenuItem.Image = Global.SINCO_SolidEdeg_1._0.My.Resources.Resources.icons8_exportação_excel_48
        Me.GeralExcelDaOSToolStripMenuItem.Name = "GeralExcelDaOSToolStripMenuItem"
        Me.GeralExcelDaOSToolStripMenuItem.Size = New System.Drawing.Size(431, 26)
        Me.GeralExcelDaOSToolStripMenuItem.Text = "Gerar Excel da OS sem Liberação para  Produção"
        '
        'ToolStripSeparator13
        '
        Me.ToolStripSeparator13.Name = "ToolStripSeparator13"
        Me.ToolStripSeparator13.Size = New System.Drawing.Size(428, 6)
        '
        'LimparPastaOrdemDeServiçoSelecionadaToolStripMenuItem
        '
        Me.LimparPastaOrdemDeServiçoSelecionadaToolStripMenuItem.Image = Global.SINCO_SolidEdeg_1._0.My.Resources.Resources.Limpar5
        Me.LimparPastaOrdemDeServiçoSelecionadaToolStripMenuItem.Name = "LimparPastaOrdemDeServiçoSelecionadaToolStripMenuItem"
        Me.LimparPastaOrdemDeServiçoSelecionadaToolStripMenuItem.Size = New System.Drawing.Size(431, 26)
        Me.LimparPastaOrdemDeServiçoSelecionadaToolStripMenuItem.Text = "Limpar OS"
        Me.LimparPastaOrdemDeServiçoSelecionadaToolStripMenuItem.ToolTipText = "Esta função limpar todos os desenhos da OS e excluir todos os arquivos de desenho" &
    "s da pasta da OS."
        '
        'ToolStripSeparator17
        '
        Me.ToolStripSeparator17.Name = "ToolStripSeparator17"
        Me.ToolStripSeparator17.Size = New System.Drawing.Size(428, 6)
        '
        'CancelarAFabricaçãoDaOSToolStripMenuItem
        '
        Me.CancelarAFabricaçãoDaOSToolStripMenuItem.Image = Global.SINCO_SolidEdeg_1._0.My.Resources.Resources.excluir
        Me.CancelarAFabricaçãoDaOSToolStripMenuItem.Name = "CancelarAFabricaçãoDaOSToolStripMenuItem"
        Me.CancelarAFabricaçãoDaOSToolStripMenuItem.Size = New System.Drawing.Size(431, 26)
        Me.CancelarAFabricaçãoDaOSToolStripMenuItem.Text = "Excluir Ordem de Serviço"
        Me.CancelarAFabricaçãoDaOSToolStripMenuItem.ToolTipText = "Cancela todos o processo de produção da OS."
        '
        'lblFator
        '
        Me.lblFator.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.lblFator.Enabled = False
        Me.lblFator.Location = New System.Drawing.Point(323, 183)
        Me.lblFator.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.lblFator.Name = "lblFator"
        Me.lblFator.Size = New System.Drawing.Size(49, 22)
        Me.lblFator.TabIndex = 36
        '
        'Label37
        '
        Me.Label37.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label37.AutoSize = True
        Me.Label37.Location = New System.Drawing.Point(1220, 85)
        Me.Label37.Name = "Label37"
        Me.Label37.Size = New System.Drawing.Size(46, 16)
        Me.Label37.TabIndex = 33
        Me.Label37.Text = "Saldo:"
        '
        'chkMostraLiberadasPelaEngenharia
        '
        Me.chkMostraLiberadasPelaEngenharia.AutoSize = True
        Me.chkMostraLiberadasPelaEngenharia.Location = New System.Drawing.Point(8, 185)
        Me.chkMostraLiberadasPelaEngenharia.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.chkMostraLiberadasPelaEngenharia.Name = "chkMostraLiberadasPelaEngenharia"
        Me.chkMostraLiberadasPelaEngenharia.Size = New System.Drawing.Size(251, 20)
        Me.chkMostraLiberadasPelaEngenharia.TabIndex = 8
        Me.chkMostraLiberadasPelaEngenharia.Text = "Mostar OS Liberada pela Engenharia"
        Me.chkMostraLiberadasPelaEngenharia.UseVisualStyleBackColor = True
        '
        'Label38
        '
        Me.Label38.AutoSize = True
        Me.Label38.Location = New System.Drawing.Point(275, 186)
        Me.Label38.Name = "Label38"
        Me.Label38.Size = New System.Drawing.Size(41, 16)
        Me.Label38.TabIndex = 35
        Me.Label38.Text = "Fator:"
        '
        'Label36
        '
        Me.Label36.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label36.AutoSize = True
        Me.Label36.Location = New System.Drawing.Point(1201, 47)
        Me.Label36.Name = "Label36"
        Me.Label36.Size = New System.Drawing.Size(64, 16)
        Me.Label36.TabIndex = 31
        Me.Label36.Text = "Liberada:"
        '
        'Label30
        '
        Me.Label30.AutoSize = True
        Me.Label30.Location = New System.Drawing.Point(460, 186)
        Me.Label30.Name = "Label30"
        Me.Label30.Size = New System.Drawing.Size(74, 16)
        Me.Label30.TabIndex = 24
        Me.Label30.Text = "Criado Por:"
        '
        'txtPesqCriadoPor
        '
        Me.txtPesqCriadoPor.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtPesqCriadoPor.Location = New System.Drawing.Point(544, 183)
        Me.txtPesqCriadoPor.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.txtPesqCriadoPor.Name = "txtPesqCriadoPor"
        Me.txtPesqCriadoPor.Size = New System.Drawing.Size(168, 22)
        Me.txtPesqCriadoPor.TabIndex = 25
        '
        'Label35
        '
        Me.Label35.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label35.AutoSize = True
        Me.Label35.Location = New System.Drawing.Point(1225, 9)
        Me.Label35.Name = "Label35"
        Me.Label35.Size = New System.Drawing.Size(39, 16)
        Me.Label35.TabIndex = 29
        Me.Label35.Text = "Qtde:"
        '
        'txtDescricao
        '
        Me.txtDescricao.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtDescricao.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtDescricao.Location = New System.Drawing.Point(93, 106)
        Me.txtDescricao.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.txtDescricao.MaxLength = 200
        Me.txtDescricao.Multiline = True
        Me.txtDescricao.Name = "txtDescricao"
        Me.txtDescricao.Size = New System.Drawing.Size(1103, 61)
        Me.txtDescricao.TabIndex = 13
        '
        'Label25
        '
        Me.Label25.AutoSize = True
        Me.Label25.Location = New System.Drawing.Point(19, 110)
        Me.Label25.Name = "Label25"
        Me.Label25.Size = New System.Drawing.Size(72, 16)
        Me.Label25.TabIndex = 12
        Me.Label25.Text = "Descrição:"
        '
        'txtCliente
        '
        Me.txtCliente.Enabled = False
        Me.txtCliente.Location = New System.Drawing.Point(93, 39)
        Me.txtCliente.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.txtCliente.Multiline = True
        Me.txtCliente.Name = "txtCliente"
        Me.txtCliente.Size = New System.Drawing.Size(493, 61)
        Me.txtCliente.TabIndex = 5
        '
        'Label20
        '
        Me.Label20.AutoSize = True
        Me.Label20.Location = New System.Drawing.Point(37, 14)
        Me.Label20.Name = "Label20"
        Me.Label20.Size = New System.Drawing.Size(53, 16)
        Me.Label20.TabIndex = 1
        Me.Label20.Text = "Projeto:"
        '
        'cboProjeto
        '
        Me.cboProjeto.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.cboProjeto.FormattingEnabled = True
        Me.cboProjeto.Location = New System.Drawing.Point(93, 10)
        Me.cboProjeto.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.cboProjeto.Name = "cboProjeto"
        Me.cboProjeto.Size = New System.Drawing.Size(493, 24)
        Me.cboProjeto.TabIndex = 2
        '
        'Label24
        '
        Me.Label24.AutoSize = True
        Me.Label24.Location = New System.Drawing.Point(667, 10)
        Me.Label24.Name = "Label24"
        Me.Label24.Size = New System.Drawing.Size(35, 16)
        Me.Label24.TabIndex = 3
        Me.Label24.Text = "Tag:"
        '
        'cboTag
        '
        Me.cboTag.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboTag.FormattingEnabled = True
        Me.cboTag.Location = New System.Drawing.Point(708, 7)
        Me.cboTag.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.cboTag.Name = "cboTag"
        Me.cboTag.Size = New System.Drawing.Size(488, 24)
        Me.cboTag.TabIndex = 4
        '
        'txtDescricaoTag
        '
        Me.txtDescricaoTag.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtDescricaoTag.Enabled = False
        Me.txtDescricaoTag.Location = New System.Drawing.Point(708, 36)
        Me.txtDescricaoTag.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.txtDescricaoTag.Multiline = True
        Me.txtDescricaoTag.Name = "txtDescricaoTag"
        Me.txtDescricaoTag.Size = New System.Drawing.Size(488, 61)
        Me.txtDescricaoTag.TabIndex = 6
        '
        'BindingNavigator1
        '
        Me.BindingNavigator1.AddNewItem = Nothing
        Me.BindingNavigator1.AutoSize = False
        Me.BindingNavigator1.CountItem = Nothing
        Me.BindingNavigator1.DeleteItem = Nothing
        Me.BindingNavigator1.ImageScalingSize = New System.Drawing.Size(30, 30)
        Me.BindingNavigator1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripButton10, Me.ToolStripButton1, Me.TSBSalvarOrdemServico, Me.ToolStripButton3, Me.ProgressBarProcessoLiberacaoOrdemServico, Me.ToolStripSeparator29, Me.ToolStripTextBox1})
        Me.BindingNavigator1.Location = New System.Drawing.Point(3, 2)
        Me.BindingNavigator1.MoveFirstItem = Nothing
        Me.BindingNavigator1.MoveLastItem = Nothing
        Me.BindingNavigator1.MoveNextItem = Nothing
        Me.BindingNavigator1.MovePreviousItem = Nothing
        Me.BindingNavigator1.Name = "BindingNavigator1"
        Me.BindingNavigator1.PositionItem = Nothing
        Me.BindingNavigator1.Size = New System.Drawing.Size(1564, 63)
        Me.BindingNavigator1.TabIndex = 29
        Me.BindingNavigator1.Text = "BindingNavigator1"
        '
        'ToolStripButton10
        '
        Me.ToolStripButton10.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.ToolStripButton10.Image = Global.SINCO_SolidEdeg_1._0.My.Resources.Resources.Ajuda
        Me.ToolStripButton10.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ToolStripButton10.Name = "ToolStripButton10"
        Me.ToolStripButton10.Size = New System.Drawing.Size(34, 60)
        Me.ToolStripButton10.Text = "Trabalhando com a BOM"
        Me.ToolStripButton10.ToolTipText = "Video de Treinamento da BOM"
        '
        'ToolStripButton1
        '
        Me.ToolStripButton1.CheckOnClick = True
        Me.ToolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.ToolStripButton1.Image = Global.SINCO_SolidEdeg_1._0.My.Resources.Resources.novo_arquivo
        Me.ToolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ToolStripButton1.Name = "ToolStripButton1"
        Me.ToolStripButton1.Size = New System.Drawing.Size(34, 60)
        Me.ToolStripButton1.Text = "Nova OS"
        Me.ToolStripButton1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.ToolStripButton1.ToolTipText = "Dicas de uso:" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Clique aqui para criar uma nova Ordem de Serviço (OS)."
        '
        'TSBSalvarOrdemServico
        '
        Me.TSBSalvarOrdemServico.CheckOnClick = True
        Me.TSBSalvarOrdemServico.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.TSBSalvarOrdemServico.Image = Global.SINCO_SolidEdeg_1._0.My.Resources.Resources.sinco_salva___Copia
        Me.TSBSalvarOrdemServico.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.TSBSalvarOrdemServico.Name = "TSBSalvarOrdemServico"
        Me.TSBSalvarOrdemServico.Size = New System.Drawing.Size(34, 60)
        Me.TSBSalvarOrdemServico.Text = "Salvar"
        Me.TSBSalvarOrdemServico.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.TSBSalvarOrdemServico.ToolTipText = "Dicas de uso:" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Após selecionar o Projeto e a Tag, clique aqui para salvar uma nov" &
    "a Ordem de Serviço (OS)."
        '
        'ToolStripButton3
        '
        Me.ToolStripButton3.CheckOnClick = True
        Me.ToolStripButton3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.ToolStripButton3.Image = Global.SINCO_SolidEdeg_1._0.My.Resources.Resources.Atualizar___Copia
        Me.ToolStripButton3.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ToolStripButton3.Name = "ToolStripButton3"
        Me.ToolStripButton3.Size = New System.Drawing.Size(34, 60)
        Me.ToolStripButton3.Text = "Atualizar os dados que são recebidos do SINCO."
        Me.ToolStripButton3.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.ToolStripButton3.ToolTipText = "Dicas de uso:" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Caso um novo cadastro de Projeto e/ou Tag seja inserido e não " & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "ap" &
    "areça no ComboBox, basta clicar aqui para atualizar os dados."
        '
        'ProgressBarProcessoLiberacaoOrdemServico
        '
        Me.ProgressBarProcessoLiberacaoOrdemServico.AutoSize = False
        Me.ProgressBarProcessoLiberacaoOrdemServico.BackColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.ProgressBarProcessoLiberacaoOrdemServico.ForeColor = System.Drawing.Color.Green
        Me.ProgressBarProcessoLiberacaoOrdemServico.Name = "ProgressBarProcessoLiberacaoOrdemServico"
        Me.ProgressBarProcessoLiberacaoOrdemServico.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.ProgressBarProcessoLiberacaoOrdemServico.RightToLeftLayout = True
        Me.ProgressBarProcessoLiberacaoOrdemServico.Size = New System.Drawing.Size(133, 25)
        Me.ProgressBarProcessoLiberacaoOrdemServico.Style = System.Windows.Forms.ProgressBarStyle.Continuous
        '
        'ToolStripSeparator29
        '
        Me.ToolStripSeparator29.Name = "ToolStripSeparator29"
        Me.ToolStripSeparator29.Size = New System.Drawing.Size(6, 63)
        '
        'ToolStripTextBox1
        '
        Me.ToolStripTextBox1.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest
        Me.ToolStripTextBox1.AutoSize = False
        Me.ToolStripTextBox1.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.ToolStripTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.ToolStripTextBox1.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.ToolStripTextBox1.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.ToolStripTextBox1.MaxLength = 10
        Me.ToolStripTextBox1.Name = "ToolStripTextBox1"
        Me.ToolStripTextBox1.Size = New System.Drawing.Size(125, 27)
        Me.ToolStripTextBox1.ToolTipText = "Digite aqui o numero do projeto para Facilitar a localização no Combox do Projeto" &
    ""
        '
        'TabPagePDF
        '
        Me.TabPagePDF.Controls.Add(Me.Button7)
        Me.TabPagePDF.Controls.Add(Me.dgvDadosPdf)
        Me.TabPagePDF.Controls.Add(Me.RichTextBox1)
        Me.TabPagePDF.Controls.Add(Me.Button6)
        Me.TabPagePDF.Location = New System.Drawing.Point(4, 25)
        Me.TabPagePDF.Name = "TabPagePDF"
        Me.TabPagePDF.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPagePDF.Size = New System.Drawing.Size(1570, 749)
        Me.TabPagePDF.TabIndex = 3
        Me.TabPagePDF.Text = "PDF"
        Me.TabPagePDF.UseVisualStyleBackColor = True
        '
        'Button7
        '
        Me.Button7.Image = Global.SINCO_SolidEdeg_1._0.My.Resources.Resources.novo_arquivo
        Me.Button7.Location = New System.Drawing.Point(166, 13)
        Me.Button7.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.Button7.Name = "Button7"
        Me.Button7.Size = New System.Drawing.Size(149, 50)
        Me.Button7.TabIndex = 7
        Me.Button7.Text = "Carregar no Cadastro"
        Me.Button7.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.ToolTipAjuda.SetToolTip(Me.Button7, "🧾 Clique aqui para gerar o PDF do desenho atual." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "⚠️ O PDF só será gerado se o d" &
        "etalhamento existir." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "        ")
        Me.Button7.UseVisualStyleBackColor = True
        '
        'dgvDadosPdf
        '
        Me.dgvDadosPdf.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dgvDadosPdf.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvDadosPdf.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.dgvDadosDesenhoCampoDados, Me.dgvDadosSelecionado})
        Me.dgvDadosPdf.Location = New System.Drawing.Point(6, 75)
        Me.dgvDadosPdf.Name = "dgvDadosPdf"
        Me.dgvDadosPdf.RowHeadersWidth = 51
        Me.dgvDadosPdf.RowTemplate.Height = 24
        Me.dgvDadosPdf.Size = New System.Drawing.Size(1558, 430)
        Me.dgvDadosPdf.TabIndex = 6
        '
        'dgvDadosDesenhoCampoDados
        '
        Me.dgvDadosDesenhoCampoDados.Frozen = True
        Me.dgvDadosDesenhoCampoDados.HeaderText = "Dados Desenho para Cadastro"
        Me.dgvDadosDesenhoCampoDados.MinimumWidth = 6
        Me.dgvDadosDesenhoCampoDados.Name = "dgvDadosDesenhoCampoDados"
        Me.dgvDadosDesenhoCampoDados.ReadOnly = True
        Me.dgvDadosDesenhoCampoDados.Width = 300
        '
        'dgvDadosSelecionado
        '
        Me.dgvDadosSelecionado.HeaderText = "dgvDadosSelecionado"
        Me.dgvDadosSelecionado.MinimumWidth = 6
        Me.dgvDadosSelecionado.Name = "dgvDadosSelecionado"
        Me.dgvDadosSelecionado.Width = 300
        '
        'RichTextBox1
        '
        Me.RichTextBox1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.RichTextBox1.Location = New System.Drawing.Point(6, 521)
        Me.RichTextBox1.Name = "RichTextBox1"
        Me.RichTextBox1.Size = New System.Drawing.Size(1558, 214)
        Me.RichTextBox1.TabIndex = 5
        Me.RichTextBox1.Text = ""
        '
        'Button6
        '
        Me.Button6.Image = Global.SINCO_SolidEdeg_1._0.My.Resources.Resources.pdf
        Me.Button6.Location = New System.Drawing.Point(11, 13)
        Me.Button6.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.Button6.Name = "Button6"
        Me.Button6.Size = New System.Drawing.Size(149, 50)
        Me.Button6.TabIndex = 4
        Me.Button6.Text = "Bucar Arquivos PDF"
        Me.Button6.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.ToolTipAjuda.SetToolTip(Me.Button6, "🧾 Clique aqui para gerar o PDF do desenho atual." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "⚠️ O PDF só será gerado se o d" &
        "etalhamento existir." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "        ")
        Me.Button6.UseVisualStyleBackColor = True
        '
        'TimerDGVListaMaterialSW
        '
        '
        'Timerdgvos
        '
        '
        'TimerdgvProcessoMaterial
        '
        '
        'TimerdgvProcesso
        '
        '
        'TimerbtnMsg
        '
        Me.TimerbtnMsg.Interval = 50
        '
        'TimerdgvMateriaisProtheus
        '
        '
        'TimerManufaturada
        '
        '
        'TimerdgvGabaritos
        '
        '
        'DataGridViewImageColumn1
        '
        Me.DataGridViewImageColumn1.FillWeight = 25.0!
        Me.DataGridViewImageColumn1.Frozen = True
        Me.DataGridViewImageColumn1.HeaderText = "dxf"
        Me.DataGridViewImageColumn1.Image = Global.SINCO_SolidEdeg_1._0.My.Resources.Resources.sem_icone
        Me.DataGridViewImageColumn1.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Zoom
        Me.DataGridViewImageColumn1.MinimumWidth = 6
        Me.DataGridViewImageColumn1.Name = "DataGridViewImageColumn1"
        Me.DataGridViewImageColumn1.ReadOnly = True
        Me.DataGridViewImageColumn1.Width = 25
        '
        'DataGridViewImageColumn2
        '
        Me.DataGridViewImageColumn2.FillWeight = 25.0!
        Me.DataGridViewImageColumn2.Frozen = True
        Me.DataGridViewImageColumn2.HeaderText = "pdf"
        Me.DataGridViewImageColumn2.Image = Global.SINCO_SolidEdeg_1._0.My.Resources.Resources.sem_icone
        Me.DataGridViewImageColumn2.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Zoom
        Me.DataGridViewImageColumn2.MinimumWidth = 6
        Me.DataGridViewImageColumn2.Name = "DataGridViewImageColumn2"
        Me.DataGridViewImageColumn2.ReadOnly = True
        Me.DataGridViewImageColumn2.Width = 25
        '
        'Button8
        '
        Me.Button8.Location = New System.Drawing.Point(307, 5)
        Me.Button8.Name = "Button8"
        Me.Button8.Size = New System.Drawing.Size(82, 48)
        Me.Button8.TabIndex = 97
        Me.Button8.Text = "Button8"
        Me.Button8.UseVisualStyleBackColor = True
        '
        'frmDadosPecaCorrente
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1583, 786)
        Me.Controls.Add(Me.TabControlPrincipal)
        Me.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.Name = "frmDadosPecaCorrente"
        Me.Text = "SINCO - Solid Edge - Lynx"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        CType(Me.dgvDadosPecas, System.ComponentModel.ISupportInitialize).EndInit()
        Me.mnudgvDadosPecas.ResumeLayout(False)
        Me.TabControlPrincipal.ResumeLayout(False)
        Me.TabPage1.ResumeLayout(False)
        Me.TabPage1.PerformLayout()
        Me.TcpAcessorios.ResumeLayout(False)
        Me.tpgGabaritos.ResumeLayout(False)
        CType(Me.dgvGabaritos, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tpgCliente.ResumeLayout(False)
        CType(Me.dgvDesenhoCliente, System.ComponentModel.ISupportInitialize).EndInit()
        Me.mnudgvDesenhoCliente.ResumeLayout(False)
        Me.gpbMateriaisProtheus.ResumeLayout(False)
        Me.gpbMateriaisProtheus.PerformLayout()
        CType(Me.dgvMateriaisProtheus, System.ComponentModel.ISupportInitialize).EndInit()
        Me.mnudgvMateriaisProtheus.ResumeLayout(False)
        Me.gpbMateriaiDesenho.ResumeLayout(False)
        CType(Me.dgvMaterialPeca, System.ComponentModel.ISupportInitialize).EndInit()
        Me.mnudgvMaterialPeca.ResumeLayout(False)
        CType(Me.dgvProcessoMaterial, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dgvProcessos, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabPage2.ResumeLayout(False)
        Me.TabPage2.PerformLayout()
        Me.TabPage5.ResumeLayout(False)
        Me.Panel4.ResumeLayout(False)
        Me.Panel4.PerformLayout()
        Me.TabControlOS.ResumeLayout(False)
        Me.TabPage6.ResumeLayout(False)
        CType(Me.DGVListaMaterialSW, System.ComponentModel.ISupportInitialize).EndInit()
        Me.mnuDGVListaMaterialSW.ResumeLayout(False)
        Me.TabPage7.ResumeLayout(False)
        CType(Me.DGVListaMaterialSWMateriais, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel3.ResumeLayout(False)
        Me.Panel3.PerformLayout()
        CType(Me.dgvos, System.ComponentModel.ISupportInitialize).EndInit()
        Me.mnudgvos.ResumeLayout(False)
        CType(Me.BindingNavigator1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.BindingNavigator1.ResumeLayout(False)
        Me.BindingNavigator1.PerformLayout()
        Me.TabPagePDF.ResumeLayout(False)
        CType(Me.dgvDadosPdf, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents btnDxf As Windows.Forms.Button
    Friend WithEvents btnGerarPdf As Windows.Forms.Button
    Friend WithEvents btnListaConjunto As Windows.Forms.Button
    Friend WithEvents btnCriarEstruturaProduto As Windows.Forms.Button
    Friend WithEvents dgvDadosPecas As Windows.Forms.DataGridView
    Friend WithEvents chkdxf As Windows.Forms.CheckBox
    Friend WithEvents chkPdf As Windows.Forms.CheckBox
    Friend WithEvents BtnGeraArquivos As Windows.Forms.Button
    Friend WithEvents ProgressBar1 As Windows.Forms.ProgressBar
    Friend WithEvents DataGridViewImageColumn1 As Windows.Forms.DataGridViewImageColumn
    Friend WithEvents mnudgvDadosPecas As Windows.Forms.ContextMenuStrip
    Friend WithEvents AbrirArquivoToolStripMenuItem As Windows.Forms.ToolStripMenuItem
    Friend WithEvents btnLimpar As Windows.Forms.Button
    Friend WithEvents AbrirDetalhamentoToolStripMenuItem As Windows.Forms.ToolStripMenuItem
    Friend WithEvents DataGridViewImageColumn2 As Windows.Forms.DataGridViewImageColumn
    Friend WithEvents ToolStripSeparator2 As Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripSeparator1 As Windows.Forms.ToolStripSeparator
    Friend WithEvents AbrirPdfToolStripMenuItem As Windows.Forms.ToolStripMenuItem
    Friend WithEvents btnAbriDetalhamentoCorrente As Windows.Forms.Button
    Friend WithEvents TabControlPrincipal As Windows.Forms.TabControl
    Friend WithEvents TabPage1 As Windows.Forms.TabPage
    Friend WithEvents TabPage2 As Windows.Forms.TabPage
    Friend WithEvents btnLimparFiltro As Windows.Forms.Button
    Friend WithEvents btnFiltrar As Windows.Forms.Button
    Friend WithEvents Label2 As Windows.Forms.Label
    Friend WithEvents txtFiltroCaminho1 As Windows.Forms.TextBox
    Friend WithEvents lblProgresso As Windows.Forms.Label
    Friend WithEvents dgvProcessos As Windows.Forms.DataGridView
    Friend WithEvents Label1 As Windows.Forms.Label
    Friend WithEvents txtendereco As Windows.Forms.TextBox
    Friend WithEvents Label8 As Windows.Forms.Label
    Friend WithEvents txtGerente As Windows.Forms.TextBox
    Friend WithEvents Label7 As Windows.Forms.Label
    Friend WithEvents txtCategoria As Windows.Forms.TextBox
    Friend WithEvents Label6 As Windows.Forms.Label
    Friend WithEvents txtAutor As Windows.Forms.TextBox
    Friend WithEvents Label5 As Windows.Forms.Label
    Friend WithEvents Label4 As Windows.Forms.Label
    Friend WithEvents txtNumeroDesenho As Windows.Forms.TextBox
    Friend WithEvents Label3 As Windows.Forms.Label
    Friend WithEvents txtTitulo As Windows.Forms.TextBox
    Friend WithEvents txtEmpresa As Windows.Forms.TextBox
    Friend WithEvents Label14 As Windows.Forms.Label
    Friend WithEvents txtRevisao As Windows.Forms.TextBox
    Friend WithEvents Label13 As Windows.Forms.Label
    Friend WithEvents Label12 As Windows.Forms.Label
    Friend WithEvents txtPalavraChave As Windows.Forms.TextBox
    Friend WithEvents txtData1 As Windows.Forms.TextBox
    Friend WithEvents Label9 As Windows.Forms.Label
    Friend WithEvents txtDataR As Windows.Forms.TextBox
    Friend WithEvents Label10 As Windows.Forms.Label
    Friend WithEvents txtData As Windows.Forms.TextBox
    Friend WithEvents Label11 As Windows.Forms.Label
    Friend WithEvents txtAreametroquadr As Windows.Forms.TextBox
    Friend WithEvents Label18 As Windows.Forms.Label
    Friend WithEvents txtCutSizey As Windows.Forms.TextBox
    Friend WithEvents Label17 As Windows.Forms.Label
    Friend WithEvents txtCutSizex As Windows.Forms.TextBox
    Friend WithEvents Label16 As Windows.Forms.Label
    Friend WithEvents Label15 As Windows.Forms.Label
    Friend WithEvents Label19 As Windows.Forms.Label
    Friend WithEvents txtMaterialSw As Windows.Forms.TextBox
    Friend WithEvents txtEspessura As Windows.Forms.TextBox
    Friend WithEvents TabPage5 As Windows.Forms.TabPage
    Friend WithEvents BindingNavigator1 As Windows.Forms.BindingNavigator
    Friend WithEvents ToolStripButton10 As Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripButton1 As Windows.Forms.ToolStripButton
    Friend WithEvents TSBSalvarOrdemServico As Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripButton3 As Windows.Forms.ToolStripButton
    Friend WithEvents ProgressBarProcessoLiberacaoOrdemServico As Windows.Forms.ToolStripProgressBar
    Friend WithEvents ToolStripSeparator29 As Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripTextBox1 As Windows.Forms.ToolStripTextBox
    Friend WithEvents Panel4 As Windows.Forms.Panel
    Friend WithEvents TabControlOS As Windows.Forms.TabControl
    Friend WithEvents TabPage6 As Windows.Forms.TabPage
    Friend WithEvents DGVListaMaterialSW As Windows.Forms.DataGridView
    Friend WithEvents TabPage7 As Windows.Forms.TabPage
    Friend WithEvents DGVListaMaterialSWMateriais As Windows.Forms.DataGridView
    Friend WithEvents txtPesqNumeroDesenho As Windows.Forms.TextBox
    Friend WithEvents Label27 As Windows.Forms.Label
    Friend WithEvents cboOpcoesAcabamento As Windows.Forms.ComboBox
    Friend WithEvents Label26 As Windows.Forms.Label
    Friend WithEvents txtPesqAcabamentoDesenho As Windows.Forms.TextBox
    Friend WithEvents btnAplicarAcabamento As Windows.Forms.Button
    Friend WithEvents Label28 As Windows.Forms.Label
    Friend WithEvents Label29 As Windows.Forms.Label
    Friend WithEvents txtPesqTipoDesenho As Windows.Forms.TextBox
    Friend WithEvents Panel3 As Windows.Forms.Panel
    Friend WithEvents lbltxtSaldoTag As Windows.Forms.Label
    Friend WithEvents lbltxtQtdeLiberada As Windows.Forms.Label
    Friend WithEvents lbltxtQtdeTag As Windows.Forms.Label
    Friend WithEvents dgvos As Windows.Forms.DataGridView
    Friend WithEvents lblFator As Windows.Forms.TextBox
    Friend WithEvents Label37 As Windows.Forms.Label
    Friend WithEvents chkMostraLiberadasPelaEngenharia As Windows.Forms.CheckBox
    Friend WithEvents Label38 As Windows.Forms.Label
    Friend WithEvents Label36 As Windows.Forms.Label
    Friend WithEvents Label30 As Windows.Forms.Label
    Friend WithEvents txtPesqCriadoPor As Windows.Forms.TextBox
    Friend WithEvents Label35 As Windows.Forms.Label
    Friend WithEvents txtDescricao As Windows.Forms.TextBox
    Friend WithEvents Label25 As Windows.Forms.Label
    Friend WithEvents txtCliente As Windows.Forms.TextBox
    Friend WithEvents Label20 As Windows.Forms.Label
    Friend WithEvents cboProjeto As Windows.Forms.ComboBox
    Friend WithEvents Label24 As Windows.Forms.Label
    Friend WithEvents cboTag As Windows.Forms.ComboBox
    Friend WithEvents txtDescricaoTag As Windows.Forms.TextBox
    Friend WithEvents mnuDGVListaMaterialSW As Windows.Forms.ContextMenuStrip
    Friend WithEvents ToolStripSeparator3 As Windows.Forms.ToolStripSeparator
    Friend WithEvents MarcarTodosToolStripMenuItem As Windows.Forms.ToolStripMenuItem
    Friend WithEvents DesmarcarTodosToolStripMenuItem As Windows.Forms.ToolStripMenuItem
    Friend WithEvents InverterSeleçãoToolStripMenuItem As Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator6 As Windows.Forms.ToolStripSeparator
    Friend WithEvents AbrirPDFDaLinhaSelecionadaToolStripMenuItem As Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator15 As Windows.Forms.ToolStripSeparator
    Friend WithEvents ExcluirODocumentoDaLinhaSelecionadaToolStripMenuItem As Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator7 As Windows.Forms.ToolStripSeparator
    Friend WithEvents MarcarComoConjuntoPrincipalDaOrdemDeServiçoToolStripMenuItem As Windows.Forms.ToolStripMenuItem
    Friend WithEvents DesmarcarComoConjuntoPrincipalToolStripMenuItem As Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator8 As Windows.Forms.ToolStripSeparator
    Friend WithEvents AlterarAQuantidadeDePeçasFabricaçãoDaLinhaSelecionadaToolStripMenuItem As Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator9 As Windows.Forms.ToolStripSeparator
    Friend WithEvents TimerDGVListaMaterialSW As Windows.Forms.Timer
    Friend WithEvents lblOrdemServicoAtiva As Windows.Forms.Label
    Friend WithEvents Timerdgvos As Windows.Forms.Timer
    Friend WithEvents dgvStatus As Windows.Forms.DataGridViewImageColumn
    Friend WithEvents dgvProcessoMaterial As Windows.Forms.DataGridView
    Friend WithEvents btnSalvar As Windows.Forms.Button
    Friend WithEvents txtPesoKg As Windows.Forms.TextBox
    Friend WithEvents Label22 As Windows.Forms.Label
    Friend WithEvents TimerdgvProcessoMaterial As Windows.Forms.Timer
    Friend WithEvents btnInserirnaOS As Windows.Forms.Button
    Friend WithEvents dgvDXF As Windows.Forms.DataGridViewImageColumn
    Friend WithEvents dgvpdf As Windows.Forms.DataGridViewImageColumn
    Friend WithEvents mnudgvos As Windows.Forms.ContextMenuStrip
    Friend WithEvents AbrirPastaDaOrdemDeServiçoToolStripMenuItem As Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator10 As Windows.Forms.ToolStripSeparator
    Friend WithEvents LiberarOrdemDeServiçoParaProduçãoToolStripMenuItem As Windows.Forms.ToolStripMenuItem
    Friend WithEvents CancelarLiberaçãoDaOSToolStripMenuItem As Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator11 As Windows.Forms.ToolStripSeparator
    Friend WithEvents AtualizarPDFsEDXFsNaPastaDaOSMToolStripMenuItem As Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator12 As Windows.Forms.ToolStripSeparator
    Friend WithEvents AlterarOFatorMultipçlicadorDaOSToolStripMenuItem As Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator14 As Windows.Forms.ToolStripSeparator
    Friend WithEvents GeralExcelDaOSToolStripMenuItem As Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator13 As Windows.Forms.ToolStripSeparator
    Friend WithEvents LimparPastaOrdemDeServiçoSelecionadaToolStripMenuItem As Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator17 As Windows.Forms.ToolStripSeparator
    Friend WithEvents CancelarAFabricaçãoDaOSToolStripMenuItem As Windows.Forms.ToolStripMenuItem
    Friend WithEvents btnConfiguracoes As Windows.Forms.Button
    Friend WithEvents btnAtualizacoesDados As Windows.Forms.Button
    Friend WithEvents Label23 As Windows.Forms.Label
    Friend WithEvents TextBox1 As Windows.Forms.TextBox
    Friend WithEvents txtPesqProcesso As Windows.Forms.TextBox
    Friend WithEvents TimerdgvProcesso As Windows.Forms.Timer
    Friend WithEvents Label31 As Windows.Forms.Label
    Friend WithEvents Label32 As Windows.Forms.Label
    Friend WithEvents ToolTipAjuda As Windows.Forms.ToolTip
    Friend WithEvents Button1 As Windows.Forms.Button
    Friend WithEvents Button3 As Windows.Forms.Button
    Friend WithEvents Button2 As Windows.Forms.Button
    Friend WithEvents Button4 As Windows.Forms.Button
    Friend WithEvents btnCriaPropriedadePadroes As Windows.Forms.Button
    Friend WithEvents Label21 As Windows.Forms.Label
    Friend WithEvents txtFiltroEmpresa As Windows.Forms.TextBox
    Friend WithEvents chkOpcaodePasta As Windows.Forms.CheckBox
    Friend WithEvents cboBloqueado As Windows.Forms.ComboBox
    Friend WithEvents Label39 As Windows.Forms.Label
    Friend WithEvents btnConverterChapa As Windows.Forms.Button
    Friend WithEvents cboAcabamento As Windows.Forms.ComboBox
    Friend WithEvents cboTipoDesenho As Windows.Forms.ComboBox
    Friend WithEvents Button5 As Windows.Forms.Button
    Friend WithEvents TabPagePDF As Windows.Forms.TabPage
    Friend WithEvents Button6 As Windows.Forms.Button
    Friend WithEvents btnMsg As Windows.Forms.Button
    Friend WithEvents TimerbtnMsg As Windows.Forms.Timer
    Friend WithEvents RichTextBox1 As Windows.Forms.RichTextBox
    Friend WithEvents dgvDadosPdf As Windows.Forms.DataGridView
    Friend WithEvents dgvDadosDesenhoCampoDados As Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents dgvDadosSelecionado As Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Button7 As Windows.Forms.Button
    Friend WithEvents lblResumo As Windows.Forms.Label
    Friend WithEvents chkiges As Windows.Forms.CheckBox
    Friend WithEvents gpbMateriaiDesenho As Windows.Forms.GroupBox
    Friend WithEvents dgvMaterialPeca As Windows.Forms.DataGridView
    Friend WithEvents gpbMateriaisProtheus As Windows.Forms.GroupBox
    Friend WithEvents txtPesqDescricao3 As Windows.Forms.TextBox
    Friend WithEvents txtPesqDescricao2 As Windows.Forms.TextBox
    Friend WithEvents txtPesqDescricao1 As Windows.Forms.TextBox
    Friend WithEvents Label34 As Windows.Forms.Label
    Friend WithEvents txtPesqCodigo As Windows.Forms.TextBox
    Friend WithEvents Label33 As Windows.Forms.Label
    Friend WithEvents dgvMateriaisProtheus As Windows.Forms.DataGridView
    Friend WithEvents TimerdgvMateriaisProtheus As Windows.Forms.Timer
    Friend WithEvents TxtPesqRP As Windows.Forms.TextBox
    Friend WithEvents Label40 As Windows.Forms.Label
    Friend WithEvents TimerManufaturada As Windows.Forms.Timer
    Friend WithEvents mnudgvMateriaisProtheus As Windows.Forms.ContextMenuStrip
    Friend WithEvents MarcarDesenhoComoRevisãoToolStripMenuItem As Windows.Forms.ToolStripMenuItem
    Friend WithEvents DesmarcarDesenhoComoRevisãoToolStripMenuItem As Windows.Forms.ToolStripMenuItem
    Friend WithEvents AssociarFichaTecnicaToolStripMenuItem As Windows.Forms.ToolStripMenuItem
    Friend WithEvents chkGabarito As Windows.Forms.CheckBox
    Friend WithEvents AssociarGabaritoAPeçaCorrenteToolStripMenuItem As Windows.Forms.ToolStripMenuItem
    Friend WithEvents dgvGabaritos As Windows.Forms.DataGridView
    Friend WithEvents TimerdgvGabaritos As Windows.Forms.Timer
    Friend WithEvents btnProjetoProtheus As Windows.Forms.Button
    Friend WithEvents TcpAcessorios As Windows.Forms.TabControl
    Friend WithEvents tpgGabaritos As Windows.Forms.TabPage
    Friend WithEvents tpgCliente As Windows.Forms.TabPage
    Friend WithEvents dgvDesenhoCliente As Windows.Forms.DataGridView
    Friend WithEvents mnudgvDesenhoCliente As Windows.Forms.ContextMenuStrip
    Friend WithEvents BuscarDesenhoDeReferenciaToolStripMenuItem As Windows.Forms.ToolStripMenuItem
    Friend WithEvents BuscarArquivoPDFDeReferenciaToolStripMenuItem As Windows.Forms.ToolStripMenuItem
    Friend WithEvents AbrirArquivoPDFToolStripMenuItem As Windows.Forms.ToolStripMenuItem
    Friend WithEvents BuscarMaterialNoProtheusToolStripMenuItem As Windows.Forms.ToolStripMenuItem
    Friend WithEvents txtPesoTotalKg As Windows.Forms.TextBox
    Friend WithEvents Label42 As Windows.Forms.Label
    Friend WithEvents txtAreaPintM2 As Windows.Forms.TextBox
    Friend WithEvents Label41 As Windows.Forms.Label
    Friend WithEvents Label44 As Windows.Forms.Label
    Friend WithEvents txtPesoTotalKgTotal As Windows.Forms.TextBox
    Friend WithEvents Label45 As Windows.Forms.Label
    Friend WithEvents txtAreaPintM2Total As Windows.Forms.TextBox
    Friend WithEvents Label46 As Windows.Forms.Label
    Friend WithEvents Label43 As Windows.Forms.Label
    Friend WithEvents dgvSelecao As Windows.Forms.DataGridViewCheckBoxColumn
    Friend WithEvents dgvIconeItemOS As Windows.Forms.DataGridViewImageColumn
    Friend WithEvents dgvDXF1 As Windows.Forms.DataGridViewImageColumn
    Friend WithEvents dgvPDF1 As Windows.Forms.DataGridViewImageColumn
    Friend WithEvents ToolStripSeparator4 As Windows.Forms.ToolStripSeparator
    Friend WithEvents BuscarArquivoDoClienteDiretemanteNoDiretorioSemPassarPeloProtheusToolStripMenuItem As Windows.Forms.ToolStripMenuItem
    Friend WithEvents btnBuscarOperacaoProtheus As Windows.Forms.Button
    Friend WithEvents btnEstrutraMaterialProtheus As Windows.Forms.Button
    Friend WithEvents mnudgvMaterialPeca As Windows.Forms.ContextMenuStrip
    Friend WithEvents AlterarQtdeToolStripMenuItem As Windows.Forms.ToolStripMenuItem
    Friend WithEvents btnPdfLote As Windows.Forms.Button
    Friend WithEvents MarcarComoNovoRevisaoToolStripMenuItem As Windows.Forms.ToolStripMenuItem
    Friend WithEvents ProgressBarOSM As Windows.Forms.ProgressBar
    Friend WithEvents cboB1_TIPO As Windows.Forms.ComboBox
    Friend WithEvents Label47 As Windows.Forms.Label
    Friend WithEvents cboB1_UM As Windows.Forms.ComboBox
    Friend WithEvents Label48 As Windows.Forms.Label
    Friend WithEvents cboB1_GRUPO As Windows.Forms.ComboBox
    Friend WithEvents Label49 As Windows.Forms.Label
    Friend WithEvents txtB1_XREVM As Windows.Forms.TextBox
    Friend WithEvents Label50 As Windows.Forms.Label
    Friend WithEvents btnSalvarCadProtheus As Windows.Forms.Button
    Friend WithEvents Button8 As Windows.Forms.Button
End Class
