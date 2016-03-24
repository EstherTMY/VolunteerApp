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
    public sealed partial class SimilarQuestion : Page
    {
        StorageFile imgFile;
        ApplicationData appData;
        private NavigationHelper navigationHelper;
        string similarNumber;
        public SimilarQuestion()
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
            similarNumber = e.Parameter.ToString();
            textBlock.Text = "Similar to "+similarNumber;
            string path = "ms-appx:///Assets/" + similarNumber + ".jpg";
            appData = ApplicationData.Current;
            imgFile = await StorageFile.GetFileFromApplicationUriAsync(new Uri(path));
            //await imgFile.CopyAsync(appData.TemporaryFolder,imgFile.Name,NameCollisionOption.ReplaceExisting);
            image.Source = new BitmapImage(new Uri("ms-appx:///Assets/SimilarQuestions/" + similarNumber + ".jpg"));
        }

        private async void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
            await imgFile.CopyAsync(appData.LocalFolder, imgFile.Name, NameCollisionOption.ReplaceExisting);
            try
            {
                StorageFolder storageFloder = ApplicationData.Current.LocalFolder;
                StorageFile file = await storageFloder.GetFileAsync(imgFile.Name);
                questions similarQuestion = new questions { filename = similarNumber};
                await App.MobileService.GetTable<questions>().InsertAsync(similarQuestion);

                await new MessageDialog("成功提交申请").ShowAsync();

            }
            catch
            {
                await new MessageDialog("提交申请失败").ShowAsync();
            }

        }
    }
}

