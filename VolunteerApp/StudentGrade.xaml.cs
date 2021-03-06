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

// “基本页”项模板在 http://go.microsoft.com/fwlink/?LinkID=390556 上有介绍

namespace VolunteerApp
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class StudentGrade : Page
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();
        string studentID;

        string studentname;
        
        string tutorusername;

        public StudentGrade()
        {
            this.InitializeComponent();

            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
            this.navigationHelper.SaveState += this.NavigationHelper_SaveState;
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
            try {
                int totalQuestionNumber = 0;
                int rightQuestionNumber = 0;
                int wrongQuestionNumber = 0;
                var na1 = (NavigateContext1)e.Parameter;
                studentname = na1.childrenName;
                studentName.Text = na1.childrenName;
                tutorusername = na1.tutorusername;
                studentID = (await App.MobileService.GetTable<children>()
       .Where(children => children.username == studentname)
       .Select(children => children.Id)
       .ToEnumerableAsync()).FirstOrDefault();
                List<bool> totalQuestions = await App.MobileService.GetTable<ansrecord>()
       .Where(ansrecord => ansrecord.studentid == studentID)
       .Select(ansrecord => ansrecord.trueorfalse)
       .ToListAsync();
                totalQuestionNumber = totalQuestions.Count();

                for (int i = 0; i < totalQuestionNumber; i++)
                {
                    if (totalQuestions[i] == true)
                    {
                        rightQuestionNumber++;
                    }
                    else
                    {
                        wrongQuestionNumber++;
                    }
                }
                double accuracy = (rightQuestionNumber * 100 / totalQuestionNumber);
                HyperlinkButton mentalArithmetic = new HyperlinkButton();
                mentalArithmetic.FontSize = 40;
                //mentalArithmetic.TextWrapping = "Wrap";
                mentalArithmetic.Content = "口算题目—\n已经做过：" + totalQuestionNumber + "道,\n" + "正确率：" + accuracy + "%";
                canvas.Children.Add(mentalArithmetic);
                Grid.SetRow(mentalArithmetic, 1);
                mentalArithmetic.Click += textButton_Click;
            }
            catch
            {
                TextBlock textWrong = new TextBlock();
                textWrong.Text = "未做过题目";
                textWrong.FontSize = 40;
                canvas.Children.Add(textWrong);
            }
        }

        private void textButton_Click(object sender, RoutedEventArgs e)
        {
            string questionType = "口算题目";
            NavigateContext na = new NavigateContext(studentID, questionType,studentname,tutorusername);
            
            this.Frame.Navigate(typeof(GradesInformation), na);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedFrom(e);
        }

        #endregion
    }
    public class NavigateContext
    {
        public string studentid { get; set; }
        public string questiontype { get; set; }
        public string studentName { get; set; }
        public string tutorusername { get; set; }
        public NavigateContext(string studentid, string questiontype, string studentName,string tutorusername)
        {
            this.questiontype = questiontype;
            this.studentid = studentid;
            this.studentName = studentName;
            this.tutorusername = tutorusername;
        }
    }
}

