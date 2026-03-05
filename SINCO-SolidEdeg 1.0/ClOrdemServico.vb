Imports System.Data.SqlClient
Imports System.Windows.Forms
Imports MySql.Data.MySqlClient

Public Class CLOrdemServico

    Public IdOrdemServico As String
    Public IDOrdemServicoItem As Integer
    Public Projeto As String
    Public Tag As String
    Public idProjeto As String
    Public idTag As String
    Public DescTag As String
    Public nivel As String
    Public Descricao As String
    Public DescEmpresa As String
    Public EnderecoOrdemServico As String
    Public CriadoPor As String
    Public DataCriacao As Date
    Public Estatus As String
    Public IdMaterial As String
    Public DescResumo As String
    Public DescDetal As String
    Public Autor As String
    Public Palavrachave As String
    Public Notas As String
    Public Espessura As String
    Public AreaPintura As Double
    Public AreaPinturaUnitario As Double
    Public NumeroDobras As String
    Public Peso As Double
    Public PesoUnitario As Double
    Public Unidade As String
    Public UnidadeSW As String
    Public ValorSW As String
    Public Altura As String
    Public Largura As String
    Public CodMatFabricante As String
    Public DtCad As String
    Public UsuarioCriacao As String
    Public UsuarioAlteracao As String
    Public DtAlteracao As String
    Public EnderecoArquivo As String
    Public MaterialSW As String
    Public QtdeTotal As Double
    Public qtde As Double
    Public txtSoldagem As String
    Public txtTipoDesenho As String
    Public txtCorte As String
    Public txtDobra As String
    Public txtSolda As String
    Public txtPintura As String
    Public txtMontagem As String
    Public txtAcabamento As String
    Public tttxtCorte As String
    Public tttxtDobra As String
    Public tttxtSolda As String
    Public tttxtPintura As String
    Public tttxtMontagem As String
    Public DataPrevisao As String
    Public ProdutoPrincipal As String
    Public Fator As String
    Public idempresa As String

    Public QtdeTag As Integer
    Public QtdeLiberada As Integer
    Public SaldoTag As Integer

    Public Liberado_Engenharia As String
    Public Data_Liberacao_Engenharia As String
    Public IdOSReferencia As String

    Public Comprimentocaixadelimitadora As String
    Public Larguracaixadelimitadora As String
    Public Espessuracaixadelimitadora As String
    Public txtItemEstoque As String

    Public RNC As String

    Public ProdutoPadrao As String
    Public CodDesenhoProduto As String
    Public CodOmie As String
    Public DescricaoProduto As String
    Public EnderecoFichaTecnica As String
    Public EnderecoIsometrico As String
    Public ProdutoCriadoPor As String
    Public DataCriacaoProduto As String

    Public bloqueado As String

    Public Function CriarOsCompleta(ByVal DgvGrid As DataGridView, ByVal timerDgvOS As Timer, ByVal timerDgvOSiTEM As Timer) As Boolean

        If My.Settings.EnderecoPastaRaizOS.ToString = "" And System.IO.Directory.Exists(My.Settings.EnderecoPastaRaizOS) = False Then

            MsgBox("O endereço onde será criado a pasta da Ordem de Serviço  não foi informado!")
            Exit Function
        Else

            Try

                'If Tag = "" Or Projeto = "" Or Descricao = "" Then

                '    MsgBox("O Projeto, Tag e ou uma descrição devem ser informados", vbInformation, "Atenção")
                'Else

                ''''''' OrdemServico.idProjeto = Nothing
                ''''''OrdemServico.Projeto = Projeto
                '''''''   OrdemServico.idTag = Nothing
                ''''''OrdemServico.Tag = Tag.ToUpper
                ''''''OrdemServico.Descricao = Descricao.ToUpper
                OrdemServico.CriadoPor = Usuario.NomeCompleto
                OrdemServico.DataCriacao = Date.Now.ToString("dd/MM/yyyy")
                OrdemServico.Estatus = "A".ToUpper
                ''''''OrdemServico.idProjeto = idProjeto
                ''''''OrdemServico.idTag = idTag
                ''''''OrdemServico.DescEmpresa = DescEmpresa
                ''''''OrdemServico.Liberado_Engenharia = ""
                ''''''OrdemServico.Data_Liberacao_Engenharia = DescEmpresa
                ''''''OrdemServico.IdOSReferencia = ""

                If OrdemServico Is Nothing OrElse
   OrdemServico.IdOrdemServico Is Nothing OrElse
   String.IsNullOrWhiteSpace(OrdemServico.IdOrdemServico.ToString()) OrElse
   OrdemServico.IdOrdemServico.ToString() = "0" Then

                    Dim idosRetono As Integer

                    Try
                        VCampo0 = ""
                        cl_BancoDados.RetornaCampoDaPesquisa("SELECT IdOrdemServico from  " & ComplementoTipoBanco & "ordemservico where IdOrdemServico = '" & OrdemServico.IdOrdemServico & "'", "IdOrdemServico")

                        idosRetono = Convert.ToInt32(VCampo0)
                    Catch ex As Exception
                        idosRetono = 0
                    Finally
                    End Try

                    If idosRetono = 0 Then

                        Try
                            VCampo0 = ""
                            cl_BancoDados.RetornaCampoDaPesquisa("SELECT max(IdOrdemServico)  as NovoIdOrdemServico FROM  " & ComplementoTipoBanco & "ordemservico", "NovoIdOrdemServico")

                            Dim NovoIdOrdemServicoDB As Integer = Convert.ToInt32(VCampo0) + 1

                            NovoIdOrdemServico = cl_BancoDados.FormatarPara5Caracteres(NovoIdOrdemServicoDB.ToString())
                        Catch ex As Exception

                            ' Em caso de erro, atribuir "00001" como valor inicial
                            NovoIdOrdemServico = "00001"

                        End Try

                        OrdemServico.EnderecoOrdemServico = (My.Settings.EnderecoPastaRaizOS & "\OS_" & NovoIdOrdemServico).ToString.ToUpper

                        System.IO.Directory.CreateDirectory(My.Settings.EnderecoPastaRaizOS & "\OS_" & NovoIdOrdemServico)
                        System.IO.Directory.CreateDirectory(My.Settings.EnderecoPastaRaizOS & "\OS_" & NovoIdOrdemServico & "\DXF")
                        System.IO.Directory.CreateDirectory(My.Settings.EnderecoPastaRaizOS & "\OS_" & NovoIdOrdemServico & "\PDF")
                        System.IO.Directory.CreateDirectory(My.Settings.EnderecoPastaRaizOS & "\OS_" & NovoIdOrdemServico & "\DFT")
                        System.IO.Directory.CreateDirectory(My.Settings.EnderecoPastaRaizOS & "\OS_" & NovoIdOrdemServico & "\PUNC")
                        System.IO.Directory.CreateDirectory(My.Settings.EnderecoPastaRaizOS & "\OS_" & NovoIdOrdemServico & "\LASER")
                        System.IO.Directory.CreateDirectory(My.Settings.EnderecoPastaRaizOS & "\OS_" & NovoIdOrdemServico & "\Projeto")
                        System.IO.Directory.CreateDirectory(My.Settings.EnderecoPastaRaizOS & "\OS_" & NovoIdOrdemServico & "\PEÇAS DE ESTOQUE")
                        System.IO.Directory.CreateDirectory(My.Settings.EnderecoPastaRaizOS & "\OS_" & NovoIdOrdemServico & "\LXDS")
                        System.IO.Directory.CreateDirectory(My.Settings.EnderecoPastaRaizOS & "\OS_" & NovoIdOrdemServico & "\IGES")

                        OrdemServico.Liberado_Engenharia = ""
                        OrdemServico.Data_Liberacao_Engenharia = ""
                        OrdemServico.IdOSReferencia = OrdemServico.IdOrdemServico

                        Try

                            cl_BancoDados.RetornaCampoDaPesquisa("Select DataPrevisao from  " & ComplementoTipoBanco & "tags where idTag = '" & OrdemServico.idTag & "'", "DataPrevisao")

                            OrdemServico.DataPrevisao = VCampo0
                        Catch ex As Exception
                            OrdemServico.DataPrevisao = ""
                        End Try

                        SalvarOrdeMServicoBanco()

                        timerDgvOS.Enabled = True

                        MsgBox("Ordem de Serviço Criada com sucesso!")

                    End If
                Else

                        'Altera dos dados da Ordem de serviço
                        cl_BancoDados.Salvar("update ordemservico set Descricao = '" & OrdemServico.Descricao & "',
Projeto = '" & OrdemServico.Projeto & "',
Tag = '" & OrdemServico.Tag & "',
idProjeto = '" & OrdemServico.idProjeto & "',
idTag = '" & OrdemServico.idTag & "',
Fator = '" & OrdemServico.Fator & "',
DataPrevisao = '" & OrdemServico.DataPrevisao & "',
DescEmpresa = '" & OrdemServico.DescEmpresa & "',
idEmpresa = '" & OrdemServico.idempresa & "',
desctag = '" & OrdemServico.DescTag & "'
where IdOrdemServico = '" & OrdemServico.IdOrdemServico & "';
update  " & ComplementoTipoBanco & "ordemservicoitem set Projeto = '" & OrdemServico.Projeto & "',
Tag = '" & OrdemServico.Tag & "',
idProjeto = '" & OrdemServico.idProjeto & "',
DataPrevisao = '" & OrdemServico.DataPrevisao & "',
idTag = '" & OrdemServico.idTag & "',
DescEmpresa = '" & OrdemServico.DescEmpresa & "',
idEmpresa = '" & OrdemServico.idempresa & "',
desctag = '" & OrdemServico.DescTag & "'
where IdOrdemServico = '" & OrdemServico.IdOrdemServico & "'") ' and idProjeto = '" & OrdemServico.idProjeto & "' and idTag = '" & OrdemServico.idTag & "'")

                        'inicio teste query unica 25/09/2025

                        '''                        'Altera dos dados da Ordem de serviço
                        '''                        cl_BancoDados.Salvar("update ordemservico set Descricao = '" & OrdemServico.Descricao & "',
                        '''Projeto = '" & OrdemServico.Projeto & "',
                        '''Tag = '" & OrdemServico.Tag & "',
                        '''idProjeto = '" & OrdemServico.idProjeto & "',
                        '''idTag = '" & OrdemServico.idTag & "',
                        '''Fator = '" & OrdemServico.Fator & "',
                        '''DataPrevisao = '" & OrdemServico.DataPrevisao & "',
                        '''DescEmpresa = '" & OrdemServico.DescEmpresa & "',
                        '''idEmpresa = '" & OrdemServico.idempresa & "',
                        '''desctag = '" & OrdemServico.DescTag & "'
                        '''where IdOrdemServico = '" & OrdemServico.IdOrdemServico & "'")

                        '''                        'Altera dos dados dos itens da Ordem de serviço
                        '''                        cl_BancoDados.Salvar("update  " & ComplementoTipoBanco & "ordemservicoitem set Projeto = '" & OrdemServico.Projeto & "',
                        '''Tag = '" & OrdemServico.Tag & "',
                        '''idProjeto = '" & OrdemServico.idProjeto & "',
                        '''DataPrevisao = '" & OrdemServico.DataPrevisao & "',
                        '''idTag = '" & OrdemServico.idTag & "',
                        '''DescEmpresa = '" & OrdemServico.DescEmpresa & "',
                        '''idEmpresa = '" & OrdemServico.idempresa & "',
                        '''desctag = '" & OrdemServico.DescTag & "'
                        '''where IdOrdemServico = '" & OrdemServico.IdOrdemServico & "'") ' and idProjeto = '" & OrdemServico.idProjeto & "' and idTag = '" & OrdemServico.idTag & "'")

                        ', , Descricao, Estatus, , , ,  FROM ordemservico o;

                        DgvGrid.CurrentRow.Cells("Projeto").Value = OrdemServico.Projeto

                        DgvGrid.CurrentRow.Cells("Tag").Value = OrdemServico.Tag

                        DgvGrid.CurrentRow.Cells("idProjeto").Value = OrdemServico.idProjeto

                        DgvGrid.CurrentRow.Cells("idTag").Value = OrdemServico.idTag

                        DgvGrid.CurrentRow.Cells("Descricao").Value = OrdemServico.Descricao

                        DgvGrid.CurrentRow.Cells("DescEmpresa").Value = OrdemServico.DescEmpresa

                        DgvGrid.CurrentRow.Cells("DataPrevisao").Value = OrdemServico.DataPrevisao

                        MsgBox("Ordem de serviço alterada com sucesso!")

                End If

                ' End If

                timerDgvOSiTEM.Enabled = True
            Catch ex As Exception

                MsgBox("Erro ao criar a Ordem de Serviço" & ex.Message)
            Finally
            End Try

        End If

    End Function

    '    Public Function TotaisPecasOrdemServico(ByVal IdOrdemServico As Integer)

    '        cl_BancoDados.RetornaCampoDaPesquisa("SELECT
    '  SUM(CASE WHEN txtCorte = '1' THEN QtdeTotal ELSE 0 END) AS CorteTotalExecutar,
    '  SUM(CASE WHEN txtDobra = '1' THEN QtdeTotal ELSE 0 END) AS DobraTotalExecutar,
    '  SUM(CASE WHEN txtSolda = '1' THEN QtdeTotal ELSE 0 END) AS SoldaTotalExecutar,
    '  SUM(CASE WHEN txtPintura = '1' THEN QtdeTotal ELSE 0 END) AS PinturaTotalExecutar,
    '  SUM(CASE WHEN txtMontagem = '1' AND ProdutoPrincipal = 'SIM' THEN QtdeTotal ELSE 0 END) AS MontagemTotalExecutar,
    '  count(CASE WHEN txtTipoDesenho = 'CHAPARIA' THEN QtdeTotal ELSE 0 END) AS QtdeTotalItens,
    '  SUM(CASE WHEN txtTipoDesenho = 'CHAPARIA' THEN QtdeTotal ELSE 0 END) AS QtdeTotalPecas,
    '  SUM(areapintura) AS areapinturatotal,
    '  SUM(peso) AS pesototal
    'FROM ordemservicoitem
    'WHERE (D_E_L_E_T_E IS NULL OR D_E_L_E_T_E = '')
    '  AND IdOrdemServico = '" & OrdemServico.IdOrdemServico & "'",
    '                                             "CorteTotalExecutar",
    '                                             "DobraTotalExecutar",
    '                                             "SoldaTotalExecutar",
    '                                             "PinturaTotalExecutar",
    '                                             "MontagemTotalExecutar",
    '                                             "QtdeTotalItens",
    '                                             "QtdeTotalPecas",
    '                                             "areapinturatotal",
    '                                             "pesototal")

    '        cl_BancoDados.Salvar("Update ordemservico set Liberado_Engenharia = 'S',
    '                Data_Liberacao_Engenharia = '" & Date.Now & "',
    '                CorteTotalExecutar = '" & VCampo0 & "',
    '                 DobraTotalExecutar = '" & VCampo1 & "',
    '                  SoldaTotalExecutar = '" & VCampo2 & "',
    '                   PinturaTotalExecutar = '" & VCampo3 & "',
    '                    MontagemTotalExecutar = '" & VCampo4 & "',
    '                    QtdeTotalItens = '" & VCampo5 & "',
    '                    QtdeTotalPecas = '" & VCampo6 & "',
    '                    areapinturatotal = '" & VCampo7 & "',
    '                    pesototal = '" & VCampo8 & "'
    '						where IdOrdemServico = '" & IdOrdemServico & "'")

    '    End Function

    Public Function SalvarOrdeMServicoBanco()

        cl_BancoDados.AbrirBanco()

        If My.Settings.TipoConexao = "MYSQL" Then

            Try

                Dim cmd As New MySqlCommand("insert into  " & ComplementoTipoBanco & "ordemservico
    (idProjeto, Projeto, idTag, Tag, Descricao, EnderecoOrdemServico,
    CriadoPor, DataCriacao, Estatus, D_E_L_E_T_E, Liberado_Engenharia,
    Data_Liberacao_Engenharia, IdOSReferencia, DescEmpresa, DataPrevisao, Fator,idempresa,DescTag)
    values
    (@idProjeto, @Projeto, @idTag, @Tag, @Descricao, @EnderecoOrdemServico,
    @CriadoPor, @DataCriacao, @Estatus, @D_E_L_E_T_E, @Liberado_Engenharia,
    @Data_Liberacao_Engenharia, @IdOSReferencia, @DescEmpresa, @DataPrevisao, @Fator,@idempresa,@DescTag)", myconect)

                cmd.Parameters.AddWithValue("@idProjeto", OrdemServico.idProjeto)
                cmd.Parameters.AddWithValue("@Projeto", OrdemServico.Projeto)
                cmd.Parameters.AddWithValue("@idTag", OrdemServico.idTag)
                cmd.Parameters.AddWithValue("@Tag", OrdemServico.Tag)
                cmd.Parameters.AddWithValue("@Descricao", OrdemServico.Descricao)
                cmd.Parameters.AddWithValue("@EnderecoOrdemServico", OrdemServico.EnderecoOrdemServico)
                cmd.Parameters.AddWithValue("@CriadoPor", If(String.IsNullOrEmpty(OrdemServico.CriadoPor), DBNull.Value, OrdemServico.CriadoPor.ToUpper().ToString).ToString)
                cmd.Parameters.AddWithValue("@DataCriacao", OrdemServico.DataCriacao.ToString("dd/MM/yyyy"))
                cmd.Parameters.AddWithValue("@Estatus", OrdemServico.Estatus)
                cmd.Parameters.AddWithValue("@D_E_L_E_T_E", "")  ' ou tratar conforme necessário
                cmd.Parameters.AddWithValue("@Liberado_Engenharia", OrdemServico.Liberado_Engenharia)
                cmd.Parameters.AddWithValue("@Data_Liberacao_Engenharia", OrdemServico.Data_Liberacao_Engenharia)
                cmd.Parameters.AddWithValue("@IdOSReferencia", OrdemServico.IdOSReferencia)
                cmd.Parameters.AddWithValue("@DescEmpresa", OrdemServico.DescEmpresa)
                cmd.Parameters.AddWithValue("@DataPrevisao", OrdemServico.DataPrevisao)
                cmd.Parameters.AddWithValue("@Fator", OrdemServico.Fator)
                cmd.Parameters.AddWithValue("@idempresa", OrdemServico.idempresa)
                cmd.Parameters.AddWithValue("@DescTag", OrdemServico.DescTag)

                cmd.ExecuteNonQuery()
            Catch ex As Exception

                MsgBox(ex.Message)
            Finally

            End Try



        End If

        cl_BancoDados.FecharBanco()

    End Function

End Class

Public Class CLOrdemServicoItem

    Public IDOrdemServicoItem As Integer
    Public IdOrdemServico As Integer
    Public Projeto As String
    Public Tag As String
    Public ESTATUS_OrdemServico As String
    Public IdMaterial As Integer
    Public QtdeTotal As Double
    Public CriadoPor As String
    Public DataCriacao As String
    Public Estatus As String
    Public Acabamento As String
    Public PrevDataEntrega As String

End Class

Public Class CLOrdemServicoItemPendencia

    Public idordemservicoitempendencia As Integer
    Public IDOrdemServicoItem As Integer
    Public IdOrdemServico As Integer
    Public IdMaterial As Integer
    Public DescricaoPendencia As String
    Public DescricaoFinalizacao As String
    Public Usuario As String
    Public DataCriacao As String
    Public D_E_L_E_T_E As String
    Public UsuarioProjeto As String
    Public DataAcertoProjet As String
    Public estatu As String

End Class

Public Class clProjeto

    Public idProjeto As String
    Public Projeto As String
    Public DescProjeto As String
    Public Responsavel As String
    Public DescEmpresa As String
    Public DataEntrada As String
    Public DataPrevisao As String
    Public DataTermino As String
    Public TotalProjeto As String
    Public StatusProj As String
    Public D_E_L_E_T_E As String
    Public DescStatus As String
    Public IdEmpresa As String
    Public liberado As String
    Public UsuarioD_E_L_E_T_E As String
    Public DataD_E_L_E_T_E As String




    Public Sub SalvarDados(IdProjeto, Projeto, DescProjeto, Responsavel, Comercial,
                           PlanejadoComercial, Financeiro, Dataprevisao, PlanejadoFinanceiro, PrazoEntrega,
                                      IdCliente, Cliente, DataEntradaPedido, Estado, EnderecoProjeto, Liberado)

        Dim sucesso As Boolean
        Try
            sucesso = False
            Dim query As String = "INSERT INTO projetos (Projeto, DescProjeto, Responsavel,Criadopor,DataCriacao, Liberado_Comercial,DataPlanejadoComercial,Liberado_Financeiro,Dataprevisao,DataPlanejadoFinanceiro,PrazoEntrega,
                                                          IdEmpresa,DescEmpresa,DataEntradaPedido,Estado,EnderecoProjeto,Liberado) VALUES
                                                        (@Projeto, @descProjeto, @responsavel,@CriadoPor,@DataCriacao,@Liberado_Comercial,@DataPlanejadoComercial,@Liberado_Financeiro,@Dataprevisao,@DataPlanejadoFinanceiro,@PrazoEntrega,
                                                          @IdEmpresa,@DescEmpresa,@DataEntradaPedido,@Estado,@EnderecoProjeto,@Liberado)"

            cl_BancoDados.AbrirBanco()

            Using da As New MySqlCommand(query, myconect)

                ' Defina os valores para os parâmetros
                da.Parameters.AddWithValue("@Projeto", Projeto)
                da.Parameters.AddWithValue("@descProjeto", DescProjeto)
                da.Parameters.AddWithValue("@responsavel", Responsavel)
                da.Parameters.AddWithValue("@Liberado_Comercial", Comercial)
                da.Parameters.AddWithValue("@DataPlanejadoComercial", PlanejadoComercial)
                da.Parameters.AddWithValue("@Liberado_Financeiro", Financeiro)
                da.Parameters.AddWithValue("@Dataprevisao", Dataprevisao)
                da.Parameters.AddWithValue("@DataPlanejadoFinanceiro", PlanejadoFinanceiro)
                da.Parameters.AddWithValue("@PrazoEntrega", PrazoEntrega)
                da.Parameters.AddWithValue("@CriadoPor", Usuario.NomeCompleto)
                da.Parameters.AddWithValue("@DataCriacao", Date.Now.Date)
                da.Parameters.AddWithValue("@IdEmpresa", 0)
                da.Parameters.AddWithValue("@DescEmpresa", Cliente)
                da.Parameters.AddWithValue("@DataEntradaPedido", DataEntradaPedido)
                da.Parameters.AddWithValue("@Estado", Estado)
                da.Parameters.AddWithValue("@EnderecoProjeto", EnderecoProjeto)
                da.Parameters.AddWithValue("@Liberado", Liberado)

                da.ExecuteNonQuery()
                sucesso = True
            End Using
            cl_BancoDados.FecharBanco()
        Catch ex As Exception
            cl_BancoDados.FecharBanco()
        End Try

    End Sub

    Public Sub UpdateDados(IdProjeto, Projeto, DescProjeto, Responsavel, PrazoEntrega, IdEmpresa,
                           DescEmpresa, DataPrevisao, DataEntradaPedido,
                           DataPlanejadoFinanceiro, Estado)
        'Public Sub UpdateDados(IdProjeto, Projeto, DescProjeto, Responsavel, Cliente, DataEntrada, DataLiberacao, DataPrazoLiberacao, DataImplantacao, DataPrevisao, Estado)
        Dim sucesso As Boolean
        Try
            sucesso = False
            Dim query As String = "UPDATE projetos SET Projeto = @Projeto,
                                          DescProjeto = @descProjeto,
                                          Responsavel = @responsavel,
                                          PrazoEntrega = @PrazoEntrega,
                                          IdEmpresa = @IdEmpresa,
                                          DescEmpresa = @DescEmpresa,
                                          DataPrevisao = @DataPrevisao,
                                          DataEntradaPedido = @DataEntradaPedido,
                                          DataPlanejadoFinanceiro = @DataPlanejadoFinanceiro,
                                          Estado = @Estado
                                  WHERE IdProjeto = @IdProjeto"

            cl_BancoDados.AbrirBanco()

            Using da As New MySqlCommand(query, myconect)
                ' Defina os valores para os parâmetros
                da.Parameters.AddWithValue("@IdProjeto", IdProjeto)
                da.Parameters.AddWithValue("@Projeto", Projeto)
                da.Parameters.AddWithValue("@descProjeto", DescProjeto)
                da.Parameters.AddWithValue("@responsavel", Responsavel)
                da.Parameters.AddWithValue("@PrazoEntrega", PrazoEntrega)
                da.Parameters.AddWithValue("@IdEmpresa", IdEmpresa)
                da.Parameters.AddWithValue("@DescEmpresa", DescEmpresa)
                da.Parameters.AddWithValue("@DataPrevisao", DataPrevisao)
                da.Parameters.AddWithValue("@DataEntradaPedido", DataEntradaPedido)
                da.Parameters.AddWithValue("@DataPlanejadoFinanceiro", DataPlanejadoFinanceiro)
                da.Parameters.AddWithValue("@Estado", Estado)

                da.ExecuteNonQuery()
                sucesso = True
            End Using
            cl_BancoDados.FecharBanco()
        Catch ex As Exception
            cl_BancoDados.FecharBanco()

        End Try
    End Sub



End Class

Public Class clProcesso

    Public IdProcesso As Integer
    Public IdMaterial As Integer
    Public DescProcesso As String
    Public SequenciaExecucao As Integer

    Public TempoPadraoMin As String
    Public Ativo As String
    Public Observacao As String


    Public Function SalvarOrdeMServicoBanco(IdProcesso, IdMaterial, SequenciaExecucao, codMatFabricante)

        cl_BancoDados.AbrirBanco()

        Try

            Dim cmd As New MySqlCommand("insert into  " & ComplementoTipoBanco & "material_processo
    (IdMaterial, IdProcesso, SequenciaExecucao, TempoPadraoMin, Ativo, Observacao, DataCriacao, UsuarioCriacao, codMatFabricante)
    values
    (@IdMaterial, @IdProcesso, @SequenciaExecucao, @TempoPadraoMin, @Ativo, @Observacao, @DataCriacao, @UsuarioCriacao,@codMatFabricante)", myconect)

            cmd.Parameters.AddWithValue("@IdMaterial", IdMaterial)
            cmd.Parameters.AddWithValue("@IdProcesso", IdProcesso)
            cmd.Parameters.AddWithValue("@SequenciaExecucao", SequenciaExecucao)
            cmd.Parameters.AddWithValue("@TempoPadraoMin", "0")
            cmd.Parameters.AddWithValue("@Ativo", "S")
            cmd.Parameters.AddWithValue("@Observacao", "")
            cmd.Parameters.AddWithValue("@UsuarioCriacao", Usuario.NomeCompleto)
            cmd.Parameters.AddWithValue("@DataCriacao", Date.Now)
            cmd.Parameters.AddWithValue("@codMatFabricante", codMatFabricante)

            cmd.ExecuteNonQuery()

            cl_BancoDados.FecharBanco()

        Catch ex As Exception

            '   MsgBox(ex.Message)
        Finally

        End Try


    End Function


End Class



Public Class CLPecaManufaturada

    'IdMontaPeca, TipoPeca, IdMaterial, PecaQtde, IdMaterialPeca, IdEmpresa, D_E_L_E_T_E, Peso, Valor,
    'UsuarioD_E_L_E_T_E, DataD_E_L_E_T_E, CodMatFabricante, UsuarioCriacao, DataCriacao

    Public IdMontaPeca As Integer
    Public TipoPeca As Integer
    Public IdMaterial As Integer
    Public PecaQtde As String
    Public IdMaterialPeca As Integer
    Public IdEmpresa As Integer
    Public D_E_L_E_T_E As String
    Public Peso As String
    Public Valor As String
    Public UsuarioD_E_L_E_T_E As String
    Public DataD_E_L_E_T_E As String
    Public CodMatFabricante As String
    Public UsuarioCriacao As String
    Public DataCriacao As String

    Public Sub SalvarDados(IdMontapeca, TipoPeca, IdMaterial, PecaQtde, IdMaterialPeca, IdEmpresa, D_E_L_E_T_E, Peso, Valor, UsuarioD_E_L_E_T_E, DataD_E_L_E_T_E, CodMatFabricante, UsuarioCriacao, DataCriacao)

        Try

            Dim query As String = "INSERT INTO montapeca (TipoPeca, IdMaterial, PecaQtde, IdMaterialPeca, IdEmpresa, D_E_L_E_T_E, Peso, Valor,
                                                          UsuarioD_E_L_E_T_E, DataD_E_L_E_T_E, CodMatFabricante, UsuarioCriacao, DataCriacao)     VALUES
                                                      (@TipoPeca, @IdMaterial, @PecaQtde, @IdMaterialPeca, @IdEmpresa, @D_E_L_E_T_E, @Peso, @Valor,
                                                          @UsuarioD_E_L_E_T_E, @DataD_E_L_E_T_E, @CodMatFabricante, @UsuarioCriacao, @DataCriacao)"

            cl_BancoDados.AbrirBanco()

            Using da As New MySqlCommand(query, myconect)
                ' Defina os valores para os parâmetros

                da.Parameters.AddWithValue("@TipoPeca", TipoPeca)
                da.Parameters.AddWithValue("@IdMaterial", IdMaterial)
                da.Parameters.AddWithValue("@PecaQtde", PecaQtde)
                da.Parameters.AddWithValue("@IdMaterialPeca", IdMaterialPeca)
                da.Parameters.AddWithValue("@IdEmpresa", IdEmpresa)
                da.Parameters.AddWithValue("@D_E_L_E_T_E", D_E_L_E_T_E)
                da.Parameters.AddWithValue("@Peso", Peso)
                da.Parameters.AddWithValue("@Valor", Valor)
                da.Parameters.AddWithValue("@UsuarioD_E_L_E_T_E", UsuarioD_E_L_E_T_E)
                da.Parameters.AddWithValue("@DataD_E_L_E_T_E", DataD_E_L_E_T_E)
                da.Parameters.AddWithValue("@CodMatFabricante", CodMatFabricante)
                da.Parameters.AddWithValue("@UsuarioCriacao", UsuarioCriacao)
                da.Parameters.AddWithValue("@DataCriacao", DataCriacao)

                da.ExecuteNonQuery()

            End Using
        Catch ex As Exception

            ' Dim errorMessage As String = $"Erro no controle: ' {ex.Message}"
            'errorMessages.Add(errorMessage)
            'MsgBox(ex.Message)
        Finally ' Tratar exceções aqui conforme necessário
        End Try

    End Sub



    Public Sub SalvarDadosGabarito(idmaterial, idmaterial_desenho, descricao)

        Try

            Dim query As String = "INSERT INTO material_gabarito (idmaterial, idmaterial_desenho, descricao)     VALUES
                                                      (@idmaterial, @idmaterial_desenho, @descricao)"

            cl_BancoDados.AbrirBanco()

            Using da As New MySqlCommand(query, myconect)
                ' Defina os valores para os parâmetros

                da.Parameters.AddWithValue("@IdMaterial", idmaterial)
                da.Parameters.AddWithValue("@idmaterial_desenho", idmaterial_desenho)
                da.Parameters.AddWithValue("@descricao", descricao)


                da.ExecuteNonQuery()

            End Using
        Catch ex As Exception

            ' Dim errorMessage As String = $"Erro no controle: ' {ex.Message}"
            'errorMessages.Add(errorMessage)
            'MsgBox(ex.Message)
        Finally ' Tratar exceções aqui conforme necessário
        End Try

    End Sub


End Class


Public Class clMaterial

    Public IdMaterialPeca As String
    Public idMaterial As String
    Public PecaManufat As String
    Public EnderecoLogo As String
    Public DescResumo As String
    Public codmatFabricante As String

    Public CodMatFabricanteGrupo As String

    Public idDimensionalReferencia As Integer

    Public TipoCota As String

    Public NomeCota As String

    Public ValorCota As String

    Public Tolerancia As String


    Public Sub SalvarDadosMaterial(DescResumo, DescDetal, Peso, Unidade, CodMatFabricante, CodigoJuridicoMat,
                                                     TotalValor, DescFamilia, PercIPI, vIPI, PercICMS,
                                                 vICMS, vLiquido, D_E_L_E_T_E, txtTipoDesenho, NumeroRP,
                                                 UsuarioCriacao, DtCad, Comprimentocaixadelimitadora,
                                                Larguracaixadelimitadora, Espessuracaixadelimitadora,
                                                Altura, Largura, Profundidade, PesoMaisEmbalagem, Espessura, Materialsw, Autor, PalavraChave)
        Try
            Dim query As String = "INSERT INTO material (DescResumo,DescDetal, Peso, Unidade, CodMatFabricante, CodigoJuridicoMat,
                                                     TotalValor, DescFamilia, PercIPI, vIPI, PercICMS,  vICMS, vLiquido,
                                                     D_E_L_E_T_E,txtTipoDesenho,NumeroRP,UsuarioCriacao, DtCad, PecaManuFat,
                                                     Comprimentocaixadelimitadora, Larguracaixadelimitadora, Espessuracaixadelimitadora,
                                                     Altura, Largura, Profundidade,PesoMaisEmbalagem,Espessura, Materialsw,Autor,PalavraChave)
                                             VALUES
                                                    (@DescResumo,@DescDetal, @Peso, @Unidade, @CodMatFabricante, @CodigoJuridicoMat,
                                                     @TotalValor, @DescFamilia, @PercIPI, @vIPI, @PercICMS,  @vICMS, @vLiquido,
                                                     @D_E_L_E_T_E, @txtTipoDesenho,@NumeroRP,@UsuarioCriacao, @DtCad,@PecaManuFat,
                                                     @Comprimentocaixadelimitadora, @Larguracaixadelimitadora, @Espessuracaixadelimitadora,
                                                     @Altura, @Largura, @Profundidade,@PesoMaisEmbalagem,@Espessura,@Materialsw,@Autor,@PalavraChave)"

            cl_BancoDados.AbrirBanco()

            Using da As New MySqlCommand(query, myconect)
                ' Defina os valores para os parâmetros
                da.Parameters.AddWithValue("@DescResumo", DescDetal)
                da.Parameters.AddWithValue("@DescDetal", DescDetal)
                da.Parameters.AddWithValue("@Peso", Peso)
                da.Parameters.AddWithValue("@Unidade", Unidade)
                da.Parameters.AddWithValue("@CodMatFabricante", CodMatFabricante)
                da.Parameters.AddWithValue("@CodigoJuridicoMat", CodigoJuridicoMat)
                da.Parameters.AddWithValue("@TotalValor", TotalValor)
                da.Parameters.AddWithValue("@DescFamilia", DescFamilia)
                da.Parameters.AddWithValue("@PercIPI", PercIPI)
                da.Parameters.AddWithValue("@vIPI", vIPI)
                da.Parameters.AddWithValue("@PercICMS", PercICMS)
                da.Parameters.AddWithValue("@vICMS", vICMS)
                da.Parameters.AddWithValue("@vLiquido", vLiquido)
                da.Parameters.AddWithValue("@D_E_L_E_T_E", D_E_L_E_T_E)
                da.Parameters.AddWithValue("@NumeroRP", NumeroRP)
                da.Parameters.AddWithValue("@txtTipoDesenho", txtTipoDesenho)
                da.Parameters.AddWithValue("@UsuarioCriacao", UsuarioCriacao)
                da.Parameters.AddWithValue("@DtCad", DtCad)
                da.Parameters.AddWithValue("@PecaManuFat", "")

                da.Parameters.AddWithValue("@Comprimentocaixadelimitadora", Comprimentocaixadelimitadora)
                da.Parameters.AddWithValue("@Larguracaixadelimitadora", Larguracaixadelimitadora)
                da.Parameters.AddWithValue("@Espessuracaixadelimitadora", Espessuracaixadelimitadora)
                da.Parameters.AddWithValue("@Altura", Altura)
                da.Parameters.AddWithValue("@Largura", Largura)
                da.Parameters.AddWithValue("@Profundidade", Profundidade)
                da.Parameters.AddWithValue("@PesoMaisEmbalagem", PesoMaisEmbalagem)
                da.Parameters.AddWithValue("@Espessura", Espessura)
                da.Parameters.AddWithValue("@Materialsw", Materialsw)
                da.Parameters.AddWithValue("@Autor", Autor)
                da.Parameters.AddWithValue("@PalavraChave", PalavraChave)

                da.ExecuteNonQuery()

            End Using
            cl_BancoDados.FecharBanco()
        Catch ex As Exception
            cl_BancoDados.FecharBanco()
            'Dim errorMessage As String = $"Erro no controle: ' {ex.Message}"
            'errorMessages.Add(errorMessage)
        Finally

        End Try

    End Sub

    Public Sub UpdateDados(IdMaterial, DescResumo, DescDetal, Peso, Unidade, CodMatFabricante, CodigoJuridicoMat,
                                                     TotalValor, DescFamilia, PercIPI, vIPI, PercICMS,
                                                     vICMS, vLiquido, D_E_L_E_T_E, txtTipoDesenho, NumeroRP,
                                                            Comprimentocaixadelimitadora, Larguracaixadelimitadora, Espessuracaixadelimitadora,
                                                     Altura, Largura, Profundidade, PesoMaisEmbalagem)
        Try

            Dim query As String = "UPDATE material SET
                                DescResumo = @DescResumo,
                                DescDetal = @DescDetal,
                                Peso = @Peso,
                                Unidade = @Unidade,
                                CodMatFabricante = @CodMatFabricante,
                                CodigoJuridicoMat = @CodigoJuridicoMat,
                                TotalValor = @TotalValor,
                                DescFamilia = @DescFamilia,
                                PercIPI = @PercIPI,
                                vIPI = @vIPI,
                                PercICMS = @PercICMS,
                                vICMS = @vICMS,
                                vLiquido = @vLiquido,
                                D_E_L_E_T_E = @D_E_L_E_T_E,
                                txtTipoDesenho = @txtTipoDesenho,
                                NumeroRP = @NumeroRP,
                                Comprimentocaixadelimitadora = @Comprimentocaixadelimitadora,
                                Larguracaixadelimitadora = @Larguracaixadelimitadora,
                                Espessuracaixadelimitadora = @Espessuracaixadelimitadora,
                                Altura = @Altura,
                                Largura = @Largura,
                                Profundidade = @Profundidade,
                                PesoMaisEmbalagem = @PesoMaisEmbalagem
                                WHERE IdMaterial = @IdMaterial"

            'If CodMatFabricante = "PRD09303" Then
            '    MsgBox(CodMatFabricante)

            'End If
            cl_BancoDados.AbrirBanco()

            Using da As New MySqlCommand(query, myconect)
                ' Defina os valores para os parâmetros
                da.Parameters.AddWithValue("@IdMaterial", IdMaterial)
                da.Parameters.AddWithValue("@DescResumo", DescDetal)
                da.Parameters.AddWithValue("@DescDetal", DescDetal)
                da.Parameters.AddWithValue("@Peso", Peso)
                da.Parameters.AddWithValue("@Unidade", Unidade)
                da.Parameters.AddWithValue("@CodMatFabricante", CodMatFabricante)
                da.Parameters.AddWithValue("@CodigoJuridicoMat", CodigoJuridicoMat)
                da.Parameters.AddWithValue("@TotalValor", TotalValor)
                da.Parameters.AddWithValue("@DescFamilia", DescFamilia)
                da.Parameters.AddWithValue("@PercIPI", PercIPI)
                da.Parameters.AddWithValue("@vIPI", vIPI)
                da.Parameters.AddWithValue("@PercICMS", PercICMS)
                da.Parameters.AddWithValue("@vICMS", vICMS)
                da.Parameters.AddWithValue("@vLiquido", vLiquido)
                da.Parameters.AddWithValue("@D_E_L_E_T_E", D_E_L_E_T_E)
                da.Parameters.AddWithValue("@NumeroRP", NumeroRP)
                da.Parameters.AddWithValue("@txtTipoDesenho", txtTipoDesenho)

                da.Parameters.AddWithValue("@Comprimentocaixadelimitadora", Comprimentocaixadelimitadora)
                da.Parameters.AddWithValue("@Larguracaixadelimitadora", Larguracaixadelimitadora)
                da.Parameters.AddWithValue("@Espessuracaixadelimitadora", Espessuracaixadelimitadora)
                da.Parameters.AddWithValue("@Altura", Altura)
                da.Parameters.AddWithValue("@Largura", Largura)
                da.Parameters.AddWithValue("@Profundidade", Profundidade)
                da.Parameters.AddWithValue("@PesoMaisEmbalagem", PesoMaisEmbalagem)

                da.ExecuteNonQuery()

            End Using
            cl_BancoDados.FecharBanco()
        Catch ex As Exception
            cl_BancoDados.FecharBanco()
            'Dim errorMessage As String = $"Erro no controle: ' {ex.Message}"
            ' errorMessages.Add(errorMessage)
        Finally
        End Try

    End Sub


