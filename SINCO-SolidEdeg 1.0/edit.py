import sys

file_path = "frmDadosPecaCorrente.vb"
with open(file_path, "r", encoding="utf-8-sig") as f:
    lines = f.readlines()

replacement = """
            ' 👉 Nenhuma leitura suja aqui! Delega para o Reader Service:
            Dim leitor As New SolidEdgeReaderService()
            leitor.ExtrairPropriedades(doc, DadosArquivoCorrente)
            
            ' Retorna e Atualiza UI
            txtTitulo.Text = DadosArquivoCorrente.Titulo
            cboAcabamento.Text = DadosArquivoCorrente.Comentarios
            txtPalavraChave.Text = DadosArquivoCorrente.PalavraChave
            txtAutor.Text = DadosArquivoCorrente.Author
            txtEmpresa.Text = DadosArquivoCorrente.CodigoJuridicoMat
            txtCategoria.Text = DadosArquivoCorrente.Verificado
            txtGerente.Text = DadosArquivoCorrente.Aprovado
            txtMaterialSw.Text = DadosArquivoCorrente.material
            cboTipoDesenho.Text = DadosArquivoCorrente.TipoDesenho
            
            txtData.Text = DadosArquivoCorrente.DataCriacaDesenho
            txtDataR.Text = DadosArquivoCorrente.DataUltimoSalvamento
            
            txtCutSizex.Text = DadosArquivoCorrente.ComprimentoBlank
            txtCutSizey.Text = DadosArquivoCorrente.LarguraBlank
            txtEspessura.Text = DadosArquivoCorrente.Espessura
            txtPesoKg.Text = DadosArquivoCorrente.Massa
            txtAreametroquadr.Text = DadosArquivoCorrente.AreaPintura
"""

# Lines an editor are 1-based, Python is 0-based.
# We want to replace lines 610 to 786.
# Index 609 corresponds to line 610, index 785 corresponds to line 786.
del lines[609:786] # Deletes from index 609 up to and excluding 786 (so 785 is the last deleted)
lines.insert(609, replacement)

with open(file_path, "w", encoding="utf-8-sig") as f:
    f.writelines(lines)
