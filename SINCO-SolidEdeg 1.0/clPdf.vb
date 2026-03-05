

Imports iText.Kernel.Pdf
Imports iText.Kernel.Pdf.Canvas

Public Class clPdf

    Public Sub EscreverPdf(EnderecoCompleto As String, caminhoArquivoDestino As String, Parametro As String)
        Try
            ' Substitui extensões ".sldprt" ou ".sldasm" por ".pdf"
            Dim inputPdf As String = System.Text.RegularExpressions.Regex.Replace(EnderecoCompleto, ".sldprt$|.sldasm$", ".pdf", System.Text.RegularExpressions.RegexOptions.IgnoreCase)

            ' Verifica se o arquivo de entrada existe
            If Not System.IO.File.Exists(inputPdf) Then
                MsgBox("O arquivo PDF de entrada não existe: " & inputPdf)
                Exit Sub
            End If

            ' Extrai o nome do arquivo de entrada
            Dim fileName As String = System.IO.Path.GetFileName(inputPdf)

            ' Monta o caminho do arquivo de saída corretamente
            Dim outputPdf As String = System.IO.Path.Combine(caminhoArquivoDestino, "qtde_" & fileName)

            ' Verifica ou cria o diretório de saída
            Dim outputDirectory As String = System.IO.Path.GetDirectoryName(outputPdf)
            If Not System.IO.Directory.Exists(outputDirectory) Then
                System.IO.Directory.CreateDirectory(outputDirectory)
            End If

            ' Inicializa o PdfReader
            Dim pdfReader As iText.Kernel.Pdf.PdfReader = Nothing
            Try
                pdfReader = New iText.Kernel.Pdf.PdfReader(inputPdf)
            Catch ex As Exception
                '  MsgBox("Erro ao abrir o arquivo PDF de entrada: " & ex.Message)
                Exit Sub
            End Try

            ' Inicializa o PdfWriter e PdfDocument
            Dim pdfWriter As New iText.Kernel.Pdf.PdfWriter(outputPdf)
            Dim pdfDocument As New iText.Kernel.Pdf.PdfDocument(pdfReader, pdfWriter)

            ' Obtém a primeira página do PDF
            Dim page As iText.Kernel.Pdf.PdfPage = pdfDocument.GetFirstPage()
            Dim canvas As New iText.Kernel.Pdf.Canvas.PdfCanvas(page)

            ' Configura o texto a ser inserido
            Dim font As iText.Kernel.Font.PdfFont = iText.Kernel.Font.PdfFontFactory.CreateFont(iText.IO.Font.Constants.StandardFonts.HELVETICA)
            canvas.BeginText()
            canvas.SetFontAndSize(font, 12)
            canvas.MoveText(50, 800) ' Define a posição do texto
            canvas.ShowText($"OS: {Parametro} - Data de Emissão do Desenho: {DateTime.Now:dd/MM/yyyy}")
            canvas.EndText()

            ' Fecha o documento PDF
            pdfDocument.Close()

            MsgBox("Texto inserido com sucesso no arquivo: " & outputPdf)
        Catch ex As Exception
            '  MsgBox("Erro ao processar o PDF: " & ex.Message & vbCrLf & ex.StackTrace)
            If ex.InnerException IsNot Nothing Then
                MsgBox("Erro interno: " & ex.InnerException.Message)
            End If
        End Try
    End Sub


    Public Sub EditarPDFParaOs(Origem As String, caminhoArquivoDestino As String, novoNomeArquivo As String, QtdeTotal As String, Identificado As String, Acabamento As String, TipoIdentidicado As String)

        Origem = System.Text.RegularExpressions.Regex.Replace(Origem, ".sldprt$|.sldasm$", ".pdf", System.Text.RegularExpressions.RegexOptions.IgnoreCase)

        ' Verifica se o arquivo de entrada existe
        If System.IO.File.Exists(Origem) Then
            Try
                ' Abre o PDF de entrada
                Dim pdfReader As New PdfReader(Origem)
                ' Cria um escritor para o novo arquivo PDF
                Dim pdfWriter As New PdfWriter(caminhoArquivoDestino)
                ' Abre o documento PDF para edição
                Dim pdfDocument As New PdfDocument(pdfReader, pdfWriter)

                ' Acessa a primeira página do PDF
                Dim page As PdfPage = pdfDocument.GetPage(1)
                ' Cria um Canvas para desenhar na página
                Dim canvas As New PdfCanvas(page)

                ' Define a posição para inserir o texto no canto inferior direito
                Dim pageWidth As Single = pdfDocument.GetDefaultPageSize().GetWidth() ' Largura da página
                Dim marginRight As Single = 10 ' Margem direita
                Dim posX As Single = pageWidth - marginRight ' Posição X no canto direito
                Dim posY As Single = 1 ' Posição Y no rodapé

                canvas.BeginText()
                canvas.SetFontAndSize(iText.Kernel.Font.PdfFontFactory.CreateFont(), 12)
                canvas.SetTextMatrix(posX, posY) ' Define a posição inicial do texto
                canvas.ShowText(TipoIdentidicado & " " & Identificado & " - Qtde. " & QtdeTotal & " - Acab. " & Acabamento & " - Emissão " & Date.Now).ToString()
                canvas.EndText()

                ' Fecha o documento PDF
                pdfDocument.Close()

                ' Pode adicionar um log ou mensagem de sucesso se necessário
                ' Console.WriteLine("Texto inserido com sucesso no arquivo: " & outputPdf)
            Catch ex As Exception
                ' Em caso de erro, exibe uma mensagem
                '  Console.WriteLine("Erro ao processar o PDF: " & ex.Message)

                'caminhoArquivoDestino = Path.GetFullPath(caminhoArquivoDestino)
                'File.Delete(caminhoArquivoDestino)

                'Origem = Path.GetFullPath(Origem)
                'caminhoArquivoDestino = Path.GetFullPath(caminhoArquivoDestino)

                'File.Copy(Origem, caminhoArquivoDestino, True)

                cl_BancoDados.CopiarArquivoInteligente(Origem, caminhoArquivoDestino)
            Finally

            End Try

        End If

    End Sub


End Class
