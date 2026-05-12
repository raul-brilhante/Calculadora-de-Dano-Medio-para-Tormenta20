using System;
using System.Drawing;
using System.Windows.Forms;

namespace CalculadoraDanoT20
{
    public partial class Form1 : Form
    {
        private ResultadosFinais totalAcumulado = new ResultadosFinais();
        private AtributosAmeaca atributosAmeacaAtual = new AtributosAmeaca();
        private NumericUpDown numBonusAtaque = null!;
        private Panel painelAtaque = null!;
        private Panel painelAmeaca = null!;
        private Panel painelConfigAmeaca = null!;
        private GroupBox grpRodadasAmeaca = null!;
        private ComboBox cmbND = null!;
        private NumericUpDown numRD = null!;
        private NumericUpDown numFortificacao = null!;
        private Label lblRodadas1 = null!;
        private Label lblRodadas2 = null!;
        private Label lblRodadas3 = null!;
        private Label lblRodadas4 = null!;
        private Label lblRodadas5 = null!;
        private Label lblRodadas6 = null!;
        private Label lblRodadas7 = null!;
        private Label lblRodadas8 = null!;
        private Label lblChanceAcerto = null!;
        private Button btnModoAtaque = null!;
        private Button btnModoAmeaca = null!;

        public Form1()
        {
            InitializeComponent();
            ConfigurarJanela();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            AjustarLayoutResponsivo();
        }

        private void AjustarLayoutResponsivo()
        {
            Control.ControlCollection controles = painelAtaque != null ? painelAtaque.Controls : this.Controls;

            if (controles.Count > 0)
            {
                foreach (Control control in controles)
                {
                    if (control is Panel && control.BackColor == ColorTranslator.FromHtml("#3D2121"))
                    {
                        int novoX = Math.Max(30, (this.ClientSize.Width - control.Width) / 2);
                        control.Location = new Point(novoX, control.Location.Y);
                    }
                    else if (control is Button)
                    {
                        if (control.Text.Contains("ADICIONAR"))
                        {
                            int centroX = (this.ClientSize.Width / 2) - 105;
                            control.Location = new Point(Math.Max(30, centroX - 100), control.Location.Y);
                        }
                        else if (control.Text.Contains("LIMPAR"))
                        {
                            int centroX = (this.ClientSize.Width / 2) + 5;
                            control.Location = new Point(Math.Max(250, centroX + 10), control.Location.Y);
                        }
                    }
                }
            }

            if (painelConfigAmeaca != null)
            {
                painelConfigAmeaca.Location = new Point(
                    Math.Max(20, (this.ClientSize.Width - painelConfigAmeaca.Width) / 2),
                    painelConfigAmeaca.Location.Y);
            }
        }

