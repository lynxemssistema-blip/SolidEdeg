
Imports System.Data.OleDb
Imports System.Drawing
Imports System.Globalization
Imports System.IO
Imports System.Runtime.InteropServices
Imports System.Text
Imports System.Windows.Forms
Imports MySql.Data.MySqlClient
Imports Org.BouncyCastle.Asn1.Cms

Imports SolidEdgeFramework

Public Class ClBancoDados

    Public Function AbrirBanco() As Boolean
        ' Incrementa o contador de referências
        OpenConnectionsCount += 1

        ' Se a conexão já existe e está aberta, apenas retorna True
        If myconect IsNot Nothing AndAlso myconect.State = ConnectionState.Open Then
            Return True
        End If

        Try
            ' 🔹 Dados fixos do banco de dados remoto de produção
            Dim server As String = "193.203.175.68"
            Dim database As String = "u494795077_Metalfisa"
            Dim user As String = "u494795077_Metalfisa"
            Dim pass As String = "10207597Rdv*"

            conexao = "Server=" & server & ";database=" & database & ";uid=" & user & ";pwd=" & pass & ";Max Pool Size=50;Connection Timeout=600;Connection Lifetime=3600;CharSet=utf8;"

            ' Se o objeto for nulo ou a string de conexão mudou, cria um novo
            If myconect Is Nothing Then
                myconect = New MySqlConnection(conexao)
            Else
                If myconect.State = ConnectionState.Broken OrElse myconect.ConnectionString <> conexao Then
                    If myconect.State <> ConnectionState.Closed Then myconect.Close()
                    myconect.ConnectionString = conexao
                End If
            End If

            If myconect.State <> ConnectionState.Open Then
                myconect.Open()
            End If

            Return True
        Catch ex As Exception
            ' Em caso de erro, decrementa o contador pois a tentativa falhou
            OpenConnectionsCount = Math.Max(0, OpenConnectionsCount - 1)
            Return False
        End Try
    End Function

    Public Sub FecharBanco()
        Try
            ' Decrementa o contador de referências
            OpenConnectionsCount -= 1

            ' Só fecha fisicamente se o contador chegar a zero ou menos
            If OpenConnectionsCount <= 0 Then
                OpenConnectionsCount = 0 ' Garante que não fique negativo

                If myconect IsNot Nothing AndAlso myconect.State = ConnectionState.Open Then
                    myconect.Close()
                End If
            End If

        Catch ex As Exception
            ' MsgBox("Erro ao fechar banco: " & ex.Message, MsgBoxStyle.Exclamation)
        End Try
    End Sub


    ' Função para carregar dados em um DataTable
    Public Function CarregarDados(ByVal query As String) As DataTable

        Dim dtTabela As New System.Data.DataTable()

        Try

            cl_BancoDados.AbrirBanco()

            Try
                ' Cria um adaptador de dados
                Using adaptador As New MySqlDataAdapter(query, myconect)
                    ' Preenche o DataTable com os dados da consulta
                    adaptador.Fill(dtTabela)

                End Using
                'End Using
            Catch ex As MySqlException
                ' Trate erros específicos do MySQL
                ' MessageBox.Show("Erro ao carregar dados: " & ex.Message)
            Catch ex As Exception
                ' Trate outros erros
                ' MessageBox.Show("Erro inesperado: " & ex.Message)
            End Try



            Return dtTabela
        Catch ex As Exception
            Return dtTabela
        Finally
            FecharBanco()
        End Try

    End Function

    Public Function FormatarData(valor As Object) As String
        If IsDate(valor) Then
            Return CDate(valor).ToString("dd/MM/yyyy")
        Else
            Return valor?.ToString()
        End If
    End Function

    Public Sub PreencherCheckedListBox(ByVal query As String, ByVal clb As CheckedListBox)
        ' Limpa o CheckedListBox antes de preencher
        clb.Items.Clear()

        ' Chama a função CarregarDados para obter os dados
        Dim dt As System.Data.DataTable = cl_BancoDados.CarregarDados(query)

        Try

            ' Verifica se há dados retornados
            If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
                ' Itera sobre as linhas do DataTable e adiciona ao CheckedListBox
                For Each row As DataRow In dt.Rows
                    ' Supondo que a primeira coluna do DataTable seja o que você quer exibir
                    clb.Items.Add(row(0).ToString())
                Next
                ' Else
                ' MessageBox.Show("Nenhum dado encontrado.")
            End If
        Catch ex As Exception
        Finally
        End Try
    End Sub

    ' Executa com proteção contra exceção e nulos
    Public Sub SafeComboBoxFill(action As System.Action, alvo As String, frm As Form)
        If action Is Nothing Then Exit Sub
        Try
            action.Invoke()
            ' Garante SelectedIndex válido se houver items
            Dim cbo = TryCast(GetAlvoControl(alvo, frm), ComboBox)
            If cbo IsNot Nothing AndAlso cbo.Items IsNot Nothing AndAlso cbo.Items.Count > 0 AndAlso cbo.SelectedIndex < 0 Then
                cbo.SelectedIndex = 0
            End If
        Catch ex As Exception
            Debug.WriteLine($"SafeComboBoxFill[{alvo}]: {ex.Message}")
        End Try
    End Sub

    Public Sub SafeCheckedListFill(sql As String, chk As CheckedListBox, alvo As String, frm As Form)
        If chk Is Nothing OrElse String.IsNullOrWhiteSpace(sql) Then Exit Sub
        Try
            ' sua função já preenche a lista; apenas limpa antes por segurança
            chk.Items.Clear()
            PreencherCheckedListBox(sql, chk)
        Catch ex As Exception
            Debug.WriteLine($"SafeCheckedListFill[{alvo}]: {ex.Message}")
        End Try
    End Sub

    ' Utilitário opcional caso queira debugar um alvo específico por nome
    Public Function GetAlvoControl(alvo As String, frm As Form) As Control
        ' Ex: "cboProjeto (SQL)" → tenta achar "cboProjeto"
        If String.IsNullOrWhiteSpace(alvo) Then Return Nothing
        Dim nome = alvo.Split(" "c)(0).Trim()
        Return frm.Controls.Find(nome, True).FirstOrDefault()
    End Function

    Public Sub RetornaCampoDaPesquisa(ByVal Valor_Para_Pesquisa As String,
                                           ByVal Campo0 As String,
                                           Optional Campo1 As String = "",
                                           Optional Campo2 As String = "",
                                           Optional Campo3 As String = "",
                                           Optional Campo4 As String = "",
                                           Optional Campo5 As String = "",
                                           Optional Campo6 As String = "",
                                           Optional Campo7 As String = "",
                                           Optional Campo8 As String = "",
                                           Optional Campo9 As String = "",
                                           Optional Campo10 As String = "")
        Try
            VCampo0 = ""
            VCampo1 = ""
            VCampo2 = ""
            VCampo3 = ""
            VCampo4 = ""
            VCampo5 = ""
            VCampo6 = ""
            VCampo7 = ""
            VCampo8 = ""
            VCampo9 = ""
            VCampo10 = ""

            cl_BancoDados.AbrirBanco()

            Try
                ' Criação do comando SQL
                Dim daMysql As New MySqlCommand(Valor_Para_Pesquisa, myconect)

                ' Execução da consulta e leitura dos dados
                Using drMysql As MySqlDataReader = daMysql.ExecuteReader()
                    If drMysql.HasRows Then
                        drMysql.Read()

                        Try
                            VCampo0 = drMysql(Campo0).ToString()
                        Catch ex As Exception
                            VCampo0 = ""
                        End Try

                        Try
                            VCampo1 = drMysql(Campo1).ToString()
                        Catch ex As Exception
                            VCampo1 = ""
                        End Try

                        Try
                            VCampo2 = drMysql(Campo2).ToString()
                        Catch ex As Exception
                            VCampo2 = ""
                        End Try

                        Try
                            VCampo3 = drMysql(Campo3).ToString()
                        Catch ex As Exception
                            VCampo3 = ""
                        End Try

                        Try
                            VCampo4 = drMysql(Campo4).ToString()
                        Catch ex As Exception
                            VCampo4 = ""
                        End Try

                        Try
                            VCampo5 = drMysql(Campo5).ToString()
                        Catch ex As Exception
                            VCampo5 = ""
                        End Try

                        Try
                            VCampo6 = drMysql(Campo6).ToString()
                        Catch ex As Exception
                            VCampo6 = ""
                        End Try

                        Try
                            VCampo7 = drMysql(Campo7).ToString()
                        Catch ex As Exception
                            VCampo7 = ""
                        End Try

                        Try
                            VCampo8 = drMysql(Campo8).ToString()
                        Catch ex As Exception
                            VCampo8 = ""
                        End Try

                        Try
                            VCampo9 = drMysql(Campo9).ToString()
                        Catch ex As Exception
                            VCampo9 = ""
                        End Try

                        Try
                            VCampo10 = drMysql(Campo10).ToString()
                        Catch ex As Exception
                            VCampo10 = ""
                        End Try
                    End If
                End Using
            Catch ex As Exception
            End Try
        Catch ex As Exception
        Finally
            cl_BancoDados.FecharBanco()
        End Try

    End Sub



    Public Sub Salvar(ByVal funcaosql As String)

        Try

            cl_BancoDados.AbrirBanco()

            If My.Settings.TipoConexao = "MYSQL" Then

                Try

                    ' Usar "Using" para garantir o fechamento correto do comando e da conexão
                    Using mycomand As New MySqlCommand(funcaosql, myconect)
                        ' Executa o comando SQL
                        Dim rowsAffected As Integer = mycomand.ExecuteNonQuery()
                        ' Opcional: Pode usar rowsAffected para verificar quantas linhas foram afetadas
                    End Using
                Catch ex As MySqlException

                    'MessageBox.Show("Erro ao executar a operação no MySQL: " & ex.Message, "Erro de Banco de Dados", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Catch ex As Exception
                    ' Tratar erros gerais
                    'MessageBox.Show("Erro inesperado: " & ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Finally

                End Try

            End If

            cl_BancoDados.FecharBanco()
        Catch ex As Exception
        Finally
            cl_BancoDados.FecharBanco()
        End Try

    End Sub

    Public Sub SalvarParametros(ByVal sql As String, ByVal parametros As List(Of MySqlParameter))
        'Using conn As New MySqlConnection()
        'conn.Open()

        cl_BancoDados.AbrirBanco()
        Using cmd As New MySqlCommand(sql, myconect)
            For Each p In parametros
                cmd.Parameters.Add(p)
            Next
            cmd.ExecuteNonQuery()
        End Using

        cl_BancoDados.FecharBanco()
        ' End Using
    End Sub

    Function FormatarPara5Caracteres(numero As String) As String
        Return numero.PadLeft(5, "0")
    End Function

    Function FormatarPara7Caracteres(numero As String) As String
        Return numero.PadLeft(7, "0")
    End Function

    Function FormatarPara6Caracteres(numero As String) As String
        Return numero.PadLeft(6, "0")
    End Function

    Public Sub FormatarDataGridView(dataGridView As DataGridView, GradeCores As String)


        With dataGridView

            'cobeçalho
            .ColumnHeadersDefaultCellStyle.Font = New System.Drawing.Font("Geist UI", 8, FontStyle.Italic) ' Define a fonte e negrito
            .ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter   ' Centraliza o texto
            .ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.White
            '.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.ColorTranslator.FromHtml("#32423D") '#32423D,#425750
            .ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.ColorTranslator.FromHtml("#6F7170") '#32423D,#425750

            .EnableHeadersVisualStyles = False ' Permite personalizar o cabeçalho
            'Permite quebra de linha
            .ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True

            'Se quiser ajustar a altura manualmente ao invés de deixar automático:
            .ColumnHeadersHeight = 75 ' ou o valor que quiser
            .ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing
            ' .AutoResizeRows(DataGridViewAutoSizeRowsMode.None)

            If GradeCores = "SIM" Then
                'Dados
                .DefaultCellStyle.Font = New System.Drawing.Font("Geist UI", 8, FontStyle.Italic)
                .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft  ' Centraliza o texto

                ' Estilo de linha de grade (as linhas de divisão)
                .CellBorderStyle = DataGridViewCellBorderStyle.Single   ' Define o estilo da borda das células (pode ser: None, Single, etc.)
                .GridColor = System.Drawing.Color.Gray  ' Cor das linhas de grade

                ' Formatar as linhas de divisão horizontais
                .RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single ' Estilo da borda das linhas de cabeçalho

                ' Formatar as linhas de divisão verticais
                .ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None  ' Estilo da borda do cabeçalho de coluna

                ' Formatar a grade de divisão entre as células de dados
                .EnableHeadersVisualStyles = False ' Desativa a aparência do cabeçalho para customizar
                .AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.ColorTranslator.FromHtml("#F9F8F3")
                .DefaultCellStyle.SelectionBackColor = System.Drawing.ColorTranslator.FromHtml("#6F7170") 'System.Drawing.Color.LightBlue ' Cor de fundo ao selecionar a célula
                .DefaultCellStyle.SelectionForeColor = System.Drawing.Color.White ' Cor da fonte ao selecionar a célula

                'seleção da linha
                .SelectionMode = DataGridViewSelectionMode.FullRowSelect
                ' .ClearSelection()
                ' .CurrentCell = Nothing

                'Oculta a coluna a esquerda antes da coluna de dados
                .RowHeadersVisible = False

            End If

        End With

    End Sub

    Public Function AlteracaoEspecifica(tabela As String, campoTabela As String, NovoValor As String, campo_id As String, Valor_id As String) As Boolean

        Try

            cl_BancoDados.AbrirBanco()

            If My.Settings.TipoConexao = "MYSQL" Then

                Try
                    AlteracaoEspecifica = False

                    ' Construção da consulta SQL usando parâmetros para prevenir SQL Injection
                    Dim sql As String = $"UPDATE {tabela} SET {campoTabela} = @NovoValor WHERE {campo_id} = @Valor_id"

                    ' Criação do comando SQL com o uso de parâmetros
                    Using mycomand As New MySqlCommand(sql.ToLower(), myconect)
                        mycomand.Parameters.AddWithValue("@NovoValor", NovoValor)
                        mycomand.Parameters.AddWithValue("@Valor_id", Valor_id)

                        ' Execução do comando SQL
                        mycomand.ExecuteNonQuery()
                    End Using

                    AlteracaoEspecifica = True
                Catch ex As Exception
                    '   MsgBox($"Erro ao atualizar o registro: {ex.Message}", vbCritical, "Erro")
                    AlteracaoEspecifica = False
                Finally
                End Try



            End If

            Return AlteracaoEspecifica
        Catch ex As Exception
            Return False
        Finally
            cl_BancoDados.FecharBanco()
        End Try

    End Function




    Public Sub FormataBtnMsg(texto As String, duracaoSegundos As Integer, btn As Button, timer As Timer)

        btn.BackColor = Color.Green

        btn.Text = texto
        timer.Interval = duracaoSegundos * 1000
        timer.Enabled = True

    End Sub




    Public Function converteStringParaDouble(ByVal valor As String) As Double
        Dim valorConvertido As Double = 0.0 ' Inicializa com 0.0 como valor padrão

        Try
            ' Verifica se a string não está vazia
            If Not String.IsNullOrEmpty(valor) Then
                ' Substitui vírgulas por pontos (se necessário) antes da conversão
                valor = valor.Replace(",", ".")

                ' Converte a string para Double usando a cultura invariável (garante ponto como separador decimal)
                valorConvertido = Convert.ToDouble(valor, CultureInfo.InvariantCulture)
            End If
        Catch ex As Exception
            ' Em caso de erro, o valor será 0.0
            valorConvertido = 0.0
        End Try

        ' Retorna o valor convertido
        Return valorConvertido
    End Function


    Public Function CopiarArquivoInteligente(origem As String, destino As String) As Boolean
        Try
            origem = Path.GetFullPath(origem)
            destino = Path.GetFullPath(destino)

            Dim caminhosParaTentar As New List(Of String)
            caminhosParaTentar.Add(origem)

            Dim pastaOrigem As String = Path.GetDirectoryName(origem)
            Dim nomeArquivo As String = Path.GetFileName(origem)

            If nomeArquivo.Contains(" ") Then
                Dim nomeComPorcento = nomeArquivo.Replace(" ", "%")
                caminhosParaTentar.Add(Path.Combine(pastaOrigem, nomeComPorcento))
            End If

            If nomeArquivo.Contains("%") Then
                Dim nomeComEspaco = nomeArquivo.Replace("%", " ")
                caminhosParaTentar.Add(Path.Combine(pastaOrigem, nomeComEspaco))
            End If

            For Each caminho In caminhosParaTentar.Distinct()
                If File.Exists(caminho) Then
                    File.Copy(caminho, destino, True)
                    Return True
                End If
            Next

            Return False
        Catch ex As Exception
            Return False
        End Try
    End Function

End Class
