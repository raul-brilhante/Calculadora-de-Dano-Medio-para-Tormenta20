using System;
using System.Drawing;
using System.Windows.Forms;

namespace CalculadoraDanoT20
{
    public partial class Form1 : Form
    {
        private ResultadosFinais totalAcumulado = new ResultadosFinais();
        private Panel painelAtaque = null!;
        private Panel painelAmeaca = null!;
        private Panel painelConfigAmeaca = null!;
        private ComboBox cmbND = null!;
        private NumericUpDown numRD = null!;
        private NumericUpDown numFortificacao = null!;
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
            this.Size = new Size(800, 700);
            this.MinimumSize = new Size(800, 700);
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
                "11", "12", "13", "14", "15", "16", "17", "18", "19", "20"
            });
            cmbND.SelectedItem = "0";
            cmbND.Text = "0";
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

            Panel painelPrincipal = new Panel();
            painelPrincipal.BackColor = ColorTranslator.FromHtml("#3D2121");
            painelPrincipal.Location = new Point(130, 30);
            painelPrincipal.Size = new Size(520, 300);
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

            yStart = 350;
            Button btnCalcular = CriarBotao("ADICIONAR ATAQUE", 180, yStart, 200, 50, true);
            btnCalcular.Click += BtnCalcular_Click;
            painelAtaque.Controls.Add(btnCalcular);

            Button btnLimpar = CriarBotao("LIMPAR / RESETAR", 400, yStart, 200, 50, false);
            btnLimpar.Click += BtnLimpar_Click;
            painelAtaque.Controls.Add(btnLimpar);

            yStart = 420;
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
            AlternarModo(true);
            AjustarLayoutResponsivo();
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

            // Garante que os botões de modo nunca fiquem escondidos atrás dos painéis.
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
        }

        private void BtnLimpar_Click(object? sender, EventArgs e)
        {
            totalAcumulado = new ResultadosFinais();
            AtualizarLabels();
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
}