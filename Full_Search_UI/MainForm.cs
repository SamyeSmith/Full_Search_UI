using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace FullSearch
{
    // Main form for the pathfinding application
    public partial class MainForm : Form
    {
        private int[,] terrain = null; // 2D array representing the terrain grid
        private Coord start, goal; // start and goal coordinates
        private List<Coord> currentPath = null; // current path found by the algorithm

        public MainForm()
        {
            InitializeComponent();
            comboAlgorithm.Items.AddRange(Enum.GetNames(typeof(Algorithm)));
            comboAlgorithm.SelectedIndex = 0;
            panelGrid.Paint += PanelGrid_Paint;
            panelGrid.Resize += (s, e) => panelGrid.Invalidate();
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

        private void PanelGrid_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.Clear(Color.White);
            if (terrain == null) return;
            int rows = terrain.GetLength(0), cols = terrain.GetLength(1);
            int w = panelGrid.ClientSize.Width, h = panelGrid.ClientSize.Height;
            float cellW = (float)w / cols, cellH = (float)h / rows;

            var pathSet = new HashSet<(int, int)>();
            if (currentPath != null) foreach (var p in currentPath) pathSet.Add((p.Row, p.Col));

            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    var rect = new RectangleF(c * cellW, r * cellH, cellW, cellH);
                    Color fill = terrain[r, c] switch
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
                        g.FillEllipse(Brushes.Orange, RectangleF.Inflate(rect, -cellW * 0.2f, -cellH * 0.2f));
                    }
                    else if (goal.Row == r && goal.Col == c)
                    {
                        g.FillEllipse(Brushes.Red, RectangleF.Inflate(rect, -cellW * 0.2f, -cellH * 0.2f));
                    }
                    else if (pathSet.Contains((r, c)))
                    {
                        g.FillRectangle(Brushes.Yellow, RectangleF.Inflate(rect, -cellW * 0.15f, -cellH * 0.15f));
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

                panelGrid.Invalidate();
            }
        }


        private (int[,], Coord, Coord) LoadMap(string filename)
        {
            try
            {
                var lines = File.ReadAllLines(filename);


                if (lines.Length < 3)
                {
                    MessageBox.Show("Map file is too short. It must include dimensions, start, goal, and terrain rows.",
                        "Invalid Map File", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return (null, new Coord(), new Coord());
                }


                var dims = lines[0].Split(' ', StringSplitOptions.RemoveEmptyEntries);
                if (dims.Length != 2 ||
                    !int.TryParse(dims[0], out int rows) ||
                    !int.TryParse(dims[1], out int cols))
                {
                    MessageBox.Show("First line must contain two integers: ROWS COLS.",
                        "Invalid Map File", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return (null, new Coord(), new Coord());
                }


                var startParts = lines[1].Split(' ', StringSplitOptions.RemoveEmptyEntries);
                if (startParts.Length != 2 ||
                    !int.TryParse(startParts[0], out int sRow) ||
                    !int.TryParse(startParts[1], out int sCol))
                {
                    MessageBox.Show("Second line must contain: START_ROW START_COL.",
                        "Invalid Map File", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return (null, new Coord(), new Coord());
                }


                var goalParts = lines[2].Split(' ', StringSplitOptions.RemoveEmptyEntries);
                if (goalParts.Length != 2 ||
                    !int.TryParse(goalParts[0], out int gRow) ||
                    !int.TryParse(goalParts[1], out int gCol))
                {
                    MessageBox.Show("Third line must contain: GOAL_ROW GOAL_COL.",
                        "Invalid Map File", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return (null, new Coord(), new Coord());
                }


                if (lines.Length < 3 + rows)
                {
                    MessageBox.Show($"Map file does not contain enough terrain rows. Expected {rows}.",
                        "Invalid Map File", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return (null, new Coord(), new Coord());
                }

                var terrain = new int[rows, cols];


                for (int r = 0; r < rows; r++)
                {
                    var rowparts = lines[3 + r].Split(' ', StringSplitOptions.RemoveEmptyEntries);

                    if (rowparts.Length != cols)
                    {
                        MessageBox.Show($"Row {r + 4} does not contain {cols} values.",
                            "Invalid Map File", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return (null, new Coord(), new Coord());
                    }

                    for (int c = 0; c < cols; c++)
                    {
                        if (!int.TryParse(rowparts[c], out int val))
                        {
                            MessageBox.Show($"Invalid terrain value at row {r + 4}, column {c + 1}.",
                                "Invalid Map File", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return (null, new Coord(), new Coord());
                        }

                        terrain[r, c] = val;
                    }
                }


                if (sRow < 0 || sRow >= rows || sCol < 0 || sCol >= cols)
                {
                    MessageBox.Show("Start position is outside the grid.",
                        "Invalid Map File", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return (null, new Coord(), new Coord());
                }

                if (gRow < 0 || gRow >= rows || gCol < 0 || gCol >= cols)
                {
                    MessageBox.Show("Goal position is outside the grid.",
                        "Invalid Map File", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return (null, new Coord(), new Coord());
                }


                return (terrain, new Coord(sRow, sCol), new Coord(gRow, gCol));
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading map:\n" + ex.Message,
                    "Map Load Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return (null, new Coord(), new Coord());
            }
        }

        private void comboAlgorithm_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