        private void ConfigurarJanela()
        {
            this.Text = "Calculadora de Dano Médio - Tormenta20";
            this.Size = new Size(800, 725);
            this.MinimumSize = new Size(800, 725);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;

            try
            {
                var assembly = System.Reflection.Assembly.GetExecutingAssembly();
                
                using (var stream = assembly.GetManifestResourceStream("CalculadoraT20.calculadorat20.ico"))
                {
                    if (stream != null)
                    {
                        this.Icon = new Icon(stream);
                    }
                }
            }
            catch
            {
                // Caso algo falhe, o programa abre normalmente com o ícone padrão
            }
            
            this.BackColor = ColorTranslator.FromHtml("#2D1B1B");
            this.Font = new Font("Segoe UI", 10F, FontStyle.Regular);
            this.ForeColor = ColorTranslator.FromHtml("#F5F5DC");

            painelAtaque = new Panel();
            painelAtaque.Location = new Point(0, 0);
            painelAtaque.Size = this.ClientSize;
            painelAtaque.BackColor = Color.Transparent;
            painelAtaque.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            this.Controls.Add(painelAtaque);

            painelAmeaca = new Panel();
            painelAmeaca.Location = new Point(0, 0);
            painelAmeaca.Size = this.ClientSize;
            painelAmeaca.BackColor = Color.Transparent;
            painelAmeaca.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            painelAmeaca.Visible = false;
            this.Controls.Add(painelAmeaca);

            btnModoAtaque = CriarBotaoModo("Ataque", 12, 20);
            btnModoAtaque.Click += (_, __) => AlternarModo(true);
            this.Controls.Add(btnModoAtaque);

            btnModoAmeaca = CriarBotaoModo("Ameaça", 12, 65);
            btnModoAmeaca.Click += (_, __) => AlternarModo(false);
            this.Controls.Add(btnModoAmeaca);

            painelConfigAmeaca = new Panel();
            painelConfigAmeaca.BackColor = ColorTranslator.FromHtml("#3D2121");
            painelConfigAmeaca.Location = new Point(130, 120);
            painelConfigAmeaca.Size = new Size(520, 220);
            painelConfigAmeaca.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            painelAmeaca.Controls.Add(painelConfigAmeaca);

            Label lblTituloAmeaca = new Label();
            lblTituloAmeaca.Text = "CONFIGURAÇÃO DE AMEAÇAS";
            lblTituloAmeaca.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblTituloAmeaca.ForeColor = ColorTranslator.FromHtml("#FFD700");
            lblTituloAmeaca.Location = new Point(92, 15);
            lblTituloAmeaca.Size = new Size(350, 25);
            painelConfigAmeaca.Controls.Add(lblTituloAmeaca);

            int xLabelAmeaca = 100;
            int xInputAmeaca = 310;
            int yAmeaca = 60;
            int yStepAmeaca = 45;

            CriarLabel("ND:", xLabelAmeaca, yAmeaca, painelConfigAmeaca);
            cmbND = new ComboBox();
            cmbND.FormattingEnabled = true;
            cmbND.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbND.Font = new Font("Segoe UI", 10F);
            cmbND.Location = new Point(xInputAmeaca, yAmeaca);
            cmbND.Size = new Size(120, 28);
            cmbND.BackColor = ColorTranslator.FromHtml("#FFF8DC");
            cmbND.ForeColor = ColorTranslator.FromHtml("#2D1B1B");
            cmbND.Items.AddRange(new object[] {
                "0", "1/4", "1/2", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10",
                "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "S", "S+"
            });
            cmbND.SelectedItem = "0";
            cmbND.Text = "0";
            cmbND.SelectedIndexChanged += CmbND_SelectedIndexChanged;
            painelConfigAmeaca.Controls.Add(cmbND);

            yAmeaca += yStepAmeaca;
            CriarLabel("RD:", xLabelAmeaca, yAmeaca, painelConfigAmeaca);
            numRD = CriarNumeric(xInputAmeaca, yAmeaca, 0, 0, 999, painelConfigAmeaca);
            numRD.Value = 0;

            yAmeaca += yStepAmeaca;
            CriarLabel("Fortificação:", xLabelAmeaca, yAmeaca, painelConfigAmeaca);
            numFortificacao = CriarNumeric(xInputAmeaca, yAmeaca, 0, 0, 100, painelConfigAmeaca);
            numFortificacao.Value = 0;

            Label lblPercentual = new Label();
            lblPercentual.Text = "%";
            lblPercentual.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblPercentual.ForeColor = ColorTranslator.FromHtml("#F5F5DC");
            lblPercentual.Location = new Point(xInputAmeaca + 130, yAmeaca + 8);
            lblPercentual.Size = new Size(25, 25);
            lblPercentual.BackColor = Color.Transparent;
            painelConfigAmeaca.Controls.Add(lblPercentual);

            grpRodadasAmeaca = new GroupBox();
            grpRodadasAmeaca.Text = "RODADAS ATÉ DERROTAR A AMEAÇA";
            grpRodadasAmeaca.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            grpRodadasAmeaca.ForeColor = ColorTranslator.FromHtml("#FFD700");
            grpRodadasAmeaca.BackColor = ColorTranslator.FromHtml("#3D2121");
            grpRodadasAmeaca.Location = new Point(30, 360);
            grpRodadasAmeaca.Size = new Size(720, 185);
            grpRodadasAmeaca.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            painelAmeaca.Controls.Add(grpRodadasAmeaca);

            int yRod = 40;
            lblRodadas1 = CriarLabelResultado(grpRodadasAmeaca, "Dano Normal:", 50, yRod);
            lblRodadas2 = CriarLabelResultado(grpRodadasAmeaca, "Normal (Concentração):", 420, yRod);

            yRod += 35;
            lblRodadas3 = CriarLabelResultado(grpRodadasAmeaca, "Normal (Dilacerante):", 50, yRod);
            lblRodadas4 = CriarLabelResultado(grpRodadasAmeaca, "Conc. + Dilacerante:", 420, yRod);

            yRod += 35;
            lblRodadas5 = CriarLabelResultado(grpRodadasAmeaca, "Normal (Lancinante):", 50, yRod);
            lblRodadas6 = CriarLabelResultado(grpRodadasAmeaca, "Conc. + Lancinante:", 420, yRod);

            yRod += 35;
            lblRodadas7 = CriarLabelResultado(grpRodadasAmeaca, "Normal (Lancinante Rev.):", 50, yRod);
            lblRodadas8 = CriarLabelResultado(grpRodadasAmeaca, "Conc. + Lancinante Rev.:", 420, yRod);

            lblChanceAcerto = new Label();
            lblChanceAcerto.Text = "Chance de Acerto: --";
            lblChanceAcerto.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblChanceAcerto.ForeColor = ColorTranslator.FromHtml("#FFD700");
            lblChanceAcerto.BackColor = Color.Transparent;
            lblChanceAcerto.Location = new Point(30, 555);
            lblChanceAcerto.Size = new Size(500, 25);
            painelAmeaca.Controls.Add(lblChanceAcerto);

            Panel painelPrincipal = new Panel();
            painelPrincipal.BackColor = ColorTranslator.FromHtml("#3D2121");
            painelPrincipal.Location = new Point(130, 30);
            painelPrincipal.Size = new Size(520, 345);
            painelPrincipal.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            painelAtaque.Controls.Add(painelPrincipal);
            
            Label lblTitulo = new Label();
            lblTitulo.Text = "CONFIGURAÇÃO DO ATAQUE";
            lblTitulo.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblTitulo.ForeColor = ColorTranslator.FromHtml("#FFD700");
            lblTitulo.Location = new Point(115, 15);
            lblTitulo.Size = new Size(300, 25);
            painelPrincipal.Controls.Add(lblTitulo);

            int xLabel = 70;
            int xInput = 310;
            int yStart = 55;
            int yStep = 45;

            CriarLabel("Bônus de ataque:", xLabel, yStart, painelPrincipal);
            numBonusAtaque = CriarNumeric(xInput, yStart, 0, -999, 999, painelPrincipal);
            numBonusAtaque.ValueChanged += NumBonusAtaque_ValueChanged;

            yStart += yStep;
            
            CriarLabel("Margem de Ameaça:", xLabel, yStart, painelPrincipal);
            numMargem = CriarNumeric(xInput, yStart, 20, 1, 20, painelPrincipal);
            
            yStart += yStep;
            CriarLabel("Multiplicador de Crítico:", xLabel, yStart, painelPrincipal);
            numMultiplicador = CriarNumeric(xInput, yStart, 2, 1, 10, painelPrincipal);

            yStart += yStep;
            CriarLabel("Bônus Numérico (Flat):", xLabel, yStart, painelPrincipal);
            numBonusFlat = CriarNumeric(xInput, yStart, 0, 0, 100, painelPrincipal);

            yStart += yStep;
            CriarLabel("Dados da Arma (ex: 1d8):", xLabel, yStart, painelPrincipal);
            txtDadosArma = CriarTextBox("", xInput, yStart, "0d0", painelPrincipal);

            yStart += yStep;
            CriarLabel("Dados Extras (ex: 2d6):", xLabel, yStart, painelPrincipal);
            txtDadosExtras = CriarTextBox("", xInput, yStart, "0d0", painelPrincipal);
            
            Label lblNota = new Label();
            lblNota.Text = "Nota: Pode usar 1d6+1d8 !";
            lblNota.Font = new Font("Segoe UI", 9F, FontStyle.Italic);
            lblNota.ForeColor = ColorTranslator.FromHtml("#B8860B");
            lblNota.Location = new Point(xInput, yStart + 25);
            lblNota.Size = new Size(200, 20);
            painelPrincipal.Controls.Add(lblNota);

            yStart = 395;
            Button btnCalcular = CriarBotao("ADICIONAR ATAQUE", 180, yStart, 200, 50, true);
            btnCalcular.Click += BtnCalcular_Click;
            painelAtaque.Controls.Add(btnCalcular);

            Button btnLimpar = CriarBotao("LIMPAR / RESETAR", 400, yStart, 200, 50, false);
            btnLimpar.Click += BtnLimpar_Click;
            painelAtaque.Controls.Add(btnLimpar);

            yStart = 465;
            GroupBox grpResultados = new GroupBox();
            grpResultados.Text = "DANO MÉDIO ACUMULADO";
            grpResultados.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            grpResultados.ForeColor = ColorTranslator.FromHtml("#FFD700");
            grpResultados.BackColor = ColorTranslator.FromHtml("#3D2121");
            grpResultados.Location = new Point(30, yStart);
            grpResultados.Size = new Size(720, 185);
            grpResultados.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;
            painelAtaque.Controls.Add(grpResultados);

            int yRes = 40;
            
            lblRes1 = CriarLabelResultado(grpResultados, "Dano Normal:", 50, yRes);
            lblRes2 = CriarLabelResultado(grpResultados, "Normal (Concentração):", 420, yRes);
            
            yRes += 35;
            lblRes3 = CriarLabelResultado(grpResultados, "Normal (Dilacerante):", 50, yRes);
            lblRes4 = CriarLabelResultado(grpResultados, "Conc. + Dilacerante:", 420, yRes);

            yRes += 35;
            lblRes5 = CriarLabelResultado(grpResultados, "Normal (Lancinante):", 50, yRes);
            lblRes6 = CriarLabelResultado(grpResultados, "Conc. + Lancinante:", 420, yRes);

            yRes += 35;
            lblRes7 = CriarLabelResultado(grpResultados, "Normal (Lancinante Rev.):", 50, yRes);
            lblRes8 = CriarLabelResultado(grpResultados, "Conc. + Lancinante Rev.:", 420, yRes);

            AtualizarLabels();
            AtualizarAtributosDaAmeaca();
            AtualizarRodadasAteDerrotarAmeaca();
            AlternarModo(true);
            AjustarLayoutResponsivo();
        }

