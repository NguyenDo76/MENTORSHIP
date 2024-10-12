Drop table if exists News
Create table News ( News_ID int identity (1,1) primary key,
                    Title text,
                    Description text,
                    Link nvarchar (50),
                    Guid nvarchar (50),
                    PubDate datetime,
                    UpdatedDate datetime,                      
                    ImageURL nvarchar (50),
                    SourceID int,
                    CategoryID int                      
                  );

Drop table if exists [User]
Create table [User] ( UserID int identity (1,1) primary key,
                      UserName nvarchar (100),
                      UserPassword nvarchar (100),
                      Email nvarchar (100),
                      JoinedDate datetime,
                      LastedSignOut datetime                 
                    );

Drop table if exists Source
Create table Source ( SourceID int identity (1,1) primary key,
                      SourceName nvarchar (50),
                      URL nvarchar (100),                                  
                    )
Insert into Source values 
('Tuoi tre', 'https://tuoitre.vn/rss.htm'),
('Thanh nien', 'https://thanhnien.vn/rss.html'),
('Vietnamexpress', 'https://vnexpress.net/rss');

Drop table if exists Categories
Create table Categories ( CategoryID int identity (1,1) primary key,
                          CategoryName nvarchar (50),                                                      
                        )

Insert into Categories values
('The thao'),
('Giai tri'),
('Van hoa');


Drop table if exists Tags
Create table Tags ( TagID int identity (1,1) primary key,
                    TagName nvarchar (100),                                                      
                  );

Drop table if exists Post
Create table Post ( PostID int identity (1,1) primary key,
                    UserID int,
                    News_ID int,
                    CategoryID int,
                    [View] int,
                    [Like] int,
                    Comment int,
                    Bookmark int                                                    
                  );               

Drop table if exists UserCategory
Create table UserCategory ( UserCategoryID int identity (1,1) primary key,
                            UserID int,
                            CategoryFlavorID int,
                            CreatedDate datetime
                          );

Drop table if exists UserTags
Create table UserTags ( UserTagID int identity (1,1) primary key,
                        UserID int,
                        TagFlavorID int,
                        CreatedDate datetime
                      );
                          
Drop table if exists NewsTags
Create table NewsTags ( NewsTagID int identity (1,1) primary key,
                        News_ID int,
                        TagID int,
                        CreatedDate datetime
                      );

