using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ButtonSelectFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                txtNomer.Text = File.ReadAllText(openFileDialog.FileName);
            }
        }

        private async void ButtonSaveFile_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            if (saveFileDialog.ShowDialog() == true)
            {
                await SaveFileContent(saveFileDialog.FileName, txtNomer.Text);
                MessageBox.Show("Файл успешно сохранен.");
            }
        }

        private async void CheckBoxDeleteAllTochki_Checked(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtNomer.Text))
            {
                txtNomer.Text = await ProcessFileContent(txtNomer.Text, txtCustomPunctuation.Text, DeleteWord: false, DeletePrepenaniya: true);
            }
            else
            {
                MessageBox.Show("Файл не выбран");
            }
        }

        private async void CheckBoxDeleteAllWord_Checked(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtNomer.Text))
            {
                txtNomer.Text = await ProcessFileContent(txtNomer.Text, txtCustomPunctuation.Text, DeletePrepenaniya: false, DeleteWord: true);
            }
            else
            {
                MessageBox.Show("Файл не выбран");
            }
        }

        private Task SaveFileContent(string filePath, string content)
        {
            return Task.Run(() => File.WriteAllText(filePath, content));
        }

        private async Task<string> ProcessFileContent(string fileContent, string deletePrepenanie, bool DeletePrepenaniya, bool DeleteWord)
        {
            if (DeletePrepenaniya)
            {
                fileContent = await Task.Run(() => Regex.Replace(fileContent, $"[{deletePrepenanie}]", ""));
            }

            if (DeleteWord)
            {
                fileContent = await Task.Run(() => Regex.Replace(fileContent, @"\b\w{1,10}\b", ""));
            }

            return fileContent;
        }
    }
}