        private void CmbND_SelectedIndexChanged(object? sender, EventArgs e)
        {
            AtualizarAtributosDaAmeaca();
        }

        private void NumBonusAtaque_ValueChanged(object? sender, EventArgs e)
        {
            AtualizarRodadasAteDerrotarAmeaca();
        }

        private void AtualizarAtributosDaAmeaca()
        {
            string ndSelecionado = cmbND.SelectedItem?.ToString() ?? "0";

            if (ndSelecionado == "0")
            {
                atributosAmeacaAtual = CriarAtributosPlaceholder(considerar: false);
                AtualizarRodadasAteDerrotarAmeaca();
                return;
            }

            atributosAmeacaAtual = ObterAtributosPorND(ndSelecionado);
            AtualizarRodadasAteDerrotarAmeaca();
        }

        private void AtualizarRodadasAteDerrotarAmeaca()
        {
            double chanceAcerto = CalcularChanceAcerto();
            double chanceAcertoConc = CalcularChanceAcertoComConcentracao();
            lblChanceAcerto.Text = $"Chance de Acerto: {(chanceAcerto * 100):F0}% (Normal) / {(chanceAcertoConc * 100):F0}% (Conc.)";

            lblRodadas1.Text = CalcularRodadasTexto(totalAcumulado.Primeiro, comConcentracao: false);
            lblRodadas2.Text = CalcularRodadasTexto(totalAcumulado.Segundo, comConcentracao: true);
            lblRodadas3.Text = CalcularRodadasTexto(totalAcumulado.Terceiro, comConcentracao: false);
            lblRodadas4.Text = CalcularRodadasTexto(totalAcumulado.Quarto, comConcentracao: true);
            lblRodadas5.Text = CalcularRodadasTexto(totalAcumulado.Quinto, comConcentracao: false);
            lblRodadas6.Text = CalcularRodadasTexto(totalAcumulado.Sexto, comConcentracao: true);
            lblRodadas7.Text = CalcularRodadasTexto(totalAcumulado.Setimo, comConcentracao: false);
            lblRodadas8.Text = CalcularRodadasTexto(totalAcumulado.Oitavo, comConcentracao: true);
        }