End Class


Public Class CLMaterialFichaTecnica

    Public idmaterial_fichatecnica As Integer
    Public idmaterial As Integer
    Public Descricao As String
    Public EnderecoArquivo As String
    Public NomeArquivo As String
    Public CodMatFabricante As String
    Public DescResumo As String
    Public DescDetal As String
    Public CodMatFabricanteNumeroRP As String

    Public Sub SalvarDadosMaterial(idmaterial, Descricao, EnderecoArquivo,
                                   NomeArquivo, CodMatFabricante, DescResumo, DescDetal, CodMatFabricanteNumeroRP)
        Try
            Dim query As String = "INSERT INTO material_fichatecnica (idmaterial, Descricao, EnderecoArquivo, 
                                                 NomeArquivo, CodMatFabricante, DescResumo, DescDetal,CodMatFabricanteNumeroRP)
                                             VALUES
                                                    (@idmaterial, @Descricao, 
                                                      @EnderecoArquivo, @NomeArquivo, 
                                                @CodMatFabricante, @DescResumo, @DescDetal,@CodMatFabricanteNumeroRP)"

            cl_BancoDados.AbrirBanco()

            Using da As New MySqlCommand(query, myconect)
                ' Defina os valores para os parâmetros
                da.Parameters.AddWithValue("@idmaterial", idmaterial)
                da.Parameters.AddWithValue("@Descricao", Descricao)
                da.Parameters.AddWithValue("@EnderecoArquivo", EnderecoArquivo)
                da.Parameters.AddWithValue("@NomeArquivo", NomeArquivo)
                da.Parameters.AddWithValue("@CodMatFabricante", CodMatFabricante)
                da.Parameters.AddWithValue("@DescResumo", DescResumo)
                da.Parameters.AddWithValue("@DescDetal", DescDetal)
                da.Parameters.AddWithValue("@CodMatFabricanteNumeroRP", CodMatFabricanteNumeroRP)

                da.ExecuteNonQuery()

            End Using
            cl_BancoDados.FecharBanco()
        Catch ex As Exception
            cl_BancoDados.FecharBanco()
            'Dim errorMessage As String = $"Erro no controle: ' {ex.Message}"
            'errorMessages.Add(errorMessage)
        Finally

        End Try

    End Sub

    Public Sub UpdateDados(idmaterial_fichatecnica, idmaterial, Descricao, EnderecoArquivo,
                           NomeArquivo, CodMatFabricante, DescResumo, DescDetal, CodMatFabricanteNumeroRP)
        Try

            Dim query As String = "UPDATE material_fichatecnica SET
                                idmaterial = @idmaterial,
                                EnderecoArquivo = @EnderecoArquivo,
                                NomeArquivo = @NomeArquivo,
                                CodMatFabricante = @CodMatFabricante,
                                DescResumo = @DescResumo,
                                DescDetal = @DescDetal,
                                CodMatFabricanteNumeroRP = @CodMatFabricanteNumeroRP
                                WHERE idmaterial_fichatecnica = @idmaterial_fichatecnica"

            'If CodMatFabricante = "PRD09303" Then
            '    MsgBox(CodMatFabricante)

            'End If
            cl_BancoDados.AbrirBanco()

            Using da As New MySqlCommand(query, myconect)
                ' Defina os valores para os parâmetros
                da.Parameters.AddWithValue("@idmaterial_fichatecnica", idmaterial_fichatecnica)
                da.Parameters.AddWithValue("@idmaterial", idmaterial)
                da.Parameters.AddWithValue("@Descricao", Descricao)
                da.Parameters.AddWithValue("@EnderecoArquivo", EnderecoArquivo)
                da.Parameters.AddWithValue("@NomeArquivo", NomeArquivo)
                da.Parameters.AddWithValue("@CodMatFabricante", CodMatFabricante)
                da.Parameters.AddWithValue("@DescResumo", DescResumo)
                da.Parameters.AddWithValue("@DescDetal", DescDetal)
                da.Parameters.AddWithValue("@CodMatFabricanteNumeroRP", CodMatFabricanteNumeroRP)

                da.ExecuteNonQuery()

            End Using
            cl_BancoDados.FecharBanco()
        Catch ex As Exception
            cl_BancoDados.FecharBanco()
            'Dim errorMessage As String = $"Erro no controle: ' {ex.Message}"
            ' errorMessages.Add(errorMessage)
        Finally
        End Try

    End Sub


