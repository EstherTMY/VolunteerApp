﻿using VolunteerApp.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.Data.Xml.Dom;
using System.Xml.Linq;
using System.Text;



// “基本页”项模板在 http://go.microsoft.com/fwlink/?LinkID=390556 上有介绍

namespace VolunteerApp
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MatchInformation : Page
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();
        private string fileName = "ms-appx:///Assets/testfile1.txt";
        HyperlinkButton[] students = new HyperlinkButton[100];
        string tutorusername;

        public MatchInformation()
        {
            this.InitializeComponent();
            
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
            this.navigationHelper.SaveState += this.NavigationHelper_SaveState;

            
        }
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedTo(e);
            //StorageFolder storage = ApplicationData.Current.LocalFolder;
            //StorageFile file = await storage.GetFileAsync("ms - appx:///Assets/information.txt");
            
            children child = new children { };
            tutorusername = e.Parameter.ToString();
            name.Text = tutorusername;
            try {
                List<string> studentsName = await App.MobileService.GetTable<corresponding>()
       .Where(corresponding => corresponding.tutorusername == tutorusername)
       .Select(corresponding => corresponding.studentusername)
       .ToListAsync();

                for (int i = 0; i < studentsName.Count(); i++)
                {
                    students[i] = new HyperlinkButton();
                    students[i].Content = studentsName[i] + "";
                    students[i].FontSize = 40;
                    canvas.Children.Add(students[i]);
                    Canvas.SetTop(students[i], i * 80);
                    students[i].Click += studensButton_Click;
                }

                //String itemName = file.DisplayName;
                //try {
                //    string content = await ReadFile();
                //    if (content.Equals("暂无信息"))
                //    {
                //        matchButton.Content = "开始配对";
                //        textBlock.Text = content;
                //    }
                //    else {
                //        char[] separator = { '\n' };
                //        String[] splitStrings = new String[100];
                //        StringBuilder theFollowWords = new StringBuilder();
                //        splitStrings = content.Split(separator);
                //        for (int i = 0; i < splitStrings.Length; i++)
                //        {
                //            if (splitStrings[i].StartsWith("姓名"))
                //            {
                //                //name.Text = splitStrings[i].Remove(0, 3);
                //            }
                //            else
                //            {
                //                theFollowWords.Append(splitStrings[i]);
                //            }
                //        }
                //        textBlock.Text = theFollowWords.ToString();
                //        matchButton.Content = "重新配对";
                //    }
                //}
                //catch
                //{
                //    name.Text = "暂无信息";
                //}
            }
            catch
            {
                TextBlock text = new TextBlock();
                text.Text = "无法获取信息";
                canvas.Children.Add(text);
            }



        }

        private void studensButton_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < students.Length; i++)
            {
                if (e.OriginalSource.Equals(students[i]))
                {
                    string studentsName = (String)students[i].Content;
                    NavigateContext1 na1 = new NavigateContext1(studentsName, tutorusername);
                    this.Frame.Navigate(typeof(StudentGrade), na1);
                    //throw new NotImplementedException();
                }
            }

        }

        public async Task<string> ReadFile()
        {
            string text;
            try {
                string fileContent;
                StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Assets/testfile1.txt"));
                using (StreamReader sRead = new StreamReader(await file.OpenStreamForReadAsync()))
                    fileContent = await sRead.ReadToEndAsync();
                    text = fileContent;
            }
            catch(Exception e) {
                name.Text = "暂无信息";
                text = "暂无信息";
            }
            return text;
        }
        /// <summary>
        /// 获取与此 <see cref="Page"/> 关联的 <see cref="NavigationHelper"/>。
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }

        /// <summary>
        /// 获取此 <see cref="Page"/> 的视图模型。
        /// 可将其更改为强类型视图模型。
        /// </summary>
        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }

        /// <summary>
        /// 使用在导航过程中传递的内容填充页。  在从以前的会话
        /// 重新创建页时，也会提供任何已保存状态。
        /// </summary>
        /// <param name="sender">
        /// 事件的来源；通常为 <see cref="NavigationHelper"/>
        /// </param>
        /// <param name="e">事件数据，其中既提供在最初请求此页时传递给
        /// <see cref="Frame.Navigate(Type, Object)"/> 的导航参数，又提供
        /// 此页在以前会话期间保留的状态的
        /// 字典。首次访问页面时，该状态将为 null。</param>
        private void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
        }

        /// <summary>
        /// 保留与此页关联的状态，以防挂起应用程序或
        /// 从导航缓存中放弃此页。值必须符合
        /// <see cref="SuspensionManager.SessionState"/> 的序列化要求。
        /// </summary>
        /// <param name="sender">事件的来源；通常为 <see cref="NavigationHelper"/></param>
        ///<param name="e">提供要使用可序列化状态填充的空字典
        ///的事件数据。</param>
        private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
        }

        #region NavigationHelper 注册

        /// <summary>
        /// 此部分中提供的方法只是用于使
        /// NavigationHelper 可响应页面的导航方法。
        /// <para>
        /// 应将页面特有的逻辑放入用于
        /// <see cref="NavigationHelper.LoadState"/>
        /// 和 <see cref="NavigationHelper.SaveState"/> 的事件处理程序中。
        /// 除了在会话期间保留的页面状态之外
        /// LoadState 方法中还提供导航参数。
        /// </para>
        /// </summary>
        /// <param name="e">提供导航方法数据和
        /// 无法取消导航请求的事件处理程序。</param>
        //protected override void OnNavigatedTo(NavigationEventArgs e)
        //{
        //    this.navigationHelper.OnNavigatedTo(e);
        //}

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedFrom(e);
        }

        #endregion
        

        private void button_Click(object sender, RoutedEventArgs e)
        {
            //ReadFile();
            
            //XmlDocument doc = await XmlDocument.LoadFromUriAsync(new Uri("ms-appx:///Assets/information.xml"));
            //textBlock.Text = doc.DocumentElement.Attributes.GetNamedItem("age").NodeValue.ToString();
        }

        private void matchButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(SelectStudents),tutorusername);
        }
    }
    public class NavigateContext1
    {
        public string childrenName { get; set; }
        public string tutorusername { get; set; }
        public NavigateContext1(string childrenName, string tutorusername)
        {
            this.childrenName = childrenName;
            this.tutorusername = tutorusername;
        }
    }
}