        private double CalcularChanceAcerto()
        {
            if (!atributosAmeacaAtual.Considerar)
            {
                return 0.0;
            }

            int bonusAtaque = (int)numBonusAtaque.Value;
            int defesaAmeaca = atributosAmeacaAtual.Defesa;

            int rolagemMinimaParaAcertar = defesaAmeaca - bonusAtaque;
            int resultadosQueAcertam = 0;

            for (int d20 = 2; d20 <= 19; d20++)
            {
                if (d20 >= rolagemMinimaParaAcertar)
                {
                    resultadosQueAcertam++;
                }
            }

            resultadosQueAcertam++;

            return resultadosQueAcertam / 20.0;
        }

        private double CalcularChanceAcertoComConcentracao()
        {
            if (!atributosAmeacaAtual.Considerar)
            {
                return 0.0;
            }

            int bonusAtaque = (int)numBonusAtaque.Value;
            int defesaAmeaca = atributosAmeacaAtual.Defesa;
            int rolagemMinimaParaAcertar = defesaAmeaca - bonusAtaque;
            int acertos = 0;

            for (int d20_1 = 1; d20_1 <= 20; d20_1++)
            {
                for (int d20_2 = 1; d20_2 <= 20; d20_2++)
                {
                    int melhor = Math.Max(d20_1, d20_2);

                    if (melhor == 1)
                    {
                        // Não conta
                    }
                    else if (melhor == 20)
                    {
                        acertos++;
                    }
                    else if (melhor >= rolagemMinimaParaAcertar)
                    {
                        acertos++;
                    }
                }
            }

            return acertos / 400.0;
        }

