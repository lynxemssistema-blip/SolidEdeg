Imports System.Data
Imports System.Drawing
Imports System.Globalization
Imports System.IO
Imports System.Linq
Imports System.Runtime.InteropServices
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Threading
Imports System.Windows.Forms
Imports iText.IO.Font.Constants
Imports iText.Kernel.Font
Imports iText.Kernel.Geom
Imports iText.Kernel.Pdf
Imports iText.Kernel.Pdf.Canvas
Imports Microsoft.Extensions.Logging
Imports MySql.Data.MySqlClient
Imports Mysqlx.Datatypes
Imports SolidEdgeAssembly
Imports SolidEdgeFramework
Imports SolidEdgeGeometry
'Imports SolidEdgeFramework
Imports SolidEdgePart
Imports UglyToad.PdfPig.Graphics.Operations.SpecialGraphicsState
Imports Environment = System.Environment
Imports Font = System.Drawing.Font
Imports Path = System.IO.Path
Imports Thread = System.Threading.Thread
Imports Newtonsoft.Json



Public Class frmDadosPecaCorrente


    ' Variável que fará a escuta dos eventos gerais do Solid Edge
    Private WithEvents seAppEvents As SolidEdgeFramework.ISEApplicationEvents_Event

    ' Guarda qual foi o último documento lido para saber quando mudou
    Private ultimoDocumentoAnalisado As String = ""



#Region "🔧 VARIÁVEIS GLOBAIS"

    Private WithEvents timerAtualizacao As New System.Windows.Forms.Timer()    ' Controle de ediçao pendente
    Private editando As Boolean = False
    Private nomePropriedadeEditando As String = ""
    Private valorEditado As String = ""

    Private Salvar As Boolean = False


    Private dtproc As New System.Data.DataTable()





    ' Variáveis de controle para o drag & drop
    Private dragIndex As Integer
    Private dragRow As DataGridViewRow
    Private dropIndex As Integer
    Private dgvBOM As System.Windows.Forms.DataGridView = Nothing
    Private executandoLote As Boolean = False



