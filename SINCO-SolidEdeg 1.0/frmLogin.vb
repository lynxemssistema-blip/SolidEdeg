Imports System.Windows.Forms
Imports System.Drawing

Public Class frmLogin

    Private Sub frmLogin_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        My.Settings.TipoConexao = "MYSQL"

        Try
            cl_BancoDados.AbrirBanco()
        Catch ex As Exception
        End Try

        Me.Text = "Portal Metalfisa - Autenticação"

        ' Preenche credenciais lembradas se configurado
        If Not String.IsNullOrEmpty(My.Settings.UsuarioLogado) Then
            Me.txtLogin.Text = My.Settings.UsuarioLogado
            Me.mskSenha.Text = My.Settings.SenhaUsuarioLogado
            Me.chkSalvarDadosEntrada.Checked = True
            Me.btnEntrar.Select()
        Else
            Me.txtLogin.Text = String.Empty
            Me.mskSenha.Text = String.Empty
            Me.chkSalvarDadosEntrada.Checked = False
            Me.txtLogin.Select()
        End If
    End Sub

    Private Sub btnEntrar_Click(sender As Object, e As EventArgs) Handles btnEntrar.Click
        Try
            If String.IsNullOrWhiteSpace(txtLogin.Text) Then
                MessageBox.Show("Por favor, informe o login do usuário.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                txtLogin.Focus()
                Exit Sub
            End If

            If String.IsNullOrWhiteSpace(mskSenha.Text) Then
                MessageBox.Show("Por favor, informe a senha de acesso.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                mskSenha.Focus()
                Exit Sub
            End If

            Usuario.RetornaDadosConfiguracao()

            If Usuario.RetornaDadosUsuario(Me.txtLogin.Text.Trim(), Me.mskSenha.Text.Trim()) Then

                If chkSalvarDadosEntrada.Checked Then
                    My.Settings.UsuarioLogado = Me.txtLogin.Text.Trim()
                    My.Settings.SenhaUsuarioLogado = Me.mskSenha.Text.Trim()
                Else
                    My.Settings.UsuarioLogado = Nothing
                    My.Settings.SenhaUsuarioLogado = Nothing
                End If

                My.Settings.Save()

                Me.DialogResult = DialogResult.OK
                Me.Close()
            Else
                MessageBox.Show("Usuário ou senha inválidos, ou conta pendente de aprovação!", "Falha de Autenticação", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                mskSenha.Clear()
                mskSenha.Focus()
            End If

        Catch ex As Exception
            MessageBox.Show("Erro ao autenticar usuário: " & ex.Message, "Erro no Sistema", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnFechar_Click(sender As Object, e As EventArgs) Handles btnFechar.Click
        Me.Close()
        Application.Exit()
    End Sub

    Private Sub chkMostrarSenha_CheckedChanged(sender As Object, e As EventArgs) Handles chkMostrarSenha.CheckedChanged
        If chkMostrarSenha.Checked Then
            mskSenha.PasswordChar = ControlChars.NullChar
        Else
            mskSenha.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42) ' "*"
        End If
    End Sub

    Private Sub chkSalvarDadosEntrada_CheckedChanged(sender As Object, e As EventArgs) Handles chkSalvarDadosEntrada.CheckedChanged
        If chkSalvarDadosEntrada.Checked Then
            My.Settings.UsuarioLogado = Me.txtLogin.Text.Trim()
            My.Settings.SenhaUsuarioLogado = Me.mskSenha.Text.Trim()
        Else
            My.Settings.UsuarioLogado = Nothing
            My.Settings.SenhaUsuarioLogado = Nothing
        End If
        My.Settings.Save()
    End Sub

End Class