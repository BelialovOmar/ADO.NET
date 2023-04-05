using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.SqlClient;  // не забути NuGet
using System.Data;
using System.IO;


namespace ADO_Lesson1
{
	public partial class MainWindow: Window
	{
		// об'єкт-підключення, основа ADO
		private SqlConnection _connection;  // MS SQL ADO

		public MainWindow()
		{
			InitializeComponent();

			// !! створення об'єкта не відкиває підключення
			_connection = new();

			// головний параметр - рядок підключення
			_connection.ConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Admin\source\repos\BelialovOmar\ADO.NET\ADO_Lesson1\ADO-Lesson1\Database1.mdf;Integrated Security=True";
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			try  // відкриваємо підключення
			{
				_connection.Open();
				// відображаємо статус підключення на вікні
				StatusConnection.Content = "Connected";
				StatusConnection.Foreground = Brushes.Green;
			}
			catch (SqlException ex)
			{
				// відображаємо статус підключення на вікні
				StatusConnection.Content = "Disconnected";
				StatusConnection.Foreground = Brushes.Red;

				MessageBox.Show(ex.Message);
				this.Close();
			}

			ShowMonitorDepartments();
		}

		private void Window_Closed(object sender, EventArgs e)
		{
			if (_connection?.State == ConnectionState.Open)
			{
				_connection.Close();
			}
		}

		#region Запити без повернення результатів
		private void CreateDepartments_Click(object sender, RoutedEventArgs e)
		{
			// команда - ресурс для передачі SQL запиту до СУБД
			SqlCommand cmd = new();

			// Обов'язкові параметри команди:
			cmd.Connection = _connection;  // відкрите підключення

			cmd.CommandText =              // та текст SQL запиту
				@"CREATE TABLE Departments (
	                Id	 UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
	                Name NVARCHAR(50) NOT NULL
                )";

			try
			{
				cmd.ExecuteNonQuery();  // NonQuery - без повернення рез-ту
				MessageBox.Show("Create OK");
			}
			catch (SqlException ex)
			{
				MessageBox.Show(
					ex.Message,
					"Create error",
					MessageBoxButton.OK,
					MessageBoxImage.Stop);
			}
			cmd.Dispose();  // команда - unmanaged, потрібно вивільняти ресурс
			
		}
		private void FillDepartments_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				SqlCommand command = new SqlCommand("INSERT INTO Departments (Id, Name) VALUES " +
										   "('D3C376E4-BCE3-4D85-ABA4-E3CF49612C94', N'IT отдел')," +
										   "('131EF84B-F06E-494B-848F-BB4BC0604266', N'Бухгалтерия')," +
										   "('8DCC3969-1D93-47A9-8B79-A30C738DB9B4', N'Служба безопасности')," +
										   "('D2469412-0E4B-46F7-80EC-8C522364D099', N'Отдел кадров')," +
										   "('1EF7268C-43A8-488C-B761-90982B31DF4E', N'Канцелярия')," +
										   "('415B36D9-2D82-4A92-A313-48312F8E18C6', N'Отдел продаж')," +
										   "('624B3BB5-0F2C-42B6-A416-099AAB799546', N'Юридическая служба')");
				command.Connection = _connection;

				command.ExecuteNonQuery();
				MessageBox.Show("Fill OK");
			}
			catch (SqlException ex)
			{
				MessageBox.Show(
					ex.Message,
					"Fill error",
					MessageBoxButton.OK,
					MessageBoxImage.Stop);
			}
		}


		private void CreateProducts_Click(object sender, RoutedEventArgs e)
		{
			String sql =
				@"CREATE TABLE Products (
	                Id			UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
	                Name		NVARCHAR(50) NOT NULL,
	                Price		FLOAT  NOT NULL
                )";
			using SqlCommand cmd = new(sql, _connection);
			try
			{
				cmd.ExecuteNonQuery();
				MessageBox.Show("Create Products OK");
			}
			catch (SqlException ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		private void FillProducts_Click(object sender, RoutedEventArgs e)
		{
			String sql = File.ReadAllText("sql/fill_products.sql") ;
			using SqlCommand cmd = new(sql, _connection);
			try
			{
				cmd.ExecuteNonQuery();
				MessageBox.Show("Fill Products OK");
			}
			catch (SqlException ex)
			{
				MessageBox.Show(ex.Message);
			}
		}
		#endregion


		#region Запити з одним (скалярним) результатом 
		private void ShowMonitorDepartments()
		{
			using SqlCommand cmd = new("SELECT COUNT(1) FROM Departments", _connection);
			try
			{
				object res = cmd.ExecuteScalar();   // Виконання запиту + повернення
													// "лівого-верхнього" результату з поверненої таблиці
													// повертає типізовані дані (число, рядок, дату-час, тощо), але
													// у формі object, відповідно для використання потрібне перетворення
				int cnt = Convert.ToInt32(res);
				StatusDepartments.Content = cnt.ToString();
			}
			catch (SqlException ex)  // помилка запиту
			{
				MessageBox.Show(ex.Message, "SQL error",
					MessageBoxButton.OK, MessageBoxImage.Stop);
				StatusDepartments.Content = "---";
			}
			catch (Exception ex)  // інші помилки (перетворення типів)
			{
				MessageBox.Show(ex.Message, "Cast error",
					MessageBoxButton.OK, MessageBoxImage.Exclamation);
				StatusDepartments.Content = "---";
			}
		}
		#endregion
	}
}
