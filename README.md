# RelearnMvvm
重学MVVM + IService + 数据库 + 导航 + UnitTest

## 技巧？

数据为有限类型：

1. 写死字符串
2. 或 用枚举

### 版本控制

写到`Properties`里面：

- 初始化时写入当前版本
- 读取存储的版本号，从而知道是否初始化了

### C# Embedded Resources

在 `项目名.文件夹.资源名` 下

就是它的完全路径，包括所有的`Namespace`

例如：

- 名称
    - 项目名：`RelearnMvvm`
    - 文件路径：`Assets/poetrydb.sqlite3`
- 获取的名字：`RelearnMvvm.Assets.poetrydb.sqlite3`

#### 使用Logical Name

`.sln` 下

```xml
<ItemGroup>
	<EmbeddedResource Include="Assets\poetrydb.sqlite3">
        <LogicalName>poetrydb.sqlite3</LogicalName>
    </EmbeddedResource>
</ItemGroup>
```



## 与数据库交互

### Model

| 问题                 | 解决                    |
| -------------------- | ----------------------- |
| 主键                 | [SQLite.PrimaryKey]     |
| 名称不符             | [SQLite.Table("name")]  |
|                      | [SQLite.Column("name")] |
| 计算的成员，不想保存 | [SQLite.Ignore]         |
| 自增                 |                         |
|                      |                         |

## 测试

至少 80% 的代码进行测试

### 断言 `Assert`

能测试的代码

### Mock

安装nuget：Moq

无法测试的代码：创建接口，然后Mock

依赖于类，可能无法测试，依赖于接口，便可测试了。

```c#
var mock = new Mock<T>()
var someObject = mock.Object;
```

#### 是否 传入特定参数，特定次数

```c#
public void Verify(Expression<Action<T>> expression, Times times);
// Times.Never
```

#### 特定返回值

首先Setup，然后Returns

```c#
public ISetup<T, TResult> Setup<TResult>(Expression<Func<T, TResult>> expression);

IReturnsResult<TMock> Returns(TResult value);

```

### 启动、结束 时 运行代码

`[SetUp]`，`[TearDown]`

### 为测试修改原先的设计

有的时候为了测试代码，就是要修改对应的实现。

例如：运行时一般不需要删除数据库的功能，但测试的时候可能需要，所以就要把它删除，所以修改了原本的设计

