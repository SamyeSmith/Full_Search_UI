namespace FullSearch
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;
        private Panel panelGrid;
        private ComboBox comboAlgorithm;
        private Button btnRun;
        private Button btnLoadMap;
        private Label lblAlg;
        private TextBox txtOutput;

        private OpenFileDialog openMapDialog;

        private string loadedMapFile = null;
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();

            this.panelGrid = new Panel();
            this.comboAlgorithm = new ComboBox();
            this.btnRun = new Button();
            this.btnLoadMap = new Button();
            this.lblAlg = new Label();
            this.txtOutput = new TextBox();

            this.openMapDialog = new OpenFileDialog();

            this.SuspendLayout();

            // panelGrid
            this.panelGrid.Location = new System.Drawing.Point(12, 12);
            this.panelGrid.Name = "panelGrid";
            this.panelGrid.Size = new System.Drawing.Size(480, 480);
            this.panelGrid.TabIndex = 0;

            // comboAlgorithm
            this.comboAlgorithm.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboAlgorithm.Location = new System.Drawing.Point(510, 70);
            this.comboAlgorithm.Name = "comboAlgorithm";
            this.comboAlgorithm.Size = new System.Drawing.Size(200, 23);

            // btnRun
            this.btnRun.Location = new System.Drawing.Point(510, 110);
            this.btnRun.Name = "btnRun";
            this.btnRun.Size = new System.Drawing.Size(200, 30);
            this.btnRun.Text = "Run Search";
            this.btnRun.Click += new System.EventHandler(this.btnRun_Click);

            // btnLoadMap (from CODE 1)
            this.btnLoadMap.Location = new System.Drawing.Point(510, 20);
            this.btnLoadMap.Name = "btnLoadMap";
            this.btnLoadMap.Size = new System.Drawing.Size(200, 30);
            this.btnLoadMap.Text = "Load a new map";
            this.btnLoadMap.UseVisualStyleBackColor = true;
            this.btnLoadMap.Click += new System.EventHandler(this.btnLoadMap_Click);

            // lblAlg
            this.lblAlg.Location = new System.Drawing.Point(510, 50);
            this.lblAlg.Size = new System.Drawing.Size(200, 20);
            this.lblAlg.Text = "Algorithm:";

            // txtOutput
            this.txtOutput.Location = new System.Drawing.Point(12, 500);
            this.txtOutput.Multiline = true;
            this.txtOutput.ScrollBars = ScrollBars.Vertical;
            this.txtOutput.Name = "txtOutput";
            this.txtOutput.Size = new System.Drawing.Size(698, 140);

            // MainForm
            this.ClientSize = new System.Drawing.Size(722, 652);
            this.Controls.Add(this.panelGrid);
            this.Controls.Add(this.comboAlgorithm);
            this.Controls.Add(this.btnRun);
            this.Controls.Add(this.btnLoadMap);
            this.Controls.Add(this.lblAlg);
            this.Controls.Add(this.txtOutput);
            this.Name = "MainForm";
            this.Text = "Full Search - WinForms (Option A, Merged)";
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
