using DataAccess;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp
{
    public partial class frmUser : Form
    {
        private string _selectedUserID = "";

        public frmUser()
        {
            InitializeComponent();
            btnAdd.Enabled = true;
            btnList.Enabled = true;
            btnUpdate.Enabled = false;
            btnDelete.Enabled = false;
        }

        private async void frmUser_Load(object sender, EventArgs e)
        {
            await GetAllUser();
        }

        private void ControlsClear()
        {
            txtEmail.Text = "";
            txtFirstName.Text = "";
            txtLastName.Text = "";
            txtPassword.Text = "";
            txtUserName.Text = "";
            txtEmail.Text = "";
        }

        //https://localhost:44399/api/users/add
        private async void PostOperations<TEntity>(TEntity entity, string baseUrl)
        {
            Uri baseUri = new Uri(baseUrl);

            ////Json'a serileştirilecek ve gönderilecek Class (TEntity)
            //User user = new User
            //{
            //    FirstName = txtFirstName.Text,
            //    LastName = txtLastName.Text,
            //    Email = txtEmail.Text,
            //    Password = txtPassword.Text,
            //    UserName = txtUserName.Text,
            //    CreatedDate = DateTime.Now.ToString(),
            //};

            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //aClient.DefaultRequestHeaders.Add("X-ZUMO-INSTALLATION-ID", "8bc6aea9-864a-44fc-9b4b-87ec64e123bd");
            //aClient.DefaultRequestHeaders.Add("X-ZUMO-APPLICATION", "OabcWgaGVdIXpqwbMTdBQcxyrOpeXa20");
            httpClient.DefaultRequestHeaders.Host = baseUri.Host;

            //Tip için (Class/User) için bir Json Serializer oluşturun
            DataContractJsonSerializer jsonSer = new DataContractJsonSerializer(typeof(TEntity));

            // Nesneyi MemoryStream'e yazmak için serileştiriciyi kullanma
            MemoryStream ms = new MemoryStream();
            jsonSer.WriteObject(ms, entity);
            ms.Position = 0;

            //StringContent'i (Json) oluşturmak için StreamReader'ı kullanma
            StreamReader sr = new StreamReader(ms);
            // JSON yeterince basitse, serileştirmeyi yapan ve kendiniz oluşturacak olan yukarıdaki 5 satırı görmezden gelebilirsiniz.
            // sonra ilk argüman olarak StringContent yapıcısına iletin
            StringContent theContent = new StringContent(sr.ReadToEnd(), System.Text.Encoding.UTF8, "application/json");

            //Verileri gönderin
            HttpResponseMessage aResponse = await httpClient.PostAsync(baseUri, theContent);
            string result = aResponse.Content.ReadAsStringAsync().Result;

            if (aResponse.IsSuccessStatusCode)
            {
                MessageBox.Show(result);
                await GetAllUser();
                ControlsClear();
            }
            else
            {
                // yanıt durum kodunu göster
                String failureMsg = "HTTP Status Kodu: " + aResponse.StatusCode.ToString() + " - Nedeni: " + aResponse.ReasonPhrase;
                MessageBox.Show(failureMsg + " / \n Gelen Mesaj: " + result);
            }
        }

        private async void btnAdd_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(txtUserName.Text) && !String.IsNullOrEmpty(txtFirstName.Text) && !String.IsNullOrEmpty(txtLastName.Text) && !String.IsNullOrEmpty(txtPassword.Text) && !String.IsNullOrEmpty(txtEmail.Text))
            {
                #region OldCode

                //Uri theUri = new Uri("https://localhost:44399/api/users/add");

                ////Json'a serileştirilecek ve gönderilecek Class
                //User user = new User
                //{
                //    FirstName = txtFirstName.Text,
                //    LastName = txtLastName.Text,
                //    Email = txtEmail.Text,
                //    Password = txtPassword.Text,
                //    UserName = txtUserName.Text,
                //    CreatedDate = DateTime.Now.ToString(),
                //};

                //HttpClient httpClient = new HttpClient();
                //httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                ////aClient.DefaultRequestHeaders.Add("X-ZUMO-INSTALLATION-ID", "8bc6aea9-864a-44fc-9b4b-87ec64e123bd");
                ////aClient.DefaultRequestHeaders.Add("X-ZUMO-APPLICATION", "OabcWgaGVdIXpqwbMTdBQcxyrOpeXa20");
                //httpClient.DefaultRequestHeaders.Host = theUri.Host;

                ////Tip için (Class/User) için bir Json Serializer oluşturun
                //DataContractJsonSerializer jsonSer = new DataContractJsonSerializer(typeof(User));

                //// Nesneyi MemoryStream'e yazmak için serileştiriciyi kullanma
                //MemoryStream ms = new MemoryStream();
                //jsonSer.WriteObject(ms, user);
                //ms.Position = 0;

                ////StringContent'i (Json) oluşturmak için StreamReader'ı kullanma
                //StreamReader sr = new StreamReader(ms);
                //// JSON yeterince basitse, serileştirmeyi yapan ve kendiniz oluşturacak olan yukarıdaki 5 satırı görmezden gelebilirsiniz.
                //// sonra ilk argüman olarak StringContent yapıcısına iletin
                //StringContent theContent = new StringContent(sr.ReadToEnd(), System.Text.Encoding.UTF8, "application/json");

                ////Verileri gönderin
                //HttpResponseMessage aResponse = await httpClient.PostAsync(theUri, theContent);
                //string result = aResponse.Content.ReadAsStringAsync().Result;

                //if (aResponse.IsSuccessStatusCode)
                //{
                //    MessageBox.Show(result);
                //    await GetAllUser();
                //    ControlsClear();
                //}
                //else
                //{
                //    // yanıt durum kodunu göster
                //    String failureMsg = "HTTP Status Kodu: " + aResponse.StatusCode.ToString() + " - Nedeni: " + aResponse.ReasonPhrase;
                //    MessageBox.Show(failureMsg + " / \n Gelen Mesaj: " + result);
                //}

                #endregion OldCode

                //Json'a serileştirilecek ve gönderilecek Class
                User user = new User
                {
                    FirstName = txtFirstName.Text,
                    LastName = txtLastName.Text,
                    Email = txtEmail.Text,
                    Password = txtPassword.Text,
                    UserName = txtUserName.Text,
                    CreatedDate = DateTime.Now.ToString(),
                };
                PostOperations<User>(user, "https://localhost:44399/api/users/add");
            }
            else
            {
                MessageBox.Show("Lütfen Hiçbir Alanı Boş Bırakmayınız !");
            }
        }

        private async void btnUpdate_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(txtUserName.Text) && !String.IsNullOrEmpty(txtFirstName.Text) && !String.IsNullOrEmpty(txtLastName.Text) && !String.IsNullOrEmpty(txtPassword.Text) && !String.IsNullOrEmpty(txtEmail.Text))
            {
                //Json'a serileştirilecek ve gönderilecek Class
                User user = new User
                {
                    Id = Convert.ToInt32(_selectedUserID),
                    FirstName = txtFirstName.Text,
                    LastName = txtLastName.Text,
                    Email = txtEmail.Text,
                    Password = txtPassword.Text,
                    UserName = txtUserName.Text,
                    CreatedDate = DateTime.Now.ToString(),
                };
                PostOperations<User>(user, "https://localhost:44399/api/users/update");

                #region OldCode

                //Uri theUri = new Uri("https://localhost:44399/api/users/update");

                ////Json'a serileştirilecek ve gönderilecek Class
                //User user = new User
                //{
                //    Id = Convert.ToInt32(_selectedUserID),
                //    FirstName = txtFirstName.Text,
                //    LastName = txtLastName.Text,
                //    Email = txtEmail.Text,
                //    Password = txtPassword.Text,
                //    UserName = txtUserName.Text,
                //    CreatedDate = DateTime.Now.ToString(),
                //};

                //HttpClient aClient = new HttpClient();
                //aClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                ////aClient.DefaultRequestHeaders.Add("X-ZUMO-INSTALLATION-ID", "8bc6aea9-864a-44fc-9b4b-87ec64e123bd");
                ////aClient.DefaultRequestHeaders.Add("X-ZUMO-APPLICATION", "OabcWgaGVdIXpqwbMTdBQcxyrOpeXa20");
                //aClient.DefaultRequestHeaders.Host = theUri.Host;

                ////Tip içim (Class/Product) için bir Json Serializer oluşturun
                //DataContractJsonSerializer jsonSer = new DataContractJsonSerializer(typeof(User));

                //// Nesneyi MemoryStream'e yazmak için serileştiriciyi kullanma
                //MemoryStream ms = new MemoryStream();
                //jsonSer.WriteObject(ms, user);
                //ms.Position = 0;

                ////StringContent'i (Json) oluşturmak için StreamReader'ı kullanma
                //StreamReader sr = new StreamReader(ms);
                //// JSON yeterince basitse, serileştirmeyi yapan ve kendiniz oluşturacak olan yukarıdaki 5 satırı görmezden gelebilirsiniz.
                //// sonra ilk argüman olarak StringContent yapıcısına iletin
                //StringContent theContent = new StringContent(sr.ReadToEnd(), System.Text.Encoding.UTF8, "application/json");

                ////Verileri gönderin
                //HttpResponseMessage aResponse = await aClient.PostAsync(theUri, theContent);
                //string result = aResponse.Content.ReadAsStringAsync().Result;

                //if (aResponse.IsSuccessStatusCode)
                //{
                //    MessageBox.Show(result);
                //    await GetAllUser();
                //    ControlsClear();
                //    btnAdd.Enabled = true;
                //    btnList.Enabled = true;
                //    btnUpdate.Enabled = false;
                //    btnDelete.Enabled = false;
                //}
                //else
                //{
                //    // yanıt durum kodunu göster
                //    String failureMsg = "HTTP Status Kodu: " + aResponse.StatusCode.ToString() + " - Nedeni: " + aResponse.ReasonPhrase;
                //    MessageBox.Show(failureMsg + " / \n Gelen Mesaj: " + result);
                //}

                #endregion OldCode
            }
            else
            {
                MessageBox.Show("Lütfen Hiçbir Alanı Boş Bırakmayınız !");
            }
        }

        private async void btnDelete_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(txtUserName.Text) && !String.IsNullOrEmpty(txtFirstName.Text) && !String.IsNullOrEmpty(txtLastName.Text) && !String.IsNullOrEmpty(txtPassword.Text) && !String.IsNullOrEmpty(txtEmail.Text))
            {
                //Json'a serileştirilecek ve gönderilecek Class
                User user = new User
                {
                    Id = Convert.ToInt32(_selectedUserID),
                    FirstName = txtFirstName.Text,
                    LastName = txtLastName.Text,
                    Email = txtEmail.Text,
                    Password = txtPassword.Text,
                    UserName = txtUserName.Text,
                    CreatedDate = DateTime.Now.ToString(),
                };
                PostOperations<User>(user, "https://localhost:44399/api/users/delete");

                #region OldCode

                //Uri theUri = new Uri("https://localhost:44399/api/users/delete");

                ////Json'a serileştirilecek ve gönderilecek Class
                //User user = new User
                //{
                //    Id = Convert.ToInt32(_selectedUserID),
                //    FirstName = txtFirstName.Text,
                //    LastName = txtLastName.Text,
                //    Email = txtEmail.Text,
                //    Password = txtPassword.Text,
                //    UserName = txtUserName.Text,
                //    CreatedDate = DateTime.Now.ToString(),
                //};

                //HttpClient aClient = new HttpClient();
                //aClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                ////aClient.DefaultRequestHeaders.Add("X-ZUMO-INSTALLATION-ID", "8bc6aea9-864a-44fc-9b4b-87ec64e123bd");
                ////aClient.DefaultRequestHeaders.Add("X-ZUMO-APPLICATION", "OabcWgaGVdIXpqwbMTdBQcxyrOpeXa20");
                //aClient.DefaultRequestHeaders.Host = theUri.Host;

                ////Tip içim (Class/Product) için bir Json Serializer oluşturun
                //DataContractJsonSerializer jsonSer = new DataContractJsonSerializer(typeof(User));

                //// Nesneyi MemoryStream'e yazmak için serileştiriciyi kullanma
                //MemoryStream ms = new MemoryStream();
                //jsonSer.WriteObject(ms, user);
                //ms.Position = 0;

                ////StringContent'i (Json) oluşturmak için StreamReader'ı kullanma
                //StreamReader sr = new StreamReader(ms);
                //// JSON yeterince basitse, serileştirmeyi yapan ve kendiniz oluşturacak olan yukarıdaki 5 satırı görmezden gelebilirsiniz.
                //// sonra ilk argüman olarak StringContent yapıcısına iletin
                //StringContent theContent = new StringContent(sr.ReadToEnd(), System.Text.Encoding.UTF8, "application/json");

                ////Verileri gönderin
                //HttpResponseMessage aResponse = await aClient.PostAsync(theUri, theContent);
                //string result = aResponse.Content.ReadAsStringAsync().Result;

                //if (aResponse.IsSuccessStatusCode)
                //{
                //    MessageBox.Show(result);
                //    await GetAllUser();
                //    ControlsClear();
                //    btnAdd.Enabled = true;
                //    btnList.Enabled = true;
                //    btnUpdate.Enabled = false;
                //    btnDelete.Enabled = false;
                //}
                //else
                //{
                //    // yanıt durum kodunu göster
                //    String failureMsg = "HTTP Status Kodu: " + aResponse.StatusCode.ToString() + " - Nedeni: " + aResponse.ReasonPhrase;
                //    MessageBox.Show(failureMsg + " / \n Gelen Mesaj: " + result);
                //}

                #endregion OldCode
            }
            else
            {
                MessageBox.Show("Lütfen Hiçbir Alanı Boş Bırakmayınız !");
            }
        }

        private async void btnList_Click(object sender, EventArgs e)
        {
            await GetAllUser();
        }

        private async Task GetAllUser()
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44399/");
            HttpResponseMessage response = await client.GetAsync("api/users/getall");
            string result = await response.Content.ReadAsStringAsync();
            var model = JsonConvert.DeserializeObject<List<User>>(result);
            dataGridView1.DataSource = model.ToList();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
                {
                    dataGridView1.CurrentRow.Selected = true;
                    _selectedUserID = dataGridView1.Rows[e.RowIndex].Cells["Id"].FormattedValue.ToString();
                    txtEmail.Text = dataGridView1.Rows[e.RowIndex].Cells["Email"].FormattedValue.ToString();
                    txtFirstName.Text = dataGridView1.Rows[e.RowIndex].Cells["FirstName"].FormattedValue.ToString();
                    txtLastName.Text = dataGridView1.Rows[e.RowIndex].Cells["LastName"].FormattedValue.ToString();
                    txtPassword.Text = dataGridView1.Rows[e.RowIndex].Cells["Password"].FormattedValue.ToString();
                    txtUserName.Text = dataGridView1.Rows[e.RowIndex].Cells["UserName"].FormattedValue.ToString();
                    btnAdd.Enabled = false;
                    btnList.Enabled = false;
                    btnUpdate.Enabled = true;
                    btnDelete.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata Oluştu :" + ex.ToString());
            }
        }
    }
}