#End Region


    'Private Sub btnDxf_Click(sender As Object, e As EventArgs) Handles btnDxf.Click
    '    Try
    '        If app Is Nothing OrElse app.Documents.Count = 0 Then Exit Sub
    '        Dim doc As Object = app.ActiveDocument
    '        If doc Is Nothing Then Exit Sub

    '        Dim caminho As String = doc.FullName
    '        Dim ext As String = IO.Path.GetExtension(caminho).ToLower()

    '        If ext = ".asm" Then
    '            Dim resp As DialogResult = MessageBox.Show(
    '                "O arquivo ativo é um Conjunto (.ASM). Deseja exportar a estrutura (BOM) em lote conforme as seleções de DXF/PDF?",
    '                "SINCO - Exportação em Lote",
    '                MessageBoxButtons.YesNo,
    '                MessageBoxIcon.Question
    '            )
    '            If resp = DialogResult.Yes Then
    '                ProcessarExportacaoBOM(dxfForcado:=True, pdfForcado:=False)
    '            End If
    '        Else
    '            ExportarDXFPlanificadoAuto(0)
    '        End If
    '    Catch ex As Exception
    '        MessageBox.Show("Erro ao gerar DXF: " & ex.Message, "Erro DXF", MessageBoxButtons.OK, MessageBoxIcon.Error)
    '    End Try
    'End Sub

    Private Sub frmDadosPecaCorrente_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        ' 🔹 Inicializa campos limpos e botões desativados
        LimaprCampos()
        SetBotoesAtivos(False)

        Try
            If app Is Nothing Then
                Try
                    app = Marshal.GetActiveObject("SolidEdge.Application")
                Catch
                    app = Nothing
                End Try
            End If

            ' Inicia a verificação a cada 1 segundo (1000 ms)
            timerAtualizacao.Interval = 1000
            timerAtualizacao.Start()
        Catch ex As Exception
        End Try

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
                row("DescricaoExibicao") = codGrupo
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
                row("DescricaoExibicao") = codTipo
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
                row("DescricaoExibicao") = codUM
            Next
            With cboB1_UM
                .DataSource = dtUM
                .DisplayMember = "DescricaoExibicao"
                .ValueMember = "Codigo_UM"
                .SelectedIndex = -1
            End With

            ' Se já houver documento aberto no Solid Edge ao inicializar, carrega-o
            If app IsNot Nothing Then
                Try
                    If app.Documents.Count > 0 AndAlso app.ActiveDocument IsNot Nothing Then
                        CarregarPropriedadesArquivoCorrente()
                    End If
                Catch
                End Try
            End If

        Catch ex As Exception
            MsgBox("Erro ao carregar listas do Protheus (Grupo, Tipo, UM):" & Environment.NewLine & ex.Message, MsgBoxStyle.Critical, "Erro")
        End Try

        Try
            If app IsNot Nothing Then
                seAppEvents = CType(app.ApplicationEvents, SolidEdgeFramework.ISEApplicationEvents_Event)
            End If
        Catch ex As Exception
            Debug.WriteLine("Não foi possível acoplar aos Eventos do Solid Edge: " & ex.Message)
        End Try

    End Sub

    ''' <summary>
    ''' Tela de Carregamento com Progresso Matemático 
    ''' </summary>
    Private Sub ProcessarMudancaDeDocumento()
        If executandoLote Then Exit Sub
        Try
            timerAtualizacao.Stop()

            ' Impede loops sincronizando o nome do documento ativo logo na entrada
            Try
                If app IsNot Nothing AndAlso app.Documents.Count > 0 Then
                    ultimoDocumentoAnalisado = app.ActiveDocument.FullName
                End If
            Catch
            End Try

            Me.Cursor = Cursors.WaitCursor

            ' 1. CONFIGURA O PROGRESSO INICIAL: Começa em 30% visualmente
            ProgressBar1.Style = ProgressBarStyle.Continuous
            ProgressBar1.Visible = True
            ProgressBar1.Value = 30 ' Preenche o primeiro terço da barra

            LimaprCampos()
            txtTitulo.Text = "CARREGANDO DADOS... POR FAVOR, AGUARDE"
            txtNumeroDesenho.Text = "PROCESSANDO ARQUIVO..."

            ' Força a barra aparecer em 30% IMEDIATAMENTE antes da tela congelar de vez
            System.Windows.Forms.Application.DoEvents()

            ' 2. INICIA A CARGA PESADA...
            ' (Opcionalmente, se você quiser, dentro de 'CarregarPropriedadesArquivoCorrente()'
            '  você pode espalhar: ProgressBar1.Value = 60 : Application.DoEvents() para dar mais passos)
            CarregarPropriedadesArquivoCorrente()

            ' 3. COMPLETA 100% ANTES DE FECHAR
            ProgressBar1.Value = 100
            System.Windows.Forms.Application.DoEvents()

            ' Espera meio segundo (500 ms) pro usuário enxergar que chegou no finalzinho (100% verde)
            System.Threading.Thread.Sleep(500)

        Catch ex As Exception
            Debug.WriteLine("Erro na troca de aba: " & ex.Message)
        Finally
            ' 4. DEVOLVE AO NORMAL
            Me.Cursor = Cursors.Default
            ProgressBar1.Value = 0
            ProgressBar1.Visible = False

            timerAtualizacao.Start()
        End Try
    End Sub


    ''' <summary>
    ''' Grava/Sincroniza os dados dos Comboboxes dentro do arquivo físico 3D atual (.PAR, .ASM ou .PSM)
    ''' </summary>
    Private Sub SincronizarPropriedadesCustomizadasDoArquivo()
        Try
            If app Is Nothing OrElse app.Documents.Count = 0 Then Exit Sub
            Dim doc As Object = app.ActiveDocument
            If doc Is Nothing Then Exit Sub

            ' Proteção: Só deixamos furar/escrever propriedades se for em arquivos 3D (PAR, ASM ou PSM)
            If doc.Type <> SolidEdgeFramework.DocumentTypeConstants.igPartDocument AndAlso doc.Type <> SolidEdgeFramework.DocumentTypeConstants.igAssemblyDocument AndAlso doc.Type <> SolidEdgeFramework.DocumentTypeConstants.igSheetMetalDocument Then
                Exit Sub
            End If

            Dim profCustom As Object = doc.Properties("Custom")

            Dim camposExigidos As New Dictionary(Of String, String) From {
                {"Tipo", Me.cboB1_TIPO.Text},
                {"Unidade", Me.cboB1_UM.Text},
                {"Grupo", Me.cboB1_GRUPO.Text}
            }

            Dim documentoSofreuAlteracoes As Boolean = False

            For Each item In camposExigidos
                Dim nomeDaVar As String = item.Key
                Dim valorAtual As String = item.Value

                ' Não grava no arquivo caso o campo do Protheus esteja vazio 
                If String.IsNullOrEmpty(valorAtual) Then Continue For

                Dim encontrado As Boolean = False
                For i As Integer = 1 To profCustom.Count
                    If profCustom.Item(i).Name = nomeDaVar Then
                        encontrado = True
                        ' Se a propriedade existe mas está com valor velho, nós atualizamos
                        If profCustom.Item(i).Value.ToString() <> valorAtual Then
                            profCustom.Item(i).Value = valorAtual
                            documentoSofreuAlteracoes = True
                        End If
                        Exit For
                    End If
                Next

                ' Se a propriedade NÃO EXISTIA dentro da peça, a gente cria na mesma hora
                If Not encontrado Then
                    profCustom.Add(nomeDaVar, valorAtual)
                    documentoSofreuAlteracoes = True
                End If
            Next

            ' Se precisou criar campo ou alterar valor velho, salva o documento CAD de forma transparente
            If documentoSofreuAlteracoes Then
                doc.Save()
                Debug.WriteLine("SINCO: Arquivo " & doc.Name & " salvo automaticamente com sucesso.")
            End If

        Catch ex As Exception
            Debug.WriteLine("ERRO PROPRIEDADES: " & ex.Message)
        End Try
    End Sub


    ''' <summary>
    ''' Evento NATIVO acionado imeditamente quando a janela de um documento ativo é alterada no Solid Edge.
    ''' </summary>
    Private Sub seAppEvents_AfterActiveDocumentChange(ByVal theDocument As Object) Handles seAppEvents.AfterActiveDocumentChange
        Try
            ' GUARD: Se está em lote (BOM/exportação), ignorar completamente.
            ' Sem este guard, Me.Invoke enfileira ProcessarMudancaDeDocumento na fila
            ' de mensagens, e ele seria executado DEPOIS que executandoLote = False,
            ' causando o encolhimento do formulário e destruição do grid.
            If executandoLote Then Exit Sub

            If Me.InvokeRequired Then
                Me.Invoke(New MethodInvoker(AddressOf ProcessarMudancaDeDocumento))
            Else
                ProcessarMudancaDeDocumento()
            End If
        Catch ex As Exception
            Debug.WriteLine("Erro AfterActiveDocumentChange: " & ex.Message)
        End Try
    End Sub



    ''' <summary>
    ''' Monitora em segundo plano se o usuário trocou a aba ativa do Solid Edge ou fechou documentos
    ''' </summary>
    Private Sub timerAtualizacao_Tick(sender As Object, e As EventArgs) Handles timerAtualizacao.Tick
        If executandoLote Then Exit Sub

        Try
            ' 1. Garante que a aplicação ainda está conectada
            If app Is Nothing Then
                Try
                    app = Marshal.GetActiveObject("SolidEdge.Application")
                Catch
                    app = Nothing
                End Try
            End If

            ' 2. Verifica se o Solid Edge está ativo e se possui documentos abertos
            Dim qtdDocs As Integer = 0
            If app IsNot Nothing Then
                Try
                    qtdDocs = app.Documents.Count
                Catch
                    ' Caso o Solid Edge tenha sido fechado pelo usuário
                    app = Nothing
                    qtdDocs = 0
                End Try
            End If

            If qtdDocs > 0 Then
                Dim caminhoAtual As String = ""

                Try
                    If app.ActiveDocument IsNot Nothing Then
                        caminhoAtual = app.ActiveDocument.FullName
                    End If
                Catch ex As Exception
                    ' Silencioso: pode falhar durante salvamento ou transição
                End Try

                ' 3. Se o arquivo na tela for DIFERENTE do último arquivo analisado, atualiza
                If Not String.IsNullOrEmpty(caminhoAtual) Then
                    If caminhoAtual <> ultimoDocumentoAnalisado Then
                        ultimoDocumentoAnalisado = caminhoAtual
                        ProcessarMudancaDeDocumento()
                    End If
                Else
                    If ultimoDocumentoAnalisado <> "" Then
                        ultimoDocumentoAnalisado = ""
                        LimaprCampos()
                        SetBotoesAtivos(False)
                    End If
                End If
            Else
                ' Nenhum documento aberto ou Solid Edge fechado
                If ultimoDocumentoAnalisado <> "" OrElse btnSalvarCadProtheus.Enabled Then
                    ultimoDocumentoAnalisado = ""
                    LimaprCampos()
                    SetBotoesAtivos(False)
                End If
            End If

        Catch ex As Exception
            ' Falhas no timer não devem disparar msgs na tela para não travar o usuário
        End Try
    End Sub




    Private Sub Button1_Click()
        DadosDesenhoCorrente()

    End Sub

    Private Sub Button2_Click()

        PlanificarDesenhoCorrente()

    End Sub

    Private Sub btnGerarPdf_Click(sender As Object, e As EventArgs) Handles btnGerarPdf.Click
        Try
            If app Is Nothing OrElse app.Documents.Count = 0 Then Exit Sub
            Dim doc As Object = app.ActiveDocument
            If doc Is Nothing Then Exit Sub

            Dim caminho As String = doc.FullName
            Dim ext As String = IO.Path.GetExtension(caminho).ToLower()

            If ext = ".asm" Then
                Dim resp As DialogResult = MessageBox.Show(
                    "O arquivo ativo é um Conjunto (.ASM). Deseja exportar a estrutura (BOM) em lote conforme as seleções de DXF/PDF?",
                    "SINCO - Exportação em Lote",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                )
                If resp = DialogResult.Yes Then
                    ProcessarExportacaoBOM(dxfForcado:=False, pdfForcado:=True)
                End If
            Else
                ExportarPDFDetalhamentoLote(0)
            End If
        Catch ex As Exception
            MessageBox.Show("Erro ao gerar PDF: " & ex.Message, "Erro PDF", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub


    ''' <summary>
    ''' Ativa ou desativa os botões principais de ação do formulário
    ''' </summary>
    Public Sub SetBotoesAtivos(ByVal ativo As Boolean)
        Try
            btnSalvarCadProtheus.Enabled = ativo
            btnGerarPdf.Enabled = ativo
            btnAbriDetalhamentoCorrente.Enabled = ativo
            btnListaConjunto.Enabled = ativo
            btnLote.Enabled = ativo
            chkdxf.Enabled = ativo
            chkPdf.Enabled = ativo
            chkiges.Enabled = ativo
            chkOpcaodePasta.Enabled = ativo
        Catch ex As Exception
        End Try
    End Sub

    Public Sub CarregarPropriedadesArquivoCorrente()

        'tenta ativa o maximo as variaveis do solid edge
        AtivarVariaveisESincronizarPropriedades()

        Try

            If app Is Nothing OrElse app.Documents.Count = 0 Then
                LimaprCampos()
                SetBotoesAtivos(False)
                Exit Sub
            End If

            Dim doc As Object = Nothing
            Try
                doc = app.ActiveDocument
            Catch
                doc = Nothing
            End Try

            If doc Is Nothing Then
                LimaprCampos()
                SetBotoesAtivos(False)
                Exit Sub
            End If

            Dim caminho As String = doc.FullName
            Dim ext As String = IO.Path.GetExtension(caminho).ToLower()
            If Not (ext = ".par" Or ext = ".psm" Or ext = ".asm") Then
                LimaprCampos()
                SetBotoesAtivos(False)
                Exit Sub
            End If

            If ext = ".par" OrElse ext = ".psm" Then
                ' ⚠️ Só recolhe o formulário se:
                '    1) Não está em lote de exportação E
                '    2) O grid da BOM não está visível atualmente
                Dim gridBomAtivo As Boolean = (dgvBOM IsNot Nothing AndAlso dgvBOM.Visible AndAlso dgvBOM.Parent IsNot Nothing)
                If Not executandoLote AndAlso Not gridBomAtivo Then
                    Me.ClientSize = New System.Drawing.Size(1120, 294)
                End If
            End If

            Dim NomeArquivo As String = IO.Path.GetFileNameWithoutExtension(caminho).ToUpper()

            txtNumeroDesenho.Text = NomeArquivo & ext
            txtendereco.Text = caminho

            ' 👉 Nenhuma leitura suja aqui! Delega para o Reader Service:
            Dim leitor As New SolidEdgeReaderService()
            leitor.ExtrairPropriedades(doc, DadosArquivoCorrente)

            ' Retorna e Atualiza UI
            txtTitulo.Text = DadosArquivoCorrente.Titulo
            txtCutSizex.Text = DadosArquivoCorrente.ComprimentoBlank
            txtCutSizey.Text = DadosArquivoCorrente.LarguraBlank
            txtEspessura.Text = DadosArquivoCorrente.Espessura
            txtPesoKg.Text = DadosArquivoCorrente.Massa
            txtAreametroquadr.Text = DadosArquivoCorrente.AreaPintura

            Dim connStringProtheus As String = "Host=192.168.1.61;Port=5432;Username=sinco2;Password=sinco25;Database=p12prd;"
            Using conn As New Npgsql.NpgsqlConnection(connStringProtheus)
                conn.Open()
                Dim sqlBusca As String = "SELECT B1_GRUPO, B1_XREVM, B1_TIPO, B1_UM FROM public.sb1010 WHERE TRIM(B1_COD) = @Codigo"
                Using cmd As New Npgsql.NpgsqlCommand(sqlBusca, conn)
                    cmd.Parameters.AddWithValue("@Codigo", NomeArquivo.Trim())
                    Using reader As Npgsql.NpgsqlDataReader = cmd.ExecuteReader()
                        If reader.Read() Then
                            Me.cboB1_GRUPO.Text = If(IsDBNull(reader("B1_GRUPO")), "", reader("B1_GRUPO").ToString().Trim())
                            Me.txtB1_XREVM.Text = If(IsDBNull(reader("B1_XREVM")), "", reader("B1_XREVM").ToString().Trim())
                            Me.cboB1_TIPO.Text = If(IsDBNull(reader("B1_TIPO")), "", reader("B1_TIPO").ToString().Trim())
                            Me.cboB1_UM.Text = If(IsDBNull(reader("B1_UM")), "", reader("B1_UM").ToString().Trim())
                        Else
                            Me.cboB1_GRUPO.Text = ""
                            Me.txtB1_XREVM.Text = ""
                            Me.cboB1_TIPO.Text = ""
                            Me.cboB1_UM.Text = ""
                        End If
                    End Using
                End Using
            End Using

            ' 👇 Sincroniza propriedades customizadas
            SincronizarPropriedadesCustomizadasDoArquivo()

            ' 🔹 Ativa os botões pois o documento foi carregado com sucesso
            SetBotoesAtivos(True)

        Catch ex As Exception
            LimaprCampos()
            SetBotoesAtivos(False)
        End Try
    End Sub

    Public Function CorrigirUTF8(textoErrado As String) As String
        If String.IsNullOrEmpty(textoErrado) Then Return ""

        ' Transforma a string errada em bytes usando o encoding ocidental (Latin1)
        Dim bytes As Byte() = Encoding.GetEncoding("ISO-8859-1").GetBytes(textoErrado)

        ' Converte esses bytes de volta para String usando UTF-8
        Return Encoding.UTF8.GetString(bytes)
    End Function

    Private Sub LimaprCampos()

        DadosArquivoCorrente.Titulo = ""
        txtTitulo.Clear()
        txtNumeroDesenho.Clear()
        txtendereco.Clear()

        DadosArquivoCorrente.AssuntoSubiTitulo = ""
        DadosArquivoCorrente.Comentarios = ""
        DadosArquivoCorrente.PalavraChave = ""
        DadosArquivoCorrente.Author = ""
        DadosArquivoCorrente.Verificado = ""
        DadosArquivoCorrente.Aprovado = ""
        DadosArquivoCorrente.material = ""
        DadosArquivoCorrente.DataCriacaDesenho = ""
        DadosArquivoCorrente.DataUltimoSalvamento = ""

        ' Dimensões
        DadosArquivoCorrente.ComprimentoBlank = ""
        txtCutSizex.Clear()

        DadosArquivoCorrente.LarguraBlank = ""
        txtCutSizey.Clear()

        DadosArquivoCorrente.Espessura = ""
        txtEspessura.Clear()

        DadosArquivoCorrente.Massa = ""
        txtPesoKg.Clear()

        DadosArquivoCorrente.AreaPintura = 0
        txtAreametroquadr.Clear()

        ' Campos Protheus
        txtB1_XREVM.Clear()
        cboB1_GRUPO.SelectedIndex = -1
        cboB1_GRUPO.Text = ""
        cboB1_TIPO.SelectedIndex = -1
        cboB1_TIPO.Text = ""
        cboB1_UM.SelectedIndex = -1
        cboB1_UM.Text = ""

    End Sub

    Public Sub ListarTodasPropriedadesDoArquivo(txtDestino As TextBox)
        Try
            ' ==========================================================
            ' 🔹 Obtém a instância ativa do Solid Edge
            ' ==========================================================
            ' Dim seApp As SolidEdgeFramework.Application =
            ' Marshal.GetActiveObject("SolidEdge.Application")

            If app Is Nothing OrElse app.Documents.Count = 0 Then
                MessageBox.Show("Nenhum documento aberto no Solid Edge.",
                            "SINCO - Solid Edge", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Exit Sub
            End If

            Dim doc As Object = app.ActiveDocument
            If doc Is Nothing Then Exit Sub

            ' ==========================================================
            ' 🔹 Builder de texto
            ' ==========================================================
            Dim sb As New StringBuilder()
            sb.AppendLine("🧾 LISTAGEM COMPLETA DE PROPRIEDADES DO ARQUIVO")
            sb.AppendLine(New String("="c, 80))
            sb.AppendLine($"📄 Arquivo: {doc.FullName}")
            sb.AppendLine($"📁 Tipo: {IO.Path.GetExtension(doc.FullName).ToUpper()}")
            sb.AppendLine(New String("="c, 80))
            sb.AppendLine()

            ' ==========================================================
            ' 🔹 Percorre todos os grupos e propriedades
            ' ==========================================================
            Dim propSets As Object = doc.Properties
            Dim totalProps As Integer = 0

            For Each propSet As Object In propSets
                Dim nomeGrupo As String = propSet.Name

                For Each prop As Object In propSet
                    Try
                        Dim nomeProp As String = prop.Name
                        Dim valor As String = ""

                        Try
                            valor = If(prop.Value IsNot Nothing, prop.Value.ToString(), "")
                        Catch
                            valor = "(erro ao ler)"
                        End Try

                        ' Verifica se é editável
                        Dim editavel As String = "Sim"
                        Try
                            Dim temp = prop.Value
                            prop.Value = temp
                        Catch
                            editavel = "Não"
                        End Try

                        ' 🧩 Linha formatada no estilo 'Grupo - Propriedade: Valor”
                        sb.AppendLine($"{nomeGrupo} - {nomeProp}: {valor}")
                        sb.AppendLine($"    ↳ Editável: {editavel}")
                        sb.AppendLine()
                        totalProps += 1

                    Catch ex As Exception
                        sb.AppendLine($"{nomeGrupo} - (Erro ao acessar propriedade): {ex.Message}")
                        sb.AppendLine()
                    End Try
                Next
            Next

            sb.AppendLine(New String("="c, 80))
            sb.AppendLine($"Total de propriedades: {totalProps}")
            sb.AppendLine("✅ Fim da listagem.")

            ' ==========================================================
            ' 🔹 Exibe no TextBox
            ' ==========================================================
            txtDestino.Text = sb.ToString()
            txtDestino.ScrollBars = ScrollBars.Both
            txtDestino.Multiline = True
            txtDestino.WordWrap = False
            txtDestino.ReadOnly = True
            txtDestino.SelectionStart = 0
            txtDestino.ScrollToCaret()

        Catch ex As Exception
            MessageBox.Show("Erro ao listar propriedades: " & ex.Message,
                        "SINCO - Solid Edge", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub


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

    Public Sub SalvarPropriedadeAlterada(nomeCompleto As String, novoValor As String)
        Try
            ' Obter instância ativa do Solid Edge
            'Dim seApp As SolidEdgeFramework.Application =
            ' Marshal.GetActiveObject("SolidEdge.Application")
            If app Is Nothing OrElse app.Documents.Count = 0 Then Exit Sub

            Dim doc As Object = app.ActiveDocument
            If doc Is Nothing Then Exit Sub

            ' Separar grupo e nome
            Dim partes = nomeCompleto.Split(New String() {" - "}, StringSplitOptions.None)
            If partes.Length <> 2 Then Exit Sub

            Dim grupo As String = partes(0).Trim()
            Dim nome As String = partes(1).Trim()

            Dim propSets As Object = doc.Properties
            Dim alterou As Boolean = False

            For Each propSet As Object In propSets
                If String.Equals(propSet.Name, grupo, StringComparison.OrdinalIgnoreCase) Then
                    For Each prop As Object In propSet
                        If String.Equals(prop.Name, nome, StringComparison.OrdinalIgnoreCase) Then
                            prop.Value = novoValor
                            alterou = True
                            Exit For
                        End If
                    Next
                End If
            Next

            If alterou Then
                ' 🔹 Força o Solid Edge a aplicar e sincronizar
                Try : doc.UpdatePropertyTextDisplay() : Catch : End Try
                Try : doc.Save() : Catch : End Try
                Try : doc.Properties.Save() : Catch : End Try


                'MessageBox.Show($"✅ Propriedade '{nomeCompleto}' atualizada com sucesso!",
                '            "SINCO - Solid Edge", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                MessageBox.Show($"⚠️ Propriedade '{nomeCompleto}' não encontrada.",
                            "SINCO - Solid Edge", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            End If

        Catch ex As Exception
            MessageBox.Show("Erro ao salvar propriedade: " & ex.Message,
                        "SINCO - Solid Edge", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub


    Private Sub txtTitulo_Leave(sender As Object, e As EventArgs) Handles txtTitulo.Leave

        SalvarPropriedadeAlterada(txtTitulo.Tag, txtTitulo.Text.ToUpper)

    End Sub


    Private Sub txtEspessura_Leave(sender As Object, e As EventArgs) Handles txtEspessura.Leave
        SalvarPropriedadeAlterada(txtEspessura.Tag, txtEspessura.Text.ToUpper)
    End Sub


    Private Sub txtCutSizex_Leave(sender As Object, e As EventArgs) Handles txtCutSizex.Leave
        SalvarPropriedadeAlterada(txtCutSizex.Tag, txtCutSizex.Text.ToUpper)
    End Sub


    Private Sub txtCutSizey_Leave(sender As Object, e As EventArgs) Handles txtCutSizey.Leave
        SalvarPropriedadeAlterada(txtCutSizey.Tag, txtCutSizey.Text.ToUpper)
    End Sub

    Private Sub txtAreametroquadr_Leave(sender As Object, e As EventArgs) Handles txtAreametroquadr.Leave
        SalvarPropriedadeAlterada(txtAreametroquadr.Tag, txtAreametroquadr.Text.ToUpper)
    End Sub


    Private Sub txtRevisao_Leave(sender As Object, e As EventArgs)
        SalvarPropriedadeAlterada(txtB1_XREVM.Tag, txtB1_XREVM.Text.ToUpper)
    End Sub

    Private Sub btnConfiguracoes_Click(sender As Object, e As EventArgs)

        cl_BancoDados.BuscarArquivoconf()


    End Sub

    Private Sub txtPesqProcesso_TextChanged(sender As Object, e As EventArgs)


        TimerdgvProcesso.Enabled = True

    End Sub

    Public Sub AtivarVariaveisESincronizarPropriedades(Optional valorBloqueado As String = Nothing, Optional silencioso As Boolean = True)
        Try

            ' 1) Garante que app aponta para a instância correta do Solid Edge
            If app Is Nothing Then
                Try
                    app = CType(Marshal.GetActiveObject("SolidEdge.Application"),
                            SolidEdgeFramework.Application)
                Catch ex As Exception
                    If Not silencioso Then
                        MessageBox.Show("Não foi possível localizar uma instância do Solid Edge aberta." &
                                    Environment.NewLine &
                                    "Detalhe: " & ex.Message,
                                    "Solid Edge",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error)
                    End If
                    Exit Sub
                End Try
            End If

            ' 2) Verifica se há documentos abertos
            Dim qtdDocs As Integer

            Try
                qtdDocs = app.Documents.Count
            Catch ex As Exception
                If Not silencioso Then
                    MessageBox.Show("Erro ao acessar app.Documents.Count: " & ex.Message,
                                "Solid Edge",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error)
                End If
                Exit Sub
            End Try

            If qtdDocs = 0 Then
                If Not silencioso Then
                    MessageBox.Show("Nenhum documento de Solid Edge está aberto.",
                                "Solid Edge",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information)
                End If
                Exit Sub
            End If

            ' 3) Pega o documento ativo
            Dim doc As Object = Nothing

            Try
                doc = app.ActiveDocument
            Catch ex As Exception
                If Not silencioso Then
                    MessageBox.Show("Erro ao acessar o documento ativo: " & ex.Message,
                                "Solid Edge",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error)
                End If
                Exit Sub
            End Try

            If doc Is Nothing Then
                If Not silencioso Then
                    MessageBox.Show("Nenhum documento ativo encontrado no Solid Edge.",
                                "Solid Edge",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information)
                End If
                Exit Sub
            End If

            ' 4) Daqui pra baixo você já tem um documento ativo garantido
            '    coloque aqui a lógica de sincronizar variáveis / propriedades
            'Exemplo:
            'SincronizarVariaveis(doc, valorBloqueado)

            Dim caminho As String = doc.FullName
            Dim extensao As String = Path.GetExtension(caminho).ToLowerInvariant()

            ' ---------- LISTA DE VARIÁVEIS CALCULADAS A GARANTIR ----------
            ' Mass_0 = para todos os tipos
            ' Área_de_superfície = mesmo tratamento de Mass_0 (todos os tipos)
            ' CutSizeX / CutSizeY = apenas .psm / .par
            Dim varsAlvos As New List(Of String) From {
                "Mass_0",
                "Área_de_superfície",      ' nome com acento
                "Area_de_superficie"       ' sinônimo sem acento
            }
            If extensao = ".psm" OrElse extensao = ".par" Then
                varsAlvos.Add("Flat_Pattern_Model_CutSizeX")
                varsAlvos.Add("Flat_Pattern_Model_CutSizeY")
            End If

            ' ---------- ACESSA VARIÁVEIS ----------
            Dim variables As Object = Nothing
            Try
                variables = doc.Variables
            Catch ex As Exception
                MessageBox.Show("Não foi possível acessar a lista de variáveis do documento." & vbCrLf & ex.Message,
                                "SINCO - Solid Edge", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub
            End Try

            ' ---------- Helper: achar variável por nome normalizado / sinônimos ----------
            Dim TryGetVariable As Func(Of IEnumerable(Of String), Object) =
                Function(nomes As IEnumerable(Of String)) As Object
                    Dim alvoNorms = nomes.Select(Function(x) NormalizeName(x)).Distinct().ToArray()
                    Try
                        For Each v As Object In variables
                            Try
                                Dim n As String = ""
                                Try : n = CStr(CallByName(v, "Name", CallType.Get)) : Catch : n = "" : End Try
                                If String.IsNullOrEmpty(n) Then Continue For
                                Dim vn = NormalizeName(n)
                                If alvoNorms.Contains(vn) Then Return v
                            Catch
                            End Try
                        Next
                    Catch
                    End Try
                    Return Nothing
                End Function

            ' ---------- se .psm: garanta Flat Pattern (para CutSizeX/Y aparecerem) ----------
            If String.Equals(extensao, ".psm", StringComparison.OrdinalIgnoreCase) Then
                Try
                    Dim sheetDoc = TryCast(doc, SolidEdgePart.SheetMetalDocument)
                    If sheetDoc IsNot Nothing Then
                        Dim fps = sheetDoc.FlatPatternModels
                        If fps IsNot Nothing AndAlso fps.Count = 0 Then
                            app.StartCommand(1494) ' Create Flat Pattern
                            Threading.Thread.Sleep(500)
                            app.DoIdle()
                        End If
                    End If
                Catch
                End Try
            End If

            ' ---------- ativa/exponhe cada variável alvo ----------
            Dim ativouAlgo As Boolean = False

            ' mapa de sinônimos para melhorar a busca
            Dim sinonimos As New Dictionary(Of String, String()) From {
                {"Mass_0", New String() {"Mass_0", "Mass0"}},
                {"Área_de_superfície", New String() {"Área_de_superfície", "Area_de_superficie", "Area de superficie", "SurfaceArea"}},
                {"Flat_Pattern_Model_CutSizeX", New String() {"Flat_Pattern_Model_CutSizeX", "FlatPatternModelCutSizeX", "CutSizeX"}},
                {"Flat_Pattern_Model_CutSizeY", New String() {"Flat_Pattern_Model_CutSizeY", "FlatPatternModelCutSizeY", "CutSizeY"}}
            }

            For Each alvo In varsAlvos
                Dim nomesBusca As IEnumerable(Of String) =
                    If(sinonimos.ContainsKey(alvo), sinonimos(alvo), New String() {alvo})

                Dim v As Object = TryGetVariable(nomesBusca)

                ' tente atualizar o doc e procurar de novo (caso de CutSize/Area aparecer após update)
                If v Is Nothing Then
                    Try : CallByName(doc, "Update", CallType.Method) : Catch : End Try
                    app.DoIdle()
                    v = TryGetVariable(nomesBusca)
                End If

                If v Is Nothing Then
                    Debug.WriteLine($"⚠️ Variável '{alvo}' não encontrada (agora).")
                    Continue For
                End If

                Dim mudou As Boolean = False

                ' Tenta IsActive
                Try
                    Dim atual As Boolean = True
                    Try : atual = CBool(CallByName(v, "IsActive", CallType.Get)) : Catch : atual = True : End Try
                    If Not atual Then
                        CallByName(v, "IsActive", CallType.Let, True)
                        mudou = True
                    End If
                Catch
                End Try

                ' Tenta Expose
                Try
                    Dim exposto As Boolean = True
                    Try : exposto = CBool(CallByName(v, "Expose", CallType.Get)) : Catch : exposto = True : End Try
                    If Not exposto Then
                        CallByName(v, "Expose", CallType.Let, True)
                        mudou = True
                    End If
                Catch
                End Try

                ' Tenta IsIncluded
                Try
                    Dim inc As Boolean = True
                    Try : inc = CBool(CallByName(v, "IsIncluded", CallType.Get)) : Catch : inc = True : End Try
                    If Not inc Then
                        CallByName(v, "IsIncluded", CallType.Let, True)
                        mudou = True
                    End If
                Catch
                End Try

                If mudou Then ativouAlgo = True
                Debug.WriteLine($"✅ Variável '{alvo}' disponível. Mudou estado: {mudou}")
            Next

            If ativouAlgo Then
                Try : CallByName(variables, "Update", CallType.Method) : Catch : End Try
                Try : CallByName(variables, "Apply", CallType.Method) : Catch : End Try
            End If

            ' ---------- recalc properties ----------
            Try : CallByName(doc, "UpdatePhysicalProperties", CallType.Method) : Catch : End Try

            ' ---------- customs: Tipo de desenho ----------
            Dim propSetsCustom As Object = doc.Properties("Custom")
            Dim nomePropTipo As String = "Tipo de desenho"
            Dim valorTipo As String = If(extensao = ".asm", "CONJUNTO DE MONTAGEM", "CHAPARIA")
            GarantirPropriedadeCustom(propSetsCustom, nomePropTipo, valorTipo, soSeVazio:=True)

            ' ---------- custom: Bloqueado (SIM/NÃO) ----------
            If String.IsNullOrWhiteSpace(valorBloqueado) Then
                GarantirPropriedadeCustom(propSetsCustom, "Bloqueado", "NÃO", soSeVazio:=True)
            Else
                Dim vBloq = If(valorBloqueado.Trim().ToUpperInvariant() = "SIM", "SIM", "NÃO")
                GarantirPropriedadeCustom(propSetsCustom, "Bloqueado", vBloq, soSeVazio:=False)
            End If

            ' ---------- sincroniza ----------
            Try : CallByName(doc, "UpdatePropertyTextDisplay", CallType.Method) : Catch : End Try
            Try : doc.Properties.Save() : Catch : End Try
            Try : doc.Save() : Catch : End Try

            Debug.WriteLine("═══════════════════════════════════════════════")
            Debug.WriteLine($"📄 {Path.GetFileName(caminho)}  → variáveis garantidas (Mass_0 / Área_de_superfície / CutSizeX/Y se aplicável)")
            Debug.WriteLine("✅ Rotina concluída.")
            Debug.WriteLine("═══════════════════════════════════════════════")

        Catch ex As Exception
            MessageBox.Show("Erro ao ativar variáveis/propriedades: " & ex.Message,
                            "SINCO - Solid Edge", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub




    '------------------------ helpers de normalização ------------------------
    Private Function StripDiacritics(s As String) As String
        If String.IsNullOrEmpty(s) Then Return ""
        Dim norm = s.Normalize(NormalizationForm.FormD)
        Dim sb As New StringBuilder(norm.Length)
        For Each ch In norm
            If CharUnicodeInfo.GetUnicodeCategory(ch) <> UnicodeCategory.NonSpacingMark Then
                sb.Append(ch)
            End If
        Next
        Return sb.ToString().Normalize(NormalizationForm.FormC)
    End Function


    '------------------------ propriedade custom genérica ------------------------
    Private Sub GarantirPropriedadeCustom(customSet As Object,
                                          nome As String,
                                          valor As String,
                                          Optional soSeVazio As Boolean = True)
        Try
            Dim existe As Boolean = False
            For Each p As Object In customSet
                Try
                    If String.Equals(CStr(p.Name), nome, StringComparison.OrdinalIgnoreCase) Then
                        existe = True
                        Dim atual As String = ""
                        Try : atual = If(p.Value IsNot Nothing, p.Value.ToString().Trim(), "") : Catch : atual = "" : End Try
                        If soSeVazio Then
                            If String.IsNullOrWhiteSpace(atual) Then p.Value = valor
                        Else
                            p.Value = valor
                        End If
                        Exit For
                    End If
                Catch
                End Try
            Next
            If Not existe Then
                Try
                    customSet.Add(nome, valor)
                Catch ex As Exception
                    Debug.WriteLine($"⚠️ Falha ao criar propriedade '{nome}': {ex.Message}")
                End Try
            End If
        Catch
        End Try
    End Sub

    Private Sub btnConverterChapa_Click(sender As Object, e As EventArgs)
        ConverterParOuPsm_AjustarDobras()
    End Sub

    Public Sub ConverterParOuPsm_AjustarDobras(Optional kFactorDesejado As Double = 0.485)
        Dim app As SolidEdgeFramework.Application = Nothing
        Dim docObj As Object = Nothing

        ' IDs de comando (podem variar por versão/linguagem – tentamos vários)
        Dim CMD_CONVERTER_PARA_CHAPA() As Integer = {11045, 11044, 11046} ' "Peça para Peça em Chapa"
        Dim CMD_MODO_SINCRONO() As Integer = {18946, 18945, 18944}       ' Alternar para Síncrono

        Try
            ' 1) Solid Edge + documento ativo
            app = CType(Marshal.GetActiveObject("SolidEdge.Application"), SolidEdgeFramework.Application)
            If app Is Nothing OrElse app.Documents.Count = 0 Then
                MsgBox("Nenhum documento aberto.", vbExclamation, "SINCO")
                Exit Sub
            End If

            docObj = app.ActiveDocument
            Dim fullName As String = CStr(CallByName(docObj, "FullName", CallType.Get))
            Dim ext As String = IO.Path.GetExtension(fullName).ToLowerInvariant()

            ' 2) Se for PAR (peça “sólida”), tentar converter para chapa
            Dim sheet As SheetMetalDocument = TryCast(docObj, SheetMetalDocument)
            If sheet Is Nothing AndAlso (TypeOf docObj Is PartDocument OrElse ext = ".par") Then
                For Each id In CMD_CONVERTER_PARA_CHAPA
                    Try : app.StartCommand(id) : app.DoIdle() : Threading.Thread.Sleep(500) : Exit For : Catch : End Try
                Next
                ' Reobtém doc ativo (pode ter trocado após converter)
                sheet = TryCast(app.ActiveDocument, SheetMetalDocument)
            End If

            ' Se ainda não é chapa, não dá pra ajustar dobras
            If sheet Is Nothing Then
                MsgBox("Este arquivo não é de chapa. Converta para .PSM e tente novamente.", vbExclamation, "SINCO")
                Exit Sub
            End If

            ' 3) (Opcional) forçar modo Síncrono
            For Each id In CMD_MODO_SINCRONO
                Try : app.StartCommand(id) : app.DoIdle() : Threading.Thread.Sleep(250) : Exit For : Catch : End Try
            Next

            ' 4) Descobrir ESPESSURA (tenta variáveis comuns e SheetMetalData)
            Dim variables As Object = Nothing
            Try : variables = sheet.Variables : Catch : variables = Nothing : End Try

            Dim espessuraMm As Double = 0
            Dim nomeVarEsp As String = ""

            Dim candidatosEsp As String() = {
            "Material Thickness", "Espessura do Material",
            "Thickness", "Espessura", "SheetMetal.Thickness"
        }

            If variables IsNot Nothing Then
                For Each v As Object In variables
                    Dim n As String = SafeGetName(v)
                    If n = "" Then Continue For
                    If candidatosEsp.Any(Function(c) String.Equals(n, c, StringComparison.OrdinalIgnoreCase)) Then
                        Dim val = SafeGetValue(v)
                        Dim num As Double
                        If TryParseNumero(val, num) Then
                            espessuraMm = num
                            nomeVarEsp = n
                            Exit For
                        End If
                    End If
                Next
            End If

            If espessuraMm <= 0 Then
                Try
                    Dim sm = sheet.SheetMetalData
                    ' Muitas instalações trazem em metros; se for bem pequeno, converte p/ mm
                    espessuraMm = sm.Thickness
                    If espessuraMm < 0.01 Then espessuraMm *= 1000.0R
                    nomeVarEsp = "SheetMetalData.Thickness"
                Catch
                End Try
            End If

            If espessuraMm <= 0 Then
                ' MsgBox("Não foi possível identificar a espessura da chapa.", vbExclamation, "SINCO")

                espessuraMm = InputBox("Não foi possível identificar a espessura da chapa, Informe a espessura da chapa em mm:", "SINCO - Espessura da Chapa", "1.0")

            End If


            If espessuraMm <= 0 Then
                MsgBox("Não foi possível identificar a espessura da chapa.", vbExclamation, "SINCO")
                Exit Sub
            End If


            ' 5) Definir BendRadius = espessura e K-Factor = 0,485
            Dim alterou As Boolean = False
            Dim exprEsp As String =
            If(String.IsNullOrWhiteSpace(nomeVarEsp),
               espessuraMm.ToString("0.###", CultureInfo.InvariantCulture),
               nomeVarEsp)

            ' (a) Padrões globais
            alterou = SetOrCreateVar(variables, "BendRadius", exprEsp) Or alterou
            alterou = SetOrCreateVar(variables, "NeutralFactor", kFactorDesejado.ToString(CultureInfo.InvariantCulture)) Or alterou
            alterou = SetOrCreateVar(variables, "Fator_Neutro", kFactorDesejado.ToString(CultureInfo.InvariantCulture)) Or alterou

            ' (b) Heurística: ajustar variáveis de raio por feature
            If variables IsNot Nothing Then
                For Each v As Object In variables
                    Dim n As String = SafeGetName(v)
                    If n = "" Then Continue For
                    Dim nn As String = RemoveDiacritics(n).ToLowerInvariant()
                    If nn.Contains("bendradius") OrElse nn.Contains("raio") AndAlso nn.Contains("dobra") Then
                        If Not SafeEditOrValue(v, n, exprEsp, espessuraMm) Then
                            ' ignora falha silenciosamente
                        Else
                            alterou = True
                        End If
                    End If
                Next
            End If

            ' 6) Aplicar/atualizar
            If alterou Then
                Try : CallByName(variables, "Update", CallType.Method) : Catch : End Try
                Try : CallByName(variables, "Apply", CallType.Method) : Catch : End Try
            End If

            Try : sheet.Update() : Catch : End Try
            Try : sheet.UpdatePhysicalProperties() : Catch : End Try
            app.DoIdle()

            MsgBox(
            $"✅ Ajustes aplicados:" & vbCrLf &
            $"• Espessura: {espessuraMm:0.###} mm ({If(nomeVarEsp = "", "valor numérico", nomeVarEsp)})" & vbCrLf &
            $"• BendRadius = {If(nomeVarEsp = "", espessuraMm.ToString("0.###"), nomeVarEsp)}" & vbCrLf &
            $"• K-factor = {kFactorDesejado:0.###}",
            vbInformation, "SINCO")

        Catch ex As Exception
            MsgBox("Erro ao converter/ajustar dobras: " & ex.Message, vbCritical, "SINCO")
        End Try
    End Sub

    '==================== Helpers ====================

    'Private Function SafeGetName(o As Object) As String
    '    Try : Return CStr(CallByName(o, "Name", CallType.Get)) : Catch : Return "" : End Try
    'End Function

    'Private Function SafeGetValue(o As Object) As String
    '    Try : Return CStr(CallByName(o, "Value", CallType.Get)) : Catch : Return "" : End Try
    'End Function
    Private Function TryParseNumero(txt As String, ByRef num As Double) As Boolean
        If String.IsNullOrWhiteSpace(txt) Then Return False

        ' Normaliza para minúsculas e remove unidades/espacos
        Dim s As String = txt.Trim().ToLowerInvariant()
        s = s.Replace("mm²", "").Replace("mm2", "")
        s = s.Replace("mm", "")
        s = s.Replace("kg", "")
        s = s.Replace(" ", "")

        ' tenta pt-BR e Invariant
        If Double.TryParse(s, Globalization.NumberStyles.Any, New Globalization.CultureInfo("pt-BR"), num) Then
            Return True
        End If
        s = s.Replace(",", ".")
        Return Double.TryParse(s, Globalization.NumberStyles.Any, Globalization.CultureInfo.InvariantCulture, num)
    End Function

    Private Function SafeGetName(o As Object) As String
        Try : Return CStr(CallByName(o, "Name", CallType.Get)) : Catch : Return "" : End Try
    End Function

    Private Function SafeGetValue(o As Object) As String
        Try : Return CStr(CallByName(o, "Value", CallType.Get)) : Catch : Return "" : End Try
    End Function

    Private Function SetOrCreateVar(variables As Object, nome As String, formulaOuValor As String) As Boolean
        If variables Is Nothing Then Return False
        Try
            Dim alvo As Object = Nothing
            For Each v As Object In variables
                If String.Equals(SafeGetName(v), nome, StringComparison.OrdinalIgnoreCase) Then
                    alvo = v : Exit For
                End If
            Next
            If alvo Is Nothing Then
                ' Add(nome, formula) – o Solid Edge cria como variável de usuário
                CallByName(variables, "Add", CallType.Method, nome, formulaOuValor)
                Return True
            Else
                Return SafeEditOrValue(alvo, nome, formulaOuValor, Nothing)
            End If
        Catch
            Return False
        End Try
    End Function

    ' Tenta .Edit(nome, formula). Se falhar, tenta atribuir .Value
    Private Function SafeEditOrValue(varObj As Object, nome As String, formula As String, Optional valorNumerico As Double? = Nothing) As Boolean
        Try
            CallByName(varObj, "Edit", CallType.Method, nome, formula)
            Return True
        Catch
            Try
                If valorNumerico.HasValue Then
                    CallByName(varObj, "Value", CallType.Let, valorNumerico.Value)
                    Return True
                Else
                    Dim num As Double
                    If TryParseNumero(formula, num) Then
                        CallByName(varObj, "Value", CallType.Let, num)
                        Return True
                    End If
                End If
            Catch
            End Try
        End Try
        Return False
    End Function



    Private Sub Button5_Click_1(sender As Object, e As EventArgs)


        Dim vals = LerFisicosProfundos(app.ActiveDocument)
        ' vals.MassaKg, vals.AreaM2, vals.CutXmm, vals.CutYmm

        txtPesoKg.Text = vals.MassaKg.ToString("0.###")
        txtAreametroquadr.Text = vals.AreaM2.ToString("0.0000")
        txtCutSizex.Text = vals.CutXmm.ToString("0.###")
        txtCutSizey.Text = vals.CutYmm.ToString("0.###")


    End Sub

    ' ============================================================
    ' 📦 Leitura PROFUNDA de Massa(kg), Área(m²) e CutSizeX/Y(mm)
    '  - Funciona para PAR/PSM (e ASM com soma recursiva)
    '  - Não depende de variáveis custom estarem ativas/visíveis
    '  - Usa múltiplos fallbacks (API geométrica e DXF temporário)
    ' ============================================================
    Public Function LerFisicosProfundos(doc As Object) As (MassaKg As Double, AreaM2 As Double, CutXmm As Double, CutYmm As Double)
        If doc Is Nothing Then Return (0, 0, 0, 0)

        Dim ext As String = System.IO.Path.GetExtension(CStr(CallByName(doc, "FullName", CallType.Get))).ToLowerInvariant()
        Dim massaKg As Double = 0.0
        Dim areaM2 As Double = 0.0
        Dim cutX As Double = 0.0
        Dim cutY As Double = 0.0

        ' ---------- 0) Força recálculo ----------
        Try : CallByName(doc, "UpdatePhysicalProperties", CallType.Method) : Catch : End Try
        Try : CallByName(doc, "Update", CallType.Method) : Catch : End Try

        ' ---------- 1) Massa / Área do documento corrente ----------
        Try
            Dim r = TryLerMassaAreaDoDocumento(doc)
            massaKg = r.MassaKg
            areaM2 = r.AreaM2
        Catch
        End Try

        ' ---------- 2) Se for ASM, soma recursivamente das ocorrências ----------
        If ext = ".asm" Then
            Dim somaAsm = SomarFisicosDeAssembly(doc)
            ' Só substitui se não veio nada do doc
            If massaKg <= 0 Then massaKg = somaAsm.MassaKg
            If areaM2 <= 0 Then areaM2 = somaAsm.AreaM2
        End If

        ' ---------- 3) CutSizeX/Y para PSM/PAR ----------
        If ext = ".psm" OrElse ext = ".par" Then
            ' 3A: tentar RangeBox do FlatPattern real
            Dim bb = TryGetFlatPatternBoundingBoxMm(doc)
            cutX = bb.WidthMm
            cutY = bb.HeightMm

            ' 3B: se falhar, gerar DXF temporário e ler extents
            If cutX <= 0 OrElse cutY <= 0 Then
                Dim dxfDims = TryGetBlankFromTempDXF(doc)
                If dxfDims.WidthMm > 0 AndAlso dxfDims.HeightMm > 0 Then
                    cutX = dxfDims.WidthMm
                    cutY = dxfDims.HeightMm
                End If
            End If
        End If

        Return (Math.Round(massaKg, 6), Math.Round(areaM2, 6), Math.Round(cutX, 3), Math.Round(cutY, 3))
    End Function

    ' ============================================================
    ' Massa/Área via propriedades internas + API geométrica (modelo)
    ' ============================================================
    Private Function TryLerMassaAreaDoDocumento(doc As Object) As (MassaKg As Double, AreaM2 As Double)
        Dim massaKg As Double = 0
        Dim areaM2 As Double = 0

        ' 1) Propriedades internas (ExtendedSummaryInformation costuma ter)
        massaKg = TryGetPropAsDouble(doc, {("ExtendedSummaryInformation", "Mass"), ("ExtendedSummaryInformation", "Massa")}, 1.0)
        areaM2 = TryGetPropAsDouble(doc, {("ExtendedSummaryInformation", "SurfaceArea"), ("ExtendedSummaryInformation", "Área de superfície")}, 1.0)

        ' 2) Se ainda zerado, tenta obter do Body
        If massaKg <= 0 OrElse areaM2 <= 0 Then
            Try
                Dim models As Object = CallByName(doc, "Models", CallType.Get)
                If models IsNot Nothing AndAlso CInt(CallByName(models, "Count", CallType.Get)) > 0 Then
                    Dim model As Object = CallByName(models, "Item", CallType.Method, 1)
                    Dim body As Object = CallByName(model, "Body", CallType.Get)

                    If body IsNot Nothing Then
                        ' (a) método direto de propriedades de massa
                        Dim m As Double = 0, a As Double = 0, v As Double = 0
                        If GetBodyMassAreaVolume(body, m, a, v) Then
                            If massaKg <= 0 Then massaKg = m
                            If areaM2 <= 0 Then areaM2 = a
                        End If

                        ' (b) Se ainda não tiver área, soma áreas de faces
                        If areaM2 <= 0 Then
                            areaM2 = TryComputeTotalAreaM2_FromBody(body)
                        End If
                    End If
                End If
            Catch
            End Try
        End If

        ' 3) Fallback duro: massa pelo volume * densidade (quando disponível)
        If massaKg <= 0 Then
            Try
                Dim densKgM3 As Double = TryGetDensityKgM3(doc)   ' ex.: variável "Propriedades_Físicas_Densidade"
                Dim volM3 As Double = TryGetBodyVolumeM3(doc)
                If densKgM3 > 0 AndAlso volM3 > 0 Then massaKg = densKgM3 * volM3
            Catch
            End Try
        End If

        Return (massaKg, areaM2)
    End Function

    Private Function GetBodyMassAreaVolume(body As Object, ByRef massKg As Double, ByRef areaM2 As Double, ByRef volumeM3 As Double) As Boolean
        massKg = 0 : areaM2 = 0 : volumeM3 = 0
        Try
            ' Muitos builds oferecem GetMassProperties(mass, area, volume, cgX, cgY, cgZ, ixx, iyy, izz, ixy, ixz, iyz)
            CallByName(body, "GetMassProperties", CallType.Method, massKg, areaM2, volumeM3, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)
            Return (massKg > 0 OrElse areaM2 > 0 OrElse volumeM3 > 0)
        Catch
            ' Alternativas: GetArea / GetVolume
            Try : areaM2 = CDbl(CallByName(body, "GetArea", CallType.Method)) : Catch : End Try
            Try : volumeM3 = CDbl(CallByName(body, "GetVolume", CallType.Method)) : Catch : End Try
            Return (areaM2 > 0 OrElse volumeM3 > 0)
        End Try
    End Function

    Private Function TryComputeTotalAreaM2_FromBody(body As Object) As Double
        Try
            Dim faces As Object = CallByName(body, "Faces", CallType.Get, 0) ' 0 = igQueryAll
            Dim cnt As Integer = CInt(CallByName(faces, "Count", CallType.Get))
            Dim areaTotalMm2 As Double = 0
            For i = 1 To cnt
                Try
                    Dim f As Object = CallByName(faces, "Item", CallType.Method, i)
                    Dim a As Double = CDbl(CallByName(f, "Area", CallType.Get))
                    ' Heurística de unidade
                    If a > 10000 Then
                        areaTotalMm2 += a            ' já parece mm²
                    Else
                        areaTotalMm2 += a * 1000000  ' veio em m²
                    End If
                Catch
                End Try
            Next
            Return areaTotalMm2 / 1000000.0R
        Catch
            Return 0
        End Try
    End Function

    Private Function TryGetBodyVolumeM3(doc As Object) As Double
        Try
            Dim models As Object = CallByName(doc, "Models", CallType.Get)
            If models Is Nothing OrElse CInt(CallByName(models, "Count", CallType.Get)) = 0 Then Return 0
            Dim model As Object = CallByName(models, "Item", CallType.Method, 1)
            Dim body As Object = CallByName(model, "Body", CallType.Get)
            Dim v As Double = 0
            Try : v = CDbl(CallByName(body, "GetVolume", CallType.Method)) : Catch : v = 0 : End Try
            Return v
        Catch
            Return 0
        End Try
    End Function

    Private Function TryGetDensityKgM3(doc As Object) As Double
        ' tenta em Custom e em Variáveis conhecidas (print do usuário mostra "Propriedades_Físicas_Densidade")
        Dim dens As Double = TryGetPropAsDouble(doc, {("Custom", "Propriedades_Físicas_Densidade"), ("Custom", "Density")}, 1.0)
        If dens > 0 Then Return dens

        ' Procura por variável
        Try
            Dim vars As Object = CallByName(doc, "Variables", CallType.Get)
            For Each v As Object In vars
                Dim n As String = ""
                Try : n = CStr(CallByName(v, "Name", CallType.Get)) : Catch : Continue For : End Try
                If NormalizeName(n).Contains("densidade") OrElse NormalizeName(n).Contains("density") Then
                    Dim val As Object = Nothing
                    Try : val = CallByName(v, "Value", CallType.Get) : Catch : End Try
                    Dim d As Double
                    If TryParseNumberWithUnits(If(val Is Nothing, "", val.ToString()), d) Then Return d
                End If
            Next
        Catch
        End Try
        Return 0
    End Function

    ' ============================================================
    ' Soma recursiva ASM (massa/área) por ocorrências
    ' ============================================================
    Private Function SomarFisicosDeAssembly(asmDoc As Object) As (MassaKg As Double, AreaM2 As Double)
        Dim m As Double = 0, a As Double = 0
        Try
            Dim occs As Object = CallByName(asmDoc, "Occurrences", CallType.Get)
            Dim cnt As Integer = 0
            Try : cnt = CInt(CallByName(occs, "Count", CallType.Get)) : Catch : cnt = 0 : End Try
            For i = 1 To cnt
                Try
                    Dim occ As Object = CallByName(occs, "Item", CallType.Method, i)
                    Dim childDoc As Object = CallByName(occ, "OccurrenceDocument", CallType.Get)
                    Dim childName As String = CStr(CallByName(occ, "OccurrenceFileName", CallType.Get))
                    Dim q As Integer = 1
                    Try : q = CInt(CallByName(occ, "Quantity", CallType.Get)) : Catch : q = 1 : End Try

                    Dim ext As String = System.IO.Path.GetExtension(childName).ToLowerInvariant()
                    If ext = ".asm" Then
                        Dim rAsm = SomarFisicosDeAssembly(childDoc)
                        m += rAsm.MassaKg * q
                        a += rAsm.AreaM2 * q
                    Else
                        Dim r = TryLerMassaAreaDoDocumento(childDoc)
                        m += r.MassaKg * q
                        a += r.AreaM2 * q
                    End If
                Catch
                End Try
            Next
        Catch
        End Try
        Return (m, a)
    End Function

    ' ============================================================
    ' FlatPattern → RangeBox (m → mm)
    ' ============================================================
    Private Function TryGetFlatPatternBoundingBoxMm(doc As Object) As (WidthMm As Double, HeightMm As Double)
        Try
            Dim sheet As Object = TryCast(doc, Object)
            Dim fps As Object = Nothing
            Try : fps = CallByName(sheet, "FlatPatternModels", CallType.Get) : Catch : fps = Nothing : End Try

            ' cria flat se necessário
            If fps Is Nothing OrElse CInt(CallByName(fps, "Count", CallType.Get)) = 0 Then
                Try : CallByName(AppDomain.CurrentDomain.GetData("SE_APP"), "StartCommand", CallType.Method, 1494) : Catch : End Try ' Create Flat Pattern
                Threading.Thread.Sleep(300) : System.Windows.Forms.Application.DoEvents()
                Try : fps = CallByName(sheet, "FlatPatternModels", CallType.Get) : Catch : fps = Nothing : End Try
                If fps Is Nothing OrElse CInt(CallByName(fps, "Count", CallType.Get)) = 0 Then Return (0, 0)
            End If

            Dim fp As Object = CallByName(fps, "Item", CallType.Method, 1)
            Dim body As Object = CallByName(fp, "Body", CallType.Get)
            Dim minX As Double, minY As Double, minZ As Double, maxX As Double, maxY As Double, maxZ As Double
            CallByName(body, "GetRangeBox", CallType.Method, minX, minY, minZ, maxX, maxY, maxZ)

            Dim dx = Math.Abs(maxX - minX) * 1000.0R
            Dim dy = Math.Abs(maxY - minY) * 1000.0R
            Return (Math.Round(dx, 3), Math.Round(dy, 3))
        Catch
            Return (0, 0)
        End Try
    End Function

    ' ============================================================
    ' Plano B: gera DXF TEMP e lê $EXTMIN/$EXTMAX do cabeçalho
    ' ============================================================
    Private Function TryGetBlankFromTempDXF(doc As Object) As (WidthMm As Double, HeightMm As Double)
        Try
            Dim full As String = CStr(CallByName(doc, "FullName", CallType.Get))
            Dim pastaTmp As String = System.IO.Path.GetTempPath()
            Dim nomeBase As String = System.IO.Path.GetFileNameWithoutExtension(full)
            Dim arqDxf As String = System.IO.Path.Combine(pastaTmp, nomeBase & "_tmp_flat.dxf")

            ' Remove se existir
            Try : If System.IO.File.Exists(arqDxf) Then System.IO.File.Delete(arqDxf) : 
            Catch : End Try

            ' Tenta exportar via SaveAsFlatDXFEx (passa qualquer face/edge válidas)
            Dim models As Object = CallByName(doc, "Models", CallType.Get)
            If models Is Nothing OrElse CInt(CallByName(models, "Count", CallType.Get)) = 0 Then Return (0, 0)
            Dim model As Object = CallByName(models, "Item", CallType.Method, 1)
            Dim body As Object = CallByName(model, "Body", CallType.Get)
            Dim faces As Object = CallByName(body, "Faces", CallType.Get, 0)
            Dim face As Object = CallByName(faces, "Item", CallType.Method, 1)
            Dim edges As Object = CallByName(face, "Edges", CallType.Get)
            Dim edge As Object = CallByName(edges, "Item", CallType.Method, 1)
            ' vertex pode ser Nothing
            CallByName(models, "SaveAsFlatDXFEx", CallType.Method, arqDxf, face, edge, Nothing, True)

            ' Espera o arquivo surgir
            Dim tries = 0
            Do While Not System.IO.File.Exists(arqDxf) AndAlso tries < 20
                Threading.Thread.Sleep(150) : tries += 1
            Loop
            If Not System.IO.File.Exists(arqDxf) Then Return (0, 0)

            ' Lê extents do DXF
            Dim minX As Double, minY As Double, maxX As Double, maxY As Double
            If ParseDxfExtents(arqDxf, minX, minY, maxX, maxY) Then
                Dim dx = Math.Abs(maxX - minX) ' DXF geralmente em mm
                Dim dy = Math.Abs(maxY - minY)
                ' limpa temporário
                Try : System.IO.File.Delete(arqDxf) : Catch : End Try
                Return (Math.Round(dx, 3), Math.Round(dy, 3))
            End If

            Try : System.IO.File.Delete(arqDxf) : Catch : End Try
        Catch
        End Try
        Return (0, 0)
    End Function

    Private Function ParseDxfExtents(caminho As String, ByRef minX As Double, ByRef minY As Double, ByRef maxX As Double, ByRef maxY As Double) As Boolean
        minX = 0 : minY = 0 : maxX = 0 : maxY = 0
        Try
            Dim lines = System.IO.File.ReadAllLines(caminho)
            ' No HEADER:
            '  $EXTMIN
            '  10
            '  <x>
            '  20
            '  <y>
            '  $EXTMAX
            '  10
            '  <x>
            '  20
            '  <y>
            Dim i As Integer = 0
            Dim foundMin As Boolean = False, foundMax As Boolean = False
            While i < lines.Length - 1
                Dim ln = lines(i).Trim()
                If ln = "$EXTMIN" Then
                    ' espera: 10, x / 20, y
                    If i + 4 < lines.Length AndAlso lines(i + 1).Trim() = "10" AndAlso lines(i + 3).Trim() = "20" Then
                        Double.TryParse(lines(i + 2).Trim().Replace(",", "."), Globalization.NumberStyles.Any, Globalization.CultureInfo.InvariantCulture, minX)
                        Double.TryParse(lines(i + 4).Trim().Replace(",", "."), Globalization.NumberStyles.Any, Globalization.CultureInfo.InvariantCulture, minY)
                        foundMin = True
                    End If
                ElseIf ln = "$EXTMAX" Then
                    If i + 4 < lines.Length AndAlso lines(i + 1).Trim() = "10" AndAlso lines(i + 3).Trim() = "20" Then
                        Double.TryParse(lines(i + 2).Trim().Replace(",", "."), Globalization.NumberStyles.Any, Globalization.CultureInfo.InvariantCulture, maxX)
                        Double.TryParse(lines(i + 4).Trim().Replace(",", "."), Globalization.NumberStyles.Any, Globalization.CultureInfo.InvariantCulture, maxY)
                        foundMax = True
                    End If
                End If
                i += 1
            End While
            Return foundMin AndAlso foundMax
        Catch
            Return False
        End Try
    End Function

    ' ============================================================
    ' Utilitários já usados
    ' ============================================================
    Private Function TryGetPropAsDouble(doc As Object,
                                        candidates As IEnumerable(Of (SetName As String, PropName As String)),
                                        Optional unitMultiplier As Double = 1.0) As Double
        For Each c In candidates
            Try
                Dim sets As Object = CallByName(doc, "Properties", CallType.Get, c.SetName)
                If sets Is Nothing Then Continue For
                For Each p As Object In sets
                    Dim n As String = ""
                    Try : n = CStr(CallByName(p, "Name", CallType.Get)) : Catch : Continue For : End Try
                    If NormalizeName(n) <> NormalizeName(c.PropName) Then Continue For
                    Dim raw As String = ""
                    Try : raw = CStr(CallByName(p, "Value", CallType.Get)) : Catch : raw = "" : End Try
                    Dim value As Double
                    If TryParseNumberWithUnits(raw, value) Then
                        Return value * unitMultiplier
                    End If
                Next
            Catch
            End Try
        Next
        Return 0.0
    End Function

    Private Function TryParseNumberWithUnits(s As String, ByRef value As Double) As Boolean
        value = 0
        If String.IsNullOrWhiteSpace(s) Then Return False
        Dim t As String = s.Trim().ToLowerInvariant()
        t = t.Replace("mm²", "").Replace("mm2", "").Replace("mm", "")
        t = t.Replace("kg", "").Replace(" m^2", "").Replace("m²", "").Replace("m2", "")
        t = t.Replace(" ", "")
        If Double.TryParse(t, Globalization.NumberStyles.Any, New Globalization.CultureInfo("pt-BR"), value) Then Return True
        t = t.Replace(",", ".")
        Return Double.TryParse(t, Globalization.NumberStyles.Any, Globalization.CultureInfo.InvariantCulture, value)
    End Function

    Private Function NormalizeName(n As String) As String
        If String.IsNullOrEmpty(n) Then Return ""
        n = RemoveDiacritics(n).Trim().ToLowerInvariant()
        n = n.Replace(" ", "").Replace("_", "")
        Return n
    End Function

    Private Function RemoveDiacritics(s As String) As String
        If String.IsNullOrEmpty(s) Then Return ""
        Dim norm = s.Normalize(NormalizationForm.FormD)
        Dim sb As New System.Text.StringBuilder(norm.Length)
        For Each ch As Char In norm
            If Globalization.CharUnicodeInfo.GetUnicodeCategory(ch) <> Globalization.UnicodeCategory.NonSpacingMark Then
                sb.Append(ch)
            End If
        Next
        Return sb.ToString().Normalize(NormalizationForm.FormC)
    End Function




    Private Sub Button7_Click(sender As Object, e As EventArgs)

        If app Is Nothing OrElse app.Documents.Count > 0 Then

            MsgBox("Feche todos os documentos do Solid Edge antes de continuar.", MsgBoxStyle.Exclamation, "SINCO - Solid Edge")

            Exit Sub

        Else


            ' [REMOVED] txtTitulo.Text = dgvDadosPdf.Rows(2).Cells("dgvDadosSelecionado").Value.ToString.ToUpper.Trim   '= If(doc.titulo, "")
            DadosArquivoCorrente.Titulo = txtTitulo.Text

            DadosArquivoCorrente.Bloqueado = "NÃO"

            ' [REMOVED] DadosArquivoCorrente.AssuntoSubiTitulo = dgvDadosPdf.Rows(2).Cells("dgvDadosSelecionado").Value.ToString.ToUpper.Trim  '= If(doc.titulo, "")
            DadosArquivoCorrente.AssuntoSubiTitulo = txtTitulo.Text


            Dim limpo As String

            ' [REMOVED] limpo = LimparUnidades(dgvDadosPdf.Rows(5).Cells("dgvDadosSelecionado").Value).ToString.ToUpper.Trim
            DadosArquivoCorrente.Massa = limpo
            txtPesoKg.Text = limpo


            ' [REMOVED] limpo = LimparUnidades(dgvDadosPdf.Rows(7).Cells("dgvDadosSelecionado").Value).ToString.ToUpper.Trim
            DadosArquivoCorrente.AreaPintura = limpo
            txtAreametroquadr.Text = limpo


            DadosArquivoCorrente.Espessura = limpo
            txtEspessura.Text = limpo


            ' [REMOVED] limpo = LimparUnidades(dgvDadosPdf.Rows(13).Cells("dgvDadosSelecionado").Value.ToString.ToUpper.Trim)
            DadosArquivoCorrente.ComprimentoBlank = limpo
            txtCutSizex.Text = limpo


            ' [REMOVED] limpo = LimparUnidades(dgvDadosPdf.Rows(14).Cells("dgvDadosSelecionado").Value.ToString.ToUpper.Trim)
            DadosArquivoCorrente.LarguraBlank = limpo
            txtCutSizey.Text = limpo



            ' [REMOVED] DadosArquivoCorrente.EnderecoArquivo = dgvDadosPdf.Rows(16).Cells("dgvDadosSelecionado").Value.ToString.ToUpper.Trim
            txtNumeroDesenho.Text = DadosArquivoCorrente.EnderecoArquivo

            ' Atualiza o campo de acabamento
            DadosArquivoCorrente.Acabamento = $"{DadosArquivoCorrente.Comentarios} - {DadosArquivoCorrente.PalavraChave}"



        End If

    End Sub


    Private Sub EnviarSINCOWeb(Optional codigoDefinitivoProtheus As String = "")
        Try
            ' 1. Pega os dados reais do seu Solid Edge e do Formulário
            Dim arquivo As String = DadosArquivoCorrente.NomeArquivoSemExtensao.Replace(".psm", "").Replace(".par", "").Replace(".asm", "").Replace(".prt", "").Replace(".PSM", "").Replace(".PAR", "").Replace(".ASM", "").Replace(".PRT", "")

            ' 👇 SUBSTITUI O NOME DO ARQUIVO PELO CÓDIGO DO PROTHEUS, SE ELE TIVER SIDO FORNECIDO/GERADO
            'If Not String.IsNullOrEmpty(codigoDefinitivoProtheus) Then
            '    arquivo = codigoDefinitivoProtheus
            'End If

            ' Continua capturando os endereços normalmente
            Dim caminhoCompleto As String = Me.txtendereco.Text

            Dim titulo As String = DadosArquivoCorrente.Titulo
            Dim grupo As String = Me.cboB1_GRUPO.Text
            Dim unidade As String = Me.cboB1_UM.Text
            Dim tipo As String = Me.cboB1_TIPO.Text
            Dim rev As String = Me.txtB1_XREVM.Text

            ' Novos campos adicionados
            Dim compBlank As String = Me.txtCutSizex.Text
            Dim largBlank As String = Me.txtCutSizey.Text
            Dim peso As String = Me.txtPesoKg.Text
            Dim areaPintura As String = Me.txtAreametroquadr.Text
            Dim espessura As String = Me.txtEspessura.Text

            ' 2. Sanitizamos os textos para o formato de Link Web (transforma espaços em %20, \ em %5C, etc)
            arquivo = Uri.EscapeDataString(If(arquivo, ""))
            ' 👇 Sanitiza o caminho longo também
            caminhoCompleto = Uri.EscapeDataString(If(caminhoCompleto, ""))

            titulo = Uri.EscapeDataString(If(titulo, ""))
            grupo = Uri.EscapeDataString(If(grupo, ""))
            unidade = Uri.EscapeDataString(If(unidade, ""))
            tipo = Uri.EscapeDataString(If(tipo, ""))
            rev = Uri.EscapeDataString(If(rev, ""))
            compBlank = Uri.EscapeDataString(If(compBlank, ""))
            largBlank = Uri.EscapeDataString(If(largBlank, ""))
            peso = Uri.EscapeDataString(If(peso, ""))
            areaPintura = Uri.EscapeDataString(If(areaPintura, ""))
            espessura = Uri.EscapeDataString(If(espessura, ""))

            ' 3. Montamos a URL completa utilizando o seu IP correto
            Dim urlBase As String = "http://192.168.1.77:3001/engenharia/sinco/formulario"

            ' 👇 Repare no '&caminho={caminhoCompleto}' injetado na nova URL
            Dim urlFinal As String = $"{urlBase}?arquivo={arquivo}&caminho={caminhoCompleto}&titulo={titulo}&grupo={grupo}&unidade={unidade}&tipo={tipo}&rev={rev}&compBlank={compBlank}&largBlank={largBlank}&peso={peso}&area={areaPintura}&espessura={espessura}"

            ' 4. Verifica se a janela já está aberta (pelo Edge) para apenas ativá-la
            Dim janelaAberta As Boolean = False
            For Each p As Process In Process.GetProcessesByName("msedge")
                ' Procura se há uma janela principal com o nome Sinco ou MetalFisa
                If Not String.IsNullOrEmpty(p.MainWindowTitle) AndAlso (p.MainWindowTitle.IndexOf("SINCO", StringComparison.OrdinalIgnoreCase) >= 0 OrElse p.MainWindowTitle.IndexOf("MetalFisa", StringComparison.OrdinalIgnoreCase) >= 0) Then
                    Try
                        ' Ativa/traz a janela pra frente caso já esteja aberta
                        AppActivate(p.Id)
                        System.Diagnostics.Process.Start(urlFinal) ' Carrega os novos dados na aba
                        janelaAberta = True
                        Exit For
                    Catch ex As Exception
                    End Try
                End If
            Next

            ' Se o sistema não tiver aberto a página antes, abre normalmente
            If Not janelaAberta Then
                System.Diagnostics.Process.Start(urlFinal)
            End If

        Catch ex As Exception
            MessageBox.Show("Erro ao tentar abrir a versão Web: " & ex.Message, "Erro Web", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub


    Private Sub btnSalvarCadProtheus_Click(sender As Object, e As EventArgs) Handles btnSalvarCadProtheus.Click

        ' ============================================================
        ' 🔹 Integração API Protheus via Node.js Middleware
        ' ============================================================

        ' 1. CONFIGURAÇÃO VISUAL INICIAL
        ProgressBar1.Style = ProgressBarStyle.Continuous
        ProgressBar1.Visible = True
        ProgressBar1.Value = 10  ' Dá o start visual
        Me.Cursor = Cursors.WaitCursor
        System.Windows.Forms.Application.DoEvents()

        Try
            ' Montar payload simplificado para enviar ao Node.js
            Dim tituloLimpo As String = UCase(DadosArquivoCorrente.Titulo).Trim()
            Dim codDesenho As String = DadosArquivoCorrente.NomeArquivoSemExtensao.Replace(".PSM", "").Replace(".PAR", "").Replace(".PRT", "").Replace(".ASM", "").Replace(".psm", "").Replace(".par", "").Replace(".prt", "").Replace(".asm", "")
            If String.IsNullOrEmpty(codDesenho) Then codDesenho = "desenho_metalfisa"
            Dim codigoSeguro As String = If(String.IsNullOrEmpty(codDesenho), "SEM_COD", codDesenho.ToUpper())

            Dim valGrupo As String = If(cboB1_GRUPO.SelectedValue IsNot Nothing, cboB1_GRUPO.SelectedValue.ToString(), cboB1_GRUPO.Text)
            valGrupo = If(String.IsNullOrWhiteSpace(valGrupo), "", valGrupo.Split({"-"c, " "c}, StringSplitOptions.RemoveEmptyEntries).FirstOrDefault())

            Dim valTipo As String = If(cboB1_TIPO.SelectedValue IsNot Nothing, cboB1_TIPO.SelectedValue.ToString(), cboB1_TIPO.Text)
            valTipo = If(String.IsNullOrWhiteSpace(valTipo), "", valTipo.Split({"-"c, " "c}, StringSplitOptions.RemoveEmptyEntries).FirstOrDefault())

            Dim valUM As String = If(cboB1_UM.SelectedValue IsNot Nothing, cboB1_UM.SelectedValue.ToString(), cboB1_UM.Text)
            valUM = If(String.IsNullOrWhiteSpace(valUM), "", valUM.Split({"-"c, " "c}, StringSplitOptions.RemoveEmptyEntries).FirstOrDefault())

            ' Validações padrão
            If valGrupo.ToString = "" Or valTipo.ToString = "" Or valUM.ToString = "" Then
                MsgBox("Os campos de Tipo, Unidade e Grupo são de preenchimento Obrigatório.", vbInformation)
                Exit Sub
            End If
            If tituloLimpo.ToString = "" Then
                MsgBox("O Titulo do desenho é de preenchimento Obrigatório", MsgBoxStyle.Exclamation, "Atenção")
                Exit Sub
            End If

            ProgressBar1.Value = 50
            System.Windows.Forms.Application.DoEvents()


            ' ============================================================
            ' BLOCO COMENTADO DEVIDO A ERROS DE SINTAXE (Falta de Using/If)
            ' ============================================================
            ' Using reader As New System.IO.StreamReader(postResponse.GetResponseStream())
            '     Dim responseResult As String = reader.ReadToEnd()
            ' 
            '     System.Diagnostics.Debug.WriteLine("SINCO: Produto tratado na API Protheus. Resposta: " & responseResult)
            ' 
            '     ' Supondo que responseResult venha da API Protheus
            '     Dim chapaCodigoProtheus As String = ""
            '     Dim novoOu As String = ""
            ' 
            '     ' 1. Extrair o conteúdo entre as chaves { }
            '     Dim matchCodigo = Regex.Match(responseResult, "(?<=\{)[^}]+(?=\})")
            '     If matchCodigo.Success Then
            '         chapaCodigoProtheus = matchCodigo.Value
            '     End If
            ' 
            '     ' 2. Lógica para identificar se foi Criado ou Alterado
            '     If responseResult.ToLower().Contains("criado") Then
            '         novoOu = "criado"
            '     ElseIf responseResult.ToLower().Contains("atualizado") Then
            '         novoOu = "Alterado"
            '     End If
            ' 
            '     ' Log de conferência
            '     '   System.Diagnostics.Debug.WriteLine("Chapa Código: " & chapaCodigoProtheus)
            '     '  System.Diagnostics.Debug.WriteLine("Status Operação: " & novoOu)
            ' 
            '     If novoOu = "criado" Then
            '         MsgBox("Produto Novo, o seu arquivo corrente deve ser renomeado para: " & chapaCodigoProtheus, vbInformation, "Novo Produto!!")
            '     Else
            '         EnviarSINCOWeb(matchCodigo.Value.ToString)
            '     End If
            ' 
            ' End Using
            ' 
            ' End Using
            ' 
            ' Else
            '     System.Diagnostics.Debug.WriteLine("SINCO: Falha ao obter token de acesso na API Protheus.")
            ' End If

        Catch exAPI As System.Net.WebException
            Dim erroDetalhado As String = exAPI.Message
            If exAPI.Response IsNot Nothing Then
                Try
                    Using reader As New System.IO.StreamReader(exAPI.Response.GetResponseStream())
                        Dim respText As String = reader.ReadToEnd()
                        erroDetalhado &= vbCrLf & "Detalhes do Servidor: " & respText
                    End Using
                Catch
                End Try
            End If
            System.Diagnostics.Debug.WriteLine("SINCO: Erro WebException Protheus -> " & erroDetalhado)
            MsgBox("SINCO: Erro na integração com a API Protheus -> " & erroDetalhado, MsgBoxStyle.Critical, "Erro de API")

        Catch exAPI As Exception
            System.Diagnostics.Debug.WriteLine("SINCO: Erro na integração com a API Protheus -> " & exAPI.Message)
            MsgBox("SINCO: Erro na integração com a API Protheus -> " & exAPI.Message, MsgBoxStyle.Critical, "Erro de API")

        Finally
            Me.Cursor = Cursors.Default
            ProgressBar1.Value = 0
            ProgressBar1.Visible = False
        End Try

    End Sub

    Private Sub btnAbriDetalhamentoCorrente_Click(sender As Object, e As EventArgs) Handles btnAbriDetalhamentoCorrente.Click

    End Sub

    Private Sub txtTitulo_TextChanged(sender As Object, e As EventArgs) Handles txtTitulo.TextChanged

    End Sub

    Private Sub btnListaConjunto_Click(sender As Object, e As EventArgs) Handles btnListaConjunto.Click
        Try
            executandoLote = True
            If app Is Nothing OrElse app.Documents.Count = 0 Then Exit Sub
            Dim doc As Object = app.ActiveDocument
            If doc Is Nothing Then Exit Sub

            Dim caminho As String = doc.FullName
            Dim ext As String = IO.Path.GetExtension(caminho).ToLower()

            If ext = ".asm" Then
                timerAtualizacao.Stop()

                ' ═══════════════════════════════════════════════════════════
                ' PASSO 1: Expandir formulário e mostrar grid IMEDIATAMENTE
                ' ═══════════════════════════════════════════════════════════
                ExibirGridBOM(Nothing)

                ' Posiciona o label de status e barra de progresso (visíveis no grid)
                Dim labelStatus As New Label()
                Try
                    labelStatus.AutoSize = True
                    labelStatus.Location = New System.Drawing.Point(ProgressBar1.Left, ProgressBar1.Top - 22)
                    labelStatus.ForeColor = System.Drawing.Color.FromArgb(31, 54, 92)
                    labelStatus.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold)
                    Me.Controls.Add(labelStatus)
                    labelStatus.BringToFront()

                    ProgressBar1.Value = 0
                    ProgressBar1.Visible = True
                    ProgressBar1.BringToFront()
                    System.Windows.Forms.Application.DoEvents()

                    ' ═══════════════════════════════════════════════════════════
                    ' PASSO 2: Ler BOM diretamente no dgvBOM visível
                    '          Linhas vão aparecendo em tempo real
                    ' ═══════════════════════════════════════════════════════════
                    Dim labelResumo As New Label()
                    ListarEstruturaBomMateriais(dgvBOM, ProgressBar1, labelStatus, labelResumo)

                    ' Verifica se obteve dados
                    Dim dtBom As DataTable = TryCast(dgvBOM.DataSource, DataTable)
                    If dtBom Is Nothing OrElse dtBom.Rows.Count = 0 Then
                        MessageBox.Show("A estrutura (BOM) retornou vazia.", "SINCO - BOM", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    Else
                        Dim qtdPecas As Integer = dtBom.Select("TipoLinha = 'PECA'").Length
                        Dim qtdMat As Integer = dtBom.Select("TipoLinha = 'MATERIAL'").Length
                        Dim qtdProc As Integer = dtBom.Select("TipoLinha = 'PROCESSO'").Length
                        MessageBox.Show(
                            "Leitura da BOM finalizada com sucesso!" & vbCrLf & vbCrLf &
                            "  Peças: " & qtdPecas.ToString() & vbCrLf &
                            "  Materiais: " & qtdMat.ToString() & vbCrLf &
                            "  Processos: " & qtdProc.ToString() & vbCrLf &
                            "  Total de linhas: " & dtBom.Rows.Count.ToString(),
                            "SINCO - BOM Concluída", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    End If

                Finally
                    Try : Me.Controls.Remove(labelStatus) : Catch : End Try
                    ProgressBar1.Visible = False
                    ultimoDocumentoAnalisado = ""
                    timerAtualizacao.Start()
                End Try
            Else
                MsgBox("O arquivo ativo não é um conjunto (.ASM).", MsgBoxStyle.Exclamation, "SINCO - BOM")
            End If
        Catch ex As Exception
            MessageBox.Show("Erro ao carregar BOM: " & ex.Message, "Erro BOM", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            executandoLote = False
        End Try
    End Sub

    Private Sub ExibirGridBOM(dtBom As DataTable)
        Me.SuspendLayout()
        Try
            ' --- Cria o grid somente na primeira vez ---
            If dgvBOM Is Nothing Then
                dgvBOM = New System.Windows.Forms.DataGridView()
                dgvBOM.Name = "dgvBOM"
                dgvBOM.Location = New System.Drawing.Point(8, 290)
                dgvBOM.Size = New System.Drawing.Size(1109, 280)
                dgvBOM.Anchor = AnchorStyles.Left Or AnchorStyles.Right Or AnchorStyles.Top Or AnchorStyles.Bottom
                dgvBOM.AllowUserToAddRows = False
                dgvBOM.ReadOnly = True
                dgvBOM.SelectionMode = DataGridViewSelectionMode.FullRowSelect
                dgvBOM.BackgroundColor = Color.White
                dgvBOM.GridColor = Color.FromArgb(224, 224, 224)
                dgvBOM.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 250, 252)
                dgvBOM.Font = New Font("Segoe UI", 9.0!)
                dgvBOM.ColumnHeadersDefaultCellStyle.Font = New Font("Segoe UI", 9.0!, FontStyle.Bold)
                dgvBOM.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(31, 54, 92)
                dgvBOM.ColumnHeadersDefaultCellStyle.ForeColor = Color.White
                dgvBOM.EnableHeadersVisualStyles = False
                Me.Controls.Add(dgvBOM)
            End If

            ' --- Vincula os dados (aceita Nothing para grid vazio) ---
            If dtBom IsNot Nothing Then
                dgvBOM.DataSource = dtBom
            End If

            ' --- Colunas de ícone (apenas se ainda não existirem) ---
            If Not dgvBOM.Columns.Contains("dgvdxf") Then
                Dim colDxf As New DataGridViewImageColumn()
                colDxf.Name = "dgvdxf"
                colDxf.HeaderText = "DXF"
                colDxf.Image = My.Resources.sem_icone
                colDxf.ImageLayout = DataGridViewImageCellLayout.Zoom
                colDxf.Width = 40
                dgvBOM.Columns.Add(colDxf)
            End If
            If Not dgvBOM.Columns.Contains("dgvpdf") Then
                Dim colPdf As New DataGridViewImageColumn()
                colPdf.Name = "dgvpdf"
                colPdf.HeaderText = "PDF"
                colPdf.Image = My.Resources.sem_icone
                colPdf.ImageLayout = DataGridViewImageCellLayout.Zoom
                colPdf.Width = 40
                dgvBOM.Columns.Add(colPdf)
            End If

            ' --- Expande o formulário ---
            Me.MinimumSize = System.Drawing.Size.Empty
            Me.MaximumSize = System.Drawing.Size.Empty
            Me.ClientSize = New System.Drawing.Size(1120, 590)

            ' --- Posiciona o grid ---
            dgvBOM.Location = New System.Drawing.Point(8, 290)
            dgvBOM.Size = New System.Drawing.Size(Me.ClientSize.Width - 16, Me.ClientSize.Height - 300)

            ' --- Torna visível ---
            dgvBOM.Visible = True
            dgvBOM.BringToFront()

        Finally
            Me.ResumeLayout(True)
            Me.Update()
            If dgvBOM IsNot Nothing Then dgvBOM.Refresh()
            System.Windows.Forms.Application.DoEvents()
        End Try
    End Sub

    Private Sub ProcessarExportacaoBOM(dxfForcado As Boolean, pdfForcado As Boolean)
        Try
            executandoLote = True
            timerAtualizacao.Stop()

            ' ═══ PASSO 1: Expandir formulário e mostrar grid IMEDIATAMENTE ═══
            Dim dtBom As DataTable = Nothing
            If dgvBOM IsNot Nothing AndAlso dgvBOM.Visible AndAlso dgvBOM.DataSource IsNot Nothing Then
                dtBom = TryCast(dgvBOM.DataSource, DataTable)
            Else
                ExibirGridBOM(Nothing)
            End If

            Dim labelStatus As New Label()
            Try
                labelStatus.AutoSize = True
                labelStatus.Location = New System.Drawing.Point(ProgressBar1.Left, ProgressBar1.Top - 22)
                labelStatus.ForeColor = System.Drawing.Color.FromArgb(31, 54, 92)
                labelStatus.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold)
                Me.Controls.Add(labelStatus)
                labelStatus.BringToFront()

                ProgressBar1.Value = 0
                ProgressBar1.Visible = True
                ProgressBar1.BringToFront()
                System.Windows.Forms.Application.DoEvents()

                ' ═══ PASSO 2: Ler BOM diretamente no dgvBOM (se ainda não tem dados) ═══
                If dtBom Is Nothing Then
                    Dim labelResumo As New Label()
                    ListarEstruturaBomMateriais(dgvBOM, ProgressBar1, labelStatus, labelResumo)
                    dtBom = TryCast(dgvBOM.DataSource, DataTable)
                End If

                If dtBom Is Nothing OrElse dtBom.Rows.Count = 0 Then
                    MessageBox.Show("A estrutura (BOM) do conjunto retornou vazia ou não pôde ser gerada.", "SINCO - Exportação", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    Exit Sub
                End If

                Dim tempChkDXF As New CheckBox() With {.Checked = chkdxf.Checked OrElse dxfForcado}
                Dim tempChkPDF As New CheckBox() With {.Checked = chkPdf.Checked OrElse pdfForcado}
                Dim tempChkIGES As New CheckBox() With {.Checked = chkiges.Checked}

                If Not (tempChkDXF.Checked OrElse tempChkPDF.Checked OrElse tempChkIGES.Checked) Then
                    tempChkDXF.Checked = dxfForcado
                    tempChkPDF.Checked = pdfForcado
                End If

                ExportarPorGrid(dgvBOM, tempChkDXF, tempChkPDF, tempChkIGES, ProgressBar1, labelStatus, chkOpcaodePasta)

                MessageBox.Show("Exportação da estrutura (BOM) em lote finalizada com sucesso!", "SINCO - Exportação", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Finally
                Try : Me.Controls.Remove(labelStatus) : Catch : End Try
                ultimoDocumentoAnalisado = ""
                timerAtualizacao.Start()
            End Try

        Catch ex As Exception
            MessageBox.Show("Erro ao realizar exportação em lote da BOM: " & ex.Message, "SINCO - Exportação", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            executandoLote = False
        End Try
    End Sub

    Private Sub cboB1_TIPO_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboB1_TIPO.SelectedIndexChanged

    End Sub

    Private Sub cboB1_TIPO_SelectedValueChanged(sender As Object, e As EventArgs) Handles cboB1_TIPO.SelectedValueChanged

    End Sub

    Private Sub btnDxf_Click(sender As Object, e As EventArgs)

    End Sub

    'Private Sub btnDxf_Click(sender As Object, e As EventArgs) Handles btnDxf.Click

    'End Sub

    Private Sub btnLote_Click(sender As Object, e As EventArgs) Handles btnLote.Click
        ' Validação das opções
        If Not chkPdf.Checked AndAlso Not chkdxf.Checked Then
            MessageBox.Show("Selecione pelo menos uma opção: Gerar PDF ou Gerar DXF.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        ' Selecionar a pasta com o diálogo moderno
        Dim fbd As New ModernFolderBrowserDialog()
        fbd.Title = "Selecione a pasta com os arquivos do Solid Edge"

        If fbd.ShowDialog(Me) = DialogResult.OK Then
            Dim pastaOrigem As String = fbd.SelectedPath

            ' Obter arquivos
            Dim arquivos As New List(Of String)
            Try
                arquivos.AddRange(IO.Directory.GetFiles(pastaOrigem, "*.dft", IO.SearchOption.AllDirectories))
                arquivos.AddRange(IO.Directory.GetFiles(pastaOrigem, "*.par", IO.SearchOption.AllDirectories))
                arquivos.AddRange(IO.Directory.GetFiles(pastaOrigem, "*.psm", IO.SearchOption.AllDirectories))
                arquivos.AddRange(IO.Directory.GetFiles(pastaOrigem, "*.asm", IO.SearchOption.AllDirectories))
            Catch ex As Exception
                MessageBox.Show("Erro ao ler os arquivos da pasta: " & ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub
            End Try

            If arquivos.Count = 0 Then
                MessageBox.Show("Nenhum arquivo do Solid Edge encontrado na pasta selecionada.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Exit Sub
            End If

            ' Conectar ao Solid Edge
            Try
                If app Is Nothing Then
                    Try
                        app = Marshal.GetActiveObject("SolidEdge.Application")
                    Catch ex As Exception
                        app = Activator.CreateInstance(Type.GetTypeFromProgID("SolidEdge.Application"))
                        app.Visible = True ' Opcional: mostrar ou esconder
                    End Try
                End If

                ' Processar cada arquivo
                For Each arquivo In arquivos
                    Dim doc As Object = Nothing
                    Try
                        ' Abrir o arquivo
                        doc = app.Documents.Open(arquivo)

                        If doc IsNot Nothing Then
                            ' Fazer as chamadas de exportação
                            If chkPdf.Checked Then
                                ExportarPDFDetalhamentoLote(0)
                            End If

                            If chkdxf.Checked Then
                                ExportarDXFPlanificadoAuto(0)
                            End If
                        End If
                    Catch ex As Exception
                        ' Ignora erro para este arquivo específico, continua com o próximo
                    Finally
                        ' Fechar o documento
                        If doc IsNot Nothing Then
                            Try
                                doc.Close()
                            Catch exClose As Exception
                            End Try
                            Marshal.ReleaseComObject(doc)
                            doc = Nothing
                        End If
                    End Try
                Next

                MessageBox.Show("Processamento em lote finalizado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information)

            Catch ex As Exception
                MessageBox.Show("Erro de comunicação com o Solid Edge: " & ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End If
    End Sub
End Class

Public Class ModernFolderBrowserDialog
    <ComImport, Guid("DC1C5A9C-E88A-4dde-A5A1-60F82A20AEF7")>
    Private Class FileOpenDialog
    End Class

    <ComImport, Guid("42f85136-db7e-439c-85f1-e4075d135fc8"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)>
    Private Interface IFileDialog
        <PreserveSig> Function Show(<[In]> hwndOwner As IntPtr) As Integer
        Sub SetFileTypes(<[In]> cFileTypes As UInteger, <[In]> rgFilterSpec As IntPtr)
        Sub SetFileTypeIndex(<[In]> iFileType As UInteger)
        Sub GetFileTypeIndex(<Out> ByRef piFileType As UInteger)
        Sub Advise(<[In]> pfde As IntPtr, <Out> ByRef pdwCookie As UInteger)
        Sub Unadvise(<[In]> dwCookie As UInteger)
        Sub SetOptions(<[In]> fos As Integer)
        Sub GetOptions(<Out> ByRef pfos As Integer)
        Sub SetDefaultFolder(<[In]> psi As IntPtr)
        Sub SetFolder(<[In]> psi As IntPtr)
        Sub GetFolder(<Out> ByRef ppsi As IntPtr)
        Sub GetCurrentSelection(<Out> ByRef ppsi As IntPtr)
        Sub SetFileName(<[In], MarshalAs(UnmanagedType.LPWStr)> pszName As String)
        Sub GetFileName(<Out, MarshalAs(UnmanagedType.LPWStr)> ByRef pszName As String)
        Sub SetTitle(<[In], MarshalAs(UnmanagedType.LPWStr)> pszTitle As String)
        Sub SetOkButtonLabel(<[In], MarshalAs(UnmanagedType.LPWStr)> pszText As String)
        Sub SetFileNameLabel(<[In], MarshalAs(UnmanagedType.LPWStr)> pszLabel As String)
        Sub GetResult(<Out> ByRef ppsi As IShellItem)
        Sub AddPlace(<[In]> psi As IntPtr, <[In]> fdap As Integer)
        Sub SetDefaultExtension(<[In], MarshalAs(UnmanagedType.LPWStr)> pszDefaultExtension As String)
        Sub Close(<[In]> hr As Integer)
        Sub SetClientGuid(<[In]> ByRef guid As Guid)
        Sub ClearClientData()
        Sub SetFilter(<[In]> pFilter As IntPtr)
    End Interface

    <ComImport, Guid("43826D1E-E718-42EE-BC55-A1E261C37BFE"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)>
    Private Interface IShellItem
        Sub BindToHandler(<[In]> pbc As IntPtr, <[In]> ByRef bhid As Guid, <[In]> ByRef riid As Guid, <Out> ByRef ppv As IntPtr)
        Sub GetParent(<Out> ByRef ppsi As IShellItem)
        Sub GetDisplayName(<[In]> sigdnName As Integer, <Out> ByRef ppszName As IntPtr)
        Sub GetAttributes(<[In]> sfgaoMask As UInteger, <Out> ByRef psfgaoAttribs As UInteger)
        Sub Compare(<[In]> psi As IShellItem, <[In]> hint As UInteger, <Out> ByRef piOrder As Integer)
    End Interface

    Public Property SelectedPath As String
    Public Property Title As String = "Selecionar Pasta"

    Public Function ShowDialog(owner As IWin32Window) As DialogResult
        Dim dialog As IFileDialog = CType(New FileOpenDialog(), IFileDialog)
        Try
            Dim options As Integer
            dialog.GetOptions(options)
            ' FOS_PICKFOLDERS = 0x20
            ' FOS_FORCEFILESYSTEM = 0x40
            options = options Or &H20 Or &H40
            dialog.SetOptions(options)
            dialog.SetTitle(Title)

            Dim hwnd As IntPtr = If(owner IsNot Nothing, owner.Handle, IntPtr.Zero)
            Dim hr As Integer = dialog.Show(hwnd)
            If hr = 0 Then
                Dim item As IShellItem = Nothing
                dialog.GetResult(item)
                If item IsNot Nothing Then
                    Dim ptr As IntPtr
                    item.GetDisplayName(&H80058000, ptr) ' SIGDN_FILESYSPATH
                    If ptr <> IntPtr.Zero Then
                        SelectedPath = Marshal.PtrToStringAuto(ptr)
                        Marshal.FreeCoTaskMem(ptr)
                        Return DialogResult.OK
                    End If
                End If
            End If
            Return DialogResult.Cancel
        Finally
            Marshal.ReleaseComObject(dialog)
        End Try
    End Function
End Class

Module ModListarComponentesMontagem


#Region "VERSÃO ANTERIOR - 26-11-2025 rev 00"
    Public Sub ListarEstruturaComTodasPropriedadesBlank(ByVal dgv As DataGridView,
                                                    ByVal pgb As ProgressBar,
                                                    ByVal lbl As Label, lblResumo As Label)

        ' Lista que acumula erros
        Dim erros As New List(Of String)

        ' Contador de documentos que deram RPC_E_DISCONNECTED
        Dim qtdRpcDesconectado As Integer = 0

        ' Helper para registrar erros com contexto
        Dim RegistrarErro As Action(Of String, Exception) =
    Sub(contexto As String, ex As Exception)
        Try
            If ex Is Nothing Then Return

            Dim msg As String

            ' Detecta erro COM de objeto desconectado (RPC_E_DISCONNECTED)
            Dim comEx = TryCast(ex, System.Runtime.InteropServices.COMException)
            If comEx IsNot Nothing AndAlso comEx.ErrorCode = &H80010108 Then
                qtdRpcDesconectado += 1
                msg = $"{contexto}: [RPC_E_DISCONNECTED] O objeto do Solid Edge foi desconectado. Detalhe: {ex.Message}"
            Else
                msg = $"{contexto}: {ex.Message}"
            End If

            erros.Add(msg)
        Catch
            ' não deixa o log quebrar o fluxo
        End Try
    End Sub

        Try
            '============================================================
            ' 🔹 Sessão do Solid Edge e documento ativo
            '============================================================
            If app Is Nothing OrElse app.Documents.Count = 0 Then
                MessageBox.Show("Nenhum documento aberto no Solid Edge.",
                        "SINCO - Solid Edge",
                        MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Exit Sub
            End If

            Dim doc As Object = app.ActiveDocument
            If doc Is Nothing Then Exit Sub

            If app IsNot Nothing Then
                Try : app.DisplayAlerts = False : Catch : End Try
            End If

            '============================================================
            ' 🔹 UI - estado inicial
            '============================================================
            pgb.Visible = True : pgb.Style = ProgressBarStyle.Marquee : pgb.Value = 0
            lbl.Visible = True : lbl.Text = "Lendo estrutura... aguarde."

            '============================================================
            ' 🔹 Tabela destino
            '============================================================
            Dim tabela As New DataTable("BOM_Completa")

            ' Colunas fixas
            tabela.Columns.Add("Nivel", GetType(Integer))
            tabela.Columns.Add("Arquivo", GetType(String))          ' com extensão
            tabela.Columns.Add("TipoArquivo", GetType(String))      ' PAR/PSM/ASM...
            tabela.Columns.Add("Qtde", GetType(Integer))            ' quantidade efetiva no Nivel
            tabela.Columns.Add("CaminhoCompleto", GetType(String))

            ' Propriedades de interesse como colunas próprias
            Dim propsInteressantes As New List(Of String) From {
            "Título", "Assunto", "Coment", "Tipo de Desenho", "Palavras-", "Autor", "Empresa",
            "Categoria", "Gerente", "Material", "Data", "DataR", "Data1",
            "Revision", "CutSizeX", "CutSizeY", "Thickness",
            "Mass", "Área_de_superfície", "Area_de_superficie", "Bloqueado"
        }
            For Each p In propsInteressantes
                If Not tabela.Columns.Contains(p) Then tabela.Columns.Add(p, GetType(String))
            Next

            ' Colunas de totais
            tabela.Columns.Add("RNC", GetType(String))
            tabela.Columns.Add("Fator", GetType(Double))
            tabela.Columns.Add("QtdeTotal", GetType(Double))
            tabela.Columns.Add("PesoTotal", GetType(Double))
            tabela.Columns.Add("AreaPinturaTotal", GetType(Double))

            ' Índice para agrupar por (Nivel|Arquivo)
            Dim indexPorChave As New Dictionary(Of String, DataRow)(StringComparer.OrdinalIgnoreCase)

            Dim totalLidos As Integer = 0
            Dim totalEstimado As Integer = 1

            '============================================================
            ' 🔹 Helpers
            '============================================================
            Dim LimparUnidades As Func(Of String, String) =
        Function(txt As String) As String
            If String.IsNullOrWhiteSpace(txt) Then Return ""
            Return txt _
                .Replace("mm²", "") _
                .Replace("MM²", "") _
                .Replace("mm", "") _
                .Replace("MM", "") _
                .Replace("^2", "") _
                .Replace("KG", "") _
                .Replace("kg", "") _
                .Trim()
        End Function

            Dim FormatarValor As Func(Of String, String, String) =
        Function(propNome As String, valorBruto As String) As String
            If String.IsNullOrWhiteSpace(valorBruto) Then Return ""
            Dim nome = propNome.Trim().ToUpperInvariant()
            Dim valor = LimparUnidades(valorBruto)

            ' Datas
            If nome = "DATA" OrElse nome = "DATAR" OrElse nome = "DATA1" Then
                Try : Return cl_BancoDados.FormatarData(valor) : Catch : Return valor : End Try
            End If

            ' Revisão
            If nome.Contains("REVISION") Then Return valor.ToUpperInvariant()

            ' CutSize / Espessura / Massa -> ponto decimal
            If nome.Contains("CUTSIZEX") Then Return valor.Replace(",", ".")
            If nome.Contains("CUTSIZEY") Then Return valor.Replace(",", ".")
            If nome.Contains("THICKNESS") Then Return valor.Replace(",", ".")
            If nome.Contains("MASS") Then Return valor.Replace(",", ".")

            ' Área mm² -> m²
            If nome.Contains("ÁREA") OrElse nome.Contains("AREA") Then
                Dim num As Double
                If Double.TryParse(valor.Replace(",", "."), NumberStyles.Any,
                                   CultureInfo.InvariantCulture, num) Then
                    Dim m2 = num / 1_000_000.0
                    Return m2.ToString("N4", CultureInfo.GetCultureInfo("pt-BR"))
                End If
            End If

            Return valor
        End Function

            ' ============================================================
            ' 🔹 PreencherPropriedades com retry + fechar docs abertos aqui
            ' ============================================================
            Dim PreencherPropriedades As Action(Of Object, DataRow) =
            Sub(docObj As Object, linha As DataRow)
                If linha Is Nothing Then Exit Sub

                Dim tentativas As Integer = 0
                Dim abriuTemp As Boolean = False   ' <-- controla se foi aberto aqui

                While tentativas < 2
                    Try
                        ' Se o objeto estiver Nothing (após erro), tenta reabrir pelo caminho da linha
                        If docObj Is Nothing Then
                            Dim caminhoArquivo As String = ""
                            Try
                                caminhoArquivo = Convert.ToString(linha("CaminhoCompleto"))
                            Catch
                                caminhoArquivo = ""
                            End Try

                            If Not String.IsNullOrWhiteSpace(caminhoArquivo) AndAlso IO.File.Exists(caminhoArquivo) Then
                                Try
                                    docObj = app.Documents.Open(caminhoArquivo)
                                    abriuTemp = True           ' <-- marcamos que abrimos aqui
                                Catch exOpen As Exception
                                    RegistrarErro($"Reabrindo documento '{caminhoArquivo}'", exOpen)
                                    Exit While
                                End Try
                            Else
                                ' Sem caminho válido, não há o que fazer
                                Exit While
                            End If
                        End If

                        ' --- CÓDIGO ORIGINAL DE LEITURA DAS PROPRIEDADES ---
                        Dim propSets As Object = docObj.Properties
                        For Each propSet As Object In propSets
                            For Each prop As Object In propSet
                                If prop Is Nothing Then Continue For

                                Dim nomeProp As String = ""
                                Try : nomeProp = CStr(prop.Name) : Catch : Continue For : End Try
                                If String.IsNullOrWhiteSpace(nomeProp) Then Continue For

                                ' 1) Preenche colunas de interesse
                                For Each alvo In propsInteressantes
                                    If nomeProp.IndexOf(alvo, StringComparison.OrdinalIgnoreCase) >= 0 Then
                                        Dim valorBruto As String = ""
                                        Try
                                            Dim v = prop.Value
                                            If v IsNot Nothing Then valorBruto = v.ToString()
                                        Catch ex As Exception
                                            valorBruto = "(erro)"
                                            RegistrarErro($"Lendo propriedade '{nomeProp}'", ex)
                                        End Try
                                        linha(alvo) = FormatarValor(nomeProp, valorBruto)
                                        Exit For
                                    End If
                                Next

                                ' 2) Mapa de espessura -> Thickness
                                If nomeProp.IndexOf("Material Thickness", StringComparison.OrdinalIgnoreCase) >= 0 _
                                   OrElse nomeProp.IndexOf("Espessura Do Material", StringComparison.OrdinalIgnoreCase) >= 0 _
                                   OrElse nomeProp.Equals("Thickness", StringComparison.OrdinalIgnoreCase) Then

                                    Dim valorBruto As String = ""
                                    Try
                                        Dim v = prop.Value
                                        If v IsNot Nothing Then valorBruto = v.ToString()
                                    Catch ex As Exception
                                        RegistrarErro($"Lendo espessura em '{nomeProp}'", ex)
                                    End Try

                                    If Not String.IsNullOrWhiteSpace(valorBruto) Then
                                        Dim limpo = LimparUnidades(valorBruto).Replace(",", ".")
                                        linha("Thickness") = limpo
                                    End If
                                End If

                            Next
                        Next
                        ' ---------------------------------------------------

                        Exit While ' sucesso, sai do loop

                    Catch comEx As System.Runtime.InteropServices.COMException
                        If comEx.ErrorCode = &H80010108 Then
                            ' RPC_E_DISCONNECTED → tenta reabrir e repetir
                            tentativas += 1
                            RegistrarErro("PreencherPropriedades - RPC_E_DISCONNECTED, tentando reabrir documento", comEx)
                            docObj = Nothing
                            Threading.Thread.Sleep(200)
                            Continue While
                        Else
                            RegistrarErro("PreencherPropriedades", comEx)
                            Exit While
                        End If

                    Catch ex As Exception
                        RegistrarErro("PreencherPropriedades", ex)
                        Exit While
                    End Try
                End While

                ' 🔻 Se abrimos o documento aqui, tentamos fechar no final
                If abriuTemp AndAlso docObj IsNot Nothing Then
                    Try
                        Dim isActive As Boolean = False
                        Try
                            isActive = (TryCast(docObj, Object) Is app.ActiveDocument)
                        Catch
                            isActive = False
                        End Try

                        If Not isActive Then
                            Try
                                CallByName(docObj, "Close", CallType.Method, False)
                            Catch ex As Exception
                                RegistrarErro("Fechando documento aberto em PreencherPropriedades", ex)
                            End Try
                        End If
                    Catch
                        ' não deixa o fechamento quebrar o fluxo
                    End Try
                End If
            End Sub
            ' ============================================================
            ' ============================================================

            Dim AdicionarOuSomarLinha As Action(Of Integer, String, String, Integer, String, Object) =
        Sub(nivel As Integer, nomeComExt As String, tipoArquivo As String, qtdeEfetiva As Integer, caminho As String, docObj As Object)
            Try
                Dim chave = $"{nivel}|{nomeComExt}".ToUpperInvariant()

                If Not indexPorChave.ContainsKey(chave) Then
                    Dim linha = tabela.NewRow()
                    linha("Nivel") = nivel
                    linha("Arquivo") = nomeComExt
                    linha("TipoArquivo") = tipoArquivo
                    linha("Qtde") = qtdeEfetiva
                    linha("CaminhoCompleto") = caminho
                    For Each p In propsInteressantes : linha(p) = "" : Next
                    linha("RNC") = "" : linha("Fator") = 1.0
                    linha("QtdeTotal") = CDbl(qtdeEfetiva)
                    linha("PesoTotal") = 0.0
                    linha("AreaPinturaTotal") = 0.0

                    PreencherPropriedades(docObj, linha)

                    Dim mass As Double = 0
                    Double.TryParse(Convert.ToString(linha("Mass")).Replace(",", "."),
                                    NumberStyles.Any, CultureInfo.InvariantCulture, mass)

                    Dim area As Double = 0
                    Double.TryParse(Convert.ToString(linha("Área_de_superfície")).Replace(",", "."),
                                    NumberStyles.Any, CultureInfo.InvariantCulture, area)

                    linha("PesoTotal") = mass * qtdeEfetiva
                    linha("AreaPinturaTotal") = area * qtdeEfetiva

                    tabela.Rows.Add(linha)
                    indexPorChave(chave) = linha

                Else
                    Dim linha = indexPorChave(chave)

                    Dim qtAnt As Integer = 0
                    Integer.TryParse(linha("Qtde").ToString(), qtAnt)
                    Dim qtNova = qtAnt + qtdeEfetiva
                    linha("Qtde") = qtNova
                    linha("QtdeTotal") = CDbl(qtNova)

                    Dim massUnit As Double = 0
                    Double.TryParse(Convert.ToString(linha("Mass")).Replace(",", "."),
                                    NumberStyles.Any, CultureInfo.InvariantCulture, massUnit)

                    Dim areaUnit As Double = 0
                    Double.TryParse(Convert.ToString(linha("Área_de_superfície")).Replace(",", "."),
                                    NumberStyles.Any, CultureInfo.InvariantCulture, areaUnit)

                    Dim pesoTot As Double = 0
                    Double.TryParse(linha("PesoTotal").ToString().Replace(",", "."),
                                    NumberStyles.Any, CultureInfo.InvariantCulture, pesoTot)

                    Dim areaTot As Double = 0
                    Double.TryParse(linha("AreaPinturaTotal").ToString().Replace(",", "."),
                                    NumberStyles.Any, CultureInfo.InvariantCulture, areaTot)

                    linha("PesoTotal") = pesoTot + (massUnit * qtdeEfetiva)
                    linha("AreaPinturaTotal") = areaTot + (areaUnit * qtdeEfetiva)
                End If
            Catch ex As Exception
                RegistrarErro($"AdicionarOuSomarLinha [{nomeComExt}]", ex)
            End Try
        End Sub

            '============================================================
            ' 🔹 Recursão (propaga multiplicador do pai)
            '============================================================
            Dim Ler As Action(Of Object, String, Integer, Integer) =
        Sub(docObj As Object, caminhoArquivo As String, nivel As Integer, multiplicadorPai As Integer)
            Try
                If docObj Is Nothing OrElse String.IsNullOrWhiteSpace(caminhoArquivo) Then Exit Sub

                totalLidos += 1
                lbl.Text = $"Lendo {IO.Path.GetFileName(caminhoArquivo)} ({totalLidos})"
                ''  System.Windows.Forms.Application.DoEvents()

                If totalEstimado > 1 Then
                    pgb.Style = ProgressBarStyle.Continuous
                    pgb.Value = Math.Min(100, CInt((totalLidos / Math.Max(1, totalEstimado)) * 100))
                End If

                Dim ext As String = IO.Path.GetExtension(caminhoArquivo).ToLower()
                Dim nomeComExt As String = IO.Path.GetFileName(caminhoArquivo)
                Dim tipoUpper As String = ext.Replace(".", "").ToUpperInvariant()

                ' Adiciona/Acumula a linha do próprio documento
                AdicionarOuSomarLinha(nivel, nomeComExt, tipoUpper, multiplicadorPai, caminhoArquivo, docObj)

                ' Apenas ASM tem filhos
                If ext <> ".asm" Then Exit Sub

                ' Ocorrências
                Dim occsObj As Object = Nothing
                Try
                    occsObj = CallByName(docObj, "Occurrences", CallType.Get)
                Catch ex As Exception
                    RegistrarErro($"Lendo Occurrences de {nomeComExt}", ex)
                    occsObj = Nothing
                End Try
                If occsObj Is Nothing Then Exit Sub

                Dim count As Integer = 0
                Try
                    count = CInt(CallByName(occsObj, "Count", CallType.Get))
                Catch ex As Exception
                    RegistrarErro($"Lendo Count de {nomeComExt}", ex)
                    count = 0
                End Try
                If count <= 0 Then Exit Sub

                totalEstimado += count

                ' Agrupa por arquivo dentro do Nivel
                Dim grupo As New Dictionary(Of String, (Caminho As String, QtdeLocal As Integer, DocObj As Object))(StringComparer.OrdinalIgnoreCase)

                For i As Integer = 1 To count
                    Try
                        Dim occ As Object = CallByName(occsObj, "Item", CallType.Method, i)

                        Dim subPath As String = ""
                        Try
                            subPath = CStr(CallByName(occ, "OccurrenceFileName", CallType.Get))
                        Catch ex As Exception
                            RegistrarErro($"Lendo OccurrenceFileName (#{i}) de {nomeComExt}", ex)
                            subPath = ""
                        End Try

                        Dim subDoc As Object = Nothing
                        Try
                            subDoc = CallByName(occ, "OccurrenceDocument", CallType.Get)
                        Catch ex As Exception
                            RegistrarErro($"Lendo OccurrenceDocument (#{i}) de {nomeComExt}", ex)
                            subDoc = Nothing
                        End Try

                        If subDoc Is Nothing AndAlso Not String.IsNullOrWhiteSpace(subPath) AndAlso IO.File.Exists(subPath) Then
                            Try
                                subDoc = app.Documents.Open(subPath)
                            Catch ex As Exception
                                RegistrarErro($"Abrindo subdocumento '{subPath}'", ex)
                                subDoc = Nothing
                            End Try
                        End If

                        Dim nomeAgrup As String = If(String.IsNullOrWhiteSpace(subPath), $"ITEM_{i}", IO.Path.GetFileName(subPath)).ToUpperInvariant()

                        Dim qLocal As Integer = 1
                        Try
                            Dim qtdProp As Object = CallByName(occ, "Quantity", CallType.Get)
                            If qtdProp IsNot Nothing Then
                                Dim qi As Integer
                                If Integer.TryParse(qtdProp.ToString(), qi) AndAlso qi > 0 Then qLocal = qi
                            End If
                        Catch ex As Exception
                            RegistrarErro($"Lendo Quantity (#{i}) de {nomeComExt}", ex)
                        End Try

                        If grupo.ContainsKey(nomeAgrup) Then
                            Dim t = grupo(nomeAgrup)
                            grupo(nomeAgrup) = (t.Caminho, t.QtdeLocal + qLocal, If(t.DocObj Is Nothing, subDoc, t.DocObj))
                        Else
                            grupo.Add(nomeAgrup, (If(String.IsNullOrWhiteSpace(subPath), "", subPath), qLocal, subDoc))
                        End If
                    Catch ex As Exception
                        RegistrarErro($"Processando ocorrência (#{i}) de {nomeComExt}", ex)
                    End Try
                Next

                For Each kv In grupo

                    Dim caminhoSub = kv.Value.Caminho
                    Dim qLocal = kv.Value.QtdeLocal
                    Dim qEfetivaFilho As Integer = qLocal * multiplicadorPai
                    Dim subDoc = kv.Value.DocObj

                    ' Se ainda não temos caminho mas temos documento, tenta pegar o FullName
                    If String.IsNullOrWhiteSpace(caminhoSub) AndAlso subDoc IsNot Nothing Then
                        Try
                            caminhoSub = CStr(CallByName(subDoc, "FullName", CallType.Get))
                        Catch ex As Exception
                            RegistrarErro("Lendo FullName de subdocumento", ex)
                            caminhoSub = ""
                        End Try
                    End If

                    ' Nome e tipo do filho (mesmo sem caminho)
                    Dim nomeFilho As String
                    Dim tipoFilho As String

                    If Not String.IsNullOrWhiteSpace(caminhoSub) Then
                        nomeFilho = IO.Path.GetFileName(caminhoSub)
                        tipoFilho = IO.Path.GetExtension(caminhoSub).Replace(".", "").ToUpperInvariant()
                    Else
                        ' fallback: usa a chave do grupo como nome
                        nomeFilho = kv.Key
                        tipoFilho = ""
                    End If

                    If subDoc IsNot Nothing AndAlso Not String.IsNullOrWhiteSpace(caminhoSub) Then
                        ' ✅ Caso NORMAL: temos o documento
                        '    → NÃO adiciona aqui, deixa o Ler(...) do filho fazer o AdicionarOuSomarLinha
                        Ler(subDoc, caminhoSub, nivel + 1, qEfetivaFilho)

                        ' Fecha se abrimos temporariamente
                        Try
                            Dim isActive As Boolean = False
                            Try : isActive = (TryCast(subDoc, Object) Is app.ActiveDocument) : Catch : End Try
                            If Not isActive AndAlso IO.File.Exists(caminhoSub) Then
                                Try : CallByName(subDoc, "Close", CallType.Method, False) : Catch : End Try
                            End If
                        Catch ex As Exception
                            RegistrarErro($"Fechando subdocumento '{caminhoSub}'", ex)
                        End Try

                    Else
                        ' ⚠ Caso EXCEÇÃO: não há documento carregado,
                        '    mas precisamos que a peça apareça na lista.
                        '    → adiciona a linha AQUI, uma única vez.
                        AdicionarOuSomarLinha(
                            nivel + 1,
                            nomeFilho,
                            tipoFilho,
                            qEfetivaFilho,
                            caminhoSub,
                            subDoc
                        )
                    End If

                Next

            Catch ex As Exception
                RegistrarErro($"Ler('{caminhoArquivo}')", ex)
            End Try
        End Sub

            '============================================================
            ' 🔹 Chamada inicial da recursão (multiplicador raiz = 1)
            '============================================================
            Ler(doc, doc.FullName, 0, 1)

            '============================================================
            ' 🔹 Limpeza de linhas “fantasmas” (sem arquivo/caminho)
            '============================================================
            For i As Integer = tabela.Rows.Count - 1 To 0 Step -1
                Dim arq As String = Convert.ToString(tabela.Rows(i)("Arquivo"))
                Dim cam As String = Convert.ToString(tabela.Rows(i)("CaminhoCompleto"))

                If String.IsNullOrWhiteSpace(arq) AndAlso String.IsNullOrWhiteSpace(cam) Then
                    tabela.Rows.RemoveAt(i)
                End If
            Next

            '============================================================
            ' 🔹 Cálculo dos TOTAIS
            '============================================================
            Dim totalQtdSolicitada As Double = 0
            Dim arquivosUnicos As New HashSet(Of String)(StringComparer.OrdinalIgnoreCase)

            For Each row As DataRow In tabela.Rows
                ' 1) Soma da QtdeTotal
                Dim qtd As Double
                Double.TryParse(
                Convert.ToString(row("QtdeTotal")).Replace(",", "."),
                NumberStyles.Any,
                CultureInfo.InvariantCulture,
                qtd
            )
                totalQtdSolicitada += qtd

                ' 2) Arquivo distinto
                Dim arq As String = Convert.ToString(row("Arquivo"))
                If Not String.IsNullOrWhiteSpace(arq) Then
                    arquivosUnicos.Add(arq)
                End If
            Next

            Dim totalPecasDiferentes As Integer = arquivosUnicos.Count
            Dim totalPecas As Integer = tabela.Rows.Count

            '============================================================
            ' 🔹 Bind no grid
            '============================================================
            dgv.DataSource = tabela
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells
            dgv.AllowUserToAddRows = False
            dgv.ColumnHeadersDefaultCellStyle.Font = New Font(dgv.Font, FontStyle.Bold)

            If dgv.Columns.Contains("Arquivo") Then dgv.Columns("Arquivo").Frozen = True

            tabelaOriginalBOM = tabela.Copy()

            Dim ocultar() As String = {
    "TipoArquivo", "CaminhoCompleto", "Categoria", "Gerente",
    "Autor", "Empresa", "Data", "DataR", "Data1",
    "Revision", "CutSizeX", "CutSizeY", "Mass",
    "Área_de_superfície", "Area_de_superficie", "Bloqueado",
    "RNC", "PesoTotal", "AreaPinturaTotal",
    "TipoLinha", "CodMatFabricante", "DescDetal", "QtdeMaterial", "PesoMaterial"
}


            For Each c In ocultar
                If dgv.Columns.Contains(c) Then dgv.Columns(c).Visible = False
            Next

            '============================================================
            ' 🔹 UI - final (incluindo os totais)
            '============================================================
            pgb.Style = ProgressBarStyle.Continuous
            pgb.Value = 100

            lblResumo.Text =
        "✅ Estrutura concluída." & vbCrLf &
        $"• Total de peças solicitadas (QtdeTotal): {totalQtdSolicitada}" & vbCrLf &
        $"• Total de peças diferentes (arquivos únicos): {totalPecasDiferentes}" & vbCrLf &
        $"• Total de peças na estrutura (linhas): {totalPecas}"

            '' System.Windows.Forms.Application.DoEvents()
            Threading.Thread.Sleep(550)

        Catch ex As Exception
            ' Erro geral do processo
            RegistrarErro("Erro geral em ListarEstruturaComTodasPropriedadesBlank", ex)

        Finally
            If app IsNot Nothing Then
                Try : app.DisplayAlerts = True : Catch : End Try
            End If
            pgb.Visible = False

            ' Se houve erros, mostra relatório organizado
            If erros IsNot Nothing AndAlso erros.Count > 0 Then
                Dim sb As New StringBuilder()
                sb.AppendLine("Foram encontrados " & erros.Count & " erro(s) durante a leitura da estrutura:")
                sb.AppendLine()

                Dim i As Integer = 1
                For Each msg In erros
                    sb.AppendLine(i.ToString("00") & " - " & msg)
                    i += 1
                Next

                If qtdRpcDesconectado > 0 Then
                    sb.AppendLine()
                    sb.AppendLine(qtdRpcDesconectado.ToString() &
                          " erro(s) são do tipo RPC_E_DISCONNECTED (necessário revisar/ajustar o arquivo no Solid Edge).")
                End If

                MessageBox.Show(sb.ToString(),
                        "Relatório de erros - Estrutura",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning)

            End If

        End Try

    End Sub

#End Region



#Region "VERSÃO DEFINITIVA - DADOS RESTAURADOS + CÁLCULOS CORRETOS"


    '================================================================================
    ' Função Global de Conversão Segura (Movemos para fora para ser compartilhada)
    '================================================================================
    Private Function ParseSeguro(ByVal v As Object) As Double
        Try
            If v Is Nothing OrElse IsDBNull(v) Then Return 0.0R
            Dim s As String = v.ToString().Trim()
            If s = "" Then Return 0.0R
            ' Força ponto como separador decimal para garantir o TryParse Invariant
            s = s.Replace(",", ".")
            Dim d As Double
            If Double.TryParse(s, Globalization.NumberStyles.Any, Globalization.CultureInfo.InvariantCulture, d) Then
                Return d
            End If
            Return 0.0R
        Catch
            Return 0.0R
        End Try
    End Function

    '================================================================================
    ' Monta estrutura BOM + MATERIAIS no MESMO DataGridView
    '================================================================================
    Public Sub ListarEstruturaBomMateriais(ByVal dgv As DataGridView,
                                           ByVal pgb As ProgressBar,
                                           ByVal lbl As Label,
                                           ByVal lblResumo As Label)

        Dim erros As New List(Of String)
        Dim qtdRpcDesconectado As Integer = 0

        ' Helper para registrar erros
        Dim RegistrarErro As Action(Of String, Exception) =
        Sub(contexto As String, ex As Exception)
            Try
                If ex Is Nothing Then Return
                Dim msg As String = $"{contexto}: {ex.Message}"
                If ex.Message.Contains("RPC_E_DISCONNECTED") OrElse ex.HResult = &H80010108 Then
                    qtdRpcDesconectado += 1
                    msg = $"{contexto}: [Solid Edge Desconectado] {ex.Message}"
                End If
                erros.Add(msg)
            Catch
            End Try
        End Sub

        ' Helper de Parse Decimal Local
        Dim ParseDecimalFlex As Func(Of Object, Decimal) =
        Function(v As Object) As Decimal
            Try
                If v Is Nothing OrElse IsDBNull(v) Then Return 0D
                Dim s As String = v.ToString().Trim()
                If s = "" Then Return 0D
                s = s.Replace(",", ".")
                Dim d As Decimal
                If Decimal.TryParse(s, Globalization.NumberStyles.Any, Globalization.CultureInfo.InvariantCulture, d) Then
                    Return d
                End If
                Return 0D
            Catch
                Return 0D
            End Try
        End Function

        ' Declarado fora do Try para ser acessível no Finally
        Dim cacheDocsAbertos As New Dictionary(Of String, Object)(StringComparer.OrdinalIgnoreCase)

        Try
            ' 1. Conexão com Solid Edge
            If app Is Nothing Then
                Try
                    app = System.Runtime.InteropServices.Marshal.GetActiveObject("SolidEdge.Application")
                Catch
                End Try
            End If

            Dim doc As Object = Nothing
            Try
                If app Is Nothing OrElse app.Documents.Count = 0 Then
                    MessageBox.Show("Nenhum documento aberto no Solid Edge.", "SINCO", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    Exit Sub
                End If
                doc = app.ActiveDocument
            Catch ex As Exception
                MessageBox.Show("Erro conexão SE: " & ex.Message)
                Exit Sub
            End Try

            If doc Is Nothing Then Exit Sub

            If app IsNot Nothing Then
                Try : app.DisplayAlerts = False : Catch : End Try
            End If

            ' 2. UI
            pgb.Visible = True
            pgb.Style = ProgressBarStyle.Marquee
            pgb.Value = 0
            lbl.Visible = True
            lbl.Text = "Lendo estrutura..."

            ' 3. Tabela
            Dim tabela As New DataTable("BOM_Completa")
            tabela.Columns.Add("Nivel", GetType(Integer))
            tabela.Columns.Add("Arquivo", GetType(String))
            tabela.Columns.Add("TipoArquivo", GetType(String))
            tabela.Columns.Add("Qtde", GetType(Double))             ' Unitário
            tabela.Columns.Add("CaminhoCompleto", GetType(String))
            tabela.Columns.Add("TipoLinha", GetType(String))

            Dim propsInteressantes As New List(Of String) From {
                "Título", "Assunto", "Coment", "Tipo de Desenho", "Palavras-", "Autor", "Empresa",
                "Categoria", "Gerente", "Material", "Data", "DataR", "Data1",
                "Revision", "CutSizeX", "CutSizeY", "Thickness",
                "Mass", "Área_de_superfície", "Area_de_superficie", "Bloqueado"
            }

            For Each p In propsInteressantes
                If Not tabela.Columns.Contains(p) Then
                    tabela.Columns.Add(p, GetType(String))
                End If
            Next

            tabela.Columns.Add("RNC", GetType(String))
            tabela.Columns.Add("Fator", GetType(Double))
            tabela.Columns.Add("QtdeTotal", GetType(Double))        ' Acumulado
            tabela.Columns.Add("PesoTotal", GetType(Double))
            tabela.Columns.Add("AreaPinturaTotal", GetType(Double))
            tabela.Columns.Add("CodMatFabricante", GetType(String))
            tabela.Columns.Add("DescDetal", GetType(String))
            tabela.Columns.Add("QtdeMaterial", GetType(Double))
            tabela.Columns.Add("PesoMaterial", GetType(Double))
            '   tabela.Columns.Add("NovoRevisao", GetType(Double))

            ' ═══ VINCULA O DATASOURCE AGORA — linhas aparecem em tempo real ═══
            dgv.DataSource = tabela
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells

            ' Oculta colunas internas imediatamente
            Dim ocultar() As String = {"TipoArquivo", "CaminhoCompleto", "Categoria", "Gerente", "Autor", "Empresa", "Data", "DataR", "Data1", "Revision", "CutSizeX", "CutSizeY", "Mass", "Área_de_superfície", "Area_de_superficie", "Bloqueado", "RNC", "PesoTotal", "AreaPinturaTotal", "QtdeMaterial", "PesoMaterial", "CodMatFabricante", "DescDetal"}
            For Each c In ocultar
                If dgv.Columns.Contains(c) Then
                    dgv.Columns(c).Visible = False
                End If
            Next
            System.Windows.Forms.Application.DoEvents()

            ' ═══ OTIMIZAÇÃO 2: Batch SQL para processos ═══
            ' Carrega TODOS os processos em UMA query, filtra na memória
            Dim dtTodosProcessos As DataTable = Nothing
            Try
                dtTodosProcessos = cl_BancoDados.CarregarDados(
                    "SELECT mp.codmatfabricante, mp.IdProcesso, p.processofabricacao AS processofabricacao, mp.SequenciaExecucao " &
                    "FROM material_processo mp " &
                    "LEFT JOIN processofabricacao p ON p.idprocessofabricacao = mp.IdProcesso " &
                    "ORDER BY mp.codmatfabricante, mp.SequenciaExecucao")
            Catch
            End Try

            Dim totalLidos As Integer = 0
            Dim totalEstimado As Integer = 1

            ' Helper Formatação Original
            Dim LimparUnidades As Func(Of String, String) =
            Function(txt)
                If String.IsNullOrWhiteSpace(txt) Then Return ""
                Return txt.Replace("mm²", "").Replace("MM²", "").Replace("mm", "").Replace("MM", "").Replace("^2", "").Replace("KG", "").Replace("kg", "").Trim()
            End Function

            Dim FormatarValor As Func(Of String, String, String) =
            Function(propNome As String, valorBruto As String) As String
                If String.IsNullOrWhiteSpace(valorBruto) Then Return ""
                Dim nome = propNome.Trim().ToUpperInvariant()
                Dim valor = LimparUnidades(valorBruto)

                If nome = "DATA" OrElse nome = "DATAR" OrElse nome = "DATA1" Then
                    Try
                        Return cl_BancoDados.FormatarData(valor)
                    Catch
                        Return valor
                    End Try
                End If
                If nome.Contains("REVISION") Then Return valor.ToUpperInvariant()
                If nome.Contains("CUTSIZEX") OrElse nome.Contains("CUTSIZEY") OrElse nome.Contains("THICKNESS") OrElse nome.Contains("MASS") Then
                    Return valor.Replace(",", ".")
                End If
                If nome.Contains("ÁREA") OrElse nome.Contains("AREA") Then
                    Dim num As Double
                    If Double.TryParse(valor.Replace(",", "."), Globalization.NumberStyles.Any, Globalization.CultureInfo.InvariantCulture, num) Then
                        Dim m2 = num / 1_000_000.0R
                        Return m2.ToString("N4", Globalization.CultureInfo.GetCultureInfo("pt-BR"))
                    End If
                End If
                Return valor
            End Function

            ' ====================================================================
            ' PreencherPropriedades (RESTAURADA A ORIGINAL QUE LÊ TUDO)
            ' ====================================================================
            Dim PreencherPropriedades As Action(Of Object, DataRow) =
            Sub(docObjP, linhaP)
                If linhaP Is Nothing Then Exit Sub
                Dim tentativas As Integer = 0
                Dim abriuTemp As Boolean = False

                While tentativas < 2
                    Try
                        If docObjP Is Nothing Then
                            Dim caminhoArquivo As String = ""
                            Try
                                caminhoArquivo = Convert.ToString(linhaP("CaminhoCompleto"))
                            Catch
                            End Try

                            If Not String.IsNullOrWhiteSpace(caminhoArquivo) Then
                                ' Usa cache — evita reabrir doc já carregado
                                If cacheDocsAbertos.ContainsKey(caminhoArquivo) Then
                                    docObjP = cacheDocsAbertos(caminhoArquivo)
                                    abriuTemp = False
                                ElseIf IO.File.Exists(caminhoArquivo) Then
                                    Try
                                        docObjP = app.Documents.Open(caminhoArquivo)
                                        cacheDocsAbertos(caminhoArquivo) = docObjP
                                        abriuTemp = False ' cache gerencia o fechamento
                                    Catch exOpen As Exception
                                        RegistrarErro($"Reabrindo doc '{caminhoArquivo}'", exOpen)
                                        Exit While
                                    End Try
                                Else
                                    Exit While
                                End If
                            Else
                                Exit While
                            End If
                        End If

                        ' --- LOOP ORIGINAL RESTAURADO ---
                        Dim propSets As Object = docObjP.Properties
                        For Each propSet As Object In propSets
                            For Each prop As Object In propSet
                                If prop Is Nothing Then Continue For

                                Dim nomeProp As String = ""
                                Try
                                    nomeProp = CStr(prop.Name)
                                Catch
                                    Continue For
                                End Try

                                If String.IsNullOrWhiteSpace(nomeProp) Then Continue For

                                ' Loop pelas propriedades de interesse
                                For Each alvo In propsInteressantes
                                    If nomeProp.IndexOf(alvo, StringComparison.OrdinalIgnoreCase) >= 0 Then
                                        Dim valorBruto As String = ""
                                        Try
                                            Dim v = prop.Value
                                            If v IsNot Nothing Then valorBruto = v.ToString()
                                        Catch ex As Exception
                                            valorBruto = "(erro)"
                                            RegistrarErro($"Lendo propriedade '{nomeProp}'", ex)
                                        End Try

                                        ' Aplica formatação
                                        linhaP(alvo) = FormatarValor(nomeProp, valorBruto)
                                        Exit For
                                    End If
                                Next

                                ' Loop específico para Espessura
                                If nomeProp.IndexOf("Material Thickness", StringComparison.OrdinalIgnoreCase) >= 0 OrElse
                                   nomeProp.IndexOf("Espessura Do Material", StringComparison.OrdinalIgnoreCase) >= 0 OrElse
                                   nomeProp.Equals("Thickness", StringComparison.OrdinalIgnoreCase) Then

                                    Dim valorBruto As String = ""
                                    Try
                                        Dim v = prop.Value
                                        If v IsNot Nothing Then valorBruto = v.ToString()
                                    Catch ex As Exception
                                        RegistrarErro($"Lendo espessura em '{nomeProp}'", ex)
                                    End Try

                                    If Not String.IsNullOrWhiteSpace(valorBruto) Then
                                        linhaP("Thickness") = LimparUnidades(valorBruto).Replace(",", ".")
                                    End If
                                End If
                            Next
                        Next
                        ' -------------------------------

                        Exit While
                    Catch comEx As System.Runtime.InteropServices.COMException
                        If comEx.ErrorCode = &H80010108 Then
                            tentativas += 1
                            docObjP = Nothing
                            Threading.Thread.Sleep(200)
                            Continue While
                        Else
                            RegistrarErro("PreencherPropriedades COM", comEx)
                            Exit While
                        End If
                    Catch ex As Exception
                        RegistrarErro("PreencherPropriedades", ex)
                        Exit While
                    End Try
                End While

                ' Cache gerencia o fechamento — não fecha aqui
            End Sub

            ' ====================================================================
            ' Adicionar Processos da Peça (OTIMIZADO — filtra do batch em memória)
            ' ====================================================================
            Dim AdicionarProcessos As Action(Of Integer, String, Double) =
            Sub(nivelProc, codPecaProc, qTotProc)
                Try
                    Dim nomeLimpo As String = IO.Path.GetFileNameWithoutExtension(codPecaProc)

                    ' Filtra do batch em memória (sem SQL individual)
                    Dim linhasProc() As DataRow = Nothing
                    If dtTodosProcessos IsNot Nothing Then
                        Try
                            linhasProc = dtTodosProcessos.Select("codmatfabricante = '" & nomeLimpo.Replace("'", "''") & "'")
                        Catch
                        End Try
                    End If

                    If linhasProc IsNot Nothing AndAlso linhasProc.Length > 0 Then
                        For Each linhaProc As DataRow In linhasProc
                            Dim nomeProc As String = ""
                            Try : nomeProc = Convert.ToString(linhaProc("processofabricacao")) : Catch : End Try

                            Dim seqProc As String = ""
                            Try : seqProc = Convert.ToString(linhaProc("SequenciaExecucao")) : Catch : End Try

                            Dim drProc As DataRow = tabela.NewRow()
                            drProc("TipoLinha") = "PROCESSO"
                            drProc("Nivel") = nivelProc

                            Dim finalName As String = "[" & seqProc & "] " & nomeProc
                            drProc("Arquivo") = finalName
                            drProc("Título") = finalName
                            drProc("TipoArquivo") = "PROC"
                            drProc("CaminhoCompleto") = ""

                            For Each pCol In propsInteressantes
                                If drProc.Table.Columns.Contains(pCol) Then
                                    drProc(pCol) = ""
                                End If
                            Next

                            drProc("Tipo de Desenho") = "PROCESSO"
                            drProc("Qtde") = 0.0R
                            drProc("QtdeTotal") = qTotProc
                            drProc("Fator") = 1.0R
                            drProc("RNC") = ""
                            drProc("PesoTotal") = 0.0R
                            drProc("AreaPinturaTotal") = 0.0R
                            drProc("PesoMaterial") = 0.0R
                            drProc("QtdeMaterial") = 0.0R
                            drProc("CodMatFabricante") = ""
                            drProc("DescDetal") = ""

                            tabela.Rows.Add(drProc)
                        Next
                    End If
                Catch ex As Exception
                    RegistrarErro("Erro Add Processos " & codPecaProc, ex)
                End Try
            End Sub

            ' ====================================================================
            ' AdicionarPeca
            ' ====================================================================
            Dim AdicionarPeca As Action(Of Integer, String, String, Double, Double, String, Object) =
            Sub(nivel, nome, tipo, qLocal, qTotal, caminho, objDoc)
                Try
                    Dim linha = tabela.NewRow()
                    linha("TipoLinha") = "PECA"
                    linha("Nivel") = nivel
                    linha("Arquivo") = nome
                    linha("TipoArquivo") = tipo

                    linha("Qtde") = qLocal
                    linha("QtdeTotal") = qTotal

                    linha("CaminhoCompleto") = caminho
                    For Each p In propsInteressantes
                        linha(p) = ""
                    Next
                    linha("Fator") = 1.0R
                    linha("RNC") = ""
                    linha("PesoTotal") = 0.0R
                    linha("AreaPinturaTotal") = 0.0R
                    linha("CodMatFabricante") = ""
                    linha("DescDetal") = ""
                    linha("QtdeMaterial") = 0.0R
                    linha("PesoMaterial") = 0.0R

                    ' Aqui lê os dados (Título, etc)
                    PreencherPropriedades(objDoc, linha)

                    ' Recalcula Peso/Área com os dados que acabaram de ser lidos
                    Dim m As Double = ParseDecimalFlex(linha("Mass"))
                    Dim a As Double = ParseDecimalFlex(linha("Área_de_superfície"))
                    linha("PesoTotal") = m * qTotal
                    linha("AreaPinturaTotal") = a * qTotal
                    '   linha("NovoRevisao") = ""

                    tabela.Rows.Add(linha)

                    Try
                        LerDadosViewMontaPecaParaTabela(tabela, nivel, nome, qTotal)
                    Catch ex As Exception
                        RegistrarErro("Erro Materiais " & nome, ex)
                    End Try

                    Try
                        AdicionarProcessos(nivel + 1, nome, qTotal)
                    Catch ex As Exception
                        RegistrarErro("Erro Processos " & nome, ex)
                    End Try

                Catch ex As Exception
                    RegistrarErro("AdicionarPeca " & nome, ex)
                End Try
            End Sub

            ' ====================================================================
            ' Recursão Ler
            ' ====================================================================
            Dim Ler As Action(Of Object, String, Integer, Double, Double) =
            Sub(obj, path, niv, qLoc, qTot)
                Try
                    If obj Is Nothing Then Exit Sub
                    totalLidos += 1
                    lbl.Text = "Lendo: " & IO.Path.GetFileName(path)
                    ' OTIMIZAÇÃO 3: DoEvents a cada 3 peças (menos overhead de message pump)
                    If totalLidos Mod 3 = 0 Then System.Windows.Forms.Application.DoEvents()

                    Dim ext As String = IO.Path.GetExtension(path).ToLower()
                    Dim nome As String = IO.Path.GetFileName(path)
                    Dim tipo As String = ext.Replace(".", "").ToUpper()

                    AdicionarPeca(niv, nome, tipo, qLoc, qTot, path, obj)

                    If ext <> ".asm" Then Exit Sub

                    Dim occs As Object = Nothing
                    Try
                        occs = CallByName(obj, "Occurrences", CallType.Get)
                    Catch
                    End Try
                    If occs Is Nothing Then Exit Sub

                    Dim count As Integer = 0
                    Try
                        count = CInt(CallByName(occs, "Count", CallType.Get))
                    Catch
                    End Try
                    totalEstimado += count

                    Dim grupo As New Dictionary(Of String, (Caminho As String, Qtde As Double, Obj As Object))

                    For i As Integer = 1 To count
                        Try
                            Dim occ = CallByName(occs, "Item", CallType.Method, i)
                            Dim subPath As String = ""
                            Try
                                subPath = CStr(CallByName(occ, "OccurrenceFileName", CallType.Get))
                            Catch
                            End Try

                            Dim subDoc As Object = Nothing
                            Try
                                subDoc = CallByName(occ, "OccurrenceDocument", CallType.Get)
                            Catch
                            End Try

                            ' Usa cache para evitar abrir o mesmo doc múltiplas vezes
                            If subDoc Is Nothing AndAlso Not String.IsNullOrEmpty(subPath) Then
                                If cacheDocsAbertos.ContainsKey(subPath) Then
                                    subDoc = cacheDocsAbertos(subPath)
                                ElseIf IO.File.Exists(subPath) Then
                                    Try
                                        subDoc = app.Documents.Open(subPath)
                                        cacheDocsAbertos(subPath) = subDoc
                                    Catch
                                    End Try
                                End If
                            End If

                            Dim qItem As Double = 1.0
                            Try
                                qItem = ParseDecimalFlex(CallByName(occ, "Quantity", CallType.Get))
                            Catch
                            End Try

                            If qItem <= 0 Then qItem = 1.0

                            Dim key As String = ""
                            If String.IsNullOrEmpty(subPath) Then
                                key = "ITEM" & i
                            Else
                                key = subPath
                            End If

                            If grupo.ContainsKey(key) Then
                                Dim t = grupo(key)
                                grupo(key) = (t.Caminho, t.Qtde + qItem, If(t.Obj Is Nothing, subDoc, t.Obj))
                            Else
                                grupo.Add(key, (subPath, qItem, subDoc))
                            End If
                        Catch
                        End Try
                    Next

                    For Each kv In grupo
                        Dim qFilhoLocal = kv.Value.Qtde
                        Dim qFilhoTotal = qTot * qFilhoLocal

                        Dim sPath = kv.Value.Caminho
                        Dim sObj = kv.Value.Obj

                        If sObj IsNot Nothing Then
                            Ler(sObj, sPath, niv + 1, qFilhoLocal, qFilhoTotal)
                            ' NÃO fecha aqui — o cache gerencia o fechamento no Finally
                        Else
                            Dim sNome = IO.Path.GetFileName(sPath)
                            Dim sTipo = IO.Path.GetExtension(sPath).Replace(".", "").ToUpper()
                            AdicionarPeca(niv + 1, sNome, sTipo, qFilhoLocal, qFilhoTotal, sPath, Nothing)
                        End If
                    Next
                Catch ex As Exception
                    RegistrarErro("Recursão " & path, ex)
                End Try
            End Sub

            Ler(doc, doc.FullName, 0, 1.0R, 1.0R)

            ' Limpeza e Totais
            Dim qtdTotalGeral As Double = 0
            For i As Integer = tabela.Rows.Count - 1 To 0 Step -1
                Dim row = tabela.Rows(i)
                Dim arq As String = row("Arquivo").ToString()

                If row("TipoLinha") = "PECA" AndAlso String.IsNullOrEmpty(arq) Then
                    tabela.Rows.RemoveAt(i)
                ElseIf row("TipoLinha") = "PECA" Then
                    qtdTotalGeral += CDbl(row("QtdeTotal"))
                End If
            Next

            ' Re-vincula após limpeza para atualizar o grid
            dgv.DataSource = Nothing
            dgv.DataSource = tabela

            ' Re-oculta colunas internas (DataSource rebind pode reexibir)
            For Each c In ocultar
                If dgv.Columns.Contains(c) Then
                    dgv.Columns(c).Visible = False
                End If
            Next

            lblResumo.Text = $"Total de Peças: {qtdTotalGeral}"
            pgb.Visible = False
            System.Windows.Forms.Application.DoEvents()

        Catch ex As Exception
            MessageBox.Show("Erro Fatal: " & ex.Message)
        Finally
            ' ═══ Fecha todos os documentos do cache ═══
            For Each kvp In cacheDocsAbertos
                Try
                    Dim isActive As Boolean = False
                    Try : isActive = (TryCast(kvp.Value, Object) Is app.ActiveDocument) : Catch : End Try
                    If Not isActive Then
                        CallByName(kvp.Value, "Close", CallType.Method, False)
                    End If
                Catch
                End Try
            Next
            cacheDocsAbertos.Clear()

            If app IsNot Nothing Then
                Try : app.DisplayAlerts = True : Catch : End Try
            End If
        End Try
    End Sub

    '===============================================================================
    ' LÊ MATERIAIS PARA TABELA - CORREÇÃO VISUAL DA COLUNA QTDE
    '===============================================================================
    Public Sub LerDadosViewMontaPecaParaTabela(tabelaDest As DataTable,
                                               nivelPeca As Integer,
                                               nomeArquivoComExt As String,
                                               qtdeTotalAcumuladaPeca As Double)

        If TabelaViewMontaPeca Is Nothing Then Exit Sub

        Dim chaveArquivo As String = IO.Path.GetFileName(nomeArquivoComExt)

        TabelaViewMontaPeca.CaseSensitive = False
        Dim dv As New DataView(TabelaViewMontaPeca)
        Dim filtro As String = chaveArquivo.Replace("'", "''")
        dv.RowFilter = $"NomeArquivoSemExtensao = '{filtro}'"

        For Each linha As DataRow In dv.ToTable().Rows
            Try
                If Not String.Equals(linha("NomeArquivoSemExtensao").ToString().Trim(), chaveArquivo.Trim(), StringComparison.OrdinalIgnoreCase) Then
                    Continue For
                End If

                iconeTipoArquivo = My.Resources.sinco_Diversos

                ' 1) PARSE (Lê os dados do banco)
                Dim PecaQtde As Double = ParseSeguro(linha("PecaQtde"))
                Dim peso As Double = ParseSeguro(linha("Peso"))

                ' 2) CÁLCULO DO MATERIAL
                ' QtdeMaterialUnitario = Quanto gasta em 1 peça (ex: 0,125)
                ' QtdeMaterialTotal    = Quanto gasta em TODAS as peças (ex: 0,125 * 4 = 0,5)
                Dim QtdeMaterialUnitario As Double = PecaQtde
                Dim QtdeMaterialTotal As Double = QtdeMaterialUnitario * qtdeTotalAcumuladaPeca

                Dim PesoMaterialTotal As Double = peso * QtdeMaterialTotal

                ' 3) PREENCHE A LINHA
                Dim drMat As DataRow = tabelaDest.NewRow()
                drMat("TipoLinha") = "MATERIAL"
                drMat("Nivel") = nivelPeca + 1

                Dim cod As String = linha("CodMatFabricante").ToString().Trim().ToUpper()
                drMat("Arquivo") = cod
                drMat("CodMatFabricante") = cod
                drMat("TipoArquivo") = "MAT"

                ' -----------------------------------------------------------------------
                ' 🔴 CORREÇÃO AQUI:
                ' Antes estava: drMat("Qtde") = QtdeMaterialUnitario  (Mostrava 0,125)
                ' Agora fica:   drMat("Qtde") = QtdeMaterialTotal     (Mostra 0,5)
                ' Isso faz com que a coluna visual 'Qtde' já mostre o valor multiplicado pela quantidade da peça pai.
                ' -----------------------------------------------------------------------
                drMat("Qtde") = QtdeMaterialTotal

                ' Coluna de totais (mantém igual para cálculo)
                drMat("QtdeTotal") = QtdeMaterialTotal

                ' Guarda o unitário apenas na coluna interna se precisar depois
                drMat("QtdeMaterial") = QtdeMaterialUnitario
                drMat("CaminhoCompleto") = ""

                Dim desc As String = linha("DescDetal").ToString().Trim().ToUpper()
                Dim matView As String = ""
                Try : matView = linha("Material").ToString().Trim().ToUpper() : Catch : End Try

                Dim titulo As String = desc
                If matView <> "" Then titulo &= " - " & matView
                drMat("Título") = titulo
                If matView <> "" Then drMat("Material") = matView
                drMat("Tipo de Desenho") = "MATERIAL"
                drMat("Fator") = 1.0R
                drMat("PesoTotal") = 0.0R
                drMat("AreaPinturaTotal") = 0.0R
                drMat("PesoMaterial") = PesoMaterialTotal

                tabelaDest.Rows.Add(drMat)
            Catch
                Continue For
            End Try
        Next
    End Sub

#End Region

    Public Sub ExportarPorGrid(dgv As DataGridView, chkDXF As CheckBox, chkPDF As CheckBox, chkIGES As CheckBox, BarraProgresso As ProgressBar, lbl As Label, chkOpcaodePasta As CheckBox)

        ' Evita processar o mesmo arquivo mais de uma vez nesta execução
        Dim arquivosProcessados As New HashSet(Of String)(StringComparer.OrdinalIgnoreCase)
        Dim errosLote As New List(Of String)()

        Try
            If dgv.Rows.Count = 0 Then
                MessageBox.Show("Nenhum item na lista de peças.", "SINCO - Solid Edge", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Exit Sub
            End If

            '──────────────────────────────────────────────
            ' 📁 Pasta de destino (pergunta UMA VEZ antes do loop)
            '──────────────────────────────────────────────
            Dim pastaDestinoLote As String = ""
            If chkOpcaodePasta.Checked = False Then
                Using fbd As New FolderBrowserDialog()
                    fbd.Description = "Selecione a pasta de destino para salvar TODOS os arquivos exportados:"
                    If fbd.ShowDialog() = DialogResult.OK Then
                        pastaDestinoLote = fbd.SelectedPath
                    Else
                        Exit Sub
                    End If
                End Using
            End If

            '──────────────────────────────────────────────
            ' 🔹 Configurações iniciais
            '──────────────────────────────────────────────
            Dim total As Integer = dgv.Rows.Count
            Dim processados As Integer = 0
            Dim exportadosDXF As Integer = 0
            Dim exportadosPDF As Integer = 0
            Dim primeiroArquivo As String = ""

            Try : primeiroArquivo = dgv.Rows(0).Cells("CaminhoCompleto").Value?.ToString()?.Trim() : Catch : End Try

            app.DisplayAlerts = False

            ' ── Garante Style Continuous (pode ter ficado em Marquee da leitura anterior) ──
            BarraProgresso.Style = ProgressBarStyle.Continuous
            BarraProgresso.Minimum = 0
            BarraProgresso.Maximum = total
            BarraProgresso.Value = 0
            BarraProgresso.Visible = True
            BarraProgresso.Refresh()
            System.Windows.Forms.Application.DoEvents()

            '──────────────────────────────────────────────
            ' 🔁 Loop principal — um arquivo por vez
            '──────────────────────────────────────────────
            For Each row As DataGridViewRow In dgv.Rows
                Dim caminhoAtual As String = ""
                Try
                    '── Lê campos da linha ──
                    Dim Bloqueado As String = row.Cells("Bloqueado").Value?.ToString()?.Trim().ToUpperInvariant()
                    If Bloqueado = "SIM" Then
                        Try : row.Cells("Bloqueado").Style.BackColor = Color.LightGreen : Catch : End Try
                        Continue For
                    End If

                    caminhoAtual = row.Cells("CaminhoCompleto").Value?.ToString()?.Trim()
                    Dim txttipodesenho As String = row.Cells("Tipo de Desenho").Value?.ToString()?.Trim()

                    If String.IsNullOrWhiteSpace(caminhoAtual) OrElse Not File.Exists(caminhoAtual) Then
                        Continue For
                    End If

                    Dim extensao As String = IO.Path.GetExtension(caminhoAtual).ToLower()
                    Dim nomeArquivo As String = IO.Path.GetFileNameWithoutExtension(caminhoAtual)
                    Dim pastaOriginal As String = IO.Path.GetDirectoryName(caminhoAtual)

                    ' Pasta de saída: mesma do arquivo (chkOpcaodePasta=True) ou pasta escolhida
                    Dim pastaDestinoFinal As String = If(chkOpcaodePasta.Checked, pastaOriginal, pastaDestinoLote)

                    Dim caminhoFull As String = IO.Path.GetFullPath(caminhoAtual)

                    '── Já processado? Atualiza ícone e pula ──
                    If arquivosProcessados.Contains(caminhoFull) Then
                        Try
                            If chkDXF.Checked Then
                                row.Cells("dgvdxf").Value = If(File.Exists(IO.Path.Combine(pastaDestinoFinal, nomeArquivo & ".dxf")), My.Resources.dxf, My.Resources.sem_icone)
                            End If
                            If chkPDF.Checked Then
                                row.Cells("dgvpdf").Value = If(File.Exists(IO.Path.Combine(pastaDestinoFinal, nomeArquivo & ".pdf")), My.Resources.pdf, My.Resources.sem_icone)
                            End If
                        Catch
                        End Try
                        Continue For
                    End If
                    arquivosProcessados.Add(caminhoFull)

                    '── Fecha todos os documentos abertos no SE ──
                    Try
                        For Each d As Object In app.Documents
                            Try : d.Close(False) : Catch : End Try
                        Next
                    Catch
                    End Try

                    '── Atualiza status — renderiza ANTES do Open bloqueante ──
                    lbl.Text = "[" & (processados + 1).ToString() & "/" & total.ToString() & "] Abrindo: " & nomeArquivo & extensao & "..."
                    lbl.Refresh()
                    ' Muda para Marquee durante o Open (mostra atividade enquanto o SE carrega)
                    BarraProgresso.Style = ProgressBarStyle.Marquee
                    BarraProgresso.MarqueeAnimationSpeed = 25
                    BarraProgresso.Refresh()
                    System.Windows.Forms.Application.DoEvents()

                    '── Abre o arquivo 3D no Solid Edge ──
                    Dim doc As Object = Nothing
                    Try
                        doc = app.Documents.Open(caminhoAtual)
                        doc.Activate()
                        app.DoIdle()
                        Threading.Thread.Sleep(300)
                    Catch exAbrir As Exception
                        errosLote.Add("Falha ao abrir '" & nomeArquivo & "': " & exAbrir.Message)
                        BarraProgresso.Style = ProgressBarStyle.Continuous
                        Continue For
                    End Try

                    ' Volta para Continuous após o Open terminar
                    BarraProgresso.Style = ProgressBarStyle.Continuous
                    BarraProgresso.Refresh()

                    '──────────────────────────────────────────────
                    ' 🔹 Exportar DXF (apenas .psm — chapa metálica)
                    '──────────────────────────────────────────────
                    If chkDXF.Checked Then
                        Select Case extensao
                            Case ".psm"
                                Try
                                    Dim dxfDestino As String = IO.Path.Combine(pastaDestinoFinal, nomeArquivo & ".dxf")
                                    lbl.Text = $"DXF: {nomeArquivo}"
                                    System.Windows.Forms.Application.DoEvents()

                                    ExportarDXFPlanificadoAuto(dxfDestino)

                                    Dim tentativas As Integer = 0
                                    Do Until File.Exists(dxfDestino) OrElse tentativas > 20
                                        Threading.Thread.Sleep(250)
                                        tentativas += 1
                                    Loop

                                    If File.Exists(dxfDestino) Then
                                        row.Cells("dgvdxf").Value = My.Resources.dxf
                                        exportadosDXF += 1
                                    Else
                                        row.Cells("dgvdxf").Value = My.Resources.sem_icone
                                        errosLote.Add($"⚠️ DXF não gerado para '{nomeArquivo}' (planificação pode não existir)")
                                    End If
                                Catch exDxf As Exception
                                    row.Cells("dgvdxf").Value = My.Resources.sem_icone
                                    errosLote.Add($"❌ Erro DXF '{nomeArquivo}': {exDxf.Message}")
                                End Try

                            Case ".par"
                                ' .PAR = peça sólida — tenta gerar DXF blank (SaveAsFlatDXFEx)
                                Try
                                    Dim dxfDestino As String = IO.Path.Combine(pastaDestinoFinal, nomeArquivo & ".dxf")
                                    lbl.Text = $"DXF: {nomeArquivo}"
                                    System.Windows.Forms.Application.DoEvents()

                                    ExportarDXFPlanificadoAuto(dxfDestino)

                                    Dim tentativas As Integer = 0
                                    Do Until File.Exists(dxfDestino) OrElse tentativas > 20
                                        Threading.Thread.Sleep(250)
                                        tentativas += 1
                                    Loop

                                    If File.Exists(dxfDestino) Then
                                        row.Cells("dgvdxf").Value = My.Resources.dxf
                                        exportadosDXF += 1
                                    Else
                                        row.Cells("dgvdxf").Value = My.Resources.sem_icone
                                        errosLote.Add($"⚠️ DXF blank não gerado para '{nomeArquivo}.par' (arquivo pode não ter planificação)")
                                    End If
                                Catch exDxfPar As Exception
                                    row.Cells("dgvdxf").Value = My.Resources.sem_icone
                                    errosLote.Add($"⚠️ DXF ignorado para '{nomeArquivo}.par': {exDxfPar.Message}")
                                End Try

                            Case Else
                                row.Cells("dgvdxf").Value = My.Resources.sem_icone
                        End Select
                    End If

                    '──────────────────────────────────────────────
                    ' 🔹 Exportar PDF via Detalhamento (.dft)
                    '──────────────────────────────────────────────
                    If chkPDF.Checked Then
                        Try
                            Dim pdfDestino As String = IO.Path.Combine(pastaDestinoFinal, nomeArquivo & ".pdf")
                            Dim caminhoDFT As String = IO.Path.Combine(pastaOriginal, nomeArquivo & ".dft")

                            lbl.Text = $"PDF: {nomeArquivo}"
                            System.Windows.Forms.Application.DoEvents()

                            If Not File.Exists(caminhoDFT) Then
                                row.Cells("dgvpdf").Value = My.Resources.sem_icone
                                errosLote.Add($"⚠️ PDF pulado — não existe detalhamento '{nomeArquivo}.dft'")
                            Else
                                ' Exporta diretamente do .dft sem depender de ActiveDocument
                                ExportarPDFDoDetalhamento(caminhoDFT, pdfDestino)

                                Dim tentativas As Integer = 0
                                Do Until File.Exists(pdfDestino) OrElse tentativas > 20
                                    Threading.Thread.Sleep(250)
                                    tentativas += 1
                                Loop

                                If File.Exists(pdfDestino) Then
                                    row.Cells("dgvpdf").Value = My.Resources.pdf
                                    exportadosPDF += 1
                                Else
                                    row.Cells("dgvpdf").Value = My.Resources.sem_icone
                                    errosLote.Add($"⚠️ PDF não gerado para '{nomeArquivo}'")
                                End If
                            End If
                        Catch exPdf As Exception
                            row.Cells("dgvpdf").Value = My.Resources.sem_icone
                            errosLote.Add($"❌ Erro PDF '{nomeArquivo}': {exPdf.Message}")
                        End Try
                    End If

                    '── IGES (laser tubo) ──
                    If chkIGES.Checked AndAlso txttipodesenho = "CORTE LASER TUBO" Then
                        Try
                            Dim igesDestino As String = IO.Path.Combine(pastaDestinoFinal, nomeArquivo & ".iges")
                            ExportarIGESAuto(igesDestino)
                        Catch exIges As Exception
                            errosLote.Add($"❌ Erro IGES '{nomeArquivo}': {exIges.Message}")
                        End Try
                    End If

                    '── Fecha documento 3D sem salvar ──
                    Try : doc.Close(False) : Catch : End Try

                    processados += 1

                    ' Truque para contornar a animação lenta do ProgressBar no Windows 10/11:
                    ' Setar Value+1 primeiro e depois o valor real força redesenho imediato.
                    BarraProgresso.Style = ProgressBarStyle.Continuous
                    Dim valorAtual As Integer = Math.Min(processados, total)
                    If valorAtual < total Then
                        BarraProgresso.Value = valorAtual + 1
                    End If
                    BarraProgresso.Value = valorAtual

                    lbl.Text = processados.ToString() & "/" & total.ToString() & " — " & nomeArquivo
                    BarraProgresso.Refresh()
                    lbl.Refresh()
                    System.Windows.Forms.Application.DoEvents()

                Catch exLinha As Exception
                    errosLote.Add($"❌ Erro inesperado '{IO.Path.GetFileName(caminhoAtual)}': {exLinha.Message}")
                End Try
            Next

            '── Garante que colunas de status estão visíveis ──
            Try
                dgv.Columns("dgvdxf").Visible = True
                dgv.Columns("dgvpdf").Visible = True
            Catch : End Try

            '── Reabre o primeiro arquivo do conjunto ──
            Try
                If Not String.IsNullOrWhiteSpace(primeiroArquivo) AndAlso File.Exists(primeiroArquivo) Then
                    Threading.Thread.Sleep(500)
                    Dim docPrimeiro = app.Documents.Open(primeiroArquivo)
                    docPrimeiro.Activate()
                    app.DoIdle()
                End If
            Catch : End Try

            '── Finalização ──
            app.DisplayAlerts = True
            BarraProgresso.Visible = False
            BarraProgresso.Value = 0
            lbl.Text = ""

            '── Resumo final ──
            Dim resumo As New System.Text.StringBuilder()
            resumo.AppendLine($"✅ DXF exportados: {exportadosDXF}")
            resumo.AppendLine($"✅ PDF exportados: {exportadosPDF}")
            If errosLote.Count > 0 Then
                resumo.AppendLine()
                resumo.AppendLine("Avisos/Erros (" & errosLote.Count.ToString() & "):")
                Dim maxErros As Integer = Math.Min(errosLote.Count, 15)
                For i As Integer = 0 To maxErros - 1
                    resumo.AppendLine("  " & errosLote(i))
                Next
                If errosLote.Count > 15 Then resumo.AppendLine("  ... e mais " & (errosLote.Count - 15).ToString() & " avisos.")
            End If
            MessageBox.Show(resumo.ToString(), "SINCO - Resultado da Exportação", MessageBoxButtons.OK,
                            If(errosLote.Count > 0, MessageBoxIcon.Warning, MessageBoxIcon.Information))

        Catch ex As Exception
            Try : app.DisplayAlerts = True : Catch : End Try
            MessageBox.Show("Erro fatal no lote: " & ex.Message, "SINCO - Exportação", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub



    Public Sub FiltrarPorCaminho(ByVal dgv As DataGridView,
                                 ByVal termo As String,
                                 ByVal empresa As String)
        Try
            If tabelaOriginalBOM Is Nothing Then Exit Sub

            ' Cria uma DataView baseada na tabela original
            Dim view As New DataView(tabelaOriginalBOM)

            Dim filtros As New List(Of String)

            ' 🔹 Trata texto de filtro por caminho
            If Not String.IsNullOrWhiteSpace(termo) Then
                termo = termo.Replace("'", "''") ' evita erro por aspas simples

                If view.Table.Columns.Contains("CaminhoCompleto") Then
                    filtros.Add($"CaminhoCompleto LIKE '%{termo}%'")
                End If
            End If

            ' 🔹 Trata texto de filtro por empresa
            If Not String.IsNullOrWhiteSpace(empresa) Then
                empresa = empresa.Replace("'", "''")

                If view.Table.Columns.Contains("Empresa") Then
                    filtros.Add($"Empresa LIKE '%{empresa}%'")
                End If
            End If

            ' 🔹 Monta o RowFilter
            If filtros.Count > 0 Then
                ' Se tiver mais de um filtro, junta com AND
                view.RowFilter = String.Join(" AND ", filtros)
            Else
                ' Sem filtros → mostra tudo
                view.RowFilter = ""
            End If

            ' Atualiza o grid
            dgv.DataSource = view

        Catch ex As Exception
            MessageBox.Show("Erro ao filtrar: " & ex.Message,
                            "Filtro", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub






End Module
