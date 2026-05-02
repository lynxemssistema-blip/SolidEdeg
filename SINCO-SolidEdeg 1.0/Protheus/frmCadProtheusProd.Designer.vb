<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmCadProtheusProd
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
        Me.btnSalvar = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.cboB1_TIPO = New System.Windows.Forms.ComboBox()
        Me.cboB1_UM = New System.Windows.Forms.ComboBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.cboB1_GRUPO = New System.Windows.Forms.ComboBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.txtB1_XREVM = New System.Windows.Forms.TextBox()
        Me.SuspendLayout()
        '
        'btnSalvar
        '
        Me.btnSalvar.Image = Global.SINCO_SolidEdeg_1._0.My.Resources.Resources.salvar
        Me.btnSalvar.Location = New System.Drawing.Point(12, 11)
        Me.btnSalvar.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.btnSalvar.Name = "btnSalvar"
        Me.btnSalvar.Size = New System.Drawing.Size(149, 50)
        Me.btnSalvar.TabIndex = 55
        Me.btnSalvar.Text = "Salvar - Protheus"
        Me.btnSalvar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.btnSalvar.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(9, 77)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(38, 16)
        Me.Label1.TabIndex = 56
        Me.Label1.Text = "Tipo:"
        '
        'cboB1_TIPO
        '
        Me.cboB1_TIPO.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboB1_TIPO.FormattingEnabled = True
        Me.cboB1_TIPO.Location = New System.Drawing.Point(12, 96)
        Me.cboB1_TIPO.Name = "cboB1_TIPO"
        Me.cboB1_TIPO.Size = New System.Drawing.Size(322, 24)
        Me.cboB1_TIPO.TabIndex = 57
        '
        'cboB1_UM
        '
        Me.cboB1_UM.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboB1_UM.FormattingEnabled = True
        Me.cboB1_UM.Location = New System.Drawing.Point(12, 155)
        Me.cboB1_UM.Name = "cboB1_UM"
        Me.cboB1_UM.Size = New System.Drawing.Size(322, 24)
        Me.cboB1_UM.TabIndex = 59
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(9, 136)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(62, 16)
        Me.Label2.TabIndex = 58
        Me.Label2.Text = "Unidade:"
        '
        'cboB1_GRUPO
        '
        Me.cboB1_GRUPO.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboB1_GRUPO.FormattingEnabled = True
        Me.cboB1_GRUPO.Location = New System.Drawing.Point(12, 213)
        Me.cboB1_GRUPO.Name = "cboB1_GRUPO"
        Me.cboB1_GRUPO.Size = New System.Drawing.Size(322, 24)
        Me.cboB1_GRUPO.TabIndex = 61
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(9, 194)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(47, 16)
        Me.Label3.TabIndex = 60
        Me.Label3.Text = "Grupo:"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(355, 194)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(95, 16)
        Me.Label4.TabIndex = 62
        Me.Label4.Text = "Rev. Metalfisa:"
        '
        'txtB1_XREVM
        '
        Me.txtB1_XREVM.Location = New System.Drawing.Point(350, 213)
        Me.txtB1_XREVM.Name = "txtB1_XREVM"
        Me.txtB1_XREVM.Size = New System.Drawing.Size(100, 22)
        Me.txtB1_XREVM.TabIndex = 63
        Me.txtB1_XREVM.Text = "00"
        '
        'frmCadProtheusProd
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(962, 254)
        Me.Controls.Add(Me.txtB1_XREVM)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.cboB1_GRUPO)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.cboB1_UM)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.cboB1_TIPO)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.btnSalvar)
        Me.Name = "frmCadProtheusProd"
        Me.Text = "Cadastro Direto Protheus"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents btnSalvar As Windows.Forms.Button
    Friend WithEvents Label1 As Windows.Forms.Label
    Friend WithEvents cboB1_TIPO As Windows.Forms.ComboBox
    Friend WithEvents cboB1_UM As Windows.Forms.ComboBox
    Friend WithEvents Label2 As Windows.Forms.Label
    Friend WithEvents cboB1_GRUPO As Windows.Forms.ComboBox
    Friend WithEvents Label3 As Windows.Forms.Label
    Friend WithEvents Label4 As Windows.Forms.Label
    Friend WithEvents txtB1_XREVM As Windows.Forms.TextBox
End Class
