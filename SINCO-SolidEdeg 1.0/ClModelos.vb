Imports MySql.Data.MySqlClient

''' <summary>
''' Modelos de dados e entidades de domínio do aplicativo (Projeto, Tag, Processo, Peça, Material).
''' </summary>

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

    Public Sub SalvarDados(IdProjeto As Object, Projeto As Object, DescProjeto As Object, Responsavel As Object, Comercial As Object,
                           PlanejadoComercial As Object, Financeiro As Object, Dataprevisao As Object, PlanejadoFinanceiro As Object, PrazoEntrega As Object,
                           IdCliente As Object, Cliente As Object, DataEntradaPedido As Object, Estado As Object, EnderecoProjeto As Object, Liberado As Object)
        Try
            Dim query As String = "INSERT INTO projetos (Projeto, DescProjeto, Responsavel,Criadopor,DataCriacao, Liberado_Comercial,DataPlanejadoComercial,Liberado_Financeiro,Dataprevisao,DataPlanejadoFinanceiro,PrazoEntrega,
                                                          IdEmpresa,DescEmpresa,DataEntradaPedido,Estado,EnderecoProjeto,Liberado) VALUES
                                                        (@Projeto, @descProjeto, @responsavel,@CriadoPor,@DataCriacao,@Liberado_Comercial,@DataPlanejadoComercial,@Liberado_Financeiro,@Dataprevisao,@DataPlanejadoFinanceiro,@PrazoEntrega,
                                                          @IdEmpresa,@DescEmpresa,@DataEntradaPedido,@Estado,@EnderecoProjeto,@Liberado)"

            cl_BancoDados.AbrirBanco()

            Using da As New MySqlCommand(query, myconect)
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
            End Using
        Catch ex As Exception
        Finally
            cl_BancoDados.FecharBanco()
        End Try
    End Sub

    Public Sub UpdateDados(IdProjeto As Object, Projeto As Object, DescProjeto As Object, Responsavel As Object, Cliente As Object,
                           DataEntrada As Object, DataLiberacao As Object, DataPrazoLiberacao As Object, DataImplantacao As Object,
                           DataPrevisao As Object, Estado As Object, Optional EnderecoProjeto As Object = "", Optional Liberado As Object = "S")
        Try
            Dim query As String = "UPDATE projetos SET Projeto = @Projeto, DescProjeto = @descProjeto, Responsavel = @responsavel,
                                                      DescEmpresa = @descEmpresa, DataEntradaPedido = @dataEntradaPedido,
                                                      DataPrevisao = @dataPrevisao, Estado = @estado, EnderecoProjeto = @enderecoProjeto,
                                                      Liberado = @liberado WHERE IdProjeto = @idProjeto"

            cl_BancoDados.AbrirBanco()

            Using da As New MySqlCommand(query, myconect)
                da.Parameters.AddWithValue("@idProjeto", IdProjeto)
                da.Parameters.AddWithValue("@Projeto", Projeto)
                da.Parameters.AddWithValue("@descProjeto", DescProjeto)
                da.Parameters.AddWithValue("@responsavel", Responsavel)
                da.Parameters.AddWithValue("@descEmpresa", Cliente)
                da.Parameters.AddWithValue("@dataEntradaPedido", DataEntrada)
                da.Parameters.AddWithValue("@dataPrevisao", DataPrevisao)
                da.Parameters.AddWithValue("@estado", Estado)
                da.Parameters.AddWithValue("@enderecoProjeto", EnderecoProjeto)
                da.Parameters.AddWithValue("@liberado", Liberado)
                da.ExecuteNonQuery()
            End Using
        Catch ex As Exception
        Finally
            cl_BancoDados.FecharBanco()
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

    Public Sub SalvarDadosTags(IdProjeto As Object, Projeto As Object, Tag As Object, DescTag As Object, QtdeTag As Object,
                               IdEmpresa As Object, DescEmpresa As Object, TipoProduto As Object, dataatual As Object,
                               DataPrevisao As Object, UnidadeProduto As Object, DataEntrada As Object, ProjetistaPlanejado As Object)
        Try
            Dim query As String = "INSERT INTO tags (Tag, DescTag, QtdeTag,QtdeLiberada,SaldoTag,IdProjeto,Projeto,IdEmpresa,DescEmpresa,TipoProduto,DataEntrada,DataPrevisao,UnidadeProduto,CriadoPor,ProjetistaPlanejado)
                                             VALUES
                                                    (@Tag, @descTag, @qtdeTag,@QtdeLiberada,@SaldoTag, @IdProjeto,@Projeto,@IdEmpresa,@DescEmpresa,@TipoProduto,@DataEntrada,@DataPrevisao,@UnidadeProduto,@CriadoPor,@ProjetistaPlanejado)"

            cl_BancoDados.AbrirBanco()

            Using da As New MySqlCommand(query, myconect)
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
        Catch ex As Exception
        Finally
            cl_BancoDados.FecharBanco()
        End Try
    End Sub

    Public Sub UpdateDados(IdTag As String, Tag As String, DescTag As String, ByVal QtdeTag As Integer, TipoProduto As String, DataPrevisao As String, UnidadeProduto As String)
        Try
            Dim query As String = "UPDATE tags SET Tag = @Tag, DescTag = @descTag, QtdeTag = @qtdeTag,SaldoTag = @SaldoTag,
                                                   DataPrevisao = @DataPrevisao,TipoProduto = @TipoProduto,
                                                   UnidadeProduto = @UnidadeProduto WHERE IdTag = @IdTag"

            cl_BancoDados.AbrirBanco()

            Using da As New MySqlCommand(query, myconect)
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
        Catch ex As Exception
        Finally
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

    Public Function SalvarOrdeMServicoBanco(IdProcesso As Object, IdMaterial As Object, SequenciaExecucao As Object, codMatFabricante As Object) As Boolean
        Try
            cl_BancoDados.AbrirBanco()

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
            Return True
        Catch ex As Exception
            Return False
        Finally
            cl_BancoDados.FecharBanco()
        End Try
    End Function
End Class

Public Class CLPecaManufaturada
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

    Public Sub SalvarDados(IdMontapeca As Object, TipoPeca As Object, IdMaterial As Object, PecaQtde As Object,
                           IdMaterialPeca As Object, IdEmpresa As Object, D_E_L_E_T_E As Object, Peso As Object,
                           Valor As Object, UsuarioD_E_L_E_T_E As Object, DataD_E_L_E_T_E As Object,
                           CodMatFabricante As Object, UsuarioCriacao As Object, DataCriacao As Object)
        Try
            Dim query As String = "INSERT INTO montapeca (TipoPeca, IdMaterial, PecaQtde, IdMaterialPeca, IdEmpresa, D_E_L_E_T_E, Peso, Valor,
                                                          UsuarioD_E_L_E_T_E, DataD_E_L_E_T_E, CodMatFabricante, UsuarioCriacao, DataCriacao)     VALUES
                                                      (@TipoPeca, @IdMaterial, @PecaQtde, @IdMaterialPeca, @IdEmpresa, @D_E_L_E_T_E, @Peso, @Valor,
                                                          @UsuarioD_E_L_E_T_E, @DataD_E_L_E_T_E, @CodMatFabricante, @UsuarioCriacao, @DataCriacao)"

            cl_BancoDados.AbrirBanco()

            Using da As New MySqlCommand(query, myconect)
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
        Finally
            cl_BancoDados.FecharBanco()
        End Try
    End Sub

    Public Sub SalvarDadosGabarito(idmaterial As Object, idmaterial_desenho As Object, descricao As Object)
        Try
            Dim query As String = "INSERT INTO material_gabarito (idmaterial, idmaterial_desenho, descricao) VALUES
                                                      (@idmaterial, @idmaterial_desenho, @descricao)"

            cl_BancoDados.AbrirBanco()

            Using da As New MySqlCommand(query, myconect)
                da.Parameters.AddWithValue("@IdMaterial", idmaterial)
                da.Parameters.AddWithValue("@idmaterial_desenho", idmaterial_desenho)
                da.Parameters.AddWithValue("@descricao", descricao)
                da.ExecuteNonQuery()
            End Using
        Catch ex As Exception
        Finally
            cl_BancoDados.FecharBanco()
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

    Public Sub SalvarDadosMaterial(DescResumo As Object, DescDetal As Object, Peso As Object, Unidade As Object, CodMatFabricante As Object, CodigoJuridicoMat As Object,
                                  TotalValor As Object, DescFamilia As Object, PercIPI As Object, vIPI As Object, PercICMS As Object,
                                  vICMS As Object, vLiquido As Object, D_E_L_E_T_E As Object, txtTipoDesenho As Object, NumeroRP As Object,
                                  UsuarioCriacao As Object, DtCad As Object, Comprimentocaixadelimitadora As Object,
                                  Larguracaixadelimitadora As Object, Espessuracaixadelimitadora As Object,
                                  Altura As Object, Largura As Object, Profundidade As Object, PesoMaisEmbalagem As Object, Espessura As Object, Materialsw As Object, Autor As Object, PalavraChave As Object)
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
        Catch ex As Exception
        Finally
            cl_BancoDados.FecharBanco()
        End Try
    End Sub

    Public Sub UpdateDados(IdMaterial As Object, DescResumo As Object, DescDetal As Object, Peso As Object, Unidade As Object, CodMatFabricante As Object, CodigoJuridicoMat As Object,
                           TotalValor As Object, DescFamilia As Object, PercIPI As Object, vIPI As Object, PercICMS As Object,
                           vICMS As Object, vLiquido As Object, D_E_L_E_T_E As Object, txtTipoDesenho As Object, NumeroRP As Object,
                           Comprimentocaixadelimitadora As Object, Larguracaixadelimitadora As Object, Espessuracaixadelimitadora As Object,
                           Altura As Object, Largura As Object, Profundidade As Object, PesoMaisEmbalagem As Object)
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

            cl_BancoDados.AbrirBanco()

            Using da As New MySqlCommand(query, myconect)
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
        Catch ex As Exception
        Finally
            cl_BancoDados.FecharBanco()
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

    Public Sub SalvarDados(z1_nomecli As Object, z1_produto As Object, z1_codcli As Object, z1_revisao As Object, z1_desccli As Object,
                           enderecoarquivo_cliente As Object, enderecoarquivo As Object, desenho As Object)
        Try
            Dim query As String = "INSERT INTO material_desenho (z1_nomecli, z1_produto, z1_codcli, z1_revisao, 
                                     z1_desccli, enderecoarquivo_cliente, 
                                       enderecoarquivo, desenho) VALUES
                                                        (@z1_nomecli, @z1_produto, @z1_codcli, @z1_revisao, 
                                     @z1_desccli, @enderecoarquivo_cliente, 
                                       @enderecoarquivo, @desenho)"

            cl_BancoDados.AbrirBanco()

            Using da As New MySqlCommand(query, myconect)
                da.Parameters.AddWithValue("@z1_nomecli", z1_nomecli)
                da.Parameters.AddWithValue("@z1_produto", z1_produto)
                da.Parameters.AddWithValue("@z1_codcli", z1_codcli)
                da.Parameters.AddWithValue("@z1_revisao", z1_revisao)
                da.Parameters.AddWithValue("@z1_desccli", z1_desccli)
                da.Parameters.AddWithValue("@enderecoarquivo_cliente", enderecoarquivo_cliente)
                da.Parameters.AddWithValue("@enderecoarquivo", enderecoarquivo)
                da.Parameters.AddWithValue("@desenho", desenho)
                da.ExecuteNonQuery()
            End Using
        Catch ex As Exception
        Finally
            cl_BancoDados.FecharBanco()
        End Try
    End Sub

    Public Sub UpdateDados(idmaterial_desenho As Object, z1_nomecli As Object, z1_produto As Object, z1_codcli As Object, z1_revisao As Object, z1_desccli As Object,
                           enderecoarquivo_cliente As Object, enderecoarquivo As Object, desenho As Object)
        Try
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
            End Using
        Catch ex As Exception
        Finally
            cl_BancoDados.FecharBanco()
        End Try
    End Sub
End Class
