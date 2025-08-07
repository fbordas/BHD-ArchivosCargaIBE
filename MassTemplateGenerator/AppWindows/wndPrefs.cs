using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Windows.Forms;
using Clients = DataProcessing.ClientData;
using DataProc = DataProcessing.DataFunctions;
using OutputState = DataProcessing.OperationState;


namespace MassTemplateGenerator
{
    public partial class WndPrefs : Form
    {
        #region [ members ]
        private bool _stateChanged = false;
        private bool _enableStateMonitor = false;
        #endregion



        #region [ utility methods ]
        private void TryExit()
        {
            if (_stateChanged)
            {
                if (MessageBox.Show(this, "Los datos han sido modificados " +
                    "y no se han guardado. ¿Desea ignorar los cambios y salir?",
                    "Datos modificados", MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Warning) == DialogResult.Cancel)
                { return; }
            }
            Close();
        }


        public IEnumerable<Control> GetAllControls(Control control, Type type = null)
        {
            var controls = control.Controls.Cast<Control>();

            //check the all value, if true then get all the controls
            //otherwise get the controls of the specified type
            if (type == null)
                return controls.SelectMany(ctrl => GetAllControls(ctrl, type))
                    .Concat(controls);
            else
                return controls.SelectMany(ctrl => GetAllControls(ctrl, type))
                    .Concat(controls).Where(c => c.GetType() == type);
        }
        #endregion



        #region [ boilerplate event handlers ]
        private void BtnCancel_Click(object sender, System.EventArgs e)
        { TryExit(); }

        private void BtnSave_Click(object sender, System.EventArgs e)
        { SaveSettings(); Close(); }

        private void TextBox_TextChanged(object sender, System.EventArgs e)
        { if (_enableStateMonitor) { _stateChanged = true; } }

        private void Controls_KeyDown(object sender, KeyEventArgs e)
        { if (e.KeyCode == Keys.Escape) { TryExit(); } }

        private void WndPrefs_Load(object sender, System.EventArgs e)
        { _enableStateMonitor = true; }

        private void exportarConfiguraciónToolStripMenuItem_Click(object sender, EventArgs e)
        { ExportSettings(); }

        private void importarConfiguraciónToolStripMenuItem_Click(object sender, EventArgs e)
        { ImportSettings(); }

        private void restaurarDatosPorDefectoToolStripMenuItem_Click(object sender, EventArgs e)
        { RestoreSettings(); }
        #endregion



        #region [ constructor + initialization + instantiation ]
        public WndPrefs()
        {
            InitializeComponent();
            LoadSettings();
            sfdSave.FileName =
                "DataExportada" + DateTime.Now.Year.ToString() +
                DataProc.GetTimestamp().ToString();
            sfdSave.InitialDirectory =
                Environment.GetFolderPath
                (Environment.SpecialFolder.MyDocuments);
            ofdOpen.InitialDirectory =
                Environment.GetFolderPath
                (Environment.SpecialFolder.MyDocuments);
        }
        #endregion



        #region [ saving + loading + restoring ]
        private void SaveSettings()
        {
            var controls = GetAllControls(this, typeof(TextBox))
                .Concat(GetAllControls(this, typeof(CheckBox)));
            foreach (SettingsProperty p in DataProc.GetSettings().Properties)
            {
                var ctrl = controls.Where(c => c.Name.Contains(p.Name)).FirstOrDefault();
                switch (ctrl.Name.Substring(0, 4))
                {
                    case "chk_":
                        DataProc.GetSettings()[p.Name] = ((CheckBox)ctrl).Checked;
                        break;
                    case "val_":
                        DataProc.GetSettings()[p.Name] = double.Parse(((TextBox)ctrl).Text);
                        break;
                    default:
                        DataProc.GetSettings()[p.Name] = ((TextBox)ctrl).Text;
                        break;
                }
            }
            DataProc.GetSettings().Save();
        }

        private void LoadSettings()
        {
            var controls = GetAllControls(this, typeof(TextBox))
                .Concat(GetAllControls(this, typeof(CheckBox)));
            foreach (SettingsProperty p in DataProc.GetSettings().Properties)
            {
                if (p.Name == "AppFirstRun") { continue; }
                var ctrl = controls.Where(c => c.Name.Contains(p.Name)).FirstOrDefault();
                switch (ctrl.Name.Substring(0, 4))
                {
                    case "chk_":
                        ((CheckBox)ctrl).Checked = (bool)DataProc.GetSettings()[p.Name];
                        EnableOrDisableDependantControls((CheckBox)ctrl);
                        break;
                    default:
                        ((TextBox)ctrl).Text = DataProc.GetSettings()[p.Name].ToString();
                        break;
                }
            }
        }

