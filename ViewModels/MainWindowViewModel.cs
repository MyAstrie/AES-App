using System;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Input;
using AES.Models;
using Prism.Commands;
using Prism.Mvvm;

namespace AES.ViewModels
{
    internal class MainWindowViewModel : BindableBase
    {
        private readonly MyAES _myAes = new();

        public string GenKeyText { get; set; } = "Генерация \n    ключа";

        #region EncryptionKeyValue

        private string? _encryptionKeyValue;

        public string? EncryptionKeyValue
        {
            get => _encryptionKeyValue;
            set => SetProperty(ref _encryptionKeyValue, value);
        }

        #endregion

        #region DecryptionKeyValue

        private string? _decryptionKeyValue;

        public string? DecryptionKeyValue
        {
            get => _decryptionKeyValue;
            set => SetProperty(ref _decryptionKeyValue, value);
        }

        #endregion

        #region PlainText

        private string? _plainText;

        public string? PlainText
        {
            get => _plainText;
            set => SetProperty(ref _plainText, value);
        } 
        
        #endregion

        #region PlainKey

        private string? _plainKey;

        public string? PlainKey
        {
            get => _plainKey;
            set => SetProperty(ref _plainKey, value);
        } 
        
        #endregion

        #region EncryptedText
        
        private string? _encryptedText;

        public string? EncryptedText
        {
            get => _encryptedText;
            set => SetProperty(ref _encryptedText, value);
        } 
        
        #endregion

        #region DecryptedText
        
        private string? _decryptedText;

        public string? DecryptedText
        {
            get => _decryptedText;
            set => SetProperty(ref _decryptedText, value);
        }

        #endregion

        #region ByteCode

        private string? _byteCode;

        public string? ByteCode
        {
            get => _byteCode;
            set => SetProperty(ref _byteCode, value);
        }

        #endregion

        #region EncryptCommand

        public ICommand EncryptCommand => new DelegateCommand(() =>
        {
            if (!String.IsNullOrEmpty(EncryptionKeyValue) && EncryptionKeyValue.Length != 16)
                MessageBox.Show("Ключ шифрования должен содержать 16 символов");

            else if (!String.IsNullOrEmpty(PlainText) && !String.IsNullOrEmpty(EncryptionKeyValue))
                EncryptedText = _myAes.Encrypt(PlainText, EncryptionKeyValue);
        });

        #endregion

        #region DecryptCommand

        public ICommand DecryptCommand => new DelegateCommand(() =>
        {
            if (!String.IsNullOrEmpty(DecryptionKeyValue) && DecryptionKeyValue.Length != 16)
                MessageBox.Show("Ключ расшифров должен содержать 16 символов");

            else if (!String.IsNullOrEmpty(PlainKey) && !String.IsNullOrEmpty(DecryptionKeyValue))
                DecryptedText = _myAes.Decrypt(PlainKey, DecryptionKeyValue);
        });

        #endregion

        #region GenerateKey

        public ICommand GenKey => new DelegateCommand(() =>
        {
            var random = RandomNumberGenerator.Create();
            var byteArr = new byte[12];
            random.GetBytes(byteArr);
            EncryptionKeyValue = Convert.ToBase64String(byteArr).Replace("=", "");
        });

        #endregion

        #region Double Click Events

        private void GetBytes(string? clickedText)
        {
            if (String.IsNullOrEmpty(clickedText)) return;
            byte[] inputBytes = Encoding.UTF8.GetBytes(clickedText);
            ByteCode = BitConverter.ToString(inputBytes).ToLower().Replace("-", " ");
        }

        public ICommand GetByteCodeForEncryptionKeyValue => new DelegateCommand(() =>
        {
            GetBytes(EncryptionKeyValue);
        });

        public ICommand GetByteCodeForDecryptionKeyValue => new DelegateCommand(() =>
        {
            GetBytes(DecryptionKeyValue);
        });

        public ICommand GetByteCodeForPlainText => new DelegateCommand(() =>
        {
            GetBytes(PlainText);
        });

        public ICommand GetByteCodeForPlainKey => new DelegateCommand(() =>
        {
            GetBytes(PlainKey);
        });

        public ICommand GetByteCodeForEncryptedText => new DelegateCommand(() =>
        {
            GetBytes(EncryptedText);
        });

        public ICommand GetByteCodeForDecryptedText => new DelegateCommand(() =>
        {
            GetBytes(DecryptedText);
        });

        #endregion
    }
}
