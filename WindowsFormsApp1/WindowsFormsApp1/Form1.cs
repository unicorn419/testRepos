using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int[] ar = new int[9] { 1,2,3,4,9,5,8,7,6};
            sort(ref ar, 0, 8);
            int i=9;
        }

        private void sort(ref int[] ar,int start, int end)
        {
            if (start < end)
            {
                int i = partition(ref ar, start, end);
                sort(ref ar, start, i - 1);
                sort(ref ar, i + 1, end);
            }
        }
        private int partition(ref int[] ar, int start, int end)
        {
            int tmp = ar[end];
            int i = start - 1;
            for (int j = start; j <= end-1; j++)
            {
                if (ar[j] < tmp)
                {
                    i++;
                    swap(ref ar, i, j);
                    
                }
            }
            swap(ref ar, i + 1, end);

            return i + 1;
        }
        private void swap(ref int[] ar, int i, int j)
        {
            if (i == j) return;

            this.richTextBox1.Text += "exchange index" + i.ToString() + " " + j.ToString() + System.Environment.NewLine;
            this.richTextBox1.Text += "exchange value" + ar[i].ToString() + " " + ar[j].ToString() + System.Environment.NewLine;
            int t = ar[i];
            ar[i] = ar[j];
            ar[j] = t;
        }
    }
}