        private string CalcularRodadasTexto(double danoMedioAcumulado, bool comConcentracao = false)
        {
            if (!atributosAmeacaAtual.Considerar || atributosAmeacaAtual.Vida <= 0 || danoMedioAcumulado <= 0)
            {
                return "0";
            }

            int bonusAtaque = (int)numBonusAtaque.Value;
            int defesaAmeaca = atributosAmeacaAtual.Defesa;

            int rolagemMinimaParaAcertar = defesaAmeaca - bonusAtaque;
            int resultadosQueAcertam = 0;

            if (comConcentracao)
            {
                for (int d20_1 = 1; d20_1 <= 20; d20_1++)
                {
                    for (int d20_2 = 1; d20_2 <= 20; d20_2++)
                    {
                        int melhor = Math.Max(d20_1, d20_2);

                        if (melhor == 1)
                        {
                            // Não conta
                        }
                        else if (melhor == 20)
                        {
                            resultadosQueAcertam++;
                        }
                        else if (melhor >= rolagemMinimaParaAcertar)
                        {
                            resultadosQueAcertam++;
                        }
                    }
                }
            }
            else
            {
                for (int d20 = 2; d20 <= 19; d20++)
                {
                    if (d20 >= rolagemMinimaParaAcertar)
                    {
                        resultadosQueAcertam++;
                    }
                }
                resultadosQueAcertam++;
            }

            if (resultadosQueAcertam <= 0)
            {
                return "∞";
            }

            double chanceAcerto = comConcentracao ? (resultadosQueAcertam / 400.0) : (resultadosQueAcertam / 20.0);
            double danoMedioEfetivoPorRodada = danoMedioAcumulado * chanceAcerto;

            if (danoMedioEfetivoPorRodada <= 0)
            {
                return "∞";
            }

            int rodadas = (int)Math.Ceiling(atributosAmeacaAtual.Vida / danoMedioEfetivoPorRodada);
            return rodadas.ToString();
        }

