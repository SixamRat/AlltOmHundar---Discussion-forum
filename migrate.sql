IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
CREATE TABLE [Categories] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(100) NOT NULL,
    [Description] nvarchar(500) NULL,
    [DisplayOrder] int NOT NULL,
    CONSTRAINT [PK_Categories] PRIMARY KEY ([Id])
);

CREATE TABLE [Users] (
    [Id] int NOT NULL IDENTITY,
    [Username] nvarchar(50) NOT NULL,
    [Email] nvarchar(100) NOT NULL,
    [PasswordHash] nvarchar(max) NOT NULL,
    [ProfileImageUrl] nvarchar(max) NULL,
    [Bio] nvarchar(500) NULL,
    [Role] int NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    [LastLoginAt] datetime2 NULL,
    CONSTRAINT [PK_Users] PRIMARY KEY ([Id])
);

CREATE TABLE [Topics] (
    [Id] int NOT NULL IDENTITY,
    [Title] nvarchar(200) NOT NULL,
    [Description] nvarchar(1000) NULL,
    [CategoryId] int NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    CONSTRAINT [PK_Topics] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Topics_Categories_CategoryId] FOREIGN KEY ([CategoryId]) REFERENCES [Categories] ([Id]) ON DELETE NO ACTION
);

CREATE TABLE [Groups] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(100) NOT NULL,
    [Description] nvarchar(500) NULL,
    [CreatedByUserId] int NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    CONSTRAINT [PK_Groups] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Groups_Users_CreatedByUserId] FOREIGN KEY ([CreatedByUserId]) REFERENCES [Users] ([Id]) ON DELETE NO ACTION
);

CREATE TABLE [PrivateMessages] (
    [Id] int NOT NULL IDENTITY,
    [SenderId] int NOT NULL,
    [ReceiverId] int NOT NULL,
    [Content] nvarchar(max) NOT NULL,
    [SentAt] datetime2 NOT NULL,
    [IsRead] bit NOT NULL,
    [ReadAt] datetime2 NULL,
    CONSTRAINT [PK_PrivateMessages] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_PrivateMessages_Users_ReceiverId] FOREIGN KEY ([ReceiverId]) REFERENCES [Users] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_PrivateMessages_Users_SenderId] FOREIGN KEY ([SenderId]) REFERENCES [Users] ([Id]) ON DELETE NO ACTION
);

CREATE TABLE [Posts] (
    [Id] int NOT NULL IDENTITY,
    [Content] nvarchar(max) NOT NULL,
    [ImageUrl] nvarchar(max) NULL,
    [TopicId] int NOT NULL,
    [UserId] int NOT NULL,
    [ParentPostId] int NULL,
    [CreatedAt] datetime2 NOT NULL,
    [UpdatedAt] datetime2 NULL,
    [IsDeleted] bit NOT NULL,
    CONSTRAINT [PK_Posts] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Posts_Posts_ParentPostId] FOREIGN KEY ([ParentPostId]) REFERENCES [Posts] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Posts_Topics_TopicId] FOREIGN KEY ([TopicId]) REFERENCES [Topics] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Posts_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id]) ON DELETE NO ACTION
);

CREATE TABLE [GroupMembers] (
    [Id] int NOT NULL IDENTITY,
    [GroupId] int NOT NULL,
    [UserId] int NOT NULL,
    [JoinedAt] datetime2 NOT NULL,
    CONSTRAINT [PK_GroupMembers] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_GroupMembers_Groups_GroupId] FOREIGN KEY ([GroupId]) REFERENCES [Groups] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_GroupMembers_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id]) ON DELETE NO ACTION
);

CREATE TABLE [GroupMessages] (
    [Id] int NOT NULL IDENTITY,
    [GroupId] int NOT NULL,
    [SenderId] int NOT NULL,
    [Content] nvarchar(max) NOT NULL,
    [SentAt] datetime2 NOT NULL,
    CONSTRAINT [PK_GroupMessages] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_GroupMessages_Groups_GroupId] FOREIGN KEY ([GroupId]) REFERENCES [Groups] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_GroupMessages_Users_SenderId] FOREIGN KEY ([SenderId]) REFERENCES [Users] ([Id]) ON DELETE NO ACTION
);

CREATE TABLE [Reactions] (
    [Id] int NOT NULL IDENTITY,
    [PostId] int NOT NULL,
    [UserId] int NOT NULL,
    [ReactionType] int NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    CONSTRAINT [PK_Reactions] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Reactions_Posts_PostId] FOREIGN KEY ([PostId]) REFERENCES [Posts] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_Reactions_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id]) ON DELETE NO ACTION
);