End Class


Public Class clTag
    Public idTag As String
    Public Tag As String
    Public DescTag As String
    Public QtdeTag As Integer
    Public QtdeLiberada As Integer
    Public SaldoTag As Integer
    Public idProjeto As String
    Public Projeto As String
    Public IdEmpresa As String
    Public DescEmpresa As String
    Public TipoProduto As String
    Public DataEntrada As String
    Public DataPrevisao As String
    Public UnidadeProduto As String
    Public CriadoPor As String
    Public ProjetistaPlanejado As String


    Public TipoAco As String
    Public TipoEquipamento As String
    Public TipoInstalacao As String
    Public TipoGas As String
    Public medicao As String
    Public TipoStatusObra As String
    Public Tipoplanta As String
    Public tipotensao As String
    Public TipoVolts As String

    Public Sub SalvarDadosTags(IdProjeto, Projeto, Tag, DescTag, QtdeTag, IdEmpresa, DescEmpresa, TipoProduto, dataatual, DataPrevisao, UnidadeProduto, DataEntrada, ProjetistaPlanejado)

        Try

            Dim query As String = "INSERT INTO tags (Tag, DescTag, QtdeTag,QtdeLiberada,SaldoTag,IdProjeto,Projeto,IdEmpresa,DescEmpresa,TipoProduto,DataEntrada,DataPrevisao,UnidadeProduto,CriadoPor,ProjetistaPlanejado)
                                             VALUES
                                                    (@Tag, @descTag, @qtdeTag,@QtdeLiberada,@SaldoTag, @IdProjeto,@Projeto,@IdEmpresa,@DescEmpresa,@TipoProduto,@DataEntrada,@DataPrevisao,@UnidadeProduto,@CriadoPor,@ProjetistaPlanejado)"

            cl_BancoDados.AbrirBanco()

            Using da As New MySqlCommand(query, myconect)
                ' Defina os valores para os parâmetros
                da.Parameters.AddWithValue("@Tag", Tag)
                da.Parameters.AddWithValue("@descTag", DescTag)
                da.Parameters.AddWithValue("@qtdeTag", QtdeTag)
                da.Parameters.AddWithValue("@QtdeLiberada", "0")
                da.Parameters.AddWithValue("@SaldoTag", QtdeTag)
                da.Parameters.AddWithValue("@IdProjeto", IdProjeto)
                da.Parameters.AddWithValue("@Projeto", Projeto)
                da.Parameters.AddWithValue("@IdEmpresa", 0)
                da.Parameters.AddWithValue("@DescEmpresa", DescEmpresa)

                da.Parameters.AddWithValue("@TipoProduto", TipoProduto)
                da.Parameters.AddWithValue("@DataPrevisao", DataPrevisao)
                da.Parameters.AddWithValue("@DataEntrada", DataEntrada)
                da.Parameters.AddWithValue("@CriadoPor", Usuario.NomeCompleto)
                da.Parameters.AddWithValue("@UnidadeProduto", UnidadeProduto)
                da.Parameters.AddWithValue("@ProjetistaPlanejado", ProjetistaPlanejado)

                da.ExecuteNonQuery()
            End Using
            cl_BancoDados.FecharBanco()
        Catch ex As Exception
            Dim errorMessage As String = $"Erro no controle: ' {ex.Message}"
            cl_BancoDados.FecharBanco()

        Finally
        End Try

    End Sub

    '  Public Sub UpdateDados(IdTag, Tag, DescTag, QtdeTag, TipoProduto, Modelo, Linha, Produto, Os, DataPrevisao, UnidadeProduto)
    Public Sub UpdateDados(IdTag As String, Tag As String, DescTag As String, ByVal QtdeTag As Integer, TipoProduto As String, DataPrevisao As String, UnidadeProduto As String)
        Try

            Dim query As String = "UPDATE tags SET Tag = @Tag, DescTag = @descTag, QtdeTag = @qtdeTag,SaldoTag = @SaldoTag,
                                                   DataPrevisao = @DataPrevisao,TipoProduto = @TipoProduto,
                                                   UnidadeProduto = @UnidadeProduto WHERE IdTag = @IdTag"

            cl_BancoDados.AbrirBanco()

            Using da As New MySqlCommand(query, myconect)
                ' Defina os valores para os parâmetros
                da.Parameters.AddWithValue("@IdTag", IdTag)
                da.Parameters.AddWithValue("@Tag", Tag)
                da.Parameters.AddWithValue("@descTag", DescTag)
                da.Parameters.AddWithValue("@QtdeTag", QtdeTag)
                da.Parameters.AddWithValue("@SaldoTag", QtdeTag)
                da.Parameters.AddWithValue("@TipoProduto", TipoProduto)

                da.Parameters.AddWithValue("@DataPrevisao", DataPrevisao)
                da.Parameters.AddWithValue("@UnidadeProduto", UnidadeProduto)

                da.ExecuteNonQuery()
            End Using
            cl_BancoDados.FecharBanco()
        Catch ex As Exception
            cl_BancoDados.FecharBanco()
            Dim errorMessage As String = $"Erro no controle: ' {ex.Message}"
            ' errorMessages.Add(errorMessage)

            ' Tratar exceções aqui conforme necessário
        End Try

    End Sub

