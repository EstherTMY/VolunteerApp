using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using VolunteerApp.Common;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkID=390556 上有介绍

namespace VolunteerApp
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class Questions : Page
    {
        string questionNumber;
        StorageFile imgFile;
        ApplicationData appData;
        string fileName;
        string tutorusername;
        string qestiontype;
        string studentid;

        private NavigationHelper navigationHelper;
        public Questions()
        {
            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);
        }
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }
        /// <summary>
        /// 在此页将要在 Frame 中显示时进行调用。
        /// </summary>
        /// <param name="e">描述如何访问此页的事件数据。
        /// 此参数通常用于配置页。</param>
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedTo(e);
            try {
                var na2 = (NavigateContext2)e.Parameter;
                questionNumber = na2.number;
                tutorusername = na2.tutorusername;
                studentid = na2.studentid;
                qestiontype = na2.questiontype;
                fileName = (await App.MobileService.GetTable<ansrecord>()
       .Where(ansrecord => ansrecord.questionid == questionNumber)
       .Select(ansrecord => ansrecord.filename)
       .ToEnumerableAsync()).FirstOrDefault();
                //         qestiontype = (await App.MobileService.GetTable<questions>()
                //.Where(questions => questions.filename == fileName)
                //.Select(questions => questions.type)
                //.ToEnumerableAsync()).FirstOrDefault();
                questionNumber.TrimEnd();
                textBlock.Text = questionNumber;

                string path = "ms-appx:///Assets/pic/" + fileName + ".jpg";
                appData = ApplicationData.Current;
                imgFile = await StorageFile.GetFileFromApplicationUriAsync(new Uri(path));
                //await imgFile.CopyAsync(appData.TemporaryFolder,imgFile.Name,NameCollisionOption.ReplaceExisting);
                image.Source = new BitmapImage(new Uri("ms-appx:///Assets/pic/" + fileName + ".jpg"));
            }
            catch
            {
                textBlock.Text = "无法获取信息";

            }
        }

        private void similarQuestion_Click(object sender, RoutedEventArgs e)
        {
            NavigateContext3 na3 = new NavigateContext3(fileName, qestiontype, tutorusername,studentid);
            this.Frame.Navigate(typeof(SimilarQuestion),na3);
        }

        private async void doItAgain_Click(object sender, RoutedEventArgs e)
        {
            await imgFile.CopyAsync(appData.LocalFolder, imgFile.Name, NameCollisionOption.ReplaceExisting);
            try {
                //StorageFolder storageFloder = ApplicationData.Current.LocalFolder;
                //StorageFile file = await storageFloder.GetFileAsync(imgFile.Name);
                assignquestion assignQuestion = new assignquestion { filename = fileName, tutor = tutorusername ,acceptchild = studentid};
                await App.MobileService.GetTable<assignquestion>().InsertAsync(assignQuestion);
                await new MessageDialog("成功提交申请").ShowAsync();
            }
            catch
            {
                await new MessageDialog("提交申请失败").ShowAsync();
            }
            
        }
    }
    public class NavigateContext3
    {
        public string number { get; set; }
        public string questiontype { get; set; }
        public string tutorusername { get; set; }
        public string studentid { get; set; }
        public NavigateContext3(string number, string questiontype, string tutorusername, string studentid)
        {
            this.number = number;
            this.questiontype = questiontype;
            this.tutorusername = tutorusername;
            this.studentid = studentid;
        }
    }
}

