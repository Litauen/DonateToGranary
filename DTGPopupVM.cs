using TaleWorlds.Library;

namespace DonateToGranary
{

    public class DTGPopupVM : ViewModel
    {
        private string _title;
        private string _smallText;
        private string _bigText;
        private string _textOverImage;
        private string _spriteName;
        private string _closeButtonText;

        public DTGPopupVM(string title, string smallText, string bigText, string textOverImage, string spriteName, string closeButtonText) 
        {
            _title = title;
            _smallText = smallText;
            _bigText = bigText;
            _textOverImage = textOverImage;
            _spriteName = spriteName;
            _closeButtonText = closeButtonText;
        }

        public void Close()
        {
            DonateFoodBaseBehavior.DeletePopupVMLayer();
        }

        public void Refresh()
        {
            this.Title = _title;
            this.SmallText = _smallText;
            this.BigText = _bigText;
            this.TextOverImage = _textOverImage;
            this.SpriteName = _spriteName;
            this.CloseButtonText = _closeButtonText;
        }


        public string Title
        {
            get
            {
                return this._title;
            }
            set
            {
                this._title = value;
                base.OnPropertyChangedWithValue(value, "PopupTitle");
            }
        }

        public string SmallText
        {
            get
            {
                return this._smallText;
            }
            set
            {
                this._smallText = value;
                base.OnPropertyChangedWithValue(value, "PopupSmallText");
            }
        }

        public string BigText
        {
            get
            {
                return this._bigText;
            }
            set
            {
                this._bigText = value;
                base.OnPropertyChangedWithValue(value, "PopupBigText");
            }
        }

        public string TextOverImage
        {
            get
            {
                return this._textOverImage;
            }
            set
            {
                this._textOverImage = value;
                base.OnPropertyChangedWithValue(value, "PopupTextOverImage");
            }
        }

        public string SpriteName
        {
            get
            {
                return this._spriteName;
            }
            set
            {
                this._spriteName = value;
                base.OnPropertyChangedWithValue(value, "SpriteName");
            }
        }

        public string CloseButtonText
        {
            get
            {
                return this._closeButtonText;
            }
            set
            {
                this._closeButtonText = value;
                base.OnPropertyChangedWithValue(value, "CloseButtonText");
            }
        }

    }
}