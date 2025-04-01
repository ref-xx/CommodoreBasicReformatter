using CommodoreBasicReformatter.Explain;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CommodoreBasicReformatter
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            TransformText(sender, e);
        }


        private void TransformText(object sender, EventArgs e)
        {
           try
            {
                var options = new Configuration
                {
                    InputContent = inputBox.Text,
                    SplitLines = chkSplit.Checked,
                    AddExplanations = chkAddExplanations.Checked
                };

                var reformatter = new Reformatter(new Grammar(), new StmtsSplitter(), new Explainer());

                var result = reformatter.Reformat(options.InputContent, options);
                outputBox.Text = result;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata oluştu: " + ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "BASIC veya metin dosyaları (*.bas;*.txt)|*.bas;*.txt|Tüm dosyalar (*.*)|*.*";
                openFileDialog.Title = "BASIC Dosyası Aç";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    inputBox.Text = File.ReadAllText(openFileDialog.FileName, Encoding.Default);
                    this.Text = "Commodore Basic Reformatter - " + Path.GetFileName(openFileDialog.FileName);

                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "BASIC dosyası (*.bas)|*.bas|Metin dosyası (*.txt)|*.txt|Tüm dosyalar (*.*)|*.*";
                saveFileDialog.Title = "Dönüştürülmüş Dosyayı Kaydet";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    File.WriteAllText(saveFileDialog.FileName, outputBox.Text, Encoding.ASCII);
                }
            }
        }

    }
}
