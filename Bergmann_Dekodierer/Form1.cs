using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace Bergmann_Dekodierer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        string[] dateiinhalt;
        string Output;

        private void Form1_Load(object sender, EventArgs e)
        {
            string[] Dateiihnahlt = File.ReadAllLines(@"Vokabelliste\Vokabelliste.txt");
            dateiinhalt = Dateiihnahlt;
            Scalingsize();
            toolStripComboBox1.SelectedIndex = 0;
        }

        private void Scalingsize()
        {
            richTextBox1.Width = Convert.ToInt16(Math.Round(Convert.ToDouble(this.Size.Width) / 2 - 20, 0));
            richTextBox2.Width = Convert.ToInt16(Math.Round(Convert.ToDouble(this.Size.Width) / 2, 0));
            richTextBox3.Width = richTextBox1.Width;
            richTextBox1.Height = richTextBox2.Height - toolStrip1.Height - 100;
            richTextBox3.Height = 100;
            richTextBox3.Location = new Point(0, richTextBox1.Height);
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            string text = richTextBox1.Text; //text vorbereiten
            text = text.Replace(@"\", @" \ ");
            text = text.Replace(@"-", @" - ");
            text = text.Replace(@".", @" . ");
            text = text.Replace(@",", @" , ");
            text = text.Replace(@";", @" ; ");
            text = text.Replace(@":", @" : ");
            text = text.Replace(@"(", @" ( ");
            text = text.Replace(@")", @" ) ");
            text = text.Replace(@"!", @" ! ");
            text = text.Replace(@"]", @" ] ");
            text = text.Replace(@"[", @" [ ");
            text = text.Replace(@"/", @" / ");
            text = text.Replace(@"&", @" & ");
            text = text.Replace(@"+", @" + ");
            text = text.Replace(@"@", @" @ ");
            text = text.Replace(@"<", @" < ");
            text = text.Replace(@">", @" > ");
            text = text.ToLower();
            text = text.Replace(@" i ", @" I ");
            text = text.Replace(@"  ", @" ");
            richTextBox1.Text = text;

            switch (toolStripComboBox1.SelectedIndex)
            {
                case 0:
                    FullResultMode();
                    break;
                case 1:
                    MissingWordMode();
                    break;
            }
        }

        private void FullResultMode()
        {
            string[] englischeWörter = richTextBox1.Text.Split(" ".ToCharArray());
            string[] deutscheWörter = new string[englischeWörter.Length];

            for (int j = 0; j < englischeWörter.GetLength(0); j++)
            {
                for (int i = 0; i < dateiinhalt.GetLength(0); i++)
                {
                    if ((englischeWörter[j].Equals(dateiinhalt[i]) || englischeWörter[j].Equals(dateiinhalt[i] + ","))|| (englischeWörter[j].Equals(dateiinhalt[i] + ".") || englischeWörter[j].Equals(dateiinhalt[i] + ":")))
                    {
                        deutscheWörter[j] = (dateiinhalt[i + 1] + " ");
                        break;
                    }
                    else
                    {
                        deutscheWörter[j] = " ";
                    }
                }
                if (deutscheWörter[j] == " ")
                {
                    deutscheWörter[j] = englischeWörter[j];
                }
            }


            Form2 newForm2 = new Form2();

            int locx = 1, locy = 1;
            double Weite, Multiplikator = 3, Power = 100;
            for (int a = 0; a < englischeWörter.Length; a++)
            {
                Weite = 0;
                try //Die Weite auswählen je nachdem wie lang der String ist wird die Länge des Labels gewählt
                {
                    if (englischeWörter[a].Length >= deutscheWörter[a].Length)
                    {
                        Weite = Multiplikator * Math.Sqrt(englischeWörter[a].Length* Power) + 10;
                    }
                    else
                    {
                        Weite = Multiplikator * Math.Sqrt(deutscheWörter[a].Length * Power) + 10;
                    }
                }
                catch
                {
                    Weite = 0;
                    Weite = Multiplikator * Math.Sqrt(englischeWörter[a].Length * Power) + 10;
                }
                if ((englischeWörter[a] == ","|| englischeWörter[a] == "/") ||(englischeWörter[a] == "."|| englischeWörter[a] == "-"))
                {
                    Weite = 20;
                }


                Label labl = addlabel(a, englischeWörter[a], locx, locy, true, Convert.ToInt16(Math.Round(Weite, 0)));
                Label labl2 = addlabel(a, deutscheWörter[a], locx, locy + 20, false, Convert.ToInt16 (Math.Round(Weite,0)));
                newForm2.Controls.Add(labl);
                newForm2.Controls.Add(labl2);
                if (locx >= newForm2.Right - 200)
                {
                    locx = 5;
                    locy += 50;
                }
                else
                {
                    locx += Convert.ToInt16(Math.Round(Weite,0));
                }
            }
            newForm2.Height = locy + 80;
            newForm2.ShowDialog();
        }



        Label addlabel(int i, string text,int startx, int endx, bool fett, int Waite)
        {
            Label labl = new Label();
            labl.Name = "Label" + i.ToString();
            labl.Text = text;
            labl.BackColor = Color.WhiteSmoke;
            if (fett == true)
            {
                labl.Font = new Font("Verdana", 11, FontStyle.Bold);
                labl.TextAlign = ContentAlignment.BottomLeft;
            }
            else
            {
                labl.Font = new Font("Verdana", 11, FontStyle.Regular);
                labl.TextAlign = ContentAlignment.TopLeft;
            }
            labl.Click += new EventHandler(this.Labl_Click); //Click Ereigniss
            labl.DoubleClick += new EventHandler(this.Labl_DoubleClick); //Doppeclick Ereigniss
            labl.Height = 20;
            labl.Width = Waite;
            labl.BringToFront();
            labl.Visible = true;
            labl.Location = new Point(startx, endx);
            return labl;
            
        }

        private void Labl_DoubleClick(object sender, EventArgs e)
        {
        }

        protected void Labl_Click(object sender, EventArgs e)  //Click Ereigniss eines Labels, hier wird die Farbe geändert
        {
            Label clickedLabel = (Label)sender;
            if (clickedLabel.BackColor == colorDialog1.Color)
            {
                clickedLabel.BackColor = Color.WhiteSmoke;
            }
            else
            {
                clickedLabel.BackColor = colorDialog1.Color;
            }
        }

        private void MissingWordMode()
        {
            richTextBox2.Clear();
            richTextBox3.Clear();
            Output = "";
            string[] wörter = richTextBox1.Text.Split(" ".ToCharArray());
            string nichtVorhanden = "";
            bool nichtvorhandenabfrage = true;

            for (int j = 0; j < wörter.GetLength(0); j++)
            {
                nichtvorhandenabfrage = true;
                for (int i = 0; i < dateiinhalt.GetLength(0); i++)
                {
                    if ((wörter[j].Equals(dateiinhalt[i]) || wörter[j].Equals(dateiinhalt[i] + ",")) || (wörter[j].Equals(dateiinhalt[i] + ".") || wörter[j].Equals(dateiinhalt[i] + ":")))
                    {
                        Output += dateiinhalt[i + 1] + " ";
                        nichtvorhandenabfrage = false;
                        break;
                    }

                }
                if (nichtvorhandenabfrage == true)
                {
                    nichtVorhanden += wörter[j] + " ";
                }

            }

            richTextBox2.Text = Output;
            richTextBox3.Text = nichtVorhanden;
        }

        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            Scalingsize();
        }

        private void richTextBox1_Click(object sender, EventArgs e)
        {
            if (richTextBox1.Text == "Text")
            {
                richTextBox1.Clear();
            }
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            colorDialog1.ShowDialog();
        }
    }
}