        private void EnableOrDisableDependantControls(CheckBox chk)
        {
            var checks = GetAllControls(this, typeof(CheckBox));
            var textboxes = GetAllControls(this, typeof(TextBox));
            foreach (var check in checks)
            {
                var tags = check.Tag.ToString().Split(';');
                foreach (var tag in tags)
                { textboxes.Where(t => t.Name == tag).FirstOrDefault()
                        .Enabled = !chk.Checked; }
            }
        }

        private void RestoreSettings()
        {
            if (MessageBox.Show(this, "Esta opción restablecerá toda la información a " +
                "los valores por defecto incluidos en la aplicación, y NO TIENE VUELTA " +
                "ATRÁS a menos que haya guardado un respaldo de la configuración actual." +
                Environment.NewLine + Environment.NewLine + "¿Desea continuar con la " +
                "restauración?", "ADVERTENCIA", MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning) != DialogResult.Yes) { return; }
            DataProc.ResetSettings();
            Close();
        }
        #endregion



        #region [ import + export ]
        private void ExportSettings()
        {
            if (sfdSave.ShowDialog() != DialogResult.OK) { return; }
            string targetfile = sfdSave.FileName;
            if (!DataProc.ValidatePath(targetfile))
            {
                MessageBox.Show(this, "La ruta de archivo especificada es " +
                    "inválida.", "Error en ruta de archivo",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            OutputState opState = DataProc.ExportSettings(targetfile);
            switch (opState)
            {
                case OutputState.IOException:
                    MessageBox.Show(this, "Error guardando el archivo en la ruta " +
                        "especificada. Verifique el log de errores para más " +
                        "información. Puede encontrar el log en la siguiente ruta:" +
                        Environment.NewLine +
                        Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) +
                        "\\BHDLMassGen.log", "Error guardando archivo",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
                case OutputState.OtherException:
                    MessageBox.Show(this, "Error no especificado o indeterminado. " +
                        "Verifique el log de errores para mayor información y " +
                        "detalles. Puede encontrar el log en la siguiente ruta:" +
                        Environment.NewLine +
                        Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) +
                        "\\BHDLMassGen.log", "Error indeterminado",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
                default:
                    MessageBox.Show(this, "Datos exportados con éxito.", "Completado",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;
            }
        }

        private void ImportSettings()
        {
            if (ofdOpen.ShowDialog() != DialogResult.OK) { return; }
            string sourcefile = ofdOpen.FileName;
            if (!DataProc.ValidatePath(sourcefile))
            {
                MessageBox.Show(this, "La ruta de archivo especificada es " +
                    "inválida.", "Error en ruta de archivo",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            bool success = DataProc.ImportSettings(sourcefile);
            if (!success)
            {
                MessageBox.Show(this, "Ha ocurrido un error importando los " +
                    "datos. Verifique el log de errores para más detalles." +
                    "Puede encontrar el log en la siguiente ruta:" +
                    Environment.NewLine +
                    Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) +
                    "\\BHDLMassGen.log", "Error importando datos",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            LoadSettings();
            MessageBox.Show(this, "Datos cargados con éxito. La configuración " +
                "también ha sido automáticamente guardada.", "Completado",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            _stateChanged = false;
        }
        #endregion



        #region [ rng handlers ]
        /// <summary>
        /// Ensures that any checkbox change triggers the window state being
        /// modified to summon the saving alert message upon attempting to
        /// exit the window.
        /// </summary>
        private void Checkboxes_CheckStateChanged(object sender, EventArgs e)
        { _stateChanged = true; }

        private void ReferenceOrAmount_CheckedChanged(object sender, EventArgs e)
        {
            if (((CheckBox)sender).Tag.ToString().Contains("Monto"))
            {
                ((TextBox)Controls
                    .Find(((CheckBox)sender).Tag.ToString(), true)[0])
                    .Text = DataProc.GetRandomAmount().ToString("F");
            } else {
                ((TextBox)Controls
                    .Find(((CheckBox)sender).Tag.ToString(), true)[0])
                    .Text = DataProc.GetRandomReference(10);
            }
            ((TextBox)Controls
                .Find(((CheckBox)sender).Tag.ToString(), true)[0])
                .Enabled = !((CheckBox)sender).Checked;
        }

        private void chkNomina_AcctRNG_CheckedChanged(object sender, EventArgs e)
        {
            var client = Clients.GetRandomBHDClient();
            txb_Nomina_Cuenta.Text = client.Key;
            txb_Nomina_Beneficiario.Text = client.Value;
            txb_Nomina_Cuenta.Enabled = txb_Nomina_Beneficiario.Enabled = 
                !((CheckBox)sender).Checked;
        }
        
        private void chkEfeCaja_AcctRNG_CheckedChanged(object sender, EventArgs e)
        {
            var client = Clients.GetRandomOtherClient();
            txb_EfeCaja_TipoID.Text = "C";
            txb_EfeCaja_ID.Text = client.Key;
            txb_EfeCaja_Beneficiario.Text = client.Value;
            txb_EfeCaja_TipoID.Enabled = txb_EfeCaja_ID.Enabled =
                txb_EfeCaja_Beneficiario.Enabled = !((CheckBox)sender).Checked;
        }

        private void chkCkAdm_AcctRNG_CheckedChanged(object sender, EventArgs e)
        {
            var client = Clients.GetRandomOtherClient();
            txb_CkAdm_Beneficiario.Text = client.Value;
            txb_CkAdm_Beneficiario.Enabled = !((CheckBox)sender).Checked;
        }

        private void chkBCCk_AcctRNG_CheckedChanged(object sender, EventArgs e)
        {
            txb_BCCk_Cuenta.Text = DataProc.GetRandomAcctNumber(11);
            txb_BCCk_Cuenta.Enabled = !((CheckBox)sender).Checked;
        }

        private void chkTBHD_AcctRNG_CheckedChanged(object sender, EventArgs e)
        {
            var client = Clients.GetRandomBHDClient();
            txb_TBHD_TipoCta.Text = "CA";
            txb_TBHD_NumCta.Text = client.Key;
            txb_TBHD_Beneficiario.Text = client.Value;
            txb_TBHD_TipoCta.Enabled = txb_TBHD_NumCta.Enabled =
              txb_TBHD_Beneficiario.Enabled = txb_TBHD_TipoCta.Enabled = 
              !((CheckBox)sender).Checked;
        }

        private void chkACH_AcctRNG_CheckedChanged(object sender, EventArgs e)
        {
            txb_ACH_NumCta.Text = DataProc.GetRandomAcctNumber(9);
            txb_ACH_NumCta.Enabled = !((CheckBox)sender).Checked;
        }

        private void chkACH_BenefRNG_CheckedChanged(object sender, EventArgs e)
        {
            txb_ACH_Beneficiario.Text = Clients.GetRandomOtherClient().Value;
            txb_ACH_Beneficiario.Enabled = !((CheckBox)sender).Checked;
        }

        private void chkLBTR_AcctRNG_CheckedChanged(object sender, EventArgs e)
        {
            txb_LBTR_NumCta.Text = DataProc.GetRandomAcctNumber(9);
            txb_LBTR_NumCta.Enabled = !((CheckBox)sender).Checked;
        }

        private void chkLBTR_IDRNG_CheckedChanged(object sender, EventArgs e)
        {
            var client = Clients.GetRandomOtherClient();
            txb_LBTR_TipoID.Text = "C";
            txb_LBTR_ID.Text = client.Key;
            txb_LBTR_Beneficiario.Text = client.Value;
            txb_LBTR_TipoID.Enabled = txb_LBTR_ID.Enabled =
                txb_LBTR_Beneficiario.Enabled = !((CheckBox)sender).Checked;
        }

        private void chkBCCk_BenefRNG_CheckedChanged(object sender, EventArgs e)
        {
            txb_BCCk_Beneficiario.Text = Clients.GetRandomOtherClient().Value;
            txb_BCCk_Beneficiario.Enabled = !((CheckBox)sender).Checked;
        }

        #endregion



        #region [ HACK: numeric input handler for max cash calue ]
        private void txbApp_MaxCashValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            // HACK: still haven't figured out a proper way to create a number-only input with decimals
            var handled = true;
            if (char.IsDigit(e.KeyChar)) { handled = false; }
            if (char.IsControl(e.KeyChar)) { handled = false; }
            if ((e.KeyChar == '.') && (!((TextBox)sender).Text.Contains("."))) { handled = false; }
            if ((e.KeyChar == '.') && (((TextBox)sender).SelectionLength == ((TextBox)sender).TextLength)) { handled = false; }
            if (((TextBox)sender).Text.Split('.').Length < 2) { handled = false; }
            if (((TextBox)sender).Text.Contains("."))
            {
                var str = ((TextBox)sender).Text.Split('.');
                if (str[1].Length <= 2) { handled = false; }
            }
            e.Handled = handled;
        }
        #endregion
    }
}
