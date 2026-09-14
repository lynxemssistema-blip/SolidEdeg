
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

    Public Function BuscarArquivoconf()

        If InputBox("Senha de acesso", "Administrador", "") = "99678982" Then
            Dim OpenfileConfiguracao As New OpenFileDialog

            ' Configura o diálogo para aceitar somente arquivos de texto
            OpenfileConfiguracao.Filter = "Arquivos de Texto (*.txt)|*.txt|Todos os arquivos (*.*)|*.*"
            OpenfileConfiguracao.FilterIndex = 1

            ' Exibe o diálogo e verifica se o usuário selecionou um arquivo
            If OpenfileConfiguracao.ShowDialog() = DialogResult.OK Then
                Dim caminhoArquivo As String = OpenfileConfiguracao.FileName

                ' Verifica se o arquivo existe
                If File.Exists(caminhoArquivo) Then

                    ' Variáveis para armazenar os parâmetros
                    Dim endereco As String = ""
                    Dim usuario As String = ""
                    Dim banco As String = ""
                    Dim senha As String = ""
                    Dim EnderecoPastaRaizOS As String = ""
                    Dim CopiaBancoDados As String = ""
                    Dim EnderecoPastaRaizRomaneio As String = ""
                    Dim Enderecoplanodecorte As String = ""
                    Dim EnderecoTemplateExcelOrdemServico As String = ""
                    Dim EnderecoTemplateExcelRomaneio As String = ""
                    Dim templateplanodecorte As String = ""
                    Dim ParametroExportarDXF As String = ""
                    Dim EnderecoPastaRaizRNC As String = ""
                    Dim EnderecoTemplateExcelRNC As String = ""
                    Dim EnderecoImagens As String = ""
                    Dim ProgramaRM As String = ""
                    Dim PlanilhaModeloLantek As String = ""

                    ' Codificação utilizada na leitura do arquivo
                    Dim codificacao As Encoding = Encoding.GetEncoding("ISO-8859-1") ' Ajustável conforme o arquivo

                    ' Lê o arquivo linha por linha
                    Dim parametrosEncontrados As New Dictionary(Of String, String)
                    Using leitor As New StreamReader(caminhoArquivo, codificacao)
                        While Not leitor.EndOfStream
                            Dim linha As String = leitor.ReadLine()?.Trim()

                            ' Ignorar linhas em branco ou mal formadas
                            If String.IsNullOrEmpty(linha) OrElse Not linha.Contains(";") Then Continue While

                            Dim partes = linha.Split(";"c)
                            If partes.Length = 2 Then
                                Dim chave = partes(0).Trim()
                                Dim valor = partes(1).Trim()

                                ' Armazena no dicionário
                                If Not parametrosEncontrados.ContainsKey(chave) Then
                                    parametrosEncontrados(chave) = valor
                                End If
                            End If
                        End While
                    End Using

                    ' Atribui valores ao My.Settings
                    Dim parametrosNecessarios = {"endereco", "Usuario", "Banco", "Senha", "EnderecoPastaRaizOS", "EnderecoTemplateExcelOrdemServico", "ParametroExportarDXF", "EnderecoImagens", "ProgramaRM", "PlanilhaModeloLantek"}
                    For Each param In parametrosNecessarios
                        If Not parametrosEncontrados.ContainsKey(param) Then
                            MsgBox($"Erro: Parâmetro '{param}' não encontrado no arquivo de configuração!", MsgBoxStyle.Critical)
                            'Return
                        End If
                    Next

                    My.Settings.MySqlBancoDados = parametrosEncontrados("Banco")
                    My.Settings.MysqlEndereco = parametrosEncontrados("endereco")
                    My.Settings.MysqlUsuario = parametrosEncontrados("Usuario")
                    My.Settings.MysqlSenha = parametrosEncontrados("Senha")
                    My.Settings.BancoDadosAtivo = parametrosEncontrados("Banco")
                    My.Settings.EnderecoImagens = parametrosEncontrados("EnderecoImagens")

                    My.Settings.EnderecoPastaRaizOS = parametrosEncontrados("EnderecoPastaRaizOS")
                    My.Settings.EnderecoTemplateExcel = parametrosEncontrados("EnderecoTemplateExcelOrdemServico")
                    My.Settings.ParametroExportarDXF = parametrosEncontrados("ParametroExportarDXF")
                    My.Settings.ProgramaRM = parametrosEncontrados("ProgramaRM")
                    My.Settings.PlanilhaModeloLantek = parametrosEncontrados("PlanilhaModeloLantek")


                    'My.Settings.SQLServerProtheus = parametrosEncontrados("SQLServerProtheus")

                    ' Salva as configurações
                    My.Settings.Save()

                    MsgBox("Configurações carregadas com sucesso!", MsgBoxStyle.Information)

                    Dim app As SolidEdgeFramework.Application = Nothing

                    Try
                        ' 🔹 Tenta pegar uma instância do Solid Edge aberta
                        app = CType(Marshal.GetActiveObject("SolidEdge.Application"), SolidEdgeFramework.Application)

                    Catch ex As COMException
                        ' 🔹 Se não houver instância aberta, cria uma nova
                        app = Activator.CreateInstance(Type.GetTypeFromProgID("SolidEdge.Application"))
                        app.Visible = True
                    Catch ex As Exception
                        MessageBox.Show("Erro ao conectar ao Solid Edge: " & ex.Message)
                    End Try

                Else
                    MsgBox("Arquivo selecionado não encontrado!", MsgBoxStyle.Exclamation)
                End If
            End If
        Else
            MsgBox("Senha inválida!", MsgBoxStyle.Exclamation)
        End If

    End Function


    Function ComboBoxDataSet(ByVal Tabela As String, ByVal Campo_Id As String, ByVal CampoDescricao As String, ByVal ObjComboBox As ComboBox, ByVal consulta As String, Optional NomeBancoCliente As String = "") As Boolean

        Try

            My.Settings.TipoConexao = "MYSQL"

            AbrirBanco()

            'If My.Settings.TipoConexao = "MYSQL" Then
            '    ' Construção da consulta SQL com parâmetros
            Dim sqlStr As String = $"SELECT {Campo_Id}, UPPER({CampoDescricao}) AS {CampoDescricao} FROM {Tabela} {consulta} ORDER BY {CampoDescricao}"

            ' Criação do adaptador e do dataset
            Using da As New MySqlDataAdapter(sqlStr, myconect)

                Dim ds As New DataSet()
                da.Fill(ds, Tabela)

                ' Configuração do ComboBox
                ObjComboBox.DataSource = ds.Tables(Tabela)
                ObjComboBox.DisplayMember = CampoDescricao.ToUpper()
                ObjComboBox.ValueMember = Campo_Id

                Return True
            End Using
            '  End If

            FecharBanco()
        Catch ex As Exception

            Return False

            cl_BancoDados.FecharBanco()

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
            FecharBanco()
        Catch ex As Exception
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

    Public Function RetornaCampoDaPesquisa(ByVal Valor_Para_Pesquisa As String,
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

            ' If My.Settings.TipoConexao = "MYSQL" Then

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

                            ' drMysql(Campo_A_Retornar).ToString()

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
                                VCampo6 = drMysql(Campo5).ToString()
                            Catch ex As Exception
                                VCampo6 = ""
                            End Try

                            Try
                                VCampo7 = drMysql(Campo5).ToString()
                            Catch ex As Exception
                                VCampo7 = ""
                            End Try

                            Try
                                VCampo8 = drMysql(Campo5).ToString()
                            Catch ex As Exception
                                VCampo8 = ""
                            End Try

                            Try
                                VCampo9 = drMysql(Campo5).ToString()
                            Catch ex As Exception
                                VCampo9 = ""
                            End Try

                            Try
                                VCampo10 = drMysql(Campo5).ToString()
                            Catch ex As Exception
                                VCampo10 = ""
                            End Try
                        Else
                            Return Nothing
                        End If
                    End Using
                Catch ex As Exception

                    '''''  ClasseEmail.EmailTratamentoErro(ex.Message)
                Finally
                    ' Log do erro e retorno de Nothing em caso de falha
                    ' MessageBox.Show("Erro ao executar a pesquisa: " & ex.Message)
                    'Return Nothing
                End Try


            'End If

            cl_BancoDados.FecharBanco()
        Catch ex As Exception
        Finally

            cl_BancoDados.FecharBanco()
        End Try

    End Function

    Public Function VerificaSaldoTag(ByVal IdTag As String) As String

        Try

            VerificaSaldoTag = 0

            cl_BancoDados.AbrirBanco()

            If My.Settings.TipoConexao = "MYSQL" Then

                Try

                    ' Criação do comando SQL
                    Dim daMysql As New MySqlCommand("SELECT QtdeTag, SaldoTag,QtdeLiberada,desctag,DataPrevisao FROM  " & ComplementoTipoBanco & "tags where IdTag = '" & IdTag & "'", myconect)

                    ' Execução da consulta e leitura dos dados
                    Using drMysql As MySqlDataReader = daMysql.ExecuteReader()

                        If drMysql.HasRows Then
                            drMysql.Read()
                            OrdemServico.QtdeTag = Convert.ToInt32(drMysql("QtdeTag").ToString())
                            OrdemServico.SaldoTag = Convert.ToInt32(drMysql("SaldoTag").ToString())
                            OrdemServico.QtdeLiberada = Convert.ToInt32(drMysql("QtdeLiberada").ToString())
                            OrdemServico.Descricao = drMysql("desctag").ToString()
                            OrdemServico.DataPrevisao = drMysql("DataPrevisao").ToString()

                        End If

                    End Using
                Catch ex As Exception

                    'ClasseEmail.EmailTratamentoErro(ex.Message)
                Finally

                End Try

            End If

            cl_BancoDados.FecharBanco()
        Catch ex As Exception
        Finally
            cl_BancoDados.FecharBanco()

        End Try

    End Function

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

            cl_BancoDados.FecharBanco()
        Catch ex As Exception
        Finally
            cl_BancoDados.FecharBanco()

        End Try

    End Function

    Public Function CalcularordemservicoitemFatorOS_TAG_PROJETO() As Boolean

        Try

            ' cl_BancoDados.AbrirBanco()

            Dim query As String = "UPDATE ordemservico os
LEFT JOIN (
    SELECT
        osi.IdOrdemServico,

        COUNT(osi.IdOrdemServicoItem) AS QtdeTotalItens,
        COALESCE(SUM(CASE WHEN osi.ordemservicoitemfinalizado = 'C' THEN 1 ELSE 0 END), 0) AS QtdeItensExecutados,
        COALESCE(SUM(osi.QtdeTotal), 0) AS QtdeTotalPecas,
        COALESCE(SUM(CASE WHEN osi.ordemservicoitemfinalizado = 'C' THEN osi.QtdeTotal ELSE 0 END), 0) AS QtdePecasExecutadas,

        COALESCE(SUM(CASE WHEN osi.txtCorte = '1' THEN osi.QtdeTotal ELSE 0 END), 0) AS CorteTotalExecutar,
        COALESCE(SUM(osi.CorteTotalExecutado), 0) AS CorteTotalExecutado,

        COALESCE(SUM(CASE WHEN osi.txtDobra = '1' THEN osi.QtdeTotal ELSE 0 END), 0) AS DobraTotalExecutar,
        COALESCE(SUM(osi.DobraTotalExecutado), 0) AS DobraTotalExecutado,

        COALESCE(SUM(CASE WHEN osi.txtSolda = '1' THEN osi.QtdeTotal ELSE 0 END), 0) AS SoldaTotalExecutar,
        COALESCE(SUM(osi.SoldaTotalExecutado), 0) AS SoldaTotalExecutado,

        COALESCE(SUM(CASE WHEN osi.txtPintura = '1' THEN osi.QtdeTotal ELSE 0 END), 0) AS PinturaTotalExecutar,
        COALESCE(SUM(osi.PinturaTotalExecutado), 0) AS PinturaTotalExecutado,

        COALESCE(SUM(CASE WHEN osi.txtMontagem = '1' AND osi.ProdutoPrincipal = 'SIM'
                          THEN osi.QtdeTotal ELSE 0 END), 0) AS MontagemTotalExecutar,

        COALESCE(SUM(osi.MontagemTotalExecutado), 0) AS MontagemTotalExecutado,

        COALESCE(SUM(osi.AreaPintura), 0) AS AreaPinturaTotal,
        COALESCE(SUM(osi.Peso), 0) AS PesoTotal

    FROM ordemservicoitem osi
    WHERE (osi.D_E_L_E_T_E IS NULL OR osi.D_E_L_E_T_E = '')
      AND (osi.EnderecoArquivo IS NOT NULL AND osi.EnderecoArquivo <> '')
      AND (osi.Liberado_Engenharia = 'S')
    GROUP BY osi.IdOrdemServico
) itens ON itens.IdOrdemServico = os.IdOrdemServico
SET
    os.QtdeTotalItens          = COALESCE(itens.QtdeTotalItens, 0),
    os.QtdeItensExecutados     = COALESCE(itens.QtdeItensExecutados, 0),
    os.QtdeTotalPecas          = COALESCE(itens.QtdeTotalPecas, 0),
    os.QtdePecasExecutadas     = COALESCE(itens.QtdePecasExecutadas, 0),

    os.CorteTotalExecutar      = COALESCE(itens.CorteTotalExecutar, 0),
    os.CorteTotalExecutado     = COALESCE(itens.CorteTotalExecutado, 0),

    os.DobraTotalExecutar      = COALESCE(itens.DobraTotalExecutar, 0),
    os.DobraTotalExecutado     = COALESCE(itens.DobraTotalExecutado, 0),

    os.SoldaTotalExecutar      = COALESCE(itens.SoldaTotalExecutar, 0),
    os.SoldaTotalExecutado     = COALESCE(itens.SoldaTotalExecutado, 0),

    os.PinturaTotalExecutar    = COALESCE(itens.PinturaTotalExecutar, 0),
    os.PinturaTotalExecutado   = COALESCE(itens.PinturaTotalExecutado, 0),

    os.MontagemTotalExecutar   = COALESCE(itens.MontagemTotalExecutar, 0),
    os.MontagemTotalExecutado  = COALESCE(itens.MontagemTotalExecutado, 0),

    os.AreaPinturaTotal        = ROUND(COALESCE(itens.AreaPinturaTotal, 0), 2),
    os.PesoTotal               = ROUND(COALESCE(itens.PesoTotal, 0), 2),

    os.CortePercentual         = COALESCE(ROUND((itens.CorteTotalExecutado / NULLIF(itens.CorteTotalExecutar, 0)) * 100, 2), 0),
    os.DobraPercentual         = COALESCE(ROUND((itens.DobraTotalExecutado / NULLIF(itens.DobraTotalExecutar, 0)) * 100, 2), 0),
    os.SoldaPercentual         = COALESCE(ROUND((itens.SoldaTotalExecutado / NULLIF(itens.SoldaTotalExecutar, 0)) * 100, 2), 0),
    os.PinturaPercentual       = COALESCE(ROUND((itens.PinturaTotalExecutado / NULLIF(itens.PinturaTotalExecutar, 0)) * 100, 2), 0),
    os.MontagemPercentual      = COALESCE(ROUND((itens.MontagemTotalExecutado / NULLIF(itens.MontagemTotalExecutar, 0)) * 100, 2), 0),

    os.PercentualPecas   = COALESCE(ROUND((itens.QtdePecasExecutadas / NULLIF(itens.QtdeTotalPecas, 0)) * 100, 2), 0),
    os.PercentualItens      = COALESCE(ROUND((itens.QtdeItensExecutados / NULLIF(itens.QtdeTotalItens, 0)) * 100, 2), 0) where os.idordemservico = " & OrdemServico.IdOrdemServico & "
UPDATE ordemservicoitem osi
SET
    CortePercentual = ROUND((osi.CorteTotalExecutado / osi.QtdeTotal) * 100, 2),
    DobraPercentual = ROUND((osi.DobraTotalExecutado / osi.QtdeTotal) * 100, 2),
    SoldaPercentual = ROUND((osi.SoldaTotalExecutado / osi.QtdeTotal) * 100, 2),
    PinturaPercentual = ROUND((osi.PinturaTotalExecutado / osi.QtdeTotal) * 100, 2),
    MontagemPercentual = ROUND((osi.MontagemTotalExecutado / osi.QtdeTotal) * 100, 2)
WHERE
    (osi.D_E_L_E_T_E IS NULL OR osi.D_E_L_E_T_E = '') and  osi.idordemservico = " & OrdemServico.IdOrdemServico & ";
UPDATE tags t
                    LEFT JOIN (
SELECT
    os.IdTag AS IdTag,

    COUNT(DISTINCT os.IdOrdemServico)                                            AS Total_OS,
    COALESCE(SUM(os.QtdeTotalPecas), 0)                                          AS Total_Pecas,

    -- Quantidade de OS executadas (distintas)
    COALESCE(COUNT(DISTINCT CASE
        WHEN os.ordemservicofinalizado = 'C' THEN os.IdOrdemServico
    END), 0)                                                                     AS QtdeOSExecutadas,

    -- Quantidade de peças executadas (soma nas OS finalizadas)
    COALESCE(SUM(CASE
        WHEN os.ordemservicofinalizado = 'C' THEN os.QtdeTotalPecas ELSE 0
    END), 0)                                                                     AS QtdePecasExecutadas,

    COALESCE(SUM(os.CorteTotalExecutar),     0)                                  AS CorteTotalExecutar,
    COALESCE(SUM(os.CorteTotalExecutado),    0)                                  AS CorteTotalExecutado,

    COALESCE(SUM(os.DobraTotalExecutar),     0)                                  AS DobraTotalExecutar,
    COALESCE(SUM(os.DobraTotalExecutado),    0)                                  AS DobraTotalExecutado,

    COALESCE(SUM(os.SoldaTotalExecutar),     0)                                  AS SoldaTotalExecutar,
    COALESCE(SUM(os.SoldaTotalExecutado),    0)                                  AS SoldaTotalExecutado,

    COALESCE(SUM(os.PinturaTotalExecutar),   0)                                  AS PinturaTotalExecutar,
    COALESCE(SUM(os.PinturaTotalExecutado),  0)                                  AS PinturaTotalExecutado,

    COALESCE(SUM(os.MontagemTotalExecutar),  0)                                  AS MontagemTotalExecutar,
    COALESCE(SUM(os.MontagemTotalExecutado), 0)                                  AS MontagemTotalExecutado
FROM ordemservico AS os
WHERE (os.D_E_L_E_T_E IS NULL OR os.D_E_L_E_T_E = '')
  AND os.Liberado_Engenharia = 'S'
GROUP BY os.IdTag
  ) resumo ON resumo.IdTag = t.IdTag
 SET
     t.QtdeOS                   = COALESCE(resumo.Total_OS, 0),
     t.QtdeOSExecutadas         = COALESCE(resumo.QtdeOSexecutadas, 0),
     t.QtdePecasOS              = COALESCE(resumo.Total_Pecas, 0),
     t.QtdePecasExecutadas      = COALESCE(resumo.QtdePecasexecutadas, 0),
     t.CorteTotalExecutar       = COALESCE(resumo.CorteTotalExecutar, 0),
     t.CorteTotalExecutado      = COALESCE(resumo.CorteTotalExecutado, 0),
     t.DobraTotalExecutar       = COALESCE(resumo.DobraTotalExecutar, 0),
     t.DobraTotalExecutado      = COALESCE(resumo.DobraTotalExecutado, 0),
     t.SoldaTotalExecutar       = COALESCE(resumo.SoldaTotalExecutar, 0),
     t.SoldaTotalExecutado      = COALESCE(resumo.SoldaTotalExecutado, 0),
     t.PinturaTotalExecutar     = COALESCE(resumo.PinturaTotalExecutar, 0),
     t.PinturaTotalExecutado    = COALESCE(resumo.PinturaTotalExecutado, 0),
     t.MontagemTotalExecutar    = COALESCE(resumo.MontagemTotalExecutar, 0),
     t.MontagemTotalExecutado   = COALESCE(resumo.MontagemTotalExecutado, 0),

      t.CortePercentual = COALESCE(ROUND((resumo.CorteTotalExecutado / NULLIF(resumo.CorteTotalExecutar, 0)) * 100, 2), 0),
     t.DobraPercentual = COALESCE(ROUND((resumo.DobraTotalExecutado / NULLIF(resumo.DobraTotalExecutar, 0)) * 100, 2), 0),
     t.SoldaPercentual = COALESCE(ROUND((resumo.SoldaTotalExecutado / NULLIF(resumo.SoldaTotalExecutar, 0)) * 100, 2), 0),
     t.PinturaPercentual = COALESCE(ROUND((resumo.PinturaTotalExecutado / NULLIF(resumo.PinturaTotalExecutar, 0)) * 100, 2), 0),
     t.MontagemPercentual = COALESCE(ROUND((resumo.MontagemTotalExecutado / NULLIF(resumo.MontagemTotalExecutar, 0)) * 100, 2), 0),
     t.PercentualOS   = COALESCE(ROUND((resumo.QtdeOSExecutadas / NULLIF(resumo.Total_OS, 0)) * 100, 2), 0),
    t.PercentualPecas      = COALESCE(ROUND((resumo.QtdePecasExecutadas / NULLIF(resumo.Total_Pecas, 0)) * 100, 2), 0);
UPDATE projetos p
LEFT JOIN (
    SELECT
        t.IdProjeto,
        COUNT(*) AS Total_Tags,
        COALESCE(SUM(t.QtdePecasOS), 0) AS Total_Pecas,

      COALESCE(count(CASE WHEN t.finalizado = 'C' THEN t.IdTag ELSE 0 END), 0) AS QtdeTagsExecutadas,
      COALESCE(SUM(CASE WHEN t.finalizado = 'C' THEN t.QtdePecasOS ELSE 0 END), 0) AS QtdePecasExecutadas,
        COALESCE(SUM(t.CorteTotalExecutar),    0) AS CorteTotalExecutar,
        COALESCE(SUM(t.CorteTotalExecutado),   0) AS CorteTotalExecutado,

        COALESCE(SUM(t.DobraTotalExecutar),    0) AS DobraTotalExecutar,
        COALESCE(SUM(t.DobraTotalExecutado),   0) AS DobraTotalExecutado,

        COALESCE(SUM(t.SoldaTotalExecutar),    0) AS SoldaTotalExecutar,
        COALESCE(SUM(t.SoldaTotalExecutado),   0) AS SoldaTotalExecutado,

        COALESCE(SUM(t.PinturaTotalExecutar),  0) AS PinturaTotalExecutar,
        COALESCE(SUM(t.PinturaTotalExecutado), 0) AS PinturaTotalExecutado,

        COALESCE(SUM(t.MontagemTotalExecutar), 0) AS MontagemTotalExecutar,
        COALESCE(SUM(t.MontagemTotalExecutado),0) AS MontagemTotalExecutado
    FROM tags t
    WHERE (t.D_E_L_E_T_E IS NULL OR t.D_E_L_E_T_E = '')
    GROUP BY t.IdProjeto
) resumo ON resumo.IdProjeto = p.IdProjeto
SET

    p.QtdeTagsExecutadas      = COALESCE(resumo.QtdeTagsExecutadas, 0),
    p.QtdePecasTags           = COALESCE(resumo.Total_Pecas, 0),
    p.QtdePecasExecutadas     = COALESCE(resumo.QtdePecasExecutadas, 0),
    p.CorteTotalExecutar      = COALESCE(resumo.CorteTotalExecutar, 0),
    p.CorteTotalExecutado     = COALESCE(resumo.CorteTotalExecutado, 0),
    p.DobraTotalExecutar      = COALESCE(resumo.DobraTotalExecutar, 0),
    p.DobraTotalExecutado     = COALESCE(resumo.DobraTotalExecutado, 0),
    p.SoldaTotalExecutar      = COALESCE(resumo.SoldaTotalExecutar, 0),
    p.SoldaTotalExecutado     = COALESCE(resumo.SoldaTotalExecutado, 0),
    p.PinturaTotalExecutar    = COALESCE(resumo.PinturaTotalExecutar, 0),
    p.PinturaTotalExecutado   = COALESCE(resumo.PinturaTotalExecutado, 0),
    p.MontagemTotalExecutar   = COALESCE(resumo.MontagemTotalExecutar, 0),
    p.MontagemTotalExecutado  = COALESCE(resumo.MontagemTotalExecutado, 0),

    p.DobraPercentual = COALESCE(ROUND((resumo.DobraTotalExecutado / NULLIF(resumo.DobraTotalExecutar, 0)) * 100, 2), 0),
    p.SoldaPercentual = COALESCE(ROUND((resumo.SoldaTotalExecutado / NULLIF(resumo.SoldaTotalExecutar, 0)) * 100, 2), 0),
    p.PinturaPercentual = COALESCE(ROUND((resumo.PinturaTotalExecutado / NULLIF(resumo.PinturaTotalExecutar, 0)) * 100, 2), 0),
    p.MontagemPercentual = COALESCE(ROUND((resumo.MontagemTotalExecutado / NULLIF(resumo.MontagemTotalExecutar, 0)) * 100, 2), 0),
    p.PercentualTags   = COALESCE(ROUND((resumo.QtdeTagsExecutadas / NULLIF(resumo.Total_Tags, 0)) * 100, 2), 0),
    p.PercentualPecas      = COALESCE(ROUND((resumo.QtdePecasExecutadas / NULLIF(resumo.Total_Pecas, 0)) * 100, 2), 0); "

            Try

                cl_BancoDados.AbrirBanco()

                Using cmd2 As New MySqlCommand(query, myconect)

                    cmd2.ExecuteNonQuery()

                End Using

                cl_BancoDados.FecharBanco()
            Catch ex As Exception
            Finally

            End Try

            'cl_BancoDados.FecharBanco()
        Catch ex As Exception
        Finally
            cl_BancoDados.FecharBanco()
        End Try

    End Function

    Public Function CopiarArquivoInteligente(origem As String, destino As String) As Boolean

        Try

            origem = Path.GetFullPath(origem)
            destino = Path.GetFullPath(destino)

            Dim caminhosParaTentar As New List(Of String)

            ' Caminho original
            caminhosParaTentar.Add(origem)

            ' Nome do arquivo com espaço → %
            Dim pastaOrigem As String = Path.GetDirectoryName(origem)
            Dim nomeArquivo As String = Path.GetFileName(origem)

            If nomeArquivo.Contains(" ") Then
                Dim nomeComPorcento = nomeArquivo.Replace(" ", "%")
                caminhosParaTentar.Add(Path.Combine(pastaOrigem, nomeComPorcento))
            End If

            ' Nome do arquivo com % → espaço
            If nomeArquivo.Contains("%") Then
                Dim nomeComEspaco = nomeArquivo.Replace("%", " ")
                caminhosParaTentar.Add(Path.Combine(pastaOrigem, nomeComEspaco))
            End If

            ' Tenta todos os caminhos
            For Each caminho In caminhosParaTentar.Distinct()
                If File.Exists(caminho) Then
                    File.Copy(caminho, destino, True)
                    Return True
                End If
            Next

            MessageBox.Show("Arquivo não encontrado após múltiplas tentativas:" & vbCrLf &
                        String.Join(vbCrLf, caminhosParaTentar))
            Return False
        Catch ex As Exception
            MessageBox.Show("Erro ao copiar arquivo: " & ex.Message)
            Return False
        End Try
    End Function


    '#Region '12/12/2026 - rev00
    ''    Public Function CalcularordemservicoitemFator(ByVal IdOrdemServico As Integer, ByVal Fator As Double) As Boolean
    ''        Try

    ''            ' 1) Atualiza os itens da OS (recalcula pelos unitários x fator)
    ''            Dim query As String =
    ''"UPDATE ordemservicoitem
    ''    SET
    ''        Fator        = @Fator,
    ''        AreaPintura  = ROUND(AreaPinturaUnitario * @Fator, 4),
    ''        Peso         = ROUND(PesoUnitario        * @Fator, 4),
    ''        QtdeTotal    = ROUND(Qtde                * @Fator, 4)
    ''    WHERE (D_E_L_E_T_E IS NULL OR D_E_L_E_T_E = '')
    ''      AND IdOrdemServico = @Id;"

    ''            Using cmd As New MySqlCommand(query, myconect)
    ''                cmd.Parameters.Add("@Fator", MySqlDbType.Decimal).Value = Fator
    ''                cmd.Parameters.Add("@Id", MySqlDbType.Int32).Value = IdOrdemServico

    ''                cmd.ExecuteNonQuery()

    ''            End Using

    ''            ' 2) Atualiza o cabeçalho da ordemservico com os agregados dos itens
    ''            query =
    ''"UPDATE ordemservico os
    ''    LEFT JOIN (
    ''        SELECT
    ''            IdOrdemServico,
    ''            COALESCE(SUM(AreaPintura), 0) AS AreaPinturaTotal,
    ''            COALESCE(SUM(Peso),        0) AS PesoTotal,
    ''            COUNT(*)                       AS QtdeTotalItens,
    ''            COALESCE(SUM(QtdeTotal),   0) AS QtdeTotalPecas
    ''        FROM ordemservicoitem
    ''        WHERE (D_E_L_E_T_E IS NULL OR D_E_L_E_T_E = '') 
    ''          AND IdOrdemServico = @Id
    ''    ) agg ON agg.IdOrdemServico = os.IdOrdemServico
    ''    SET
    ''        os.Fator            = @Fator,
    ''        os.AreaPinturaTotal = ROUND(agg.AreaPinturaTotal, 2),
    ''        os.PesoTotal        = ROUND(agg.PesoTotal, 2),
    ''        os.QtdeTotalItens   = COALESCE(agg.QtdeTotalItens, 0),
    ''        os.QtdeTotalPecas   = COALESCE(agg.QtdeTotalPecas, 0)
    ''    WHERE os.IdOrdemServico = @Id
    ''      AND (os.D_E_L_E_T_E IS NULL OR os.D_E_L_E_T_E = '');"

    ''            Try

    ''                Using cmd2 As New MySqlCommand(query, myconect)
    ''                    cmd2.Parameters.Add("@Fator", MySqlDbType.Decimal).Value = Fator
    ''                    cmd2.Parameters.Add("@Id", MySqlDbType.Int32).Value = IdOrdemServico

    ''                    cmd2.ExecuteNonQuery()

    ''                End Using
    ''            Catch ex As Exception
    ''            Finally

    ''            End Try

    ''            Return True
    ''        Catch

    ''            Return False
    ''        End Try
    ''    End Function
    '#End Region



    Public Function CalcularordemservicoitemFator(ByVal IdOrdemServico As Integer, ByVal NovoFator As Double) As Boolean
        Try
            ' Garante conexão aberta
            If myconect Is Nothing Then Return False
            If myconect.State <> ConnectionState.Open Then myconect.Open()

            ' =================================================================================
            ' 1) ATUALIZA OS ITENS (ordemservicoitem)
            ' A mágica acontece aqui: 
            ' Pegamos o (QtdeTotal atual) dividimos pelo (Fator antigo) -> isso nos dá a qtde real da árvore para 1 conjunto.
            ' Depois multiplicamos pelo @NovoFator.
            ' =================================================================================
            Dim queryItens As String = "
                UPDATE ordemservicoitem
                SET
                    QtdeTotal   = ROUND((QtdeTotal   / CASE WHEN Fator = 0 THEN 1 ELSE Fator END) * @NovoFator, 4),
                    Peso        = ROUND((Peso        / CASE WHEN Fator = 0 THEN 1 ELSE Fator END) * @NovoFator, 4),
                    AreaPintura = ROUND((AreaPintura / CASE WHEN Fator = 0 THEN 1 ELSE Fator END) * @NovoFator, 4),
                    
                    -- Atualiza o campo Fator da linha para o novo valor
                    Fator       = @NovoFator
                WHERE IdOrdemServico = @Id
                  AND (D_E_L_E_T_E IS NULL OR D_E_L_E_T_E = '');"

            Using cmd As New MySqlCommand(queryItens, myconect)
                ' Convertendo para Decimal para garantir precisão no MySQL
                cmd.Parameters.Add("@NovoFator", MySqlDbType.Decimal).Value = Convert.ToDecimal(NovoFator)
                cmd.Parameters.Add("@Id", MySqlDbType.Int32).Value = IdOrdemServico

                cmd.ExecuteNonQuery()
            End Using

            ' =================================================================================
            ' 2) ATUALIZA O CABEÇALHO (ordemservico)
            ' Recalcula os totais somando as colunas atualizadas dos itens
            ' =================================================================================
            Dim queryHeader As String = "
                UPDATE ordemservico os
                LEFT JOIN (
                    SELECT
                        IdOrdemServico,
                        COALESCE(SUM(AreaPintura), 0) AS SomaArea,
                        COALESCE(SUM(Peso),        0) AS SomaPeso,
                        COUNT(IdOrdemServicoItem)     AS ContagemItens,
                        COALESCE(SUM(QtdeTotal),   0) AS SomaPecas
                    FROM ordemservicoitem
                    WHERE IdOrdemServico = @Id
                      AND (D_E_L_E_T_E IS NULL OR D_E_L_E_T_E = '')
                ) agg ON agg.IdOrdemServico = os.IdOrdemServico
                SET
                    os.Fator            = @NovoFator,
                    os.AreaPinturaTotal = ROUND(agg.SomaArea, 2),
                    os.PesoTotal        = ROUND(agg.SomaPeso, 2),
                    os.QtdeTotalItens   = COALESCE(agg.ContagemItens, 0),
                    os.QtdeTotalPecas   = COALESCE(agg.SomaPecas, 0)
                WHERE os.IdOrdemServico = @Id
                  AND (os.D_E_L_E_T_E IS NULL OR os.D_E_L_E_T_E = '');"

            Using cmd2 As New MySqlCommand(queryHeader, myconect)
                cmd2.Parameters.Add("@NovoFator", MySqlDbType.Decimal).Value = Convert.ToDecimal(NovoFator)
                cmd2.Parameters.Add("@Id", MySqlDbType.Int32).Value = IdOrdemServico

                cmd2.ExecuteNonQuery()
            End Using

            Return True

        Catch ex As Exception
            ' Dica: Em produção, descomente a linha abaixo para saber o erro exato se falhar
            ' MessageBox.Show("Erro ao recalcular fator: " & ex.Message)
            Return False
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


End Class
