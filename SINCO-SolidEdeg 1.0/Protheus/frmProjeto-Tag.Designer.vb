<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmProjeto_Tag
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
        Me.dgvProjeto = New System.Windows.Forms.DataGridView()
        Me.dgvTag = New System.Windows.Forms.DataGridView()
        Me.TimerdgvProjeto = New System.Windows.Forms.Timer(Me.components)
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.txtPesqCliente = New System.Windows.Forms.TextBox()
        Me.txtPesqProjeto = New System.Windows.Forms.TextBox()
        Me.TimerdgvTag = New System.Windows.Forms.Timer(Me.components)
        Me.btnExportar = New System.Windows.Forms.Button()
        CType(Me.dgvProjeto, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dgvTag, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'dgvProjeto
        '
        Me.dgvProjeto.AllowUserToAddRows = False
        Me.dgvProjeto.AllowUserToDeleteRows = False
        Me.dgvProjeto.AllowUserToOrderColumns = True
        Me.dgvProjeto.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dgvProjeto.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        Me.dgvProjeto.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells
        Me.dgvProjeto.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvProjeto.Location = New System.Drawing.Point(12, 65)
        Me.dgvProjeto.Name = "dgvProjeto"
        Me.dgvProjeto.ReadOnly = True
        Me.dgvProjeto.RowHeadersWidth = 51
        Me.dgvProjeto.RowTemplate.Height = 24
        Me.dgvProjeto.Size = New System.Drawing.Size(1045, 236)
        Me.dgvProjeto.TabIndex = 0
        '
        'dgvTag
        '
        Me.dgvTag.AllowUserToAddRows = False
        Me.dgvTag.AllowUserToDeleteRows = False
        Me.dgvTag.AllowUserToOrderColumns = True
        Me.dgvTag.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dgvTag.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        Me.dgvTag.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells
        Me.dgvTag.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvTag.Location = New System.Drawing.Point(12, 307)
        Me.dgvTag.Name = "dgvTag"
        Me.dgvTag.ReadOnly = True
        Me.dgvTag.RowHeadersWidth = 51
        Me.dgvTag.RowTemplate.Height = 24
        Me.dgvTag.Size = New System.Drawing.Size(1045, 292)
        Me.dgvTag.TabIndex = 1
        '
        'TimerdgvProjeto
        '
        Me.TimerdgvProjeto.Interval = 800
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(411, 25)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(51, 16)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "Cliente:"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(749, 25)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(53, 16)
        Me.Label2.TabIndex = 3
        Me.Label2.Text = "Projeto:"
        '
        'txtPesqCliente
        '
        Me.txtPesqCliente.Location = New System.Drawing.Point(466, 22)
        Me.txtPesqCliente.Name = "txtPesqCliente"
        Me.txtPesqCliente.Size = New System.Drawing.Size(252, 22)
        Me.txtPesqCliente.TabIndex = 4
        '
        'txtPesqProjeto
        '
        Me.txtPesqProjeto.Location = New System.Drawing.Point(808, 22)
        Me.txtPesqProjeto.Name = "txtPesqProjeto"
        Me.txtPesqProjeto.Size = New System.Drawing.Size(252, 22)
        Me.txtPesqProjeto.TabIndex = 5
        '
        'TimerdgvTag
        '
        Me.TimerdgvTag.Interval = 500
        '
        'btnExportar
        '
        Me.btnExportar.Location = New System.Drawing.Point(12, 10)
        Me.btnExportar.Name = "btnExportar"
        Me.btnExportar.Size = New System.Drawing.Size(212, 49)
        Me.btnExportar.TabIndex = 6
        Me.btnExportar.Text = "Exportar Projeto Totvs > SINCO"
        Me.btnExportar.UseVisualStyleBackColor = True
        '
        'frmProjeto_Tag
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1069, 605)
        Me.Controls.Add(Me.btnExportar)
        Me.Controls.Add(Me.txtPesqProjeto)
        Me.Controls.Add(Me.txtPesqCliente)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.dgvTag)
        Me.Controls.Add(Me.dgvProjeto)
        Me.Name = "frmProjeto_Tag"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Buscar Projeto e Tag do Protheus"
        CType(Me.dgvProjeto, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dgvTag, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents dgvProjeto As Windows.Forms.DataGridView
    Friend WithEvents dgvTag As Windows.Forms.DataGridView
    Friend WithEvents TimerdgvProjeto As Windows.Forms.Timer
    Friend WithEvents Label1 As Windows.Forms.Label
    Friend WithEvents Label2 As Windows.Forms.Label
    Friend WithEvents txtPesqCliente As Windows.Forms.TextBox
    Friend WithEvents txtPesqProjeto As Windows.Forms.TextBox
    Friend WithEvents TimerdgvTag As Windows.Forms.Timer
    Friend WithEvents btnExportar As Windows.Forms.Button
End Class
