using FileTemplates;
using System;
using System.Windows.Forms;
using Clients = DataProcessing.ClientData;
using OutputState = DataProcessing.OperationState;
using Utils = DataProcessing.DataFunctions;

namespace MassTemplateGenerator
{
    public partial class WndMain : Form
    {
        #region [ constructor + initialization ]
        public WndMain(bool forcefirstrun = false)
        {
            if (forcefirstrun) { Utils.GetSettings().AppFirstRun = true; Utils.GetSettings().Save(); }
            InitializeComponent();
            if (Utils.GetSettings().AppFirstRun)
            {
                if (MessageBox.Show(this, "Esta aplicación contiene data CONFIDENCIAL " +
                    "cuyo uso solamente está autorizado para pruebas internas del " +
                    "aplicativo IBE del Banco BHD León." + Environment.NewLine +
                    Environment.NewLine + "Al utilizarla, el usuario reconoce que " +
                    "asume el compromiso del uso apropiado de la misma, y que " +
                    "cualquier violación de privacidad será ÚNICA, PERSONAL Y " +
                    "EXCLUSIVAMENTE su responsabilidad." + Environment.NewLine +
                    Environment.NewLine + "Si está de acuerdo con estos términos, " +
                    "pulse el botón Aceptar. De lo contrario, pulse Cancelar para " +
                    "cerrar la aplicación.", "Advertencia de Privacidad y Términos de Uso",
                    MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.Cancel)
                { Environment.Exit(0); }
            }
            Utils.GetSettings().AppFirstRun = false;
            Utils.GetSettings().Save();
            Clients.LoadClientData();
            var names = Utils.GetAllTemplateNames();
            foreach (var n in names) { cbxType.Items.Add(n); }
            cbxType.SelectedIndex = 0;

            this.Text = this.Text + " v" +
                typeof(WndMain).Assembly.GetName().Version.ToString();

            sfdSave.InitialDirectory =
                Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        }
        #endregion



        #region [ boilerplate event handlers ]
        private void ayudaToolStripMenuItem_Click(object sender, EventArgs e)
        { System.Diagnostics.Process.Start(Utils.GetTempLogPath()); }

        private void SalirToolStripMenuItem_Click(object sender, EventArgs e)
        { Application.Exit(); }

        private void TmrTimer_Tick(object sender, EventArgs e)
        { sblStatus.Text = "Listo."; tmrTimer.Stop(); }

        private void ContribuirAlProyecto_Click(object sender, EventArgs e)
        {
            string email = 
                "subject=Generador de archivos de cargas masivas" +
                "&body=Tengo una contribución para el proyecto.";
            System.Diagnostics.Process.Start
                ("mailto:fernando_bordas@bhdleon.com.do?" + email);
        }

        private void PreferenciasToolStripMenuItem_Click(object sender, EventArgs e)
        { new WndPrefs().ShowDialog(); }

        private void AcercaDeToolStripMenuItem_Click(object sender, EventArgs e)
        { new WndAbout().ShowDialog(); }

        private void BtnOpen_Click(object sender, EventArgs e)
        { 
            sfdSave.ShowDialog();
            if (sfdSave.FileName.Contains(":\\") || sfdSave.FileName.Contains("\\\\"))
            { txbPath.Text = sfdSave.FileName; }
        }

        private void BtnGenerate_Click(object sender, EventArgs e)
        {
            if (!Utils.ValidatePath(txbPath.Text))
            {
                MessageBox.Show(this, "La ruta de archivo especificada es inválida.",
                    "Error en ruta de archivo", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }
            GenerateFile(cbxType.Text, txbPath.Text, (int)numAmount.Value);
        }
        #endregion



        #region [ record generation and i/o handling ]
        /// <summary>
        /// Generates the output file based on the specified parameters.
        /// </summary>
        /// <param name="template">The template to use for generating the file
        /// contents.</param>
        /// <param name="path"></param>
        /// <param name="amount"></param>
        private void GenerateFile(string template, string path, int amount)
        {
            if (amount < 1)
            {
                MessageBox.Show(this, "La cantidad de registros a generar no " +
                    "puede ser menor que 1.", "Error en cantidad de registros",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            sblStatus.Text = "Generando...";
            ITemplate p = Utils.GetTemplateFromDescriptiveName(template);
            string output = p.Generate(amount);
            OutputState st = Utils.WriteToDisk(output, path);
            switch (st)
            {
                case OutputState.IOException:
                    MessageBox.Show(this, "Error guardando el archivo en la ruta " +
                        "especificada. Verifique el log de errores para más " +
                        "información. Puede encontrar el log en la siguiente ruta:" +
                        Environment.NewLine + Utils.GetErrorLogLocation(),
                        "Error guardando archivo",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    sblStatus.Text = "Error guardando archivo.";
                    break;
                case OutputState.OtherException:
                    MessageBox.Show(this, "Error no especificado o indeterminado. " +
                        "Verifique el log de errores para mayor información y " +
                        "detalles. Puede encontrar el log en la siguiente ruta:" +
                        Environment.NewLine + Utils.GetErrorLogLocation(),
                        "Error indeterminado",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    sblStatus.Text = "Error genérico/indeterminado.";
                    break;
                default:
                    sblStatus.Text = 
                        "Completado. " + amount.ToString() + " registros generados en " +
                            "la ruta especificada.";
                    break;
            }
            sfdSave.FileName = txbPath.Text = string.Empty;
            tmrTimer.Start();
        }
        #endregion
    }
}
