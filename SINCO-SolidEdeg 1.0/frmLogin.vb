Imports System.Windows.Forms

Public Class frmLogin

    Private Sub frmLogin_Load(sender As Object, e As EventArgs) Handles MyBase.Load


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

                '   MsgBox("Usuário ou senha inválidos!", vbInformation, "Falha ao entrar no sitema SINCO !!")

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
End Class