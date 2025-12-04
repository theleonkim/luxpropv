CREATE TABLE [dbo].[PropertyTour360] (
    [Id]         INT            IDENTITY (1, 1) NOT NULL,
    [PropertyId] INT            NOT NULL,
    [TourUrl]    NVARCHAR (500) NOT NULL,
    [Title]      NVARCHAR (150) NULL,
    [IsActive]   BIT            DEFAULT ((1)) NOT NULL,
    [CreatedAt]  DATETIME       DEFAULT (getdate()) NOT NULL,
    [CreatedBy]  NVARCHAR (100) NULL,
    [UpdatedAt]  DATETIME       NULL,
    [UpdatedBy]  NVARCHAR (100) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_PropertyTour360_Property] FOREIGN KEY ([PropertyId]) REFERENCES [dbo].[Propiedad] ([Propiedad_ID])
);

