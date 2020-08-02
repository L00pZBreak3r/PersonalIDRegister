using System;
using System.Windows.Media;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

using PersonalIDRegister.Helpers;

using DocumentLibrary.Models;
using DocumentLibrary.Contexts;

namespace PersonalIDRegister.ViewModels
{
  internal class DocumentControlViewModel : WindowViewModelBase, IDataErrorInfo
  {
    public DocumentControlViewModel(Window win)
      : base(win)
    {
      OkCommand = new RelayCommand(OnOkCommand, OkCommandCanExecute);
      CancelCommand = new RelayCommand(OnCancelCommand);
    }

    #region OkCommand

    public ICommand OkCommand { get; set; }

    private bool OkCommandCanExecute(object p)
    {
      return string.IsNullOrEmpty(Error);
    }

    private void OnOkCommand(object p)
    {
      if (AddNewDocument())
      {
        OperationText = "Документ добавлен в базу";
        OperationTextColor.Color = Colors.Green;
      }
      else
      {
        OperationText = "Такой документ уже существует в базе";
        OperationTextColor.Color = Colors.Red;
      }
      RaisePropertyChanged("OperationTextColor");
    }

    #endregion

    #region CancelCommand

    public ICommand CancelCommand { get; set; }

    private void OnCancelCommand(object p)
    {
      CloseWindow();
    }

    #endregion

    #region DocumentTypeItemsSource

    private static readonly string[] mDocumentTypeItemsSource = new string[] { "Паспорт", "Свидетельство о рождении" };

    public string[] DocumentTypeItemsSource
    {
      get { return mDocumentTypeItemsSource; }
    }

    #endregion

    #region DocumentTypeSelectedIndex

    private IdentificationDocument.IdentificationDocumentType mDocumentType;

    public int DocumentTypeSelectedIndex
    {
      get
      {
        return (int)mDocumentType;
      }
      set
      {
        if ((int)mDocumentType != value)
        {
          mDocumentType = (value == 1) ? IdentificationDocument.IdentificationDocumentType.BirthCertificate : IdentificationDocument.IdentificationDocumentType.Passport;
          RaisePropertyChanged();
          RaisePropertyChanged("DocumentFieldVisible");
        }
      }
    }

    #endregion
    
    private bool AddNewDocument()
    {
      bool res = true;
      var nd = new IdentificationDocument();
      nd.FirstName = mFirstName;
      nd.LastName = mLastName;
      nd.MiddleName = mMiddleName;
      nd.DocumentType = mDocumentType;
      nd.DocumentNumber = mDocumentNumber;
      nd.DocumentSerial = mDocumentSerial;
      nd.DocumentDate = mDocumentDate;
      nd.DocumentIssuer = mDocumentIssuer;
      nd.DocumentIssuerCode = mDocumentIssuerCode;
      using (var db = new IdentificationDocumentContext())
      {
        var docs = db.Documents;
        foreach (var d in docs)
          if (nd.Equals(d))
          {
            res = false;
            break;
          }
        if (res)
        {
          db.Documents.Add(nd);
          db.SaveChanges();
        }
      }
      return res;
    }

    public string this[string columnName]
    {
      get
      {
        string result = string.Empty;
        switch (columnName)
        {
          case "FirstName":
            result = ValidateFirstName();
            break;
          case "LastName":
            result = ValidateLastName();
            break;
          case "DocumentNumber":
            result = ValidateDocumentNumber();
            break;
          case "DocumentDate":
            result = ValidateDocumentDate();
            break;
          case "DocumentIssuer":
            result = ValidateDocumentIssuer();
            break;
          case "DocumentIssuerCode":
            result = ValidateDocumentIssuerCode();
            break;
          case "DocumentSerial":
            result = ValidateDocumentSerial();
            break;
        }
        return result;

      }
    }

    private string ValidateAll()
    {
      var errors = new[]
      {
                ValidateFirstName(),
                ValidateLastName(),
                ValidateDocumentNumber(),
                ValidateDocumentDate(),
                ValidateDocumentIssuer(),
                ValidateDocumentIssuerCode(),
                ValidateDocumentSerial()
            };
      return string.Join("\n", errors.Where(error => !string.IsNullOrEmpty(error)));
    }

    private string ValidateFirstName()
    {
      if (string.IsNullOrWhiteSpace(mFirstName))
        return "Поле ИМЯ должно быть заполнено";
      return string.Empty;
    }

    private string ValidateLastName()
    {
      if (string.IsNullOrWhiteSpace(mLastName))
        return "Поле ФАМИЛИЯ должно быть заполнено";
      return string.Empty;
    }

    private string ValidateDocumentNumber()
    {
      if (mDocumentNumber <= 0)
        return "Неправильно заполнено поле НОМЕР ДОКУМЕНТА";
      return string.Empty;
    }

