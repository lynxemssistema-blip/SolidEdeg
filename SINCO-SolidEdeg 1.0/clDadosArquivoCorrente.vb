

Imports System.IO
Imports System.Runtime.InteropServices
Imports System.Text
Imports System.Windows.Forms
Imports MySql.Data.MySqlClient

Public Class clDadosArquivoCorrente

    Public IdMaterial As Integer
    Public NomeArquivoComExtensao As String
    Public NomeArquivoSemExtensao As String
    Public EnderecoArquivo As String

    Public EnderecoArquivoAterior As String

    Public Extencao As String

    Public DataCriacaDesenho As String
    Public DataUltimoSalvamento As String
    Public SalvoUltimaVezPor As String

    Public Titulo As String
    Public AssuntoSubiTitulo As String
    Public Comentarios As String
    Public Author As String
    Public PalavraChave As String

    'Processo
    Public soldagem As String

    Public Acabamento As String
    Public TipoDesenho As String
    Public Corte As String
    Public Dobra As String
    Public Solda As String
    Public Pintura As String
    Public Montagem As String
    Public ItemEstoque As String
    Public rnc As String

    'Public Sobra_Fabrica As String
    Public qtde As String

    Public ArquivoPdf As String
    Public ArquivoDxf As String
    Public ArquivoDft As String
    Public ArquivoLXDS As String

    'Caixa Delimitadora
    Public Profundidadeaixadelimitadora As String

    Public Larguracaixadelimitadora As String
    Public Alturacaixadelimitadora As String

    'Lista de Corte
    Public ComprimentoBlank As String = Nothing

    Public LarguraBlank As String = Nothing
    Public Espessura As String = Nothing
    Public PerimetroCorteExterno As String
    Public PerimetroCorteInterno As String
    Public NumeroDobras As String
    Public Massa As String
    Public material As String
    Public AreaPintura As String

    Public propertyName As String
    Public propertyValue As String
    Public propertyNames As Object = Nothing
    Public propertyValues As Object = Nothing
    Public propertyTypes As Object = Nothing
    Public wasResolvedLC As Object = Nothing
    Public NomePropriedadeListCut As String
    Public DescricaoPendencia As String
    Public Sobra_Fabrica As String

    Public EnderecoFichaTecnica As String
    Public EnderecoIsometrico As String

    Public Bloqueado As String
    Public Aprovado As String
    Public Verificado As String

    Public EnderecoImagem As String

    Public Fator As Integer
    Public AreaPinturaTotal As Double
    Public PesoTotal As Double
    Public qtdeTotal As Double

    Public CodigoJuridicoMat As String 'Codigo e cliente - será a referencia da equivalencia do desenho MetalFisa



    Public Sub GerarImagemModeloCorrente(Optional caminhoDestino As String = "")
        Try
            ' 🔹 Obtém a instância do Solid Edge
            Dim seApp As SolidEdgeFramework.Application =
            Marshal.GetActiveObject("SolidEdge.Application")
            If seApp Is Nothing Then
                MessageBox.Show("Solid Edge não está em execução.", "SINCO - Solid Edge",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Exit Sub
            End If

            ' 🔹 Documento ativo
            Dim doc As Object = seApp.ActiveDocument
            If doc Is Nothing Then
                MessageBox.Show("Nenhum documento ativo.", "SINCO - Solid Edge",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Exit Sub
            End If

            ' 🔹 Caminho e extensão
            Dim caminhoArquivo As String = doc.FullName
            Dim ext As String = Path.GetExtension(caminhoArquivo).ToLower()
            Dim nomeArquivo As String = Path.GetFileNameWithoutExtension(caminhoArquivo)

            ' 🔹 Aceita somente PAR, PSM e ASM
            If Not (ext = ".par" Or ext = ".psm" Or ext = ".asm") Then
                MessageBox.Show("Somente arquivos .PAR, .PSM ou .ASM podem gerar imagem.",
                            "SINCO - Solid Edge", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Exit Sub
            End If

            ' 🔹 Define destino padrão
            If String.IsNullOrEmpty(caminhoDestino) Then
                caminhoDestino = Path.Combine(Path.GetDirectoryName(caminhoArquivo),
                                           $"{nomeArquivo}.{ext}.png")
            End If

            ' 🔹 Remove se já existir
            If File.Exists(caminhoDestino) Then File.Delete(caminhoDestino)

            ' 🔹 Usa método compatível (sem SeImageFormat)
            ' Alguns SolidEdge salvam direto apenas com caminho
            Try
                ' Este método é aceito em Part, SheetMetal e Assembly
                doc.SaveAsImage(caminhoDestino)
            Catch ex As Exception
                ' Caso não suporte, tenta via "SavePreviewPicture" (fallback)
                Try
                    doc.SavePreviewPicture(caminhoDestino)
                Catch
                    Throw New Exception("Esta versão do Solid Edge não suporta exportar imagem diretamente.")
                End Try
            End Try

            ' 🔹 Confirma sucesso
            MessageBox.Show($"✅ Imagem gerada com sucesso!{vbCrLf}{caminhoDestino}",
                        "SINCO - Solid Edge", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As COMException
            MessageBox.Show("Erro COM ao gerar imagem: " & ex.Message,
                        "SINCO - Solid Edge", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Catch ex As Exception
            MessageBox.Show("Erro ao gerar imagem: " & ex.Message,
                        "SINCO - Solid Edge", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub


    Public Sub SalvarCorrente()
        Try
            ' ============================================================
            ' 🔹 Verifica tipo de conexão
            ' ============================================================
            If My.Settings.TipoConexao.ToUpper() <> "MYSQL" Then Exit Sub

            ' ============================================================
            ' 🔹 Monta SQL de inserção / atualização
            ' ============================================================
            Dim sql As String =
"INSERT INTO " & ComplementoTipoBanco & "material (
DescResumo, DescDetal, PecaManuFat, Autor, Palavrachave, Notas, Espessura, AreaPintura, NumeroDobras, Peso,
Unidade, Altura, Largura, Profundidade, CodMatFabricante, DtCad, UsuarioCriacao, UsuarioAlteracao, DtAlteracao,
CodigoJuridicoMat, StatusMat, MaterialSW, EnderecoArquivo, Acabamento, txtSoldagem, txtTipoDesenho, txtCorte,
txtDobra, txtSolda, txtPintura, txtMontagem, Comprimentocaixadelimitadora, Larguracaixadelimitadora,
Espessuracaixadelimitadora, txtItemEstoque, D_E_L_E_T_E, EnderecoImagem,Bloqueado)
VALUES (
@DescResumo, @DescDetal, @PecaManuFat, @Autor, @Palavrachave, @Notas, @Espessura, @AreaPintura, @NumeroDobras, @Peso,
@Unidade, @Altura, @Largura, @Profundidade, @CodMatFabricante, @DtCad, @UsuarioCriacao, @UsuarioAlteracao, @DtAlteracao,
@CodigoJuridicoMat, @StatusMat, @MaterialSW, @EnderecoArquivo, @Acabamento, @txtSoldagem, @txtTipoDesenho, @txtCorte,
@txtDobra, @txtSolda, @txtPintura, @txtMontagem, @Compcx, @Largcx, @Espcx, @txtItemEstoque, @D_E_L_E_T_E, @EnderecoImagem,Bloqueado)
ON DUPLICATE KEY UPDATE
DescResumo = VALUES(DescResumo), DescDetal = VALUES(DescDetal), PecaManuFat = VALUES(PecaManuFat),
Autor = VALUES(Autor), Palavrachave = VALUES(Palavrachave), Notas = VALUES(Notas), Espessura = VALUES(Espessura),
AreaPintura = VALUES(AreaPintura), NumeroDobras = VALUES(NumeroDobras), Peso = VALUES(Peso), Unidade = VALUES(Unidade),
Altura = VALUES(Altura), Largura = VALUES(Largura), Profundidade = VALUES(Profundidade), UsuarioAlteracao = VALUES(UsuarioAlteracao),
DtAlteracao = VALUES(DtAlteracao), StatusMat = VALUES(StatusMat), MaterialSW = VALUES(MaterialSW),
EnderecoArquivo = VALUES(EnderecoArquivo), Acabamento = VALUES(Acabamento), txtSoldagem = VALUES(txtSoldagem),
txtTipoDesenho = VALUES(txtTipoDesenho), txtCorte = VALUES(txtCorte), txtDobra = VALUES(txtDobra),
txtSolda = VALUES(txtSolda), txtPintura = VALUES(txtPintura), txtMontagem = VALUES(txtMontagem),
Comprimentocaixadelimitadora = VALUES(Comprimentocaixadelimitadora), Larguracaixadelimitadora = VALUES(Larguracaixadelimitadora),
Espessuracaixadelimitadora = VALUES(Espessuracaixadelimitadora), txtItemEstoque = VALUES(txtItemEstoque),
EnderecoImagem = VALUES(EnderecoImagem), Bloqueado = VALUES(Bloqueado);"


            ' ============================================================
            ' 🔹 Executa no banco
            ' ============================================================
            cl_BancoDados.AbrirBanco()

            Using cmd As New MySqlCommand(sql, myconect)
                ' --- Preenche parâmetros ---
                cmd.Parameters.AddWithValue("@DescResumo", UCase(DadosArquivoCorrente.Titulo))
                cmd.Parameters.AddWithValue("@DescDetal", UCase(DadosArquivoCorrente.AssuntoSubiTitulo))
                cmd.Parameters.AddWithValue("@PecaManuFat", "S")
                cmd.Parameters.AddWithValue("@Autor", UCase(DadosArquivoCorrente.Author))
                cmd.Parameters.AddWithValue("@Palavrachave", UCase(DadosArquivoCorrente.PalavraChave))
                cmd.Parameters.AddWithValue("@Notas", UCase(DadosArquivoCorrente.Comentarios))
                cmd.Parameters.AddWithValue("@Espessura", Replace(DadosArquivoCorrente.Espessura, ",", "."))
                cmd.Parameters.AddWithValue("@AreaPintura", Replace(DadosArquivoCorrente.AreaPintura, ",", "."))
                cmd.Parameters.AddWithValue("@NumeroDobras", Replace(DadosArquivoCorrente.NumeroDobras, ",", "."))
                cmd.Parameters.AddWithValue("@Peso", Replace(DadosArquivoCorrente.Massa, ",", "."))
                cmd.Parameters.AddWithValue("@Unidade", "PC")
                cmd.Parameters.AddWithValue("@Altura", Replace(DadosArquivoCorrente.ComprimentoBlank, ",", "."))
                cmd.Parameters.AddWithValue("@Largura", Replace(DadosArquivoCorrente.LarguraBlank, ",", "."))
                cmd.Parameters.AddWithValue("@Profundidade", "0")
                cmd.Parameters.AddWithValue("@CodMatFabricante", UCase(DadosArquivoCorrente.NomeArquivoSemExtensao))
                cmd.Parameters.AddWithValue("@DtCad", Now.ToString("yyyy-MM-dd HH:mm:ss"))
                cmd.Parameters.AddWithValue("@UsuarioCriacao", UCase(Usuario.NomeCompleto))
                cmd.Parameters.AddWithValue("@UsuarioAlteracao", UCase(DadosArquivoCorrente.SalvoUltimaVezPor))
                cmd.Parameters.AddWithValue("@DtAlteracao", Now.ToString("yyyy-MM-dd HH:mm:ss"))
                cmd.Parameters.AddWithValue("@CodigoJuridicoMat", DadosArquivoCorrente.CodigoJuridicoMat)
                cmd.Parameters.AddWithValue("@StatusMat", "A")
                cmd.Parameters.AddWithValue("@MaterialSW", UCase(DadosArquivoCorrente.material))
                cmd.Parameters.AddWithValue("@EnderecoArquivo", UCase(DadosArquivoCorrente.EnderecoArquivo)) ' mantém as barras
                cmd.Parameters.AddWithValue("@Acabamento", UCase(DadosArquivoCorrente.Acabamento))
                cmd.Parameters.AddWithValue("@txtSoldagem", UCase(DadosArquivoCorrente.soldagem))
                cmd.Parameters.AddWithValue("@txtTipoDesenho", UCase(DadosArquivoCorrente.TipoDesenho))
                cmd.Parameters.AddWithValue("@txtCorte", UCase(DadosArquivoCorrente.Corte))
                cmd.Parameters.AddWithValue("@txtDobra", UCase(DadosArquivoCorrente.Dobra))
                cmd.Parameters.AddWithValue("@txtSolda", UCase(DadosArquivoCorrente.Solda))
                cmd.Parameters.AddWithValue("@txtPintura", UCase(DadosArquivoCorrente.Pintura))
                cmd.Parameters.AddWithValue("@txtMontagem", UCase(DadosArquivoCorrente.Montagem))
                cmd.Parameters.AddWithValue("@Compcx", Replace(DadosArquivoCorrente.Alturacaixadelimitadora, ",", "."))
                cmd.Parameters.AddWithValue("@Largcx", Replace(DadosArquivoCorrente.Larguracaixadelimitadora, ",", "."))
                cmd.Parameters.AddWithValue("@Espcx", Replace(DadosArquivoCorrente.Profundidadeaixadelimitadora, ",", "."))
                cmd.Parameters.AddWithValue("@txtItemEstoque", UCase(DadosArquivoCorrente.ItemEstoque))
                cmd.Parameters.AddWithValue("@D_E_L_E_T_E", "")
                cmd.Parameters.AddWithValue("@EnderecoImagem", UCase(DadosArquivoCorrente.EnderecoImagem))
                cmd.Parameters.AddWithValue("@Bloqueado", DadosArquivoCorrente.Bloqueado)


                cmd.ExecuteNonQuery()


            End Using

            '' ============================================================
            '' 🔹 Integração API Protheus (Criar Produto)
            '' ============================================================
            'Try
            '    ' Ignorar erros de certificado SSL (ambiente de testes/local) e forçar TLS 1.2
            '    System.Net.ServicePointManager.SecurityProtocol = DirectCast(3072, System.Net.SecurityProtocolType) Or System.Net.SecurityProtocolType.Tls11 Or System.Net.SecurityProtocolType.Tls
            '    System.Net.ServicePointManager.ServerCertificateValidationCallback = Function(sender, cert, chain, sslPolicyErrors) True

            '    ' 1. Obter Token Bearer
            '    Dim tokenUrl As String = "https://192.168.1.60:47500/tlpp/oauth2/token?grant_type=password&username=sinco&password=Metal1120"
            '    Dim tokenRequest As System.Net.HttpWebRequest = CType(System.Net.WebRequest.Create(tokenUrl), System.Net.HttpWebRequest)
            '    tokenRequest.Method = "GET"
            '    tokenRequest.Timeout = 10000 ' 10 segundos timeout

            '    Dim accessToken As String = ""
            '    Using tokenResponse As System.Net.HttpWebResponse = CType(tokenRequest.GetResponse(), System.Net.HttpWebResponse)
            '        Using reader As New System.IO.StreamReader(tokenResponse.GetResponseStream())
            '            Dim responseText As String = reader.ReadToEnd()
            '            ' Busca simples pelo token no JSON via Regex para evitar dependência externa
            '            Dim match As System.Text.RegularExpressions.Match = System.Text.RegularExpressions.Regex.Match(responseText, """access_token""\s*:\s*""([^""]+)""")
            '            If match.Success Then
            '                accessToken = match.Groups(1).Value
            '            End If
            '        End Using
            '    End Using

            '    ' 2. Realizar POST caso tenha obtido o token
            '    If Not String.IsNullOrEmpty(accessToken) Then
            '        Dim postUrl As String = "https://192.168.1.60:47500/produtos/create"
            '        Dim postRequest As System.Net.HttpWebRequest = CType(System.Net.WebRequest.Create(postUrl), System.Net.HttpWebRequest)
            '        postRequest.Method = "POST"
            '        postRequest.ContentType = "application/json"
            '        postRequest.Headers.Add("Authorization", "Bearer " & accessToken)
            '        postRequest.Timeout = 15000 ' 15 segundos timeout

            '        ' Montar JSON dinâmico baseado na peça corrente
            '        Dim descProduto As String = UCase(DadosArquivoCorrente.Titulo)
            '        If String.IsNullOrEmpty(descProduto) Then descProduto = "PRODUTO XPTO 01"
            '        Dim codDesenho As String = DadosArquivoCorrente.NomeArquivoSemExtensao
            '        If String.IsNullOrEmpty(codDesenho) Then codDesenho = "desenho_metalfisa"

            '        ' Substitui aspas e quebras de linha para evitar quebro em JSON mal-formado
            '        descProduto = descProduto.Replace("""", "\""").Replace(vbCrLf, " ").Replace(vbLf, " ").Replace(vbCr, " ")
            '        codDesenho = codDesenho.Replace("""", "\""").Replace(vbCrLf, " ").Replace(vbLf, " ").Replace(vbCr, " ")

            '        Dim jsonPayload As String = "{" &
            '            " ""data"": {" &
            '            "  ""produtos"": [" &
            '            "   {" &
            '            "    ""EMPRESA"": ""01""," &
            '            "    ""CFILANT"": ""01""," &
            '            "    ""B1_GRUPO"": ""MT2""," &
            '            "    ""B1_DESC"": """ & descProduto & """," &
            '            "    ""B1_XREVM"": ""01""," &
            '            "    ""B1_TIPO"": ""PA""," &
            '            "    ""B1_UM"": ""UN""," &
            '            "    ""B1_LOCPAD"": ""03""," &
            '            "    ""B1_POSIPI"": ""00000000""," &
            '            "    ""B1_FINALID"": ""1""," &
            '            "    ""B1_ORIGEM"": ""0""," &
            '            "    ""B1_XCODDES"": """ & codDesenho & """" &
            '            "   }" &
            '            "  ]" &
            '            " }" &
            '            "}"

            '        Using writer As New System.IO.StreamWriter(postRequest.GetRequestStream())
            '            writer.Write(jsonPayload)
            '        End Using

            '        ' Enviar e ler a resposta do Protheus
            '        Using postResponse As System.Net.HttpWebResponse = CType(postRequest.GetResponse(), System.Net.HttpWebResponse)
            '            Using reader As New System.IO.StreamReader(postResponse.GetResponseStream())
            '                Dim responseResult As String = reader.ReadToEnd()
            '                System.Diagnostics.Debug.WriteLine("SINCO: Produto criado na API Protheus. Resposta: " & responseResult)
            '            End Using
            '        End Using
            '    Else
            '        System.Diagnostics.Debug.WriteLine("SINCO: Falha ao obter token de acesso na API Protheus.")
            '    End If
            'Catch exAPI As Exception
            '    ' Apenas loga o erro, para não travar o processo principal de salvamento (MySQL)
            '    System.Diagnostics.Debug.WriteLine("SINCO: Erro na integração com a API Protheus -> " & exAPI.Message)
            'End Try

        Catch ex As Exception

            MessageBox.Show("❌ Erro ao salvar: " & ex.Message, "SINCO - Banco de Dados", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally

            cl_BancoDados.FecharBanco()

        End Try
    End Sub


    ' ============================================================
    ' 🧱 Helper para adicionar parâmetros
    ' ============================================================
    Private Sub AddTextParameterMysql(cmd As MySqlCommand, paramName As String, value As Object)
        cmd.Parameters.AddWithValue(paramName, If(value Is Nothing, String.Empty, value.ToString().Trim()))
    End Sub




End Class
