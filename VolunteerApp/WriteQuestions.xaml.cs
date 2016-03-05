using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using VolunteerApp.Common;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkID=390556 上有介绍

namespace VolunteerApp
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class WriteQuestions : Page
    {
        private NavigationHelper navigationHelper;

        public WriteQuestions()
        {
            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);
        }
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }
        public void ChooseIMG()
        {
            FileOpenPicker picker = new FileOpenPicker();
            picker.FileTypeFilter.Add(".png");
            picker.FileTypeFilter.Add(".jpg");

            picker.ContinuationData["DATA"] = "MYDATA";  // The app may be closed while displaying the picker dialog, and this data will be passed back to the application on activation.

            picker.PickSingleFileAndContinue();
        }
        /// <summary>
        /// 在此页将要在 Frame 中显示时进行调用。
        /// </summary>
        /// <param name="e">描述如何访问此页的事件数据。
        /// 此参数通常用于配置页。</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            //this.OnNavigatedTo(e);
            this.navigationHelper.OnNavigatedTo(e);
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            ChooseIMG();
        }

        private async void submitButton_Click(object sender, RoutedEventArgs e)
        {
            if (questionText.Text !="")
            {
               await WriteFile(questionText.Text);
                this.Frame.Navigate(typeof(previewQuestions));
            }
        }
        public async Task WriteFile(string content)
        {
           
                IStorageFolder applicationFolder = ApplicationData.Current.LocalFolder;
                IStorageFile file = await applicationFolder.CreateFileAsync("test.txt",CreationCollisionOption.OpenIfExists);
                await FileIO.WriteTextAsync(file,content);
        }
    }
}
