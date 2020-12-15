using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Windows.Forms;

namespace Unidades
{
    public partial class Unidades : Form
    {
        string URL = "https://localhost:44363/api/Unidades";
        public Unidades()
        {
            InitializeComponent();
            dataUnidades.AutoGenerateColumns = false;
        }

        private async void Unidades_Load(object sender, EventArgs e)
        {
            string respuesta = await GetHttp();
            List<Models.Request.ReqUnidades> lst = JsonConvert.DeserializeObject<List<Models.Request.ReqUnidades>>(respuesta);
            dataUnidades.DataSource = lst;
        }

        private async void bActualizar_Click(object sender, EventArgs e)
        {
            string respuesta = await GetHttp();
            List<Models.Request.ReqUnidades> lst = JsonConvert.DeserializeObject<List<Models.Request.ReqUnidades>>(respuesta);
            dataUnidades.DataSource = lst;
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(txtDescripcion.Text.Trim()))
            {
                MessageBox.Show("Rellena los campos");
                return;
            }

            Models.Request.ReqUnidades obj = new Models.Request.ReqUnidades();
            obj.descripcion = txtDescripcion.Text.Trim();
            obj.estado = Convert.ToBoolean(ckEstado.Checked);
            string resultado = Send<Models.Request.ReqUnidades>(URL, obj, "POST");
            MessageBox.Show(resultado);
        }

        public string Send<T>(string url, T objectRequest, string metodo = "POST")
        {
            string result = "";
            try
            {
                string json = Newtonsoft.Json.JsonConvert.SerializeObject(objectRequest);

                WebRequest request = WebRequest.Create(url);
                request.Method = metodo;
                request.PreAuthenticate = true;
                request.ContentType = "application/json;charset=utf-8'";
                request.Timeout = 10000; 

                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    streamWriter.Write(json);
                    streamWriter.Flush();
                }

                var httpResponse = (HttpWebResponse)request.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    result = streamReader.ReadToEnd();
                }
            }
            catch (Exception e)
            {
                result = e.Message;
            }

            return result;
        }

        public async Task<string> GetHttp()
        {
            WebRequest request = WebRequest.Create(URL);
            WebResponse response = request.GetResponse();

            StreamReader sr = new StreamReader(response.GetResponseStream());
            return await sr.ReadToEndAsync();
        }
    }
}