End Class



Public Class clMaterialDesenhoCliente

    Public idmaterial_desenho As String
    Public z1_nomecli As String
    Public z1_produto As String
    Public z1_codcli As String
    Public z1_revisao As String
    Public z1_desccli As String
    Public enderecoarquivo_cliente As String
    Public enderecoarquivo As String
    Public desenho As String





    Public Sub SalvarDados(z1_nomecli, z1_produto, z1_codcli, z1_revisao, z1_desccli,
                           enderecoarquivo_cliente, enderecoarquivo, desenho)

        Dim sucesso As Boolean
        Try
            sucesso = False
            Dim query As String = "INSERT INTO material_desenho (z1_nomecli, z1_produto, z1_codcli, z1_revisao, 
                                     z1_desccli, enderecoarquivo_cliente, 
                                       enderecoarquivo, desenho) VALUES
                                                        (@z1_nomecli, @z1_produto, @z1_codcli, @z1_revisao, 
                                     @z1_desccli, @enderecoarquivo_cliente, 
                                       @enderecoarquivo, @desenho)"

            cl_BancoDados.AbrirBanco()

            Using da As New MySqlCommand(query, myconect)


                ' Defina os valores para os parâmetros
                da.Parameters.AddWithValue("@z1_nomecli", z1_nomecli)
                da.Parameters.AddWithValue("@z1_produto", z1_produto)
                da.Parameters.AddWithValue("@z1_codcli", z1_codcli)
                da.Parameters.AddWithValue("@z1_revisao", z1_revisao)
                da.Parameters.AddWithValue("@z1_desccli", z1_desccli)
                da.Parameters.AddWithValue("@enderecoarquivo_cliente", enderecoarquivo_cliente)
                da.Parameters.AddWithValue("@enderecoarquivo", enderecoarquivo)
                da.Parameters.AddWithValue("@desenho", desenho)

                da.ExecuteNonQuery()
                sucesso = True
            End Using
            cl_BancoDados.FecharBanco()
        Catch ex As Exception
            cl_BancoDados.FecharBanco()
        End Try

    End Sub

    Public Sub UpdateDados(idmaterial_desenho, z1_nomecli, z1_produto, z1_codcli, z1_revisao, z1_desccli,
                           enderecoarquivo_cliente, enderecoarquivo, desenho)
        'Public Sub UpdateDados(IdProjeto, Projeto, DescProjeto, Responsavel, Cliente, DataEntrada, DataLiberacao, DataPrazoLiberacao, DataImplantacao, DataPrevisao, Estado)
        Dim sucesso As Boolean
        Try
            sucesso = False
            Dim query As String = "UPDATE material_desenho SET z1_nomecli = @z1_nomecli,
                                          z1_produto = @z1_produto,
                                          z1_codcli = @z1_codcli,
                                          z1_revisao = @z1_revisao,
                                          z1_desccli = @z1_desccli,
                                          enderecoarquivo_cliente = @enderecoarquivo_cliente,
                                          enderecoarquivo = @enderecoarquivo,
                                          desenho = @desenho
                                  WHERE idmaterial_desenho = @idmaterial_desenho"

            cl_BancoDados.AbrirBanco()

            Using da As New MySqlCommand(query, myconect)
                ' Defina os valores para os parâmetros
                da.Parameters.AddWithValue("@idmaterial_desenho", idmaterial_desenho)
                da.Parameters.AddWithValue("@z1_nomecli", z1_nomecli)
                da.Parameters.AddWithValue("@z1_produto", z1_produto)
                da.Parameters.AddWithValue("@z1_codcli", z1_codcli)
                da.Parameters.AddWithValue("@z1_revisao", z1_revisao)
                da.Parameters.AddWithValue("@z1_desccli", z1_desccli)
                da.Parameters.AddWithValue("@enderecoarquivo_cliente", enderecoarquivo_cliente)
                da.Parameters.AddWithValue("@enderecoarquivo", enderecoarquivo)
                da.Parameters.AddWithValue("@desenho", desenho)

                da.ExecuteNonQuery()
                sucesso = True
            End Using
            cl_BancoDados.FecharBanco()
        Catch ex As Exception
            cl_BancoDados.FecharBanco()

        End Try
    End Sub



End Class