        private AtributosAmeaca ObterAtributosPorND(string nd)
        {
            return nd switch
            {
                "1/4" => new AtributosAmeaca
                { Considerar = true, Vida = 7, Defesa = 11, ResistenciaForte = 3, ResistenciaMedia = 0, ResistenciaFraca = -2 },
                "1/2" => new AtributosAmeaca
                { Considerar = true, Vida = 15, Defesa = 14, ResistenciaForte = 6, ResistenciaMedia = 3, ResistenciaFraca = -1 },
                "1" => new AtributosAmeaca
                { Considerar = true, Vida = 35, Defesa = 16, ResistenciaForte = 11, ResistenciaMedia = 5, ResistenciaFraca = 0 },
                "2" => new AtributosAmeaca
                { Considerar = true, Vida = 70, Defesa = 19, ResistenciaForte = 13, ResistenciaMedia = 7, ResistenciaFraca = 2 },
                "3" => new AtributosAmeaca
                { Considerar = true, Vida = 105, Defesa = 21, ResistenciaForte = 15, ResistenciaMedia = 9, ResistenciaFraca = 3 },
                "4" => new AtributosAmeaca
                { Considerar = true, Vida = 140, Defesa = 23, ResistenciaForte = 16, ResistenciaMedia = 10, ResistenciaFraca = 4 },
                "5" => new AtributosAmeaca
                { Considerar = true, Vida = 200, Defesa = 24, ResistenciaForte = 17, ResistenciaMedia = 11, ResistenciaFraca = 5 },
                "6" => new AtributosAmeaca
                { Considerar = true, Vida = 240, Defesa = 27, ResistenciaForte = 18, ResistenciaMedia = 12, ResistenciaFraca = 6 },
                "7" => new AtributosAmeaca
                { Considerar = true, Vida = 280, Defesa = 31, ResistenciaForte = 20, ResistenciaMedia = 14, ResistenciaFraca = 7 },
                "8" => new AtributosAmeaca
                { Considerar = true, Vida = 320, Defesa = 33, ResistenciaForte = 21, ResistenciaMedia = 15, ResistenciaFraca = 8 },
                "9" => new AtributosAmeaca
                { Considerar = true, Vida = 360, Defesa = 34, ResistenciaForte = 21, ResistenciaMedia = 15, ResistenciaFraca = 9 },
                "10" => new AtributosAmeaca
                { Considerar = true, Vida = 400, Defesa = 36, ResistenciaForte = 22, ResistenciaMedia = 16, ResistenciaFraca = 10 },
                "11" => new AtributosAmeaca
                { Considerar = true, Vida = 550, Defesa = 41, ResistenciaForte = 24, ResistenciaMedia = 18, ResistenciaFraca = 11 },
                "12" => new AtributosAmeaca
                {  Considerar = true, Vida = 600, Defesa = 43, ResistenciaForte = 26, ResistenciaMedia = 20, ResistenciaFraca = 12 },
                "13" => new AtributosAmeaca
                { Considerar = true, Vida = 650, Defesa = 44, ResistenciaForte = 26, ResistenciaMedia = 20, ResistenciaFraca = 13 },
                "14" => new AtributosAmeaca
                { Considerar = true, Vida = 700, Defesa = 46, ResistenciaForte = 28, ResistenciaMedia = 22, ResistenciaFraca = 14 },
                "15" => new AtributosAmeaca
                { Considerar = true, Vida = 750, Defesa = 50, ResistenciaForte = 28, ResistenciaMedia = 22, ResistenciaFraca = 15 },
                "16" => new AtributosAmeaca
                { Considerar = true, Vida = 800, Defesa = 53, ResistenciaForte = 30, ResistenciaMedia = 24, ResistenciaFraca = 16 },
                "17" => new AtributosAmeaca
                { Considerar = true, Vida = 1020, Defesa = 54, ResistenciaForte = 30, ResistenciaMedia = 24, ResistenciaFraca = 17 },
                "18" => new AtributosAmeaca
                { Considerar = true, Vida = 1080, Defesa = 56, ResistenciaForte = 32, ResistenciaMedia = 26, ResistenciaFraca = 18 },
                "19" => new AtributosAmeaca
                { Considerar = true, Vida = 1140, Defesa = 59, ResistenciaForte = 32, ResistenciaMedia = 26, ResistenciaFraca = 19 },
                "20" => new AtributosAmeaca
                { Considerar = true, Vida = 1200, Defesa = 61, ResistenciaForte = 34, ResistenciaMedia = 28, ResistenciaFraca = 20 },
                "S" => new AtributosAmeaca
                { Considerar = true, Vida = 2500, Defesa = 65, ResistenciaForte = 36, ResistenciaMedia = 30, ResistenciaFraca = 22 },
                "S+" => new AtributosAmeaca
                { Considerar = true, Vida = 4000, Defesa = 70, ResistenciaForte = 38, ResistenciaMedia = 33, ResistenciaFraca = 25 },
                _ => CriarAtributosPlaceholder(considerar: false)
            };
        }

        private AtributosAmeaca CriarAtributosPlaceholder(bool considerar)
        {
            return new AtributosAmeaca
            {
                Considerar = considerar,
                Vida = 0,
                Defesa = 0,
                ResistenciaForte = 0,
                ResistenciaMedia = 0,
                ResistenciaFraca = 0
            };
        }

        private Button CriarBotaoModo(string texto, int x, int y)
        {
            Button btn = new Button();
            btn.Text = texto;
            btn.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btn.Location = new Point(x, y);
            btn.Size = new Size(108, 31);
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 1;
            btn.FlatAppearance.BorderColor = ColorTranslator.FromHtml("#B8860B");
            btn.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            return btn;
        }

        private void AlternarModo(bool modoAtaque)
        {
            painelAtaque.Visible = modoAtaque;
            painelAmeaca.Visible = !modoAtaque;

            btnModoAtaque.BackColor = modoAtaque
                ? ColorTranslator.FromHtml("#8B0000")
                : ColorTranslator.FromHtml("#6B4545");
            btnModoAtaque.ForeColor = modoAtaque
                ? ColorTranslator.FromHtml("#FFD700")
                : ColorTranslator.FromHtml("#F5F5DC");

            btnModoAmeaca.BackColor = !modoAtaque
                ? ColorTranslator.FromHtml("#8B0000")
                : ColorTranslator.FromHtml("#6B4545");
            btnModoAmeaca.ForeColor = !modoAtaque
                ? ColorTranslator.FromHtml("#FFD700")
                : ColorTranslator.FromHtml("#F5F5DC");

            btnModoAtaque.BringToFront();
            btnModoAmeaca.BringToFront();
        }

