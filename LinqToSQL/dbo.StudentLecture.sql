CREATE TABLE [dbo].[StudentLecture]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [StudentId] INT NOT NULL, 
    [LectureId] INT NOT NULL, 
    CONSTRAINT [StudentFK] FOREIGN KEY ([StudentId]) REFERENCES [dbo].[Student]([Id]) ON DELETE CASCADE, 
    CONSTRAINT [LectureFK] FOREIGN KEY ([LectureId]) REFERENCES [dbo].[Lecture]([Id]) ON DELETE CASCADE
)
