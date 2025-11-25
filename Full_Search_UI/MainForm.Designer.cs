namespace FullSearch
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;
        private Panel panelGrid; // panel is a container for other controls, mainly for grouping and layout
        private ComboBox comboAlgorithm; // combo box is the equivelant of a drop down menu
        private Button btnRun; // button to run the selected search algorithm
        private Button btnLoadMap; // button to load a new map file
        private Label lblAlg; // label for the algorithm selection combo box
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
            panelGrid = new Panel();
            comboAlgorithm = new ComboBox();
            btnRun = new Button();
            btnLoadMap = new Button();
            lblAlg = new Label();
            txtOutput = new TextBox();
            openMapDialog = new OpenFileDialog();
            SuspendLayout();
            // 
            // panelGrid
            // 
            panelGrid.Location = new Point(12, 12);
            panelGrid.Name = "panelGrid";
            panelGrid.Size = new Size(480, 480);
            panelGrid.TabIndex = 0;
            // 
            // comboAlgorithm
            // 
            comboAlgorithm.DropDownStyle = ComboBoxStyle.DropDownList;
            comboAlgorithm.Location = new Point(510, 70);
            comboAlgorithm.Name = "comboAlgorithm";
            comboAlgorithm.Size = new Size(200, 23);
            comboAlgorithm.TabIndex = 1;
            comboAlgorithm.SelectedIndexChanged += comboAlgorithm_SelectedIndexChanged;
            // 
            // btnRun
            // 
            btnRun.Location = new Point(510, 110);
            btnRun.Name = "btnRun";
            btnRun.Size = new Size(200, 30);
            btnRun.TabIndex = 2;
            btnRun.Text = "Run Search";
            btnRun.Click += btnRun_Click;
            // 
            // btnLoadMap
            // 
            btnLoadMap.Location = new Point(510, 20);
            btnLoadMap.Name = "btnLoadMap";
            btnLoadMap.Size = new Size(200, 30);
            btnLoadMap.TabIndex = 3;
            btnLoadMap.Text = "Load a new map";
            btnLoadMap.UseVisualStyleBackColor = true;
            btnLoadMap.Click += btnLoadMap_Click;
            // 
            // lblAlg
            // 
            lblAlg.Location = new Point(510, 50);
            lblAlg.Name = "lblAlg";
            lblAlg.Size = new Size(200, 20);
            lblAlg.TabIndex = 4;
            lblAlg.Text = "Algorithm:";
            // 
            // txtOutput
            // 
            txtOutput.Location = new Point(12, 500);
            txtOutput.Multiline = true;
            txtOutput.Name = "txtOutput";
            txtOutput.ScrollBars = ScrollBars.Vertical;
            txtOutput.Size = new Size(698, 140);
            txtOutput.TabIndex = 5;
            // 
            // MainForm
            // 
            ClientSize = new Size(722, 652);
            Controls.Add(panelGrid);
            Controls.Add(comboAlgorithm);
            Controls.Add(btnRun);
            Controls.Add(btnLoadMap);
            Controls.Add(lblAlg);
            Controls.Add(txtOutput);
            Name = "MainForm";
            Text = "Full Search";
            ResumeLayout(false);
            PerformLayout();
        }
    }
}
