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

        // Loaded map file path
        public MainForm()
        {
            InitializeComponent(); // Initialize UI components (ie. buttons, panels, etc)
            comboAlgorithm.Items.AddRange(Enum.GetNames(typeof(Algorithm))); // Populate algorithm selection combo box
            comboAlgorithm.SelectedIndex = 0; // Default to first algorithm
            panelGrid.Paint += PanelGrid_Paint; // Attach paint event handler for grid panel
            panelGrid.Resize += (s, e) => panelGrid.Invalidate(); // Redraw grid on resize
        }

        private void btnRun_Click(object sender, EventArgs e) // Event handler for "Run Search" button click
        {
            // Ensure user has loaded a map
            if (loadedMapFile == null) // No map loaded
            {
                MessageBox.Show("Load a map first."); // Prompt user to load a map
                return;
            }

            // Load the map file chosen by the user
            (terrain, start, goal) = LoadMap(loadedMapFile); // Load terrain, start, and goal from the map file

            var algName = comboAlgorithm.SelectedItem.ToString(); // Get selected algorithm name
            if (!Enum.TryParse<Algorithm>(algName, out var alg)) // Parse algorithm enum
                alg = Algorithm.BreadthFirst; // Default to BreadthFirst if parsing fails

            var pathfinder = PathFinderFactory.NewPathFinder(alg); // Create pathfinder instance based on selected algorithm
            currentPath = pathfinder.FindPath(terrain, start, goal); // Find path using the selected algorithm

            txtOutput.Clear(); // Clear previous output

            if (currentPath == null || currentPath.Count == 0) // No path found
            {
                txtOutput.AppendText("No path found.\r\n"); // Indicate no path was found
            }
            else // Path found
            {
                txtOutput.AppendText($"Path found ({currentPath.Count} steps):\r\n"); // Indicate path found with step count
                foreach (var c in currentPath) // Output each coordinate in the path
                    txtOutput.AppendText(c.ToString() + "\r\n"); // Output coordinate


                SaveFileDialog saveDialog = new SaveFileDialog(); // Prompt user to save the path to a file
                saveDialog.Title = "Save Path File";
                saveDialog.Filter = "Text Files (*.txt)|*.txt"; // Only allow text files
                saveDialog.FileName = Path.GetFileNameWithoutExtension(loadedMapFile) // Base map name
                                        + "_Path_" + pathfinder.Name + ".txt"; // Default file name

                if (saveDialog.ShowDialog() == DialogResult.OK) // User chose to save
                {
                    using (var w = new StreamWriter(saveDialog.FileName)) // Write path to file
                    {
                        foreach (var c in currentPath) // Write each coordinate
                            w.WriteLine($"{c.Row} {c.Col}"); // Write coordinate to file

                        if (pathfinder is AStar a) // If using A* algorithm, save additional info
                            w.WriteLine(a.OpenListSorts); // Write number of open list sorts
                        w.WriteLine($"Total Path Length: {currentPath.Count}"); // Write total path length
                    }

                    txtOutput.AppendText($"\r\nSaved to: {saveDialog.FileName}\r\n"); // Indicate successful save
                }
                else
                {
                    txtOutput.AppendText("\r\nSave cancelled.\r\n"); // Indicate save was cancelled
                }
            }

            panelGrid.Invalidate(); // Redraw the grid with the new path
        }

        private void PanelGrid_Paint(object sender, PaintEventArgs e) // Paint event handler for grid panel
        {
            var g = e.Graphics; // Get graphics context
            g.Clear(Color.White); // Clear background

            if (terrain == null) return; // No terrain to draw

            int rows = terrain.GetLength(0), cols = terrain.GetLength(1); // Get terrain dimensions
            int w = panelGrid.ClientSize.Width, h = panelGrid.ClientSize.Height; // Get panel size
            float cellW = (float)w / cols, cellH = (float)h / rows; // Calculate cell size

            var pathSet = new HashSet<(int, int)>(); // Set to track path coordinates for quick lookup
            if (currentPath != null) foreach (var p in currentPath) pathSet.Add((p.Row, p.Col)); // Draw each cell in the terrain grid

            for (int r = 0; r < rows; r++) // Iterate over rows
            {
                for (int c = 0; c < cols; c++) // Iterate over columns
                {
                    var rect = new RectangleF(c * cellW, r * cellH, cellW, cellH); // Calculate cell rectangle
                    Color fill = terrain[r, c] switch // Determine fill color based on terrain type
                    {
                        0 => Color.Black,
                        1 => Color.White,
                        2 => Color.LightGreen,
                        3 => Color.LightBlue,
                        _ => Color.Gray
                    };
                    using (var brush = new SolidBrush(fill)) g.FillRectangle(brush, rect); // Fill cell with color
                    g.DrawRectangle(Pens.Gray, Rectangle.Round(rect)); // Draw cell border

                    if (start.Row == r && start.Col == c) // Draw start position
                    {
                        g.FillEllipse(Brushes.Orange, RectangleF.Inflate(rect, -cellW * 0.2f, -cellH * 0.2f)); // Draw start as orange circle
                    }
                    else if (goal.Row == r && goal.Col == c) // Draw goal position
                    {
                        g.FillEllipse(Brushes.Red, RectangleF.Inflate(rect, -cellW * 0.2f, -cellH * 0.2f)); // Draw goal as red circle
                    }
                    else if (pathSet.Contains((r, c))) // Draw path cell
                    {
                        g.FillRectangle(Brushes.Yellow, RectangleF.Inflate(rect, -cellW * 0.15f, -cellH * 0.15f)); // Draw path cell as yellow rectangle
                    }
                }
            }
        }
        private void btnLoadMap_Click(object sender, EventArgs e) // Event handler for "Load Map" button click
        {
            openMapDialog.Title = "Select map file"; // Set dialog title
            openMapDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*"; // Set file filter

            if (openMapDialog.ShowDialog() == DialogResult.OK) // User selected a file
            {
                loadedMapFile = openMapDialog.FileName; // Store selected file path

                // Load map so it can be drawn immediately
                (terrain, start, goal) = LoadMap(loadedMapFile);
                currentPath = null; // Clear any existing path

                txtOutput.Text = $"Loaded map: {Path.GetFileName(loadedMapFile)}"; // Indicate successful load

                panelGrid.Invalidate(); // Redraw grid with new map
            }
        }


        private (int[,], Coord, Coord) LoadMap(string filename) // Load map from file
        {
            try // Try to read and parse the map file
            {
                var lines = File.ReadAllLines(filename); // Read all lines from the file


                if (lines.Length < 3) // Ensure there are enough lines for dimensions, start, and goal
                {
                    MessageBox.Show("Map file is too short. It must include dimensions, start, goal, and terrain rows.",
                        "Invalid Map File", MessageBoxButtons.OK, MessageBoxIcon.Error); // Show error message
                    return (null, new Coord(), new Coord()); // Return nulls on error
                }


                var dims = lines[0].Split(' ', StringSplitOptions.RemoveEmptyEntries); // Parse dimensions from the first line
                if (dims.Length != 2 || // Ensure there are exactly two dimension values
                    !int.TryParse(dims[0], out int rows) ||
                    !int.TryParse(dims[1], out int cols))
                {
                    MessageBox.Show("First line must contain two integers: ROWS COLS.",
                        "Invalid Map File", MessageBoxButtons.OK, MessageBoxIcon.Error); // Show error message
                    return (null, new Coord(), new Coord()); // Return nulls on error
                }


                var startParts = lines[1].Split(' ', StringSplitOptions.RemoveEmptyEntries); // Parse start position from the second line
                if (startParts.Length != 2 || // Ensure there are exactly two start position values
                    !int.TryParse(startParts[0], out int sRow) ||
                    !int.TryParse(startParts[1], out int sCol))
                {
                    MessageBox.Show("Second line must contain: START_ROW START_COL.",
                        "Invalid Map File", MessageBoxButtons.OK, MessageBoxIcon.Error);// Show error message
                    return (null, new Coord(), new Coord()); // Return nulls on error
                }


                var goalParts = lines[2].Split(' ', StringSplitOptions.RemoveEmptyEntries); // Parse goal position from the third line
                if (goalParts.Length != 2 || // Ensure there are exactly two goal position values
                    !int.TryParse(goalParts[0], out int gRow) ||
                    !int.TryParse(goalParts[1], out int gCol))
                {
                    MessageBox.Show("Third line must contain: GOAL_ROW GOAL_COL.",
                        "Invalid Map File", MessageBoxButtons.OK, MessageBoxIcon.Error); // Show error message
                    return (null, new Coord(), new Coord()); // Return nulls on error
                }


                if (lines.Length < 3 + rows) // Ensure there are enough lines for the terrain rows
                {
                    MessageBox.Show($"Map file does not contain enough terrain rows. Expected {rows}.",
                        "Invalid Map File", MessageBoxButtons.OK, MessageBoxIcon.Error); // Show error message
                    return (null, new Coord(), new Coord()); // Return nulls on error
                }

                var terrain = new int[rows, cols]; // Initialize terrain array


                for (int r = 0; r < rows; r++) // Parse each terrain row
                {
                    var rowparts = lines[3 + r].Split(' ', StringSplitOptions.RemoveEmptyEntries); // Split row into individual terrain values

                    if (rowparts.Length != cols) // Ensure the correct number of columns
                    {
                        MessageBox.Show($"Row {r + 4} does not contain {cols} values.",
                            "Invalid Map File", MessageBoxButtons.OK, MessageBoxIcon.Error); // Show error message
                        return (null, new Coord(), new Coord()); // Return nulls on error
                    }

                    for (int c = 0; c < cols; c++) // Parse each terrain value
                    {
                        if (!int.TryParse(rowparts[c], out int val)) // Parse terrain value
                        {
                            MessageBox.Show($"Invalid terrain value at row {r + 4}, column {c + 1}.",
                                "Invalid Map File", MessageBoxButtons.OK, MessageBoxIcon.Error); // Show error message
                            return (null, new Coord(), new Coord()); // Return nulls on error
                        }

                        terrain[r, c] = val; // Store terrain value
                    }
                }


                if (sRow < 0 || sRow >= rows || sCol < 0 || sCol >= cols) // Validate start position
                {
                    MessageBox.Show("Start position is outside the grid.",
                        "Invalid Map File", MessageBoxButtons.OK, MessageBoxIcon.Error); // Show error message
                    return (null, new Coord(), new Coord()); // Return nulls on error
                }

                if (gRow < 0 || gRow >= rows || gCol < 0 || gCol >= cols) // Validate goal position
                {
                    MessageBox.Show("Goal position is outside the grid.",
                        "Invalid Map File", MessageBoxButtons.OK, MessageBoxIcon.Error); // Show error message
                    return (null, new Coord(), new Coord()); // Return nulls on error
                }


                return (terrain, new Coord(sRow, sCol), new Coord(gRow, gCol)); // Return loaded terrain, start, and goal
            }
            catch (Exception ex) // Catch any exceptions during file reading/parsing
            {
                MessageBox.Show("Error loading map:\n" + ex.Message,
                    "Map Load Error", MessageBoxButtons.OK, MessageBoxIcon.Error); // Show error message
                return (null, new Coord(), new Coord()); // Return nulls on error
            }
        }

        private void comboAlgorithm_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
