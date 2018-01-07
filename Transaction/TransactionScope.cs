using System;
using System.Transactions; 
using System.Threading.Tasks;

namespace TransactionScope_
{
    class TransactionScope_
    {
        public static void DisplayTransactionInfo(string title, TransactionInformation ti)
        {
            if (ti == null)
            {
                Console.WriteLine("{0} TransactionInformation is null", title);
                return;
            }

            Console.WriteLine(title);
            Console.WriteLine("Creation Time: {0:T}", ti.CreationTime);
            Console.WriteLine("Status: {0}", ti.Status);
            Console.WriteLine("Local ID: {0}", ti.LocalIdentifier);
            Console.WriteLine("Distributed ID: {0}", ti.DistributedIdentifier);
            Console.WriteLine();
        }

        static void OnTransactionCompleted(Object sender, TransactionEventArgs e)
        {
            DisplayTransactionInfo("Tx Completed", e.Transaction.TransactionInformation);
        }

        #region /* 单线程的环境事务 可以使用一个 也可以使用多个 */
        static void TransactionScopeSingle()
        {
            using(var scope = new TransactionScope())
            {
                Transaction.Current.TransactionCompleted += OnTransactionCompleted;

                DisplayTransactionInfo("Ambient TX created", Transaction.Current.TransactionInformation);

                //doing sth

                //这里申明 如果外层有环境事务 就用外层的 如果没有 就新建一个
                using (var scope2 = new TransactionScope(TransactionScopeOption.Required))
                {
                    Transaction.Current.TransactionCompleted += OnTransactionCompleted;

                    DisplayTransactionInfo("Inner Transaction Scope", Transaction.Current.TransactionInformation);

                    //doing sth
                    scope2.Complete();
                }

                scope.Complete();
            }
        }
        #endregion

        #region /* 多线程的环境事务 要使用同一个 需要建立依赖 */
        static void TransactionScopeMulti()
        {
            try
            {
                var options = new TransactionOptions
                {
                    IsolationLevel = IsolationLevel.ReadUncommitted,  //读取不加锁
                    Timeout = TimeSpan.FromSeconds(90)
                };
                using(var scope = new TransactionScope(TransactionScopeOption.Required, options))
                {
                    Transaction.Current.TransactionCompleted += OnTransactionCompleted;

                    DisplayTransactionInfo("Main thread tx", Transaction.Current.TransactionInformation);

                    Task.Factory.StartNew(TaskMethod, Transaction.Current.DependentClone(DependentCloneOption.BlockCommitUntilComplete));

                    //doing sth

                    scope.Complete();
                }
            }catch(TransactionAbortedException ex)
            {
                Console.WriteLine("Main Transaction was aborted " + ex.Message);
            }
        }

        static void TaskMethod(object dependentTx)
        {
            var dTx = dependentTx as DependentTransaction;

            try
            {
                Transaction.Current = dTx;

                //这里使用的是主线程的环境事务
                using(var scope = new TransactionScope())
                {
                    Transaction.Current.TransactionCompleted += OnTransactionCompleted;

                    DisplayTransactionInfo("Task Tx", Transaction.Current.TransactionInformation);

                    scope.Complete();
                }
            }catch(TransactionAbortedException ex)
            {
                Console.WriteLine("Task Method Transaction was aborted, {0}", ex.Message);
            }
            finally
            {
                if (dTx != null)
                    dTx.Complete();
            }
        }
        #endregion

        static void Main(string[] args)
        {
        }
    }
}
