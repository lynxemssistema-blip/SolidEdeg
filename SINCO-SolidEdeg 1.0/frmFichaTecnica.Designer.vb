<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmFichaTecnica
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
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.txtnomeArquivo = New System.Windows.Forms.TextBox()
        Me.txtEnderecoArquivo = New System.Windows.Forms.TextBox()
        Me.txtDescricaoArquivo = New System.Windows.Forms.TextBox()
        Me.btnSalvar = New System.Windows.Forms.Button()
        Me.btnNovo = New System.Windows.Forms.Button()
        Me.btnExcluir = New System.Windows.Forms.Button()
        Me.btnCancelar = New System.Windows.Forms.Button()
        Me.dgvDados = New System.Windows.Forms.DataGridView()
        Me.TimerdgvDados = New System.Windows.Forms.Timer(Me.components)
        Me.btnBuscarArquivo = New System.Windows.Forms.Button()
        CType(Me.dgvDados, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(9, 80)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(78, 16)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Nome Doc.:"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(9, 127)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(118, 16)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "Endereço Arquivo:"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(13, 177)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(72, 16)
        Me.Label3.TabIndex = 2
        Me.Label3.Text = "Descrição:"
        '
        'txtnomeArquivo
        '
        Me.txtnomeArquivo.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtnomeArquivo.Location = New System.Drawing.Point(12, 101)
        Me.txtnomeArquivo.Name = "txtnomeArquivo"
        Me.txtnomeArquivo.Size = New System.Drawing.Size(301, 22)
        Me.txtnomeArquivo.TabIndex = 3
        '
        'txtEnderecoArquivo
        '
        Me.txtEnderecoArquivo.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtEnderecoArquivo.Location = New System.Drawing.Point(12, 146)
        Me.txtEnderecoArquivo.Name = "txtEnderecoArquivo"
        Me.txtEnderecoArquivo.Size = New System.Drawing.Size(668, 22)
        Me.txtEnderecoArquivo.TabIndex = 4
        '
        'txtDescricaoArquivo
        '
        Me.txtDescricaoArquivo.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtDescricaoArquivo.Location = New System.Drawing.Point(12, 199)
        Me.txtDescricaoArquivo.Multiline = True
        Me.txtDescricaoArquivo.Name = "txtDescricaoArquivo"
        Me.txtDescricaoArquivo.Size = New System.Drawing.Size(756, 71)
        Me.txtDescricaoArquivo.TabIndex = 5
        '
        'btnSalvar
        '
        Me.btnSalvar.Location = New System.Drawing.Point(12, 12)
        Me.btnSalvar.Name = "btnSalvar"
        Me.btnSalvar.Size = New System.Drawing.Size(109, 44)
        Me.btnSalvar.TabIndex = 6
        Me.btnSalvar.Text = "Salvar"
        Me.btnSalvar.UseVisualStyleBackColor = True
        '
        'btnNovo
        '
        Me.btnNovo.Location = New System.Drawing.Point(127, 13)
        Me.btnNovo.Name = "btnNovo"
        Me.btnNovo.Size = New System.Drawing.Size(109, 44)
        Me.btnNovo.TabIndex = 7
        Me.btnNovo.Text = "Novo"
        Me.btnNovo.UseVisualStyleBackColor = True
        '
        'btnExcluir
        '
        Me.btnExcluir.Location = New System.Drawing.Point(242, 13)
        Me.btnExcluir.Name = "btnExcluir"
        Me.btnExcluir.Size = New System.Drawing.Size(109, 44)
        Me.btnExcluir.TabIndex = 8
        Me.btnExcluir.Text = "Excluir"
        Me.btnExcluir.UseVisualStyleBackColor = True
        '
        'btnCancelar
        '
        Me.btnCancelar.Location = New System.Drawing.Point(659, 12)
        Me.btnCancelar.Name = "btnCancelar"
        Me.btnCancelar.Size = New System.Drawing.Size(109, 44)
        Me.btnCancelar.TabIndex = 9
        Me.btnCancelar.Text = "Cancelar"
        Me.btnCancelar.UseVisualStyleBackColor = True
        '
        'dgvDados
        '
        Me.dgvDados.AllowUserToAddRows = False
        Me.dgvDados.AllowUserToDeleteRows = False
        Me.dgvDados.AllowUserToOrderColumns = True
        Me.dgvDados.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dgvDados.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCellsExceptHeader
        Me.dgvDados.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders
        Me.dgvDados.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvDados.Location = New System.Drawing.Point(12, 285)
        Me.dgvDados.Name = "dgvDados"
        Me.dgvDados.ReadOnly = True
        Me.dgvDados.RowHeadersWidth = 51
        Me.dgvDados.RowTemplate.Height = 24
        Me.dgvDados.Size = New System.Drawing.Size(755, 326)
        Me.dgvDados.TabIndex = 10
        '
        'TimerdgvDados
        '
        '
        'btnBuscarArquivo
        '
        Me.btnBuscarArquivo.Location = New System.Drawing.Point(688, 144)
        Me.btnBuscarArquivo.Name = "btnBuscarArquivo"
        Me.btnBuscarArquivo.Size = New System.Drawing.Size(75, 23)
        Me.btnBuscarArquivo.TabIndex = 11
        Me.btnBuscarArquivo.Text = "Buscar"
        Me.btnBuscarArquivo.UseVisualStyleBackColor = True
        '
        'frmFichaTecnica
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(779, 623)
        Me.Controls.Add(Me.btnBuscarArquivo)
        Me.Controls.Add(Me.dgvDados)
        Me.Controls.Add(Me.btnCancelar)
        Me.Controls.Add(Me.btnExcluir)
        Me.Controls.Add(Me.btnNovo)
        Me.Controls.Add(Me.btnSalvar)
        Me.Controls.Add(Me.txtDescricaoArquivo)
        Me.Controls.Add(Me.txtEnderecoArquivo)
        Me.Controls.Add(Me.txtnomeArquivo)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Name = "frmFichaTecnica"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Associação de Ficha Tecnica"
        CType(Me.dgvDados, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents Label1 As Windows.Forms.Label
    Friend WithEvents Label2 As Windows.Forms.Label
    Friend WithEvents Label3 As Windows.Forms.Label
    Friend WithEvents txtnomeArquivo As Windows.Forms.TextBox
    Friend WithEvents txtEnderecoArquivo As Windows.Forms.TextBox
    Friend WithEvents txtDescricaoArquivo As Windows.Forms.TextBox
    Friend WithEvents btnSalvar As Windows.Forms.Button
    Friend WithEvents btnNovo As Windows.Forms.Button
    Friend WithEvents btnExcluir As Windows.Forms.Button
    Friend WithEvents btnCancelar As Windows.Forms.Button
    Friend WithEvents dgvDados As Windows.Forms.DataGridView
    Friend WithEvents TimerdgvDados As Windows.Forms.Timer
    Friend WithEvents btnBuscarArquivo As Windows.Forms.Button
End Class
