using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ADO_Lesson1
{
	/// <summary>
	/// Interaction logic for OrmWindow.xaml
	/// </summary>
	public partial class OrmWindow: Window
	{
		public ObservableCollection<Entity.Department> Departments
		{
			get; set;
		}
		private readonly SqlConnection _connection;



		public OrmWindow()
		{
			InitializeComponent();
			Departments = new();

			this.DataContext = this;   // місце пошуку {Binding Departments}
			_connection = new(App.ConnectionString);
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			try
			{
				_connection.Open();
				
				SqlCommand cmd = new()
				{
					Connection = _connection
				};

				#region Load Departments
				cmd.CommandText = "SELECT D.Id, D.Name FROM Departments D";
				var reader = cmd.ExecuteReader();
				
				while (reader.Read())
				{
					Departments.Add(
							new Entity.Department
							{
								Id = reader.GetGuid(0),
								Name = reader.GetString(1)
							}
						);
				}
				reader.Close();
				#endregion

				//#region Load Products
				//cmd.CommandText = "SELECT P.* FROM Products P WHERE P.DeleteDt IS NULL";
				//reader = cmd.ExecuteReader();
				//while (reader.Read())
				//{
				//	Products.Add(new(reader));
				//}
				//reader.Close();
				//#endregion

				//#region Load Managers
				//cmd.CommandText = "SELECT M.Id, M.Surname, M.Name, M.Secname, M.Id_main_Dep, M.Id_sec_dep, M.Id_chief FROM Managers M";
				//reader = cmd.ExecuteReader();
				//while (reader.Read())
				//{
				//	Managers.Add(
				//		new Entity.Manager
				//		{
				//			Id = reader.GetGuid(0),
				//			Surname = reader.GetString(1),
				//			Name = reader.GetString(2),
				//			Secname = reader.GetString(3),
				//			IdMainDep = reader.GetGuid(4),
				//			IdSecDep = reader.GetValue(5) == DBNull.Value
				//						? null
				//						: reader.GetGuid(5),
				//			IdChief = reader.IsDBNull(6)
				//						? null
				//						: reader.GetGuid(6)
				//		});
				//}
				//reader.Close();
				//#endregion

				//#region Load Sales
				//cmd.CommandText = "SELECT S.* FROM Sales S";
				//reader = cmd.ExecuteReader();
				//while (reader.Read())
				//{
				//	Sales.Add(new(reader));
				//}
				//reader.Close();
				//#endregion

				cmd.Dispose();
			}
			catch (SqlException ex)
			{
				MessageBox.Show(
					ex.Message,
					"Window will be closed",
					MessageBoxButton.OK,
					MessageBoxImage.Error);
				this.Close();
			}
		}
	}
}
