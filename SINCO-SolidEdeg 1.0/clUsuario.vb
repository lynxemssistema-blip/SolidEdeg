
Imports MySql.Data.MySqlClient

Public Class clUsuario

    Public idUsuario, NomeCompleto, idSetor, Setor, TipoUsuario, Login, Senha, email, Descricao, Foto, txtCorte,
                           txtDobra, txtSolda, txtPintura, txtMontagem, txtAlmoxarifado, CriadoPor, DataCadastro,
                           D_E_L_E_T_E, MapaProducao, Romaneio, OrdemServico, SolidWorks, Sigla, TokenAPI As String

    Public Function RetornaDadosUsuario(ByVal LoginEntrada As String, ByVal SenhaEntrada As String) As Boolean

        Dim postUrl As String = "http://192.168.1.77:3001/api/auth/login"
        Dim request As System.Net.HttpWebRequest = CType(System.Net.WebRequest.Create(postUrl), System.Net.HttpWebRequest)
        request.Method = "POST"
        request.ContentType = "application/json"

        Dim LoginLimpo As String = LoginEntrada.Trim()
        Dim SenhaLimpa As String = SenhaEntrada.Trim()

        ' Prepara o JSON com usuário e senha sem os espaços em branco acidentais
        Dim postData As String = "{""login"":""" & LoginLimpo.Replace("""", "\""") & """,""senha"":""" & SenhaLimpa.Replace("""", "\""") & """}"
        Dim byteArray As Byte() = System.Text.Encoding.UTF8.GetBytes(postData)
        request.ContentLength = byteArray.Length

        Try
            Using dataStream As System.IO.Stream = request.GetRequestStream()
                dataStream.Write(byteArray, 0, byteArray.Length)
            End Using

            ' Tenta realizar o login via API (que lida com bcrypt e gera o token)
            Using response As System.Net.HttpWebResponse = CType(request.GetResponse(), System.Net.HttpWebResponse)
                If response.StatusCode = System.Net.HttpStatusCode.OK Then
                    Using reader As New System.IO.StreamReader(response.GetResponseStream())
                        Dim responseFromServer As String = reader.ReadToEnd()
                        ' Aqui poderíamos parsear o JSON para pegar o Token, usando algo simples
                        Try
                            Dim jsonObj As Object = Newtonsoft.Json.Linq.JObject.Parse(responseFromServer)
                            TokenAPI = jsonObj("token").ToString()
                        Catch exToken As Exception
                            ' Fallback caso não consiga ler o token
                        End Try
                    End Using
                End If
            End Using
        Catch ex As System.Net.WebException
            ' Se a API retornar erro (401 - Unauthorized, etc), a senha é inválida
            RetornaDadosUsuario = False
            Return False
        Catch ex As Exception
            ' Outros erros de rede
            RetornaDadosUsuario = False
            Return False
        End Try

        ' Se passou da API sem Exception, o login é válido. 
        ' Agora resgatamos as informações preenchendo as variáveis, mas apenas pelo Login.
        cl_BancoDados.AbrirBanco()
        If My.Settings.TipoConexao = "MYSQL" Then
            Try
                Using da As New MySqlCommand("SELECT * FROM  " & ComplementoTipoBanco & "usuario WHERE Login = @Login", myconect)
                    da.Parameters.AddWithValue("@Login", LoginLimpo)

                    Using dr As MySqlDataReader = da.ExecuteReader()
                        If dr.HasRows Then
                            dr.Read()

                            idUsuario = dr("idUsuario").ToString()
                            NomeCompleto = dr("NomeCompleto").ToString()
                            idSetor = dr("idSetor").ToString()
                            Setor = dr("Setor").ToString()
                            TipoUsuario = dr("TipoUsuario").ToString()
                            Login = dr("Login").ToString()
                            Senha = dr("Senha").ToString()
                            email = dr("email").ToString()
                            Descricao = dr("Descricao").ToString()
                            txtCorte = dr("txtCorte").ToString()
                            txtDobra = dr("txtDobra").ToString()
                            txtSolda = dr("txtSolda").ToString()
                            txtPintura = dr("txtPintura").ToString()
                            txtMontagem = dr("txtMontagem").ToString()
                            txtAlmoxarifado = dr("txtAlmoxarifado").ToString()
                            CriadoPor = dr("CriadoPor").ToString()
                            DataCadastro = dr("DataCadastro").ToString()
                            D_E_L_E_T_E = dr("D_E_L_E_T_E").ToString()
                            MapaProducao = dr("MapaProducao").ToString()
                            Romaneio = dr("Romaneio").ToString()
                            OrdemServico = dr("OrdemServico").ToString()
                            SolidWorks = dr("SolidWorks").ToString()
                            Sigla = dr("Sigla").ToString()

                            RetornaDadosUsuario = True
                        Else
                            RetornaDadosUsuario = False
                        End If
                    End Using
                End Using
            Catch ex As Exception
                RetornaDadosUsuario = False
            Finally
            End Try
        End If

        cl_BancoDados.FecharBanco()

    End Function

    Public idConfigucacaoSistema, Bancoendereco, Bancousuario, banco, Bancosenha,
        EnderecoPastaRaizOS, EnderecoTemplateExcel, CopiaBancoDados, EnderecoPastaRaizRomaneio,
        EnderecoTemplateExcelRomaneio, ParametroExportarDXF, EnderecoNovoFormatoA3,
        EnderecoNovoFormatoA4, DataCriacao, Usuario, PermitirPecastxtcorte0noPlanoCorte,
        PermitirVerLiberado_EngenharianoPlanoCorte, EnviarEmailLiberacaoOS As String

    Public Function RetornaDadosConfiguracao() As Boolean

        cl_BancoDados.AbrirBanco()
        If My.Settings.TipoConexao = "MYSQL" Then

            Try

                Using da As New MySqlCommand("SELECT * FROM  " & ComplementoTipoBanco & "configucacaosistema WHERE idconfigucacaosistema = 1 and d_e_l_e_t_e <> '*' or d_e_l_e_t_e is null", myconect)
                    ' Adicionar parâmetros para evitar SQL Injection

                    ' Executar o comando e abrir o DataReader
                    Using dr As MySqlDataReader = da.ExecuteReader()
                        ' Verificar se há linhas retornadas
                        If dr.HasRows Then
                            ' Ler a primeira linha retornada
                            dr.Read()

                            ' Preencher as variáveis com os valores retornados
                            EnviarEmailLiberacaoOS = dr("EnviarEmailLiberacaoOS").ToString()

                            RetornaDadosConfiguracao = True

                            '''''MyTaskPanelHost.TimerdgvPlanejamentoProjetista.Enabled = True
                        Else
                            EnviarEmailLiberacaoOS = ""
                            RetornaDadosConfiguracao = False
                        End If
                    End Using
                End Using
            Catch ex As Exception
            Finally

            End Try


        End If
        cl_BancoDados.FecharBanco()

    End Function


End Class