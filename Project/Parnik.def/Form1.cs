using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Microsoft.VisualBasic;

namespace Parnik.def
{

    struct Parnikvar
    {
        public UInt32 FileSgn;
        public UInt32 unknown;

    }

    struct ParnikCoords
    {
        public Single posX;
        public Single posY;
        public Single posZ;
        public Single rotX;
        public Single rotY;
        public Single rotZ;
        public List<Single> objNames;
        public List<Single> rotlist;

        public ParnikCoords(Single pX, Single pY, Single pZ, Single rX, Single rY, Single rZ, List<Single> objN, List<Single> rl)
        {
            posX = pX;
            posY = pY;
            posZ = pZ;
            rotX = rX;
            rotY = rY;
            rotZ = rZ;
            objNames = objN;
            rotlist = rl;
            
        }
    }



    public partial class Form1 : Form
    {
        private Single positionX;
        private Single positionY;
        private Single positionZ;
        private Single rotationX;
        private Single rotationY;
        private Single rotationZ;
        List<ParnikCoords> ParnikCoords;
        private List<Single> nameslist;
        private List<Single> rotationlist;


        public Form1()
        {
            InitializeComponent();
            openFileDialog1.Filter = "Mafia parnik.def file(*.def)|*.def";
        }

        string filename;
        List<byte[]> Points = new List<byte[]>();
        List<Single> Coords = new List<Single>();

        Parnikvar PV = new Parnikvar();

        private void openAFileToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (openFileDialog1.ShowDialog() == DialogResult.Cancel) { return; }
            filename = openFileDialog1.FileName;
            Coordinates.Items.Clear();
            Rotation.Items.Clear();
            ParnikCoords = new List<ParnikCoords>();
            Coords.Clear();
            Point.Items.Clear();

            BinaryReader reader = new BinaryReader(File.Open(filename, FileMode.Open));
            PV.FileSgn = reader.ReadUInt32();
            PV.unknown = reader.ReadUInt32();

            if (PV.FileSgn == 4)
            {
                filepath.Visible = true;
                filepath.Text = filename;

                for (int k = 0; k < PV.unknown * 4; k++)
                {
                    nameslist = new List<Single>();
                    rotationlist = new List<Single>();
                    positionX = reader.ReadSingle();
                    nameslist.Add(positionX);
                    Coords.Add(positionX);

                    positionY = reader.ReadSingle();
                    nameslist.Add(positionY);
                    Coords.Add(positionY);

                    positionZ = reader.ReadSingle();
                    nameslist.Add(positionZ);
                    Coords.Add(positionZ);


                    rotationX = reader.ReadSingle();
                    rotationlist.Add(rotationX);
                    Coords.Add(rotationX);

                    rotationY = reader.ReadSingle();
                    rotationlist.Add(rotationY);
                    Coords.Add(rotationY);

                    rotationZ = reader.ReadSingle();
                    rotationlist.Add(rotationZ);
                    Coords.Add(rotationZ);
                    ParnikCoords.Add(new ParnikCoords(positionX, positionY, positionZ, rotationX, rotationY, rotationZ, nameslist, rotationlist));
                }

                reader.Close();
                VisualGroup();
            }

            else
            {
                MessageBox.Show("Couldn't parse a file!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                reader.Close();
            }
            
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                contextMenuStrip1.Show(MousePosition, ToolStripDropDownDirection.Right);
            }
        }

        private void editToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            int a = Point.SelectedIndex;
            int i = Coordinates.SelectedIndex;
            if (ParnikCoords != null && i >= 0 && a >= 0)
            {
                string newname = Interaction.InputBox("Type a new value of coordinate:", "New coord", Coordinates.SelectedItem.ToString());
                if (newname == "")
                    MessageBox.Show("Didn't change name!", "New coord", MessageBoxButtons.OK, MessageBoxIcon.Information);
                else
                {
                    Single newnamesingle = Convert.ToSingle(newname);
                    Interaction.Beep();
                    ParnikCoords[a].objNames[i] = newnamesingle;
                    //VisualGroup();
                    VisualObject(a);
                }
            }
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            int a = Point.SelectedIndex;
            int i = Rotation.SelectedIndex;
            if (ParnikCoords != null && i >= 0 && a >= 0)
            {
                string newname = Interaction.InputBox("Type a new value of coordinate:", "New coord", Rotation.SelectedItem.ToString());
                if (newname == "")
                    MessageBox.Show("Didn't change name!", "New coord", MessageBoxButtons.OK, MessageBoxIcon.Information);
                else
                {
                    Single newnamesingle = Convert.ToSingle(newname);
                    Interaction.Beep();
                    ParnikCoords[a].rotlist[i] = newnamesingle;
                    //VisualGroup();
                    VisualObject(a);
                }
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Powered by Smelson! From Russia with love!", "About", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BinaryWriter writer = new BinaryWriter(File.Open(filename, FileMode.Create));
            writer.Write(PV.FileSgn);
            writer.Write(PV.unknown);


            for (int a = 0; a < ParnikCoords.Count; a++)
            {

                for (int i = 0; i < ParnikCoords[a].objNames.Count; i++)
                {
                    writer.Write(ParnikCoords[a].objNames[i]);
                    i++;
                    writer.Write(ParnikCoords[a].objNames[i]);
                    i++;
                    writer.Write(ParnikCoords[a].objNames[i]);

                    for (int g = 0; g < ParnikCoords[a].rotlist.Count; g++)
                    {
                        writer.Write(ParnikCoords[a].rotlist[g]);
                        g++;
                        writer.Write(ParnikCoords[a].rotlist[g]);
                        g++;
                        writer.Write(ParnikCoords[a].rotlist[g]);
                    }
                }
            }

            writer.Close();
        }

        private void VisualGroup()
        {
            Point.Items.Clear();

            if (ParnikCoords != null)
            for (int a = 0; a < ParnikCoords.Count; a++)
            Point.Items.Add((a + 1).ToString());
        }

        private void VisualObject(int ngr)
        {
            Coordinates.Items.Clear();
            Rotation.Items.Clear();

            if (ParnikCoords[ngr].objNames != null)
                for (int i = 0; i < ParnikCoords[ngr].objNames.Count; i++)
                {
                    Coordinates.Items.Add(ParnikCoords[ngr].objNames[i]);
                    Rotation.Items.Add(ParnikCoords[ngr].rotlist[i]);
                }
        }

        private void Point_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            VisualObject(Point.SelectedIndex);
        }


    }
}
