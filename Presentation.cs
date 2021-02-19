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
    public partial class Presentation : Form
    {
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;

        public Presentation()
        {
            InitializeComponent();

            CreateEnvironmentPresentation();

            Engine.Init();

            Engine.environment.actuator.RaiseChangeEnvironment += new EnvironmentActuator.ChangingEnvironmentActuator(presentation_OnEnvironmentChange);
            Engine.environment.actuator.RaiseChangeCat += new EnvironmentActuator.ChangingCatActuator(presentation_OnCatChange);

            Engine.Start();
        }

        private void presentation_OnEnvironmentChange(Place place)
        {
            try
            {
                Invoke(new Action(() =>
                {
                    if (place == null)
                        return;

                    PictureBox picture = (PictureBox)tableLayoutPanel1.Controls.Find("pictureBoxGeneral" + place.index.ToString(), false)[0];

                    picture.ImageLocation = String.Empty;

                    if (place.pigeon != null)
                        picture.ImageLocation = place.pigeon.ImagePath;

                    if (place.food != null)
                        picture.ImageLocation = place.food.ImagePath;
                }));
            }
            catch (System.InvalidOperationException ex)
            {
                Engine.Stop();
            }
        }

        private void presentation_OnCatChange(bool visible)
        {
            try
            {
                Invoke(new Action(() =>
                {
                    pictureBox2.Visible = visible;
                }));
            }
            catch (System.InvalidOperationException ex)
            {
                Engine.Stop();
            }
        }

        public void ClickOnPlace(object sender, MouseEventArgs e)
        {
            Engine.environment.TryAddFood(tableLayoutPanel1.GetColumn((PictureBox)sender));
        }

        private void CreateEnvironmentPresentation()
        {
            int placeSize = Config.environmentPlaceSizeWidth * Config.environmentSize;

            tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();

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
