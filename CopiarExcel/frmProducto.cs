using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Reflection;

namespace CopiarExcel
{
    public partial class frmProducto : Form
    {
        private List<beProducto> lbeProducto;

        public frmProducto()
        {
            InitializeComponent();
        }

        private void cargarProductos(object sender, EventArgs e)
        {
            lbeProducto = new List<beProducto>();
            Random oRandom = new Random();
            beProducto obeProducto;
            for(int i=1;i<=10;i++)
            {
                obeProducto = new beProducto();
                obeProducto.IdProducto = i;
                obeProducto.Nombre = String.Format("Producto {0}", i);
                obeProducto.Precio = oRandom.Next(100)+10;
                obeProducto.Stock = (short)(oRandom.Next(10) + 1);
                lbeProducto.Add(obeProducto);
            }
            dgvProducto.DataSource = lbeProducto;
        }

        private void pegarExcel(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.V)
            {
                string formato = DataFormats.CommaSeparatedValue;
                object contenido = Clipboard.GetData(formato);
                if (contenido != null && contenido is MemoryStream)
                {
                    byte[] buffer;
                    using (MemoryStream ms = (MemoryStream)contenido) buffer = ms.ToArray();
                    string lista = Encoding.UTF8.GetString(buffer).Replace("\r\n", ";");
                    string[] data = lista.Split(';');
                    PropertyInfo[] propiedades = lbeProducto[0].GetType().GetProperties();
                    if ((data.Length - 1) % propiedades.Length == 0)
                    {
                        beProducto obeProducto = null;
                        PropertyInfo propiedad;
                        int c = 0;
                        Type tipo = null;
                        while (c < data.Length - 1)
                        {
                            for (int i = 0; i < propiedades.Length; i++)
                            {
                                if (i == 0) obeProducto = new beProducto();
                                propiedad = obeProducto.GetType().GetProperty(propiedades[i].Name);
                                tipo = propiedad.PropertyType;
                                propiedad.SetValue(obeProducto, Convert.ChangeType(data[c], tipo));
                                if (i == propiedades.Length - 1) lbeProducto.Add(obeProducto);
                                c++;
                            }
                        }
                        dgvProducto.DataSource = null;
                        dgvProducto.DataSource = lbeProducto;
                    }
                    else MessageBox.Show("Numero de columnas a copiar del Excel y la Grilla deben ser iguales");
                }
                else MessageBox.Show("No hay un rango de celdas a copiar");
            }
        }
    }
}