CREATE TABLE [Reports] (
    [Id] int NOT NULL IDENTITY,
    [PostId] int NOT NULL,
    [ReportedByUserId] int NOT NULL,
    [Reason] nvarchar(500) NOT NULL,
    [Status] int NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    [ReviewedAt] datetime2 NULL,
    [ReviewedByUserId] int NULL,
    [AdminNotes] nvarchar(max) NULL,
    CONSTRAINT [PK_Reports] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Reports_Posts_PostId] FOREIGN KEY ([PostId]) REFERENCES [Posts] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_Reports_Users_ReportedByUserId] FOREIGN KEY ([ReportedByUserId]) REFERENCES [Users] ([Id]) ON DELETE NO ACTION
);

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Description', N'DisplayOrder', N'Name') AND [object_id] = OBJECT_ID(N'[Categories]'))
    SET IDENTITY_INSERT [Categories] ON;
INSERT INTO [Categories] ([Id], [Description], [DisplayOrder], [Name])
VALUES (1, N'Allmänt hunprat', 1, N'Hundsnack'),
(2, N'Allt om hundgodis och belöningar', 2, N'Hundgodissnack'),
(3, N'Träning och uppfostran', 3, N'Träningssnack'),
(4, N'Allt om valpar', 4, N'Valpsnack');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Description', N'DisplayOrder', N'Name') AND [object_id] = OBJECT_ID(N'[Categories]'))
    SET IDENTITY_INSERT [Categories] OFF;

CREATE UNIQUE INDEX [IX_GroupMembers_GroupId_UserId] ON [GroupMembers] ([GroupId], [UserId]);

CREATE INDEX [IX_GroupMembers_UserId] ON [GroupMembers] ([UserId]);

CREATE INDEX [IX_GroupMessages_GroupId] ON [GroupMessages] ([GroupId]);

CREATE INDEX [IX_GroupMessages_SenderId] ON [GroupMessages] ([SenderId]);

CREATE INDEX [IX_Groups_CreatedByUserId] ON [Groups] ([CreatedByUserId]);

CREATE INDEX [IX_Posts_ParentPostId] ON [Posts] ([ParentPostId]);

CREATE INDEX [IX_Posts_TopicId] ON [Posts] ([TopicId]);

CREATE INDEX [IX_Posts_UserId] ON [Posts] ([UserId]);

CREATE INDEX [IX_PrivateMessages_ReceiverId] ON [PrivateMessages] ([ReceiverId]);

CREATE INDEX [IX_PrivateMessages_SenderId] ON [PrivateMessages] ([SenderId]);

CREATE UNIQUE INDEX [IX_Reactions_PostId_UserId] ON [Reactions] ([PostId], [UserId]);

CREATE INDEX [IX_Reactions_UserId] ON [Reactions] ([UserId]);

CREATE INDEX [IX_Reports_PostId] ON [Reports] ([PostId]);

CREATE INDEX [IX_Reports_ReportedByUserId] ON [Reports] ([ReportedByUserId]);

CREATE INDEX [IX_Topics_CategoryId] ON [Topics] ([CategoryId]);

CREATE UNIQUE INDEX [IX_Users_Email] ON [Users] ([Email]);

CREATE UNIQUE INDEX [IX_Users_Username] ON [Users] ([Username]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20251002155742_InitialCreate', N'9.0.9');

UPDATE [Categories] SET [Description] = N'Allmänt hundprat'
WHERE [Id] = 1;
SELECT @@ROWCOUNT;


INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20251008103802_UpdateForAzure', N'9.0.9');

DROP INDEX [IX_Posts_TopicId] ON [Posts];

CREATE TABLE [GroupInvitations] (
    [Id] int NOT NULL IDENTITY,
    [GroupId] int NOT NULL,
    [InvitedUserId] int NOT NULL,
    [InvitedByUserId] int NOT NULL,
    [Status] nvarchar(20) NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    CONSTRAINT [PK_GroupInvitations] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_GroupInvitations_Groups_GroupId] FOREIGN KEY ([GroupId]) REFERENCES [Groups] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_GroupInvitations_Users_InvitedByUserId] FOREIGN KEY ([InvitedByUserId]) REFERENCES [Users] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_GroupInvitations_Users_InvitedUserId] FOREIGN KEY ([InvitedUserId]) REFERENCES [Users] ([Id]) ON DELETE NO ACTION
);

CREATE INDEX [IX_Posts_TopicId_CreatedAt] ON [Posts] ([TopicId], [CreatedAt]);

CREATE UNIQUE INDEX [IX_GroupInvitations_GroupId_InvitedUserId] ON [GroupInvitations] ([GroupId], [InvitedUserId]);

CREATE INDEX [IX_GroupInvitations_InvitedByUserId] ON [GroupInvitations] ([InvitedByUserId]);

CREATE INDEX [IX_GroupInvitations_InvitedUserId] ON [GroupInvitations] ([InvitedUserId]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20251009040858_Sync_20251009_0608', N'9.0.9');

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20251009051523_AddPostImageUrl', N'9.0.9');

COMMIT;
GO

