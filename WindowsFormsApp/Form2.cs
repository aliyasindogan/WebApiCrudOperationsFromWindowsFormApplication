using DataAccess;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp
{
    public partial class Form2 : Form
    {
        private HttpsAsync<User> httpsAsync = new HttpsAsync<User>();
        private string baseURL = "https://localhost:44399/api/users/add2";
        private string baseGetURL = "https://localhost:44399/api/users/getall";

        public Form2()
        {
            InitializeComponent();
        }

        private void btnList_Click(object sender, EventArgs e)
        {
            User user = new User
            {
                FirstName = "Ali " + DateTime.Now.ToString(),
                LastName = "Doğan",
                Email = "a@gmail.com",
                Password = "12345",
                UserName = "username",
                CreatedDate = DateTime.Now.ToString(),
            };

            //PostAdd
            Task.Run(() =>
           {
               string text = httpsAsync.PostAdd(user, baseURL).Result;
               var model = JsonConvert.DeserializeObject<User>(text);
               Debug.WriteLine("return Task<string> : " + model.Id.ToString() + " " + model.FirstName + " " + model.LastName);
           });

            //httpsAsync.PostAddModelReturn
            Task.Run(() =>
            {
                User user1 = httpsAsync.PostAddModelReturn(user, baseURL).Result;
                Debug.WriteLine("return Task<TEntity>: " + user1.Id.ToString() + " " + user1.FirstName + " " + user1.LastName + " " + user1.Password);

                // httpsAsync.GetAll
                List<User> users = httpsAsync.GetAll(baseGetURL).Result;
                var usersOrderByDesc = users.OrderByDescending(x => x.Id).ToList();

                //Set dataGridView1 DataSource
                dataGridView1.Invoke(new Action(() => { dataGridView1.DataSource = usersOrderByDesc; }));

                Debug.WriteLine("------------------------------------------------------------------");
                foreach (var item in users)
                {
                    Debug.WriteLine("return Task<List<TEntity>>: " + item.Id.ToString() + " " + item.FirstName + " " + item.LastName + " " + item.Password);
                    Debug.WriteLine("------------------------------------------------------------------");
                }
            });
        }
    }
}