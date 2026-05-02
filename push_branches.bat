cd "c:\Users\adm\Desktop\MetalFisa Anty"
git commit -m "Dados Web Metalfisa"
git push -u origin dados-web-metalfisa

cd "c:\Users\adm\Documents\SolidEdeg rev 4.0\Macros\SINCO-SolidEdeg 1.0-24-02-2026-metalfisa AntyGravity\SINCO-SolidEdeg 1.0"
git checkout -b dados-web-metalfisa
git add "SINCO-SolidEdeg 1.0\frmDadosPecaCorrente.vb" "SINCO-SolidEdeg 1.0\frmDadosPecaCorrente.Designer.vb" "SINCO-SolidEdeg 1.0\frmDadosPecaCorrente.resx" "SINCO-SolidEdeg 1.0\clDadosArquivoCorrente.vb" "SINCO-SolidEdeg 1.0\SINCO-SolidEdeg 1.0.vbproj"
git commit -m "Dados Web Metalfisa"
git push -u origin dados-web-metalfisa
