using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=391641 上有介绍

namespace VolunteerApp
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
   
    public sealed partial class MainPage : Page
    {
        MatchInformation matchInformation = new MatchInformation();
        string content;
        private string fileName = "testfile1.txt";
        string userName;
        public MainPage()
        {
            this.InitializeComponent();
            
            this.NavigationCacheMode = NavigationCacheMode.Required;
        }

        /// <summary>
        /// 在此页将要在 Frame 中显示时进行调用。
        /// </summary>
        /// <param name="e">描述如何访问此页的事件数据。
        /// 此参数通常用于配置页。</param>
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            // TODO: 准备此处显示的页面。

            // TODO: 如果您的应用程序包含多个页面，请确保
            // 通过注册以下事件来处理硬件“后退”按钮:
            // Windows.Phone.UI.Input.HardwareButtons.BackPressed 事件。
            // 如果使用由某些模板提供的 NavigationHelper，
            // 则系统会为您处理该事件。
            userName = e.Parameter.ToString();
        }
        public async Task<string> ReadFile()
        {
            string text;
            try
            {
                string fileContent;
                StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
                StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appdata:///local/userimformation.txt"));
                using (StreamReader sRead = new StreamReader(await file.OpenStreamForReadAsync()))
                    fileContent = await sRead.ReadToEndAsync();
                text = fileContent;
            }
            catch (Exception e)
            {
                text = e.Message;
            }
            return text;
        }


        private void MatchButton_Click(object sender, RoutedEventArgs e)
        {
                    
            this.Frame.Navigate(typeof(MatchInformation),userName);

        }

        private void WriteQuestionButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(WriteQuestions));
        }

        private void TrainingButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
