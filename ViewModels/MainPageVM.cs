// <copyright file="MainPageVM.cs" company="COMPRION GmbH">
//   Copyright (c) COMPRION GmbH. All rights reserved.
// </copyright>

using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Reflection;

namespace CollectionViewKeepLastItemInView.ViewModels
{
    /// <summary>
    /// </summary>
    internal sealed class MainPageVM : BindableObject
    {
        private readonly Queue<string> itemVmQueue;

        public MainPageVM()
        {
            this.itemVmQueue = new Queue<string>();
            this.AddItemCommand = new Command(this.AddItemCollection);
        }

        /// <summary>
        /// The item source that contains the frames to be displayed.
        /// </summary>
        public ObservableCollection<String> ItemsSource { get; private set; }

        public Command AddItemCommand { get; private set; }

        private void AddItemCollection()
        {
            this.itemVmQueue.Enqueue("Hallo");
            this.itemVmQueue.Enqueue("Dies ist ein Test");

            this.ItemsSource = new ObservableCollection<string>(this.itemVmQueue);
            this.RaisePropertyChanged(() => this.ItemsSource);
        }

        private void RaisePropertyChanged<T>(Expression<Func<T>> property)
        {
            var name = this.GetMemberInfo(property).Name;
            Dispatcher.Dispatch(() => this.OnPropertyChanged(name));
        }

        private MemberInfo GetMemberInfo(Expression expression)
        {
            MemberExpression operand;
            var lambdaExpression = (LambdaExpression)expression;
            if (lambdaExpression.Body is UnaryExpression)
            {
                var body = (UnaryExpression)lambdaExpression.Body;
                operand = (MemberExpression)body.Operand;
            }
            else
            {
                operand = (MemberExpression)lambdaExpression.Body;
            }

            return operand.Member;
        }
    }
}
