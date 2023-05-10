using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;


namespace DataBase
{
    public partial class Form1 : Form
    {
        private SqlConnection sqlConnection;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["piesleg"].ConnectionString); 
            sqlConnection.Open(); 

            if (sqlConnection.State == ConnectionState.Open) 
            {
                SqlDataAdapter dataAdapter1 = new SqlDataAdapter("select * from Orders", sqlConnection); 
                DataSet ds = new DataSet(); 
                dataAdapter1.Fill(ds); 
                dataGridView2.DataSource = ds.Tables[0];

                //MessageBox.Show("Pieslēgums atvērts"); 
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            

            try
            {
                SqlCommand cmd = new SqlCommand(
                $"insert into Auto (Nosaukums, IzlaidumaDatums, Nobraukums, Jauda, Cena) " +
                $"values(@Nos, @Dat, @Norauk, @Jauda, @Cena)",
                sqlConnection);

                cmd.Parameters.AddWithValue("Nos", textBox1.Text);
                cmd.Parameters.AddWithValue("Dat", textBox2.Text);
                cmd.Parameters.AddWithValue("Norauk", textBox3.Text);
                cmd.Parameters.AddWithValue("Jauda", textBox4.Text);
                cmd.Parameters.AddWithValue("Cena", Double.Parse(textBox5.Text));

                textBox1.Text = "";
                textBox2.Text = "";
                textBox3.Text = "";
                textBox4.Text = "";
                textBox5.Text = "";
                cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {
                MessageBox.Show("Kaut kas nav labi");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //datu adapteris info atlases komandai
            SqlDataAdapter adapter = new SqlDataAdapter("select * from orders",sqlConnection);             
            //datu kopas objekts datu reprezentācijai aplikācijā
            DataSet datuKopa = new DataSet();             
            adapter.Fill(datuKopa);
            //datu avots datu izvadei uz formas
            dataGridView1.DataSource = datuKopa.Tables[0];
        }

        private void button3_Click(object sender, EventArgs e)
        {
            SqlDataReader dataReader = null; 
            try
            {                 
                //sākumā iztīram listView no iepriekšējiem datiem
                listView1.Items.Clear();                 
                //izveido komandu
                SqlCommand komanda = new SqlCommand("select ProductName, QuantityPerUnit, UnitPrice from Products",sqlConnection);                 
                dataReader = komanda.ExecuteReader();                                  
                ListViewItem item = null;        
                //pārlasam tabulas datus ar datareader kursoru un pievienojam datus no tabulas uz listView
                while (dataReader.Read())                 {                     
                    item = new ListViewItem(new string[]                      
                    {
                        Convert.ToString(dataReader["ProductName"]),                    
                        Convert.ToString(dataReader["QuantityPerUnit"]),                     
                        Convert.ToString(dataReader["UnitPrice"])
                    });                     
                    listView1.Items.Add(item);                 
                }             
            } catch (Exception ex)             {                 
                MessageBox.Show(ex.Message);             
            }  finally {                 
                //ja dataReader ir izveidots un vēl nav aizvērts
                if (dataReader != null && !dataReader.IsClosed)                     
                    //aizvērt dataReader
                    dataReader.Close();             
            } 
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            (dataGridView2.DataSource as DataTable).DefaultView.RowFilter = 
                $"ShipCity LIKE '%{textBox6.Text}%'";
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBox1.SelectedIndex)
            {
                case 0:
                    (dataGridView2.DataSource as DataTable).DefaultView.RowFilter = "";
                    break;
                case 1: 
                    (dataGridView2.DataSource as DataTable).DefaultView.RowFilter = $"Freight <=10"; 
                    break;
                case 2: 
                    (dataGridView2.DataSource as DataTable).DefaultView.RowFilter = $"Freight >=10 AND Freight <=50"; 
                    break;
                case 3: 
                    (dataGridView2.DataSource as DataTable).DefaultView.RowFilter = $"Freight >=50"; 
                    break;

                
            }
        }
    }
}
