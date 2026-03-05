Imports System.Text
Imports UglyToad.PdfPig

Public Class ClDadosDesenhoPdf

End Class

' Representa o desenho/documento lido do PDF
Public Class DocumentoDesenho
    ' ——— Cabeçalho principal ———
    Public Property numero_desenho As String
    Public Property descricao_desenho As String
    Public Property titulo As String

    Public Property material As String
    Public Property acabamento As String

    Public Property formato As String
    Public Property escala As String

    Public Property peso_valor As String     ' ex: "0,264"
    Public Property peso_unidade As String   ' ex: "kg"

    Public Property espessura As String
    Public Property largurablank As String
    Public Property comprimentoblank As String

    Public Property tipo_de_desenho As String

    Public Property Area_de_Pintura As String

    Public Property cliente As String
    Public Property codigo_cliente As String
    Public Property caminho_arquivo As String

    Public Property revisao As String

    ' ——— Folhas (carimbo) ———
    Public Property folha_atual As String
    Public Property total_folhas As String

    ' ——— Responsáveis e datas ———
    Public Property responsavel_desenho As String
    Public Property responsavel_aprovacao As String
    Public Property responsavel_revisao As String

    Public Property data_desenho As String
    Public Property data_aprovacao As String
    Public Property data_revisao As String

    ' ——— Notas/Tolerâncias (texto livre) ———
    Public Property tolerancias_texto As String
    Public Property observacoes As String

    ' ——— BOM ———
    Public Property itens_bom As List(Of BomItem)

    ' ——— Extras genéricos ———
    Public Property campos_extras As Dictionary(Of String, String)
End Class

Public Class BomItem
    Public Property item As String                ' ex: "1"
    Public Property document_number As String     ' ex: "MT5-19896.psm"
    Public Property title As String               ' ex: "TRILHO"
    Public Property material As String
    Public Property quantity As String            ' ex: "3"
    Public Property unidade As String             ' opcional
End Class


'Public Overrides Function ToString() As String
'        Return $"Número Desenho: {numero_desenho}" & Environment.NewLine &
'               $"Nome Documento: {nome_documento}" & Environment.NewLine &
'                 $"Desenho: {Desenho}" & Environment.NewLine &
'                   $"Revisao: {Revisao}" & Environment.NewLine &
'                     $"Peso: {Peso}" & Environment.NewLine &
'                       $"Acabamento: {Acabamento}" & Environment.NewLine &
'                         $"Descrição: {Descrição}" & Environment.NewLine &
'                               $"Cod_Metalfisa: {Cod_Metalfisa}" & Environment.NewLine &
'                                     $"Notas_Gerais: {Notas_Gerais}" & Environment.NewLine &
'               $"Material: {material}"
'    End Function



Public Class PdfUtils

    Public Shared Function ExtrairTextoPdf(caminhoArquivoPdf As String) As String
        Dim sb As New StringBuilder()

        Using document = PdfDocument.Open(caminhoArquivoPdf)
            For Each page In document.GetPages()
                sb.AppendLine(page.Text)
            Next
        End Using

        Return sb.ToString()
    End Function


End Class
