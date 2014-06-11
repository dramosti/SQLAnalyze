using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace HLP.SQLAnalyse.Model
{
    public class Static
    {

        public static void FecharWindow(object i)
        {
            (i as Window).Close();
        }
        public static void MinimizeWindow(object i)
        {
            (i as Window).WindowState = WindowState.Minimized;
        }

        public static object ExecuteMethodByReflection(string xNamespace, string xType, string xMethod, object[] parameters)
        {
            Type t = GetTypeByReflection(xNamespace: xNamespace, xType: xType);

            if (t != null)
            {
                MethodInfo method = t.GetMethod(name: xMethod);

                if (method != null)
                    return method.Invoke(obj: t, parameters: parameters);
            }

            return null;
        }
        public static Type GetTypeByReflection(string xNamespace, string xType)
        {
            Type t = null;

            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

            if (assemblies != null)
            {
                Assembly selectedAssembly = assemblies.ToList().FirstOrDefault(
                    i => i.ManifestModule.Name.Equals(value: xNamespace));

                if (selectedAssembly != null)
                    t = selectedAssembly.GetTypes().FirstOrDefault(i => i.Name == xType);
            }

            return t;
        }



        public static void SelectRowByIndex(DataGrid dataGrid, int rowIndex)
        {
            if (!dataGrid.SelectionUnit.Equals(DataGridSelectionUnit.FullRow))
                throw new ArgumentException("The SelectionUnit of the DataGrid must be set to FullRow.");

            if (rowIndex < 0 || rowIndex > (dataGrid.Items.Count - 1))
                throw new ArgumentException(string.Format("{0} is an invalid row index.", rowIndex));

            dataGrid.SelectedItems.Clear();
            /* set the SelectedItem property */
            object item = dataGrid.Items[rowIndex]; // = Product X
            dataGrid.SelectedItem = item;

            DataGridRow row = dataGrid.ItemContainerGenerator.ContainerFromIndex(rowIndex) as DataGridRow;
            if (row == null)
            {
                /* bring the data item (Product object) into view
                 * in case it has been virtualized away */
                dataGrid.ScrollIntoView(item);
                row = dataGrid.ItemContainerGenerator.ContainerFromIndex(rowIndex) as DataGridRow;
            }
            //TODO: Retrieve and focus a DataGridCell object
        }
    }
}
