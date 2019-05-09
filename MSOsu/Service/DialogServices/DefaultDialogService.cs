using System;
using Microsoft.Win32;
using System.Windows;
using System.Text.RegularExpressions;
using MSOsu.View;

namespace MSOsu.Service.DialogServices
{
    public class DefaultDialogService : IDialogService
    {
        public string filePath;

        public string GetFilePath()
        {
            return filePath;
        }

        public bool OpenFileDialog(string openFilter = "Все файлы (*.*)|*.*")
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = false;
            openFileDialog.Filter = openFilter;
            if (openFileDialog.ShowDialog() == true)
            {
                filePath = openFileDialog.FileName;
                return true;
            }
            return false;
        }

        string GetDefaultExt(string saveFilter)
        {
            return Regex.Match(saveFilter, @"\.(.+?)$").Groups[1].Value;
        }

        public bool SaveFileDialog(string saveFilter = "Все файлы (*.*)|*.*")
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.DefaultExt = GetDefaultExt(saveFilter);
            saveFileDialog.AddExtension = true;
            saveFileDialog.OverwritePrompt = true;
            saveFileDialog.Filter = saveFilter;
            if (saveFileDialog.ShowDialog() == true)
            {
                filePath = saveFileDialog.FileName;
                return true;
            }
            return false;
        }

        public void ShowErrorMessage(string message, string caption) 
        {
            MetroMessage metroDialog = new MetroMessage();
            metroDialog.SetMessage(message, caption, true);
            metroDialog.ShowDialog();
        }

        public void ShowInformationMessage(string message, string caption) 
        {
            MetroMessage metroDialog = new MetroMessage();
            metroDialog.SetMessage(message, caption, false);
            metroDialog.ShowDialog();
        }

        public bool ShowChoiceMessage(string message, string caption) 
        {
            MessageBoxResult result = MessageBox.Show(message, caption, MessageBoxButton.YesNo, MessageBoxImage.Information);
            if (result == MessageBoxResult.Yes)
                return true;
            return false;
        }
    }
}
