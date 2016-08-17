create table Users
(
[UserID] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
[Name] NVARCHAR(35) NOT NULL, --http://stackoverflow.com/questions/30485/what-is-a-reasonable-length-limit-on-person-name-fields
[Surname] NVARCHAR(35) NOT NULL,
[Email] NVARCHAR(254) NOT NULL, --RFC standart
[AvatarImagePath] NVARCHAR(MAX), --http://stackoverflow.com/questions/4377740/database-design-preferred-field-length-for-file-paths
[Login] NVARCHAR(254) NOT NULL, --RFC standart
[Password] NVARCHAR(254) NOT NULL, --RFC standart
[IsAdmin] BIT NOT NULL,
[Version] ROWVERSION,
)
go

create table Projects
(
[ProjectID] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
[Name] NVARCHAR(254) NOT NULL,
[Prefix] NVARCHAR(10) NOT NULL, --prefix must be small
[IsDeleted] BIT NOT NULL,
[Version] ROWVERSION
)
go

create table Bugs
(
[BugID] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
[Subject] NVARCHAR(254) NOT NULL, --brief description/title
[Description] NVARCHAR(MAX) NOT NULL, --can be as large as possible
[AssigneeID] INT FOREIGN KEY REFERENCES Users(UserID),
[ProjectID] INT NOT NULL FOREIGN KEY REFERENCES Projects(ProjectID),
[CreationDate] DATETIME NOT NULL,
[ModificationDate] DATETIME NOT NULL,
[BugStatus] TINYINT NOT NULL,
[Priority] TINYINT NOT NULL,
[Version] ROWVERSION
)
go

create table Comments
(
[CommentID] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
[Text] NVARCHAR(MAX), --can be as large as possible, and if it's large there will be an expand button
[CreationDate] DATETIME NOT NULL,
[ModificationDate] DATETIME,
[UserID] INT FOREIGN KEY REFERENCES Users(UserID),
[BugID] INT NOT NULL FOREIGN KEY REFERENCES Bugs(BugID),
[Version] ROWVERSION
)

go

create table Attachments
(
[AttachmentID] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
[BugID] INT NOT NULL FOREIGN KEY REFERENCES Bugs(BugID),
[Name] NVARCHAR(255) NOT NULL, --is common for many OS, --http://serverfault.com/questions/9546/filename-length-limits-on-linux
[ContentPath] NVARCHAR(MAX), --http://stackoverflow.com/questions/4377740/database-design-preferred-field-length-for-file-paths
[Version] ROWVERSION
)

go

create table UsersToProjects
(
[UserID] INT NOT NULL FOREIGN KEY REFERENCES Users(UserID),
[ProjectID] INT FOREIGN KEY (ProjectID) REFERENCES Projects(ProjectID),
CONSTRAINT UserProject PRIMARY KEY CLUSTERED ([UserID], [ProjectID]),
[Version] ROWVERSION
)
go

create table Filters
(
[FilterID] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
[UserID] INT NOT NULL FOREIGN KEY REFERENCES Users(UserID),
[Title] NVARCHAR(254),
[FilterContent] NVARCHAR(MAX), --xml representation of serialized class, length is not too much but we don't know it beforehand. Anyway MAX will not do any harm
[Version] ROWVERSION
)
go