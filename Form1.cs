namespace WindowsFormsApplicationPruebaPDF
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Diagnostics;
    using System.Drawing.Text;
    using System.IO;
    using System.Linq;
    using System.Windows.Forms;
    using PdfSharp.Drawing;

    public partial class Form1 : Form
    {
        //   XBrush BrushBlack = XBrushes.Black;
        //  XFont fontArial5 = new XFont("Planet Benson 2", 5, XFontStyle.Bold, new XPdfFontOptions(PdfFontEmbedding.Always));

        private static string GetFontNameFromFile(string filename)
        {
            PrivateFontCollection fontCollection = new PrivateFontCollection();
            fontCollection.AddFontFile("c:\\fonts\\ufonts.com_gotham-bold.ttf");
            return fontCollection.Families[0].Name;
        }

        public Form1()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            this.toolStripStatusLabel1.Text = string.Empty;
            TelnetPDF.ClassParametrosTelnetPDF cp = new TelnetPDF.ClassParametrosTelnetPDF();
            cp.FileRoute = "c:\\temp\\";         //"D:\\TelnetPDF\\WebApplicationTelnetPDF\\WebApplicationTelnetPDF\\files\\";
            cp.Imageroute = ConfigurationSettings.AppSettings["ImageRoute"];
            cp.Urlpath =ConfigurationSettings.AppSettings["Urlpath"].ToString(); // "/files/";
            cp.Urlmensaje = ConfigurationSettings.AppSettings["Urlmensaje"].ToString(); // "Descargar Archivo Aqui";
            cp.CantidadCamposxRegistro = ConfigurationSettings.AppSettings["CantidadCamposxRegistro"].ToString();
            cp.CnnStringSqlServerSinPassword = ConfigurationSettings.AppSettings["CnnStringSqlServerSinPassword"].ToString();
            cp.CuentaUniverse = ConfigurationSettings.AppSettings["CuentaUniverse"].ToString();    
            cp.PasswordSqlServerEncriptado = ConfigurationSettings.AppSettings["PasswordSqlServerEncriptado"].ToString();
            cp.PasswordUniverseEncriptado = ConfigurationSettings.AppSettings["PasswordUniverseEncriptado"].ToString();
            cp.ServerUniverse = ConfigurationSettings.AppSettings["ServerUniverse"].ToString();
            cp.UsuarioUniverse = ConfigurationSettings.AppSettings["UsuarioUniverse"].ToString();
            cp.LogFile = ConfigurationSettings.AppSettings["PathLog"].ToString();
            cp.FontRoute = ConfigurationSettings.AppSettings["FontRoute"].ToString();

            TelnetPDF.comprobantesPDF ts = new TelnetPDF.comprobantesPDF();
            string res=ts.GenerarFactura(this.sNumeroComprobante.Text.Trim(), cp, true, true);
            toolStripStatusLabel1.Text = "Generacion Finalizada:" + res;
           // MessageBox.Show("Generacion Finalizada:" + res, "OK", MessageBoxButtons.OK);

            Cursor.Current = Cursors.Default;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Process.Start(@"c:\temp\");
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private bool ClearFolder(string folderName)
        {

            bool retvalue = true;
            try
            {
                DirectoryInfo dir = new DirectoryInfo(folderName);

                foreach (FileInfo fi in dir.GetFiles())
                {
                    fi.Delete();
                }

                foreach (DirectoryInfo di in dir.GetDirectories())
                {
                    this.ClearFolder(di.FullName);
                    di.Delete();
                }
            }
            catch 
            {
                retvalue = false;
                this.toolStripStatusLabel1.Text = "Hay Pdfs Abiertos";
            }
            return retvalue;
        }

        private void buttonBorrar_Click(object sender, EventArgs e)
        {
            if (this.ClearFolder(@"c:\temp\")) { }
                this.toolStripStatusLabel1.Text = "Borrado Finalizado";
        }

        private void button4_Click(object sender, EventArgs e)
        {
       


        }


 

        private void button5_Click(object sender, EventArgs e)
        {
            string a = GetFontNameFromFile("");
        }

      

        private List<string> ParseaCampo(string Parrafo, XFont font, double dRango, XGraphics graph)
        {
            List<string> retvalue = new List<string>();

            var size = graph.MeasureString(Parrafo, font);
            int a=1;
            int rango = 100;
          
            if (size.Width > dRango)
            {
                int contador = 1;
                int lastspace = 0;
                double auxSize;

                while (a < rango)
                {
                    if (Parrafo.Substring(contador, 1) == " ")
                        lastspace = contador;

                    var size1 = graph.MeasureString(Parrafo.Substring(0,contador), font);
                    auxSize = size1.Width;
                    contador++;
                    a=(int)(auxSize);
                }
                retvalue.Add(Parrafo.Substring(0, lastspace));

                a = 1;
                contador = 1;
                string subparrafo = Parrafo.Substring(lastspace).TrimStart();
                int maxlenght = subparrafo.Length;
                lastspace = 0;
                while ((a < rango)&&(contador<maxlenght))
                {
                    var size1 = graph.MeasureString(subparrafo.Substring(0, contador), font);
                    auxSize = size1.Width;
                    contador++;
                    a = (int)(auxSize);
                }
              
                retvalue.Add(subparrafo.Substring(0, contador));

            }
            else
            {
                retvalue.Add(Parrafo);
            }
            return retvalue;

        }

        private void button4_Click_1(object sender, EventArgs e)
        {

            Cursor.Current = Cursors.WaitCursor;
            toolStripStatusLabel1.Text = "";

            Stream myStream = null;
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            int counter = 0;  
            string line;
            IList<string> DatosComprobante = new List<string>();


            Cursor.Current = Cursors.WaitCursor;
            toolStripStatusLabel1.Text = "";

            TelnetPDF.ClassParametrosTelnetPDF cp = new TelnetPDF.ClassParametrosTelnetPDF
            {
                FileRoute = "c:\\temp\\",         //"D:\\TelnetPDF\\WebApplicationTelnetPDF\\WebApplicationTelnetPDF\\files\\";

                Imageroute = ConfigurationSettings.AppSettings["ImageRoute"],
                Urlpath = ConfigurationSettings.AppSettings["Urlpath"].ToString(), // "/files/";
                Urlmensaje = ConfigurationSettings.AppSettings["Urlmensaje"].ToString(), // "Descargar Archivo Aqui";
                Archivo = ConfigurationSettings.AppSettings["Archivo"].ToString(),
                Archivo2 = ConfigurationSettings.AppSettings["Archivo2"].ToString(),
                CantidadCamposxRegistro = ConfigurationSettings.AppSettings["CantidadCamposxRegistro"].ToString(),
                CnnStringSqlServerSinPassword = ConfigurationSettings.AppSettings["CnnStringSqlServerSinPassword"].ToString(),
                CuentaUniverse = ConfigurationSettings.AppSettings["CuentaUniverse"].ToString(),

                PasswordSqlServerEncriptado = ConfigurationSettings.AppSettings["PasswordSqlServerEncriptado"].ToString(),
                PasswordUniverseEncriptado = ConfigurationSettings.AppSettings["PasswordUniverseEncriptado"].ToString(),
                Path = ConfigurationSettings.AppSettings["Path"].ToString(),
                Path2 = ConfigurationSettings.AppSettings["Path2"].ToString(),
                ServerUniverse = ConfigurationSettings.AppSettings["ServerUniverse"].ToString(),
                UsuarioUniverse = ConfigurationSettings.AppSettings["UsuarioUniverse"].ToString(),
                LogFile = ConfigurationSettings.AppSettings["PathLog"].ToString(),
                FontRoute = ConfigurationSettings.AppSettings["FontRoute"].ToString()
            };

            TelnetPDF.comprobantesPDF ts = new TelnetPDF.comprobantesPDF();


             
            openFileDialog1.InitialDirectory = "c:\\";
            openFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    System.IO.StreamReader file = new System.IO.StreamReader(@openFileDialog1.FileName);
                    int iSkip = 0;
                       toolStripProgressBar1.Value = 0;
                        while ((line = file.ReadLine()) != null) 
                          {
                              line = line.Replace('þ', '|');
                            DatosComprobante.Add(line);
                            counter++;
                         }
                       

                        toolStripProgressBar1.Maximum = counter;
                        int LngLotes;
                        if (counter < 250)
                            LngLotes = counter;
                        else
                            LngLotes = 250;
                        
                       //analizo la cantidad de lotes a procesar
                        int cantidadlotes = counter / LngLotes;


                        for (int i = 1; i <= cantidadlotes; i++)
                        {
                            var subList = DatosComprobante.Skip(iSkip).Take(LngLotes).ToList();
                          //  if (i == 4)
                          //  {
                                string p = ts.GenerarFacturasLotes(subList, cp, true, true, i.ToString());
                           // }
                                //list.Skip(2).Take(5).ToList();
                            toolStripProgressBar1.Value = (i * LngLotes);
                            iSkip = iSkip + LngLotes;
                        }

                        Cursor.Current = Cursors.Default;
                        toolStripStatusLabel1.Text = "Lote Creado";

                }

                  
               

                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }

        }

        private void statusStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

       

        private void button6_Click(object sender, EventArgs e)
        {
            WsGeneracionComprobantes.pdfgen.prod.WSGeneracionComprobantes ws = new WsGeneracionComprobantes.pdfgen.prod.WSGeneracionComprobantes();
           string VariablePerturbadora= ws.SGenerarComprobanteruta(sNumeroComprobante.Text.Trim(), @"c:\inetpub\wwwroot\newmailer\files\");
            var url = "http://facturas.antina.com.ar/files/" + VariablePerturbadora;
            toolStripStatusLabel1.Text = VariablePerturbadora;
 
        }

        private void sNumeroComprobante_TextChanged(object sender, EventArgs e)
        {

        }

        private void button5_Click_1(object sender, EventArgs e)
        {
            wsgeneracioncomprobantes85.WSGeneracionComprobantes ws = new wsgeneracioncomprobantes85.WSGeneracionComprobantes();
            string VariablePerturbadora = ws.SGenerarComprobanteruta(this.sNumeroComprobante.Text.Trim(), @"C:\inetpub\wwwroot\DescargaComprobante\files\");
            var url = "http://facturas.antina.com.ar/files/" + VariablePerturbadora;
            toolStripStatusLabel1.Text = VariablePerturbadora;
        }
    }
}
