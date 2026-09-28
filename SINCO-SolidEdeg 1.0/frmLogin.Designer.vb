<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmLogin
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmLogin))
        Me.pnlHeader = New System.Windows.Forms.Panel()
        Me.pnlHeaderAccent = New System.Windows.Forms.Panel()
        Me.PictureBox2 = New System.Windows.Forms.PictureBox()
        Me.lblSubtitulo = New System.Windows.Forms.Label()
        Me.lblDescricao = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.txtLogin = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.mskSenha = New System.Windows.Forms.MaskedTextBox()
        Me.chkSalvarDadosEntrada = New System.Windows.Forms.CheckBox()
        Me.chkMostrarSenha = New System.Windows.Forms.CheckBox()
        Me.btnEntrar = New System.Windows.Forms.Button()
        Me.btnFechar = New System.Windows.Forms.Button()
        Me.lblFooter = New System.Windows.Forms.Label()
        Me.pnlHeader.SuspendLayout()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'pnlHeader
        '
        Me.pnlHeader.BackColor = System.Drawing.Color.White
        Me.pnlHeader.Controls.Add(Me.pnlHeaderAccent)
        Me.pnlHeader.Controls.Add(Me.PictureBox2)
        Me.pnlHeader.Controls.Add(Me.lblSubtitulo)
        Me.pnlHeader.Controls.Add(Me.lblDescricao)
        Me.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top
        Me.pnlHeader.Location = New System.Drawing.Point(0, 0)
        Me.pnlHeader.Name = "pnlHeader"
        Me.pnlHeader.Size = New System.Drawing.Size(430, 116)
        Me.pnlHeader.TabIndex = 6
        '
        'pnlHeaderAccent
        '
        Me.pnlHeaderAccent.BackColor = System.Drawing.Color.FromArgb(CType(CType(37, Byte), Integer), CType(CType(99, Byte), Integer), CType(CType(235, Byte), Integer))
        Me.pnlHeaderAccent.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.pnlHeaderAccent.Location = New System.Drawing.Point(0, 114)
        Me.pnlHeaderAccent.Name = "pnlHeaderAccent"
        Me.pnlHeaderAccent.Size = New System.Drawing.Size(430, 2)
        Me.pnlHeaderAccent.TabIndex = 3
        '
        'PictureBox2
        '
        Me.PictureBox2.BackColor = System.Drawing.Color.Transparent
        Me.PictureBox2.Image = CType(resources.GetObject("PictureBox2.Image"), System.Drawing.Image)
        Me.PictureBox2.Location = New System.Drawing.Point(125, 12)
        Me.PictureBox2.Name = "PictureBox2"
        Me.PictureBox2.Size = New System.Drawing.Size(180, 48)
        Me.PictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.PictureBox2.TabIndex = 0
        Me.PictureBox2.TabStop = False
        '
        'lblSubtitulo
        '
        Me.lblSubtitulo.BackColor = System.Drawing.Color.Transparent
        Me.lblSubtitulo.Font = New System.Drawing.Font("Segoe UI", 11.5!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSubtitulo.ForeColor = System.Drawing.Color.Navy
        Me.lblSubtitulo.Location = New System.Drawing.Point(12, 64)
        Me.lblSubtitulo.Name = "lblSubtitulo"
        Me.lblSubtitulo.Size = New System.Drawing.Size(406, 24)
        Me.lblSubtitulo.TabIndex = 1
        Me.lblSubtitulo.Text = "PORTAL METALFISA"
        Me.lblSubtitulo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblDescricao
        '
        Me.lblDescricao.BackColor = System.Drawing.Color.Transparent
        Me.lblDescricao.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDescricao.ForeColor = System.Drawing.Color.Navy
        Me.lblDescricao.Location = New System.Drawing.Point(12, 88)
        Me.lblDescricao.Name = "lblDescricao"
        Me.lblDescricao.Size = New System.Drawing.Size(406, 23)
        Me.lblDescricao.TabIndex = 2
        Me.lblDescricao.Text = "Sistema Integrado CAD / Solid Edge"
        Me.lblDescricao.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.BackColor = System.Drawing.Color.Transparent
        Me.Label3.Font = New System.Drawing.Font("Segoe UI", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.ForeColor = System.Drawing.Color.FromArgb(CType(CType(100, Byte), Integer), CType(CType(116, Byte), Integer), CType(CType(139, Byte), Integer))
        Me.Label3.Location = New System.Drawing.Point(36, 134)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(128, 19)
        Me.Label3.TabIndex = 7
        Me.Label3.Text = "USUÁRIO / LOGIN"
        '
        'txtLogin
        '
        Me.txtLogin.BackColor = System.Drawing.Color.White
        Me.txtLogin.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtLogin.Font = New System.Drawing.Font("Segoe UI", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtLogin.ForeColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(41, Byte), Integer), CType(CType(59, Byte), Integer))
        Me.txtLogin.Location = New System.Drawing.Point(36, 153)
        Me.txtLogin.Name = "txtLogin"
        Me.txtLogin.Size = New System.Drawing.Size(358, 32)
        Me.txtLogin.TabIndex = 0
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.BackColor = System.Drawing.Color.Transparent
        Me.Label4.Font = New System.Drawing.Font("Segoe UI", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.ForeColor = System.Drawing.Color.FromArgb(CType(CType(100, Byte), Integer), CType(CType(116, Byte), Integer), CType(CType(139, Byte), Integer))
        Me.Label4.Location = New System.Drawing.Point(36, 192)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(56, 19)
        Me.Label4.TabIndex = 8
        Me.Label4.Text = "SENHA"
        '
        'mskSenha
        '
        Me.mskSenha.BackColor = System.Drawing.Color.White
        Me.mskSenha.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.mskSenha.Font = New System.Drawing.Font("Segoe UI", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.mskSenha.ForeColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(41, Byte), Integer), CType(CType(59, Byte), Integer))
        Me.mskSenha.Location = New System.Drawing.Point(36, 211)
        Me.mskSenha.Name = "mskSenha"
        Me.mskSenha.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.mskSenha.Size = New System.Drawing.Size(358, 32)
        Me.mskSenha.TabIndex = 1
        '
        'chkSalvarDadosEntrada
        '
        Me.chkSalvarDadosEntrada.AutoSize = True
        Me.chkSalvarDadosEntrada.BackColor = System.Drawing.Color.Transparent
        Me.chkSalvarDadosEntrada.Font = New System.Drawing.Font("Segoe UI", 8.5!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkSalvarDadosEntrada.ForeColor = System.Drawing.Color.FromArgb(CType(CType(71, Byte), Integer), CType(CType(85, Byte), Integer), CType(CType(105, Byte), Integer))
        Me.chkSalvarDadosEntrada.Location = New System.Drawing.Point(36, 248)
        Me.chkSalvarDadosEntrada.Name = "chkSalvarDadosEntrada"
        Me.chkSalvarDadosEntrada.Size = New System.Drawing.Size(200, 24)
        Me.chkSalvarDadosEntrada.TabIndex = 2
        Me.chkSalvarDadosEntrada.Text = "Lembrar dados de acesso"
        Me.chkSalvarDadosEntrada.UseVisualStyleBackColor = False
        '
        'chkMostrarSenha
        '
        Me.chkMostrarSenha.AutoSize = True
        Me.chkMostrarSenha.BackColor = System.Drawing.Color.Transparent
        Me.chkMostrarSenha.Font = New System.Drawing.Font("Segoe UI", 8.5!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkMostrarSenha.ForeColor = System.Drawing.Color.FromArgb(CType(CType(71, Byte), Integer), CType(CType(85, Byte), Integer), CType(CType(105, Byte), Integer))
        Me.chkMostrarSenha.Location = New System.Drawing.Point(274, 248)
        Me.chkMostrarSenha.Name = "chkMostrarSenha"
        Me.chkMostrarSenha.Size = New System.Drawing.Size(124, 24)
        Me.chkMostrarSenha.TabIndex = 3
        Me.chkMostrarSenha.Text = "Mostrar senha"
        Me.chkMostrarSenha.UseVisualStyleBackColor = False
        '
        'btnEntrar
        '
        Me.btnEntrar.BackColor = System.Drawing.Color.FromArgb(CType(CType(31, Byte), Integer), CType(CType(54, Byte), Integer), CType(CType(92, Byte), Integer))
        Me.btnEntrar.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btnEntrar.FlatAppearance.BorderSize = 0
        Me.btnEntrar.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnEntrar.Font = New System.Drawing.Font("Segoe UI", 9.5!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnEntrar.ForeColor = System.Drawing.Color.White
        Me.btnEntrar.Location = New System.Drawing.Point(36, 282)
        Me.btnEntrar.Name = "btnEntrar"
        Me.btnEntrar.Size = New System.Drawing.Size(172, 38)
        Me.btnEntrar.TabIndex = 4
        Me.btnEntrar.Text = "Entrar no Sistema"
        Me.btnEntrar.UseVisualStyleBackColor = False
        '
        'btnFechar
        '
        Me.btnFechar.BackColor = System.Drawing.Color.FromArgb(CType(CType(226, Byte), Integer), CType(CType(232, Byte), Integer), CType(CType(240, Byte), Integer))
        Me.btnFechar.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btnFechar.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnFechar.FlatAppearance.BorderSize = 0
        Me.btnFechar.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnFechar.Font = New System.Drawing.Font("Segoe UI", 9.5!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnFechar.ForeColor = System.Drawing.Color.FromArgb(CType(CType(71, Byte), Integer), CType(CType(85, Byte), Integer), CType(CType(105, Byte), Integer))
        Me.btnFechar.Location = New System.Drawing.Point(222, 282)
        Me.btnFechar.Name = "btnFechar"
        Me.btnFechar.Size = New System.Drawing.Size(172, 38)
        Me.btnFechar.TabIndex = 5
        Me.btnFechar.Text = "Fechar"
        Me.btnFechar.UseVisualStyleBackColor = False
        '
        'lblFooter
        '
        Me.lblFooter.BackColor = System.Drawing.Color.Transparent
        Me.lblFooter.Font = New System.Drawing.Font("Segoe UI", 7.5!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblFooter.ForeColor = System.Drawing.Color.FromArgb(CType(CType(148, Byte), Integer), CType(CType(163, Byte), Integer), CType(CType(184, Byte), Integer))
        Me.lblFooter.Location = New System.Drawing.Point(0, 336)
        Me.lblFooter.Name = "lblFooter"
        Me.lblFooter.Size = New System.Drawing.Size(430, 16)
        Me.lblFooter.TabIndex = 9
        Me.lblFooter.Text = "SINCO v1.0 • Metalfisa — Solid Edge Integration"
        Me.lblFooter.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'frmLogin
        '
        Me.AcceptButton = Me.btnEntrar
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 20.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(248, Byte), Integer), CType(CType(250, Byte), Integer), CType(CType(252, Byte), Integer))
        Me.CancelButton = Me.btnFechar
        Me.ClientSize = New System.Drawing.Size(430, 360)
        Me.Controls.Add(Me.lblFooter)
        Me.Controls.Add(Me.btnFechar)
        Me.Controls.Add(Me.btnEntrar)
        Me.Controls.Add(Me.chkMostrarSenha)
        Me.Controls.Add(Me.chkSalvarDadosEntrada)
        Me.Controls.Add(Me.mskSenha)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.txtLogin)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.pnlHeader)
        Me.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmLogin"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Portal Metalfisa - Autenticação"
        Me.pnlHeader.ResumeLayout(False)
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents pnlHeader As Windows.Forms.Panel
    Friend WithEvents pnlHeaderAccent As Windows.Forms.Panel
    Friend WithEvents PictureBox2 As Windows.Forms.PictureBox
    Friend WithEvents lblSubtitulo As Windows.Forms.Label
    Friend WithEvents lblDescricao As Windows.Forms.Label
    Friend WithEvents Label3 As Windows.Forms.Label
    Friend WithEvents txtLogin As Windows.Forms.TextBox
    Friend WithEvents Label4 As Windows.Forms.Label
    Friend WithEvents mskSenha As Windows.Forms.MaskedTextBox
    Friend WithEvents chkSalvarDadosEntrada As Windows.Forms.CheckBox
    Friend WithEvents chkMostrarSenha As Windows.Forms.CheckBox
    Friend WithEvents btnEntrar As Windows.Forms.Button
    Friend WithEvents btnFechar As Windows.Forms.Button
    Friend WithEvents lblFooter As Windows.Forms.Label
End Class
