Imports System.Collections.Generic
Imports System.Net
Imports System.Net.Http
Imports System.Text
Imports System.Threading.Tasks
Imports System.Windows.Forms
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq
Imports SINCO_SolidEdeg_1._0.DocumentoDesenho
Imports Environment = System.Environment

Public Class ClIA

    Private Shared ReadOnly client As New HttpClient()

    Public Async Function ObterRespostaGPT(prompt As String, Agente As String) As Task(Of String)

        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12

        ' A chave da API é lida de variável de ambiente para não ficar exposta no código.
        ' Configure: painel de controle > variáveis de ambiente > OPENAI_API_KEY
        Dim apiKey As String = Environment.GetEnvironmentVariable("OPENAI_API_KEY")
        If String.IsNullOrWhiteSpace(apiKey) Then
            Return "Erro: variável de ambiente OPENAI_API_KEY não configurada."
        End If
        Dim url As String = "https://api.openai.com/v1/chat/completions"

        Dim requestData = New With {
            .model = "gpt-5.1",
            .messages = New Object() {
                New With {.role = "system", .content = Agente},
                New With {.role = "user", .content = prompt}
            },
            .temperature = 0.7
        }

        Dim json = JsonConvert.SerializeObject(requestData)
        Dim content = New StringContent(json, Encoding.UTF8, "application/json")

        client.DefaultRequestHeaders.Clear()
        client.DefaultRequestHeaders.Add("Authorization", "Bearer " & apiKey)

        Try
            Dim response = Await client.PostAsync(url, content)
            Dim responseString = Await response.Content.ReadAsStringAsync()

            If response.IsSuccessStatusCode Then
                Dim jsonResponse As OpenAIResponse = JsonConvert.DeserializeObject(Of OpenAIResponse)(responseString)

                If jsonResponse IsNot Nothing AndAlso jsonResponse.choices.Count > 0 Then
                    Return jsonResponse.choices(0).message.content.Trim()
                Else
                    Return "A resposta da API não continha escolhas válidas."
                End If
            Else
                Return "Erro HTTP da API: " & response.StatusCode & Environment.NewLine & responseString
            End If
        Catch ex As Exception
            Return "Erro: " & ex.Message
        End Try
    End Function

    ' Classes para deserializar a resposta da OpenAI
    Public Class OpenAIResponse
        Public Property choices As List(Of Choice)
    End Class

    Public Class Choice
        Public Property message As Message
        Public Property finish_reason As String
        Public Property index As Integer
    End Class

    Public Class Message
        Public Property role As String
        Public Property content As String
    End Class


    Public Shared ReadOnly Property AgenteExtracaoDesenhoCompleto As String
        Get
            Dim sb As New System.Text.StringBuilder()

            sb.AppendLine(
                "Você é um especialista em leitura de textos extraídos de arquivos PDF de desenhos técnicos (carimbo, notas, tolerâncias, cotas, listas, etc.)." & vbLf &
"Receberá o TEXTO BRUTO de um PDF (sem formatação). Sua tarefa é extrair o MÁXIMO de informações, retornando sempre um único JSON VÁLIDO exatamente com as chaves abaixo (sem comentários, sem texto extra):" & vbLf &
"" & vbLf &
"{" & vbLf &
" ""numero_desenho"": null," & vbLf &
" ""descricao_desenho"": null," & vbLf &
" ""titulo"": null," & vbLf &
"" & vbLf &
" ""material"": null," & vbLf &
" ""acabamento"": null," & vbLf &
"" & vbLf &
" ""formato"": null," & vbLf &
" ""escala"": null," & vbLf &
"" & vbLf &
" ""peso_valor"": null," & vbLf &
" ""peso_unidade"": null," & vbLf &
"" & vbLf &
" ""Area_de_Pintura"": null," & vbLf &
"" & vbLf &
" ""cliente"": null," & vbLf &
" ""codigo_cliente"": null," & vbLf &
" ""caminho_arquivo"": null," & vbLf &
"" & vbLf &
" ""revisao"": null," & vbLf &
"" & vbLf &
" ""espessura"": null," & vbLf &
" ""largurablank"": null," & vbLf &
" ""comprimentoblank"": null" & vbLf &
"}" & vbLf &
"" & vbLf &
"REGRAS GERAIS" & vbLf &
"- Saída: apenas um objeto JSON exatamente com as chaves acima, na mesma ordem. Sem texto antes/depois." & vbLf &
"- Se um campo não puder ser identificado com confiança, use null." & vbLf &
"- Não invente informações. Normalize espaços, hífens/traços, separadores e acentuação quando necessário." & vbLf &
"- Quando extrair peso, separe: ""peso_valor"" (apenas número como string) e ""peso_unidade"" (ex.: ""kg"")." & vbLf &
"- numero_desenho: detecte padrões MT (MT1…MT8, MTP, MTA), normalize com hífen se pertinente (ex.: ""MT4-23699"")." & vbLf &
"- formato: procure ""FORMATO A3/A4 …"" no carimbo. escala: valores como ""1:1"" ou ""S/E""." & vbLf &
"- material: linhas com ""CHAPA DE AÇO … mm""; extraia a espessura preferencialmente daqui." & vbLf &
"- acabamento: termos como ""PINTADO"". cliente/codigo_cliente: nomes/códigos no carimbo. caminho_arquivo: caminhos completos se aparecerem." & vbLf &
"- revisao: letra/número próximo de ""REV."" / ""REVISÃO""." & vbLf &
"" & vbLf &
"HEURÍSTICAS PARA ESPESSURA / LARGURABLANK / COMPRIMENTOBLANK" & vbLf &
"1) espessura (chapa):" & vbLf &
" - Priorize a espessura presente na linha de material (ex.: ""CHAPA DE AÇO … 2,25 MM""). Extraia apenas o número como string." & vbLf &
" - Se houver ""ESPESSURA: 2,25 mm"", capture ""2,25"". Se houver 4,75/2.25 etc., mantenha o separador usado no texto." & vbLf &
" - Se não houver evidência de chaparia/planificação, retorne null." & vbLf &
"2) largurablank/comprimentoblank (dimensões do blank/planificado):" & vbLf &
" - Procure pares de cotas máximas (retângulo externo) da peça planificada, geralmente em mm." & vbLf &
" - Use termos como ""planificado"", ""flat"", ""cut size"", ""dimensões do blank"", ""largura x comprimento""." & vbLf &
" - Se houver CutSizeX/CutSizeY, mapeie: CutSizeX → largurablank; CutSizeY → comprimentoblank." & vbLf &
" - Se aparecerem várias medidas grandes, assuma largurablank = menor dos dois maiores valores e comprimentoblank = maior dos dois maiores, desde que pertençam ao mesmo bloco lógico." & vbLf &
" - Ignore valores pequenos óbvios (furos/raios). Se não houver indícios confiáveis, retorne null." & vbLf &
"" & vbLf &
"TRATAMENTO DE UNIDADES E FORMATAÇÃO" & vbLf &
"- Remova ""mm"", ""MM"", ""m²"", ""kg"" do valor onde apropriado. Preserve ""peso_unidade""." & vbLf &
"- Converta vírgula/ponto apenas quando necessário; preferir vírgula se o texto estiver em pt-BR." & vbLf &
"- Ignore ruídos típicos do carimbo (blocos de tolerância, letras de margem, etc.)." & vbLf &
"" & vbLf &
"VALIDAÇÃO / CONFIANÇA" & vbLf &
"- Se candidatos de blank forem muito próximos (ex.: 807,3 e 813,7), escolha o par que melhor representa o retângulo externo (maiores dimensões globais). Não some cotas parciais." & vbLf &
"- Ex.: ""PESO: 006,570 kg"" → ""peso_valor"": ""6,570"", ""peso_unidade"": ""kg""." & vbLf &
"- Se o campo for ambíguo, mantenha null." & vbLf &
"" & vbLf &
"Saída: um único objeto JSON com as chaves especificadas, nesta ordem, e valores extraídos ou null."
)

            Return sb.ToString()
        End Get
    End Property

    Public Shared Async Function LerDadosDesenhoComIA(caminhoPdf As String) As Task(Of DocumentoDesenho)
        ' 1) Extrair texto
        Dim textoPdf As String = PdfUtils.ExtrairTextoPdf(caminhoPdf)

        If String.IsNullOrWhiteSpace(textoPdf) Then
            ' Se você tiver OCR, habilite aqui:
            ' textoPdf = PdfUtils.ExtrairTextoPdfComOcr(caminhoPdf)
            If String.IsNullOrWhiteSpace(textoPdf) Then
                Throw New Exception("Não foi possível extrair texto do PDF (pode ser somente imagem/vetorial).")
            End If
        End If

        ' 2) Prompt de usuário
        Dim promptUsuario As String =
        "Segue abaixo o texto bruto extraído de um arquivo PDF de desenho técnico." & Environment.NewLine &
        "Use o AGENTE (system prompt) para extrair o máximo de informações possíveis, no formato JSON especificado." & Environment.NewLine &
        "TEXTO DO PDF:" & Environment.NewLine &
        "------------------------" & Environment.NewLine &
        textoPdf & Environment.NewLine &
        "------------------------"

        Dim agente As String = AgenteExtracaoDesenhoCompleto

        ' 3) Chamada ao modelo
        Dim respostaBruta As String = Await AgentesIA.ObterRespostaGPT(promptUsuario, agente)

        ' 4) Extrair exclusivamente o JSON (caso venha com texto antes/depois)
        Dim json As String = TryExtractFirstJsonObject(respostaBruta)
        If String.IsNullOrWhiteSpace(json) Then
            ' como fallback, tenta a resposta inteira
            json = respostaBruta
        End If

        ' 5) Desserializar de forma tolerante
        Dim settings As New JsonSerializerSettings With {
        .MissingMemberHandling = MissingMemberHandling.Ignore,
        .NullValueHandling = NullValueHandling.Include,
        .DateParseHandling = DateParseHandling.None
    }

        Try
            Dim doc As DocumentoDesenho = JsonConvert.DeserializeObject(Of DocumentoDesenho)(json, settings)

            If doc Is Nothing Then doc = New DocumentoDesenho()
            If doc.itens_bom Is Nothing Then doc.itens_bom = New List(Of BomItem)()
            If doc.campos_extras Is Nothing Then doc.campos_extras = New Dictionary(Of String, String)()

            Return doc
        Catch ex As Exception
            Throw New Exception("Falha ao desserializar a resposta da IA. JSON recebido:" &
                            Environment.NewLine & json, ex)
        End Try
    End Function

    ' Extrai o primeiro objeto JSON {...} válido de uma string
    Private Shared Function TryExtractFirstJsonObject(text As String) As String
        If String.IsNullOrWhiteSpace(text) Then Return Nothing

        ' Heurística: encontra par de chaves balanceadas
        Dim startIdx = text.IndexOf("{"c)
        While startIdx >= 0 AndAlso startIdx < text.Length
            Dim depth As Integer = 0
            For i = startIdx To text.Length - 1
                Dim ch = text(i)
                If ch = "{"c Then depth += 1
                If ch = "}"c Then
                    depth -= 1
                    If depth = 0 Then
                        Dim candidate = text.Substring(startIdx, i - startIdx + 1).Trim()
                        ' valida rapidamente
                        If LooksLikeJson(candidate) Then
                            Return candidate
                        End If
                        Exit For
                    End If
                End If
            Next
            startIdx = text.IndexOf("{"c, startIdx + 1)
        End While

        Return Nothing
    End Function

    Private Shared Function LooksLikeJson(s As String) As Boolean
        Try
            Dim token = JToken.Parse(s)
            Return token IsNot Nothing AndAlso token.Type = JTokenType.Object
        Catch
            Return False
        End Try
    End Function


End Class
