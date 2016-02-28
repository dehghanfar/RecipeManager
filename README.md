# RecipeManager


The intent of this application is to calculate a receipt based different ingredient in a recipe.
In the receipt we need to show Total Discount, Total Tax and total costs.

The rule is:

- Sales Tax (%8.6 of the total price rounded up to the nearest 7 cents, applies to everything except produce)

- Wellness Discount (-%5 of the total price rounded up to the nearest cent, applies only to organic items)

- Total Cost (should include the sales tax and the discount)

To run this application please follow the below steps:

1-	Create a database call it: Recipe


2-	Run below query on the created database

```

USE [Recipe]
GO
/****** Object:  Table [dbo].[Ingredient]    Script Date: 2/25/2016 11:47:52 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Ingredient](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](128) NULL,
	[Price] [decimal](18, 2) NOT NULL,
	[IsOrganic] [bit] NOT NULL CONSTRAINT [DF_Ingredient_IsOrganic]  DEFAULT ((0)),
	[IngredientType] [smallint] NOT NULL,
 CONSTRAINT [PK_Ingredient] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET IDENTITY_INSERT [dbo].[Ingredient] ON 

INSERT [dbo].[Ingredient] ([Id], [Title], [Description], [Price], [IsOrganic], [IngredientType]) VALUES (1, N'Garlic', N'clove of organic garlic', CAST(0.67 AS Decimal(18, 2)), 1, 1)
INSERT [dbo].[Ingredient] ([Id], [Title], [Description], [Price], [IsOrganic], [IngredientType]) VALUES (2, N'Lemon', N'Lemon', CAST(2.03 AS Decimal(18, 2)), 0, 1)
INSERT [dbo].[Ingredient] ([Id], [Title], [Description], [Price], [IsOrganic], [IngredientType]) VALUES (3, N'Corn', N'cup of corn', CAST(0.87 AS Decimal(18, 2)), 0, 1)
INSERT [dbo].[Ingredient] ([Id], [Title], [Description], [Price], [IsOrganic], [IngredientType]) VALUES (4, N'Breast', N'chicken breast', CAST(2.19 AS Decimal(18, 2)), 0, 2)
INSERT [dbo].[Ingredient] ([Id], [Title], [Description], [Price], [IsOrganic], [IngredientType]) VALUES (5, N'Bacon', N'slice of bacon', CAST(0.24 AS Decimal(18, 2)), 0, 2)
INSERT [dbo].[Ingredient] ([Id], [Title], [Description], [Price], [IsOrganic], [IngredientType]) VALUES (8, N'Pasta', N'ounce of pasta', CAST(0.31 AS Decimal(18, 2)), 0, 3)
INSERT [dbo].[Ingredient] ([Id], [Title], [Description], [Price], [IsOrganic], [IngredientType]) VALUES (9, N'Olive', N'cup of organic olive oil', CAST(1.92 AS Decimal(18, 2)), 1, 3)
INSERT [dbo].[Ingredient] ([Id], [Title], [Description], [Price], [IsOrganic], [IngredientType]) VALUES (10, N'Vinegar', N'cup of vinegar', CAST(1.26 AS Decimal(18, 2)), 0, 3)
INSERT [dbo].[Ingredient] ([Id], [Title], [Description], [Price], [IsOrganic], [IngredientType]) VALUES (11, N'Salt', N'teaspoon of salt', CAST(0.16 AS Decimal(18, 2)), 0, 3)
INSERT [dbo].[Ingredient] ([Id], [Title], [Description], [Price], [IsOrganic], [IngredientType]) VALUES (12, N'Pepper', N'teaspoon of pepper', CAST(0.17 AS Decimal(18, 2)), 0, 3)
SET IDENTITY_INSERT [dbo].[Ingredient] OFF

```

3- change the connection string in the web.config

```
 <add name="RecipeEntity" connectionString="data source=ERFAN-PC;initial catalog=Recipe;integrated security=True;MultipleActiveResultSets=True;App=EntityFramework" providerName="System.Data.SqlClient" />
```


4- run the application in the visual studio

Below picutre is screen shot of the application.

![alt text](http://s18.postimg.org/jifczwzk9/image.png "Logo Title Text 1")

User can put the ingredient  and unit amount in order to calculate the final receipt.






