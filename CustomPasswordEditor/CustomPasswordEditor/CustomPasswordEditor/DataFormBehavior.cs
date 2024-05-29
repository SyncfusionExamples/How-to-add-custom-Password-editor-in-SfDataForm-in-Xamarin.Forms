using Syncfusion.XForms.DataForm;
using Syncfusion.XForms.DataForm.Editors;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace CustomPasswordEditor
{
   public class DataFormBehavior : Behavior<SfDataForm>
    {
        SfDataForm dataForm;
        protected override void OnAttachedTo(SfDataForm bindable)
        {
            base.OnAttachedTo(bindable);

            dataForm = bindable as SfDataForm;
            if (Device.RuntimePlatform == Device.UWP)
            {
                dataForm.RegisterEditor("PasswordEditor", new PasswordEditor(dataForm));
                dataForm.RegisterEditor("Password", "PasswordEditor");
            }
            dataForm.DataObject = new Company();
            dataForm.ValidationMode = ValidationMode.LostFocus;
        }
    }

 
    #region CustomPasswordEntry
    public class PasswordEditor : DataFormEditor<CustomPasswordEntry>
    {
        public PasswordEditor(SfDataForm dataForm) : base(dataForm)
        {
        }
        protected override CustomPasswordEntry OnCreateEditorView(DataFormItem dataFormItem)
        {
            return new CustomPasswordEntry();
        }
        protected override void OnInitializeView(DataFormItem dataFormItem, CustomPasswordEntry view)
        {
            view.IsPassword = true;
            view.Placeholder = "Enter value";
            view.PlaceholderColor = Color.White;
        }

        protected override void OnWireEvents(CustomPasswordEntry view)
        {
           view.TextChanged += View_TextChanged;
           view.Unfocused += View_Unfocused;
        }

        protected override bool OnValidateValue(CustomPasswordEntry view)
        {
            return DataForm.Validate("Password");
        }
        private void View_Unfocused(object sender, FocusEventArgs e)
        {
            OnValidateValue(sender as CustomPasswordEntry);
        }

        private void View_TextChanged(object sender, TextChangedEventArgs e)
        {
            OnCommitValue(sender as CustomPasswordEntry);
        }
        protected override void OnCommitValue(CustomPasswordEntry view)
        {
            var dataFormItemView = view.Parent as DataFormItemView;
            var textValue = view.Text;
            this.DataForm.ItemManager.SetValue(dataFormItemView.DataFormItem, view.Text);
        }
    }
    public class CustomPasswordEntry : Entry
    {
        public CustomPasswordEntry()
        {

        }
    }
    #endregion

}
