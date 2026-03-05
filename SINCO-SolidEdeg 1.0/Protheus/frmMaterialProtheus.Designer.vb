<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMaterialProtheus
    Inherits System.Windows.Forms.Form

    'Descartar substituições de formulário para limpar a lista de componentes.
    <System.Diagnostics.DebuggerNonUserCode()> _
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
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.TxtPesqRP = New System.Windows.Forms.TextBox()
        Me.Label40 = New System.Windows.Forms.Label()
        Me.dgvMateriaisProtheus = New System.Windows.Forms.DataGridView()
        Me.txtPesqDescricao3 = New System.Windows.Forms.TextBox()
        Me.txtPesqDescricao2 = New System.Windows.Forms.TextBox()
        Me.txtPesqDescricao1 = New System.Windows.Forms.TextBox()
        Me.Label34 = New System.Windows.Forms.Label()
        Me.TimerdgvMateriaisProtheus = New System.Windows.Forms.Timer(Me.components)
        Me.mnudgvMateriaisProtheus = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.SalvarMaterialSelecionadoNoSINCOToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.btnFechar = New System.Windows.Forms.Button()
        CType(Me.dgvMateriaisProtheus, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.mnudgvMateriaisProtheus.SuspendLayout()
        Me.SuspendLayout()
        '
        'TxtPesqRP
        '
        Me.TxtPesqRP.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.TxtPesqRP.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.TxtPesqRP.Location = New System.Drawing.Point(12, 41)
        Me.TxtPesqRP.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.TxtPesqRP.Name = "TxtPesqRP"
        Me.TxtPesqRP.Size = New System.Drawing.Size(116, 22)
        Me.TxtPesqRP.TabIndex = 51
        Me.TxtPesqRP.Tag = "SummaryInformation - Título"
        '
        'Label40
        '
        Me.Label40.AutoSize = True
        Me.Label40.Location = New System.Drawing.Point(9, 23)
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
        Me.dgvMateriaisProtheus.Location = New System.Drawing.Point(12, 76)
        Me.dgvMateriaisProtheus.Name = "dgvMateriaisProtheus"
        Me.dgvMateriaisProtheus.ReadOnly = True
        Me.dgvMateriaisProtheus.RowHeadersWidth = 51
        Me.dgvMateriaisProtheus.RowTemplate.Height = 24
        Me.dgvMateriaisProtheus.Size = New System.Drawing.Size(1086, 470)
        Me.dgvMateriaisProtheus.TabIndex = 6
        '
        'txtPesqDescricao3
        '
        Me.txtPesqDescricao3.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.txtPesqDescricao3.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtPesqDescricao3.Location = New System.Drawing.Point(376, 41)
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
        Me.txtPesqDescricao2.Location = New System.Drawing.Point(255, 41)
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
        Me.txtPesqDescricao1.Location = New System.Drawing.Point(134, 41)
        Me.txtPesqDescricao1.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.txtPesqDescricao1.Name = "txtPesqDescricao1"
        Me.txtPesqDescricao1.Size = New System.Drawing.Size(115, 22)
        Me.txtPesqDescricao1.TabIndex = 52
        Me.txtPesqDescricao1.Tag = "SummaryInformation - Título"
        '
        'Label34
        '
        Me.Label34.AutoSize = True
        Me.Label34.Location = New System.Drawing.Point(131, 23)
        Me.Label34.Name = "Label34"
        Me.Label34.Size = New System.Drawing.Size(72, 16)
        Me.Label34.TabIndex = 2
        Me.Label34.Text = "Descrição."
        '
        'TimerdgvMateriaisProtheus
        '
        '
        'mnudgvMateriaisProtheus
        '
        Me.mnudgvMateriaisProtheus.ImageScalingSize = New System.Drawing.Size(20, 20)
        Me.mnudgvMateriaisProtheus.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.SalvarMaterialSelecionadoNoSINCOToolStripMenuItem})
        Me.mnudgvMateriaisProtheus.Name = "mnudgvMateriaisProtheus"
        Me.mnudgvMateriaisProtheus.Size = New System.Drawing.Size(332, 56)
        '
        'SalvarMaterialSelecionadoNoSINCOToolStripMenuItem
        '
        Me.SalvarMaterialSelecionadoNoSINCOToolStripMenuItem.Name = "SalvarMaterialSelecionadoNoSINCOToolStripMenuItem"
        Me.SalvarMaterialSelecionadoNoSINCOToolStripMenuItem.Size = New System.Drawing.Size(331, 24)
        Me.SalvarMaterialSelecionadoNoSINCOToolStripMenuItem.Text = "Salvar Material Selecionado no SINCO"
        '
        'btnFechar
        '
        Me.btnFechar.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnFechar.Location = New System.Drawing.Point(960, 9)
        Me.btnFechar.Name = "btnFechar"
        Me.btnFechar.Size = New System.Drawing.Size(138, 45)
        Me.btnFechar.TabIndex = 55
        Me.btnFechar.Text = "Fechar"
        Me.btnFechar.UseVisualStyleBackColor = True
        '
        'frmMaterialProtheus
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1110, 558)
        Me.Controls.Add(Me.btnFechar)
        Me.Controls.Add(Me.TxtPesqRP)
        Me.Controls.Add(Me.Label40)
        Me.Controls.Add(Me.dgvMateriaisProtheus)
        Me.Controls.Add(Me.txtPesqDescricao3)
        Me.Controls.Add(Me.txtPesqDescricao2)
        Me.Controls.Add(Me.Label34)
        Me.Controls.Add(Me.txtPesqDescricao1)
        Me.Name = "frmMaterialProtheus"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Materiais do Protheus"
        CType(Me.dgvMateriaisProtheus, System.ComponentModel.ISupportInitialize).EndInit()
        Me.mnudgvMateriaisProtheus.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents TxtPesqRP As Windows.Forms.TextBox
    Friend WithEvents Label40 As Windows.Forms.Label
    Friend WithEvents dgvMateriaisProtheus As Windows.Forms.DataGridView
    Friend WithEvents txtPesqDescricao3 As Windows.Forms.TextBox
    Friend WithEvents txtPesqDescricao2 As Windows.Forms.TextBox
    Friend WithEvents txtPesqDescricao1 As Windows.Forms.TextBox
    Friend WithEvents Label34 As Windows.Forms.Label
    Friend WithEvents TimerdgvMateriaisProtheus As Windows.Forms.Timer
    Friend WithEvents mnudgvMateriaisProtheus As Windows.Forms.ContextMenuStrip
    Friend WithEvents SalvarMaterialSelecionadoNoSINCOToolStripMenuItem As Windows.Forms.ToolStripMenuItem
    Friend WithEvents btnFechar As Windows.Forms.Button
End Class
