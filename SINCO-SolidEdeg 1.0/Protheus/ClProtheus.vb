Imports System.Data
Imports System.Globalization
Imports System.Linq.Expressions
Imports System.Text
Imports System.Windows.Forms
Imports MySql.Data.MySqlClient
Imports Mysqlx.Crud
Imports Npgsql

Public Class Protheus

    Private Shared ReadOnly connString As String =
        "Host=192.168.1.61;Port=5432;Username=sinco2;Password=sinco25;Database=p12prd;"

    ''' <summary>
    ''' Carrega o ComboBox com os pedidos do Protheus (SC5 + SA1).
    ''' Display: C5_NUM - A1_NOME
    ''' Value:   C5_NUM
    ''' </summary>
    ''' <param name="cbo">ComboBox que receberá os dados</param>
    ''' <param name="owner">Form (opcional) para trocar cursor para WaitCursor</param>
    Public Shared Sub CarregarPedidosNoCombo(cbo As ComboBox, Optional owner As Form = Nothing)

        Dim dt As New DataTable()

        Try
            If owner IsNot Nothing Then owner.Cursor = Cursors.WaitCursor

            Using conn As New NpgsqlConnection(connString)
                conn.Open()

                Dim sql As String =
                    "SELECT " &
                    "   sc5.c5_num, " &
                    "   sa1.a1_nome " &
                    "FROM public.sc5010 sc5 " &
                    "LEFT JOIN public.sa1010 sa1 " &
                    "   ON sa1.a1_cod = sc5.c5_cliente " &
                    "  AND COALESCE(sa1.d_e_l_e_t_, '') = '' " &
                    "WHERE " &
                    "   COALESCE(sc5.c5_liberok, '') = '' " &
                    "ORDER BY sc5.c5_num"

                Using da As New NpgsqlDataAdapter(sql, conn)
                    da.Fill(dt)
                End Using
            End Using

            ' Cria coluna descritiva p/ exibição
            If Not dt.Columns.Contains("Descricao") Then
                dt.Columns.Add("Descricao", GetType(String))
            End If

            For Each row As DataRow In dt.Rows
                Dim num As String = If(row("c5_num") IsNot DBNull.Value, row("c5_num").ToString().Trim(), "")
                Dim nome As String = If(row("a1_nome") IsNot DBNull.Value, row("a1_nome").ToString().Trim(), "")
                row("Descricao") = $"{num} - {nome}"
            Next

            ' Faz o binding no ComboBox
            With cbo
                .DataSource = dt
                .DisplayMember = "Descricao" ' o que aparece
                .ValueMember = "c5_num"      ' valor interno
                .SelectedIndex = -1          ' nada selecionado
            End With

        Catch ex As Exception
            MessageBox.Show("Erro ao carregar pedidos do Protheus:" &
                            Environment.NewLine &
                            ex.Message,
                            "Protheus - Erro",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error)
        Finally
            If owner IsNot Nothing Then owner.Cursor = Cursors.Default
        End Try

    End Sub

    Public Shared Sub CarregarItensDoPedidoNoCombo(cbo As ComboBox,
                                               numeroPedido As String,
                                               Optional owner As Form = Nothing)

        ' Se não tiver pedido, limpa o combo e sai
        If String.IsNullOrWhiteSpace(numeroPedido) Then
            cbo.DataSource = Nothing
            cbo.Items.Clear()
            Return
        End If

        Dim dt As New DataTable()

        Try
            If owner IsNot Nothing Then owner.Cursor = Cursors.WaitCursor

            Using conn As New NpgsqlConnection(connString)
                conn.Open()

                Dim sql As String =
                "SELECT " &
                "   c6.c6_num, " &
                "   c6.c6_item, " &
                "   c6.c6_produto, " &
                "   c6.c6_descri " &
                "FROM public.sc6010 c6 " &
                "WHERE " &
                "   COALESCE(c6.d_e_l_e_t_, '') = '' " &
                "   AND c6.c6_num = @num " &
                "ORDER BY c6.c6_item"

                Using cmd As New NpgsqlCommand(sql, conn)
                    cmd.Parameters.AddWithValue("@num", numeroPedido)

                    Using da As New NpgsqlDataAdapter(cmd)
                        da.Fill(dt)
                    End Using
                End Using
            End Using

            ' Coluna para exibição: "001 - PRODUTO - DESCRIÇÃO"
            If Not dt.Columns.Contains("Descricao") Then
                dt.Columns.Add("Descricao", GetType(String))
            End If

            For Each row As DataRow In dt.Rows
                Dim item As String = If(row("c6_item") IsNot DBNull.Value, row("c6_item").ToString().Trim(), "")
                Dim prod As String = If(row("c6_produto") IsNot DBNull.Value, row("c6_produto").ToString().Trim(), "")
                Dim descri As String = If(row("c6_descri") IsNot DBNull.Value, row("c6_descri").ToString().Trim(), "")
                row("Descricao") = $"{item} - {prod} - {descri}"
            Next

            With cbo
                .DataSource = dt
                .DisplayMember = "Descricao"   ' o que aparece
                .ValueMember = "c6_produto"    ' valor interno (pode trocar p/ c6_item se preferir)
                .SelectedIndex = -1
            End With

        Catch ex As Exception
            MessageBox.Show("Erro ao carregar itens do pedido no Protheus:" &
                        Environment.NewLine &
                        ex.Message,
                        "Protheus - Erro",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error)
        Finally
            If owner IsNot Nothing Then owner.Cursor = Cursors.Default
        End Try

    End Sub

    Public Shared Sub CarregarPedidosNoGrid(dgv As DataGridView, cliente As String, projeto As String,
                                           Optional owner As Form = Nothing)

        Dim dt As New DataTable()

        Try
            If owner IsNot Nothing Then owner.Cursor = Cursors.WaitCursor

            Using conn As New NpgsqlConnection(connString)
                conn.Open()

                Dim sql As String =
                    "SELECT " &
                    "   sc5.c5_num, " &
                    "   sa1.a1_nome, " &
                    "   TO_CHAR(TO_DATE(sc5.c5_emissao, 'YYYYMMDD'), 'DD/MM/YY') AS emissao " &
                    "   FROM public.sc5010 sc5 " &
                    "   LEFT JOIN public.sa1010 sa1 " &
                    "   ON sa1.a1_cod = sc5.c5_cliente " &
                    "   WHERE " &
                    "   COALESCE(sc5.d_e_l_e_t_, '') = '' " &         ' somente registros ativos
                    "   and sc5.c5_tipo = 'N'" &
                    "   and sa1.a1_nome like '%" & cliente & "%'" &
                    "   and sc5.c5_num like '%" & projeto & "%'" &
                    "ORDER BY sc5.c5_num;"

                Using da As New NpgsqlDataAdapter(sql, conn)
                    da.Fill(dt)
                End Using
            End Using

            dgv.AutoGenerateColumns = True
            dgv.DataSource = dt

        Catch ex As Exception
            MessageBox.Show("Erro ao carregar pedidos do Protheus no grid:" &
                            Environment.NewLine &
                            ex.Message,
                            "Protheus - Erro",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error)
        Finally
            If owner IsNot Nothing Then owner.Cursor = Cursors.Default
        End Try

    End Sub

    Public Shared Sub CarregarItensPedidosNoGrid(dgv As DataGridView, C5_NUM As String,
                                           Optional owner As Form = Nothing)

        Dim dt As New DataTable()

        Try
            If owner IsNot Nothing Then owner.Cursor = Cursors.WaitCursor

            Using conn As New NpgsqlConnection(connString)
                conn.Open()

                Dim sql As String =
                    "SELECT
    c5.c5_num,
    c5.c5_cliente,
    sa1.a1_nome      AS nome_cliente,
    c6.c6_item,
    c6.c6_produto,
    sb1.b1_desc      AS descricao_produto_cadastro,
    c6.c6_descri     AS descricao_produto_pedido,
    c6.c6_um,
    c6.c6_qtdven
   FROM public.sc5010 c5
JOIN public.sc6010 c6
    ON c6.c6_filial = c5.c5_filial
   AND c6.c6_num    = c5.c5_num
JOIN public.sa1010 sa1
    ON sa1.a1_cod = c5.c5_cliente
   AND COALESCE(sa1.d_e_l_e_t_, '') = ''
JOIN public.sb1010 sb1
    ON sb1.b1_cod = c6.c6_produto
   AND COALESCE(sb1.d_e_l_e_t_, '') = ''
WHERE
    COALESCE(c5.d_e_l_e_t_, '') = ''
    AND COALESCE(c6.d_e_l_e_t_, '') = ''
    AND c5.c5_num = '" & C5_NUM &"'
ORDER BY c6.c6_item;"


                Using da As New NpgsqlDataAdapter(sql, conn)
                    da.Fill(dt)
                End Using
            End Using

            dgv.AutoGenerateColumns = True
            dgv.DataSource = dt

        Catch ex As Exception
            MessageBox.Show("Erro ao carregar pedidos do Protheus no grid:" &
                            Environment.NewLine &
                            ex.Message,
                            "Protheus - Erro",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error)
        Finally
            If owner IsNot Nothing Then owner.Cursor = Cursors.Default
        End Try

    End Sub

    Public Shared Sub CarregarDesenhoMetalfisaCliente(dgv As DataGridView, NumeroDesenho As String,
                                           Optional owner As Form = Nothing)

        Dim dt As New DataTable()

        Try
            If owner IsNot Nothing Then owner.Cursor = Cursors.WaitCursor

            Using conn As New NpgsqlConnection(connString)
                conn.Open()


                Dim sql As String =
                    "Select z1_nomecli, z1_produto,z1_codcli, z1_desccli,z1_revisao FROM Public.sz1010
 where z1_produto = '" & NumeroDesenho & "'
And d_e_l_e_t_ <> '*'"

                Using da As New NpgsqlDataAdapter(sql, conn)
                    da.Fill(dt)
                End Using
            End Using

            dgv.AutoGenerateColumns = True
            dgv.DataSource = dt

        Catch ex As Exception
            MessageBox.Show("Erro ao carregar Desenhos de Referencia do Protheus no grid:" &
                            Environment.NewLine &
                            ex.Message,
                            "Protheus - Erro",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error)
        Finally
            If owner IsNot Nothing Then owner.Cursor = Cursors.Default
        End Try

    End Sub

    Public Shared Sub CarregarMaterialProtheus(dgv As DataGridView, NumeroDesenho As String,
                                               desc1 As String,
                                               desc2 As String,
                                               desc3 As String,
                                         Optional owner As Form = Nothing)

        Dim dt As New DataTable()

        Try
            If owner IsNot Nothing Then owner.Cursor = Cursors.WaitCursor

            Using conn As New NpgsqlConnection(connString)
                conn.Open()


                Dim sql As String =
                    "SELECT b1_cod, b1_desc,b1_um, b1_uprc FROM public.sb1010 where d_e_l_e_t_ <> '*'
and b1_cod like '%" & NumeroDesenho & "%'
and b1_desc like '%" & desc1 & "%'
and b1_desc like '%" & desc2 & "%'
and b1_desc like '%" & desc3 & "%'
ORDER BY r_e_c_n_o_ DESC LIMIT 100"

                Using da As New NpgsqlDataAdapter(sql, conn)
                    da.Fill(dt)
                End Using
            End Using

            dgv.AutoGenerateColumns = True
            dgv.DataSource = dt

        Catch ex As Exception
            MessageBox.Show("Erro ao carregar Materiais de Referencia do Protheus no grid:" &
                            Environment.NewLine &
                            ex.Message,
                            "Protheus - Erro",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error)
        Finally
            If owner IsNot Nothing Then owner.Cursor = Cursors.Default
        End Try

    End Sub



    ''' <summary>
    ''' Carrega as operações (roteiro) de um produto na SG2010 e monta
    ''' um DataTable temporário no formato:
    ''' IdMaterial, IdProcesso, SequenciaExecucao, TempoPadraoMin,
    ''' Ativo, Observacao, DataCriacao, UsuarioCriacao, codMatFabricante.
    ''' </summary>
    ''' <param name="produto">Código do produto (G2_PRODUTO)</param>
    ''' <param name="usuarioLogado">Usuário responsável pela criação do registro</param>
    Public Shared Function GerarTabelaTempProcesso(produto As String,
                                                   usuarioLogado As String) As DataTable

        '-------------------------------------------------------
        ' 1) Monta o schema da tabela temp
        '-------------------------------------------------------
        Dim dtTemp As New DataTable("TempProcesso")

        dtTemp.Columns.Add("IdMaterial", GetType(String))
        dtTemp.Columns.Add("IdProcesso", GetType(Integer))          ' pode receber DBNull.Value
        dtTemp.Columns.Add("SequenciaExecucao", GetType(String))
        dtTemp.Columns.Add("TempoPadraoMin", GetType(Decimal))
        dtTemp.Columns.Add("Ativo", GetType(String))
        dtTemp.Columns.Add("Observacao", GetType(String))
        dtTemp.Columns.Add("DataCriacao", GetType(DateTime))
        dtTemp.Columns.Add("UsuarioCriacao", GetType(String))
        dtTemp.Columns.Add("codMatFabricante", GetType(String))

        '-------------------------------------------------------
        ' 2) Helper para normalizar texto:
        '    - Remove espaços e caracteres especiais
        '    - Mantém só letras e números
        '    - Converte para maiúsculo
        '-------------------------------------------------------
        Dim Normalizar As Func(Of String, String) =
            Function(valor As String) As String
                If String.IsNullOrWhiteSpace(valor) Then Return String.Empty

                Dim sb As New StringBuilder()
                For Each ch As Char In valor
                    If Char.IsLetterOrDigit(ch) Then
                        sb.Append(Char.ToUpperInvariant(ch))
                    End If
                Next
                Return sb.ToString()
            End Function

        Try
            '---------------------------------------------------
            ' 3) Carrega TODOS os processos de fabricação (MySQL)
            '    usando o padrão cl_BancoDados.AbrirBanco / myconect
            '---------------------------------------------------
            Dim dtProcessos As New DataTable()

            cl_BancoDados.AbrirBanco()

            If My.Settings.TipoConexao = "MYSQL" Then
                Try
                    Dim sqlProc As String =
                        "SELECT IdProcessoFabricacao, processofabricacao,CodigoProcessoFabricacao FROM processofabricacao;"

                    ' myconect: conexão MySQL já aberta pelo cl_BancoDados
                    Dim cmdProc As New MySqlCommand(sqlProc, myconect)

                    Using daProc As New MySqlDataAdapter(cmdProc)
                        daProc.Fill(dtProcessos)
                    End Using

                Catch ex As Exception
                    MessageBox.Show("Erro ao carregar processos de fabricação (MySQL):" &
                                    Environment.NewLine & ex.Message,
                                    "MySQL - Erro",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error)
                End Try
            Else
                MessageBox.Show("TipoConexao não é MYSQL. Verifique My.Settings.TipoConexao.",
                                "Configuração de Conexão",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning)
            End If

            ' Se tiver método pra fechar, você pode usar:
            ' cl_BancoDados.FecharBanco()

            ' Mapa: chave normalizada -> IdProcesso
            Dim mapaProcessos As New Dictionary(Of String, Integer)()

            For Each row As DataRow In dtProcessos.Rows
                Dim nomeProc As String = If(row("CodigoProcessoFabricacao"), "").ToString.Replace(" ", "").Replace("-", "").ToString()
                Dim chave As String = Normalizar(nomeProc)

                If Not String.IsNullOrEmpty(chave) AndAlso
                   Not mapaProcessos.ContainsKey(chave) Then

                    mapaProcessos.Add(chave, CInt(row("IdProcessoFabricacao")))
                End If
            Next

            '---------------------------------------------------
            ' 4) Busca as operações do produto na SG2010 (PostgreSQL)
            '---------------------------------------------------
            Dim dtOper As New DataTable()



            Using conn As New NpgsqlConnection(connString)
                conn.Open()

                Dim sqlOper As String =
                    "SELECT " &
                    "   TRIM(g2_produto) AS produto, " &
                    "   TRIM(g2_operac)  AS sequencia, " &
                    "   TRIM(g2_recurso) AS recurso, " &
                    "   TRIM(g2_descri)  AS descricao " &
                    "FROM public.sg2010 " &
                    "WHERE COALESCE(d_e_l_e_t_, '') = '' " &
                    "  AND TRIM(g2_produto) = @produto " &
                    "ORDER BY g2_operac;"

                Using cmdOper As New NpgsqlCommand(sqlOper, conn)
                    cmdOper.Parameters.AddWithValue("@produto", produto.ToString.Replace(".par", "").Replace(".psm", "").Replace(".asm", "").Trim())

                    Using daOper As New NpgsqlDataAdapter(cmdOper)
                        daOper.Fill(dtOper)
                    End Using
                End Using
            End Using

            '---------------------------------------------------
            ' 5) Monta as linhas da tabela temp
            '---------------------------------------------------
            For Each row As DataRow In dtOper.Rows
                ' ---- Match do processo (recurso x processofabricacao) normalizado ----
                Dim recursoBruto As String = row("recurso").ToString()
                Dim chaveRecurso As String = Normalizar(recursoBruto)

                Dim idProcesso As Object = DBNull.Value

                If Not String.IsNullOrEmpty(chaveRecurso) AndAlso
                   mapaProcessos.ContainsKey(chaveRecurso) Then

                    idProcesso = mapaProcessos(chaveRecurso)
                End If

                ' ---- Ajuste da sequência: 0010 -> 1, 0020 -> 2, 0030 -> 3... ----
                Dim sequenciaBruta As String = row("sequencia").ToString().Trim()
                Dim sequenciaExecucao As String = sequenciaBruta

                ' extrai apenas dígitos
                Dim sbSeq As New StringBuilder()
                For Each ch As Char In sequenciaBruta
                    If Char.IsDigit(ch) Then
                        sbSeq.Append(ch)
                    End If
                Next

                Dim seqNum As Integer
                If Integer.TryParse(sbSeq.ToString(), seqNum) Then
                    ' 10 -> 1, 20 -> 2, 30 -> 3...
                    Dim seqExecNum As Integer = seqNum \ 10   ' divisão inteira
                    sequenciaExecucao = seqExecNum.ToString()
                End If

                ' ---- Cria linha na tabela temp ----
                Dim nova As DataRow = dtTemp.NewRow()

                nova("IdMaterial") = ""                               ' você preenche depois
                nova("IdProcesso") = idProcesso                       ' pode ser DBNull se não encontrar
                nova("SequenciaExecucao") = sequenciaExecucao
                nova("TempoPadraoMin") = 0D                            ' placeholder, ajustar se tiver campo de tempo
                nova("Ativo") = "S"
                nova("Observacao") = ""
                nova("DataCriacao") = DateTime.Now
                nova("UsuarioCriacao") = usuarioLogado
                nova("codMatFabricante") = row("produto").ToString()  ' código do produto

                dtTemp.Rows.Add(nova)


                ' Busca a MAIOR sequência existente para o material atual
                cl_BancoDados.RetornaCampoDaPesquisa("
            SELECT IFNULL(MAX(SequenciaExecucao), 0) AS UltimaSequencia 
            FROM material_processo 
            WHERE IdMaterial = '" & DadosArquivoCorrente.IdMaterial & "'", "UltimaSequencia")

                Dim Sequencia As Integer = 1
                If IsNumeric(VCampo0) Then Sequencia = CInt(VCampo0) + 1

                ' Chama a função que grava o processo no banco


                ProcessosPadrao.SalvarOrdeMServicoBanco(
                idProcesso, 'ProcessosPadrao.IdProcesso,
                DadosArquivoCorrente.IdMaterial,
                sequenciaExecucao, 'Sequencia,
                DadosArquivoCorrente.NomeArquivoSemExtensao
            )

            Next
        Catch ex As Exception
            MessageBox.Show("Erro ao gerar tabela temporária de processos:" &
                            Environment.NewLine & ex.Message,
                            "Protheus - Erro",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error)
        End Try

        Return dtTemp

    End Function


    Private Shared Function GarantirMaterialERetornarId(codMatFabricante As String,
                                                   desc As String,
                                                   um As String,
                                                   usuarioCriacao As String) As Integer

        codMatFabricante = If(codMatFabricante, "").Trim().ToUpper()
        desc = If(desc, "").Trim().ToUpper()
        um = If(um, "").Trim().ToUpper()

        If codMatFabricante = "" Then Return 0

        cl_BancoDados.AbrirBanco()

        ' 1) tenta localizar
        Dim idExistente As Integer = 0
        Using cmd As New MySqlCommand("SELECT IdMaterial FROM material WHERE CodMatFabricante = @cod LIMIT 1;", myconect)
            cmd.Parameters.AddWithValue("@cod", codMatFabricante)
            Dim obj = cmd.ExecuteScalar()
            If obj IsNot Nothing AndAlso obj IsNot DBNull.Value Then
                Integer.TryParse(obj.ToString(), idExistente)
            End If
        End Using

        ' 2) se não existir -> INSERT via sua função
        If idExistente <= 0 Then
            ' Usa sua função exatamente como já está no seu sistema
            classeMaterial.SalvarDadosMaterial(
            DescResumo:=desc,
            DescDetal:=desc,
            Peso:="0",
            Unidade:=um,
            CodMatFabricante:=codMatFabricante,
            CodigoJuridicoMat:="DIVERSOS",
            TotalValor:="0",
            DescFamilia:="MATERIAL",
            PercIPI:="0",
            vIPI:="0",
            PercICMS:="0",
            vICMS:="0",
            vLiquido:="0",
            D_E_L_E_T_E:="",
            txtTipoDesenho:="MATERIAL",
            NumeroRP:=codMatFabricante,
            UsuarioCriacao:=usuarioCriacao,
            DtCad:=Date.Now.Date,
            Comprimentocaixadelimitadora:="",
            Larguracaixadelimitadora:="",
            Espessuracaixadelimitadora:="",
            Altura:="",
            Largura:="",
            Profundidade:="",
            PesoMaisEmbalagem:="",
            Espessura:="",
            Materialsw:="",
            Autor:="",
            PalavraChave:=""
        )
        Else
            ' 3) se existir -> UPDATE via sua função (opcional, mas mantém seu padrão)
            classeMaterial.UpdateDados(
            IdMaterial:=idExistente,
            DescResumo:=desc,
            DescDetal:=desc,
            Peso:="0",
            Unidade:=um,
            CodMatFabricante:=codMatFabricante,
            CodigoJuridicoMat:="DIVERSOS",
            TotalValor:="0",
            DescFamilia:="MATERIAL",
            PercIPI:="0",
            vIPI:="0",
            PercICMS:="0",
            vICMS:="0",
            vLiquido:="0",
            D_E_L_E_T_E:="",
            txtTipoDesenho:="MATERIAL",
            NumeroRP:=codMatFabricante,
            Comprimentocaixadelimitadora:="",
            Larguracaixadelimitadora:="",
            Espessuracaixadelimitadora:="",
            Altura:="",
            Largura:="",
            Profundidade:="",
            PesoMaisEmbalagem:=""
        )
        End If

        ' 4) busca o id final
        'Dim idFinal As Integer = 0
        'Using cmd2 As New MySqlCommand("SELECT IdMaterial FROM material WHERE CodMatFabricante = @cod LIMIT 1;", myconect)
        '    cmd2.Parameters.AddWithValue("@cod", codMatFabricante)
        '    Dim obj2 = cmd2.ExecuteScalar()
        '    If obj2 IsNot Nothing AndAlso obj2 IsNot DBNull.Value Then
        '        Integer.TryParse(obj2.ToString(), idFinal)
        '    End If
        'End Using

        ' Return idFinal
    End Function

    Public Shared Function GerarTabelaTempEstrutura(produtoPai As String,
                                                idMaterialPai As Integer,
                                                idEmpresa As Integer,
                                                usuarioLogado As String,
                                                Optional tipoPeca As String = "E") As DataTable

        Dim dtTemp As New DataTable("TempEstrutura")

        ' colunas no formato do seu INSERT montapeca
        dtTemp.Columns.Add("TipoPeca", GetType(String))
        dtTemp.Columns.Add("IdMaterial", GetType(Integer))
        dtTemp.Columns.Add("PecaQtde", GetType(Decimal))
        dtTemp.Columns.Add("IdMaterialPeca", GetType(Integer))
        dtTemp.Columns.Add("IdEmpresa", GetType(Integer))
        dtTemp.Columns.Add("CodMatFabricante", GetType(String))
        dtTemp.Columns.Add("UsuarioCriacao", GetType(String))
        dtTemp.Columns.Add("DataCriacao", GetType(DateTime))

        ' extras (para grid/log)
        dtTemp.Columns.Add("DescComponente", GetType(String))
        dtTemp.Columns.Add("UM", GetType(String))
        dtTemp.Columns.Add("Tipo", GetType(String))
        dtTemp.Columns.Add("Grupo", GetType(String))
        dtTemp.Columns.Add("DataInicio", GetType(String))
        dtTemp.Columns.Add("DataFim", GetType(String))

        Dim dtBom As New DataTable()

        ' 1) busca Protheus (sua SQL parametrizada)
        Using conn As New NpgsqlConnection(connString)
            conn.Open()

            Dim sqlBom As String =
            "SELECT " &
            "    TRIM(g1.G1_COMP) AS componente, " &
            "    b1.B1_DESC       AS desc_componente, " &
            "    b1.B1_UM         AS um, " &
            "    b1.B1_TIPO       AS tipo, " &
            "    b1.B1_GRUPO      AS grupo, " &
            "    CAST(g1.G1_QUANT AS NUMERIC(18,6)) AS quantidade, " &
            "    TRIM(g1.G1_INI)  AS data_inicio, " &
            "    TRIM(g1.G1_FIM)  AS data_fim " &
            "FROM public.sg1010 g1 " &
            "LEFT JOIN public.sb1010 b1 " &
            "       ON TRIM(b1.B1_COD) = TRIM(g1.G1_COMP) " &
            "      AND COALESCE(b1.D_E_L_E_T_, '') = '' " &
            "WHERE TRIM(g1.G1_COD) = @produto " &
            "  AND COALESCE(g1.D_E_L_E_T_, '') = '' " &
            "ORDER BY TRIM(g1.G1_COMP);"

            Using cmd As New NpgsqlCommand(sqlBom, conn)
                cmd.Parameters.AddWithValue("@produto", produtoPai.Replace(".par", "").Replace(".psm", "").Replace(".asm", "").Trim())

                Using da As New NpgsqlDataAdapter(cmd)
                    da.Fill(dtBom)
                End Using
            End Using
        End Using

        ' 2) monta linhas já garantindo material no MySQL
        cl_BancoDados.AbrirBanco()

        For Each r As DataRow In dtBom.Rows
            Dim codComp As String = If(r("componente"), "").ToString().Trim().ToUpper().Replace(" ", "")
            If codComp = "" Then Continue For

            Dim descComp As String = If(r("desc_componente"), "").ToString()
            Dim umComp As String = If(r("um"), "").ToString()
            Dim tipoComp As String = If(r("tipo"), "").ToString()
            Dim grupoComp As String = If(r("grupo"), "").ToString()

            '  Dim dataIni As String = If(r("data_inicio"), "").ToString()
            '  Dim dataFim As String = If(r("data_fim"), "").ToString()

            Dim qtd As Decimal = 0D
            If r("quantidade") IsNot DBNull.Value Then
                If TypeOf r("quantidade") Is Decimal Then
                    qtd = CType(r("quantidade"), Decimal)
                Else
                    Decimal.TryParse(r("quantidade").ToString(),
                                 NumberStyles.Any,
                                 CultureInfo.InvariantCulture,
                                 qtd)
                End If
            End If

            ' ✅ garante cadastro do material e pega o ID
            Dim idMaterialComp As Integer = GarantirMaterialERetornarId(
            codMatFabricante:=codComp,
            desc:=descComp,
            um:=umComp,
            usuarioCriacao:=usuarioLogado
        )

            If idMaterialComp <= 0 Then
                ' se não conseguiu criar/localizar por algum motivo, não monta a linha
                Continue For
            End If

            Dim nova As DataRow = dtTemp.NewRow()
            nova("TipoPeca") = tipoPeca
            nova("IdMaterial") = idMaterialPai
            nova("PecaQtde") = qtd
            nova("IdMaterialPeca") = idMaterialComp
            nova("IdEmpresa") = idEmpresa
            nova("CodMatFabricante") = codComp
            nova("UsuarioCriacao") = usuarioLogado
            nova("DataCriacao") = DateTime.Now

            ' extras
            nova("DescComponente") = descComp
            nova("UM") = umComp
            nova("Tipo") = tipoComp
            nova("Grupo") = grupoComp
            nova("DataInicio") = "" ' dataIni
            nova("DataFim") = "" 'dataFim

            dtTemp.Rows.Add(nova)
        Next

        Return dtTemp
    End Function




End Class
