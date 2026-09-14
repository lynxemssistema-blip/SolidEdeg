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
        Me.mnudgvDadosPecas = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.AbrirArquivoToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator()
        Me.AbrirDetalhamentoToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.AbrirPdfToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.MarcarComoNovoRevisaoToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.chkdxf = New System.Windows.Forms.CheckBox()
        Me.chkPdf = New System.Windows.Forms.CheckBox()
        Me.btnSalvarCadProtheus = New System.Windows.Forms.Button()
        Me.txtB1_XREVM = New System.Windows.Forms.TextBox()
        Me.Label50 = New System.Windows.Forms.Label()
        Me.cboB1_GRUPO = New System.Windows.Forms.ComboBox()
        Me.Label49 = New System.Windows.Forms.Label()
        Me.cboB1_UM = New System.Windows.Forms.ComboBox()
        Me.Label48 = New System.Windows.Forms.Label()
        Me.cboB1_TIPO = New System.Windows.Forms.ComboBox()
        Me.Label47 = New System.Windows.Forms.Label()
        Me.txtPesoKg = New System.Windows.Forms.TextBox()
        Me.Label22 = New System.Windows.Forms.Label()
        Me.txtEspessura = New System.Windows.Forms.TextBox()
        Me.txtAreametroquadr = New System.Windows.Forms.TextBox()
        Me.Label18 = New System.Windows.Forms.Label()
        Me.txtCutSizey = New System.Windows.Forms.TextBox()
        Me.Label17 = New System.Windows.Forms.Label()
        Me.txtCutSizex = New System.Windows.Forms.TextBox()
        Me.Label16 = New System.Windows.Forms.Label()
        Me.Label15 = New System.Windows.Forms.Label()
        Me.txtendereco = New System.Windows.Forms.TextBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.txtNumeroDesenho = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.txtTitulo = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.btnAbriDetalhamentoCorrente = New System.Windows.Forms.Button()
        Me.btnGerarPdf = New System.Windows.Forms.Button()
        Me.chkiges = New System.Windows.Forms.CheckBox()
        Me.lblResumo = New System.Windows.Forms.Label()
        Me.chkOpcaodePasta = New System.Windows.Forms.CheckBox()
        Me.mnudgvDesenhoCliente = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.BuscarDesenhoDeReferenciaToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.BuscarArquivoPDFDeReferenciaToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.AbrirArquivoPDFToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator4 = New System.Windows.Forms.ToolStripSeparator()
        Me.BuscarArquivoDoClienteDiretemanteNoDiretorioSemPassarPeloProtheusToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnudgvMateriaisProtheus = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.AssociarFichaTecnicaToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.AssociarGabaritoAPeçaCorrenteToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.BuscarMaterialNoProtheusToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnudgvMaterialPeca = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.AlterarQtdeToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
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
        Me.ProgressBar1 = New System.Windows.Forms.ProgressBar()
        Me.btnLote = New System.Windows.Forms.Button()
        Me.mnudgvDadosPecas.SuspendLayout()
        Me.mnudgvDesenhoCliente.SuspendLayout()
        Me.mnudgvMateriaisProtheus.SuspendLayout()
        Me.mnudgvMaterialPeca.SuspendLayout()
        Me.mnuDGVListaMaterialSW.SuspendLayout()
        Me.mnudgvos.SuspendLayout()
        Me.SuspendLayout()
        '
        'btnListaConjunto
        '
        Me.btnListaConjunto.Location = New System.Drawing.Point(318, 11)
        Me.btnListaConjunto.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.btnListaConjunto.Name = "btnListaConjunto"
        Me.btnListaConjunto.Size = New System.Drawing.Size(154, 50)
        Me.btnListaConjunto.TabIndex = 5
        Me.btnListaConjunto.Text = "BOM"
        Me.ToolTipAjuda.SetToolTip(Me.btnListaConjunto, resources.GetString("btnListaConjunto.ToolTip"))
        Me.btnListaConjunto.UseVisualStyleBackColor = True
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
        Me.chkdxf.Location = New System.Drawing.Point(776, 121)
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
        Me.chkPdf.Location = New System.Drawing.Point(910, 121)
        Me.chkPdf.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.chkPdf.Name = "chkPdf"
        Me.chkPdf.Size = New System.Drawing.Size(117, 20)
        Me.chkPdf.TabIndex = 8
        Me.chkPdf.Text = "Converter PDF"
        Me.chkPdf.UseVisualStyleBackColor = True
        '
        'btnSalvarCadProtheus
        '
        Me.btnSalvarCadProtheus.Image = Global.SINCO_SolidEdeg_1._0.My.Resources.Resources.salvar
        Me.btnSalvarCadProtheus.Location = New System.Drawing.Point(11, 11)
        Me.btnSalvarCadProtheus.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.btnSalvarCadProtheus.Name = "btnSalvarCadProtheus"
        Me.btnSalvarCadProtheus.Size = New System.Drawing.Size(139, 50)
        Me.btnSalvarCadProtheus.TabIndex = 96
        Me.btnSalvarCadProtheus.Text = "Salvar"
        Me.btnSalvarCadProtheus.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.btnSalvarCadProtheus.UseVisualStyleBackColor = True
        '
        'txtB1_XREVM
        '
        Me.txtB1_XREVM.Location = New System.Drawing.Point(508, 107)
        Me.txtB1_XREVM.Name = "txtB1_XREVM"
        Me.txtB1_XREVM.Size = New System.Drawing.Size(100, 22)
        Me.txtB1_XREVM.TabIndex = 95
        Me.txtB1_XREVM.Tag = "revision"
        Me.txtB1_XREVM.Text = "00"
        '
        'Label50
        '
        Me.Label50.AutoSize = True
        Me.Label50.Location = New System.Drawing.Point(513, 87)
        Me.Label50.Name = "Label50"
        Me.Label50.Size = New System.Drawing.Size(95, 16)
        Me.Label50.TabIndex = 94
        Me.Label50.Text = "Rev. Metalfisa:"
        '
        'cboB1_GRUPO
        '
        Me.cboB1_GRUPO.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboB1_GRUPO.FormattingEnabled = True
        Me.cboB1_GRUPO.Location = New System.Drawing.Point(206, 107)
        Me.cboB1_GRUPO.Name = "cboB1_GRUPO"
        Me.cboB1_GRUPO.Size = New System.Drawing.Size(167, 24)
        Me.cboB1_GRUPO.TabIndex = 93
        '
        'Label49
        '
        Me.Label49.AutoSize = True
        Me.Label49.Location = New System.Drawing.Point(207, 88)
        Me.Label49.Name = "Label49"
        Me.Label49.Size = New System.Drawing.Size(47, 16)
        Me.Label49.TabIndex = 92
        Me.Label49.Text = "Grupo:"
        '
        'cboB1_UM
        '
        Me.cboB1_UM.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboB1_UM.FormattingEnabled = True
        Me.cboB1_UM.Location = New System.Drawing.Point(116, 108)
        Me.cboB1_UM.Name = "cboB1_UM"
        Me.cboB1_UM.Size = New System.Drawing.Size(73, 24)
        Me.cboB1_UM.TabIndex = 91
        '
        'Label48
        '
        Me.Label48.AutoSize = True
        Me.Label48.Location = New System.Drawing.Point(118, 89)
        Me.Label48.Name = "Label48"
        Me.Label48.Size = New System.Drawing.Size(62, 16)
        Me.Label48.TabIndex = 90
        Me.Label48.Text = "Unidade:"
        '
        'cboB1_TIPO
        '
        Me.cboB1_TIPO.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboB1_TIPO.FormattingEnabled = True
        Me.cboB1_TIPO.Location = New System.Drawing.Point(11, 108)
        Me.cboB1_TIPO.Name = "cboB1_TIPO"
        Me.cboB1_TIPO.Size = New System.Drawing.Size(79, 24)
        Me.cboB1_TIPO.TabIndex = 89
        '
        'Label47
        '
        Me.Label47.AutoSize = True
        Me.Label47.Location = New System.Drawing.Point(11, 89)
        Me.Label47.Name = "Label47"
        Me.Label47.Size = New System.Drawing.Size(38, 16)
        Me.Label47.TabIndex = 88
        Me.Label47.Text = "Tipo:"
        '
        'txtPesoKg
        '
        Me.txtPesoKg.AcceptsTab = True
        Me.txtPesoKg.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtPesoKg.Enabled = False
        Me.txtPesoKg.Location = New System.Drawing.Point(388, 107)
        Me.txtPesoKg.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.txtPesoKg.Name = "txtPesoKg"
        Me.txtPesoKg.Size = New System.Drawing.Size(103, 22)
        Me.txtPesoKg.TabIndex = 55
        '
        'Label22
        '
        Me.Label22.AutoSize = True
        Me.Label22.Location = New System.Drawing.Point(388, 87)
        Me.Label22.Name = "Label22"
        Me.Label22.Size = New System.Drawing.Size(60, 16)
        Me.Label22.TabIndex = 56
        Me.Label22.Text = "Peso kg:"
        '
        'txtEspessura
        '
        Me.txtEspessura.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtEspessura.Enabled = False
        Me.txtEspessura.Location = New System.Drawing.Point(140, 226)
        Me.txtEspessura.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.txtEspessura.Name = "txtEspessura"
        Me.txtEspessura.Size = New System.Drawing.Size(105, 22)
        Me.txtEspessura.TabIndex = 12
        '
        'txtAreametroquadr
        '
        Me.txtAreametroquadr.AcceptsTab = True
        Me.txtAreametroquadr.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtAreametroquadr.Enabled = False
        Me.txtAreametroquadr.Location = New System.Drawing.Point(11, 226)
        Me.txtAreametroquadr.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.txtAreametroquadr.Name = "txtAreametroquadr"
        Me.txtAreametroquadr.Size = New System.Drawing.Size(103, 22)
        Me.txtAreametroquadr.TabIndex = 15
        '
        'Label18
        '
        Me.Label18.AutoSize = True
        Me.Label18.Location = New System.Drawing.Point(11, 204)
        Me.Label18.Name = "Label18"
        Me.Label18.Size = New System.Drawing.Size(57, 16)
        Me.Label18.TabIndex = 50
        Me.Label18.Text = "Area M²:"
        '
        'txtCutSizey
        '
        Me.txtCutSizey.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtCutSizey.Enabled = False
        Me.txtCutSizey.Location = New System.Drawing.Point(368, 226)
        Me.txtCutSizey.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.txtCutSizey.Name = "txtCutSizey"
        Me.txtCutSizey.Size = New System.Drawing.Size(81, 22)
        Me.txtCutSizey.TabIndex = 14
        Me.txtCutSizey.Tag = "Flat_Pattern_Model_CutSizeY"
        '
        'Label17
        '
        Me.Label17.AutoSize = True
        Me.Label17.Location = New System.Drawing.Point(364, 203)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(90, 16)
        Me.Label17.TabIndex = 48
        Me.Label17.Text = "Comprimento:"
        '
        'txtCutSizex
        '
        Me.txtCutSizex.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtCutSizex.Enabled = False
        Me.txtCutSizex.Location = New System.Drawing.Point(252, 226)
        Me.txtCutSizex.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.txtCutSizex.Name = "txtCutSizex"
        Me.txtCutSizex.Size = New System.Drawing.Size(103, 22)
        Me.txtCutSizex.TabIndex = 13
        Me.txtCutSizex.Tag = "Flat_Pattern_Model_CutSizeX"
        '
        'Label16
        '
        Me.Label16.AutoSize = True
        Me.Label16.Location = New System.Drawing.Point(256, 203)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(56, 16)
        Me.Label16.TabIndex = 46
        Me.Label16.Text = "Largura:"
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.Location = New System.Drawing.Point(140, 203)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(75, 16)
        Me.Label15.TabIndex = 44
        Me.Label15.Text = "Espessura:"
        '
        'txtendereco
        '
        Me.txtendereco.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtendereco.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtendereco.Enabled = False
        Me.txtendereco.Location = New System.Drawing.Point(527, 165)
        Me.txtendereco.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.txtendereco.Name = "txtendereco"
        Me.txtendereco.Size = New System.Drawing.Size(630, 22)
        Me.txtendereco.TabIndex = 19
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(524, 143)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(69, 16)
        Me.Label8.TabIndex = 30
        Me.Label8.Text = "Endereço:"
        '
        'txtNumeroDesenho
        '
        Me.txtNumeroDesenho.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtNumeroDesenho.Enabled = False
        Me.txtNumeroDesenho.Location = New System.Drawing.Point(321, 165)
        Me.txtNumeroDesenho.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.txtNumeroDesenho.Name = "txtNumeroDesenho"
        Me.txtNumeroDesenho.Size = New System.Drawing.Size(195, 22)
        Me.txtNumeroDesenho.TabIndex = 1
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(321, 143)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(130, 16)
        Me.Label3.TabIndex = 20
        Me.Label3.Text = "Numero Documento:"
        '
        'txtTitulo
        '
        Me.txtTitulo.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtTitulo.Font = New System.Drawing.Font("Arial Narrow", 7.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTitulo.Location = New System.Drawing.Point(11, 165)
        Me.txtTitulo.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.txtTitulo.Name = "txtTitulo"
        Me.txtTitulo.Size = New System.Drawing.Size(289, 22)
        Me.txtTitulo.TabIndex = 0
        Me.txtTitulo.Tag = "SummaryInformation - Título"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(11, 143)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(43, 16)
        Me.Label1.TabIndex = 18
        Me.Label1.Text = "Titulo:"
        '
        'btnAbriDetalhamentoCorrente
        '
        Me.btnAbriDetalhamentoCorrente.Image = Global.SINCO_SolidEdeg_1._0.My.Resources.Resources.par
        Me.btnAbriDetalhamentoCorrente.Location = New System.Drawing.Point(156, 11)
        Me.btnAbriDetalhamentoCorrente.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.btnAbriDetalhamentoCorrente.Name = "btnAbriDetalhamentoCorrente"
        Me.btnAbriDetalhamentoCorrente.Size = New System.Drawing.Size(156, 50)
        Me.btnAbriDetalhamentoCorrente.TabIndex = 12
        Me.btnAbriDetalhamentoCorrente.Text = "Detalhamento"
        Me.btnAbriDetalhamentoCorrente.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.ToolTipAjuda.SetToolTip(Me.btnAbriDetalhamentoCorrente, "📄 Caso haja detalhamento para a peça atual, o desenho será aberto automaticament" &
        "e.")
        Me.btnAbriDetalhamentoCorrente.UseVisualStyleBackColor = True
        '
        'btnGerarPdf
        '
        Me.btnGerarPdf.Image = Global.SINCO_SolidEdeg_1._0.My.Resources.Resources.alterar
        Me.btnGerarPdf.Location = New System.Drawing.Point(626, 88)
        Me.btnGerarPdf.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.btnGerarPdf.Name = "btnGerarPdf"
        Me.btnGerarPdf.Size = New System.Drawing.Size(123, 50)
        Me.btnGerarPdf.TabIndex = 3
        Me.btnGerarPdf.Text = "Processar"
        Me.btnGerarPdf.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.btnGerarPdf.UseVisualStyleBackColor = True
        '
        'chkiges
        '
        Me.chkiges.AutoSize = True
        Me.chkiges.Location = New System.Drawing.Point(1042, 121)
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
        Me.lblResumo.Location = New System.Drawing.Point(1085, 86)
        Me.lblResumo.Name = "lblResumo"
        Me.lblResumo.Size = New System.Drawing.Size(44, 20)
        Me.lblResumo.TabIndex = 23
        Me.lblResumo.Text = "-----"
        Me.ToolTipAjuda.SetToolTip(Me.lblResumo, "🧾 Exibe a Ordem de Serviço atualmente selecionada.")
        '
        'chkOpcaodePasta
        '
        Me.chkOpcaodePasta.AutoSize = True
        Me.chkOpcaodePasta.Checked = True
        Me.chkOpcaodePasta.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkOpcaodePasta.Location = New System.Drawing.Point(776, 88)
        Me.chkOpcaodePasta.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.chkOpcaodePasta.Name = "chkOpcaodePasta"
        Me.chkOpcaodePasta.Size = New System.Drawing.Size(243, 20)
        Me.chkOpcaodePasta.TabIndex = 22
        Me.chkOpcaodePasta.Text = "Salvar na Pasta corrente do arquivo"
        Me.chkOpcaodePasta.UseVisualStyleBackColor = True
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
        'TimerbtnMsg
        '
        Me.TimerbtnMsg.Interval = 50
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
        'ProgressBar1
        '
        Me.ProgressBar1.Location = New System.Drawing.Point(8, 259)
        Me.ProgressBar1.Name = "ProgressBar1"
        Me.ProgressBar1.Size = New System.Drawing.Size(1109, 23)
        Me.ProgressBar1.TabIndex = 97
        '
        'btnLote
        '
        Me.btnLote.Location = New System.Drawing.Point(478, 11)
        Me.btnLote.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.btnLote.Name = "btnLote"
        Me.btnLote.Size = New System.Drawing.Size(154, 50)
        Me.btnLote.TabIndex = 104
        Me.btnLote.Text = "Lote/Pasta"
        Me.ToolTipAjuda.SetToolTip(Me.btnLote, resources.GetString("btnLote.ToolTip"))
        Me.btnLote.UseVisualStyleBackColor = True
        '
        'frmDadosPecaCorrente
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1172, 294)
        Me.Controls.Add(Me.btnLote)
        Me.Controls.Add(Me.ProgressBar1)
        Me.Controls.Add(Me.chkiges)
        Me.Controls.Add(Me.txtB1_XREVM)
        Me.Controls.Add(Me.lblResumo)
        Me.Controls.Add(Me.chkOpcaodePasta)
        Me.Controls.Add(Me.Label50)
        Me.Controls.Add(Me.btnSalvarCadProtheus)
        Me.Controls.Add(Me.cboB1_GRUPO)
        Me.Controls.Add(Me.btnListaConjunto)
        Me.Controls.Add(Me.cboB1_TIPO)
        Me.Controls.Add(Me.Label49)
        Me.Controls.Add(Me.chkPdf)
        Me.Controls.Add(Me.Label22)
        Me.Controls.Add(Me.chkdxf)
        Me.Controls.Add(Me.cboB1_UM)
        Me.Controls.Add(Me.txtPesoKg)
        Me.Controls.Add(Me.Label48)
        Me.Controls.Add(Me.Label47)
        Me.Controls.Add(Me.txtEspessura)
        Me.Controls.Add(Me.txtAreametroquadr)
        Me.Controls.Add(Me.Label18)
        Me.Controls.Add(Me.txtCutSizey)
        Me.Controls.Add(Me.btnGerarPdf)
        Me.Controls.Add(Me.Label17)
        Me.Controls.Add(Me.btnAbriDetalhamentoCorrente)
        Me.Controls.Add(Me.txtCutSizex)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.Label16)
        Me.Controls.Add(Me.txtTitulo)
        Me.Controls.Add(Me.Label15)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.txtNumeroDesenho)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.txtendereco)
        Me.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.Name = "frmDadosPecaCorrente"
        Me.Text = "SINCO - Solid Edge - Lynx"
        Me.mnudgvDadosPecas.ResumeLayout(False)
        Me.mnudgvDesenhoCliente.ResumeLayout(False)
        Me.mnudgvMateriaisProtheus.ResumeLayout(False)
        Me.mnudgvMaterialPeca.ResumeLayout(False)
        Me.mnuDGVListaMaterialSW.ResumeLayout(False)
        Me.mnudgvos.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btnGerarPdf As Windows.Forms.Button
    Friend WithEvents btnListaConjunto As Windows.Forms.Button
    Friend WithEvents chkdxf As Windows.Forms.CheckBox
    Friend WithEvents chkPdf As Windows.Forms.CheckBox
    Friend WithEvents DataGridViewImageColumn1 As Windows.Forms.DataGridViewImageColumn
    Friend WithEvents mnudgvDadosPecas As Windows.Forms.ContextMenuStrip
    Friend WithEvents AbrirArquivoToolStripMenuItem As Windows.Forms.ToolStripMenuItem
    Friend WithEvents AbrirDetalhamentoToolStripMenuItem As Windows.Forms.ToolStripMenuItem
    Friend WithEvents DataGridViewImageColumn2 As Windows.Forms.DataGridViewImageColumn
    Friend WithEvents ToolStripSeparator2 As Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripSeparator1 As Windows.Forms.ToolStripSeparator
    Friend WithEvents AbrirPdfToolStripMenuItem As Windows.Forms.ToolStripMenuItem
    Friend WithEvents btnAbriDetalhamentoCorrente As Windows.Forms.Button
    Friend WithEvents Label1 As Windows.Forms.Label
    Friend WithEvents txtendereco As Windows.Forms.TextBox
    Friend WithEvents Label8 As Windows.Forms.Label
    Friend WithEvents txtNumeroDesenho As Windows.Forms.TextBox
    Friend WithEvents Label3 As Windows.Forms.Label
    Friend WithEvents txtTitulo As Windows.Forms.TextBox
    Friend WithEvents txtAreametroquadr As Windows.Forms.TextBox
    Friend WithEvents Label18 As Windows.Forms.Label
    Friend WithEvents txtCutSizey As Windows.Forms.TextBox
    Friend WithEvents Label17 As Windows.Forms.Label
    Friend WithEvents txtCutSizex As Windows.Forms.TextBox
    Friend WithEvents Label16 As Windows.Forms.Label
    Friend WithEvents Label15 As Windows.Forms.Label
    Friend WithEvents txtEspessura As Windows.Forms.TextBox
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
    Friend WithEvents Timerdgvos As Windows.Forms.Timer
    Friend WithEvents txtPesoKg As Windows.Forms.TextBox
    Friend WithEvents Label22 As Windows.Forms.Label
    Friend WithEvents TimerdgvProcessoMaterial As Windows.Forms.Timer
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
    Friend WithEvents TimerdgvProcesso As Windows.Forms.Timer
    Friend WithEvents ToolTipAjuda As Windows.Forms.ToolTip
    Friend WithEvents chkOpcaodePasta As Windows.Forms.CheckBox
    Friend WithEvents TimerbtnMsg As Windows.Forms.Timer
    Friend WithEvents lblResumo As Windows.Forms.Label
    Friend WithEvents chkiges As Windows.Forms.CheckBox
    Friend WithEvents TimerdgvMateriaisProtheus As Windows.Forms.Timer
    Friend WithEvents TimerManufaturada As Windows.Forms.Timer
    Friend WithEvents mnudgvMateriaisProtheus As Windows.Forms.ContextMenuStrip
    Friend WithEvents MarcarDesenhoComoRevisãoToolStripMenuItem As Windows.Forms.ToolStripMenuItem
    Friend WithEvents DesmarcarDesenhoComoRevisãoToolStripMenuItem As Windows.Forms.ToolStripMenuItem
    Friend WithEvents AssociarFichaTecnicaToolStripMenuItem As Windows.Forms.ToolStripMenuItem
    Friend WithEvents AssociarGabaritoAPeçaCorrenteToolStripMenuItem As Windows.Forms.ToolStripMenuItem
    Friend WithEvents TimerdgvGabaritos As Windows.Forms.Timer
    Friend WithEvents mnudgvDesenhoCliente As Windows.Forms.ContextMenuStrip
    Friend WithEvents BuscarDesenhoDeReferenciaToolStripMenuItem As Windows.Forms.ToolStripMenuItem
    Friend WithEvents BuscarArquivoPDFDeReferenciaToolStripMenuItem As Windows.Forms.ToolStripMenuItem
    Friend WithEvents AbrirArquivoPDFToolStripMenuItem As Windows.Forms.ToolStripMenuItem
    Friend WithEvents BuscarMaterialNoProtheusToolStripMenuItem As Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator4 As Windows.Forms.ToolStripSeparator
    Friend WithEvents BuscarArquivoDoClienteDiretemanteNoDiretorioSemPassarPeloProtheusToolStripMenuItem As Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnudgvMaterialPeca As Windows.Forms.ContextMenuStrip
    Friend WithEvents AlterarQtdeToolStripMenuItem As Windows.Forms.ToolStripMenuItem
    Friend WithEvents MarcarComoNovoRevisaoToolStripMenuItem As Windows.Forms.ToolStripMenuItem
    Friend WithEvents cboB1_TIPO As Windows.Forms.ComboBox
    Friend WithEvents Label47 As Windows.Forms.Label
    Friend WithEvents cboB1_UM As Windows.Forms.ComboBox
    Friend WithEvents Label48 As Windows.Forms.Label
    Friend WithEvents cboB1_GRUPO As Windows.Forms.ComboBox
    Friend WithEvents Label49 As Windows.Forms.Label
    Friend WithEvents txtB1_XREVM As Windows.Forms.TextBox
    Friend WithEvents Label50 As Windows.Forms.Label
    Friend WithEvents btnSalvarCadProtheus As Windows.Forms.Button
    Friend WithEvents ProgressBar1 As Windows.Forms.ProgressBar
    Friend WithEvents btnLote As Windows.Forms.Button
End Class
