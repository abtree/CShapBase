using System;
using System.Transactions;
using System.Threading;
using System.Threading.Tasks;

namespace DependentTransaction_
{
    class DependentTransaction_
    {
        public void DisplayTransactionInfo(string title, TransactionInformation ti)
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

        public void TxTask(object obj)
        {
            var tx = obj as DependentTransaction;
            DisplayTransactionInfo("Dependent Transaction", tx.TransactionInformation);

            Thread.Sleep(3000);

            tx.Complete();  //必须每个依赖事务完成 

            DisplayTransactionInfo("Dependent Transaction Complete", tx.TransactionInformation);
        }

        public void DependentTransactionHandle()
        {
            var tx = new CommittableTransaction();
            DisplayTransactionInfo("Root TX Created", tx.TransactionInformation);

            try
            {
                Task.Factory.StartNew(TxTask, tx.DependentClone(DependentCloneOption.BlockCommitUntilComplete));

                tx.Commit();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                tx.Rollback();
            }
            DisplayTransactionInfo("Root TX completed", tx.TransactionInformation);
        }
    }
}
