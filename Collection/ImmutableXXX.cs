using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace ImmutableXXX
{
    public struct SAccount
    {
        public string Name { get; set; }
        public string Account { get; set; }
    }
    class ImmutableXXX
    {
        List<SAccount> accounts = new List<SAccount>()
        {
            new SAccount {
                Name = "aaa",
                Account = "AAA"
            },
            new SAccount
            {
                Name = "bbb",
                Account = "BBB"
            },
            new SAccount
            {
                Name = "ccc",
                Account = "CCC"
            }
        };
        //需要在NuGet包使用
        ImmutableList<SAccount> immuLists = accounts.ToImmutableList();
        ImmutableList<Account>.Builder builder = immuLists.ToBuilder();
        for(int i = 0; int < builder.Count; ++i){
            SAccount a = builder[i];
         }

        ImmutableList<SAccount> overAccounts = builder.ToImmutable();
        foreach(SAccount item in overAccounts){
            
        }
    }
}
