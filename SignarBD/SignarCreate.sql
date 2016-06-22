create table Users
(
[UserID] int IDENTITY(1,1) not null PRIMARY KEY,
[Name] nvarchar(MAX) not null,
[Surname] nvarchar(MAX) not null,
[Login] nvarchar(MAX) not null,
[Password] nvarchar(MAX) not null,
[IsAdmin] bit not null,
[Version] rowversion
)
go

create table Projects
(
[ProjectID] int IDENTITY(1,1) not null PRIMARY KEY,
[Name] nvarchar(MAX) not null,
[Prefix] nvarchar(MAX) not null,
[IsDeleted] bit not null,
[Version] rowversion
)
go

create table Bugs
(
[BugID] int IDENTITY(1,1) not null PRIMARY KEY,
[Subject] nvarchar(MAX) not null,
[Description] nvarchar(MAX) not null,
[AssigneeID] int FOREIGN KEY references Users(UserID),
[ProjectID] int not null FOREIGN KEY references Projects(ProjectID),
[CreationDate] datetime not null,
[ModificationDate] datetime not null,
[BugStatus] tinyint not null,
[Priority] tinyint not null,
[AttachmentsPath] nvarchar(MAX),
[Version] rowversion
)
go


create table UsersToProjects
(
[ID] int IDENTITY(1,1) not null PRIMARY KEY,
[UserID] int not null FOREIGN KEY references Users(UserID),
[ProjectID] int FOREIGN KEY (ProjectID) references Projects(ProjectID),
[Version] rowversion
)
go

create table Filters
(
[FilterID] int IDENTITY(1,1) not null PRIMARY KEY,
[UserID] int not null FOREIGN KEY references Users(UserID),
[Title] nvarchar(MAX),
[FilterContent] nvarchar(MAX)
)
go
