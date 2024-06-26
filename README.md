Setup project guides :
1. Clone this project to your local computer
```
git clone https://github.com/darwishub/speakingrosestest.git
```
2. Make a new DB for this project on your LocalDB
\
![alt text](https://i.imgur.com/gjZug9U.png)
\
3. Enter a DB name, in this case the name is TaskDB_by_darwis, then OK.
\
![alt text](https://i.imgur.com/lNz8Dz7.png)
\
4. Add new query to the new DB
\
![alt text](https://i.imgur.com/Qb3DPFK.png)
You can grab the query here or from sql.txt
```
/****** Object:  Table [dbo].[Tasks]    Script Date: 6/26/2024 5:02:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tasks](
	[TaskId] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](250) NULL,
	[Description] [nvarchar](500) NULL,
	[Priority] [tinyint] NOT NULL,
	[DueDate] [datetime] NOT NULL,
	[Status] [bit] NULL,
 CONSTRAINT [PK_Tasks] PRIMARY KEY CLUSTERED 
(
	[TaskId] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Tasks] ADD  CONSTRAINT [DF_Tasks_Priority]  DEFAULT ((1)) FOR [Priority]
GO
ALTER TABLE [dbo].[Tasks] ADD  CONSTRAINT [DF_Tasks_DueDate]  DEFAULT (getdate()) FOR [DueDate]
GO
ALTER TABLE [dbo].[Tasks] ADD  CONSTRAINT [DF_Tasks_Status]  DEFAULT ((0)) FOR [Status]
GO

```
5. Now the table has been added to the new DB
\
![alt text](https://i.imgur.com/8jmZhHn.png)
\
6. Now change the DefaultConnection on the appsettings.json based on your instance and DB name
\
![alt text](https://i.imgur.com/3fYs4r0.png)
7. Open terminal and run this command
```
cd .\speakingroses\
dotnet run / dotnet watch run
```
![alt text](https://i.imgur.com/A4mr2SI.png)
\
8. Yuhuuuuu!
![alt text](https://i.imgur.com/LdI1VwU.png)

Run Unit Test:
1. Open terminal and run this command
```
cd .\speakingroses.test\
dotnet test
```
![alt text](https://i.imgur.com/LO4fEcw.png)
