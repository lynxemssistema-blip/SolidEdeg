<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmListaDesenhoCliente
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
        Me.dgvDesenhosClientesProtheus = New System.Windows.Forms.DataGridView()
        Me.mnudgvDesenhosClientesProtheus = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.AssociarItemDesenhoClienteProthuesComDesenhoArquivoToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.TimerdgvDesenhosClientesProtheus = New System.Windows.Forms.Timer(Me.components)
        CType(Me.dgvDesenhosClientesProtheus, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.mnudgvDesenhosClientesProtheus.SuspendLayout()
        Me.SuspendLayout()
        '
        'dgvDesenhosClientesProtheus
        '
        Me.dgvDesenhosClientesProtheus.AllowUserToAddRows = False
        Me.dgvDesenhosClientesProtheus.AllowUserToDeleteRows = False
        Me.dgvDesenhosClientesProtheus.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCellsExceptHeader
        Me.dgvDesenhosClientesProtheus.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvDesenhosClientesProtheus.ContextMenuStrip = Me.mnudgvDesenhosClientesProtheus
        Me.dgvDesenhosClientesProtheus.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgvDesenhosClientesProtheus.Location = New System.Drawing.Point(0, 0)
        Me.dgvDesenhosClientesProtheus.Name = "dgvDesenhosClientesProtheus"
        Me.dgvDesenhosClientesProtheus.ReadOnly = True
        Me.dgvDesenhosClientesProtheus.RowHeadersWidth = 51
        Me.dgvDesenhosClientesProtheus.RowTemplate.Height = 24
        Me.dgvDesenhosClientesProtheus.Size = New System.Drawing.Size(1095, 503)
        Me.dgvDesenhosClientesProtheus.TabIndex = 0
        '
        'mnudgvDesenhosClientesProtheus
        '
        Me.mnudgvDesenhosClientesProtheus.ImageScalingSize = New System.Drawing.Size(20, 20)
        Me.mnudgvDesenhosClientesProtheus.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.AssociarItemDesenhoClienteProthuesComDesenhoArquivoToolStripMenuItem})
        Me.mnudgvDesenhosClientesProtheus.Name = "mnudgvDesenhosClientesProtheus"
        Me.mnudgvDesenhosClientesProtheus.Size = New System.Drawing.Size(494, 28)
        '
        'AssociarItemDesenhoClienteProthuesComDesenhoArquivoToolStripMenuItem
        '
        Me.AssociarItemDesenhoClienteProthuesComDesenhoArquivoToolStripMenuItem.Name = "AssociarItemDesenhoClienteProthuesComDesenhoArquivoToolStripMenuItem"
        Me.AssociarItemDesenhoClienteProthuesComDesenhoArquivoToolStripMenuItem.Size = New System.Drawing.Size(493, 24)
        Me.AssociarItemDesenhoClienteProthuesComDesenhoArquivoToolStripMenuItem.Text = "Associar Item Desenho Cliente Protheus com Desenho/Arquivo"
        '
        'TimerdgvDesenhosClientesProtheus
        '
        '
        'frmListaDesenhoCliente
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1095, 503)
        Me.Controls.Add(Me.dgvDesenhosClientesProtheus)
        Me.Name = "frmListaDesenhoCliente"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Lista Desenhos Cliente"
        CType(Me.dgvDesenhosClientesProtheus, System.ComponentModel.ISupportInitialize).EndInit()
        Me.mnudgvDesenhosClientesProtheus.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents dgvDesenhosClientesProtheus As Windows.Forms.DataGridView
    Friend WithEvents TimerdgvDesenhosClientesProtheus As Windows.Forms.Timer
    Friend WithEvents mnudgvDesenhosClientesProtheus As Windows.Forms.ContextMenuStrip
    Friend WithEvents AssociarItemDesenhoClienteProthuesComDesenhoArquivoToolStripMenuItem As Windows.Forms.ToolStripMenuItem
End Class
