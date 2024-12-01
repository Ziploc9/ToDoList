using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Text.Json;
using System.Drawing;

namespace ToDoList
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            LimpiarTextBox();
            dataGridViewTareas.Columns.Add("Tarea", "Lista de Tarea"); // Agregar columna para las                                         
            dataGridViewTareas.Columns["Tarea"].HeaderCell.Style.Font = new Font("Century Gothic", 12, FontStyle.Bold); // Cambiar la fuente del título de la columna
            dataGridViewTareas.Columns["Tarea"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter; // Alineo al cnetro
            dataGridViewTareas.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill; // Ajustar el tamaño de la columna
            CargarInformacion();
           
        }
        
        public class Tarea
        {
            public string Descripcion { get; set; }
        }
        
        private void GuardarTareas(List<Tarea> tareas)
        {
            string json = JsonSerializer.Serialize(tareas);
            File.WriteAllText("tareas.json", json);
        }

        public List<Tarea> CargarTareas()
        {
            if (File.Exists("tareas.json"))
            {
                string json = File.ReadAllText("tareas.json");
                return JsonSerializer.Deserialize<List<Tarea>>(json);
            }
            return new List<Tarea>();
        }

        private void CargarInformacion()
        {
            List<Tarea> tareas = CargarTareas();

            foreach (var tarea in tareas)
            {
                dataGridViewTareas.Rows.Add(tarea.Descripcion);  // Agregar la tarea al DataGridView
            }
        }

        #region Limpiar Texto Nuevas Tareas
        private void LimpiarTextBox()
        {
            txtTareaNueva.Text = "Nuevo Recordatorio";
            /** Asignacion de eventos enter y leave**/
            txtTareaNueva.Enter += TxtTareaNueva_Enter;
            txtTareaNueva.Leave += TxtTareaNueva_Leave;

        }

        /** Limpia el texto de la caja**/
        private void TxtTareaNueva_Enter(object sender, EventArgs e)
        {
            if(txtTareaNueva.Text == "Nuevo Recordatorio")
            {
                txtTareaNueva.Text = string.Empty; // Limpia el texto
                txtTareaNueva.ForeColor = System.Drawing.Color.Black;
                txtTareaNueva.Font = new System.Drawing.Font(txtTareaNueva.Font.FontFamily, 10);
            }
        }

       /** Restaura el texto de la caja**/
        private void TxtTareaNueva_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtTareaNueva.Text))
            {
                txtTareaNueva.Text = "Nuevo Recordatorio";
                txtTareaNueva.ForeColor = System.Drawing.Color.Crimson;
                txtTareaNueva.Font = new System.Drawing.Font(txtTareaNueva.Font.FontFamily, 12);
            }
        }

        #endregion

        
        private void btnCrear_Click(object sender, EventArgs e)
        {
            if(txtTareaNueva.Text != "Nuevo Recordatorio")
            {
                
                try
                {
                    // Verificar que el TextBox no esté vacío
                    if (!string.IsNullOrWhiteSpace(txtTareaNueva.Text))
                    {
                        Tarea nuevaTarea = new Tarea
                        {
                            Descripcion = txtTareaNueva.Text
                        };

                        // Agregar la tarea al DataGridView
                        dataGridViewTareas.Rows.Add(nuevaTarea.Descripcion);

                        List<Tarea> tareas = CargarTareas();
                        tareas.Add(nuevaTarea);

                        GuardarTareas(tareas);

                        // Limpiar el TextBox después de agregar la tarea
                        txtTareaNueva.Text = string.Empty;
                    }
                    else
                    {
                        MessageBox.Show("Por favor, ingresa una tarea antes de agregarla.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }

                }
                catch (Exception ex) 
                {

                }

            }

        }

        /**  Boton Eliminar **/
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridViewTareas.SelectedRows.Count > 0)
                {
                    // Verifica si el valor de la celda es null antes de convertirlo
                    var descripcionTarea = dataGridViewTareas.SelectedRows[0].Cells[0].Value;

                    if (descripcionTarea != null) // Si no es null, continuar
                    {
                        // Convertir a string solo si el valor no es null
                        string tarea = descripcionTarea.ToString();

                        // Eliminar la tarea del DataGridView
                        dataGridViewTareas.Rows.RemoveAt(dataGridViewTareas.SelectedRows[0].Index);

                        // Cargar tareas actuales
                        List<Tarea> tareas = CargarTareas();

                        // Eliminar la tarea de la lista
                        tareas.RemoveAll(t => t.Descripcion == tarea);

                        // Guardar las tareas actualizadas en el archivo JSON
                        GuardarTareas(tareas);
                    }
                    else
                    {
                        MessageBox.Show("La tarea seleccionada está vacía o no tiene valor.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Por favor, selecciona una tarea para eliminar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("No puedes borrar algo que no está o ocurrió un error: " + ex.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }


       
    }
}
