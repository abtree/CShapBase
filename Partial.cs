//BigClassPart1.cs
[CustomAttribute]
partial class TheBigClass : TheBigBaseClass, IBigClass
{
	public void method1(){

	}
}

//BigClassPart2.cs
[AnotherAttribute]
partial class TheBigClass : IOtherClass
{
	public void method2(){

	}
}

//编译后 会生成如下类（将两部分合并）
[CustomAttribute][AnotherAttribute]
class TheBigClass : TheBigBaseClass, IBigClass, IOtherClass
{
	public void method1(){

	}

	public void method2(){

	}
}