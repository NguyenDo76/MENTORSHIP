Drop table if exists News
Create table News ( News_ID int identity (1,1) primary key,
                    Title text,
                    Description text,
                    Link nvarchar(255),
                    Guid nvarchar (255),
                    PubDate datetime,
                    UpdatedDate datetime,                      
                    ImageURL nvarchar (255),
                    SourceCategoriesID int,                                  
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
                      URLViewSource nvarchar (500),                                  
                    )
Insert into Source values 
('Tuoi tre', 'https://tuoitre.vn/rss.htm','view-source:https://tuoitre.vn/rss.htm'),
('Thanh nien', 'https://thanhnien.vn/rss.html','view-source:https://thanhnien.vn/rss.html'),
('Vietnamexpress', 'https://vnexpress.net/rss','view-source:https://vnexpress.net/rss'),
('Kenh14', 'https://kenh14.vn/index.rss','view-source:https://kenh14.vn/index.rss');

Drop table if exists Categories
Create table Categories ( CategoryID int identity (1,1) primary key,
                          CategoryName nvarchar (50),                                                      
                        )

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

Drop table if exists SourceCategories
Create table SourceCategories ( SourceCategoriesID int identity (1,1) primary key,
                                SourceID int,
                                CategoryID int,
                                LinkRSS nvarchar(100)
                              );
Insert into SourceCategories values
(1, 1, 'https://tuoitre.vn/rss/the-thao.rss'),
(1, 2, 'https://tuoitre.vn/rss/giai-tri.rss'),
(1, 3, 'https://tuoitre.vn/rss/van-hoa.rss'),
(2, 1, 'https://thanhnien.vn/rss/the-thao.rss'),
(2, 2, 'https://thanhnien.vn/rss/giai-tri.rss'),
(2, 3, 'https://thanhnien.vn/rss/van-hoa.rss'),
(4, 1, 'https://kenh14.vn/sport.rss'),
(3, 1, 'https://vnexpress.net/rss/the-thao.rss'),
(3, 2, 'https://vnexpress.net/rss/giai-tri.rss');
                         
