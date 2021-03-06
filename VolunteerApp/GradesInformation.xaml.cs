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
using System.Text;

// “基本页”项模板在 http://go.microsoft.com/fwlink/?LinkID=390556 上有介绍

namespace VolunteerApp
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class GradesInformation : Page
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();
        String[] QuestionNumbers = new String[100];
        HyperlinkButton[] questionNumber = new HyperlinkButton[100];
        List<string> questions;
        string questionType;
        string tutorusrname;
        string studentid;

        public GradesInformation()
        {
            this.InitializeComponent();

            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
            this.navigationHelper.SaveState += this.NavigationHelper_SaveState;
        }
        public async Task<string> ReadFile()
        {
            string text;
            try
            {
                string fileContent;
                StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Assets/GradesInformation.txt"));
                using (StreamReader sRead = new StreamReader(await file.OpenStreamForReadAsync()))
                    fileContent = await sRead.ReadToEndAsync();
                text = fileContent;
            }
            catch (Exception e)
            {
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

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedTo(e);
            //this.navigationHelper.OnNavigatedTo(e);
            //String content = await ReadFile();
            //char[] separator = { '\n'};
            try {
                var na = (NavigateContext)e.Parameter;
                questionType = na.questiontype;
                tutorusrname = na.tutorusername;
                studentid = na.studentid;
                name.Text = na.studentName;
                type.Text = na.questiontype;
                string studentId = na.studentid;

                questions = await App.MobileService.GetTable<ansrecord>()
       .Where(ansrecord => ansrecord.studentid == studentId)
       .Select(ansrecord => ansrecord.questionid)
       .ToListAsync();
                List<bool> totalQuestions = await App.MobileService.GetTable<ansrecord>()
       .Where(ansrecord => ansrecord.studentid == studentId)
       .Select(ansrecord => ansrecord.trueorfalse)
       .ToListAsync();

                //        int leftSum = 0;
                //        int count = 0;
                //        //String[] splitStrings = new String[100];
                //        ansrecord record = new ansrecord { };
                //        List<string> result = await App.MobileService.GetTable<ansrecord>()
                //.Where(ansrecord => ansrecord.trueorfalse == false)
                //.Select(ansrecord => ansrecord.questionid)
                //.ToListAsync();



                //        //StringBuilder theFollowWords = new StringBuilder();
                //        //splitStrings = content.Split(separator);
                //        //for (int i = 0; i < splitStrings.Length; i++)
                //        //{
                //        //    if (splitStrings[i].StartsWith("姓名"))
                //        //    {
                //        //        name.Text = splitStrings[i].Remove(0, 3);
                //        //    }
                //        //    else
                //        //    {
                //        //        if (splitStrings[i].StartsWith("第")) {
                //        //            theFollowWords.Append(splitStrings[i]);
                //        //        }
                //        //        else
                //        //        {
                //        //            QuestionNumbers = splitStrings[i].Split(' ');

                for (int j = 0; j < questions.Count(); j++)
                {
                    questionNumber[j] = new HyperlinkButton();
                    if (totalQuestions[j] == true) {
                        questionNumber[j].Content = "题号：" + questions[j] + "\t\t√";
                    }
                    else
                    {
                        questionNumber[j].Content = "题号：" + questions[j] + "\t\t×";
                    }

                    questionNumber[j].FontSize = 40;
                    canvas.Children.Add(questionNumber[j]);
                    //if (j % 5 == 0 && j != 0)
                    //{
                    //    leftSum = 0;
                    //    count++;
                    //}
                    //else if (j == 0)
                    //{
                    //    leftSum = 0;
                    //}
                    //else
                    //{
                    //    leftSum = leftSum + 80;
                    //}
                    if (j > 5) {
                        canvas.Height = 428 + 70 * (j - 5);
                    }
                    Canvas.SetTop(questionNumber[j], j * 70);
                    //Canvas.SetLeft(questionNumber[j], leftSum);
                    questionNumber[j].Click += questionButton_Click;
                }
            }
            catch
            {
                TextBlock wrongText = new TextBlock();
                wrongText.Text = "无法获取信息";
                canvas.Children.Add(wrongText);
            }
        }


        private void questionButton_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < questionNumber.Length; i++)
            {
                if (e.OriginalSource.Equals(questionNumber[i]))
                {
                    string number = (String)questions[i];
                    NavigateContext2 na2 = new NavigateContext2(number, questionType, tutorusrname,studentid);
                    this.Frame.Navigate(typeof(Questions), na2);
                    //throw new NotImplementedException();
                }
            }
        }

        //}
        //gradeInformationBlock.Text = theFollowWords.ToString();



        //private void questionButton_Click(object sender, RoutedEventArgs e)
        //{
        //    for (int i=0;i<questionNumber.Length;i++) {
        //        if (e.OriginalSource.Equals(questionNumber[i])) {
        //            string number = (String)questionNumber[i].Content;
        //            this.Frame.Navigate(typeof(Questions), number);
        //            //throw new NotImplementedException();
        //        }
        //    }

        //}

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedFrom(e);
        }

        #endregion
    }

    public class NavigateContext2
    {
        public string number { get; set; }
        public string questiontype { get; set; }
        public string tutorusername { get; set; }
        public string studentid { get; set; }
        public NavigateContext2(string number, string questiontype,string tutorusername, string studentid)
        {
            this.number = number;
            this.questiontype = questiontype;
            this.tutorusername = tutorusername;
            this.studentid = studentid;
        }
    }

}