        private void CriarLabel(string texto, int x, int y, Control parent) {
            Label l = new Label();
            l.Text = texto;
            l.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            l.ForeColor = ColorTranslator.FromHtml("#F5F5DC");
            l.Location = new Point(x, y + 8);
            l.Size = new Size(200, 25);
            l.BackColor = Color.Transparent;
            parent.Controls.Add(l);
        }
        
        private TextBox CriarTextBox(string texto, int x, int y, string def, Control parent) {
            TextBox t = new TextBox();
            t.Font = new Font("Segoe UI", 10F);
            t.Location = new Point(x, y);
            t.Size = new Size(120, 28);
            t.Text = def;
            t.BackColor = ColorTranslator.FromHtml("#FFF8DC");
            t.ForeColor = ColorTranslator.FromHtml("#2D1B1B");
            t.BorderStyle = BorderStyle.FixedSingle;
            parent.Controls.Add(t);
            return t;
        }
        
        private NumericUpDown CriarNumeric(int x, int y, int def, int min, int max, Control parent) {
            NumericUpDown n = new NumericUpDown();
            n.Font = new Font("Segoe UI", 10F);
            n.Location = new Point(x, y);
            n.Size = new Size(120, 28);
            n.Minimum = min;
            n.Maximum = max;
            n.Value = def;
            n.BackColor = ColorTranslator.FromHtml("#FFF8DC");
            n.ForeColor = ColorTranslator.FromHtml("#2D1B1B");
            parent.Controls.Add(n);
            return n;
        }
        
        private Button CriarBotao(string texto, int x, int y, int w, int h, bool positivo) {
            Button btn = new Button();
            btn.Text = texto;
            btn.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            btn.Location = new Point(x, y);
            btn.Size = new Size(w, h);
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            
            if (positivo) {
                btn.BackColor = ColorTranslator.FromHtml("#8B0000");
                btn.ForeColor = ColorTranslator.FromHtml("#FFD700");
                btn.FlatAppearance.MouseOverBackColor = ColorTranslator.FromHtml("#A52A2A");
            } else {
                btn.BackColor = ColorTranslator.FromHtml("#6B4545");
                btn.ForeColor = ColorTranslator.FromHtml("#F5F5DC");
                btn.FlatAppearance.MouseOverBackColor = ColorTranslator.FromHtml("#8B6969");
            }
            
            return btn;
        }
        
        private Label CriarLabelResultado(GroupBox g, string titulo, int x, int y) {
            Label lTitulo = new Label();
            lTitulo.Text = titulo;
            lTitulo.Font = new Font("Segoe UI", 10F, FontStyle.Regular);
            lTitulo.ForeColor = ColorTranslator.FromHtml("#F5F5DC");
            lTitulo.Location = new Point(x, y);
            lTitulo.Size = new Size(180, 25);
            lTitulo.TextAlign = ContentAlignment.MiddleLeft;
            lTitulo.BackColor = Color.Transparent;
            
            Label lValor = new Label();
            lValor.Text = "0.0";
            lValor.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
            lValor.ForeColor = ColorTranslator.FromHtml("#FFD700");
            lValor.Location = new Point(x + 180, y);
            lValor.Size = new Size(100, 25);
            lValor.TextAlign = ContentAlignment.MiddleLeft;
            lValor.BackColor = Color.Transparent;
            
            g.Controls.Add(lTitulo);
            g.Controls.Add(lValor);
            return lValor;
        }

        private double CalculaDado(string dado)
        {
            if (string.IsNullOrWhiteSpace(dado)) return 0.0;
            try
            {
                dado = dado.ToLower().Trim();
                if (dado.Contains("+"))
                {
                    string[] partes = dado.Split('+');
                    double total = 0;
                    foreach (var parte in partes) total += CalculaDado(parte);
                    return total;
                }

                int posD = dado.IndexOf('d');
                if (posD == -1) return 0.0; 

                int numDados = 1;
                if (posD > 0)
                {
                    string qtdStr = dado.Substring(0, posD);
                    if (!int.TryParse(qtdStr, out numDados)) numDados = 1;
                }

                string valorStr = dado.Substring(posD + 1);
                int valorDado = int.Parse(valorStr);

                return ((numDados + (numDados * valorDado)) / 2.0);
            }
            catch { return 0.0; }
        }

