namespace MSOsu.Service
{
    public interface IDialogService
    {
        void ShowInformationMessage(string messagen, string caption);   // показ информационного сообщения
        void ShowErrorMessage(string message, string caption);   // показ сообщения ошибки
        bool OpenFileDialog(string operFilter);  // открытие файла
        bool SaveFileDialog(string saveFilter);  // сохранение файла
        string GetFilePath(); // путь к выбранному файлу
    }
}
