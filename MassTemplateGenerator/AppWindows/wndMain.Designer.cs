
namespace MassTemplateGenerator
{
    partial class WndMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WndMain));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cbxType = new System.Windows.Forms.ComboBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.numAmount = new System.Windows.Forms.NumericUpDown();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btnOpen = new System.Windows.Forms.Button();
            this.txbPath = new System.Windows.Forms.TextBox();
            this.stpMenu = new System.Windows.Forms.MenuStrip();
            this.archivoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.preferenciasToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.salirToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.opcionesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ayudaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contribuirAlProyectoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.acercaDeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btnGenerate = new System.Windows.Forms.Button();
            this.sfdSave = new System.Windows.Forms.SaveFileDialog();
            this.stpStatus = new System.Windows.Forms.StatusStrip();
            this.sblStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.tmrTimer = new System.Windows.Forms.Timer(this.components);
            this.ttTooltips = new System.Windows.Forms.ToolTip(this.components);
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numAmount)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.stpMenu.SuspendLayout();
            this.stpStatus.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cbxType);
            this.groupBox1.Location = new System.Drawing.Point(12, 27);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(276, 50);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Plantilla";
            // 
            // cbxType
            // 
            this.cbxType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxType.FormattingEnabled = true;
            this.cbxType.Location = new System.Drawing.Point(6, 19);
            this.cbxType.Name = "cbxType";
            this.cbxType.Size = new System.Drawing.Size(264, 21);
            this.cbxType.TabIndex = 0;
            this.ttTooltips.SetToolTip(this.cbxType, "Las plantillas disponibles se mostrarán en esta lista.");
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.numAmount);
            this.groupBox2.Location = new System.Drawing.Point(294, 27);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(68, 50);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Registros";
            // 
            // numAmount
            // 
            this.numAmount.Increment = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.numAmount.Location = new System.Drawing.Point(6, 19);
            this.numAmount.Maximum = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            this.numAmount.Name = "numAmount";
            this.numAmount.Size = new System.Drawing.Size(56, 20);
            this.numAmount.TabIndex = 1;
            this.ttTooltips.SetToolTip(this.numAmount, "Determina cuántos registros se generarán en el archivo resultante.");
            this.numAmount.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.btnOpen);
            this.groupBox3.Controls.Add(this.txbPath);
            this.groupBox3.Location = new System.Drawing.Point(12, 83);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(350, 50);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Archivo a guardar";
            // 
            // btnOpen
            // 
            this.btnOpen.Image = ((System.Drawing.Image)(resources.GetObject("btnOpen.Image")));
            this.btnOpen.Location = new System.Drawing.Point(311, 17);
            this.btnOpen.Name = "btnOpen";
            this.btnOpen.Size = new System.Drawing.Size(33, 23);
            this.btnOpen.TabIndex = 2;
            this.ttTooltips.SetToolTip(this.btnOpen, "Abre un cuadro de diálogo para seleccionar la ruta donde se generará el archivo r" +
        "esultante.");
            this.btnOpen.UseVisualStyleBackColor = true;
            this.btnOpen.Click += new System.EventHandler(this.BtnOpen_Click);
            // 
            // txbPath
            // 
            this.txbPath.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txbPath.Location = new System.Drawing.Point(6, 19);
            this.txbPath.Name = "txbPath";
            this.txbPath.ReadOnly = true;
            this.txbPath.Size = new System.Drawing.Size(299, 20);
            this.txbPath.TabIndex = 3;
            this.ttTooltips.SetToolTip(this.txbPath, "Muestra la ruta elegida para el archivo que será generado.");
            // 
            // stpMenu
            // 
            this.stpMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.archivoToolStripMenuItem,
            this.opcionesToolStripMenuItem});
            this.stpMenu.Location = new System.Drawing.Point(0, 0);
            this.stpMenu.Name = "stpMenu";
            this.stpMenu.Size = new System.Drawing.Size(484, 24);
            this.stpMenu.TabIndex = 3;
            this.stpMenu.Text = "menuStrip1";
            // 
            // archivoToolStripMenuItem
            // 
            this.archivoToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.preferenciasToolStripMenuItem,
            this.salirToolStripMenuItem});
            this.archivoToolStripMenuItem.Name = "archivoToolStripMenuItem";
            this.archivoToolStripMenuItem.Size = new System.Drawing.Size(75, 20);
            this.archivoToolStripMenuItem.Text = "&Aplicación";
            // 
            // preferenciasToolStripMenuItem
            // 
            this.preferenciasToolStripMenuItem.Name = "preferenciasToolStripMenuItem";
            this.preferenciasToolStripMenuItem.Size = new System.Drawing.Size(138, 22);
            this.preferenciasToolStripMenuItem.Text = "&Preferencias";
            this.preferenciasToolStripMenuItem.Click += new System.EventHandler(this.PreferenciasToolStripMenuItem_Click);
            // 
            // salirToolStripMenuItem
            // 
            this.salirToolStripMenuItem.Name = "salirToolStripMenuItem";
            this.salirToolStripMenuItem.ShortcutKeyDisplayString = "Alt+F4";
            this.salirToolStripMenuItem.Size = new System.Drawing.Size(138, 22);
            this.salirToolStripMenuItem.Text = "&Salir";
            this.salirToolStripMenuItem.Click += new System.EventHandler(this.SalirToolStripMenuItem_Click);
            // 
            // opcionesToolStripMenuItem
            // 
            this.opcionesToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ayudaToolStripMenuItem,
            this.contribuirAlProyectoToolStripMenuItem,
            this.acercaDeToolStripMenuItem});
            this.opcionesToolStripMenuItem.Name = "opcionesToolStripMenuItem";
            this.opcionesToolStripMenuItem.Size = new System.Drawing.Size(84, 20);
            this.opcionesToolStripMenuItem.Text = "In&formación";
            // 
            // ayudaToolStripMenuItem
            // 
            this.ayudaToolStripMenuItem.Name = "ayudaToolStripMenuItem";
            this.ayudaToolStripMenuItem.Size = new System.Drawing.Size(224, 22);
            this.ayudaToolStripMenuItem.Text = "In&formación de la aplicación";
            this.ayudaToolStripMenuItem.Click += new System.EventHandler(this.ayudaToolStripMenuItem_Click);
            // 
            // contribuirAlProyectoToolStripMenuItem
            // 
            this.contribuirAlProyectoToolStripMenuItem.Name = "contribuirAlProyectoToolStripMenuItem";
            this.contribuirAlProyectoToolStripMenuItem.Size = new System.Drawing.Size(224, 22);
            this.contribuirAlProyectoToolStripMenuItem.Text = "&Contribuir al proyecto";
            this.contribuirAlProyectoToolStripMenuItem.Click += new System.EventHandler(this.ContribuirAlProyecto_Click);
            // 
            // acercaDeToolStripMenuItem
            // 
            this.acercaDeToolStripMenuItem.Name = "acercaDeToolStripMenuItem";
            this.acercaDeToolStripMenuItem.Size = new System.Drawing.Size(224, 22);
            this.acercaDeToolStripMenuItem.Text = "&Acerca de...";
            this.acercaDeToolStripMenuItem.Click += new System.EventHandler(this.AcercaDeToolStripMenuItem_Click);
            // 
            // btnGenerate
            // 
            this.btnGenerate.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGenerate.Image = ((System.Drawing.Image)(resources.GetObject("btnGenerate.Image")));
            this.btnGenerate.Location = new System.Drawing.Point(368, 27);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Padding = new System.Windows.Forms.Padding(4, 0, 0, 0);
            this.btnGenerate.Size = new System.Drawing.Size(105, 106);
            this.btnGenerate.TabIndex = 4;
            this.btnGenerate.Text = "Generar";
            this.btnGenerate.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.ttTooltips.SetToolTip(this.btnGenerate, "Genera el archivo a partir de la plantilla elegida con los parámetros especificad" +
        "os.");
            this.btnGenerate.UseVisualStyleBackColor = true;
            this.btnGenerate.Click += new System.EventHandler(this.BtnGenerate_Click);
            // 
            // sfdSave
            // 
            this.sfdSave.DefaultExt = "txt";
            this.sfdSave.FileName = "plantilladecarga";
            this.sfdSave.Filter = "Archivos de texto|*.txt";
            // 
            // stpStatus
            // 
            this.stpStatus.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.sblStatus});
            this.stpStatus.Location = new System.Drawing.Point(0, 137);
            this.stpStatus.Name = "stpStatus";
            this.stpStatus.Size = new System.Drawing.Size(484, 22);
            this.stpStatus.SizingGrip = false;
            this.stpStatus.TabIndex = 5;
            // 
            // sblStatus
            // 
            this.sblStatus.Name = "sblStatus";
            this.sblStatus.Size = new System.Drawing.Size(35, 17);
            this.sblStatus.Text = "Listo.";
            // 
            // tmrTimer
            // 
            this.tmrTimer.Interval = 5000;
            this.tmrTimer.Tick += new System.EventHandler(this.TmrTimer_Tick);
            // 
            // ttTooltips
            // 
            this.ttTooltips.AutomaticDelay = 250;
            this.ttTooltips.IsBalloon = true;
            // 
            // WndMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(484, 159);
            this.Controls.Add(this.stpStatus);
            this.Controls.Add(this.btnGenerate);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.stpMenu);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.stpMenu;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(500, 198);
            this.MinimumSize = new System.Drawing.Size(500, 198);
            this.Name = "WndMain";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Generador de archivos de carga masiva";
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numAmount)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.stpMenu.ResumeLayout(false);
            this.stpMenu.PerformLayout();
            this.stpStatus.ResumeLayout(false);
            this.stpStatus.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox cbxType;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.NumericUpDown numAmount;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.MenuStrip stpMenu;
        private System.Windows.Forms.ToolStripMenuItem archivoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem preferenciasToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem salirToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem opcionesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem acercaDeToolStripMenuItem;
        private System.Windows.Forms.Button btnGenerate;
        private System.Windows.Forms.Button btnOpen;
        private System.Windows.Forms.TextBox txbPath;
        private System.Windows.Forms.SaveFileDialog sfdSave;
        private System.Windows.Forms.ToolStripMenuItem contribuirAlProyectoToolStripMenuItem;
        private System.Windows.Forms.StatusStrip stpStatus;
        private System.Windows.Forms.ToolStripStatusLabel sblStatus;
        private System.Windows.Forms.Timer tmrTimer;
        private System.Windows.Forms.ToolTip ttTooltips;
        private System.Windows.Forms.ToolStripMenuItem ayudaToolStripMenuItem;
    }
}

