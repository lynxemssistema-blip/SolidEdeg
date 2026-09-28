
Imports MySql.Data.MySqlClient

Public Class clUsuario

    Public idUsuario, NomeCompleto, idSetor, Setor, TipoUsuario, Login, Senha, email, Descricao, Foto, txtCorte,
                           txtDobra, txtSolda, txtPintura, txtMontagem, txtAlmoxarifado, CriadoPor, DataCadastro,
                           D_E_L_E_T_E, MapaProducao, Romaneio, OrdemServico, SolidWorks, Sigla, TokenAPI As String

    Public Function RetornaDadosUsuario(ByVal LoginEntrada As String, ByVal SenhaEntrada As String) As Boolean

        Dim LoginLimpo As String = If(LoginEntrada, "").Trim()
        Dim SenhaLimpa As String = If(SenhaEntrada, "").Trim()

        If String.IsNullOrEmpty(LoginLimpo) Then
            Return False
        End If

        Dim apiAutenticou As Boolean = False

        ' 1. Tenta autenticação prévia via API (caso o ambiente web/Node esteja em execução)
        Try
            Dim postUrl As String = "http://192.168.1.77:3001/api/auth/login"
            Dim request As System.Net.HttpWebRequest = CType(System.Net.WebRequest.Create(postUrl), System.Net.HttpWebRequest)
            request.Method = "POST"
            request.ContentType = "application/json"
            request.Timeout = 2500 ' Timeout ágil de 2.5s para não travar caso a API esteja inacessível

            Dim postData As String = "{""login"":""" & LoginLimpo.Replace("""", "\""") & """,""senha"":""" & SenhaLimpa.Replace("""", "\""") & """}"
            Dim byteArray As Byte() = System.Text.Encoding.UTF8.GetBytes(postData)
            request.ContentLength = byteArray.Length

            Using dataStream As System.IO.Stream = request.GetRequestStream()
                dataStream.Write(byteArray, 0, byteArray.Length)
            End Using

            Using response As System.Net.HttpWebResponse = CType(request.GetResponse(), System.Net.HttpWebResponse)
                If response.StatusCode = System.Net.HttpStatusCode.OK Then
                    Using reader As New System.IO.StreamReader(response.GetResponseStream())
                        Dim responseFromServer As String = reader.ReadToEnd()
                        Try
                            Dim jsonObj As Object = Newtonsoft.Json.Linq.JObject.Parse(responseFromServer)
                            TokenAPI = jsonObj("token").ToString()
                        Catch exToken As Exception
                        End Try
                        apiAutenticou = True
                    End Using
                End If
            End Using
        Catch ex As Exception
            ' API indisponível ou falhou: prossegue com autenticação direta no banco MySQL
            apiAutenticou = False
        End Try

        ' 2. Validação e carregamento dos dados do usuário diretamente no MySQL
        Try
            cl_BancoDados.AbrirBanco()

            If My.Settings.TipoConexao = "MYSQL" Then
                Dim sql As String
                If apiAutenticou Then
                    sql = "SELECT * FROM " & ComplementoTipoBanco & "usuario WHERE Login = @Login AND (D_E_L_E_T_E <> '*' OR D_E_L_E_T_E IS NULL) LIMIT 1"
                Else
                    sql = "SELECT * FROM " & ComplementoTipoBanco & "usuario WHERE Login = @Login AND Senha = @Senha AND (D_E_L_E_T_E <> '*' OR D_E_L_E_T_E IS NULL) LIMIT 1"
                End If

                Using da As New MySqlCommand(sql, myconect)
                    da.Parameters.AddWithValue("@Login", LoginLimpo)
                    If Not apiAutenticou Then
                        da.Parameters.AddWithValue("@Senha", SenhaLimpa)
                    End If

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

                            Return True
                        Else
                            Return False
                        End If
                    End Using
                End Using
            Else
                Return False
            End If
        Catch ex As Exception
            Return False
        Finally
            cl_BancoDados.FecharBanco()
        End Try

    End Function

    Public idConfigucacaoSistema, Bancoendereco, Bancousuario, banco, Bancosenha,
        EnderecoPastaRaizOS, EnderecoTemplateExcel, CopiaBancoDados, EnderecoPastaRaizRomaneio,
        EnderecoTemplateExcelRomaneio, ParametroExportarDXF, EnderecoNovoFormatoA3,
        EnderecoNovoFormatoA4, DataCriacao, Usuario, PermitirPecastxtcorte0noPlanoCorte,
        PermitirVerLiberado_EngenharianoPlanoCorte, EnviarEmailLiberacaoOS As String

    Public Function RetornaDadosConfiguracao() As Boolean

        Try
            cl_BancoDados.AbrirBanco()

            If My.Settings.TipoConexao = "MYSQL" Then
                Using da As New MySqlCommand("SELECT * FROM " & ComplementoTipoBanco & "configucacaosistema WHERE idconfigucacaosistema = 1 AND (d_e_l_e_t_e <> '*' OR d_e_l_e_t_e IS NULL) LIMIT 1", myconect)
                    Using dr As MySqlDataReader = da.ExecuteReader()
                        If dr.HasRows Then
                            dr.Read()
                            EnviarEmailLiberacaoOS = dr("EnviarEmailLiberacaoOS").ToString()
                            Return True
                        Else
                            EnviarEmailLiberacaoOS = ""
                            Return False
                        End If
                    End Using
                End Using
            Else
                Return False
            End If
        Catch ex As Exception
            Return False
        Finally
            cl_BancoDados.FecharBanco()
        End Try

    End Function

End Class