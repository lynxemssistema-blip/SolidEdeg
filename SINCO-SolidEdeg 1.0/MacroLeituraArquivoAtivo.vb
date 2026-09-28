'Imports SolidEdgeFramework
Imports System.Drawing
Imports System.IO
Imports System.Runtime.InteropServices
Imports System.Threading
Imports System.Windows.Forms
Imports Microsoft.Extensions.Logging
Imports Microsoft.Office.Interop.Excel
Imports SolidEdgeGeometry
Imports SolidEdgePart
Imports Thread = System.Threading.Thread

Module MacroLeituraArquivoAtivo

    Public frm As New frmDadosPecaCorrente()

    ' Funções Win32 para manipular janelas
    <DllImport("user32.dll", SetLastError:=True)>
    Private Function SetParent(ByVal hWndChild As IntPtr, ByVal hWndNewParent As IntPtr) As IntPtr
    End Function

    Sub Main()
        ''Try
        ''    ' 🔹 Conecta ao Solid Edge
        ''    Dim app As SolidEdgeFramework.Application = Marshal.GetActiveObject("SolidEdge.Application")

        ''    ' 🔹 Exibe o formulário (não modal)
        ''    frm.Show()
        ''    frm.TopMost = True
        ''    frm.BringToFront()

        ''    ' 🔹 Define o Solid Edge como janela “pai” do form
        ''    SetParent(frm.Handle, CType(app.hWnd, IntPtr))

        ''    ' 🔹 Mantém a UI viva sem bloquear o Solid Edge
        ''    Do While frm.Visible
        ''        Application.DoEvents()
        ''        Threading.Thread.Sleep(50)
        ''    Loop

        ''Catch ex As Exception
        ''    MsgBox("Erro: " & ex.Message, MsgBoxStyle.Critical)
        ''Finally
        ''    frm.Dispose()
        ''End Try

        ' 🔹 Registra o filtro de mensagens OLE para evitar rejeições COM (0x8001010A / RPC_E_SERVERCALL_RETRYLATER)
        OleMessageFilter.Register()

        ' 🔹 Garante que o Solid Edge esteja ativo e 100% interativo para o usuário
        SolidEdgeFactory.AtivarSolidEdge()

        Using login As New frmLogin()
            If login.ShowDialog() <> DialogResult.OK Then
                Return
            End If
        End Using

        ' 🔹 Define posição no segundo monitor (se disponível) ou centraliza
        Dim monitores = Screen.AllScreens
        If monitores.Length > 1 Then
            frm.StartPosition = FormStartPosition.Manual
            frm.Location = New System.Drawing.Point(monitores(1).Bounds.X + 100, monitores(1).Bounds.Y + 100)
        Else
            frm.StartPosition = FormStartPosition.CenterScreen
        End If

        ' 🔹 Inicia a aplicação no loop nativo de mensagens do Windows Forms
        ' Sem nenhum loop de Sleep(50), sem modalidade presa, permitindo ao usuário interagir livremente com o Solid Edge
        System.Windows.Forms.Application.Run(frm)
    End Sub

    Public Sub MostrarFormulario(ByVal formObj As Form)
        Try
            formObj.Show()
            formObj.BringToFront()
        Catch ex As Exception
        End Try
    End Sub


    Public Sub DadosDesenhoCorrente()


        Try
            ' 🔹 Conecta à sessão ativa do Solid Edge
            Dim app As SolidEdgeFramework.Application = Nothing
            app = Marshal.GetActiveObject("SolidEdge.Application")

            ' 🔹 Verifica se há documento aberto
            If app.Documents.Count = 0 Then
                MsgBox("Nenhum documento aberto no Solid Edge.", MsgBoxStyle.Exclamation)
                Exit Sub
            End If

            ' 🔹 Obtém documento ativo
            Dim docBase As Object = app.ActiveDocument
            Dim tipoDoc As String = docBase.Type.ToString()
            Dim nomeArquivo As String = IO.Path.GetFileName(docBase.FullName)
            Dim caminhoArquivo As String = docBase.FullName
            Dim material As String = ""
            Dim massa As Double = 0, volume As Double = 0, area As Double = 0

            ' ==============================================================
            ' 🧱 PEÇA SÓLIDA (.PAR)
            ' ==============================================================
            If TypeOf docBase Is SolidEdgePart.PartDocument Then
                Dim doc As SolidEdgePart.PartDocument = CType(docBase, SolidEdgePart.PartDocument)

                Try : material = doc.MaterialTableItem.Name : Catch : material = "Sem material" : End Try

                Try
                    Dim body = doc.Models.Item(1).Body
                    volume = body.Volume * 1000000
                    area = body.Area * 1000000
                    Dim props = doc.PhysicalProperties
                    props.Update()
                    massa = props.Mass
                Catch
                End Try

                ' ==============================================================
                ' 🧱 CHAPA METÁLICA (.PSM)
                ' ==============================================================
            ElseIf TypeOf docBase Is SolidEdgePart.SheetMetalDocument Then
                Dim doc As SolidEdgePart.SheetMetalDocument = CType(docBase, SolidEdgePart.SheetMetalDocument)

                Try : material = doc.MaterialTableItem.Name : Catch : material = "Sem material" : End Try

                Try
                    Dim body = doc.Models.Item(1).Body
                    volume = body.Volume * 1000000
                    area = body.Area * 1000000
                    Dim props = doc.PhysicalProperties
                    props.Update()
                    massa = props.Mass
                Catch
                End Try

            Else
                MsgBox("O documento ativo não é um arquivo de peça (.PAR) nem de chapa (.PSM).", MsgBoxStyle.Exclamation)
                Exit Sub
            End If

            ' ==============================================================
            ' 🧾 Exibe resultado
            ' ==============================================================
            Dim msg As String =
                $"📄 Arquivo: {nomeArquivo}" & vbCrLf &
                $"📂 Caminho: {caminhoArquivo}" & vbCrLf &
                $"🧱 Material: {material}" & vbCrLf &
                $"⚖️ Massa: {massa:F3} kg" & vbCrLf &
                $"📏 Volume: {volume:F2} mm³" & vbCrLf &
                $"📐 Área: {area:F2} mm²"

            MsgBox(msg, MsgBoxStyle.Information, "Dados do Arquivo Ativo")

        Catch ex As Exception
            MsgBox("Erro: " & ex.Message, MsgBoxStyle.Critical)
        End Try


    End Sub

    Public Sub DxfArquivoCorrente()
        Try
            ' 🔹 Conecta ao Solid Edge
            Dim seApp As SolidEdgeFramework.Application = Marshal.GetActiveObject("SolidEdge.Application")
            seApp.DisplayAlerts = False

            If seApp.Documents.Count = 0 Then
                MessageBox.Show("Nenhum documento aberto.", "SINCO-SolidEdge", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Exit Sub
            End If

            ' 🔹 Obtém o documento ativo
            Dim docBase As Object = seApp.ActiveDocument
            If Not TypeOf docBase Is SheetMetalDocument Then
                MessageBox.Show("O documento ativo não é um arquivo de chapa metálica (.PSM).", "SINCO-SolidEdge", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub
            End If

            Dim doc As SheetMetalDocument = CType(docBase, SheetMetalDocument)

            ' 🔹 Atualiza o modelo
            Try : doc.Update() : Catch : End Try

            ' ==============================================================
            ' 🔹 Garante que existe planificação (Flat Pattern)
            ' ==============================================================

            ' Se não existir, cria via comando
            If doc.FlatPatternModels.Count = 0 Then
                seApp.StartCommand(1494) ' seSheetMetalFlatPatternCreate

                ' Aguarda o Solid Edge processar a planificação
                Dim tentativas As Integer = 0
                While doc.FlatPatternModels.Count = 0 AndAlso tentativas < 100
                    Threading.Thread.Sleep(100)
                    tentativas += 1
                End While

                If doc.FlatPatternModels.Count = 0 Then
                    MessageBox.Show("Não foi possível criar a planificação. Verifique se há erro geométrico na peça.", "SINCO-SolidEdge", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Exit Sub
                End If
            End If

            ' 🔹 Acessa a planificação
            Dim flat As FlatPatternModel = doc.FlatPatternModels.Item(1)

            ' 🔹 Atualiza a planificação (garante dados corretos)
            Try : flat.UpdateFlatPattern() : Catch : End Try

            ' 🔹 Ativa a planificação
            flat.Activate()

            ' ==============================================================
            ' 🔹 Exporta DXF na mesma pasta do arquivo
            ' ==============================================================
            Dim pastaOrigem As String = Path.GetDirectoryName(doc.FullName)
            Dim nomeBase As String = Path.GetFileNameWithoutExtension(doc.FullName)
            Dim caminhoDXF As String = Path.Combine(pastaOrigem, nomeBase & "_BLANK.DXF")

            If File.Exists(caminhoDXF) Then File.Delete(caminhoDXF)

            ' Usa o comando interno oficial
            seApp.StartCommand(1496) ' seSheetMetalFlatPatternSaveAsDXF

            ' Aguarda o DXF aparecer
            Dim espera As Integer = 0
            While Not File.Exists(caminhoDXF) AndAlso espera < 100
                Threading.Thread.Sleep(100)
                espera += 1
            End While

            If File.Exists(caminhoDXF) Then
                MessageBox.Show($"✅ DXF exportado com sucesso!" & vbCrLf &
                                $"📁 {caminhoDXF}", "SINCO-SolidEdge", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                MessageBox.Show("⚠️ O DXF não foi encontrado. O comando foi executado, mas o Solid Edge pode não ter completado a exportação.", "SINCO-SolidEdge", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            End If

            seApp.DisplayAlerts = True

        Catch ex As Exception
            MessageBox.Show("Erro ao exportar DXF: " & ex.Message, "SINCO-SolidEdge", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

    End Sub

    Public Sub PlanificarDesenhoCorrente()
        Try
            ' 🔹 Conecta ao Solid Edge em execução
            Dim seApp As SolidEdgeFramework.Application =
                Marshal.GetActiveObject("SolidEdge.Application")

            ' 🔹 Verifica se há documento aberto
            If seApp.Documents.Count = 0 Then
                MessageBox.Show("Nenhum documento aberto no Solid Edge.", "SINCO", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Exit Sub
            End If

            ' 🔹 Obtém o documento ativo
            Dim doc As Object = seApp.ActiveDocument

            '' 🔹 Garante que é um arquivo de chapa (.psm)
            'If Not TypeOf doc Is SolidEdgePart.SheetMetalDocument Then
            '    MessageBox.Show("O documento ativo não é um arquivo de chapa metálica (.PSM).", "SINCO", MessageBoxButtons.OK, MessageBoxIcon.Error)
            '    Exit Sub
            'End If

            Dim sheetDoc As SheetMetalDocument = CType(doc, SheetMetalDocument)

            ' 🔹 Se não houver planificação, cria uma
            If sheetDoc.FlatPatternModels.Count = 0 Then
                seApp.StartCommand(1494) ' Cria planificação automaticamente
                Threading.Thread.Sleep(1500)
            End If

            ' 🔹 Ativa a planificação
            seApp.StartCommand(1493) ' Abre ambiente de planificação
            Threading.Thread.Sleep(1000)
            seApp.DoIdle()

            ' 🔹 Atualiza visualização
            seApp.ActiveWindow.View.Fit()

            ' 🔹 Define o caminho de exportação DXF
            Dim pasta As String = Path.GetDirectoryName(sheetDoc.FullName)
            Dim nomeBase As String = Path.GetFileNameWithoutExtension(sheetDoc.FullName)
            Dim caminhoDXF As String = Path.Combine(pasta, nomeBase & ".dxf")

            ' 🔹 Remove se já existir
            If File.Exists(caminhoDXF) Then File.Delete(caminhoDXF)

            ' ==========================================================
            ' 🔹 Aqui é o "Salvar Como DXF" nativo
            ' ==========================================================
            sheetDoc.SaveAs(caminhoDXF)

            '' 🔹 Confirmação
            'If File.Exists(caminhoDXF) Then
            '    'MessageBox.Show($"✅ DXF exportado com sucesso!" & vbCrLf &
            '    '                $"📁 {caminhoDXF}",
            '    '                "SINCO - Solid Edge",
            '    '                MessageBoxButtons.OK, MessageBoxIcon.Information)
            'Else
            '    MessageBox.Show("⚠️ O DXF não foi criado. Verifique permissões de pasta ou nome do arquivo.",
            '                    "SINCO - Solid Edge", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            'End If

        Catch ex As Exception
            'MessageBox.Show("Erro ao exportar DXF: " & ex.Message, "SINCO - Solid Edge",
            '                MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Public Sub ExportarPDFDetalhamento()
        Try
            ' 🔹 Conecta ao Solid Edge ativo
            Dim seApp As SolidEdgeFramework.Application =
                Marshal.GetActiveObject("SolidEdge.Application")

            ' 🔹 Verifica se há documento aberto
            If seApp.Documents.Count = 0 Then
                MessageBox.Show("Nenhum documento aberto no Solid Edge.", "SINCO - Solid Edge", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Exit Sub
            End If

            ' 🔹 Obtém o arquivo corrente (peça ou conjunto)
            Dim doc As Object = seApp.ActiveDocument
            Dim caminhoArquivo As String = doc.FullName
            Dim pasta As String = Path.GetDirectoryName(caminhoArquivo)
            Dim nomeBase As String = Path.GetFileNameWithoutExtension(caminhoArquivo)

            ' 🔹 Caminho esperado do detalhamento (.DFT)
            Dim caminhoDFT As String = Path.Combine(pasta, nomeBase & ".dft")

            If Not File.Exists(caminhoDFT) Then
                'MessageBox.Show($"❌ O detalhamento '{nomeBase}.dft' não foi encontrado na pasta." & vbCrLf &
                '                $"📁 {pasta}",
                '                "SINCO - Solid Edge",
                '                MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Exit Sub
            End If

            ' ==============================================================
            ' 🔹 Abre o detalhamento e exporta PDF
            ' ==============================================================
            Dim draftDoc As SolidEdgeDraft.DraftDocument =
                CType(seApp.Documents.Open(caminhoDFT), SolidEdgeDraft.DraftDocument)

            ' 🔹 Garante que o arquivo foi aberto
            If draftDoc Is Nothing Then
                'MessageBox.Show("Erro ao abrir o detalhamento.", "SINCO - Solid Edge",
                '                MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub
            End If



            ' 🔹 Caminho para salvar o PDF
            Dim caminhoPDF As String = Path.Combine(pasta, nomeBase & ".pdf")

            ' 🔹 Remove o PDF anterior se existir
            If File.Exists(caminhoPDF) Then File.Delete(caminhoPDF)

            ' 🔹 Exporta como PDF
            draftDoc.SaveAs(caminhoPDF)

            ' 🔹 Envia para a API para associação e IA
            SalvarAssocicaoPDFAPI(nomeBase, caminhoPDF)

            ' 🔹 Fecha o detalhamento (sem salvar alterações)
            draftDoc.Close(False)



            ' ==============================================================
            ' 🔹 Confirmação final
            ' ==============================================================
            'If File.Exists(caminhoPDF) Then
            '    'MessageBox.Show("✅ PDF do detalhamento gerado com sucesso!" & vbCrLf &
            '    '                $"📁 {caminhoPDF}",
            '    '                "SINCO - Solid Edge",
            '    '                MessageBoxButtons.OK, MessageBoxIcon.Information)
            'Else
            '    MessageBox.Show("⚠️ O detalhamento foi aberto, mas o PDF não foi criado.",
            '                    "SINCO - Solid Edge",
            '                    MessageBoxButtons.OK, MessageBoxIcon.Warning)
            'End If

        Catch ex As Exception
            'MessageBox.Show("Erro ao gerar PDF do detalhamento: " & ex.Message,
            '                "SINCO - Solid Edge",
            '                MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub





    '   Valor	Descrição	Comportamento
    '0   Automático	PDF salvo na mesma pasta do arquivo origem
    '1   Selecionar pasta (modo lote)	Abre uma vez o FolderBrowserDialog e usa a mesma pasta até o final do For
    '2   Selecionar pasta (modo individual)	Abre o diálogo toda vez, para cada arquivo exportado
    Private pastaDestinoPDFLote As String = ""

    Public Sub ExportarPDFDetalhamentoLote(Optional ByVal modoDestinoOuCaminho As Object = 0)
        Try
            Dim modoDestino As Integer = 0
            Dim caminhoForcado As String = ""

            If TypeOf modoDestinoOuCaminho Is String Then
                Dim strVal As String = CStr(modoDestinoOuCaminho)
                If strVal = "0" OrElse strVal = "1" OrElse strVal = "2" Then
                    modoDestino = CInt(strVal)
                Else
                    caminhoForcado = strVal ' Caminho explícito do PDF
                    modoDestino = -1
                End If
            Else
                Try
                    modoDestino = CInt(modoDestinoOuCaminho)
                Catch
                    modoDestino = 0
                End Try
            End If

            ' 🔹 Conecta ao Solid Edge ativo
            Dim seApp As SolidEdgeFramework.Application =
            Marshal.GetActiveObject("SolidEdge.Application")

            If seApp Is Nothing OrElse seApp.Documents.Count = 0 Then Exit Sub

            Dim doc As Object = seApp.ActiveDocument
            If doc Is Nothing Then Exit Sub

            Dim caminhoArquivo As String = doc.FullName
            Dim pastaOrigem As String = Path.GetDirectoryName(caminhoArquivo)
            Dim nomeBase As String = Path.GetFileNameWithoutExtension(caminhoArquivo)

            Dim caminhoDFT As String = Path.Combine(pastaOrigem, nomeBase & ".dft")
            If Not File.Exists(caminhoDFT) Then Exit Sub

            ' 🔹 Abre o detalhamento
            Dim draftDoc As SolidEdgeDraft.DraftDocument =
            CType(seApp.Documents.Open(caminhoDFT), SolidEdgeDraft.DraftDocument)

            If draftDoc Is Nothing Then Exit Sub

            Dim caminhoPDF As String = ""

            ' ============================================================
            ' 🧭 Escolha do destino conforme o modo
            ' ============================================================
            If modoDestino = -1 AndAlso caminhoForcado <> "" Then
                If caminhoForcado.ToUpper().EndsWith(".PDF") Then
                    caminhoPDF = caminhoForcado
                Else
                    caminhoPDF = Path.Combine(caminhoForcado, nomeBase & ".pdf")
                End If
            Else
                Select Case modoDestino
                Case 0
                    ' 📁 Modo automático → mesma pasta do arquivo
                    caminhoPDF = Path.Combine(pastaOrigem, nomeBase & ".pdf")

                Case 1
                    ' 📂 Modo lote → pergunta apenas na primeira vez
                    If String.IsNullOrWhiteSpace(pastaDestinoPDFLote) OrElse Not Directory.Exists(pastaDestinoPDFLote) Then
                        Using fbd As New FolderBrowserDialog()
                            fbd.Description = "Selecione a pasta de destino dos PDFs (modo lote):"
                            fbd.SelectedPath = pastaOrigem
                            If fbd.ShowDialog() <> DialogResult.OK Then
                                draftDoc.Close(False)
                                Exit Sub
                            End If
                            pastaDestinoPDFLote = fbd.SelectedPath
                        End Using
                    End If
                    caminhoPDF = Path.Combine(pastaDestinoPDFLote, nomeBase & ".pdf")

                Case 2
                    ' 📂 Modo individual → pergunta toda vez
                    Using fbd As New FolderBrowserDialog()
                        fbd.Description = "Selecione a pasta de destino do PDF:"
                        fbd.SelectedPath = pastaOrigem
                        If fbd.ShowDialog() <> DialogResult.OK Then
                            draftDoc.Close(False)
                            Exit Sub
                        End If
                        caminhoPDF = Path.Combine(fbd.SelectedPath, nomeBase & ".pdf")
                    End Using

                Case Else
                    caminhoPDF = Path.Combine(pastaOrigem, nomeBase & ".pdf")
                End Select
            End If

            ' ============================================================
            ' 💾 Exporta o PDF
            ' ============================================================
            If File.Exists(caminhoPDF) Then File.Delete(caminhoPDF)
            draftDoc.SaveAs(caminhoPDF)

            ' 🔹 Envia para a API para associação e IA
            SalvarAssocicaoPDFAPI(nomeBase, caminhoPDF)

            draftDoc.Close(False)

            ' ============================================================
            ' 🔹 Log ou debug
            ' ============================================================
            'If File.Exists(caminhoPDF) Then
            '    Debug.WriteLine($"✅ PDF exportado: {caminhoPDF}")
            'Else
            '    Debug.WriteLine($"⚠️ Falha ao exportar PDF: {nomeBase}")
            'End If

        Catch ex As Exception
            Debug.WriteLine($"❌ Erro ao exportar PDF: {ex.Message}")
        End Try
    End Sub





    ' 🔸 Variável global para armazenar a pasta de destino durante o modo 1 (lote)
    Private pastaDestinoDXFLote As String = ""



    Public Sub ExportarArquivosEmLote(dgv As DataGridView, chkDXF As System.Windows.Forms.CheckBox, chkPDF As System.Windows.Forms.CheckBox)
        Try
            Dim seApp As SolidEdgeFramework.Application = Marshal.GetActiveObject("SolidEdge.Application")

            If dgv.Rows.Count = 0 Then
                MessageBox.Show("Nenhum item na lista de peças.", "SINCO - Solid Edge",
                                MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Exit Sub
            End If

            Dim total As Integer = dgv.Rows.Count
            Dim exportados As Integer = 0

            For Each row As DataGridViewRow In dgv.Rows
                Try
                    Dim caminho As String = row.Cells("CaminhoCompleto").Value.ToString()
                    If Not File.Exists(caminho) Then Continue For

                    Dim tipo As String = Path.GetExtension(caminho).ToLower()
                    Dim nomeBase As String = Path.GetFileNameWithoutExtension(caminho)
                    Dim pasta As String = Path.GetDirectoryName(caminho)

                    ' =============================================================
                    ' 🔹 Exportar DXF — apenas para arquivos de chapa (.PSM)
                    ' =============================================================
                    If chkDXF.Checked AndAlso tipo = ".psm" Then
                        Try
                            Dim doc As SheetMetalDocument =
                                CType(seApp.Documents.Open(caminho), SheetMetalDocument)

                            ' Atualiza modelo
                            Try : doc.Update() : Catch : End Try

                            ' Garante planificação
                            If doc.FlatPatternModels.Count = 0 Then
                                seApp.StartCommand(1494) ' Cria planificação
                                Threading.Thread.Sleep(500)
                            End If

                            If doc.FlatPatternModels.Count > 0 Then
                                Dim flat As FlatPatternModel = doc.FlatPatternModels.Item(1)
                                Try : flat.UpdateFlatPattern() : Catch : End Try
                                flat.Activate()

                                Dim caminhoDXF As String = Path.Combine(pasta, nomeBase & "_BLANK.dxf")
                                If File.Exists(caminhoDXF) Then File.Delete(caminhoDXF)

                                seApp.StartCommand(1496) ' Salvar como DXF
                                Threading.Thread.Sleep(500)
                            End If

                            doc.Close(False)
                            exportados += 1

                        Catch ex As Exception
                            MessageBox.Show($"Erro ao exportar DXF de {nomeBase}: {ex.Message}",
                                            "SINCO - Solid Edge", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                        End Try
                    End If

                    ' =============================================================
                    ' 🔹 Exportar PDF — apenas se houver arquivo de detalhamento
                    ' =============================================================
                    If chkPDF.Checked Then
                        Try
                            Dim caminhoDFT As String = Path.Combine(pasta, nomeBase & ".dft")
                            Dim caminhoPDF As String = Path.Combine(pasta, nomeBase & ".pdf")

                            If File.Exists(caminhoDFT) Then
                                Dim draftDoc As SolidEdgeDraft.DraftDocument =
                                    CType(seApp.Documents.Open(caminhoDFT), SolidEdgeDraft.DraftDocument)

                                If File.Exists(caminhoPDF) Then File.Delete(caminhoPDF)
                                draftDoc.SaveAs(caminhoPDF)

                                ' 🔹 Envia para a API para associação e IA
                                SalvarAssocicaoPDFAPI(nomeBase, caminhoPDF)

                                draftDoc.Close(False)
                                exportados += 1
                            End If

                        Catch ex As Exception
                            MessageBox.Show($"Erro ao gerar PDF de {nomeBase}: {ex.Message}",
                                            "SINCO - Solid Edge", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                        End Try
                    End If

                Catch
                    ' Ignora erros por arquivo e segue para o próximo
                End Try
            Next

            MessageBox.Show($"✅ Exportação concluída!" & vbCrLf &
                            $"Arquivos processados: {total}" & vbCrLf &
                            $"Exportações bem-sucedidas: {exportados}",
                            "SINCO - Solid Edge",
                            MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception
            MessageBox.Show("Erro geral na exportação em lote: " & ex.Message,
                            "SINCO - Solid Edge",
                            MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub






    Public Sub AbriDetalhamentoDesenhoCorrente()


        Try
            ' 🔹 Conecta à sessão ativa do Solid Edge
            Dim app As SolidEdgeFramework.Application = Nothing
            app = Marshal.GetActiveObject("SolidEdge.Application")

            ' 🔹 Verifica se há documento aberto
            If app.Documents.Count = 0 Then
                MsgBox("Nenhum documento aberto no Solid Edge.", MsgBoxStyle.Exclamation)
                Exit Sub
            End If

            ' 🔹 Obtém documento ativo
            Dim docBase As Object = app.ActiveDocument

            Dim Endereco As String = docBase.FullName

            Endereco = Replace(Endereco, ".SLDPRT", ".dft")
            Endereco = Replace(Endereco, ".SLDASM", ".dft")
            Endereco = Replace(Endereco, ".sldprt", ".dft")
            Endereco = Replace(Endereco, ".sldasm", ".dft")
            Endereco = Replace(Endereco, ".asm", ".dft")
            Endereco = Replace(Endereco, ".ASM", ".dft")
            Endereco = Replace(Endereco, ".psm", ".dft")
            Endereco = Replace(Endereco, ".PSM", ".dft")
            Endereco = Replace(Endereco, ".par", ".dft")
            Endereco = Replace(Endereco, ".PAR", ".dft")

            If File.Exists(Endereco) Then
                Process.Start(Endereco)

            Else
                MessageBox.Show("Arquivo não encontrado: " & Endereco, "SINCO - Solid Edge",
                            MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            End If


        Catch ex As Exception
            MsgBox("Erro: " & ex.Message, MsgBoxStyle.Critical)
        End Try


    End Sub


    Public Sub PlanificarDesenhoCorrenteLote(Optional modoDestino As Integer = 0)
        Try
            ' 🔹 Conecta ao Solid Edge em execução
            Dim seApp As SolidEdgeFramework.Application =
            Marshal.GetActiveObject("SolidEdge.Application")

            ' 🔹 Verifica se há documento aberto
            If seApp Is Nothing OrElse seApp.Documents.Count = 0 Then
                MessageBox.Show("Nenhum documento aberto no Solid Edge.", "SINCO", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Exit Sub
            End If

            ' 🔹 Obtém o documento ativo
            Dim doc As Object = seApp.ActiveDocument
            If doc Is Nothing Then Exit Sub

            ' 🔹 Verifica se é um documento de chapa metálica (.PSM)
            If Not TypeOf doc Is SolidEdgePart.SheetMetalDocument Then
                Debug.WriteLine("❌ Documento ativo não é do tipo SheetMetal (.PSM).")
                Exit Sub
            End If

            Dim sheetDoc As SheetMetalDocument = CType(doc, SheetMetalDocument)

            ' ============================================================
            ' 🧩 Cria planificação, se não existir
            ' ============================================================
            If sheetDoc.FlatPatternModels.Count = 0 Then
                seApp.StartCommand(1494) ' Criar planificação
                Threading.Thread.Sleep(1500)
            End If

            ' ============================================================
            ' 🔹 Ativa a planificação
            ' ============================================================
            seApp.StartCommand(1493) ' Alternar para ambiente de planificação
            Threading.Thread.Sleep(1000)
            seApp.DoIdle()
            seApp.ActiveWindow.View.Fit()

            ' ============================================================
            ' 📂 Define caminho base
            ' ============================================================
            Dim pastaOrigem As String = Path.GetDirectoryName(sheetDoc.FullName)
            Dim nomeBase As String = Path.GetFileNameWithoutExtension(sheetDoc.FullName)
            Dim caminhoDXF As String = ""

            ' ============================================================
            ' 🧭 Escolha do destino conforme modoDestino
            ' ============================================================
            Select Case modoDestino
                Case 0
                    ' 📁 Modo automático → mesma pasta
                    caminhoDXF = Path.Combine(pastaOrigem, nomeBase & ".dxf")

                Case 1
                    ' 📂 Modo lote → pergunta apenas uma vez
                    If String.IsNullOrWhiteSpace(pastaDestinoDXFLote) OrElse Not Directory.Exists(pastaDestinoDXFLote) Then
                        Using fbd As New FolderBrowserDialog()
                            fbd.Description = "Selecione a pasta de destino dos arquivos DXF (modo lote):"
                            fbd.SelectedPath = pastaOrigem
                            If fbd.ShowDialog() <> DialogResult.OK Then Exit Sub
                            pastaDestinoDXFLote = fbd.SelectedPath
                        End Using
                    End If
                    caminhoDXF = Path.Combine(pastaDestinoDXFLote, nomeBase & ".dxf")

                Case 2
                    ' 📂 Modo individual → pergunta toda vez
                    Using fbd As New FolderBrowserDialog()
                        fbd.Description = "Selecione a pasta de destino do arquivo DXF:"
                        fbd.SelectedPath = pastaOrigem
                        If fbd.ShowDialog() <> DialogResult.OK Then Exit Sub
                        caminhoDXF = Path.Combine(fbd.SelectedPath, nomeBase & ".dxf")
                    End Using

                Case Else
                    caminhoDXF = Path.Combine(pastaOrigem, nomeBase & ".dxf")
            End Select

            ' ============================================================
            ' 💾 Remove DXF existente e exporta novo
            ' ============================================================
            If File.Exists(caminhoDXF) Then File.Delete(caminhoDXF)
            sheetDoc.SaveAs(caminhoDXF)

            ' ============================================================
            ' ✅ Confirmação / Log
            ' ============================================================
            If File.Exists(caminhoDXF) Then
                Debug.WriteLine($"✅ DXF exportado: {caminhoDXF}")
            Else
                Debug.WriteLine($"⚠️ Falha ao gerar DXF: {nomeBase}")
            End If

        Catch ex As Exception
            Debug.WriteLine($"❌ Erro ao exportar DXF: {ex.Message}")
        End Try
    End Sub

    Public Sub ExportarDXF_SE2025()
        Try
            ' 🔹 Obtém a instância ativa do Solid Edge
            Dim seApp As SolidEdgeFramework.Application =
            Marshal.GetActiveObject("SolidEdge.Application")

            If seApp Is Nothing OrElse seApp.Documents.Count = 0 Then
                MessageBox.Show("Nenhum documento aberto no Solid Edge.",
                            "SINCO - Solid Edge", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Exit Sub
            End If

            ' 🔹 Obtém o documento ativo
            Dim docBase As Object = seApp.ActiveDocument

            If Not TypeOf docBase Is SolidEdgePart.SheetMetalDocument Then
                MessageBox.Show("O documento ativo não é um arquivo de chapa metálica (.PSM).",
                            "SINCO - Solid Edge", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub
            End If

            Dim doc As SolidEdgePart.SheetMetalDocument = CType(docBase, SolidEdgePart.SheetMetalDocument)

            ' 🔹 Atualiza o modelo antes de exportar
            Try : doc.Update() : Catch : End Try
            seApp.DoIdle()

            ' 🔹 Garante que existe planificação
            If doc.Models.Count = 0 Then
                MessageBox.Show("A peça não contém nenhum modelo 3D válido.", "SINCO - Solid Edge", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub
            End If

            ' 🔹 Cria a planificação se necessário
            If doc.FlatPatternModels.Count = 0 Then
                seApp.StartCommand(1494) ' Cria Flat Pattern (seSheetMetalFlatPatternCreate)
                Thread.Sleep(1000)
                seApp.DoIdle()
            End If

            ' 🔹 Define o caminho de exportação
            Dim pastaOrigem As String = Path.GetDirectoryName(doc.FullName)
            Dim nomeBase As String = Path.GetFileNameWithoutExtension(doc.FullName)
            Dim caminhoDXF As String = Path.Combine(pastaOrigem, nomeBase & "_BLANK.DXF")

            If File.Exists(caminhoDXF) Then
                Try : File.Delete(caminhoDXF) : Catch : End Try
            End If

            ' 🔹 Força o comando nativo do Solid Edge
            seApp.StartCommand(1496) ' seSheetMetalFlatPatternSaveAsDXF

            ' 🔹 Aguarda o DXF ser gerado
            Dim tentativas As Integer = 0
            Do While Not File.Exists(caminhoDXF) AndAlso tentativas < 60
                Thread.Sleep(500)
                tentativas += 1
            Loop

            ' 🔹 Confirmação
            If File.Exists(caminhoDXF) Then
                MessageBox.Show("✅ DXF exportado com sucesso!" & vbCrLf &
                            "📁 " & caminhoDXF,
                            "SINCO - Solid Edge", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                MessageBox.Show("⚠️ O DXF não foi gerado. Tente abrir o modelo e criar a planificação manualmente uma vez.",
                            "SINCO - Solid Edge", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            End If

        Catch ex As Exception
            MessageBox.Show("Erro ao exportar DXF: " & ex.Message,
                        "SINCO - Solid Edge", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Public Sub ExportarPlanificacaoDXF_Final(Optional modoDestino As Integer = 0)
        Dim seApp As SolidEdgeFramework.Application = Nothing
        Dim sheetDoc As SheetMetalDocument = Nothing
        Dim caminhoDXF As String = ""

        Try
            ' ===============================================================
            ' 🔹 Conecta ao Solid Edge
            ' ===============================================================
            Try
                seApp = Marshal.GetActiveObject("SolidEdge.Application")
            Catch
                MsgBox("Solid Edge não está em execução.", vbCritical, "SINCO - Solid Edge")
                Exit Sub
            End Try

            If seApp.Documents.Count = 0 Then
                MsgBox("Nenhum documento aberto.", vbExclamation, "SINCO - Solid Edge")
                Exit Sub
            End If

            ' ===============================================================
            ' 🔹 Obtém o documento ativo e valida tipo
            ' ===============================================================
            Dim doc As Object = seApp.ActiveDocument
            If Not TypeOf doc Is SheetMetalDocument Then
                MsgBox("O documento ativo não é um arquivo de chapa metálica (.PSM).", vbExclamation, "SINCO - Solid Edge")
                Exit Sub
            End If

            sheetDoc = DirectCast(doc, SheetMetalDocument)
            seApp.DoIdle()

            ' ===============================================================
            ' 🔹 Cria a planificação, se não existir
            ' ===============================================================
            If sheetDoc.FlatPatternModels.Count = 0 Then
                seApp.StartCommand(1494) ' Criar planificação automaticamente
                Threading.Thread.Sleep(1500)
                seApp.DoIdle()
            End If

            ' 🔹 Atualiza a planificação
            Try
                Dim flat = sheetDoc.FlatPatternModels.Item(1)
                Try : CallByName(flat, "Update", CallType.Method) : Catch : End Try
            Catch
            End Try

            ' ===============================================================
            ' 🔹 Escolhe face base automaticamente (maior face plana)
            ' ===============================================================
            Dim maiorFace As Object = Nothing
            Dim maiorArea As Double = 0

            Try
                ' Obtém o primeiro corpo sólido do modelo
                Dim model = sheetDoc.Models.Item(1)
                Dim bodies = model.Body

                ' Algumas versões retornam uma coleção, outras um único corpo
                If TypeOf bodies Is SolidEdgeGeometry.Body Then
                    For Each face As SolidEdgeGeometry.Face In bodies.Faces(SolidEdgeGeometry.FeatureTopologyQueryTypeConstants.igQueryAll)
                        Try
                            Dim area As Double = face.Area
                            If area > maiorArea Then
                                maiorArea = area
                                maiorFace = face
                            End If
                        Catch
                        End Try
                    Next
                ElseIf TypeOf bodies Is IEnumerable Then
                    For Each body As SolidEdgeGeometry.Body In bodies
                        For Each face As SolidEdgeGeometry.Face In body.Faces(SolidEdgeGeometry.FeatureTopologyQueryTypeConstants.igQueryAll)
                            Try
                                Dim area As Double = face.Area
                                If area > maiorArea Then
                                    maiorArea = area
                                    maiorFace = face
                                End If
                            Catch
                            End Try
                        Next
                    Next
                End If

                If maiorFace Is Nothing Then
                    Throw New Exception("Nenhuma face plana encontrada.")
                End If

            Catch ex As Exception
                MsgBox("Falha ao encontrar face base: " & ex.Message, vbCritical, "SINCO - SolidEdeg 1.0")
            End Try


            ' ===============================================================
            ' 🔹 Define caminho do DXF
            ' ===============================================================
            Dim pasta As String = Path.GetDirectoryName(sheetDoc.FullName)
            Dim nomeBase As String = Path.GetFileNameWithoutExtension(sheetDoc.FullName)
            caminhoDXF = Path.Combine(pasta, nomeBase & "_PLANIFICACAO.DXF")

            If File.Exists(caminhoDXF) Then File.Delete(caminhoDXF)

            ' ===============================================================
            ' 🔹 Tenta usar método nativo “SaveFlatAsDXF”
            ' ===============================================================
            Dim exportou As Boolean = False

            Try
                Dim flat = sheetDoc.FlatPatternModels.Item(1)

                ' Alguns builds do SE expõem o método SaveFlatAsDXF com parâmetros (Face, Caminho)
                flat.GetType().InvokeMember(
                    "SaveFlatAsDXF",
                    Reflection.BindingFlags.InvokeMethod,
                    Nothing,
                    flat,
                    New Object() {maiorFace, caminhoDXF}
                )

                exportou = File.Exists(caminhoDXF)
            Catch ex As Exception
                Debug.WriteLine("Método SaveFlatAsDXF indisponível: " & ex.Message)
            End Try

            ' ===============================================================
            ' 🔹 Fallback - usa SaveAsTranslated se método acima falhar
            ' ===============================================================
            If Not exportou Then
                Try
                    sheetDoc.SaveAsTranslated(caminhoDXF)
                    exportou = File.Exists(caminhoDXF)
                Catch ex As Exception
                    Debug.WriteLine("Fallback SaveAsTranslated falhou: " & ex.Message)
                End Try
            End If

            ' ===============================================================
            ' 🔹 Resultado
            ' ===============================================================
            If exportou Then
                MsgBox("✅ Planificação DXF criada com sucesso!" & vbCrLf & caminhoDXF,
                       vbInformation, "SINCO - Solid Edge")
            Else
                MsgBox("⚠️ Não foi possível gerar o DXF planificado." & vbCrLf &
                       "Verifique se o modelo possui uma planificação válida.",
                       vbExclamation, "SINCO - Solid Edge")
            End If

        Catch ex As Exception
            MsgBox("❌ Erro ao gerar planificação: " & ex.Message, vbCritical, "SINCO - Solid Edge")
        End Try
    End Sub

    'Public Sub ExportarDXFPlanificadoAuto(Optional modoDestino As Integer = 0)
    '    Dim app As SolidEdgeFramework.Application = Nothing
    '    app = Marshal.GetActiveObject("SolidEdge.Application")


    '    Dim doc As Object = Nothing
    '    Dim sheetDoc As SheetMetalDocument = Nothing

    '    Try

    '        ' 🔹 Verifica se há documento aberto
    '        If app.Documents.Count = 0 Then
    '            MsgBox("Nenhum documento aberto no Solid Edge.", MsgBoxStyle.Exclamation)
    '            Exit Sub
    '        End If




    '        doc = app.ActiveDocument
    '        sheetDoc = TryCast(doc, SheetMetalDocument)
    '        If sheetDoc Is Nothing Then
    '            '''  MessageBox.Show("O documento ativo não é .PSM (chapa).", "SINCO - Solid Edge", MessageBoxButtons.OK, MessageBoxIcon.Warning)
    '            Exit Sub
    '        End If

    '        ' Dá uma atualizada geral
    '        Try : sheetDoc.Update() : Catch : End Try
    '        app.DoIdle()

    '        Dim models As Models = sheetDoc.Models
    '        If models Is Nothing OrElse models.Count = 0 Then
    '            Throw New Exception("Nenhum modelo encontrado no .PSM.")
    '        End If

    '        Dim model As SolidEdgePart.Model = models.Item(1)
    '        Dim useFlatPattern As Boolean = False

    '        ' Rota 1: usar FlatPattern se existir e estiver OK
    '        Dim flatModels As FlatPatternModels = sheetDoc.FlatPatternModels
    '        If flatModels IsNot Nothing AndAlso flatModels.Count > 0 Then
    '            For i = 1 To flatModels.Count
    '                Dim fp = flatModels.Item(i)
    '                Dim ok As Boolean = False
    '                Try
    '                    ok = fp.IsUpToDate
    '                Catch
    '                    ok = False
    '                End Try
    '                If ok Then
    '                    useFlatPattern = True
    '                    Exit For
    '                End If
    '            Next
    '        End If

    '        ' Caminho de saída
    '        ' Dim pasta = Path.GetDirectoryName(sheetDoc.FullName)
    '        ' Dim nome = Path.GetFileNameWithoutExtension(sheetDoc.FullName)
    '        Dim pastaOrigem As String = Path.GetDirectoryName(sheetDoc.FullName)
    '        Dim nomeBase As String = Path.GetFileNameWithoutExtension(sheetDoc.FullName)

    '        Dim caminhoDXF = Path.Combine(pastaOrigem, nomeBase & ".DXF")


    '        ' ============================================================
    '        ' 🧭 Escolha do destino conforme modoDestino
    '        ' ============================================================
    '        Select Case modoDestino
    '            Case 0
    '                ' 📁 Modo automático → mesma pasta
    '                caminhoDXF = Path.Combine(pastaOrigem, nomeBase & ".dxf")

    '            Case 1
    '                ' 📂 Modo lote → pergunta apenas uma vez
    '                If String.IsNullOrWhiteSpace(pastaDestinoDXFLote) OrElse Not Directory.Exists(pastaDestinoDXFLote) Then
    '                    Using fbd As New FolderBrowserDialog()
    '                        fbd.Description = "Selecione a pasta de destino dos arquivos DXF (modo lote):"
    '                        fbd.SelectedPath = pastaOrigem
    '                        If fbd.ShowDialog() <> DialogResult.OK Then Exit Sub
    '                        pastaDestinoDXFLote = fbd.SelectedPath
    '                    End Using
    '                End If
    '                caminhoDXF = Path.Combine(pastaDestinoDXFLote, nomeBase & ".dxf")

    '            Case 2
    '                ' 📂 Modo individual → pergunta toda vez
    '                Using fbd As New FolderBrowserDialog()
    '                    fbd.Description = "Selecione a pasta de destino do arquivo DXF:"
    '                    fbd.SelectedPath = pastaOrigem
    '                    If fbd.ShowDialog() <> DialogResult.OK Then Exit Sub
    '                    caminhoDXF = Path.Combine(fbd.SelectedPath, nomeBase & ".dxf")
    '                End Using

    '            Case Else
    '                caminhoDXF = Path.Combine(pastaOrigem, nomeBase & ".dxf")
    '        End Select


    '        ' Limpa arquivo anterior
    '        Try
    '            If File.Exists(caminhoDXF) Then
    '                File.Delete(caminhoDXF)
    '                'File.Delete(caminhoDXF.Replace(".dxf", ".dft"))
    '                'File.Delete(caminhoDXF.Replace(".dxf", ".lds"))
    '                'File.Delete(caminhoDXF.Replace(".dxf", ".dft"))

    '            End If






    '        Catch ex As Exception
    '        Finally
    '        End Try


    '        ' Se NÃO vamos usar FlatPattern, precisamos de face/aresta consistentes
    '        Dim faceRef As Face = Nothing
    '        Dim edgeRef As Edge = Nothing
    '        Dim vertexRef As Vertex = Nothing

    '        If Not useFlatPattern Then
    '            SelecionarFaceEArestaConfiaveis(model, faceRef, edgeRef)
    '            If faceRef Is Nothing OrElse edgeRef Is Nothing Then
    '                Throw New Exception("Falha ao selecionar face/aresta para planificação (useFlatPattern=False).")
    '            End If
    '        End If

    '        ' Exporta
    '        Try
    '            If useFlatPattern Then
    '                ' Quando usa Flat Pattern, ainda é boa prática passar face/edge válidas
    '                Dim face2 As Face = Nothing, edge2 As Edge = Nothing
    '                SelecionarFaceEArestaConfiaveis(model, face2, edge2)
    '                models.SaveAsFlatDXFEx(caminhoDXF, face2, edge2, Nothing, True)
    '            Else
    '                models.SaveAsFlatDXFEx(caminhoDXF, faceRef, edgeRef, vertexRef, False)
    '            End If
    '        Catch ex As Exception
    '            Throw New Exception("Falha no SaveAsFlatDXFEx(): " & ex.Message)
    '        End Try

    '        ' Aguarda o arquivo aparecer
    '        Dim tries = 0
    '        Do While Not File.Exists(caminhoDXF) AndAlso tries < 10
    '            Thread.Sleep(300)
    '            app.DoIdle()
    '            tries += 1
    '        Loop

    '        'If File.Exists(caminhoDXF) Then
    '        '    MessageBox.Show("✅ DXF planificado gerado:" & vbCrLf & caminhoDXF, "SINCO - Solid Edge", MessageBoxButtons.OK, MessageBoxIcon.Information)
    '        'Else
    '        '    MessageBox.Show("⚠️ O DXF não foi criado (ou veio vazio). Verifique se há dobras válidas e espessura definida.", "SINCO - Solid Edge", MessageBoxButtons.OK, MessageBoxIcon.Warning)
    '        'End If

    '    Catch ex As Exception
    '    Finally
    '        '   MessageBox.Show("❌ Erro: " & ex.Message, "SINCO - Solid Edge", MessageBoxButtons.OK, MessageBoxIcon.Error)
    '    End Try
    'End Sub


    Public Sub ExportarDXFPlanificadoAuto(Optional ByVal modoDestinoOuCaminho As Object = 0)
        Dim app As SolidEdgeFramework.Application = Nothing
        Dim modoDestino As Integer = 0
        Dim caminhoForcado As String = ""

        If TypeOf modoDestinoOuCaminho Is String Then
            Dim strVal As String = CStr(modoDestinoOuCaminho)
            If strVal = "0" OrElse strVal = "1" OrElse strVal = "2" Then
                modoDestino = CInt(strVal)
            Else
                caminhoForcado = strVal ' Caminho explícito do DXF
                modoDestino = -1
            End If
        Else
            Try
                modoDestino = CInt(modoDestinoOuCaminho)
            Catch
                modoDestino = 0
            End Try
        End If

        Try
            app = Marshal.GetActiveObject("SolidEdge.Application")
        Catch
            Exit Sub
        End Try

        Dim doc As Object = Nothing
        Dim sheetDoc As SheetMetalDocument = Nothing

        Try
            If app.Documents.Count = 0 Then Exit Sub

            doc = app.ActiveDocument
            sheetDoc = TryCast(doc, SheetMetalDocument)
            If sheetDoc Is Nothing Then Exit Sub

            Try : sheetDoc.Update() : Catch : End Try
            app.DoIdle()

            Dim models As Models = sheetDoc.Models
            If models Is Nothing OrElse models.Count = 0 Then
                Throw New Exception("Nenhum modelo encontrado no .PSM.")
            End If

            Dim model As SolidEdgePart.Model = models.Item(1)
            Dim useFlatPattern As Boolean = False

            Dim flatModels As FlatPatternModels = sheetDoc.FlatPatternModels
            If flatModels IsNot Nothing AndAlso flatModels.Count > 0 Then
                For i = 1 To flatModels.Count
                    Dim fp = flatModels.Item(i)
                    Dim ok As Boolean = False
                    Try : ok = fp.IsUpToDate : Catch : ok = False : End Try
                    If ok Then
                        useFlatPattern = True
                        Exit For
                    End If
                Next
            End If

            Dim pastaOrigem As String = Path.GetDirectoryName(sheetDoc.FullName)
            Dim nomeBase As String = Path.GetFileNameWithoutExtension(sheetDoc.FullName)
            Dim caminhoDXF As String = ""

            ' ============================================================
            ' 🧭 Escolha do destino conforme modoDestino
            ' ============================================================
            If modoDestino = -1 AndAlso caminhoForcado <> "" Then
                If caminhoForcado.ToUpper().EndsWith(".DXF") Then
                    caminhoDXF = caminhoForcado
                Else
                    caminhoDXF = Path.Combine(caminhoForcado, nomeBase & ".dxf")
                End If
            Else
                Select Case modoDestino
                    Case 0
                        ' 📁 Modo automático → mesma pasta do arquivo
                        caminhoDXF = Path.Combine(pastaOrigem, nomeBase & ".dxf")
                    Case 1
                        ' 📂 Modo lote → pergunta apenas uma vez
                        If String.IsNullOrWhiteSpace(pastaDestinoDXFLote) OrElse Not Directory.Exists(pastaDestinoDXFLote) Then
                            Using fbd As New FolderBrowserDialog()
                                fbd.Description = "Selecione a pasta de destino dos arquivos DXF (modo lote):"
                                fbd.SelectedPath = pastaOrigem
                                If fbd.ShowDialog() <> DialogResult.OK Then Exit Sub
                                pastaDestinoDXFLote = fbd.SelectedPath
                            End Using
                        End If
                        caminhoDXF = Path.Combine(pastaDestinoDXFLote, nomeBase & ".dxf")
                    Case 2
                        ' 📂 Modo individual → pergunta toda vez
                        Using fbd As New FolderBrowserDialog()
                            fbd.Description = "Selecione a pasta de destino do arquivo DXF:"
                            fbd.SelectedPath = pastaOrigem
                            If fbd.ShowDialog() <> DialogResult.OK Then Exit Sub
                            caminhoDXF = Path.Combine(fbd.SelectedPath, nomeBase & ".dxf")
                        End Using
                    Case Else
                        caminhoDXF = Path.Combine(pastaOrigem, nomeBase & ".dxf")
                End Select
            End If

            ' Limpa arquivo anterior se existir
            Try
                If File.Exists(caminhoDXF) Then File.Delete(caminhoDXF)
            Catch
            End Try

            Dim faceRef As Face = Nothing
            Dim edgeRef As Edge = Nothing
            Dim vertexRef As Vertex = Nothing

            If Not useFlatPattern Then
                SelecionarFaceEArestaConfiaveis(model, faceRef, edgeRef)
                If faceRef Is Nothing OrElse edgeRef Is Nothing Then
                    Throw New Exception("Falha ao selecionar face/aresta.")
                End If
            End If

            ' Exporta
            Try
                If useFlatPattern Then
                    Dim face2 As Face = Nothing, edge2 As Edge = Nothing
                    SelecionarFaceEArestaConfiaveis(model, face2, edge2)
                    models.SaveAsFlatDXFEx(caminhoDXF, face2, edge2, Nothing, True)
                Else
                    models.SaveAsFlatDXFEx(caminhoDXF, faceRef, edgeRef, vertexRef, False)
                End If
            Catch ex As Exception
                Throw New Exception("Falha no SaveAsFlatDXFEx(): " & ex.Message)
            End Try

        Catch ex As Exception
            ' MessageBox.Show("❌ Erro: " & ex.Message, "SINCO - Solid Edge", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Public Sub ExportarIGESAuto(Optional ByVal modoDestinoOuCaminho As Object = 0)
        Dim app As SolidEdgeFramework.Application = Nothing
        Dim modoDestino As Integer = 0
        Dim caminhoForcado As String = ""

        If TypeOf modoDestinoOuCaminho Is String Then
            Dim strVal As String = CStr(modoDestinoOuCaminho)
            If strVal = "0" OrElse strVal = "1" OrElse strVal = "2" Then
                modoDestino = CInt(strVal)
            Else
                caminhoForcado = strVal ' Caminho explícito do IGES
                modoDestino = -1
            End If
        Else
            Try
                modoDestino = CInt(modoDestinoOuCaminho)
            Catch
                modoDestino = 0
            End Try
        End If

        Try
            ' Tenta pegar a sessão do Solid Edge já aberta
            app = CType(Marshal.GetActiveObject("SolidEdge.Application"), SolidEdgeFramework.Application)
        Catch ex As Exception
            MsgBox("Solid Edge não está aberto.", MsgBoxStyle.Exclamation, "SINCO - Solid Edge")
            Exit Sub
        End Try

        Dim doc As Object = Nothing
        Dim partDoc As SolidEdgePart.PartDocument = Nothing
        Dim sheetDoc As SolidEdgePart.SheetMetalDocument = Nothing
        Dim doc3D As Object = Nothing

        Try
            ' 🔹 Verifica se há documento aberto
            If app.Documents.Count = 0 Then
                MsgBox("Nenhum documento aberto no Solid Edge.", MsgBoxStyle.Exclamation, "SINCO - Solid Edge")
                Exit Sub
            End If

            doc = app.ActiveDocument

            ' Aceita apenas PAR ou PSM
            partDoc = TryCast(doc, SolidEdgePart.PartDocument)
            sheetDoc = TryCast(doc, SolidEdgePart.SheetMetalDocument)

            If partDoc Is Nothing AndAlso sheetDoc Is Nothing Then
                MsgBox("O documento ativo não é .PAR nem .PSM (peça/chapas).", MsgBoxStyle.Exclamation, "SINCO - Solid Edge")
                Exit Sub
            End If

            If partDoc IsNot Nothing Then
                doc3D = partDoc
            Else
                doc3D = sheetDoc
            End If

            ' Atualiza o modelo antes de exportar (boa prática)
            Try : doc3D.Update() : Catch : End Try
            app.DoIdle()

            ' Caminho de origem
            Dim pastaOrigem As String = Path.GetDirectoryName(CStr(doc3D.FullName))
            Dim nomeBase As String = Path.GetFileNameWithoutExtension(CStr(doc3D.FullName))

            ' Caminho de saída IGES
            Dim caminhoIGES As String = Path.Combine(pastaOrigem, nomeBase & ".iges")

            ' ============================================================
            ' 🧭 Escolha do destino conforme modoDestino
            '    (mesma lógica da DXF, reutilizando pastaDestinoDXFLote)
            ' ============================================================
            If modoDestino = -1 AndAlso caminhoForcado <> "" Then
                If caminhoForcado.ToUpper().EndsWith(".IGES") OrElse caminhoForcado.ToUpper().EndsWith(".IGS") Then
                    caminhoIGES = caminhoForcado
                Else
                    caminhoIGES = Path.Combine(caminhoForcado, nomeBase & ".iges")
                End If
            Else
                Select Case modoDestino
                Case 0
                    ' 📁 Modo automático → mesma pasta do arquivo
                    caminhoIGES = Path.Combine(pastaOrigem, nomeBase & ".iges")

                Case 1
                    ' 📂 Modo lote → pergunta apenas uma vez
                    If String.IsNullOrWhiteSpace(pastaDestinoDXFLote) OrElse Not Directory.Exists(pastaDestinoDXFLote) Then
                        Using fbd As New FolderBrowserDialog()
                            fbd.Description = "Selecione a pasta de destino dos arquivos IGES (modo lote):"
                            fbd.SelectedPath = pastaOrigem
                            If fbd.ShowDialog() <> DialogResult.OK Then Exit Sub
                            pastaDestinoDXFLote = fbd.SelectedPath
                        End Using
                    End If
                    caminhoIGES = Path.Combine(pastaDestinoDXFLote, nomeBase & ".iges")

                Case 2
                    ' 📂 Modo individual → pergunta toda vez
                    Using fbd As New FolderBrowserDialog()
                        fbd.Description = "Selecione a pasta de destino do arquivo IGES:"
                        fbd.SelectedPath = pastaOrigem
                        If fbd.ShowDialog() <> DialogResult.OK Then Exit Sub
                        caminhoIGES = Path.Combine(fbd.SelectedPath, nomeBase & ".iges")
                    End Using

                Case Else
                    caminhoIGES = Path.Combine(pastaOrigem, nomeBase & ".igs")
                End Select
            End If

            ' Remove IGES anterior, se existir
            Try
                If File.Exists(caminhoIGES) Then
                    File.Delete(caminhoIGES)
                End If
            Catch
                ' Se não conseguir apagar, tenta sobrescrever assim mesmo
            End Try

            ' ============================================================
            ' 💾 Exporta para IGES (SaveCopyAs)
            ' ============================================================
            Try
                ' SaveCopyAs respeita o formato pela extensão (.igs)
                doc3D.SaveCopyAs(caminhoIGES)
            Catch ex As Exception
                MessageBox.Show("Falha ao exportar IGES (SaveCopyAs): " & ex.Message,
                            "SINCO - Solid Edge",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error)
                Exit Sub
            End Try

            app.DoIdle()

            ' Aguarda o arquivo aparecer
            Dim tries As Integer = 0
            Do While Not File.Exists(caminhoIGES) AndAlso tries < 10
                Threading.Thread.Sleep(300)
                app.DoIdle()
                tries += 1
            Loop

            'If File.Exists(caminhoIGES) Then
            '    MessageBox.Show("✅ IGES gerado com sucesso:" & vbCrLf & caminhoIGES,
            '                "SINCO - Solid Edge",
            '                MessageBoxButtons.OK,
            '                MessageBoxIcon.Information)
            'Else
            '    MessageBox.Show("⚠️ O arquivo IGES não foi criado. Verifique o modelo ou as opções de exportação.",
            '                "SINCO - Solid Edge",
            '                MessageBoxButtons.OK,
            '                MessageBoxIcon.Warning)
            'End If

        Catch ex As Exception
            MessageBox.Show("❌ Erro ao exportar IGES: " & ex.Message,
                        "SINCO - Solid Edge",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error)
        End Try

    End Sub


    ' ---------------------------------------------------------------
    ' Escolhe a MAIOR face PLANA do corpo + a ARESTA retilínea mais longa dessa face
    ' ---------------------------------------------------------------
    Private Sub SelecionarFaceEArestaConfiaveis(model As SolidEdgePart.Model, ByRef faceOut As Face, ByRef edgeOut As Edge)
        faceOut = Nothing : edgeOut = Nothing
        Try
            Dim body As Body = CType(model.Body, Body)
            Dim faces As Faces = CType(body.Faces(FeatureTopologyQueryTypeConstants.igQueryAll), Faces)
            If faces Is Nothing OrElse faces.Count = 0 Then Exit Sub

            Dim melhorArea As Double = -1.0
            Dim melhorFace As Face = Nothing

            ' 1) maior face PLANA
            For i = 1 To faces.Count
                Dim f As Face = CType(faces.Item(i), Face)
                Dim ehPlano As Boolean = False
                Try
                    ' Algumas versões expõem .SurfaceType, outras têm .GeometryType
                    Dim tipo As Integer = -1
                    Try
                        tipo = CInt(CallByName(f, "SurfaceType", CallType.Get))
                    Catch
                        Try : tipo = CInt(CallByName(f, "GeometryType", CallType.Get)) : Catch : End Try
                    End Try
                    ' igPlane = 2 geralmente
                    ehPlano = (tipo = 2)
                Catch
                    ehPlano = False
                End Try
                If Not ehPlano Then Continue For

                ' Área da face (nem todas expõem .Area; use Try)
                Dim area As Double = 0
                Try
                    area = CDbl(CallByName(f, "Area", CallType.Get))
                Catch
                    area = 0
                End Try

                If area > melhorArea Then
                    melhorArea = area
                    melhorFace = f
                End If
            Next

            If melhorFace Is Nothing Then Exit Sub

            ' 2) nessa face, pega a aresta linear mais longa
            Dim edges As Edges = CType(melhorFace.Edges, Edges)
            If edges Is Nothing OrElse edges.Count = 0 Then Exit Sub

            Dim melhorCompr As Double = -1.0
            Dim melhorEdge As Edge = Nothing

            For j = 1 To edges.Count
                Dim e As Edge = CType(edges.Item(j), Edge)
                Dim ehLinha As Boolean = False
                Try
                    Dim tipoE As Integer = CInt(CallByName(e, "GeometryType", CallType.Get))
                    ' igLine = 1 em geral
                    ehLinha = (tipoE = 1)
                Catch
                    ehLinha = False
                End Try
                If Not ehLinha Then Continue For

                Dim comp As Double = 0
                Try
                    comp = CDbl(CallByName(e, "Length", CallType.Get))
                Catch
                    comp = 0
                End Try

                If comp > melhorCompr Then
                    melhorCompr = comp
                    melhorEdge = e
                End If
            Next

            If melhorEdge Is Nothing Then Exit Sub

            faceOut = melhorFace
            edgeOut = melhorEdge

        Catch
            faceOut = Nothing : edgeOut = Nothing
        End Try
    End Sub

    Public Sub SalvarAssocicaoPDFAPI(nomeBase As String, caminhoPDF As String)
        System.Threading.ThreadPool.QueueUserWorkItem(
            Sub()
                Try
                    ' 1. Verificar se o registro já existe no banco MySQL para evitar repetição
                    Dim urlDB As String = caminhoPDF.Replace("\", "\\")
                    Dim queryExist As String = $"SELECT id FROM produtos_docs WHERE produto_codigo = '{nomeBase}' AND url_arquivo = '{urlDB}'"
                    
                    Dim dt As System.Data.DataTable = Nothing
                    Try
                        cl_BancoDados.AbrirBanco()
                        dt = cl_BancoDados.CarregarDados(queryExist)
                        cl_BancoDados.FecharBanco()
                    Catch
                        ' Se falhar o banco direto, segue em frente pois o app já usa
                    End Try

                    If dt Is Nothing OrElse dt.Rows.Count = 0 Then
                        ' 2. Enviar via POST para a API Node.js (que extrai os metadados com IA)
                        Dim postUrl As String = "http://192.168.1.77:3001/api/docs"
                        Dim request As System.Net.HttpWebRequest = CType(System.Net.WebRequest.Create(postUrl), System.Net.HttpWebRequest)
                        request.Method = "POST"
                        request.ContentType = "application/json"
                        request.Timeout = 10000 ' 10 segundos de timeout para não prender threads
                        
                        Dim urlEscapada As String = caminhoPDF.Replace("\", "\\").Replace("""", "\""")
                        Dim titulo As String = nomeBase & ".pdf"
                        Dim postData As String = "{""produto_codigo"":""" & nomeBase & """,""titulo"":""" & titulo & """,""url_arquivo"":""" & urlEscapada & """,""tipo"":""Desenho""}"

                        Dim byteArray As Byte() = System.Text.Encoding.UTF8.GetBytes(postData)
                        request.ContentLength = byteArray.Length

                        Using dataStream As System.IO.Stream = request.GetRequestStream()
                            dataStream.Write(byteArray, 0, byteArray.Length)
                        End Using

                        Using response As System.Net.HttpWebResponse = CType(request.GetResponse(), System.Net.HttpWebResponse)
                            ' O Node.js se encarrega de ler e inserir
                        End Using
                    End If
                Catch ex As Exception
                    System.Diagnostics.Debug.WriteLine("Erro ao associar PDF na API Node.js: " & ex.Message)
                End Try
            End Sub)
    End Sub
    ''' <summary>
    ''' Exporta PDF diretamente de um arquivo .dft (detalhamento) usando o caminho explicito.
    ''' Nao usa ActiveDocument - abre o .dft pelo caminho informado e fecha apos exportar.
    ''' </summary>
    ''' <param name="caminhoDFT">Caminho completo do arquivo .dft</param>
    ''' <param name="caminhoPDFDestino">Caminho completo onde o PDF sera salvo</param>
    Public Sub ExportarPDFDoDetalhamento(caminhoDFT As String, caminhoPDFDestino As String)
        Try
            If Not File.Exists(caminhoDFT) Then
                Debug.WriteLine("Aviso ExportarPDFDoDetalhamento: arquivo nao encontrado: " & caminhoDFT)
                Exit Sub
            End If

            Dim seApp As SolidEdgeFramework.Application =
                CType(Marshal.GetActiveObject("SolidEdge.Application"), SolidEdgeFramework.Application)

            If seApp Is Nothing Then Exit Sub

            Try : seApp.DisplayAlerts = False : Catch : End Try

            Dim draftDoc As SolidEdgeDraft.DraftDocument = Nothing
            Try
                draftDoc = CType(seApp.Documents.Open(caminhoDFT), SolidEdgeDraft.DraftDocument)

                If draftDoc Is Nothing Then
                    Debug.WriteLine("Aviso ExportarPDFDoDetalhamento: nao foi possivel abrir " & IO.Path.GetFileName(caminhoDFT))
                    Exit Sub
                End If

                Try
                    seApp.SetGlobalParameter(172, 1)
                Catch
                End Try

                Try
                    If File.Exists(caminhoPDFDestino) Then File.Delete(caminhoPDFDestino)
                Catch
                End Try

                draftDoc.SaveAs(caminhoPDFDestino)
                
                ' 🔹 Envia para a API para associação e IA
                Dim nomeProd As String = IO.Path.GetFileNameWithoutExtension(caminhoPDFDestino)
                SalvarAssocicaoPDFAPI(nomeProd, caminhoPDFDestino)

                Debug.WriteLine("PDF exportado: " & caminhoPDFDestino)

            Finally
                Try
                    If draftDoc IsNot Nothing Then draftDoc.Close(False)
                Catch
                End Try
                Try : seApp.DisplayAlerts = True : Catch : End Try
            End Try

        Catch ex As Exception
            Debug.WriteLine("Erro ExportarPDFDoDetalhamento: " & ex.Message)
            Throw
        End Try
    End Sub

End Module


