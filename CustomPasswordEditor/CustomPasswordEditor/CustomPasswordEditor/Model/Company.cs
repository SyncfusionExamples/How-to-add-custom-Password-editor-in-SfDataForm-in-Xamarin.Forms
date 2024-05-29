using Syncfusion.XForms.DataForm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CustomPasswordEditor
{
    public class Company : NotificationObject
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Name should not be empty")]
        public string Name { get; set; }
       
        public string Addresses { get; set; }
        public string Password { get; set; }
       
        public Company()
        {
        }
    }
    public class NotificationObject : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
