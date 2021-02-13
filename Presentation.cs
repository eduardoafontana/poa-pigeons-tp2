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

            Engine.Start();
        }

        private void presentation_OnEnvironmentChange(List<Place> places)
        {
            Invoke(new Action(() =>
            {
                int count = 0;

                string place = String.Empty;

                foreach (var item in places)
                {
                    PictureBox picture = (PictureBox)tableLayoutPanel1.Controls.Find("pictureBoxGeneral" + count.ToString(), false)[0];

                    if (item.pigeon != null)
                    {
                        picture.ImageLocation = item.pigeon.ImagePath;
                        picture.Visible = true;
                    }
                    else
                    {
                        picture.Visible = false;
                    }

                    count++;
                }
            }));
        }

        private void CreateEnvironmentPresentation()
        {
            int placeSize = Config.environmentPlaceSizeWidth * Config.environmentSize;

            tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();

            tableLayoutPanel1.BackColor = System.Drawing.Color.Gainsboro;
            tableLayoutPanel1.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            tableLayoutPanel1.ColumnCount = Config.environmentSize;
            tableLayoutPanel1.RowCount = 1;
            tableLayoutPanel1.Size = new System.Drawing.Size(placeSize + Config.environmentSize, Config.environmentPlaceSizeHeight);
            tableLayoutPanel1.Location = new System.Drawing.Point(71, 512);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.BackColor = Color.Azure;
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
                pictureBoxGeneral.Size = new System.Drawing.Size(40, 55);
                pictureBoxGeneral.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
                pictureBoxGeneral.Anchor = AnchorStyles.None;
                pictureBoxGeneral.Visible = false;
                tableLayoutPanel1.Controls.Add(pictureBoxGeneral, e, 0);
                ((ISupportInitialize)(pictureBoxGeneral)).EndInit();
            }
        }

        private void Presentation_FormClosed(object sender, FormClosedEventArgs e)
        {
            Engine.Stop();
        }
    }
}
