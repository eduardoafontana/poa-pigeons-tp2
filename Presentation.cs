using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static PigeonsTP2.Environment;

namespace PigeonsTP2
{
    /// <summary>
    /// This class is responsible for initializing the game and rendering the elements on screen.
    /// </summary>
    public partial class Presentation : Form
    {
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;

        public Presentation()
        {
            /// <summary>
            /// This method is responsible for initialize the standard graphic elements of windows forms.
            /// </summary>
            InitializeComponent();

            /// <summary>
            /// This method is responsible for initialize the game specific graphic elements.
            /// </summary>
            CreateEnvironmentPresentation();

            /// <summary>
            /// Starts the game engine, where basic objects are created.
            /// </summary>
            Engine.Init();

            /// <summary>
            /// Association with game threads, to update the graphic elements on the form. 
            /// The association makes the connection through asynchronous delegates between the threads and the screen object.
            /// </summary>
            Engine.environment.actuator.RaiseChangeEnvironment += new EnvironmentActuator.ChangingEnvironmentActuator(Presentation_OnEnvironmentChange);
            Engine.environment.actuator.RaiseChangeCat += new EnvironmentActuator.ChangingCatActuator(Presentation_OnCatChange);

            /// <summary>
            /// Start the game.
            /// </summary>
            Engine.Start();
        }

        /// <summary>
        /// Method responsible for updating the graphic elements. 
        /// Note that there is no game logic here, just updating the object according to its state. The game logic is controlled by the objects in the game.
        /// This method updates pigeons and feeds specifically.
        /// </summary>
        private void Presentation_OnEnvironmentChange(Place place)
        {
            try
            {
                Invoke(new Action(() =>
                {
                    if (place == null)
                        return;

                    PictureBox picture = (PictureBox)tableLayoutPanel1.Controls.Find("pictureBoxGeneral" + place.Index.ToString(), false)[0];

                    picture.ImageLocation = String.Empty;

                    if (place.Pigeon != null)
                        picture.ImageLocation = place.Pigeon.ImagePath;

                    if (place.Food != null)
                        picture.ImageLocation = place.Food.ImagePath;
                }));
            }
            catch (System.InvalidOperationException)
            {
                Engine.Stop();
            }
        }

        /// <summary>
        /// Method responsible for updating the graphic elements. 
        /// Note that there is no game logic here, just updating the object according to its state. The game logic is controlled by the objects in the game.
        /// This method updates the cat specifically.
        /// </summary>
        private void Presentation_OnCatChange(bool visible)
        {
            try
            {
                Invoke(new Action(() =>
                {
                    pictureBox2.Visible = visible;
                }));
            }
            catch (System.InvalidOperationException)
            {
                Engine.Stop();
            }
        }

        /// <summary>
        /// Method that detects the click on the form and delegates this action to the game. There is no game logic here.
        /// </summary>
        public void ClickOnPlace(object sender, MouseEventArgs e)
        {
            Engine.environment.TryAddFood(tableLayoutPanel1.GetColumn((PictureBox)sender));
        }

        private void CreateEnvironmentPresentation()
        {
            int placeSize = Config.environmentPlaceSizeWidth * Config.environmentSize;

            tableLayoutPanel1 = new TableLayoutPanel();

            tableLayoutPanel1.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            tableLayoutPanel1.ColumnCount = Config.environmentSize;
            tableLayoutPanel1.RowCount = 1;
            tableLayoutPanel1.Size = new System.Drawing.Size(placeSize + Config.environmentSize, Config.environmentPlaceSizeHeight);
            tableLayoutPanel1.Location = new System.Drawing.Point(71, 512);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.BackColor = Color.Transparent;
            tableLayoutPanel1.CellBorderStyle = TableLayoutPanelCellBorderStyle.None;
            tableLayoutPanel1.BringToFront();

            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, Config.environmentPlaceSizeWidth));

            for (int e = 0; e < Config.environmentSize; e++)
            {
                tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, Config.environmentPlaceSizeWidth));
            }

            pictureBox1.Controls.Add(tableLayoutPanel1);

            for (int e = 0; e < Config.environmentSize; e++)
            { 
                PictureBox pictureBoxGeneral = new PictureBox();
                ((ISupportInitialize)(pictureBoxGeneral)).BeginInit();
                pictureBoxGeneral.InitialImage = null;
                pictureBoxGeneral.Name = "pictureBoxGeneral" + e.ToString();
                pictureBoxGeneral.Size = new System.Drawing.Size(Config.environmentPlaceSizeWidth, 55);
                pictureBoxGeneral.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
                pictureBoxGeneral.Anchor = AnchorStyles.Bottom;
                pictureBoxGeneral.Visible = true;
                pictureBoxGeneral.MouseClick += new MouseEventHandler(ClickOnPlace);
                tableLayoutPanel1.Controls.Add(pictureBoxGeneral, e, 0);
                ((ISupportInitialize)(pictureBoxGeneral)).EndInit();
            }

            pictureBox2.Visible = false;
        }

        private void Presentation_FormClosed(object sender, FormClosedEventArgs e)
        {
            Engine.Stop();
        }
    }
}
