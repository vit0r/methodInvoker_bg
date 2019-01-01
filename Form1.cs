using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace BackugroundWork
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            dataGridView1.AutoGenerateColumns = true;
            button1.Text = "GO!";
            label1.Text = string.Empty;
            this.CenterToScreen();
            this.Text = "AppTestBeginInvoker";
        }

        private void createDrawString(string percent)
        {
            progressBar1.Update();

            StringFormat stringFormat = new StringFormat()
            {
                FormatFlags = StringFormatFlags.NoFontFallback,
                Trimming = StringTrimming.None,
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };

            var c = progressBar1.CreateGraphics();
            c.DrawString(percent, SystemFonts.DefaultFont, Brushes.Black, progressBar1.ClientRectangle, stringFormat);
            c.Flush();
        }

        private void UpdateComponents()
        {
            IAsyncResult asyncResult = null;
            try
            {
                progressBar1.Step = 1;
                progressBar1.Maximum = 2000;
                var bs = new BindingSource();
                for (int i = 1; i <= progressBar1.Maximum; i++)
                {
                    bs.List.Add(new { Linha = i, Data = DateTime.Now,Par=(i%2==0?"YEAH":"NOPE")});
                    var percent = string.Format("{0}%", Math.Round(Convert.ToDecimal(bs.List.Count * 100 / progressBar1.Maximum)));

                    asyncResult = BeginInvoke(new MethodInvoker(() =>
                    {
                        dataGridView1.DataSource = bs.List;
                        label1.Text = string.Format("{0} linha(s) adicionada(s).", bs.List.Count);
                        progressBar1.PerformStep();
                        createDrawString(percent);
                        Application.DoEvents();
                    }));

                    EndInvoke(asyncResult);
                }
            }
            catch (Exception ex)
            {
                EndInvoke(asyncResult);
                //MessageBox.Show(this, string.Format("Exception in UpdateComponents\n{0}", ex.InnerException), "AppTestBeginInvoker", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }/*
            finally {
                EndInvoke(asyncResult);
            }*/
        }

        private void button1_Click(object sender, EventArgs e)
        {
            UpdateComponents();
        }

        private void dataGridView1_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            dataGridView1.FirstDisplayedScrollingRowIndex = e.RowIndex;
        }

        private void button1_MouseLeave(object sender, EventArgs e)
        {
            //this.Cursor = Cursors.Default;
        }

        private void button1_MouseEnter(object sender, EventArgs e)
        {
            button1.Cursor = Cursors.Hand;
        }
    }
}
