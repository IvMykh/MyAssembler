using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Input;
using Microsoft.Win32;
using MyAssembler.Core;
using MyAssembler.UI.Properties;
using MyAssembler.UI.View;
using MyAssembler.UI.ViewModel.Infrastructure;

namespace MyAssembler.UI.ViewModel
{
    class Presenter
        : ObservableObject
    {
        // Fields.
        private string _sourceCode;
        private string _status;
        private string _errorMessage;
        private int _selectedTabIndex;
        private StatusBarStyle _statusBarStyle;

        private ObservableCollection<ProtocolItem> _protocolItems;
        private ObservableCollection<IdentifierAddressPair> _byteCellItems;
        private ObservableCollection<IdentifierAddressPair> _wordCellItems;
        private ObservableCollection<IdentifierAddressPair> _labelItems;

        // Properties.
        public string SourceCode
        {
            get {
                return _sourceCode;
            }
            set {
                _sourceCode = value;
                RaisePropertyChangedEvent("SourceCode");

                resetStatus();
            }
        }
        public string Status
        {
            get { 
                return _status; 
            }
            set { 
                _status = value;
                RaisePropertyChangedEvent("Status");
            }
        }
        public string ErrorMessage
        {
            get {
                return _errorMessage;
            }
            set {
                _errorMessage = value;
                RaisePropertyChangedEvent("ErrorMessage");
            }
        }
        public int SelectedTabIndex
        {
            get { 
                return _selectedTabIndex; 
            }
            set {
                _selectedTabIndex = value;
                RaisePropertyChangedEvent("SelectedTabIndex");
            }
        }

        public ObservableCollection<ProtocolItem> ProtocolItems
        {
            get { 
                return _protocolItems; 
            }
            set {
                _protocolItems = value;
                RaisePropertyChangedEvent("ProtocolItems");
            }
        }
        public ObservableCollection<IdentifierAddressPair> ByteCellItems
        {
            get {
                return _byteCellItems; 
            }
            set {
                _byteCellItems = value;
                RaisePropertyChangedEvent("ByteCellItems");
            }
        }
        public ObservableCollection<IdentifierAddressPair> WordCellItems
        {
            get {
                return _wordCellItems; 
            }
            set {
                _wordCellItems = value;
                RaisePropertyChangedEvent("WordCellItems");
            }
        }
        public ObservableCollection<IdentifierAddressPair> LabelItems
        {
            get {
                return _labelItems; 
            }
            set {
                _labelItems = value;
                RaisePropertyChangedEvent("LabelItems");
            }
        }
        public StatusBarStyle StatusBarStyle
        {
            get {
                return _statusBarStyle;
            }
            set {
                _statusBarStyle = value;
                RaisePropertyChangedEvent("StatusBarStyle");
            }
        }

        public Assembler MyAssembler { get; private set; }



        public Presenter()
        {
            MyAssembler = new Assembler();
            
            SourceCode = string.Empty;
            resetStatus();
        }

        private void resetStatus()
        {
            Status = Resources.ReadyStatus;
            ErrorMessage = string.Empty;
            StatusBarStyle = StatusBarStyle.Neutral;
        }

        private ObservableCollection<ProtocolItem> getProtocolItems(
            TranslationResult result)
        {
            var items = new ObservableCollection<ProtocolItem>();

            for (int i = 0; i < result.TokensLists.Count; i++)
            {
                items.Add(new ProtocolItem(
                    result.TokensLists[i], result.TranslatedBytes[i], result.Addresses[i]));
            }

            return items;
        }
        private ObservableCollection<IdentifierAddressPair> getLabelItems(
            TranslationResult result)
        {
            var items = new ObservableCollection<IdentifierAddressPair>();

            foreach (var item in result.Labels)
            {
                items.Add(new IdentifierAddressPair {
                    Identifier = item.Key,
                    Address = string.Format("{0:X2}", item.Value)
                });
            }

            return items;
        }
        private ObservableCollection<IdentifierAddressPair> getByteCellItems(
            TranslationResult result)
        {
            var items = new ObservableCollection<IdentifierAddressPair>();

            foreach (var item in result.ByteCells)
            {
                items.Add(new IdentifierAddressPair {
                    Identifier = item.Key,
                    Address = string.Format("{0:X2}", item.Value)
                });
            }


            return items;
        }
        private ObservableCollection<IdentifierAddressPair> getWordCellItems(
            TranslationResult result)
        {
            var items = new ObservableCollection<IdentifierAddressPair>();

            foreach (var item in result.WordCells)
            {
                items.Add(new IdentifierAddressPair {
                    Identifier = item.Key,
                    Address = string.Format("{0:X2}", item.Value)
                });
            }

            return items;
        }

        private TranslationResult performBuilding()
        {
            ProtocolItems = null;
            ByteCellItems = null;
            WordCellItems = null;
            LabelItems = null;

            try
            {
                TranslationResult result = MyAssembler.Translate(SourceCode);

                Status = Resources.BuiltSuccessfullyStatus;
                StatusBarStyle = StatusBarStyle.Success;

                SelectedTabIndex = 1;

                ProtocolItems = getProtocolItems(result);
                ByteCellItems = getByteCellItems(result);
                WordCellItems = getWordCellItems(result);
                LabelItems = getLabelItems(result);

                // Write binary file and .bat file
                
                var batFilePath = Path.Combine(
                    Resources.BuildOutputDirectory, Resources.RunBatFileName); 
                            
                File.WriteAllText(batFilePath, 
                    string.Format(Resources.RunBatFileContent, Resources.ProgramComFileName));


                var comFilePath = Path.Combine(
                    Resources.BuildOutputDirectory, Resources.ProgramComFileName); 

                using (var stream = new FileStream(
                    comFilePath, FileMode.OpenOrCreate, FileAccess.Write))
                {
                    using (var binaryWriter = new BinaryWriter(stream))
                    {
                        foreach (var item in result.TranslatedBytes)
                        {
                            binaryWriter.Write(item);
                        }
                    }
                }

                return result;
            }
            catch (CompilationErrorException cee)
            {
                Status = Resources.ErrorOccurredStatus;
                ErrorMessage = cee.Message;
                StatusBarStyle = StatusBarStyle.Fail;
            }

            return null;
        }

        public ICommand BuildCommand
        {
            get {
                return new DelegateCommand(
                    () => {
                        performBuilding();
                    });
            }
        }
        public ICommand RunCommand
        {
            get {
                return new DelegateCommand(
                    () => {
                        TranslationResult result = performBuilding();

                        if (result != null)
                        {
                            var batFilePath = Path.Combine(
                                Resources.BuildOutputDirectory, Resources.RunBatFileName); 

                            Process.Start(
                                Resources.DosBoxExePath,
                                string.Format(Resources.DosBoxParameterFormat, Path.GetFullPath(batFilePath)));
                        }
                    });
            }
        }
        public ICommand ClearBuildOutputCommand
        {
            get
            {
                return new DelegateCommand(
                    () => {
                        ProtocolItems = null;
                        LabelItems = null;
                        ByteCellItems = null;
                        WordCellItems = null;

                        resetStatus();
                        Status = Resources.BuildOutputClearedStatus;
                    });
            }
        }


        private string selectFile<T>()
           where T : FileDialog, new()
        {
            var fileDialog = new T();
            fileDialog.Filter = Resources.FileDialogFilter;

            string initialDirectory = Path.GetFullPath(
                Path.Combine(
                    Directory.GetCurrentDirectory(),
                    Resources.DataDirectory)
                );

            if (!Directory.Exists(initialDirectory))
            {
                Directory.CreateDirectory(initialDirectory);
            }

            fileDialog.InitialDirectory = initialDirectory;

            bool? dialogResult = fileDialog.ShowDialog();

            if (dialogResult == true)
            {
                return fileDialog.FileName;
            }

            return string.Empty;
        }

        public ICommand NewFileCommand
        {
            get {
                return new DelegateCommand(
                    () => {
                        SourceCode = string.Empty;

                        resetStatus();
                        Status = Resources.TextEditorEmptiedStatus;
                    });
            }
        }
        public ICommand OpenFileCommand
        {
            get {
                return new DelegateCommand(
                    () => {
                        string filePath = selectFile<OpenFileDialog>();
                        
                        if (!string.IsNullOrEmpty(filePath))
                        {
                            SourceCode = File.ReadAllText(filePath, Encoding.Default);

                            resetStatus();
                            Status = Resources.OpenedSuccessfullyStatus;
                        }
                    });
            }
        }
        public ICommand SaveFileCommand
        {
            get {
                return new DelegateCommand(
                    () => {
                        string filePath = selectFile<SaveFileDialog>();

                        if (!string.IsNullOrEmpty(filePath))
                        {
                            File.WriteAllText(filePath, SourceCode, Encoding.Default);

                            resetStatus();
                            Status = Resources.SavedSuccessfullyStatus;
                        }
                    });
            }
        }

        public ICommand ShowAboutCommand
        {
            get {
                return new DelegateCommand(
                    () => {
                        var aboutWindow = new AboutWindow();
                        Nullable<bool> dialogResult = aboutWindow.ShowDialog();
                    });
            }
        }
    }
}
