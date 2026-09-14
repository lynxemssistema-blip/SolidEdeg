Imports System.Windows.Forms
Imports System.Drawing

Public Class frmLogin

    Private Sub frmLogin_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' 🔹 Estiliza a interface com a identidade visual moderna do app direto
        EstilizarInterface()

        My.Settings.TipoConexao = "MYSQL"

        Try
            cl_BancoDados.AbrirBanco()
        Catch ex As Exception
        Finally
        End Try


        '  cl_BancoDados.ComboBoxDataSet("usuario", "IdUsuario", "Login", cboLogin1, " WHERE (D_E_L_E_T_E = '' OR D_E_L_E_T_E IS NULL )", "")

        Me.Text = "Sistema SINCO - Lynx - Cliente: " & My.Settings.BancoDadosAtivo

        If My.Settings.UsuarioLogado <> "" Then

            Me.txtLogin.Text = My.Settings.UsuarioLogado
            Me.mskSenha.Text = My.Settings.SenhaUsuarioLogado
            Me.chkSalvarDadosEntrada.Checked = True
        Else

            Me.txtLogin.Text = Nothing
            Me.mskSenha.Text = Nothing
            Me.chkSalvarDadosEntrada.Checked = False

        End If

        '        'dgvListaUsuario.DataSource = cl_BancoDados.CarregarDados("SELECT
        '    IdUsuario,
        '    NomeCompleto,
        '    Login,
        '    Senha,
        '    Sigla
        'FROM
        '     " & ComplementoTipoBanco & " usuario
        'WHERE
        '    (D_E_L_E_T_E <> '*' OR D_E_L_E_T_E IS NULL)
        '    AND Login = Senha order by IdUsuario")

    End Sub

    Private Sub btnEntrar_Click(sender As Object, e As EventArgs) Handles btnEntrar.Click


        Try

            Dim TotalUsuario As String
            'Verificar a quantide de usuarios cadastrado no sistema
            cl_BancoDados.RetornaCampoDaPesquisa("SELECT count(IdUsuario) as qtde FROM usuario", "qtde")

            TotalUsuario = VCampo0

            Usuario.RetornaDadosConfiguracao()

            Dim entrar As Boolean

            If Usuario.RetornaDadosUsuario(Me.txtLogin.Text, Me.mskSenha.Text) = True Then

                entrar = True

                'MyTaskPanelHost.txtPesqCriadoPor.Text = Usuario.Login

                If chkSalvarDadosEntrada.Checked = True Then

                    My.Settings.UsuarioLogado = Me.txtLogin.Text
                    My.Settings.SenhaUsuarioLogado = Me.mskSenha.Text
                    ' My.Settings.TipoUsuario = "A"
                Else

                    My.Settings.UsuarioLogado = Nothing
                    My.Settings.SenhaUsuarioLogado = Nothing
                    'My.Settings.TipoUsuario = Nothing

                End If

                ' Me.Hide()

                My.Settings.Save()
                '  BancoDados.FecharBanco()

                Me.Hide()

                MostrarFormulario(frm)

                ' F'ormulariofrmPrincial.ShowDialog()
            Else

                entrar = False

                MsgBox("Usuário, senha inválidos, ou conta pendente de aprovação!", vbInformation, "Falha ao entrar no sistema SINCO !!")

            End If

        Catch ex As Exception

            MsgBox("Usuário ou senha inválidos", vbInformation, "Falha ao entrar no sitema SINCO !!")

        Finally

        End Try

    End Sub

    Private Sub btnFechar_Click(sender As Object, e As EventArgs) Handles btnFechar.Click



        Me.Close()

        Application.Exit()

    End Sub

    Private Sub chkMostrarSenha_CheckedChanged(sender As Object, e As EventArgs) Handles chkMostrarSenha.CheckedChanged

        If chkMostrarSenha.Checked = False Then

            mskSenha.PasswordChar = "********"
        Else

            mskSenha.PasswordChar = Nothing

        End If

    End Sub

    Private Sub chkSalvarDadosEntrada_CheckedChanged(sender As Object, e As EventArgs) Handles chkSalvarDadosEntrada.CheckedChanged

        If chkSalvarDadosEntrada.Checked = True Then

            My.Settings.UsuarioLogado = Me.txtLogin.Text
            My.Settings.SenhaUsuarioLogado = Me.mskSenha.Text
        Else

            My.Settings.UsuarioLogado = Nothing
            My.Settings.SenhaUsuarioLogado = Nothing

        End If

        My.Settings.Save()

    End Sub

    Private Sub btnRecuperarSenha_Click(sender As Object, e As EventArgs) Handles btnRecuperarSenha.Click

        Dim email As String = InputBox("Digite o e-mail associado à sua conta:", "Recuperação de Senha")

        If String.IsNullOrEmpty(email) Then
            MsgBox("E-mail não pode ser vazio.", MsgBoxStyle.Exclamation, "Recuperação de Senha")
            Return
            Exit Sub
        End If

        ' Verifica se o e-mail existe no banco de dados
        cl_BancoDados.RetornaCampoDaPesquisa("SELECT Email,NomeCompleto,Login,Senha FROM usuario WHERE Email ='" & email & "'", "Email", "NomeCompleto", "Login", "Senha")

        If VCampo0.ToString <> "" Then

            Usuario.email = VCampo0
            Usuario.NomeCompleto = VCampo1
            Usuario.Login = VCampo2
            Usuario.Senha = VCampo3

            Dim corpoEmail As String = "Olá " & VCampo1 & "," & vbCrLf & vbCrLf &
                    "Aqui estão os detalhes da sua conta:" & vbCrLf &
                    "Login: " & Usuario.Login & vbCrLf &
                    "Senha: " & Usuario.Senha & vbCrLf & vbCrLf &
                    "Se você não solicitou essa recuperação de senha,
                     por favor, ignore este e-mail." & vbCrLf &
                    "Atenciosamente," & vbCrLf &
                    "Equipe de Suporte"

            If My.Settings.BancoDadosAtivo = "alfatec2" Then

                '       ClasseEmail.EmailAlfatec(Usuario.email, Usuario.NomeCompleto, corpoEmail, "Recuperação de senha")

                MsgBox("Em instantes você recebera o e-mail de recuperação de senha", vbInformation, "Atenção")

            ElseIf My.Settings.BancoDadosAtivo = "amceletrica" Then

                '     ClasseEmail.EnviarEmailamc("arquivos.engenharia@amcsolucoes.com.br", "Recuperação de Senha", email, corpoEmail)
                MsgBox("E-mail enviado com sucesso para " & email, MsgBoxStyle.Information, "Recuperação de Senha")
            End If
        Else

            MsgBox("Email não cadastro!", vbInformation, "Recuperação de Senha")

        End If

    End Sub

    Private Sub PictureBox2_DoubleClick(sender As Object, e As EventArgs) Handles PictureBox2.DoubleClick
        cl_BancoDados.BuscarArquivoconf()
    End Sub

    Private Sub EstilizarInterface()
        ' 1. Configurações gerais do formulário
        Me.FormBorderStyle = FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.BackColor = Color.FromArgb(248, 250, 252) ' slate-50 (cinza claro moderno)
        Me.ClientSize = New Size(430, 420)
        Me.Text = "Portal Metalfisa"

        ' 2. Ajuste do cabeçalho e logo
        Me.PictureBox2.BackColor = Color.Transparent
        Me.PictureBox2.Location = New Point(125, 12)
        Me.PictureBox2.Size = New Size(180, 52)
        Me.PictureBox2.SizeMode = PictureBoxSizeMode.Zoom

        ' Adiciona rótulos textuais de cabeçalho dinamicamente
        Dim lblSubtitulo As New Label()
        lblSubtitulo.Text = "Portal Metalfisa"
        lblSubtitulo.Font = New Font("Segoe UI", 12, FontStyle.Bold)
        lblSubtitulo.ForeColor = Color.White
        lblSubtitulo.BackColor = Color.Transparent
        lblSubtitulo.TextAlign = ContentAlignment.MiddleCenter
        lblSubtitulo.Location = New Point(0, 72)
        lblSubtitulo.Size = New Size(Me.ClientSize.Width, 22)
        Me.Controls.Add(lblSubtitulo)
        lblSubtitulo.BringToFront()

        Dim lblDescricao As New Label()
        lblDescricao.Text = "Acesso Restrito ao Sistema"
        lblDescricao.Font = New Font("Segoe UI", 8.5, FontStyle.Regular)
        lblDescricao.ForeColor = Color.FromArgb(200, 220, 255)
        lblDescricao.BackColor = Color.Transparent
        lblDescricao.TextAlign = ContentAlignment.MiddleCenter
        lblDescricao.Location = New Point(0, 94)
        lblDescricao.Size = New Size(Me.ClientSize.Width, 18)
        Me.Controls.Add(lblDescricao)
        lblDescricao.BringToFront()

        ' Desenha o fundo azul escuro do cabeçalho (#1F365C)
        AddHandler Me.Paint, Sub(s, e)
                                 Dim headerRect As New Rectangle(0, 0, Me.ClientSize.Width, 120)
                                 Using brush As New SolidBrush(Color.FromArgb(31, 54, 92))
                                     e.Graphics.FillRectangle(brush, headerRect)
                                 End Using
                                 Using pen As New Pen(Color.FromArgb(40, 60, 110), 1)
                                     e.Graphics.DrawLine(pen, 0, 120, Me.ClientSize.Width, 120)
                                 End Using
                             End Sub

        ' 3. Ocular ícones legados
        Me.Label1.Visible = False
        Me.Label2.Visible = False

        ' 4. Posicionar e estilizar rótulos e campos de texto
        ' Login
        Me.Label3.Text = "USUÁRIO / LOGIN"
        Me.Label3.Font = New Font("Segoe UI", 8.5, FontStyle.Bold)
        Me.Label3.ForeColor = Color.FromArgb(100, 116, 139) ' slate-500
        Me.Label3.Location = New Point(40, 138)
        Me.Label3.Size = New Size(350, 18)

        Me.txtLogin.Font = New Font("Segoe UI", 11)
        Me.txtLogin.Location = New Point(40, 158)
        Me.txtLogin.Size = New Size(350, 28)
        Me.txtLogin.BackColor = Color.White
        Me.txtLogin.ForeColor = Color.FromArgb(30, 41, 59) ' slate-800

        ' Senha
        Me.Label4.Text = "SENHA"
        Me.Label4.Font = New Font("Segoe UI", 8.5, FontStyle.Bold)
        Me.Label4.ForeColor = Color.FromArgb(100, 116, 139)
        Me.Label4.Location = New Point(40, 200)
        Me.Label4.Size = New Size(350, 18)

        Me.mskSenha.Font = New Font("Segoe UI", 11)
        Me.mskSenha.PasswordChar = "*"c
        Me.mskSenha.Location = New Point(40, 220)
        Me.mskSenha.Size = New Size(350, 28)
        Me.mskSenha.BackColor = Color.White
        Me.mskSenha.ForeColor = Color.FromArgb(30, 41, 59)

        ' 5. Estilizar CheckBoxes
        Me.chkSalvarDadosEntrada.Text = "Salvar dados de acesso"
        Me.chkSalvarDadosEntrada.Font = New Font("Segoe UI", 9)
        Me.chkSalvarDadosEntrada.ForeColor = Color.FromArgb(71, 85, 105)
        Me.chkSalvarDadosEntrada.Location = New Point(40, 260)
        Me.chkSalvarDadosEntrada.Size = New Size(180, 20)

        Me.chkMostrarSenha.Text = "Mostrar senha"
        Me.chkMostrarSenha.Font = New Font("Segoe UI", 9)
        Me.chkMostrarSenha.ForeColor = Color.FromArgb(71, 85, 105)
        Me.chkMostrarSenha.Location = New Point(250, 260)
        Me.chkMostrarSenha.Size = New Size(140, 20)

        ' 6. Estilizar Botões
        ' btnEntrar
        Me.btnEntrar.Text = "Entrar no Sistema"
        Me.btnEntrar.FlatStyle = FlatStyle.Flat
        Me.btnEntrar.FlatAppearance.BorderSize = 0
        Me.btnEntrar.BackColor = Color.FromArgb(31, 54, 92)
        Me.btnEntrar.ForeColor = Color.White
        Me.btnEntrar.Font = New Font("Segoe UI", 10, FontStyle.Bold)
        Me.btnEntrar.Location = New Point(40, 300)
        Me.btnEntrar.Size = New Size(165, 36)
        Me.btnEntrar.Cursor = Cursors.Hand

        ' btnFechar
        Me.btnFechar.Text = "Fechar"
        Me.btnFechar.FlatStyle = FlatStyle.Flat
        Me.btnFechar.FlatAppearance.BorderSize = 0
        Me.btnFechar.BackColor = Color.FromArgb(226, 232, 240)
        Me.btnFechar.ForeColor = Color.FromArgb(71, 85, 105)
        Me.btnFechar.Font = New Font("Segoe UI", 10, FontStyle.Bold)
        Me.btnFechar.Location = New Point(225, 300)
        Me.btnFechar.Size = New Size(165, 36)
        Me.btnFechar.Cursor = Cursors.Hand

        ' btnRecuperarSenha
        Me.btnRecuperarSenha.Enabled = True
        Me.btnRecuperarSenha.Text = "Esqueceu a senha?"
        Me.btnRecuperarSenha.FlatStyle = FlatStyle.Flat
        Me.btnRecuperarSenha.FlatAppearance.BorderSize = 0
        Me.btnRecuperarSenha.BackColor = Color.Transparent
        Me.btnRecuperarSenha.ForeColor = Color.FromArgb(37, 99, 235)
        Me.btnRecuperarSenha.Font = New Font("Segoe UI", 9, FontStyle.Bold)
        Me.btnRecuperarSenha.Location = New Point(40, 355)
        Me.btnRecuperarSenha.Size = New Size(350, 26)
        Me.btnRecuperarSenha.Cursor = Cursors.Hand
        Me.btnRecuperarSenha.TextAlign = ContentAlignment.MiddleCenter
    End Sub
End Class