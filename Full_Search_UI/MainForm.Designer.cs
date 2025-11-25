namespace FullSearch
{
    partial class MainForm
    {
        // Required designer variables for Windows Forms, all toolbox components
        private System.ComponentModel.IContainer components = null;// container for components

        private Panel panelGrid; // panel is a container for other controls, mainly for grouping and layout

        private ComboBox comboAlgorithm; // combo box is the equivelant of a drop down menu

        private Button btnRun; // button to run the selected search algorithm
        private Button btnLoadMap; // button to load a new map file

        private Label lblAlg; // label for the algorithm selection combo box

        private TextBox txtOutput;// text box to display output information

        private OpenFileDialog openMapDialog; // dialog to open map files

        private string loadedMapFile = null; // path to the currently loaded map file
        // Dispose method to clean up any resources being used
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        // Method to initialize the components of the form
        private void InitializeComponent() 
        {
            panelGrid = new Panel(); // panel to display the grid
            comboAlgorithm = new ComboBox(); // combo box for selecting the search algorithm
            btnRun = new Button(); // button to run the search
            btnLoadMap = new Button(); // button to load a new map
            lblAlg = new Label(); // label for the algorithm selection
            txtOutput = new TextBox(); // text box for output display
            openMapDialog = new OpenFileDialog(); // dialog to open map files
            SuspendLayout(); // suspend layout logic while initializing components
            // 
            // panelGrid
            // 
            panelGrid.Location = new Point(12, 12); // position of the panel
            panelGrid.Name = "panelGrid"; // name of the panel
            panelGrid.Size = new Size(480, 480); // size of the panel
            panelGrid.TabIndex = 0;
            // 
            // comboAlgorithm
            // 
            comboAlgorithm.DropDownStyle = ComboBoxStyle.DropDownList; // set to drop-down list style
            comboAlgorithm.Location = new Point(510, 70); // position of the combo box
            comboAlgorithm.Name = "comboAlgorithm"; // name of the combo box
            comboAlgorithm.Size = new Size(200, 23); // size of the combo box
            comboAlgorithm.TabIndex = 1; 
            comboAlgorithm.SelectedIndexChanged += comboAlgorithm_SelectedIndexChanged; // what happens when the selected index changes
            // 
            // btnRun
            // 
            btnRun.Location = new Point(510, 110); // position of the run button
            btnRun.Name = "btnRun"; // name of the run button
            btnRun.Size = new Size(200, 30); // size of the run button
            btnRun.TabIndex = 2; 
            btnRun.Text = "Run Search"; // text displayed on the run button
            btnRun.Click += btnRun_Click; // next step when the run button is clicked
            // 
            // btnLoadMap
            // 
            btnLoadMap.Location = new Point(510, 20); // position of the load map button
            btnLoadMap.Name = "btnLoadMap"; // name of the load map button
            btnLoadMap.Size = new Size(200, 30); // size of the load map button
            btnLoadMap.TabIndex = 3;
            btnLoadMap.Text = "Load a new map"; // text displayed on the load map button
            btnLoadMap.UseVisualStyleBackColor = true; // use visual styles for the button
            btnLoadMap.Click += btnLoadMap_Click; // next step when the load map button is clicked
            // 
            // lblAlg
            // 
            lblAlg.Location = new Point(510, 50); // position of the algorithm label
            lblAlg.Name = "lblAlg"; // name of the algorithm label
            lblAlg.Size = new Size(200, 20); // size of the algorithm label
            lblAlg.TabIndex = 4;
            lblAlg.Text = "Algorithm:"; // text displayed on the algorithm label
            // 
            // txtOutput
            // 
            txtOutput.Location = new Point(12, 500); // position of the output text box
            txtOutput.Multiline = true; // allow multiple lines of text
            txtOutput.Name = "txtOutput"; // name of the output text box
            txtOutput.ScrollBars = ScrollBars.Vertical; // vertical scroll bars for the text box
            txtOutput.Size = new Size(698, 140); // size of the output text box
            txtOutput.TabIndex = 5;
            // 
            // MainForm
            // 
            ClientSize = new Size(722, 652); // size of the main form
            Controls.Add(panelGrid); // add the panel to the form
            Controls.Add(comboAlgorithm); // add the combo box to the form
            Controls.Add(btnRun); // add the run button to the form
            Controls.Add(btnLoadMap); // add the load map button to the form
            Controls.Add(lblAlg); // add the algorithm label to the form
            Controls.Add(txtOutput); // add the output text box to the form
            Name = "MainForm"; // name of the main form
            Text = "Full Search"; // title of the main form
            ResumeLayout(false); // resume layout logic
            PerformLayout(); // perform layout
        }
    }
}
