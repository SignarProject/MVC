create table Users
(
[UserID] INT IDENTITY(1,1) not null PRIMARY KEY,
[Name] NVARCHAR(35) not null, --http://stackoverflow.com/questions/30485/what-is-a-reasonable-length-limit-on-person-name-fields
[Surname] NVARCHAR(35) not null,
[Email] NVARCHAR(254) not null, --RFC standart
[Login] NVARCHAR(254) not null, --RFC standart
[Password] NVARCHAR(254) not null, --RFC standart
[IsAdmin] BIT not null,
[Version] ROWVERSION,
)
go

create table Projects
(
[ProjectID] INT IDENTITY(1,1) not null PRIMARY KEY,
[Name] NVARCHAR(254) not null,
[Prefix] NVARCHAR(10) not null, --prefix must be small
[IsDeleted] BIT not null,
[Version] ROWVERSION
)
go

create table Bugs
(
[BugID] INT IDENTITY(1,1) not null PRIMARY KEY,
[Subject] NVARCHAR(254) not null, --brief description/title
[Description] NVARCHAR(MAX) not null, --can be as large as possible
[AssigneeID] INT FOREIGN KEY references Users(UserID),
[ProjectID] INT not null FOREIGN KEY references Projects(ProjectID),
[CreationDate] DATETIME not null,
[ModificationDate] DATETIME not null,
[BugStatus] TINYINT not null,
[Priority] TINYINT not null,
[Version] ROWVERSION
)
go

create table Comments
(
[CommentID] INT IDENTITY(1,1) not null PRIMARY KEY,
[Text] NVARCHAR(MAX), --can be as large as possible, and if it's large there will be an expand button
[CreationDate] DATETIME not null,
[ModificationDate] DATETIME,
[UserID] INT FOREIGN KEY references Users(UserID),
[BugID] INT not null FOREIGN KEY references Bugs(BugID),
[Version] ROWVERSION
)

go

create table Attachments
(
[AttachmentID] INT IDENTITY(1,1) not null PRIMARY KEY,
[BugID] INT not null FOREIGN KEY references Bugs(BugID),
[Name] NVARCHAR(255) not null, --is common for many OS, --http://serverfault.com/questions/9546/filename-length-limits-on-linux
[ContentPath] NVARCHAR(MAX), --http://stackoverflow.com/questions/4377740/database-design-preferred-field-length-for-file-paths
[Version] ROWVERSION
)

go

create table UsersToProjects
(
[UserID] INT not null FOREIGN KEY references Users(UserID),
[ProjectID] INT FOREIGN KEY (ProjectID) references Projects(ProjectID),
CONSTRAINT UserProject PRIMARY KEY NONCLUSTERED ([UserID], [ProjectID]),
[Version] ROWVERSION
)
go

create table Filters
(
[FilterID] INT IDENTITY(1,1) not null PRIMARY KEY,
[UserID] INT not null FOREIGN KEY references Users(UserID),
[Title] NVARCHAR(254),
[FilterContent] NVARCHAR(MAX), --xml representation of serialized class, length is not too much but we don't know it beforehand. Anyway MAX will not do any harm
[Version] ROWVERSION
)
go