Imports System.Runtime.InteropServices
Imports MySql.Data.MySqlClient

Module Module1

    ' 🔹 Conecta ao Solid Edge apenas quando necessário
    Public app As Object = Nothing

    Public Function ConectarSolidEdge() As Object
        Try
            Return Marshal.GetActiveObject("SolidEdge.Application")
        Catch ex As Exception
            MsgBox("Erro ao conectar com o Solid Edge: " & ex.Message, vbExclamation)
            Return Nothing
        End Try
    End Function

    ' Conexão e controle de banco de dados
    Public cl_BancoDados As New ClBancoDados
    Public myconect As New MySqlConnection
    Public mycomand As New MySqlCommand
    Public conexao As String
    Public OpenConnectionsCount As Integer = 0

    ' Objetos de negócio e domínio
    Public tabelaOriginalBOM As DataTable
    Public DadosArquivoCorrente As New clDadosArquivoCorrente
    Public classePecaManufaturada As New CLPecaManufaturada
    Public classeMaterial As New clMaterial
    Public MaterialDesenhoCliente As New clMaterialDesenhoCliente
    Public ComplementoTipoBanco As String = ""
    Public AgentesIA As New ClIA
    Public BancoCliente As String
    Public NaoMudar As String = ""
    Public DescarregarLynx As Boolean = True
    Public ProdutoPrincipal As String
    Public ProcessosPadrao As New clProcesso
    Public Usuario As New clUsuario
    Public ContadorBarradeProgresso As Integer
    Public Projeto As New clProjeto
    Public TagProjeto As New clTag
    Public colunas() As String
    Public qtdePecaLm As Double
    Public InserirNovoTabelas As Boolean = False

    ' Tabelas e DataTables globais
    Public dtDesenhos As DataTable
    Public TabelaViewMontaPeca As DataTable
    Public TabelaRnc As DataTable
    Public IdMontaPeca As Integer

    ' Informações auxiliares
    Public TituloPadraoProduto As String
    Public HoraServidor As String

    ' Excel e PDF
    Public TemplatesExcel As New ClExcel
    Public pdfsinco As New clPdf

    ' Campos auxiliares
    Public DescricaoFinalizacaoPendencia As String
    Public ExtensaoArquivoCorrente As String
    Public IdMAterial As String

    ' Campos globais reutilizáveis
    Public VCampo0 As String = ""
    Public VCampo1 As String = ""
    Public VCampo2 As String = ""
    Public VCampo3 As String = ""
    Public VCampo4 As String = ""
    Public VCampo5 As String = ""
    Public VCampo6 As String = ""
    Public VCampo7 As String = ""
    Public VCampo8 As String = ""
    Public VCampo9 As String = ""
    Public VCampo10 As String = ""

    ' Flags e controles
    Public estatus As Boolean = False
    Public BloqueaArquivoExistente As Boolean = False
    Public vemdalista As Boolean = False
    Public IdEmpresa As String = ""
    Public descempresa As String = ""

    ' Imagens
    Public iconeTipoArquivo As System.Drawing.Image
    Public iconeAtencao As System.Drawing.Image
    Public iconePDF As System.Drawing.Image
    Public iconeDXF As System.Drawing.Image
    Public iconeLXDS As System.Drawing.Image

End Module
