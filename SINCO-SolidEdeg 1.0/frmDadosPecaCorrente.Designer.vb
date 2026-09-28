<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmDadosPecaCorrente
    Inherits System.Windows.Forms.Form

    'Descartar substitui Dispose para limpar a lista de componentes.
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
    Private components As System.ComponentModel.IContainer = Nothing

    'OBSERVAÇÃO: o procedimento a seguir é exigido pelo Windows Form Designer
    'Pode ser modificado usando o Windows Form Designer.  
    'Não o modifique usando o editor de códigos.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim DataGridViewCellStyle11 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle12 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.pnlToolbar = New System.Windows.Forms.Panel()
        Me.btnSalvarCadProtheus = New System.Windows.Forms.Button()
        Me.btnEstruturaProtheus = New System.Windows.Forms.Button()
        Me.btnAbriDetalhamentoCorrente = New System.Windows.Forms.Button()
        Me.btnListaConjunto = New System.Windows.Forms.Button()
        Me.btnLote = New System.Windows.Forms.Button()
        Me.btnGerarPdf = New System.Windows.Forms.Button()
        Me.btnLimparBOM = New System.Windows.Forms.Button()
        Me.lblResumo = New System.Windows.Forms.Label()
        Me.grpProtheus = New System.Windows.Forms.GroupBox()
        Me.Label47 = New System.Windows.Forms.Label()
        Me.cboB1_TIPO = New System.Windows.Forms.ComboBox()
        Me.Label49 = New System.Windows.Forms.Label()
        Me.cboB1_GRUPO = New System.Windows.Forms.ComboBox()
        Me.Label48 = New System.Windows.Forms.Label()
        Me.cboB1_UM = New System.Windows.Forms.ComboBox()
        Me.Label50 = New System.Windows.Forms.Label()
        Me.txtB1_XREVM = New System.Windows.Forms.TextBox()
        Me.LabelRevCliente = New System.Windows.Forms.Label()
        Me.txtB1_XREVC = New System.Windows.Forms.TextBox()
        Me.LabelNCM = New System.Windows.Forms.Label()
        Me.txtB1_POSIPI = New System.Windows.Forms.TextBox()
        Me.grpExportacao = New System.Windows.Forms.GroupBox()
        Me.chkOpcaodePasta = New System.Windows.Forms.CheckBox()
        Me.chkdxf = New System.Windows.Forms.CheckBox()
        Me.chkPdf = New System.Windows.Forms.CheckBox()
        Me.chkiges = New System.Windows.Forms.CheckBox()
        Me.grpIdentificacao = New System.Windows.Forms.GroupBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtTitulo = New System.Windows.Forms.TextBox()
        Me.lblCodProtheus = New System.Windows.Forms.Label()
        Me.txtCodProtheus = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.txtNumeroDesenho = New System.Windows.Forms.TextBox()
        Me.grpDimensoes = New System.Windows.Forms.GroupBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.txtendereco = New System.Windows.Forms.TextBox()
        Me.Label16 = New System.Windows.Forms.Label()
        Me.txtCutSizex = New System.Windows.Forms.TextBox()
        Me.Label17 = New System.Windows.Forms.Label()
        Me.txtCutSizey = New System.Windows.Forms.TextBox()
        Me.Label15 = New System.Windows.Forms.Label()
        Me.txtEspessura = New System.Windows.Forms.TextBox()
        Me.Label22 = New System.Windows.Forms.Label()
        Me.txtPesoKg = New System.Windows.Forms.TextBox()
        Me.Label18 = New System.Windows.Forms.Label()
        Me.txtAreametroquadr = New System.Windows.Forms.TextBox()
        Me.dgvBOM = New System.Windows.Forms.DataGridView()
        Me.dgvdxf = New System.Windows.Forms.DataGridViewImageColumn()
        Me.dgvpdf = New System.Windows.Forms.DataGridViewImageColumn()
        Me.pnlStatus = New System.Windows.Forms.Panel()
        Me.lblStatusOperacao = New System.Windows.Forms.Label()
        Me.ProgressBar1 = New System.Windows.Forms.ProgressBar()
        Me.timerAtualizacao = New System.Windows.Forms.Timer(Me.components)
        Me.pnlToolbar.SuspendLayout()
        Me.grpProtheus.SuspendLayout()
        Me.grpExportacao.SuspendLayout()
        Me.grpIdentificacao.SuspendLayout()
        Me.grpDimensoes.SuspendLayout()
        CType(Me.dgvBOM, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlStatus.SuspendLayout()
        Me.SuspendLayout()
        '
        'pnlToolbar
        '
        Me.pnlToolbar.BackColor = System.Drawing.Color.FromArgb(CType(CType(241, Byte), Integer), CType(CType(245, Byte), Integer), CType(CType(249, Byte), Integer))
        Me.pnlToolbar.Controls.Add(Me.btnSalvarCadProtheus)
        Me.pnlToolbar.Controls.Add(Me.btnEstruturaProtheus)
        Me.pnlToolbar.Controls.Add(Me.btnAbriDetalhamentoCorrente)
        Me.pnlToolbar.Controls.Add(Me.btnListaConjunto)
        Me.pnlToolbar.Controls.Add(Me.btnLote)
        Me.pnlToolbar.Controls.Add(Me.btnGerarPdf)
        Me.pnlToolbar.Controls.Add(Me.btnLimparBOM)
        Me.pnlToolbar.Controls.Add(Me.lblResumo)
        Me.pnlToolbar.Dock = System.Windows.Forms.DockStyle.Top
        Me.pnlToolbar.Location = New System.Drawing.Point(0, 0)
        Me.pnlToolbar.Name = "pnlToolbar"
        Me.pnlToolbar.Size = New System.Drawing.Size(1240, 58)
        Me.pnlToolbar.TabIndex = 0
        '
        'btnSalvarCadProtheus
        '
        Me.btnSalvarCadProtheus.BackColor = System.Drawing.Color.White
        Me.btnSalvarCadProtheus.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btnSalvarCadProtheus.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(CType(CType(203, Byte), Integer), CType(CType(213, Byte), Integer), CType(CType(225, Byte), Integer))
        Me.btnSalvarCadProtheus.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSalvarCadProtheus.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSalvarCadProtheus.ForeColor = System.Drawing.Color.FromArgb(CType(CType(31, Byte), Integer), CType(CType(54, Byte), Integer), CType(CType(92, Byte), Integer))
        Me.btnSalvarCadProtheus.Image = Global.SINCO_SolidEdeg_1._0.My.Resources.Resources.salvar
        Me.btnSalvarCadProtheus.Location = New System.Drawing.Point(12, 10)
        Me.btnSalvarCadProtheus.Name = "btnSalvarCadProtheus"
        Me.btnSalvarCadProtheus.Size = New System.Drawing.Size(100, 36)
        Me.btnSalvarCadProtheus.TabIndex = 0
        Me.btnSalvarCadProtheus.Text = "  Salvar"
        Me.btnSalvarCadProtheus.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.btnSalvarCadProtheus.UseVisualStyleBackColor = False
        '
        'btnEstruturaProtheus
        '
        Me.btnEstruturaProtheus.BackColor = System.Drawing.Color.White
        Me.btnEstruturaProtheus.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btnEstruturaProtheus.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(CType(CType(203, Byte), Integer), CType(CType(213, Byte), Integer), CType(CType(225, Byte), Integer))
        Me.btnEstruturaProtheus.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnEstruturaProtheus.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnEstruturaProtheus.ForeColor = System.Drawing.Color.FromArgb(CType(CType(31, Byte), Integer), CType(CType(54, Byte), Integer), CType(CType(92, Byte), Integer))
        Me.btnEstruturaProtheus.Image = Global.SINCO_SolidEdeg_1._0.My.Resources.Resources.lista
        Me.btnEstruturaProtheus.Location = New System.Drawing.Point(118, 10)
        Me.btnEstruturaProtheus.Name = "btnEstruturaProtheus"
        Me.btnEstruturaProtheus.Size = New System.Drawing.Size(150, 36)
        Me.btnEstruturaProtheus.TabIndex = 1
        Me.btnEstruturaProtheus.Text = "  Estrutura Protheus"
        Me.btnEstruturaProtheus.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.btnEstruturaProtheus.UseVisualStyleBackColor = False
        '
        'btnAbriDetalhamentoCorrente
        '
        Me.btnAbriDetalhamentoCorrente.BackColor = System.Drawing.Color.White
        Me.btnAbriDetalhamentoCorrente.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btnAbriDetalhamentoCorrente.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(CType(CType(203, Byte), Integer), CType(CType(213, Byte), Integer), CType(CType(225, Byte), Integer))
        Me.btnAbriDetalhamentoCorrente.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnAbriDetalhamentoCorrente.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnAbriDetalhamentoCorrente.ForeColor = System.Drawing.Color.FromArgb(CType(CType(31, Byte), Integer), CType(CType(54, Byte), Integer), CType(CType(92, Byte), Integer))
        Me.btnAbriDetalhamentoCorrente.Image = Global.SINCO_SolidEdeg_1._0.My.Resources.Resources.par
        Me.btnAbriDetalhamentoCorrente.Location = New System.Drawing.Point(274, 10)
        Me.btnAbriDetalhamentoCorrente.Name = "btnAbriDetalhamentoCorrente"
        Me.btnAbriDetalhamentoCorrente.Size = New System.Drawing.Size(95, 36)
        Me.btnAbriDetalhamentoCorrente.TabIndex = 2
        Me.btnAbriDetalhamentoCorrente.Text = "  Det."
        Me.btnAbriDetalhamentoCorrente.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.btnAbriDetalhamentoCorrente.UseVisualStyleBackColor = False
        '
        'btnListaConjunto
        '
        Me.btnListaConjunto.BackColor = System.Drawing.Color.White
        Me.btnListaConjunto.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btnListaConjunto.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(CType(CType(203, Byte), Integer), CType(CType(213, Byte), Integer), CType(CType(225, Byte), Integer))
        Me.btnListaConjunto.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnListaConjunto.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnListaConjunto.ForeColor = System.Drawing.Color.FromArgb(CType(CType(31, Byte), Integer), CType(CType(54, Byte), Integer), CType(CType(92, Byte), Integer))
        Me.btnListaConjunto.Image = Global.SINCO_SolidEdeg_1._0.My.Resources.Resources.lista
        Me.btnListaConjunto.Location = New System.Drawing.Point(375, 10)
        Me.btnListaConjunto.Name = "btnListaConjunto"
        Me.btnListaConjunto.Size = New System.Drawing.Size(110, 36)
        Me.btnListaConjunto.TabIndex = 3
        Me.btnListaConjunto.Text = "  Est. (BOM)"
        Me.btnListaConjunto.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.btnListaConjunto.UseVisualStyleBackColor = False
        '
        'btnLote
        '
        Me.btnLote.BackColor = System.Drawing.Color.White
        Me.btnLote.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btnLote.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(CType(CType(203, Byte), Integer), CType(CType(213, Byte), Integer), CType(CType(225, Byte), Integer))
        Me.btnLote.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnLote.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnLote.ForeColor = System.Drawing.Color.FromArgb(CType(CType(31, Byte), Integer), CType(CType(54, Byte), Integer), CType(CType(92, Byte), Integer))
        Me.btnLote.Image = Global.SINCO_SolidEdeg_1._0.My.Resources.Resources.pasta
        Me.btnLote.Location = New System.Drawing.Point(491, 10)
        Me.btnLote.Name = "btnLote"
        Me.btnLote.Size = New System.Drawing.Size(110, 36)
        Me.btnLote.TabIndex = 4
        Me.btnLote.Text = "  Lote / Pasta"
        Me.btnLote.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.btnLote.UseVisualStyleBackColor = False
        '
        'btnGerarPdf
        '
        Me.btnGerarPdf.BackColor = System.Drawing.Color.FromArgb(CType(CType(24, Byte), Integer), CType(CType(119, Byte), Integer), CType(CType(242, Byte), Integer))
        Me.btnGerarPdf.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btnGerarPdf.FlatAppearance.BorderSize = 0
        Me.btnGerarPdf.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnGerarPdf.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnGerarPdf.ForeColor = System.Drawing.Color.White
        Me.btnGerarPdf.Image = Global.SINCO_SolidEdeg_1._0.My.Resources.Resources.alterar
        Me.btnGerarPdf.Location = New System.Drawing.Point(607, 10)
        Me.btnGerarPdf.Name = "btnGerarPdf"
        Me.btnGerarPdf.Size = New System.Drawing.Size(115, 36)
        Me.btnGerarPdf.TabIndex = 5
        Me.btnGerarPdf.Text = "  Processar"
        Me.btnGerarPdf.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.btnGerarPdf.UseVisualStyleBackColor = False
        '
        'btnLimparBOM
        '
        Me.btnLimparBOM.BackColor = System.Drawing.Color.White
        Me.btnLimparBOM.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btnLimparBOM.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(CType(CType(203, Byte), Integer), CType(CType(213, Byte), Integer), CType(CType(225, Byte), Integer))
        Me.btnLimparBOM.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnLimparBOM.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnLimparBOM.ForeColor = System.Drawing.Color.FromArgb(CType(CType(185, Byte), Integer), CType(CType(28, Byte), Integer), CType(CType(28, Byte), Integer))
        Me.btnLimparBOM.Image = Global.SINCO_SolidEdeg_1._0.My.Resources.Resources.excluir
        Me.btnLimparBOM.Location = New System.Drawing.Point(728, 10)
        Me.btnLimparBOM.Name = "btnLimparBOM"
        Me.btnLimparBOM.Size = New System.Drawing.Size(115, 36)
        Me.btnLimparBOM.TabIndex = 6
        Me.btnLimparBOM.Text = "  Limpar BOM"
        Me.btnLimparBOM.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.btnLimparBOM.UseVisualStyleBackColor = False
        '
        'lblResumo
        '
        Me.lblResumo.AutoSize = True
        Me.lblResumo.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblResumo.ForeColor = System.Drawing.Color.FromArgb(CType(CType(71, Byte), Integer), CType(CType(85, Byte), Integer), CType(CType(105, Byte), Integer))
        Me.lblResumo.Location = New System.Drawing.Point(852, 18)
        Me.lblResumo.Name = "lblResumo"
        Me.lblResumo.Size = New System.Drawing.Size(40, 20)
        Me.lblResumo.TabIndex = 7
        Me.lblResumo.Text = "-----"
        '
        'grpProtheus
        '
        Me.grpProtheus.Controls.Add(Me.Label47)
        Me.grpProtheus.Controls.Add(Me.cboB1_TIPO)
        Me.grpProtheus.Controls.Add(Me.Label49)
        Me.grpProtheus.Controls.Add(Me.cboB1_GRUPO)
        Me.grpProtheus.Controls.Add(Me.Label48)
        Me.grpProtheus.Controls.Add(Me.cboB1_UM)
        Me.grpProtheus.Controls.Add(Me.Label50)
        Me.grpProtheus.Controls.Add(Me.txtB1_XREVM)
        Me.grpProtheus.Controls.Add(Me.LabelRevCliente)
        Me.grpProtheus.Controls.Add(Me.txtB1_XREVC)
        Me.grpProtheus.Controls.Add(Me.LabelNCM)
        Me.grpProtheus.Controls.Add(Me.txtB1_POSIPI)
        Me.grpProtheus.Font = New System.Drawing.Font("Segoe UI", 8.5!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grpProtheus.ForeColor = System.Drawing.Color.FromArgb(CType(CType(51, Byte), Integer), CType(CType(65, Byte), Integer), CType(CType(85, Byte), Integer))
        Me.grpProtheus.Location = New System.Drawing.Point(12, 62)
        Me.grpProtheus.Name = "grpProtheus"
        Me.grpProtheus.Size = New System.Drawing.Size(734, 78)
        Me.grpProtheus.TabIndex = 1
        Me.grpProtheus.TabStop = False
        Me.grpProtheus.Text = " Classificação Protheus "
        '
        'Label47
        '
        Me.Label47.AutoSize = True
        Me.Label47.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label47.ForeColor = System.Drawing.Color.FromArgb(CType(CType(71, Byte), Integer), CType(CType(85, Byte), Integer), CType(CType(105, Byte), Integer))
        Me.Label47.Location = New System.Drawing.Point(10, 23)
        Me.Label47.Name = "Label47"
        Me.Label47.Size = New System.Drawing.Size(38, 19)
        Me.Label47.TabIndex = 0
        Me.Label47.Text = "Tipo:"
        '
        'cboB1_TIPO
        '
        Me.cboB1_TIPO.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboB1_TIPO.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboB1_TIPO.FormattingEnabled = True
        Me.cboB1_TIPO.Items.AddRange(New Object() {"PA - PRODUTO ACABADO", "PI - PRODUTO INTERMEDIARIO", "BN - BENEFICIAMENTO", "MP - MATERIA PRIMA", "MC - MATERIAL CONSUMO", "GG - GASTO GERAL", "ME - MERCADORIA", "MO - MAO DE OBRA", "OI - OUTROS INSUMOS", "SV - SERVICO"})
        Me.cboB1_TIPO.Location = New System.Drawing.Point(10, 42)
        Me.cboB1_TIPO.Name = "cboB1_TIPO"
        Me.cboB1_TIPO.Size = New System.Drawing.Size(120, 28)
        Me.cboB1_TIPO.TabIndex = 1
        '
        'Label49
        '
        Me.Label49.AutoSize = True
        Me.Label49.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label49.ForeColor = System.Drawing.Color.FromArgb(CType(CType(71, Byte), Integer), CType(CType(85, Byte), Integer), CType(CType(105, Byte), Integer))
        Me.Label49.Location = New System.Drawing.Point(135, 23)
        Me.Label49.Name = "Label49"
        Me.Label49.Size = New System.Drawing.Size(51, 19)
        Me.Label49.TabIndex = 2
        Me.Label49.Text = "Grupo:"
        '
        'cboB1_GRUPO
        '
        Me.cboB1_GRUPO.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboB1_GRUPO.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboB1_GRUPO.FormattingEnabled = True
        Me.cboB1_GRUPO.Location = New System.Drawing.Point(135, 42)
        Me.cboB1_GRUPO.Name = "cboB1_GRUPO"
        Me.cboB1_GRUPO.Size = New System.Drawing.Size(175, 28)
        Me.cboB1_GRUPO.TabIndex = 3
        '
        'Label48
        '
        Me.Label48.AutoSize = True
        Me.Label48.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label48.ForeColor = System.Drawing.Color.FromArgb(CType(CType(71, Byte), Integer), CType(CType(85, Byte), Integer), CType(CType(105, Byte), Integer))
        Me.Label48.Location = New System.Drawing.Point(315, 23)
        Me.Label48.Name = "Label48"
        Me.Label48.Size = New System.Drawing.Size(35, 19)
        Me.Label48.TabIndex = 4
        Me.Label48.Text = "UM:"
        '
        'cboB1_UM
        '
        Me.cboB1_UM.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboB1_UM.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboB1_UM.FormattingEnabled = True
        Me.cboB1_UM.Location = New System.Drawing.Point(315, 42)
        Me.cboB1_UM.Name = "cboB1_UM"
        Me.cboB1_UM.Size = New System.Drawing.Size(65, 28)
        Me.cboB1_UM.TabIndex = 5
        '
        'Label50
        '
        Me.Label50.AutoSize = True
        Me.Label50.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label50.ForeColor = System.Drawing.Color.FromArgb(CType(CType(71, Byte), Integer), CType(CType(85, Byte), Integer), CType(CType(105, Byte), Integer))
        Me.Label50.Location = New System.Drawing.Point(385, 23)
        Me.Label50.Name = "Label50"
        Me.Label50.Size = New System.Drawing.Size(77, 19)
        Me.Label50.TabIndex = 6
        Me.Label50.Text = "Rev. Metal:"
        '
        'txtB1_XREVM
        '
        Me.txtB1_XREVM.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtB1_XREVM.Location = New System.Drawing.Point(385, 42)
        Me.txtB1_XREVM.Name = "txtB1_XREVM"
        Me.txtB1_XREVM.Size = New System.Drawing.Size(75, 27)
        Me.txtB1_XREVM.TabIndex = 7
        Me.txtB1_XREVM.Tag = "revision"
        Me.txtB1_XREVM.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'LabelRevCliente
        '
        Me.LabelRevCliente.AutoSize = True
        Me.LabelRevCliente.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelRevCliente.ForeColor = System.Drawing.Color.FromArgb(CType(CType(71, Byte), Integer), CType(CType(85, Byte), Integer), CType(CType(105, Byte), Integer))
        Me.LabelRevCliente.Location = New System.Drawing.Point(465, 23)
        Me.LabelRevCliente.Name = "LabelRevCliente"
        Me.LabelRevCliente.Size = New System.Drawing.Size(83, 19)
        Me.LabelRevCliente.TabIndex = 8
        Me.LabelRevCliente.Text = "Rev. Cliente:"
        '
        'txtB1_XREVC
        '
        Me.txtB1_XREVC.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtB1_XREVC.Location = New System.Drawing.Point(465, 42)
        Me.txtB1_XREVC.Name = "txtB1_XREVC"
        Me.txtB1_XREVC.Size = New System.Drawing.Size(85, 27)
        Me.txtB1_XREVC.TabIndex = 9
        Me.txtB1_XREVC.Tag = "rev_cliente"
        Me.txtB1_XREVC.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'LabelNCM
        '
        Me.LabelNCM.AutoSize = True
        Me.LabelNCM.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelNCM.ForeColor = System.Drawing.Color.FromArgb(CType(CType(71, Byte), Integer), CType(CType(85, Byte), Integer), CType(CType(105, Byte), Integer))
        Me.LabelNCM.Location = New System.Drawing.Point(555, 23)
        Me.LabelNCM.Name = "LabelNCM"
        Me.LabelNCM.Size = New System.Drawing.Size(44, 19)
        Me.LabelNCM.TabIndex = 10
        Me.LabelNCM.Text = "NCM:"
        '
        'txtB1_POSIPI
        '
        Me.txtB1_POSIPI.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtB1_POSIPI.Location = New System.Drawing.Point(555, 42)
        Me.txtB1_POSIPI.Name = "txtB1_POSIPI"
        Me.txtB1_POSIPI.Size = New System.Drawing.Size(165, 27)
        Me.txtB1_POSIPI.TabIndex = 11
        Me.txtB1_POSIPI.Tag = "ncm"
        Me.txtB1_POSIPI.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'grpExportacao
        '
        Me.grpExportacao.Controls.Add(Me.chkOpcaodePasta)
        Me.grpExportacao.Controls.Add(Me.chkdxf)
        Me.grpExportacao.Controls.Add(Me.chkPdf)
        Me.grpExportacao.Controls.Add(Me.chkiges)
        Me.grpExportacao.Font = New System.Drawing.Font("Segoe UI", 8.5!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grpExportacao.ForeColor = System.Drawing.Color.FromArgb(CType(CType(51, Byte), Integer), CType(CType(65, Byte), Integer), CType(CType(85, Byte), Integer))
        Me.grpExportacao.Location = New System.Drawing.Point(752, 62)
        Me.grpExportacao.Name = "grpExportacao"
        Me.grpExportacao.Size = New System.Drawing.Size(476, 78)
        Me.grpExportacao.TabIndex = 2
        Me.grpExportacao.TabStop = False
        Me.grpExportacao.Text = " Opções de Conversão / Saída "
        '
        'chkOpcaodePasta
        '
        Me.chkOpcaodePasta.AutoSize = True
        Me.chkOpcaodePasta.Checked = True
        Me.chkOpcaodePasta.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkOpcaodePasta.Font = New System.Drawing.Font("Segoe UI", 8.5!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkOpcaodePasta.ForeColor = System.Drawing.Color.FromArgb(CType(CType(51, Byte), Integer), CType(CType(65, Byte), Integer), CType(CType(85, Byte), Integer))
        Me.chkOpcaodePasta.Location = New System.Drawing.Point(12, 21)
        Me.chkOpcaodePasta.Name = "chkOpcaodePasta"
        Me.chkOpcaodePasta.Size = New System.Drawing.Size(266, 24)
        Me.chkOpcaodePasta.TabIndex = 0
        Me.chkOpcaodePasta.Text = "Salvar na pasta corrente do arquivo"
        Me.chkOpcaodePasta.UseVisualStyleBackColor = True
        '
        'chkdxf
        '
        Me.chkdxf.AutoSize = True
        Me.chkdxf.Font = New System.Drawing.Font("Segoe UI", 8.5!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkdxf.ForeColor = System.Drawing.Color.FromArgb(CType(CType(2, Byte), Integer), CType(CType(132, Byte), Integer), CType(CType(199, Byte), Integer))
        Me.chkdxf.Location = New System.Drawing.Point(12, 47)
        Me.chkdxf.Name = "chkdxf"
        Me.chkdxf.Size = New System.Drawing.Size(125, 24)
        Me.chkdxf.TabIndex = 1
        Me.chkdxf.Text = "Converter DXF"
        Me.chkdxf.UseVisualStyleBackColor = True
        '
        'chkPdf
        '
        Me.chkPdf.AutoSize = True
        Me.chkPdf.Font = New System.Drawing.Font("Segoe UI", 8.5!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkPdf.ForeColor = System.Drawing.Color.FromArgb(CType(CType(220, Byte), Integer), CType(CType(38, Byte), Integer), CType(CType(38, Byte), Integer))
        Me.chkPdf.Location = New System.Drawing.Point(145, 47)
        Me.chkPdf.Name = "chkPdf"
        Me.chkPdf.Size = New System.Drawing.Size(125, 24)
        Me.chkPdf.TabIndex = 2
        Me.chkPdf.Text = "Converter PDF"
        Me.chkPdf.UseVisualStyleBackColor = True
        '
        'chkiges
        '
        Me.chkiges.AutoSize = True
        Me.chkiges.Font = New System.Drawing.Font("Segoe UI", 8.5!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkiges.ForeColor = System.Drawing.Color.FromArgb(CType(CType(124, Byte), Integer), CType(CType(58, Byte), Integer), CType(CType(237, Byte), Integer))
        Me.chkiges.Location = New System.Drawing.Point(280, 47)
        Me.chkiges.Name = "chkiges"
        Me.chkiges.Size = New System.Drawing.Size(145, 24)
        Me.chkiges.TabIndex = 3
        Me.chkiges.Text = "Converter IGES"
        Me.chkiges.UseVisualStyleBackColor = True
        '
        'grpIdentificacao
        '
        Me.grpIdentificacao.Controls.Add(Me.Label1)
        Me.grpIdentificacao.Controls.Add(Me.txtTitulo)
        Me.grpIdentificacao.Controls.Add(Me.lblCodProtheus)
        Me.grpIdentificacao.Controls.Add(Me.txtCodProtheus)
        Me.grpIdentificacao.Controls.Add(Me.Label3)
        Me.grpIdentificacao.Controls.Add(Me.txtNumeroDesenho)
        Me.grpIdentificacao.Font = New System.Drawing.Font("Segoe UI", 8.5!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grpIdentificacao.ForeColor = System.Drawing.Color.FromArgb(CType(CType(51, Byte), Integer), CType(CType(65, Byte), Integer), CType(CType(85, Byte), Integer))
        Me.grpIdentificacao.Location = New System.Drawing.Point(12, 146)
        Me.grpIdentificacao.Name = "grpIdentificacao"
        Me.grpIdentificacao.Size = New System.Drawing.Size(1216, 78)
        Me.grpIdentificacao.TabIndex = 3
        Me.grpIdentificacao.TabStop = False
        Me.grpIdentificacao.Text = " Identificação do Documento CAD "
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.Color.FromArgb(CType(CType(71, Byte), Integer), CType(CType(85, Byte), Integer), CType(CType(105, Byte), Integer))
        Me.Label1.Location = New System.Drawing.Point(14, 23)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(166, 19)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Título da Peça / Conjunto:"
        '
        'txtTitulo
        '
        Me.txtTitulo.Font = New System.Drawing.Font("Segoe UI", 9.5!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTitulo.Location = New System.Drawing.Point(14, 42)
        Me.txtTitulo.Name = "txtTitulo"
        Me.txtTitulo.Size = New System.Drawing.Size(420, 29)
        Me.txtTitulo.TabIndex = 1
        Me.txtTitulo.Tag = "SummaryInformation - Título"
        '
        'lblCodProtheus
        '
        Me.lblCodProtheus.AutoSize = True
        Me.lblCodProtheus.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCodProtheus.ForeColor = System.Drawing.Color.FromArgb(CType(CType(3, Byte), Integer), CType(CType(105, Byte), Integer), CType(CType(161, Byte), Integer))
        Me.lblCodProtheus.Location = New System.Drawing.Point(445, 23)
        Me.lblCodProtheus.Name = "lblCodProtheus"
        Me.lblCodProtheus.Size = New System.Drawing.Size(147, 19)
        Me.lblCodProtheus.TabIndex = 2
        Me.lblCodProtheus.Text = "Cód. Protheus (Real):"
        '
        'txtCodProtheus
        '
        Me.txtCodProtheus.BackColor = System.Drawing.Color.FromArgb(CType(CType(241, Byte), Integer), CType(CType(245, Byte), Integer), CType(CType(249, Byte), Integer))
        Me.txtCodProtheus.Font = New System.Drawing.Font("Segoe UI", 9.5!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCodProtheus.ForeColor = System.Drawing.Color.FromArgb(CType(CType(3, Byte), Integer), CType(CType(105, Byte), Integer), CType(CType(161, Byte), Integer))
        Me.txtCodProtheus.Location = New System.Drawing.Point(445, 42)
        Me.txtCodProtheus.Name = "txtCodProtheus"
        Me.txtCodProtheus.ReadOnly = True
        Me.txtCodProtheus.Size = New System.Drawing.Size(220, 29)
        Me.txtCodProtheus.TabIndex = 3
        Me.txtCodProtheus.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.ForeColor = System.Drawing.Color.FromArgb(CType(CType(71, Byte), Integer), CType(CType(85, Byte), Integer), CType(CType(105, Byte), Integer))
        Me.Label3.Location = New System.Drawing.Point(675, 23)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(225, 19)
        Me.Label3.TabIndex = 4
        Me.Label3.Text = "Número do Desenho / Documento:"
        '
        'txtNumeroDesenho
        '
        Me.txtNumeroDesenho.Font = New System.Drawing.Font("Segoe UI", 9.5!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtNumeroDesenho.ForeColor = System.Drawing.Color.FromArgb(CType(CType(15, Byte), Integer), CType(CType(23, Byte), Integer), CType(CType(42, Byte), Integer))
        Me.txtNumeroDesenho.Location = New System.Drawing.Point(675, 42)
        Me.txtNumeroDesenho.Name = "txtNumeroDesenho"
        Me.txtNumeroDesenho.Size = New System.Drawing.Size(525, 29)
        Me.txtNumeroDesenho.TabIndex = 5
        Me.txtNumeroDesenho.Tag = "ProjectInformation - Número do documento"
        '
        'grpDimensoes
        '
        Me.grpDimensoes.Controls.Add(Me.Label8)
        Me.grpDimensoes.Controls.Add(Me.txtendereco)
        Me.grpDimensoes.Controls.Add(Me.Label16)
        Me.grpDimensoes.Controls.Add(Me.txtCutSizex)
        Me.grpDimensoes.Controls.Add(Me.Label17)
        Me.grpDimensoes.Controls.Add(Me.txtCutSizey)
        Me.grpDimensoes.Controls.Add(Me.Label15)
        Me.grpDimensoes.Controls.Add(Me.txtEspessura)
        Me.grpDimensoes.Controls.Add(Me.Label22)
        Me.grpDimensoes.Controls.Add(Me.txtPesoKg)
        Me.grpDimensoes.Controls.Add(Me.Label18)
        Me.grpDimensoes.Controls.Add(Me.txtAreametroquadr)
        Me.grpDimensoes.Font = New System.Drawing.Font("Segoe UI", 8.5!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grpDimensoes.ForeColor = System.Drawing.Color.FromArgb(CType(CType(51, Byte), Integer), CType(CType(65, Byte), Integer), CType(CType(85, Byte), Integer))
        Me.grpDimensoes.Location = New System.Drawing.Point(12, 230)
        Me.grpDimensoes.Name = "grpDimensoes"
        Me.grpDimensoes.Size = New System.Drawing.Size(1216, 78)
        Me.grpDimensoes.TabIndex = 4
        Me.grpDimensoes.TabStop = False
        Me.grpDimensoes.Text = " Dimensões do Blank & Propriedades Físicas "
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.ForeColor = System.Drawing.Color.FromArgb(CType(CType(71, Byte), Integer), CType(CType(85, Byte), Integer), CType(CType(105, Byte), Integer))
        Me.Label8.Location = New System.Drawing.Point(14, 23)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(139, 19)
        Me.Label8.TabIndex = 0
        Me.Label8.Text = "Caminho do Arquivo:"
        '
        'txtendereco
        '
        Me.txtendereco.BackColor = System.Drawing.Color.FromArgb(CType(CType(248, Byte), Integer), CType(CType(250, Byte), Integer), CType(CType(252, Byte), Integer))
        Me.txtendereco.Font = New System.Drawing.Font("Segoe UI", 8.5!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtendereco.Location = New System.Drawing.Point(14, 42)
        Me.txtendereco.Name = "txtendereco"
        Me.txtendereco.ReadOnly = True
        Me.txtendereco.Size = New System.Drawing.Size(435, 26)
        Me.txtendereco.TabIndex = 1
        '
        'Label16
        '
        Me.Label16.AutoSize = True
        Me.Label16.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label16.ForeColor = System.Drawing.Color.FromArgb(CType(CType(71, Byte), Integer), CType(CType(85, Byte), Integer), CType(CType(105, Byte), Integer))
        Me.Label16.Location = New System.Drawing.Point(460, 23)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(109, 19)
        Me.Label16.TabIndex = 2
        Me.Label16.Text = "Cut Size X (mm):"
        '
        'txtCutSizex
        '
        Me.txtCutSizex.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCutSizex.Location = New System.Drawing.Point(460, 42)
        Me.txtCutSizex.Name = "txtCutSizex"
        Me.txtCutSizex.Size = New System.Drawing.Size(120, 27)
        Me.txtCutSizex.TabIndex = 3
        Me.txtCutSizex.Tag = "Flat Pattern Model - CutSizeX"
        Me.txtCutSizex.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label17
        '
        Me.Label17.AutoSize = True
        Me.Label17.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label17.ForeColor = System.Drawing.Color.FromArgb(CType(CType(71, Byte), Integer), CType(CType(85, Byte), Integer), CType(CType(105, Byte), Integer))
        Me.Label17.Location = New System.Drawing.Point(590, 23)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(109, 19)
        Me.Label17.TabIndex = 4
        Me.Label17.Text = "Cut Size Y (mm):"
        '
        'txtCutSizey
        '
        Me.txtCutSizey.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCutSizey.Location = New System.Drawing.Point(590, 42)
        Me.txtCutSizey.Name = "txtCutSizey"
        Me.txtCutSizey.Size = New System.Drawing.Size(120, 27)
        Me.txtCutSizey.TabIndex = 5
        Me.txtCutSizey.Tag = "Flat Pattern Model - CutSizeY"
        Me.txtCutSizey.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label15.ForeColor = System.Drawing.Color.FromArgb(CType(CType(71, Byte), Integer), CType(CType(85, Byte), Integer), CType(CType(105, Byte), Integer))
        Me.Label15.Location = New System.Drawing.Point(720, 23)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(108, 19)
        Me.Label15.TabIndex = 6
        Me.Label15.Text = "Espessura (mm):"
        '
        'txtEspessura
        '
        Me.txtEspessura.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtEspessura.Location = New System.Drawing.Point(720, 42)
        Me.txtEspessura.Name = "txtEspessura"
        Me.txtEspessura.Size = New System.Drawing.Size(120, 27)
        Me.txtEspessura.TabIndex = 7
        Me.txtEspessura.Tag = "Sheet Metal Gage - Thickness"
        Me.txtEspessura.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label22
        '
        Me.Label22.AutoSize = True
        Me.Label22.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label22.ForeColor = System.Drawing.Color.FromArgb(CType(CType(71, Byte), Integer), CType(CType(85, Byte), Integer), CType(CType(105, Byte), Integer))
        Me.Label22.Location = New System.Drawing.Point(850, 23)
        Me.Label22.Name = "Label22"
        Me.Label22.Size = New System.Drawing.Size(68, 19)
        Me.Label22.TabIndex = 8
        Me.Label22.Text = "Peso (Kg):"
        '
        'txtPesoKg
        '
        Me.txtPesoKg.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPesoKg.Location = New System.Drawing.Point(850, 42)
        Me.txtPesoKg.Name = "txtPesoKg"
        Me.txtPesoKg.Size = New System.Drawing.Size(130, 27)
        Me.txtPesoKg.TabIndex = 9
        Me.txtPesoKg.Tag = "Mass"
        Me.txtPesoKg.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label18
        '
        Me.Label18.AutoSize = True
        Me.Label18.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label18.ForeColor = System.Drawing.Color.FromArgb(CType(CType(71, Byte), Integer), CType(CType(85, Byte), Integer), CType(CType(105, Byte), Integer))
        Me.Label18.Location = New System.Drawing.Point(990, 23)
        Me.Label18.Name = "Label18"
        Me.Label18.Size = New System.Drawing.Size(117, 19)
        Me.Label18.TabIndex = 10
        Me.Label18.Text = "Área Pintura (m²):"
        '
        'txtAreametroquadr
        '
        Me.txtAreametroquadr.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAreametroquadr.Location = New System.Drawing.Point(990, 42)
        Me.txtAreametroquadr.Name = "txtAreametroquadr"
        Me.txtAreametroquadr.Size = New System.Drawing.Size(210, 27)
        Me.txtAreametroquadr.TabIndex = 11
        Me.txtAreametroquadr.Tag = "Area"
        Me.txtAreametroquadr.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'dgvBOM
        '
        Me.dgvBOM.AllowUserToAddRows = False
        Me.dgvBOM.AllowUserToDeleteRows = False
        DataGridViewCellStyle11.BackColor = System.Drawing.Color.FromArgb(CType(CType(248, Byte), Integer), CType(CType(250, Byte), Integer), CType(CType(252, Byte), Integer))
        Me.dgvBOM.AlternatingRowsDefaultCellStyle = DataGridViewCellStyle11
        Me.dgvBOM.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dgvBOM.BackgroundColor = System.Drawing.Color.White
        Me.dgvBOM.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        DataGridViewCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle12.BackColor = System.Drawing.Color.FromArgb(CType(CType(31, Byte), Integer), CType(CType(54, Byte), Integer), CType(CType(92, Byte), Integer))
        DataGridViewCellStyle12.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle12.ForeColor = System.Drawing.Color.White
        DataGridViewCellStyle12.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle12.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle12.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgvBOM.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle12
        Me.dgvBOM.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvBOM.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.dgvdxf, Me.dgvpdf})
        Me.dgvBOM.EnableHeadersVisualStyles = False
        Me.dgvBOM.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dgvBOM.GridColor = System.Drawing.Color.FromArgb(CType(CType(226, Byte), Integer), CType(CType(232, Byte), Integer), CType(CType(240, Byte), Integer))
        Me.dgvBOM.Location = New System.Drawing.Point(12, 314)
        Me.dgvBOM.Name = "dgvBOM"
        Me.dgvBOM.ReadOnly = True
        Me.dgvBOM.RowHeadersWidth = 25
        Me.dgvBOM.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvBOM.Size = New System.Drawing.Size(1216, 280)
        Me.dgvBOM.TabIndex = 5
        Me.dgvBOM.Visible = False
        '
        'dgvdxf
        '
        Me.dgvdxf.HeaderText = "DXF"
        Me.dgvdxf.Image = Global.SINCO_SolidEdeg_1._0.My.Resources.Resources.sem_icone
        Me.dgvdxf.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Zoom
        Me.dgvdxf.MinimumWidth = 6
        Me.dgvdxf.Name = "dgvdxf"
        Me.dgvdxf.ReadOnly = True
        Me.dgvdxf.Width = 40
        '
        'dgvpdf
        '
        Me.dgvpdf.HeaderText = "PDF"
        Me.dgvpdf.Image = Global.SINCO_SolidEdeg_1._0.My.Resources.Resources.sem_icone
        Me.dgvpdf.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Zoom
        Me.dgvpdf.MinimumWidth = 6
        Me.dgvpdf.Name = "dgvpdf"
        Me.dgvpdf.ReadOnly = True
        Me.dgvpdf.Width = 40
        '
        'pnlStatus
        '
        Me.pnlStatus.BackColor = System.Drawing.Color.FromArgb(CType(CType(241, Byte), Integer), CType(CType(245, Byte), Integer), CType(CType(249, Byte), Integer))
        Me.pnlStatus.Controls.Add(Me.lblStatusOperacao)
        Me.pnlStatus.Controls.Add(Me.ProgressBar1)
        Me.pnlStatus.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.pnlStatus.Location = New System.Drawing.Point(0, 327)
        Me.pnlStatus.Name = "pnlStatus"
        Me.pnlStatus.Size = New System.Drawing.Size(1240, 38)
        Me.pnlStatus.TabIndex = 6
        '
        'lblStatusOperacao
        '
        Me.lblStatusOperacao.AutoEllipsis = True
        Me.lblStatusOperacao.Font = New System.Drawing.Font("Segoe UI", 8.5!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblStatusOperacao.ForeColor = System.Drawing.Color.FromArgb(CType(CType(31, Byte), Integer), CType(CType(54, Byte), Integer), CType(CType(92, Byte), Integer))
        Me.lblStatusOperacao.Location = New System.Drawing.Point(14, 9)
        Me.lblStatusOperacao.Name = "lblStatusOperacao"
        Me.lblStatusOperacao.Size = New System.Drawing.Size(480, 20)
        Me.lblStatusOperacao.TabIndex = 1
        Me.lblStatusOperacao.Text = "Pronto"
        Me.lblStatusOperacao.Visible = False
        '
        'ProgressBar1
        '
        Me.ProgressBar1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ProgressBar1.Location = New System.Drawing.Point(504, 9)
        Me.ProgressBar1.Name = "ProgressBar1"
        Me.ProgressBar1.Size = New System.Drawing.Size(718, 20)
        Me.ProgressBar1.TabIndex = 0
        Me.ProgressBar1.Visible = False
        '
        'timerAtualizacao
        '
        Me.timerAtualizacao.Interval = 1000
        '
        'frmDadosPecaCorrente
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 20.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(248, Byte), Integer), CType(CType(250, Byte), Integer), CType(CType(252, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(1240, 365)
        Me.Controls.Add(Me.dgvBOM)
        Me.Controls.Add(Me.pnlStatus)
        Me.Controls.Add(Me.grpDimensoes)
        Me.Controls.Add(Me.grpIdentificacao)
        Me.Controls.Add(Me.grpExportacao)
        Me.Controls.Add(Me.grpProtheus)
        Me.Controls.Add(Me.pnlToolbar)
        Me.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ForeColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(41, Byte), Integer), CType(CType(59, Byte), Integer))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.Name = "frmDadosPecaCorrente"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "SINCO - Solid Edge | Metalfisa | SINCO"
        Me.pnlToolbar.ResumeLayout(False)
        Me.pnlToolbar.PerformLayout()
        Me.grpProtheus.ResumeLayout(False)
        Me.grpProtheus.PerformLayout()
        Me.grpExportacao.ResumeLayout(False)
        Me.grpExportacao.PerformLayout()
        Me.grpIdentificacao.ResumeLayout(False)
        Me.grpIdentificacao.PerformLayout()
        Me.grpDimensoes.ResumeLayout(False)
        Me.grpDimensoes.PerformLayout()
        CType(Me.dgvBOM, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlStatus.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents pnlToolbar As Windows.Forms.Panel
    Friend WithEvents btnGerarPdf As Windows.Forms.Button
    Friend WithEvents btnListaConjunto As Windows.Forms.Button
    Friend WithEvents chkdxf As Windows.Forms.CheckBox
    Friend WithEvents chkPdf As Windows.Forms.CheckBox
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
    Friend WithEvents txtPesoKg As Windows.Forms.TextBox
    Friend WithEvents Label22 As Windows.Forms.Label
    Friend WithEvents chkOpcaodePasta As Windows.Forms.CheckBox
    Friend WithEvents lblResumo As Windows.Forms.Label
    Friend WithEvents chkiges As Windows.Forms.CheckBox
    Friend WithEvents grpProtheus As Windows.Forms.GroupBox
    Friend WithEvents cboB1_TIPO As Windows.Forms.ComboBox
    Friend WithEvents Label47 As Windows.Forms.Label
    Friend WithEvents cboB1_UM As Windows.Forms.ComboBox
    Friend WithEvents Label48 As Windows.Forms.Label
    Friend WithEvents cboB1_GRUPO As Windows.Forms.ComboBox
    Friend WithEvents Label49 As Windows.Forms.Label
    Friend WithEvents txtB1_XREVM As Windows.Forms.TextBox
    Friend WithEvents Label50 As Windows.Forms.Label
    Friend WithEvents LabelRevCliente As Windows.Forms.Label
    Friend WithEvents txtB1_XREVC As Windows.Forms.TextBox
    Friend WithEvents LabelNCM As Windows.Forms.Label
    Friend WithEvents txtB1_POSIPI As Windows.Forms.TextBox
    Friend WithEvents grpExportacao As Windows.Forms.GroupBox
    Friend WithEvents grpIdentificacao As Windows.Forms.GroupBox
    Friend WithEvents lblCodProtheus As Windows.Forms.Label
    Friend WithEvents txtCodProtheus As Windows.Forms.TextBox
    Friend WithEvents grpDimensoes As Windows.Forms.GroupBox
    Friend WithEvents btnSalvarCadProtheus As Windows.Forms.Button
    Friend WithEvents btnEstruturaProtheus As Windows.Forms.Button
    Friend WithEvents pnlStatus As Windows.Forms.Panel
    Friend WithEvents lblStatusOperacao As Windows.Forms.Label
    Friend WithEvents ProgressBar1 As Windows.Forms.ProgressBar
    Friend WithEvents btnLote As Windows.Forms.Button
    Friend WithEvents dgvBOM As Windows.Forms.DataGridView
    Friend WithEvents dgvdxf As Windows.Forms.DataGridViewImageColumn
    Friend WithEvents dgvpdf As Windows.Forms.DataGridViewImageColumn
    Friend WithEvents timerAtualizacao As Windows.Forms.Timer
    Friend WithEvents btnLimparBOM As Windows.Forms.Button
End Class
