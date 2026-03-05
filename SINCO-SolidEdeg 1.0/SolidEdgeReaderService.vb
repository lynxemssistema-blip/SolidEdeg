Imports System.Runtime.InteropServices
Imports System.Text

Public Class SolidEdgeReaderService

    ''' <summary>
    ''' Lê as propriedades do arquivo corrente do Solid Edge e preenche o objeto clDadosArquivoCorrente fornecido.
    ''' </summary>
    ''' <param name="doc">O documento ativo do Solid Edge (app.ActiveDocument)</param>
    ''' <param name="dadosArquivoCorrente">Objeto que receberá os dados extraídos</param>
    Public Sub ExtrairPropriedades(ByVal doc As Object, ByRef dadosArquivoCorrente As clDadosArquivoCorrente)
        Try
            If doc Is Nothing Then Exit Sub

            Dim caminho As String = doc.FullName
            Dim ext As String = IO.Path.GetExtension(caminho).ToLower()
            Dim nomeArquivo As String = IO.Path.GetFileNameWithoutExtension(caminho).ToUpper()

            ' Apenas processa arquivos válidos do Solid Edge
            If Not (ext = ".par" Or ext = ".psm" Or ext = ".asm") Then Exit Sub

            dadosArquivoCorrente.EnderecoArquivo = caminho
            dadosArquivoCorrente.NomeArquivoSemExtensao = nomeArquivo & ext
            dadosArquivoCorrente.NomeArquivoComExtensao = $"{nomeArquivo}{ext}"

            ' Define um conjunto de propriedades que estamos interessados em extrair para otimizar a iteração
            Dim propsInteressantes As New HashSet(Of String)(StringComparer.OrdinalIgnoreCase) From {
                "Título", "Assunto", "Coment", "Palavras-", "Autor", "Empresa", "Categoria", "Gerente",
                "Material", "Data", "DataR", "Data1", "Revision", "CutSizeX", "CutSizeY",
                "Material Thickness", "Mass", "Área_de_superfície", "Area_de_superficie", "Bloqueado", "Tipo de Desenho",
                "Document Number", "Title", "Subject", "Author", "Keywords", "Comments", "Category", "Company"
            }

            Dim propSets As Object = doc.Properties

            For Each propSet As Object In propSets
                For Each prop As Object In propSet
                    Try
                        ' Ignora propriedades cujo nome não nos interessa
                        If Not propsInteressantes.Any(Function(p) prop.Name.IndexOf(p, StringComparison.OrdinalIgnoreCase) >= 0) Then Continue For

                        Dim valorRaw As Object = Nothing
                        Try
                            valorRaw = prop.Value
                        Catch
                            Continue For
                        End Try

                        Dim valor As String = If(valorRaw IsNot Nothing, valorRaw.ToString(), "")

                        ' Mapeamento de Propriedades
                        Select Case True
                            ' Informações Gerais
                            Case prop.Name.IndexOf("Título", StringComparison.OrdinalIgnoreCase) >= 0 OrElse prop.Name.IndexOf("Title", StringComparison.OrdinalIgnoreCase) >= 0
                                dadosArquivoCorrente.Titulo = valor
                            Case prop.Name.IndexOf("Assunto", StringComparison.OrdinalIgnoreCase) >= 0 OrElse prop.Name.IndexOf("Subject", StringComparison.OrdinalIgnoreCase) >= 0
                                dadosArquivoCorrente.AssuntoSubiTitulo = valor
                            Case prop.Name.IndexOf("Bloqueado", StringComparison.OrdinalIgnoreCase) >= 0
                                dadosArquivoCorrente.Bloqueado = valor
                            Case prop.Name.IndexOf("Coment", StringComparison.OrdinalIgnoreCase) >= 0 OrElse prop.Name.IndexOf("Comments", StringComparison.OrdinalIgnoreCase) >= 0
                                dadosArquivoCorrente.Comentarios = valor
                            Case prop.Name.IndexOf("Palavras-chave", StringComparison.OrdinalIgnoreCase) >= 0 OrElse prop.Name.IndexOf("Keywords", StringComparison.OrdinalIgnoreCase) >= 0
                                dadosArquivoCorrente.PalavraChave = valor
                            Case prop.Name.IndexOf("Autor", StringComparison.OrdinalIgnoreCase) >= 0 OrElse prop.Name.IndexOf("Author", StringComparison.OrdinalIgnoreCase) >= 0
                                dadosArquivoCorrente.Author = valor
                            Case prop.Name.IndexOf("Empresa", StringComparison.OrdinalIgnoreCase) >= 0 OrElse prop.Name.IndexOf("Company", StringComparison.OrdinalIgnoreCase) >= 0
                                dadosArquivoCorrente.CodigoJuridicoMat = valor
                            Case prop.Name.IndexOf("Categ", StringComparison.OrdinalIgnoreCase) >= 0 OrElse prop.Name.IndexOf("Category", StringComparison.OrdinalIgnoreCase) >= 0
                                dadosArquivoCorrente.Verificado = valor
                            Case prop.Name.IndexOf("Gerente", StringComparison.OrdinalIgnoreCase) >= 0
                                dadosArquivoCorrente.Aprovado = valor
                            Case String.Equals(prop.Name, "Material", StringComparison.OrdinalIgnoreCase)
                                dadosArquivoCorrente.material = valor
                            Case prop.Name.IndexOf("Tipo de Desenho", StringComparison.OrdinalIgnoreCase) >= 0
                                dadosArquivoCorrente.TipoDesenho = valor

                            ' Datas
                            Case String.Equals(prop.Name, "Data", StringComparison.OrdinalIgnoreCase)
                                dadosArquivoCorrente.DataCriacaDesenho = FormatarDataSegura(valor)
                            Case String.Equals(prop.Name, "DataR", StringComparison.OrdinalIgnoreCase)
                                dadosArquivoCorrente.DataUltimoSalvamento = FormatarDataSegura(valor)

                            ' Dimensões e Físicas
                            Case prop.Name.IndexOf("CutSizeX", StringComparison.OrdinalIgnoreCase) >= 0
                                dadosArquivoCorrente.ComprimentoBlank = LimparUnidades(valor)
                            Case prop.Name.IndexOf("CutSizeY", StringComparison.OrdinalIgnoreCase) >= 0
                                dadosArquivoCorrente.LarguraBlank = LimparUnidades(valor)
                            Case prop.Name.IndexOf("Material Thickness", StringComparison.OrdinalIgnoreCase) >= 0 OrElse prop.Name.IndexOf("Espessura do Material", StringComparison.OrdinalIgnoreCase) >= 0
                                dadosArquivoCorrente.Espessura = LimparUnidades(valor)
                            Case prop.Name.IndexOf("Mass", StringComparison.OrdinalIgnoreCase) >= 0
                                dadosArquivoCorrente.Massa = LimparUnidades(valor)
                            Case prop.Name.IndexOf("Área_de_superfície", StringComparison.OrdinalIgnoreCase) >= 0 OrElse prop.Name.IndexOf("Area_de_superficie", StringComparison.OrdinalIgnoreCase) >= 0
                                dadosArquivoCorrente.AreaPintura = ConverterAreaParaMetrosQuadrados(valor)

                        End Select

                    Catch ex As Exception
                        Continue For
                    End Try
                Next
            Next

            ' Concatena acabamento
            dadosArquivoCorrente.Acabamento = $"{dadosArquivoCorrente.Comentarios} - {dadosArquivoCorrente.PalavraChave}"

        Catch ex As Exception
            ' Falha silenciosa no Service ou pode gerar Log
            Console.WriteLine($"Erro ao extrair propriedades do arquivo corrente: {ex.Message}")
        End Try
    End Sub

    ''' <summary>
    ''' Remove strings de unidades comuns encontradas no Solid Edge
    ''' </summary>
    Private Function LimparUnidades(texto As String) As String
        If String.IsNullOrWhiteSpace(texto) Then Return ""
        Return texto _
            .Replace("mm²", "") _
            .Replace("MM²", "") _
            .Replace("mm", "") _
            .Replace("MM", "") _
            .Replace("^2", "") _
            .Replace("KG", "") _
            .Replace("kg", "") _
            .Trim()
    End Function

    ''' <summary>
    ''' Converte string de área (mm²) para string de metros quadrados (m²)
    ''' </summary>
    Private Function ConverterAreaParaMetrosQuadrados(valor As String) As String
        Dim limpo As String = LimparUnidades(valor)
        Dim valorNumerico As Double

        If Double.TryParse(limpo.Replace(",", "."), Globalization.NumberStyles.Any, Globalization.CultureInfo.InvariantCulture, valorNumerico) Then
            Dim metrosQuadrados As Double = valorNumerico / 1000000.0
            Return metrosQuadrados.ToString("N4", Globalization.CultureInfo.GetCultureInfo("pt-BR"))
        Else
            Return valor
        End If
    End Function

    ''' <summary>
    ''' Formata uma string de data tentando garantir o padrão de exibição
    ''' </summary>
    Private Function FormatarDataSegura(valor As Object) As String
        If valor Is Nothing Then Return ""
        If IsDate(valor) Then
            Return CDate(valor).ToString("dd/MM/yyyy")
        Else
            Return valor.ToString()
        End If
    End Function

End Class
