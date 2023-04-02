using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace THA_W5_BRYAN_C
{
    public partial class Form1 : Form
    {
        DataTable dtFilter = new DataTable();
        DataTable dtProdukTampil = new DataTable();
        DataTable dtCategory = new DataTable();
        public string categoryNow = "";
        public string idNow = "";
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            CBFilter.Enabled = false;
            dtProdukTampil.Columns.Add("ID Product");
            dtProdukTampil.Columns.Add("Nama Product");
            dtProdukTampil.Columns.Add("Harga");
            dtProdukTampil.Columns.Add("Stock");
            dtProdukTampil.Columns.Add("ID Category");
            dtProdukTampil.Rows.Add("J001", "Jas Hitam", 100000, 10, "C1");
            dtProdukTampil.Rows.Add("T001", "T-Shirt Black Pink", 70000, 20, "C2");
            dtProdukTampil.Rows.Add("T002", "T-Shirt Obsessive", 75000, 16, "C2");
            dtProdukTampil.Rows.Add("R001", "Rok mini", 82000, 25, "C3");
            dtProdukTampil.Rows.Add("J002", "Jeans Biru", 90000, 5, "C3");
            dtProdukTampil.Rows.Add("C001", "Celana Pendek Coklat", 60000, 11, "C4");
            dtProdukTampil.Rows.Add("C002", "Cawat Blink-blink", 100000, 1, "C5");
            dtProdukTampil.Rows.Add("R002", "Rocca Shirt", 50000, 8, "C2");

            dtCategory.Columns.Add("ID Category", typeof(string));
            dtCategory.Columns.Add("Nama Category", typeof(string));

            dtCategory.Rows.Add("C1", "Jas");
            dtCategory.Rows.Add("C2", "T-Shirt");
            dtCategory.Rows.Add("C3", "Rok");
            dtCategory.Rows.Add("C4", "Celana");
            dtCategory.Rows.Add("C5", "Cawat");

            CBFilter.Text = "";

            dtFilter = dtProdukTampil.Copy();

            dataGridViewCategory.DataSource = dtCategory;
            dataGridViewTampil.DataSource = dtFilter;

            CBCategory.DataSource = dtCategory.Copy();
            CBCategory.DisplayMember = "Nama Category";
            CBCategory.ValueMember = "ID Category";

            CBFilter.DataSource = dtCategory.Copy();
            CBFilter.DisplayMember = "Nama Category";
            CBFilter.ValueMember = "ID Category";
            dataGridViewCategory.CurrentCell = null;
            dataGridViewTampil.ClearSelection();
            dataGridViewTampil.CurrentCell = null;
            CBCategory.Text = "";
            CBFilter.Text = "";

        }



        private void btnAddProduct_Click(object sender, EventArgs e)
        {
            if (TBNameDetails.Text == "" || TBHarga.Text == "" || TBStock.Text == "" || CBCategory.SelectedValue == null)
            {
                MessageBox.Show("Fill the Box");
                return;
            }
            string firstchar = TBNameDetails.Text.Substring(0, 1).ToUpper();
            var ProductIDNew = dtProdukTampil.AsEnumerable().Select(roww => roww.Field<string>("ID Product"));
            int ID = 1;

            foreach (DataRow roww in dtProdukTampil.Rows)
            {
                if (roww[0].ToString()[0].ToString() == firstchar)
                {
                    ID++;
                }

            }

            string MenampilkanID = firstchar + ID;

            for (int i = ID.ToString().Length; i < 3; i++)
            {
                firstchar = string.Concat(firstchar, "0");
            }
            firstchar = string.Concat(firstchar, ID.ToString());
            DataRow row = dtProdukTampil.NewRow();
            row[0] = firstchar;
            row[1] = TBNameDetails.Text;
            row[2] = TBHarga.Text;
            row[3] = TBStock.Text;
            row[4] = CBCategory.SelectedValue;
            dtProdukTampil.Rows.Add(row);
            Filter("");
        }
        private void btnEditProduct_Click(object sender, EventArgs e)
        {
            string NameNew = TBNameDetails.Text;
            string CategoryNew = CBCategory.SelectedValue.ToString();
            int PriceNew = Convert.ToInt32(TBHarga.Text);
            int StockNew = Convert.ToInt32(TBStock.Text);
            if (idNow == "")
            {
                MessageBox.Show("Choose the object first");
            }
            else
            {
                foreach (DataRow row in dtProdukTampil.Rows)
                {
                    if (row[0].ToString() != idNow)
                    {
                        continue;
                    }
                    row[1] = NameNew;
                    row[2] = PriceNew;
                    row[3] = StockNew;
                    row[4] = CategoryNew;
                    if (row[3].ToString() != "0")
                    {
                        continue;
                    }
                    dtProdukTampil.Rows.Remove(row);
                    Filter("");
                    return;
                }
            }
            Filter("");
        }
        private void btnRemoveProduct_Click(object sender, EventArgs e)
        {

            foreach (DataRow row in dtProdukTampil.Rows)
            {
                if (row[0].ToString() != idNow)
                {
                    continue;
                }
                dtProdukTampil.Rows.Remove(row);
                Filter("");
                return;
            }
            Filter("");
        }

        private void dataGridViewTampil_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow row = dataGridViewTampil.Rows[e.RowIndex];
            idNow = row.Cells[0].Value.ToString();
            TBNameDetails.Text = row.Cells[1].Value.ToString();
            TBHarga.Text = row.Cells[2].Value.ToString();
            TBStock.Text = row.Cells[3].Value.ToString();
            CBCategory.SelectedValue = row.Cells[4].Value.ToString();
        }

       

        private void btnAddCategory_Click(object sender, EventArgs e)
        {
            string CategoryNew2 = TBCategoryName.Text;
            var ProductIDNew = dtCategory.AsEnumerable().Select(row => row.Field<string>("ID Category"));
            bool Check = false;
            foreach (DataRow row in dtCategory.Rows)
            {
                if (TBCategoryName.Text != row[1].ToString())
                {
                    continue;
                }
                Check = true;
            }
            if (Check == true)
            {
                MessageBox.Show("Category is filled");
                return;
            }
            string firstchar = "C";
            int ID = 1;

            if (CategoryNew2 == "")
            {
                MessageBox.Show("Fill the Box first");
                return;
            }

            while (ProductIDNew.Contains(firstchar + ID))
            {
                ID++;
            }

            string IDCategory = firstchar + ID;

            DataRow roww = dtCategory.NewRow();
            roww[0] = IDCategory;
            roww[1] = CategoryNew2;

            dtCategory.Rows.Add(roww);

            Filter("");
        }

        private void btnRemoveCategory_Click(object sender, EventArgs e)
        {
            int i;
            foreach (DataRow row in dtCategory.Rows)
            {
                if (row[0].ToString() != categoryNow)
                {
                    continue;
                }
                dtCategory.Rows.Remove(row);
                for (i = dtProdukTampil.Rows.Count - 1; i >= 0; i--)
                {
                    if (dtProdukTampil.Rows[i]["ID Category"] == categoryNow)
                    {
                        dtProdukTampil.Rows.Remove(dtProdukTampil.Rows[i]);
                    }
                }
                Filter("");
                return;
            }
            for (i = dtProdukTampil.Rows.Count - 1; i >= 0; i--)
            {
                if (dtProdukTampil.Rows[i]["ID Category"] == categoryNow)
                {
                    dtProdukTampil.Rows.Remove(dtProdukTampil.Rows[i]);
                }
            }
            Filter("");
        }

        bool select = false;
        private void btnFilter_Click(object sender, EventArgs e)
        {
            CBFilter.Enabled = true;
            select = true;
        }

        private void CBFilter_SelectionChangeCommitted(object sender, EventArgs e)
        {
            Filter(CBFilter.SelectedValue.ToString());
            if (select == true)
            {
                CBFilter.Text = CBFilter.SelectedValue.ToString();
            }
        }

        private void btnShowAll_Click(object sender, EventArgs e)
        {
            Filter("");
            select = false;
            CBFilter.Enabled = false;
        }

        private void dataGridViewCategory_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            categoryNow = dataGridViewCategory.Rows[e.RowIndex].Cells[0].Value.ToString();
            TBCategoryName.Text = dataGridViewCategory.Rows[e.RowIndex].Cells[1].Value.ToString();
        }

        
        public void Filter(string StringVal)
        {
            if (StringVal == "")
            {
                dtFilter.Rows.Clear();
                for (int i = 0; i < dtProdukTampil.Rows.Count; i++)
                {
                    DataRow row = dtFilter.NewRow();
                    row[0] = dtProdukTampil.Rows[i][0];
                    row[1] = dtProdukTampil.Rows[i][1];
                    row[2] = dtProdukTampil.Rows[i][2];
                    row[3] = dtProdukTampil.Rows[i][3];
                    row[4] = dtProdukTampil.Rows[i][4];
                    dtFilter.Rows.Add(row);
                }
            }
            else
            {
                dtFilter.Rows.Clear();
                for (int j = 0; j < dtProdukTampil.Rows.Count; j++)
                {
                    if (dtProdukTampil.Rows[j][4].ToString() == StringVal)
                    {
                        DataRow row = dtFilter.NewRow();
                        row[0] = dtProdukTampil.Rows[j][0];
                        row[1] = dtProdukTampil.Rows[j][1];
                        row[2] = dtProdukTampil.Rows[j][2];
                        row[3] = dtProdukTampil.Rows[j][3];
                        row[4] = dtProdukTampil.Rows[j][4];
                        dtFilter.Rows.Add(row);
                    }
                }
            }

            dataGridViewTampil.CurrentCell = null;
            dataGridViewCategory.CurrentCell = null;
            TBNameDetails.Clear();
            TBHarga.Clear();
            TBStock.Clear();
            idNow = "";
            categoryNow = "";
            CBCategory.DataSource = dtCategory.Copy();
            CBFilter.DataSource = dtCategory.Copy();
            CBCategory.Text = "";
            CBFilter.Text = "";
            TBCategoryName.Text = "";
        }
    }
}

