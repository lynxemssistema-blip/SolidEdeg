Public Class frmCadProtheusProd
    Private Sub btnSalvar_Click(sender As Object, e As EventArgs) Handles btnSalvar.Click

        ' ============================================================
        ' 🔹 Integração API Protheus (Criar Produto)
        ' ============================================================
        Try
                ' Ignorar erros de certificado SSL (ambiente de testes/local) e forçar TLS 1.2
                System.Net.ServicePointManager.SecurityProtocol = DirectCast(3072, System.Net.SecurityProtocolType) Or System.Net.SecurityProtocolType.Tls11 Or System.Net.SecurityProtocolType.Tls
            System.Net.ServicePointManager.ServerCertificateValidationCallback = Function(s, cert, chain, sslPolicyErrors) True

            ' 1. Obter Token Bearer
            Dim tokenUrl As String = "https://192.168.1.60:47500/tlpp/oauth2/token?grant_type=password&username=sinco&password=Metal1120"
            Dim tokenRequest As System.Net.HttpWebRequest = CType(System.Net.WebRequest.Create(tokenUrl), System.Net.HttpWebRequest)
            tokenRequest.Method = "GET"
            tokenRequest.Timeout = 10000 ' 10 segundos timeout

            Dim accessToken As String = ""
            Using tokenResponse As System.Net.HttpWebResponse = CType(tokenRequest.GetResponse(), System.Net.HttpWebResponse)
                Using reader As New System.IO.StreamReader(tokenResponse.GetResponseStream())
                    Dim responseText As String = reader.ReadToEnd()
                    ' Busca simples pelo token no JSON via Regex para evitar dependência externa
                    Dim match As System.Text.RegularExpressions.Match = System.Text.RegularExpressions.Regex.Match(responseText, """access_token""\s*:\s*""([^""]+)""")
                    If match.Success Then
                        accessToken = match.Groups(1).Value
                    End If
                End Using
            End Using

            ' 2. Realizar POST caso tenha obtido o token
            If Not String.IsNullOrEmpty(accessToken) Then
                Dim postUrl As String = "https://192.168.1.60:47500/produtos/create"
                Dim postRequest As System.Net.HttpWebRequest = CType(System.Net.WebRequest.Create(postUrl), System.Net.HttpWebRequest)
                postRequest.Method = "POST"
                postRequest.ContentType = "application/json"
                postRequest.Headers.Add("Authorization", "Bearer " & accessToken)
                postRequest.Timeout = 15000 ' 15 segundos timeout

                ' Montar JSON dinâmico baseado na peça corrente com higienização p/ evitar JSON quebrado
                Dim tituloLimpo As String = UCase(DadosArquivoCorrente.AssuntoSubiTitulo & " " & DadosArquivoCorrente.Titulo).Trim()
                tituloLimpo = tituloLimpo.Replace("""", "\""").Replace(vbCrLf, " ").Replace(vbLf, " ").Replace(vbCr, " ")

                Dim b1_cod_Protheus As String = DadosArquivoCorrente.NomeArquivoSemExtensao
                If String.IsNullOrEmpty(b1_cod_Protheus) Then b1_cod_Protheus = ""
                b1_cod_Protheus = b1_cod_Protheus.Replace("""", "\""").Replace(vbCrLf, " ").Replace(vbLf, " ").Replace(vbCr, " ")

                Dim codDesenho As String = DadosArquivoCorrente.EnderecoArquivo
                If String.IsNullOrEmpty(codDesenho) Then codDesenho = "desenho_metalfisa"
                codDesenho = codDesenho.Replace("""", "\""").Replace(vbCrLf, " ").Replace(vbLf, " ").Replace(vbCr, " ")

                Dim massaBruta As String = If(String.IsNullOrWhiteSpace(DadosArquivoCorrente.Massa), "0", DadosArquivoCorrente.Massa.Replace(",", "."))
                Dim pesoValor As Double = 0
                Double.TryParse(massaBruta, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, pesoValor)
                Dim pesoFormatado As String = pesoValor.ToString("0.##", System.Globalization.CultureInfo.InvariantCulture)

                ' Captura com segurança apenas o Código selecionado (seja do ValueMember ou do Text digitado se nulo)
                ' E pega apenas o PRIMEIRO grupo de caracteres (antes de espaço ou hífen).
                Dim valGrupo As String = If(cboB1_GRUPO.SelectedValue IsNot Nothing, cboB1_GRUPO.SelectedValue.ToString(), cboB1_GRUPO.Text)
                valGrupo = If(String.IsNullOrWhiteSpace(valGrupo), "", valGrupo.Split({"-"c, " "c}, StringSplitOptions.RemoveEmptyEntries).FirstOrDefault())

                Dim valTipo As String = If(cboB1_TIPO.SelectedValue IsNot Nothing, cboB1_TIPO.SelectedValue.ToString(), cboB1_TIPO.Text)
                valTipo = If(String.IsNullOrWhiteSpace(valTipo), "", valTipo.Split({"-"c, " "c}, StringSplitOptions.RemoveEmptyEntries).FirstOrDefault())

                Dim valUM As String = If(cboB1_UM.SelectedValue IsNot Nothing, cboB1_UM.SelectedValue.ToString(), cboB1_UM.Text)
                valUM = If(String.IsNullOrWhiteSpace(valUM), "", valUM.Split({"-"c, " "c}, StringSplitOptions.RemoveEmptyEntries).FirstOrDefault())


                Dim jsonPayload As String = "{" &
                        " ""data"": {" &
                        "  ""produtos"": [" &
                        "   {" &
                        "    ""EMPRESA"": ""01""," &
                        "    ""CFILANT"": ""01""," &
                        "    ""B1_COD"": """ & b1_cod_Protheus.Trim & """," &
                        "    ""B1_GRUPO"": """ & valGrupo & """," &
                        "    ""B1_DESC"": """ & tituloLimpo & """," &
                        "    ""B1_XREVM"": """ & Me.txtB1_XREVM.Text.Trim & """," &
                        "    ""B1_TIPO"": """ & valTipo & """," &
                        "    ""B1_UM"": """ & valUM & """," &
                        "    ""B1_LOCPAD"": ""03""," &
                        "    ""B1_POSIPI"": ""00000000""," &
                        "    ""B1_FINALID"": ""1""," &
                        "    ""B1_ORIGEM"": ""0""," &
                        "    ""B1_XCODDES"": """ & codDesenho & """," &
                        "    ""B1_PESO"": " & pesoFormatado & "," &
                        "    ""B1_PESBRU"": " & pesoFormatado &
                        "   }" &
                        "  ]" &
                        " }" &
                        "}"

                Using writer As New System.IO.StreamWriter(postRequest.GetRequestStream())
                    writer.Write(jsonPayload)
                End Using

                ' Enviar e ler a resposta do Protheus
                Using postResponse As System.Net.HttpWebResponse = CType(postRequest.GetResponse(), System.Net.HttpWebResponse)
                    Using reader As New System.IO.StreamReader(postResponse.GetResponseStream())
                        Dim responseResult As String = reader.ReadToEnd()
                        System.Diagnostics.Debug.WriteLine("SINCO: Produto criado na API Protheus. Resposta: " & responseResult)

                        ' Usa Expressão Regular para extrair o código B1_COD do JSON de resposta
                        Dim matchCodigo As System.Text.RegularExpressions.Match = System.Text.RegularExpressions.Regex.Match(responseResult, """B1_COD""\s*:\s*""([^""]+)""")

                        If matchCodigo.Success Then
                            ' Retorna somente o número gerado (ex: MT3-5050)
                            MsgBox(matchCodigo.Groups(1).Value)
                        Else
                            ' Caso não encontre B1_COD, exibe a resposta completa caso a estrutura seja diferente
                            MsgBox(responseResult)
                        End If

                    End Using
                End Using
            Else
                System.Diagnostics.Debug.WriteLine("SINCO: Falha ao obter token de acesso na API Protheus.")
            End If
        Catch exAPI As Exception
            ' Apenas loga o erro, para não travar o processo principal de salvamento (MySQL)
            System.Diagnostics.Debug.WriteLine("SINCO: Erro na integração com a API Protheus -> " & exAPI.Message)
            MsgBox("SINCO: Erro na integração com a API Protheus -> " & exAPI.Message)

        End Try



    End Sub

    Private Sub frmCadProtheusProd_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            Dim connString As String = "Host=192.168.1.61;Port=5432;Username=sinco2;Password=sinco25;Database=p12prd;"

            Dim dtGrupo As New System.Data.DataTable()
            Dim dtTipo As New System.Data.DataTable()
            Dim dtUM As New System.Data.DataTable()

            Using conn As New Npgsql.NpgsqlConnection(connString)
                conn.Open()

                ' 1. Busca os grupos na tabela SBM010
                Dim sqlGrupo As String = "SELECT BM_GRUPO AS ""Código"", BM_DESC AS ""Descrição"" FROM public.SBM010 WHERE D_E_L_E_T_ = ' ' ORDER BY BM_GRUPO"
                Using cmdGrupo As New Npgsql.NpgsqlCommand(sqlGrupo, conn)
                    Using daGrupo As New Npgsql.NpgsqlDataAdapter(cmdGrupo)
                        daGrupo.Fill(dtGrupo)
                    End Using
                End Using

                ' 2. Busca Tipos de Produto na tabela SX5010 (X5_TABELA = '02')
                Dim sqlTipo As String = "SELECT X5_CHAVE AS ""Código_Tipo"", X5_DESCRI AS ""Descrição"" FROM public.SX5010 WHERE X5_TABELA = '02' AND D_E_L_E_T_ = ' ' ORDER BY X5_CHAVE"
                Using cmdTipo As New Npgsql.NpgsqlCommand(sqlTipo, conn)
                    Using daTipo As New Npgsql.NpgsqlDataAdapter(cmdTipo)
                        daTipo.Fill(dtTipo)
                    End Using
                End Using

                ' 3. Busca Unidades de Medida na tabela SAH010
                Dim sqlUM As String = "SELECT AH_UNIMED AS ""Codigo_UM"", AH_UMRES AS ""Descrição"" FROM public.SAH010 WHERE D_E_L_E_T_ = ' ' ORDER BY AH_UNIMED"
                Using cmdUM As New Npgsql.NpgsqlCommand(sqlUM, conn)
                    Using daUM As New Npgsql.NpgsqlDataAdapter(cmdUM)
                        daUM.Fill(dtUM)
                    End Using
                End Using
            End Using

            ' ================= BINDING GRUPO =================
            If Not dtGrupo.Columns.Contains("DescricaoExibicao") Then dtGrupo.Columns.Add("DescricaoExibicao", GetType(String))
            For Each row As System.Data.DataRow In dtGrupo.Rows
                Dim codGrupo As String = If(row("Código") IsNot DBNull.Value, row("Código").ToString().Trim(), "")
                Dim descGrupo As String = If(row("Descrição") IsNot DBNull.Value, row("Descrição").ToString().Trim(), "")
                row("DescricaoExibicao") = $"{codGrupo} - {descGrupo}"
            Next
            With cboB1_GRUPO
                .DataSource = dtGrupo
                .DisplayMember = "DescricaoExibicao"
                .ValueMember = "Código"
                .SelectedIndex = -1
            End With

            ' ================= BINDING TIPO =================
            If Not dtTipo.Columns.Contains("DescricaoExibicao") Then dtTipo.Columns.Add("DescricaoExibicao", GetType(String))
            For Each row As System.Data.DataRow In dtTipo.Rows
                Dim codTipo As String = If(row("Código_Tipo") IsNot DBNull.Value, row("Código_Tipo").ToString().Trim(), "")
                Dim descTipo As String = ""
                If dtTipo.Columns.Contains("Descrição") Then descTipo = If(row("Descrição") IsNot DBNull.Value, row("Descrição").ToString().Trim(), "")

                ' Exibe "Codigo - Descrição" se a descrição for encontrada, caso contrário mostra só o código
                row("DescricaoExibicao") = If(descTipo <> "", $"{codTipo} - {descTipo}", codTipo)
            Next
            With cboB1_TIPO
                .DataSource = dtTipo
                .DisplayMember = "DescricaoExibicao"
                .ValueMember = "Código_Tipo"
                .SelectedIndex = -1
            End With

            ' ================= BINDING UM =================
            If Not dtUM.Columns.Contains("DescricaoExibicao") Then dtUM.Columns.Add("DescricaoExibicao", GetType(String))
            For Each row As System.Data.DataRow In dtUM.Rows
                Dim codUM As String = If(row("Codigo_UM") IsNot DBNull.Value, row("Codigo_UM").ToString().Trim(), "")
                Dim descUM As String = ""
                If dtUM.Columns.Contains("Descrição") Then descUM = If(row("Descrição") IsNot DBNull.Value, row("Descrição").ToString().Trim(), "")

                ' Exibe "Codigo - Descrição" se a descrição for encontrada, caso contrário mostra só o código
                row("DescricaoExibicao") = If(descUM <> "", $"{codUM} - {descUM}", codUM)
            Next
            With cboB1_UM
                .DataSource = dtUM
                .DisplayMember = "DescricaoExibicao"
                .ValueMember = "Codigo_UM"
                .SelectedIndex = -1
            End With

        Catch ex As Exception
            MsgBox("Erro ao carregar listas do Protheus (Grupo, Tipo, UM):" & Environment.NewLine & ex.Message, MsgBoxStyle.Critical, "Erro")
        End Try

    End Sub

End Class