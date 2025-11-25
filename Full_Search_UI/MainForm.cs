using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace FullSearch
{
    public partial class MainForm : Form
    {
        private int[,] terrain = null;
        private Coord start, goal;
        private List<Coord> currentPath = null;

        public MainForm()
        {
            InitializeComponent();
            comboAlgorithm.Items.AddRange(Enum.GetNames(typeof(Algorithm)));
            comboAlgorithm.SelectedIndex = 0;
            panelGrid.Paint += PanelGrid_Paint;
            panelGrid.Resize += (s,e)=> panelGrid.Invalidate();
        }

        private void btnRun_Click(object sender, EventArgs e)
        {
            // Ensure user has loaded a map
            if (loadedMapFile == null)
            {
                MessageBox.Show("Load a map first.");
                return;
            }

            // Load the map file chosen by the user
            (terrain, start, goal) = LoadMap(loadedMapFile);

            var algName = comboAlgorithm.SelectedItem.ToString();
            if (!Enum.TryParse<Algorithm>(algName, out var alg))
                alg = Algorithm.BreadthFirst;

            var pathfinder = PathFinderFactory.NewPathFinder(alg);
            currentPath = pathfinder.FindPath(terrain, start, goal);

            txtOutput.Clear();

            if (currentPath == null || currentPath.Count == 0)
            {
                txtOutput.AppendText("No path found.\r\n");
            }
            else
            {
                txtOutput.AppendText($"Path found ({currentPath.Count} steps):\r\n");
                foreach (var c in currentPath)
                    txtOutput.AppendText(c.ToString() + "\r\n");


                SaveFileDialog saveDialog = new SaveFileDialog();
                saveDialog.Title = "Save Path File";
                saveDialog.Filter = "Text Files (*.txt)|*.txt";
                saveDialog.FileName = Path.GetFileNameWithoutExtension(loadedMapFile)
                                        + "_Path_" + pathfinder.Name + ".txt";

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    using (var w = new StreamWriter(saveDialog.FileName))
                    {
                        foreach (var c in currentPath)
                            w.WriteLine($"{c.Row} {c.Col}");

                        if (pathfinder is AStar a)
                            w.WriteLine(a.OpenListSorts);
                    }

                    txtOutput.AppendText($"\r\nSaved to: {saveDialog.FileName}\r\n");
                }
                else
                {
                    txtOutput.AppendText("\r\nSave cancelled.\r\n");
                }
            }

            panelGrid.Invalidate();  // redraw with path
        }


        //private void btnRun_Click(object sender, EventArgs e)
        //{
        //    // Ensure user has loaded a map
        //    if (loadedMapFile == null)
        //    {
        //        MessageBox.Show("Load a map first.");
        //        return;
        //    }

        //    // Load the map file chosen by the user
        //    (terrain, start, goal) = LoadMap(loadedMapFile);

        //    var algName = comboAlgorithm.SelectedItem.ToString();
        //    if (!Enum.TryParse<Algorithm>(algName, out var alg))
        //        alg = Algorithm.BreadthFirst;

        //    var pathfinder = PathFinderFactory.NewPathFinder(alg);
        //    currentPath = pathfinder.FindPath(terrain, start, goal);

        //    txtOutput.Clear();

        //    if (currentPath == null || currentPath.Count == 0)
        //    {
        //        txtOutput.AppendText("No path found.\r\n");
        //    }
        //    else
        //    {
        //        txtOutput.AppendText($"Path found ({currentPath.Count} steps):\r\n");
        //        foreach (var c in currentPath)
        //            txtOutput.AppendText(c.ToString() + "\r\n");

        //        var outName = Path.GetFileNameWithoutExtension(loadedMapFile)
        //                     + "Path_" + pathfinder.Name + ".txt";

        //        using var w = new StreamWriter(outName);
        //        foreach (var c in currentPath)
        //            w.WriteLine(c.ToString());

        //        if (pathfinder is AStar a)
        //            w.WriteLine(a.OpenListSorts);

        //        txtOutput.AppendText($"\r\nSaved to: {outName}\r\n");
        //    }

        //    panelGrid.Invalidate();  // redraw after loading
        //}

        private void PanelGrid_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.Clear(Color.White);
            if (terrain == null) return;
            int rows = terrain.GetLength(0), cols = terrain.GetLength(1);
            int w = panelGrid.ClientSize.Width, h = panelGrid.ClientSize.Height;
            float cellW = (float)w / cols, cellH = (float)h / rows;

            var pathSet = new HashSet<(int,int)>();
            if (currentPath != null) foreach (var p in currentPath) pathSet.Add((p.Row, p.Col));

            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    var rect = new RectangleF(c * cellW, r * cellH, cellW, cellH);
                    Color fill = terrain[r,c] switch
                    {
                        0 => Color.Black,
                        1 => Color.White,
                        2 => Color.LightGreen,
                        3 => Color.LightBlue,
                        _ => Color.Gray
                    };
                    using (var brush = new SolidBrush(fill)) g.FillRectangle(brush, rect);
                    g.DrawRectangle(Pens.Gray, Rectangle.Round(rect));

                    if (start.Row == r && start.Col == c)
                    {
                        g.FillEllipse(Brushes.Orange, RectangleF.Inflate(rect, -cellW*0.2f, -cellH*0.2f));
                    }
                    else if (goal.Row == r && goal.Col == c)
                    {
                        g.FillEllipse(Brushes.Red, RectangleF.Inflate(rect, -cellW*0.2f, -cellH*0.2f));
                    }
                    else if (pathSet.Contains((r,c)))
                    {
                        g.FillRectangle(Brushes.Yellow, RectangleF.Inflate(rect, -cellW*0.15f, -cellH*0.15f));
                    }
                }
            }
        }
        private void btnLoadMap_Click(object sender, EventArgs e)
        {
            openMapDialog.Title = "Select map file";
            openMapDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";

            if (openMapDialog.ShowDialog() == DialogResult.OK)
            {
                loadedMapFile = openMapDialog.FileName;

                // Load map so it can be drawn immediately
                (terrain, start, goal) = LoadMap(loadedMapFile);
                currentPath = null;

                txtOutput.Text = $"Loaded map: {Path.GetFileName(loadedMapFile)}";

                panelGrid.Invalidate();   // <= ⭐ THIS DRAWS THE GRID NOW
            }
        }


        private (int[,], Coord, Coord) LoadMap(string filename)
        {
            var lines = File.ReadAllLines(filename);
            var dims = lines[0].Split(' ', StringSplitOptions.RemoveEmptyEntries);
            int rows = int.Parse(dims[0]), cols = int.Parse(dims[1]);
            var startParts = lines[1].Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var goalParts = lines[2].Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var start = new Coord(int.Parse(startParts[0]), int.Parse(startParts[1]));
            var goal = new Coord(int.Parse(goalParts[0]), int.Parse(goalParts[1]));
            var terrain = new int[rows, cols];
            for (int r = 0; r < rows; r++)
            {
                var rowparts = lines[3 + r].Split(' ', StringSplitOptions.RemoveEmptyEntries);
                for (int c = 0; c < cols; c++) terrain[r, c] = int.Parse(rowparts[c]);
            }
            return (terrain, start, goal);
        }
    }
}
