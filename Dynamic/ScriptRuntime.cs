using System;

namespace ScriptRuntime_
{
    class ScriptRuntime_
    {
        /*
         * 运行此程序需要配置环境 详情网上查询
         */
        internal void method1()
        {
            string scriptToUse = "AmountDisc.py";  //这里以python为例
            ScriptRuntime scriptRuntime = ScriptRuntime.CreateFromConfiguration();
            ScriptEngine pythEng = scriptRuntime.GetEngine("Python");
            ScriptSource source = pythEng.CreateScriptSourceFromFile(scriptToUse);
            ScriptScope scope = pythEng.CreateScope();
            scope.SetVariable("prodCount", 123);
            scope.SetVariable("amt", 124m);
            source.Execute(scope);
            var ret = scope.GetVariable("retAmt").ToString();
        }

        internal void method2()
        {
            ScriptRuntime scriptRuntime = ScriptRuntime.CreateFromConfiguration();
            dynamic ccalcRate = scriptRuntime.UseFile("AmountDisc.py");
            ccalcRate.CalcTax("124m");  //直接调用脚本的函数
        }
    }
}