        private void BtnCalcular_Click(object? sender, EventArgs e)
        {
            int margem = (int)numMargem.Value;
            int multiplicador = (int)numMultiplicador.Value;
            int flat = (int)numBonusFlat.Value;
            int rd = (int)numRD.Value;
            double danoArma = CalculaDado(txtDadosArma.Text);
            double dadosExtra = CalculaDado(txtDadosExtras.Text);
            
            double chanceNaoCritar = (margem - 1.0) * 5.0;
            double chanceCritar = 100.0 - chanceNaoCritar;
            
            double chanceCritarC = chanceCritar + ((chanceNaoCritar * chanceCritar) / 100.0);
            double chanceNaoCritarC = 100.0 - chanceCritarC;

            ResultadosFinais r = new ResultadosFinais();

            r.Primeiro = (((((danoArma) + flat + dadosExtra - rd) * (chanceNaoCritar)) + 
                           ((((danoArma) * multiplicador) + flat + dadosExtra - rd) * (chanceCritar))) / 100);

            r.Segundo = ((((danoArma) + flat + dadosExtra - rd) * (chanceNaoCritarC) + 
                          (((danoArma) * multiplicador) + flat + dadosExtra - rd) * (chanceCritarC)) / 100);

            r.Terceiro = (((((danoArma) + flat + dadosExtra - rd) * (chanceNaoCritar)) + 
                           ((((danoArma) * multiplicador) + flat + 10 + dadosExtra - rd) * (chanceCritar))) / 100);

            r.Quarto = ((((danoArma) + flat + dadosExtra - rd) * (chanceNaoCritarC) + 
                         (((danoArma) * multiplicador) + flat + 10 + dadosExtra - rd) * (chanceCritarC)) / 100);

            r.Quinto = (((((danoArma) + flat + dadosExtra - rd) * (chanceNaoCritar)) + 
                         (((((danoArma) + flat) * multiplicador) + dadosExtra - rd) * (chanceCritar))) / 100);

            r.Sexto = ((((danoArma) + flat + dadosExtra - rd) * (chanceNaoCritarC) + 
                        ((((danoArma) + flat) * multiplicador) + dadosExtra - rd) * (chanceCritarC)) / 100);

            r.Setimo = (((((danoArma) + flat + dadosExtra - rd) * (chanceNaoCritar)) + 
                         (((((danoArma) + 10) * multiplicador) + flat + dadosExtra - rd) * (chanceCritar))) / 100);

            r.Oitavo = ((((danoArma) + flat + dadosExtra - rd) * (chanceNaoCritarC) + 
                         ((((danoArma) + 10) * multiplicador) + flat + dadosExtra - rd) * (chanceCritarC)) / 100);

            totalAcumulado.Primeiro += r.Primeiro;
            totalAcumulado.Segundo += r.Segundo;
            totalAcumulado.Terceiro += r.Terceiro;
            totalAcumulado.Quarto += r.Quarto;
            totalAcumulado.Quinto += r.Quinto;
            totalAcumulado.Sexto += r.Sexto;
            totalAcumulado.Setimo += r.Setimo;
            totalAcumulado.Oitavo += r.Oitavo;

            AtualizarLabels();
            AtualizarRodadasAteDerrotarAmeaca();
        }

        private void BtnLimpar_Click(object? sender, EventArgs e)
        {
            totalAcumulado = new ResultadosFinais();
            AtualizarLabels();
            AtualizarRodadasAteDerrotarAmeaca();
        }

        private void AtualizarLabels()
        {
            lblRes1.Text = totalAcumulado.Primeiro.ToString("F2");
            lblRes2.Text = totalAcumulado.Segundo.ToString("F2");
            lblRes3.Text = totalAcumulado.Terceiro.ToString("F2");
            lblRes4.Text = totalAcumulado.Quarto.ToString("F2");
            lblRes5.Text = totalAcumulado.Quinto.ToString("F2");
            lblRes6.Text = totalAcumulado.Sexto.ToString("F2");
            lblRes7.Text = totalAcumulado.Setimo.ToString("F2");
            lblRes8.Text = totalAcumulado.Oitavo.ToString("F2");
        }
    }

    public class ResultadosFinais
    {
        public double Primeiro { get; set; } = 0.0;
        public double Segundo { get; set; } = 0.0;
        public double Terceiro { get; set; } = 0.0;
        public double Quarto { get; set; } = 0.0;
        public double Quinto { get; set; } = 0.0;
        public double Sexto { get; set; } = 0.0;
        public double Setimo { get; set; } = 0.0;
        public double Oitavo { get; set; } = 0.0;
    }

    public class AtributosAmeaca
    {
        public bool Considerar { get; set; } = false;
        public int Vida { get; set; } = 0;
        public int Defesa { get; set; } = 0;
        public int ResistenciaForte { get; set; } = 0;
        public int ResistenciaMedia { get; set; } = 0;
        public int ResistenciaFraca { get; set; } = 0;
    }
}