    private string ValidateDocumentDate()
    {
      if ((mDocumentType == IdentificationDocument.IdentificationDocumentType.Passport) && (mDocumentDate.Ticks < 618199776000000000L))
        return "Неправильно заполнено поле ДАТА ВЫДАЧИ";
      return string.Empty;
    }

    private string ValidateDocumentIssuer()
    {
      if (string.IsNullOrWhiteSpace(mDocumentIssuer))
        return "Поле КЕМ ВЫДАН должно быть заполнено";
      return string.Empty;
    }

    private string ValidateDocumentIssuerCode()
    {
      if ((mDocumentType == IdentificationDocument.IdentificationDocumentType.Passport) && string.IsNullOrWhiteSpace(mDocumentIssuerCode))
        return "Поле КОД ПОДРАЗДЕЛЕНИЯ должно быть заполнено";
      return string.Empty;
    }

    private string ValidateDocumentSerial()
    {
      if ((mDocumentType == IdentificationDocument.IdentificationDocumentType.Passport) && string.IsNullOrWhiteSpace(mDocumentSerial))
        return "Поле СЕРИЯ ДОКУМЕНТА должно быть заполнено";
      return string.Empty;
    }

    public string Error => ValidateAll();

    #region DocumentFieldVisible

    public bool DocumentFieldVisible
    {
      get { return mDocumentType == IdentificationDocument.IdentificationDocumentType.Passport; }
    }

    #endregion

    #region FirstName

    private string mFirstName;

    public string FirstName
    {
      get { return mFirstName; }
      set
      {
        OperationText = "";
        if (mFirstName != value)
        {
          mFirstName = value;
          RaisePropertyChanged();
        }
      }
    }

    #endregion

    #region LastName

    private string mLastName;

    public string LastName
    {
      get { return mLastName; }
      set
      {
        OperationText = "";
        if (mLastName != value)
        {
          mLastName = value;
          RaisePropertyChanged();
        }
      }
    }

    #endregion

    #region MiddleName

    private string mMiddleName;

    public string MiddleName
    {
      get { return mMiddleName; }
      set
      {
        OperationText = "";
        if (mMiddleName != value)
        {
          mMiddleName = value;
          RaisePropertyChanged();
        }
      }
    }

    #endregion

    #region DocumentNumber

    private int mDocumentNumber;
    private string mDocumentNumberString;

    public string DocumentNumber
    {
      get { return mDocumentNumberString; }
      set
      {
        OperationText = "";
        if (mDocumentNumberString != value)
        {
          mDocumentNumberString = value;
          RaisePropertyChanged();
          int.TryParse(value, out mDocumentNumber);
        }
      }
    }

    #endregion

    #region DocumentSerial

    private string mDocumentSerial;

    public string DocumentSerial
    {
      get { return mDocumentSerial; }
      set
      {
        OperationText = "";
        if (mDocumentSerial != value)
        {
          mDocumentSerial = value;
          RaisePropertyChanged();
        }
      }
    }

    #endregion

    #region DocumentIssuer

    private string mDocumentIssuer;

    public string DocumentIssuer
    {
      get { return mDocumentIssuer; }
      set
      {
        OperationText = "";
        if (mDocumentIssuer != value)
        {
          mDocumentIssuer = value;
          RaisePropertyChanged();
        }
      }
    }

    #endregion

    #region DocumentIssuerCode

    private string mDocumentIssuerCode;

    public string DocumentIssuerCode
    {
      get { return mDocumentIssuerCode; }
      set
      {
        OperationText = "";
        if (mDocumentIssuerCode != value)
        {
          mDocumentIssuerCode = value;
          RaisePropertyChanged();
        }
      }
    }

    #endregion

    #region DocumentDate

    private DateTime mDocumentDate;
    private string mDocumentDateString;

    public string DocumentDate
    {
      get { return mDocumentDateString; }
      set
      {
        OperationText = "";
        if (mDocumentDateString != value)
        {
          mDocumentDateString = value;
          RaisePropertyChanged();
          DateTime.TryParse(value, out mDocumentDate);
        }
      }
    }

    #endregion

    #region OperationText

    private string mOperationText;

    public string OperationText
    {
      get { return mOperationText; }
      set
      {
        if (mOperationText != value)
        {
          mOperationText = value;
          RaisePropertyChanged();
        }
      }
    }

    #endregion

    #region OperationTextColor

    private SolidColorBrush mOperationTextColor = new SolidColorBrush(Colors.Red);

    public SolidColorBrush OperationTextColor
    {
      get { return mOperationTextColor; }
      set
      {
        if (mOperationTextColor != value)
        {
          mOperationTextColor = value;
          RaisePropertyChanged();
        }
      }
    }

    #endregion
  }
}
