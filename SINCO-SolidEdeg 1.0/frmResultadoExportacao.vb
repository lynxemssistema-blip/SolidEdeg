Imports System.IO
Imports System.Drawing
Imports System.Windows.Forms

Public Class ResultadoItemExportacao
    Public Property NomeArquivo As String = ""
    Public Property Extensao As String = ""
    Public Property DXFGerado As Boolean = False
    Public Property PDFGerado As Boolean = False
    Public Property Observacao As String = ""
End Class

Public Class frmResultadoExportacao
    Inherits Form

    Private pnlTopo As Panel
    Private lblTitulo As Label
    Private lblStats As Label
    Private splitMain As SplitContainer
    Private dgvItens As DataGridView
    Private grpErros As GroupBox
    Private rtbErros As RichTextBox
    Private pnlBotoes As Panel
    Private btnSalvarLog As Button
    Private btnFechar As Button

    Private ReadOnly _resultados As List(Of ResultadoItemExportacao)
    Private ReadOnly _erros As List(Of String)
    Private ReadOnly _totalDXF As Integer
    Private ReadOnly _totalPDF As Integer

    Public Sub New(resultados As List(Of ResultadoItemExportacao),
                   erros As List(Of String),
                   totalDXF As Integer,
                   totalPDF As Integer)
        _resultados = resultados
        _erros = erros
        _totalDXF = totalDXF
        _totalPDF = totalPDF
        InitLayout()
        CarregarDados()
    End Sub

    Private Sub InitLayout()
        Me.Text = "SINCO - Resultado da Exportacao"
        Me.Size = New Size(900, 640)
        Me.MinimumSize = New Size(700, 450)
        Me.StartPosition = FormStartPosition.CenterScreen
        Me.Font = New Font("Segoe UI", 9)
        Me.BackColor = Color.FromArgb(30, 30, 30)
        Me.ForeColor = Color.White

        pnlTopo = New Panel() With {
            .Dock = DockStyle.Top,
            .Height = 72,
            .BackColor = Color.FromArgb(18, 18, 18),
            .Padding = New Padding(14, 8, 14, 0)
        }

        lblTitulo = New Label() With {
            .Text = "Resultado da Exportacao em Lote",
            .AutoSize = True,
            .Font = New Font("Segoe UI", 13, FontStyle.Bold),
            .ForeColor = Color.FromArgb(100, 200, 255),
            .Location = New Point(14, 8)
        }

        lblStats = New Label() With {
            .AutoSize = True,
            .Font = New Font("Segoe UI", 9),
            .ForeColor = Color.FromArgb(180, 230, 180),
            .Location = New Point(16, 40)
        }
        pnlTopo.Controls.AddRange({lblTitulo, lblStats})

        splitMain = New SplitContainer() With {
            .Dock = DockStyle.Fill,
            .Orientation = Orientation.Horizontal,
            .SplitterDistance = 350,
            .BackColor = Color.FromArgb(30, 30, 30),
            .Panel1MinSize = 150,
            .Panel2MinSize = 120
        }

        dgvItens = New DataGridView() With {
            .Dock = DockStyle.Fill,
            .AllowUserToAddRows = False,
            .AllowUserToDeleteRows = False,
            .ReadOnly = True,
            .BackgroundColor = Color.FromArgb(40, 40, 40),
            .GridColor = Color.FromArgb(60, 60, 60),
            .BorderStyle = BorderStyle.None,
            .SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            .RowHeadersVisible = False,
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            .EnableHeadersVisualStyles = False
        }
        dgvItens.DefaultCellStyle.BackColor = Color.FromArgb(40, 40, 40)
        dgvItens.DefaultCellStyle.ForeColor = Color.WhiteSmoke
        dgvItens.DefaultCellStyle.SelectionBackColor = Color.FromArgb(0, 120, 215)
        dgvItens.DefaultCellStyle.SelectionForeColor = Color.White
        dgvItens.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(18, 18, 18)
        dgvItens.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(100, 200, 255)
        dgvItens.ColumnHeadersDefaultCellStyle.Font = New Font("Segoe UI", 9, FontStyle.Bold)
        dgvItens.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(45, 45, 45)

        grpErros = New GroupBox() With {
            .Dock = DockStyle.Fill,
            .Text = " Log de Erros / Avisos ",
            .ForeColor = Color.FromArgb(255, 180, 80),
            .BackColor = Color.FromArgb(30, 30, 30),
            .Font = New Font("Segoe UI", 9, FontStyle.Bold),
            .Padding = New Padding(6)
        }
        rtbErros = New RichTextBox() With {
            .Dock = DockStyle.Fill,
            .ReadOnly = True,
            .BackColor = Color.FromArgb(18, 18, 18),
            .ForeColor = Color.FromArgb(220, 220, 220),
            .Font = New Font("Consolas", 9),
            .BorderStyle = BorderStyle.None,
            .WordWrap = False,
            .ScrollBars = RichTextBoxScrollBars.Both
        }
        grpErros.Controls.Add(rtbErros)

        splitMain.Panel1.Controls.Add(dgvItens)
        splitMain.Panel2.Controls.Add(grpErros)

        pnlBotoes = New Panel() With {
            .Dock = DockStyle.Bottom,
            .Height = 50,
            .BackColor = Color.FromArgb(18, 18, 18),
            .Padding = New Padding(10, 9, 10, 9)
        }
        btnSalvarLog = New Button() With {
            .Text = "Salvar Log",
            .Size = New Size(120, 30),
            .Location = New Point(10, 10),
            .FlatStyle = FlatStyle.Flat,
            .BackColor = Color.FromArgb(0, 100, 180),
            .ForeColor = Color.White,
            .Font = New Font("Segoe UI", 9, FontStyle.Bold),
            .Cursor = Cursors.Hand
        }
        btnSalvarLog.FlatAppearance.BorderColor = Color.FromArgb(0, 140, 220)

        btnFechar = New Button() With {
            .Text = "Fechar",
            .Size = New Size(90, 30),
            .Anchor = AnchorStyles.Right Or AnchorStyles.Top,
            .FlatStyle = FlatStyle.Flat,
            .BackColor = Color.FromArgb(60, 60, 60),
            .ForeColor = Color.White,
            .Font = New Font("Segoe UI", 9),
            .Cursor = Cursors.Hand
        }
        btnFechar.FlatAppearance.BorderColor = Color.FromArgb(90, 90, 90)
        btnFechar.Location = New Point(pnlBotoes.Width - 110, 10)
        pnlBotoes.Controls.AddRange({btnSalvarLog, btnFechar})

        Me.Controls.AddRange({splitMain, pnlTopo, pnlBotoes})
        AddHandler btnSalvarLog.Click, AddressOf OnSalvarLog
        AddHandler btnFechar.Click, Sub(s, ev) Me.Close()
        AddHandler pnlBotoes.Resize, Sub(s, ev)
                                         If btnFechar IsNot Nothing Then
                                             btnFechar.Left = pnlBotoes.Width - 110
                                         End If
                                     End Sub
    End Sub

    Private Sub CarregarDados()
        Dim total As Integer = If(_resultados IsNot Nothing, _resultados.Count, 0)
        Dim erroCount As Integer = If(_erros IsNot Nothing, _erros.Count, 0)
        lblStats.Text = "Total: " & total.ToString() &
                        "   |   DXF gerados: " & _totalDXF.ToString() &
                        "   |   PDF gerados: " & _totalPDF.ToString() &
                        "   |   Avisos/Erros: " & erroCount.ToString()

        Dim dt As New DataTable()
        dt.Columns.Add("Arquivo", GetType(String))
        dt.Columns.Add("Tipo", GetType(String))
        dt.Columns.Add("DXF", GetType(String))
        dt.Columns.Add("PDF", GetType(String))
        dt.Columns.Add("Observacao", GetType(String))

        If _resultados IsNot Nothing Then
            For Each item In _resultados
                Dim dr As DataRow = dt.NewRow()
                dr("Arquivo") = item.NomeArquivo
                dr("Tipo") = item.Extensao.ToUpperInvariant()
                dr("DXF") = If(item.DXFGerado, "OK", "-")
                dr("PDF") = If(item.PDFGerado, "OK", "-")
                dr("Observacao") = item.Observacao
                dt.Rows.Add(dr)
            Next
        End If

        dgvItens.DataSource = dt
        If dgvItens.Columns.Contains("Arquivo") Then dgvItens.Columns("Arquivo").FillWeight = 42
        If dgvItens.Columns.Contains("Tipo") Then dgvItens.Columns("Tipo").FillWeight = 7
        If dgvItens.Columns.Contains("DXF") Then dgvItens.Columns("DXF").FillWeight = 8
        If dgvItens.Columns.Contains("PDF") Then dgvItens.Columns("PDF").FillWeight = 8
        If dgvItens.Columns.Contains("Observacao") Then dgvItens.Columns("Observacao").FillWeight = 35

        For Each row As DataGridViewRow In dgvItens.Rows
            If row.Cells("DXF").Value?.ToString() = "OK" Then
                row.Cells("DXF").Style.ForeColor = Color.FromArgb(100, 230, 100)
                row.Cells("DXF").Style.Font = New Font("Segoe UI", 9, FontStyle.Bold)
            End If
            If row.Cells("PDF").Value?.ToString() = "OK" Then
                row.Cells("PDF").Style.ForeColor = Color.FromArgb(100, 230, 100)
                row.Cells("PDF").Style.Font = New Font("Segoe UI", 9, FontStyle.Bold)
            End If
            Dim obs As String = row.Cells("Observacao").Value?.ToString()
            If Not String.IsNullOrEmpty(obs) Then
                row.Cells("Observacao").Style.ForeColor = Color.FromArgb(255, 200, 80)
            End If
        Next

        If _erros Is Nothing OrElse _erros.Count = 0 Then
            rtbErros.ForeColor = Color.FromArgb(100, 230, 100)
            rtbErros.Text = "Nenhum erro registrado. Processo concluido com sucesso."
            grpErros.Text = " Log de Erros / Avisos — Sem erros "
        Else
            grpErros.Text = " Log de Erros / Avisos (" & _erros.Count.ToString() & " ocorrencias) "
            rtbErros.Clear()
            Dim header As String = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") & " — Exportacao em Lote SINCO" & vbCrLf & New String("-"c, 70) & vbCrLf
            rtbErros.SelectionStart = rtbErros.TextLength
            rtbErros.SelectionColor = Color.FromArgb(140, 140, 140)
            rtbErros.AppendText(header)

            For Each ln In _erros
                rtbErros.SelectionStart = rtbErros.TextLength
                Dim lower As String = ln.ToLower()
                If lower.Contains("falha") OrElse lower.Contains("erro") Then
                    rtbErros.SelectionColor = Color.FromArgb(255, 100, 100)
                ElseIf lower.Contains("aviso") OrElse lower.Contains("pulado") OrElse lower.Contains("nao gerado") Then
                    rtbErros.SelectionColor = Color.FromArgb(255, 200, 80)
                Else
                    rtbErros.SelectionColor = Color.FromArgb(200, 200, 200)
                End If
                rtbErros.AppendText(ln & vbCrLf)
            Next
        End If
        rtbErros.SelectionStart = 0
        rtbErros.ScrollToCaret()
    End Sub

    Private Sub OnSalvarLog(sender As Object, e As EventArgs)
        Using sfd As New SaveFileDialog()
            sfd.Title = "Salvar Log da Exportacao"
            sfd.Filter = "Arquivo de Log (*.txt)|*.txt|Todos (*.*)|*.*"
            sfd.FileName = "SINCO_Export_" & DateTime.Now.ToString("yyyyMMdd_HHmm") & ".txt"
            sfd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
            If sfd.ShowDialog() <> DialogResult.OK Then Exit Sub

            Dim sb As New System.Text.StringBuilder()
            sb.AppendLine("SINCO - Solid Edge | Relatorio de Exportacao em Lote")
            sb.AppendLine(New String("="c, 72))
            sb.AppendLine("Data/Hora : " & DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"))
            sb.AppendLine("Total     : " & (_resultados?.Count).GetValueOrDefault().ToString() & " arquivo(s) processado(s)")
            sb.AppendLine("DXF       : " & _totalDXF.ToString() & " gerado(s)")
            sb.AppendLine("PDF       : " & _totalPDF.ToString() & " gerado(s)")
            sb.AppendLine("Erros     : " & (_erros?.Count).GetValueOrDefault().ToString() & " ocorrencia(s)")
            sb.AppendLine(New String("-"c, 72))
            sb.AppendLine()
            sb.AppendLine("ITENS PROCESSADOS:")
            sb.AppendLine(New String("-"c, 72))
            If _resultados IsNot Nothing Then
                For Each item In _resultados
                    sb.AppendLine(String.Format("  {0,-44} {1,-5}  DXF:{2}  PDF:{3}  {4}",
                                                 item.NomeArquivo, item.Extensao.ToUpperInvariant(),
                                                 If(item.DXFGerado, "OK ", "---"),
                                                 If(item.PDFGerado, "OK ", "---"),
                                                 item.Observacao))
                Next
            End If
            sb.AppendLine()
            sb.AppendLine("LOG DE ERROS / AVISOS:")
            sb.AppendLine(New String("-"c, 72))
            If _erros IsNot Nothing AndAlso _erros.Count > 0 Then
                For Each err In _erros
                    sb.AppendLine("  " & err)
                Next
            Else
                sb.AppendLine("  Nenhum erro registrado.")
            End If
            Try
                File.WriteAllText(sfd.FileName, sb.ToString(), System.Text.Encoding.UTF8)
                MessageBox.Show("Log salvo em:" & vbCrLf & sfd.FileName, "SINCO", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Catch ex As Exception
                MessageBox.Show("Erro ao salvar: " & ex.Message, "SINCO", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Using
    End Sub
End Class
