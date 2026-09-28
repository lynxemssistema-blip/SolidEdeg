Imports System.IO
Imports System.Runtime.InteropServices
Imports System.Windows.Forms

''' <summary>
''' Classe utilitária para manipulação e exportação de dados para Excel de forma segura e otimizada.
''' </summary>
Public Class ClExcel

    ''' <summary>
    ''' Exporta o conteúdo de um DataGridView para um arquivo CSV/Excel de texto formatado.
    ''' </summary>
    Public Function ExportarDataGridViewParaCsv(dgv As DataGridView, caminhoArquivo As String) As Boolean
        Try
            If dgv Is Nothing OrElse dgv.Rows.Count = 0 Then Return False

            Using sw As New StreamWriter(caminhoArquivo, False, System.Text.Encoding.UTF8)
                ' Cabeçalhos
                Dim colunas As New List(Of String)
                For Each col As DataGridViewColumn In dgv.Columns
                    If col.Visible Then colunas.Add("""" & col.HeaderText.Replace("""", """""") & """")
                Next
                sw.WriteLine(String.Join(";", colunas))

                ' Linhas
                For Each row As DataGridViewRow In dgv.Rows
                    If Not row.IsNewRow Then
                        Dim valores As New List(Of String)
                        For Each col As DataGridViewColumn In dgv.Columns
                            If col.Visible Then
                                Dim val As String = If(row.Cells(col.Index).Value IsNot Nothing, row.Cells(col.Index).Value.ToString(), "")
                                valores.Add("""" & val.Replace("""", """""") & """")
                            End If
                        Next
                        sw.WriteLine(String.Join(";", valores))
                    End If
                Next
            End Using
            Return True
        Catch ex As Exception
            MessageBox.Show("Erro ao exportar dados para CSV: " & ex.Message, "Excel/Exportação", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End Try
    End Function

    ''' <summary>
    ''' Libera objetos COM do Excel de forma segura da memória evitando processos órfãos EXCEL.EXE.
    ''' </summary>
    Public Shared Sub LiberarObjetoCom(ByVal obj As Object)
        Try
            If obj IsNot Nothing AndAlso Marshal.IsComObject(obj) Then
                Marshal.ReleaseComObject(obj)
            End If
        Catch
        Finally
            obj = Nothing
        End Try
    End Sub

End Class